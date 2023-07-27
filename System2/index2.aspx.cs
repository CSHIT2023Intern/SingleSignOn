using System;
using System.Web;
using static SingleSignOn.Login;

namespace SingleSignOn
{
    public partial class index2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null && (bool)Session["LoggedIn"])
            {
                if (Request.Cookies[TokenManager.TokenCookieName] != null)
                {
                    welcomeLabel.Text = "Welcome to System 2!";
                    logoutbtn.Visible = true;
                }
                else
                {
                    Response.Redirect("web2.aspx");
                }
            }
            else
            {
                Response.Redirect("web2.aspx");
            }
        }

        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            TokenManager.CentralizedLogout();

            // 檢查是否有儲存token的cookie存在並取得token的值
            if (Request.Cookies[TokenManager.TokenCookieName] != null)
            {
                string token = Request.Cookies[TokenManager.TokenCookieName].Value;

                // 分割token以取得使用者帳號和isAzureADLogin標誌
                string[] tokenData = token.Split('_');
                if (tokenData.Length == 3)
                {
                    //string account = tokenData[0];
                    bool isAzureADLogin = Convert.ToBoolean(tokenData[2]);
                    if (isAzureADLogin)
                    {
                        // 取得 Azure AD 登出 URL
                        string authority = "https://login.microsoftonline.com/9902d6cb-2777-42b8-8d31-31a3f6db7e74"; // 設定你的 Azure AD 中的租戶 ID
                        string redirectUri = "https://localhost:44345/Logout.aspx"; // 設定為登出後的回調 URL
                        string logoutUrl = $"{authority}/oauth2/v2.0/logout?post_logout_redirect_uri={HttpUtility.UrlEncode(redirectUri)}";

                        Session["LoggedIn"] = false;

                        Response.Redirect(logoutUrl);
                    }
                    else
                    {
                        // 本地帳號/密碼 登出
                        Session["LoggedIn"] = false;
                        Response.Redirect("web2.aspx");
                    }
                }
            }
        }
    }
}