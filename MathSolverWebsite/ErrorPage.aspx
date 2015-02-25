<%@ Page Title="Error" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="MathSolverWebsite.ErrorPage" %>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="centeredDivOpen">
        <p class="titleText">Error!</p>

        <p class="sectionText">There has been a general error with the website. We apologize for the inconvenience!</p>

        <p class="sectionText">If you think there is a problem please report it. Send an email to <a href="MAILTO:contact@mathologica.com">contact@mathologica.com</a>. If you report the problem it could be fixed!</p>
        
        <img class="centerImage" src="Images/ErrorLogo.png" alt="Error"  />
                    
    </div>
</asp:Content>
