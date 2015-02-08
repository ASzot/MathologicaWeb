<%@ Page Title="Products" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="MathSolverWebsite.Products" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
        <script>
            (function (i, s, o, g, r, a, m) {
                i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
                    (i[r].q = i[r].q || []).push(arguments)
                }, i[r].l = 1 * new Date(); a = s.createElement(o),
                m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
            })(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

            ga('create', 'UA-56848508-1', 'auto');
            ga('send', 'pageview');

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <p class="pageTitle">Products</p>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="centeredDiv">
        <p>
            Take math solving capability on the go with the Mathologica mobile phone app.
        </p>

        <div>
            <a href="http://www.windowsphone.com/s?appid=85bd9de4-fbd6-48fa-bb58-b063c863ac8d" target="_blank">
                <img class="storeBtn" src="Images/258x67_WP_Store_cyan.png" />
            </a>
        </div>
    </div>
</asp:Content>
