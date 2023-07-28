using System;
using System.Web;
using static SingleSignOn.Login;

namespace SingleSignOn
{
    public partial class index2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] == null || !(bool)Session["LoggedIn"] || Request.Cookies[TokenManager.TokenCookieName] == null)
            {
                Response.Redirect("web2.aspx");
            }
            else
            {
                welcomeLabel.Text = "Welcome to System 2!";
            }
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
                    string redirectUri = "https://localhost:44343/web2.aspx"; // 設定為登出後的回調 URL
                    string logoutUrl = $"{authority}/oauth2/v2.0/logout?post_logout_redirect_uri={HttpUtility.UrlEncode(redirectUri)}";

                    Session["LoggedIn"] = false;
                    Response.Redirect(isAzureADLogin ? logoutUrl : "web2.aspx");
                }
            }
        }
    }
}