<%@ Page Title="Mathologica-Free Math Solver" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MathSolverWebsite._Default" %>

<asp:Content runat="server" ID="HeadContent" ContentPlaceHolderID="HeadContent">
    
</asp:Content>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">

    <!-- Meta description here. -->
    <meta name="description" content="" />
    
    <!-- JSX graph include. -->
    <script async="async" type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jsxgraph/0.99.3/jsxgraphcore.js"></script>

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
                <span class="gen-drop-down-wrapper pointable">
                    <asp:DropDownList ID="DropDownList1" runat="server" CssClass="gen-drop-down pointable"></asp:DropDownList>
                </span>
                <div class="m-tool-bar">
                    <div class="tool-bar-btn-space">
                        <input type="button" style="width: 45%; font-size: 20px" class="gen-btn" value="CLR" />
                        <input type="button" class="gen-btn" style="font-size: 48px;" value="-" />
                        <input type="button" class="gen-btn" style="font-size: 32px;" value="+" />
                    </div>
                    <div class="m-tool-bar-btn-overlay">
                        <div class="m-icon-tool-bar">

                        </div>
                    </div>
                </div>
            </div>
            <div class="ad-space" style="height: 90px; width: 70%">
                <p>(Ad here)</p>
            </div>
        </div>

        <div id="right-paneling">
            <div id="toolbar-space">
                <div id="tool-bar-selection-space">
                    <div class="subject-bar-btn noselect">Basic</div>
                    <div class="subject-bar-btn noselect">Trig</div>
                    <div class="subject-bar-btn noselect">Calc</div>
                    <div class="subject-bar-btn noselect">Symb</div>
                    <div class="subject-bar-btn noselect">Prob</div>
                    <div class="subject-bar-btn noselect">Lin Alg</div>
                </div>
                <div id="toolbar-btn-space">
                    <div class="toolbar-btn noselect">
                        <div class="toolbar-icon">
                            <span class="mathquill-rendered-math pointable">`+`</span>
                        </div>
                    </div>
                    <div class="toolbar-btn noselect">
                        <div class="toolbar-icon">
                            <span class="mathquill-rendered-math pointable">`-`</span>
                        </div>
                    </div>
                    <div class="toolbar-btn">
                        <div class="toolbar-icon">
                            <span class="mathquill-rendered-math pointable">`*`</span>
                        </div>
                    </div>
                    <div class="toolbar-btn">
                        <div class="toolbar-icon-large">
                            <span class="mathquill-rendered-math pointable">`\frac{x}{y}`</span>
                        </div>
                    </div>
                    <div class="toolbar-btn">
                        <div class="toolbar-icon">
                            <span class="mathquill-rendered-math pointable">`x`</span>
                        </div>
                    </div>
                    <div class="toolbar-btn">
                        <div class="toolbar-icon">
                            <span class="mathquill-rendered-math pointable">`y`</span>
                        </div>
                    </div>
                    <div class="toolbar-btn">
                        <div class="toolbar-icon">
                            <span class="mathquill-rendered-math pointable">`z`</span>
                        </div>
                    </div>
                    <div class="toolbar-btn">
                        <div class="toolbar-icon">
                            <span class="mathquill-rendered-math pointable noselect">`\theta`</span>
                        </div>
                    </div>
                    <div class="toolbar-btn">
                        <div class="toolbar-icon">
                            <span class="mathquill-rendered-math pointable">`x^n`</span>
                        </div>
                    </div>
                    <div class="toolbar-btn">
                        <div class="toolbar-icon">
                            <span class="mathquill-rendered-math pointable">`\sqrt{x}`</span>
                        </div>
                    </div>
                    <div class="toolbar-btn">
                        <div class="toolbar-icon">
                            <span class="mathquill-rendered-math pointable">`\root{n}{x}`</span>
                        </div>
                    </div>
                    <div class="toolbar-btn">
                        <div class="toolbar-icon">
                            <span class="mathquill-rendered-math pointable">`|x|`</span>
                        </div>
                    </div>
                    <div class="toolbar-btn">
                        <div class="toolbar-icon">
                            <span class="mathquill-rendered-math pointable">`=`</span>
                        </div>
                    </div>
                    <div class="toolbar-btn">
                        <div class="toolbar-icon">
                            <span class="mathquill-rendered-math pointable">`>`</span>
                        </div>
                    </div>
                    <div class="toolbar-btn">
                        <div class="toolbar-icon">
                            <span class="mathquill-rendered-math pointable">`<`</span>
                        </div>
                    </div>
                </div>
            </div>

            <div id="pod-space">
                <div style="margin-left: 10px;">
                    <p class="pob-title">Problem of the Day:</p>
                    <div style="text-align: center;" class="pob-problem">
                        <p>Volume Integral</p>
                        <div>
                            <span class="mathquill-rendered-math noselect pointable">`\int\int\int_V \frac{\cos(xy)x^2}{\ln(z)} dV`</span>
                        </div>
                    </div>
                </div>
            </div>

            <div id="account-space">
                <div id="signup-space" class="account-division">
                    Sign Up
                </div>
                <div id="login-space" class="account-division">
                    Log In
                </div>
            </div>
        </div>
        <div style="clear: both;"></div>
    </div>
</asp:Content>
