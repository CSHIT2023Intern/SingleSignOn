<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SingleSignOn.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="style.css" />
    <title>SingleSignOn Index</title>
</head>
<body style="background-color: whitesmoke;">
    <br />
    <br />
    <br />
    <br />
    <form id="indexForm" runat="server">
        <br />
        <br />
        <br />
        <p>
            <asp:Label ID="title" runat="server" Text="Login's Index" Style="color: white;"></asp:Label>
            <br />
            <br />
            <asp:Label ID="welcomeLabel" runat="server" Text="" Style="color:white;"></asp:Label>
        </p>
        <br />
        <p>
            <asp:Button ID="web1btn" runat="server" Text="web1" OnClick="Web1btn_Click"/>
        </p>
        <p>
            <asp:Button ID="web2btn" runat="server" Text="web2" OnClick="Web2btn_Click"/>
        </p>
        <p>
            <asp:Button ID="web3btn" runat="server" Text="web3" OnClick="Web3btn_Click"/>
        </p>
        <br />
        <asp:Label ID="errorLabel" runat="server" Text="" Style="color:white;"></asp:Label>
        <br />
        <p>
            <asp:Button ID="logoutbtn" runat="server" Text="登出" Style="border-color: white;" OnClick="LogoutButton_Click" />
        </p>
        <br />
    </form>
</body>
</html>
