function MenuItem(name, displayTex, addStr, useLargerIcon, subItems, needsSpacing) {
    this.SubItems = subItems;
    this.name = name;
    this.displayTex = displayTex;
    this.useLargerIcon = useLargerIcon;
    this.addStr = addStr;
    if (typeof needsSpacing === 'undefined' || typeof subItems === null)
        needsSpacing = false;

    this.needsSpacing = needsSpacing;
}

var uniqueId = (function () {
    var id = 0;

    return function () { return id += 1; };
})();

setCookie = function (c_name, value, exdays) {
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + exdays);
    var expires = exdate.toUTCString();
    var isIE8 = (document.documentMode !== undefined);
    if (exdays == 0) {
        expires = (isIE8 == true) ? "" : "0";
    }
    var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + expires);
    document.cookie = c_name + "=" + c_value;
}

getCookie = function (cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) != -1) return c.substring(name.length, c.length);
    }
    return "";
}

deleteCookie = function (name) {
    document.cookie = name + '=; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
}

function TopicMenu(items) {
    this.items = items;
    this.outputItems = function () {
        var htmlStr = "";
        for (var i = 0; i < this.items.length; ++i) {
            var currentItem = this.items[i];
            var thisElementId = "tme" + i.toString();
            htmlStr += "<div class='toolbar-btn noselect'>";

            var iconStyling = currentItem.useLargerIcon ? "toolbar-icon-larger" : "toolbar-icon";
            var subItems = currentItem.SubItems;
            var displaySub = typeof subItems !== 'undefined' && typeof subItems !== null;
            htmlStr += "<div id='" + thisElementId + "' onclick='onToolBarEleClicked(this.id);' class='" + iconStyling + " toolbar-icn" + (displaySub ? " include-padding" : "") + "' title='" + currentItem.name + "'>`" + currentItem.displayTex + "`</div>";
            if (displaySub) {
                htmlStr += "<span class='toolbar-btn-dropdown" + (currentItem.needsSpacing ? " extra-space" : "") + "'>▼</span>";
                htmlStr += "<div class='sub-toolbar-btn-space' id='" + uniqueId() + "'>";
                for (var j = 0; j < subItems.length; ++j) {
                    var currentSubItem = subItems[j];
                    iconStyling = currentSubItem.useLargerIcon ? "toolbar-icon-larger" : "toolbar-icon";
                    var id = "tme" + (i + "A" + j);
                    htmlStr += "<div id='" + id + "' onclick='onToolBarEleClicked(this.id);' style='border-color: #D5E9F7' class='sub-toolbar-btn " + iconStyling + " toolbar-icn'>" +
                        "`" +
                        currentSubItem.displayTex + "`</div>";
                }
                htmlStr += "</div>";
            }

            htmlStr += "</div>";
        }

        return htmlStr;
    };
}

var basic = new TopicMenu(
    [
        new MenuItem("Minus", "-", "-", false),
        new MenuItem("Plus", "+", "+", false),
        new MenuItem("Multiply", "*", "*", false),
        new MenuItem("Fraction", "\\frac{x}{y}", "\\frac{}{}", true),
        new MenuItem("Divide", "/", "/", false),
        new MenuItem("Variable x", "x", "x", false),
        new MenuItem("Variable y", "y", "y", false),
        new MenuItem("Variable z", "z", "z", false),
        new MenuItem("Raise to power", "x^n", "^", false),
        new MenuItem("Square root", "sqrt(x)", "\\sqrt{}", false),
        new MenuItem("nth root", "root(n)(x)", "\\nthroot{}{}", false),
        new MenuItem("Absolute value", "|x|", "|x|", false),
        new MenuItem("Equals", "=", "=", false),
        new MenuItem("Greater than", ">", "\\gt", false),
        new MenuItem("Less than", "<", "\\lt", false),
        new MenuItem("Greater equal to", ">=", "\\ge", false),
        new MenuItem("Less equal to", "<=", "\\le", false),
        new MenuItem("Open parentheses", "(", "(", false),
        new MenuItem("Close parentheses", ")", ")", false),
        new MenuItem("Lowercase theta", "theta", "\\theta", false),
        new MenuItem("Euler's number", "e", "e", false),
        new MenuItem("Pi", "pi", "\\pi", false),
        new MenuItem("Imaginary number.", "i", "i", false),
    ]);

