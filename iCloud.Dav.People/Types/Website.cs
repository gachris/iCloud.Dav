namespace iCloud.Dav.People
{
    /// <summary>A web site defined in a <see cref="Person"/>.</summary>
    /// <seealso cref="WebsiteTypes" />
    public class Website
    {
        #region Properties

        /// <summary>The URL of the web site.</summary>
        /// <remarks>The format of the URL is not validated.</remarks>
        public virtual string Url { get; set; }

        /// <summary>The type of web site (e.g. home page, work, etc).</summary>
        public virtual WebsiteTypes WebsiteType { get; set; }

        /// <summary>Indicates a personal home page.</summary>
        public virtual bool IsPersonalSite => (WebsiteType & WebsiteTypes.Personal) == WebsiteTypes.Personal;

        /// <summary>Indicates a work-related web site.</summary>
        public virtual bool IsWorkSite => (WebsiteType & WebsiteTypes.Work) == WebsiteTypes.Work;

        #endregion
    }
}
