var TDLpath = $(`#TDLpath`).val();
$.ajax({
    url: `${TDLpath}`,
    type: "GET",
    data: { "stf": stf },
    dataType: "json"
})
    .done(data => {
        let docFrag = $(document.createDocumentFragment());
        $.each(data, function (i, i_val) {
            const eleT = $(`<tr></tr>`).append(`<td>${i_val.交辦內容}</td>
                                        <td>${i_val.批示}</td>
                                        <td>${i_val.狀態}</td>`);
            docFrag.append(eleT);
        });
        $("#ToDolist_tbody").html(docFrag);
    })