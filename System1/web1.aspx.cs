using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SSO
{
    public partial class web1 : System.Web.UI.Page
    {
        //protected void LoginButton_Click(object sender, EventArgs e)
        //{
        //    string returnUrl = Request.Url.AbsoluteUri;
        //    Response.Redirect("https://localhost:44303/Login.aspx?returnUrl=" + Server.UrlEncode(returnUrl));
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (Session["UserLoggedIn"] == null || !(bool)Session["UserLoggedIn"])
                //    {
                //    string returnUrl = Request.Url.AbsoluteUri;
                //    Response.Redirect("https://localhost:44303/Login.aspx?returnUrl=" + Server.UrlEncode(returnUrl));
                //}
                //else
                //{
                if (Request.Cookies["Token"] != null || Session["Account"] != null)
                {
                    string token = Request.Cookies["Token"].Value;
                    if (ValidateToken(token))
                    {
                        Response.Redirect("https://localhost:44375/index1.aspx");
                    }
                    else
                    {
                        errorLabel.Text = "Invalid token. Access denied.";
                    }
                }
                else
                {
                    string returnUrl = Request.Url.AbsoluteUri;
                    Response.Redirect("https://localhost:44303/Login.aspx?returnUrl=" + Server.UrlEncode(returnUrl));
                }
                //}
            }
        }

        //// 檢查 User 是否已登入
        //private bool UserLoggedIn()
        //{
        //    string account = Session["Account"] as string;
        //    return !string.IsNullOrEmpty(account);
        //}

        private bool ValidateToken(string token)
        {
            HttpCookie cookie = Request.Cookies["Token"];
            if (cookie != null)
            {
                string storedToken = cookie.Value;
                return string.Equals(token, storedToken);
            }
            return false;
        }
    }
}