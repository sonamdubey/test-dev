﻿var AnimateCTA = (function () {
    var container, animationClass, $window, campaignContainer, campaignContainerHeight, sellingPitchText, isTopSellingPitch;
    function _setSelector() {
        animationClass = 'animated';
        container = $('.cta-animation-container');
        campaignContainer = $('.campaign-with-animation');
        $window = $(window);
        campaignContainerHeight = campaignContainer.outerHeight();
        sellingPitchText = campaignContainer.find('#topSellingBlock').text().trim();
        isTopSellingPitch = sellingPitchText != undefined && sellingPitchText != "";
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
                        if (gaObj.id == gaEnum.Model_Page)
                        {
                            triggerNonInteractiveGA("Model_Page", "FloatingLeadCTA_FullWidth_Shown", "");
                        }
                        cwTracking.trackCustomData(bhriguPageName, "ES_FloatingLeadCTA_FullWidthShown", "versionId=" + versionId + (isTopSellingPitch ? "|LeadSupportingText=" + sellingPitchText : ""));
                        fullShown = true;
                    }  
                }
                else if (!campaignContainer.hasClass(animationClass)) {
                    campaignContainer.addClass(animationClass);
                    if (typeof(partialShown) != "undefined" && !partialShown)
                    {
                        if (gaObj.id == gaEnum.Model_Page)
                        {
                            triggerNonInteractiveGA("Model_Page", "FloatingLeadCTA_Partial_GetBestOffers_Shown", "");
                        }
                        cwTracking.trackCustomData(bhriguPageName, "ES_FloatingLeadCTA_AnimatedShown", "versionId=" + versionId + (isTopSellingPitch ? "|LeadSupportingText=" + sellingPitchText : ""));
                        partialShown = true;
                    }
                }
            });
        }

        $("a.js-mfg").click(function(){           
                if ($(this).closest(".js-floating-btn").hasClass("animated")) {
                    triggerGA(pageName, "FloatingLeadCTA_Partial_GetBestOffers_Click", "");
                    cwTracking.trackCustomData(bhriguPageName, "ES_FloatingLeadCTA_AnimatedClick", "versionId=" + versionId + (isTopSellingPitch ? "|LeadSupportingText=" + sellingPitchText : ""));
                }
                else if ($(this).closest(".js-floating-btn").hasClass("campaign-with-animation")) {
                    triggerGA(pageName, "FloatingLeadCTA_FullWidth_Click", "");
                    cwTracking.trackCustomData(bhriguPageName, "ES_FloatingLeadCTA_FullWidthClick", "versionId=" + versionId + (isTopSellingPitch ? "|LeadSupportingText=" + sellingPitchText : ""));
                }
        });

    }
    return {
        registerEvents: registerEvents
    }
})();
