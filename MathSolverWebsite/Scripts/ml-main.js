
function MenuItem(name, displayTex, addStr, useLargerIcon) {
    this.name = name;
    this.displayTex = displayTex;
    this.useLargerIcon = useLargerIcon;
    this.addStr = addStr;
}

function TopicMenu(items) {
    this.items = items;
    this.outputItems = function () {
        var htmlStr = "";
        for (var i = 0; i < this.items.length; ++i) {
            var currentItem = this.items[i];
            var thisElementId = "tme" + i.toString();
            htmlStr += "<div class='toolbar-btn noselect' onclick='onToolBarEleClicked(this.id);' id='" + thisElementId + "' title='" + currentItem.name + "'>";
            var iconStyling = currentItem.useLargerIcon ? "toolbar-icon-larger" : "toolbar-icon";
            htmlStr += "<div class='" + iconStyling + "'>`" + currentItem.displayTex + "`</div>";
            htmlStr += "</div>";
        }

        return htmlStr;
    };
}

var basic = new TopicMenu(
    [
        new MenuItem("Plus", "+", "+", false),
        new MenuItem("Minus", "-", "-", false),
        new MenuItem("Multiply", "*", "*", false),
        new MenuItem("Fraction", "x/y", "\\frac{}{}", true),
        new MenuItem("Divide", "-:", "\\div", false),
        new MenuItem("Divide", "/", "/", false),
        new MenuItem("Variable x", "x", "x", false),
        new MenuItem("Variable y", "y", "y", false),
        new MenuItem("Variable z", "z", "z", false),
        new MenuItem("Lowercase theta", "theta", "\\theta", false),
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
        new MenuItem("Euler's number", "e", "e", false),
        new MenuItem("Pi", "pi", "\\pi", false),
        new MenuItem("Imaginary number.", "i", "i", false),
    ]);

var trig = new TopicMenu(
    [
        new MenuItem("Derivative", "d/(dx)", "\\frac{d}{dx}", true),
        new MenuItem("Derivative of function", "(df)/(dx)", "\\frac{df}{dx}", true),
        new MenuItem("Indefinite integral", "\\int", "\\int", true),
        new MenuItem("Definite integral", "\\int_{a}^{b}", "\\int_{}^{}", true),
        new MenuItem("Limit", "\\lim_(x\\toh)", "\\lim_{x\\to}", false),
        new MenuItem("Summation", "\\sum_(i=0)^N", "\\sum^{}_{i=}", true),
        new MenuItem("Natural log (log base e)", "ln", "\\ln", false),
        new MenuItem("Sine", "sin", "\\sin", false),
        new MenuItem("Cosine", "cos", "\\cos", false),
        new MenuItem("Tangent", "tan", "\\tan", false),
        new MenuItem("Dot", "@", "\\circ", false),
        new MenuItem("Define a function", "f(x)=", "f(x)=", false),
        new MenuItem("nth derivative of function", "f^n(x)", "f^{}(x)", false),
        new MenuItem("Euler's number", "e", "e", false),
        new MenuItem("Pi", "pi", "\\pi", false),
        new MenuItem("Imaginary number.", "i", "i", false),
        new MenuItem("Infinity", "oo", "\\inf", false),
        new MenuItem("Lowercase theta", "theta", "\\theta", false),
    ]);

var calc = new TopicMenu(
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
    ]);

var symb = new TopicMenu(
    [
        new MenuItem("Logarithm", "log", "\\log", false),
        new MenuItem("Natural log (log base e)", "ln", "\\ln", false),
        new MenuItem("Logarithm base n", "log_{n}", "log_{}", false),
        new MenuItem("Sine", "sin", "\\sin", false),
        new MenuItem("Cosine", "cos", "\\cos", false),
        new MenuItem("Tangent", "tan", "\\tan", false),
        new MenuItem("Cosecant, the reciprocal of sine.", "csc", "\\csc", false),
        new MenuItem("Secant, the reciprocal of cosine.", "sec", "\\sec", false),
        new MenuItem("Cotangent, the reciprocal of tangent.", "cot", "\\cot", false),
        new MenuItem("Inverse sine", "arcsin", "\\arcsin", false),
        new MenuItem("Inverse cosine", "arccos", "\\arccos", false),
        new MenuItem("Inverse tangent", "arctan", "\\arctan", false),
        new MenuItem("Dot", "@", "\\circ", false),
        new MenuItem("Define a function", "f(x)=", "f(x)=", false),
    ]);

