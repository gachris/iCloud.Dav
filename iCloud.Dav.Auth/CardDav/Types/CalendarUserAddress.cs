namespace iCloud.Dav.Auth.CardDav.Types;

internal sealed class CalendarUserAddress
{
    public bool Preferred { get; }

    public string Value { get; }

    public CalendarUserAddress(bool preferred, string value) => (Preferred, Value) = (preferred, value);
}
