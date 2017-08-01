using Escc.Redirects.Admin.MVC.Authorize;
using Escc.Redirects.Admin.MVC.Models;
using Escc.Redirects.Admin.MVC.Models.DataModels;
using Escc.Redirects.Admin.MVC.Models.ViewModels;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Escc.Redirects.Admin.MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // Return the Index view
            return View();
        }

        [CustomAuthorize()]
        [Route("ViewRedirects", Name = "ViewRedirects")]
        public ActionResult ViewRedirects(int Type, int ShowResultsFrom = 0, string Query = "Null", string Alert = "")
        {
            var model = new RedirectsViewModel();

            if(Query != "Null")
            {
                model.Search = true;
                model.Query = Query;
            }
            else
            {
                model.Total = GetTotalRedirects(Type);
            }

            if (ShowResultsFrom < 0)
            {
                ShowResultsFrom = 0;
            }
            else if (ShowResultsFrom > model.Total)
            {
                ShowResultsFrom = model.Total - 500;
            }

            model.Alert = Alert;
            model.ShowResultsFrom = ShowResultsFrom;
            model.Type = Type;
            switch (Type)
            {
                case 1:
                    model.RedirectTypeString = "Short Urls";
                    model.TypeInfoText = "These short URLs should be used for publicity. Each one is preceded by \"eastsussex.gov.uk / \". For example \"arts\" becomes \"eastsussex.gov.uk/arts\".";
                    break;
                case 2:
                    model.RedirectTypeString = "Moved Urls";
                    model.TypeInfoText = " These pages and sections on eastsussex.gov.uk have moved, but visitors are redirected to the new location.";
                    break;
            }

            try
            {
                model.RedirectsTable.Table = GetRedirectsTable(Type, ShowResultsFrom, Query);
            }
            catch (Exception error)
            {
                model.ErrorMessage = error.Message;
                throw;
            }
  
            return View(model);
        }

        public Redirect GetRedirect(int id)
        {
            // Create a new null Redirect
            Redirect redirect = new Redirect(0, null, 0, null, null);
            // Connect to the Database and use the usp_Redirect_Select stored procedure and id to get a single Redirect.
            using (var reader = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["RedirectsWriter"].ConnectionString, CommandType.StoredProcedure, "usp_Redirect_Select", new SqlParameter("@redirectId", id)))
            {
                while (reader.Read())
                {
                    redirect = new Redirect(Int32.Parse(reader["RedirectId"].ToString()), reader["Pattern"].ToString(), Int32.Parse(reader["Type"].ToString()), reader["Comment"].ToString(), reader["Destination"].ToString());
                }
            }
            return redirect;
        }

        public DataTable GetRedirectsTable(int Type, int ShowResultsFrom = 0, string Query = "Null")
        {
            // Create a fresh DataTable
            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("URL", typeof(string));
            table.Columns.Add("Destination", typeof(string));
            table.Columns.Add("Comment", typeof(string));
            table.Columns.Add("Edit", typeof(HtmlString));
            table.Columns.Add("Delete", typeof(HtmlString));

            var ResultsList = new List<TableRedirectModel>();
            // Connect to the Database using the usp_Redirect_SelectByType stored procedure and passing the redirect type
            using (var reader = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["RedirectsReader"].ConnectionString, "usp_Redirect_SelectByType", new SqlParameter("@type", Type), new SqlParameter("@sort", "pattern")))
            {
                while (reader.Read())
                {
                    ResultsList.Add(new TableRedirectModel(int.Parse(reader.GetValue(0).ToString()), reader.GetValue(1).ToString(), reader.GetValue(2).ToString(), reader.GetValue(3).ToString()));
                }
            }
            if(Query == "Null")
            {
                ResultsList = ResultsList.OrderByDescending(x => x.ID).ToList();
                ResultsList.RemoveRange(0, ShowResultsFrom);
                var ResultsToShow = ResultsList.Take(500);
                foreach (var model in ResultsToShow)
                {
                    // Populate the Datatable
                    var edit = new HtmlString(string.Format("<button type=\"button\" class=\"btn btn-warning btn-sm\" data-toggle=\"modal\" data-target=\".{0}\">Edit</button>", string.Format("Edit{0}",model.ID)));
                    var delete = new HtmlString(string.Format("<button type=\"button\" class=\"btn btn-danger btn-sm\" data-toggle=\"modal\" data-target=\".{0}\">Delete</button>", string.Format("Delete{0}", model.ID)));
                    table.Rows.Add(model.ID, model.URL, model.Destination, model.Comment, edit, delete);
                }

            }
            else
            {
                ResultsList = ResultsList.OrderByDescending(x => x.ID).ToList();
                foreach (var model in ResultsList)
                {
                    if(model.Destination.ToLower().Contains(Query.ToLower()) || model.URL.ToLower().Contains(Query.ToLower()))
                    {
                        // Populate the Datatable
                        var edit = new HtmlString(string.Format("<button type=\"button\" class=\"btn btn-warning btn-sm\" data-toggle=\"modal\" data-target=\".{0}\">Edit</button>", string.Format("Edit{0}", model.ID)));
                        var delete = new HtmlString(string.Format("<button type=\"button\" class=\"btn btn-danger btn-sm\" data-toggle=\"modal\" data-target=\".{0}\">Delete</button>", string.Format("Delete{0}", model.ID)));
                        table.Rows.Add(model.ID, model.URL, model.Destination, model.Comment, edit, delete);
                    }
                }
            }
            return table;
        }

        public int GetTotalRedirects(int Type)
        {
            var Total = 0;
            // Connect to the Database using the usp_Redirect_SelectByType stored procedure and passing the redirect type
            using (var reader = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["RedirectsReader"].ConnectionString, "usp_Redirect_SelectByType", new SqlParameter("@type", Type), new SqlParameter("@sort", "pattern")))
            {
                while (reader.Read())
                {
                    Total++;
                }
            }
            return Total;
        }
    }
}