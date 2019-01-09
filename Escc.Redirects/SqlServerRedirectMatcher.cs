using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using Dapper;

namespace Escc.Redirects
{
    /// <summary>
    /// Looks up the requested URL against a list of redirects in a SQL server database
    /// </summary>
    /// <seealso cref="Escc.Redirects.IRedirectMatcher" />
    public class SqlServerRedirectMatcher : IRedirectMatcher
    {
        private readonly string _connectionString;

        /// <summary>
        /// Creates a new instance of <see cref="SqlServerRedirectMatcher"/>
        /// </summary>
        /// <param name="connectionString">The connection string for the SQL Server database containing the redirects</param>
        public SqlServerRedirectMatcher(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Gets or sets whether to throw an error if the connection to SQL Server is not configured
        /// </summary>
        /// <value>
        /// <c>true</c> to throw an error on missing configuration; <c>false</c> to return <c>null</c>.
        /// </value>
        public bool ThrowErrorOnMissingConfiguration { get; set; } = true;

        /// <summary>
        /// Try to match the requested URL against a configured redirect
        /// </summary>
        /// <param name="requestedUrl">The requested URL.</param>
        /// <returns>
        /// The matched redirect, or <c>null</c> if no matching redirect found
        /// </returns>
        /// <exception cref="System.ArgumentNullException">requestedUrl</exception>
        /// <exception cref="ConfigurationErrorsException">RedirectsReader connection string not found in configuration file</exception>
        public Redirect MatchRedirect(Uri requestedUrl)
        {
            if (requestedUrl == null) throw new ArgumentNullException("requestedUrl");
            if (String.IsNullOrWhiteSpace(_connectionString)) return null;

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var reader = connection.ExecuteReader("usp_Redirect_MatchRequest", new { request = requestedUrl.PathAndQuery.TrimStart('/') }, commandType: CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        var redirectId = Int32.Parse(reader["RedirectId"].ToString(), CultureInfo.InvariantCulture);

                        // Get the URL
                        var destinationUrl = new Uri(reader["Destination"].ToString(), UriKind.RelativeOrAbsolute);

                        // Get the HTTP status code
                        var redirectType = (RedirectType)Enum.Parse(typeof(RedirectType), reader["Type"].ToString());
                        var statusCode = (redirectType == RedirectType.Moved) ? 301 : 303;

                        // Return the redirect
                        return new Redirect()
                        {
                            RedirectId = redirectId,
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