var trig = new TopicMenu(
    [
        new MenuItem("Logarithm", "log", "\\log \\left( \\right)", false,
            [
                new MenuItem("Logarithm base n", "\\log_n", "log_{} \\left( \\right)", false),
            ]),
        new MenuItem("Natural log (log base e)", "ln", "\\ln", false),
        new MenuItem("Sine", "sin", "\\sin \\left( \\right)", false,
            [
                new MenuItem("Cosecant, the reciprocal of sine.", "csc", "\\csc \\left( \\right)", false),
                new MenuItem("Inverse sine", "arcsin", "\\arcsin \\left( \\right)", false),
                new MenuItem("Inverse cosecant", "\\text{arccsc}", "\\arccsc \\left( \\right)", false),
            ]),
        new MenuItem("Cosine", "cos", "\\cos \\left( \\right)", false,
            [
                new MenuItem("Secant, the reciprocal of cosine.", "sec", "\\sec \\left( \\right)", false),
                new MenuItem("Inverse cosine", "arccos", "\\arccos \\left( \\right)", false),
                new MenuItem("Inverse secant", "\\text{arcsec}", "\\arcsec \\left( \\right)", false),
            ]),
        new MenuItem("Tangent", "tan", "\\tan \\left( \\right)", false,
            [
                new MenuItem("Cotangent, the reciprocal of tangent.", "cot", "\\cot \\left( \\right)", false),
                new MenuItem("Inverse tangent", "arctan", "\\arctan \\left( \\right)", false),
                new MenuItem("Inverse cotangent", "\\text{arccot}", "\\arccot \\left( \\right)", false),
            ]),
        new MenuItem("Define a function", "f(x)=", "f(x)=", false),
    ]);

