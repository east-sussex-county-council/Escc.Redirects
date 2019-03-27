using System;
using System.Collections.Generic;
using System.Text;

namespace Escc.Redirects
{
    /// <summary>
    /// Settings for working with redirects
    /// </summary>
    public class RedirectSettings
    {
        /// <summary>
        /// The connection string to use with <see cref="SqlServerRedirectMatcher"/>
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
