using System;
using System.Web;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using Microsoft.Identity.Client;
using System.IO;
using System.Security.Cryptography;
using System.Data.SqlTypes;
using System.Web.UI;

namespace SingleSignOn
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies[TokenManager.TokenCookieName] == null)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["returnUrl"]))
                { }
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.QueryString["returnUrl"]))
                {
                    string returnUrl = Request.QueryString["returnUrl"];

                    Response.Redirect(returnUrl);
                }
                Response.Redirect("Index.aspx");
            }
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string account = accountTextBox.Text;
            string password = passwordTextBox.Text;

            if (AuthenticateUser(account, password))
            {
                TokenManager tokenManager = new TokenManager();
                bool isAzureADLogin = false;
                string token = tokenManager.GenerateToken(account, isAzureADLogin);
                tokenManager.StoreToken(token);

                string returnUrl = Request.QueryString["returnUrl"] ?? "https://localhost:44345/Index.aspx";
                Response.Redirect($"{returnUrl}?token={HttpUtility.UrlEncode(token)}");
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
            string returnUrl = Request.QueryString["returnUrl"] ?? "https://localhost:44345/Login.aspx?returnUrl=https://localhost:44345/Index.aspx";
            HttpCookie returnUrlCookie = new HttpCookie("ReturnUrlCookie", returnUrl)
            {
                Domain = "localhost"
            };
            Response.Cookies.Add(returnUrlCookie);
            Response.Redirect("https://localhost:44342/");
        }

        private bool AuthenticateUser(string account, string password)
        {
            return (account == "test" && password == "0000");
        }

        // Token的產生、儲存、驗證
        public class TokenManager
        {
            public static readonly string TokenCookieName = "CookieToken";

            public string GenerateToken(string account, bool isAzureADLogin)
            {
                string tokenData = $"{account}_{Guid.NewGuid()}_{isAzureADLogin}";
                string encryptedToken = TokenHelper.EncryptToken(tokenData);
                return encryptedToken;
            }

            public void StoreToken(string token)
            {
                HttpCookie tokenCookie = new HttpCookie(TokenCookieName, $"{token}")
                {
                    Expires = DateTime.Now.AddMinutes(30),
                    Domain = "localhost"
                };
                HttpContext.Current.Response.Cookies.Add(tokenCookie);
            }

            public bool ValidateToken(HttpRequestBase request, string token, out string account)
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
                        account = storeTokenData[0];

                        // 驗證 cookie中的token 與 帶回來的token
                        if (returnTokenData[1] == storeTokenData[1])
                        {
                            return true;
                        }
                    }
                }
                account = null;
                return false;
            }

            public static void CentralizedLogout()
            {
                // 清除儲存在 cookie 中的 token
                HttpCookie tokenCookie = new HttpCookie(TokenCookieName)
                {
                    Expires = DateTime.Now.AddDays(-1),
                    Domain = "localhost"
                };
                HttpContext.Current.Response.Cookies.Add(tokenCookie);

                // 清除儲存在 cookie 中的 returnUrl
                HttpCookie returnUrlCookie = new HttpCookie("ReturnUrlCookie")
                {
                    Expires = DateTime.Now.AddDays(-1),
                    Domain = "localhost"
                };
                HttpContext.Current.Response.Cookies.Add(returnUrlCookie);
            }
        }

        // 加解密 Token
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