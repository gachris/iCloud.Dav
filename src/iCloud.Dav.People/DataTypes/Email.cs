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
/// Represents an email value that can be associated with a contact.
/// </summary>
public class Email : EncodableDataType, IRelatedDataType
{
    #region Fields/Consts

    /// <summary>
    /// A constant string representing the school email type.
    /// </summary>
    public const string School = "_$!<School>!$_";

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets a value indicating whether the email is preferred.
    /// </summary>
    public virtual bool IsPreferred { get; set; }

    /// <summary>
    /// Gets or sets the type of email.
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
                Parameters.Remove("TYPE");
                Parameters.Set("TYPE", typeInternal.StringArrayFlags().Select(x => x.ToUpperInvariant()));
            }
            else
            {
                Parameters.Remove("TYPE");
            }

            switch (value)
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

    /// <summary>
    /// Gets or sets the label of the email.
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
                Parameters.Set("TYPE", EmailTypeMapping.GetType(EmailType.Other).StringArrayFlags().Select(x => x.ToUpperInvariant()));
            }
            else if (value != null)
            {
                Properties.Set("X-ABLABEL", value);
                Parameters.Remove("TYPE");
            }
        }
    }

    /// <summary>
    /// Gets or sets the address of the email.
    /// </summary>
    public virtual string Address { get; set; }

    /// <summary>
    /// Gets the list of properties associated with the email.
    /// </summary>
    public virtual CardDataTypePropertyList Properties { get; protected set; }

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="Email"/> class.
    /// </summary>
    public Email()
    {
        Initialize();
        Type = EmailType.Other;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Email"/> class with a string value.
    /// </summary>
    /// <param name="value">The value of the email.</param>
    public Email(string value)
    {
        Initialize();
        Type = EmailType.Other;

        if (string.IsNullOrWhiteSpace(value)) return;

        var serializer = new EmailSerializer();
        CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
    }

    #region Methods

    /// <summary>
    /// Initializes the properties of the email.
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
        return !(obj is null) && (ReferenceEquals(this, obj) || obj.GetType() == GetType() && Equals((Email)obj));
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns><see langword = "true" /> if the specified object is equal to the current object; otherwise, <see langword = "false" />.</returns>
    protected bool Equals(Email obj)
    {
        return string.Equals(Address, obj.Address, StringComparison.OrdinalIgnoreCase) &&
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
            hash = hash * 23 + (Address != null ? Address.ToLowerInvariant().GetHashCode() : 0);
            hash = hash * 23 + Type.GetHashCode();
            hash = hash * 23 + (Label != null ? Label.GetHashCode() : 0);
            hash = hash * 23 + IsPreferred.GetHashCode();
            return hash;
        }
    }

    #endregion
}