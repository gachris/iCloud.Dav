using System;

namespace iCloud.Dav.People
{
    /// <summary>A source of directory information for a <see cref="Person"/>.</summary>
    /// <remarks>
    ///     <para>
    ///         A source identifies a directory that contains or provided
    ///         information for the Person.  A source consists of a URI
    ///         and a context.  The URI is generally a URL; the
    ///         context identifies the protocol and type of URI.  For
    ///         example, a Person associated with an LDAP directory entry
    ///         will have an ldap:// URL and a context of "LDAP".
    ///     </para>
    /// </remarks>
    public class Source
    {
        #region Properties

        /// <summary>The context of the source URI.</summary>
        /// <remarks>
        ///     The context identifies how the URI should be
        ///     interpreted.  Example is "LDAP", which indicates
        ///     the URI is an LDAP reference.
        /// </remarks>
        public virtual string Context { get; set; }

        /// <summary>The URI of the source.</summary>
        public virtual Uri Uri { get; set; }

        #endregion
    }
}
