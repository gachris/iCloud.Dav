using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Core.WebDav.Cal
{
    /// <summary>
    /// Represents the properties associated with a response.
    /// </summary>
    internal class Prop : IXmlSerializable
    {
        #region Properties

        /// <summary>
        /// Gets the URL of the principal associated with the resource.
        /// </summary>
        public CurrentUserPrincipal CurrentUserPrincipal { get; set; }

        /// <summary>
        /// Gets or sets the display name associated with the property.
        /// </summary>
        public DisplayName DisplayName { get; set; }

        /// <summary>
        /// Gets the URL of the calendar home collection for the principal.
        /// </summary>
        public CalendarHomeSet CalendarHomeSet { get; set; }

        /// <summary>
        /// Gets the set of calendar user addresses associated with the resource.
        /// </summary>
        public CalendarUserAddressSet CalendarUserAddressSet { get; set; }

        /// <summary>
        /// Gets or sets the calendar color associated with the property.
        /// </summary>
        /// <remarks>
        /// This element is in the http://apple.com/ns/ical/ namespace.
        /// </remarks>
        public CalendarColor CalendarColor { get; set; }

        /// <summary>
        /// Gets or sets the calendar order associated with the property.
        /// </summary>
        /// <remarks>
        /// This element is in the http://apple.com/ns/ical/ namespace.
        /// </remarks>
        public CalendarOrder CalendarOrder { get; set; }

        /// <summary>
        /// Gets or sets the calendar timezone associated with the property.
        /// </summary>
        /// <remarks>
        /// This element is in the urn:ietf:params:xml:ns:caldav namespace.
        /// </remarks>
        public CalendarTimezone CalendarTimezone { get; set; }

        /// <summary>
        /// Gets or sets the calendar description associated with the property.
        /// </summary>
        /// <remarks>
        /// This element is in the urn:ietf:params:xml:ns:caldav namespace.
        /// </remarks>
        public CalendarDescription CalendarDescription { get; set; }

        /// <summary>
        /// Gets or sets the sync token associated with the property.
        /// </summary>
        public SyncToken SyncToken { get; set; }

        /// <summary>
        /// Gets or sets the value of the "getctag" property.
        /// </summary>
        /// <remarks>
        /// This element is in the http://calendarserver.org/ns/ namespace.
        /// </remarks>
        public GetCTag GetCTag { get; set; }

        /// <summary>
        /// Gets or sets the value of the "getetag" property.
        /// </summary>
        public GetETag GetETag { get; set; }

        /// <summary>
        /// Gets or sets the address data associated with the property.
        /// </summary>
        /// <remarks>
        /// This element is in the urn:ietf:params:xml:ns:caldav namespace.
        /// </remarks>
        public CalendarData CalendarData { get; set; }

        /// <summary>
        /// Gets or sets the ResourceType representing object the resource types associated with the property.
        /// </summary>
        public ResourceType ResourceType { get; set; }

        /// <summary>
        /// Gets or sets the CurrentUserPrivilegeSet object representing the current user privilege set associated with the property.
        /// </summary>
        public CurrentUserPrivilegeSet CurrentUserPrivilegeSet { get; set; }

        /// <summary>
        /// Gets or sets the SupportedReportSet object representing the supported report set associated with the property.
        /// </summary>
        public SupportedReportSet SupportedReportSet { get; set; }

        /// <summary>
        /// Gets or sets the SupportedCalendarComponentSet object representing the supported calendar component set associated with the property.
        /// </summary>
        public SupportedCalendarComponentSet SupportedCalendarComponentSet { get; set; }

        /// <summary>
        /// Gets or sets the owner associated with the property.
        /// </summary>
        public Owner Owner { get; set; }

        /// <summary>
        /// Gets or sets the pre-publish URL associated with the property.
        /// </summary>
        public PrePublishUrl PrePublishUrl { get; set; }

        /// <summary>
        /// Gets or sets the push key associated with the property.
        /// </summary>
        public PushKey PushKey { get; set; }

        /// <summary>
        /// Gets or sets the available quota bytes associated with the property.
        /// </summary>
        public QuotaAvailableBytes QuotaAvailableBytes { get; set; }

        /// <summary>
        /// Gets or sets the member to be added associated with the property.
        /// </summary>
        public AddMember AddMember { get; set; }

        /// <summary>
        /// Gets or sets the allowed sharing modes associated with the property.
        /// </summary>
        public AllowedSharingModes AllowedSharingModes { get; set; }

        /// <summary>
        /// Gets or sets whether the resource is autoprovisioned.
        /// </summary>
        public Autoprovisioned Autoprovisioned { get; set; }

        /// <summary>
        /// Gets or sets the bulk requests associated with the property.
        /// </summary>
        public BulkRequests BulkRequests { get; set; }

        /// <summary>
        /// Gets or sets the free-busy set of calendars associated with the property.
        /// </summary>
        public CalendarFreeBusySet CalendarFreeBusySet { get; set; }

        /// <summary>
        /// Gets or sets the default alarm for VEVENT with date associated with the property.
        /// </summary>
        public DefaultAlarmVEventDate DefaultAlarmVEventDate { get; set; }

        /// <summary>
        /// Gets or sets the default alarm for VEVENT with datetime associated with the property.
        /// </summary>
        public DefaultAlarmVEventDatetime DefaultAlarmVEventDatetime { get; set; }

        /// <summary>
        /// Gets or sets the language code associated with the property.
        /// </summary>
        public LanguageCode LanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the location code associated with the property.
        /// </summary>
        public LocationCode LocationCode { get; set; }

        /// <summary>
        /// Gets or sets the publish URL associated with the property.
        /// </summary>
        public PublishUrl PublishUrl { get; set; }

        /// <summary>
        /// Gets or sets the push transports associated with the property.
        /// </summary>
        public PushTransports PushTransports { get; set; }

        /// <summary>
        /// Gets or sets the used quota bytes associated with the property.
        /// </summary>
        public QuotaUsedBytes QuotaUsedBytes { get; set; }

        /// <summary>
        /// Gets or sets the refresh rate associated with the property.
        /// </summary>
        public RefreshRate RefreshRate { get; set; }

        /// <summary>
        /// Gets or sets the resource ID associated with the property.
        /// </summary>
        public ResourceId ResourceId { get; set; }

        /// <summary>
        /// Gets or sets the schedule calendar transparency associated with the property.
        /// </summary>
        public ScheduleCalendarTransp ScheduleCalendarTransp { get; set; }

        /// <summary>
        /// Gets or sets the schedule default calendar URL associated with the property.
        /// </summary>
        public ScheduleDefaultCalendarURL ScheduleDefaultCalendarURL { get; set; }

        /// <summary>
        /// Gets or sets the source associated with the property.
        /// </summary>
        public Source Source { get; set; }

        /// <summary>
        /// Gets or sets the subscribed strip alarms associated with the property.
        /// </summary>
        public SubscribedStripAlarms SubscribedStripAlarms { get; set; }

        /// <summary>
        /// Gets or sets the subscribed strip attachments associated with the property.
        /// </summary>
        public SubscribedStripAttachments SubscribedStripAttachments { get; set; }

        /// <summary>
        /// Gets or sets the subscribed strip todos associated with the property.
        /// </summary>
        public SubscribedStripTodos SubscribedStripTodos { get; set; }

        /// <summary>
        /// Gets or sets the supported calendar component sets associated with the property.
        /// </summary>
        public SupportedCalendarComponentSets SupportedCalendarComponentSets { get; set; }

        #endregion

        #region IXmlSerializable Implementation

        /// <summary>
        /// Gets the XML schema for this object.
        /// </summary>
        /// <returns>An <see cref="XmlSchema"/> object.</returns>
        public XmlSchema GetSchema() => new XmlSchema();

        /// <summary>
        /// Reads the XML representation of the multi-status response.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> object to read from.</param>
        public void ReadXml(XmlReader reader)
        {
            if (!reader.IsStartElement("prop")) return;

            while (reader.Read())
            {
                if (reader.IsStartElement("displayname") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    DisplayName = new DisplayName();
                    DisplayName.ReadXml(reader);
                }
                else if (reader.IsStartElement("calendar-color", "http://apple.com/ns/ical/") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    CalendarColor = new CalendarColor();
                    CalendarColor.ReadXml(reader);
                }
                else if (reader.IsStartElement("calendar-order", "http://apple.com/ns/ical/") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    CalendarOrder = new CalendarOrder();
                    CalendarOrder.ReadXml(reader);
                }
                else if (reader.IsStartElement("calendar-timezone", "urn:ietf:params:xml:ns:caldav") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    CalendarTimezone = new CalendarTimezone();
                    CalendarTimezone.ReadXml(reader);
                }
                else if (reader.IsStartElement("calendar-description", "urn:ietf:params:xml:ns:caldav") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    CalendarDescription = new CalendarDescription();
                    CalendarDescription.ReadXml(reader);
                }
                else if (reader.IsStartElement("sync-token") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    SyncToken = new SyncToken();
                    SyncToken.ReadXml(reader);
                }
                else if (reader.IsStartElement("getctag", "http://calendarserver.org/ns/") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    GetCTag = new GetCTag();
                    GetCTag.ReadXml(reader);
                }
                else if (reader.IsStartElement("getetag") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    GetETag = new GetETag();
                    GetETag.ReadXml(reader);
                }
                else if (reader.IsStartElement("calendar-data", "urn:ietf:params:xml:ns:caldav") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    CalendarData = new CalendarData();
                    CalendarData.ReadXml(reader);
                }
                else if (reader.IsStartElement("resourcetype") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    ResourceType = new ResourceType();
                    ResourceType.ReadXml(reader);
                }
                else if (reader.IsStartElement("current-user-privilege-set") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    CurrentUserPrivilegeSet = new CurrentUserPrivilegeSet();
                    CurrentUserPrivilegeSet.ReadXml(reader);
                }
                else if (reader.IsStartElement("supported-report-set") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    SupportedReportSet = new SupportedReportSet();
                    SupportedReportSet.ReadXml(reader);
                }
                else if (reader.IsStartElement("supported-calendar-component-set", "urn:ietf:params:xml:ns:caldav") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    SupportedCalendarComponentSet = new SupportedCalendarComponentSet();
                    SupportedCalendarComponentSet.ReadXml(reader);
                }
                else if (reader.IsStartElement("calendar-home-set", "urn:ietf:params:xml:ns:caldav") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    CalendarHomeSet = new CalendarHomeSet();
                    CalendarHomeSet.ReadXml(reader);
                }
                else if (reader.IsStartElement("calendar-user-address-set", "urn:ietf:params:xml:ns:caldav") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    CalendarUserAddressSet = new CalendarUserAddressSet();
                    CalendarUserAddressSet.ReadXml(reader);
                }
                else if (reader.IsStartElement("current-user-principal") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    CurrentUserPrincipal = new CurrentUserPrincipal();
                    CurrentUserPrincipal.ReadXml(reader);
                }
                else if (reader.IsStartElement("add-member") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    AddMember = new AddMember();
                    AddMember.ReadXml(reader);
                }
                else if (reader.IsStartElement("allowed-sharing-modes", "http://calendarserver.org/ns/") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    AllowedSharingModes = new AllowedSharingModes();
                    AllowedSharingModes.ReadXml(reader);
                }
                else if (reader.IsStartElement("autoprovisioned", "http://apple.com/ns/ical/") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    Autoprovisioned = new Autoprovisioned();
                    Autoprovisioned.ReadXml(reader);
                }
                else if (reader.IsStartElement("bulk-requests", "http://me.com/_namespace/") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    BulkRequests = new BulkRequests();
                    BulkRequests.ReadXml(reader);
                }
                else if (reader.IsStartElement("calendar-free-busy-set", "urn:ietf:params:xml:ns:caldav") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    CalendarFreeBusySet = new CalendarFreeBusySet();
                    CalendarFreeBusySet.ReadXml(reader);
                }
                else if (reader.IsStartElement("default-alarm-vevent-date", "urn:ietf:params:xml:ns:caldav") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    DefaultAlarmVEventDate = new DefaultAlarmVEventDate();
                    DefaultAlarmVEventDate.ReadXml(reader);
                }
                else if (reader.IsStartElement("default-alarm-vevent-datetime", "urn:ietf:params:xml:ns:caldav") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    DefaultAlarmVEventDatetime = new DefaultAlarmVEventDatetime();
                    DefaultAlarmVEventDatetime.ReadXml(reader);
                }
                else if (reader.IsStartElement("quota-used-bytes") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    QuotaUsedBytes = new QuotaUsedBytes();
                    QuotaUsedBytes.ReadXml(reader);
                }
                else if (reader.IsStartElement("publish-url", "http://calendarserver.org/ns/") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    PublishUrl = new PublishUrl();
                    PublishUrl.ReadXml(reader);
                }
                else if (reader.IsStartElement("push-transports", "http://calendarserver.org/ns/") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    PushTransports = new PushTransports();
                    PushTransports.ReadXml(reader);
                }
                else if (reader.IsStartElement("refreshrate", "http://calendarserver.org/ns/") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    RefreshRate = new RefreshRate();
                    RefreshRate.ReadXml(reader);
                }
                else if (reader.IsStartElement("resource-id", "DAV:") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    ResourceId = new ResourceId();
                    ResourceId.ReadXml(reader);
                }
                else if (reader.IsStartElement("schedule-calendar-transp", "urn:ietf:params:xml:ns:caldav") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    ScheduleCalendarTransp = new ScheduleCalendarTransp();
                    ScheduleCalendarTransp.ReadXml(reader);
                }
                else if (reader.IsStartElement("schedule-default-calendar-URL", "urn:ietf:params:xml:ns:caldav") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    ScheduleDefaultCalendarURL = new ScheduleDefaultCalendarURL();
                    ScheduleDefaultCalendarURL.ReadXml(reader);
                }
                else if (reader.IsStartElement("source", "http://calendarserver.org/ns/") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    Source = new Source();
                    Source.ReadXml(reader);
                }
                else if (reader.IsStartElement("subscribed-strip-alarms", "http://calendarserver.org/ns/") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    SubscribedStripAlarms = new SubscribedStripAlarms();
                    SubscribedStripAlarms.ReadXml(reader);
                }
                else if (reader.IsStartElement("subscribed-strip-attachments", "http://calendarserver.org/ns/") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    SubscribedStripAttachments = new SubscribedStripAttachments();
                    SubscribedStripAttachments.ReadXml(reader);
                }
                else if (reader.IsStartElement("subscribed-strip-todos", "http://calendarserver.org/ns/") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    SubscribedStripTodos = new SubscribedStripTodos();
                    SubscribedStripTodos.ReadXml(reader);
                }
                else if (reader.IsStartElement("supported-calendar-component-sets", "urn:ietf:params:xml:ns:caldav") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    SupportedCalendarComponentSets = new SupportedCalendarComponentSets();
                    SupportedCalendarComponentSets.ReadXml(reader);
                }
                else if (reader.IsStartElement("language-code", "http://apple.com/ns/ical/") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    LanguageCode = new LanguageCode();
                    LanguageCode.ReadXml(reader);
                }
                else if (reader.IsStartElement("location-code", "http://apple.com/ns/ical/") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    LocationCode = new LocationCode();
                    LocationCode.ReadXml(reader);
                }
                else if (reader.IsStartElement("quota-available-bytes", "DAV:") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    QuotaAvailableBytes = new QuotaAvailableBytes();
                    QuotaAvailableBytes.ReadXml(reader);
                }
                else if (reader.IsStartElement("pushkey", "http://calendarserver.org/ns/") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    PushKey = new PushKey();
                    PushKey.ReadXml(reader);
                }
                else if (reader.IsStartElement("pre-publish-url", "http://calendarserver.org/ns/") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    PrePublishUrl = new PrePublishUrl();
                    PrePublishUrl.ReadXml(reader);
                }
                else if (reader.IsStartElement("owner", "DAV:") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    Owner = new Owner();
                    Owner.ReadXml(reader);
                }
                else if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "prop" && reader.Depth == 3)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Writes the XML representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> object to write to.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("prop", "DAV:");

            CurrentUserPrivilegeSet?.WriteXml(writer);
            AddMember?.WriteXml(writer);
            AllowedSharingModes?.WriteXml(writer);
            Autoprovisioned?.WriteXml(writer);
            BulkRequests?.WriteXml(writer);
            CalendarFreeBusySet?.WriteXml(writer);
            DefaultAlarmVEventDate?.WriteXml(writer);
            DefaultAlarmVEventDatetime?.WriteXml(writer);
            LanguageCode?.WriteXml(writer);
            LocationCode?.WriteXml(writer);
            PublishUrl?.WriteXml(writer);
            PushTransports?.WriteXml(writer);
            QuotaUsedBytes?.WriteXml(writer);
            RefreshRate?.WriteXml(writer);
            ResourceId?.WriteXml(writer);
            ScheduleCalendarTransp?.WriteXml(writer);
            ScheduleDefaultCalendarURL?.WriteXml(writer);
            Source?.WriteXml(writer);
            SubscribedStripAlarms?.WriteXml(writer);
            SubscribedStripAttachments?.WriteXml(writer);
            SubscribedStripTodos?.WriteXml(writer);
            SupportedCalendarComponentSets?.WriteXml(writer);
            SupportedReportSet?.WriteXml(writer);
            Owner?.WriteXml(writer);
            PrePublishUrl?.WriteXml(writer);
            PushKey?.WriteXml(writer);
            QuotaAvailableBytes?.WriteXml(writer);
            GetETag?.WriteXml(writer);
            GetCTag?.WriteXml(writer);
            SyncToken?.WriteXml(writer);
            CalendarData?.WriteXml(writer);
            CalendarDescription?.WriteXml(writer);
            ResourceType?.WriteXml(writer);
            DisplayName?.WriteXml(writer);
            CalendarColor?.WriteXml(writer);
            CalendarOrder?.WriteXml(writer);
            CalendarTimezone?.WriteXml(writer);
            SupportedCalendarComponentSet?.WriteXml(writer);
            CurrentUserPrincipal?.WriteXml(writer);
            CalendarHomeSet?.WriteXml(writer);
            CalendarUserAddressSet?.WriteXml(writer);

            writer.WriteEndElement();
        }

        #endregion
    }
}