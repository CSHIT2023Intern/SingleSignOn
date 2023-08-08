using System;
using static SingleSignOn.Login;

namespace SingleSignOn
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["redirectUrl"]))
            {
                TokenManager.CentralizedLogout();

                string returnUrl = Request.QueryString["redirectUrl"];
                Response.Redirect(returnUrl);
            }
        }
    }
}