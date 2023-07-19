<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index2.aspx.cs" Inherits="SSO.index2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="style2.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0/dist/js/bootstrap.bundle.min.js"></script>

    <title>System 2's Index</title>

</head>
<body style="height: 97.7vh;">

    <form id="form1" runat="server">
        <div style="background-color: #47B7DD; width: 100%; height: 7%; margin-left: 0px; text-align: center;">
            <asp:Image ID="logoImage" runat="server" ImageUrl="../imgs/csmulogo.jpg" Style="float: left;" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="title2" runat="server" Text="選 課 系 統"></asp:Label>

            <asp:Button ID="logoutbtn" runat="server" Text="登出" OnClick="LogoutButton_Click" />
            <asp:Label ID="welcomeLabel" runat="server" Text=""></asp:Label>
        </div>

        <!-- OuterTab -->
        <div class="btn-group" role="group" aria-label="Button group with nested dropdown">
            <div class="btn-group" role="group">
                <button id="btnGroupDrop1" type="button" style="border-radius: 0;" class="btn btn-secondary btn-sm dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false">
                    選課作業
                </button>
                <ul class="dropdown-menu" aria-labelledby="btnGroupDrop1" style="font-size: 14px;">
                    <li><a class="dropdown-item" href="#">初選作業</a></li>
                    <li><a class="dropdown-item" href="#">初選結果查詢</a></li>
                    <li><a class="dropdown-item" href="#">[新]線上退選</a></li>
                    <li><a class="dropdown-item" href="#">[新]線上加選</a></li>
                    <li><a class="dropdown-item" href="#">第2次線上加選</a></li>
                    <li><a class="dropdown-item" href="#">選課單確認作業</a></li>
                </ul>
            </div>

            <div class="btn-group" role="group">
                <button id="btnGroupDrop2" type="button" style="border-radius: 0;" class="btn btn-secondary btn-sm dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false">
                    課程查詢
                </button>
                <ul class="dropdown-menu" aria-labelledby="btnGroupDrop2" style="font-size: 14px;">
                    <li><a class="dropdown-item" href="#">查詢課表</a></li>
                    <li><a class="dropdown-item" href="#">不可修/選科目</a></li>
                    <li><a class="dropdown-item" href="#">未開成課程</a></li>
                </ul>
            </div>

            <div class="btn-group" role="group">
                <button id="btnGroupDrop3" type="button" style="border-radius: 0;" class="btn btn-secondary btn-sm dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false">
                    成績查詢
                </button>
                <ul class="dropdown-menu" aria-labelledby="btnGroupDrop3" style="font-size: 14px;">
                    <li><a class="dropdown-item" href="#">歷年成績</a></li>
                </ul>
            </div>
        </div>

        <div style="background-color: #4F4842; width: 100%; height: 7%; text-align: center; position: absolute; bottom: 0; font-size: 14px;">
            <p style="color: white;">
                適用於IE6以上之瀏覽器 | 最佳解析度：1024*768<br />
                版權所有©2012 中山醫學大學 Chung Shan Medical University, All right reserved.
            </p>
        </div>
    </form>
</body>
</html>
