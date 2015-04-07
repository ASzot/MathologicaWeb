<%@ Page Title="Mathologica-Free Math Solver" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MathSolverWebsite._Default" %>

<asp:Content runat="server" ID="HeadContent" ContentPlaceHolderID="HeadContent">
    
</asp:Content>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">

    <!-- Meta description here. -->
    <meta name="description" content="" />
    
    <!-- JSX graph include. -->
    <script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jsxgraph/0.99.3/jsxgraphcore.js"></script>

    <!-- MathJax include. -->
    <script async="async" type="text/javascript" src="https://cdn.mathjax.org/mathjax/latest/MathJax.js?config=AM_HTMLorMML"></script>

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

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <div id="panel-container">
        <div id="left-paneling">
            <div id="work-space">
            </div>
            <div id="eval-space">
                <input type="button" class="solve-btn" value="Solve" />
                <span class="gen-drop-down-wrapper">
                    <asp:DropDownList ID="DropDownList1" runat="server" CssClass="gen-drop-down"></asp:DropDownList>
                </span>
                <div class="tool-bar-btn-space">
                    <input type="button" class="gen-btn" value="CLR" />
                    <input type="button" class="gen-btn" style="font-size: 48px;" value="-" />
                    <input type="button" class="gen-btn" style="font-size: 32px;" value="+" />
                </div>
            </div>
            <div class="ad-space" style="height: 90px; width: 70%">
                <p>(Ad here)</p>
            </div>
        </div>

        <div id="right-paneling">
            <div id="toolbar-space">

            </div>

            <div id="pod-space">

            </div>

            <div id="account-space">

            </div>
        </div>
        <div style="clear: both;"></div>
    </div>
</asp:Content>