var prob = new TopicMenu(
    [
        new MenuItem("Number zero", "0", "0", false),
        new MenuItem("Number one", "1", "1", false),
        new MenuItem("Number two", "2", "2", false),
        new MenuItem("Number three", "3", "3", false),
        new MenuItem("Number four", "4", "4", false),
        new MenuItem("Number five", "5", "5", false),
        new MenuItem("Number six", "6", "6", false),
        new MenuItem("Number seven", "7", "7", false),
        new MenuItem("Number eight", "8", "8", false),
        new MenuItem("Number nine", "9", "9", false),
    ]);

var linAlg = new TopicMenu(
    [
        new MenuItem("Number zero", "\\alpha", "\\alpha", false),
        new MenuItem("Number one", "1", "1", false),
        new MenuItem("Number two", "2", "2", false),
        new MenuItem("Number three", "3", "3", false),
        new MenuItem("Number four", "4", "4", false),
        new MenuItem("Number five", "5", "5", false),
        new MenuItem("Number six", "6", "6", false),
        new MenuItem("Number seven", "7", "7", false),
        new MenuItem("Number eight", "8", "8", false),
        new MenuItem("Number nine", "9", "9", false),
    ]);

var menus = [basic, trig, calc, symb, prob, linAlg];

/////////////////////////
// Tool-bar
///////////////////////


function onToolBarEleClicked(clickedId) {
    var index = clickedId.substring(3, clickedId.length);

    var clickedItem = currentMenu.items[index];


    var phvalue = selectedTextBox.attr("placeholder");
    var val = selectedTextBox.mathquill('latex');
    if (phvalue == val) {
        selectedTextBox.mathquill('latex', "");
        selectedTextBox.css('color', 'black');
        selectedTextBox.css('font-style', 'normal');
        selectedTextBox.focus();
    }

    selectedTextBox.mathquill('write', clickedItem.addStr);

    selectedTextBox.focus();

    mathInputChangedSafe();

    hideModeBox();
}

$(document).ready(function () {
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
        MathJax.Hub.Queue(["Typeset", MathJax.Hub]);
    }

    for (var i = 0; i < 7; ++i) {
        $("#sb" + i.toString()).click(onSubjectBarBtnClicked);
    }

    // Create the toolbar.
    var operatorsHTML = operatorsMenu.outputItems();
    $("#toolbar-btn-space").html(operatorsHTML);
});



var inputBoxIds = [];
var selectedTextBox = null;

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

function fixInput(latex) {
    var replaced = latex.replace(/(\\)?sqrt/g, function ($0, $1) { return $1 ? $0 : '\\sqrt{}'; });
    replaced = replaced.replace(/<=/g, function ($0, $1) { return $1 ? $0 : '\\le'; });
    replaced = replaced.replace(/>=/g, function ($0, $1) { return $1 ? $0 : '\\ge'; });

    var fixSymbols =
        [
            'arcsin',
            'arccos',
            'arctan',

            'arccsc',
            'arcsec',
            'arccot',

            'sin',
            'cos',
            'tan',

            'csc',
            'sec',
            'cot',

            'ln',
            'log',

            'pi'
        ];

    for (var i = 0; i < fixSymbols.length; ++i) {
        var fixSymbol = fixSymbols[i];
        replaced = replaced.replace(new RegExp("(\\\\|arc)?" + fixSymbol, 'g'),
            function ($0, $1) { return $1 ? $0 : '\\' + fixSymbol; });
    }

    if (replaced != latex) {
        setLatexInput(replaced);
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
