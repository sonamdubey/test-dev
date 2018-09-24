var AnimateCTA = (function () {
    var container, animationClass, $window, campaignContainer, campaignContainerHeight;
    function _setSelector() {
        animationClass = 'animated';
        container = $('.cta-animation-container');
        campaignContainer = $('.campaign-with-animation');
        $window = $(window);
        campaignContainerHeight = campaignContainer.outerHeight();
    }

    function registerEvents() {
        _setSelector();
        if (campaignContainer.length) {
            var animationClass = 'animated';
            $window.on('scroll', function () {
                var windowScrollTop = $window.scrollTop() + $window.innerHeight();
                var containerScrollTop = container.offset().top + container.height() + campaignContainerHeight;
                if ($window.scrollTop() === 0 || windowScrollTop < containerScrollTop) {
                    campaignContainer.removeClass(animationClass);
                    if (typeof(fullShown) != "undefined" && !fullShown)
                    { 
                        triggerNonInteractiveGA("Model_Page", "FloatingLeadCTA_FullWidth_Shown", "");
                        fullShown = true;
                    }  
                }
                else if (!campaignContainer.hasClass(animationClass)) {
                    campaignContainer.addClass(animationClass);
                    if (typeof(partialShown) != "undefined" && !partialShown)
                    {
                        triggerNonInteractiveGA("Model_Page", "FloatingLeadCTA_Partial_GetBestOffers_Shown", "");
                        partialShown = true;
                    }
                }
            });
        }

        $(".campaign__floating-btn").on("click", "a", function () {

            if ($(this).hasClass("js-mfg")) {
                if ($(this).closest(".js-floating-btn").hasClass("animated")) {
                    triggerGA($(this).data("cat"), "FloatingLeadCTA_Partial_GetBestOffers_Click", "");
                }
                else if ($(this).closest(".js-floating-btn").hasClass("campaign-with-animation")) {
                    triggerGA($(this).data("cat"), "FloatingLeadCTA_FullWidth_Click", "");
                }
                else if ($(this).data("group") == "default" && $(this).data("cat") == "Model_Page") {
                    triggerGA($(this).data("cat"), "FloatingLeadCTA_Partial_DefaultCTA_Click", "");
                }
            }

        });

    }
    return {
        registerEvents: registerEvents
    }
})();
