
var TaskilyCharts = (function () {
    var tc = {};

    tc.loadChart = function (id, type, container) {
        google.setOnLoadCallback(function () {

            var cd = new google.visualization.DataTable();
            if (type == 'Table') {
                cd.addColumn('string', 'Task');
                cd.addColumn('number', 'Weight');
                cd.addColumn('number', 'Weight %');
                cd.addColumn('number', 'Count');
                cd.addColumn('number', 'Count %');
                cd.addColumn('number', 'Importance');
            }
            else {
                cd.addColumn('string', 'Task');
                cd.addColumn('number', type);
            }

            var chartFormat = '##';

            if (type == 'CountPercentage' || type == 'Weight Percentage') {
                chartFormat = '##.#%';
            }

            loadSurveyJson(id, type, cd, function () {

                var options = {
                    title: 'Tasks by ' + type,
                    legend: { position: 'none' },
                    colors: ['#5cb85c'],
                    hAxis: { format: chartFormat },
                    chartArea: { 'width': '60%', 'height': '85%' }
                }

                cd.sort([{ column: 1, desc: true }, { column: 0 }]);

                if (type == 'Table') {
                    var percent_formatter = new google.visualization.NumberFormat({ pattern: '##.#%' });
                    var num_formatter = new google.visualization.NumberFormat({ pattern: '###.00' });
                    percent_formatter.format(cd, 2);
                    percent_formatter.format(cd, 4);
                    num_formatter.format(cd, 5);

                    var table = new google.visualization.Table(container);
                    table.draw(cd, { showRowNumber: true });
                }
                else {
                    var chart = new google.visualization.BarChart(container);
                    chart.draw(cd, options);
                }
            });
        });
    };

    function loadSurveyJson(id, countType, chartData, callback) {
        $.ajax({
            url: '/Api/Stats/GetSummary/' + id,
            dataType: 'json',
            cache: true,
            success: function (data) {
                $.each(data.Tasks, function (i, task) {
                    switch (countType) {
                        case 'Count':
                            chartData.addRow([task.Name, task.Count]);
                            break;
                        case 'Weight':
                            chartData.addRow([task.Name, task.Weight]);
                            break;
                        case 'CountPercentage':
                            chartData.addRow([task.Name, task.CountPercent]);
                            break;
                        case 'Weight Percentage':
                            chartData.addRow([task.Name, task.WeightPercent]);
                            break;
                        case 'Table':
                            chartData.addRow([task.Name, task.Weight, task.WeightPercent, task.Count, task.CountPercent, task.Importance]);
                            break;
                    }
                });

                callback();
            }
        });
    }

    return tc; 

});