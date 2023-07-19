using System;
using System.Web;
using static SingleSignOn.Login;

namespace SingleSignOn
{
    public partial class frontpage2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null && (bool)Session["LoggedIn"])
            {
                if (Request.Cookies[TokenManager.TokenCookieName] != null)
                {
                    title.Text = "Welcome to System2！";
                    btnLogout.Visible = true;
                }
                else
                {
                    Response.Redirect("login2.aspx");
                }
            }
            else
            {
                Response.Redirect("login2.aspx");
            }
        }

        protected void Btn_Logout_Click(object sender, EventArgs e)
        {
            TokenManager.CentralizedLogout();

            Session["LoggedIn"] = false; // 清除使用者登入狀態

            Response.Redirect("login2.aspx");
        }
    }
}