namespace Escc.Redirects.Admin
{
    /// <summary>
    /// A repository in which to store redirects
    /// </summary>
    public interface IRedirectsRepository
    {
        /// <summary>
        /// Add or update a redirect in the repository.
        /// </summary>
        /// <param name="redirect">The redirect.</param>
        void SaveRedirect(Redirect redirect);

        /// <summary>
        /// Delete a redirect from the repository.
        /// </summary>
        /// <param name="redirectId">The redirect identifier.</param>
        void DeleteRedirect(int redirectId);
    }
}