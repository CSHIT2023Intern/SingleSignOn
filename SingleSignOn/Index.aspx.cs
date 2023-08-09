using System;
using System.Web;
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
                if (Request.Cookies["UserInformation"] != null)
                {
                    string userFullName = Request.Cookies["UserInformation"]["FullName"];
                    welcomeLabel.Text = userFullName + ", Welcome to SingleSignOn!";
                }
                else
                {
                    welcomeLabel.Text = "Welcome to SingleSignOn!";
                }
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
                        string cleanCookieUri = "https://localhost:44345/Logout.aspx";
                        string redirectUri = "https://localhost:44345/Login.aspx";
                        string logoutUrl = $"{authority}/oauth2/v2.0/logout?post_logout_redirect_uri={HttpUtility.UrlEncode(cleanCookieUri)}?redirectUrl={HttpUtility.UrlEncode(redirectUri)}";
                        Response.Redirect(logoutUrl);
                    }
                    else
                    {
                        TokenManager.CentralizedLogout();
                        Response.Redirect("Login.aspx");
                    }
                }
            }
        }
    }
}