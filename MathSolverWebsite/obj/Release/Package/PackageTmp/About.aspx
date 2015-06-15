<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="about.aspx.cs" Inherits="MathSolverWebsite.AboutPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style type="text/css">
        .body {
            overflow-y: auto;
            font-size: 20px;
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
    <div class="ws-container">
        <div class="heading-1">ABOUT</div>
        <div>
            <p>
                Mathologica began with the idea that knowledge and understanding should be free, period. This, as 

                well as a passion for mathematics throughout high school, inspired the idea of creating a math 

                solving application that would aid each and every student with understanding mathematical 

                problems, by providing detailed computations and solutions, free of charge. Many students 

                cannot afford or rather should not pay for such a solver, therein lying the solution: Mathologica.
            </p>
        </div>
        
        <div>
            <p>
                Price, or lack thereof, does not suggest a lack of performance. Mathologica offers an intuitive

                and user friendly interface and input system, suitable for all users. Simplicity and accuracy have 

                always been and will continue to be Mathologica’s priories moving forward, leading to the 

                production of an easy to use application.
            </p>
        </div>

        <div>
            <p>
                But we aren’t yet perfect. Please feel free and encouraged to provide suggestions and feedback

                as to your experience with Mathologica. Community engagement is vital to creating the perfect 

                math solver.
            </p>
        </div>

        <div>
            <p>
                Please send suggestions and feedback to contact@mathologica.com. Don’t forget to follow us 

                on Twitter and Facebook!
            </p>
        </div>

        <div  style="margin-bottom: 150px;">
            <p>
                Special thanks to Camellia Clark for her support and creation of Mathologica’s Logo!
            </p>
        </div>
    </div>
</asp:Content>
