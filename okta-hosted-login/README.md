# ASP.NET Core & Okta-Hosted Login Page Example

This example shows you how to use the `Okta.AspNetCore` library to log in a user. The user's browser is first redirected to the Okta-hosted login page. After the user authenticates, they are redirected back to your application. ASP.NET Core automatically populates `HttpContext.User` with the information Okta sends back about the user.

## Prerequisites

Before running this sample, you will need the following:

* An Okta Developer Account, you can sign up for one at https://developer.okta.com/signup/.
* An Okta Application, configured for Web mode. This is done from the Okta Developer Console and you can find instructions [here][OIDC Web Application Setup Instructions].  When following the wizard, use the default properties.  They are designed to work with our sample applications.


## Running This Example

Clone this repo and replace the okta configuration placeholders in the `appsettings.json` with your configuration values from the Okta Developer Console. 
You can see all the available configuration options in the [okta-aspnet GitHub](https://github.com/okta/okta-aspnet/blob/master/README.md).
For step-by-step instructions, visit the Okta [Sign Users in to Your Web Application guide]. The guide will walk you through adding Okta login to your ASP.NET application.

> **NOTE:** This sample is using ASP.NET Core 2.2 which enforces HTTPS. This is a recommended practice for web applications. Check out [Enforce HTTPS in ASP.NET Core] for more details.

#### Visual Studio

If run this project in Visual Studio it will start the web application on ports 5000 for HTTP and 44314 for HTTPS. You can change this configuration in the `launchSettings.json`. Make sure to [update your Okta Application] in your Developer Console with the correct Base URI (for example https://localhost:44314).

#### dotnet CLI

If you run this project via the dotnet CLI it will start the web application on ports 5000 for HTTP and 5001 for HTTPS. You can change this configuration in the `launchSettings.json`. Make sure to [update your Okta Application] in your Developer Console with the correct Base URI (for example https://localhost:5001).  

> **NOTE:** If you’ve never run an ASP.NET Core 2.x application before, you may notice a strange error page come up warning you that the site is potentially unsafe.
This is because ASP.NET Core creates an HTTPS development certificate for you as part of the first-run experience, but it still needs to be trusted. You can ignore the warning by clicking on Advanced and telling the browser that it’s okay to visit this site even though there is no certificate for it. Or you can trust the certificate to get rid of this warning, check out [Configuring HTTPS in ASP.NET Core across different platforms] for more details.

If you see a home page that allows you to login, then things are working!  Clicking the **Log in** link will redirect you to the Okta hosted sign-in page.

You can login with the same account that you created when signing up for your Developer Org, or you can use a known username and password from your Okta Directory.

**Note:** If you are currently using your Developer Console, you already have a Single Sign-On (SSO) session for your Org.  You will be automatically logged into your application as the same user that is using the Developer Console.  You may want to use an incognito tab to test the flow from a blank slate.

[OIDC Middleware Library]: https://github.com/okta/okta-aspnet
[Authorization Code Flow]: https://developer.okta.com/authentication-guide/implementing-authentication/auth-code
[OIDC Web Application Setup Instructions]: https://developer.okta.com/authentication-guide/implementing-authentication/auth-code#1-setting-up-your-application
[ASP.NET MVC quickstart]:https://developer.okta.com/quickstart/#/okta-sign-in-page/dotnet/aspnetcore
[Enforce HTTPS in ASP.NET Core]: https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-2.2&tabs=visual-studio
[Configuring HTTPS in ASP.NET Core across different platforms]:https://devblogs.microsoft.com/aspnet/configuring-https-in-asp-net-core-across-different-platforms/
[Sign Users in to Your Web Application guide]: https://developer.okta.com/guides/sign-into-web-app/aspnet/before-you-begin/
[update your Okta Application]: https://developer.okta.com/guides/sign-into-web-app/aspnet/create-okta-application/