using System;

namespace iCloud.Dav.People
{
    /// <summary>
    ///     Telephone information for a <see cref="Person" />.
    /// </summary>
    /// <seealso cref="PhoneTypes" />
    [Serializable]
    public class Phone
    {
        #region Properties

        /// <summary>Indicates a BBS number.</summary>
        /// <seealso cref="IsModem" />
        /// <seealso cref="PhoneTypes" />
        public virtual bool IsBBS => (PhoneType & PhoneTypes.BBS) == PhoneTypes.BBS;

        /// <summary>Indicates a car number.</summary>
        /// <seealso cref="PhoneTypes" />
        public virtual bool IsCar => (PhoneType & PhoneTypes.Car) == PhoneTypes.Car;

        /// <summary>Indicates a cellular number.</summary>
        /// <seealso cref="PhoneTypes" />
        public virtual bool IsCellular => (PhoneType & PhoneTypes.Cellular) == PhoneTypes.Cellular;

        /// <summary>Indicates a fax number.</summary>
        /// <seealso cref="PhoneTypes" />
        public virtual bool IsFax => (PhoneType & PhoneTypes.Fax) == PhoneTypes.Fax;

        /// <summary>Indicates a home number.</summary>
        /// <seealso cref="IsWork" />
        /// <seealso cref="PhoneTypes" />
        public virtual bool IsHome => (PhoneType & PhoneTypes.Home) == PhoneTypes.Home;

        /// <summary>Indicates an ISDN number.</summary>
        /// <seealso cref="PhoneTypes" />
        public virtual bool IsISDN => (PhoneType & PhoneTypes.ISDN) == PhoneTypes.ISDN;

        /// <summary>Indicates a messaging service number.</summary>
        /// <seealso cref="PhoneTypes" />
        public virtual bool IsMessagingService => (PhoneType & PhoneTypes.MessagingService) == PhoneTypes.MessagingService;

        /// <summary>Indicates a modem number.</summary>
        /// <seealso cref="IsBBS" />
        /// <seealso cref="PhoneTypes" />
        public virtual bool IsModem => (PhoneType & PhoneTypes.Modem) == PhoneTypes.Modem;

        /// <summary>Indicates a pager number.</summary>
        /// <seealso cref="PhoneTypes" />
        public virtual bool IsPager => (PhoneType & PhoneTypes.Pager) == PhoneTypes.Pager;

        /// <summary>Indicates a preferred number.</summary>
        /// <seealso cref="PhoneTypes" />
        public virtual bool IsPreferred => (PhoneType & PhoneTypes.Preferred) == PhoneTypes.Preferred;

        /// <summary>Indicates a video number.</summary>
        /// <seealso cref="PhoneTypes" />
        public virtual bool IsVideo => (PhoneType & PhoneTypes.Video) == PhoneTypes.Video;

        /// <summary>Indicates a voice number.</summary>
        /// <seealso cref="PhoneTypes" />
        public virtual bool IsVoice => (PhoneType & PhoneTypes.Voice) == PhoneTypes.Voice;

        /// <summary>Indicates a work number.</summary>
        /// <seealso cref="IsHome" />
        /// <seealso cref="PhoneTypes" />
        public virtual bool IsWork => (PhoneType & PhoneTypes.Work) == PhoneTypes.Work;

        /// <summary>The full telephone number.</summary>
        public virtual string FullNumber { get; set; }

        /// <summary>The phone subtype.</summary>
        /// <seealso cref="IsVideo" />
        /// <seealso cref="IsVoice" />
        /// <seealso cref="IsWork" />
        public virtual PhoneTypes PhoneType { get; set; }

        #endregion
    }
}
