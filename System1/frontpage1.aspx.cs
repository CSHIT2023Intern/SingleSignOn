﻿using System;
using System.Web;
using static SingleSignOn.Login;

namespace SingleSignOn
{
    public partial class frontpage1 : System.Web.UI.Page
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
                Response.Redirect("login1.aspx");
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.QueryString["returnUrl"]))
                {
                    title.Text = "Welcome to System1!";
                }
                title.Text = "Welcome to System1!";
            }
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

                        if (!string.IsNullOrEmpty(Request.QueryString["returnUrl"]))
                        {
                            string returnUrl = Request.QueryString["returnUrl"];
                            string logoutUrl = $"{authority}/oauth2/v2.0/logout?post_logout_redirect_uri={HttpUtility.UrlEncode(returnUrl)}";
                            Response.Redirect(logoutUrl);
                        }
                        else
                        {
                            string returnUrl = "https://localhost:44350/login1.aspx";
                            string logoutUrl = $"{authority}/oauth2/v2.0/logout?post_logout_redirect_uri={HttpUtility.UrlEncode(returnUrl)}";
                            Response.Redirect(logoutUrl);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Request.QueryString["returnUrl"]))
                        {
                            string returnUrl = Request.QueryString["returnUrl"];
                            Response.Redirect(returnUrl);
                        }
                        else
                        {
                            Response.Redirect("login1.aspx");
                        }
                    }
                }
            }
        }
    }
}