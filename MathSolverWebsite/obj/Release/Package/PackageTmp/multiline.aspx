<%@ Page Title="Multi Line" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="multiline.aspx.cs" Inherits="MathSolverWebsite.multiline" %>
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
                    Multi-line in Mathologica is using several lines to input one problem. Using this system can make solving problems much easier.
                </p>


                <h2 class="heading-2">How to Use:</h2>

                <ul style="list-style: circle">
                    <li>
                        <p><b>System of equations: </b>Enter multiple equations using separate lines. </p>
                        <div style="text-align: center" class="pob-problem">
                            <span class="hidden">2y+4x=2; 5x-2y=5</span>
                            <div>
                                <ul>
                                    <li>`2x+4x=2`</li>
                                    <li>`5x-2y=5`</li>
                                </ul>
                            </div>
                        </div>
                    </li>
                    <li>
                        <p><b>Assigning Functions: </b>Assign functions and then use them later.</p>
                        
                        <div style="text-align: center" class="pob-problem">
                            <span class="hidden">f(x)=2x^2+4; 3f(4)-3</span>
                            <div>
                                <ul>
                                    <li>`f(x)=2x^2+4`</li>
                                    <li>`3f(4)-3`</li>
                                </ul>
                            </div>
                        </div>
                    </li>

                    <li>
                        <p><b>Graphing: </b>Graph multiple equations at the same time.</p>
                        
                        <div style="text-align: center" class="pob-problem">
                            <span class="hidden">x^2-3x+2; 3x-2; -8x^2+3</span>
                            <div>
                                <ul>
                                    <li>`x^2-3x+2`</li>
                                    <li>`3x-2`</li>
                                    <li>`-8x^2+3`</li>
                                </ul>
                            </div>
                        </div>
                    </li>
                    <li>
                        <p><b>Assigning Values: </b>Assign values and then use them later.</p>
                        <div style="text-align: center" class="pob-problem">
                            <span class="hidden">A=1; B=-6; C=9; Ax^2+Bx+C=0</span>
                            <div>
                                <ul>
                                    <li>`A=1`</li>
                                    <li>`B=-6`</li>
                                    <li>`C=9`</li>
                                    <li>`Ax^2+Bx+C=0`</li>
                                </ul>
                            </div>
                        </div>
                    </li>
                </ul>

                <h2 class="heading-2">When to Use Multi-Line</h2>
                <p>
                    Multi-line can be utilized for a variety of applications. Assigning values and functions
                    greatly simplifies the amount of work that has to be done to solve a problem.
                </p>

                <ul>
                    <li>
                        <b>Algebra:</b> Work with functions with ease by using the multi-line system. Consider the following problems.
                        <p>Translate the function `f(x)=x^2` nine units up three units to the left, reflect it across the x-axis with a vertical stretch of three.</p>
                        <div style="text-align: center" class="pob-problem">
                            <span class="hidden">f(x)=x^2; -3f(x+3)+9</span>
                            <div>
                                <ul>
                                    <li>`f(x)=x^2`</li>
                                    <li>`-3f(x+3)+9`</li>
                                </ul>
                            </div>
                        </div>
                        <p>Given `f(x)=3x-2` and `g(x)=x^2-3` find `g(f(x))`</p>
                        <div style="text-align: center" class="pob-problem">
                            <span class="hidden">f(x)=3x-2; g(x)=x^2-3; g(f(x))</span>
                            <div>
                                <ul>
                                    <li>`f(x)=3x-2`</li>
                                    <li>`g(x)=x^2-3`</li>
                                    <li>`g(f(x))`</li>
                                </ul>
                            </div>
                        </div>
                        <p>Given `f(x)=sin(x)` find the zeroes for `2f(3x)-1`.</p>
                        <div style="text-align: center" class="pob-problem">
                            <span class="hidden">f(x)=\sin(x); 2f(3x)-1=0</span>
                            <div>
                                <ul>
                                    <li>`f(x)=\sin(x)`</li>
                                    <li>`2f(3x)-1=0`</li>
                                </ul>
                            </div>
                        </div>
                        
                        <p>Graph the equations `x-2` and `x^2-4`</p>
                        <div style="text-align: center" class="pob-problem">
                            <span class="hidden">y=x-2; y=x^2-4</span>
                            <div>
                                <ul>
                                    <li>`y=x-2`</li>
                                    <li>`y=x^2-4`</li>
                                </ul>
                            </div>
                        </div>
                    </li>
                    <li>
                        <b>Calculus: </b> Consider the following problems involving areas of calculus.
                        <p>Find the derivative of the function `f(x)=\ln(2x^2)`</p>
                        <div style="text-align: center" class="pob-problem">
                            <span class="hidden">f(x)=\ln(2x^2); \frac{df}{dx}</span>
                            <div>
                                <ul>
                                    <li>`f(x)=\ln(2x^2)`</li>
                                    <li>`\frac{df}{dx}`</li>
                                </ul>
                            </div>
                        </div>
                        <p>Find the critical points of the function `y=3x^3-7x^2+4`</p>
                        <div style="text-align: center" class="pob-problem">
                            <span class="hidden">y=3x^2-7x^2+4; \frac{df}{dx}=0</span>
                            <div>
                                <ul>
                                    <li>`f(x)=3x^2-7x^2+4`</li>
                                    <li>`\frac{df}{dx}=0`</li>
                                </ul>
                            </div>
                        </div>
                        <p>Use functions in a variety of scenarios to achieve inputs that were not possible before.</p>
                        
                        <div style="text-align: center" class="pob-problem">
                            <span class="hidden">f(x)=\int_0^x t^2 dt; f(2)</span>
                            <div>
                                <ul>
                                    <li>`f(x)=\int_0^x t^2 dt`</li>
                                    <li>`f(2)`</li>
                                </ul>
                            </div>
                        </div>
                        <p style="text-align: center">or</p>
                        <div style="text-align: center" class="pob-problem">
                            <span class="hidden">f(x)=sin(x); \sum_{n=1}^5 f^n(x)</span>
                            <div>
                                <ul>
                                    <li>`f(x)=sin(x)`</li>
                                    <li>`\sum_{n=1}^4 f^n(x)`</li>
                                </ul>
                            </div>
                        </div>
                        <p style="text-align: center">or even use multiple functions at once</p>
                        <div style="text-align: center" class="pob-problem">
                            <span class="hidden">f(x)=sin(x); T(q)=\sum_{n=1}^q f^n(x); T(4)</span>
                            <div>
                                <ul>
                                    <li>`f(x)=sin(x)`</li>
                                    <li>`T(q)=\sum_{n=1}^q f^n(x)`</li>
                                    <li>`T(4)`</li>
                                </ul>
                            </div>
                        </div>
                    </li>
                    <li>
                        <b>Linear Algebra: </b>When it comes to the topics of linear algebra assigning values and then using them later becomes very useful. Consider the following problems.
                        <p>Assign vectors and then use them later.</p>
                        <div style="text-align: center" class="pob-problem">
                            <span class="hidden">A=[6,7,2]; B=[2,9,1]; A+2B</span>
                            <div>
                                <ul>
                                    <li>`A=[6, 7, 2]`</li>
                                    <li>`B=[2,9,1]`</li>
                                    <li>`A + 2B`</li>
                                </ul>
                            </div>
                        </div>
                        <p>Assign matrices and use them later.</p>
                        
                        <div style="text-align: center" class="pob-problem">
                            <span class="hidden">A=\vectora{a, b}{c, d}; B=\vectora{2, 9}{1, 0}; 2A-B</span>
                            <div>
                                <ul>
                                    <li>`A=[(a, b), (c, d)]`</li>
                                    <li>`B=[(2, 9), (1, 0)]`</li>
                                    <li>`2A-B`</li>
                                </ul>
                            </div>
                        </div>
                        <p>Even use functions on assigned values.</p>
                        <div style="text-align: center" class="pob-problem">
                            <span class="hidden">M=\vectorb{1, 2, 3}{0, 1, 4}{5, 6, 0}; M^{-1}</span>
                            <div>
                                <ul>
                                    <li>`M=((1, 2, 3),(0, 1, 4),(5, 6, 0))`</li>
                                    <li>`M^{-1}`</li>
                                </ul>
                            </div>
                        </div>
                    </li>
                    <li>
                        <b>Advanced Calculus: </b>In many problems of advanced calculus using multi-line input becomes necessary. Consider the following line integral problem. 
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
                            Notice how in this input several functions and a boundary were defined and a problem to evaluate was given. 
                            The boundary restrictions for a problem must be given in the same input, the boundary and the problem that uses the 
                            boundary cannot be entered independently. More examples on how to enter the problems of higher level calculus go to the practice pages 
                            <a href="/prac/topic?Name=Line+Integrals+with+Vector+Functions">here</a> or <a href="/prac/topic?Name=Surface+Integrals">here</a>.
                        </p>
                    </li>
                </ul>
                


                <ul class="math-list" style="margin-bottom: 150px;">
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
