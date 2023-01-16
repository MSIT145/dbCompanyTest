var TDLpath = $(`#TDLpath`).val();
console.log(`PartialView`)
var TDL_DTpath = $(`#TDL_DTpath`).val();
var TID;
$.ajax({
    url: `${TDLpath}`,
    type: "GET",
    data: { "stf": stf },
    dataType: "json"
})
    .done(data => {
        let docFrag = $(document.createDocumentFragment());
        $.each(data, function (i, i_val) {
            const eleT = $(`<tr></tr>`).append(`<td>${i_val.交辦事項id}</td>
                                        <td>${i_val.表單類型}</td>
                                        <td>${i_val.表單內容}</td>
                                        <td>${i_val.表單狀態}</td>
                                        <td><button class= "btn btn-primary mb-3 btnTDL_DT">詳細資料
                                        </button></td>`
            );
            docFrag.append(eleT);
        });
        $("#ToDolist_tbody").html(docFrag);

        $(".btnTDL_DT").on('click', function () {
            let TID = $(this).parents("td").siblings("td").first().text()
           
        })
    });



  /*  < a href = "${TDL_DTpath}" ></a >*/