using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System.Configuration;
using System.Threading.Tasks;
using System.Web;
using graph_tutorial.Helpers;
using graph_tutorial.TokenStorage;
using System.Security.Claims;
using System;
using static SingleSignOn.Login;

namespace graph_tutorial
{
    public partial class Startup
    {
        // Load configuration settings from PrivateSettings.config
        private static string appId = ConfigurationManager.AppSettings["ida:AppId"];
        private static string appSecret = ConfigurationManager.AppSettings["ida:AppSecret"];
        private static string redirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];
        private static string graphScopes = ConfigurationManager.AppSettings["ida:AppScopes"];

        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = appId,
                    Authority = "https://login.microsoftonline.com/common/v2.0",
                    Scope = $"openid email profile offline_access {graphScopes}",
                    RedirectUri = redirectUri,
                    PostLogoutRedirectUri = redirectUri,
                    TokenValidationParameters = new TokenValidationParameters
                    {
                        // For demo purposes only, see below
                        ValidateIssuer = false

                        // In a real multi-tenant app, you would add logic to determine whether the
                        // issuer was from an authorized tenant
                        //ValidateIssuer = true,
                        //IssuerValidator = (issuer, token, tvp) =>
                        //{
                        //  if (MyCustomTenantValidation(issuer))
                        //  {
                        //    return issuer;
                        //  }
                        //  else
                        //  {
                        //    throw new SecurityTokenInvalidIssuerException("Invalid issuer");
                        //  }
                        //}
                    },
                    Notifications = new OpenIdConnectAuthenticationNotifications
                    {
                        AuthenticationFailed = OnAuthenticationFailedAsync,
                        AuthorizationCodeReceived = OnAuthorizationCodeReceivedAsync
                    }
                }
            );
        }

        private static Task OnAuthenticationFailedAsync(AuthenticationFailedNotification<OpenIdConnectMessage,
            OpenIdConnectAuthenticationOptions> notification)
        {
            notification.HandleResponse();
            string redirect = $"/Home/Error?message={notification.Exception.Message}";
            if (notification.ProtocolMessage != null && !string.IsNullOrEmpty(notification.ProtocolMessage.ErrorDescription))
            {
                redirect += $"&debug={notification.ProtocolMessage.ErrorDescription}";
            }
            notification.Response.Redirect(redirect);
            return Task.FromResult(0);
        }

        
        private async Task OnAuthorizationCodeReceivedAsync(AuthorizationCodeReceivedNotification notification)
        {
            notification.HandleCodeRedemption();

            var idClient = ConfidentialClientApplicationBuilder.Create(appId)
                .WithRedirectUri(redirectUri)
                .WithClientSecret(appSecret)
                .Build();

            var signedInUser = new ClaimsPrincipal(notification.AuthenticationTicket.Identity);
            var tokenStore = new SessionTokenStore(idClient.UserTokenCache, HttpContext.Current, signedInUser);

            try
            {
                string[] scopes = graphScopes.Split(' ');

                var result = await idClient.AcquireTokenByAuthorizationCode(
                    scopes, notification.Code).ExecuteAsync();

                var userDetails = await GraphHelper.GetUserDetailsAsync(result.AccessToken);

                tokenStore.SaveUserDetails(userDetails);
                notification.HandleCodeRedemption(null, result.IdToken);

                // Store DisplayName in a cookie
                var userInformationCookie = new HttpCookie("UserInformation");
                userInformationCookie.Values["FullName"] = userDetails.DisplayName;
                userInformationCookie.Expires = DateTime.Now.AddHours(1);
                HttpContext.Current.Response.Cookies.Add(userInformationCookie);

                string account = notification.AuthenticationTicket.Identity.Name; // 從驗證Ticket中取得使用者帳號

                TokenManager tokenManager = new TokenManager();

                bool isAzureADLogin = true;
                string token = tokenManager.GenerateToken(account, isAzureADLogin);

                tokenManager.StoreToken(token);

                // 取消Owin Middleware的預設行為
                notification.HandleResponse();

                // 從request中訪問returnUrlCookie
                if (notification.Request.Cookies["ReturnUrlCookie"] == null || string.IsNullOrEmpty(notification.Request.Cookies["ReturnUrlCookie"].ToString()))
                {
                    string returnUrl = "https://localhost:44345/Login.aspx?returnUrl=https://localhost:44345/Index.aspx";
                    string redirectUrl = $"{returnUrl}?token={HttpUtility.UrlEncode(token)}";
                    notification.Response.Redirect(redirectUrl);
                }
                else
                {
                    string returnUrl = notification.Request.Cookies["ReturnUrlCookie"].ToString();
                    string redirectUrl = $"{returnUrl}?token={HttpUtility.UrlEncode(token)}";
                    notification.Response.Redirect(redirectUrl);
                }
                return;
            }
            catch (MsalException ex)
            {
                string message = "AcquireTokenByAuthorizationCodeAsync threw an exception";
                notification.HandleResponse();
                notification.Response.Redirect($"/Home/Error?message={message}&debug={ex.Message}");
            }
            catch (Microsoft.Graph.ServiceException ex)
            {
                string message = "GetUserDetailsAsync threw an exception";
                notification.HandleResponse();
                notification.Response.Redirect($"/Home/Error?message={message}&debug={ex.Message}");
            }
        }

        /*
        private async Task OnAuthorizationCodeReceivedAsync(AuthorizationCodeReceivedNotification notification)
        {
            var idClient = ConfidentialClientApplicationBuilder.Create(appId)
                .WithRedirectUri(redirectUri)
                .WithClientSecret(appSecret)
                .Build();

            string message;
            string debug;

            try
            {
                string[] scopes = graphScopes.Split(' ');

                var result = await idClient.AcquireTokenByAuthorizationCode(
                    scopes, notification.Code).ExecuteAsync();

                var userDetails = await GraphHelper.GetUserDetailsAsync(result.AccessToken);

                message = "User info retrieved.";
                debug = $"User: {userDetails.DisplayName}, Email: {userDetails.Email}";
            }
            catch (MsalException ex)
            {
                message = "AcquireTokenByAuthorizationCodeAsync threw an exception";
                debug = ex.Message;
            }

            var queryString = $"message={message}&debug={debug}";
            if (queryString.Length > 2048)
            {
                queryString = queryString.Substring(0, 2040) + "...";
            }

            notification.HandleResponse();
            notification.Response.Redirect($"/Home/Error?{queryString}");
        }*/
    }
}