var calc = new TopicMenu(
    [
        new MenuItem("Derivative", "d/(dx)", "\\frac{d}{dx}", true,
            [
                new MenuItem("Derivative of function", "(df)/(dx)", "\\frac{df}{dx}", true),
            ]),
        new MenuItem("Indefinite integral", "\\int", "\\int", true,
            [
                new MenuItem("Double indefinite integral", "\\int\\int_{D}", "\\int\\int", true),
                new MenuItem("Triple indefinite integral", "\\int\\int\\int_{V}", "\\int\\int\\int", true),
            ]),
        new MenuItem("Definite integral", "\\int_{a}^{b}", "\\int_{}^{}", true,
            [
                new MenuItem("Double definite integral", "\\int\\int_{D}", "\\int_{}^{}\\int_{}^{}", true),
                new MenuItem("Triple definite integral", "\\int\\int\\int_{V}", "\\int_{}^{}\\int_{}^{}\\int_{}^{}", true),
            ]),
        new MenuItem("Limit", "\\lim_(x\\toh)", "\\lim_{x \\to \\EMPTYGP{} } \\left( \\right)", true),
        new MenuItem("Summation", "\\sum_(n=0)^N", "\\sum_{n= \\EMPTYGP{} }^{}", true),
        new MenuItem("Natural log (log base e)", "ln", "\\ln", false),
        new MenuItem("Sine", "sin", "\\sin", false),
        new MenuItem("Cosine", "cos", "\\cos", false),
        new MenuItem("Tangent", "tan", "\\tan", false),
        new MenuItem("Define a function", "f(x)", "f(x)=", false,
            [
                new MenuItem("nth derivative of function", "f^n(x)", "f^{}(x)", false),
            ]),
        new MenuItem("Euler's number", "e", "e", false),
        new MenuItem("Pi", "pi", "\\pi", false),
        new MenuItem("Imaginary number.", "i", "i", false),
        new MenuItem("Infinity", "oo", "\\inf", false),
        new MenuItem("Lowercase theta", "theta", "\\theta", false),
        new MenuItem("Partial Derivative", "\\frac{\\partial}{\\partial x}", "\\frac{\\partial}{\\partial x}", true,
            [
                new MenuItem("Partial derivative of function", "\\frac{\\partial f}{\\partial x}", "\\frac{\\partial f}{\\partial x}", true),
            ]),
        new MenuItem("x unit vector", "\\vec{i}", "\\vec{i}", false,
            [
                new MenuItem("y unit vector", "\\vec{j}", "\\vec{j}", false),
                new MenuItem("z unit vector", "\\vec{k}", "\\vec{k}", false),
            ]),
        new MenuItem("Nabla", "\\nabla", "\\nabla", false,
            [
                new MenuItem("Divergence", "\\nabla\\cdot", "\nabla\\cdot", false),
                new MenuItem("Curl", "\\nabla\\times", "\\nabla\\cdot", false),
                //new MenuItem("Laplacian", "\\nabla^{2}", "\\nabla^{2}", false),
            ]),
        new MenuItem("Divergence", "\\text{div}(F)", "div(\\EMPTYGP{})", false),
        new MenuItem("Curl", "\\text{curl}(F)", "\\text{curl}(\\EMPTYGP{})", false),
        new MenuItem("Line integral", "\\oint_{C}\\vec{F} \\cdot d\\vec{r}", "F(x,y)=\\EMPTYGP{}\\vec{i}+\\EMPTYGP{}\\vec{j};x(t)=\\EMPTYGP{};y(t)=\\EMPTYGP{};\\EMPTYGP{} \\le t \\le \\EMPTYGP{};\\oint_{c}F(x,y) \\cdot dr", true,
            [
                new MenuItem("Line Integral", "\\oint_{C}f(x,y)\\ds", "f(x,y)=\\EMPTYGP{}; x(t)=\\EMPTYGP{};y(t)=\\EMPTYGP{}; \\EMPTYGP{} \\le t \\le \\EMPTYGP{}; \\oint_{c}f(x,y)ds", true),
            ], true),
        new MenuItem("Surface integral", "\\int\\int_{\\Sigma}f(x,y,z)\\d \\sigma", "f(x,y,z)=\\EMPTYGP{};x(t,s)=\\EMPTYGP{};y(t,s)=\\EMPTYGP{};z(t,s)=\\EMPTYGP{};\\EMPTYGP{}\\le t \\le \\EMPTYGP{};\\EMPTYGP{}\\le s \\le \\EMPTYGP{}; \\int\\int_{\\Sigma}f(x,y,z)d\\sigma", true),
    ]);

