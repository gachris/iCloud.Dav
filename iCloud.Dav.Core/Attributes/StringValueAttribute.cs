using iCloud.Dav.Core.Utils;
using System;

namespace iCloud.Dav.Core.Attributes
{
    /// <summary>Defines an attribute containing a string representation of the member.</summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class StringValueAttribute : Attribute
    {
        private readonly string text;

        /// <summary>The text which belongs to this member.</summary>
        public string Text
        {
            get
            {
                return this.text;
            }
        }

        /// <summary>Creates a new string value attribute with the specified text.</summary>
        public StringValueAttribute(string text)
        {
            this.text = text.ThrowIfNullOrEmpty(nameof(text));
        }
    }
}
