<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="web1.aspx.cs" Inherits="SingleSignOn.web1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="style1.css" />
    <title>System1</title>
</head>
<body>
    <form id="loginForm" runat="server" style="margin: 0 auto; width: 300px; text-align: center;">
        <br />
        <p>
            <asp:Label ID="Label1" runat="server" Text="System 1" Style="font-family: 'Times New Roman', 'DFKai-sb'; font-size: 35px;"></asp:Label>
        </p>
        <br />
        <asp:Label ID="errorLabel" runat="server" Text=""></asp:Label>
    </form>
</body>
</html>