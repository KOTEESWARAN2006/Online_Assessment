var Question_ids = [];
var currentquestion_index = 0;
function update_question_no() {
    $("#Question_no").html((currentquestion_index + 1) + ".");
};

$(document).ready(function () {
    update_question_no();


    $.ajax({
        url: "/Project/Get_data_for_livetest",
        type: "POST",
        data: { Test_id: $("#test_id").val() },
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
            }

            update_timer();

            var Countdowntimer = setInterval(function () {
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
                }
                update_timer();
            }, 1000);

            fetchQuestionIds();
        },
        error: function () {
            alert("Unable to fetch timer");
        }
    });







    function fetchQuestionIds() {
        $.ajax({
            url: "/Project/Get_only_questionids",
            type: "POST",
            dataType: "json",
            data: { test_id: $("#test_id").val() },
            success: function (Result) {
                Result.forEach(function (question_id) {
                    Question_ids.push(question_id);
                });
                Get_question_answer();
            },
            error: function () {
                alert("Question ID fetch failed");
            }
        });
    }

    Get_question_answer();






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

});

function prevclick() {


    if (currentquestion_index != 0) {
        currentquestion_index--;
        Get_question_answer();
        update_question_no();
    }
    else {
        alert("This is first question");
    }
};


function nextclick() {
    if (currentquestion_index < Question_ids.length - 1) {
        currentquestion_index++;
        update_question_no();
        Get_question_answer();
    } else {
        alert('No more questions.');
    }
};

function Get_question_answer() {

    if (Question_ids.length > 0 && currentquestion_index < Question_ids.length) {
        $.ajax({
            url: "/Project/Get_question_answer",
            type: "POST",
            dataType: "json",
            data: { Question_id: Question_ids[currentquestion_index] },
            success: function (Question_answer) {
                var source = $("#template").html();
                var template = Handlebars.compile(source);
                var html = template(Question_answer);
                $("#test_body").html(html);
                update_question_no()
            },
            error: function () {
                alert("Unable to fetch question and answer");
            }
        });
    }
};
