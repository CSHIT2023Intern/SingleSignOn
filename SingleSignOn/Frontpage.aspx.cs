using System;
using System.Web;
using static SingleSignOn.Login;

namespace SingleSignOn
{
    public partial class Frontpage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies[TokenManager.TokenCookieName] == null)
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                if(Request.Cookies["UserInformation"]!=null)
                {
                    string userFullName = Request.Cookies["UserInformation"]["FullName"];
                    title.Text = userFullName + ", Welcome to SingleSignOn!";
                }
                else
                {
                    title.Text = "歡迎進入單一登入系統!";
                }  
            }
        }

        protected void Btn_System1_Click(object sender, EventArgs e)
        {
            string returnUrl = "https://localhost:44328/Login.aspx";
            Response.Redirect("https://localhost:44350/frontpage1.aspx?returnUrl=" + returnUrl);
        }

        protected void Btn_System2_Click(object sender, EventArgs e)
        {
            string returnUrl = "https://localhost:44328/Login.aspx";
            Response.Redirect("https://localhost:44326/frontpage2.aspx?returnUrl=" + returnUrl);
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
                        string authority = "https://login.microsoftonline.com/f8cdef31-a31e-4b4a-93e4-5f571e91255a";
                        string redirectUri = "https://localhost:44328/Login.aspx"; // 設定為登出後的回調 URL
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