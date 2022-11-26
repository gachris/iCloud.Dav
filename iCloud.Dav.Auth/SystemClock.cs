using System;

namespace iCloud.Dav.Auth
{
    /// <summary>A default clock implementation that wraps the <see cref="DateTime.Now" /> property.</summary>
    public class SystemClock : IClock
    {
        /// <summary>The default instance.</summary>
        public static readonly IClock Default = new SystemClock();

        /// <summary>Constructs a new system clock.</summary>
        protected SystemClock()
        {
        }

        /// <summary>
        /// Gets a <see cref="DateTime" /> object that is set to the current date and time on this computer,
        /// expressed as the local time.
        /// </summary>
        public DateTime Now => DateTime.Now;

        /// <summary>
        /// Gets a <see cref="DateTime" /> object that is set to the current date and time on this computer,
        /// expressed as UTC time.
        /// </summary>
        public DateTime UtcNow => DateTime.UtcNow;
    }
}