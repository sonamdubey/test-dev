var scrollTitleDict = [];
var articlesParams = [];
var titleIndex = 1;
var pageViewArray = [];
var apiString = '/api/content/';
var prevIndex = 0;
var isNews = 0;
var isVideo = 0;
var views = 0;
var likes = 0;

var initialize = function () {

    scrollTitleDict.push({
        key: 0,
        value: 0
    });

    articlesParams.push(new Object({
        Title: document.title,
        Url: window.location.pathname,
    }));

//add default article(root article) at 0th position of related article
    relatedArticlesParams.unshift(
     new Object({
        categoryId: defaultCatId,
        categoryMaskingName: defaultCatMaskingName,
        basicId: defaultBasicId
    })
);
    
    pageViewArray.push(true);
    for (var i = 0; i < relatedArticlesParams.length; ++i) {
        pageViewArray.push(false);
    }
};

var contentDetails = {
    viewModel: function () {
        this.detail = ko.observableArray([]),
        this.getResponse = function (data) {
            if (data.news != undefined && data.news != null && data.news.content != null) {
                isNews = 1;
                isVideo = 0;                
                data.news.content.replace(/<img/g, "<img class='margin-top10 imgWidth margin-bottom10'");
                this.detail.push(data.news);
            }
            else if (data.expertreview != undefined && data.expertreview != null) {
                isNews = 0;
                isVideo = 0;               
                this.detail.push(data.expertreview);
            }

            else if (data.video != undefined && data.video != null)
            {
                isNews = 0;
                isVideo = 1;                
                this.detail.push(data.video);
                setTimeout(onYouTubeIframeReady(data.video.videoId), 1000);                
            }
        };
    }
};
var triggerPageView = function (index) {
    var userCookie = $.cookie('CWC');
    if ((isNews && $('div[divbasicId=' + relatedArticlesParams[index].basicId + '] p:first:in-viewport').length > 0) || $('div[divbasicId=' + relatedArticlesParams[index].basicId + '] p:first p+p:first:in-viewport').length > 0 || $('div[divbasicId=' + relatedArticlesParams[index].basicId + '] p:first div+p:first:in-viewport').length > 0) {
            pageViewArray[index] = true;         
            var pageName = isNews ? "newsviewed":"expertreviewviewed";
            if (index == 1)
                Common.utils.trackAction("CWNonInteractive", "contentcons", userCookie + "-" + pageName + "-start-" + defaultBasicId + "-" + relatedArticlesParams[index].basicId, window.location.pathname);
                else
                Common.utils.trackAction("CWNonInteractive", "contentcons", userCookie + "-" + pageName + "-page-" + defaultBasicId + "-" + relatedArticlesParams[index].basicId, window.location.pathname);
    }
    else if (isVideo && $('div[divbasicId=' + relatedArticlesParams[index].basicId + '] img:first:in-viewport').length > 0) {
        pageViewArray[index] = true;
        Common.utils.firePageView(window.location.pathname);
        if (relatedArticlesParams[1].basicId == relatedArticlesParams[index].basicId)
            Common.utils.trackAction("CWNonInteractive", "contentcons", userCookie + "-videoviewed-start-" + defaultBasicId + "-" + relatedArticlesParams[index].basicId, window.location.pathname);
        else
            Common.utils.trackAction("CWNonInteractive", "contentcons", userCookie + "-videoviewed-page" + defaultBasicId + "-" + relatedArticlesParams[index].basicId, window.location.pathname);
    }
}
var checkInViewPort = function () {
    //Added a patch for stopping request for videos when gallery is present on the page
    if ($('.social:in-viewport:last a:first').parent().next().attr('id') == 'relatedGallery' && $('#galleryEnd:in-viewport').length == 0)
        return false;
    else if ($('.socialvideo:in-viewport').attr('basicId')) {
        try {
            var winlocarr = articlesParams[titleIndex - 1].Url.split('-');
            var basicId = winlocarr[winlocarr.length - 1].split('/')[0];
            return $('.socialvideo:in-viewport:last').attr('basicId') == basicId.toString();
        }
        catch (error) {
                console.log(error.message);
            }
    }
    else {
        var articleInView = $('.social:in-viewport:last a:first').length > 0 ? 1 : 0;
        try {
            return ((articleInView) ? $('.social:in-viewport:last a:first').attr('href').split('carwale.com/')[1].split('?')[0] == articlesParams[titleIndex - 1].Url.split('/m/')[1] : false);
        }
        catch (error) {
            console.log(error.message);
        }
    }
    return false;
}
var formatDate = function (inputDate) {
    var seconds = Math.floor((new Date() - new Date(inputDate)) / 1000);

    var interval = Math.floor(seconds / 31536000);
    if (interval >= 1) {
        if (interval == 1)
            return interval + " year ago";
        return interval + " years ago";
    }
    interval = Math.floor(seconds / 2592000);
    if (interval >= 1) {
        if (interval == 1)
            return interval + " month ago";
        return interval + " months ago";
    }
    interval = Math.floor(seconds / 86400);
    if (interval >= 1) {
        if (interval == 1)
            return interval + " day ago";
        return interval + " days ago";
    }
    interval = Math.floor(seconds / 3600);
    if (interval >= 1) {
        if (interval == 1)
            return interval + " hour ago";
        return interval + " hours ago";
    }
    interval = Math.floor(seconds / 60);
    if (interval >= 1) {
        if (interval == 1)
            return interval + " minute ago";
        return interval + " minutes ago";
    }
    if (seconds == 1)
        return Math.floor(seconds) + " second ago";
    return Math.floor(seconds) + " seconds ago";
}
$(document).ready(function () {
    currentViewModel = new contentDetails.viewModel();
    initialize();
    ko.applyBindings(currentViewModel, document.getElementById("infinite-scroll"));
    $(window).scroll(function () {
        var scrollTop = $(this).scrollTop();
        if (titleIndex > prevIndex && titleIndex < relatedArticlesParams.length && !(pageViewArray[titleIndex]) && checkInViewPort()) {
            $('.news-loader').show();
            $.get(apiString + relatedArticlesParams[titleIndex].categoryMaskingName + "/" + relatedArticlesParams[titleIndex].basicId+'/')
              .success(function (data) {
                  ++titleIndex;
                  $('.news-loader').hide();
                  currentViewModel.getResponse(data);
                  scrollTitleDict.push({
                      key: scrollTop,
                      value: titleIndex - 1
                  });
                  if (data.news != undefined && data.news != null) {
                      isVideo = 0;
                      articlesParams.push(new Object({
                          Title: data.news.title,
                          Url: '/m' + data.news.articleUrl
                      }));
                      window.history.replaceState(scrollTop, data.news.title, '/m' + data.news.articleUrl);

                      contentDetailsAds.Dfp.getDfpAd(data.news.basicId, true);
                  }
                  else if (data.expertreview != undefined && data.expertreview != null)
                  {
                      isVideo = 0;
                      articlesParams.push(new Object({
                          Title: data.expertreview.title,
                          Url: '/m' + data.expertreview.articleUrl
                      }));
                      window.history.replaceState(scrollTop, data.expertreview.title, '/m' + data.expertreview.articleUrl);
                      
                      contentDetailsAds.Dfp.getDfpAd(data.expertreview.basicId);
                  }

                  else if (data.video != undefined && data.video != null) {
                      isVideo = 1;
                      var urlNew = '/m/' + data.video.makeName.toLowerCase().split(" ").join("") + "-cars/" + data.video.maskingName.toLowerCase() + '/videos/' + $.trim(data.video.subCatName).toLowerCase().split(' ').join('-') + "-" + data.video.basicId + "/";
                      articlesParams.push(new Object({
                          Title: data.video.videoTitle,                        
                          Url : urlNew
                      }));
                      window.history.replaceState(scrollTop, data.video.videoTitle, urlNew);
                      retrieveParameters(data.video.videoId, data.video.basicId);
                      triggerPageView(titleIndex - 1);
                  }
                  $('img.lazy').lazyload(); 
              })
              .error(function (data) {
                  $(".swiper-imgLoader news-loader").css("display", "none");
                  console.log("error in getting API response");
              });
            prevIndex = titleIndex;           
        }
        if (scrollTitleDict.length > 1) {
            Common.utils.setUrlTitleOnScroll(scrollTop, scrollTitleDict, articlesParams);
            if (!(pageViewArray[titleIndex-1])) triggerPageView(titleIndex - 1);

        }
    });
    
    if (typeof(relatedArticlesParams) != 'undefined' && relatedArticlesParams.length > 0 && defaultCatId == 1 && typeof (adblockDetecter) != 'undefined' && adblockDetecter)
    {
        contentDetailsAds.Dfp.setTurboWidgetDfpAttribute(relatedArticlesParams[0].basicId);
        contentDetailsAds.Dfp.callDfp(); 
    }
});


