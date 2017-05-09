var helpfulReviews = [
    {
        "comments": "",
        "pros": null,
        "cons": null,
        "liked": 0,
        "disliked": 0,
        "viewed": 0,
        "makeMaskingName": null,
        "modelMaskingName": null,
        "overAllRating": {
            "overAllRating": 0
        },
        "reviewId": 0,
        "reviewTitle": "",
        "reviewDate": "",
        "writtenBy": "test"
    }
];

var reviewCategory = {
    1: 'helpful',
    2: 'recent',
    3: 'positive',
    4: 'negative',
    5: 'neutral'
}

var $window, overallSpecsTabsContainer, overallSpecsTab, specsFooter, topNavBarHeight;

var listItemHeight = 230; // min item height + pagination height

var vmPagination = function (curPgNum, pgSize, totalRecords) {
    var self = this;
    self.totalData = ko.observable(totalRecords);
    self.pageNumber = ko.observable(curPgNum);
    self.pageSize = ko.observable(pgSize);
    self.pageSlot = ko.observable(5);
    self.totalPages = ko.computed(function () {
        var div = Math.ceil(self.totalData() / self.pageSize());
        return div;
    });
    self.paginated = ko.computed(function () {
        var pgSlot;

        if (self.pageNumber() < 4) {
            pgSlot = self.pageSlot();
        } else {
            pgSlot = self.pageNumber() + self.pageSlot() - 3;
        }

        if (self.totalPages() > pgSlot) {
            return pgSlot;
        } else {
            return self.totalPages();
        }

    });
    self.hasPrevious = ko.computed(function () {
        return self.pageNumber() != 1;
    });
    self.hasNext = ko.computed(function () {
        return self.pageNumber() != self.totalPages();
    });
    self.next = function () {
        if (self.pageNumber() < self.totalPages())
            return self.pageNumber() + 1;
        return self.pageNumber();
    }
    self.previous = function () {
        if (self.pageNumber() > 1) {
            return self.pageNumber() - 1;
        }
        return self.pageNumber();
    }
};


