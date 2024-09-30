using System.Runtime.Serialization;
using iCloud.Dav.People.DataTypes.Mapping;
using iCloud.Dav.People.Extensions;
using iCloud.Dav.People.Serialization.DataTypes;
using vCard.Net;
using vCard.Net.DataTypes;

namespace iCloud.Dav.People.DataTypes;

/// <summary>
/// Represents a web site value that can be associated with a contact.
/// </summary>
public class Website : EncodableDataType, IRelatedDataType
{
    #region Fields/Consts

    /// <summary>
    /// A constant string representing the home page web site type.
    /// </summary>
    public const string HomePage = "_$!<HomePage>!$_";

    /// <summary>
    /// A constant string representing the school web site type.
    /// </summary>
    public const string School = "_$!<School>!$_";

    /// <summary>
    /// A constant string representing the blog web site type.
    /// </summary>
    public const string Blog = "BLOG";

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets a value indicating whether the web site is preferred.
    /// </summary>
    public virtual bool IsPreferred { get; set; }

    /// <summary>
    /// Gets or sets the type of web site.
    /// </summary>
    public virtual WebsiteType Type
    {
        get
        {
            var types = Parameters.GetMany("TYPE");

            _ = types.TryParse<WebsiteTypeInternal>(out var typeInternal);
            var isPreferred = typeInternal.HasFlag(WebsiteTypeInternal.Pref);
            if (isPreferred)
            {
                IsPreferred = true;
                typeInternal = typeInternal.RemoveFlags(WebsiteTypeInternal.Pref);
            }

            var typeFromInternal = WebsiteTypeMapping.GetType(typeInternal);
            if (typeFromInternal is 0)
            {
                typeFromInternal = (Label?.Value) switch
                {
                    HomePage => WebsiteType.HomePage,
                    School => WebsiteType.School,
                    Blog => WebsiteType.Blog,
                    _ => WebsiteType.Custom,
                };
            }

            return typeFromInternal;
        }
        set
        {
            var typeInternal = WebsiteTypeMapping.GetType(value);
            if (IsPreferred)
            {
                typeInternal = typeInternal.AddFlags(WebsiteTypeInternal.Pref);
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

            Label = value switch
            {
                WebsiteType.HomePage => new Label() { Value = School },
                WebsiteType.Blog => new Label() { Value = Blog },
                WebsiteType.Custom => new Label() { Value = Label?.Value },
                _ => null,
            };
        }
    }

    /// <summary>
    /// Gets or sets the label of the web site.
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
                Parameters.Set("TYPE", WebsiteTypeMapping.GetType(WebsiteType.Other).StringArrayFlags().Select(x => x.ToUpperInvariant()));
            }
            else if (value != null)
            {
                Properties.Set("X-ABLABEL", value);
                Parameters.Remove("TYPE");
            }
        }
    }

    /// <summary>
    /// Gets or sets the URL of the web site.
    /// </summary>
    /// <remarks>
    /// Note that this property is not validated and may contain invalid or incomplete URLs.
    /// </remarks>
    public virtual string Url { get; set; }

    /// <summary>
    /// Gets the list of properties associated with the web site.
    /// </summary>
    public virtual CardDataTypePropertyList Properties { get; protected set; }

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="Website"/> class.
    /// </summary>
    public Website()
    {
        Initialize();
        Type = WebsiteType.Other;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Website"/> class with a string value.
    /// </summary>
    /// <param name="value">The value of the web site.</param>
    public Website(string value)
    {
        Initialize();
        Type = WebsiteType.Other;

        if (string.IsNullOrWhiteSpace(value)) return;

        var serializer = new WebsiteSerializer();
        CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
    }

    #region Methods

    /// <summary>
    /// Initializes the properties of the web site.
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
        return obj is not null && (ReferenceEquals(this, obj) || obj.GetType() == GetType() && Equals((Website)obj));
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns><see langword = "true" /> if the specified object is equal to the current object; otherwise, <see langword = "false" />.</returns>
    protected bool Equals(Website obj)
    {
        return Equals(IsPreferred, obj.IsPreferred) &&
               Equals(Type, obj.Type) &&
               Equals(Label, obj.Label) &&
               string.Equals(Url, obj.Url, StringComparison.OrdinalIgnoreCase);
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
            hash = hash * 23 + (Url != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Url) : 0);
            return hash;
        }
    }

    #endregion
}