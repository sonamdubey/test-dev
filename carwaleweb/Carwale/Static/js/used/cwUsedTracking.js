var cwUsedTracking = (function () {
    var _eventCategory;

    var eventCategory = {
        UsedDetailsPage: "UsedDetailsPage",
        UsedSearchPage: "UsedSearchPage",
        UsedPhotoGallery: "UsedPhotoGallery",
    };
    var eventActions = {
        gsdUnverifiedText: "Get_Seller_Details_Click_Unverified",
        gsdVerifiedText: "Get_Seller_Details_Click_Verified",
    };

    var setEventCategory = function (category) {
        if (typeof category !== 'undefined' && eventCategory.hasOwnProperty(category)) {
            _eventCategory = category;
        }
    }

    //field require for 'inputParams' is 
    //action:- action of tracking
    //label:- label for tracking,
    //qs:- Querystring for tracking(in case you want to track some differnt qs rather then qs present in url), 
    //sendQs:- boolean, whether to track Qs or not(for value true:- it will track qs present in url
    //                                                       false:- it will not track qs)
    var track = function (inputParams) {
        if (typeof cwTracking !== "undefined") {
            if (inputParams.qs) {
                cwTracking.trackCustomDataWithQs(_eventCategory, inputParams.action, inputParams.label, inputParams.qs);
            }
            else {
                cwTracking.trackCustomData(_eventCategory, inputParams.action, inputParams.label, inputParams.sendQs);
            }
        }
    };

    return {
        eventCategory: eventCategory,
        eventActions: eventActions,
        track: track,
        setEventCategory: setEventCategory,
    };
})();