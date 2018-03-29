ContentTracking = {
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

        findNearestPercentage: function(percentage) {
            for(var i = 1; i < ContentTracking.trackList.length; i++) {
                if(percentage < ContentTracking.trackList[i]) {
                    return ContentTracking.trackList[i - 1];
                }
            }
            return 100;
        },

        contentFlow: function (percentage, basicId, eventLabel, eventCategory) {
            var nearestPercentage = ContentTracking.page.findNearestPercentage(percentage);
            switch (nearestPercentage) {
                case 100:
                    if (!ContentTracking.trackingStatus[basicId][100]) {
                        ContentTracking.trackingStatus[basicId][100] = true;
                        triggerGA(eventCategory, "100_Viewed", eventLabel);
                    }
                case 75:
                    if (!ContentTracking.trackingStatus[basicId][75]) {
                        ContentTracking.trackingStatus[basicId][75] = true;
                        triggerGA(eventCategory, "75_Viewed", eventLabel);
                    }
                case 50:
                    if (!ContentTracking.trackingStatus[basicId][50]) {
                        ContentTracking.trackingStatus[basicId][50] = true;
                        triggerGA(eventCategory, "50_Viewed", eventLabel);
                    }
                case 25:
                    if (!ContentTracking.trackingStatus[basicId][25]) {
                        ContentTracking.trackingStatus[basicId][25] = true;
                        triggerGA(eventCategory, "25_Viewed", eventLabel);
                    }
                case 10:
                    if (!ContentTracking.trackingStatus[basicId][10]) {
                        ContentTracking.trackingStatus[basicId][10] = true;
                        triggerGA(eventCategory, "10_Viewed", eventLabel);
                    }
                case 0:
                    if (!ContentTracking.trackingStatus[basicId][0]) {
                        ContentTracking.trackingStatus[basicId][0] = true;
                        triggerGA(eventCategory, "Page_Load", eventLabel);
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
                var trackObj = new ContentTracking.tracking.start();
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
            if (ContentTracking.trackingStatus[Id] == undefined) {
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
                if (localStorage.BasicId != undefined && localStorage.BasicId.split('|').indexOf(basicId) == -1)
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