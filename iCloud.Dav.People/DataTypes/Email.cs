using iCloud.Dav.People.PeopleComponents;
using iCloud.Dav.People.Serialization.DataTypes;
using System;
using System.IO;
using vCard.Net;
using vCard.Net.DataTypes;

namespace iCloud.Dav.People.DataTypes
{
    /// <summary>
    ///     An email address in a <see cref="Contact" />.
    /// </summary>
    /// <remarks>
    ///     Most Person email addresses are Internet email addresses.  However,
    ///     the Person specification allows other email address formats,
    ///     such as CompuServe and X400.  Unless otherwise specified, an
    ///     address is assumed to be an Internet address.
    /// </remarks>
    /// <seealso cref="DataTypes.EmailType" />
    [Serializable]
    public class Email : EncodableDataType
    {
        #region Properties

        /// <summary>The email address.</summary>
        /// <remarks>
        ///     The format of the email address is not validated by the class.
        /// </remarks>
        public virtual string Address { get; set; }

        /// <summary>The email address type.</summary>
        public virtual EmailType EmailType { get; set; }

        public virtual bool IsPreferred { get; set; }

        public virtual string Label { get; set; }

        #endregion

        public Email()
        {
        }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            var serializer = new EmailSerializer();
            CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
        }
    }
}