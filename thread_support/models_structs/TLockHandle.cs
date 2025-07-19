using System;

namespace Chizl.ThreadSupport
{
    public sealed class TLockHandle : IDisposable
    {
        private readonly Guid _instanceGuid;
        private readonly Guid _lockId;
        private readonly TLockOptions _options;
        private readonly bool _acquired;
        private bool _disposed;

        public Guid InstanceGuid => _instanceGuid;
        public Guid LockId => _lockId;
        public bool Acquired => _acquired;
        public TLockOptions Options => _options;
        internal TLockHandle(Guid lockId, Guid instanceGuid, bool acquired, TLockOptions options)
        {
            _lockId = lockId;
            _instanceGuid = instanceGuid;
            _acquired = acquired;
            _options = options;
        }
        public void Dispose()
        {
            if (!_disposed)
            {
                if (_acquired)
                    TLock.Release(_instanceGuid); // static release helper (see below)

                _disposed = true;
            }
        }
    }
}
