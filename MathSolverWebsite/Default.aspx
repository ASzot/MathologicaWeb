<%@ Page Title="Mathologica-Free Math Solver" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MathSolverWebsite._Default" %>

<asp:Content runat="server" ID="HeadContent" ContentPlaceHolderID="HeadContent">
    
</asp:Content>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">

    <!-- Meta description here. -->
    <meta name="description" content="" />
    
    <!-- JSX graph include. -->
    <script async="async" type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jsxgraph/0.99.3/jsxgraphcore.js"></script>

    <script type="text/javascript" src="Scripts/ml-main.js"></script> 

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
    
    <script>
        // Basic styling stuff.

        var prevWidth = 0;

        function getParameterByName(name) {
            var match = RegExp('[?&]' + name + '=([^&]*)').exec(window.location.search);
            return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
        }

        $(document).ready(function () {
            $("#tool-bar-overlay").mouseenter(function () {
                $("#tool-bar-space").show(200);
            });

            $("#tool-bar-overlay").mouseleave(function () {
                var isHovered = $("#tool-bar-space").is(":hover");
                if (!isHovered) {
                    $("#tool-bar-space").hide(200);
                }
            });

            $("#tool-bar-space").mouseleave(function () {
                var width = $(window).width;
                if (width < 749)
                    $("#tool-bar-space").hide(200);
            });

            $("#tool-bar-overlay").click(function () {
                $("#tool-bar-space").toggle(200);
            });


            $("#clear-btn-id").click(function (e) {
                onClearBtnClicked();
                e.stopPropagation();

                return false;
            });

            $("#remove-btn-id").click(function (e) {
                clearInputBtn_Clicked();
                e.stopPropagation();
                return false;
            });

            $("#add-btn-id").click(function (e) {
                addInputBtn_Clicked();
                e.stopPropagation();
                return false;
            });

            $(".account-space").click(function (e) {
                $(".account-popup").toggle(300);
                return false;
            });

            prevWidth = $(window).width();

            $(window).resize(function (e) {
                var winWidth = $(window).width();
                if (winWidth != prevWidth && winWidth) {
                    if (winWidth > 749) {
                        $("#tool-bar-space").show();
                    }
                    else {
                        $("#tool-bar-space").hide();
                    }
                    prevWidth = winWidth;
                }
            });

            var html = $("#input-list").html();

            var inputDispVal = getParameterByName("InputDisp");
            if (inputDispVal !== null && inputDispVal != "") {
                var splitInput = inputDispVal.split(';');
                for (var i = 0; i < splitInput.length; ++i) {
                    var inputHtml = createInputBox(i, i == 0);
                    html += inputHtml;
                }

                $("#input-list").html(html);


                for (var i = 0; i < splitInput.length; ++i) {
                    $("#mathInputSpan" + i).mathquill('editable');
                    $("#mathInputSpan" + i).mathquill('latex', splitInput[i]);

                    if (i == 0)
                        selectedTextBox = $("#mathInputSpan" + i);
                    inputBoxIds.push(i);
                }
            }
            else {
                var inputBoxHtml = createInputBox(0, true);

                $("#input-list").html(html + inputBoxHtml);
                $("#mathInputSpan0").mathquill("editable");
                selectedTextBox = $("#mathInputSpan0");
                inputBoxIds.push(0);
            }
        });


        function mathInputChanged(event) {
            if (typeof event == 'object' && event !== null) {
                if (event.which == 13 || event.key == 13) {
                    event.stopPropagation = true;
                    solveBtnClick();
                    return;
                }
            }

            var latex = getLatexInput();

            fixInput(latex);

            var encodedLatex = htmlEncode(latex);

            $("#<%= hiddenUpdateTxtBox.ClientID %>").val(encodedLatex);


            $("#<%= hiddenUpdateBtn.ClientID %>").click();
        }

        function solveBtnClick() {
            // Make sure there is not an error in the eval drop down.
            var dropDown = document.getElementById("<% = evalDropDownList.ClientID %>");
            var txt = dropDown.options[dropDown.selectedIndex].text;
            if (txt == "Input is too long" ||
                txt == "Invalid input" ||
                txt == "Enter input above" ||
                txt == "Please wait...")
                return;

            // Take the previous solve info and move it up a 'space'.
            $("#<% = hiddenSolveBtn.ClientID %>").click();

        }

        Date.prototype.today = function () { 
            return (((this.getMonth() + 1) < 10) ? "0" : "") + (this.getMonth() + 1) + "/" + ((this.getDate() < 10) ? "0" : "") + this.getDate() + "/" + this.getFullYear();
        }

        function setInputs(inputs) {
            $("#input-list").html("");
            inputBoxIds = [];
            selectedTextBox = null;
            var totalHtml = "";
            for (var i = 0; i < inputs.length; ++i) {
                totalHtml += createInputBox(i, i == 0);
                inputBoxIds[i] = i;
            }

            $("#input-list").html(totalHtml);
            selectedTextBox = $("#mathInputSpan0");
            
            for (var i = 0; i < inputs.length; ++i) {
                $("#mathInputSpan" + i).mathquill('editable');
                $("#mathInputSpan" + i).mathquill('latex', inputs[i]);
            }

            mathInputChanged(null);
        }

        $.fn.slideFadeToggle = function (easing, callback) {
            return this.animate({ opacity: 'toggle', height: 'toggle' }, "fast", easing, callback);
        };

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function BeginRequestHandler(sender, args) {
        }
        function EndRequestHandler(sender, args) {
            var senderId = "";
            if (typeof sender._postBackSettings.sourceElement == 'object' && sender._postBackSettings.sourceElement !== null)
                senderId = sender._postBackSettings.sourceElement.id;
            var errorTxt = $("#<%= parseErrorSpan.ClientID %>").html();
            if (errorTxt == "") {
                $("#parse-errors-id").hide();
            }
            else {
                $("#parse-errors-id").show();
            }
            if (senderId != "id-solve-btn" && senderId.indexOf("hiddenSolveBtn") == -1)
                return;
            var prevSolveOutput = $("#<% = calcOutput.ClientID %>").html();

            // Remove all of the existing graphs. (There can only be one graph at once).
            $("#work-list-disp").prepend("<div class='prev-output'>" + prevSolveOutput + "</div><input type='button' class='save-btn' value='Save' /><div class='horiz-divide'></div>");
            MathJax.Hub.Queue(["Typeset", MathJax.Hub]);

            $(".input-disp-area").each(function () {
                $(this).click(function () {
                    var inputTxt = $(this).children("p").text();
                    var selCmd = $(this).children(".selected-cmd-txt").html();

                    var inputTxts = inputTxt.split(",");

                    setInputs(inputTxts);
                });
            });

            $(".save-btn").each(function () {
                $(this).click(function () {
                    var prevOutput = $(this).prev();
                    var inputInfo = prevOutput.children(".input-disp-area");
                    var inputTxt = inputInfo.children("p").text();

                    var isAuthen = '<% = Request.IsAuthenticated  %>';
                    if (isAuthen == "True") {
                        var currentDate = new Date();

                        $("#<%= hiddenSavedProblemTxtBox.ClientID %>").val(htmlEncode(inputTxt));
                        var currentDate = currentDate.today();
                        $("#<%= hiddenTimeTxtBox.ClientID %>").val(htmlEncode(currentDate));

                        $("#<%= hiddenSaveProbBtn.ClientID %>").click();
                    }
                    else {
                        $(this).parent().append("<div class='messagepop pop'><p>Create an account to save problems and access them anywhere at anytime!</p>" + 
                            "<a class='btn-link' href='account/register.aspx'>" + 
                                "<div class='signup-space account-division'>" + 
                                    "Sign Up" + 
                                "</div>" + 
                            "</a><a class='close' href='#'>Close</a></div>");
                        $(".pop").slideFadeToggle();

                        $(".close").live('click', function () {
                            $(".pop").remove();
                            return false;
                        });
                    }
                });
            });
        }

        function scrollToBottom() {
            var objDiv = document.getElementById("work-space");
            objDiv.scrollTop = objDiv.scrollHeight;
        }

        function onClearBtnClicked() {
            $("#work-list-disp").html("");
        }

    </script>

