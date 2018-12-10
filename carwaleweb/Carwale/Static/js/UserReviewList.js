var ViewModel = function () {
    var self = this;
    self.makeName = makeName;
    self.maskingName = maskingName;
    self.modelId = modelId;
    self.shortUrl = shortUrl;
    self.versionId = versionId;
    self.versionUrl = (versionId > 0 ? '-' + versionId : '/');
    self.reviewCount = reviewCount;
    self.currentPage = ko.observable(currentPage);
    self.nextPage = currentPage;
    self.pageNumber = ko.observable(pageNumber);
    self.pagination = ko.computed(function () {
        var pageList = [];
        var pageNum = self.pageNumber();
        if (pageNum > 1)
        {
            var start = 0, end = pageNum;
            var page = self.currentPage();
            if (page == pageNum)
            {
                end = pageNum;
                if (pageNum > 2) start = page - 2;
                else start = page - 1; 
            }
            else if (page == 1) 
            {
                start = 1;
                if (pageNum > 2) end = page + 2;
                else end = page + 1;
            }
            else if ((page + 1) == pageNum)
            {
                start = page - 1;
                end = page + 1;
            }
            else { start = page; end = page +2; }
            pageList.push(page-1);
            for (var pageno = start; pageno <= end; pageno++) pageList.push(pageno);
            pageList.push(-1);
        }
        return pageList;
    }, self);
    self.userReviewList = ko.observableArray([]);
    self.Models = ko.observableArray([]);
}
var userReviewListing = {
    prevSelectedVersion: "",
    prevSelectedSort: "",
    action: "",
    label: "",
    actualReviewCount: reviewCount,
    trackFilter: function (currEle) {
        var id = currEle.attr('id');
        if (id === 'drpReleventSort') {
            var newSort = $('#drpReleventSort :selected').text();
            userReviewListing.action = 'SortOrderChange';
            userReviewListing.label = 'prevval=' + userReviewListing.prevSelectedSort + '|newval=' + newSort + '|modelid=' + modelId + '|reviewcount=' + userReviewListing.actualReviewCount;
            userReviewListing.prevSelectedSort = newSort;
        }
        else {
            var newVersion = $('#drpReviewedVersions :selected').text();
            userReviewListing.action = 'VersionChange';
            userReviewListing.label = 'prevval=' + userReviewListing.prevSelectedVersion + '|newval=' + newVersion + '|modelid=' + modelId + '|reviewcount=' + userReviewListing.actualReviewCount;
            userReviewListing.prevSelectedVersion = newVersion;
        }
        cwTracking.trackCustomData('ReviewsListingsPage', userReviewListing.action, userReviewListing.label, false);
    }
}
var getPromise;
var processedPages = [];
var pageViewPages = [];
var starRatings = ["empty", "one", "two", "three", "four", "five"];

function loadNextArticle(params) {
    getPromise = $.get("/api/UserReviews/", params);
    getHandler(false);
}

function getHandler(replace, currEle) {
    $('.content-loader').show();
    getPromise
       .done(function (data) {
           $('.content-loader').hide();
           if (replace) {
               viewModel.pageNumber(Number(Math.ceil(data.reviewCount / 10)));
               $("#userReviewListing").replaceWith("");
               $("#reviewCountId").text(data.reviewCount);
               viewModel.userReviewList(data.reviews);
               userReviewListing.actualReviewCount = data.reviewCount;
               userReviewListing.trackFilter(currEle);
           }
           else
               viewModel.userReviewList.push.apply(viewModel.userReviewList, data.reviews);           
       });
}

function initialize() {
    viewModel = new ViewModel();
    ko.applyBindings(viewModel, document.getElementById("userreview-next-list"));
    ko.applyBindings(viewModel, document.getElementById("paginationReviews"));
    if (!(isMobile))
    {
        ko.applyBindings(viewModel.Models, document.getElementById("drpRevModel"));
        viewModel.Models([{ "ModelId": -1, "ModelName": "--Select Model--", "MaskingName": "" }]);
    }
    window.addEventListener('scroll', scrollHandler, false);
    userReviewListing.prevSelectedSort = $('#drpReleventSort').find(":selected").text();
    userReviewListing.prevSelectedVersion = $('#drpReviewedVersions').find(":selected").text();
}

