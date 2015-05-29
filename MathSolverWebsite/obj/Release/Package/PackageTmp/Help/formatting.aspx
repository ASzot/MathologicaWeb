<%@ Page Title="Formatting" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="formatting.aspx.cs" Inherits="MathSolverWebsite.help.FormattingPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <link rel="stylesheet" type="text/css" href="/Content/css/mlogica-practice.css" />
    <!-- MathJax include. -->
    <script async="async" type="text/javascript" src="https://cdn.mathjax.org/mathjax/latest/MathJax.js?config=AM_HTMLorMML"></script>
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
                <h1 class="heading-1">Formatting Help</h1>
                <p>
                    <b>Superscripts: </b>You can input superscripts one of two ways. The first method is to use the ^ key on your keyboard. This will create a superscript and allow you to enter what you need to. Once finished, arrow over or click past the superscript to move on with your input. You may also select the X^N function on the symbols toolbar. 
                </p>

                <p>
                    <b>Fractions: </b>To input fractions, use the / symbol on your keyboard. This will prompt you to first enter the numerator. Note, you do not need to use parenthesis to separate the numerator from the denominator.  After entering the numerator, use the down arrow or click on the denominator to enter it. After inputting all values in the fraction, simply use the arrow right key to move onto the next part of your input. You may also prompt the fraction symbol by clicking on its symbol in the symbols toolbar. 
                </p>
                
                <p>
                    <b>Square Root: </b>If you wish to enter a square root, use the prefix sqrt. This will change into a square root symbol which allows you to input whatever you need very similarly to the fraction. If you wish, you can also select the square root icon from the symbols toolbar. 
                </p>
                <p style="margin-bottom: 150px;">
                    <b>Pi: </b>To use the value of `\pi`, simply input the letters pi into the input box. This will automatically be converted into the `\pi` symbol. The π symbol can also be found in the symbols toolbox. 
                </p>
            </div>
        </div>
    </div>
</asp:Content>
