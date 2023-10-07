namespace iCloud.Dav.People.DataTypes;

/// <summary>
/// Interface for related data types in vCard format, providing access to a list of properties.
/// </summary>
public interface IRelatedDataType
{
    /// <summary>
    /// Gets the list of properties for the related data type.
    /// </summary>
    CardDataTypePropertyList Properties { get; }
}