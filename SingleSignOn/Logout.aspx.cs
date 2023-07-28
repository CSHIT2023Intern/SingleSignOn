using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SingleSignOn
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LogoutUser();
        }

        public void LogoutUser()
        {
            Session.Clear();
            Session.Abandon();

            if (Request.Cookies["AuthToken"] != null)
            {
                HttpCookie authTokenCookie = new HttpCookie("AuthToken")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(authTokenCookie);
            }

            if (Request.Cookies["Token"] != null)
            {
                HttpCookie tokenCookie = new HttpCookie("Token")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(tokenCookie);
            }

            string redirectUrl = Request.QueryString["returnUrl"];
            Response.Redirect(string.IsNullOrEmpty(redirectUrl) ? "Login.aspx" : redirectUrl);
        }
    }
}