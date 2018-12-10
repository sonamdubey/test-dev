var adTracking = {
    lbContainerTop: 0,
    midContainerTop: 0,
    rosContainerTop: 0,
    maxWindowScrollPoint: 0,
    callSlugHeight: 0,
    trackLeaderBoard: true,
    trackMiddleAd: true,
    trackRos: true,
    windowHeight: $(window).height(),


    setSelectors: function(){
        adTracking.lbContainerTop = $('#div-gpt-ad-1419227721763-0').closest('.ad-container').offset().top;
        adTracking.midContainerTop = $('#midAdContainer').offset().top;
        
        adTracking.callSlugHeight = $('.slug-class').height();

        if (adTracking.trackLeaderBoard && adTracking.isElementInViewPort(adTracking.lbContainerTop, 0, '#div-gpt-ad-1419227721763-0')) {
            adTracking.trackLeaderBoard = false;
            cwTracking.trackCustomData("ModelPageMsite", "LeaderBoardAd", "NA", false);
        }
    },
    registerEvents: function () {
        adTracking.setSelectors();
        
        $(window).scroll(function () {           
            var scrollTop = $(window).scrollTop();
            if (adTracking.maxWindowScrollPoint < scrollTop) {
                adTracking.maxWindowScrollPoint = scrollTop;
                adTracking.bhriguTracking(scrollTop);
            }
        });
    },

    bhriguTracking: function (windowScrollTop) {
        if (adTracking.trackLeaderBoard && adTracking.isElementInViewPort(adTracking.lbContainerTop, windowScrollTop, '#div-gpt-ad-1419227721763-0')) {
            adTracking.trackLeaderBoard = false;
            cwTracking.trackCustomData("ModelPageMsite", "LeaderBoardAd", "NA", false);
        }

        adTracking.midContainerTop = $('#midAdContainer').offset().top;
        if (adTracking.trackMiddleAd && adTracking.isElementInViewPort(adTracking.midContainerTop, windowScrollTop, '#div-gpt-ad-1419228003697-0'))
        {
            adTracking.trackMiddleAd = false;
            cwTracking.trackCustomData("ModelPageMsite", "ModelMidAd", "NA", false);
        } 

        adTracking.rosContainerTop = $('#divBottomAdBar').offset().top;
        if (adTracking.trackRos && adTracking.isElementInViewPort(adTracking.rosContainerTop, windowScrollTop, '#div-gpt-ad-1435297201507-0')) {
            adTracking.trackRos = false;
            cwTracking.trackCustomData("ModelPageMsite", "ROSAd", "NA", false);
        }
    },

    isElementInViewPort: function (containerTop, windowScrollTop, adId) {
        return ((containerTop - windowScrollTop + parseInt($(adId).css('marginTop'))) < (adTracking.windowHeight - adTracking.callSlugHeight));
    }
}

$(document).ready(function () {
    adTracking.registerEvents();
});