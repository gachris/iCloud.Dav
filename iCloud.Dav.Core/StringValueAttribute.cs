using iCloud.Dav.Core.Utils;
using System;

namespace iCloud.Dav.Core;

/// <summary>Defines an attribute containing a string representation of the member.</summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class StringValueAttribute : Attribute
{
    /// <summary>The text which belongs to this member.</summary>
    public string Text { get; }

    /// <summary>Creates a new string value attribute with the specified text.</summary>
    public StringValueAttribute(string text)
    {
        Text = text.ThrowIfNullOrEmpty(nameof(text));
    }
}
