var AnimateRibbon = (function () {
    var screenScroll, offersRibbon;
    function _setSelectors() {
        screenScroll = $(document).scrollTop();
        offersRibbon = $(".offers-ribbon");
    }

    function animateRibbon() {

        $(document).on("scroll", function () {
            if ($(".offers-ribbon").length > 0) {
                _setSelectors();

                var header = $(".header-fixed-white");
                var ribbonPosition = parseInt($(".offers-ribbon").css("top"));
                var scrollTop = (screenScroll + ribbonPosition);
                var containerTop = $(".hide-ribbon").offset().top;
                var ribbonHeight = offersRibbon.outerHeight();
                var offersContainerbottom = containerTop + $(".hide-ribbon").outerHeight();
                if (scrollTop + ribbonHeight >= containerTop && scrollTop <= offersContainerbottom) {
                    offersRibbon.hide();
                    //$(".offer-container").addClass("animate");
                }
                else {
                    offersRibbon.fadeIn(50);
                    //$(".offer-container").removeClass("animate");
                }
            }
        });
    }


    function translateRibbon() {

        $(document).on("scroll", function () {
            _setSelectors();
            var fixedValue = 85;
            var prefixes = ["-moz-", "-webkit-", "-o-", "-ms-"];
            if (screenScroll >= 0 && screenScroll < fixedValue) {
                for (var i = 0; i < prefixes.length; i++) {
                    var prefix = prefixes[i] + "transform";
                    offersRibbon.css(prefix, "translateX(" + (screenScroll) + "px)");
                }
                $(".offers-ribbon__content").css("opacity", "1");
            }
            else {
                for (var i = 0; i < prefixes.length; i++) {
                    var prefix = prefixes[i] + "transform";
                    offersRibbon.css(prefix, "translateX(" + fixedValue + "px)");
                }

                $(".offers-ribbon__content").css("opacity", "0");
            }
            for (var i = 0; i < prefixes.length; i++) {
                var prefix = prefixes[i] + "transform";
                $(".ribbon-icon-wrapper").css(prefix, "rotate(" + screenScroll + "deg)");
            }
        });
    }

    var scrollToOffers = function() {
        var offersContainerTop = $(".offer-container").offset().top;
        $("body,html").animate({
            scrollTop: offersContainerTop - 70
        }, 1000);
    }

    function scrollOffers() {
        $(document).off("click", ".offers-ribbon");
        $(document).on("click", ".offers-ribbon", function () {
            scrollToOffers();
        });
    }

    function registerEvents() {
        _setSelectors();
        translateRibbon();
        animateRibbon();
        scrollOffers();

    }
    return {
        registerEvents: registerEvents,
        scrollToOffers : scrollToOffers
    }
})();

var AnimateIcons = (function () {
    
    function registerEvents() {
        $(document).on("scroll", function () {
            for (var i = 0; i < $(".hide-ribbon").length; i++) {
                var screenScroll = $(document).scrollTop();
                var currentContainer = $(".hide-ribbon").eq(i)
                var containerTop = currentContainer.offset().top;
                var offersContainerbottom = containerTop + currentContainer.outerHeight();
                if (screenScroll >= containerTop - 195 && screenScroll <= offersContainerbottom) {
                    $(".offer-container").eq(i).addClass("animate");
                }
                else {
                    $(".offer-container").eq(i).removeClass("animate");
                }
            }
        });
    }
    return { registerEvents: registerEvents }

})();

var PositionRibbon = (function () {
    var header;
    function _setSelectors() {
        header = $(".header-fixed-white");
    }

    function registerEvents() {
        _setSelectors();
        var ribbonPos = ($(".offers-ribbon").attr("data-position")) + "px";
        $(".offers-ribbon").css("top", ribbonPos);
    }
    return { registerEvents: registerEvents }

})();

var ShowToolTip = (function () {
    
    function openTooltip() {
        $(document).off("click", ".js-offer__tnc");
        $(document).on("click", ".js-offer__tnc", function (event) {
            $(this).closest(".offer-container").find($(".offers-tool-tip")).toggleClass("hide");
            event.stopPropagation();
        });
    }

    function closeTooltip() {
        $(document).on("click", function () {
            if (!($(this).closest(".offer-container").find($(".offers-tool-tip")).hasClass("hide"))) {
                $(".offers-tool-tip").addClass("hide");
            }
        });
    }

    function registerEvents() {
        openTooltip();
        closeTooltip();
    }
    return { registerEvents: registerEvents }

})();


var OpenOffers = (function () {

    function openOemOffers() {
        $(document).off("click", ".js-open-offer");
        $(document).on("click", ".js-open-offer", function () {
            var divClosestOffer = $(this).closest(".offer-container");
            divClosestOffer.find($(".js-open-offer").closest(".offers__list")).addClass('open-offers');
            $(this).closest('ul').children().find(".offers-list__content").removeClass('truncate-offer')
            divClosestOffer.find($(".js-open-offer")).hide();
        });
    }
    function registerEvents() {
        openOemOffers();
    }
    return { registerEvents: registerEvents }
})();

$(document).ready(function () {
    OpenOffers.registerEvents();
    ShowToolTip.registerEvents();
    AnimateRibbon.registerEvents();
    PositionRibbon.registerEvents();
    AnimateIcons.registerEvents();
});