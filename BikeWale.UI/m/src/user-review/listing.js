var reviewId;

var helpfulReviews = [
    {
        rating: 5,
        title: "Best Ecomomical bike",
        description: "Get in touch with your nearest Bajaj service center in Pune for service enquiries, service and repair costs and more!",
        author: "Omkar Thakur",
        submittedOn: "Jan 03, 2017",
        likesCount: 52,
        disLikesCount: 52,
        viewsCount: 210
    },
    {
        rating: 3,
        title: "Perfect Mileage bike for daily office goers",
        description: "Get in touch with your nearest Bajaj service center in Pune for service enquiries, service and repair costs and more!",
        author: "Omkar Thakur",
        submittedOn: "Jan 03, 2017",
        likesCount: 25,
        disLikesCount: 25,
        viewsCount: 211
    },
    {
        rating: 4,
        title: "I am very happy with this bike",
        description: "Get in touch with your nearest Bajaj service center in Pune for service enquiries, service and repair costs and more!",
        author: "Omkar Thakur",
        submittedOn: "Jan 03, 2017",
        likesCount: 52,
        disLikesCount: 52,
        viewsCount: 212
    },
    {
        rating: 2,
        title: "Good bike less maintenance but less mileage",
        description: "Get in touch with your nearest Bajaj service center in Pune for service enquiries, service and repair costs and more!",
        author: "Omkar Thakur",
        submittedOn: "Jan 03, 2017",
        likesCount: 52,
        disLikesCount: 52,
        viewsCount: 213
    },
    {
        rating: 1,
        title: "Best Ecomomical bike",
        description: "Get in touch with your nearest Bajaj service center in Pune for service enquiries, service and repair costs and more!",
        author: "Omkar Thakur",
        submittedOn: "Jan 03, 2017",
        likesCount: 52,
        disLikesCount: 52,
        viewsCount: 214
    },
    {
        rating: 3,
        title: "Perfect Mileage bike for daily office goers",
        description: "Get in touch with your nearest Bajaj service center in Pune for service enquiries, service and repair costs and more!",
        author: "Omkar Thakur",
        submittedOn: "Jan 03, 2017",
        likesCount: 25,
        disLikesCount: 25,
        viewsCount: 215
    },
    {
        rating: 5,
        title: "Best Ecomomical bike",
        description: "Get in touch with your nearest Bajaj service center in Pune for service enquiries, service and repair costs and more!",
        author: "Omkar Thakur",
        submittedOn: "Jan 03, 2017",
        likesCount: 52,
        disLikesCount: 52,
        viewsCount: 216
    },
    {
        rating: 3,
        title: "Perfect Mileage bike for daily office goers",
        description: "Get in touch with your nearest Bajaj service center in Pune for service enquiries, service and repair costs and more!",
        author: "Omkar Thakur",
        submittedOn: "Jan 03, 2017",
        likesCount: 25,
        disLikesCount: 25,
        viewsCount: 217
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
var reportAbusePopup, appendState;

function abuseClick() {
    reportAbusePopup.open();
    appendState('reportPopup');
}

docReady(function() {    

    bwcache.setScope('ReviewDetailPage');    

    reviewId = $('#divAbuse').attr('data-reviewId');

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

    var modelUserReviews = function () {
        var self = this;

        self.activeReviewList = ko.observableArray(helpfulReviews);
        self.activeReviewCategory = ko.observable(0);

        self.reviewsAvailable = ko.observable(true);
        self.categoryName = ko.observable('');

        self.toggleReviewList = function (data, event) {
            self.tabEvents.toggleTab($(event.currentTarget));
            self.tabEvents.getReviews($(event.currentTarget));
        };

        self.tabEvents = {
            toggleTab: function (element) {
                if(!element.hasClass('active')) {
                    element.siblings().removeClass('active');
                    element.addClass('active');
                }
            },

            getReviews: function (element) {
                var categoryId = Number(element.attr('data-category')),
                    pageNumber = Number(element.attr('data-page-num')),
                    categoryCount = Number(element.attr('data-count'));
                
                /*
                if data is present in cache
                    self.activeReviewList(cache[data-id]);

                else ajax call
                    self.activeReviewList(result);
                    cache[data-id] = result;
                */

                if(categoryCount) {
                    self.reviewsAvailable(true);
                    self.activeReviewCategory(categoryId);
                    
                    element.attr('data-page-num', pageNumber + 1);
                }
                else {
                    self.tabEvents.setNoReview(categoryId);
                }
            },

            setNoReview: function (categoryId) {
                self.reviewsAvailable(false);
                self.categoryName(reviewCategory[categoryId]);
            }

        }
    }

    var vmModelUserReviews = new modelUserReviews(),
        reviewListingCard = document.getElementById('modelReviewsListing');

    if(reviewListingCard) {
        ko.applyBindings(vmModelUserReviews, reviewListingCard);
    }

    $window = $(window);
    overallSpecsTabsContainer = $('#overallSpecsTopContent');
    overallSpecsTab = $('#overallSpecsTab');
    specsFooter = $('#listingFooter');
    topNavBarHeight = overallSpecsTab.height();

    if(overallSpecsTabsContainer.length > 0) {
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
    
    function scrollToStart (listId, focusPoint, tab) {
        $('html, body').animate({ scrollTop: focusPoint.offset().top }, 1000);
        centerItVariableWidth(tab, listId + ' .overall-specs-tabs-container');
        return false;
    };

});

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
                document.getElementById("divAbuse").innerHTML = "Your request has been sent to the administrator.";
            }
        });
    }
}