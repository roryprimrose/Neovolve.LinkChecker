using System;
using System.Net;

namespace Neovolve.LinkChecker
{
    /// <summary>
    /// The <see cref="ResourceState"/>
    /// class is used to define the state of a resourcce.
    /// </summary>
    public class ResourceState
    {
        /// <summary>
        /// Gets or sets the MIME type.
        /// </summary>
        /// <value>The MIME type.</value>
        public String MimeType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the response code.
        /// </summary>
        /// <value>The response code.</value>
        public HttpStatusCode ResponseCode
        {
            get;
            set;
        }
    }
}