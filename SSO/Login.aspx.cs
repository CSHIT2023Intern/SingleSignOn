using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SSO
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
                    Session["ReturnUrl"] = returnUrl;
                }

                if (Request.Cookies["Token"] != null)
                {
                    string token = Request.Cookies["Token"].Value;
                    if (ValidateToken(token))
                    {
                        //string account = GetAccountFromToken(token);
                        //Session["Account"] = account;
                        Response.Redirect("Index.aspx");
                    }
                }
            }
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string account = accountTextBox.Text;
            string password = passwordTextBox.Text;

            if (AuthenticateUser(account, password))
            {
                Session["Account"] = account;

                string token = GenerateToken(account);

                StoreToken(token);

                string ReturnUrl = Session["ReturnUrl"] as string;
                if (!string.IsNullOrEmpty(ReturnUrl))
                {
                    Response.Redirect(ReturnUrl + "?token=" + HttpUtility.UrlEncode(token));
                }
                else
                {
                    Response.Redirect("Index.aspx" + "?token=" + HttpUtility.UrlEncode(token));
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

        private bool AuthenticateUser(string account, string password)
        {
            if (account == "test" && password == "0000")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string GenerateToken(string account)
        {
            // string token = Guid.NewGuid().ToString();

            byte[] accountBytes = Encoding.UTF8.GetBytes(account);
            byte[] timestampBytes = BitConverter.GetBytes(DateTime.UtcNow.Ticks);
            byte[] tokenBytes = new byte[accountBytes.Length + timestampBytes.Length];
            Array.Copy(accountBytes, 0, tokenBytes, 0, accountBytes.Length);
            Array.Copy(timestampBytes, 0, tokenBytes, accountBytes.Length, timestampBytes.Length);
            byte[] hashBytes;
            using (SHA256 sha256 = SHA256.Create())
            {
                hashBytes = sha256.ComputeHash(tokenBytes);
            }
            string token = Convert.ToBase64String(hashBytes);
            return token;
        }

        private void StoreToken(string token)
        {
            HttpCookie cookie = new HttpCookie("Token", token)
            {
                Expires = DateTime.Now.AddDays(1) // 設定 cookie 的過期時間
            };
            Response.Cookies.Add(cookie);
        }

        private bool ValidateToken(string token)
        {
            HttpCookie cookie = Request.Cookies["Token"];
            if (cookie != null)
            {
                string storedToken = cookie.Value;
                return string.Equals(token, storedToken);
            }
            return false;
        }

        private string GetAccountFromToken(string token)
        {
            byte[] hashBytes = Convert.FromBase64String(token);
            byte[] accountBytes = new byte[hashBytes.Length - 8];
            Array.Copy(hashBytes, 0, accountBytes, 0, accountBytes.Length);
            string account = Encoding.UTF8.GetString(accountBytes);
            return account;
        }
    }
}