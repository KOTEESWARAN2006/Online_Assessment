$(document).ready(function () {
    alert("ok");
    $('input[type="checkbox"]').change(function () {
        $('input[type="checkbox"]').not(this).prop('checked', false).val(false);
        $(this).val(true);
    })
})

function Add_question() {
    var Question = {
        Subject_Id: $("#Subject_Id").val(),
        Question: $("#Question").val(),
        Difficulty_Id: $("#Difficulty_Id").val()
    }
    var Option = [];
    Option.push({
        Options: $("#Option1").val(),
        Answer: $("#checkbox1").val()
    });
    Option.push({
        Options: $("#Option2").val(),
        Answer: $("#checkbox2").val()
    });
    Option.push({
        Options: $("#Option3").val(),
        Answer: $("#checkbox3").val()
    });
    Option.push({
        Options: $("#Option4").val(),
        Answer: $("#checkbox4").val()
    });

    $.ajax({
        url: '/Project/Add_Question',
        type: 'POST',
        data: {Question:JSON.stringify(Question),Option:JSON.stringify(Option)},
        dataType: 'json',
        success: function () {
            alert("pass");
        },
        error: function () {
            alert("fail");
        }
    })
}