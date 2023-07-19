using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SSO
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string redirectUrl = Request.QueryString["returnUrl"];

            Session.Clear();
            Session.Abandon();
            // Response.Cookies["Token"].Expires = DateTime.Now.AddDays(-1);

            HttpCookie cookie = Request.Cookies["Token"];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }

            if (!string.IsNullOrEmpty(redirectUrl))
            {
                Response.Redirect(redirectUrl);
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }
    }
}