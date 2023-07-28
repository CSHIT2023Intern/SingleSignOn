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
            if (Session["LoggedIn"] == null || !(bool)Session["LoggedIn"])
            {
                Response.Redirect("Login.aspx");
            }
            else if (Request.Cookies[TokenManager.TokenCookieName] == null)
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
            Response.Redirect("https://localhost:44396/index1.aspx");
        }

        protected void Web2btn_Click(object sender, EventArgs e)
        {
            Response.Redirect("https://localhost:44343/index2.aspx");
        }

        protected void Web3btn_Click(object sender, EventArgs e)
        {
            Response.Redirect("https://localhost:44391/index3.aspx");
        }

        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            TokenManager.CentralizedLogout();

            if (Request.Cookies[TokenManager.TokenCookieName] != null)
            {
                string token = TokenHelper.DecryptToken(Request.Cookies[TokenManager.TokenCookieName].Value);
                string[] tokenData = token.Split('_');

                if (tokenData.Length == 3 && bool.TryParse(tokenData[2], out bool isAzureADLogin))
                {
                    string authority = "https://login.microsoftonline.com/410d1846-1236-446b-85d6-b3aa69060f16";
                    string redirectUri = "https://localhost:44345/Login.aspx"; // 設定為登出後的回調 URL
                    string logoutUrl = $"{authority}/oauth2/v2.0/logout?post_logout_redirect_uri={HttpUtility.UrlEncode(redirectUri)}";

                    Session["LoggedIn"] = false;
                    Response.Redirect(isAzureADLogin ? logoutUrl : "Login.aspx");
                }
            }
        }
    }
}