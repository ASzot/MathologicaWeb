<%@ Page Title="MultiLine" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="multiline.aspx.cs" Inherits="MathSolverWebsite.multiline" %>
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

            .pob-problem div ul {
                list-style: none;
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
                <h1 class="heading-1">Multi-line Help</h1>
                <h2 class="heading-2">What is Multi-Line Input?</h2>
                <p>
                   Multi-line in Mathologica refers to using more than one function or line of input to solve a

problem. The most common example would be a system of equations like shown below. 

Inputting these equations requires multi-line:
                </p>

                <div style="text-align: center" class="pob-problem">
                    <span class="hidden">2y+4x=2; 5x-2y=5</span>
                    <div>
                        <ul>
                            <li>`2x+4x=2`</li>
                            <li>`5x-2y=5`</li>
                        </ul>
                    </div>
                </div>

                <h2 class="heading-2">How to Use:</h2>
                <p>
                    The multi-line is a simple system, and can be used 2 different ways:

                </p>

                <ul style="list-style: decimal">
                    <li>
                        To add another “line of input”, simply press the + icon located on the Calculator

page, to the right of the drop down menu. Press this icon multiple times to obtain 

the desired amount of inputs. Press the – icon, located next to the + icon, to 

remove a line.
                    </li>
                    <li>
                        Semi-colons can be utilized, similarly to adding input lines, to separate functions.

Note this requires semi-colons, commas will not work. Simply type your desired 

functions into the input box on the Calculator page, separating each function with 

a semi-colon.
                    </li>
                </ul>

                <p>
                    Consider the examples below for each technique. Both approaches will result in the same

solution:
                </p>

                <p>Technqiue 1:</p>
                
                <div style="text-align: center" class="pob-problem">
                    <span class="hidden">A=2;B=3;C=4;Ax^2+Bx+C=0</span>
                    <div>
                        <ul>
                            <li>`A=2;B=3;C=4`</li>
                            <li>`Ax^2+Bx+C=0`</li>
                        </ul>
                    </div>
                </div>

                <p>Technique 2:</p>
                
                <div style="text-align: center" class="pob-problem">
                    <span class="hidden">A=2;B=3;C=4;Ax^2+Bx+C=0</span>
                    <div>
                        `2x^2+3x+4=0`
                    </div>
                </div>

                <h2 class="heading-2">When to Use Multi-Line</h2>
                <p>
                    Multi-Line can be utilized for a variety of applications. This includes graphing more than one

                    function on a coordinate plan, solving complicated calculus problems, or easily entering problems in general.
                </p>

                <p>
                    Multi-line input can also be used for graphing. Using the methods listed above, input multiple functions into the input box. Then select the graphing option from the dropdown menu to show all of the functions on the same coordinate plane. The following functions would be graphed together for easy comparison:
                </p>
                
                <div style="text-align: center" class="pob-problem">
                    <span class="hidden">y=x-2; y=x^2-4</span>
                    <div>
                        <ul>
                            <li>`y=x-2`</li>
                            <li>`y=x^2-4`</li>
                        </ul>
                    </div>
                </div>

                <p>
                    This method of inputting problems becomes especially useful in the higher levels of calculus. Consider the following problem involving line integrals. 
                </p>

                <p><i>Vector field `\vec{F}(x,y,z)` is given by  `x\vec{j}-y\vec{j}`. A particles position with respect to time is given by `x(t)=\cos(t), y(t)=\sin(t)` find `\oint \vec{F}*d\vec{r}` from `t=-\frac{\pi}{2}` to `t=\frac{\pi}{2}`</i></p>
                
                <div style="text-align: center" class="pob-problem">
                    <span class="hidden">f(x,y)=xy; x(t)=\cos(t); y(t)=\sin(t); 0 \lt t \lt \frac{\pi}{2}; \oint_{c}f(x,y)ds</span>
                    <div>
                        <ul>
                            <li>`\vec{F}(x,y,z)=y\vec{j}-x\vec{j}`</li>
                            <li>`x(t)=\cos(t)`</li>
                            <li>`y(t)=\sin(t)`</li>
                            <li>`0 \lt t \lt 2\pi`</li>
                            <li>`\oint \vec{F}*d\vec{r}`</li>
                        </ul>
                    </div>
                </div>

                <p>
                    In this input several functions were defined and a boundary were defined and a problem to evaluate was given. 
                    The boundary restrictions for a problem must be given in the same input, the boundary and the problem that uses the 
                    boundary cannot be entered independently.</p>

                <ul class="math-list" style="margin-bottom: 150px;">
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