var symb = new TopicMenu(
    [
        new MenuItem("Underscore variable", "x_{n}", "x_{}", false),
        new MenuItem("Variable x", "x", "x", false),
        new MenuItem("Variable y", "y", "y", false),
        new MenuItem("Variable z", "z", "z", false),
        new MenuItem("Variable w", "w", "w", false),
        new MenuItem("Variable t", "t", "t", false),
        new MenuItem("Variable a", "a", "a", false),
        new MenuItem("Variable b", "b", "b", false),
        new MenuItem("Variable c", "c", "c", false),
        new MenuItem("Lowercase theta", "theta", "\\theta", false),
        new MenuItem("Lowercase alpha", "alpha", "\\alpha", false),
        new MenuItem("Lowercase beta", "beta", "\\beta", false),
        new MenuItem("Lowercase gamma", "gamma", "\\gamma", false),
        new MenuItem("Lowercase delta", "delta", "\\delta", false),
        new MenuItem("Lowercase epsilon", "epsilon", "\\epsilon", false),
        new MenuItem("Lowercase eta", "eta", "\\eta", false),
        new MenuItem("Lowercase kappa", "kappa", "\\kappa", false),
        new MenuItem("Lowercase lambda", "lambda", "\\lambda", false),
        new MenuItem("Lowercase mu", "mu", "\\mu", false),
        new MenuItem("Lowercase rho", "rho", "\\rho", false),
        new MenuItem("Lowercase sigma", "sigma", "\\sigma", false),
        new MenuItem("Lowercase tau", "tau", "\\tau", false),
        new MenuItem("Lowercase phi", "phi", "\\phi", false),
        new MenuItem("Lowercase psi", "psi", "\\psi", false),
        new MenuItem("Lowercase omega", "omega", "\\omega", false),
        new MenuItem("Zero", "0", "0", false),
        new MenuItem("One", "1", "1", false),
        new MenuItem("Two", "2", "2", false),
        new MenuItem("Three", "3", "3", false),
        new MenuItem("Four", "4", "4", false),
        new MenuItem("Five", "5", "5", false),
        new MenuItem("Six", "6", "6", false),
        new MenuItem("Seven", "7", "7", false),
        new MenuItem("Eight", "8", "8", false),
        new MenuItem("Nine", "9", "9", false),
    ]);

var prob = new TopicMenu(
    [
        new MenuItem("Summation", "\\sum_(i=0)^N", "\\sum^{}_{i=}", true),
        new MenuItem("Factorial", "!", "\\EMPTYGP{}!", false),
        new MenuItem("Binomial", "((n),(r))", "\\binom{}{}", true),
        new MenuItem("Choose", "nCr", "\\EMPTYGP{}\\text{C}\\EMPTYGP{}", false),
        new MenuItem("Permutation", "nPr", "\\EMPTYGP{}\\text{P}\\EMPTYGP{}", false),
    ]);

var linAlg = new TopicMenu(
    [
        new MenuItem("2D row vector", "[x,y]", "[\\EMPTYGP{},\\EMPTYGP{}]", true,
            [
                new MenuItem("3D row vector", "[x,y,z]", "[\\EMPTYGP{}, \\EMPTYGP{}, \\EMPTYGP{}]", true),
                new MenuItem("4D row vector", "[x,y,z,w]", "[\\EMPTYGP{},\\EMPTYGP{},\\EMPTYGP{},\\EMPTYGP{}]", true),
            ]),
        new MenuItem("2D column vector", "[[x],[y]]", "\\vectora{}{}", true,
            [
                new MenuItem("3D column vector", "[[x],[y],[z]]", "\\vectorb{}{}{}", true),
                new MenuItem("3D column vector", "[[x],[y],[z],[w]]", "\\vectorb{}{}{}{}", true),
            ]),
        new MenuItem("2x2 matrix", "[(a,b),(c,d)]", "\\vectora{\\EMPTYGP{}, \\EMPTYGP{}}{\\EMPTYGP{}, \\EMPTYGP{}}", true,
            [
                new MenuItem("3x3 matrix", "[[a, b, c], [d, e, f], [g, h, k]]", "\\vectorb{\\EMPTYGP{}, \\EMPTYGP{}, \\EMPTYGP{}}{\\EMPTYGP{}, \\EMPTYGP{}, \\EMPTYGP{}}{\\EMPTYGP{}, \\EMPTYGP{}, \\EMPTYGP{}}", true),
                new MenuItem("4x4 matrix", "[[a, b, c, d], [e, f, g, h], [k, l, m, n], [p, q, r, s]]", "\\vectorc{\\EMPTYGP{}, \\EMPTYGP{}, \\EMPTYGP{}, \\EMPTYGP{}}{\\EMPTYGP{}, \\EMPTYGP{}, \\EMPTYGP{}, \\EMPTYGP{}}{\\EMPTYGP{}, \\EMPTYGP{}, \\EMPTYGP{}, \\EMPTYGP{}} {\\EMPTYGP{}, \\EMPTYGP{}, \\EMPTYGP{}, \\EMPTYGP{}}", true),
            ]),
        new MenuItem("Dot product", "\\vec{a}\\cdot\\vec{b}", "\\cdot", false),
        new MenuItem("Cross product", "\\vec{a}\\times\\vec{b}", "\\times", false),
        new MenuItem("Determinant", "det(A)", "det(\\EMPTYGP{})", false),
        new MenuItem("Transpose", "[(a,b),(c,d)]^{T}", "^{T}", true),
        new MenuItem("Inverse", "[(a,b),(c,d)]^{-1}", "^{-1}", true),
        new MenuItem("x unit vector", "\\vec{i}", "\\vec{i}", false,
            [
                new MenuItem("y unit vector", "\\vec{j}", "\\vec{j}", false),
                new MenuItem("z unit vector", "\\vec{k}", "\\vec{k}", false),
            ]),
    ]);

