using System;
using System.Web;

namespace SSO
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 檢查 Session 是否有儲存 id_token
            if (Session["id_token"] != null)
            {
                string username = Session["Account"] as string;
                welcomeLabel.Text = "Welcome, " + username + "!";
            }
            else
            {
                Response.Redirect("https://localhost:44303/Login.aspx");
            }

            //// 檢查使用者是否已登入
            //if (!User.Identity.IsAuthenticated)
            //{
            //    Response.Redirect("Login.aspx");
            //}
            //else
            //{
            //    string username = Session["Account"] as string;
            //    welcomeLabel.Text = "Welcome, " + username + "!";
            //}

            //if (!IsPostBack)
            //{
            //    //if (Request.Cookies["Token"] != null || Session["Account"] != null)
            //    //{
            //    //string token = Request.Cookies["Token"].Value;
            //    string token = Request.QueryString["token"];
            //    string generatedToken = Session["GeneratedToken"] as string;

            //    if (ValidateToken(token, generatedToken))
            //    {
            //        string username = Session["Account"] as string;
            //        welcomeLabel.Text = "Welcome, " + username + "!";
            //    }
            //    else
            //    {
            //        Logout();
            //    }
            //    //}
            //    //else
            //    //{
            //    //    Logout();
            //    //}
            //}
        }

        protected void Web1btn_Click(object sender, EventArgs e)
        {
            RedirectWithToken("https://localhost:44375/index1.aspx");
        }

        protected void Web2btn_Click(object sender, EventArgs e)
        {
            RedirectWithToken("https://localhost:44351/index2.aspx");
        }

        protected void Web3btn_Click(object sender, EventArgs e)
        {
            RedirectWithToken("https://localhost:44374/index3.aspx");
        }

        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("https://localhost:44303/Logout.aspx");
        }

        private void RedirectWithToken(string targetUrl)
        {
            string token = Request.QueryString["token"];
            string generatedToken = Session["GeneratedToken"] as string;

            if (ValidateToken(token, generatedToken))
            {
                Response.Redirect(targetUrl + "?token=" + HttpUtility.UrlEncode(token));
            }
            else
            {
                Response.Redirect("https://localhost:44303/Login.aspx");
            }
        }

        private bool ValidateToken(string token, string generatedToken)
        {
            //HttpCookie cookie = Request.Cookies["Token"];
            //if (cookie != null)
            //{
            //    string storedToken = cookie.Value;
            //    return string.Equals(token, storedToken);
            //}
            //return false;

            return string.Equals(token, generatedToken);
        }

        //private void StoreToken(string token)
        //{
        //    HttpCookie cookie = new HttpCookie("Token", token)
        //    {
        //        Expires = DateTime.Now.AddDays(1) // 設定 cookie 的過期時間
        //    };
        //    Response.Cookies.Add(cookie);
        //}

        //private void Logout()
        //{
        //    RemoveToken();
        //    Session.Clear();
        //    Session.Abandon();

        //    Response.Redirect("https://localhost:44303/Login.aspx");
        //}

        //private void RemoveToken()
        //{
        //    HttpCookie cookie = Request.Cookies["Token"];
        //    if (cookie != null)
        //    {
        //        cookie.Expires = DateTime.Now.AddDays(-1);
        //        Response.Cookies.Add(cookie);
        //    }
        //}
    }
}