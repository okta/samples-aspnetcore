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

A typical resource-server requires a frontend and a backend application, so you will need to start each process as described in the next section.

## Running This Example

### Backend

Run the example with your preferred tool and write down the port of your Web API application to configure Okta afterwards.

> **NOTE:** Although we highly recommend using HTTPS in production, we are using HTTP here to be compatible with our [SPA samples](#Prerequisites) that expect a protected endpoint on HTTP://localhost:8000/api/messages. 

#### Run the Web API application from Visual Studio

If you run this project in Visual Studio it will start the resource server on port 8000 using HTTP. You can change this configuration in the `launchSettings.json` in the Properties folder.  
You can browse to `http://localhost:8000/api/messages` to ensure it has started. If you get a 401 HTTP error, it indicates that the resource server is up. You will need to pass an access token to access the resource, which will be done by the front-end below.

#### Run the Web API application from dotnet CLI

If you run this project via the dotnet CLI it will start the resource server on port 8000 using HTTP. You can change this configuration in the `launchSettings.json` in the Properties folder.

Navigate to the folder where the project file is located and type the following:

```dotnet run```

You can browse to `http://localhost:8000/api/messages` to ensure it has started. If you get a 401 HTTP error, it indicates that the resource server is up. You will need to pass an access token to access the resource, which will be done by the front-end below.

### Add your Okta configuration to the sample's appsettings

Replace the okta configuration placeholders in the `appsettings.json` with your configuration values from the [Okta Developer Console]. 
You can see all the available configuration options in the [okta-aspnet GitHub](https://github.com/okta/okta-aspnet/blob/master/docs/aspnetcore-webapi.md#configuration-reference).
For step-by-step instructions, visit the Okta [Protect your API endpoints guide]. The guide will walk you through adding Okta authentication to your API endpoints.

### Front-end

If you want to use one of our front-end samples, open a new terminal window and run the [front-end sample project of your choice](#Prerequisites).  
Make sure to update the resource server URI configuration to use your configured resource-server port (for example `http://localhost:8000`). 
Once the front-end sample is running, you can navigate to `http://localhost:8080` in your browser and sign in to the front-end application.  Once signed in, you can navigate to the "Messages" page to see the interaction with the resource server.


[Implicit Flow]: https://developer.okta.com/authentication-guide/implementing-authentication/implicit
[Okta Angular Sample Apps]: https://github.com/okta/samples-js-angular
[Okta Vue Sample Apps]: https://github.com/okta/samples-js-vue
[Okta React Sample Apps]: https://github.com/okta/samples-js-react
[OIDC SPA Setup Instructions]: https://developer.okta.com/authentication-guide/implementing-authentication/implicit#1-setting-up-your-application
[Enforce HTTPS in ASP.NET Core]: https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-2.2&tabs=visual-studio
[Configuring HTTPS in ASP.NET Core across different platforms]:https://devblogs.microsoft.com/aspnet/configuring-https-in-asp-net-core-across-different-platforms/
[Protect your API endpoints guide]: https://developer.okta.com/guides/protect-your-api/aspnetcore/before-you-begin/