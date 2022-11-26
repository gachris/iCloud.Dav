using System.Collections.Generic;
using System.IO;
using vCard.Net;
using vCard.Net.DataTypes;

namespace iCloud.Dav.People.DataTypes
{
    /// <summary>
    /// Represents an RFC 6350 "X-ABRELATEDNAMES" extended value.
    /// </summary>
    public class X_ABRelatedNames : EncodableDataType
    {
        public virtual string Name { get; set; }

        /// <summary>The url types.</summary>
        public virtual IList<string> Types
        {
            get => Parameters.GetMany("TYPE");
            set => Parameters.Set("TYPE", value);
        }

        #region Properties

        public virtual string Label { get; set; }

        public virtual bool IsPreferred { get; set; }

        /// <summary>The related person type.</summary>
        public virtual RelatedPeopleType RelatedPersonType { get; set; }

        #endregion

        public X_ABRelatedNames()
        {
        }

        public X_ABRelatedNames(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            var serializer = new X_ABRelatedNamesSerializer();
            CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
        }
    }
}