var menus = [basic, trig, calc, symb, prob, linAlg];
var currentMenu = basic;

var inputBoxIds = [];
var selectedTextBox = null;

/////////////////////////
// Tool-bar
///////////////////////

function onToolBarEleClicked(clickedId, event) {
    if (typeof clickedId.substring === 'undefined')
        return;
    var index = clickedId.substring(3, clickedId.length);

    var subIndex = -1;
    if (index.indexOf('A') > -1) {
        var split = index.split('A');
        index = split[0];
        subIndex = split[1];
    }

    var clickedItem = currentMenu.items[index];
    if (subIndex != -1) {
        clickedItem = clickedItem.SubItems[subIndex];
    }

    if (clickedItem.addStr.indexOf(';') >= 0) {
        // This is a complete substitution of input.
        var splitInput = clickedItem.addStr.split(';');
        var html = "";
        inputBoxIds = [];
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

        mathInputChanged();
        return;
    }

    if (selectedTextBox == null & inputBoxIds.length > 0) {
        selectedTextBox = $("#mathInputSpan" + inputBoxIds[0]);
    }

    selectedTextBox.mathquill('write', clickedItem.addStr);


    mathInputChanged();

    return true;
}

$(document).ready(function () {
    $("body").click(function (e) {
        if ($(e.target).closest(".sub-toolbar-btn-space").length == 0)
            $(".sub-toolbar-btn-space").fadeOut();
    });

    function onSubjectBarBtnClicked() {
        // Make the correct subject selected.
        $(this).attr('class', 'subject-bar-btn-clicked');
        var id = jQuery(this).attr("id");
        var idIndex = id.substring(2, id.length);

        for (var i = 0; i < 7; ++i) {
            if (i == idIndex)
                continue;

            $("#sb" + i).attr('class', 'subject-bar-btn');
        }

        // Update the tool box with the correct items for the menu.
        var selectedMenu = menus[idIndex];
        currentMenu = selectedMenu;

        var toolBoxHTML = selectedMenu.outputItems();

        $('#toolbar-btn-space').html(toolBoxHTML);

        if (toolBoxHTML.indexOf("extra-space") != -1) {
            $(".toolbar-btn").css("margin-top", "-4px");
        }
        else if (idIndex == 5)
            $(".toolbar-btn").css("margin-top", "-3px");

        MathJax.Hub.Queue(["Typeset", MathJax.Hub], function () {
            $(".sub-toolbar-btn-space").each(function () {
                $(this).hide();
            });

            $(".toolbar-btn-dropdown").each(function () {
                var parentHeight = $(this).parent().height();
                var parentWidth = $(this).parent().width();
                if ($(this).hasClass("extra-space")) {
                    parentWidth += 30.0;
                }

                var paddingHeight = ((parentHeight - 19.0) / 2.0);
                //$(this).css('padding-top', paddingHeight + "px");
                //$(this).css('padding-bottom', paddingHeight + "px");
                $(this).css('height', parentHeight + "px");
                $(this).css('line-height', parentHeight + "px");
                $(this).css('top', "-" + parentHeight + "px");
                $(this).css('margin-bottom', "-" + parentHeight + "px");
                var factor = Math.pow(parentWidth, new Number(1.0)) / 1.4;
                $(this).css('left', (factor) + 'px');
                $(this).css('width', "20px");

                $(this).click(function (e) {
                    var subBtns = $(this).parent().children(".sub-toolbar-btn-space");
                    subBtns.toggle();
                    $(".sub-toolbar-btn-space").each(function () {
                        if ($(this).attr('id') != subBtns.attr('id')) {
                            if ($(this).is(":visible"))
                                $(this).hide();
                        }
                    });

                    return false;
                });
            });

            $(".sub-toolbar-btn-space").mouseleave(function () {
                setTimeout(function () {
                    $(".sub-toolbar-btn-space").fadeOut();
                }, 2000);
            });
            $(".sub-toolbar-btn-space").mouseenter(function () {
                $(this).stop(true, true).fadeIn();
            });
        });
    }

    for (var i = 0; i < 7; ++i) {
        $("#sb" + i.toString()).click(onSubjectBarBtnClicked);
    }

    // Create the toolbar.
    var operatorsHTML = basic.outputItems();
    $("#toolbar-btn-space").html(operatorsHTML);

    

    // Create the multi-line help pop up.
    if (getCookie('visited')) {
        // The user has been to this page before.
    }
    else {
        setCookie('visited', true, 999);
        // This is the user's first time on this page.

        setTimeout(function () {
            $("<div class='small-popup pop'><p>Multi-line input is great. Check out <a href='/multiline'>mathologica.com/multiline</a> for more.</p><a style='margin-top: 0px;' class='close' href='#'>Close</a></div>").insertAfter("#add-btn-id");

            $(".close").live('click', function () {
                $(".pop").remove();
                return false;
            });
        }, 2000);
    }
});


