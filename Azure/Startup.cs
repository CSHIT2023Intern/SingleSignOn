using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System.Threading.Tasks;
using System.Web;
using static SingleSignOn.Login;

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

            string clientId = "8ac293ab-6917-4723-a148-ea7a57832f27";
            string authority = "https://login.microsoftonline.com/9902d6cb-2777-42b8-8d31-31a3f6db7e74"; // 設定 Azure AD 中的租用戶 ID

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = clientId,
                Authority = authority,
                ResponseType = OpenIdConnectResponseType.IdToken,
                // RedirectUri = "https://localhost:44396/web1.aspx", // 設定為 MVC 應用程式的 returnUrl
                PostLogoutRedirectUri = "https://localhost:44345/Logout.aspx", // 設定為登出後的 returnUrl
                TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = false // 在測試時，可能需要暫時禁用驗證 Issuer
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

                        // 取消 Owin Middleware 的預設行為
                        txt.HandleResponse();

                        string redirectUrl = "https://localhost:44345/Index.aspx?token=" + HttpUtility.UrlEncode(token);
                        txt.Response.Redirect(redirectUrl);
                        return Task.FromResult(0);
                    }
                }
            });
            ConfigureAuth(app);
        }
    }
}