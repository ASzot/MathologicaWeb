<%@ Page Title="Error" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="MathSolverWebsite.ErrorPage" %>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="w-section">
        <div class="w-container" style="max-width: 920px;">
            <div class="sample">
                <h1 class="heading-1">Error</h1>
                <p>It looks like there was an issue with doing that operation. If this was a problem with solving a math problem 
                    <b>please send an email to <a href="mailto:contact@mathologica.com">contact@mathologica.com</a></b>
                     so the problem can be fixed.
                    
                </p>
                <img style="display: block; margin-left: auto; margin-right:auto;" height="200" width="200" src="Images/ErrorIcon.jpg" />
            </div>
        </div>
    </div>
</asp:Content>
