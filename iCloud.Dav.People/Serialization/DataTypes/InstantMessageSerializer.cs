using iCloud.Dav.People.DataTypes;
using System;
using System.IO;
using vCard.Net.Serialization;

namespace iCloud.Dav.People.Serialization.DataTypes
{
    public class InstantMessageSerializer : vCard.Net.Serialization.DataTypes.EncodableDataTypeSerializer
    {
        public InstantMessageSerializer() : base()
        {
        }

        public InstantMessageSerializer(SerializationContext ctx) : base(ctx)
        {
        }

        public override Type TargetType => typeof(InstantMessage);

        public override string SerializeToString(object obj)
        {
            if (!(obj is InstantMessage instantMessage))
            {
                return null;
            }

            var value = instantMessage.UserName;

            switch (instantMessage.ServiceType)
            {
                case InstantMessageServiceType.AIM:
                case InstantMessageServiceType.ICQ:
                    value = value.Insert(0, "aim:");
                    break;
                case InstantMessageServiceType.Facebook:
                case InstantMessageServiceType.GoogleTalk:
                case InstantMessageServiceType.Jabber:
                    value = value.Insert(0, "xmpp:");
                    break;
                case InstantMessageServiceType.QQ:
                case InstantMessageServiceType.GaduGadu:
                    value = value.Insert(0, "x-apple:");
                    break;
                case InstantMessageServiceType.MSN:
                    value = value.Insert(0, "msnim:");
                    break;
                case InstantMessageServiceType.Skype:
                    value = value.Insert(0, "skype:");
                    break;
                case InstantMessageServiceType.Yahoo:
                    value = value.Insert(0, "ymsgr:");
                    break;
                default:
                    break;
            }

            return Encode(instantMessage, value);
        }

        public InstantMessage Deserialize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (!(CreateAndAssociate() is InstantMessage instantMessage))
            {
                return null;
            }

            // Decode the value, if necessary!
            value = Decode(instantMessage, value);

            if (value is null)
            {
                return null;
            }

            switch (instantMessage.ServiceType)
            {
                case InstantMessageServiceType.AIM:
                case InstantMessageServiceType.ICQ:
                    value = value.Replace("aim:", string.Empty);
                    break;
                case InstantMessageServiceType.Facebook:
                case InstantMessageServiceType.GoogleTalk:
                case InstantMessageServiceType.Jabber:
                    value = value.Replace("xmpp:", string.Empty);
                    break;
                case InstantMessageServiceType.QQ:
                case InstantMessageServiceType.GaduGadu:
                    value = value.Replace("x-apple:", string.Empty);
                    break;
                case InstantMessageServiceType.MSN:
                    value = value.Replace("msnim:", string.Empty);
                    break;
                case InstantMessageServiceType.Skype:
                    value = value.Replace("skype:", string.Empty);
                    break;
                case InstantMessageServiceType.Yahoo:
                    value = value.Replace("ymsgr:", string.Empty);
                    break;
                default:
                    break;
            }

            instantMessage.UserName = value;

            return instantMessage;
        }

        public override object Deserialize(TextReader tr) => Deserialize(tr.ReadToEnd());
    }
}