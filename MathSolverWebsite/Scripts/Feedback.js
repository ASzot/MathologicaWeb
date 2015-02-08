

window.onload = function () {
    var textareas = document.getElementsByTagName('textarea');
    var count = textareas.length;
    for (var i = 0; i < count; i++) {
        textareas[i].onkeydown = function (e) {
            if (this.id == "titleTxtArea" && (e.keyCode == 13 || e.which == 13)) {
                e.preventDefault();
            }
            else if (e.keyCode == 9 || e.which == 9) {
                e.preventDefault();
                var s = this.selectionStart;
                this.value = this.value.substring(0, this.selectionStart) + "\t" + this.value.substring(this.selectionEnd);
                this.selectionEnd = s + 1;
            }
        }
    }


};

function onSendClick() {
    var titleTxt = document.getElementById("titleTxtArea").value;
    var messageTxt = document.getElementById("messageTxtArea").value;
    window.open('mailto:test@example.com?subject=' + titleTxt + '&body=' + messageTxt);
}