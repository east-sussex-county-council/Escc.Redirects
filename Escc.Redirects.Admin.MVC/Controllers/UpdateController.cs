using Escc.Redirects.Admin.MVC.Authorize;
using Escc.Redirects.Admin.MVC.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Escc.Redirects.Admin.MVC.Controllers
{
    public class UpdateController : Controller
    {
        [CustomAuthorize()]
        public ActionResult Edit(int RedirectID, int Type, string Pattern, string Destination, string Comment)
        {
            // Take the passed parameters and create a Redirect object
            Redirect edit = new Redirect(RedirectID, Pattern, Type, Comment, Destination);
            // pass the redirect to the updateDatabase method, and specify false that it is not a new redirect
            UpdateDatabase(edit, false);
            // Return the Success view, and pass it the redirect to display
            return RedirectToRoute("ViewRedirects", new { Type = Type, Alert = string.Format("Redirect {0} for: \"{1}\" has been updated.", RedirectID, Pattern) });
        }

        [CustomAuthorize()]
        public ActionResult Delete(int Type, int RedirectID, string Pattern)
        {
            // Create a null Redirect with the passed ID
            Redirect delete = new Redirect(RedirectID, null, 0, null, null);
            // Connect to the database and Delete the Redirect
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["RedirectsWriter"].ConnectionString, CommandType.StoredProcedure, "usp_Redirect_Delete", new SqlParameter("@redirectId", RedirectID));
            // Return the Success view and pass it the redirect to display
            return RedirectToRoute("ViewRedirects", new { Type = Type, Alert = string.Format("Redirect {0} for: \"{1}\" has been deleted.", RedirectID, Pattern) });
        }

        [CustomAuthorize()]
        public ActionResult Add(int Type, string Pattern, string Destination, string Comment)
        {
            // Take the passed paraneters and create a Redirect Object with a null ID
            Redirect add = new Redirect(0, Pattern, Type, Comment, Destination);
            // pass the redirect to the UpdateDatabase method and specify true that it is a new redirect
            UpdateDatabase(add, true);
            return RedirectToRoute("ViewRedirects", new {Type = Type, Alert = string.Format("The Redirect for: {0} has been created.", Pattern) });
        }

        public void UpdateDatabase(Redirect redirect, Boolean newEntry)
        {
            // bug fix #1
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

            // bug fix #2
            // There can be trouble redirecting using URLs with a trailing /
            // Use TrimEnd() to remove any trailing / characters from the pattern and destination.
            absolute = absolute.TrimEnd('/');
            redirect.Destination = redirect.Destination.TrimEnd('/');

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
                SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["RedirectsWriter"].ConnectionString, CommandType.StoredProcedure, "usp_Redirect_Update", values);
            }
            // if the redirect is a new redirect
            else
            {
                // connect to the database and insert the new redirect
                SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["RedirectsWriter"].ConnectionString, CommandType.StoredProcedure, "usp_Redirect_Insert", values);
            }
        }

    }
}