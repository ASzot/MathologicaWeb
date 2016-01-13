<%@ Page Title="Mathologica-Free Math Solver" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MathSolverWebsite._Default" %>

<asp:Content runat="server" ID="HeadContent" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">

    <!-- Meta description here. -->
    <meta name="description" content="Free math problem solver that answers questions ranging from algebra to higher level calculus, showing step by step work with explanations for free." />

    <!-- JSX graph include. -->
    <link rel="stylesheet" href="JSXGraph/jsxgraph.css" type="text/css" />
    <script async="async" type="text/javascript" src="JSXGraph/jsxgraphcore.js"></script>

    <script type="text/javascript" src="Scripts/ml-main.js"></script>
    <link rel="stylesheet" href="Content/css/mlogica-work.css" type="text/css" />

    <!-- MathJax include. -->
    <script async="async" type="text/javascript" src="https://cdn.mathjax.org/mathjax/latest/MathJax.js?config=AM_HTMLorMML"></script>

    <!--Mathquill include-->
    <link rel="stylesheet" type="text/css" href="/mathquill/mathquill.css" />
    <script src="/mathquill/mathquill.js">

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

    <script>
        var prevWidth = 0;
        var btnDropDownTimeout = 0;

        function getParameterByName(name) {
            var match = RegExp('[?&]' + name + '=([^&]*)').exec(window.location.search);
            return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
        }

        function mathInputChanged(event) {
            if (typeof event == 'object' && event !== null) {
                if (event.which == 13 || event.key == 13) {
                    event.stopPropagation = true;
                    solveBtnClick();
                    return;
                }
            }

            $(".more-popup").hide();

            if (selectedTextBox != null) {
                fixInput(selectedTextBox);
            }

            var latex = getLatexInput();

            var encodedLatex = htmlEncode(latex);

            $("#<%= hiddenUpdateTxtBox.ClientID %>").val(encodedLatex);

            $("#<%= hiddenUpdateBtn.ClientID %>").click();
            MathJax.Hub.Queue(["Typeset", MathJax.Hub, "funcDispList"]);
        }

        function solveBtnClick() {
            // Make sure there is not an error in the eval drop down.
            var dropDown = document.getElementById("<% = evalDropDownList.ClientID %>");
            var txt = dropDown.options[dropDown.selectedIndex].text;
            if (txt.indexOf("Input is too long") != -1 ||
                txt.indexOf("Invalid input") != -1 ||
                txt.indexOf("Enter input above") != -1 ||
                txt.indexOf("Please wait...") != -1)
                return;

            var latexInput = getLatexInput();
            if (latexInput == "")
                return;

            // Take the previous solve info and move it up a 'space'.
            $("#<% = hiddenSolveBtn.ClientID %>").click();

            $("#<% = loadingArea.ClientID %>").show();
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
                setEditableByObject($("#mathInputSpan" + i));
                setValueForObject($("#mathInputSpan" + i), inputs[i]);
            }

            mathInputChanged(null);
        }

        $.fn.slideFadeToggle = function (easing, callback) {
            return this.animate({ opacity: 'toggle', height: 'toggle' }, "fast", easing, callback);
        };

        function createPopUp(innerHtml) {
            $(".pop").remove();
            return "<div class='messagepop pop'>" + innerHtml + "<a class='close' href='#'>Close</a></div>";
        }

        function showPopUp() {
            $(".pop").slideFadeToggle();

            $(".close").live('click', function () {
                $(".pop").remove();
                return false;
            });
        }

        function addPobBtnCallback() {
            $(".pob-problem").click(function (e) {
                // Paste into the input.
                var inputDisp = $(this).find(".hidden").html();
                var inputDispSplit = inputDisp.split('|');
                var inputDispEncoded = encodeURIComponent(inputDispSplit[0]);
                window.location.replace("/Default?Index=" + inputDispSplit[1] + "&InputDisp=" + inputDispEncoded + ((inputDispSplit[2] == null || inputDispSplit[2] == "") ? "" : "&UseRad=" + inputDispSplit[1]));
            });
        }

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function BeginRequestHandler(sender, args) {

        }
        function EndRequestHandler(sender, args) {
            var senderId = "";
            if (typeof sender._postBackSettings.sourceElement == 'object' && sender._postBackSettings.sourceElement !== null)
                senderId = sender._postBackSettings.sourceElement.id;
            var errorTxt = $("#<%= parseErrorSpan.ClientID %>").html();
            if (/\S/.test(errorTxt)) {
                $("#parse-errors-id").show();
            }
            else {
                $("#parse-errors-id").hide();
            }

            if (senderId != "id-solve-btn" && senderId.indexOf("hiddenSolveBtn") == -1) {
                if (senderId.indexOf("hiddenUpdateBtn") != -1 || senderId.indexOf("RadBtn") != -1 || senderId.indexOf("functionDefsListView") != -1) {
                    MathJax.Hub.Queue(['Typeset', MathJax.Hub, 'funcDispList']);
                }
                else if (senderId.indexOf('updateExampleTimer') != -1 || senderId.indexOf('exampleNav') != -1) {
                    MathJax.Hub.Queue(['Typeset', MathJax.Hub, "pob-space"]);
                    addPobBtnCallback();
                }

                return;
            }

            var prevSolveOutput = $("#<% = calcOutput.ClientID %>").html();

            $("#<% = calcOutput.ClientID %>").html("");

            // Hide all of the previous work steps.
            $(".workCollapseBtn").each(function () {
                $(this).parent().next().hide();
            });

            var resultListCount = $("#work-list-disp").children().length / 2;

            // Have a max count of results to be displayed.
            if (resultListCount > 4) {
                $("#work-list-disp").children().first().remove();
                $("#work-list-disp").children().first().remove();
            }

            // Remove all of the previous graphs.
            $("#graphbox").remove();

            // Remove all of the existing graphs. (There can only be one graph at once).
            $("#work-list-disp").append("<div class='prev-output'>" + prevSolveOutput + "<div class='more-options-area'>" +
                "<div style='border-right: 1px solid #adadad' class='link-btn icon-btn'><img src='/Images/LinkIcon.png' />" +
                "</div><div class='share-btn icon-btn'><img src='/Images/SaveIcon.png' /></div>" +

                "</div></div><div class='horiz-divide'></div>");

            MathJax.Hub.Queue(["Typeset", MathJax.Hub], function () {

            });

            var workSpaceEle = $("#work-space");
            // Scroll to the solution.
            var scrollToVal = workSpaceEle.scrollTop() + $("#work-list-disp").children().last().prev().offset().top;
            workSpaceEle.scrollTop(scrollToVal);

            if (workSpaceEle[0].scrollHeight - workSpaceEle.scrollTop() == workSpaceEle.outerHeight())
                exitScrollMode();
            else {
                enterScrollMode();
            }

            $(".input-disp-area").each(function () {
                $(this).click(function () {
                    var inputTxt = $(this).children("p").text();
                    var selCmd = $(this).children(".selected-cmd-txt").html();

                    var inputTxts = inputTxt.split(",");

                    setInputs(inputTxts);

                    // Scroll to the bottom.
                    var objDiv = document.getElementById("work-space");
                    objDiv.scrollTop = objDiv.scrollHeight;
                });
            });

            var x = $("#work-list-disp").children().last().prev().children(".input-disp-area").children(".input-disp-txt").children("span").each(function () {
                $(this).mathquill();
            });

            $(".workCollapseBtn").each(function () {
                $(this).unbind("click").click(function () {
                    // Close all of the other work panels.

                    $(".workCollapseBtn").not(this).each(function () {
                        $(this).parent().next().hide();
                    });

                    $(this).parent().next().toggle();

                    var workSpaceEle = $("#work-space");
                    if (workSpaceEle[0].scrollHeight - workSpaceEle.scrollTop() == workSpaceEle.outerHeight())
                        exitScrollMode();
                    else {
                        enterScrollMode();
                    }
                });
            });

            $(".sub-work-list-toggle-btn").each(function () {
               $(this).click(function () {
                    var htmlVal = $(this).val();
                    if (htmlVal.indexOf('+') != -1)
                        $(this).val('- Hide Work Steps');
                    else
                        $(this).val("+ Show Work Steps");
                    $(this).next().toggle();
                });
            });

            $(".link-btn").click(function () {
                // Get the input.
                var prevOutput = $(this).parent().parent();
                var inputInfo = prevOutput.children(".input-disp-area");
                var inputTxt = inputInfo.children("p").text();

                // Create the link to the input.
                var linkStr = "mathologica.com/Default?Index=0&InputDisp=" + htmlEncode(inputTxt);
                $(this).parent().parent().parent().parent().prepend(createPopUp(
                    "<p>Copy and past the link to share this problem.</p>" +
                    "<input class='copy-past-link' type='text' value='" + linkStr + "' />"
                        ));
                showPopUp();

                $(".copy-past-link").select();
            });

            $(".share-btn").each(function () {
                $(this).click(function () {
                    var prevOutput = $(this).parent().parent();
                    var inputInfo = prevOutput.children(".input-disp-area");
                    var inputTxt = inputInfo.children("p").text();

                    var isAuthen = '<% = Request.IsAuthenticated  %>';
                    if (isAuthen == "True") {
                        var currentDate = new Date();

                        $("#<%= hiddenSavedProblemTxtBox.ClientID %>").val(htmlEncode(inputTxt));
                        var currentDate = currentDate.today();
                        $("#<%= hiddenTimeTxtBox.ClientID %>").val(htmlEncode(currentDate));

                        $("#<%= hiddenSaveProbBtn.ClientID %>").click();

                        $(this).parent().parent().parent().parent().prepend(createPopUp(
                            "<p class='text-notice' style='font-size: 25px'>Problem Saved</p>"
                            ));
                        showPopUp();
                    }
                    else {
                        $(this).parent().parent().parent().parent().prepend(createPopUp(
                            "<p>Create an account to save problems and access them anywhere at anytime!</p>" +
                            "<a class='btn-link' href='/account/register'>" +
                                "<div class='signup-space account-division'>" +
                                    "Sign Up" +
                                "</div>" +
                            "</a>" +
                            "<a class='btn-link' href='/account/login'>" +
                                "<div class='login-space account-division' style='margin-right: 29px; width: 198px;'>" +
                                    "Log In" +
                                "</div>" +
                            "</a>"
                                ));
                        showPopUp();
                    }
                });
            });

        }

        // When the user clicks the remove result section button.
        function onClearBtnClicked() {
            $("#work-list-disp").html("");
            removeInput();
            exitScrollMode();
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

            $("#work-space").on('scroll', function () {
                var elem = $(this);
                if (elem[0].scrollHeight - elem.scrollTop() == elem.outerHeight())
                    exitScrollMode();
                else {
                    enterScrollMode();
                }
            });

            $("#to-bottom-btn").click(function (e) {
                var objDiv = document.getElementById("work-space");
                exitScrollMode();
                objDiv.scrollTop = objDiv.scrollHeight;
            });

            $("#nav-clear-btn").click(function (e) {
                onClearBtnClicked();
            });

            $("#example-nav-forward").click(function (e) {
                $("#<% = exampleNavForwardBtn.ClientID %>").click();
                e.stopPropagation();
            });

            $("#example-nav-backward").click(function (e) {
                $("#<% = exampleNavBackBtn.ClientID %>").click();
                e.stopPropagation();
            });

            addPobBtnCallback();

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

            // Is the input that should be pre entered?
            var inputDispVal = getParameterByName("InputDisp");

            if (inputDispVal !== null && inputDispVal != "") {
                var splitInput = inputDispVal.split(';');
                for (var i = 0; i < splitInput.length; ++i) {
                    var inputHtml = createInputBox(i, i == 0);
                    html += inputHtml;
                }

                $("#input-list").html(html);

                for (var i = 0; i < splitInput.length; ++i) {
                    // Set that index of the input box with the appropriate math.
                    setEditableByObject($("#mathInputSpan" + i), 'editable');
                    setValueForObject($("#mathInputSpan" + i), splitInput[i]);

                    // By default have the user select the first input box of the pre entered math. 
                    if (i == 0)
                        selectedTextBox = $("#mathInputSpan" + i);

                    // Add the input box to the input field text box list.
                    inputBoxIds.push(i);
                }
            }
            else {
                var inputBoxHtml = createInputBox(0, true);

                $("#input-list").html(html + inputBoxHtml);
                // Just create the input box with no math in it. 
                setEditableByObject($("#mathInputSpan0"), "editable");
                selectedTextBox = $("#mathInputSpan0");
                inputBoxIds.push(0);
            }

            var popUpTiming = 0;

            $(".more-popup").mouseleave(function () {
                setTimeout(function () {
                    // Don't even think about using 'this' here.
                    $(".more-popup").hide(1000);
                    $("#expand-more-popup").html("+");
                }, 2000);

            });
            $(".more-popup").mouseenter(function () {
                $(this).stop(true, true).show();
                $("#expand-more-popup").html("-");
            });

            $("#expand-more-popup").click(function (e) {
                var html = $(this).html();
                if (html == "+")
                    $(this).html("-");
                else
                    $(this).html("+");
                $(".more-popup").toggle();
            });
        });
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
                    <div id="work-nav-space">
                        <a class="btn-link">
                            <div class="work-nav-btn" id="nav-clear-btn">
                                CLR
                            </div>
                        </a>
                        <a class="btn-link">
                            <div class="work-nav-btn" id="to-bottom-btn">
                                &#x25BC;
                            </div>
                        </a>
                    </div>
                    <div id="work-list-disp">
                    </div>
                    <asp:UpdatePanel ID="resultUpdatePanel" runat="server">
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
                    <div runat="server" class="loading-area" style="display: none;" id="loadingArea">
                        <p>Loading</p>
                        <img src="Images/loading.gif" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="parse-errors" id="parse-errors-id" style="display: none;">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <span id="parseErrorSpan" class="parse-error-txt" runat="server"></span>
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
                <script async src="//pagead2.googlesyndication.com/pagead/js/adsbygoogle.js"></script>
                <!-- Lower Ad -->
                <ins class="adsbygoogle"
                    style="display: block"
                    data-ad-client="ca-pub-3516117000150402"
                    data-ad-slot="5259228574"
                    data-ad-format="auto"></ins>
                <script>
                    (adsbygoogle = window.adsbygoogle || []).push({});
                </script>
            </div>
        </div>

        <div id="right-paneling">
            <div id="toolbar-space" class="noselect">
                <div id="tool-bar-selection-space">
                    <div id="sb0" class="subject-bar-btn-clicked noselect">Basic</div>
                    <div id="sb1" class="subject-bar-btn noselect">Trig</div>
                    <div id="sb2" class="subject-bar-btn noselect">Calc</div>
                    <div id="sb3" class="subject-bar-btn noselect">Symb</div>
                    <div id="sb4" class="subject-bar-btn noselect">Prob</div>
                    <div id="sb5" class="subject-bar-btn noselect">Lin Alg</div>
                </div>
                <div id="toolbar-btn-space">
                </div>
                <div class="more-popup" style="display: none;">
                    <div class="more-popup-content">
                        <asp:UpdatePanel runat="server" ID="moreContentUpdatePanel">
                            <ContentTemplate>
                                <asp:RadioButton AutoPostBack="true" TextAlign="Left" CssClass="angle-rad-btn" Text="Use Radians: " OnCheckedChanged="angleRadBtn_CheckedChanged" runat="server" ID="radRadBtn" GroupName="angleGroup" />
                                <asp:RadioButton AutoPostBack="true" TextAlign="Left" CssClass="angle-rad-btn" Text="Use Degrees: " OnCheckedChanged="angleRadBtn_CheckedChanged" runat="server" ID="degRadBtn" GroupName="angleGroup" />

                                <asp:ListView ID="functionDefsListView" runat="server" OnItemCommand="functionDefsListView_ItemCommand" OnItemDeleting="functionDefsListView_ItemDeleting">
                                    <LayoutTemplate>
                                        <span class="func-list-title">Function Definitions</span>
                                        <ul class="func-disp-list" id="funcDispList">
                                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                        </ul>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <li>
                                            <span>`<%# Eval("FuncName") %> = <%# Eval("FuncDef") %>`</span>
                                            <asp:Button CssClass="del-span" runat="server" ID="SelectCategoryButton" CommandName="Delete"
                                                CommandArgument='<%# Eval("FuncName") %>' Text="Delete" />
                                        </li>
                                    </ItemTemplate>
                                    <EmptyDataTemplate>
                                        <span class='func-list-title'>No functions defined.</span>
                                    </EmptyDataTemplate>
                                </asp:ListView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div id="expand-more-popup" class="noselect">+</div>

            <div id="pod-space">
                <div style="margin-left: 10px;">
                    <input type="button" id="example-nav-backward" style="float: left;" class="example-nav-btn" value="&#x25C0;" />
                    <p class="pob-title"></p>
                    <input type="button" id="example-nav-forward" style="float: right;" class="example-nav-btn" value="&#x25B6;" />
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Timer ID="updateExampleTimer" runat="server" Interval="10000" OnTick="updateExampleTimer_Tick"></asp:Timer>
                                <div id="exampleOutputContent" runat="server"></div>
                            <asp:Button runat="server" ID="exampleNavBackBtn" CssClass="hidden" OnClick="exampleNavBackBtn_Click" />
                            <asp:Button runat="server" ID="exampleNavForwardBtn" CssClass="hidden" OnClick="exampleNavForwardBtn_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div id="account-space">
                <section id="login">
                    <asp:LoginView ID="LoginView1" runat="server" ViewStateMode="Disabled">
                        <AnonymousTemplate>
                            <a class="btn-link" href="/account/register">
                                <div class="signup-space account-division">
                                    Sign Up
                                </div>
                            </a>
                            <a class="btn-link" href="/account/login">
                                <div class="login-space account-division">
                                    Log In
                                </div>
                            </a>
                        </AnonymousTemplate>
                        <LoggedInTemplate>
                            <a class="btn-link" href="/account/you">
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
                                        <asp:LoginStatus CssClass="logout-btn" ID="LoginStatus1" runat="server" LogoutAction="Redirect" LogoutText="Log off" LogoutPageUrl="~/" />
                                    </div>
                                </a>
                                <a class="btn-link" href="/account/manage">
                                    <div class="account-popup-item noselect">
                                        Settings
                                    </div>
                                </a>
                            </div>
                        </LoggedInTemplate>
                    </asp:LoginView>
                </section>
            </div>
        </div>
        <div style="clear: both;"></div>
    </div>
</asp:Content>