using System;
using System.Web;
using static SingleSignOn.Login;

namespace SingleSignOn
{
    public partial class frontpage1 : System.Web.UI.Page
    {
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

            Session["LoggedIn"] = false; // 清除使用者登入狀態

            Response.Redirect("login1.aspx");
        }
    }
}