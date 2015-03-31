using System;

namespace Neovolve.LinkChecker
{
    internal interface IResourceLoader
    {
        /// <summary>
        /// Gets the content of the resource.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns>A <see cref="string"/> that contains the content of the resource.</returns>
        String GetResourceContent(Uri location);

        /// <summary>
        /// Gets the resource state.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns>A <see cref="ResourceState"/> value.</returns>
        ResourceState GetResourceState(Uri location);
    }
}