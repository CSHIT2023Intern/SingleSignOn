using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static SingleSignOn.Login;

namespace SingleSignOn
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies[TokenManager.TokenCookieName] == null)
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                welcomeLabel.Text = "Welcome!";
            }
        }

        protected void Web1btn_Click(object sender, EventArgs e)
        {
            string returnUrl = "https://localhost:44345/Login.aspx";
            Response.Redirect("https://localhost:44396/web1.aspx?returnUrl=" + returnUrl);
        }

        protected void Web2btn_Click(object sender, EventArgs e)
        {
            string returnUrl = "https://localhost:44345/Login.aspx";
            Response.Redirect("https://localhost:44343/web2.aspx?returnUrl=" + returnUrl);
        }

        protected void Web3btn_Click(object sender, EventArgs e)
        {
            string returnUrl = "https://localhost:44345/Login.aspx";
            Response.Redirect("https://localhost:44391/web3.aspx?returnUrl=" + returnUrl);
        }

        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            TokenManager.CentralizedLogout();

            HttpCookie tokenCookie = Request.Cookies[TokenManager.TokenCookieName];
            if (tokenCookie != null)
            {
                string token = TokenHelper.DecryptToken(tokenCookie.Value);
                string[] tokenData = token.Split('_');

                if (tokenData.Length == 3 && bool.TryParse(tokenData[2], out bool isAzureADLogin))
                {
                    if (isAzureADLogin)
                    {
                        string authority = "https://login.microsoftonline.com/410d1846-1236-446b-85d6-b3aa69060f16";
                        string redirectUri = "https://localhost:44345/Login.aspx";
                        string logoutUrl = $"{authority}/oauth2/v2.0/logout?post_logout_redirect_uri={HttpUtility.UrlEncode(redirectUri)}";
                        Response.Redirect(logoutUrl);
                    }
                    else
                    {
                        Response.Redirect("Login.aspx");
                    }
                }
            }
        }
    }
}