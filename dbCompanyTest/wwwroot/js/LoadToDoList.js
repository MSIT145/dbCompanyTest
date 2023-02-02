﻿var TDLpath = $(`#TDLpath`).val();
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
                eleT = $(`<tr></tr>`).append(`<td class="col-1">${i_val.交辦事項id}</td>
                                        <td class="col-1">${i_val.表單類型}</td>
                                        <td class="col-3">${i_val.表單內容}</td>
                                        <td class="col-1">${i_val.表單狀態}</td>
                                        <td class="col-1"><a href = "${TDL_DTpath}/?listNum=${i_val.交辦事項id}"><button class= "btn btn-primary mb-3 btnTDL_DT">詳細資料
                                        </button></a></td>`
                );
                docFrag.append(eleT);
            }
            else {
                eleT_sec = $(`<tr></tr>`).append(`<td class="col-1">${i_val.交辦事項id}</td>
                                        <td class="col-1">${i_val.表單類型}</td>
                                        <td class="col-3">${i_val.表單內容}</td>
                                        <td class="col-1">${i_val.表單狀態}</td>
                                        <td class="col-1"><a href = "${TDL_DTpath}/?listNum=${i_val.交辦事項id}"><button class= "btn btn-primary mb-3 btnTDL_DT">詳細資料
                                        </button></a></td>`
                );
                docFrag_sec.append(eleT_sec);
            }
            
           
        });
        $("#ToDolist_tbody").prepend(docFrag);
        $("#ToDolist_tbody_sec").append(docFrag_sec);
    });

//$("#tfoot_click").on("click", function () {
//    $("#ToDolist_tbody_sec").toggle(2000);
//    console.log(`123`);
//});
//$("#tfoot_click").click(function () {
//    console.log(`123456`)
//    $("#123").toggleClass("active");

//    //if ($(this).text() == "Open")
//    //    $(this).text("Close")
//    //else
//    //    $(this).text("Open");

//});

/*  < a href = "${TDL_DTpath}" ></a >*/