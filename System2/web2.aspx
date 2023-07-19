<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="web2.aspx.cs" Inherits="SSO.web2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="style2.css" />
    <title>System2</title>
</head>
<body>
    <form id="loginForm" runat="server" style="margin: 0 auto; width: 300px; text-align: center;">
        <br />
        <p>
            <asp:Label ID="Label1" runat="server" Text="System 2" Style="font-family: 'Times New Roman', 'DFKai-sb'; font-size: 35px;"></asp:Label>
        </p>
        <%--<p>
            <asp:Button ID="loginButton" runat="server" Text="登入" Style="font-size: 18px; font-family:'Times New Roman', 'DFKai-sb'; border-radius: 3px;" OnClick="LoginButton_Click" />
        </p>--%>
        <asp:Label ID="welcomeLabel" runat="server" Text=""></asp:Label>
        <br />
        <asp:Label ID="errorLabel" runat="server" Text=""></asp:Label>
    </form>
</body>
</html>