namespace iCloud.vCard.Net;

internal static class Constants
{
    public class Card
    {
        public const string vCard_Type = "VCARD";
        public const string vCard_Version = "3.0";

        public const string BEGIN = "BEGIN";
        public const string FN = "FN";
        public const string PRODID = "PRODID";
        public const string REV = "REV";
        public const string UID = "UID";
        public const string VERSION = "VERSION";
        public const string END = "END";
    }

    public class ContactGroup
    {
        public const string GroupKind = "group";
        public const string urn_Prefix = "urn:uuid:";

        public const string N = "N";
        public const string X_ADDRESSBOOKSERVER_KIND = "X-ADDRESSBOOKSERVER-KIND";
        public const string X_ADDRESSBOOKSERVER_MEMBER = "X-ADDRESSBOOKSERVER-MEMBER";
    }

    public class Contact
    {
        public const string N = "N";
        public const string NICKNAME = "NICKNAME";
        public const string TITLE = "TITLE";
        public const string ORG = "ORG";
        public const string NOTE = "NOTE";
        public const string BDAY = "BDAY";
        public const string X_PHONETIC_FIRST_NAME = "X-PHONETIC-FIRST-NAME";
        public const string X_PHONETIC_LAST_NAME = "X-PHONETIC-LAST-NAME";
        public const string X_PHONETIC_ORG = "X-PHONETIC-ORG";
        public const string X_ABShowAs = "X-ABShowAs";

        public static class Phone
        {
            public static class Property
            {
                public const string TEL = "TEL";
                public const string TYPE = "TYPE";
                public const string X_ABLABEL = "X-ABLABEL";
            }

            public static class CustomType
            {
                public const string AppleWatch = "APPLE WATCH";
                public const string School = "_$!<School>!$_";
            }
        }

        public static class Photo
        {
            public static class Property
            {
                public const string PHOTO = "PHOTO";
            }

            public static class Subproperty
            {
                public const string VALUE = "VALUE";
                public const string X_ABCROP_RECTANGLE = "X-ABCROP-RECTANGLE";

                public static class Value
                {
                    public const string URI = "URI";
                    public const string ABClipRect = "ABClipRect_1";
                    public const string ABClipRectFormat = "ABClipRect_1&{0}&{1}&{2}&{3}&==";
                }
            }
        }

        public static class EmailAddress
        {
            public static class Property
            {
                public const string EMAIL = "EMAIL";
                public const string TYPE = "TYPE";
                public const string X_ABLABEL = "X-ABLABEL";
            }

            public static class CustomType
            {
                public const string School = "_$!<School>!$_";
            }
        }

        public static class Website
        {
            public static class Property
            {
                public const string URL = "URL";
                public const string TYPE = "TYPE";
                public const string X_ABLABEL = "X-ABLABEL";
            }

            public static class CustomType
            {
                public const string HomePage = "_$!<HomePage>!$_";
                public const string School = "_$!<School>!$_";
                public const string Blog = "BLOG";
            }
        }

        public static class Profile
        {
            public const string CustomProfilePrefix = "x-apple:";

            public static class Property
            {
                public const string URL = "URL";
                public const string TYPE = "TYPE";
                public const string X_ABLABEL = "X-ABLABEL";
                public const string X_SOCIALPROFILE = "X-SOCIALPROFILE";
                public const string X_USER = "X-USER";
            }
        }

        public static class Address
        {
            public static class Property
            {
                public const string ADR = "ADR";
                public const string TYPE = "TYPE";
                public const string X_ABLABEL = "X-ABLABEL";
                public const string X_ABADR = "X-ABADR";
            }

            public static class CustomType
            {
                public const string School = "_$!<School>!$_";
            }
        }

        public static class Date
        {
            public static class Property
            {
                public const string X_ABDATE = "X-ABDATE";
                public const string TYPE = "TYPE";
                public const string X_ABLABEL = "X-ABLABEL";
            }

            public static class CustomType
            {
                public const string Anniversary = "_$!<Anniversary>!$_";
            }
        }

        public static class RelatedPerson
        {
            public static class Property
            {
                public const string X_ABRELATEDNAMES = "X-ABRELATEDNAMES";
                public const string TYPE = "TYPE";
                public const string X_ABLABEL = "X-ABLABEL";
            }

            public static class CustomType
            {
                public const string Father = "_$!<Father>!$_";
                public const string Mother = "_$!<Mother>!$_";
                public const string Parent = "_$!<Parent>!$_";
                public const string Brother = "_$!<Brother>!$_";
                public const string Sister = "_$!<Sister>!$_";
                public const string Child = "_$!<Child>!$_";
                public const string Friend = "_$!<Friend>!$_";
                public const string Spouse = "_$!<Spouse>!$_";
                public const string Partner = "_$!<Partner>!$_";
                public const string Assistant = "_$!<Assistant>!$_";
                public const string Manager = "_$!<Manager>!$_";
            }
        }
    }
}