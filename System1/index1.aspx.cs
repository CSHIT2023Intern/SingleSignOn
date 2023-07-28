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
            if (Session["LoggedIn"] != null && (bool)Session["LoggedIn"])
            {
                if (Request.Cookies[TokenManager.TokenCookieName] != null)
                {
                    welcomeLabel.Text = "Welcome to System 1!";
                    logoutbtn.Visible = true;
                }
                else
                {
                    Response.Redirect("web1.aspx");
                }
            }
            else
            {
                Response.Redirect("web1.aspx");
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

            if (Request.Cookies[TokenManager.TokenCookieName] != null)
            {
                string token = TokenHelper.DecryptToken(Request.Cookies[TokenManager.TokenCookieName].Value);
                string[] tokenData = token.Split('_');

                if (tokenData.Length == 3)
                {
                    bool isAzureADLogin = Convert.ToBoolean(tokenData[2]);
                    if (isAzureADLogin)
                    {
                        string authority = "https://login.microsoftonline.com/410d1846-1236-446b-85d6-b3aa69060f16";
                        string redirectUri = "https://localhost:44396/web1.aspx"; // 設定為登出後的回調 URL
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