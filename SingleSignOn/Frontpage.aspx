<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Frontpage.aspx.cs" Inherits="SingleSignOn.Frontpage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Single SignOn Frontpage</title>

    <style>
        *{
            font-family:標楷體;  
        }
        body{
            background-color: #E4EFF6;
        }
        #title{
             width: 200px;
             height: 20px;
             margin: 10px;
             color: #2987B4;
        }
        #container{
            /*margin: 50px;*/
            padding: 15px;
            width: 230px;
            background-color:#c3d8e7;
            border-radius: 30px;
            border-color:#5293BF; border-style:solid;
            box-shadow: 0 0px 70px rgba(0, 0, 0, 0.1);
            display: flex;
            justify-content: space-between;
            margin: auto;
  
            /*定位對齊*/
            position:relative;   
            margin: auto;
            top: 100px;
            text-align:center;  
        }
        .tab{
            height: 40px;
        }
        .system_name{
            /*定位對齊*/
            position:relative;   
            margin: auto;
            top: 100px;
            text-align:center;
        }
        #Btn_System1, #Btn_System2, #Btn_Logout{
            color: white;  
            background: #8DB6CD;
            width: 200px;
            height: 30px;
            margin: 10px;
            padding: 5px;
            border-radius: 30px;
            border-color:#5293BF; border-style:solid;
        }
        #Btn_System1:hover, #Btn_System2:hover, #Btn_Logout:hover{
            background: #5382A2;
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
                <asp:Label ID="title" runat="server" Text="單一登入 首頁"></asp:Label>
                
                <form runat="server">
                    <div class="tab"></div>
                    <asp:Button ID="Btn_System1" runat="server" Text="系統1" OnClick="Btn_System1_Click" />
                    <div class="tab"></div>
                    <asp:Button ID="Btn_System2" runat="server" Text="系統2" OnClick="Btn_System2_Click" />
                    <div class="tab"></div>
                    <asp:Button ID="Btn_Logout" runat="server" Text="登出" OnClick="Btn_Logout_Click" />
                </form>
            </div>
        </div>
    </div>
</body>
</html>

