/* taskily.js */

var Taskily = function (pickNo) {
    var tskly = {};

    closebtn = '<button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>';

    var pickTotal = pickNo;

    tskly.updatePickerBar = function () {
        var pickedCount = $('.picked').length;
        if (pickedCount == 1) {
            $('#pickCount').text(pickedCount + " item");

        }
        else {
            $('#pickCount').text(pickedCount + " items");

        }

        var picklist = '';
        $.each($('.picked'), function (i, picked) {
            picklist = picklist + '<span class="label label-info picker-item" data-taskid="' + $(picked).attr("data-id") + '"><span class="nowrap">' + $(picked).attr("data-name") + closebtn + '</span></span>';
        });


        $("#pickList").html(picklist);

        if (pickedCount == pickTotal) {
            $('#pickError').text('');
            $(".task-btn-clear").hide();
            $('.task-btn-pick').fadeIn();
            $('.picker-bar').animate({ backgroundColor: '#dfd' }, 200);
        }
        else {
            $(".task-btn-pick").hide();
            $(".task-btn-clear").fadeIn();
            if (pickedCount > pickTotal) {
                $('#pickError').html("please pick only <strong>" + pickTotal + "</strong> items.");
                $('.picker-bar').animate({ backgroundColor: '#fdd' }, 200);
            }
            else {
                $('#pickError').html("pick <strong>" + pickTotal + "</strong> items");
                $('.picker-bar').animate({ backgroundColor: '#ddd' }, 200);
            }
        }

        var cb = this.clearCallback; 

        $('.picker-item').on('closed.bs.alert', function () {
            clearbtn(this, cb);
        });
    }; 

        function clearbtn(item, cb) {
            var tId = $(item).attr("data-taskId");
            $('input[id=r_' + tId + ']').attr('checked', false);
            $('#lbl_' + tId).removeClass('picked');
            cb();
        };

    tskly.clearCallback = function () {
        taskily.updatePickerBar();
    };

    tskly.clear = function () {
        $('.picked').removeClass('picked');
        $('input:checked').attr('checked', false);
        taskily.updatePickerBar();
    };

    

    return tskly;
}

