using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using WebApplication1.Models;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Configuration;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security;
using System.Globalization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Claims;

namespace WebApplication1
{
    public partial class Startup
    {
        private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static string aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        private static string tenant = ConfigurationManager.AppSettings["ida:Tenant"];
        private static string postLogoutRedirectUri = ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"];
        string authority = String.Format(CultureInfo.InvariantCulture, aadInstance, tenant);
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        /*
        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            //app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "57336bd5-a80f-4b48-a29a-07fdea6ef91e",
            //    clientSecret: "mK5cYvTsFcUtv6nd50TR4a4");

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = clientId,
                Authority = authority,
                PostLogoutRedirectUri = postLogoutRedirectUri,
                //RedirectUri = postLogoutRedirectUri,
                AuthenticationMode = AuthenticationMode.Passive,
                //ResponseType= 
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    AuthenticationFailed = context =>
                    {
                        context.HandleResponse();
                        context.Response.Redirect("/Error?message=" + context.Exception.Message);
                        return Task.FromResult(0);
                    }
                        ,
                    SecurityTokenValidated = context =>
                    {
                        context.AuthenticationTicket.Identity.AddClaim(new System.Security.Claims.Claim("AddByMe", "test"));
                        return Task.FromResult(0);
                    },
                    RedirectToIdentityProvider = context =>
                    {
                        //string newRedirectUri = context.Request.r;
                        var userName = context.Request.Query.Get("userName");
                        context.ProtocolMessage.Parameters.Add("login_hint", userName);
                        string appBaseUrl = context.Request.Scheme + "://" + context.Request.Host + context.Request.PathBase;
                        string currentUrl = context.Request.Scheme + "://" + context.Request.Host + context.Request.Path;
                        context.ProtocolMessage.RedirectUri = currentUrl;
                        context.ProtocolMessage.PostLogoutRedirectUri = appBaseUrl;
                        return Task.FromResult(0);
                    },
                    AuthorizationCodeReceived = (notification) =>
                    {
                        var a = HttpContext.Current;
                        return Task.FromResult(0);
                    }
                },
                TokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters
                {
                    // map the claimsPrincipal's roles to the roles claim
                    RoleClaimType = "roles",
                },
            });

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }*/

        public void ConfigureAuth(IAppBuilder app)
        {
            //app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);


            string ClientId = ConfigurationManager.AppSettings["ida:ClientID"];
            string Authority = "https://login.microsoftonline.com/common/";

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = ClientId,
                    Authority = Authority,
                    TokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = false,
                    },
                    Notifications = new OpenIdConnectAuthenticationNotifications()
                    {
                        RedirectToIdentityProvider = (context) =>
                        {
                            string appBaseUrl = context.Request.Scheme + "://" + context.Request.Host + context.Request.PathBase;
                            context.ProtocolMessage.RedirectUri = appBaseUrl;
                            context.ProtocolMessage.PostLogoutRedirectUri = appBaseUrl;
                            return Task.FromResult(0);
                        },
                        SecurityTokenValidated = (context) =>
                        {
                        // retriever caller data from the incoming principal
                        string issuer = context.AuthenticationTicket.Identity.FindFirst("iss").Value;
                            string UPN = context.AuthenticationTicket.Identity.FindFirst(ClaimTypes.Name).Value;
                            string tenantID = context.AuthenticationTicket.Identity.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;

                            //if (
                            //    // the caller comes from an admin-consented, recorded issuer
                            //    (db.Tenants.FirstOrDefault(a => ((a.IssValue == issuer) && (a.AdminConsented))) == null)
                            //    // the caller is recorded in the db of users who went through the individual onboardoing
                            //    && (db.Users.FirstOrDefault(b => ((b.UPN == UPN) && (b.TenantID == tenantID))) == null)
                            //    )
                            //// the caller was neither from a trusted issuer or a registered user - throw to block the authentication flow
                            //throw new SecurityTokenValidationException();
                            return Task.FromResult(0);
                        },
                        AuthenticationFailed = (context) =>
                        {
                            context.OwinContext.Response.Redirect("/Home/Error?message=" + context.Exception.Message);
                            context.HandleResponse(); // Suppress the exception
                        return Task.FromResult(0);
                        }
                    }
                });

        }
    }
}