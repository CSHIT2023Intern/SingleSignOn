using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;

namespace SSO
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["AuthCookie"] != null)
            {
                Response.Redirect("https://localhost:44303/Index.aspx");
            }
            else
            {
                Response.Redirect("https://localhost:44342/");
            }

            //if (!IsPostBack)
            //{
            //    string idToken = Request.QueryString["id_token"];
            //    bool isValidToken = ValidateToken(idToken);

            //    if (isValidToken)
            //    {
            //        string generatedToken = GenerateTokenFromIdToken(idToken);
            //        string returnUrl = Session["ReturnUrl"] as string;
            //        Response.Redirect(!string.IsNullOrEmpty(returnUrl) ? returnUrl + "?token=" + HttpUtility.UrlEncode(generatedToken) : "https://localhost:44303/Index.aspx" + "?token=" + HttpUtility.UrlEncode(generatedToken));
            //    }
            //}
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string account = accountTextBox.Text;
            string password = passwordTextBox.Text;

            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert", "alert('請輸入帳號及密碼！');", true);
            }
            else if (account != null)
            {
                Session["Account"] = account;
                string token = GenerateTokenFromIdToken(account);
                StoreToken(token);
                string ReturnUrl = Session["ReturnUrl"] as string;
                Response.Redirect(!string.IsNullOrEmpty(ReturnUrl) ? ReturnUrl + "?token=" + HttpUtility.UrlEncode(token) : "https://localhost:44303/Index.aspx" + "?token=" + HttpUtility.UrlEncode(token));
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert", "alert('帳號及密碼填寫錯誤！');", true);
            }
        }

        private string GenerateTokenFromIdToken(string idToken)
        {
            byte[] accountBytes = Encoding.UTF8.GetBytes(idToken);
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
            Session["GeneratedToken"] = token;
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
            return cookie != null && string.Equals(token, cookie.Value);
        }
    }
}