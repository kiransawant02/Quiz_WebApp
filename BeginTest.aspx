<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BeginTest.aspx.cs" Inherits="Quiz_Assignment.BeginTest" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="StyleSheet.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="form-container">
            <asp:Label ID="lblemail" runat="server" Text="Enter your Email : "></asp:Label>
            <asp:TextBox ID="txtemail" runat="server" required></asp:TextBox>
            <asp:Button runat="server" ID="Button1" Text="BEGIN EXAM" OnClick="beginexambtnclick" />
        </div>
    </form>
</body>
</html>
