using iCloud.Dav.People.DataTypes;
using System;
using System.IO;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;

namespace iCloud.Dav.People.Serialization.DataTypes
{
    /// <summary>
    /// Serializes and deserializes a <see cref="InstantMessage"/> object to and from a string representation, according to the vCard specification.
    /// </summary>
    public class InstantMessageSerializer : EncodableDataTypeSerializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InstantMessageSerializer"/> class.
        /// </summary>
        public InstantMessageSerializer() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstantMessageSerializer"/> class with the given <see cref="SerializationContext"/>.
        /// </summary>
        /// <param name="ctx">The <see cref="SerializationContext"/> to use.</param>
        public InstantMessageSerializer(SerializationContext ctx) : base(ctx)
        {
        }

        /// <summary>
        /// Gets the Type that this <see cref="InstantMessageSerializer"/> can serialize and deserialize, which is <see cref="InstantMessage"/>.
        /// </summary>
        public override Type TargetType => typeof(InstantMessage);

        /// <summary>
        /// Converts a <see cref="InstantMessage"/> object to a string representation.
        /// </summary>
        /// <param name="obj">The <see cref="InstantMessage"/> object to be serialized.</param>
        /// <returns>A string representation of the <see cref="InstantMessage"/> object.</returns>
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

        /// <summary>
        /// Converts a string representation of a <see cref="InstantMessage"/> object to a <see cref="InstantMessage"/> object.
        /// </summary>
        /// <param name="value">The string representation of the <see cref="InstantMessage"/> object to be deserialized.</param>
        /// <returns>A <see cref="InstantMessage"/> object.</returns>
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

        /// <summary>
        /// This method deserializes a <see cref="InstantMessage"/> object from the given <see cref="TextReader"/>.
        /// </summary>
        /// <param name="tr">The <see cref="TextReader"/> to deserialize the <see cref="InstantMessage"/> object from.</param>
        /// <returns>A <see cref="InstantMessage"/> object.</returns>
        public override object Deserialize(TextReader tr) => Deserialize(tr.ReadToEnd());
    }
}