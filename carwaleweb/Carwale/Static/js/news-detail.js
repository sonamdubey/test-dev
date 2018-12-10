Common.showCityPopup = false;

var processedArticles = [];
var photosLoaded = [];
var getPromise;
var getPhotosPromise;
var viewModel;

var ViewModel = function () {
    var self = this;

    self.articles = ko.observableArray([]);
    self.defaultArticle = {
        articleUrl: window.location.pathname,
        title: document.title,
        basicId: articleBasicid,
    }
    self.loadgplus = function (element, data) {
        gapi.plusone.render('gplus-' + data.basicId, { 'href': 'https://www.carwale.com' + data.articleUrl, 'callback': 'googlePlusCallBack' });
        FB.XFBML.parse(document.getElementById("article-comments-" + data.basicId));
        $('img.lazy').lazyload();
    };
}

var ArticleDetailVM = function (data) {
    var self = this;

    self.pageList = data.pageList;
    self.tagsList = data.tagsList.join(',');
    self.vehicleTagsList = data.vehicleTagsList;
    self.imagePath = data.imagePath;
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
    self.facebookCommentCount = data.facebookCommentCount;
    self.title = data.title;
    self.articleUrl = data.articleUrl;
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
            setUrlAndTitle(articleDetail.title, articleDetail.articleUrl);
            if (!articleDetail.pageViewFired) {
                Common.utils.trackAction("CWNonInteractive", "contentcons", "newsviewed", articleDetail.articleUrl);
                articleDetail.pageViewFired = true;
            }
        }
        else {
            setUrlAndTitle(viewModel.defaultArticle.title, viewModel.defaultArticle.articleUrl);
        }
    }
    else if (visibleLastPageEnd.length > 0) {
        var index = visibleLastPageEnd[0].getAttribute('idx');
        var articleDetail = (index >= 0) ? viewModel.articles()[index] : viewModel.defaultArticle;
        setUrlAndTitle(articleDetail.title, articleDetail.articleUrl);
    }
}

function loadNextArticle() {    
    var params = {
        basicid: relatedBasicIds.shift()
    }
    getPromise = $.get("/api/v1/article/content/", params);
    getHandler();
    if (processedArticles.length > 1)
    {
        googletag.pubads().refresh([AdSlots['div-gpt-ad-1514458400839-0']]);
        Common.utils.trackAction('CWNonInteractive', 'NetworkAdTest', 'News details page_TW_d', 'Ad loaded_' + (processedArticles.length));
    }
}

function getHandler() {
    getPromise
       .done(function (data) {
           viewModel.articles.push(new ArticleDetailVM(data));
       });
}

function setUrlAndTitle(title, url) {
    if (window.location.pathname != url) {
        var obj = { Page: title, Url: url };
        history.replaceState(obj, obj.Page, obj.Url);
        document.title = title;
    }
}


function commentsToggleHandler() {
    var basicId = $(this).attr('id').split('-')[2];
    var index = $(this).attr('idx');
    $(this).find('span').toggleClass('hide');
    $(this).next().toggleClass('hide');
}

function commentsHashLinkHandler() {
    var basicId = $(this).attr('href').split('-')[2];
    var index = $(this).attr('idx');
    var commentBox = $('#comments-toggle-' + basicId);
    var toggleText = commentBox.find('span');
    $(toggleText[0]).addClass('hide');
    $(toggleText[1]).removeClass('hide');
    commentBox.next().removeClass('hide');
}

function initialize() {
    viewModel = new ViewModel();
    ko.applyBindings(viewModel, document.getElementById("related-articles"));
    window.addEventListener('scroll', scrollHandler, false);
    $(document).on('click', "[id^=comments-toggle-]", commentsToggleHandler);
    $(document).on('click', "a[href^='#article-comments-']", commentsHashLinkHandler);
}

var googlePlusCallBack = function () {
    Common.utils.trackAction('CWInteractive', 'share', 'GooglePlus', window.location.href);
}

initialize();
