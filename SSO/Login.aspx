<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SSO.Login"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="style.css" />
    <title>CSHIT-Intern SSO</title>
</head>
<body style="background-color: black;">
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <form id="loginForm" runat="server">
        <br />
        <br />
        <br />
        <p>
            <asp:Label ID="title" runat="server" Text="Single Sign On System"></asp:Label>
        </p>
        <br />
        <br />
        <p>
            <asp:Label ID="accountLabel" runat="server" AssociatedControlID="accountTextBox" Text="帳號：" Style="font-size: 20px;" />
            <asp:TextBox ID="accountTextBox" runat="server" Style="padding: 3px; border-radius: 3px; font-size: 18px; font-family: 'Times New Roman', 'DFKai-sb';" onKeyUp="value=value.replace(/[\W]/g,'') " />
        </p>
        <p>
            <asp:Label ID="passwordLabel" runat="server" AssociatedControlID="passwordTextBox" Text="密碼：" Style="font-size: 20px;" />
            <asp:TextBox ID="passwordTextBox" runat="server" TextMode="Password" Style="padding: 3px; border-radius: 3px; font-size: 18px; font-family: 'Times New Roman', 'DFKai-sb';" onKeyUp="value=value.replace(/[\W]/g,'') " />
        </p>
        <br />
        <br />
        <br />
        <p>
            <asp:Button ID="loginbtn" runat="server" Text="登入" Style="border: none;" OnClick="LoginButton_Click" />
        </p>
        <br />
        <br />
    </form>
</body>
</html>
