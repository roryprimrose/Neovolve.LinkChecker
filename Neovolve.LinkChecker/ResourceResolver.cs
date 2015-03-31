using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Neovolve.LinkChecker
{
    internal class ResourceResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceResolver"/> class.
        /// </summary>
        /// <param name="loader">The loader.</param>
        public ResourceResolver(IResourceLoader loader)
        {
            // Checks whether the loader parameter has been supplied
            if (loader == null)
            {
                const String LoaderParameterName = "loader";

                throw new ArgumentNullException(LoaderParameterName);
            }

            Loader = loader;
        }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <param name="metadata">The relative location.</param>
        /// <param name="baseLocation">The base location.</param>
        public void UpdateResourceMetadata(ResourceMetadata metadata, Uri baseLocation)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException("metadata");
            }

            if (baseLocation == null)
            {
                throw new ArgumentNullException("baseLocation");
            }

            if (baseLocation.IsAbsoluteUri == false)
            {
                throw new ArgumentException("The baseLocation value is not an absolute Uri");
            }

            Uri actualLocation;

            if (metadata.RelativeLocation != null)
            {
                actualLocation = new Uri(baseLocation, metadata.RelativeLocation);
            }
            else
            {
                actualLocation = baseLocation;
            }

            ResourceState resolvedState = Loader.GetResourceState(actualLocation);

            metadata.ResponseCode = resolvedState.ResponseCode;
            metadata.MimeType = resolvedState.MimeType;
        }

        /// <summary>
        /// Gets the related resources.
        /// </summary>
        /// <param name="parentMetadata">The parent metadata.</param>
        /// <param name="baseLocation">The base location.</param>
        /// <returns></returns>
        public Collection<ResourceMetadata> GetRelatedResources(ResourceMetadata parentMetadata, Uri baseLocation)
        {
            const String AnchorPattern = @"(?<=<a[^>]*href=(\""|')(?!mailto:|javascript:))(.*?)(?=(\""|')[^>]*>)";
            const String ImagePattern = @"(?<=<img[^>]*src=(\""|'))(.*?)(?=(\""|')[^>]*>)";

            Collection<ResourceMetadata> items = new Collection<ResourceMetadata>();
            Uri absoluteLocation;

            if (parentMetadata.RelativeLocation != null)
            {
                absoluteLocation = new Uri(baseLocation, parentMetadata.RelativeLocation);
            }
            else
            {
                absoluteLocation = baseLocation;
            }

            Regex linkResolver = new Regex(AnchorPattern);
            List<String> pathsResolved = new List<String>();

            // Get the content of the resource and identify the links in it
            String resourceContent = Loader.GetResourceContent(absoluteLocation);
            MatchCollection linkMatches = linkResolver.Matches(resourceContent);

            for (Int32 index = 0; index < linkMatches.Count; index++)
            {
                Match linkMatch = linkMatches[index];
                //Debug.WriteLine("Found regex link: " + linkMatch);

                // If there resource isn't valid, skip to the next item
                if (ResourceIsValidate(baseLocation, linkMatch.Value) == false)
                {
                    continue;
                }

                // Determine the relative path for this item
                Uri relativeLocation = DetermineRelativeLocation(linkMatch.Value, baseLocation);

                if (pathsResolved.Contains(relativeLocation.ToString().ToUpperInvariant()) == false)
                {
                    ResourceMetadata newResource = new ResourceMetadata(relativeLocation, parentMetadata.RelativeLocation);

                    pathsResolved.Add(newResource.RelativeLocation.ToString().ToUpperInvariant());

                    items.Add(newResource);
                }
            }

            // Get the image references in the content
            Regex imageResolver = new Regex(ImagePattern);
            MatchCollection imageMatches = imageResolver.Matches(resourceContent);

            for (Int32 index = 0; index < imageMatches.Count; index++)
            {
                Match imageMatch = imageMatches[index];
                //Debug.WriteLine("Found regex link: " + imageMatch);

                // If there resource isn't valid, skip to the next item
                if (ResourceIsValidate(baseLocation, imageMatch.Value) == false)
                {
                    continue;
                }

                // Determine the relative path for this item
                Uri relativeLocation = DetermineRelativeLocation(imageMatch.Value, baseLocation);

                if (pathsResolved.Contains(relativeLocation.ToString().ToUpperInvariant()) == false)
                {
                    ResourceMetadata newResource = new ResourceMetadata(relativeLocation, parentMetadata.RelativeLocation);

                    pathsResolved.Add(newResource.RelativeLocation.ToString().ToUpperInvariant());

                    items.Add(newResource);
                }
            }

            return items;
        }

        /// <summary>
        /// Validates the resource.
        /// </summary>
        /// <param name="baseLocation">The base location.</param>
        /// <param name="itemLocation">The item location.</param>
        /// <returns></returns>
        private static Boolean ResourceIsValidate(
            Uri baseLocation, String itemLocation)
        {
            Uri location;

            try
            {
                location = new Uri(itemLocation, UriKind.RelativeOrAbsolute);
            }
            catch (Exception)
            {
                // This is not a valid uri
                return false;
            }

            if (location.IsAbsoluteUri)
            {
                if (location.AbsoluteUri.StartsWith(baseLocation.AbsoluteUri, StringComparison.OrdinalIgnoreCase)
                    == false)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines the relative location.
        /// </summary>
        /// <param name="relativeLocation">The relative location.</param>
        /// <param name="baseLocation">The base location.</param>
        /// <returns></returns>
        private static Uri DetermineRelativeLocation(String relativeLocation, Uri baseLocation)
        {
            const String BookmarkIndicator = "#";
            String locationWithoutBookmark;

            // Check if there is a bookmark
            if (relativeLocation.Contains(BookmarkIndicator))
            {
                locationWithoutBookmark = relativeLocation.Substring(0, relativeLocation.IndexOf(BookmarkIndicator, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                locationWithoutBookmark = relativeLocation;
            }

            Uri calculatedLocation = new Uri(locationWithoutBookmark, UriKind.RelativeOrAbsolute);

            if (calculatedLocation.IsAbsoluteUri)
            {
                return baseLocation.MakeRelativeUri(calculatedLocation);
            }

            return calculatedLocation;
        }

        /// <summary>
        /// Gets or sets the loader.
        /// </summary>
        /// <value>The loader.</value>
        private IResourceLoader Loader
        {
            get;
            set;
        }
    }
}