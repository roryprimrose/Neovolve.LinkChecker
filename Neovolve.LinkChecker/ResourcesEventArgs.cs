using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Neovolve.LinkChecker
{
    /// <summary>
    /// 
    /// </summary>
    public class ResourcesEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourcesEventArgs"/> class.
        /// </summary>
        /// <param name="resources">The resources.</param>
        public ResourcesEventArgs(IEnumerable<ResourceMetadata> resources)
        {
            List<ResourceMetadata> newList = new List<ResourceMetadata>(resources);
            
            Resources = new ReadOnlyCollection<ResourceMetadata>(newList);
        }

        /// <summary>
        /// Gets or sets the resources.
        /// </summary>
        /// <value>The resources.</value>
        public ReadOnlyCollection<ResourceMetadata> Resources
        {
            get;
            private set;
        }
    }
}