
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
