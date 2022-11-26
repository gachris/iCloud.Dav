using System;

namespace iCloud.Dav.Core
{
    /// <summary>
    /// Implementation of <see cref="IBackOff" /> that increases the back-off period for each retry attempt using a
    /// randomization function that grows exponentially. In addition, it also adds a randomize number of milliseconds
    /// for each attempt.
    /// </summary>
    public class ExponentialBackOff : IBackOff
    {
        /// <summary>The random instance which generates a random number to add the to next back-off.</summary>
        private readonly Random _random = new Random();
        /// <summary>The maximum allowed number of retries.</summary>
        private const int MaxAllowedNumRetries = 20;
        private readonly TimeSpan _deltaBackOff;
        private readonly int _maxNumOfRetries;

        /// <summary>
        /// Gets the delta time span used to generate a random milliseconds to add to the next back-off.
        /// If the value is <see cref="TimeSpan.Zero" /> then the generated back-off will be exactly 1, 2, 4,
        /// 8, 16, etc. seconds. A valid value is between zero and one second. The default value is 250ms, which means
        /// that the generated back-off will be [0.75-1.25]sec, [1.75-2.25]sec, [3.75-4.25]sec, and so on.
        /// </summary>
        public TimeSpan DeltaBackOff => _deltaBackOff;

        /// <summary>Gets the maximum number of retries. Default value is <c>10</c>.</summary>
        public int MaxNumOfRetries => _maxNumOfRetries;

        /// <summary>Constructs a new exponential back-off with default values.</summary>
        public ExponentialBackOff() : this(TimeSpan.FromMilliseconds(250.0), 10)
        {
        }

        /// <summary>Constructs a new exponential back-off with the given delta and maximum retries.</summary>
        public ExponentialBackOff(TimeSpan deltaBackOff, int maximumNumOfRetries = 10)
        {
            if (deltaBackOff < TimeSpan.Zero || deltaBackOff > TimeSpan.FromSeconds(1.0))
                throw new ArgumentOutOfRangeException(nameof(deltaBackOff));
            if (maximumNumOfRetries < 0 || maximumNumOfRetries > 20)
                throw new ArgumentOutOfRangeException(nameof(deltaBackOff));
            _deltaBackOff = deltaBackOff;
            _maxNumOfRetries = maximumNumOfRetries;
        }

        public TimeSpan GetNextBackOff(int currentRetry)
        {
            if (currentRetry <= 0)
                throw new ArgumentOutOfRangeException(nameof(currentRetry));
            if (currentRetry > MaxNumOfRetries)
                return TimeSpan.MinValue;
            var random = _random;
            var deltaBackOff = DeltaBackOff;
            var minValue = (int)(deltaBackOff.TotalMilliseconds * -1.0);
            deltaBackOff = DeltaBackOff;
            var maxValue = (int)(deltaBackOff.TotalMilliseconds * 1.0);
            var num = random.Next(minValue, maxValue);
            return TimeSpan.FromMilliseconds((int)(Math.Pow(2.0, currentRetry - 1.0) * 1000.0 + num));
        }
    }
}