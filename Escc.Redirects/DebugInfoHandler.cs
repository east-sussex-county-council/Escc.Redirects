using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace Escc.Redirects
{
    /// <summary>
    /// Adds debug info to the redirect to make management easier
    /// </summary>
    class DebugInfoHandler : IRedirectHandler
    {
        /// <summary>
        /// Execute the handler, and return an updated redirect
        /// </summary>
        /// <param name="redirect">The redirect.</param>
        /// <returns>
        /// The same redirect, which may have been updated
        /// </returns>
        public Redirect HandleRedirect(Redirect redirect)
        {
            HttpContext.Current.Response.AppendHeader("X-ESCC-Redirect", redirect.RedirectId.ToString(CultureInfo.InvariantCulture));
            return redirect;
        }
    }
}
