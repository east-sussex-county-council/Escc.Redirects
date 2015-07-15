using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Escc.Redirects
{
    /// <summary>
    /// Take an action on a matched redirect
    /// </summary>
    public interface IRedirectHandler
    {
        /// <summary>
        /// Execute the handler, and return an updated redirect
        /// </summary>
        /// <param name="redirect">The redirect.</param>
        /// <returns>The same redirect, which may have been updated</returns>
        Redirect HandleRedirect(Redirect redirect);
    }
}
