Common.showCityPopup = false;

$(document).ready(function () {

    $("#more-tab").click(function () {
        if ($("#more-nav").hasClass('hide')) {
            $("#more-nav").slideDown().removeClass("hide");
            $("html, body").animate({ scrollTop: $("#more-nav").offset().top }, 1000);
            $("#more-tab").find("#f-nav-icon").removeClass("more-arrow").addClass("more-arrow-down");
        }
        else {
            $("#more-nav").slideUp().addClass("hide");
            $("html, body").animate({ scrollTop: $("#more-tab").offset().top }, 500);
            $("#more-tab").find("#f-nav-icon").removeClass("more-arrow-down").addClass("more-arrow");
        }
    });

    $('#dropdown-nav .dropdown-menu').dropdown_menu({
        sub_indicators: true,
        drop_shadows: true,
        close_delay: 300
    });
});

function initQuickPQSubNavigation(modelId) {
    var versionId = "";
    var pageId = "29";
    var pqCookieCityId = $.cookie('_CustCityId');
    if (Number(pqCookieCityId) > 0) {
        $.ajax({
            type: 'GET',
            url: '/webapi/GeoCity/GetPQCitiesByModelId/?modelid=' + modelId,
            dataType: 'Json',
            success: function (json) {
                var arrLen = json.length;
                if (arrLen > 0) {
                    for (var i = 0; i < arrLen; i++) {
                        if (json[i].CityId == pqCookieCityId) {
                            redirectQuotation(versionId, modelId, pageId);
                            return;
                        }
                    }
                    openPqPopUp(versionId, modelId, pageId);
                } else {
                    openPqPopUp(versionId, modelId, pageId);
                }
            }
        });
    } else {
        openPqPopUp(versionId, modelId, pageId);
    }
}

function redirectQuotation(versionId, modelId, pageId) {
    $.cookie('_PQModelId', modelId, { path: '/' });
    $.cookie('_PQVersionId', versionId, { path: '/' });
    $.cookie('_PQPageId', pageId, { path: '/' });
    window.location.href = '/new/quotation.aspx';
}

function openPqPopUp(versionId, modelId, pageId) {
    var caption = "";
    var url = "/new/quickpqwidget.aspx?model=" + modelId + "&version=" + versionId + "&pageid=" + pageId;
    var applyIframe = false;
    var GB_Html = "";
    GB_show(caption, url, 230, 500, applyIframe, GB_Html);
}

var processedArticles = [];
var photosLoaded = [];
var getPromise;
var getPhotosPromise;
var viewModel;

var ViewModel = function () {
    var self = this;
    self.index = -1;
    self.colorboxIndex = 0;
    self.articles = ko.observableArray([]);
    self.defaultArticle = {
        articleUrl: window.location.pathname,
        title: document.title,
        basicId: articleBasicId,
        images: ko.observableArray([]),
        galleryHandler: function (elements, data) {
            if (self.defaultArticle.images()[self.defaultArticle.images().length - 1].ImageId === data.ImageId) {
                galleryHandler(self.defaultArticle.basicId);
            }
        }
    }
    self.loadgplus = function (element, data) {
        gapi.plusone.render('gplus-' + data.basicId, { 'href': 'https://www.carwale.com' + data.articleUrl, 'callback': 'googlePlusCallBack' });
        $('img.lazy').lazyload();
    };
}

var ArticleDetailVM = function (data) {
    var self = this;

    self.pageList = data.pageList;
    self.tagsList = data.tagsList[0] + " Cars";
    self.vehicleTagsList = data.vehicleTagsList;
    self.mainImgCaption = data.mainImgCaption;
    self.isMainImgSet = data.isMainImgSet;
    self.showGallery = data.showGallery;
    self.categoryId = data.categoryId;
    self.hostUrl = data.hostUrl;
    self.authorName = data.authorName;
    self.displayDate = Editorial.utils.getDisplayDate(data.publishedDate);
    self.views = data.views;
    self.authorMaskingName = data.authorMaskingName;
    self.basicId = data.basicId;
    self.title = data.title;
    self.articleUrl = data.articleUrl;
    self.images = ko.observableArray([]);
    self.galleryHandler = function (elements, data) {
        if (self.images()[self.images().length - 1].ImageId === data.ImageId) {
            galleryHandler(self.basicId);
        }
    }
}

function getFormattedDate(date) {
    var date = new Date(date);
    return date.toDateString().split(' ').splice(1, 3).join(' ');
}

function galleryHandler(basicId) {
        $("#article-photos-" + basicId + " a[rel='slidePhoto-" + basicId + "']").colorbox({
        onComplete: function (a, b) {
            var colorboxindex = $.colorbox.element().index();
            if (!thumbnailclick) {
                if (viewModel.colorboxIndex < colorboxindex) { Common.utils.trackAction("CWInteractive", "desktop_expert_reviews", "image_next", "image_next"); }
                else if (viewModel.colorboxIndex > colorboxindex) { Common.utils.trackAction("CWInteractive", "desktop_expert_reviews", "image_prev", "image_prev"); }
            } else {
                thumbnailclick = false;
                Common.utils.trackAction("CWInteractive", "desktop_expert_reviews", "image_thumbnail", "image_thumbnail");
            }
            viewModel.colorboxIndex = colorboxindex;
            var imageDetails = null;
            var articleDetail = ko.dataFor($(this).parent()[0]);  
            imageDetails = articleDetail.images()[colorboxindex];
            if (imageDetails && imageDetails.MakeBase) {
                Common.utils.firePageView("/" + Common.utils.formatSpecial(imageDetails.MakeBase.makeName) + "-cars/" + imageDetails.ModelBase.MaskingName + "/images/" + Editorial.utils.getImageUrl(imageDetails.ImageName, imageDetails.ImageId));
            }
        },
        onClosed: function () { Common.utils.firePageView(window.location.pathname); },
        onOpen: function () { thumbnailclick = true; }
    });
    $("#article-photos-" + basicId + " img.lazy").lazyload();
}

