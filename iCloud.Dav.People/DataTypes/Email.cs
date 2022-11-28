using iCloud.Dav.People.DataTypes.Mapping;
using iCloud.Dav.People.Serialization.DataTypes;
using iCloud.Dav.People.Utils;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
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
    /// <seealso cref="EmailType" />
    [Serializable]
    public class Email : EncodableDataType, IRelatedDataType
    {
        public const string School = "_$!<School>!$_";

        public virtual bool IsPreferred { get; set; }

        /// <summary>The email address.</summary>
        /// <remarks>
        ///     The format of the email address is not validated by the class.
        /// </remarks>
        public virtual string Address { get; set; }

        /// <summary>
        /// The email address type.
        /// </summary>
        public virtual EmailType Type
        {
            get
            {
                var types = Parameters.GetMany("TYPE");

                _ = types.TryParse<EmailTypeInternal>(out var typeInternal);
                var isPreferred = typeInternal.HasFlag(EmailTypeInternal.Pref);
                if (isPreferred)
                {
                    IsPreferred = true;
                    typeInternal = typeInternal.RemoveFlags(EmailTypeInternal.Pref);
                }

                var typeFromInternal = EmailTypeMapping.GetType(typeInternal);
                if (typeFromInternal is 0)
                {
                    switch (Label?.Value)
                    {
                        case School:
                            typeFromInternal = EmailType.School;
                            break;
                        default:
                            typeFromInternal = EmailType.Custom;
                            break;
                    }
                }

                return typeFromInternal;
            }
            set
            {
                var typeInternal = EmailTypeMapping.GetType(value);
                if (IsPreferred)
                {
                    typeInternal = typeInternal.AddFlags(EmailTypeInternal.Pref);
                }

                if (!(typeInternal is 0))
                {
                    Parameters.Set("TYPE", typeInternal.StringArrayFlags().Select(x => x.ToUpperInvariant()));
                }
                else
                {
                    Parameters.Remove("TYPE");
                }

                var typeFromInternal = EmailTypeMapping.GetType(typeInternal);

                switch (typeFromInternal)
                {
                    case EmailType.School:
                        Label = new Label() { Value = School };
                        break;
                    case EmailType.Custom:
                        Label = new Label() { Value = Label?.Value };
                        break;
                    default:
                        Label = null;
                        break;
                }
            }
        }

        public virtual Label Label
        {
            get => Properties.Get<Label>("X-ABLABEL");
            set
            {
                if (value == null)
                {
                    Properties.Remove("X-ABLABEL");
                    var typeInternal = EmailTypeMapping.GetType(EmailType.Other);
                    Parameters.Set("TYPE", typeInternal.StringArrayFlags().Select(x => x.ToUpperInvariant()));
                }
                else
                {
                    Properties.Set("X-ABLABEL", value);
                    Parameters.Remove("TYPE");
                }
            }
        }

        /// <summary>
        /// Returns a list of properties that are associated with the TEL object.
        /// </summary>
        public virtual CardDataTypePropertyList Properties { get; protected set; }

        public Email()
        {
            Initialize();
            Type = EmailType.Other;
        }

        public Email(string value)
        {
            Initialize();
            Type = EmailType.Other;
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            var serializer = new EmailSerializer();
            CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
        }

        private void Initialize() => Properties = new CardDataTypePropertyList();

        protected override void OnDeserializing(StreamingContext context)
        {
            base.OnDeserializing(context);

            Initialize();
        }
    }
}