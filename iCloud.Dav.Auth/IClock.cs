using System;

namespace iCloud.Dav.Auth
{
    /// <summary>Clock wrapper for getting the current time.</summary>
    public interface IClock
    {
        /// <summary>
        /// Gets a <see cref="T:System.DateTime" /> object that is set to the current date and time on this computer,
        /// expressed as the local time.
        /// </summary>
        DateTime Now { get; }

        /// <summary>
        /// Gets a <see cref="T:System.DateTime" /> object that is set to the current date and time on this computer,
        /// expressed as UTC time.
        /// </summary>
        DateTime UtcNow { get; }
    }
}