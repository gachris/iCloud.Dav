using System;

namespace iCloud.Dav.People.Serialization;

/// <summary>
/// Represents a delegate that resolves the .NET type that corresponds to a vCard property.
/// </summary>
/// <param name="context">The context object.</param>
/// <returns>The .NET type that corresponds to the vCard property.</returns>
internal delegate Type TypeResolverDelegate(object context);