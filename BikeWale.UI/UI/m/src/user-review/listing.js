
var reviewId = 0, makeid, modelid, vmUserReviews, modelReviewsSection, categoryId = 1
var pageNumber = 1, modelName, makeName, vmModelExpertReviewsList, vmModelPopularBikes, vmModelPopularBikesBodyStyle, expertReviewCount;
var reg = new RegExp('^[0-9]*$');
var helpfulReviews = [];
var expertReviewWidgetHtml;
var reviewsReadPerSession;

var reviewCategory = {
    2: 'helpful',
    1: 'recent',
    5: 'positive',
    6: 'critical',
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

function resetCollapsibleContent() {
    var activeCollapsible = $('.user-review-list').find('.collapsible-content.active');
    activeCollapsible.removeClass('active');
    activeCollapsible.find('.read-more-target').text('Read more');
}

function applyLikeDislikes() {
    try {
        $(".upvoteListButton").each(function () {
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
                $('#downvoteBtn' + "-" + locReviewId).removeClass('active');
                $('#downvoteBtn' + "-" + locReviewId).prop('disabled', false);
                $('#upvoteBtn' + "-" + locReviewId).prop('disabled', false);
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
        //$('#upvoteText').text("Liked");
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
        //$('#downvoteText').text("Disliked");
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
    
    bwcache.removeAll(true);

    applyLikeDislikes();

    modelReviewsSection = $("#modelReviewsListing");
    modelName = $('#modelName').attr('data-modelName');
    makeName = $('#modelName').attr('data-makeName');
    reviewId = $('#divAbuse').attr('data-reviewId');
    reviewsReadPerSession = modelReviewsSection.attr('data-userReviewsReadCount');

    if ($('#section-review-details').length > 0) {
        makeid = $('#section-review-details').attr('data-makeid');
        modelid = $('#section-review-details').attr('data-modelId');
    }
    else {
        makeid = $('#section-review-list').attr('data-makeid');
        modelid = $('#section-review-list').attr('data-modelId');
        expertReviewCount = $('#section-review-list').attr('data-expertReviewCount');
    }

    var vote = bwcache.get("ReviewDetailPage_reviewVote_" + reviewId);

    if (vote != null && vote.vote) {
        if (vote.vote == "0") {
            $('#downvoteButton').addClass('active');
            $('#upvoteButton').attr('disabled', 'disabled');
        }
        else {
            $('#upvoteButton').addClass('active');
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
            var formattedText = originalText && originalText.length > 120 ? originalText.substring(0, 120) + '...' : originalText;
            $(element).text(formattedText);
        }
    };

    ko.bindingHandlers.truncatedText = {
        update: function (element, valueAccessor, allBindingsAccessor) {
            if (ko.utils.unwrapObservable(valueAccessor())) {
                var originalText = ko.utils.unwrapObservable(valueAccessor()),
                    length = parseInt(element.getAttribute("data-trimlength")) || 150,
                    truncatedText = originalText.length > length ? originalText.substring(0, length) + "..." : originalText;
                ko.bindingHandlers.text.update(element, function () {
                    return truncatedText;
                });
            }
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
        self.IsInitialized = ko.observable(false);
        self.PagesListHtml = ko.observable("");
        self.PrevPageHtml = ko.observable("");
        self.NextPageHtml = ko.observable("");
        self.activeReviewList = ko.observableArray(helpfulReviews);
        self.activeReviewCategory = ko.observable(0);
        self.reviewsAvailable = ko.observable(true);
        self.categoryName = ko.observable('');
        self.IsLoading = ko.observable(false);
        self.Filters = ko.observable({ pn: 1, ps: 8, model: modelid, so: reviewId > 0 ? 1 : 2, skipreviewid: reviewId });
        self.firstReadMoreClick = ko.observable(false);
        self.clickedReviewId = ko.observable();

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
                        resetCollapsibleContent();
                        applyLikeDislikes();

                        if (self.firstReadMoreClick()) {
                            var collpasibleContent = $(document).find('.read-more-target[data-reviewId=' + self.clickedReviewId() + ']').closest('.collapsible-content');
                            $('html, body').scrollTop(collpasibleContent.closest('.list-item').offset() ?(collpasibleContent.closest('.list-item').offset().top - $('#overallSpecsTab').height()) : "0");
                            collpasibleContent.addClass('active');
                            self.firstReadMoreClick(false);
                        }
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
                    resetCollapsibleContent();
                    applyLikeDislikes();

                    if (self.firstReadMoreClick()) {
                        var collpasibleContent = $(document).find('.read-more-target[data-reviewId=' + self.clickedReviewId() + ']').closest('.collapsible-content');
                        $('html, body').scrollTop(collpasibleContent.closest('.list-item').offset().top - $('#overallSpecsTab').height());
                        collpasibleContent.addClass('active');
                        self.firstReadMoreClick(false);
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

        self.readMore = function (event) {
            if (!self.activeReviewList().length && modelid) {
                self.getUserReviews();

            }

            if ($('#modelReviewsListing')) {
                $('#modelReviewsListing').attr('data-readMoreCount', parseInt($('#modelReviewsListing').attr('data-readMoreCount')) + 1);
            }

            if ($('#modelReviewsListing').attr('data-readMoreCount') == (parseInt(reviewsReadPerSession) - 1)) {
                $.ajax({
                    type: "GET",
                    url: "/m/expertreviews/list/?makeId=" + makeid + "&modelId=" + modelid + "&topCount=12",
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
                $('.expert-review-list .swiper-container:not(".noSwiper")').each(function (index, element) {
                    $(this).addClass('sw-' + index);
                    $('.sw-' + index).swiper({
                        effect: 'slide',
                        speed: 300,
                        nextButton: $(this).find('.swiper-button-next'),
                        prevButton: $(this).find('.swiper-button-prev'),
                        pagination: $(this).find('.swiper-pagination'),
                        slidesPerView: 'auto',
                        paginationClickable: true,
                        spaceBetween: 10,
                        preloadImages: false,
                        lazyLoading: true,
                        lazyLoadingInPrevNext: true,
                        watchSlidesProgress: true,
                        watchSlidesVisibility: true

                    });


                })
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
            $('.swiper-image-preview img.lazy').lazyload();

            if (expertReviewWidgetHtml && expertReviewWidgetHtml.trim()) {
                triggerGA("User_Reviews", "ExpertReviews_CarouselLoaded", makeName + "_" + modelName);
            }
        };
    }

    vmUserReviews = new modelUserReviews();

    $(document).on("click", ".read-more-target", function (e) {
        if (!vmUserReviews.IsInitialized()) {
            vmUserReviews.clickedReviewId($(this).attr('data-reviewId'));
            vmUserReviews.firstReadMoreClick(true);
            vmUserReviews.init(e);
            vmUserReviews.readMore(e);
        }
    });

    $(document).on("click", ".expert-review-list .swiper-card a", function (e) {
        triggerGA("User_Reviews", "ExpertReviews_CarouselCard_Clicked", makeName + "_" + modelName);
    });

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

    var modelExpertReviewsList = function () {
        var self = this;
        self.IsInitialized = ko.observable(false);
        self.expertReviewsList = ko.observableArray();

        self.init = function () {
            if (!self.IsInitialized()) {

                self.IsInitialized(true);
                ko.applyBindings(self, $(".article-list")[0]);

                self.fetchExpertReviews();
            }
        };

        self.fetchExpertReviews = function () {
            $.ajax({
                type: "GET",
                url: "/api/cms/cat/V2/8/posts/12/make/" + makeid + "/?modelId=" + modelid,
                success: function (response) {
                    self.expertReviewsList(JSON.parse(response));
                }
            });
        };
    }

    vmModelExpertReviewsList = new modelExpertReviewsList();

    var modelPopularBikes = function () {
        var self = this;
        self.bikeList = ko.observableArray();
        self.showCheckOnRoadCTA = ko.observable(false);
        self.showPriceInCityCTA = ko.observable(false);
        self.pageCatId = ko.observable();
        self.pqSourceId = ko.observable();
        self.IsInitialized = ko.observable(false);
        self.IsLoading = ko.observable(true);

        self.init = function () {
            if (!self.IsInitialized()) {
                self.IsInitialized(true);
                ko.applyBindings(self, $("#popular-bikes-widget")[0]);
                self.fetchPopularBikes();
            }
        };

        self.fetchPopularBikes = function () {
            $.ajax({
                type: "GET",
                url: "/api/popularbikesbymake/" + makeid + "/12/?cityId=" + globalCityId,
                success: function (response) {
                    self.bikeList(JSON.parse(response));
                    $('#popular-bikes-widget .swiper-container:not(".noSwiper")').each(function (index, element) {
                        $(this).addClass('sw-' + index);
                        $('.sw-' + index).swiper({
                            effect: 'slide',
                            speed: 300,
                            nextButton: $(this).find('.swiper-button-next'),
                            prevButton: $(this).find('.swiper-button-prev'),
                            pagination: $(this).find('.swiper-pagination'),
                            slidesPerView: 'auto',
                            spaceBetween: 10,
                            preloadImages: false,
                            lazyLoading: true,
                            lazyLoadingInPrevNext: true,
                            watchSlidesProgress: true,
                            watchSlidesVisibility: true,
                            onInit: function (swiper) {
                                self.IsLoading(false);
                            }
                        });
                    })
                }
            });
        };
    }

    vmModelPopularBikes = new modelPopularBikes();

    var modelPopularBikesBodyStyle = function () {
        var self = this;
        self.bikeList = ko.observableArray();
        self.showCheckOnRoadCTA = ko.observable(false);
        self.showPriceInCityCTA = ko.observable(false);
        self.pageCatId = ko.observable();
        self.pqSourceId = ko.observable();
        self.IsInitialized = ko.observable(false);
        self.IsLoading = ko.observable(true);

        self.init = function () {
            if (!self.IsInitialized()) {
                self.IsInitialized(true);
                ko.applyBindings(self, $("#popular-bodystyle-bikes-list")[0]);
                self.fetchPopularBikesByBodyStyle();
            }
        };

        self.fetchPopularBikesByBodyStyle = function () {
            $.ajax({
                type: "GET",
                url: "/api/popularbikesbybodystyle/" + modelid + "/12/?cityId=" + globalCityId,
                success: function (response) {
                    self.bikeList(JSON.parse(response));
                    $('#popular-bodystyle-bikes-list .swiper-container:not(".noSwiper")').each(function (index, element) {
                        $(this).addClass('sw-' + index);
                        $('.sw-' + index).swiper({
                            effect: 'slide',
                            speed: 300,
                            nextButton: $(this).find('.swiper-button-next'),
                            prevButton: $(this).find('.swiper-button-prev'),
                            pagination: $(this).find('.swiper-pagination'),
                            slidesPerView: 'auto',
                            spaceBetween: 10,
                            preloadImages: false,
                            lazyLoading: true,
                            lazyLoadingInPrevNext: true,
                            watchSlidesProgress: true,
                            watchSlidesVisibility: true,
                            onInit: function (swiper) {
                                self.IsLoading(false);
                            }
                        });
                    })
                }
            });
        };
    }

    vmModelPopularBikesBodyStyle = new modelPopularBikesBodyStyle();

    var expertReviewsTemplate = bwcache.get("ExpertReviewsListTemplate")
    if (expertReviewsTemplate) {
        $("#expert-review-ul").html(expertReviewsTemplate);
        vmModelExpertReviewsList.init();
    }
    else {
        $("#expert-review-ul").load("/UI/Templates/ExpertReviewsList_Mobile.html", function (responseTxt, statusTxt, xhr) {
            if (statusTxt == "success") {
                bwcache.set("ExpertReviewsListTemplate", responseTxt, true);
                vmModelExpertReviewsList.init();
            }
        });
    }

    $(".tabs-type-switch").click(function (e) {

        if (e.target.getAttribute('data-tabs') == 'expertReviewContent') {
            triggerGA("User_Reviews", "Clicked_Toggle_ExpertReviews", makeName + "_" + modelName);           
            var bikeReviewsListTemplate = bwcache.get("BikeReviewsListTemplate")
            if (bikeReviewsListTemplate) {
                $("#popular-bikes-widget").html(BikeReviewsListTemplate);
                vmModelExpertReviewsList.init();
                $("#popular-bodystyle-bikes-list").html(BikeReviewsListTemplate);
                vmModelPopularBikesBodyStyle.init();
            }
            else {
                if (vmModelPopularBikes.bikeList().length == 0) {
                    $("#popular-bikes-widget").load("/UI/Templates/BikesSwiperList_Mobile.html", function (responseTxt, statusTxt, xhr) {
                        if (statusTxt == "success") {
                            bwcache.set("BikeReviewsListTemplate", responseTxt, true);
                            $("#popular-bodystyle-bikes-list").html(responseTxt);
                            vmModelPopularBikes.init();
                            vmModelPopularBikesBodyStyle.init();
                        }
                    });
                }
            }
        }

        if (e.target.getAttribute('data-tabs') == 'userReviewContent') {
            triggerGA("User_Reviews", "Clicked_Toggle_UserReviews", makeName + "_" + modelName);           
        }
    });

    if (parseInt(expertReviewCount) > 0) {
        triggerNonInteractiveGA("User_Reviews", "Toggle_Appeared_on_PageLoad", makeName + "_" + modelName);
    }

    $(document).on("click", ".article-list", function (e) {
        triggerGA("User_Reviews", "Clicked_ExpertReviews_List", makeName + "_" + modelName);
    });

    var similarBikesSwiper = new Swiper('#similar-bikes-swiper', {
        effect: 'slide',
        speed: 300,
        slidesPerView: 'auto',
        spaceBetween: 10,
        preloadImages: false,
        lazyLoading: true,
        lazyLoadingInPrevNext: true,
        watchSlidesProgress: true,
        watchSlidesVisibility: true,
        onSlideChangeStart: function (swiper, event) {
            triggerGA("User_Reviews", "Clicked_On_SimilarBikes_Carousel", makeName + "_" + modelName + "_" + (reviewId > 0 ? "Details" : "List"));
        }
    });
});