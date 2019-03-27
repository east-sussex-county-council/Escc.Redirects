using System;

namespace Escc.Redirects
{
    /// <summary>
    /// Extracts the originally requested path from the URL passed to a 404 page
    /// </summary>
    public interface INotFoundRequestPathResolver
    {
        /// <summary>
        /// Find the path which was originally requested and normalise it to an absolute URL.
        /// </summary>
        /// <param name="requestUrl">The request URL.</param>
        /// <returns></returns>
        Uri NormaliseRequestedPath(Uri requestUrl);
    }
}