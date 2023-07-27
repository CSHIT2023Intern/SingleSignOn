using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Web;
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
                    welcomeLabel.Text = "Welcome to System1！";
                    logoutbtn.Visible = true;
                }
                else
                {
                    Response.Redirect("login1.aspx");
                }
            }
            else
            {
                Response.Redirect("login1.aspx");
            }
        }

        protected void Web2btn_Click(object sender, EventArgs e)
        {
            Response.Redirect("https://localhost:44343/web2.aspx");

            //string url = "https://localhost:44343/web2.aspx";
            //string script = "window.open('" + url + "', '_blank');";
            //ScriptManager.RegisterStartupScript(this, GetType(), "OpenNewTab", script, true);
        }

        protected void Web3btn_Click(object sender, EventArgs e)
        {
            Response.Redirect("https://localhost:44379/web3.aspx");

            //string url = "https://localhost:44379/web3.aspx";
            //string script = "window.open('" + url + "', '_blank');";
            //ScriptManager.RegisterStartupScript(this, GetType(), "OpenNewTab", script, true);
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
                    //string userAcc = tokenData[0];
                    bool isAzureADLogin = Convert.ToBoolean(tokenData[2]);
                    if (isAzureADLogin)
                    {
                        // 取得 Azure AD 登出 URL
                        string authority = "https://login.microsoftonline.com/9902d6cb-2777-42b8-8d31-31a3f6db7e74"; // 設定你的 Azure AD 中的租戶 ID
                        string redirectUri = "https://localhost:44396/web1.aspx"; // 設定為登出後的回調 URL
                        string logoutUrl = $"{authority}/oauth2/v2.0/logout?post_logout_redirect_uri={HttpUtility.UrlEncode(redirectUri)}";

                        Session["LoggedIn"] = false; // 清除使用者登入狀態

                        Response.Redirect(logoutUrl);
                    }
                    else
                    {
                        // 本地帳號/密碼 登出
                        Session["LoggedIn"] = false;
                        Response.Redirect("web1.aspx");
                    }
                }
            }
        }

        /*
        protected void Page_Load(object sender, EventArgs e)
        { 
            if (Session["LoggedIn"] != null && (bool)Session["LoggedIn"])
            {
                if (Request.Cookies[TokenManager.TokenCookieName] != null)
                {
                    title.Text = "Welcome to System1！";
                    btnLogout.Visible = true;
                }
                else
                {
                    Response.Redirect("login1.aspx");
                }
            }
            else
            {
                Response.Redirect("login1.aspx");
            }
        }

        protected void Btn_Logout_Click(object sender, EventArgs e)
        {
            TokenManager.CentralizedLogout();

            // 取得 Azure AD 登出 URL
            string authority = "https://login.microsoftonline.com/9902d6cb-2777-42b8-8d31-31a3f6db7e74"; // 設定你的 Azure AD 中的租戶 ID
            string redirectUri = "https://localhost:44396/web1.aspx"; // 設定為登出後的回調 URL
            string logoutUrl = $"{authority}/oauth2/v2.0/logout?post_logout_redirect_uri={HttpUtility.UrlEncode(redirectUri)}";

            Session["LoggedIn"] = false; // 清除使用者登入狀態

            Response.Redirect(logoutUrl);
        }*/
    }
}