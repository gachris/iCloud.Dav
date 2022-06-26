using System;

namespace iCloud.Dav.People
{
    /// <summary>A postal address.</summary>
    [Serializable]
    public class Address
    {
        #region Properties

        /// <summary>
        ///  The unique id of postal address.
        /// </summary>
        public virtual string Id { get; set; }

        /// <summary>The type of postal address.</summary>
        public virtual AddressTypes AddressType { get; set; }

        /// <summary>The city or locality of the address.</summary>
        public virtual string City { get; set; }

        /// <summary>The country name of the address.</summary>
        public virtual string Country { get; set; }

        /// <summary>
        ///  The country code of postal address.
        /// </summary>
        public virtual string CountryCode { get; set; }

        /// <summary>
        ///  The preferred of postal address.
        /// </summary>
        public virtual bool IsPreferred { get; set; }

        /// <summary>The postal code (e.g. ZIP code) of the address.</summary>
        public virtual string PostalCode { get; set; }

        /// <summary>The region (state or province) of the address.</summary>
        public virtual string Region { get; set; }

        /// <summary>The street of the delivery address.</summary>
        public virtual string Street { get; set; }

        /// <summary>Indicates a domestic delivery address.</summary>
        public virtual bool IsDomestic => (AddressType & AddressTypes.Domestic) == AddressTypes.Domestic;

        /// <summary>Indicates a home address.</summary>
        public virtual bool IsHome => (AddressType & AddressTypes.Home) == AddressTypes.Home;

        /// <summary>Indicates an international address.</summary>
        public virtual bool IsInternational => (AddressType & AddressTypes.International) == AddressTypes.International;

        /// <summary>Indicates a parcel delivery address.</summary>
        public virtual bool IsParcel => (AddressType & AddressTypes.Parcel) == AddressTypes.Parcel;

        /// <summary>Indicates a postal address.</summary>
        public virtual bool IsPostal => (AddressType & AddressTypes.Postal) == AddressTypes.Postal;

        /// <summary>Indicates a work address.</summary>
        public virtual bool IsWork => (AddressType & AddressTypes.Work) == AddressTypes.Work;

        #endregion
    }
}
