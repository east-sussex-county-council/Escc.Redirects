using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Escc.Redirects
{
    /// <summary>
    /// Ensures that the destination of a redirect is an absolute URL
    /// </summary>
    public interface IConvertToAbsoluteUrlHandler : IRedirectHandler
    {
    }
}
