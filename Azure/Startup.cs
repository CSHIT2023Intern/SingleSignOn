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
            // 配置身分驗證中間件
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            string clientId = "223c0741-66d6-4358-8a11-36ca97950fcd";
            string authority = "https://login.microsoftonline.com/f8cdef31-a31e-4b4a-93e4-5f571e91255a";

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
                    AuthenticationFailed = context =>
                    {
                        // 處理驗證失敗的情況
                        return Task.FromResult(0);
                    },
                    SecurityTokenValidated = context =>
                    {
                        // 處理驗證成功的情況

                        string userAcc = context.AuthenticationTicket.Identity.Name;

                        TokenManager tokenManager = new TokenManager();

                        bool isAzureADLogin = true;
                        string token = tokenManager.GenerateToken(userAcc, isAzureADLogin);

                        tokenManager.StoreToken(token);

                        // 取消Owin Middleware的預設行為
                        context.HandleResponse();

                        // 從請求(request)中訪問returnUrlCookie
                        if (context.Request.Cookies["ReturnUrlCookie"] == null || string.IsNullOrEmpty(context.Request.Cookies["ReturnUrlCookie"].ToString()))
                        {
                            string returnUrl = "https://localhost:44345/Login.aspx?returnUrl=https://localhost:44345/Frontpage.aspx";
                            string redirectUrl = $"{returnUrl}?token={HttpUtility.UrlEncode(token)}";
                            context.Response.Redirect(redirectUrl);
                        }
                        else
                        {
                            string returnUrl = context.Request.Cookies["ReturnUrlCookie"].ToString();
                            string redirectUrl = $"{returnUrl}?token={HttpUtility.UrlEncode(token)}";

                            context.Response.Redirect(redirectUrl);
                        }

                        return Task.FromResult(0);
                    }
                }
            });
        }
    }
}
