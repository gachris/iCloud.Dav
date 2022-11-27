﻿using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Calendar.CalDav.Types
{
    [XmlRoot(ElementName = "calendar-multiget", Namespace = "urn:ietf:params:xml:ns:caldav")]
    internal sealed class CalendarMultiget : IXmlSerializable
    {
        public List<string> Href { get; }

        public CalendarMultiget()
        {
            Href = new List<string>();
        }

        public XmlSchema GetSchema() => new XmlSchema();

        public void ReadXml(XmlReader reader) => throw new NotSupportedException();

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("prop", "DAV:");
            writer.WriteElementString("add-member", "DAV:", null);
            writer.WriteElementString("allowed-sharing-modes", "http://calendarserver.org/ns/", null);
            writer.WriteElementString("autoprovisioned", "http://apple.com/ns/ical/", null);
            writer.WriteElementString("bulk-requests", "http://me.com/_namespace/", null);
            writer.WriteElementString("calendar-color", "http://apple.com/ns/ical/", null);
            writer.WriteElementString("calendar-description", "urn:ietf:params:xml:ns:caldav", null);
            writer.WriteElementString("calendar-free-busy-set", "urn:ietf:params:xml:ns:caldav", null);
            writer.WriteElementString("calendar-order", "http://apple.com/ns/ical/", null);
            writer.WriteElementString("calendar-timezone", "urn:ietf:params:xml:ns:caldav", null);
            writer.WriteElementString("current-user-privilege-set", "DAV:", null);
            writer.WriteElementString("default-alarm-vevent-date", "urn:ietf:params:xml:ns:caldav", null);
            writer.WriteElementString("default-alarm-vevent-datetime", "urn:ietf:params:xml:ns:caldav", null);
            writer.WriteElementString("displayname", "DAV:", null);
            writer.WriteElementString("getctag", "http://calendarserver.org/ns/", null);
            writer.WriteElementString("language-code", "http://apple.com/ns/ical/", null);
            writer.WriteElementString("location-code", "http://apple.com/ns/ical/", null);
            writer.WriteElementString("owner", "DAV:", null);
            writer.WriteElementString("pre-publish-url", "http://calendarserver.org/ns/", null);
            writer.WriteElementString("publish-url", "http://calendarserver.org/ns/", null);
            writer.WriteElementString("push-transports", "http://calendarserver.org/ns/", null);
            writer.WriteElementString("pushkey", "http://calendarserver.org/ns/", null);
            writer.WriteElementString("quota-available-bytes", "DAV:", null);
            writer.WriteElementString("quota-used-bytes", "DAV:", null);
            writer.WriteElementString("refreshrate", "http://calendarserver.org/ns/", null);
            writer.WriteElementString("resource-id", "DAV:", null);
            writer.WriteElementString("resourcetype", "DAV:", null);
            writer.WriteElementString("schedule-calendar-transp", "urn:ietf:params:xml:ns:caldav", null);
            writer.WriteElementString("schedule-default-calendar-URL", "urn:ietf:params:xml:ns:caldav", null);
            writer.WriteElementString("source", "http://calendarserver.org/ns/", null);
            writer.WriteElementString("subscribed-strip-alarms", "http://calendarserver.org/ns/", null);
            writer.WriteElementString("subscribed-strip-attachments", "http://calendarserver.org/ns/", null);
            writer.WriteElementString("subscribed-strip-todos", "http://calendarserver.org/ns/", null);
            writer.WriteElementString("supported-calendar-component-set", "urn:ietf:params:xml:ns:caldav", null);
            writer.WriteElementString("supported-calendar-component-sets", "urn:ietf:params:xml:ns:caldav", null);
            writer.WriteElementString("supported-report-set", "DAV:", null);
            writer.WriteElementString("sync-token", "DAV:", null);
            writer.WriteElementString("getetag", "DAV:", null);
            writer.WriteElementString("calendar-data", "urn:ietf:params:xml:ns:caldav", null);
            writer.WriteEndElement();

            Href.ForEach(href => writer.WriteElementString("href", "DAV:", href));
        }
    }
}