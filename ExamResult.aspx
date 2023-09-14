<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExamResult.aspx.cs" Inherits="Quiz_Assignment.ExamResult" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Exam Result</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Exam Result</h1>

            <asp:Label ID="ResultLabel" runat="server" Text=""></asp:Label>

            <h3>Email saved in the cookie:</h3>
            <asp:Label ID="EmailLabel" runat="server" Text=""></asp:Label>

            <h2>Questions and Answers:</h2>
            <!-- Add a Panel to display questions and answers -->
            <asp:Panel ID="QuestionPanel" runat="server">
                <!-- The questions and answers will be added dynamically here -->
            </asp:Panel>
        </div>
    </form>
</body>
</html>
