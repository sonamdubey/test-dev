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

});