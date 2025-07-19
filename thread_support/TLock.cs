using System;
using System.Linq;
using System.Threading;
using Chizl.Extensions;
using System.Collections.Concurrent;

namespace Chizl.ThreadSupport
{
    public static class TLock
    {
        const int RELEASED = 0;
        static readonly object _lock = new object();    //used only with add
        private static readonly ConcurrentDictionary<Guid, TLockModel> _lockList = new ConcurrentDictionary<Guid, TLockModel>();
        private static readonly AutoResetEvent[] _resetEvents = new AutoResetEvent[] { new AutoResetEvent(false) };
        private static TLockOptions _defaultOptions = new TLockOptions();

        /// <summary>
        /// TLock.Acquire is a threadsafe global class that works off of First Locked, First Released.<br/>
        /// Meaning you can use this in any class and/or within any thread with the same lockId.  If the lockId, "Guid", is 
        /// already in use, it will block moving forward until set instance timeout or released by previous lock.
        /// </summary>
        /// <param name="lockId">A guid that you want to blocking if attempted to use at the same time, somewhere else.</param>
        /// <param name="options">(Optional) Defaults: timeout=60s, showTimeoutException=false<br/>
        /// TLockOptions options are not static.  They are specific to the calling instance.  This is where you set the timeout 
        /// and if you perfer to have an exception thrown instead of TLockHandle returned.  Only occurs if failed to lock within the timeout period.
        /// </param>
        /// <param name="name">(Optional) Name.  This is only used if you have the option showTimeoutException set to true.  Helps you identify where it might be.</param>
        /// <returns>TLockHandle struct: </returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="TimeoutException"></exception>
        public static TLockHandle Acquire(Guid lockId, TLockOptions? options = null, string name = "")
        {
            var opts = options ?? _defaultOptions;
            var instanceGuid = Guid.NewGuid();
            var guidName = $"{lockId}{(string.IsNullOrWhiteSpace(name) ? "" : $" ({name})")}";
            var timeToLeave = DateTime.UtcNow.Add(opts.TimeOut);

            bool acquired = false;

            if (lockId.IsEmpty())
                throw new ArgumentException("lockId cannot be empty");

            if (!Add(instanceGuid, lockId))
            {
                while (true)
                {
                    var remaining = timeToLeave.Subtract(DateTime.UtcNow);
                    if (remaining.TotalMilliseconds <= 0)
                    {
                        Release(instanceGuid);
                        break;
                    }

                    var result = WaitHandle.WaitAny(_resetEvents, remaining);
                    if (result == WaitHandle.WaitTimeout)
                    {
                        Release(instanceGuid);
                        break;
                    }
                    else if (IsNext(lockId, instanceGuid))
                    {
                        acquired = true;
                        break;
                    }
                }
            }
            else
                acquired = true;

            if (!acquired && opts.ShowTimeoutException)
                throw new TimeoutException($"Failed to acquire lock for {guidName} after {opts.TimeOut}.");

            return new TLockHandle(lockId, instanceGuid, acquired, opts);
        }

        /// <summary>
        /// This will release the InstanceGuid based on what was returned from Acquire() within TLockHandle.InstanceGuid.<br/>
        /// Release() Is automatically called, if your within a using(...) call.  If called afterward, it's ignore as the <br/>
        /// instanceGuid will not be exist anymore.
        /// </summary>
        /// <param name="instanceGuid">Guid that was created on entrance of Acquire()</param>
        /// <returns></returns>
        public static bool Release(Guid instanceGuid)
        {
            if (_lockList.TryRemove(instanceGuid, out _))
            {
                _resetEvents[RELEASED].Set();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Add Instance with passed in Guid uniqueId.
        /// </summary>
        /// <param name="instanceGuid">Guid that was created on entrance of Acquire()</param>
        /// <param name="uniqueId">Guid passed into Acquire() by user</param>
        /// <returns>true: if instance was added successfully with no other in the dictionary for this uniqueId.</returns>
        private static bool Add(Guid instanceGuid, Guid uniqueId)
        {
            //using lock(), since "Add()" is a very fast method call and timer isn't required.
            //This lock helps for many threads attempting to add with the same uniqueId at the same time.
            lock (_lock)
            {
                var waitInQueue = !_lockList.Values.Any(x => x.LockId == uniqueId);
                _lockList.TryAdd(instanceGuid, TLockModel.New(uniqueId));
                return waitInQueue;
            }
        }

        /// <summary>
        /// When multiple Aquire() called with the same Guid uniqueId, a lock will occur on all after the first.<br/>
        /// This checks existing Guid InstanceId is the next lock that can be aquired.
        /// </summary>
        /// <param name="uniqueId">Guid passed into Acquire() by user</param>
        /// <param name="instanceGuid">Existing instance Guid.</param>
        /// <returns></returns>
        private static bool IsNext(Guid uniqueId, Guid instanceGuid)
        {
            var record = _lockList
                .Where(w => w.Value.LockId == uniqueId)
                .OrderBy(w => w.Value.EventTime)
                .FirstOrDefault();

            return record.Value != null && instanceGuid == record.Key;
        }
    }
}
