using vCards;

namespace iCloud.Dav.People
{
    /// <summary>
    /// A postal address.
    /// </summary>
    public class VCardAddress : vCardDeliveryAddress
    {
        /// <summary>
        ///  The unique id of postal address.
        /// </summary>
        public virtual string Id { get; set; }

        /// <summary>
        ///  The country code of postal address.
        /// </summary>
        public virtual string CountryCode { get; set; }

        /// <summary>
        ///  The preferred of postal address.
        /// </summary>
        public virtual bool IsPreferred { get; set; }
    }
}