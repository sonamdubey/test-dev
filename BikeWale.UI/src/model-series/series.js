docReady(function () {
    $('a.read-more-bike-preview').click(function () {
        if (!$(this).hasClass('open')) {
            $('.preview-main-content').hide();
            $('.preview-more-content').show();
            $(this).text($(this).text() === 'Read more' ? 'Collapse' : 'Read more');
            $(this).addClass("open");
        }
        else if ($(this).hasClass('open')) {
            $('.preview-main-content').show();
            $('.preview-more-content').hide();
            $(this).text($(this).text() === 'Read more' ? 'Collapse' : 'Read more');
            $(this).removeClass('open');
        }
    });

    $('.upcoming-bike-carousel').jcarousel();

    $('.upcoming-bike-carousel .jcarousel-control-prev')
        .on('jcarouselcontrol:active', function () {
            $(this).removeClass('inactive');
        })
        .on('jcarouselcontrol:inactive', function () {
            $(this).addClass('inactive');
        })
        .jcarouselControl({
            target: '-=1'
        });

    $('.upcoming-bike-carousel .jcarousel-control-next')
        .on('jcarouselcontrol:active', function () {
            $(this).removeClass('inactive');
        })
        .on('jcarouselcontrol:inactive', function () {
            $(this).addClass('inactive');
        })
        .jcarouselControl({
            target: '+=1'
        });

    $('.comparison-carousel .jcarousel-control-prev')

    .jcarouselControl({
        target: '-=2'
    });
    $('.comparison-carousel .jcarousel-control-next')

    .jcarouselControl({
        target: '+=2'
    });

    $('.used-model-swiper .jcarousel-control-next')
        .jcarouselControl({
            target: '+=2'
        });


    $('.used-model-swiper .jcarousel-control-prev')

    .jcarouselControl({
        target: '-=2'
    });

    $('.used-model-swiper .jcarousel-control-next')
        .jcarouselControl({
            target: '+=2'
        });


    $('.model-list-carousel .jcarousel-control-prev')

    .jcarouselControl({
        target: '-=2'
    });

    $('.model-list-carousel .jcarousel-control-next')
        .jcarouselControl({
            target: '+=2'
        });


    $(".jcarousel-control-next").on("jcarouselcontrol:active", function () {
        $(this).closest('.jcarousel-wrapper').attr('data-overlay', 'both');
    })
    $(".jcarousel-control-prev").on("jcarouselcontrol:active", function () {
        $(this).closest('.jcarousel-wrapper').attr('data-overlay', 'both');
    })
    $(".jcarousel-control-next").on("jcarouselcontrol:inactive", function () {
        $(this).closest('.jcarousel-wrapper').attr('data-overlay', 'left');
    })

    $(".jcarousel-control-prev").on("jcarouselcontrol:inactive", function () {
        $(this).closest('.jcarousel-wrapper').attr('data-overlay', 'right');
    })
    //find bike carousel

    $('.find-specs-tabs-wrapper .specs-btn').on('click', function () {

        var specBtn = $(this).attr('data-swipe');
        var swiperSlide = $('#rightBikeSwiper').find('.rightbike__swiper-card[data-model-index="' + specBtn + '"]');
        var indexOfSlide = swiperSlide.index();
        $(this).closest('.find-specs-tabs-wrapper').find('.active').removeClass('active');
        $(this).addClass('active');
        if (indexOfSlide >= 0) {
            var tag = $('#rightBikeSwiper').find('.rightbike__swiper-card .recommended-tag:not(.popular-tag)');
            var popTag = $('#rightBikeSwiper').find('.rightbike__swiper-card .recommended-tag.popular-tag');
            tag.text('');
            var recommendedTag = swiperSlide.find('.recommended-tag');

            if (recommendedTag.hasClass('popular-tag')) {
                recommendedTag.text('Recommended Bike');
            }
            else {
                
                recommendedTag.text('Recommended Bike');
                popTag.text('Most Popular');
            }
            $('.right-bike-swiper .jcarousel').jcarousel('scroll', indexOfSlide);
            $('#rightBikeSwiper').find('.rightbike__swiper-card.featured-card').removeClass('featured-card');
            swiperSlide.addClass('featured-card');
        }

    });

    var overallTabs = $('.overall-tabs'),
        overallTabsOffsetTop = overallTabs.offset().top,
        overallSpecsTabsContainer = $('.overall-specs-tabs-container'),
        seriesTabsContentWrapper = $('#seriesTabsContentWrapper'),
        overallWrapperHeight = seriesTabsContentWrapper.outerHeight() + overallTabsOffsetTop
    overallTabs.find('.overall-specs-tabs-wrapper > .navtab:first-child').addClass('active');



    $(window).scroll(function () {
        var windowScrollTop = $(window).scrollTop();

        if (windowScrollTop > overallTabsOffsetTop) {
            overallSpecsTabsContainer.addClass('fixed-tab-nav');
        }
        else if (windowScrollTop < overallTabsOffsetTop) {
            overallSpecsTabsContainer.removeClass('fixed-tab-nav');
        }
        if (windowScrollTop > seriesTabsContentWrapper.outerHeight()) {
            overallSpecsTabsContainer.removeClass('fixed-tab-nav');
        }

        $('#seriesTabsContentWrapper .bw-model-tabs-data').each(function () {
            var top = $(this).offset().top - overallSpecsTabsContainer.height(),
            bottom = top + $(this).outerHeight();
            if (windowScrollTop >= top && windowScrollTop <= bottom) {
                overallSpecsTabsContainer.find('.navtab').removeClass('active');
                $('#seriesTabsContentWrapper .bw-mode-tabs-data').removeClass('active');

                $(this).addClass('active');
                overallSpecsTabsContainer.find('a[data-href="' + $(this).attr('data-id') + '"]').addClass('active');
            }
        });

    });
    $(".navtab").click(function () {

        try {
            var scrollSectionId = $(this).data('href');
            $('html,body').animate({
                scrollTop: $(scrollSectionId).offset().top - 40
            },
          'slow');
            triggerGA('Make_Page', 'Floating_Navigation_Clicked', $(this).data("lab"));
        }
        catch (e) {
            console.log(e);
        }
    });
});