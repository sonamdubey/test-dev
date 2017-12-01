// review popup
ReviewPopup = $('#reviewPopup');
var effect = 'slide', directionDown = { direction: 'down' }, duration = 500, directionRight = { direction: 'right'};
var isDesktop = $('#d-review-popup__link')
docReady(function () {
    reviewPopup = {
        containerRightPosition: '463px',
        open: function (element) {
            if (isDesktop.length) {
                element.animate({ 'right': 0 });
                popup.lock();
            }
            else {
                element.show(effect, directionDown, duration, function () { });
                $('body').css('overflow', 'hidden');
                $(".blackOut-window").show();
            }
            window.history.pushState('addreviewPopup', '', '');
        },

        close: function (element) {

            if (isDesktop.length) {
                element.animate({ 'right': '-' + reviewPopup.containerRightPosition });
                popup.unlock();             
            }
            else {
                element.hide(effect, directionDown, duration, function () { });
                $('body').css('overflow', 'auto');
                $(".blackOut-window").hide();
            }
        }
    };

    $(".review-popup__link").click(function () {
        reviewPopup.open(ReviewPopup);
    });

    $('.review-popup .review-popup-close-btn, .blackOut-window').mouseup(function () {
        reviewPopup.close(ReviewPopup);
        window.history.back();
    });
    $(window).on('popstate', function (event) {
        if (ReviewPopup.is(':visible')) {
            reviewPopup.close(ReviewPopup);
        }
    });
    $(document).keyup(function (e) {
        if (e.keyCode == 27) {
            if (ReviewPopup.is(':visible')) {
                reviewPopup.close(ReviewPopup);
                window.history.back();
            }
        }
    });
});
