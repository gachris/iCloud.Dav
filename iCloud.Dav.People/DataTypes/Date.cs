using iCloud.Dav.People.DataTypes.Mapping;
using iCloud.Dav.People.Extensions;
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
    /// Represents a date value that can be associated with a contact.
    /// </summary>
    public class Date : EncodableDataType, IRelatedDataType
    {
        #region Fields/Consts

        /// <summary>
        /// A constant string representing the anniversary date type.
        /// </summary>
        public const string Anniversary = "_$!<Anniversary>!$_";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this date is preferred.
        /// </summary>
        public virtual bool IsPreferred { get; set; }

        /// <summary>
        /// Gets or sets the date type.
        /// </summary>
        public virtual DateType Type
        {
            get
            {
                var types = Parameters.GetMany("TYPE");

                _ = types.TryParse<DateTypeInternal>(out var typeInternal);
                var isPreferred = typeInternal.HasFlag(DateTypeInternal.Pref);
                if (isPreferred)
                {
                    IsPreferred = true;
                    typeInternal = typeInternal.RemoveFlags(DateTypeInternal.Pref);
                }

                var typeFromInternal = DateTypeMapping.GetType(typeInternal);
                if (typeFromInternal is 0)
                {
                    switch (Label?.Value)
                    {
                        case Anniversary:
                            typeFromInternal = DateType.Anniversary;
                            break;
                        default:
                            typeFromInternal = DateType.Custom;
                            break;
                    }
                }

                return typeFromInternal;
            }
            set
            {
                var typeInternal = DateTypeMapping.GetType(value);
                if (IsPreferred)
                {
                    typeInternal = typeInternal.AddFlags(DateTypeInternal.Pref);
                }

                if (!(typeInternal is 0))
                {
                    Parameters.Remove("TYPE");
                    Parameters.Set("TYPE", typeInternal.StringArrayFlags().Select(x => x.ToUpperInvariant()));
                }
                else
                {
                    Parameters.Remove("TYPE");
                }

                switch (value)
                {
                    case DateType.Anniversary:
                        Label = new Label() { Value = Anniversary };
                        break;
                    default:
                        Label = null;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets the label associated with the date.
        /// </summary>
        public virtual Label Label
        {
            get => Properties.Get<Label>("X-ABLABEL");
            set
            {
                if (value == null && Label != null)
                {
                    Properties.Remove("X-ABLABEL");
                    Parameters.Remove("TYPE");
                    Parameters.Set("TYPE", DateTypeMapping.GetType(DateType.Other).StringArrayFlags().Select(x => x.ToUpperInvariant()));
                }
                else if (value != null)
                {
                    Properties.Set("X-ABLABEL", value);
                    Parameters.Remove("TYPE");
                }
            }
        }

        /// <summary>
        /// Gets or sets the date and time value.
        /// </summary>
        public virtual DateTime DateTime { get; set; }

        /// <summary>
        /// Gets or sets a collection of properties associated with the date.
        /// </summary>
        public virtual CardDataTypePropertyList Properties { get; protected set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Date"/> class.
        /// </summary>
        public Date()
        {
            Initialize();
            Type = DateType.Other;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Date"/> class with a string value.
        /// </summary>
        /// <param name="value">A string representation of the date value.</param>
        public Date(string value)
        {
            Initialize();
            Type = DateType.Other;

            if (string.IsNullOrWhiteSpace(value)) return;

            var serializer = new DateSerializer();
            CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
        }

        #region Methods

        /// <summary>
        /// Initializes the properties of the date.
        /// </summary>
        private void Initialize() => Properties = new CardDataTypePropertyList();

        /// <summary>
        /// This method is called during deserialization to initialize the object before any deserialization is done.
        /// </summary>
        /// <param name="context">The context for the serialization or deserialization operation.</param>
        protected override void OnDeserializing(StreamingContext context)
        {
            base.OnDeserializing(context);

            Initialize();
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return !(obj is null) && (ReferenceEquals(this, obj) || obj.GetType() == GetType() && Equals((Date)obj));
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><see langword = "true" /> if the specified object is equal to the current object; otherwise, <see langword = "false" />.</returns>
        protected bool Equals(Date obj)
        {
            return Equals(IsPreferred, obj.IsPreferred) &&
                   Equals(Type, obj.Type) &&
                   Equals(Label, obj.Label) &&
                   DateTime.Equals(obj.DateTime);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + IsPreferred.GetHashCode();
                hash = hash * 23 + Type.GetHashCode();
                hash = hash * 23 + (Label != null ? Label.GetHashCode() : 0);
                hash = hash * 23 + DateTime.GetHashCode();
                return hash;
            }
        }

        #endregion
    }
}