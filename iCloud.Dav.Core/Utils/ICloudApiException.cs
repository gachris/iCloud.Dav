using System;

namespace iCloud.Dav.Core.Utils
{
    /// <summary>Represents an exception thrown by an API Service.</summary>
    public class ICloudApiException : Exception
    {
        /// <summary>Gets the service name which related to this exception.</summary>
        public string ServiceName { get; }

        /// <summary>Creates an API Service exception.</summary>
        public ICloudApiException(string serviceName, string message, Exception inner) : base(message, inner)
        {
            ServiceName = serviceName.ThrowIfNullOrEmpty(nameof(serviceName));
        }

        /// <summary>Creates an API Service exception.</summary>
        public ICloudApiException(string serviceName, string message) : this(serviceName, message, null)
        {
        }

        public override string ToString()
        {
            return string.Format("The service {1} has thrown an exception: {0}", base.ToString(), ServiceName);
        }
    }
}
