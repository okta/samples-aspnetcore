# Okta ASP.NET Core Web API Resource Server Example

This sample application authenticates requests against your Web API application, using access tokens.

The access tokens are obtained via the [Implicit Flow][].  As such, you will need to use one of our front-end samples with this project.  It is the responsibility of the front-end to authenticate the user, then use the obtained access tokens to make requests to this resource server.

## Prerequisites

Before running this sample, you will need the following:

* An Okta Developer Account, you can sign up for one at https://developer.okta.com/signup/.
* An Okta Application, configured for Single-Page App (SPA) mode. This is done from the Okta Developer Console and you can find instructions [here][OIDC SPA Setup Instructions].  When following the wizard, use the default properties.  They are are designed to work with our sample applications.
* One of our front-end sample applications to demonstrate the interaction with the resource server:
  * [Okta Angular Sample Apps][]
  * [Okta React Sample Apps][]
  * [Okta Vue Sample Apps][]

*Note:* This sample is pre-configured with an open CORS policy to make it easy to test with frontend projects. Make sure to restrict it to known origins in your production applications.

A typical resource-server requires a frontend and a backend application, so you will need to start each process:

## Running This Example

### Backend

Clone this repo and replace the okta configuration placeholders in the `appsettings.json` with your configuration values from the Okta Developer Console. 
You can see all the available configuration options in the [okta-aspnet GitHub](https://github.com/okta/okta-aspnet/blob/master/README.md).
For step-by-step instructions, visit the Okta [ASP.NET Web API quickstart].

> **NOTE:** This sample is using ASP.NET Core 2.2 which enforces HTTPS. This is a recommended practice for web applications. Check out [Enforce HTTPS in ASP.NET Core] for more details.

#### Visual Studio

If run this project in Visual Studio it will start the resource server on ports 5000 for HTTP and 44314 for HTTPS. You can change this configuration in the `launchSettings.json`.  You can browse to `https://localhost:44314/api/messages` to ensure it has started. If you get a 401 HTTP error, it indicates that the resource server is up. You will need to pass an access token to access the resource, which will be done by the front-end below.

#### dotnet CLI

If you run this project via the dotnet CLI it will start the resource server on ports 5000 for HTTP and 5001 for HTTPS. You can change this configuration in the `launchSettings.json`.  You can browse to `https://localhost:5001/api/messages` to ensure it has started. If you get a 401 HTTP error, it indicates that the resource server is up. You will need to pass an access token to access the resource, which will be done by the front-end below.


> **NOTE:** If you’ve never run an ASP.NET Core 2.x application before, you may notice a strange error page come up warning you that the site is potentially unsafe.
This is because ASP.NET Core creates an HTTPS development certificate for you as part of the first-run experience, but it still needs to be trusted. You can ignore the warning by clicking on Advanced and telling the browser that it’s okay to visit this site even though there is no certificate for it. Or you can trust the certificate to get rid of this warning, check out [Configuring HTTPS in ASP.NET Core across different platforms] for more details.

### Front-end

If you want to use one of our front-end samples, open a new terminal window and run the [front-end sample project of your choice](Prerequisites).  
Make sure to update the resource server URI configuration to use your configured resource server port (for example `https://localhost:5001`). 
Once the front-end sample is running, you can navigate to http://localhost:8080 in your browser and log in to the front-end application.  Once logged in, you can navigate to the "Messages" page to see the interaction with the resource server.


[Implicit Flow]: https://developer.okta.com/authentication-guide/implementing-authentication/implicit
[Okta Angular Sample Apps]: https://github.com/okta/samples-js-angular
[Okta Vue Sample Apps]: https://github.com/okta/samples-js-vue
[Okta React Sample Apps]: https://github.com/okta/samples-js-react
[OIDC SPA Setup Instructions]: https://developer.okta.com/authentication-guide/implementing-authentication/implicit#1-setting-up-your-application
[ASP.NET Core Web API quickstart]: https://developer.okta.com/quickstart/#/widget/dotnet/aspnetcore
[Enforce HTTPS in ASP.NET Core]: https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-2.2&tabs=visual-studio
[Configuring HTTPS in ASP.NET Core across different platforms]:https://devblogs.microsoft.com/aspnet/configuring-https-in-asp-net-core-across-different-platforms/