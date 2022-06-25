using iCloud.Dav.Core.Services;

namespace iCloud.Dav.People
{
    public class Membership : IDirectResponseSchema
    {
        #region Properties

        /// <summary>
        /// The unique id of Membership.
        /// </summary>
        public virtual string ContactGroupId { get; set; }

        /// <summary>
        /// The resource name of Membership.
        /// </summary>
        public virtual string ContactGroupResourceName { get; set; }

        /// <summary>The e-tag of this response.</summary>
        /// <remarks>
        /// Will be set by the service deserialization method,
        /// or the by json response parser if implemented on service.
        /// </remarks>
        public virtual string ETag { get; set; }

        #endregion
    }
}