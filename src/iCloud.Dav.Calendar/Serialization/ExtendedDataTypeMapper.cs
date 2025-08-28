using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;

namespace iCloud.Dav.Calendar.Serialization;

internal class ExtendedDataTypeMapper
{
    private class PropertyMapping
    {
        public Type ObjectType { get; set; }

        public TypeResolverDelegate Resolver { get; set; }

        public bool AllowsMultipleValuesPerProperty { get; set; }
    }

    private readonly IDictionary<string, PropertyMapping> _propertyMap = new Dictionary<string, PropertyMapping>(StringComparer.OrdinalIgnoreCase);

    public ExtendedDataTypeMapper()
    {
        AddPropertyMapping("ACTION", typeof(AlarmAction), allowsMultipleValues: false);
        AddPropertyMapping("ATTACH", typeof(Attachment), allowsMultipleValues: false);
        AddPropertyMapping("ATTENDEE", typeof(Attendee), allowsMultipleValues: false);
        AddPropertyMapping("CATEGORIES", typeof(string), allowsMultipleValues: true);
        AddPropertyMapping("COMMENT", typeof(string), allowsMultipleValues: false);
        AddPropertyMapping("COMPLETED", typeof(CalDateTime), allowsMultipleValues: false);
        AddPropertyMapping("CONTACT", typeof(string), allowsMultipleValues: false);
        AddPropertyMapping("CREATED", typeof(CalDateTime), allowsMultipleValues: false);
        AddPropertyMapping("DTEND", typeof(CalDateTime), allowsMultipleValues: false);
        AddPropertyMapping("DTSTAMP", typeof(CalDateTime), allowsMultipleValues: false);
        AddPropertyMapping("DTSTART", typeof(CalDateTime), allowsMultipleValues: false);
        AddPropertyMapping("DUE", typeof(CalDateTime), allowsMultipleValues: false);
        AddPropertyMapping("DURATION", typeof(TimeSpan), allowsMultipleValues: false);
        AddPropertyMapping("EXDATE", typeof(ExceptionDates), allowsMultipleValues: false);
        AddPropertyMapping("EXRULE", typeof(RecurrencePattern), allowsMultipleValues: false);
        AddPropertyMapping("FREEBUSY", typeof(FreeBusyEntry), allowsMultipleValues: true);
        AddPropertyMapping("GEO", typeof(GeographicLocation), allowsMultipleValues: false);
        AddPropertyMapping("LAST-MODIFIED", typeof(CalDateTime), allowsMultipleValues: false);
        AddPropertyMapping("ORGANIZER", typeof(Organizer), allowsMultipleValues: false);
        AddPropertyMapping("PERCENT-COMPLETE", typeof(int), allowsMultipleValues: false);
        AddPropertyMapping("PRIORITY", typeof(int), allowsMultipleValues: false);
        AddPropertyMapping("RDATE", typeof(RecurrenceDates), allowsMultipleValues: false);
        AddPropertyMapping("RECURRENCE-ID", typeof(CalDateTime), allowsMultipleValues: false);
        AddPropertyMapping("RELATED-TO", typeof(string), allowsMultipleValues: false);
        AddPropertyMapping("REQUEST-STATUS", typeof(RequestStatus), allowsMultipleValues: false);
        AddPropertyMapping("REPEAT", typeof(int), allowsMultipleValues: false);
        AddPropertyMapping("RESOURCES", typeof(string), allowsMultipleValues: true);
        AddPropertyMapping("RRULE", typeof(RecurrencePattern), allowsMultipleValues: false);
        AddPropertyMapping("SEQUENCE", typeof(int), allowsMultipleValues: false);
        AddPropertyMapping("STATUS", ResolveStatusProperty, allowsMultipleValues: false);
        AddPropertyMapping("TRANSP", typeof(TransparencyType), allowsMultipleValues: false);
        AddPropertyMapping("TRIGGER", typeof(Trigger), allowsMultipleValues: false);
        AddPropertyMapping("TZNAME", typeof(string), allowsMultipleValues: false);
        AddPropertyMapping("TZOFFSETFROM", typeof(UtcOffset), allowsMultipleValues: false);
        AddPropertyMapping("TZOFFSETTO", typeof(UtcOffset), allowsMultipleValues: false);
        AddPropertyMapping("TZURL", typeof(Uri), allowsMultipleValues: false);
        AddPropertyMapping("URL", typeof(Uri), allowsMultipleValues: false);
    }

    protected Type ResolveStatusProperty(object context)
    {
        if (context is not ICalendarObject calendarObject)
        {
            return null;
        }

        var parent = calendarObject.Parent;
        return parent is not CalendarEvent
            ? parent is not Todo ? parent is Journal ? typeof(JournalStatus) : null : typeof(TodoStatus)
            : typeof(EventStatus);
    }

    public void AddPropertyMapping(string name, Type objectType, bool allowsMultipleValues)
    {
        if (name != null && !(objectType == null))
        {
            var value = new PropertyMapping
            {
                ObjectType = objectType,
                AllowsMultipleValuesPerProperty = allowsMultipleValues
            };
            _propertyMap[name] = value;
        }
    }

    public void AddPropertyMapping(string name, TypeResolverDelegate resolver, bool allowsMultipleValues)
    {
        if (name != null && resolver != null)
        {
            var value = new PropertyMapping
            {
                Resolver = resolver,
                AllowsMultipleValuesPerProperty = allowsMultipleValues
            };
            _propertyMap[name] = value;
        }
    }

    public void RemovePropertyMapping(string name)
    {
        if (name != null && _propertyMap.ContainsKey(name))
        {
            _propertyMap.Remove(name);
        }
    }

    public virtual bool GetPropertyAllowsMultipleValues(object obj)
    {
        ICalendarProperty calendarProperty = obj as ICalendarProperty;
        return !string.IsNullOrWhiteSpace(calendarProperty?.Name) && _propertyMap.TryGetValue(calendarProperty.Name, out var value)
            ? value.AllowsMultipleValuesPerProperty
            : false;
    }

    public virtual Type GetPropertyMapping(object obj)
    {
        return obj is not ICalendarProperty calendarProperty || calendarProperty.Name == null
            ? null
            : !_propertyMap.TryGetValue(calendarProperty.Name, out var value)
            ? null
            : value.Resolver != null ? value.Resolver(calendarProperty) : value.ObjectType;
    }
}