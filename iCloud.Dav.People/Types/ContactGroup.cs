using iCloud.Dav.Core;
using iCloud.Dav.People.Serialization.Converters;
using iCloud.Dav.People.Serialization.Read;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace iCloud.Dav.People.Types;

[TypeConverter(typeof(ContactGroupConverter))]
public class ContactGroup : IDirectResponseSchema
{
    #region Properties

    /// <summary>A value that uniquely identifies the Person.</summary>
    /// <remarks>
    ///     This value is optional.  The string must be any string
    ///     that can be used to uniquely identify the Person.  The
    ///     usage of the field is determined by the software.  Typical
    ///     possibilities for a unique string include a URL, a GUID,
    ///     or an LDAP directory path.  However, there is no particular
    ///     standard dictated by the Person specification.
    /// </remarks>
    public virtual string? UniqueId { get; set; }

    /// <summary>The name of the group.</summary>
    public virtual string? Name { get; set; }

    /// <summary>The formatted name of the group.</summary>
    /// <remarks>
    ///     This property allows the name of the group to be
    ///     written in a manner specific to his or her culture.
    ///     The formatted name is not required to strictly
    ///     correspond with the family name, given name, etc.
    /// </remarks>
    public virtual string? FormattedName { get; set; }

    /// <summary>The name of the product that generated the group.</summary>
    public virtual string? ProductId { get; set; }

    /// <summary>The revision date of the group.</summary>
    /// <remarks>
    ///     The revision date is not automatically updated by the
    ///     group when modifying properties.  It is up to the
    ///     developer to change the revision date as needed.
    /// </remarks>
    public virtual DateTime? RevisionDate { get; set; }

    /// <summary>
    /// The group type of the ContactGroup.
    /// </summary>
    public virtual string? GroupType { get; internal set; }

    /// <summary>The e-tag of the person.</summary>
    /// <remarks>
    /// Will be set by the service deserialization method,
    /// or the by json response parser if implemented on service.
    /// </remarks>
    public virtual string? ETag { get; set; }

    /// <summary>
    /// The member resource names of the ContactGroup.
    /// </summary>
    public virtual IList<string> MemberResourceNames { get; }

    /// <summary>
    /// The resource name of ContactGroup.
    /// </summary>
    public virtual string? ResourceName { get; set; }

    /// <summary>
    /// The Url of the ContactGroup.
    /// </summary>
    public virtual string? Url { get; internal set; }

    /// <summary>
    /// The count of members.
    /// </summary>
    public virtual int MemberCount => MemberResourceNames.Count;

    #endregion

    /// <summary>
    ///     Initializes a new instance of the <see cref="ContactGroup" /> class.
    /// </summary>
    public ContactGroup() : base()
    {
        GroupType = "group";
        MemberResourceNames = new List<string>();
    }

    /// <summary>
    ///     Loads a new instance of the <see cref="ContactGroup" /> class
    ///     from a text reader.
    /// </summary>
    /// <param name="input">An initialized text reader.</param>
    public ContactGroup(TextReader input) : this()
    {
        new ContactGroupReader().ReadInto(this, input);
    }

    /// <summary>
    ///     Loads a new instance of the <see cref="ContactGroup" /> class
    ///     from a byte array.
    /// </summary>
    /// <param name="bytes">An initialized byte array.</param>
    public ContactGroup(byte[] bytes) : this()
    {
        var standardReader = new ContactGroupReader();
        standardReader.ReadInto(this, new StringReader(Encoding.UTF8.GetString(bytes)));
    }

    /// <summary>
    ///     Loads a new instance of the <see cref="ContactGroup" /> class
    ///     from a text file.
    /// </summary>
    /// <param name="path">
    ///     The path to a text file containing ContactGroup data in
    ///     any recognized ContactGroup format.
    /// </param>
    public ContactGroup(string path) : this()
    {
        using var streamReader = new StreamReader(path);
        new ContactGroupReader().ReadInto(this, streamReader);
    }

    #region Methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="uniqueId"></param>
    /// <returns></returns>
    public virtual bool AddMemberResource(string uniqueId)
    {
        if (!MemberResourceNames.Contains(uniqueId))
        {
            MemberResourceNames.Add(uniqueId);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="uniqueId"></param>
    /// <returns></returns>
    public virtual bool RemoveMemberResource(string uniqueId)
    {
        if (MemberResourceNames.Contains(uniqueId))
        {
            MemberResourceNames.Remove(uniqueId);
            return true;
        }
        return false;
    }

    #endregion
}
