using Ical.Net.Interfaces.Serialization;
using System;
using System.Text;

namespace iCloud.Dav.Calendar.Utils
{
    public class EncodingProvider : IEncodingProvider
    {
        private readonly ISerializationContext _mSerializationContext;

        public EncodingProvider(ISerializationContext ctx)
        {
            this._mSerializationContext = ctx;
        }

#pragma warning disable CA1041 // Provide ObsoleteAttribute message
        [Obsolete]
#pragma warning restore CA1041 // Provide ObsoleteAttribute message
        protected byte[] Decode7Bit(string value)
        {
            try
            {
                return new UTF7Encoding().GetBytes(value);
            }
            catch
            {
                return null;
            }
        }

        protected byte[] Decode8Bit(string value)
        {
            try
            {
                return new UTF8Encoding().GetBytes(value);
            }
            catch
            {
                return null;
            }
        }

        protected byte[] DecodeBase64(string value)
        {
            try
            {
                return Convert.FromBase64String(value);
            }
            catch
            {
                return null;
            }
        }

        protected virtual EncodingProvider.DecoderDelegate GetDecoderFor(string encoding)
        {
            if (encoding == null)
                return null;
            string upper = encoding.ToUpper();
            if (upper == "7BIT")
                return new EncodingProvider.DecoderDelegate(this.Decode7Bit);
            if (upper == "8BIT")
                return new EncodingProvider.DecoderDelegate(this.Decode8Bit);
            if (upper == "BASE64")
                return new EncodingProvider.DecoderDelegate(this.DecodeBase64);
            return null;
        }

        protected string Encode7Bit(byte[] data)
        {
            try
            {
                return new UTF7Encoding().GetString(data);
            }
            catch
            {
                return null;
            }
        }

        protected string Encode8Bit(byte[] data)
        {
            try
            {
                return new UTF8Encoding().GetString(data);
            }
            catch
            {
                return null;
            }
        }

        protected string EncodeBase64(byte[] data)
        {
            try
            {
                return Convert.ToBase64String(data);
            }
            catch
            {
                return null;
            }
        }

        protected virtual EncodingProvider.EncoderDelegate GetEncoderFor(string encoding)
        {
            if (encoding == null)
                return null;
            string upper = encoding.ToUpper();
            if (upper == "7BIT")
                return new EncodingProvider.EncoderDelegate(this.Encode7Bit);
            if (upper == "8BIT")
                return new EncodingProvider.EncoderDelegate(this.Encode8Bit);
            if (upper == "BASE64")
                return new EncodingProvider.EncoderDelegate(this.EncodeBase64);
            return null;
        }

        public string Encode(string encoding, byte[] data)
        {
            if (encoding == null || data == null)
                return null;
            EncodingProvider.EncoderDelegate encoderFor = this.GetEncoderFor(encoding);
            if (encoderFor == null)
                return null;
            return encoderFor(data);
        }

        public string DecodeString(string encoding, string value)
        {
            if (encoding == null || value == null)
                return null;
            byte[] bytes = this.DecodeData(encoding, value);
            if (bytes == null)
                return null;
            return (this._mSerializationContext.GetService(typeof(IEncodingStack)) as IEncodingStack).Current.GetString(bytes);
        }

        public byte[] DecodeData(string encoding, string value)
        {
            if (encoding == null || value == null)
                return null;
            EncodingProvider.DecoderDelegate decoderFor = this.GetDecoderFor(encoding);
            if (decoderFor == null)
                return null;
            return decoderFor(value);
        }

        public delegate string EncoderDelegate(byte[] data);

        public delegate byte[] DecoderDelegate(string value);
    }
}