using System;

namespace Escc.Redirects
{
    /// <summary>
    /// Try to match the requested URL against a configured redirect
    /// </summary>
    public interface IRedirectMatcher
    {
        /// <summary>
        /// Try to match the requested URL against a configured redirect
        /// </summary>
        /// <param name="requestedUrl">The requested URL.</param>
        /// <returns>The matched redirect, or <c>null</c> if no matching redirect found</returns>
        Redirect MatchRedirect(Uri requestedUrl);
    }
}