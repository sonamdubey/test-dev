function hasSpecialCharacters(strModel) {
    var isValid = true;
    if (/^[0-9a-z\-\s]+$/.test(strModel)) {
        isValid = false;
    }
    return isValid;
}

function removeHyphens(str) {
    str = str.replace(/^\-+|\-+$/g, '');
    return str;
}

function showHideMatchError(element, error) {
    if (error) {
        element.parent().find('.error-icon').removeClass('hide');
        element.parent().find('.bw-blackbg-tooltip').removeClass('hide');
        element.addClass('border-red')
    }
    else {
        element.parent().find('.error-icon').addClass('hide');
        element.parent().find('.bw-blackbg-tooltip').addClass('hide');
        element.removeClass('border-red');
    }
}
function showToast(msg) {
    $('.toast').text(msg).stop().fadeIn(400).delay(3000).fadeOut(400);
}

// navigation
$('#nav-btn').on('click', function () {
    navDrawer.open();
    appendState('navDrawer');
});

var navDrawer = {
    nav: $('#nav-drawer'),

    open: function () {
        navDrawer.nav.addClass('drawer-active')
        blackOverlay.active();
    },

    close: function () {
        navDrawer.nav.removeClass('drawer-active');
        blackOverlay.inactive();
    }
};

// escape key
$(document).keydown(function (e) {
    if (e.keyCode == 27) {
        if ($('#nav-drawer').hasClass('drawer-active')) {
            navDrawer.close();
        }
    }
});

var blackOverlay = {
    bodyElement: $('body'),

    active: function () {
        blackOverlay.bodyElement.addClass('black-overlay-active');
    },

    inactive: function () {
        blackOverlay.bodyElement.removeClass('black-overlay-active');
    },
};

// nav drawer accordion
$('#nav-drawer .collapsible').on('click', '.collapsible-header', function () {
    collapsible.toggleBody($(this), $('#nav-drawer'));
});

var collapsible = {

    closeAllBody: function (elementParent) {
        var headers = $(elementParent).find('.collapsible-header.active');
        headers.next('.collapsible-body').slideUp();
        headers.removeClass('active');
    },

    toggleBody: function (elementHeader, elementParent) {
        var element = $(elementHeader);

        if (!element.hasClass('active')) {
            collapsible.closeAllBody(elementParent);
            element.addClass('active');
            element.next('.collapsible-body').slideDown();
        }
        else {
            element.removeClass('active');
            element.next('.collapsible-body').slideUp();
        }
    }    
};

$('#black-overlay').on('click', function () {
    $(window).trigger('popstate');
    history.back();
});

/* popup state */
var appendState = function (state) {
    window.history.pushState(state, '', '');
};

$(window).on('popstate', function (event) {
    if ($('#nav-drawer').hasClass('drawer-active')) {
        navDrawer.close();
    }
});