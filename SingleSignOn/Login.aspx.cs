using System;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using Microsoft.Identity.Client;
using System.Web.UI;

namespace SingleSignOn
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string returnUrl = Request.QueryString["returnUrl"];
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    // 將returnUrl存儲在Session中
                    Session["ReturnUrl"] = returnUrl;
                }
            }
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string account = accountTextBox.Text;
            string password = passwordTextBox.Text;

            if (AuthenticateUser(account, password))
            {
                // 設定登入狀態為已登入
                Session["LoggedIn"] = true;

                TokenManager tokenManager = new TokenManager();

                bool isAzureADLogin = false; // 登入方式為直接使用使用者名稱和密碼，將此設為false
                string token = tokenManager.GenerateToken(account, isAzureADLogin);

                tokenManager.StoreToken(token);

                string returnUrl = Session["ReturnUrl"].ToString();
                if (Session["ReturnUrl"] != null)
                {
                    Session.Remove("ReturnUrl");// 移除ReturnUrl變數，因為已經使用過了

                    // 將 Token 作為 QueryString 參數附加到重導向的 URL 中
                    string redirectUrl = $"{returnUrl}?token={HttpUtility.UrlEncode(token)}";
                    Response.Redirect(redirectUrl);
                }
                else
                {
                    Response.Redirect(returnUrl);
                }
            }
            else if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert", "alert('請輸入帳號及密碼！');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert", "alert('帳號及密碼填寫錯誤！');", true);
            }
        }

        protected void ADButton_Click(object sender, EventArgs e)
        {
            // ASP.NET Web 應用程式 (.NET Framework 4.7.2) - MVC
            Response.Redirect("https://localhost:44342/");
        }





        private bool AuthenticateUser(string account, string password)
        {
            return (account == "test" && password == "0000");
        }

        public class TokenManager
        {
            string secretKey = ConfigurationManager.AppSettings["SecretKey"];
            //private static readonly string SecretKey = ConfigurationManager.AppSettings["SecretKey"];
            public static readonly string TokenCookieName = "CookieToken";

            // 靜態字典，用來追踪使用者帳號對應的Token
            // private static readonly Dictionary<string, string> activeUserTokens = new Dictionary<string, string>();

            public string GenerateToken(string account, bool isAzureADLogin)
            {
                string tokenData = $"{account}_{Guid.NewGuid()}_{isAzureADLogin}";
                /*
                if (!string.IsNullOrEmpty(secretKey))
                {
                    using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
                    {
                        byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(tokenData));
                        string token = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

                        // 將使用者帳號和Token加入字典中
                        activeUserTokens[userAcc] = token;

                        return token;
                    }
                }*/
                return tokenData;
            }

            public void StoreToken(string token)
            {
                HttpCookie tokenCookie = new HttpCookie(TokenCookieName, $"{token}")
                {
                    Expires = DateTime.Now.AddMinutes(30), // 設置適當的過期時間
                    Domain = "localhost" // 設定為主域名
                };

                // 將 Cookie 添加到 Response 中
                HttpContext.Current.Response.Cookies.Add(tokenCookie);
            }

            /*
            public bool ValidateToken(HttpRequestBase request, string token, out string userAcc)
            {
                HttpCookie tokenCookie = request.Cookies[TokenCookieName];
                if (tokenCookie != null && !string.IsNullOrEmpty(tokenCookie.Value))
                {
                    string storedToken = tokenCookie.Value;

                    // 解析 Token，取得 userAcc
                    string[] tokenData = storedToken.Split('_');
                    if (tokenData.Length == 3)
                    {
                        userAcc = tokenData[0];

                        // 驗證 Token 是否有效並和使用者帳號匹配
                        if (token == storedToken && activeUserTokens.ContainsKey(userAcc) && activeUserTokens[userAcc] == token)
                        {
                            return true;
                        }
                    }
                }

                userAcc = null;
                return false;
            }*/

            public bool ValidateToken(HttpRequestBase request, string token, out string account)
            {
                HttpCookie tokenCookie = request.Cookies[TokenCookieName];
                if (tokenCookie != null && !string.IsNullOrEmpty(tokenCookie.Value))
                {
                    string storedToken = tokenCookie.Value;
                    account = "";

                    // 解析 Token，取得 account
                    string[] tokenData = storedToken.Split('_');
                    if (tokenData.Length == 3)
                    {
                        account = tokenData[0];
                    }

                    // 驗證 Token 是否有效
                    return token == storedToken;
                }

                account = null;
                return false;
            }

            public static void CentralizedLogout()
            {
                HttpCookie tokenCookie = new HttpCookie(TokenCookieName)
                {
                    Expires = DateTime.Now.AddDays(-1),
                    Domain = "localhost"
                };
                HttpContext.Current.Response.Cookies.Add(tokenCookie);
            }
        }
    }
}