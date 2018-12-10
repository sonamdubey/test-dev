ContentTracking = {
    contentElement : undefined,
    isDetails : false,
    category : undefined,
    isScroll : false,
    trackingStatus: {},
    pageviewStatus: {},
    trackList : [10, 25, 50, 75, 100],
    articleUrl : undefined,

    page : {
    
        getPageType: function () {
            if (ContentTracking.isDetails) { return "details"; }
            else { return "listing"; }
        },

        getRelatedTag: function () {
            return $(".related-details::in-viewport").length > 0 ? "-related" : "";
        },

        getBasicInfo: function (element, len) {

            var categoryName = $(element[len]).attr("categoryname");
            var categoryId = $(element[len]).attr("categoryid");
            var Id = $(element[len]).attr("id");

            return { id: Id, categoryId: categoryId, categoryName: categoryName };
        },

        visibleContent: function (contentTopPosition,contentHeight) {
            var visibleHeight = ($(window).height() - contentTopPosition);
            return visibleHeight > contentHeight ? contentHeight : visibleHeight;
        },

        preProcess: function (basicId, trackcount) {
            for (var count = trackcount; count >= 0 ; count--)
            {
                ContentTracking.trackingStatus[basicId][ContentTracking.trackList[count]] = true;
            }
        },

        contentFlow: function (percentage, basicId, categoryName,url) {
            var date = new Date();
            if (percentage >= 100) {
                if (!ContentTracking.trackingStatus[basicId][100]) {                    
                    ContentTracking.trackingStatus[basicId][100] = true;
                    Common.utils.trackAction("CWInteractive", "contentcons", categoryName + "-" + ContentTracking.page.getPageType() + ContentTracking.page.getRelatedTag() + "-100", date.toLocaleString());
                    if (!ContentTracking.isScroll) {
                        ContentTracking.page.preProcess(basicId,3);
                    }
                }
            }
            else if (percentage >= 75) {
                if (!ContentTracking.trackingStatus[basicId][75]) {
                    ContentTracking.trackingStatus[basicId][75] = true;
                    Common.utils.trackAction("CWInteractive", "contentcons", categoryName + "-" + ContentTracking.page.getPageType() + ContentTracking.page.getRelatedTag() + "-75", date.toLocaleString());
                    if (!ContentTracking.isScroll) {
                        ContentTracking.page.preProcess(basicId,2);
                    }
                }
            }
            else if (percentage >= 50) {
                if (!ContentTracking.trackingStatus[basicId][50]) {
                    ContentTracking.trackingStatus[basicId][50] = true;
                    Common.utils.trackAction("CWInteractive", "contentcons", categoryName + "-" + ContentTracking.page.getPageType() + ContentTracking.page.getRelatedTag() + "-50", date.toLocaleString());
                    if (!ContentTracking.isScroll) {
                        ContentTracking.page.preProcess(basicId,1);
                    }
                }
            }
            else if (percentage >= 25) {
                if (!ContentTracking.trackingStatus[basicId][25]) {
                    ContentTracking.trackingStatus[basicId][25] = true;
                    Common.utils.trackAction("CWInteractive", "contentcons", categoryName + "-" + ContentTracking.page.getPageType() + ContentTracking.page.getRelatedTag() + "-25", date.toLocaleString());
                    if (!ContentTracking.isScroll) {
                        ContentTracking.page.preProcess(basicId,0);
                    }
                }
            }
            else if (percentage >= 10) {
                if (!ContentTracking.trackingStatus[basicId][10]) {
                    ContentTracking.trackingStatus[basicId][10] = true;
                    Common.utils.trackAction("CWInteractive", "contentcons", categoryName + "-" + ContentTracking.page.getPageType() + ContentTracking.page.getRelatedTag() + "-10", date.toLocaleString());
                }
            }
        },

        percetageStatus: function () {
            var perstatus = undefined;
            if (ContentTracking.isDetails) {
                perstatus= {
                    10: false, 25: false, 50: false, 75: false, 100: false
                };
            }
            else {
                perstatus = {
                    10: false, 50: false, 100: false
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
                ContentTracking.tracking.clickTrackingShareBtns();
            });
        }
    },

    tracking: {

        setUpTracking: function (isDetails,category, contentElement) {
            ContentTracking.contentElement = contentElement;
            ContentTracking.isDetails = isDetails;
            ContentTracking.category = category;            
        },

        createObject: function (Id) {
            if (ContentTracking.trackingStatus[Id] == undefined) {
                ContentTracking.trackingStatus[Id] = ContentTracking.page.percetageStatus();
            }
        },

        firePageView: function (basicId) {
            var urlsplit = window.location.href.split("-");
            var curBasicId = urlsplit[urlsplit.length - 1].split("/")[0];

            if (!ContentTracking.pageviewStatus[basicId] && basicId == curBasicId) {
                ContentTracking.pageviewStatus[basicId] = true;
                if (ContentTracking.page.getRelatedTag() != "") {
                    Common.utils.firePageView(window.location.pathname);
                }
            }
        },
        start: function ()
        {
            var articleObj = $(ContentTracking.contentElement + ':in-viewport');
            if (articleObj.length > 0)
            {
                var objLen = articleObj.length - 1;
                var content = ContentTracking.page.getBasicInfo(articleObj, objLen);
                ContentTracking.tracking.firePageView(content.id);
                ContentTracking.tracking.createObject(content.id + '_' + content.categoryId);
                if (!ContentTracking.trackingStatus[content.id + '_' + content.categoryId][100])
                {
                    var contentTopPosition = $(articleObj[objLen]).offset().top - $(window).scrollTop() - 60;
                    var contentHeight = $(articleObj[objLen]).height();
                    var visibleHeight = ContentTracking.page.visibleContent(contentTopPosition, contentHeight);
                    ContentTracking.page.contentFlow(((visibleHeight * 100) / contentHeight), content.id + "_" + content.categoryId, content.categoryName,window.location.href);
                }                
            }
        },
        setDefaultIdInLocalStorage:function(basicId)
        {
            if (window.localStorage && basicId!=undefined) {
                if (localStorage.BasicId != undefined && localStorage.BasicId.split('|').indexOf(basicId)==-1)
                    localStorage.BasicId+="|" + basicId;
                else localStorage.BasicId= basicId;
            }
        },

        clickTrackingShareBtns: function () {
            window.twttr = (function (d, s, id) {
                var js, fjs = d.getElementsByTagName(s)[0],
                  t = window.twttr || {};
                if (d.getElementById(id)) return t;
                js = d.createElement(s);
                js.id = id;
                js.src = "https://platform.twitter.com/widgets.js";
                fjs.parentNode.insertBefore(js, fjs);

                t._e = [];
                t.ready = function (f) {
                    t._e.push(f);
                };

                return t;
            }(document, "script", "twitter-wjs"));

            twttr.ready(
              function (twttr) {
                  // bind events here
                  window.twttr.events.bind('click', function () {
                      Common.utils.trackAction('CWInteractive', 'share', 'Twitter', window.location.href);
                  });
              }
            );
        },

        adRequest: function (index,length,actionName)
        {
            Common.utils.trackAction("CWInteractive", "NetworkAdTest", actionName, "Ad loaded_" + length + "_" + index);
        },
},
};
$(document).ready(function (e) {
    ContentTracking.page.registerEvent();
});
