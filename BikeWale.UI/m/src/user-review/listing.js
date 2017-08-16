var reviewId = 0, modelid, vmUserReviews, modelReviewsSection,categoryId = 1, pageNumber = 1;
var reg = new RegExp('^[0-9]*$');
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
var reportAbusePopup, appendState;

function abuseClick() {
    reportAbusePopup.open();
    appendState('reportPopup');
}

function upVoteListReview(e) {
    try {
        var localReviewId = e.currentTarget.getAttribute("data-reviewid");
        bwcache.set("ReviewDetailPage_reviewVote_" + localReviewId, { "vote": "1" });
        $('#upvoteBtn' + "-" + localReviewId).addClass('active');
        $('#downvoteBtn' + "-" + localReviewId).attr('disabled', 'disabled');

        if (reg.test($('#upvoteCount' + "-" + localReviewId).text()))
            $('#upvoteCount' + "-" + localReviewId).text(parseInt($('#upvoteCount' + "-" + localReviewId).text()) + 1);

        voteListUserReview(1, localReviewId);
    } catch (e) {
        console.log(e);
    }
}

function downVoteListReview(e) {
    try {
        var localReviewId = e.currentTarget.getAttribute("data-reviewid");
        bwcache.set("ReviewDetailPage_reviewVote_" + localReviewId, { "vote": "0" });
        $('#downvoteBtn' + "-" + localReviewId).addClass('active');
        $('#upvoteBtn' + "-" + localReviewId).attr('disabled', 'disabled');

        if (reg.test($('#downvoteCount' + "-" + localReviewId).text()))
            $('#downvoteCount' + "-" + localReviewId).text(parseInt($('#downvoteCount' + "-" + localReviewId).text()) + 1);

        voteListUserReview(0, localReviewId);
    } catch (e) {
        console.log(e);
    }
}

