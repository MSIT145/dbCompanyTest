var TDLpath = $(`#TDLpath`).val();

console.log(`PartialView`)


$.ajax({
    url: `${TDLpath}`,
    type: "GET",
    data: { "stf": stf },
    dataType: "json"
})
    .done(data => {
        let docFrag = $(document.createDocumentFragment());
        $.each(data, function (i, i_val) {
            const eleT = $(`<tr></tr>`).append(`<td>${i_val.表單類型}</td>
                                        <td>${i_val.表單內容}</td>
                                        <td>${i_val.表單狀態}</td>`);
            docFrag.append(eleT);
        });
        $("#ToDolist_tbody").html(docFrag);
    })