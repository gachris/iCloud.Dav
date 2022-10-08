namespace iCloud.Dav.Core;

/// <summary>Describe the type of this parameter (Path or Query).</summary>
public enum RequestParameterType
{
    /// <summary>A path parameter which is inserted into the path portion of the request URI.</summary>
    Path,
    /// <summary>A query parameter which is inserted into the query portion of the request URI.</summary>
    Query,
}
