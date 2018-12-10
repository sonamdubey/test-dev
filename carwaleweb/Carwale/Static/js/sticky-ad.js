var stickyAd = (function () {
    var element, container, containerTop,  $window, overallContainer, elementTop, windowScrollTop,maxWindowScrollPoint;

    function _setSelectores() {
        element = $('#floating-ad');
        container = element.closest('.ad-container');
        headerHeight = $('#header').height();
        $window = $(window);
        overallContainer = $('.overall-container');
        maxWindowScrollPoint = 0;
    }

    function registerEvents() {
        _setSelectores();
        $window.scroll(function () {
            toggleStickyAd();
            var scrollTop = $(window).scrollTop();
            if (maxWindowScrollPoint < scrollTop)
            {
                maxWindowScrollPoint = scrollTop;
            }
        });
    }

    function toggleStickyAd() {
        overallContainerHeight = overallContainer.height() + overallContainer.offset().top;
        containerTop = container.offset().top;
        windowScrollTop = $window.scrollTop();
        if ((windowScrollTop + headerHeight) < containerTop) {
            element.removeClass('ad-container--fixed');
        }
        else if (((windowScrollTop + headerHeight) > containerTop) && (overallContainerHeight > (windowScrollTop + headerHeight + element.outerHeight()))) {
            element.addClass('ad-container--fixed');
            element.css('top', '50px');
        }
        else if (overallContainerHeight > (windowScrollTop + headerHeight)) {
            elementTop = element.css('top');
            var overallContainerBottom = (overallContainerHeight - (windowScrollTop + element.height()));
            var elementBottom = 20 - overallContainerBottom;         // here 20 is bottom margin
            element.css('top', -elementBottom + 'px');
        }
    }
    return {
        registerEvents: registerEvents,
        toggleStickyAd: toggleStickyAd
    }
})();

$(document).ready(function () {
    stickyAd.registerEvents();

    setTimeout(function () {
        stickyAd.toggleStickyAd();
    }, 300);

});