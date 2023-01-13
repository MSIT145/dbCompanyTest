var path = $(`#path`).val();
$.ajax({
    url: `${path}`,
    type: "GET",
    dataType: "json"
})
    .done(data => {
        let docFrag = $(document.createDocumentFragment());
        $.each(data, function (i, i_val) {
            const eleT = $(`<tr></tr>`).append(`<td>${i_val.訂單編號}</td>
                        <td>${i_val.客戶編號}</td>
                        <td>${i_val.商品名稱}</td>
                        <td>${i_val.尺寸種類}</td>
                        <td>${i_val.商品數量}</td>
                        <td>${i_val.送貨地址}</td>
                        <td>
                        <a asp-action="Edit" asp-route-id="${i_val.訂單編號}">詳細清單</a> 
                        </td>`);
            docFrag.append(eleT);
        });
        $("#Sheeplist_tbody").html(docFrag);
    })