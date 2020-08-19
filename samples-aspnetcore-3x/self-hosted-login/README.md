# ASP.NET Core & Self-Hosted Login Page Example

This example shows you how to use the [Okta ASP.NET Core SDK] to sign in a user. The user's browser is first redirected to the self-hosted sign-in page on your ASP.NET Core application. Once the user is successfully authenticated via Okta, ASP.NET Core automatically populates `HttpContext.User` with the information Okta sends back about the user.

## Prerequisites

Before running this sample, you will need the following:

* An Okta Developer Account, you can sign up for one at https://developer.okta.com/signup/.
* An Okta Application, configured for Web mode. This is done from the Okta Developer Console and you can find instructions [here][OIDC Web Application Setup Instructions].  When following the wizard, use the default properties.  They are designed to work with our sample applications.

## Running This Example

### Clone this repository

```git clone https://github.com/okta/samples-aspnetcore.git```

### Run the web application

Run the example with your preferred tool and write down the port of your web application to configure Okta afterwards.

> **NOTE:** This sample is using ASP.NET Core 3.1 which enforces HTTPS. This is a recommended practice for web applications. Check out [Enforce HTTPS in ASP.NET Core] for more details.

> Because of recent changes in [Set-Cookie behavior (SameSite)](https://web.dev/samesite-cookies-explained) this code will only work properly if it's configured to use https. Check out [Work with SameSite cookies in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/samesite?view=aspnetcore-3.1) for more details.

#### Run the web application from Visual Studio

If you run this project in Visual Studio it will start the web application on port 44314 using HTTPS. You can change this configuration in the `launchSettings.json` in the Properties folder.

#### Run the web application from dotnet CLI

If you run this project via the dotnet CLI it will start the web application on port 5001 using HTTPS. You can change this configuration in the `launchSettings.json` in the Properties folder.

Navigate to the folder where the project file is located and type the following:

```dotnet run```

#### Trust the local dev certificate if necessary

If you’ve never run an ASP.NET Core 3.x application before, you may notice a strange error page come up warning you that the site is potentially unsafe.
This is because ASP.NET Core creates an HTTPS development certificate for you as part of the first-run experience, but it still needs to be trusted. You can ignore the warning by clicking on Advanced and telling the browser that it’s okay to visit this site even though there is no certificate for it. Or you can trust the certificate to get rid of this warning, check out [Configuring HTTPS in ASP.NET Core across different platforms] for more details.

### Add the correct configuration to the Okta Developer Console (including the port you just found in [Run the web application](#run-the-web-application))

Go to your [Okta Developer Console] and update the following parameters in your Okta Web Application configuration:
* **Login redirect URI** - for example, `https://localhost:5001/authorization-code/callback`
* **Logout redirect URI** - for example, `https://localhost:5001/signout/callback`

Also, enable CORS to allow your self-hosted page to make an XHR to the Okta API with the Okta session cookie. To do so, click **API > Trusted Origins** in your [Okta Developer Console], and add your web application’s base URL (for example, `https://localhost:5001/`) as a **Trusted Origin**.

For step-by-step instructions, visit the Okta [Sign Users in to Your Web Application guide] which will show you how to sign users in using Okta and, [Sign Users Out guide] which will show you how to sign users out of your app and out of Okta.

### Add the same configuration to the sample's appsettings

Replace the okta configuration placeholders in the `appsettings.json` with your configuration values from the [Okta Developer Console]. 
You can see all the available configuration options in the [okta-aspnet GitHub](https://github.com/okta/okta-aspnet/blob/master/docs/aspnetcore-mvc.md#configuration-reference).
For step-by-step instructions, visit the Okta [Sign Users in to Your Web Application guide]. The guide will walk you through adding Okta sign-in to your ASP.NET application.

### Run again and try to sign in

Click the **Sign In** link in the Home page and it will redirect you to the self-hosted sign-in page.

You can sign in with the same account that you created when signing up for your Developer Org, or you can use a known username and password from your Okta Directory.

**Note:** If you are currently using your Developer Console, you already have a Single Sign-On (SSO) session for your Org.  You will be automatically signed into your application as the same user that is using the Developer Console.  You may want to use an incognito tab to test the flow from a blank slate.

[Okta ASP.NET Core SDK]: https://github.com/okta/okta-aspnet
[OIDC Web Application Setup Instructions]: https://developer.okta.com/authentication-guide/implementing-authentication/auth-code#1-setting-up-your-application
[Enforce HTTPS in ASP.NET Core]: https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-2.2&tabs=visual-studio
[Configuring HTTPS in ASP.NET Core across different platforms]:https://devblogs.microsoft.com/aspnet/configuring-https-in-asp-net-core-across-different-platforms/
[Sign Users in to Your Web Application guide]: https://developer.okta.com/guides/sign-into-web-app/aspnet/before-you-begin/
[Sign Users Out guide]: https://developer.okta.com/guides/sign-users-out/aspnetcore/before-you-begin/
[Okta Developer Console]: https://login.okta.com