function scrollHandler() {
    var visibleNextPageTrigger = $(".next-page-trigger:in-viewport");
    var visibleLastPageEnd = $(".last-page-end:in-viewport");
    var visibleFirstPage = $(".first-page-start:in-viewport");

    if (visibleNextPageTrigger.length > 0 ) {
        var visibleId = visibleNextPageTrigger[0].getAttribute('basic');
        if (processedArticles.indexOf(visibleId) < 0) {
            processedArticles.push(visibleId);
            loadNextArticle();
        }
    }

    if ((visibleLastPageEnd.length > 0 && visibleFirstPage.length > 0) || visibleFirstPage.length > 0) {
        var index = visibleFirstPage[0].getAttribute('idx');
        if (index >= 0) {
            var articleDetail = viewModel.articles()[index];
            viewModel.index = Number(index);
            setUrlAndTitle(articleDetail.title, articleDetail.articleUrl);
            if (!articleDetail.pageViewFired) {
                Common.utils.trackAction("CWNonInteractive", "contentcons", "expertreviewviewed", articleDetail.articleUrl);
                articleDetail.pageViewFired = true;
            }
        }
        else {
            viewModel.index = -1;
            setUrlAndTitle(viewModel.defaultArticle.title, viewModel.defaultArticle.articleUrl);
        }
    }
    else if (visibleLastPageEnd.length > 0) {
        var index = visibleLastPageEnd[0].getAttribute('idx');
        var articleDetail = (index >= 0) ? viewModel.articles()[index] : viewModel.defaultArticle;
        viewModel.index = -1;
        setUrlAndTitle(articleDetail.title, articleDetail.articleUrl);
    }
}

function loadNextArticle() {
    var params = {
        basicid: relatedBasicIds.shift()
    }
    getPromise = $.get("/api/v1/article/content/", params);
    getHandler();
    googletag.pubads().refresh([AdSlots['div-gpt-ad-1514458400839-0']]);
    Common.utils.trackAction('CWNonInteractive', 'NetworkAdTest', 'News details page_TW_d', 'Ad loaded_' + (processedArticles.length + 1));
}

function getHandler() {
    getPromise
       .done(function (data) {
           viewModel.articles.push(new ArticleDetailVM(data));
           photosToggleHandler(viewModel.articles().length - 1);
       });
}

function setUrlAndTitle(title, url) {
    if (window.location.pathname != url) {
        var obj = { Page: title, Url: url };
        history.replaceState(obj, obj.Page, obj.Url);
        document.title = title;
    }
}

function photosToggleHandler(index) {
    var basicId = $("div.photos-slug[idx=" + index + "]").closest("[basic]").attr("basic");
    togglePhotos(basicId, index);
}

function photosHashLinkHandler() {
    var basicId = $(this).attr('href').split('-')[1];
    var index = $(this).attr('idx');
    var photosPage = $('#photos-toggle-' + basicId);
    togglePhotos(basicId, index);
    photosPage.next().removeClass('hide');
}

function togglePhotos(basicId, index) {
    var articleDetail = (index >= 0) ?  viewModel.articles()[index] :  viewModel.defaultArticle;
    if (articleDetail.images().length === 0) {
        getArticlePhotos(basicId, index);
    }
}

function getArticlePhotos(basicId, index) {
    var params = {
        basicid: basicId
    }
    getPhotosPromise = $.get("/api/image/GetArticlePhotos/", params);
    getPhotosHandler(basicId, index);
}

function getPhotosHandler(basicId, index) {
    getPhotosPromise
       .done(function (data) {
           (index >= 0) ? viewModel.articles()[index].images(data) : viewModel.defaultArticle.images(data);
           $("#article-photos-" + basicId).find(".fa-spinner").hide();
       });
}
function initialize() {
    viewModel = new ViewModel();
    ko.applyBindings(viewModel, document.getElementById("related-articles"));
    ko.applyBindings(viewModel.defaultArticle, document.getElementById('article-photos-' + viewModel.defaultArticle.basicId));
    window.addEventListener('scroll', scrollHandler, false);
    $(document).on('click', "a[href^='#photos-']", photosHashLinkHandler);
    photosToggleHandler(-1);
    videoTracking.setStateChangeCallBack();
}

var googlePlusCallBack = function () {
    Common.utils.trackAction('CWInteractive', 'share', 'GooglePlus', window.location.href);
}

var videoTracking = {
    setStateChangeCallBack: function () {
            try {
                player.addEventListener('onStateChange', videoTracking.fireVideoPageView);
            }
            catch (e) {
                setTimeout(function () { videoTracking.setStateChangeCallBack(); }, 300);
            }
        },
fireVideoPageView: function (event) {
            switch(event.data)
            {
                case YT.PlayerState.PLAYING: 
                    Common.utils.firePageView(event.target.a.getAttribute("data-video-url"));
                    Common.utils.trackAction('CWInteractive', 'desktop_expert_review_detail', 'video', event.target.a.getAttribute("data-car-name"));
                    break;
                case YT.PlayerState.ENDED:
                case YT.PlayerState.PAUSED:
                    Common.utils.firePageView(window.location.pathname);
                    break;
            }
        }
}
initialize();