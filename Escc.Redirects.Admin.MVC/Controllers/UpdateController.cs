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

namespace Escc.Redirects.Admin.MVC.Controllers
{
    public class UpdateController : Controller
    {

        public ActionResult Edit(int id, int type, string pattern, string destination, string comment)
        {
            // Take the passed parameters and create a Redirect object
            Redirect edit = new Redirect(id, pattern, type, comment, destination);
            // pass the redirect to the updateDatabase method, and specify false that it is not a new redirect
            UpdateDatabase(edit, false);
            // Return the Success view, and pass it the redirect to display
            return View("Success", edit);
        }
        public ActionResult Delete(int id)
        {
            // Create a null Redirect with the passed ID
            Redirect delete = new Redirect(id, null, 0, null, null);
            // Connect to the database and Delete the Redirect
            //SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["RedirectsWriter"].ConnectionString, CommandType.StoredProcedure, "usp_Redirect_Delete", new SqlParameter("@redirectId", id));
            // Return the Success view and pass it the redirect to display
            return View("Success", delete);
        }

        public ActionResult Add(int type, string pattern, string destination, string comment)
        {
            // Take the passed paraneters and create a Redirect Object with a null ID
            Redirect add = new Redirect(0, pattern, type, comment, destination);
            // pass the redirect to the UpdateDatabase method and specify true that it is a new redirect
            UpdateDatabase(add, true);
            // Return the Success view and pass it the redirect to display
            return View("Success", add);
        }

        public ActionResult Success()
        {
            // Return the Success view
            return View();
        }

        public void UpdateDatabase(Redirect redirect, Boolean newEntry)
        {
            // bug fix
            // Strip the protocal and domain if entered by the user
            // Use try catch in case user types in a relative path
            string absolute = "";
            try
            {
                Uri from = new Uri(redirect.Pattern);
                absolute = from.AbsolutePath;
            }
            catch (Exception)
            {
                absolute = redirect.Pattern;
            }

            // Create an array of SqlParameters and populate the data from the passed Redirect Object.
            var values = new SqlParameter[5];
            values[0] = new SqlParameter("@pattern", absolute);
            values[1] = new SqlParameter("@destination", redirect.Destination);
            values[2] = new SqlParameter("@type", redirect.Type);
            values[3] = new SqlParameter("@comment", redirect.Comment);

            // If the redirect is not a new redirect
            if (newEntry == false)
            {
                // add an extra parameter for the ID
                values[4] = new SqlParameter("@redirectId", redirect.RedirectID);
                // connect to the database and update the Redirect
                //SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["RedirectsWriter"].ConnectionString, CommandType.StoredProcedure, "usp_Redirect_Update", values);
            }
            // if the redirect is a new redirect
            else
            {
                // connect to the database and insert the new redirect
                //SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["RedirectsWriter"].ConnectionString, CommandType.StoredProcedure, "usp_Redirect_Insert", values);
            }
        }

    }
}