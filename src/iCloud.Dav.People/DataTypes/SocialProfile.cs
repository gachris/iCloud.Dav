using System.Runtime.Serialization;
using iCloud.Dav.People.DataTypes.Mapping;
using iCloud.Dav.People.Extensions;
using iCloud.Dav.People.Serialization.DataTypes;
using vCard.Net;
using vCard.Net.DataTypes;

namespace iCloud.Dav.People.DataTypes;

/// <summary>
/// Represents a social profile value that can be associated with a contact.
/// </summary>
public class SocialProfile : EncodableDataType, IRelatedDataType
{
    #region Fields/Consts

    /// <summary>
    /// A constant string representing the Twitter URL.
    /// </summary>
    public static string Twitter = "https://twitter.com/{0}";

    /// <summary>
    /// A constant string representing the Facebook URL.
    /// </summary>
    public static string Facebook = "https://www.facebook.com/{0}";

    /// <summary>
    /// A constant string representing the LinkedIn URL.
    /// </summary>
    public static string LinkedIn = "https://www.linkedin.com/in/{0}";

    /// <summary>
    /// A constant string representing the Flickr URL.
    /// </summary>
    public static string Flickr = "https://www.flickr.com/photos/{0}/";

    /// <summary>
    /// A constant string representing the Myspace URL.
    /// </summary>
    public static string Myspace = "https://www.myspace.com/{0}";

    /// <summary>
    /// A constant string representing the SinaWeibo URL.
    /// </summary>
    public static string SinaWeibo = "https://www.weibo.com/n/{0}";

    /// <summary>
    /// A constant string representing a custom URL.
    /// </summary>
    public static string Custom = "x-apple:{0}";

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets a value indicating whether the social profile is preferred.
    /// </summary>
    public virtual bool IsPreferred { get; set; }

    /// <summary>
    /// Gets or sets the type of social profile.
    /// </summary>
    public virtual SocialProfileType Type
    {
        get
        {
            var types = Parameters.GetMany("TYPE");

            _ = types.TryParse<SocialProfileTypeInternal>(out var typeInternal);
            var isPreferred = typeInternal.HasFlag(SocialProfileTypeInternal.Pref);
            if (isPreferred)
            {
                IsPreferred = true;
                typeInternal = typeInternal.RemoveFlags(SocialProfileTypeInternal.Pref);
            }

            var typeFromInternal = SocialProfileTypeMapping.GetType(typeInternal);
            if (typeFromInternal is 0)
            {
                typeFromInternal = SocialProfileType.Custom;
            }

            return typeFromInternal;
        }
        set
        {
            var typeInternal = SocialProfileTypeMapping.GetType(value);
            if (IsPreferred)
            {
                typeInternal = typeInternal.AddFlags(SocialProfileTypeInternal.Pref);
            }

            if (typeInternal is not 0)
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
                case SocialProfileType.Twitter:
                    Label = null;
                    Url = string.Format(Twitter, UserName);
                    break;
                case SocialProfileType.Facebook:
                    Url = string.Format(Facebook, UserName);
                    Label = null;
                    break;
                case SocialProfileType.LinkedIn:
                    Url = string.Format(LinkedIn, UserName);
                    Label = null;
                    break;
                case SocialProfileType.Flickr:
                    Url = string.Format(Flickr, UserName);
                    Label = null;
                    break;
                case SocialProfileType.Myspace:
                    Url = string.Format(Myspace, UserName);
                    Label = null;
                    break;
                case SocialProfileType.SinaWeibo:
                    Url = string.Format(SinaWeibo, UserName);
                    Label = null;
                    break;
                case SocialProfileType.Custom:
                    Url = string.Format(Custom, UserName);
                    Label = new Label() { Value = Label?.Value };
                    break;
            }
        }
    }

    /// <summary>
    /// Gets or sets the label of the social profile.
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
                Parameters.Set("TYPE", SocialProfileTypeMapping.GetType(SocialProfileType.Twitter).StringArrayFlags().Select(x => x.ToUpperInvariant()));
            }
            else if (value != null)
            {
                Properties.Set("X-ABLABEL", value);
                Parameters.Remove("TYPE");
            }
        }
    }

    /// <summary>
    /// Gets or sets the user name of the social profile.
    /// </summary>
    public virtual string UserName
    {
        get => Parameters.Get("X-USER");
        set
        {
            Parameters.Set("X-USER", value);

            switch (Type)
            {
                case SocialProfileType.Twitter:
                    Url = string.Format(Twitter, UserName);
                    break;
                case SocialProfileType.Facebook:
                    Url = string.Format(Facebook, UserName);
                    break;
                case SocialProfileType.LinkedIn:
                    Url = string.Format(LinkedIn, UserName);
                    break;
                case SocialProfileType.Flickr:
                    Url = string.Format(Flickr, UserName);
                    break;
                case SocialProfileType.Myspace:
                    Url = string.Format(Myspace, UserName);
                    break;
                case SocialProfileType.SinaWeibo:
                    Url = string.Format(SinaWeibo, UserName);
                    break;
                case SocialProfileType.Custom:
                    Url = string.Format(Custom, UserName);
                    break;
            }
        }
    }

    /// <summary>
    /// Gets or sets the URL of the social profile. The URL is automatically generated by combining the social profile's type and user name.
    /// </summary>
    /// <remarks>
    /// For example, if the type is <c>SocialProfileType.Twitter</c> and the user name is "johndoe", the URL would be "https://twitter.com/johndoe".
    /// Note that this property is not validated and may contain invalid or incomplete URLs.
    /// </remarks>
    public virtual string Url { get; set; }

    /// <summary>
    /// Gets the list of properties associated with the social profile.
    /// </summary>
    public virtual CardDataTypePropertyList Properties { get; protected set; }

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="SocialProfile"/> class.
    /// </summary>
    public SocialProfile()
    {
        Initialize();
        Type = SocialProfileType.Twitter;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SocialProfile"/> class with a string value.
    /// </summary>
    /// <param name="value">The value of the social profile.</param>
    public SocialProfile(string value)
    {
        Initialize();
        Type = SocialProfileType.Twitter;

        if (string.IsNullOrWhiteSpace(value)) return;

        var serializer = new SocialProfileSerializer();
        CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
    }

    #region Methods

    /// <summary>
    /// Initializes the properties of the social profile.
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
        return obj is not null && (ReferenceEquals(this, obj) || obj.GetType() == GetType() && Equals((SocialProfile)obj));
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns><see langword = "true" /> if the specified object is equal to the current object; otherwise, <see langword = "false" />.</returns>
    protected bool Equals(SocialProfile obj)
    {
        return string.Equals(UserName, obj.UserName, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(Url, obj.Url, StringComparison.OrdinalIgnoreCase) &&
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
            hash = hash * 23 + (Url != null ? Url.ToLowerInvariant().GetHashCode() : 0);
            hash = hash * 23 + Type.GetHashCode();
            hash = hash * 23 + (Label != null ? Label.GetHashCode() : 0);
            hash = hash * 23 + IsPreferred.GetHashCode();
            return hash;
        }
    }

    #endregion
}