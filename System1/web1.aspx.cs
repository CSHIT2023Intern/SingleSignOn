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
                if (Request.Cookies[TokenManager.TokenCookieName] != null)
                {
                    string token = Request.Cookies[TokenManager.TokenCookieName].Value;

                    TokenManager tokenManager = new TokenManager();

                    bool isValidToken = tokenManager.ValidateToken(new HttpRequestWrapper(Request), token, out string account);

                    if (isValidToken == true)
                    {
                        Session["LoggedIn"] = true;

                        Session["user"] = account;

                        Response.Redirect("index1.aspx");
                    }
                    else
                    {
                        Response.Redirect("https://localhost:44345/Login.aspx");
                    }
                }
                else
                {
                    string returnUrl = Server.UrlEncode(Request.Url.ToString());
                    Response.Redirect("https://localhost:44345/Login.aspx?returnUrl=" + returnUrl);
                }
            }
        }
    }
}