function setUrlAndTitle(page)
{
    var pageurl = (page == 1 && intialPage == 0) ? '' : '-p' + page;
    var url = viewModel.shortUrl + pageurl + viewModel.versionUrl;
    if (window.location.pathname != url) 
    {        
        viewModel.currentPage(Number(page));
        var obj = { Page: document.title, Url: url };
        history.replaceState(obj, obj.Page, obj.Url);
        if (pageViewPages.indexOf(page) < 0) {
            Common.utils.trackAction("CWNonInteractive", "contentcons", "userReviewList", url);
            Common.utils.firePageView(url);
            pageViewPages.push(page);
        }
    }
}

function scrollHandler() {

    var visibleNextPageTrigger = $(".next-page-trigger:in-viewport");
    var visibleFirstPage = $(".first-page-start:in-viewport");

    if (visibleNextPageTrigger.length > 0) {
        var visiblePage = Number(visibleNextPageTrigger[0].getAttribute('pagetag'));
        if (visiblePage > 0 && processedPages.indexOf(visiblePage) < 0) {
            processedPages.push(visiblePage);
            viewModel.nextPage = Number(visiblePage);
            loadNextArticle(getParam());
        }
        if (visiblePage > 0)
        {
            setUrlAndTitle(visiblePage - 1);
        }                
    }
    else if (visibleFirstPage.length > 0)
    {
        var visiblePage = Number(visibleFirstPage[0].getAttribute('pagetag'));
        setUrlAndTitle(visiblePage);
    }
}

function getRateSpan(value) {
    var starClass,
        absVal = Math.floor(value),
        rating = starRatings[absVal];

    if (value > absVal && value > 1) {
        starClass = rating + "Half-rating";
    } else if (value > absVal && value < 1) {
        starClass = "half-rating";
    } else {
        starClass = rating + "-rating";
    }
    return "<span class='rating-sprite " + starClass + "'></span>";

}

function removeHtml(text)
{
    if (text != null && text != '') 
    {
        var reg = /<[^>]+>/g;
        text = text.replace(reg, '');
        text = text.replace('Read full review','');
    }
    return text;
}

function getParam()
{
    var sortCritiria = Number($("#drpReleventSort :selected").val());
    var versionId = Number($("#drpReviewedVersions :selected").val());
    var params = {
        modelId: viewModel.modelId,
        versionId: versionId > 0 ? versionId : -1,
        pageNo: viewModel.nextPage,
        pageSize: 10,
        sortCriteria: sortCritiria > 0 ? sortCritiria : 1
    }
    return params;
}

function viewReviews() {
    var makeObj = $("#drpRevMake :selected"), modelObj = $("#drpRevModel :selected");
    var errObj = $("#reviewErr");
    if (makeObj.val() < 1) {
        errObj.text("*Please select make for reviews");
        errObj.show();
        return;
    }
    if (modelObj.val() < 1) {
        errObj.text("*Please select model for reviews");
        errObj.show();
        return;
    }
    errObj.hide();
    location.href = "/research/" + Common.utils.formatSpecial(makeObj.text()) + "-cars/" + modelObj.attr('mask') + "/userreviews/";
}
var isValidProsCons = function (prosCons) {
	if (!prosCons || prosCons.length == 0) {
		return false;
	}
	else {
		prosCons = prosCons.toLowerCase().trim();
		return !(prosCons == "na" || prosCons == "n/a" || prosCons == "n\a");
	}
}
$(document).ready(function () {
    
    $(document).on("change", "#drpReleventSort, #drpReviewedVersions", function ()
    {
        viewModel.currentPage(1);
        viewModel.nextPage = 1;
        viewModel.versionId = -1;
        processedPages = [];
        getPromise = $.get("/api/UserReviews/", getParam());
        getHandler(true, $(this));
        setUrlAndTitle(viewModel.currentPage());        
    });

    $("#reviewOnRoad").click(function () {
        var modelId = $(this).attr("modelid");
        if (isMobile) redirectOrOpenPopup(this, '28');
        else openPqPopUp('',modelId, '28', this);
    });

    $("#drpRevMake").change(function () {
        var makeId = $(this).val();
        if (makeId > 0) $("#reviewErr").hide();
        bindModelsList("all", makeId, viewModel, document.getElementById("drpRevModel"), "--Select Model--");
    });
    initialize();
});