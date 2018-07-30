
var reviewId = 0, vmUserReviews, modelReviewsSection, modelid, abusereviewId = 0, categoryId = 1, pageNumber = 1, modelName, makeName, makeId;
var reg = new RegExp('^[0-9]*$');
var reviewsReadPerSession;

var helpfulReviews = [];
var expertReviewWidgetHtml;

var reviewCategory = {
    2: 'helpful',
    1: 'recent',
    5: 'positive',
    6: 'critical',
    7: 'neutral'
}

var $window, overallSpecsTabsContainer, overallSpecsTab, specsFooter, topNavBarHeight;

var appendState, listItemHeight = 230; // min item height + pagination height

function abuseClick() {
    reportAbusePopup.open();
    appendState('reportPopup');
}

function upVoteReview() {
    bwcache.set("ReviewDetailPage_reviewVote_" + reviewId, { "vote": "1" });
    $('#upvoteBtn').addClass('active');
    $('#upvoteTxt').text("Liked");
    $('#downvoteBtn').attr('disabled', 'disabled');
    $('#upvoteCount').text(parseInt($('#upvoteCount').text()) + 1);
    voteUserReview(1);
}

function upVoteListReview(e) {
    var localReviewId = e.currentTarget.getAttribute("data-reviewid");
    bwcache.set("ReviewDetailPage_reviewVote_" + localReviewId, { "vote": "1" });
    $('#upvoteBtn' + "-" + localReviewId).addClass('active');
    $('#downvoteBtn' + "-" + localReviewId).attr('disabled', 'disabled');

    if (reg.test($('#upvoteCount' + "-" + localReviewId).text()))
        $('#upvoteCount' + "-" + localReviewId).text(parseInt($('#upvoteCount' + "-" + localReviewId).text()) + 1);

    voteListUserReview(1, localReviewId);
}

function downVoteReview() {
    bwcache.set("ReviewDetailPage_reviewVote_" + reviewId, { "vote": "0" });
    $('#downvoteBtn').addClass('active');
    $('#downvoteTxt').text("Disliked");
    $('#upvoteBtn').attr('disabled', 'disabled');
    $('#downvoteCount').text(parseInt($('#downvoteCount').text()) + 1);
    voteUserReview(0);
}

function downVoteListReview(e) {
    var localReviewId = e.currentTarget.getAttribute("data-reviewid");
    bwcache.set("ReviewDetailPage_reviewVote_" + localReviewId, { "vote": "0" });
    $('#downvoteBtn' + "-" + localReviewId).addClass('active');
    $('#upvoteBtn' + "-" + localReviewId).attr('disabled', 'disabled');

    if (reg.test($('#downvoteCount' + "-" + localReviewId).text())) {

        $('#downvoteCount' + "-" + localReviewId).text(parseInt($('#downvoteCount' + "-" + localReviewId).text()) + 1);
    }

    voteListUserReview(0, localReviewId);
}

function voteListUserReview(vote, locReviewId) {
    $.ajax({
        type: "POST",
        url: "/api/user-reviews/voteUserReview/?reviewId=" + locReviewId + "&vote=" + vote,
        success: function (response) {
        }
    });
}

function voteUserReview(vote) {
    $.ajax({
        type: "POST",
        url: "/api/user-reviews/voteUserReview/?reviewId=" + reviewId + "&vote=" + vote,
        success: function (response) {
        }
    });
}

function reportReview(e) {
    abusereviewId = e.currentTarget.getAttribute("data-reviewid");
    reportAbusePopup.open();
}

