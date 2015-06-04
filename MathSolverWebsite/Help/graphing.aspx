<%@ Page Title="Graphing" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="graphing.aspx.cs" Inherits="MathSolverWebsite.help.GraphingPage" %>
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
        .pob-problem div {
            cursor: pointer;
            font-size: 25px;
        }
    </style>
    <script>
        $(document).ready(function () {

            $(".pob-problem").click(function (e) {
                // Paste into the input.
                var inputDisp = $(this).find(".hidden").html();
                var inputDispSplit = inputDisp.split('|');
                var inputDispEncoded = encodeURIComponent(inputDispSplit[0]);
                window.location.href = ("/Default?Index=" + inputDispSplit[1] + "&InputDisp=" + inputDispEncoded + ((inputDispSplit[2] == null || inputDispSplit[2] == "") ? "" : "&UseRad=" + inputDispSplit[1]));
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="w-section">
        <div class="w-container" style="max-width: 920px;">
            <div class="sample">
                <h1 class="heading-1">Graphing Help</h1>
                <p>
                    Mathologica offers a simple and easy to use interface that allows you to graph a variety of 

                    functions.
                </p>

                <p>
                    <b>Step 1:</b> Input the function(s) you wish to graph in the input box. This can be done a variety of 

                    ways.

                </p>

                <ul style="list-style: none">
                    <li>
                        <div style="text-align: center;" class="pob-problem">
                            <span class='hidden'>y=3x+5|0</span>
                            <div>`y=3x+5`</div>
                        </div>
                    </li>
                    <li style="text-align: center">
                        or
                    </li>
                    <li>
                        <div style="text-align: center;" class="pob-problem">
                            <span class='hidden'>f(x)=3x+5|0</span>
                            <div>`f(x)=3x+5`</div>
                        </div>
                    </li>
                    <li style="text-align: center">
                        or
                    </li>
                    <li>
                        <div style="text-align: center;" class="pob-problem">
                            <span class='hidden'>3x+5|0</span>
                            <div>`3x+5`</div>
                        </div>
                    </li>
                </ul>

                <p>
                    All three inputs will result in the same displayed graph.
                </p>
                <p>
                    <b>Step 2:</b> Select Graph from the drop down menu located next to the solution button.
                </p>
                <p>
                    <b>Step 3:</b> Click Solve and watch your graph appear.
                </p>
                <p style="margin-bottom: 150px;">
                    Mathologica supports graphing multiple functions on one coordinate plane. To do so, simply 

                    type in the desired functions for step one, using a multi-line input, and follow steps 2-3 to graph.

                    Note: Mathologica cannot graph a function with more than one variable, meaning functions such 

                    as `y=xz+5` will not work because of the two variables `x` and `z`. This functionality is similar to most 

                    graphing calculators.
                </p>
            </div>
        </div>
    </div>
</asp:Content>
