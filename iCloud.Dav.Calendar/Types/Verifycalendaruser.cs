using System.Xml.Serialization;

namespace iCloud.Dav.Calendar.Types
{
    [XmlRoot(ElementName = "verify-calendar-user", Namespace = "http://me.com/_namespace/")]
    public class Verifycalendaruser
    {
        public static readonly Getctag Default = new Getctag();

        [XmlText]
        public string Value { get; set; }
    }
}
