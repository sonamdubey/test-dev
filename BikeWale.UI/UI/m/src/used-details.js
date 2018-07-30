$(document).ready(function () {

    var $window = $(window),
        overallSpecsTabsContainer = $('.overall-specs-tabs-container'),
        modelBottomCard = $('#model-bottom-card-wrapper'),
        modelSpecsFooter = $('#modelSpecsFooter'),
        topNavBarHeight = overallSpecsTabsContainer.height();

    var tabsLength = $('.overall-specs-tabs-wrapper li').length - 1;
    if (tabsLength < 3) {
        $('.overall-specs-tabs-wrapper li').css({ 'display': 'inline-block', 'width': 'auto' });
    }

    $('.overall-specs-tabs-wrapper li').first().addClass('active');
    var bodHt, footerHt, scrollPosition;

    $(window).scroll(function () {
        var windowScrollTop = $window.scrollTop(),
            modelSpecsTabsOffsetTop = modelBottomCard.offset().top,
            modelSpecsFooterOffsetTop = modelSpecsFooter.offset().top;

        if (windowScrollTop > modelSpecsTabsOffsetTop) {
            overallSpecsTabsContainer.addClass('fixed-tab-nav');
        }

        else if (windowScrollTop < modelSpecsTabsOffsetTop) {
            overallSpecsTabsContainer.removeClass('fixed-tab-nav');
        }

        if (overallSpecsTabsContainer.hasClass('fixed-tab-nav')) {
            if (windowScrollTop > modelSpecsFooterOffsetTop - topNavBarHeight) {
                overallSpecsTabsContainer.removeClass('fixed-tab-nav');
            }
        }


        $('#model-bottom-card-wrapper .bw-model-tabs-data').each(function () {
            var top = $(this).offset().top - overallSpecsTabsContainer.height(),
                bottom = top + $(this).outerHeight();
            if (windowScrollTop >= top && windowScrollTop <= bottom) {
                overallSpecsTabsContainer.find('li').removeClass('active');
                $('#model-bottom-card-wrapper .bw-mode-tabs-data').removeClass('active');

                $(this).addClass('active');

                var currentActiveTab = overallSpecsTabsContainer.find('li[data-tabs="#' + $(this).attr('id') + '"]');
                overallSpecsTabsContainer.find(currentActiveTab).addClass('active');

            }
        });

        var scrollToTab = $('#model-bottom-card-wrapper .bw-model-tabs-data:eq(3)');
        if (scrollToTab.length != 0) {
            if (windowScrollTop > scrollToTab.offset().top - 45) {
                if (!$('#overall-specs-tab').hasClass('scrolled-left')) {
                    $('.overall-specs-tabs-container').addClass('scrolled-left');
                    scrollHorizontal(400);
                }
            }

            else if (windowScrollTop < scrollToTab.offset().top) {
                if ($('#overall-specs-tab').hasClass('scrolled-left')) {
                    $('.overall-specs-tabs-container').removeClass('scrolled-left');
                    scrollHorizontal(0);
                }
            }
        }
        bodHt = $('body').height();
        footerHt = $('footer').height();
        scrollPosition = $(this).scrollTop();
        if (scrollPosition + $(window).height() > (bodHt - footerHt))
            $('.float-button').hide().removeClass('float-fixed');
        else
            $('.float-button').show().addClass('float-fixed');

    });

    function scrollHorizontal(pos) {
        $('#overall-specs-tab').animate({ scrollLeft: pos + 'px' }, 500);
    }

    $('.overall-specs-tabs-wrapper li').click(function () {
        var target = $(this).attr('data-tabs');
        $('html, body').animate({ scrollTop: $(target).offset().top - overallSpecsTabsContainer.height() }, 1000);
        centerItVariableWidth($(this), '.overall-specs-tabs-container');
        return false;
    });

    function centerItVariableWidth(target, outer) {
        var out = $(outer);
        var tar = target;
        var x = out.width();
        var y = tar.outerWidth(true);
        var z = tar.index();
        var q = 0;
        var m = out.find('li');
        for (var i = 0; i < z; i++) {
            q += $(m[i]).outerWidth(true);
        }
        out.animate({ scrollLeft: Math.max(0, q - (x - y) / 2) }, 500, 'swing');
    }

    $('.carousel-navigation-photos .swiper-slide').first().addClass('swiper-slide-active');

});


$('#model-main-image').on('click', '.model-gallery-target', function () {
    $('#model-gallery-container').show();

    var slideToClick = function (swiper) {
        var clickedSlide = swiper.slides[swiper.clickedIndex];
        $('.carousel-navigation-photos .swiper-slide').removeClass('swiper-slide-active');
        $(clickedSlide).addClass('swiper-slide-active');
        galleryTop.slideTo(swiper.clickedIndex, 500);
    };

    var galleryThumbs = new Swiper('.carousel-navigation-photos', {
        slideActiveClass: '',
        spaceBetween: 0,
        slidesPerView: 'auto',
        slideToClickedSlide: true,
        preloadImages: false,
        lazyLoading: true,
        lazyLoadingInPrevNext: true,
        watchSlidesProgress: true,
        watchSlidesVisibility: true,
        onTap: slideToClick
    });

    var slidegalleryThumbs = function (swiper) {
        galleryThumbs.slideTo(swiper.activeIndex, 500);
        galleryThumbs.slides.removeClass('swiper-slide-active');
        galleryThumbs.slides[swiper.activeIndex].className += ' swiper-slide-active';

        showImgTitle(galleryTop);
    };

    var galleryTop = new Swiper('.carousel-stage-photos', {
        nextButton: '.swiper-button-next',
        prevButton: '.swiper-button-prev',
        spaceBetween: 10,
        preloadImages: false,
        lazyLoading: true,
        lazyLoadingInPrevNext: true,
        watchSlidesProgress: true,
        watchSlidesVisibility: true,
        onSlideChangeEnd: slidegalleryThumbs
    });

    var currentStagePhoto, currentStageActiveImage;
    function showImgTitle(swiper) {
        imgTitle = $(galleryThumbs.slides[swiper.activeIndex]).find('img').attr('title');
        imgTotalCount = galleryThumbs.slides.length;
        $(".media-title").text(imgTitle);
        $(".gallery-count").text(swiper.activeIndex + 1 + " of " + imgTotalCount.toString());
        currentStagePhoto = $(".connected-carousels-photos .stage-photos");
        currentStageActiveImage = currentStagePhoto.find(".swiper-slide.swiper-slide-active img");
        currentStagePhoto.find('.carousel-stage-photos').css({ 'height': currentStageActiveImage.height() });
    }

    showImgTitle(galleryTop);
});

$('#model-gallery-container').on('click', '.gallery-close-btn', function () {
    $('#model-gallery-container').hide();
    $('.carousel-navigation-photos .swiper-slide').removeClass('swiper-slide-active');
    $('.carousel-navigation-photos .swiper-slide').first().addClass('swiper-slide-active')
});