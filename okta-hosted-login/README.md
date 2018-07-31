# ASP.NET Core & Okta-Hosted Login Page Example

This example shows you how to use the Okta.AspNetCore library to log in a user. The user's browser is first redirected to the Okta-hosted login page. After the user authenticates, they are redirected back to your application. ASP.NET Core automatically populates `HttpContext.User` with the information Okta sends back about the user.

## Prerequisites

Before running this sample, you will need the following:

* An Okta Developer Account, you can sign up for one at https://developer.okta.com/signup/.
* An Okta Application, configured for Web mode. This is done from the Okta Developer Console and you can find instructions [here][OIDC Web Application Setup Instructions].  When following the wizard, use the default properties.  They are designed to work with our sample applications.


## Running This Example

Clone this repo and replace the okta configuration placeholders in the `appsettings.json` with your configuration values from the Okta Developer Console. 
You can see all the available configuration options in the [okta-aspnet GitHub](https://github.com/okta/okta-aspnet/blob/master/README.md).
For step-by-step instructions, visit the Okta [ASP.NET MVC quickstart]. The quickstart will guide you through adding Okta login to your ASP.NET application.

Now start your server and navigate to http://localhost:8080 in your browser.

If you see a home page that allows you to login, then things are working!  Clicking the **Log in** link will redirect you to the Okta hosted sign-in page.

You can login with the same account that you created when signing up for your Developer Org, or you can use a known username and password from your Okta Directory.

**Note:** If you are currently using your Developer Console, you already have a Single Sign-On (SSO) session for your Org.  You will be automatically logged into your application as the same user that is using the Developer Console.  You may want to use an incognito tab to test the flow from a blank slate.

[OIDC Middleware Library]: https://github.com/okta/okta-aspnet
[Authorization Code Flow]: https://developer.okta.com/authentication-guide/implementing-authentication/auth-code
[OIDC Web Application Setup Instructions]: https://developer.okta.com/authentication-guide/implementing-authentication/auth-code#1-setting-up-your-application
[ASP.NET MVC quickstart]:https://developer.okta.com/quickstart/#/okta-sign-in-page/dotnet/aspnetcore