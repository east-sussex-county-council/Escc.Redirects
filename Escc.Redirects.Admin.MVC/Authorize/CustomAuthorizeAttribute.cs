using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Escc.Redirects.Admin.MVC.Authorize
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        // Based on this post here http://stackoverflow.com/questions/11539217/specifying-roles-in-web-config-of-an-asp-net-mvc-application
        // As MVC uses routing the traditional way of providing authorization for specific parts of a website no longer apply.
        // In fact using authorization in a web.config alone has the potential to create security loopholes in an MVC application, especially for custom defined routes.
        // To authorize specific parts of an MVC site you have to provide the [Authorize} attribute on the controller methods.
        // This class provides a way to use an [Authorize] attribute and still be able to specify roles in the web.config, keeping that data hidden.

        public CustomAuthorizeAttribute(params string[] roleKeys)
        {
            List<string> roles = new List<string>(roleKeys.Length);
            var allRoles = (NameValueCollection)ConfigurationManager.GetSection("roles");
            foreach (var roleKey in roleKeys)
            {
                roles.Add(allRoles[roleKey]);
            }
            this.Roles = string.Join(",", roles);
        }
    }
}