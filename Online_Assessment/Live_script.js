$(document).ready(function () {
    var Question_ids = [];
    var Test_id = $("#test_id").val();
    var User_id = $("#user_id").val();

    $.ajax({
        url: "/Project/Get_questions_for_test",
        type: "POST",
        data: { Test_id: Test_id },
        dataType: 'json',
        success: function (Question_list) {
            Question_list.forEach(function (question) {
                alert(question.Questions);
                Question_ids.push(question.Questions);
            });
            $("#Question_id").val(Question_list[0].Question_Id);
            $("#Question").text(Question_ids[0]);

            $.ajax({
                url: "/Project/Get_options_for_question",
                type: "POST",
                data: { Question_id: 1 },
                dataType: "json",
                success: function (Option_list) {
                    $("#option1").val(Option_list[0].Option_Id);
                    for (var i = 0; i < Option_list.length; i++) {
                        $("#option" + (i + 1)).val(Option_list[i].Option_Id);
                        $("#answer" + (i + 1)).text(Option_list[i].Options);
                    }
                },
                error: function () {
                    alert("Optionlist fail");
                }
            });
        },
        error: function () {
            alert("Questionlist fail");
        }
    });



    $.ajax({
        url: "/Project/Get_data_for_livetest",
        type: "POST",
        data: { Test_id: Test_id },
        dataType: "json",
        success: function (result) {
            var timer = sqltimetojstime(result);

            var splitedtimer = timer.split(":");
            var hour = parseInt(splitedtimer[0], 10);
            var minute = parseInt(splitedtimer[1], 10);
            var second = parseInt(splitedtimer[2], 10);

            function update_timer() {
                var displayhour = hour;
                var displayminute = minute;
                var displaysecond = second;
                if (hour < 10) {
                    displayhour = "0" + hour;
                }
                if (minute < 10) {
                    displayminute = "0" + minute;
                }
                if (second < 10) {
                    displaysecond = "0" + second;
                }
                $("#Timer").text(displayhour + ":" + displayminute + ":" + displaysecond);
            };

            update_timer();

            var Countdowntimer;

            if (!Countdowntimer) {

                Countdowntimer = setInterval(function () {
                    if (second > 0) {
                        second--;
                    }
                    else {
                        if (minute > 0) {
                            second = 59;
                            minute--;
                        }
                        else {
                            if (hour > 0) {
                                second = 59;
                                minute = 59;
                                hour--;
                            }
                            else {
                                clearInterval(Countdowntimer);
                            }
                        }
                    } update_timer();
                }, 1000);
            };
        },
        error: function () {
            alert("Unable to fetch timer");
        }
    });
});

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