# Escc.Redirects

Provide a way to redirect URLs when the original resource is no longer available.

- if a URL matches a Microsoft Content Management Server 2002 page, it's not redirected
- if a URL matches an `*.aspx` file on disk, it's not redirected
- if a page is missing but a matching redirect is found, the redirect is executed
- querystrings on URLs are preserved when redirecting, unless the new URL already has a querystring
- if a page is missing and no matching redirect is found, it falls through to standard IIS/ASP.NET behaviour for 404 responses 

To begin redirecting URLs you need to register the HTTP module in `web.config`. For IIS6 or IIS7+ in Classic mode: 

	<configuration>
	  <system.web>
	    <httpModules> 
	      <add name="RedirectsModule" type="Escc.Redirects.RedirectsModule" />
	    </httpModules>
	  </system.web>
	</configuration>

For IIS7+ in Integrated mode:

	<configuration>
	  <system.webServer>
	    <modules> 
	      <add name="RedirectsModule" type="Escc.Redirects.RedirectsModule" />
	    </modules>
	  </system.webServer>
	</configuration>

## Excluding specific patterns from redirection

Because an HTTP module executes early in the request pipeline, there may be something later which would handle a URL that might otherwise be redirected. For example, the request might be rewritten in the `Application_BeginRequest` event in `global.asax.cs`. 

You can configure paths to be ignored using comma-separated regular expressions in `web.config`:

	<configuration>
		<appSettings>
		    <add key="Escc.Redirects.IgnorePaths" value="page[abc].aspx,page[0-9].aspx" />
	  	</appSettings>
	</configuration>

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