function getLatexInput() {
    var latex = "";
    var lastAdded = false;
    for (var i = 0; i < inputBoxIds.length; ++i) {
        var addLatex = $("#mathInputSpan" + inputBoxIds[i]).mathquill('latex');

        if (lastAdded)
            latex += "; ";

        if (addLatex != "") {
            latex += addLatex;
            lastAdded = true;
        }
        else
            lastAdded = false;
    }

    return latex;
}

function addInputBtn_Clicked() {
    var html = $("#input-list").html();

    var ni = inputBoxIds.length;

    var inputBoxHtml = createInputBox(ni, ni == 0);

    html = html + inputBoxHtml;

    $("#input-list").html(html);
    inputBoxIds.push(ni);

    updateInputBoxes();

    $("#mathInputSpan" + ni).focus();

    selectedTextBox = $("#mathInputSpan" + ni);

    mathInputChanged();
}

function setLatexInput(setInput) {
    if (selectedTextBox != null) {
        selectedTextBox.mathquill('latex', setInput);
        selectedTextBox.focus();
    }
}

function updateInputBoxes() {
    for (var i = 0; i < inputBoxIds.length; ++i) {
        var index = inputBoxIds[i];

        var inputSpan = $("#mathInputSpan" + index);

        var innerLatex = inputSpan.mathquill('latex');
        inputSpan.html(innerLatex);

        inputSpan.mathquill("editable");
    }
}

