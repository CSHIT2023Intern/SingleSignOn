using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static SingleSignOn.Login;

namespace SingleSignOn
{
    public partial class Frontpage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null && (bool)Session["LoggedIn"])
            {
                if (Request.Cookies[TokenManager.TokenCookieName] != null)
                {
                    title.Text = "Welcome to SingleSignOn！";
                    Btn_Logout.Visible = true;
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }

        protected void Btn_System1_Click(object sender, EventArgs e)
        {
            Response.Redirect("https://localhost:44396/frontpage1.aspx");
        }

        protected void Btn_System2_Click(object sender, EventArgs e)
        {
            Response.Redirect("https://localhost:44343/frontpage2.aspx");
        }

        protected void Btn_Logout_Click(object sender, EventArgs e)
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
                        // 取得 Azure AD 登出 URL
                        string authority = "https://login.microsoftonline.com/410d1846-1236-446b-85d6-b3aa69060f16";
                        string redirectUri = "https://localhost:44345/Login.aspx"; // 設定為登出後的回調 URL
                        string logoutUrl = $"{authority}/oauth2/v2.0/logout?post_logout_redirect_uri={HttpUtility.UrlEncode(redirectUri)}";

                        Session["LoggedIn"] = false; // 清除使用者登入狀態

                        Response.Redirect(logoutUrl);
                    }
                    else
                    {
                        // 本地帳號/密碼 登出
                        Session["LoggedIn"] = false;
                        Response.Redirect("Login.aspx");
                    }
                }
            }
        }
    }
}