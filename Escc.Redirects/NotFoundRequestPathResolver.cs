using System;
using System.Globalization;
using System.Web;

namespace Escc.Redirects
{
    /// <summary>
    /// Extracts the originally requested path from the URL passed to a 404 page
    /// </summary>
    public class NotFoundRequestPathResolver
    {
        /// <summary>
        /// Find the path which was originally requested and normalise it to a server-relative URL. The URL format is different depending on whether it came direct from IIS or via ASP.NET
        /// </summary>
        /// <param name="requestUrl">The request URL.</param>
        /// <returns></returns>
        public Uri NormaliseRequestedPath(Uri requestUrl)
        {
            var originalUrl = String.Empty;
            var queryString = HttpUtility.ParseQueryString(requestUrl.Query);
            if (!String.IsNullOrEmpty(queryString["aspxerrorpath"]))
            {
                originalUrl = queryString["aspxerrorpath"];
            }
            else
            {
                try
                {
                    string urlNotFound = HttpUtility.UrlDecode(requestUrl.Query);
                    int? requestedUrlPos = null;
                    if (urlNotFound != null)
                    {
                        requestedUrlPos = urlNotFound.LastIndexOf("404;", StringComparison.OrdinalIgnoreCase);
                    }
                    if (requestedUrlPos.HasValue && requestedUrlPos.Value > -1)
                    {
                        originalUrl = new Uri(urlNotFound.Substring(requestedUrlPos.Value + 4)).PathAndQuery;
                    }

                }
                catch (UriFormatException)
                {
                    // If someone's trying to feed in something other than an unrecognised URL, just show them the standard 404
                    return null;
                }
            }

            // Using Server.TransferRequest to pass a URL to this page can sometimes result in an extra request to check the URL of the 404 page itself, which we can ignore
            if (originalUrl.StartsWith(requestUrl.AbsolutePath))
            {
                return null;
            }

            if (!String.IsNullOrEmpty(originalUrl) && originalUrl != "/")
            {
                originalUrl = originalUrl.Replace(Environment.NewLine, String.Empty).TrimEnd('/').ToLower(CultureInfo.CurrentCulture);
            }

            return new Uri(requestUrl, originalUrl);
        }
    }
}