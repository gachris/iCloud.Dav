using iCloud.Dav.Calendar.DataTypes;
using iCloud.Dav.Calendar.WebDav.DataTypes;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace iCloud.Dav.Calendar.Extensions;

internal static class WebDavExtensions
{
    public const string CollectionResourceType = "collection";
    public const string CalendarResourceType = "calendar";

    public static bool IsCalendar(this Response response)
    {
        if (!response.IsOK()) return false;

        var propStat = response.GetSuccessPropStat();

        var isCalendarResourceType = propStat.Prop.ResourceType?.Names?.Any(resourceType => string.Equals(resourceType, CalendarResourceType, StringComparison.InvariantCultureIgnoreCase)) ?? false;
        return isCalendarResourceType || !response.HasExtension();
    }

    public static bool IsCollection(this Response response)
    {
        if (!response.IsOK()) return false;

        var propStat = response.GetSuccessPropStat();

        var isCollectionResourceType = propStat.Prop.ResourceType?.Names?.Length == 1 && string.Equals(propStat.Prop.ResourceType?.Names.First(), CollectionResourceType, StringComparison.InvariantCultureIgnoreCase);
        return isCollectionResourceType || !response.HasExtension();
    }

    public static bool IsOK(this Response response)
    {
        if (response.HasError())
        {
            return false;
        }

        return response.StatusCode is HttpStatusCode.OK
            || response.PropStat.Length > 0 && response.PropStat[0].StatusCode is HttpStatusCode.OK;
    }

    public static bool HasExtension(this Response response)
    {
        var href = response.Href.Value.TrimEnd('/');
        return Path.HasExtension(href);
    }

    public static bool HasError(this Response response)
    {
        return response.Error?.Errors?.Any() ?? false;
    }

    public static PropStat GetSuccessPropStat(this Response response)
    {
        return response.IsOK() && response.PropStat.Length > 0 ? response.PropStat[0] : null;
    }

    public static string ExtractId(this Href href)
    {
        var value = href.Value.TrimEnd('/');
        return Path.GetFileNameWithoutExtension(value);
    }

    public static CalendarListEntry ToCalendar(this Response response)
    {
        var calendarListEntry = new CalendarListEntry()
        {
            Id = response.Href.ExtractId(),
            Deleted = response.StatusCode == HttpStatusCode.NotFound ? true : (bool?)null
        };

        if (calendarListEntry.Deleted == true)
            return calendarListEntry;

        var propStat = response.GetSuccessPropStat();
        int.TryParse(propStat.Prop.CalendarOrder?.Value, out var order);

        calendarListEntry.ETag = propStat.Prop.GetETag?.Value;
        calendarListEntry.CTag = propStat.Prop.GetCTag?.Value;
        calendarListEntry.Color = propStat.Prop.CalendarColor?.Value.ToRgb();
        calendarListEntry.Summary = propStat.Prop.DisplayName?.Value;
        calendarListEntry.Description = propStat.Prop.CalendarDescription?.Value;
        calendarListEntry.Order = order;
        calendarListEntry.TimeZone = propStat.Prop.CalendarTimezone?.Value.ToVTimeZone();
        calendarListEntry.Privileges.AddRange(propStat.Prop.CurrentUserPrivilegeSet?.Privilege?.Select(privilege => privilege.Name) ?? Array.Empty<string>());
        calendarListEntry.SupportedReports.AddRange(propStat.Prop.SupportedReportSet?.SupportedReport?.Select(supportedReport => supportedReport.Report.Name) ?? Array.Empty<string>());
        calendarListEntry.SupportedCalendarComponents.AddRange(propStat.Prop.SupportedCalendarComponentSet?.Comp?.Select(comp => comp.Name) ?? Array.Empty<string>());

        return calendarListEntry;
    }

    public static string FromRgb(this string color)
    {
        if (color.Length == 7 && color[0] == '#')
        {
            return string.Concat(color, "FF");
        }
        else
        {
            return color;
        }
    }

    public static string ToRgb(this string color)
    {
        if (color.Length == 9 && color[0] == '#')
        {
            return color.Substring(0, 7);
        }
        else
        {
            return color;
        }
    }

    public static HttpStatusCode? ToHttpStatusCode(this string status)
    {
        if (string.IsNullOrWhiteSpace(status)) return null;

        var statusParts = status.Split(' ');
        var statusCodeString = statusParts.Length > 0 ? (int?)Convert.ToInt16(statusParts[1]) : null;
        return (HttpStatusCode?)statusCodeString;
    }
}