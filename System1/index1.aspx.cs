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
    public partial class index1 : System.Web.UI.Page
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
                        //string account = GetAccountFromToken(token);
                        //Session["Account"] = account;

                        //string username = Session["Account"] as string;
                        //string username = GetAccountFromToken(token);
                        //welcomeLabel.Text = "Welcome, " + username;
                        welcomeLabel.Text = "姓名：test";
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

        protected void Web2btn_Click(object sender, EventArgs e)
        {
            Response.Redirect("https://localhost:44351/index2.aspx");
            
            //string url = "https://localhost:44351/index2.aspx";
            //string script = "window.open('" + url + "', '_blank');";
            //ScriptManager.RegisterStartupScript(this, GetType(), "OpenNewTab", script, true);
        }

        protected void Web3btn_Click(object sender, EventArgs e)
        {
            Response.Redirect("https://localhost:44374/index3.aspx");
            
            //string url = "https://localhost:44374/index3.aspx";
            //string script = "window.open('" + url + "', '_blank');";
            //ScriptManager.RegisterStartupScript(this, GetType(), "OpenNewTab", script, true);
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