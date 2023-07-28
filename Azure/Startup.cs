using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System.Configuration;
using System.IdentityModel.Tokens;
using static SingleSignOn.Login;
using System.Web;

[assembly: OwinStartup(typeof(Azure.Startup))]


namespace Azure
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            string clientId = "98578604-4999-42f7-9a6a-e1a5bac209da";
            string authority = "https://login.microsoftonline.com/410d1846-1236-446b-85d6-b3aa69060f16";

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = clientId,
                Authority = authority,
                ResponseType = OpenIdConnectResponseType.IdToken,
                TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = false // 在測試時，可能需要暫時禁用驗證Issuer
                },
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    AuthenticationFailed = txt =>
                    {
                        // 處理驗證失敗的情況
                        return Task.FromResult(0);
                    },
                    SecurityTokenValidated = txt =>
                    {
                        // 處理驗證成功的情況
                        string account = txt.AuthenticationTicket.Identity.Name;
                        TokenManager tokenManager = new TokenManager();
                        bool isAzureADLogin = true;
                        string token = tokenManager.GenerateToken(account, isAzureADLogin);
                        tokenManager.StoreToken(token);

                        // 取消Owin Middleware的預設行為
                        txt.HandleResponse();

                        // 從請求(request)中訪問returnUrlCookie
                        string returnUrl = txt.Request.Cookies["ReturnUrlCookie"]?.ToString();
                        if (string.IsNullOrEmpty(returnUrl))
                        {
                            returnUrl = "https://localhost:44345/Login.aspx?returnUrl=https://localhost:44345/Index.aspx";
                        }

                        string redirectUrl = $"{returnUrl}?token={HttpUtility.UrlEncode(token)}";
                        txt.Response.Redirect(redirectUrl);

                        return Task.FromResult(0);
                    }
                }
            });
        }
    }
}
