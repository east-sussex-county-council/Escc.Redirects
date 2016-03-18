using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Xml.Linq;
using Escc.ActiveDirectory;
using EsccWebTeam.Data.Web;
using Microsoft.ApplicationBlocks.Data;

namespace Escc.Redirects.Admin
{
    public partial class ShortUrls : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var sort = "pattern";
            if (Request.QueryString["sort"] == "destination") sort = "destination";
            if (Request.QueryString["sort"] == "date") sort = "date";

            sortLinks.Controls.Add(new LiteralControl((sort == "destination" || sort == "date") ? "<li><a href=\"shorturls.aspx\">Short URL</a></li>" : "<li>Short URL</li>"));
            sortLinks.Controls.Add(new LiteralControl((sort == "destination") ? "<li>Where it redirects to</li>" : "<li><a href=\"shorturls.aspx?sort=destination\">Where it redirects to</a></li>"));
            sortLinks.Controls.Add(new LiteralControl((sort == "date") ? "<li>Most recent first</li>" : "<li><a href=\"shorturls.aspx?sort=date\">Most recent first</a></li>"));

            var editConfig = XElement.Load(Server.MapPath(@".\edit\web.config"));
            var allowed = editConfig.Descendants("authorization").Descendants("allow").Attributes("roles").Select( attr => attr.Value).ToList();

            var permissions = new LogonIdentityGroupMembershipChecker();
            editTable.Visible = (allowed.Count > 0 && permissions.UserIsInGroup(allowed));
            add.Visible = editTable.Visible;
            viewTable.Visible = !editTable.Visible;

            var table = editTable.Visible ? editTable : viewTable;
            using (var reader = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["RedirectsReader"].ConnectionString, "usp_Redirect_SelectByType", new SqlParameter("@type", 1), new SqlParameter("@sort", sort)))
            {
                table.DataSource = reader;
                table.DataBind();
            }
        }

        protected void AddRedirect_Click(object sender, EventArgs e)
        {
            Http.Status303SeeOther(new Uri("edit/edit.aspx?type=1", UriKind.Relative));
        }
    }
}