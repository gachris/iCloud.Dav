namespace iCloud.Dav.People
{
    /// <summary>A formatted delivery label.</summary>
    /// <seealso cref="Address" />
    public class Label
    {
        #region Properties

        /// <summary>The type of delivery address for the label.</summary>
        public virtual AddressTypes AddressType { get; set; }

        /// <summary>The formatted delivery text.</summary>
        public virtual string Text { get; set; }

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
