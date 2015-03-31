using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;

namespace Neovolve.LinkChecker
{
    /// <summary>
    /// The <see cref="ResourceProcessor"/>
    /// class is used to process resources and raise events for new resources found 
    /// and status updates of resources processed.
    /// </summary>
    public class ResourceProcessor
    {
        /// <summary>
        /// Occurs when a resource is found.
        /// </summary>
        public event EventHandler<ResourceEventArgs> ResourceFound;

        /// <summary>
        /// Occurs when [resources found].
        /// </summary>
        public event EventHandler<ResourcesEventArgs> ResourcesFound;

        /// <summary>
        /// Occurs when a resource is updated.
        /// </summary>
        public event EventHandler<ResourceEventArgs> ResourceUpdated;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceProcessor"/> class.
        /// </summary>
        public ResourceProcessor()
        {
            ResourcesStore = new Dictionary<String, ResourceMetadata>();

            ThreadPool.SetMaxThreads(10, 5);
        }

        /// <summary>
        /// Exports the resources.
        /// </summary>
        /// <returns></returns>
        public IList<ResourceMetadata> ExportResources()
        {
            return new List<ResourceMetadata>(ResourcesStore.Values);
        }

        /// <summary>
        /// Imports the resources.
        /// </summary>
        /// <param name="resources">The resources.</param>
        public void ImportResources(IEnumerable<ResourceMetadata> resources)
        {
            ImportResourcesInternal(resources, false);
        }

        /// <summary>
        /// Replays the by status codes.
        /// </summary>
        /// <param name="codes">The codes.</param>
        public void ReplayByStatusCodes(List<String> codes)
        {
            ReplayCodes = codes;

            foreach (ResourceMetadata metadata in ResourcesStore.Values)
            {
                CheckResource(metadata);
            }
        }

        /// <summary>
        /// Checks the resource.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        private void CheckResource(ResourceState metadata)
        {
            if (ReplayCodes == null
                || ReplayCodes.Contains(metadata.ResponseCode.ToString()))
            {
                ThreadPool.QueueUserWorkItem(ResolveLinks, metadata);
            }
        }

        /// <summary>
        /// Gets or sets the replay codes.
        /// </summary>
        /// <value>The replay codes.</value>
        private List<string> ReplayCodes
        {
            get;
            set;
        }

        /// <summary>
        /// Starts the specified base location.
        /// </summary>
        /// <param name="baseLocation">The base location.</param>
        /// <param name="filterExpression">The filter expression.</param>
        public void Start(String baseLocation, String filterExpression)
        {
            // TODO: Manage cases where there is an existing session running

            ResourcesStore.Clear();
            BaseLocation = new Uri(baseLocation);

            if (String.IsNullOrEmpty(filterExpression) == false)
            {
                FilterExpression = new Regex(filterExpression);
            }
            else
            {
                FilterExpression = null;
            }

            ResourceMetadata metadata = new ResourceMetadata(null, null);

            AddResource(metadata);

            OnResourceFound(new ResourceEventArgs(metadata));

            ReplayAll();
        }

        /// <summary>
        /// Raises the <see cref="ResourceFound"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Neovolve.LinkChecker.ResourceEventArgs"/> instance containing the event data.</param>
        protected internal virtual void OnResourceFound(ResourceEventArgs e)
        {
            if (ResourceFound != null)
            {
                ResourceFound(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ResourcesFound"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Neovolve.LinkChecker.ResourcesEventArgs"/> instance containing the event data.</param>
        protected internal virtual void OnResourcesFound(ResourcesEventArgs e)
        {
            if (ResourcesFound != null)
            {
                ResourcesFound(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ResourceUpdated"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Neovolve.LinkChecker.ResourceEventArgs"/> instance containing the event data.</param>
        protected internal virtual void OnResourceUpdated(ResourceEventArgs e)
        {
            if (ResourceUpdated != null)
            {
                ResourceUpdated(this, e);
            }
        }

        /// <summary>
        /// Adds the resource.
        /// </summary>
        /// <param name="newResource">The new resource.</param>
        /// <returns></returns>
        private Boolean AddResource(ResourceMetadata newResource)
        {
            if (ResourcesStore.ContainsKey(newResource.Key))
            {
                return false;
            }

            if (FilterExpression != null && newResource.RelativeLocation != null
                && FilterExpression.IsMatch(newResource.RelativeLocation.ToString()))
            {
                return false;
            }

            ResourcesStore.Add(newResource.Key, newResource);

            return true;
        }

        /// <summary>
        /// Imports the resources internal.
        /// </summary>
        /// <param name="resources">The resources.</param>
        /// <param name="checkResources">if set to <c>true</c> [check resources].</param>
        private void ImportResourcesInternal(IEnumerable<ResourceMetadata> resources, Boolean checkResources)
        {
            foreach (ResourceMetadata newResource in resources)
            {
                Boolean resourceAdded = AddResource(newResource);

                if (resourceAdded
                    && checkResources)
                {
                    CheckResource(newResource);
                }
            }

            OnResourcesFound(new ResourcesEventArgs(resources));
        }

        /// <summary>
        /// Replays all.
        /// </summary>
        private void ReplayAll()
        {
            // Add all the items to the thread pool
            foreach (ResourceMetadata metadata in ResourcesStore.Values)
            {
                CheckResource(metadata);
            }
        }

        /// <summary>
        /// Resolves the links.
        /// </summary>
        /// <param name="state">The state.</param>
        private void ResolveLinks(Object state)
        {
            // Get the listview item
            ResourceMetadata metadata = state as ResourceMetadata;

            if (metadata == null)
            {
                throw new InvalidCastException("state does not contain a ResourceMetadata type.");
            }

            Thread.CurrentThread.Name = "Resolve " + metadata.RelativeLocation;

            HttpResourceLoader loader = new HttpResourceLoader();
            ResourceResolver resolver = new ResourceResolver(loader);

            metadata.Status = ResourceMetadataStatus.Processing;
            OnResourceUpdated(new ResourceEventArgs(metadata));

            resolver.UpdateResourceMetadata(metadata, BaseLocation);
            metadata.Status = ResourceMetadataStatus.HeadersChecked;

            if (String.IsNullOrEmpty(metadata.MimeType) == false
                && metadata.MimeType.Contains("text"))
            {
                metadata.Status = ResourceMetadataStatus.Downloading;
                OnResourceUpdated(new ResourceEventArgs(metadata));

                IEnumerable<ResourceMetadata> relatedResources = resolver.GetRelatedResources(metadata, BaseLocation);

                ImportResourcesInternal(relatedResources, true);
            }

            metadata.Status = ResourceMetadataStatus.ResourcesChecked;

            OnResourceUpdated(new ResourceEventArgs(metadata));
        }

        /// <summary>
        /// Gets or sets the base location.
        /// </summary>
        /// <value>The base location.</value>
        public Uri BaseLocation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the filter expression.
        /// </summary>
        /// <value>The filter expression.</value>
        public Regex FilterExpression
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the resources store.
        /// </summary>
        /// <value>The resources store.</value>
        private Dictionary<String, ResourceMetadata> ResourcesStore
        {
            get;
            set;
        }
    }
}