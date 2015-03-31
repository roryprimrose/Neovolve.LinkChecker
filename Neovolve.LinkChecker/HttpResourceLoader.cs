using System;
using System.IO;
using System.Net;

namespace Neovolve.LinkChecker
{
    internal class HttpResourceLoader : IResourceLoader
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
            HttpWebRequest request = WebRequest.Create(location) as HttpWebRequest;

            if (request == null)
            {
                throw new WebException("Failed to create web request.");
            }
            
            request.AllowAutoRedirect = true;
            request.Method = "GET";
            request.Credentials = CredentialCache.DefaultCredentials;

            try
            {
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                if (response == null)
                {
                    throw new WebException("Failed to get web response");
                }
                
                Stream responseStream = response.GetResponseStream();

                using (StreamReader reader = new StreamReader(responseStream))
                {
                    String contents = reader.ReadToEnd();

                    response.Close();

                    return contents;
                }
            }
            catch (WebException ex)
            {
                return ex.Status.ToString();
            }
        }

        /// <summary>
        /// Gets the resource metadata.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns>A <see cref="ResourceMetadata"/> value.</returns>
        public ResourceState GetResourceState(Uri location)
        {
            HttpWebRequest request = WebRequest.Create(location) as HttpWebRequest;

            if (request == null)
            {
                throw new WebException("Failed to create web request.");
            }
            
            request.AllowAutoRedirect = true;
            request.Method = "HEAD";
            request.Credentials = CredentialCache.DefaultCredentials;

            ResourceState itemState = new ResourceState();

            try
            {
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                
                if (response == null)
                {
                    throw new WebException("Failed to get web response");
                }
                
                itemState.MimeType = response.ContentType;
                itemState.ResponseCode = response.StatusCode;

                response.Close();
            }
            catch (Exception)
            {
                itemState.ResponseCode = HttpStatusCode.ServiceUnavailable;
            }
            
            return itemState;
        }
    }
}