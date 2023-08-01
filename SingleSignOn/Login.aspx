<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SingleSignOn.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>SSO登入頁面</title>

    <style>
        *{
            font-family:標楷體;  
        }
        body{
            background-color:#E4EFF6;
            
        }
        h3{
             width: 200px;
             height: 20px;
             margin: 10px;
             color: #2987B4;
        }
        #account, #password{
             width: 150px;
             height: 20px;
             margin: 10px;
             color: #2987B4;
        }
        #container{
            /*margin: 50px;*/
            padding: 15px;
            width: 230px;
            height: 230px;
            background-color: #c3d8e7;
            border-radius: 30px;
            border-style:solid;
            box-shadow: 0 0px 70px rgba(0, 0, 0, 0.1);
            border-color:#5293BF;
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
            background: #8DB6CD;
            width: 200px;
            height: 30px;
            margin: 10px;
            padding: 5px;
            border-radius: 30px;
            border-color:#5293BF;
            
            border-style:solid;
        }
        #Btn_Login:hover, #Btn_AzureAD:hover{
            background: #5382A2;
        }
        TextBox{
            padding: 5px;
            border: none; 
            border:solid 1px #ccc;
            border-radius: 30px;
        }
    </style>
</head>
<body>
    <div class="system_name">
      <h2>單一登入系統</h2>
    </div>
    
    <div class="login_page">
        <div id="container">
            <div class="login">
                <h3>登入</h3>
                
                <form runat="server">
                    帳號:<asp:TextBox ID="account" runat="server" placeholder="請輸入帳號"></asp:TextBox><br />
                    <div class="tab"></div>
                    密碼:<asp:TextBox ID="password" runat="server" placeholder="請輸入密碼" TextMode="Password"></asp:TextBox><br />
                    <div class="tab"></div>
                    <asp:Button ID="Btn_Login" runat="server" Text="登入" OnClick="Btn_Login_Click" />
                    <asp:Button ID="Btn_AzureAD" runat="server" Text="使用Azure AD登入" OnClick="Btn_AzureAD_Click" />
                </form>
            </div>
        </div>
    </div>
</body>
</html>
