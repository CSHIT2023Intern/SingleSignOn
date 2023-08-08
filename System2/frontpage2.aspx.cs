using System;
using System.Web;
using static SingleSignOn.Login;

namespace SingleSignOn
{
    public partial class frontpage2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies[TokenManager.TokenCookieName] == null)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["returnUrl"]))
                {
                    string loginUrl = Request.QueryString["returnUrl"];
                    Response.Redirect(loginUrl);
                }
                else
                {
                    Response.Redirect("login2.aspx");
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.QueryString["returnUrl"]))
                {
                    if (Request.Cookies["UserInformation"] != null)
                    {
                        string userFullName = Request.Cookies["UserInformation"]["FullName"];
                        title.Text = userFullName + ", Welcome to System2!";
                    }
                    else
                    {
                        title.Text = "Welcome to System2!";
                    }
                }
                else
                {
                    if (Request.Cookies["UserInformation"] != null)
                    {
                        string userFullName = Request.Cookies["UserInformation"]["FullName"];
                        title.Text = userFullName + ", Welcome to System2!";
                    }
                    else
                    {
                        title.Text = "Welcome to System2!";
                    }
                }
            }
        }

        protected void Btn_Logout_Click(object sender, EventArgs e)
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

                        if (!string.IsNullOrEmpty(Request.QueryString["returnUrl"]))
                        {
                            string returnUrl = Request.QueryString["returnUrl"];
                            string logoutUrl = $"{authority}/oauth2/v2.0/logout?post_logout_redirect_uri={HttpUtility.UrlEncode(cleanCookieUri)}?redirectUrl={HttpUtility.UrlEncode(returnUrl)}";
                            Response.Redirect(logoutUrl);
                        }
                        else
                        {
                            string returnUrl = "https://localhost:44343/login2.aspx";
                            string logoutUrl = $"{authority}/oauth2/v2.0/logout?post_logout_redirect_uri={HttpUtility.UrlEncode(cleanCookieUri)}?redirectUrl={HttpUtility.UrlEncode(returnUrl)}";
                            Response.Redirect(logoutUrl);
                        }
                    }
                    else
                    {
                        TokenManager.CentralizedLogout();

                        if (!string.IsNullOrEmpty(Request.QueryString["returnUrl"]))
                        {
                            string returnUrl = Request.QueryString["returnUrl"];
                            Response.Redirect(returnUrl);
                        }
                        else
                        {
                            Response.Redirect("login2.aspx");
                        }
                    }
                }
            }
        }
    }
}