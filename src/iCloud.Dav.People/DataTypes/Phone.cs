using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using iCloud.Dav.People.DataTypes.Mapping;
using iCloud.Dav.People.Extensions;
using iCloud.Dav.People.Serialization.DataTypes;
using vCard.Net;
using vCard.Net.DataTypes;

namespace iCloud.Dav.People.DataTypes;

/// <summary>
/// Represents a phone value that can be associated with a contact.
/// </summary>
public class Phone : EncodableDataType, IRelatedDataType
{
    #region Fields/Consts

    /// <summary>
    /// A constant string representing the apple watch phone type.
    /// </summary>
    public const string AppleWatch = "APPLE WATCH";

    /// <summary>
    /// A constant string representing the school phone type.
    /// </summary>
    public const string School = "_$!<School>!$_";

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets a value indicating whether this phone is preferred.
    /// </summary>
    public virtual bool IsPreferred { get; set; }

    /// <summary>
    /// Gets or sets the phone type.
    /// </summary>
    public virtual PhoneType Type
    {
        get
        {
            var types = Parameters.GetMany("TYPE");

            _ = types.TryParse<PhoneTypeInternal>(out var typeInternal);
            var isPreferred = typeInternal.HasFlag(PhoneTypeInternal.Pref);
            if (isPreferred)
            {
                IsPreferred = true;
                typeInternal = typeInternal.RemoveFlags(PhoneTypeInternal.Pref);
            }

            var typeFromInternal = PhoneTypeMapping.GetType(typeInternal);
            if (typeFromInternal is 0)
            {
                switch (Label?.Value)
                {
                    case School:
                        typeFromInternal = PhoneType.School;
                        break;
                    case AppleWatch:
                        typeFromInternal = PhoneType.AppleWatch;
                        break;
                    default:
                        typeFromInternal = PhoneType.Custom;
                        break;
                }
            }

            return typeFromInternal;
        }
        set
        {
            var typeInternal = PhoneTypeMapping.GetType(value);
            if (IsPreferred)
            {
                typeInternal = typeInternal.AddFlags(PhoneTypeInternal.Pref);
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
                case PhoneType.School:
                    Label = new Label() { Value = School };
                    break;
                case PhoneType.AppleWatch:
                    Label = new Label() { Value = AppleWatch };
                    break;
                case PhoneType.Custom:
                    Label = new Label() { Value = Label?.Value };
                    break;
                default:
                    Label = null;
                    break;
            }
        }
    }

    /// <summary>
    /// Gets or sets the label associated with the phone.
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
                Parameters.Set("TYPE", PhoneTypeMapping.GetType(PhoneType.Other).StringArrayFlags().Select(x => x.ToUpperInvariant()));
            }
            else if (value != null)
            {
                Properties.Set("X-ABLABEL", value);
                Parameters.Remove("TYPE");
            }
        }
    }

    /// <summary>
    /// Gets or sets the full phone number, including any area codes, country codes, and extension numbers.
    /// </summary>
    public virtual string FullNumber { get; set; }

    /// <summary>
    /// Gets or sets a collection of properties associated with the phone.
    /// </summary>
    public virtual CardDataTypePropertyList Properties { get; protected set; }

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="Phone"/> class.
    /// </summary>
    public Phone()
    {
        Initialize();
        Type = PhoneType.Other;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Phone"/> class with a string value.
    /// </summary>
    /// <param name="value">A string representation of the phone value.</param>
    public Phone(string value)
    {
        Initialize();
        Type = PhoneType.Other;

        if (string.IsNullOrWhiteSpace(value)) return;

        var serializer = new PhoneSerializer();
        CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
    }

    #region Methods

    /// <summary>
    /// Initializes the properties of the phone.
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
        return !(obj is null) && (ReferenceEquals(this, obj) || obj.GetType() == GetType() && Equals((Phone)obj));
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns><see langword = "true" /> if the specified object is equal to the current object; otherwise, <see langword = "false" />.</returns>
    protected bool Equals(Phone obj)
    {
        return string.Equals(FullNumber, obj.FullNumber, StringComparison.OrdinalIgnoreCase) &&
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
            hash = hash * 23 + (FullNumber != null ? FullNumber.ToLowerInvariant().GetHashCode() : 0);
            hash = hash * 23 + Type.GetHashCode();
            hash = hash * 23 + (Label != null ? Label.GetHashCode() : 0);
            hash = hash * 23 + IsPreferred.GetHashCode();
            return hash;
        }
    }

    #endregion
}