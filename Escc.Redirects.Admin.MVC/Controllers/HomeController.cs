using Escc.Redirects.Admin.MVC.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Escc.Redirects.Admin.MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // Return the Index view
            return View();
        }

        public ActionResult ShortUrls()
        {
            // Create and populate a Datatable
            DataTable table = GetRedirectsTable(1);
            // Return the ShortUrls view and pass it the table
            return View(table);
        }

        public ActionResult Moved()
        {
            // Create and populate a Datatable
            DataTable table = GetRedirectsTable(2);
            // Return the Moved view and pass it the table
            return View(table);
        }

        public ActionResult Edit(int id)
        {
            // Create a single Redirect
            Redirect edit = GetRedirect(id);
            // Return the Edit view and pass it the redirect
            return View(edit);
        }

        public ActionResult Delete(int id)
        {
            // Create a single Redirect
            Redirect delete = GetRedirect(id);
            // Return the Delete view and pass it the redirect
            return View(delete);
        }

        public ActionResult Add(int id)
        {
            // Create a single Redirect with null values
            Redirect add = new Redirect(0, null, id, null, null);
            // Return the Add view and pass it the null Redirect
            return View(add);
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

        public DataTable GetRedirectsTable(int type)
        {
            // Create a fresh DataTable
            DataTable table = new DataTable();
            if (type == 1)
            {
                table.Columns.Add("Short URL", typeof(string));
            }
            else
            {
                table.Columns.Add("Moved URL", typeof(string));
            }
            table.Columns.Add("Redirects To", typeof(string));
            table.Columns.Add("Comment", typeof(string));
            table.Columns.Add("Edit", typeof(HtmlString));
            table.Columns.Add("Delete", typeof(HtmlString));

            // Connect to the Database using the usp_Redirect_SelectByType stored procedure and passing the redirect type
            using (var reader = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["RedirectsReader"].ConnectionString, "usp_Redirect_SelectByType", new SqlParameter("@type", type), new SqlParameter("@sort", "pattern")))
            {
                while (reader.Read())
                {
                    // Populate the Datatable
                    HtmlString edit = new HtmlString("<a href=edit?Id=" + reader.GetValue(0) + ">edit</a>");
                    HtmlString delete = new HtmlString("<a href=delete?Id=" + reader.GetValue(0) + ">delete</a>");
                    table.Rows.Add(reader.GetValue(1).ToString(), reader.GetValue(2).ToString(), reader.GetValue(3).ToString(), edit, delete);
                }
            }
            return table;
        }


    }
}