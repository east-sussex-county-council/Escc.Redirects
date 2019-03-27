using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escc.Redirects
{
    /// <summary>
    /// Preserves the querystring of the original request in the destination request, unless the destination has its own
    /// </summary>
    public class PreserveQueryStringHandler : IPreserveQueryStringHandler
    {
        /// <summary>
        /// Execute the handler, and return an updated redirect
        /// </summary>
        /// <param name="redirect">The redirect.</param>
        /// <returns>
        /// The same redirect, which may have been updated
        /// </returns>
        /// <exception cref="System.ArgumentNullException">redirect</exception>
        /// <exception cref="System.NullReferenceException">
        /// redirect.RequestedUrl cannot be null
        /// or
        /// redirect.DestinationUrl cannot be null
        /// </exception>
        public Redirect HandleRedirect(Redirect redirect)
        {
            if (redirect == null) throw new ArgumentNullException("redirect");
            if (redirect.RequestedUrl == null) throw new ArgumentException("redirect.RequestedUrl cannot be null");
            if (redirect.DestinationUrl == null) throw new ArgumentException("redirect.DestinationUrl cannot be null");

            // If the request had a querystring, and the redirect didn't change it, keep the original one
            if (String.IsNullOrEmpty(redirect.DestinationUrl.Query) && !String.IsNullOrEmpty(redirect.RequestedUrl.Query))
            {
                redirect.DestinationUrl = new Uri(redirect.DestinationUrl + redirect.RequestedUrl.Query);
            }

            return redirect;
        }
    }
}