docReady(function () {



    var modelUserReviews = function () {
        var self = this;
        self.IsInitialized = ko.observable(false);
        self.PagesListHtml = ko.observable("");
        self.PrevPageHtml = ko.observable("");
        self.NextPageHtml = ko.observable("");
        self.activeReviewList = ko.observableArray(helpfulReviews);
        self.activeReviewCategory = ko.observable(0);
        self.reviewsAvailable = ko.observable(true);
        self.categoryName = ko.observable('');
        self.IsLoading = ko.observable(false);
        self.Filters = ko.observable({ pn: 1, ps: 8, model: 411, so: 2 });
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

                self.Filters()["make"] = self.Filters()["make"] || eleSection.data("make") || "";

                self.Filters()["model"] = self.Filters()["model"] || eleSection.data("model") || "";

                self.Filters()["cat"] = self.Filters()["cat"] || eleSection.data("cat") || "";

                self.Filters()["order"] = self.Filters()["order"] || eleSection.data("order") || "";

                var filterType = $(e.target).data("category");
                if (filterType && filterType!="0") {
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
                    categoryCount = Number(element.attr('data-count')),

                catTypes = element.attr('data-cattypes');

                self.Filters()["pn"] = pageNumber || 1;
                self.Filters()["so"] = categoryId;
                self.Filters()["cat"] = catTypes;

                if (categoryId != 1 && categoryId != 2) {
                    self.Filters()["so"] = 2;
                }

                if (categoryCount) {
                    self.reviewsAvailable(true);
                    self.activeReviewCategory(categoryId);
                    //element.attr('data-page-num', pageNumber + 1);
                }
                else {
                    self.tabEvents.setNoReview(categoryId);
                }

                self.getUserReviews();
            },

            setNoReview: function (categoryId) {
                self.reviewsAvailable(false);
                self.categoryName(reviewCategory[categoryId]);
            }

        };

        self.ApplyPagination = function () {
            try {
                var pag = new vmPagination(self.Filters().pn, self.Filters().ps, self.TotalReviews());
                self.Pagination(pag);
                if (self.Pagination()) {
                    var n = self.Pagination().paginated(), pages = '', prevpg = '', nextpg = '';
                    var qs = window.location.pathname + window.location.hash;
                    var rstr = qs.match(/page-[0-9]+/i);
                    var startIndex = (self.Pagination().pageNumber() - 2 > 0) ? (self.Pagination().pageNumber() - 2) : 1;
                    for (var i = startIndex ; i <= n; i++) {
                        var pageUrl = qs.replace(rstr, "page-" + i);
                        pages += ' <li class="page-url ' + (i == self.CurPageNo() ? 'active' : '') + ' "><a  data-bind="click : function(d,e) { $root.ChangePageNumber(e); } " data-pagenum="' + i + '" href="' + pageUrl + '">' + i + '</a></li>';
                    }
                    self.PagesListHtml(pages);
                    $(".pagination-control-prev,.pagination-control-next").removeClass("active inactive");
                    if (self.Pagination().hasPrevious()) {
                        prevpg = "<a  data-bind='click : $root.ChangePageNumber' data-pagenum='" + self.Pagination().previous() + "' href='" + qs.replace(rstr, "page-" + self.Pagination().previous()) + "' class='bwmsprite prev-page-icon'/>";
                    } else {
                        prevpg = "<a href='javascript:void(0)' class='bwmsprite prev-page-icon'></a>";
                        $(".pagination-control-prev").addClass("inactive");
                    }
                    self.PrevPageHtml(prevpg);
                    if (self.Pagination().hasNext()) {
                        nextpg = "<a  data-bind='click : $root.ChangePageNumber' data-pagenum='" + self.Pagination().next() + "' href='" + qs.replace(rstr, "page-" + self.Pagination().next()) + "' class='bwmsprite next-page-icon'/>";
                    } else {
                        nextpg = "<a href='javascript:void(0)' class='bwmsprite next-page-icon'></a>";
                        $(".pagination-control-next").addClass("inactive");
                    }
                    self.NextPageHtml(nextpg);
                    $("#pagination-list li[data-pagenum=" + self.Pagination().pageNumber() + "]").addClass("active");

                }
            } catch (e) {
                console.warn("Unable to apply pagination.");
            }

        };

        self.ChangePageNumber = function (e) {
            try {
                var ele = $(e.target), pnum = parseInt(ele.attr("data-pagenum"), 10);
                if (pnum && !isNaN(pnum) && !ele.parent().hasClass("active")) {
                    var selHash = ele.attr("data-hash");
                    self.Filters()["pn"] = pnum;

                    if (selHash) {
                        var arr = selHash.split('&');
                        var curcityId = arr[0].split("=")[1], curmakeId = arr[1].split("=")[1], curmodelId = arr[2].split("=")[1];

                    }
                    self.CurPageNo(pnum);
                    self.getUserReviews();
                    $("#overallSpecsTab ul li.active").attr('data-page-num', pnum);
                }
                e.preventDefault();
                $('html, body').scrollTop(0);
            } catch (e) {
                console.warn("Unable to change page number : " + e.message);
            }
            return false;
        };

        self.getUserReviews = function () {
            self.Filters.notifySubscribers();

            var qs = self.QueryString();

            if (self.PreviousQS() != qs) {

                self.IsLoading(true);
                self.PreviousQS(qs);
                var apiUrl = "/api/user-reviews/search/?reviews=true&" + qs;
                $.getJSON(apiUrl)
                .done(function (response) {
                    if (response && response.result) {
                        self.activeReviewList(response.result);
                        self.TotalReviews(response.totalCount);
                        self.noReviews(false);
                    }

                })
                .fail(function () {
                    self.noReviews(true);
                })
                .always(function () {
                    self.ApplyPagination();
                    window.location.hash = qs;
                    self.IsLoading(false);
                    $('html, body').scrollTop($('#modelReviewsListing').offset().top);
                });
            }
            else {
                history.back();
            }
        };

        self.setPageFilters = function (e) {
            var currentQs = window.location.hash.substr(1);
            if (currentQs != "") {
                var _filters = currentQs.split("&"), objFilter = {};
                for (var i = 0; i < _filters.length; i++) {
                    var f = _filters[i].split("=");
                    self.Filters()[f[0]] = f[1];
                }
                self.CurPageNo((self.Filters()["pageNo"] ? parseInt(self.Filters()["pageNo"]) : 0));

                if (self.Filters()["make"] && self.Filters()["make"] != "") {
                    var selOption = $("#brand-slideIn-drawer ul li[data-makeid='" + self.Filters()["make"] + "']");
                    self.selectedMake(selOption.data("makename"));
                }

                if (self.Filters()["yearLaunch"] && self.Filters()["yearLaunch"] != "") {
                    var selOption = $("#year-slideIn-drawer ul li[data-bikeyear='" + self.Filters()["yearLaunch"] + "']");
                    self.selectedYear(selOption.data("bikeyear"));
                }

                self.init(e);
            }

        };

    }

    var vmUserReviews = new modelUserReviews();

    $("#overallSpecsTab ul li , #pagination-list-content ul li").click(function (e) {
        if (vmUserReviews && !vmUserReviews.IsInitialized()) {
            vmUserReviews.init(e);
            $('html, body').scrollTop(0);
            return false;
        }
    });



    $window = $(window);
    overallSpecsTabsContainer = $('#overallSpecsTopContent');
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

            else if (windowScrollTop + listItemHeight < specsTabsOffsetTop) {
                overallSpecsTab.removeClass('fixed-tab-nav');
            }

            if (overallSpecsTab.hasClass('fixed-tab-nav')) {
                if (windowScrollTop + listItemHeight > specsFooterOffsetTop - topNavBarHeight) {
                    overallSpecsTab.removeClass('fixed-tab-nav');
                }
            }
        });
    }

    $('.user-review-tabs .overall-specs-tabs-wrapper li').click(function () {
        scrollToStart('#modelReviewsListing', $('#overallSpecsTopContent'), $(this));
    });

    function scrollToStart(listId, focusPoint, tab) {
        $('html, body').animate({ scrollTop: focusPoint.offset().top }, 1000);
        centerItVariableWidth(tab, listId + ' .overall-specs-tabs-container');
        return false;
    };

});