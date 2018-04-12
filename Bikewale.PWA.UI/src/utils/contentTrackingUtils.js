import {triggerNonInteractiveGA}  from './analyticsUtils'
import {  GA_PAGE_MAPPING } from './constants'

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

        getBasicInfo: function (element, len) {

            var currentEle = element[len];
            var categoryId = currentEle.getAttribute('data-category-id');
            var Id = currentEle.getAttribute('data-id');
            var eventLabel = currentEle.getAttribute('data-event-label') + '_' + Id;
            if(typeof(gaObj)!="undefined")
            {
                gaObj = GA_PAGE_MAPPING["DetailsPage"];
            }
            var eventCategory = gaObj.name;
            return { id: Id, categoryId: categoryId, eventLabel: eventLabel, eventCategory: eventCategory };
        },

        visibleContent: function (contentTopPosition, contentHeight) {
            var visibleHeight = (document.documentElement.clientHeight - contentTopPosition);
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
                    triggerNonInteractiveGA(eventCategory, eventAction, eventLabel);
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

           window.addEventListener('scroll', function () {
                ContentTracking.tracking.start();
            });

           window.addEventListener('load', function () {
                ContentTracking.tracking.start();
                ContentTracking.tracking.setDefaultIdInLocalStorage((document.getElementsByClassName('content-details')[0]).getAttribute('data-id'));
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
            var articleObj = document.getElementsByClassName(ContentTracking.contentElement)
            if (articleObj.length > 0) {
                var objLen = articleObj.length - 1;
                var content = ContentTracking.page.getBasicInfo(articleObj, objLen);
                ContentTracking.tracking.createObject(content.id + '_' + content.categoryId);
                if (!ContentTracking.trackingStatus[content.id + '_' + content.categoryId][100]) {
                    var indexArticleObj = articleObj[objLen];
                    var contentTopPosition = indexArticleObj.getBoundingClientRect().top - 60;
                    var contentHeight = indexArticleObj.clientHeight;
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



module.exports = {
    ContentTracking
    };