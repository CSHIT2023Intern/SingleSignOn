using System;
using System.Web;
using static SingleSignOn.Login;

namespace SingleSignOn
{
    public partial class web1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies[TokenManager.TokenCookieName] != null)
                {
                    string token = Request.Cookies[TokenManager.TokenCookieName].Value;

                    TokenManager tokenManager = new TokenManager();

                    bool isValidToken = tokenManager.ValidateToken(new HttpRequestWrapper(Request), token, out string account);

                    if (isValidToken == true)
                    {
                        // 儲存使用者帳號名稱
                        Session["user"] = account;

                        if (!string.IsNullOrEmpty(Request.QueryString["returnUrl"]))
                        {
                            string returnUrl = Request.QueryString["returnUrl"];
                            Response.Redirect("https://localhost:44396/index1.aspx?returnUrl=" + returnUrl);
                        }

                        Response.Redirect("web1.aspx");
                    }

                    Response.Redirect("https://localhost:44345/Login.aspx");
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