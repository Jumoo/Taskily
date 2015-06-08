
function bindOrderButtons() {

    $(".up").click(function () {
        var picked = $(this).parents('li');
        var above = picked.prev();

        if (above.length > 0) {
            picked.detach().insertBefore(above);
            checkTopBottom(picked);
            checkTopBottom(above);
        }

        updateIdList(picked.parent());

    });

    $(".down").click(function () {
        var picked = $(this).parents('li');
        var below = picked.next();

        if (below.length > 0) {
            picked.detach().insertAfter(below);

            checkTopBottom(picked);
            checkTopBottom(below);
        }

        updateIdList(picked.parent());
    });
}

function checkNodeTopBottom(node) {
    var classBtnHide = "btn-hidden";

    var above = node.previousElementSibling;
    var below = node.nextElementSibling;

    var upbtn = node.querySelector(".order-buttons .up");
    if (above == null) {
        upbtn.classList.add(classBtnHide);
    }
    else {
        upbtn.classList.remove(classBtnHide);
    }

    var dwnbtn = node.querySelector(".order-buttons .down");
    if (below == null) {
        dwnbtn.classList.add(classBtnHide);
    }
    else {
        dwnbtn.classList.remove(classBtnHide);
    }

}

function checkTopBottom(item) {
    // is it the top
    var btnClass = "btn-hidden";

    var top, bottom;

    top = item.prev();
    if (top.length == 0) {
        item.find(".up").addClass(btnClass);
    }
    else {
        item.find(".up").removeClass(btnClass);
    }

    bottom = item.next();
    if (bottom.length == 0) {
        item.find(".down").addClass(btnClass);
    }
    else {
        item.find(".down").removeClass(btnClass);
    }
}


function updateIdList(list)
{
    var idlist = '';
    $.each(list.children(), function (i, item) {

        checkNodeTopBottom(item);

        idlist = idlist + item.getAttribute('data-id');

        if (i < list.children().length - 1) {
            idlist = idlist + ',';
        }

    });
    $("#taskorder").val(idlist);
}