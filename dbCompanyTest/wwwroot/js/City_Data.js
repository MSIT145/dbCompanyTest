$.each(data, function (i, i_value) { $("#city").append(`<option value="${i}">${i_value.name}</option>`) });
$.each(data[0].districts, function (p, p_value) { $("#town").append(`<option>${p_value.zip}${p_value.name}</option>`); })
$("#city").change(function () {
    let area = $(this).val();
    $("#town").empty();
    $.each(data[area].districts, function (j, j_value) {$("#town").append(`<option>${j_value.zip}${j_value.name}</option>`)})
})




