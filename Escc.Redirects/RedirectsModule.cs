using System;
using System.IO;
using System.Web;
using System.Web.Hosting;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace Escc.Redirects
{
    /// <summary>
    /// An HTTP module to redirect URLs which are not found locally
    /// </summary>
    public class RedirectsModule : IHttpModule
    {
        /// <summary>
        /// You will need to configure this module in the Web.config file of your
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpModule Members

        public void Dispose()
        {
            //clean-up code here.
        }

        public void Init(HttpApplication context)
        {
            if (context == null) throw new ArgumentNullException("context");
            context.BeginRequest += new EventHandler(RedirectsModule_BeginRequest);
        }

        #endregion

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void RedirectsModule_BeginRequest(object sender, EventArgs e)
        {          
            try
            {     
                // If the requested path exists, do nothing
                if (File.Exists(HostingEnvironment.MapPath(HttpContext.Current.Request.Url.AbsolutePath)))
                {
                    return;
                }

                // Try to match the requested URL to a redirect and, if successful, run handlers for the redirect
                var matchers = new IRedirectMatcher[] {new SqlServerRedirectMatcher()};
                var handlers = new IRedirectHandler[] {new ConvertToAbsoluteUrlHandler(), new PreserveQueryStringHandler(), new GoToUrlHandler()};

                foreach (var matcher in matchers)
                {
                    var redirect = matcher.MatchRedirect(HttpContext.Current.Request.Url);
                    if (redirect != null)
                    {
                        foreach (var handler in handlers)
                        {
                            redirect = handler.HandleRedirect(redirect);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // If there's a problem, publish the error and continue
                ExceptionManager.Publish(ex);
            }
        }
    }
}
