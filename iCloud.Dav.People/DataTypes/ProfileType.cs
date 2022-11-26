namespace iCloud.Dav.People.DataTypes
{
    /// <summary>Identifies the type of social profile type in a Person.</summary>
    /// <seealso cref="Profile" />
    public enum ProfileType
    {
        /// <summary>Indicates the twitter type.</summary>
        Twitter = 1,
        /// <summary>Indicates the facebook type.</summary>
        Facebook = 2,
        /// <summary>Indicates the linkedin type.</summary>
        LinkedIn = 3,
        /// <summary>Indicates the flickr type.</summary>
        Flickr = 4,
        /// <summary>Indicates the myspace type.</summary>
        Myspace = 5,
        /// <summary>Indicates the sina weibo type.</summary>
        SinaWeibo = 6,
        /// <summary>Indicates the unknown type.</summary>
        Custom = 7,
    }
}