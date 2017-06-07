var vmUserReviews, modelReviewsSection, modelid;

var helpfulReviews = [];

var reviewCategory = {
    2: 'helpful',
    1: 'recent',
    5: 'positive',
    6: 'negative',
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


function downVoteReview() {
    bwcache.set("ReviewDetailPage_reviewVote_" + reviewId, { "vote": "0" });
    $('#downvoteBtn').addClass('active');
    $('#downvoteTxt').text("Disliked");
    $('#upvoteBtn').attr('disabled', 'disabled');
    $('#downvoteCount').text(parseInt($('#downvoteCount').text()) + 1);
    voteUserReview(0);
}


function voteUserReview(vote) {
    $.ajax({
        type: "POST",
        url: "/api/user-reviews/voteUserReview/?reviewId=" + reviewId + "&vote=" + vote,
        success: function (response) {
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

    if (!isError) {
        var commentsForAbuse = $("#txtAbuseComments").val().trim();

        $.ajax({
            type: "POST",
            url: "/api/user-reviews/abuseUserReview/?reviewId=" + reviewId + "&comments=" + commentsForAbuse,
            success: function (response) {
                reportAbusePopup.close();
                document.getElementById("divReportAbuse").innerHTML = "Your request has been sent to the administrator.";
            }
        });
    }
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

docReady(function() {


    modelReviewsSection = $("#modelReviewsListing");
    bwcache.setOptions({ 'EnableEncryption': true });
    
    bwcache.removeAll(true);

    reviewId = $('#divReportAbuse').attr('data-reviewId');

   var vote = bwcache.get("ReviewDetailPage_reviewVote_" + reviewId);

    if (vote != null && vote.vote) {
        if (vote.vote == "0") {
            $('#downvoteBtn').addClass('active');
            $('#downvoteTxt').text("Downvoted");
            $('#upvoteBtn').attr('disabled', 'disabled');
        }
        else {
            $('#upvoteBtn').addClass('active');
            $('#upvoteTxt').text("Upvoted");
            $('#downvoteBtn').attr('disabled', 'disabled');
        }
    }

    if ($('#hdnModelId').length > 0)
        modelid = $('#hdnModelId').val();


    $('#bike-rating-box').find('.answer-star-list input[type=radio]').change(function () {
        var button = $(this),
           buttonValue = Number(button.val());
        var q = $('#rate-star-' + buttonValue).attr('data-querystring');
        window.location.href = "/rate-your-bike/" + modelid + "/?q=" + q;
    });

    var modelUserReviews = function () {
        var self = this;
        
        self.activeReviewList = ko.observableArray(helpfulReviews);
        self.activeReviewCategory = ko.observable(0);
        self.reviewsAvailable = ko.observable(true);
        self.categoryName = ko.observable('');
        
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

        resetCollapsibleContent();
    });

    function resetCollapsibleContent() {
        var activeCollapsible = $('.user-review-list').find('.collapsible-content.active');
        activeCollapsible.removeClass('active');
        activeCollapsible.find('.read-more-target').text('...Read more');
    }

    $('#btnReportClick').on('click', function() {
        reportAbusePopup.open();
    });

    $('#report-background, .report-abuse-close-btn').on('click', function() {
        reportAbusePopup.close();
    });

    $(document).keydown(function (event) {
        if(event.keyCode == 27) {
            if(reportAbusePopup.popupElement.is(':visible')) {
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
});