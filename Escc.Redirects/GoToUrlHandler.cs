using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using EsccWebTeam.Data.Web;

namespace Escc.Redirects
{
    /// <summary>
    /// Once a URL has been recognised, this redirects to it with correct HTTP response
    /// </summary>
    public class GoToUrlHandler : IRedirectHandler
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
            try
            {
                // Generate redirect headers and end this response to ensure they're followed
                switch (redirect.StatusCode)
                {
                    case 301:
                        Http.Status301MovedPermanently(redirect.DestinationUrl);
                        break;
                    case 303:
                        Http.Status303SeeOther(redirect.DestinationUrl);
                        break;
                }
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }

            return redirect;
        }
    }
}
