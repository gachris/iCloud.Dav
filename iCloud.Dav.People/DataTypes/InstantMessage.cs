using iCloud.Dav.People.DataTypes.Mapping;
using iCloud.Dav.People.Extensions;
using iCloud.Dav.People.Serialization.DataTypes;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using vCard.Net;
using vCard.Net.DataTypes;

namespace iCloud.Dav.People.DataTypes;

/// <summary>
/// Represents an instant message value that can be associated with a contact.
/// </summary>
public class InstantMessage : EncodableDataType, IRelatedDataType
{
    #region Properties

    /// <summary>
    /// Gets or sets a value indicating whether this instant message is preferred.
    /// </summary>
    public virtual bool IsPreferred { get; set; }

    /// <summary>
    /// Gets or sets the instant message type.
    /// </summary>
    public virtual InstantMessageType Type
    {
        get
        {
            var types = Parameters.GetMany("TYPE");

            _ = types.TryParse<InstantMessageTypeInternal>(out var typeInternal);
            var isPreferred = typeInternal.HasFlag(InstantMessageTypeInternal.Pref);
            if (isPreferred)
            {
                IsPreferred = true;
                typeInternal = typeInternal.RemoveFlags(InstantMessageTypeInternal.Pref);
            }

            var typeFromInternal = InstantMessageTypeMapping.GetType(typeInternal);
            if (typeFromInternal is 0)
            {
                switch (Label?.Value)
                {
                    default:
                        typeFromInternal = InstantMessageType.Custom;
                        break;
                }
            }

            return typeFromInternal;
        }
        set
        {
            var typeInternal = InstantMessageTypeMapping.GetType(value);
            if (IsPreferred)
            {
                typeInternal = typeInternal.AddFlags(InstantMessageTypeInternal.Pref);
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
                case InstantMessageType.Custom:
                    Label = new Label() { Value = Label?.Value };
                    break;
                default:
                    Label = null;
                    break;
            }
        }
    }

    /// <summary>
    /// Gets or sets the label associated with the instant message.
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
                Parameters.Set("TYPE", InstantMessageTypeMapping.GetType(InstantMessageType.Other).StringArrayFlags().Select(x => x.ToUpperInvariant()));
            }
            else if (value != null)
            {
                Properties.Set("X-ABLABEL", value);
                Parameters.Remove("TYPE");
            }
        }
    }

    /// <summary>
    /// Gets or sets the user name associated with the instant message.
    /// </summary>
    public virtual string UserName { get; set; }

    /// <summary>
    /// Gets or sets the type of instant messaging service used for the contact.
    /// </summary>
    public virtual InstantMessageServiceType ServiceType
    {
        get
        {
            _ = Parameters.Get("X-SERVICE-TYPE").TryParse<InstantMessageServiceType>(out var serviceType);
            return serviceType;
        }
        set => Parameters.Set("X-SERVICE-TYPE", value.StringArrayFlags().Select(x => x.ToLowerInvariant()));
    }

    /// <summary>
    /// Gets or sets a collection of properties associated with the instant message.
    /// </summary>
    public virtual CardDataTypePropertyList Properties { get; protected set; }

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="InstantMessage"/> class.
    /// </summary>
    public InstantMessage()
    {
        Initialize();
        Type = InstantMessageType.Other;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstantMessage"/> class with a string value.
    /// </summary>
    /// <param name="value">A string representation of the instant message value.</param>
    public InstantMessage(string value)
    {
        Initialize();
        Type = InstantMessageType.Other;

        if (string.IsNullOrWhiteSpace(value)) return;

        var serializer = new InstantMessageSerializer();
        CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
    }

    #region Methods

    /// <summary>
    /// Initializes the properties of the instant message.
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
        return !(obj is null) && (ReferenceEquals(this, obj) || obj.GetType() == GetType() && Equals((InstantMessage)obj));
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns><see langword = "true" /> if the specified object is equal to the current object; otherwise, <see langword = "false" />.</returns>
    protected bool Equals(InstantMessage obj)
    {
        return string.Equals(UserName, obj.UserName, StringComparison.OrdinalIgnoreCase) &&
               Equals(ServiceType, obj.ServiceType) &&
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
            hash = hash * 23 + (UserName != null ? UserName.ToLowerInvariant().GetHashCode() : 0);
            hash = hash * 23 + ServiceType.GetHashCode();
            hash = hash * 23 + Type.GetHashCode();
            hash = hash * 23 + (Label != null ? Label.GetHashCode() : 0);
            hash = hash * 23 + IsPreferred.GetHashCode();
            return hash;
        }
    }

    #endregion
}