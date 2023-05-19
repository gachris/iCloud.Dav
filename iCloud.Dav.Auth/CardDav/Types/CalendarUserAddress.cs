namespace iCloud.Dav.Auth.CardDav.Types
{
    /// <summary>
    /// Represents a calendar user address, which includes a value and a preferred flag.
    /// </summary>
    internal sealed class CalendarUserAddress
    {
        /// <summary>
        /// Gets a value indicating whether this calendar user address is preferred.
        /// </summary>
        public bool Preferred { get; }

        /// <summary>
        /// Gets the value of this calendar user address.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarUserAddress"/> class with the specified parameters.
        /// </summary>
        /// <param name="preferred">A value indicating whether this calendar user address is preferred.</param>
        /// <param name="value">The value of this calendar user address.</param>
        public CalendarUserAddress(bool preferred, string value)
        {
            Preferred = preferred;
            Value = value;
        }
    }
}