var TDLpath = $(`#TDLpath`).val();
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
        let docFrag_sec = $(document.createDocumentFragment());
        $.each(data, function (i, i_val) {
            let eleT ="";
            let eleT_sec ="";
            if (i <= 4) {
                eleT = $(`<tr></tr>`).append(`<td>${i_val.交辦事項id}</td>
                                        <td>${i_val.表單類型}</td>
                                        <td>${i_val.表單內容}</td>
                                        <td>${i_val.表單狀態}</td>
                                        <td><a href = "${TDL_DTpath}/?listNum=${i_val.交辦事項id}"><button class= "btn btn-primary mb-3 btnTDL_DT">詳細資料
                                        </button></a></td>`
                );
            }
            else {
                eleT_sec = $(`<tr></tr>`).append(`<td>${i_val.交辦事項id}</td>
                                        <td>${i_val.表單類型}</td>
                                        <td>${i_val.表單內容}</td>
                                        <td>${i_val.表單狀態}</td>
                                        <td><a href = "${TDL_DTpath}/?listNum=${i_val.交辦事項id}"><button class= "btn btn-primary mb-3 btnTDL_DT">詳細資料
                                        </button></a></td>`
                );
            }
            docFrag.append(eleT);
            docFrag_sec.append(eleT_sec);
        });
        $("#ToDolist_tbody").prepend(docFrag);
        //$(".tr_Hide").css("display", "none");
        $("#ToDolist_tbody_sec").append(docFrag_sec);
    });

$("#tfoot_click").on("click", function () {
    $("#ToDolist_tbody_sec").toggle(2000);
    console.log(`123`);
});


/*  < a href = "${TDL_DTpath}" ></a >*/