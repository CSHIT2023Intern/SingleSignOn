using System;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using System.Security.Claims;

[assembly: OwinStartup(typeof(SSO.Startup))]

namespace SSO
{
    public class Startup
    {
        private static readonly string clientId = "8ac293ab-6917-4723-a148-ea7a57832f27";
        private static readonly string authority = "https://login.microsoftonline.com/9902d6cb-2777-42b8-8d31-31a3f6db7e74";
        private static readonly string redirectUri = "https://localhost:44303/Login.aspx"; // 登入後重導向的URL

        public void Configuration(IAppBuilder app)
        {
            // 設定Cookie驗證
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie", // 設定驗證類型
                LoginPath = new PathString("/Login.aspx"), // 未驗證時導向的頁面
                CookieName = "AuthCookie", // Set your authentication cookie name
                ExpireTimeSpan = TimeSpan.FromMinutes(30), // Set your desired expiration time
            });

            // 設定OpenID Connect驗證
            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = clientId,
                Authority = authority,
                RedirectUri = redirectUri,
                ResponseType = "id_token", // 僅驗證ID Token
                Scope = "openid", // 要求驗證OpenID
                SignInAsAuthenticationType = "Cookies",

                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = n =>
                    {
                        // 獲取 id_token 的值
                        string idToken = n.ProtocolMessage.IdToken;

                        // 儲存 id_token 到 Cookie 中
                        var authenticationManager = n.OwinContext.Authentication;
                        authenticationManager.SignIn(new AuthenticationProperties
                        {
                            IsPersistent = true,
                        }, new ClaimsIdentity(new[] { new Claim("id_token", idToken) }));

                        return Task.CompletedTask;
                    }
                }
            });
        }
    }
}