# ASP.NET Core & Embedded-Widget Login Page Example

## Introduction
> :grey_exclamation: The use of this Sample uses an SDK that requires usage of the Okta Identity Engine. 
This functionality is in general availability but is being gradually rolled out to customers. If you want
to request to gain access to the Okta Identity Engine, please reach out to your account manager. If you 
do not have an account manager, please reach out to oie@okta.com for more information.

This Sample Application will show you the best practices for integrating Authentication by embedding the 
Sign In Widget into your application. The Sign In Widget is powered by [Okta's Identity Engine](https://
developer.okta.com/docs/concepts/ie-intro/) and will adjust your user experience based on policies. 
Once integrated, you will be able to utilize all the features of Okta's Sign In Widget in your application.

For information and guides on how to build your app with this sample, please take a look at the [{FRAMEWORK} 
guides for Embedded Sign In Widget Authentication](link to DevDoc SBS guide)

## Installation & Running The App

### Prerequisites

Before running this sample, you will need the following:

* An Okta Developer Account, you can sign up for one at https://developer.okta.com/signup/.
* An Okta Application, configured for Web mode. This is done from the Okta Developer Console; find instructions [here][OIDC Web Application Setup Instructions].  When following the wizard, use the default properties.  They are designed to work with our sample applications.

### Clone the repository

```git clone https://github.com/okta/samples-aspnetcore.git```

### Run the web application

Run the example with your preferred tool, see [Run the web application from Visual Studio](#run-the-web-application-from-visual-studio) or [Run the web application from dotnet CLI](#run-the-web-application-from-dotnet-cli).

> **NOTE:** This sample is using ASP.NET Core 3.1 which enforces HTTPS. This is a recommended practice for web applications. See [Enforce HTTPS in ASP.NET Core] for more details.

> Because of [Set-Cookie behavior (SameSite)](https://web.dev/samesite-cookies-explained) this code will only work properly if it's configured to use https. See [Work with SameSite cookies in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/samesite?view=aspnetcore-3.1) for more details.

#### Run the web application from Visual Studio

When you run this project in Visual Studio it starts the web application on port 44314 using HTTPS. You can change this configuration in the `launchSettings.json` in the Properties folder.

#### Run the web application from dotnet CLI

When you run this project via the dotnet CLI it will start the web application on port 5001 using HTTPS. You can change this configuration in the `launchSettings.json` in the Properties folder.

Navigate to the folder where the project file is located and type the following:

```dotnet run```

### Trust the local dev certificate if necessary

When you run an ASP.NET Core 3.x application for the first time, you may notice an error page warning you that the site is potentially unsafe.
This is because ASP.NET Core creates an HTTPS development certificate for you as part of the first-run experience that needs to be trusted by your browser. You can ignore the warning by clicking on Advanced and telling the browser to visit this site even though there is no certificate for it. Or you can trust the certificate to get rid of this warning, check out [Configuring HTTPS in ASP.NET Core across different platforms] for more details.

### Configure your application in the Okta Developer Console

Go to your [Okta Developer Console] and update the following parameters in your Okta Web Application configuration:
* **Login redirect URI** - `https://localhost:44314/interactioncode/callback`
* **Logout redirect URI** - `https://localhost:44314/account/signout`

The sample application defines an `InteractionCodeController` which receives the `interaction_code` upon successful login; review the `RedeemInteractionCodeAndSignInAsync` method of the `InteractionCodeController` for example code illustrating how to exchange the interaction code for Okta tokens.

### Enable CORS (Cross-Origin Resource Sharing)

Your application must be configured to allow your application to make requests to the Okta API using the Okta session cookie. To enable CORS for your application do the following:

- In your [Okta Developer Console], go to **Security > API > Trusted Origins** 
- Add your web application’s base URL `https://localhost:44314/` as a **Trusted Origin**.

For step-by-step instructions, visit the Okta [Sign Users in to Your Web Application guide] which shows how to sign users in using Okta and, [Sign Users Out guide] which shows how to sign users out of your application and out of Okta.

### Configure your project

Replace the okta configuration placeholders in the `appsettings.json` with configuration values from your [Okta Developer Console]. 
All the available configuration options are documented here [https://github.com/okta/okta-aspnet/blob/master/docs/aspnetcore-mvc.md#configuration-reference](https://github.com/okta/okta-aspnet/blob/master/docs/aspnetcore-mvc.md#configuration-reference).

For step-by-step instructions, visit the Okta [Sign Users in to Your Web Application guide]. The guide will walk you through adding Okta sign-in to your ASP.NET application.

### Run your application and sign in

Click the **Sign In** link on the Home page and you are directed to the sign-in page.

Sign in using the same account you created when you signed up for your Developer Org, or you can use a known username and password from your Okta Directory.

**Note:** If you are currently using your Developer Console, you already have a Single Sign-On (SSO) session for your Org.  You will be automatically signed into your application as the same user that is using the Developer Console.  You may want to use an incognito tab to test the flow from a blank slate.

[Okta ASP.NET Core SDK]: https://github.com/okta/okta-aspnet
[OIDC Web Application Setup Instructions]: https://developer.okta.com/authentication-guide/implementing-authentication/auth-code#1-setting-up-your-application
[Enforce HTTPS in ASP.NET Core]: https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-2.2&tabs=visual-studio
[Configuring HTTPS in ASP.NET Core across different platforms]:https://devblogs.microsoft.com/aspnet/configuring-https-in-asp-net-core-across-different-platforms/
[Sign Users in to Your Web Application guide]: https://developer.okta.com/guides/sign-into-web-app/aspnet/before-you-begin/
[Sign Users Out guide]: https://developer.okta.com/guides/sign-users-out/aspnetcore/before-you-begin/
[Okta Developer Console]: https://login.okta.com