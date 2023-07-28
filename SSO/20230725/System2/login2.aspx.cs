using System;
using System.Web;
using static SingleSignOn.Login;

namespace SingleSignOn
{
    public partial class login2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies[TokenManager.TokenCookieName] != null)
                {
                    string token = Request.Cookies[TokenManager.TokenCookieName].Value;

                    TokenManager tokenManager = new TokenManager();

                    bool isValidToken = tokenManager.ValidateToken(new HttpRequestWrapper(Request), token, out string userAcc);

                    if (isValidToken == true)
                    {
                        // 記錄用戶已經在其他系統登入過，以便其他子站使用
                        Session["LoggedIn"] = true;

                        // 儲存使用者帳號名稱
                        Session["user"] = userAcc;

                        Response.Redirect("frontpage2.aspx");
                    }
                    else
                    {
                        Response.Redirect("https://localhost:44345/Login.aspx");
                    }
                }
                else
                {
                    string returnUrl = Server.UrlEncode(Request.Url.ToString());
                    Response.Redirect("https://localhost:44345/Login.aspx?returnUrl=" + returnUrl);
                }
            }
        }
    }
}