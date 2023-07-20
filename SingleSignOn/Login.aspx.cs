using System;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;

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

        protected void Btn_Login_Click(object sender, EventArgs e)
        {
            string userAcc = account.Text;
            string userPwd = password.Text;

            if (AuthenticateUser(userAcc, userPwd))
            {
                // 設定登入狀態為已登入
                Session["LoggedIn"] = true;

                TokenManager tokenManager = new TokenManager();

                string token = tokenManager.GenerateToken(userAcc);

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
            else
            {
                string errorMessage = "帳號或密碼錯誤！";
                string script = $"<script>alert('{errorMessage}');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "LoginError", script);
            }
        }

        private bool AuthenticateUser(string userAcc, string userPwd)
        {
            if (userAcc == "user" && userPwd == "0000")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public class TokenManager
        {
            string secretKey = ConfigurationManager.AppSettings["SecretKey"];
            //private static readonly string SecretKey = ConfigurationManager.AppSettings["SecretKey"];
            public static readonly string TokenCookieName = "CookieToken";

            // 靜態字典，用來追踪使用者帳號對應的Token
            private static readonly Dictionary<string, string> activeUserTokens = new Dictionary<string, string>();

            public string GenerateToken(string userAcc)
            {
                string tokenData = $"{userAcc}_{Guid.NewGuid()}";
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
                }
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
                    if (tokenData.Length == 2)
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

            public bool ValidateToken(HttpRequestBase request, string token, out string userAcc)
            {
                HttpCookie tokenCookie = request.Cookies[TokenCookieName];
                if (tokenCookie != null && !string.IsNullOrEmpty(tokenCookie.Value))
                {
                    string storedToken = tokenCookie.Value;
                    userAcc = "";

                    // 解析 Token，取得 userAcc
                    string[] tokenData = storedToken.Split('_');
                    if (tokenData.Length == 2)
                    {
                        userAcc = tokenData[0];
                    }

                    // 驗證 Token 是否有效
                    return token == storedToken;
                }

                userAcc = null;
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