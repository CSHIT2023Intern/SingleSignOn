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
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["Token"] != null || Session["Account"] != null)
                {
                    string token = Request.Cookies["Token"].Value;
                    if (ValidateToken(token))
                    {
                        string username = Session["Account"] as string;
                        welcomeLabel.Text = "Welcome, " + username + "!";
                    }
                    else
                    {
                        Logout();
                    }
                }
                else
                {
                    Logout();
                }
            }
        }

        protected void Web1btn_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["token"] != null)
            {
                string token = Request.QueryString["token"];
                if (ValidateToken(token))
                {
                    Response.Redirect("https://localhost:44375/index1.aspx");
                }
                else
                {
                    errorLabel.Text = "Invalid token. Access denied.";
                }
            }
            else
            {
                errorLabel.Text = "Missing token. Access denied.";
            }
        }

        protected void Web2btn_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["token"] != null)
            {
                string token = Request.QueryString["token"];
                if (ValidateToken(token))
                {
                    Response.Redirect("https://localhost:44351/index2.aspx");
                }
                else
                {
                    errorLabel.Text = "Invalid token. Access denied.";
                }
            }
            else
            {
                errorLabel.Text = "Missing token. Access denied.";
            }
        }

        protected void Web3btn_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["token"] != null)
            {
                string token = Request.QueryString["token"];
                if (ValidateToken(token))
                {
                    Response.Redirect("https://localhost:44374/index3.aspx");
                }
                else
                {
                    errorLabel.Text = "Invalid token. Access denied.";
                }
            }
            else
            {
                errorLabel.Text = "Missing token. Access denied.";
            }
        }

        //// 檢查 User 是否已登入
        //private bool UserLoggedIn()
        //{
        //    string account = Session["Account"] as string;
        //    return !string.IsNullOrEmpty(account);
        //}

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

        private void StoreToken(string token)
        {
            HttpCookie cookie = new HttpCookie("Token", token)
            {
                Expires = DateTime.Now.AddDays(1) // 設定 cookie 的過期時間
            };
            Response.Cookies.Add(cookie);
        }

        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("https://localhost:44303/Logout.aspx");
        }

        private void Logout()
        {
            RemoveToken();
            Session.Clear();
            Session.Abandon();

            Response.Redirect("https://localhost:44303/Login.aspx");
        }

        private void RemoveToken()
        {
            HttpCookie cookie = Request.Cookies["Token"];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
        }
    }
}