function retrieveParameters(videoId,basicId) {
        $.ajax({
            type: "GET",
            url: "https://www.googleapis.com/youtube/v3/videos?part=statistics&id=" + videoId + "&key=AIzaSyCQRyGIUebquGytlnfHQL9CT47T_Fh4WNA",
            async: false,
            dataType: 'json',
            success: function (response) {
                if (typeof (response.items) == "object" && response.items.length > 0) {
                    views = response.items[0].statistics.viewCount;
                    likes = response.items[0].statistics.likeCount;
                    if(views > 0 && likes > 0)
                    sendParameters(basicId);
                }
               
            }
        });

    }
function sendParameters(basicId) {
    var result = "";
    $.ajax({
        type: "POST",
        url: "/ajaxpro/Carwale.UI.Editorial.VideoDetails,Carwale.ashx",
        async: false,
        data: '{"basicId":"' + basicId + '","views":"' + views + '","likes":"' + likes + '"}',
        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "UpdateParameters"); },
        success: function (response) {
            responseObj = eval('(' + response + ')');
            result = responseObj.value;
        }
    });
}

var contentDetailsAds = {
    Dfp: {
        setLeaderBoardDfpAttribute: function (basicId) {
            try {
                ContentTracking.tracking.adRequest(titleIndex - 1, relatedArticlesParams.length, 'News details page_LB_m');

                $('#dfp_' + basicId).html("<div class=\"text-center padding-top10\"><div class=\"adunit sponsored\" data-adunit=\"Carwale_News_Mobile_320x50\" data-dimensions=\"320x50,320x100\"></div></div>");
            }
            catch (err) {
                console.log('dfp:' + err.message);
            }
        },
        getDfpAd: function (basicId, isNewsCategory) {
            try {

                if (typeof (adblockDetecter) != 'undefined' && adblockDetecter)
                {
                    contentDetailsAds.Dfp.setLeaderBoardDfpAttribute(basicId);

                    if (isNewsCategory != undefined && isNewsCategory)
                    {
                        contentDetailsAds.Dfp.setTurboWidgetDfpAttribute(basicId);
                    }
                    contentDetailsAds.Dfp.callDfp();
                }
            }
            catch (err) {
                console.log('dfp:' + err.message);
            }
        },
        setTurboWidgetDfpAttribute: function (basicId) {
            try {
                ContentTracking.tracking.adRequest(titleIndex - 1, relatedArticlesParams.length, 'News details page_TW_m');
                
                $('#dfp-tw-' + basicId).html("<div class=\"text-center news-ad-slot margin-bottom10\"><div class=\"adunit sponsored\" data-adunit=\"Carwale_News_Mobile_300x250\" data-dimensions=\"300x250\"></div></div>");
            }
            catch (err) {
                console.log('dfp:' + err.message);
            }
        },
        callDfp: function()
        {
            $.dfp({
                dfpID: '1017752',
                enableSingleRequest: false,
                collapseEmptyDivs: true,
                refreshExisting: false,
                afterAllAdsLoaded: function (adUnits) {
                    var parentAdUnit = $(adUnits).parent();
                    if ($(adUnits).hasClass('display-block')) { // ad present
                        parentAdUnit.removeClass("hide");
                    } else {
                        parentAdUnit.addClass("hide"); // no ad present
                    }
                }
            });
        }
    }
}

