using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace Escc.Redirects
{
    /// <summary>
    /// Looks up the requested URL against a list of redirects in a SQL server database
    /// </summary>
    public class SqlServerRedirectMatcher : IRedirectMatcher
    {
        /// <summary>
        /// Try to match the requested URL against a configured redirect
        /// </summary>
        /// <param name="requestedUrl">The requested URL.</param>
        /// <returns>
        /// The matched redirect, or <c>null</c> if no matching redirect found
        /// </returns>
        /// <exception cref="System.ArgumentNullException">requestedUrl</exception>
        public Redirect MatchRedirect(Uri requestedUrl)
        {
            if (requestedUrl == null) throw new ArgumentNullException("requestedUrl");
            if (ConfigurationManager.ConnectionStrings["RedirectsReader"] == null || String.IsNullOrEmpty(ConfigurationManager.ConnectionStrings["RedirectsReader"].ConnectionString))
            {
                throw new ConfigurationErrorsException("RedirectsReader connection string not found in configuration file");
            }

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["RedirectsReader"].ConnectionString))
            {
                using (var reader = connection.ExecuteReader("usp_Redirect_MatchRequest", new {request = requestedUrl.PathAndQuery.TrimStart('/')}, commandType: CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        // Get the URL
                        var destinationUrl = new Uri(reader["Destination"].ToString(), UriKind.RelativeOrAbsolute);

                        // Get the HTTP status code
                        var redirectType = (RedirectType)Enum.Parse(typeof(RedirectType), reader["Type"].ToString());
                        var statusCode = (redirectType == RedirectType.Moved) ? 301 : 303;

                        // Return the redirect
                        return new Redirect()
                        {
                            RequestedUrl = requestedUrl,
                            DestinationUrl = destinationUrl,
                            StatusCode = statusCode
                        };
                    }
                }
            }

            return null;
        }

        private enum RedirectType
        {
            ShortUrl = 1,
            Moved = 2
        }
    }
}
