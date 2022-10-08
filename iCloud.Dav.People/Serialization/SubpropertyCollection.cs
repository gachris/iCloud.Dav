using iCloud.Dav.Core.Utils;
using System;
using System.Collections;
using System.Collections.ObjectModel;

namespace iCloud.Dav.People.Serialization;

/// <summary>
///     A collection of <see cref="Subproperty" /> objects.
/// </summary>
/// <remarks>
///     <para>
///         This class is a general-purpose collection of
///         <see cref="Subproperty" /> objects.
///     </para>
///     <para>
///         A property of a Person contains a piece of
///         contact information, such as an email address
///         or web site.  A subproperty indicates options
///         or attributes of the property, such as the
///         type of email address or character set.
///     </para>
/// </remarks>
/// <seealso cref="CardProperty" />
/// <seealso cref="Subproperty" />
public class SubpropertyCollection : Collection<Subproperty>
{
    /// <summary>Adds a subproperty without a value.</summary>
    /// <param name="name">The name of the subproperty.</param>
    public void Add(string name)
    {
        name.ThrowIfNullOrEmpty(nameof(name));
        Add(new Subproperty(name));
    }

    /// <summary>
    ///     Adds a subproperty with the specified name and value.
    /// </summary>
    /// <param name="name">The name of the new subproperty to add.</param>
    /// <param name="value">
    ///     The value of the new subproperty to add.  This can be null.
    /// </param>
    public void Add(string name, string value) => Add(new Subproperty(name, value));

    /// <summary>
    ///     Either adds or updates a subproperty with the specified name.
    /// </summary>
    /// <param name="name">
    ///     The name of the subproperty to add or update.
    /// </param>
    /// <param name="value">
    ///     The value of the subproperty to add or update.
    /// </param>
    public void AddOrUpdate(string name, string value)
    {
        name.ThrowIfNullOrEmpty(nameof(name));

        var index = IndexOf(name);
        if (index == -1)
            Add(name, value);
        else
            this[index].Value = value;
    }

    /// <summary>
    ///     Determines if the collection contains a subproperty
    ///     with the specified name.
    /// </summary>
    /// <param name="name">The name of the subproperty.</param>
    /// <returns>
    ///     True if the collection contains a subproperty with the
    ///     specified name, or False otherwise.
    /// </returns>
    public bool Contains(string name)
    {
        foreach (var subproperty in this)
        {
            if (string.Compare(name, subproperty.Name, StringComparison.OrdinalIgnoreCase) == 0)
                return true;
        }
        return false;
    }

    /// <summary>
    ///     Builds a string array containing subproperty names.
    /// </summary>
    /// <returns>
    ///     A string array containing the unmodified name of
    ///     each subproperty in the collection.
    /// </returns>
    public string[] GetNames()
    {
        var arrayList = new ArrayList(Count);
        foreach (var subproperty in this)
            arrayList.Add(subproperty.Name);
        return (string[])arrayList.ToArray(typeof(string));
    }

    /// <summary>
    ///     Builds a string array containing all subproperty
    ///     names that match one of the names in an array.
    /// </summary>
    /// <param name="filteredNames">
    ///     A list of valid subproperty names.
    /// </param>
    /// <returns>
    ///     A string array containing the names of all subproperties
    ///     that match an entry in the filterNames list.
    /// </returns>
    public string[] GetNames(string[] filteredNames)
    {
        filteredNames.ThrowIfNull(nameof(filteredNames));

        var array = (string[])filteredNames.Clone();
        for (var index = 0; index < array.Length; ++index)
        {
            if (!string.IsNullOrEmpty(array[index]))
                array[index] = array[index].Trim().ToUpperInvariant();
        }
        var arrayList = new ArrayList();
        foreach (var subproperty in this)
        {
            var str = subproperty.Name?.ToUpperInvariant();
            var index = Array.IndexOf(array, str);
            if (index != -1)
                arrayList.Add(array[index]);
        }
        return (string[])arrayList.ToArray(typeof(string));
    }

    /// <summary>
    ///     Get the value of the subproperty with
    ///     the specified name.
    /// </summary>
    /// <param name="name">The name of the subproperty.</param>
    /// <returns>
    ///     The value of the subproperty or null if no
    ///     such subproperty exists in the collection.
    /// </returns>
    public string GetValue(string name)
    {
        name.ThrowIfNullOrEmpty(nameof(name));

        var index = IndexOf(name);
        return index == -1 ? null : this[index].Value;
    }

    /// <summary>
    ///     Gets the value of the first subproperty with the
    ///     specified name, or the first value specified in
    ///     a list.
    /// </summary>
    /// <param name="name">The expected name of the subproperty.</param>
    /// <param name="namelessValues">
    ///     A list of values that are sometimes listed as
    ///     subproperty names.  The first matching value is
    ///     returned if the name parameter does not match.
    /// </param>
    public string GetValue(string name, string[] namelessValues)
    {
        name.ThrowIfNullOrEmpty(nameof(name));

        var index1 = IndexOf(name);
        if (index1 != -1)
            return this[index1].Value;
        if (namelessValues == null || namelessValues.Length == 0)
            return null;
        var index2 = IndexOfAny(namelessValues);
        return index2 == -1 ? null : this[index2].Name;
    }

    /// <summary>
    ///     Searches for a subproperty with the specified name.
    /// </summary>
    /// <param name="name">The name of the subproperty.</param>
    /// <returns>
    ///     The collection (zero-based) index of the first
    ///     subproperty that matches the specified name.  The
    ///     function returns -1 if no match is found.
    /// </returns>
    public int IndexOf(string name)
    {
        for (var index = 0; index < Count; ++index)
        {
            if (string.Compare(name, this[index].Name, StringComparison.OrdinalIgnoreCase) == 0)
                return index;
        }
        return -1;
    }

    /// <summary>
    ///     Finds the first subproperty that has any of the
    ///     specified names.
    /// </summary>
    /// <param name="names">An array of names to search.</param>
    /// <returns>
    ///     The collection index of the first subproperty with
    ///     the specified name, or -1 if no subproperty was found.
    /// </returns>
    public int IndexOfAny(string[] names)
    {
        names.ThrowIfNull(nameof(names));

        for (var index = 0; index < Count; ++index)
        {
            foreach (var name in names)
            {
                if (string.Compare(this[index].Name, name, StringComparison.OrdinalIgnoreCase) == 0)
                    return index;
            }
        }
        return -1;
    }
}
