<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SingleSignOn.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>SSO登入頁面</title>

    <style>
        *{
            font-family:微軟正黑體;  
        }
        body{
            background-color: white;
        }
        h3{
             width: 200px;
             height: 20px;
             margin: 10px;
             color: #f7b3cb;
        }
        #account, #password{
             width: 150px;
             height: 20px;
             margin: 10px;
             color: #f7b3cb;
        }
        #container{
            /*margin: 50px;*/
            padding: 15px;
            width: 230px;
            height: 230px;
            background-color: white;
            border-radius: 5px;
            border-top: 10px solid  #f7b3cb;
            box-shadow: 0 0px 70px rgba(0, 0, 0, 0.1);
  
            /*定位對齊*/
            position:relative;   
            margin: auto;
            top: 100px;
            text-align:center;  
        }
        .system_name{
            /*定位對齊*/
            position:relative;   
            margin: auto;
            top: 100px;
            text-align:center;
        }
        #Btn_Login, #Btn_AzureAD{
            color: white;  
            background: #f7b3cb;
            width: 200px;
            height: 30px;
            margin: 10px;
            padding: 5px;
            border-radius: 5px;
            border: 0px;
        }
        #Btn_Login:hover, #Btn_AzureAD:hover{
            background: #cff3fd;
        }
        TextBox{
            padding: 5px;
            border: none; 
            border:solid 1px #ccc;
            border-radius: 5px;
        }
    </style>
</head>
<body>
    <div class="system_name">
      <h2>Single SignOn系統</h2>
    </div>
    
    <div class="login_page">
        <div id="container">
            <div class="login">
                <h3>登入 Login</h3>
                
                <form runat="server">
                    帳號:<asp:TextBox ID="account" runat="server" placeholder="輸入帳號"></asp:TextBox><br />
                    <div class="tab"></div>
                    密碼:<asp:TextBox ID="password" runat="server" placeholder="輸入密碼" TextMode="Password"></asp:TextBox><br />
                    <div class="tab"></div>
                    <asp:Button ID="Btn_Login" runat="server" Text="登入" OnClick="Btn_Login_Click" />
                    <asp:Button ID="Btn_AzureAD" runat="server" Text="以Azure AD登入" OnClick="Btn_AzureAD_Click" />
                </form>
            </div>
        </div>
    </div>
</body>
</html>
