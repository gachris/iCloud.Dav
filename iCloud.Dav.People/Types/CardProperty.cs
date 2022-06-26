using iCloud.Dav.Core.Utils;
using System;

namespace iCloud.Dav.People
{
    /// <summary>
    ///     A property of a <see cref="Person"/>.
    /// </summary>
    public class CardProperty
    {
        #region Fields/Consts

        private readonly SubpropertyCollection _subproperties;

        #endregion

        #region Properties

        /// <summary>The group name of the property.</summary>
        public string Group { get; set; }

        /// <summary>The language code of the property.</summary>
        public string Language { get; set; }

        /// <summary>The name of the property (e.g. TEL).</summary>
        public string Name { get; set; }

        /// <summary>The value of the property.</summary>
        public object Value { get; set; }

        /// <summary>
        ///     Subproperties of the Card property, not including
        ///     the name, encoding, and character set.
        /// </summary>
        public SubpropertyCollection Subproperties => _subproperties;

        /// <summary>
        ///     Creates a blank <see cref="CardProperty" /> object.
        /// </summary>
        public CardProperty() => _subproperties = new SubpropertyCollection();

        #endregion

        /// <summary>
        ///     Creates a <see cref="CardProperty" /> object
        ///     with the specified name and a null value.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        public CardProperty(string name) : this()
        {
            Name = name;
        }

        /// <summary>
        ///     Creates a <see cref="CardProperty" /> object with the
        ///     specified name and value.
        /// </summary>
        /// <remarks>
        ///     The Card specification supports multiple values in
        ///     certain fields, such as the N field.  The value specified
        ///     in this constructor is loaded as the first value.
        /// </remarks>
        public CardProperty(string name, string value) : this()
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        ///     Initializes a CardProperty with the specified
        ///     name, value and group.
        /// </summary>
        /// <param name="name">The name of the Card property.</param>
        /// <param name="value">The value of the Card property.</param>
        /// <param name="group">The group name of the Card property.</param>
        public CardProperty(string name, string value, string group) : this()
        {
            Group = group;
            Name = name;
            Value = value;
        }

        /// <summary>
        ///     Creates a <see cref="CardProperty" /> with the
        ///     specified name and a byte array as a value.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The value as a byte array.</param>
        public CardProperty(string name, byte[] value) : this()
        {
            Name = name.ThrowIfNullOrEmpty(nameof(name));
            Value = value;
        }

        /// <summary>
        ///     Creates a <see cref="CardProperty" /> with
        ///     the specified name and date/time as a value.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The date/time value.</param>
        public CardProperty(string name, DateTime value) : this()
        {
            Name = name.ThrowIfNullOrEmpty(nameof(name));
            Value = value;
        }

        /// <summary>
        ///     Initializes the Card property with the specified
        ///     name and values.
        /// </summary>
        public CardProperty(string name, ValueCollection values) : this()
        {
            Name = name.ThrowIfNullOrEmpty(nameof(name));
            Value = values.ThrowIfNull(nameof(values));
        }

        public override string ToString() => Value.ToString();
    }
}
