using System;
using System.Web;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using Microsoft.Identity.Client;
using System.IO;
using System.Security.Cryptography;

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

                    if (Request.Cookies[TokenManager.TokenCookieName] != null)
                    {
                        Response.Redirect(returnUrl);
                    }
                }
            }
        }

        protected void Btn_Login_Click(object sender, EventArgs e)
        {
            string userAcc = account.Text;
            string userPwd = password.Text;

            if (AuthenticateUser(userAcc, userPwd))
            {
                Session["LoggedIn"] = true;

                TokenManager tokenManager = new TokenManager();

                bool isAzureADLogin = false;
                string token = tokenManager.GenerateToken(userAcc, isAzureADLogin);

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

        protected void Btn_AzureAD_Click(object sender, EventArgs e)
        {
            string returnUrl = Request.QueryString["returnUrl"];
            // 將 returnUrl 存儲於 cookie 中
            HttpCookie returnUrlCookie = new HttpCookie("ReturnUrlCookie", returnUrl);
            Response.Cookies.Add(returnUrlCookie);
            Response.Redirect("https://localhost:44342/");
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

        // Token的產生、儲存、驗證
        public class TokenManager
        {
            public static readonly string TokenCookieName = "CookieToken";

            // 靜態字典，用來追踪使用者帳號對應的Token
            // private static readonly Dictionary<string, string> activeUserTokens = new Dictionary<string, string>();

            public string GenerateToken(string userAcc, bool isAzureADLogin)
            {
                string tokenData = $"{userAcc}_{Guid.NewGuid()}_{isAzureADLogin}";

                // 將 token 資料加密
                string encryptedToken = TokenHelper.EncryptToken(tokenData);

                // 儲存加密後的 token
                // activeUserTokens[userAcc] = encryptedToken;

                return encryptedToken;
            }

            public void StoreToken(string token)
            {
                HttpCookie tokenCookie = new HttpCookie(TokenCookieName, $"{token}")
                {
                    Expires = DateTime.Now.AddMinutes(30),
                    Domain = "localhost" // 設定為主域名
                };
                HttpContext.Current.Response.Cookies.Add(tokenCookie);
            }

            public bool ValidateToken(HttpRequestBase request, string token, out string userAcc)
            {
                HttpCookie tokenCookie = request.Cookies[TokenCookieName];
                if (tokenCookie != null && !string.IsNullOrEmpty(tokenCookie.Value))
                {
                    // 解密 cookie 中的 token && 解析解密後的 token 資料
                    string storedToken = TokenHelper.DecryptToken(tokenCookie.Value);
                    string[] storeTokenData = storedToken.Split('_');

                    // 解密 帶回來的 token && 解析解密後的 token 資料
                    string returnToken = TokenHelper.DecryptToken(token);
                    string[] returnTokenData = returnToken.Split('_');

                    if (storeTokenData.Length == 3 && returnTokenData.Length == 3)
                    {
                        userAcc = storeTokenData[0];

                        // 驗證 cookie中的token 與 帶回來的token
                        if (returnTokenData[1] == storeTokenData[1])
                        {
                            return true;
                        }
                    }
                }

                userAcc = null;
                return false;
            }

            public static void CentralizedLogout()
            {
                // 清除儲存在cookie中的token
                HttpCookie tokenCookie = new HttpCookie(TokenCookieName)
                {
                    Expires = DateTime.Now.AddDays(-1),
                    Domain = "localhost"
                };
                HttpContext.Current.Response.Cookies.Add(tokenCookie);

                // 清除儲存在cookie中的returnUrl
                HttpCookie returnUrlCookie = new HttpCookie("ReturnUrlCookie")
                {
                    Expires = DateTime.Now.AddDays(-1),
                    Domain = "localhost"
                };
                HttpContext.Current.Response.Cookies.Add(returnUrlCookie);
            }
        }

        // 加解密Token
        public class TokenHelper
        {
            private const string EncryptionKey = "123456789"; // 加密金鑰

            public static string EncryptToken(string tokenData)
            {
                byte[] clearBytes = Encoding.UTF8.GetBytes(tokenData);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
                0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
            });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }

            public static string DecryptToken(string encryptedToken)
            {
                byte[] cipherBytes = Convert.FromBase64String(encryptedToken);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
                0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
            });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        return Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
        }
    }
}