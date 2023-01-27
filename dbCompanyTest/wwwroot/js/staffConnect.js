﻿var Staff_Home_StaffNum = $("#StaffNumpath").val();
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.start().then(async function () {
    const data = await fetch(`${Staff_Home_StaffNum}`);
    const StaffNum = await data.text();
    if (StaffNum == "fales")
        alert("連線逾時請重新登入");
    else
        connection.invoke("getName", StaffNum).catch(function (err) {
            alert('傳送錯誤: ' + err.toString());
        });
});

$("#inp_start").on("click", function () {
    let come_from_num = stf;
    let Send_To_num = $("#inp_super_vs").val();
    let msg = `您有新表單待簽`;
    connection.invoke("SendNotice", come_from_num, Send_To_num, msg).catch(function (err) {
        alert('傳送錯誤: ' + err.toString());
    });
});




connection.on("receive", function (msg) {
    $(`#Search_layout`).val(msg);

});