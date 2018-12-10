var scrollTitleDict = [];
var articlesParams = [];
var titleIndex = 1;
var Content = {
    expertReview: {
        relatedArticleCount: 0,
        scrollCallback: function() {
            if (relatedArticles.length > Content.expertReview.relatedArticleCount) {
                if (Content.expertReview.relatedArticleCount > 0) {
                    var pageUrl = '/m' + $('[data-page-url]:last').data('page-url');
                    Common.utils.firePageView(pageUrl);
                    //reinitialize image swiper
                    Content.swiper.registerSwiperCarousal('.jscroll-added:last-child .image-swiper:not(".noSwiper")');
                    Swiper.Initialize();
                    Content.expertReview.initializeIframe();
                    ++titleIndex;
                    if (Content.expertReview.relatedArticleCount > 1) {
                        var x = document.querySelectorAll(".jscroll-added");
                        scrollTitleDict.push({
                            key: (scrollTitleDict[titleIndex - 2].key + x[Content.expertReview.relatedArticleCount - 2].offsetHeight),
                            value: titleIndex - 1
                        });
                    } else {     
                        scrollTitleDict.push({
                            key: $('.expert-review-details').height() - $(window).height(),
                            value: titleIndex - 1
                        });
                    }
                    articlesParams.push(new Object({
                        Title: articleTitle,
                        Url: pageUrl
                    }));
                } // track values assigned in previous callback
                if (Content.expertReview.relatedArticleCount + 1 < relatedArticles.length) {
                    var categoryId = relatedArticles[Content.expertReview.relatedArticleCount + 1].categoryId;
                    var basicId = relatedArticles[Content.expertReview.relatedArticleCount + 1].basicId;
                    var url = window.location.origin + "/m";
                    switch (categoryId) {
                        case 1:
                            url += "/newsdetail/" + basicId + "/?isRelatedArticle=true";
                            break;
                        case 2:
                        case 8:
                            url += "/expertreviewdetails/" + basicId + "/?isRelatedArticle=true";
                            break;
                        case 13:
                            url += "/videosdetails/?basicId=" + basicId;
                    }
                    $('a[data-next]:last').attr('href', url);
                }
                Content.expertReview.relatedArticleCount++;
            };
            $('img.lazy').lazyload();
        },
        initialize: function() {
            scrollTitleDict.push({
                key: 0,
                value: 0
            });

            articlesParams.push(new Object({
                Title: document.title,
                Url: window.location.pathname,
            }));
            //add default article(root article) at 0th position of related article
            relatedArticles.unshift(
                new Object({
                    categoryId: defaultCatId,
                    categoryMaskingName: defaultCatMaskingName,
                    basicId: defaultBasicId
                })
            );
        },
        initializeIframe: function() {
            var element = $('.swiper-wrapper:last iframe');
            if (element.length > 0) {
                onYouTubeIframeReady(element.eq(0).data('id'), true);
                Content.expertReview.setStateChangeCallBack();
            }

        },
        setStateChangeCallBack: function() {
            try {
                player.addEventListener('onStateChange', Content.expertReview.fireVideoPageView);
            } catch (e) {
                setTimeout(function() { Content.expertReview.setStateChangeCallBack(); }, 300);
            }
        },
        fireVideoPageView: function(event) {
            switch (event.data) {
                case YT.PlayerState.PLAYING:
                    Common.utils.firePageView(event.target.a.getAttribute("data-video-url"));
                    Common.utils.trackAction('CWInteractive', 'msite_expert_review_detail', 'video', event.target.a.getAttribute("data-car-name"));
                    break;
                case YT.PlayerState.ENDED:
                case YT.PlayerState.PAUSED:
                    Common.utils.firePageView(window.location.pathname);
                    break;
            }
        }
    },
    swiper: {
        registerSwiperCarousal: function(element) {
            var currentSwiper = $(element);
            if (currentSwiper.length > 0) {
                var swiper = currentSwiper.swiper({
                    nextButton: currentSwiper.find('.swiper-button-next'),
                    prevButton: currentSwiper.find('.swiper-button-prev'),
                    slidesPerView: 1,
                    paginationClickable: true,
                    spaceBetween: 10,
                    watchSlidesVisibility: true,
                    lazyLoading: true,
                    lazyLoadingInPrevNext: true,
                    lazyLoadingInPrevNextAmount: 2,
                });
                swiper.on('onSlideChangeEnd', function() {
                    Common.utils.trackAction('CWInteractive', 'msite_image_gallery', 'expert_reviews_gallery', swiper.activeIndex.toString());
                });
            }
        }
    }
};

$(document).ready(function() {
    Content.expertReview.initialize()
    Content.expertReview.scrollCallback();
    Content.swiper.registerSwiperCarousal('.image-swiper:not(".noSwiper")');
    Content.expertReview.initializeIframe();
    $('.scroll').jscroll({
        autoTrigger: true,
        autoTriggerUntil: 10,
        callback: Content.expertReview.scrollCallback,
        nextSelector: "a[data-next]:last",
        loadingHtml: "<div class='text-center margin-top5'><img src='https://imgd.aeplcdn.com/0x0/cw/static/circle-loader.gif'/></div>",
        container: '.scroll-container'
    });
    $("#divDesc img").addClass("imgWidth").addClass("margin-bottom10");
    $(window).scroll(function() {
        var scrollTop = $(this).scrollTop();
        if (scrollTitleDict.length > 1) {
            Common.utils.setUrlTitleOnScroll(scrollTop, scrollTitleDict, articlesParams);
        }
    });
});

function VideoClickGATrack(carname) {
    dataLayer.push({ event: 'CWInteractive', cat: 'msite_video_linkages', act: 'expert_review_carousel', lab: carname });
}