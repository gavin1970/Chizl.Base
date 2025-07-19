using System;

namespace Chizl.ThreadSupport
{
    internal class TLockModel
    {
        private TLockModel(Guid lockId)
        {
            EventTime = DateTime.UtcNow.Ticks;
            LockId = lockId;
        }
        public long EventTime { get; }
        public Guid LockId { get; }
        public static TLockModel New(Guid lockId) => new TLockModel(lockId);
    }
}
