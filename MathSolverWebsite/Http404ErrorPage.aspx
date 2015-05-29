<%@ Page Title="Error" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Http404ErrorPage.aspx.cs" Inherits="MathSolverWebsite.Http404ErrorPage" %>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="w-section">
        <div class="w-container" style="max-width: 920px;">
            <div class="sample">
                <h1 class="heading-1">404 Error</h1>
                <p>Oops, the page you are looking for cannot be found. Be sure you typed in the address correctly or go back to the home page 
                    <a href="/">www.mathologica.com</a>. If you think this was an error on our part please contact us at 
                    <a href="mailto:contact@mathologica.com">contact@mathologica.com</a>
                </p>
                <img style="display: block; margin-left: auto; margin-right:auto;" height="200" width="200" src="Images/ErrorIcon.jpg" />
            </div>
        </div>
    </div>
</asp:Content>
