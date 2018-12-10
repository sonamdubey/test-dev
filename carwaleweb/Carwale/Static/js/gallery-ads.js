//const $ = require('jquery');

var GalleryAds = {
    swipeCounter: 0,
    abTestVal: Number($.cookie("_abtest")),
    isAdblockDetecter: typeof (adblockDetecter) != 'undefined' && adblockDetecter,

    responsive: {
        refreshAd: function (carousal) {
            if (GalleryAds.responsive.isRefreshRequired(carousal)) {
                googletag.pubads().refresh([GalleryAds.responsive.getAdSlot()]);
            }
        },
        
        isRefreshRequired: function (carousal) {
            if (!GalleryAds.isAdblockDetecter)
                return false;

            if (carousal == "carmodel-image-swipper") {
                if (GalleryAds.abTestVal > 0 && GalleryAds.abTestVal < 51) {
                    return GalleryAds.swipeCounter % 2 == 0;
                }
                else{
                    return GalleryAds.swipeCounter % 3 == 0;
                }           
            }
            else if (carousal == "tab" || carousal == "image-gallery__colourselection")
                return true;            
            return false;
        },

        getAdSlot: function () {
            if (GalleryAds.abTestVal > 0 && GalleryAds.abTestVal < 51) {
                return AdSlots['div-gpt-ad-1516616442657-0'];
            }
            else{
                return AdSlots['div-gpt-ad-1516616442657-1'];
            }
        },        

        callDfp: function () {
            try {
                $.dfp({
                    dfpID: '1017752',
                    enableSingleRequest: false,
                    collapseEmptyDivs: true,
                    refreshExisting: true
                });
            }
            catch (err) {
                console.log("dfp:", err.message)
            }
        }       
    },

    desktopBtfLeaderBoard: {
        insertAd: function () {
                GalleryAds.swipeCounter = 0;
                $('#leaderBoard').html("<div class=\"adunit margin-bottom15 margin-top15\" data-adunit=\"Carwale_Image_Details_BTF_970X90\" data-dimensions=\"970x90,970x60,728x90,960x66,970x66,960x60\"></div>");
                GalleryAds.responsive.callDfp();
        },

        refreshAd: function () {
            if (GalleryAds.desktopBtfLeaderBoard.isRefreshRequired()) {
                GalleryAds.responsive.callDfp();
            }
        },

        isRefreshRequired: function () {
            if (!GalleryAds.isAdblockDetecter)
                return false;

            return GalleryAds.swipeCounter % 3 == 0;
        }
    }
}
//module.exports = GalleryAds;