</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <style type="text/css">
        #<%=UpdatePanel1.ClientID%> {
            height: 60px;
            bottom: 0px;
            width: 81%;
            display: block;
            position: relative;
            float: left;
        }
    </style>
    <div id="panel-container">
        <div id="left-paneling">
            <div id="work-space">
                <div id="inner-work-space-id" class="inner-work-space">
                    <div id="work-list-disp">

                    </div>
                    <asp:UpdatePanel ID="resultUpdatePanel" runat="server" >
                        <ContentTemplate>
                                <div id="calcOutput" class="hidden" runat="server">

                                </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div id="input-area">
                        <ul id="input-list">

                        </ul>
                    </div>
                </div>
            </div>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:TextBox runat="server" ID="hiddenSavedProblemTxtBox" CssClass="hidden" />
                    <asp:TextBox runat="server" ID="hiddenTimeTxtBox" CssClass="hidden" />
                    <asp:Button runat="server" ID="hiddenSaveProbBtn" CssClass="hidden" OnClick="hiddenSaveProbBtn_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="parse-errors" id="parse-errors-id" style="display: none;">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <span id="parseErrorSpan" class="parse-error-txt" runat="server">

                        </span>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id="eval-space">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <input type="button" id="id-solve-btn" class="solve-btn" value="Solve" onclick="solveBtnClick();" />
                        <asp:Button runat="server" ID="hiddenSolveBtn" CssClass="hidden" OnClick="hiddenSolveBtn_Click" />
                        <asp:Button runat="server" ID="hiddenUpdateBtn" CssClass="hidden" OnClick="hiddenUpdateBtn_Click" />
                        <asp:TextBox runat="server" ID="hiddenUpdateTxtBox" CssClass="hidden" />
                        <span class="gen-drop-down-wrapper pointable">
                            <asp:DropDownList ID="evalDropDownList" runat="server" CssClass="gen-drop-down pointable"></asp:DropDownList>
                        </span>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="m-tool-bar">
                    <div class="tool-bar-btn-space" id="tool-bar-space">
                        <input type="button" id="clear-btn-id" style="width: 45%; font-size: 20px" class="gen-btn" value="CLR" />
                        <input type="button" id="remove-btn-id" class="gen-btn" style="font-size: 48px;" value="-" />
                        <input type="button" id="add-btn-id" class="gen-btn" style="font-size: 32px;" value="+" />
                    </div>
                    <div class="m-tool-bar-btn-overlay" id="tool-bar-overlay">
                        <div class="m-icon-tool-bar">
                            ^
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
                <section id="login">
                    <asp:LoginView ID="LoginView1" runat="server" ViewStateMode="Disabled">
                        <AnonymousTemplate>
                            <a class="btn-link" href="account/register.aspx">
                                <div class="signup-space account-division">
                                    Sign Up
                                </div>
                            </a>
                            <a class="btn-link" href="account/login.aspx">
                                <div class="login-space account-division">
                                    Log In
                                </div>
                            </a>
                        </AnonymousTemplate>
                        <LoggedInTemplate>
                            <a class="btn-link" href="account/you">
                                <div class="you-space account-division noselect">
                                    You
                                </div>
                            </a>
                            <a class="btn-link">
                                <div class="account-space account-division">
                                    <asp:LoginName ID="LoginName1" runat="server" CssClass="username noselect" />
                                </div>
                            </a>
                            <div class="account-popup">
                                <a class="btn-link" href="#">
                                    <div class="account-popup-item noselect">
                                        <asp:LoginStatus ID="LoginStatus1" runat="server" LogoutAction="Redirect" LogoutText="Log off" LogoutPageUrl="~/" />
                                    </div>
                                </a>
                                <a class="btn-link" href="account/manage.aspx">
                                    <div class="account-popup-item noselect">
                                        Settings
                                    </div>
                                </a>
                            </div>

                            <%--   Hello, <a id="A1" runat="server" class="username" href="~/Account/Manage" title="Manage your account">
                                    </a>!
                                --%>
                        </LoggedInTemplate>
                    </asp:LoginView>
                </section>
            </div>
        </div>
        <div style="clear: both;"></div>
    </div>
</asp:Content>
