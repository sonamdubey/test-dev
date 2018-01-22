docReady(function() {
	$('.model-card__pros-cons').on('click', '.pros-cons__more-btn', function(event) {
		$(this).hide();
		$(this).closest('.pros-cons__content').find('li').show();
	});

    //interesting fact popup
	interestingFactPopup.registerEvents();
});
var interestingFactPopup = (function () {
    var fixedPoupContainer, readMoreBtn, closeBtn;

    function _setSelectores() {
        fixedPoupContainer = $('#interestingFact');
        readMoreBtn = $('.interesting-fact__read-more'),
        closeBtn = $('.interesting-fact-popup .close');
    }

    function registerEvents() {
        _setSelectores();
        readMoreBtn.on('click', function () {
            var interestingFactContainer = $(this).closest('.interesting-fact-section'),
                interestingFactContent = interestingFactContainer.find('.interesting-fact__content').text();
            open(interestingFactContent);
            history.pushState('interestingFactPopup', '', '');
        });

        closeBtn.on('click', function () {
            if (fixedPoupContainer.is(':visible')) {
                window.history.back();
            }
        });
        $('.interesting-fact__whiteout-window').on('click', function () {
            if (fixedPoupContainer.is(':visible')) {
                window.history.back();
            }
        });
        $(".interesting-fact-popup .interesting-fact__content").scroll(function () {
            var interestingFactContainer = $(this),
                containerPosition = interestingFactContainer.scrollTop();
            if (containerPosition <= 0 && containerPosition > interestingFactContainer.outerHeight()) {
                interestingFactContainer.attr('data-overlay', 'none');
            }
            else if (containerPosition <= 0) {
                interestingFactContainer.attr('data-overlay', 'bottom');
            }
            else if (containerPosition > interestingFactContainer.outerHeight()) {
                interestingFactContainer.attr('data-overlay', 'top');
            }
            else {
                interestingFactContainer.attr('data-overlay', 'both');
            }
        });
    }

    function open(interestingFactContent) {
        bodyBackground.lock();
        fixedPoupContainer.addClass('interesting-popup--active');
        fixedPoupContainer.find('.interesting-fact__content').text(interestingFactContent);
    }

    function close() {
        bodyBackground.unlock();
        fixedPoupContainer.removeClass('interesting-popup--active');
    }

    $(window).on('popstate', function () {
        if (fixedPoupContainer.is(':visible')) {
            close();
        }
    });

    $(document).keydown(function (e) {
        if (e.keyCode == 27) {
            if (fixedPoupContainer.is(':visible')) {
                window.history.back();
            }
        }
    });
    return {
        registerEvents: registerEvents,
        open: open
    }
})();

var bodyBackground = {
    lock: function () {
        var htmlElement = $('html'), bodyElement = $('body');
        if ($(document).height() > $(window).height()) {
            var windowScrollTop = htmlElement.scrollTop() ? htmlElement.scrollTop() : bodyElement.scrollTop();
            if (windowScrollTop < 0) {
                windowScrollTop = 0;
            }
            htmlElement.addClass('lock-browser-scroll').css('top', -windowScrollTop);
        }
    },

    unlock: function () {
        var htmlElement = $('html'),
            windowScrollTop = parseInt(htmlElement.css('top'));
        htmlElement.removeClass('lock-browser-scroll');
        $('html, body').scrollTop(-windowScrollTop);
    }
};