var ContentTracking = {
    contentElement: undefined,
    isDetails: false,
    category: undefined,
    isScroll: false,
    trackingStatus: {},
    pageviewStatus: {},
    trackList: [0, 10, 25, 50, 75, 100],
    articleUrl: undefined,

    page: {

        getPageType: function () {
            if (ContentTracking.isDetails) { return "details"; }
            else { return "listing"; }
        },

        getRelatedTag: function () {
            return $(".related-details").length > 0 ? "-related" : "";
        },

        getBasicInfo: function (element, len) {

            var categoryId = $(element[len]).data("category-id");
            var Id = $(element[len]).data("id");
            var eventLabel = $(element[len]).data("event-label") + '_' + Id;
            var eventCategory = $('body').data("page-name");
            return { id: Id, categoryId: categoryId, eventLabel: eventLabel, eventCategory: eventCategory };
        },

        visibleContent: function (contentTopPosition, contentHeight) {
            var visibleHeight = ($(window).height() - contentTopPosition);
            return visibleHeight > contentHeight ? contentHeight : visibleHeight;
        },

        contentFlow: function (percentage, basicId, eventLabel, eventCategory) {
            var j = 0, checkpoint;
            for (var i = ContentTracking.trackList.length - 1; i >= 0 ; i--) {
                checkpoint = ContentTracking.trackList[i];
                if (checkpoint <= percentage && !ContentTracking.trackingStatus[basicId][checkpoint]) {
                    j = i;
                    break;
                }
            }
            for (var i = j; i >= 0; i--) {
                checkpoint = ContentTracking.trackList[i];
                if (!ContentTracking.trackingStatus[basicId][checkpoint]) {
                    ContentTracking.trackingStatus[basicId][checkpoint] = true;
                    var eventAction = checkpoint === 0 ? "Page_Load" : checkpoint.toString() + "_Viewed";
                    triggerGA(eventCategory, eventAction, eventLabel);
                }
            }       
        },

        percetageStatus: function () {
            var perstatus = undefined;
            if (ContentTracking.isDetails) {
                perstatus = {
                    0: false, 10: false, 25: false, 50: false, 75: false, 100: false
                };
            }
            return perstatus;
        },

        registerEvent: function () {

            $(window).scroll(function () {
                ContentTracking.tracking.start();
            });

            $(window).load(function () {
                ContentTracking.tracking.start();
                ContentTracking.tracking.setDefaultIdInLocalStorage($('.content-details').attr('id'));
            });
        }
    },

    tracking: {

        setUpTracking: function (isDetails, contentElement) {

            ContentTracking.contentElement = contentElement;
            ContentTracking.isDetails = isDetails;
        },

        createObject: function (Id) {
            if (ContentTracking.trackingStatus[Id] === undefined) {
                ContentTracking.trackingStatus[Id] = ContentTracking.page.percetageStatus();
            }
        },


        start: function () {
            var articleObj = $(ContentTracking.contentElement);
            if (articleObj.length > 0) {
                var objLen = articleObj.length - 1;
                var content = ContentTracking.page.getBasicInfo(articleObj, objLen);
                ContentTracking.tracking.createObject(content.id + '_' + content.categoryId);
                if (!ContentTracking.trackingStatus[content.id + '_' + content.categoryId][100]) {
                    var contentTopPosition = $(articleObj[objLen]).offset().top - $(window).scrollTop() - 60;
                    var contentHeight = $(articleObj[objLen]).height();
                    var visibleHeight = ContentTracking.page.visibleContent(contentTopPosition, contentHeight);
                    var eventLabel = content.eventLabel;
                    var eventCategory = content.eventCategory;
                    ContentTracking.page.contentFlow(((visibleHeight * 100) / contentHeight), content.id + "_" + content.categoryId, eventLabel, eventCategory);
                }
            }
        },
        setDefaultIdInLocalStorage: function (basicId) {
            if (window.localStorage && basicId != undefined) {
                if (localStorage.BasicId != undefined && localStorage.BasicId.split('|').indexOf(basicId) === -1)
                    localStorage.BasicId += "|" + basicId;
                else localStorage.BasicId = basicId;
            }
        },

    },
};
docReady(function () {

    ContentTracking.tracking.setUpTracking(1, '.content-details');
    ContentTracking.page.registerEvent(1, '.content-details');

});