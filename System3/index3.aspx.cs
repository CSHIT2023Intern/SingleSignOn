using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static SingleSignOn.Login;

namespace SingleSignOn
{
    public partial class index3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null && (bool)Session["LoggedIn"])
            {
                if (Request.Cookies[TokenManager.TokenCookieName] != null)
                {
                    welcomeLabel.Text = "Welcome to System 3!";
                    logoutbtn.Visible = true;
                }
                else
                {
                    Response.Redirect("web3.aspx");
                }
            }
            else
            {
                Response.Redirect("web3.aspx");
            }
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
                    //string userAcc = tokenData[0];
                    bool isAzureADLogin = Convert.ToBoolean(tokenData[2]);
                    if (isAzureADLogin)
                    {
                        string authority = "https://login.microsoftonline.com/410d1846-1236-446b-85d6-b3aa69060f16";
                        string redirectUri = "https://localhost:44391/web3.aspx"; // 設定為登出後的回調 URL
                        string logoutUrl = $"{authority}/oauth2/v2.0/logout?post_logout_redirect_uri={HttpUtility.UrlEncode(redirectUri)}";

                        Session["LoggedIn"] = false; // 清除使用者登入狀態

                        Response.Redirect(logoutUrl);
                    }
                    else
                    {
                        Session["LoggedIn"] = false;
                        Response.Redirect("web3.aspx");
                    }
                }
            }
        }
    }
}