using iCloud.Dav.People.Serialization.DataTypes;
using System;
using System.IO;
using vCard.Net;
using vCard.Net.DataTypes;

namespace iCloud.Dav.People.DataTypes
{
    /// <summary>
    ///     Phone information for a <see cref="Contact" />.
    /// </summary>
    /// <seealso cref="DataTypes.PhoneType" />
    [Serializable]
    public class Phone : EncodableDataType
    {
        #region Properties

        /// <summary>The full telephone number.</summary>
        public virtual string FullNumber { get; set; }

        /// <summary>The phone type.</summary>
        public virtual PhoneType PhoneType { get; set; }

        public virtual string Label { get; set; }

        public virtual bool IsPreferred { get; set; }

        #endregion

        public Phone()
        {
        }

        public Phone(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            var serializer = new PhoneSerializer();
            CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
        }
    }
}