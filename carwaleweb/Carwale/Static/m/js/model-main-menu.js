var modelMenu = $('#model-main-menu');
var modelMenuUl = document.getElementById('model-section-list');
var pos = modelMenu.position();
var menusections = ["divModelImg"];
var isVersionPage = modelMenu.attr("isVersionPage") == "True";
var isModelHighlighted = modelMenu.has(".model-section-active").length > 0;
$("#model-section-list a[href*=#]").each(function (i, e) { var href = e.href.split('#'); if ($.trim(href[1]) != "") menusections[menusections.length] = href[1] });

function modelMenuScroll() {
if (!isVersionPage && (typeof isMileagePage=="undefined")) {
    var firstSection = modelMenu.find("li:first");
    var col = firstSection;
    $.each(menusections, function (i, e) {
        if ($("#" + e).is(":in-viewport")) {
            if (!(e == menusections[0])) col = modelMenu.find("a[href*=#" + e + "]").parent();
            return false;
        }
    });
    modelMenu.find("li").removeClass("model-section-active");
	if (isModelHighlighted || col != firstSection) col.addClass("model-section-active");
	if (col.length) modelMenuUl.scrollLeft = col[0].offsetLeft;
}
var windowpos = $(window).scrollTop();
if (pos && windowpos >= (pos.top)) {
    $('.blank-model-layer').css({ 'display': 'block', 'overflow': 'hidden' })
    $('.blank-model-layer').show();
    modelMenu.addClass('model-main-menu--fixed');
}
else {
    $('.blank-model-layer').hide();
    modelMenu.removeClass('model-main-menu--fixed');
}
}

window.addEventListener('scroll', debounce(function () {
    modelMenuScroll();
}, 0));

var activeSection = document.getElementsByClassName('model-section-active')[0];
if (activeSection != undefined) {
    activeSection.scrollIntoView(false);
}