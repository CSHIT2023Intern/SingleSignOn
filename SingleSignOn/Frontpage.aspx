<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Frontpage.aspx.cs" Inherits="SingleSignOn.Frontpage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Single SignOn Frontpage</title>

    <style>
        *{
            font-family:微軟正黑體;  
        }
        body{
            background-color: white;
        }
        #title{
             width: 200px;
             height: 20px;
             margin: 10px;
             color: #f7b3cb;
        }
        #container{
            /*margin: 50px;*/
            padding: 15px;
            width: 230px;
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
        #Btn_System1, #Btn_System2, #Btn_Logout{
            color: white;  
            background: #f7b3cb;
            width: 200px;
            height: 30px;
            margin: 10px;
            padding: 5px;
            border-radius: 5px;
            border: 0px;
        }
        #Btn_System1:hover, #Btn_System2:hover, #Btn_Logout:hover{
            background: #cff3fd;
        }
    </style>
</head>
<body>
    <div class="system_name">
      <h2>Single SignOn</h2>
    </div>
    
    <div class="login_page">
        <div id="container">
            <div class="login">
                <asp:Label ID="title" runat="server" Text="Single SignOn 首頁"></asp:Label>
                
                <form runat="server">
                    <div class="tab"></div>
                    <asp:Button ID="Btn_System1" runat="server" Text="System1" OnClick="Btn_System1_Click" />
                    <div class="tab"></div>
                    <asp:Button ID="Btn_System2" runat="server" Text="System2" OnClick="Btn_System2_Click" />
                    <div class="tab"></div>
                    <asp:Button ID="Btn_Logout" runat="server" Text="登出" OnClick="Btn_Logout_Click" />
                </form>
            </div>
        </div>
    </div>
</body>
</html>
