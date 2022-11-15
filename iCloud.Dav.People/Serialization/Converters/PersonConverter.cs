using iCloud.Dav.People.Serialization.Read;
using iCloud.Dav.People.Serialization.Write;
using iCloud.Dav.People.Types;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;

namespace iCloud.Dav.People.Serialization.Converters;

public sealed class PersonConverter : TypeConverter
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
            var person = new Person();
            var cardStandardReader = new ContactReader();
            cardStandardReader.ReadInto(person, new StringReader(input));
            return person;
        }
        throw GetConvertFromException(value);
    }

    public object ConvertFromPath(string path)
    {
        using var streamReader = new StreamReader(path);
        var person = new Person();
        new ContactReader().ReadInto(person, streamReader);

        person.UniqueId = Guid.NewGuid().ToString().ToUpper();

        return person;
    }

    public object ConvertToStream(Person person)
    {
        var stream = new MemoryStream();
        var textWriter = new StreamWriter(stream);
        var writer = new ContactWriter();
        writer.Write(person, textWriter, Encoding.UTF8.WebName);
        textWriter.Flush();
        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }

    //public string ReadAsString()
    //{
    //    return ReadAsStreamReader().ReadToEnd();
    //}

    //public StreamReader ReadAsStreamReader()
    //{
    //    var stream = ReadAsStream();
    //    var streamReader = new StreamReader(stream);
    //    return streamReader;
    //}
}
