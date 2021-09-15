﻿using System;

namespace iCloud.Dav.Auth.Utils
{
    /// <summary>A default clock implementation that wraps the <see cref="P:System.DateTime.Now" /> property.</summary>
    public class SystemClock : IClock
    {
        /// <summary>The default instance.</summary>
        public static readonly IClock Default = new SystemClock();

        /// <summary>Constructs a new system clock.</summary>
        protected SystemClock()
        {
        }

        /// <summary>
        /// Gets a <see cref="T:System.DateTime" /> object that is set to the current date and time on this computer,
        /// expressed as the local time.
        /// </summary>
        public DateTime Now
        {
            get
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// Gets a <see cref="T:System.DateTime" /> object that is set to the current date and time on this computer,
        /// expressed as UTC time.
        /// </summary>
        public DateTime UtcNow
        {
            get
            {
                return DateTime.UtcNow;
            }
        }
    }
}
