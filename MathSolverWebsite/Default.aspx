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
    
    <div class="w-row main-input">

        <div class="w-col w-col-7 w-col-medium-6 w-col-small-6 w-col-tiny-6 w-clearfix left">
            <div class="w-clearfix all-input">
                <div class="input-landing input"></div>
                <div class="submission">
                    <input style="height: 40px" type="button" class="solve_button" value="Solve" />
                    <span class="genDropDownWrapper">
                        <asp:DropDownList CssClass="genDropDown" runat="server" ID="evaluateDropDownList"></asp:DropDownList>
                    </span>
                    <input style="height: 40px" type="button" class="button" value="Edit+" />
                </div>
            </div>
        </div>

        <div class="w-col w-col-5 w-col-medium-6 w-col-small-6 w-col-tiny-6 w-clearfix right">
            <div class="symbols">
                <div class="w-tabs" data-duration-in="300" data-duration-out="100">
                    <div class="w-tab-menu tabs-menu">
                    <a class="w-tab-link w-inline-block tab-link" data-w-tab="Tab 1">
                        <div class="text-link">Basic</div>
                    </a>
                    <a class="w-tab-link w-inline-block tab-link" data-w-tab="Tab 2">
                        <div class="text-link">Trig</div>
                    </a>
                    <a class="w-tab-link w-inline-block tab-link" data-w-tab="Tab 3">
                        <div class="text-link">Calc</div>
                    </a>
                    <a class="w-tab-link w--current w-inline-block tab-link" data-w-tab="Tab 4">
                        <div class="text-link">Symbols</div>
                    </a>
                    </div>
                    <div class="w-tab-content">
                    <div class="w-tab-pane tab-content" data-w-tab="Tab 1"></div>
                    <div class="w-tab-pane tab-content" data-w-tab="Tab 2"></div>
                    <div class="w-tab-pane tab-content" data-w-tab="Tab 3"></div>
                    <div class="w-tab-pane w--tab-active tab-content" data-w-tab="Tab 4"></div>
                    </div>
                </div>
            </div>
            <div class="symbols social-media"></div>
            <div class="symbols"></div>
        </div>
    </div>

    <div class="w-section">
        <div class="ad"></div>
    </div>

</asp:Content>
