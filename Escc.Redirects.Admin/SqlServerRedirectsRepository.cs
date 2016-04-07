using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using Microsoft.ApplicationBlocks.Data;

namespace Escc.Redirects.Admin
{
    /// <summary>
    /// Store redirects in a SQL Server database
    /// </summary>
    /// <seealso cref="Escc.Redirects.Admin.IRedirectsRepository" />
    public class SqlServerRedirectsRepository : IRedirectsRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerRedirectsRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public SqlServerRedirectsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Add or update a redirect in the repository.
        /// </summary>
        /// <param name="redirect">The redirect.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void SaveRedirect(Redirect redirect)
        {
            if (redirect == null) throw new ArgumentNullException(nameof(redirect));

            var values = new SqlParameter[5];
            values[0] = new SqlParameter("@pattern", redirect.RequestedUrl.ToString());
            values[1] = new SqlParameter("@destination", redirect.DestinationUrl.ToString());
            values[2] = new SqlParameter("@type", redirect.StatusCode);
            values[3] = new SqlParameter("@comment", redirect.Comment);

            if (redirect.RedirectId.HasValue)
            {
                values[4] = new SqlParameter("@redirectId", redirect.RedirectId.Value);
                SqlHelper.ExecuteNonQuery(_connectionString, CommandType.StoredProcedure, "usp_Redirect_Update", values);
            }
            else
            {
                SqlHelper.ExecuteNonQuery(_connectionString, CommandType.StoredProcedure, "usp_Redirect_Insert", values);
            }
        }

        /// <summary>
        /// Delete a redirect from the repository.
        /// </summary>
        /// <param name="redirectId">The redirect identifier.</param>
        public void DeleteRedirect(int redirectId)
        {
            SqlHelper.ExecuteNonQuery(_connectionString, CommandType.StoredProcedure, "usp_Redirect_Delete", new SqlParameter("@redirectId", redirectId));
        }
    }
}