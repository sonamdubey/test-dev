var directionRight = { direction: 'right' };

// brand
$('#brand-filter-selection').on('click', function () {
    slideInDrawer.open($('#brand-slideIn-drawer'));
    appendState('brand-drawer');
});

$('#close-brand-slideIn-drawer').on('click', function () {
    slideInDrawer.close($('#brand-slideIn-drawer'));
    history.back();
});

// year
$('#year-filter-selection').on('click', function () {
    slideInDrawer.open($('#year-slideIn-drawer'));
    appendState('year-drawer');
});

$('#close-year-slideIn-drawer').on('click', function () {
    slideInDrawer.close($('#year-slideIn-drawer'));
    history.back();
});

var objMakesList = $("#brand-slideIn-drawer ul li").slice(1), makeInputEle = $("#brand-slideIn-drawer input[type='text']");

var slideInDrawer = {
    open: function (container) {

        if (container) {
            container.show(effect, directionRight, duration, function () {
                container.addClass('fix-header-input');
            });
            windowScreen.lock();
        }
    },

    close: function (container) {
        if (container) {
            container.hide(effect, directionRight, duration, function () { });
            container.removeClass('fix-header-input');
            windowScreen.unlock();
        }
    }
};

var windowScreen = {
    htmlElement: $('html'),

    bodyElement: $('body'),

    lock: function () {
        if ($(document).height() > $(window).height()) {
            var windowScrollTop = windowScreen.htmlElement.scrollTop() ? windowScreen.htmlElement.scrollTop() : windowScreen.bodyElement.scrollTop();
            if (windowScrollTop < 0) {
                windowScrollTop = 0;
            }
            windowScreen.htmlElement.addClass('lock-browser-scroll').css('top', -windowScrollTop);
        }
    },

    unlock: function () {
        var windowScrollTop = parseInt(windowScreen.htmlElement.css('top'));

        windowScreen.htmlElement.removeClass('lock-browser-scroll');
        $('html, body').scrollTop(-windowScrollTop);
    }
};

/* popup state */
var appendState = function (state) {
    window.history.pushState(state, '', '');
};

$(window).on('popstate', function (event) {
    if ($('#brand-slideIn-drawer').is(':visible')) {
        slideInDrawer.close($('#brand-slideIn-drawer'));
    }
    if ($('#year-slideIn-drawer').is(':visible')) {
        slideInDrawer.close($('#year-slideIn-drawer'));
    }
});