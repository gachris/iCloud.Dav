using iCloud.Dav.Core.Utils;
using iCloud.Dav.People.Serialization.DataTypes;
using System;
using System.Drawing;
using System.IO;
using vCard.Net;
using vCard.Net.DataTypes;

namespace iCloud.Dav.People.DataTypes
{
    /// <summary>A photo embedded in a <see cref="Contact"/>.</summary>
    /// <remarks>
    ///     <para>
    ///         You must specify the photo using a path, a byte array.
    ///     </para>
    /// </remarks>
    [Serializable]
    public class Photo : EncodableDataType
    {
        #region Properties

        /// <summary>The URL of the image.</summary>
        /// <remarks>
        ///     Changing the URL will automatically invalidate the internal
        ///     image data if previously fetched.
        /// </remarks>
        public Uri Url { get; set; }

        /// <summary>The raw bytes of the image data.</summary>
        public byte[] Data { get; set; }

        public Rectangle Rectangle { get; set; }

        #endregion

        /// <summary>Loads a photograph from an array of bytes.</summary>
        /// <param name="buffer">
        ///     An array of bytes containing the raw data from
        ///     any of the supported image formats.
        /// </param>
        /// <param name="rectangle"></param>
        public Photo(byte[] buffer, Rectangle rectangle)
        {
            buffer.ThrowIfNull(nameof(buffer));
            Data = (byte[])buffer.Clone();
            Rectangle = rectangle;
        }

        /// <summary>The URL of the image.</summary>
        /// <param name="url">A URL pointing to an image.</param>
        /// <param name="rectangle"></param>
        public Photo(Uri url, Rectangle rectangle)
        {
            Url = url.ThrowIfNull(nameof(url));
            Rectangle = rectangle;
        }

        public Photo()
        {
        }

        public Photo(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            var serializer = new PhotoSerializer();
            CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
        }
    }
}