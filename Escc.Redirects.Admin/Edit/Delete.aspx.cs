using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using EsccWebTeam.Data.Web;
using Microsoft.ApplicationBlocks.Data;

namespace Escc.Redirects.Admin.Edit
{
    public partial class Delete : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && !String.IsNullOrEmpty(Request.QueryString["redirect"]))
            {
                // Get the redirect id from the querystring
                int editRedirectId;
                try
                {
                    editRedirectId = Int32.Parse(Request.QueryString["redirect"], CultureInfo.InvariantCulture);

                    // Read it from the db and populate the form
                    using (var reader = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["RedirectsWriter"].ConnectionString, CommandType.StoredProcedure, "usp_Redirect_Select", new SqlParameter("@redirectId", editRedirectId)))
                    {
                        while (reader.Read())
                        {
                            this.redirectId.Value = editRedirectId.ToString(CultureInfo.InvariantCulture);
                            this.type.Value = reader["Type"].ToString();
                            this.pattern.InnerText = reader["Pattern"].ToString();
                        }
                    }
                }
                catch (OverflowException) { }
                catch (FormatException) { }

            }
            else
            {
                Http.Status400BadRequest();
            }
        }

        protected void Edit_Click(object sender, EventArgs e)
        {
            Http.Status303SeeOther(new Uri("edit.aspx?redirect=" + this.redirectId.Value, UriKind.Relative));
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            new SqlServerRedirectsRepository(ConfigurationManager.ConnectionStrings["RedirectsWriter"].ConnectionString).DeleteRedirect(Int32.Parse(this.redirectId.Value, CultureInfo.InvariantCulture));

            // Go back to the list where the redirect was
            switch (this.type.Value)
            {
                case "1":
                    Http.Status303SeeOther(new Uri("../shorturls.aspx", UriKind.Relative));
                    break;
                case "2":
                    Http.Status303SeeOther(new Uri("../moved.aspx", UriKind.Relative));
                    break;
            }
        }
    }
}