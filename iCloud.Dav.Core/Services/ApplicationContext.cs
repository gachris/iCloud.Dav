using iCloud.Dav.Core.Logger;
using System;

namespace iCloud.Dav.Core.Services
{
    public static class ApplicationContext
    {
        private static ILogger logger;

        /// <summary>Returns the logger used within this application context.</summary>
        /// <remarks>It creates a <see cref="T:ICloud.Api.Logging.NullLogger" /> if no logger was registered previously</remarks>
        public static ILogger Logger
        {
            get
            {
                return ApplicationContext.logger ?? (ApplicationContext.logger = new NullLogger());
            }
        }

        /// <summary>Registers a logger with this application context.</summary>
        /// <exception cref="T:System.InvalidOperationException">Thrown if a logger was already registered.</exception>
        public static void RegisterLogger(ILogger loggerToRegister)
        {
            if (ApplicationContext.logger != null && !(ApplicationContext.logger is NullLogger))
                throw new InvalidOperationException("A logger was already registered with this context.");
            ApplicationContext.logger = loggerToRegister;
        }
    }
}
