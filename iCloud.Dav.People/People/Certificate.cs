using iCloud.Dav.Core.Utils;
using System;
using System.Security.Cryptography.X509Certificates;

namespace iCloud.Dav.People
{
    /// <summary>A certificate attached to a Person.</summary>
    /// <remarks>
    ///     <para>
    ///         A Person can be associated with a public key or
    ///         authentication certificate.  This is typically
    ///         a public X509 certificate that allows people to
    ///         use the key for validating messages.
    ///     </para>
    /// </remarks>
    [Serializable]
    public class Certificate
    {
        #region Properties

        /// <summary>The raw data of the certificate as a byte array.</summary>
        /// <remarks>
        ///     Most certificates consist of 8-bit binary data
        ///     that is encoded into a text format using BASE64
        ///     or a similar system.  This property provides
        ///     access to the computer-friendly, decoded data.
        /// </remarks>
        public byte[] Data { get; set; }

        /// <summary>
        ///     A short string that identifies the type of certificate.
        /// </summary>
        /// <remarks>The most common type is X509.</remarks>
        public string KeyType { get; set; }

        #endregion
        /// <summary>
        ///     Creates a new instance of the <see cref="Certificate" /> class.
        /// </summary>
        public Certificate()
        {
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="Certificate" />
        ///     class using the specified key type and raw certificate data.
        /// </summary>
        /// <param name="keyType">
        ///     A string that identifies the type of certificate,
        ///     such as X509.
        /// </param>
        /// <param name="data">
        ///     The raw certificate data stored as a byte array.
        /// </param>
        public Certificate(string keyType, byte[] data)
        {
            KeyType = keyType.ThrowIfNullOrEmpty(nameof(keyType));
            Data = data.ThrowIfNull(nameof(data));
        }

        /// <summary>
        ///     Creates a Person certificate based on an X509 certificate.
        /// </summary>
        /// <param name="x509">An initialized X509 certificate.</param>
        public Certificate(X509Certificate2 x509)
        {
            x509.ThrowIfNull(nameof(x509));
            Data = x509.RawData;
            KeyType = "X509";
        }
    }
}
