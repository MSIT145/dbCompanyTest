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