<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index3.aspx.cs" Inherits="SSO.index3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" type="text/css" href="style3.css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0/dist/css/bootstrap.min.css" />

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0/dist/js/bootstrap.bundle.min.js"></script>

    <title>System 3's Index</title>

    <style>
        body {
            min-height: 100vh;
            background: linear-gradient(-45deg, #ffffff 25%, #e0e0e0 25%, #e0e0e0 50%, #ffffff 50%, #ffffff 75%, #e0e0e0 75%);
        }
    </style>
</head>
<body style="height: 97.7vh;">
    <form id="form1" runat="server">
        <div style="background-color: white; width: 100%; height: 7%; margin-left: 0px;">
            <asp:Label ID="title3" runat="server" Text="中山醫學大學 ee-class"></asp:Label>

            <asp:Button ID="logoutbtn" runat="server" Text="登出" OnClick="LogoutButton_Click" Style="margin-top: 10px;" />
            <asp:Label ID="welcomeLabel" runat="server" Text=""></asp:Label>
        </div>
    </form>
</body>
</html>
