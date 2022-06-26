using iCloud.Dav.Core.Utils;
using System;
using System.IO;
using System.Net;

namespace iCloud.Dav.People
{
    /// <summary>A photo embedded in a <see cref="Person"/>.</summary>
    /// <remarks>
    ///     <para>
    ///         You must specify the photo using a path, a byte array,
    ///         or a System.Drawing.Bitmap instance. The class will
    ///         extract the underlying raw bytes for storage into the
    ///         Person.  You can call the <see cref="!:GetBitmap" /> function
    ///         to create a new Windows bitmap object (e.g. for display
    ///         on a form) or <see cref="Photo.GetBytes" /> to extract the raw
    ///         bytes (e.g. for transmission from a web page).
    ///     </para>
    /// </remarks>
    [Serializable]
    public class Photo
    {
        #region Fields/Consts

        /// <summary>The raw bytes of the image data.</summary>
        /// <remarks>
        ///     The raw bytes can be passed directly to the photo object
        ///     or fetched from a file or remote URL.  A .NET bitmap object
        ///     can also be specified, in which case the constructor
        ///     will load the raw bytes from the bitmap.
        /// </remarks>
        private byte[] _data;
        /// <summary>The url of the image.</summary>
        private Uri _url;
        private readonly PhotoImageFormat _format;

        #endregion

        #region Properties

        public PhotoImageFormat PhotoFormat => _format;

        /// <summary>
        ///     Indicates the bytes of the raw image have
        ///     been loaded by the object.
        /// </summary>
        /// <seealso cref="Photo.Fetch" />
        public bool IsLoaded => _data != null;

        /// <summary>The URL of the image.</summary>
        /// <remarks>
        ///     Changing the URL will automatically invalidate the internal
        ///     image data if previously fetched.
        /// </remarks>
        /// <seealso cref="Photo.Fetch" />
        public Uri Url
        {
            get => _url;
            set
            {
                if (value == null)
                {
                    _data = null;
                    _url = null;
                }
                else
                {
                    if (!(_url != value))
                        return;
                    _data = null;
                    _url = value;
                }
            }
        }

        #endregion

        /// <summary>Loads a photograph from an array of bytes.</summary>
        /// <param name="buffer">
        ///     An array of bytes containing the raw data from
        ///     any of the supported image formats.
        /// </param>
        /// <param name="format">The image format.</param>
        public Photo(byte[] buffer, PhotoImageFormat format)
        {
            buffer.ThrowIfNull(nameof(buffer));
            _data = (byte[])buffer.Clone();
            _format = format;
        }

        /// <summary>The URL of the image.</summary>
        /// <param name="url">A URL pointing to an image.</param>
        public Photo(Uri url) => _url = url.ThrowIfNull(nameof(url));

        /// <summary>Creates a new Person photo from an image file.</summary>
        /// <param name="path">
        ///     The path to an image of any supported format.
        /// </param>
        public Photo(string path)
        {
            path.ThrowIfNullOrEmpty(nameof(path));

            _url = new Uri(path);
            var extension = Path.GetExtension(path);
            if (extension == ".bmp")
                _format = PhotoImageFormat.Bmp;
            else if (extension == ".gif")
            {
                _format = PhotoImageFormat.Gif;
            }
            else
            {
                if (!(extension == ".jpg") && !(extension == ".jpeg"))
                    return;
                _format = PhotoImageFormat.Jpeg;
            }
        }

        #region Methods

        /// <summary>Fetches a linked image asynchronously.</summary>
        /// <remarks>
        ///     This is a simple utility method for accessing the image
        ///     referenced by the URL.  For asynchronous or advanced
        ///     loading you will need to download the image yourself
        ///     and load the bytes directly into the class.
        /// </remarks>
        /// <seealso cref="IsLoaded" />
        /// <seealso cref="Url" />
        public void Fetch()
        {
            var webResponse = !(_url == null) ? WebRequest.CreateDefault(_url).GetResponse() : throw new InvalidOperationException();
            using var responseStream = webResponse.GetResponseStream();
            _data = new byte[webResponse.ContentLength];
            responseStream.Read(_data, 0, (int)webResponse.ContentLength);
        }

        /// <summary>Returns a copy of the raw bytes of the image.</summary>
        /// <returns>
        ///     A byte array containing the raw bytes of the image.
        /// </returns>
        /// <remarks>
        ///     A copy of the raw bytes are returned.  Modifying the
        ///     array will not modify the photo.
        /// </remarks>
        public byte[] GetBytes() => (byte[])_data.Clone();

        #endregion
    }
}
