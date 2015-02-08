<%@ Page Title="Common Problems" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CommonProblems.aspx.cs" Inherits="MathSolverWebsite.Help.CommonProblems" %>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <p class="titleText">General Help</p>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="centeredDiv">
        <ul>
            <li class="helpTopicListItem">
                <p class="probQuestionTxt">Problem: Math has ` characters around it like `x^2`</p>
                <p>Solution: Javascript may be disabled in your browser try enabling it.</p>
            </li>
            <li class="helpTopicListItem">
                <p class="probQuestionTxt">Problem: Couldn't interpret input always being displayed</p>
                <p>Solution: Ensure the input is actually correct. For rules regarding this go to the <a href="GeneralHelp">general help page</a>. If the input is definitely correct send an email to us 
                    <a class="fakeLink" onclick="BAROMETER.show();">here</a> and we will do our best to fix the problem.
                </p>
            </li>
            <li class="helpTopicListItem">
                <p class="probQuestionTxt">Problem: Two carets are displayed in the text box.</p>
                <p>Solution: We are currently aware of this problem and are attempting to fix it. In the meantime this other "fake" caret doesn't matter. While it is a nuisance it doesn't affect input, just try to ignore it.</p>
            </li>
            <li class="helpTopicListItem">
                <p class="probQuestionTxt">Problem: Toolbar menu and textbox input are missing.</p>
                <p>Solution: Javascript may be disabled in your browser try enabling it.</p>
            </li>
        </ul>
    </div>

</asp:Content>
