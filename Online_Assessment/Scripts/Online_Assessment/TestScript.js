﻿$(document).ready(function () {
    Get_test_lists();
});

function Get_test_lists() {
    $.ajax({
        url: '/Project/Get_test_lists',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $("#test_table").DataTable().clear().destroy();
            $("#test_table").DataTable({
                data: data,
                columns: [
                    {
                        data: '',
                        render: function (data, type, row) {
                            return row.Test_Id + "<select id='result_dropdown' onclick='results_for_admin(" + row.Test_Id + ")'><option>See Results</option></select>";
                        }
                    },
                    { data: "Test_name" },
                    {
                        data: '',
                        render: function (data, type, row) {
                            return sqldatetojsdate(row.Created_date);
                        }
                    },
                    {
                        data: '',
                        render: function (data, type, row) {
                            return sqldatetojsdate(row.Start_date);
                        }
                    },
                    {
                        data: '',
                        render: function (data, type, row) {
                            return sqldatetojsdate(row.End_date);
                        }
                    },
                    {

                        data: '',
                        render: function (data, type, row) {
                            return sqltimetojstime(row.Duration);
                        }
                    },
                    {
                        data: '',
                        render: function (data, type, row) {
                            return '<button onclick="Invite_users(' + row.Test_Id + ')" class="btn btn-success">Invite Users</button>' + ' ' +
                                '<button onclick="Add_edit_question(' + row.Test_Id + ')" class="btn btn-primary">Add/Edit Questions</button>';
                        }
                    }
                ]
            });
        },
        error: function () {
            alert("Get test list fail");
        }
    });
};
function create_test() {
    var test_details = {
        Test_name: $("#test_name").val(),
        Start_date: decodeURIComponent($("#startdate").val()),
        End_date: decodeURIComponent($("#enddate").val()),
        Duration: decodeURIComponent($("#duration").val()) + ':00'
    };
$.ajax({
    url: '/Project/Create_test',
    type: 'POST',
    data: {test:JSON.stringify(test_details)},
    dataType: 'json',
    success: function () {
        alert("Succes");
    },
    error: function () {
        alert("fail");
    }
});
};



function sqldatetojsdate(sqldate) {

    var timestamp = parseInt(sqldate.replace(/\/Date\((\d+)\)\//, '$1'), 10);

    var date = new Date(timestamp);

    var optionsDate = {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit'
    };
    var optionsTime = {
        hour: '2-digit',
        minute: '2-digit',
        second: '2-digit',
        hour12: false
    };

    var jsdate = date.toLocaleString(undefined, optionsDate);
    var jstime = date.toLocaleString(undefined, optionsTime);
    return jsdate + ' ' + jstime;
};

function sqltimetojstime(sqltime) {

    if (!sqltime) {
        return "00:00:00";
    }

    var hour = sqltime.Hours;
    var minute = sqltime.Minutes;
    var second = sqltime.Seconds;

    var formattedHour = String(hour).padStart(2, '0');
    var formattedMinute = String(minute).padStart(2, '0');
    var formattedSecond = String(second).padStart(2, '0');

    return formattedHour + ':' + formattedMinute + ':' + formattedSecond;
};

function Add_edit_question(Test_Id) {
    window.location.href = "/Project/Question_map_page/" + Test_Id;
};

function Invite_users(Test_Id) {
    window.location.href = "/Project/Invitation_page/" + Test_Id;
};

function View_results(Test_Id) {
    window.location.href = "/Project/Result_admin/" + Test_Id;
}

function results_for_admin() {
}
