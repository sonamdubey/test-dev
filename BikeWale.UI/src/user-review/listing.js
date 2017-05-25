var vmUserReviews, modelReviewsSection;

var helpfulReviews = [];

var reviewCategory = {
    2: 'helpful',
    1: 'recent',
    5: 'positive',
    6: 'negative',
    7: 'neutral'
}

var $window, overallSpecsTabsContainer, overallSpecsTab, specsFooter, topNavBarHeight;

var listItemHeight = 230; // min item height + pagination height

docReady(function() {

    modelReviewsSection = $("#modelReviewsListing");

    var modelUserReviews = function () {
        var self = this;
        /*
        self.IsInitialized = ko.observable(false);
        self.PagesListHtml = ko.observable("");
        self.PrevPageHtml = ko.observable("");
        self.NextPageHtml = ko.observable("");
        */
        self.activeReviewList = ko.observableArray(helpfulReviews);
        self.activeReviewCategory = ko.observable(0);
        self.reviewsAvailable = ko.observable(true);
        self.categoryName = ko.observable('');
        /*
        self.IsLoading = ko.observable(false);
        self.Filters = ko.observable({ pn: 1, ps: 8, model: modelid, so: 2, skipreviewid: reviewId });
        self.QueryString = ko.computed(function () {
            var qs = "";
            $.each(self.Filters(), function (i, val) {
                if (val != null && val != "")
                    qs += "&" + i + "=" + val;
            });
            qs = qs.substr(1);
            return qs;
        });
        self.PageUrl = ko.observable();
        self.CurPageNo = ko.observable();
        self.PreviousQS = ko.observable("");
        self.TotalReviews = ko.observable();
        self.noReviews = ko.observable(self.TotalReviews() == 0);
        self.Pagination = ko.observable(new vmPagination());


        self.init = function (e) {
            if (!self.IsInitialized()) {

                var eleSection = $("#modelReviewsListing");
                ko.applyBindings(self, eleSection[0]);


                self.Filters()["cat"] = self.Filters()["cat"] || eleSection.data("cat") || "";
                self.Filters()["pn"] = self.Filters()["pn"] || eleSection.data("pn") || "";
                self.Filters()["ps"] = self.Filters()["ps"] || eleSection.data("ps") || "";
                self.Filters()["so"] = self.Filters()["so"] || eleSection.data("so") || "";

                var filterType = $(e.target).data("category");
                if (filterType && filterType != "0") {
                    self.toggleReviewList(e);
                }
                else if (e.target) {
                    self.ChangePageNumber(e);
                }
                else {
                    self.getUserReviews();
                }

                $(document).on("click", "#pagination-list-content ul li,.pagination-control-prev a,.pagination-control-next a", function (e) {
                    if (self.IsInitialized()) {
                        self.ChangePageNumber(e);
                    }
                });

                self.IsInitialized(true);
            }
        };
        */

        self.toggleReviewList = function (event) {
            self.tabEvents.toggleTab($(event.currentTarget));
            self.tabEvents.getReviews($(event.currentTarget));
        };

        self.tabEvents = {
            toggleTab: function (element) {
                if (!element.hasClass('active')) {
                    element.siblings().removeClass('active');
                    element.addClass('active');
                }
            },

            getReviews: function (element) {
                var categoryId = Number(element.attr('data-category')),
                    pageNumber = Number(element.attr('data-page-num') || 1),
                    categoryCount = Number(element.attr('data-count'));

                if (categoryCount) {
                    self.reviewsAvailable(true);
                    self.activeReviewCategory(categoryId);
                }
                else {
                    self.tabEvents.setNoReview(categoryId);
                }
            },

            setNoReview: function (categoryId) {
                self.reviewsAvailable(false);
                self.categoryName(reviewCategory[categoryId]);
            }

        };

    }

    vmUserReviews = new modelUserReviews();
    ko.applyBindings(vmUserReviews, modelReviewsSection[0]);

    $window = $(window);
    overallSpecsTabsContainer = $('#overallTabsWrapper');
    overallSpecsTab = $('#overallSpecsTab');
    specsFooter = $('#listingFooter');
    topNavBarHeight = overallSpecsTab.height();

    if (overallSpecsTabsContainer.length > 0) {
        $(window).scroll(function () {
            var windowScrollTop = $window.scrollTop(),
                specsTabsOffsetTop = overallSpecsTabsContainer.offset().top,
                specsFooterOffsetTop = specsFooter.offset().top;

            if (windowScrollTop > specsTabsOffsetTop) {
                overallSpecsTab.addClass('fixed-tab-nav');
            }

            else if (windowScrollTop < specsTabsOffsetTop) {
                overallSpecsTab.removeClass('fixed-tab-nav');
            }

            if (overallSpecsTab.hasClass('fixed-tab-nav')) {
                if (windowScrollTop + listItemHeight > specsFooterOffsetTop - topNavBarHeight) {
                    overallSpecsTab.removeClass('fixed-tab-nav');
                }
            }
        });
    }

    $("#overallSpecsTab a , #pagination-list-content ul li").click(function (e) {
        $('html, body').animate({ scrollTop: $('#overallTabsWrapper').offset().top }, 500);
    });

});