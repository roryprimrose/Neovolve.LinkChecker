using System;

namespace Neovolve.LinkChecker
{
    /// <summary>
    /// The <see cref="ResourceMetadata"/>
    /// class defines the attributes of a resource.
    /// </summary>
    public class ResourceMetadata : ResourceState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceMetadata"/> class.
        /// </summary>
        /// <param name="relativeLocation">The relative location.</param>
        /// <param name="referrer">The referrer.</param>
        public ResourceMetadata(Uri relativeLocation, Uri referrer)
        {
            if (relativeLocation != null
                && relativeLocation.IsAbsoluteUri)
            {
                throw new ArgumentException(
                    "relativeLocation is an absolute location when a relative location is required.");
            }

            if (referrer != null
                && referrer.IsAbsoluteUri)
            {
                throw new ArgumentException("referrer is an absolute location when a relative location is required.");
            }

            RelativeLocation = relativeLocation;
            Referrer = referrer;

            if (RelativeLocation != null)
            {
                Key = RelativeLocation.ToString();
            }
            else
            {
                Key = String.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public String Key
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the referrer.
        /// </summary>
        /// <value>The referrer.</value>
        public Uri Referrer
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the relative location.
        /// </summary>
        /// <value>The relative location.</value>
        public Uri RelativeLocation
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public ResourceMetadataStatus Status
        {
            get;
            set;
        }
    }
}