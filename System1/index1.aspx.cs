using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Web;
using System.Web.UI;
using static SingleSignOn.Login;

namespace SingleSignOn
{
    public partial class index1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string tokenCookieName = TokenManager.TokenCookieName;
            HttpCookie tokenCookie = Request.Cookies[tokenCookieName];

            if (tokenCookie == null)
            {
                string returnUrl = Request.QueryString["returnUrl"];
                Response.Redirect(string.IsNullOrEmpty(returnUrl) ? "web1.aspx" : returnUrl);
            }
            else
            {
                welcomeLabel.Text = "Welcome to System 1!";
            }
        }

        protected void Web2btn_Click(object sender, EventArgs e)
        {
            OpenNewTab("https://localhost:44343/web2.aspx");
        }

        protected void Web3btn_Click(object sender, EventArgs e)
        {
            OpenNewTab("https://localhost:44391/web3.aspx");
        }

        private void OpenNewTab(string url)
        {
            string script = "window.open('" + url + "', '_blank');";
            ScriptManager.RegisterStartupScript(this, GetType(), "OpenNewTab", script, true);
        }

        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            TokenManager.CentralizedLogout();

            string tokenCookieName = TokenManager.TokenCookieName;
            HttpCookie tokenCookie = Request.Cookies[tokenCookieName];

            if (tokenCookie != null)
            {
                string token = TokenHelper.DecryptToken(tokenCookie.Value);
                string[] tokenData = token.Split('_');

                if (tokenData.Length == 3 && bool.TryParse(tokenData[2], out bool isAzureADLogin))
                {
                    string authority = "https://login.microsoftonline.com/410d1846-1236-446b-85d6-b3aa69060f16";
                    string returnUrl = Request.QueryString["returnUrl"] ?? "https://localhost:44343/web1.aspx";
                    string logoutUrl = $"{authority}/oauth2/v2.0/logout?post_logout_redirect_uri={HttpUtility.UrlEncode(returnUrl)}";
                    Response.Redirect(isAzureADLogin ? logoutUrl : returnUrl);
                }
            }
        }
    }
}