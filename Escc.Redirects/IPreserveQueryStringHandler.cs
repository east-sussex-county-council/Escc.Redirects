using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Escc.Redirects
{
    /// <summary>
    /// Preserves the querystring of the original request in the destination request, unless the destination has its own
    /// </summary>
    public interface IPreserveQueryStringHandler : IRedirectHandler
    {
    }
}
