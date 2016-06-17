using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI;
using EsccWebTeam.Data.Web;
using EsccWebTeam.EastSussexGovUK.MasterPages;
using Microsoft.ApplicationBlocks.Data;

namespace Escc.Redirects.Admin.Edit
{
    public partial class EditPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var skinnable = Master as BaseMasterPage;
            if (skinnable != null)
            {
                skinnable.Skin = new CustomerFocusSkin(ViewSelector.CurrentViewIs(MasterPageFile));
            }

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
                            this.pattern.Text = "/" + reader["Pattern"].ToString();
                            this.destination.Text = reader["Destination"].ToString();

                            SelectRedirectType(reader["Type"].ToString());

                            this.comment.Text = reader["Comment"].ToString();
                        }
                    }

                    this.Title = Properties.Resources.EditRedirect;
                    this.h1.Text = Properties.Resources.EditRedirect;
                }
                catch (OverflowException) { }
                catch (FormatException) { }

            }
            else if (!IsPostBack && !String.IsNullOrEmpty(Request.QueryString["type"]))
            {
                SelectRedirectType(Request.QueryString["type"]);
            }

        }

        private void SelectRedirectType(string typeToSelect)
        {
            this.type.ClearSelection();
            var item = this.type.Items.FindByValue(typeToSelect);
            if (item != null) item.Selected = true;
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var redirect = new Redirect();
                redirect.RequestedUrl = new Uri(this.pattern.Text, UriKind.RelativeOrAbsolute);
                redirect.DestinationUrl = new Uri(this.destination.Text, UriKind.RelativeOrAbsolute);
                redirect.StatusCode = Int32.Parse(this.type.SelectedValue, CultureInfo.InvariantCulture);
                redirect.Comment = this.comment.Text;

                if (!String.IsNullOrEmpty(this.redirectId.Value))
                {
                    redirect.RedirectId = Int32.Parse(this.redirectId.Value, CultureInfo.InvariantCulture);
                    new SqlServerRedirectsRepository(ConfigurationManager.ConnectionStrings["RedirectsWriter"].ConnectionString).SaveRedirect(redirect);
                }
                else
                {
                    new SqlServerRedirectsRepository(ConfigurationManager.ConnectionStrings["RedirectsWriter"].ConnectionString).SaveRedirect(redirect);
                }

                // Go back to the list where the edited redirect will be shown
                switch (this.type.SelectedValue)
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
}