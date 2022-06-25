using iCloud.Dav.Core.Utils;

namespace iCloud.Dav.People
{
    /// <summary>A subproperty of a Person property.</summary>
    /// <remarks>
    ///     <para>
    ///         A Person is fundamentally a set of properties in NAME:VALUE
    ///         format, where the name is a keyword like "EMAIL" and the
    ///         value is a string appropriate for the keyword (e.g. an email
    ///         address for the EMAIL property, or a BASE64 encoded image
    ///         for the PHOTO property).
    ///     </para>
    ///     <para>
    ///         All Person properties support subproperties.  These can
    ///         be global options like encoding or value type, or might be
    ///         options specific to the keyword.  For example, all Person
    ///         properties can have an encoding subproperty that identifies
    ///         the text encoding of the value.  A phone property, however,
    ///         supports special properties that identify the type and purpose
    ///         of the phone.
    ///     </para>
    ///     <para>
    ///         A subproperty is not required to have a value.  In such a case
    ///         the subproperty acts like a flag.  For example, the TEL
    ///         property of the Person specification is used to indicate a
    ///         telephone number associated with the person.  This property
    ///         supports a subproperty called BBS, which indicates the telephone
    ///         number is for a dial-up bulletin board system.  The BBS
    ///         subproperty does not need a value; the existance of the BBS
    ///         subproperty is sufficient to indicate the telephone number is
    ///         for a BBS system.
    ///     </para>
    /// </remarks>
    public class Subproperty
    {
        #region Properties

        /// <summary>The name of the subproperty.</summary>
        public virtual string Name { get; set; }

        /// <summary>The optional value of the subproperty.</summary>
        public virtual string Value { get; set; }

        #endregion

        /// <summary>
        ///     Creates a subproperty with the specified
        ///     name and no value.
        /// </summary>
        /// <param name="name">The name of the subproperty.</param>
        public Subproperty(string name) => Name = name.ThrowIfNullOrEmpty(nameof(name));

        /// <summary>
        ///     Creates a subproperty with the specified
        ///     name and value.
        /// </summary>
        /// <param name="name">The name of the subproperty.</param>
        /// <param name="value">
        ///     The value of the subproperty.  This can be null.
        /// </param>
        public Subproperty(string name, string value)
        {
            Name = name.ThrowIfNullOrEmpty(nameof(name));
            Value = value;
        }
    }
}
