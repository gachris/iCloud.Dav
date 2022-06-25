using System;
using System.Collections.Specialized;
using System.IO;

namespace iCloud.Dav.People.Utils
{
    /// <summary>Base class for Person generators.</summary>
    /// <seealso cref="CardReader" />
    /// <seealso cref="CardStandardWriter" />
    internal abstract class CardWriter
    {
        /// <summary>Holds output warnings.</summary>
        private readonly StringCollection _warnings = new();

        /// <summary>
        ///     A collection of warning messages that were generated
        ///     during the output of a Person.
        /// </summary>
        public StringCollection Warnings => _warnings;

        /// <summary>
        ///     Writes a Person to an I/O stream using the format
        ///     implemented by the class.
        /// </summary>
        /// <param name="card">The Person to write the I/O string.</param>
        /// <param name="output">The text writer to use for output.</param>
        /// <param name="charsetName">The charsetName to use for output.</param>
        /// <remarks>
        ///     The implementor should not close or flush the stream.
        ///     The caller owns the stream and may not wish for the
        ///     stream to be closed (e.g. the caller may call the
        ///     function again with a different Person).
        /// </remarks>
        public abstract void Write(Person card, TextWriter output, string charsetName);

        /// <summary>Writes the Person to the specified filename.</summary>
        public virtual void Write(Person card, string filename, string charsetName)
        {
            if (card == null)
                throw new ArgumentNullException(nameof(card));
            using var streamWriter = new StreamWriter(filename);
            Write(card, streamWriter, charsetName);
        }
    }
}
