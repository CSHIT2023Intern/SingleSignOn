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
            if (IsLoggedInWithToken())
            {
                welcomeLabel.Text = "Welcome to System 1!";
            }
            else
            {
                Response.Redirect("web1.aspx");
            }
        }

        private bool IsLoggedInWithToken()
        {
            if (Session["LoggedIn"] != null && (bool)Session["LoggedIn"])
            {
                return Request.Cookies[TokenManager.TokenCookieName] != null;
            }
            return false;
        }

        protected void Web2btn_Click(object sender, EventArgs e)
        {
            OpenNewTab("https://localhost:44343/web2.aspx");
        }

        protected void Web3btn_Click(object sender, EventArgs e)
        {
            OpenNewTab("https://localhost:44379/web3.aspx");
        }

        private void OpenNewTab(string url)
        {
            string script = "window.open('" + url + "', '_blank');";
            ScriptManager.RegisterStartupScript(this, GetType(), "OpenNewTab", script, true);
        }

        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            TokenManager.CentralizedLogout();

            HttpCookie tokenCookie = Request.Cookies[TokenManager.TokenCookieName];
            if (tokenCookie != null)
            {
                string token = tokenCookie.Value;
                string[] tokenData = token.Split('_');
                if (tokenData.Length == 3 && bool.TryParse(tokenData[2], out bool isAzureADLogin))
                {
                    if (isAzureADLogin)
                    {
                        string authority = "https://login.microsoftonline.com/9902d6cb-2777-42b8-8d31-31a3f6db7e74"; // 設定 Azure AD 中的租用戶 ID
                        string redirectUri = "https://localhost:44345/Logout.aspx"; // 設定為登出後的 redirectUri
                        string logoutUrl = $"{authority}/oauth2/v2.0/logout?post_logout_redirect_uri={HttpUtility.UrlEncode(redirectUri)}";
                        Session["LoggedIn"] = false;
                        Response.Redirect(logoutUrl);
                    }
                    else
                    {
                        Session["LoggedIn"] = false;
                        Response.Redirect("web1.aspx");
                    }
                }
            }
        }
    }
}