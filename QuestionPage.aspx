<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuestionPage.aspx.cs" Inherits="Quiz_Assignment.QuestionPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Quiz Questions</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="QuestionLabel" runat="server" Text=""></asp:Label>
            <br />
            <asp:RadioButtonList ID="AnswerChoices" runat="server"></asp:RadioButtonList>
            <br />
            <asp:Button ID="NextButton" runat="server" Text="Next" OnClick="NextButton_Click" />
        </div>
    </form>
</body>
</html>