function applyLikeDislikes() {
    $(".feedback-button").each(function () {
        var locReviewId = this.getAttribute("data-reviewid");
        var listVote = bwcache.get("ReviewDetailPage_reviewVote_" + locReviewId);

        if (listVote != null && listVote.vote) {
            if (listVote.vote == "0") {
                $('#downvoteBtn' + "-" + locReviewId).addClass('active');
                $('#upvoteBtn' + "-" + locReviewId).removeClass('active').attr('disabled', 'disabled');
            }
            else {
                $('#upvoteBtn' + "-" + locReviewId).addClass('active');
                $('#downvoteBtn' + "-" + locReviewId).removeClass('active').attr('disabled', 'disabled');
            }
        }
        else {
            $('#upvoteBtn' + "-" + locReviewId).removeClass('active');
            $('#upvoteBtn' + "-" + locReviewId).prop('disabled', false);
            $('#downvoteBtn' + "-" + locReviewId).removeClass('active');
            $('#downvoteBtn' + "-" + locReviewId).prop('disabled', false);
        }
    });
}

function reportAbuse() {
    var isError = false;

    if ($("#txtAbuseComments").val().trim() == "") {
        $("#spnAbuseComments").html("Comments are required");
        isError = true;
    } else {
        $("#spnAbuseComments").html("");
    }

    var locReviewId;
    if (abusereviewId > 0) {
        locReviewId = abusereviewId;
        document.getElementById("pReport-" + locReviewId).innerHTML = "Your request has been sent to the administrator.";
    }
    else {
        locReviewId = reviewId;
        document.getElementById("divReportAbuse").innerHTML = "Your request has been sent to the administrator.";
    }

    if (!isError) {
        var commentsForAbuse = $("#txtAbuseComments").val().trim();
        $.ajax({
            type: "POST",
            url: "/api/user-reviews/abuseUserReview/?reviewId=" + locReviewId + "&comments=" + commentsForAbuse,
            success: function (response) {
                reportAbusePopup.close();
            }
        });
    }
}

function resetCollapsibleContent() {
    var activeCollapsible = $('.user-review-list').find('.collapsible-content.active');
    activeCollapsible.removeClass('active');
    activeCollapsible.find('.read-more-target').text('Read more');
}

