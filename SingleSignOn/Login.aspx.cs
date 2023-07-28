﻿using System;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using Microsoft.Identity.Client;
using System.Web.UI;

namespace SingleSignOn
{
    public partial class Login : System.Web.UI.Page
    {
        // 還要再修改
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (IsUserLoggedIn())
                {
                    string account = accountTextBox.Text; // Use a default or previously used account
                    TokenManager tokenManager = new TokenManager();

                    bool isAzureADLogin = false;
                    string token = tokenManager.GenerateToken(account, isAzureADLogin);

                    tokenManager.StoreToken(token);

                    HttpCookie tokenCookie = new HttpCookie("AuthToken", token);
                    Response.Cookies.Add(tokenCookie);

                    string returnUrl = Request.QueryString["returnUrl"];
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        string redirectUrl = $"{returnUrl}?token={HttpUtility.UrlEncode(token)}";
                        Response.Redirect(redirectUrl);
                    }
                    else
                    {
                        Response.Redirect("Index.aspx?token=" + HttpUtility.UrlEncode(token));
                    }
                }
            }
        }

        private bool IsUserLoggedIn()
        {
            return Session["LoggedIn"] != null && (bool)Session["LoggedIn"];
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string account = accountTextBox.Text;
            string password = passwordTextBox.Text;

            if (AuthenticateUser(account, password))
            {
                Session["LoggedIn"] = true;

                string returnUrl = Request.QueryString["returnUrl"];

                TokenManager tokenManager = new TokenManager();

                bool isAzureADLogin = false;
                string token = tokenManager.GenerateToken(account, isAzureADLogin);

                tokenManager.StoreToken(token);

                HttpCookie tokenCookie = new HttpCookie("AuthToken", token);
                Response.Cookies.Add(tokenCookie);

                //string returnUrl = Session["ReturnUrl"].ToString();
                //if (Session["ReturnUrl"] != null)
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    Session.Remove("ReturnUrl");

                    string redirectUrl = $"{returnUrl}?token={HttpUtility.UrlEncode(token)}";
                    Response.Redirect(redirectUrl);
                }
                else
                {
                    Response.Redirect("Index.aspx?token=" + HttpUtility.UrlEncode(token));
                }
            }
            else if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert", "alert('請輸入帳號及密碼！');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert", "alert('帳號及密碼填寫錯誤！');", true);
            }
        }

        protected void ADButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["returnUrl"]))
            {
                string returnUrl = Request.QueryString["returnUrl"];
                HttpCookie returnUrlCookie = new HttpCookie("ReturnUrlCookie", returnUrl);
                Response.Cookies.Add(returnUrlCookie);
                Response.Redirect("https://localhost:44342/");
            }
            else
            {
                string returnUrl = "https://localhost:44345/Index.aspx";
                HttpCookie returnUrlCookie = new HttpCookie("ReturnUrlCookie", returnUrl);
                Response.Cookies.Add(returnUrlCookie);
                Response.Redirect("https://localhost:44342/");
            }
        }



        private bool AuthenticateUser(string account, string password)
        {
            return (account == "test" && password == "0000");
        }

        public class TokenManager
        {
            string secretKey = ConfigurationManager.AppSettings["SecretKey"];
            //private static readonly string SecretKey = ConfigurationManager.AppSettings["SecretKey"];
            public static readonly string TokenCookieName = "CookieToken";

            // 靜態字典，用來追踪使用者帳號對應的Token
            // private static readonly Dictionary<string, string> activeUserTokens = new Dictionary<string, string>();

            public string GenerateToken(string account, bool isAzureADLogin)
            {
                string tokenData = $"{account}_{Guid.NewGuid()}_{isAzureADLogin}";
                return tokenData;
            }

            public void StoreToken(string token)
            {
                HttpCookie tokenCookie = new HttpCookie(TokenCookieName, $"{token}")
                {
                    Expires = DateTime.Now.AddMinutes(30), // 設置適當的過期時間
                    Domain = "localhost" // 設定為主域名
                };

                HttpContext.Current.Response.Cookies.Add(tokenCookie);
            }

            public bool ValidateToken(HttpRequestBase request, string token, out string account)
            {
                HttpCookie tokenCookie = request.Cookies[TokenCookieName];
                if (tokenCookie != null && !string.IsNullOrEmpty(tokenCookie.Value))
                {
                    string storedToken = tokenCookie.Value;
                    account = "";

                    // 解析 Token，取得 account
                    string[] tokenData = storedToken.Split('_');
                    if (tokenData.Length == 3)
                    {
                        account = tokenData[0];
                    }

                    // 驗證 Token 是否有效
                    return token == storedToken;
                }

                account = null;
                return false;
            }

            public static void CentralizedLogout()
            {
                HttpCookie tokenCookie = new HttpCookie(TokenCookieName)
                {
                    Expires = DateTime.Now.AddDays(-1),
                    Domain = "localhost"
                };
                HttpContext.Current.Response.Cookies.Add(tokenCookie);
            }
        }
    }
}