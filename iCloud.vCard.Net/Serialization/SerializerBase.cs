using System;

namespace iCloud.vCard.Net.Serialization;

public abstract class SerializerBase<T> : IServiceProvider
{
    public abstract CardPropertyList Serialize(T obj);

    public abstract void Deserialize(CardPropertyList properties, T obj);

    public object? GetService(Type serviceType) => throw new NotImplementedException();
}
