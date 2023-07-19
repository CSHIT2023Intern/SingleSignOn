using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SSO
{
    public partial class index3 : System.Web.UI.Page
    {
        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            //string redirectUrl = "https://localhost:44375/web1.aspx";
            //Response.Redirect("https://localhost:44303/Logout.aspx?returnUrl=" + Server.UrlEncode(redirectUrl));

            Response.Redirect("https://localhost:44303/Logout.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["Token"] != null || Session["Account"] != null)
                {
                    string token = Request.Cookies["Token"]?.Value;
                    if (ValidateToken(token))
                    {
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

        // 驗證 token
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