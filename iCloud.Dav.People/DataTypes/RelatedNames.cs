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
    /// Represents a related name value that can be associated with a contact.
    /// </summary>
    public class RelatedNames : EncodableDataType, IRelatedDataType
    {
        #region Fields/Consts

        /// <summary>
        /// A constant string representing the "Father" related name type.
        /// </summary>
        public const string Father = "_$!<Father>!$_";

        /// <summary>
        /// A constant string representing the "Mother" related name type.
        /// </summary>
        public const string Mother = "_$!<Mother>!$_";

        /// <summary>
        /// A constant string representing the "Parent" related name type.
        /// </summary>
        public const string Parent = "_$!<Parent>!$_";

        /// <summary>
        /// A constant string representing the "Brother" related name type.
        /// </summary>
        public const string Brother = "_$!<Brother>!$_";

        /// <summary>
        /// A constant string representing the "Sister" related name type.
        /// </summary>
        public const string Sister = "_$!<Sister>!$_";

        /// <summary>
        /// A constant string representing the "Child" related name type.
        /// </summary>
        public const string Child = "_$!<Child>!$_";

        /// <summary>
        /// A constant string representing the "Friend" related name type.
        /// </summary>
        public const string Friend = "_$!<Friend>!$_";

        /// <summary>
        /// A constant string representing the "Spouse" related name type.
        /// </summary>
        public const string Spouse = "_$!<Spouse>!$_";

        /// <summary>
        /// A constant string representing the "Partner" related name type.
        /// </summary>
        public const string Partner = "_$!<Partner>!$_";

        /// <summary>
        /// A constant string representing the "Assistant" related name type.
        /// </summary>
        public const string Assistant = "_$!<Assistant>!$_";

        /// <summary>
        /// A constant string representing the "Manager" related name type.
        /// </summary>
        public const string Manager = "_$!<Manager>!$_";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the related name is preferred.
        /// </summary>
        public virtual bool IsPreferred { get; set; }

        /// <summary>
        /// Gets or sets the type of the related name.
        /// </summary>
        public virtual RelatedNamesType Type
        {
            get
            {
                var types = Parameters.GetMany("TYPE");

                _ = types.TryParse<RelatedNamesTypeInternal>(out var typeInternal);
                var isPreferred = typeInternal.HasFlag(RelatedNamesTypeInternal.Pref);
                if (isPreferred)
                {
                    IsPreferred = true;
                    typeInternal = typeInternal.RemoveFlags(RelatedNamesTypeInternal.Pref);
                }

                var typeFromInternal = RelatedNamesMapping.GetType(typeInternal);
                if (typeFromInternal is 0)
                {
                    switch (Label?.Value)
                    {
                        case Father:
                            typeFromInternal = RelatedNamesType.Father;
                            break;
                        case Mother:
                            typeFromInternal = RelatedNamesType.Mother;
                            break;
                        case Parent:
                            typeFromInternal = RelatedNamesType.Parent;
                            break;
                        case Brother:
                            typeFromInternal = RelatedNamesType.Brother;
                            break;
                        case Sister:
                            typeFromInternal = RelatedNamesType.Sister;
                            break;
                        case Child:
                            typeFromInternal = RelatedNamesType.Child;
                            break;
                        case Friend:
                            typeFromInternal = RelatedNamesType.Friend;
                            break;
                        case Spouse:
                            typeFromInternal = RelatedNamesType.Spouse;
                            break;
                        case Partner:
                            typeFromInternal = RelatedNamesType.Partner;
                            break;
                        case Assistant:
                            typeFromInternal = RelatedNamesType.Assistant;
                            break;
                        case Manager:
                            typeFromInternal = RelatedNamesType.Manager;
                            break;
                        default:
                            typeFromInternal = RelatedNamesType.Custom;
                            break;
                    }
                }

                return typeFromInternal;
            }
            set
            {
                var typeInternal = RelatedNamesMapping.GetType(value);
                if (IsPreferred)
                {
                    typeInternal = typeInternal.AddFlags(RelatedNamesTypeInternal.Pref);
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
                    case RelatedNamesType.Father:
                        Label = new Label() { Value = Father };
                        break;
                    case RelatedNamesType.Mother:
                        Label = new Label() { Value = Mother };
                        break;
                    case RelatedNamesType.Parent:
                        Label = new Label() { Value = Parent };
                        break;
                    case RelatedNamesType.Brother:
                        Label = new Label() { Value = Brother };
                        break;
                    case RelatedNamesType.Sister:
                        Label = new Label() { Value = Sister };
                        break;
                    case RelatedNamesType.Child:
                        Label = new Label() { Value = Child };
                        break;
                    case RelatedNamesType.Friend:
                        Label = new Label() { Value = Friend };
                        break;
                    case RelatedNamesType.Spouse:
                        Label = new Label() { Value = Spouse };
                        break;
                    case RelatedNamesType.Partner:
                        Label = new Label() { Value = Partner };
                        break;
                    case RelatedNamesType.Assistant:
                        Label = new Label() { Value = Assistant };
                        break;
                    case RelatedNamesType.Manager:
                        Label = new Label() { Value = Manager };
                        break;
                    default:
                        Label = null;
                        break;
                }

            }
        }

        /// <summary>
        /// Gets or sets the label of the related name.
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
                    Parameters.Set("TYPE", RelatedNamesMapping.GetType(RelatedNamesType.Other).StringArrayFlags().Select(x => x.ToUpperInvariant()));
                }
                else if (value != null)
                {
                    Properties.Set("X-ABLABEL", value);
                    Parameters.Remove("TYPE");
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the related person.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets the list of properties associated with the related name.
        /// </summary>
        public virtual CardDataTypePropertyList Properties { get; protected set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="RelatedNames"/> class.
        /// </summary>
        public RelatedNames()
        {
            Initialize();
            Type = RelatedNamesType.Other;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelatedNames"/> class with a string value.
        /// </summary>
        /// <param name="value">The value of the related name.</param>
        public RelatedNames(string value)
        {
            Initialize();
            Type = RelatedNamesType.Other;

            if (string.IsNullOrWhiteSpace(value)) return;

            var serializer = new RelatedNamesSerializer();
            CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
        }

        #region Methods

        /// <summary>
        /// Initializes the properties of the related name.
        /// </summary>
        private void Initialize() => Properties = new CardDataTypePropertyList();

        /// <summary>
        /// This method is called during deserialization of the object, before the object is deserialized.
        /// </summary>
        /// <param name="context">The streaming context for the deserialization.</param>
        protected override void OnDeserializing(StreamingContext context)
        {
            base.OnDeserializing(context);

            Initialize();
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return !(obj is null) && (ReferenceEquals(this, obj) || obj.GetType() == GetType() && Equals((RelatedNames)obj));
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><see langword = "true" /> if the specified object is equal to the current object; otherwise, <see langword = "false" />.</returns>
        protected bool Equals(RelatedNames obj)
        {
            return string.Equals(Name, obj.Name, StringComparison.OrdinalIgnoreCase) &&
                   Equals(Type, obj.Type) &&
                   Equals(Label, obj.Label) &&
                   Equals(IsPreferred, obj.IsPreferred);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (Name != null ? Name.ToLowerInvariant().GetHashCode() : 0);
                hash = hash * 23 + Type.GetHashCode();
                hash = hash * 23 + (Label != null ? Label.GetHashCode() : 0);
                hash = hash * 23 + IsPreferred.GetHashCode();
                return hash;
            }
        }

        #endregion
    }
}