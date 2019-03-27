# Escc.Redirects

Provide a way to redirect URLs when the original resource is no longer available.

- if a URL matches an Umbraco page, it's not redirected
- if a URL matches an `*.aspx` file on disk, it's not redirected
- if a URL is too long for `HostingEnvironment.MapPath` to handle, it's not redirected
- if a page is missing but a matching redirect is found, the redirect is executed
- querystrings on URLs are preserved when redirecting, unless the new URL already has a querystring
- if a page is missing and no matching redirect is found, it falls through to standard IIS/ASP.NET behaviour for 404 responses 

Errors are reported to [Exceptionless](https://github.com/exceptionless/Exceptionless). 

## Use 404 pages to implement redirects
To implement redirects using this project add code to a custom error page for 404 responses to check for redirects before returning a 404 response.
See the documentation for [Escc.EastSussexGovUK](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK) for an example of how to configure a 404 page that checks for redirects.

### ASP.NET Core applications
You will need to inject and use a redirect matcher and redirect handlers before updating the response headers. This requires the `Microsoft.AspNetCore.Diagnostics` package to be installed.

Startup:

    public void ConfigureServices(IServiceCollection services)
    {
		...
        services.Configure<RedirectSettings>(options => configuration.GetSection("Escc.Redirects").Bind(options));
        services.TryAddSingleton<INotFoundRequestPathResolver, NotFoundRequestPathResolver>();
        services.TryAddSingleton<IRedirectMatcher, SqlServerRedirectMatcher>();
        services.TryAddSingleton<IConvertToAbsoluteUrlHandler, ConvertToAbsoluteUrlHandler>();
        services.TryAddSingleton<IPreserveQueryStringHandler, PreserveQueryStringHandler>();
		...
	}

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
		...
		app.UseStatusCodePagesWithReExecute("/HttpStatus/Status{0}");
		...
	}

Controller:

	[AllowAnonymous]
    public class HttpStatusController : Controller
    {
        private readonly INotFoundRequestPathResolver _notFoundRequestPathResolver;
        private readonly IRedirectMatcher _redirectMatcher;
        private readonly IConvertToAbsoluteUrlHandler _convertToAbsoluteUrlHandler;
        private readonly IPreserveQueryStringHandler _preserveQueryStringHandler;

        public HttpStatusController(INotFoundRequestPathResolver notFoundRequestPathResolver, IRedirectMatcher redirectMatcher, IConvertToAbsoluteUrlHandler convertToAbsoluteUrlHandler, IPreserveQueryStringHandler preserveQueryStringHandler)
        {
            _notFoundRequestPathResolver = notFoundRequestPathResolver;
            _redirectMatcher = redirectMatcher;
            _convertToAbsoluteUrlHandler = convertToAbsoluteUrlHandler;
            _preserveQueryStringHandler = preserveQueryStringHandler;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Status404()
        {
            var requestFeature = HttpContext.Features.Get<IHttpRequestFeature>();
            var absoluteRequestedUrl = new Uri(new Uri(Request.GetDisplayUrl()), new Uri(requestFeature.RawTarget, UriKind.Relative));
            absoluteRequestedUrl = _notFoundRequestPathResolver?.NormaliseRequestedPath(absoluteRequestedUrl);

            var redirect = _redirectMatcher?.MatchRedirect(absoluteRequestedUrl);
            if (redirect != null)
            {
                redirect = _convertToAbsoluteUrlHandler?.HandleRedirect(redirect);
                redirect = _preserveQueryStringHandler?.HandleRedirect(redirect);
                Response.Headers.Add("Location", redirect.DestinationUrl.ToString());
                return new StatusCodeResult(redirect.StatusCode);
            }

			// No redirect matched - return a 404 view here
			return View();
		}
	}

### ASP.NET applications running the .NET Framework

You will need to use handlers that update the HTTP response when a redirect is matched. For projects targeting .NET Framework, these are provided in the `Escc.Redirects.Handlers` project.

Controller:

	using Escc.Redirects;
	using Escc.Redirects.Handlers;

	...

    public async Task<ActionResult> NotFound()
    {
        var requestedPath = new NotFoundRequestPathResolver().NormaliseRequestedPath(Request.Url);

        var matchers = new IRedirectMatcher[] { new SqlServerRedirectMatcher(ConfigurationManager.ConnectionStrings["RedirectsReader"].ConnectionString) };
        var handlers = new IRedirectHandler[] { new ConvertToAbsoluteUrlHandler(), new PreserveQueryStringHandler(), new DebugInfoHandler(), new GoToUrlHandler() };

        foreach (var matcher in matchers)
        {
            var redirect = matcher.MatchRedirect(requestedUrl);
            if (redirect != null)
            {
                foreach (var handler in handlers)
                {
                    redirect = handler.HandleRedirect(redirect);
                }
                return null;
            }
        }

        return View();
	}



## Managing redirects in SQL Server 
To use the default `SqlServerRedirectMatcher` you need to create a database using the script in `SqlServer.sql` and configure the connection string.

For .NET Core in (for development) `secrets.json`:

	{
	  "Escc.Redirects": {
	    "ConnectionString": "xxx"
	  }
	}

For .NET Framework in `web.config`:

	<configuration>
		<connectionStrings>
		    <add name="RedirectsReader" connectionString="xxx" />
	  	</connectionStrings>
	</configuration>

An example 301 redirect in SQL Server from `/page1.aspx` to `/page2.aspx`:

	INSERT INTO Redirect (Pattern, Destination, Type, Comment) VALUES ('page1.aspx', '/page2.aspx', 2, 'Example')

## Redirecting static files
To redirect requests for a file extension which is not ordinarily processed by ASP.NET you need to add a handler in `web.config`. For IIS6 or IIS7+ in Classic mode: 

	<configuration>
	  <system.web>
	    <httpHandlers>
	      <add path="*.htm" verb="*" type="System.Web.UI.PageHandlerFactory" validate="true" />
	    </httpHandlers>
	  </system.web>
	</configuration>

For IIS7+ in Integrated mode:

	<configuration>
	 <system.web>
	    <compilation targetFramework="4.0">
	      <buildProviders>
	        <add extension=".htm" type="System.Web.Compilation.PageBuildProvider" />
	      </buildProviders>
	    </compilation>
	  </system.web>
	  <system.webServer>
	    <handlers>
	      <add name="PageHandlerFactory-Integrated-htm" path="*.htm" verb="GET,HEAD,POST,DEBUG" type="System.Web.UI.PageHandlerFactory" resourceType="Unspecified" requireAccess="Script" preCondition="integratedMode" />
	    </handlers>
	  </system.webServer>
	</configuration>
