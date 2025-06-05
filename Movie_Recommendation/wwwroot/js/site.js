$(document).ready(function () {
    var setText = $('.editorText');
    setText.each(function (index, element) {
        var cont = "";
        var child = element.children;
        for (let i = 0; i < child.length; i++) {
            cont = cont + child[i].outerText;
        }
        var set = "";
        if (cont.split(/\s+/).length > 20) {
            set = cont.split(/\s+/).slice(0, 20).join(" ") + "...";
        } else {
            set = cont.split(/\s+/).slice(0, 20).join(" ");
        }
        element.textContent = set;
    });


})