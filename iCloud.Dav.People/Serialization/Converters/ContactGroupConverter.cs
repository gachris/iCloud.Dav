using iCloud.Dav.People.Serialization.Read;
using iCloud.Dav.People.Types;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;

namespace iCloud.Dav.People.Serialization.Converters;

internal sealed class ContactGroupConverter : TypeConverter
{
    /// <summary>
    /// TypeConverter method override.
    /// </summary>
    /// <param name="context">ITypeDescriptorContext</param>
    /// <param name="sourceType">Type to convert from</param>
    /// <returns>true if conversion is possible</returns>
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type? sourceType)
    {
        if (sourceType == typeof(string))
            return true;
        return false;
    }

    /// <summary>
    /// TypeConverter method implementation.
    /// </summary>
    /// <param name="context">ITypeDescriptorContext</param>
    /// <param name="culture">current culture (see CLR specs)</param>
    /// <param name="value">value to convert from</param>
    /// <returns>value that is result of conversion</returns>
    public override object ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        var input = (string)value;
        if (!string.IsNullOrEmpty(input))
        {
            var contactGroup = new ContactGroup();
            var bytes = Encoding.UTF8.GetBytes(input);
            var contactGroupReader = new ContactGroupReader();
            contactGroupReader.ReadInto(contactGroup, new StringReader(Encoding.UTF8.GetString(bytes)));
            return contactGroup;
        }
        throw GetConvertFromException(value);
    }

    ///// <summary>
    /////     Loads a new instance of the <see cref="ContactGroup" /> class
    /////     from a text reader.
    ///// </summary>
    ///// <param name="input">An initialized text reader.</param>
    //public ContactGroup(TextReader input) : this()
    //{
    //    new ContactGroupReader().ReadInto(this, input);
    //}

    ///// <summary>
    /////     Loads a new instance of the <see cref="ContactGroup" /> class
    /////     from a byte array.
    ///// </summary>
    ///// <param name="bytes">An initialized byte array.</param>
    //public ContactGroup(byte[] bytes) : this()
    //{
    //    var standardReader = new ContactGroupReader();
    //    standardReader.ReadInto(this, new StringReader(Encoding.UTF8.GetString(bytes)));
    //}

    ///// <summary>
    /////     Loads a new instance of the <see cref="ContactGroup" /> class
    /////     from a text file.
    ///// </summary>
    ///// <param name="path">
    /////     The path to a text file containing ContactGroup data in
    /////     any recognized ContactGroup format.
    ///// </param>
    //public ContactGroup(string path) : this()
    //{
    //    using var streamReader = new StreamReader(path);
    //    new ContactGroupReader().ReadInto(this, streamReader);
    //}

}
