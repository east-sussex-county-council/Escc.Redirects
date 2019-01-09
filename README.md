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

You will need to use handlers that update the HTTP response when a redirect is matched. For projects targeting .NET Framework, these are provided in the `Escc.Redirects.Handlers` project.

## Managing redirects in SQL Server 
To use the default `SqlServerRedirectMatcher` you need to create a database using the script in `SqlServer.sql` and configure the connection in `web.config`:

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
