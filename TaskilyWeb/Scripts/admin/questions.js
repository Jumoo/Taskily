/*
 * the question api
 */

var taskilyQuestions = (function (survey, table) {
    var qst = {};

    var tableID = table;
    var surveyID = survey;

    qst.loadQuestions = function ()
    {

        $.ajax({
            url: '/api/questions/all/' + surveyID,
            type: 'GET',
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    var row = ['<tr id="qstrow_' + data[i].ID + '">',
                        '<td>' + data[i].Name + '</td>',
                        '<td>' + data[i].Text + '</td>'];

                    row.push('<td>'
                        + '<a href="#" class="btn btn-danger btn-remove-qst" data-qstid="' + data[i].ID + '" id="qstbtn_' + data[i].ID + '">Remove</a>&nbsp;'
                        + '<a href="#" class="btn btn-primary btn-edit-qst" data-qstid="' + data[i].ID + '" id="qstbtn_' + data[i].ID + '">Edit</a>'
                        + '</td>');

                    row.push('</tr>');

                    $(row.join()).appendTo(tableID);
                }

                // bind.
                bindQstButtons();
            },
            error: function (msg) {
                alert(msg);
            }
        });
    }

    function bindQstButtons() {
        $(".btn-remove-qst").click(function (e) {
            e.stopPropagation();
            e.preventDefault();
            var qID = $(this).data("qstid");
            removeQuestion(qID);
        });

        $("#addQst").click(function (e) {
            e.stopPropagation();
            e.preventDefault();
            // lots of things to add.
            addQuestion()
        });

        $(".btn-edit-qst").click(function (e) {
            e.stopPropagation();
            e.preventDefault();
            var qID = $(this).data("qstid");
            editQuestion(qID)
        });

        $("#clearQst").click(function (e) {
            e.preventDefault();
            e.stopPropagation();
            clearQuestion();
        });

        $("#qstType").change(function () {
            if (this.value == "2") {
                $('#question-choices').fadeIn();
            }
            else {
                $('#question-choices').fadeOut();
            }
        })
    }


    function removeQuestion(id)
    {
        $.ajax({
            url: '/api/questions/delete/' + id,
            type: 'DELETE',
            success: function (data) {
                $('#qstrow_' + data.ID).fadeOut();
            },
            error: function (data) {
                alert("Problems removing the question");
            }
        });
    }

    function editQuestion(id)
    {
        $.ajax({
            url: '/api/questions/Get/' + id,
            type: 'GET',
            success: function(data)
            {
                $('#qstID').val(data.ID);
                $('#qstName').val(data.Name);
                $('#qstText').val(data.Text);
                $('#qstType').val(data.Type);
                $('#qstData').val(data.Data);
                $('#addQst').html("Save question");

                if (data.Type == 2)
                {
                    $('#question-choices').fadeIn();
                }
                else {
                    $('#question-choices').fadeOut();
                }
            },
            error: function (data) {
                alert('problems getting the question');
            }
        })
    }

    function clearQuestion()
    {
        $('#qstID').val(0);
        $('#qstName').val('');
        $('#qstText').val('');
        $('#qstType').val(0);
        $('#qstData').val('');
        $('#addQst').html("Add question");
        $('#question-choices').fadeOut();
    }

    function addQuestion()
    {
        var id = $('#qstID').val();
        var name = $('#qstName').val();
        var text = $('#qstText').val();
        var type = $('#qstType').val();
        var qdata = $('#qstData').val();
        // some validation....


        if ( name == "" || text == "" )
        {
            alert("you need to enter some details first");
            return ; 
        }

        var question = {
            SurveyID: surveyID,
            Name: name,
            Text: text,
            Type: type,
            Data: qdata,
            ID: id
        };

        $.ajax({

            url: '/api/questions/add/' + surveyID,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(question),
            success: function (data) {

                var existing = $('#qstrow_' + data.ID);
                if ( existing.length > 0 )
                {
                    existing.remove();
                }
                
                $( ['<tr id="qstrow_' + data.ID + '">',
                    '<td>' + data.Name + '</td>',
                    '<td>' + data.Text + '</td>',
                    '<td>' + '<a href="#" class="btn btn-danger btn-remove-qst" data-qstid="' + data.ID + '" id="qstbtn_' + data.ID + '">Remove</a>&nbsp;' + 
                    '<a href="#" class="btn btn-primary btn-edit-qst" data-qstid="' + data.ID + '" id="qstbtn_' + data.ID + '">Edit</a>' + '</td>',
                    '</tr>'].join()).appendTo(tableID);

                clearQuestion();
                bindQstButtons();
            },
            error: function(msg)
            {
                alert("Falied to add " + msg)
            }

        });
        
    }

    return qst;
});