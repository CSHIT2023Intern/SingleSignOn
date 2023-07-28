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

            string clientId = "08ab7252-c7f9-4828-86ad-683e28516815";
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
                    AuthenticationFailed = context =>
                    {
                        // 處理驗證失敗的情況
                        return Task.FromResult(0);
                    },
                    SecurityTokenValidated = context =>
                    {
                        // 處理驗證成功的情況，如果需要，可以在這裡同步資料庫的用戶信息

                        string userAcc = context.AuthenticationTicket.Identity.Name;

                        TokenManager tokenManager = new TokenManager();

                        bool isAzureADLogin = true;
                        string token = tokenManager.GenerateToken(userAcc, isAzureADLogin);

                        tokenManager.StoreToken(token);

                        // 取消Owin Middleware的預設行為
                        context.HandleResponse();

                        // 從請求(request)中訪問returnUrlCookie
                        var returnUrlCookie = context.Request.Cookies["ReturnUrlCookie"].ToString();
                        if (returnUrlCookie != null && !string.IsNullOrEmpty(returnUrlCookie))
                        {
                            string returnUrl = returnUrlCookie;
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
