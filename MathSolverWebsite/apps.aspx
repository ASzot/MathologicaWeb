<%@ Page Title="Apps" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="apps.aspx.cs" Inherits="MathSolverWebsite.AppsPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <link rel="stylesheet" type="text/css" href="/Content/css/mlogica-practice.css" />

    <style>
        .sample {
          min-height: 200px;
          margin-top: 10px;
          padding: 1px 10px 10px;
          border-radius: 6px;
          background-color: white;
        }

    </style>

    <!-- Google analytics -->
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
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    
    <div class="w-section">
        <div class="w-container" style="max-width: 920px;">
            <div class="sample">
                <h1>Take Mathologica on the Go!</h1>
                <p>
                    Download the free Mathologica app for Windows/Android devices and compute problems on the 

                    move, anywhere and everywhere. IOS coming soon!
                </p>
                <h1>Free and Instant Access</h1>
                <p>
                    Get immediate access to solutions and step-by-step work to math problems on your mobile 

                    devices. Utilization of Mathologica does not require network and is free and easy to use. 

                    Mathologica has been specifically optimized to perform on all platforms, from desktop, to tablet 

                    and mobile devices.
                </p>
                <asp:ImageButton CssClass="promote-image" ID="ImageButton1" runat="server" ImageUrl="~/Images/Branding/en_app_rgb_wo_60.png" />
                <asp:ImageButton CssClass="promote-image" ID="ImageButton2" runat="server" ImageUrl="~/Images/Branding/258x67_WP_Store_cyan.png" />
            </div>
        </div>
    </div>

</asp:Content>
