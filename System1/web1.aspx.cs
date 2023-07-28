﻿using System;
using System.Diagnostics;
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
                    if (tokenManager.ValidateToken(new HttpRequestWrapper(Request), token, out string account))
                    {
                        Session["LoggedIn"] = true;
                        Session["user"] = account;
                        Response.Redirect("index1.aspx");
                    }
                    else
                    {
                        Response.Redirect("https://localhost:44345/Login.aspx");
                    }
                }
                else
                {
                    Response.Redirect($"https://localhost:44345/Login.aspx?returnUrl={Server.UrlEncode(Request.Url.ToString())}");
                }
            }
        }
    }
}