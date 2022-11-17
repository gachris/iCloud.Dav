namespace iCloud.vCard.Net.Serialization.Mapping;

internal class TypeMapping<TInternal, T>
{
    public TypeMapping(TInternal typeInternal, T type)
    {
        TypeInternal = typeInternal;
        Type = type;
    }

    public TInternal TypeInternal { get; }

    public T Type { get; }
}
