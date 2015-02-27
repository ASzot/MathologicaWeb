<%@ Page Title="Mathologica-Free Math Solver" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MathSolverWebsite._Default" %>

<asp:Content runat="server" ID="HeadContent" ContentPlaceHolderID="HeadContent">
<%--    <script src="https://www.desmos.com/api/v0.4/calculator.js?apiKey=dcb31709b452b1cf9dc26972add0fda6"></script>--%>
</asp:Content>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">

    <meta name="description" 
        content="A math solver capable of showing step by step work and explanations for free. Solve algebra, trig, and calculus problems with detailed work." />

    <script async="async" type="text/javascript" src="https://cdn.mathjax.org/mathjax/latest/MathJax.js?config=AM_HTMLorMML"></script>

    
<%--    <script>

        var calculator;
        var prevGraphDataStr = "";
        $(document).ready(function () {
            var elt = document.getElementById('calculator');
            calculator = Desmos.Calculator(elt,
                {
                    expressions: false,
                    settingsMenu: true,
                    zoomButtons: true,
                });

            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(PageLoaded);


            function PageLoaded(sender, args) {

                var graphDataStr = $("#<% = graphFuncTxtBox.ClientID %>").val();
                console.log(graphDataStr);
                if (graphDataStr == "") {
                    $("#graphSectionDiv").hide();
                    console.log("Hiding the graph as the set value was blank");
                    return;
                }

                var workSectionVisible = $("#<% = resultSectionDiv.ClientID %>").is(":visible");
                console.log(workSectionVisible);
                if (workSectionVisible) {
                    $("#graphSectionDiv").show();
                    console.log("showing the graph");
                    calculator.resize();
                }
                else {
                    $("#graphSectionDiv").hide();
                }

                if (prevGraphDataStr == graphDataStr)
                    return;2



                console.log(graphDataStr);

                var splitEqStrs = graphDataStr.split(";");

                // Each is an equation to graph.
                calculator.setBlank();
                for (var i = 0; i < splitEqStrs.length; ++i) {
                    var curExpStr = splitEqStrs[i];
                    console.log("Plotting equation: " + curExpStr);
                    calculator.setExpression({ id: 'graph' + i, latex: curExpStr });
                }

                prevGraphDataStr = graphDataStr;
            }
        });

    </script>--%>


    <script type="text/javascript" src="Scripts/Main.js"></script>
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

    
    <p class="motoTxt">
        Solve math problems and show work <b>for free</b>
    </p>

    <div class="subjectBar">
        <div id="sb0" class="subjectBarButtonClicked">
            <span>Basic</span>
        </div>
        <div id="sb3" class="subjectBarButton">
            <span>Trig</span>
        </div>
        <div id="sb2" class="subjectBarButton">
            <span>Calculus</span>
        </div>
        <div id="sb1" class="subjectBarButton">
            <span>Symbols</span>
        </div>
        <div id="sb4" class="subjectBarButton">
            <span>Numbers</span>
        </div>
    </div>

    <div>
        
        <div id="toolBox">
        </div>

        <div id="editBox">
            <span class="titleHeader" id="editTitleHeader" onclick="toggleEditBoxVisibility();">Edit+</span>
            <br />
            <div class="editContainer" id="editContainerId">
                <input id="addInputBtnId" type="button" class="btn-clear" value="Add Line" />
                <input id="removeInputBtnId" type="button" class="btn-clear" value="Remove Line" />
                <input id="clearResultBtnId" type="button" class="btn-clear" value="Clear Result"/>
            </div>
        </div>

        <div id="modeBox">
            <span class="titleHeader" id="optionsTitleHeader" onclick="toggleModeBoxVisibility();">Options+</span>
            <br />
            <div class="optionsContainer" id="optionsContainerId">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:RadioButton TextAlign="Left" runat="server" CssClass="siteRadBtn" GroupName="angleMode" ID="radRadBtn" Text="Use Radians:" OnCheckedChanged="angleRadBtn_CheckedChanged" AutoPostBack="true" Checked="true" />
                        <asp:RadioButton TextAlign="Left" runat="server" CssClass="siteRadBtn" GroupName="angleMode" ID="degRadBtn" Text="Use Degrees:" OnCheckedChanged="angleRadBtn_CheckedChanged" AutoPostBack="true" Checked="false" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <script async="async" type="text/javascript">

        $(document).click(function () {
            hideEditBox();
        });

        $(".titleHeader").click(function (e) {
            e.stopPropagation();
            return false;
        });

        $(".editContainer").click(function (e) {
            e.stopPropagation();
            return false;
        });

        $("#clearResultBtnId").click(function (e) {
            onClearBtnClicked();
            e.stopPropagation();

            $("#graphSectionDiv").hide();

            return false;
        });

        $("#removeInputBtnId").click(function (e) {
            clearInputBtn_Clicked();
            e.stopPropagation();
            return false;
        });

        $("#addInputBtnId").click(function (e) {
            addInputBtn_Clicked();
            e.stopPropagation();
            return false;
        });

        var changeMath = true;

        function mathInputChangedSafe() {
            var latex = getLatexInput();

            fixInput(latex);

            var encodedLatex = htmlEncode(latex);

            $("#<%= hiddenInputTxtBox.ClientID %>").val(encodedLatex);

            $("#<%= hiddenUpdateBtn.ClientID %>").click();
        }

        function mathInputChanged(event) {
            if (typeof event == 'object' && event !== null) {
                if (event.which == 13 || event.key == 13) {
                    inputMathEnter();
                    return;
                }
            }

            var latex = getLatexInput();

            fixInput(latex);

            var encodedLatex = htmlEncode(latex);

            $("#<%= hiddenInputTxtBox.ClientID %>").val(encodedLatex);
            

            $("#<%= hiddenUpdateBtn.ClientID %>").click();
        }

        function onClearBtnClicked() {
            $("#<%= resultSectionDiv.ClientID %>").html("");
            $("#<%= resultSectionDiv.ClientID %>").hide();
            $("#<%= workSectionDiv.ClientID %>").html("");
        }

        function inputMathEnter() {

            var evalDropDownList = document.getElementById("<%= evaluteDropList.ClientID %>");
            var txt = evalDropDownList.options[evalDropDownList.selectedIndex].text;
            if (txt == "Input is too long." || txt == "Invalid input" || txt == "Enter input above." || txt == "Please wait...")
                return;

            $("#<%= loadingDiv.ClientID %>").attr('class', 'notHidden');

            $("#<%= hiddenDisplayBtn.ClientID %>").click();

            changeMath = true;
        }

        function pageLoad() {
            if (!changeMath)
                return;

            MathJax.Hub.Queue(["Typeset", MathJax.Hub]);

            changeMath = false;
        }

        function exampleChange() {
            alert("Changed");
        }
    </script>

    <div id="mathInputArea">
        <div>
            <ul id="inputList">
            </ul>
        </div>

        <div class="btnToolbar">
            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <span runat="server" class="parseErrorTxt" id="parseErrorSpan"></span>
                    <div>
                        <input type="button" title="Evaluate the input" class="btn-solve" onclick="inputMathEnter();" value="Solve" />
                        <span class="genDropDownWrapper">
                            <asp:DropDownList ID="evaluteDropList" runat="server" CssClass="genDropDown"></asp:DropDownList>
                        </span>
                        <asp:Button CssClass="hiddenBtn" runat="server" ID="hiddenUpdateBtn" ClientIDMode="Static" OnClick="hiddenUpdate_Click" />
                        <asp:TextBox CssClass="hiddenBtn" runat="server" ID="hiddenInputTxtBox" ClientIDMode="Static" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <asp:Panel runat="server">
            <asp:UpdatePanel runat="server" ID="contentUpdate" UpdateMode="Conditional">
                <ContentTemplate>
                    <div runat="server" class="hidden" id="loadingDiv" style="margin-top: 30px; border-top: 1px solid black; border-bottom: 1px solid black;">
                        <p style="text-align: center; font-size: 25pt;">Loading</p>
                        <img id="loadingImg" src="Images/loading.gif" class="centerImage" style="margin-top: 30px;" />
                    </div>
                    <asp:Button CssClass="hiddenBtn" runat="server" ID="hiddenDisplayBtn" ClientIDMode="Static" OnClick="hiddenDisplayBtn_Click" />
                    <div runat="server" visible="false" id="resultSectionDiv" class="resultSection"></div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
        
        <div id="graphSectionDiv" style="display:none;" >
            <p class="sectionHeading"><input class="workCollapseBtn" type="button" onclick="$('#graphDiv').toggle();" value="Graph" /></p>
            <div id="graphDiv">
                <div id="calculator" style="width: 100%; height: 500px;"></div>
            </div>
        </div>

<%--        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <input type="text" id="graphFuncTxtBox" runat="server" value="" class="hidden" />
            </ContentTemplate>
        </asp:UpdatePanel>--%>

        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div runat="server" visible="false" id="workSectionDiv" class="resultSection"></div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

     <%
            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (!(b.IsMatch(u) || v.IsMatch(u.Substring(0, 4)))) {
                Response.Write(
                    "<div>" +
                        "<div id='adPanel' style='margin-top: 50px;'>" + 
                            "<script async src='//pagead2.googlesyndication.com/pagead/js/adsbygoogle.js'></script>" + 
                            "<!-- Mathologica Default Ad -->" + 
                            "<ins class='adsbygoogle'" + 
                                    "style='display:inline-block; width:728px; height:90px'" + 
                                    "data-ad-client='ca-pub-3516117000150402'" + 
                                    "data-ad-slot='4392690570'></ins>" + 
                            "<script>" + 
                                "(adsbygoogle = window.adsbygoogle || []).push({});" + 
                            "</script>" + 
                        "</div>" + 
                    "</div>");
            }
        %>
    
</asp:Content>
