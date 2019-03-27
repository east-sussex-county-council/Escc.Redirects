using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Escc.Redirects
{
    /// <summary>
    /// Ensures that the destination of a redirect is an absolute URL
    /// </summary>
    public class ConvertToAbsoluteUrlHandler : IConvertToAbsoluteUrlHandler
    {
        /// <summary>
        /// Execute the handler, and return an updated redirect
        /// </summary>
        /// <param name="redirect">The redirect.</param>
        /// <returns>
        /// The same redirect, which may have been updated
        /// </returns>
        /// <exception cref="System.ArgumentNullException">redirect</exception>
        /// <exception cref="System.NullReferenceException">redirect.DestinationUrl cannot be null</exception>
        public Redirect HandleRedirect(Redirect redirect)
        {
            if (redirect == null) throw new ArgumentNullException("redirect");
            if (redirect.DestinationUrl == null) throw new ArgumentException("redirect.DestinationUrl cannot be null");

            if (!redirect.DestinationUrl.IsAbsoluteUri)
            {
                redirect.DestinationUrl = new Uri(redirect.RequestedUrl, redirect.DestinationUrl);
            }
            return redirect;
        }
    }
}
