using System;

namespace Neovolve.LinkChecker.UnitTests
{
    internal class ResourceLoaderStub : IResourceLoader
    {
        /// <summary>
        /// Gets the content of the resource.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns>
        /// A <see cref="string"/> that contains the content of the resource.
        /// </returns>
        public String GetResourceContent(Uri location)
        {
            return ContentToReturn;
        }

        /// <summary>
        /// Gets the resource metadata.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns>A <see cref="ResourceMetadata"/> value.</returns>
        public ResourceState GetResourceState(Uri location)
        {
            return StateToReturn;
        }

        /// <summary>
        /// Gets or sets the content to return.
        /// </summary>
        /// <value>The content to return.</value>
        public String ContentToReturn
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the state to return.
        /// </summary>
        /// <value>The state to return.</value>
        public ResourceState StateToReturn
        {
            get;
            set;
        }
    }
}