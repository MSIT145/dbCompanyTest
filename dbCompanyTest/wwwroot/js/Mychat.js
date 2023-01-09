$(`.chatBody`).hide(500);
$(`.chatMain`).hover(function () {
    $(`.eyes-left`).toggleClass(`eyes-left-hover`);
    $(`.eyes-right`).toggleClass(`eyes-right-hover`);
    $(`.mouth`).toggleClass(`mouth-hover`);
}, function () {
    $(`.eyes-left`).toggleClass(`eyes-left-hover`);
    $(`.eyes-right`).toggleClass(`eyes-right-hover`);
    $(`.mouth`).toggleClass(`mouth-hover`);
}).click(function () {
    $(`.chatBody`).toggle(500);
});

$(`#send`).on(`click`, function () {
    let Mymsg = $(`#message`).val();
    if (Mymsg) {
        //console.log(Mymsg);
        $(`.chatBBody`).append(`<div class="chat">
                <div class="myName">我</div>
                <div class="myChat">
                    ${Mymsg}
                </div>
            </div>`);
        $(`#message`).val("");
    }
    console.log($(".chat")[$(".chatBBody").find(".chat").length - 1].scrollHeight);
    console.log($(".chat")[$(".chatBBody").find(".chat").length - 1]);
    $('.chatBBody').animate({
        scrollTop: $(".chatBBody").offset().top + $(".chat")[$(".chatBBody").find(".chat").length-1].scrollHeight
    }, 500);
});







var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.start().then(function () {
    console.log("Hub 連線完成");
}).catch(function (err) {
    alert('連線錯誤: ' + err.toString());
});

//connection.on("UpdList", function (jsonList) {
//    var list = JSON.parse(jsonList);
//    $("#IDList li").remove();
//    for (i = 0; i < list.length; i++) {
//        $("#IDList").append($("<li></li>").attr("class", "list-group-item").text(list[i]));
//    }
//});

connection.on("UpdSelfID", function (id) {
    $('#SelfID').html(id);
});

//connection.on("UpdContent", function (msg) {
//    $("#Content").append($("<li></li>").attr("class", "list-group-item").text(msg));
//});

$('#sendButton').on('click', function () {
    let selfID = $('#SelfID').html();
    let message = $('#message').val();
    //let sendToID = $('#sendToID').val();
    connection.invoke("SendMessage", selfID, message, /*sendToID*/).catch(function (err) {
        alert('傳送錯誤: ' + err.toString());
    });
});