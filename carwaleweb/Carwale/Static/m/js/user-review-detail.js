        /* floating button */
        var floatingBtn = $('#floating-btn'),
            floatingBtnWrapper = $('#floating-btn-wrapper');

if ($.cookie('isNewReview') && $.cookie('isNewReview') == 'true') {
    floatingBtnWrapper.show();
    $('.write-review').removeClass('hide');
    $.cookie('isNewReview', 'false');
}

if (floatingBtnWrapper.is(':visible')) {
    window.addEventListener("scroll", toggleFloatingBtn);
}

var $window = $(window),
    windowHeight = $window.height() - floatingBtnWrapper.innerHeight();

function toggleFloatingBtn() {
    var windowPositionTop = $window.scrollTop(),
        btnWrapperOffset = floatingBtnWrapper.offset();

    if(windowPositionTop + windowHeight > btnWrapperOffset.top) {
        floatingBtn.removeClass('floating-btn bg-grey-transparent');
    }
    else if(windowPositionTop + windowHeight < btnWrapperOffset.top) {
        if(!floatingBtn.hasClass('floating-btn')) {
            floatingBtn.addClass('floating-btn bg-grey-transparent');
        }
    }
}