function updateView(e) {
    // for bhrigu updation
    var index = Number(e.currentTarget.getAttribute('data-id')) + 1;
    $.each(vmUserReviews.activeReviewList(), function (i, val) {
        if (e.currentTarget.getAttribute("data-reviewid") == val.reviewId) {
            index = i + 1;

        }

    });
    label = 'ModelId=' + modelid + '|TabName=' + reviewCategory[categoryId] + '|ReviewOrder=' + (index + (pageNumber - 1) * 8) + '|PageSource=' + $('#pageSource').val();
    cwTracking.trackUserReview("ReadMoreClick", label);

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

function voteListUserReview(vote, locReviewId) {
    try {
        $.ajax({
            type: "POST",
            url: "/api/user-reviews/voteUserReview/?reviewId=" + locReviewId + "&vote=" + vote,
            success: function (response) {
            }
        });
    } catch (e) {
        console.log(e);
    }
}

function applyLikeDislikes() {
    try {
        $(".upvoteListButton").each(function () {
            var locReviewId = this.getAttribute("data-reviewid");
            var listVote = bwcache.get("ReviewDetailPage_reviewVote_" + locReviewId);

            if (listVote != null && listVote.vote) {
                if (listVote.vote == "0") {
                    $('#downvoteBtn' + "-" + locReviewId).addClass('active');
                    $('#upvoteBtn' + "-" + locReviewId).attr('disabled', 'disabled');
                }
                else {
                    $('#upvoteBtn' + "-" + locReviewId).addClass('active');
                    $('#downvoteBtn' + "-" + locReviewId).attr('disabled', 'disabled');
                }
            }
            else {
                $('#upvoteBtn' + "-" + locReviewId).removeClass('active');
                $('#downvoteBtn' + "-" + locReviewId).prop('disabled', false);
            }
        });
    } catch (e) {
        console.log(e);
    }
}

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


function upVoteReview() {
    try {
        bwcache.set("ReviewDetailPage_reviewVote_" + reviewId, { "vote": "1" });
        $('#upvoteButton').addClass('active');
        $('#upvoteText').text("Liked");
        $('#downvoteButton').attr('disabled', 'disabled');
        $('#upvoteCount').text(parseInt($('#upvoteCount').text()) + 1);
        voteUserReview(1);
    } catch (e) {
        console.log(e);
    }
}


function downVoteReview() {
    try {
        bwcache.set("ReviewDetailPage_reviewVote_" + reviewId, { "vote": "0" });
        $('#downvoteButton').addClass('active');
        $('#downvoteText').text("Disliked");
        $('#upvoteButton').attr('disabled', 'disabled');
        $('#downvoteCount').text(parseInt($('#downvoteCount').text()) + 1);
        voteUserReview(0);
    } catch (e) {
        console.log(e);
    }
}


function voteUserReview(vote) {
    try {
        $.ajax({
            type: "POST",
            url: "/api/user-reviews/voteUserReview/?reviewId=" + reviewId + "&vote=" + vote,
            success: function (response) {
            }
        });
    } catch (e) {
        console.log(e);
    }
}
function reportAbuse() {
    try {
        var isError = false;

        if ($("#txtAbuseComments").val().trim() == "") {
            $("#spnAbuseComments").html("Comments are required");
            isError = true;
        } else {
            $("#spnAbuseComments").html("");
        }

        if (!isError) {
            var commentsForAbuse = $("#txtAbuseComments").val().trim();

            $.ajax({
                type: "POST",
                url: "/api/user-reviews/abuseUserReview/?reviewId=" + reviewId + "&comments=" + commentsForAbuse,
                success: function (response) {
                    reportAbusePopup.close();
                    document.getElementById("divAbuse").innerHTML = "Your request has been sent to the administrator.";
                }
            });
        }
    } catch (e) {
        console.log(e);
    }
}


docReady(function () {

    bwcache.setOptions({ 'EnableEncryption': true });

    bwcache.removeAll(true);

    applyLikeDislikes();

    modelReviewsSection = $("#modelReviewsListing");

    reviewId = $('#divAbuse').attr('data-reviewId');

    if ($('#section-review-details').length > 0)
        modelid = $('#section-review-details').attr('data-modelId');
    else
        modelid = $('#section-review-list').attr('data-modelId');

    var vote = bwcache.get("ReviewDetailPage_reviewVote_" + reviewId);

    if (vote != null && vote.vote) {
        if (vote.vote == "0") {
            $('#downvoteButton').addClass('active');
            $('#downvoteText').text("Disliked");
            $('#upvoteButton').attr('disabled', 'disabled');
        }
        else {
            $('#upvoteButton').addClass('active');
            $('#upvoteText').text("Liked");
            $('#downvoteButton').attr('disabled', 'disabled');
        }
    }

    $('#bike-rating-box').find('.answer-star-list input[type=radio]').change(function () {
        var button = $(this),
           buttonValue = Number(button.val());
        var q = $('#rate-star-' + buttonValue).attr('data-querystring');
        window.location.href = "/m/rate-your-bike/" + modelid + "/?q=" + q;
    });

    /* popup state */
    appendState = function (state) {
        window.history.pushState(state, '', '');
    };

    $(window).on('popstate', function (event) {
        if ($('#report-abuse-popup').is(':visible')) {
            reportAbusePopup.close();
        }
    });

    $('.report-abuse-close-btn').on('click', function () {
        reportAbusePopup.close();
        history.back();
    });

    reportAbusePopup = {
        open: function () {
            $('#report-abuse-popup').show();
            $('body').addClass('lock-browser-scroll');
            $('#popup-background').show();
        },

        close: function () {
            $('#report-abuse-popup').hide();
            $('body').removeClass('lock-browser-scroll');
            $('#popup-background').hide();
        }
    };

    ko.bindingHandlers.truncateDesc = {
        update: function (element, valueAccessor) {
            var originalText = strip(valueAccessor());
            var formattedText = originalText && originalText.length > 120 ? originalText.substring(0, 120) : originalText;
            $(element).text(formattedText);
        }
    };

    ko.bindingHandlers.CurrencyText = {
        update: function (element, valueAccessor) {
            var amount = valueAccessor();
            var formattedAmount = ko.unwrap(amount) !== null ? formatPrice(amount) : 0;
            $(element).text(formattedAmount);
        }
    };

    function strip(html) {
        var tmp = document.createElement("div");
        tmp.innerHTML = html;
        return tmp.textContent || tmp.innerText || "";
    }

    ko.bindingHandlers.formattedVotes = {
        update: function (element, valueAccessor) {
            var amount = valueAccessor();
            var formattedVote = ko.unwrap(amount) > 999 ? (amount / 1000).toFixed(1) + 'K' : amount;
            $(element).text(formattedVote);
        }
    };

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
        self.Filters = ko.observable({ pn: 1, ps: 8, model: modelid, so: 1, skipreviewid: reviewId });
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
                self.TotalReviews(categoryCount)

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
                    $("#pagination-list li").removeClass("active");
                    $("#pagination-list li a[data-pagenum=" + self.Pagination().pageNumber() + "]").parent().addClass("active");

                }
            } catch (e) {
                console.warn("Unable to apply pagination.");
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
                    self.TotalReviews(activeReviewCat.attr('data-count'));
                    pageNumber = pnum;
                    self.CurPageNo(pnum);
                    self.getUserReviews();
                    activeReviewCat.attr('data-page-num', pnum);
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
                var cacheKey = "UserReviews_mo_" + modelid + "_cat_" + self.Filters()["so"] + "_pn_" + self.Filters()["pn"] + "_ps_" + self.Filters()["ps"], skipreviewid = self.Filters()["skipreviewid"];

                if (skipreviewid && skipreviewid > 0) {
                    cacheKey += "_skiprid_" + skipreviewid;
                }

                var userreviewsData = bwcache.get(cacheKey);
                if (!userreviewsData) {
                    var apiUrl = "/api/user-reviews/search/?reviews=true&" + qs;
                    $.getJSON(apiUrl)
                    .done(function (response) {
                        if (response && response.result) {
                            self.activeReviewList(response.result);
                            self.TotalReviews(response.totalCount);
                            self.noReviews(false);
                            bwcache.set({ 'key': cacheKey, 'value': response, 'expiryTime': 30 });
                        }

                    })
                    .fail(function () {
                        self.noReviews(true);
                    })
                    .success(function () {
                        var listItem = $('.user-review-list .list-item');
                        for (var i = listItem.length - 1; i > 0 ; i--) {
                            if ($(listItem[i]).find('.rating-badge').length == 0)
                                $(listItem[i]).remove();
                        }
                    })
                    .always(function () {
                        self.ApplyPagination();
                        //window.location.hash = qs;
                        self.IsLoading(false);
                        $('html, body').scrollTop(modelReviewsSection.offset().top);
                    });
                }
                else {
                    self.activeReviewList(userreviewsData.result);
                    self.TotalReviews(userreviewsData.totalCount);
                    self.noReviews(false);
                    self.ApplyPagination();
                    //window.location.hash = qs;
                    self.IsLoading(false);
                    $('html, body').scrollTop(modelReviewsSection.offset().top);
                    var listItem = $('.user-review-list .list-item');
                    for (var i = listItem.length - 1; i > 0 ; i--) {
                        if ($(listItem[i]).find('.rating-badge').length == 0)
                            $(listItem[i]).remove();
                    }
                }
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

        self.checkValidCache = function () {

        }

    }

    vmUserReviews = new modelUserReviews();

    $("#overallSpecsTab ul li , #pagination-list-content ul li").click(function (e) {
        if (vmUserReviews && !vmUserReviews.IsInitialized()) {
            vmUserReviews.IsLoading(true);
            $('html, body').scrollTop(modelReviewsSection.offset().top);
            vmUserReviews.init(e);
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

    $('.user-review-tabs .overall-specs-tabs-wrapper li').click(function () {
        scrollToStart('#modelReviewsListing', $('#overallSpecsTopContent'), $(this));
    });

    function scrollToStart(listId, focusPoint, tab) {
        $('html, body').animate({ scrollTop: focusPoint.offset().top }, 1000);
        centerItVariableWidth(tab, listId + ' .overall-specs-tabs-container');
        return false;
    };

    var checkedStar = $('input[name=rate-bike]:checked').val();
    if (checkedStar)
        document.getElementById('rate-star-' + parseInt(checkedStar)).checked = false;

});