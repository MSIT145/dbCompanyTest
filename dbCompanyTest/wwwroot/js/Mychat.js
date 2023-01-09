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
    let Mymsg = $(`#msg`).val();
    if (Mymsg) {
        console.log(Mymsg);
        $(`.chatBBody`).append(`<div class="chat">
                <div class="myName">我</div>
                <div class="myChat">
                    ${Mymsg}
                </div>
            </div>`);
        $(`#msg`).val("");
    }
});