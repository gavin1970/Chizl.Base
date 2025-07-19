using Chizl.Extensions;
using System;

namespace Chizl.ThreadSupport
{
    public readonly struct TLockOptions
    {
        private static readonly TimeSpan MIN_RANGE = TimeSpan.FromMilliseconds(0.00);
        private static readonly TimeSpan MAX_RANGE = TimeSpan.FromMinutes(30);
        private static readonly TimeSpan DEFAULT_TIMEOUT = TimeSpan.FromSeconds(60);
        private static readonly bool DEFAULT_SHOW_TIMEOUT_EXCEPTION = false;
        private readonly TimeSpan _timeout;
        private readonly bool _showTimeoutException;

        public TLockOptions(TimeSpan timeout) : this(timeout, DEFAULT_SHOW_TIMEOUT_EXCEPTION) { }
        public TLockOptions(bool showTimeoutException) : this(DEFAULT_TIMEOUT, showTimeoutException) { }
        public TLockOptions(TimeSpan timeout, bool showTimeoutException)
        {
            _timeout = timeout.Clamp(MIN_RANGE, MAX_RANGE);
            _showTimeoutException = showTimeoutException;
        }

        /// <summary>
        /// Set Timeout to wait on thread lock. Default: ${DEFAULT_TIMEOUT} 60sec.  Range: 0ms - 30min
        /// </summary>
        public TimeSpan TimeOut => _timeout;

        /// <summary>
        /// true: Throw exceptions after timeout<br/>
        /// false: Release object after timeout.
        /// </summary>
        public bool ShowTimeoutException => _showTimeoutException;
    }
}
