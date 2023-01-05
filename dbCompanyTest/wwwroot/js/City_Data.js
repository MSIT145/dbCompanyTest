$.each(data, function (i, i_value) { $("#city").append(`<option>${i_value.name}</option>`) });

$("#city").change(function () {
    let Myarea = $(`#city option:selected`).index();
    area(Myarea);
})
$(`#city`).val($(`#q1`).val());
if ($(`#q1`).val()) {
    area($(`#city`).index() + 1);
}

function area(a) {
    $("#town").empty();
    $.each(data[a].districts, function (j, j_value) { $("#town").append(`<option>${j_value.name}</option>`) })
    if ($(`#q2`).val()) {
        $(`#town`).val($(`#q2`).val());
        console.log($(`#q2`).val());
    }
}
