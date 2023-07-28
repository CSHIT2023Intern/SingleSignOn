using System;
using System.Diagnostics;
using System.Web;
using static SingleSignOn.Login;

namespace SingleSignOn
{
    public partial class web1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (IsTokenValid(out string account))
                {
                    Session["LoggedIn"] = true;
                    Session["user"] = account;
                    Response.Redirect("index1.aspx");
                }
                else
                {
                    RedirectToLogin();
                }
            }
        }

        private bool IsTokenValid(out string account)
        {
            account = null;
            HttpCookie tokenCookie = Request.Cookies[TokenManager.TokenCookieName];
            if (tokenCookie != null)
            {
                string token = tokenCookie.Value;
                TokenManager tokenManager = new TokenManager();
                return tokenManager.ValidateToken(new HttpRequestWrapper(Request), token, out account);
            }
            return false;
        }

        private void RedirectToLogin()
        {
            string returnUrl = Server.UrlEncode(Request.Url.ToString());
            Response.Redirect("https://localhost:44345/Login.aspx?returnUrl=" + returnUrl);
        }
    }
}