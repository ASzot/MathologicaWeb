<%@ Page Title="FAQ" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="faq.aspx.cs" Inherits="MathSolverWebsite.help.FAQPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <link rel="stylesheet" type="text/css" href="/Content/css/mlogica-practice.css" />
    <style type="text/css">
        .body {
            overflow-y: auto;
            font-size: 25px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="w-section">
        <div class="w-container" style="max-width: 920px;">
            <div class="sample">
                <h1 class="heading-1">FAQ</h1>
                <p><b>Problem: Math has ` characters around it like `x^2`</b></p>
                <p>
                    This is the math taking time to render.
                    If the math continues to not render even after several seconds then there is a problem with the data connection.
                </p>
                <p><b>Problem: Invalid input always being Displayed</b></p>
                <p>
                    Ensure the input is actually correct. It is possible invalid characters are being entered or that the expression is syntactically incorrect. For the exact rules of input 
                    go to the <a href="general.aspx">general help page</a>. If the input is definitely correct send us an email at <a href="mailto:contact@mathologica.com">contact@mathologica.com</a> and 
                    we will do our best to fix the problem.
                </p>
                <p><b>Problem Toolbar Menu and Textbox Input are Missing</b></p>
                <p style="margin-bottom: 150px;">
                    JavaScript is likely disabled in your browser, try enabling it.
                </p>
            </div>
        </div>
    </div>
</asp:Content>