function clearInputBtn_Clicked() {
    if (inputBoxIds.length == 1) {
        // Clear the final text box.

        var createdBoxHtml = createInputBox(0, true);
        $("#input-list").html(createdBoxHtml);
        updateInputBoxes();

        mathInputChanged(null);

        return;
    }

    var index = Number(inputBoxIds.length - 1);

    var html = $("#input-list").html();

    var removeIndex = -1;
    for (var i = 0; i < inputBoxIds.length; ++i) {
        if (inputBoxIds[i] == index) {
            removeIndex = i;
            break;
        }
    }

    var listEles = html.split("</li>");

    for (var i = 0; i < listEles.length; ++i) {
        if (listEles[i] == "") {
            listEles.splice(i--, 1);
            continue;
        }

        listEles[i] = listEles[i] + "</li>";
    }

    if (removeIndex == -1) {
        // Some error message thing goes here.
    }

    listEles.splice(removeIndex, 1);

    html = "";
    for (var i = 0; i < listEles.length; ++i) {
        html += listEles[i];
    }

    $("#input-list").html(html);

    inputBoxIds.splice(index, 1);

    updateInputBoxes();

    mathInputChanged();
}

function inputBoxLostFocus() {
}

function inputBoxGainedFocus() {
}

function createInputBox(index, hasInputQuery) {
    var inputBoxHtml;
    inputBoxHtml = "<li id='inputEle" + index + "'>";
    inputBoxHtml += "<div class='input-txt-box-area'>";
    if (hasInputQuery)
        inputBoxHtml += "<div class='text-notice'>&gt;</div>";
    else {
        inputBoxHtml += "<div style='visibility: hidden' class='text-notice'>&gt;</div>";
    }
    inputBoxHtml += "<span runat='server' onPaste='return false' id='mathInputSpan" + index + "' onkeyup='mathInputChanged(event);' class='mathquill-editable' onclick='onMathInputSpan_Clicked(this.id);'></span>";
    inputBoxHtml += "</div>";
    inputBoxHtml += "</li>";

    return inputBoxHtml;
}

function fixInput(selectedTxtBox) {
    var latex = selectedTxtBox.mathquill('latex');
    var replaced = latex.replace(/(\\)?sqrt/g, function ($0, $1) { return $1 ? $0 : '\\sqrt{}'; });
    replaced = replaced.replace(/<=/g, function ($0, $1) { return $1 ? $0 : '\\le'; });
    replaced = replaced.replace(/>=/g, function ($0, $1) { return $1 ? $0 : '\\ge'; });

    var fixSymbols =
        [
            //'arcsin',
            //'arccos',
            //'arctan',

            //'arccsc',
            //'arcsec',
            //'arccot',

            //'sin',
            //'cos',
            //'tan',

            //'csc',
            //'sec',
            //'cot',

            //'ln',
            //'log',

            'pi'
        ];

    for (var i = 0; i < fixSymbols.length; ++i) {
        var fixSymbol = fixSymbols[i];
        replaced = replaced.replace(new RegExp("(\\\\|arc)?" + fixSymbol, 'g'),
            function ($0, $1) { return $1 ? $0 : '\\' + fixSymbol; });
    }

    if (replaced != latex) {
        selectedTxtBox.mathquill('latex', replaced);
    }
}

function htmlEncode(value) {
    return $('<div/>').text(value).html();
}

function htmlDecode(value) {
    return $('<div/>').html(value).text();
}

function onMathInputSpan_Clicked(clickedId) {
    selectedTextBox = $("#" + clickedId);
    selectedTextBox.focus();
}

function enterScrollMode() {
    $("#to-bottom-btn").show();
    $("#eval-space").hide();
    $("#parse-error-txt").hide();

    $("#work-space").css('height', 'moz-calc(100% - 204px)');
    $("#work-space").css('height', 'webkit-calc(100% - 204px)');
    $("#work-space").css('height', '-o-calc(100% - 204px)');
    $("#work-space").css('height', 'calc(100% - 204px)');
}

function exitScrollMode() {
    $("#to-bottom-btn").hide();
    $("#eval-space").show(200);
    $("#parse-error-txt").show(200);

    $("#work-space").css('height', 'moz-calc(100% - 264px)');
    $("#work-space").css('height', 'webkit-calc(100% - 264px)');
    $("#work-space").css('height', '-o-calc(100% - 264px)');
    $("#work-space").css('height', 'calc(100% - 264px)');
}