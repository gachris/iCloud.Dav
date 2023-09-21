using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Core.WebDav.Card
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
        /// Gets the URL of the address book home collection for the principal.
        /// </summary>
        public AddressbookHomeSet AddressbookHomeSet { get; set; }

        /// <summary>
        /// Gets or sets the value of the "getcontentlength" property.
        /// </summary>
        public GetContentLength GetContentLength { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the property.
        /// </summary>
        public CreationDate CreationDate { get; set; }

        /// <summary>
        /// Gets or sets the last modified date of the property.
        /// </summary>
        public GetLastModified GetLastModified { get; set; }

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
        public AddressData AddressData { get; set; }

        /// <summary>
        /// Gets or sets the MeCard associated with the property.
        /// </summary>
        /// <remarks>
        /// This element is in the http://calendarserver.org/ns/ namespace.
        /// </remarks>
        public MeCard MeCard { get; set; }

        /// <summary>
        /// Gets or sets the ResourceType representing the resource types associated with the property.
        /// </summary>
        public ResourceType ResourceType { get; set; }

        /// <summary>
        /// Gets or sets the GuardianRestricted associated with the property.
        /// </summary>
        public GuardianRestricted GuardianRestricted { get; set; }

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
                else if (reader.IsStartElement("me-card", "http://calendarserver.org/ns/"))
                {
                    MeCard = new MeCard();
                    MeCard.ReadXml(reader);
                }
                else if (reader.IsStartElement("getcontentlength"))
                {
                    GetContentLength = new GetContentLength();
                    GetContentLength.ReadXml(reader);
                }
                else if (reader.IsStartElement("resourcetype"))
                {
                    ResourceType = new ResourceType();
                    ResourceType.ReadXml(reader);
                }
                else if (reader.IsStartElement("creationdate"))
                {
                    CreationDate = new CreationDate();
                    CreationDate.ReadXml(reader);
                }
                else if (reader.IsStartElement("getlastmodified"))
                {
                    GetLastModified = new GetLastModified();
                    GetLastModified.ReadXml(reader);
                }
                else if (reader.IsStartElement("sync-token"))
                {
                    SyncToken = new SyncToken();
                    SyncToken.ReadXml(reader);
                }
                else if (reader.IsStartElement("getctag", "http://calendarserver.org/ns/"))
                {
                    GetCTag = new GetCTag();
                    GetCTag.ReadXml(reader);
                }
                else if (reader.IsStartElement("getetag"))
                {
                    GetETag = new GetETag();
                    GetETag.ReadXml(reader);
                }
                else if (reader.IsStartElement("address-data", "urn:ietf:params:xml:ns:carddav"))
                {
                    AddressData = new AddressData();
                    AddressData.ReadXml(reader);
                }
                else if (reader.IsStartElement("guardian-restricted"))
                {
                    GuardianRestricted = new GuardianRestricted();
                    GuardianRestricted.ReadXml(reader);
                }
                else if (reader.IsStartElement("addressbook-home-set", "urn:ietf:params:xml:ns:carddav") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    AddressbookHomeSet = new AddressbookHomeSet();
                    AddressbookHomeSet.ReadXml(reader);
                }
                else if (reader.IsStartElement("current-user-principal") && reader.Depth == 4)
                {
                    if (reader.IsEmptyElement) continue;

                    CurrentUserPrincipal = new CurrentUserPrincipal();
                    CurrentUserPrincipal.ReadXml(reader);
                }
                else if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "prop")
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Writes the XML representation of the property update.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> object to write to.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("prop", "DAV:");

            if (MeCard is null
                && SyncToken is null
                && GetETag is null
                && CurrentUserPrincipal is null
                && DisplayName is null
                && AddressbookHomeSet is null)
            {
                writer.WriteElementString("allprop", "DAV:", null);
            }
            else
            {
                DisplayName?.WriteXml(writer);
                AddressbookHomeSet?.WriteXml(writer);
                CurrentUserPrincipal?.WriteXml(writer);
                GetETag?.WriteXml(writer);
                MeCard?.WriteXml(writer);
                SyncToken?.WriteXml(writer);
            }

            writer.WriteEndElement();
        }

        #endregion
    }
}