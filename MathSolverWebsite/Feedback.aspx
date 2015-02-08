<%@ Page Title="Feedback" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Feedback.aspx.cs" Inherits="MathSolverWebsite.Feedback" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="Scripts/Feedback.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <p class="titleText">Feedback</p>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="centeredDiv" style="border: 0px solid black;">
    <p>Thank you for voicing your opinion on this website. Any general suggestions, feedback, problems, or complaints are more than welcomed!</p>
        <p>Title:</p>
        <textarea class="textAreaShortResponse" id="titleTxtArea" maxlength="40"></textarea>
    </div>
    <div class="centeredDiv" style="border: 0px solid black;">
        <p>Message:</p>
        <textarea class="textAreaLongResponse" id="messageTxtArea"></textarea>
    </div>
    <div class="centeredDiv" style="border: 0px solid black;">
        <p>We will do nothing with used email other than potentially emailing back a response.</p>
        <input type="button" class="defGoodBtn" onclick="onSendClick();" value="Submit"/>
    </div>
</asp:Content>
