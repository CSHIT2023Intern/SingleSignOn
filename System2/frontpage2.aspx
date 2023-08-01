<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frontpage2.aspx.cs" Inherits="SingleSignOn.frontpage2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>System2 FrontPage</title>
    <style>
        *{
            font-family:標楷體;  
        }
        body{
            background-color:#E4EFF6;
        }
        #title{
             width: 200px;
             height: 20px;
             margin: 10px;
             color: #3387BF;
        }
        #container{
            /*margin: 50px;*/
            padding: 15px;
            width: 330px;
            height: 300px;
            background-color: #c3d8e7;
            border-radius: 30px;
            border-style:solid;
            border-color:#5293BF;
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
        .tab{
            height: 105px;
        }
        #btnLogout{
            color: white;  
            background: #8DB6CD;
            width: 200px;
            height: 30px;
            margin: 10px;
            padding: 5px;
            border-radius:30px;
            border-color:#5293BF; border-style:solid;
        }
        #btnLogout:hover{
            background: #5382A2;
        }
    </style>
</head>
<body>
    <div class="system_name">
        <h2>系統2</h2>
    </div>

    <div class="front_page">
        <div id="container">
            <div class="index">
                <asp:Label ID="title" runat="server" Text="系統2-首頁"></asp:Label>

                <form runat="server">
                    <div class="tab"></div>
                    <div class="tab"></div>
                    <asp:Button ID="btnLogout" runat="server" Text="登出" OnClick="Btn_Logout_Click" />
                </form>
            </div>
        </div>
    </div>
</body>
</html>