var reportAbusePopup = {
    popupElement: $('#report-abuse'),

    bgContainer: $('#report-background'),

    open: function () {
        reportAbusePopup.popupElement.show();
        popup.lock();
        $(".blackOut-window").hide();
        reportAbusePopup.bgContainer.show();
    },
    close: function () {
        reportAbusePopup.popupElement.hide();
        popup.unlock();
        reportAbusePopup.bgContainer.hide();
    }
};

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

    bwcache.removeAll(true);

    modelReviewsSection = $("#modelReviewsListing");

    reviewId = $('#divReportAbuse').attr('data-reviewId');
    modelName = $('#modelName').attr('data-modelName');
    makeName = $('#modelName').attr('data-makeName');
    reviewsReadPerSession = modelReviewsSection.attr('data-userReviewsReadCount');

    var vote = bwcache.get("ReviewDetailPage_reviewVote_" + reviewId);

    if (vote != null && vote.vote) {
        if (vote.vote == "0") {
            $('#downvoteBtn').addClass('active');
            $('#downvoteTxt').text("Disliked");
            $('#upvoteBtn').attr('disabled', 'disabled');
        }
        else {
            $('#upvoteBtn').addClass('active');
            $('#upvoteTxt').text("Liked");
            $('#downvoteBtn').attr('disabled', 'disabled');
        }
    }

    applyLikeDislikes();

    if ($('#hdnModelId').length > 0)
        modelid = $('#hdnModelId').val();

    if ($('#hdnMakeId').length > 0)
        makeId = $('#hdnMakeId').val();

    $('#bike-rating-box').find('.answer-star-list input[type=radio]').change(function () {
        var button = $(this),
           buttonValue = Number(button.val());
        var q = $('#rate-star-' + buttonValue).attr('data-querystring');
        window.location.href = "/rate-your-bike/" + modelid + "/?q=" + q;
    });

    ko.bindingHandlers.CurrencyText = {
        update: function (element, valueAccessor) {
            var amount = valueAccessor();
            var formattedAmount = ko.unwrap(amount) !== null ? formatPrice(amount) : 0;
            $(element).text(formattedAmount);
        }
    };

    ko.bindingHandlers.trimText = {
        update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            if (ko.utils.unwrapObservable(valueAccessor())) {
                var untrimmedText = ko.utils.unwrapObservable(valueAccessor());
                var defaultMaxLength = 20;
                var maxLength = ko.utils.unwrapObservable(allBindingsAccessor().trimTextLength) || defaultMaxLength;
                var formattedAmount = untrimmedText.length > maxLength ? untrimmedText.substring(0, maxLength - 1) + '...' : untrimmedText;
                $(element).text(formattedAmount);
            }
        }
    };

    ko.bindingHandlers.truncatedText = {
        update: function (element, valueAccessor, allBindingsAccessor) {
            if (ko.utils.unwrapObservable(valueAccessor())) {
                var originalText = ko.utils.unwrapObservable(valueAccessor()),
                     length = parseInt(element.getAttribute("data-trimlength")) || 65,
                    truncatedText = originalText.length > length ? originalText.substring(0, length) + "..." : originalText;
                ko.bindingHandlers.text.update(element, function () {
                    return truncatedText;
                });
            }
        }
    };

    $('#bike-rating-box1').find('.answer-star-list input[type=radio]').change(function () {
        var button = $(this),
           buttonValue = Number(button.val());
        var q = $('#share-rate-' + buttonValue).attr('data-querystring');
        window.location.href = "/rate-your-bike/" + modelid + "/?q=" + q;
    });

    $('#bike-rating-box2').find('.answer-star-list input[type=radio]').change(function () {
        var button = $(this),
           buttonValue = Number(button.val());
        var q = $('#rate-bikestar-' + buttonValue).attr('data-querystring');
        window.location.href = "/rate-your-bike/" + modelid + "/?q=" + q;
    });


    ko.bindingHandlers.formattedVotes = {
        update: function (element, valueAccessor) {
            try {
                var amount = valueAccessor();
                var formattedStringArray = (amount / 1000).toString().match(/\d+[.]+\d/);
                if (amount % 1000 == 0 && amount > 0) {
                    var formattedVote = amount / 1000 + '.0k';
                }
                else {
                    var formattedVote = ko.unwrap(amount) > 999 && formattedStringArray ? formattedStringArray[0] + 'k' : amount;
                }
                $(element).text(formattedVote);
            } catch (e) {
                console.warn(e);
            }
        }
    };

    var modelUserReviews = function () {
        var self = this;
        if ($('#overallSpecsTab .active')[0])
            var reviewCount = $('#overallSpecsTab .active')[0].getAttribute("data-count");
        self.IsInitialized = ko.observable(false);
        self.IsApiData = ko.observable(false);
        self.PagesListHtml = ko.observable("");
        self.activeReviewList = ko.observableArray([]);
        self.activeReviewCategory = ko.observable(0);
        self.reviewsAvailable = ko.observable(true);
        self.PrevPageHtml = ko.observable("");
        self.NextPageHtml = ko.observable("");
        self.categoryName = ko.observable('');
        self.PreviousQS = ko.observable("");
        self.IsLoading = ko.observable(false);
        self.Pagination = ko.observable(new vmPagination());
        self.TotalReviews = ko.observable(reviewCount);
        self.noReviews = ko.observable(self.TotalReviews() == 0);
        self.PageUrl = ko.observable();
        self.CurPageNo = ko.observable();
        self.firstReadMoreClick = ko.observable(false);
        self.clickedReviewId = ko.observable();

        self.Filters = ko.observable({ pn: 1, ps: 10, model: modelid, so: reviewId > 0 ? 1 : 2, skipreviewid: reviewId });
        self.QueryString = ko.computed(function () {
            var qs = "";
            $.each(self.Filters(), function (i, val) {
                if (val != null && val != "")
                    qs += "&" + i + "=" + val;
            });
            qs = qs.substr(1);
            return qs;
        });

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
                categoryId = Number(element.attr('data-category')),
                   pageNumber = Number(element.attr('data-page-num') || 1),
                   categoryCount = Number(element.attr('data-count'));

                catTypes = element.attr('data-cattypes');
                self.TotalReviews(categoryCount);

                self.Filters()["pn"] = pageNumber || 1;
                self.Filters()["so"] = categoryId;
                self.Filters()["cat"] = catTypes;

                if (categoryCount) {
                    self.reviewsAvailable(true);
                    self.activeReviewCategory(categoryId);
                }
                else {
                    self.tabEvents.setNoReview(categoryId);
                }

                triggerGA('User_Reviews', 'Tabs_Clicked', modelName + ' _Clicked_on_' + reviewCategory[categoryId]);
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
                        prevpg = "<a  data-bind='click : $root.ChangePageNumber' data-pagenum='" + self.Pagination().previous() + "' href='" + qs.replace(rstr, "page-" + self.Pagination().previous()) + "' class='bwmsprite bwsprite prev-page-icon'/>";
                    } else {
                        prevpg = "<a href='javascript:void(0)' class='bwmsprite prev-page-icon'></a>";
                        $(".pagination-control-prev").addClass("inactive");
                    }
                    self.PrevPageHtml(prevpg);
                    if (self.Pagination().hasNext()) {
                        nextpg = "<a  data-bind='click : $root.ChangePageNumber' data-pagenum='" + self.Pagination().next() + "' href='" + qs.replace(rstr, "page-" + self.Pagination().next()) + "' class='bwmsprite bwsprite next-page-icon'/>";
                    } else {
                        nextpg = "<a href='javascript:void(0)' class='bwmsprite next-page-icon'></a>";
                        $(".pagination-control-next").addClass("inactive");
                    }
                    self.NextPageHtml(nextpg);
                    $("#pagination-list li").removeClass("active");
                    $("#pagination-list li a[data-pagenum=" + self.Pagination().pageNumber() + "]").parent().addClass("active");

                }
            } catch (e) {
                console.warn("Unable to apply pagination." + e);
            }

        };

        self.ChangePageNumber = function (e) {
            try {
                var ele = $(e.target), pnum = parseInt(ele.attr("data-pagenum"), 10);
                if (pnum && !isNaN(pnum) && !ele.parent().hasClass("active")) {
                    var activeReviewCat = $("#overallSpecsTab ul li.active");
                    var selHash = ele.attr("data-hash");
                    self.Filters()["pn"] = pnum;

                    if (selHash) {
                        var arr = selHash.split('&');
                        var curcityId = arr[0].split("=")[1], curmakeId = arr[1].split("=")[1], curmodelId = arr[2].split("=")[1];

                    }
                    if (activeReviewCat.length > 0) {
                        self.TotalReviews(activeReviewCat.attr('data-count'));
                        activeReviewCat.attr('data-page-num', pnum);
                    }
                    pageNumber = pnum;
                    self.CurPageNo(pnum);
                    self.getUserReviews();
                }
                e.preventDefault();
                $('html, body').scrollTop(modelReviewsSection.offset().top);
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
                var apiUrl = "/api/user-reviews/search/?reviews=true&" + qs;
                $.getJSON(apiUrl)
                .done(function (response) {
                    if (response && response.result) {
                        self.IsApiData(true);
                        self.activeReviewList(response.result);
                        self.TotalReviews(response.totalCount);
                        self.noReviews(false);
                        var listItem = $('.user-review-list .list-item');
                        for (var i = listItem.length; i >= response.result.length; i--) {
                            $(listItem[i]).remove();
                        }

                        resetCollapsibleContent();
                        applyLikeDislikes();

                        $('html, body').scrollTop(modelReviewsSection.offset().top);

                        if (self.firstReadMoreClick()) {
                            var collpasibleContent = $(document).find('.read-more-target[data-reviewId=' + self.clickedReviewId() + ']').closest('.collapsible-content');
                            $('html, body').scrollTop(collpasibleContent.closest('.list-item').offset() ? (collpasibleContent.closest('.list-item').offset().top - $('#overallSpecsTab').height()) : "0");
                            collpasibleContent.addClass('active');
                            self.firstReadMoreClick(false);
                        }
                    }

                })
                .fail(function () {
                    self.noReviews(true);
                    $('html, body').scrollTop(modelReviewsSection.offset().top);
                })
                .always(function () {
                    self.ApplyPagination();
                    //window.location.hash = qs;
                    self.IsLoading(false);
                });
                self.ApplyPagination();
            }
            self.PreviousQS(qs);
        };

        self.setPageFilters = function (e) {
            var currentQs = window.location.hash.substr(1);
            if (currentQs != "") {
                vmUserReviews.IsLoading(true);
                var _filters = currentQs.split("&"), objFilter = {};
                for (var i = 0; i < _filters.length; i++) {
                    var f = _filters[i].split("=");
                    self.Filters()[f[0]] = f[1];
                }
                self.CurPageNo((self.Filters()["pn"] ? parseInt(self.Filters()["pn"]) : 0));
                if (self.Filters()["so"]) {
                    var ele = $("#overallSpecsTab ul li[data-category='" + self.Filters()["so"] + "']");
                    self.tabEvents.toggleTab(ele);
                    self.init(e);
                };

            }

        };

        self.readMore = function (event) {
            if (!self.activeReviewList().length && modelid) {
                self.getUserReviews();

            }

            if ($('#modelReviewsListing')) {
                $('#modelReviewsListing').attr('data-readMoreCount', parseInt($('#modelReviewsListing').attr('data-readMoreCount')) + 1);
            }

            if ($('#modelReviewsListing').attr('data-readMoreCount') == (parseInt(reviewsReadPerSession) - 1) ){
                $.ajax({
                    type: "GET",
                    url: "/expertreviews/list/?makeId=" + makeId + "&modelId=" + modelid + "&topCount=12",
                    success: function (response) {
                        expertReviewWidgetHtml = response;                        
                        if (parseInt($('#modelReviewsListing').attr('data-readMoreCount')) > (parseInt(reviewsReadPerSession) - 1)) {
                            self.bindExpertReviewWidget(event);
                        }
                    }
                });
            }

            if ($('#modelReviewsListing').attr('data-readMoreCount') == reviewsReadPerSession) {
                self.bindExpertReviewWidget(event);
            }

            updateView(event);
            logBhrighu(event, 'ReadMoreClick');

            triggerGA("User_Reviews", "Clicked_On_Read_More", makeName + "_" + modelName + "_" + (reviewId > 0 ? "Details" : "List"));

            return true;
        };

        self.bindExpertReviewWidget = function (event) {
            var reviewId = event.currentTarget.getAttribute("data-reviewid");            
            var ele = $(document).find('.insertWidget[data-reviewId=' + reviewId + ']');
            ele.after(expertReviewWidgetHtml);

            $('.expert-review-list .jcarousel').jcarousel({
                vertical: false
            });
            $('.jcarousel-control-prev').on('jcarouselcontrol:active', function () {
                $(this).removeClass('inactive');
            }).on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
            }).jcarouselControl({
                target: '-=' + _target
            });
            $('.jcarousel-control-next').on('jcarouselcontrol:active', function () {
                $(this).removeClass('inactive');                
            }).on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
            }).jcarouselControl({
                target: '+=' + _target
            });
           
            $('.jcarousel ul img.lazy').lazyload();

            if (expertReviewWidgetHtml.trim()) {
                triggerGA("User_Reviews", "ExpertReviews_CarouselLoaded", makeName + "_" + modelName);
            }

            $(".expert-review-list .jcarousel").on('jcarousel:visiblein', 'li', function (event, carousel) {
                $(this).find("img.lazy").trigger('appear');
            });

        };
    };

    $(document).on("click", ".read-more-target", function (e) {
        if (!vmUserReviews.IsInitialized()) {
            vmUserReviews.clickedReviewId($(this).attr('data-reviewId'));
            vmUserReviews.init(e);
            vmUserReviews.readMore(e);
            vmUserReviews.firstReadMoreClick(true);
        }
    });

    $(document).on("click", ".expert-review-list .jcarousel-card a", function (e) {
        triggerGA("User_Reviews", "ExpertReviews_CarouselCard_Clicked", makeName + "_" + modelName);
    });

    $(document).on("click", "#similar-bike-list .jcarousel-control-next", function (e) {
        triggerGA("User_Reviews", "Clicked_On_SimilarBikes_Carousel", makeName + "_" + modelName + "_" + (reviewId > 0 ? "Details" : "List"));
    });

    $(document).on("click", "#similar-bike-list .jcarousel-control-prev", function (e) {
        triggerGA("User_Reviews", "Clicked_On_SimilarBikes_Carousel", makeName + "_" + modelName + "_" + (reviewId > 0 ? "Details" : "List"));
    });

    $(document).on("click", "#pagination-list-content ul li, .pagination-control-prev a, .pagination-control-next a,#overallSpecsTab .overall-specs-tabs-wrapper a", function (e) {
        e.preventDefault();
        if (!vmUserReviews.IsInitialized()) {
            vmUserReviews.IsLoading(true);
            $('html, body').scrollTop(modelReviewsSection.offset().top);
            vmUserReviews.init(e);

        }
        else {
            vmUserReviews.ChangePageNumber(e);
        }

    });

    vmUserReviews = new modelUserReviews();

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

        resetCollapsibleContent();
    });


    $('#btnReportClick').on('click', function () {
        reportAbusePopup.open();
    });

    $('#report-background, .report-abuse-close-btn').on('click', function () {
        reportAbusePopup.close();
    });

    $(document).keydown(function (event) {
        if (event.keyCode == 27) {
            if (reportAbusePopup.popupElement.is(':visible')) {
                reportAbusePopup.close();
            }
        }
    });

    appendState = function (state) {
        window.history.pushState(state, '', '');
    };

    var checkedStar = $('input[name=rate-bike]:checked').val();
    if (checkedStar)
        document.getElementById('rate-star-' + parseInt(checkedStar)).checked = false;

    var chkRating1 = $('input[name=share-rate]:checked').val();
    if (chkRating1)
        document.getElementById('share-rate-' + parseInt(chkRating1)).checked = false;

    var chkRating2 = $('input[name=rate-bikestar]:checked').val();
    if (chkRating2)
        document.getElementById('rate-bikestar-' + parseInt(chkRating2)).checked = false;
});

function logBhrighu(e, action) {

    var index = Number(e.currentTarget.getAttribute('data-id')) + 1;
    $.each(vmUserReviews.activeReviewList(), function (i, val) {
        if (e.currentTarget.getAttribute("data-reviewid") == val.reviewId) {
            index = i + 1;

        }

    });
    label = 'modelId=' + modelid + '|tabName=' + reviewCategory[categoryId] + '|reviewOrder=' + (index + (pageNumber - 1) * 10) + '|pageSource=' + $('#pageSource').val();
    cwTracking.trackUserReview(action, label);
}


function updateView(e) {
    try {
        var reviewId = e.currentTarget.getAttribute("data-reviewid");
        $.ajax({
            type: "POST",
            url: "/api/user-reviews/updateView/" + reviewId + "/",
            success: function (response) {
            }
        });
    } catch (e) {
        console.log(e);
    }
}