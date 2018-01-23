docReady(function() {
	$('.model-card__pros-cons').on('click', '.pros-cons__more-btn', function(event) {
		$(this).hide();
		$(this).closest('.pros-cons__content').find('li').show();
	});

    //interesting fact popup
	interestingFactPopup.registerEvents();

    //floating navbar
	floatingNav.registerEvents();
});
var floatingNav = (function () {
    var overallTabsContainer, overallContainer;
    function _setSelectores() {
        overallTabsContainer = $('.overall-tabs__content');
        overallContainer = $('#overallContainer');
    }
    function registerEvents() {
        _setSelectores();
        $(window).scroll(function () {
                var windowScrollTop = $(window).scrollTop(),
                    specsTabsOffsetTop = $('.overall-tabs__placeholder').offset().top,
                    overallContainerHeight = overallContainer.outerHeight(),
                    topNavBarHeight = overallTabsContainer.height();

                var currentActiveTab;

                if (windowScrollTop > specsTabsOffsetTop) {
                    overallTabsContainer.addClass('fixed-tab-nav');
                }

                else if (windowScrollTop < specsTabsOffsetTop) {
                    overallTabsContainer.removeClass('fixed-tab-nav');
                }

                if (overallTabsContainer.hasClass('fixed-tab-nav')) {
                    if (windowScrollTop > Math.ceil(overallContainerHeight) - (topNavBarHeight)) {
                        overallTabsContainer.removeClass('fixed-tab-nav');
                    }
                }
		
                $('#overallContainer .overall-tabs-data').each(function () {
                    var top = $(this).offset().top - topNavBarHeight,
                        bottom = top + $(this).outerHeight();
                    if (windowScrollTop >= top && windowScrollTop <= bottom) {
                        overallTabsContainer.find('li').removeClass('tab--active');
                        $('#overallContainer .overall-tabs-data').removeClass('tab--active');

                        $(this).addClass('tab--active');

                        currentActiveTab = overallTabsContainer.find('li[data-tabs="' + $(this).attr('data-id') + '"]');
                        overallTabsContainer.find(currentActiveTab).addClass('tab--active');
                        //centerItVariableWidth(overallTabsContainer.find('li[data-tabs="' + $(this).attr('data-id') + '"]'), '.overall-tabs__content');
                        

                    }
                });

            var tabElementThird = overallContainer.find('.overall-tabs-data:eq(3)'),
                tabElementSixth = overallContainer.find('.overall-tabs-data:eq(6)'),
                tabElementNinth = overallContainer.find('.overall-tabs-data:eq(9)'),
                tabElementTwelve = overallContainer.find('.overall-tabs-data:eq(13)');

                if (tabElementThird.length != 0) {
                    focusFloatingTab(tabElementThird, 300, 0);
                }

                if (tabElementSixth.length != 0) {
                    focusFloatingTab(tabElementSixth, 600, 300);
                }

                if (tabElementNinth.length != 0) {
                    focusFloatingTab(tabElementNinth, 900, 600);
                }

                if (tabElementTwelve.length != 0) {
                    focusFloatingTab(tabElementTwelve, 1200, 900);
                }

        });

        $('.overall-tabs__list li').on('click', function () {
            var target = $(this).attr('data-tabs'),
                topNavBarHeight = $('.overall-tabs__content').height();
            $('html, body').animate({ scrollTop: Math.ceil($(".overall-tabs-data[data-id=" + target+"]").offset().top) - topNavBarHeight }, 1000);
            centerItVariableWidth($(this), '.overall-tabs__content');
        });

    }
    function focusFloatingTab(element, startPosition, endPosition) {
        var windowScrollTop = $(window).scrollTop();
        if (windowScrollTop > element.offset().top - 45) {
            if (!overallTabsContainer.hasClass('scrolled-left-' + startPosition)) {
                overallTabsContainer.addClass('scrolled-left-' + startPosition);
                scrollHorizontal(startPosition);
            }
        }

        else if (windowScrollTop < element.offset().top) {
            if (overallTabsContainer.hasClass('scrolled-left-' + startPosition)) {
                overallTabsContainer.removeClass('scrolled-left-' + startPosition);
                scrollHorizontal(endPosition);
            }
        }
    };
    function scrollHorizontal(pos) {
        $('.overall-tabs__content').animate({ scrollLeft: pos - 15 + 'px' }, 500);
    }
    return {
        registerEvents: registerEvents
    }
})();
var interestingFactPopup = (function () {
    var fixedPoupContainer, container, readMoreBtn, closeBtn;

    function _setSelectores() {
        fixedPoupContainer = $('#interestingFact');
        closeBtn = $('.interesting-fact-popup .close');
        container = $('.interesting-popup--active');
    }

    function registerEvents() {
        _setSelectores();
        $('.interesting-fact__read-more').on('click', function () {
            var interestingFactContainer = $(this).closest('.interesting-fact-section'),
                interestingFactContent = interestingFactContainer.find('.interesting-fact__content').text();
            open(interestingFactContent);
            history.pushState('interestingFactPopup', '', '');
            _setSelectores();
        });

        closeBtn.on('click', function () {
            if (container.is(':visible')) {
                window.history.back();
            }
        });
        $('.interesting-fact__whiteout-window').on('click', function () {
            if (container.is(':visible')) {
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
        if (container.is(':visible')) {
            close();
        }
    });

    $(document).keydown(function (e) {
        if (e.keyCode == 27) {
            if (container.is(':visible')) {
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
 