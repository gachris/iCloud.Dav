using System.Collections.Specialized;
using System.IO;

namespace iCloud.Dav.People.Utils
{
    /// <summary>
    ///     An abstract reader for Person and Person-like file formats.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The <see cref="Warnings" /> property is a string collection
    ///         containing a description of each warning encountered during
    ///         the read process.  An implementor of a card reader should
    ///         populate this collection as the Person data is being parsed.
    ///     </para>
    /// </remarks>
    internal abstract class CardReader
    {
        /// <summary>
        ///     Stores the warnings issued by the implementor
        ///     of the Person reader.  Currently warnings are
        ///     simple string messages; a future version will
        ///     store line numbers, severity levels, etc.
        /// </summary>
        /// <seealso cref="Warnings" />
        private readonly StringCollection _warnings;

        /// <summary>Initializes the base reader.</summary>
        protected CardReader() => _warnings = new StringCollection();

        /// <summary>Reads a Person from the specified input stream.</summary>
        /// <param name="reader">
        ///     A text reader that points to the beginning of
        ///     a Person in the format expected by the implementor.
        /// </param>
        /// <returns>
        ///     An initialized <see cref="Person" /> object.
        /// </returns>
        public Person Read(TextReader reader)
        {
            var card = new Person();
            ReadInto(card, reader);
            return card;
        }

        /// <summary>
        ///     Reads Person information from a text reader and
        ///     populates into an existing Person object.
        /// </summary>
        /// <param name="card">An initialized Person object.</param>
        /// <param name="reader">
        ///     A text reader containing Person data in the format
        ///     expected by the card reader class.
        /// </param>
        public abstract void ReadInto(Person card, TextReader reader);

        /// <summary>A collection of warning messages.</summary>
        /// <remarks>Reseved for future use.</remarks>
        public StringCollection Warnings => _warnings;
    }
}
