using System;

namespace Neovolve.LinkChecker
{
    /// <summary>
    /// 
    /// </summary>
    public class ResourceEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceEventArgs"/> class.
        /// </summary>
        /// <param name="resource">The resource.</param>
        public ResourceEventArgs(ResourceMetadata resource)
        {
            Resource = resource;
        }

        /// <summary>
        /// Gets or sets the resource.
        /// </summary>
        /// <value>The resource.</value>
        public ResourceMetadata Resource
        {
            get;
            private set;
        }
    }
}