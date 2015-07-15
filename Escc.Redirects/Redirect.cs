using System;

namespace Escc.Redirects
{
    /// <summary>
    /// An instruction to redirect one URL to another
    /// </summary>
    public class Redirect
    {
        /// <summary>
        /// Gets or sets the HTTP status code to use.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the URL to redirect from.
        /// </summary>
        /// <value>
        /// The requested URL.
        /// </value>
        public Uri RequestedUrl { get; set; }

        /// <summary>
        /// Gets or sets the destination URL.
        /// </summary>
        /// <value>
        /// The destination URL.
        /// </value>
        public Uri DestinationUrl { get; set; }
    }
}
