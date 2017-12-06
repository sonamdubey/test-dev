// review popup
var reviewPopupCotent = $('#reviewPopup');
var desktopUserReview = $('#userReviewContentDesktop').length;

docReady(function () {
	reviewPopup = {
		settings: {
			effect: 'slide',
			direction: {
				direction: 'down'
			},
			duration: 500
		},
        open: function (element) {
        	if (desktopUserReview) {
        		element.animate({ 'right': 0 });
        		popup.lock();
            }
            else {
        		element.show(reviewPopup.settings.effect, reviewPopup.settings.direction, reviewPopup.settings.duration, function () { });
        		$('body').css('overflow', 'hidden');
        		$(".blackOut-window").show();
        	}
            window.history.pushState('addreviewPopup', '', '');
        },

        close: function (element) {
        	if (desktopUserReview) {
        		element.animate({ 'right': '-' + ($('#reviewPopup').width() + $('.review-popup-close-btn').outerWidth()) });
        		popup.unlock();
            }
            else {
        		element.hide(reviewPopup.settings.effect, reviewPopup.settings.direction, reviewPopup.settings.duration, function () { });
        		$('body').css('overflow', 'auto');
        		$(".blackOut-window").hide();
        	}
        }
    };

    $(".review-popup__link").on('click', function () {
        reviewPopup.open(reviewPopupCotent);
    });

    $('.review-popup .review-popup-close-btn, .blackOut-window').on('click', function () {
        reviewPopup.close(reviewPopupCotent);
        window.history.back();
    });

    $(window).on('popstate', function (event) {
        if (reviewPopupCotent.is(':visible')) {
            reviewPopup.close(reviewPopupCotent);
        }
    });

    $(document).keyup(function (e) {
        if (e.keyCode == 27) {
            if (reviewPopupCotent.is(':visible')) {
                reviewPopup.close(reviewPopupCotent);
                window.history.back();
            }
        }
    });
});
