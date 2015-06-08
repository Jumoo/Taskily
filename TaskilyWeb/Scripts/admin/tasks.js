/*
 * taskapi. 
 */

var input_taskname_text = '<input type="text" id="task_name_{0}" value="{1}" class="form-control">';
var input_taskdesc_text = '<input type="text" id="task_desc_{0}" value="{1}" class="form-control">';

var btn_update_task = '<a href="#" class="btn-update-task btn btn-sm btn-success" data-taskid="{0}" id="task_update_btn_{0}">Update</a>&nbsp;';
var btn_remove_task = '<a href="#" class="{2} btn btn-sm btn-danger" data-taskid="{0}" id="taskbtn_{0}">{1}</a>';


var taskilyTasks = (function (survey, table) {

    var tsk = {};

    var tableID = table;
    var surveyID = survey; 

    tsk.loadTasks = function()
    {

        $.ajax({
            url: '/api/tasks/all/' + surveyID,
            type: 'GET',
            success: function(data)
            {
                for (var i = 0; i < data.length; i++) {
                    var activeclass = '';

                    if (data[i].Active == false) {
                        activeclass = 'task-row-inactive';
                    }                    

                    var row = ['<tr id="taskrow_' + data[i].ID + '" class="task-row '+ activeclass + '">',
                        '<td>' + input_taskname_text.format(data[i].ID, data[i].Name) + '</td>',
                        '<td>' + input_taskdesc_text.format(data[i].ID, data[i].Description) + '</td>'] ;

                    if (data[i].Active == true) {
                        row.push('<td>'
                            + btn_update_task.format(data[i].ID) 
                            + btn_remove_task.format(data[i].ID, 'Remove', 'btn-remove-task')
                            + '</td>');
                    }
                    else {
                        row.push('<td>' + btn_remove_task.format(data[i].ID, 'Activate', 'btn-activate-task') + '</td>') ;
                    }
                    row.push('</tr>');

                    $( row.join() ).appendTo(tableID);
                }

                bindTaskButtons();

            },
            error: function(msg)
            {
                alert(msg);
            }

        });
    
    }    

    function bindTaskButtons()
    {
        $(".btn-remove-task").click(function (e) {
            e.stopPropagation();
            e.preventDefault();
            var taskID = $(this).data("taskid");
            removeTask(taskID);
        });

        $(".btn-update-task").click(function (e) {
            e.stopPropagation();
            e.preventDefault();
            var taskID = $(this).data("taskid");
            updateTask(taskID);
        });

        $("#addTask").click(function (e) {
            e.stopPropagation();
            e.preventDefault();
            var tName = $("#newTask").val();
            var tDesc = $("#newDesc").val();
            addTask(tName, tDesc);
        });

        $(".btn-activate-task").click(function (e) {
            e.stopPropagation();
            e.preventDefault();
            var taskID = $(this).data("taskid");
            activateTask(taskID);
        });
    }

    function removeTask(taskId) {
        $.ajax({
            url: '/api/tasks/delete/' + taskId,
            type: 'DELETE',
            success: function (data) {

                if (data.Active == false) {
                    toggleButton(data.ID);
                    btn.unbind('click');
                    btn.click(function (e) {
                        e.stopPropagation();
                        var taskID = $(this).data("taskid");
                        activateTask(taskID);
                    });
                }
                else {
                    $("#taskrow_" + data.ID).fadeOut();
                }
            },
            error: function (data) {
                alert("Error removing task");
            }
        });
    }

    function updateTask(taskId)
    {
        var taskName = $("#task_name_" + taskId).val();
        var taskDesc = $("#task_desc_" + taskId).val();

        var task = {
            'ID': taskId,
            'SurveyID': surveyID,
            'Name': taskName,
            'Description': taskDesc
        };

        $.ajax({
            url: '/api/tasks/update/' + surveyID, 
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(task),
            success: function (data) {
                var row = $('#taskrow_' + data.ID).addClass('task-done') ;
                setTimeout(function () {
                    row.toggleClass('task-done');
                }, 1000);

            },
            error: function (msg) {
                alert('failed to update task');
            }
        });

    }

    function activateTask(taskId)
    {
        $.ajax({
            url: '/api/tasks/activate/' + taskId,
            type: 'PUT',
            success: function (data) {
                toggleButton(data.ID);
                btn.unbind('click');
                btn.click(function (e) {
                    e.stopPropagation();
                    var taskID = $(this).data("taskid");
                    removeTask(taskID);
                });
            },
            error: function (data) {
                alert("Error activating task");
            }
        });
    }

    function toggleButton(taskId)
    {
        var btn = $('#taskbtn_' + taskId);
        var row = $('#taskrow_' + taskId).addClass('task-done');

        if (btn.html() == 'Activate') {
            btn.addClass("btn-remove-task");
            btn.addClass("btn-danger");
            btn.removeClass("btn-activate-task");
            btn.removeClass("btn-warning");
            row.removeClass("task-row-inactive");
            btn.html("Remove");
        }
        else {
            btn.removeClass("btn-remove-task");
            btn.removeClass("btn-danger");
            btn.addClass("btn-activate-task");
            btn.addClass("btn-warning");
            btn.html("Activate");
            row.addClass("task-row-inactive");
        }

        setTimeout(function () {
            row.toggleClass('task-done');
        }, 1000);

    }

    function addTask(name, description) {
        var task = {
            'SurveyID': surveyID,
            'Active': true,
            'Name': name,
            'Description': description
        };

        $.ajax({
            url: '/api/tasks/add/' + surveyID,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(task),
            success: function (data) {
                $(
                  ['<tr id="taskrow_' + data.ID + '" class="task-row">',
                     '<td>' + input_taskname_text.format(data.ID, data.Name) + '</td>',
                     '<td>' + input_taskdesc_text.format(data.ID, data.Description) + '</td>',
                     '<td>' + btn_update_task.format(data.ID)
                     + btn_remove_task.format(data.ID, 'Remove', 'btn-remove-task') + '</td>',
                   '</tr>'
                  ].join()).appendTo(tableID);
            },
            error: function (msg) {
                alert( "Failed to add " + msg)
            }
        });
    }

    return tsk;
});

