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

    $(".jcarousel-control-left, .jcarousel-control-right").on('click', function () {
        if ($(this).find(".jcarousel-control-next").hasClass('inactive')) {
            $(this).closest('.jcarousel-wrapper').attr('data-overlay', 'left');
        }
        else if ($(this).find(".jcarousel-control-prev").hasClass('inactive')) {
            $(this).closest('.jcarousel-wrapper').attr('data-overlay', 'right');
        }
        else {
            $(this).closest('.jcarousel-wrapper').attr('data-overlay', 'right');
        }
    });
    //find bike carousel

    $('.find-specs-tabs-wrapper .specs-btn').on('click', function () {

        var specBtn = $(this).attr('data-swipe');
        var swiperSlide = $('#rightBikeSwiper').find('.rightbike__swiper-card[data-model-index="' + specBtn + '"]');
        var indexOfSlide = swiperSlide.closest('li').index();
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
        overallSpecsTabsContainer = $('.overall-specs-tabs-container'),
        seriesTabsContentWrapper = $('#seriesTabsContentWrapper');
    overallTabs.find('.overall-specs-tabs-wrapper > .navtab:first-child').addClass('active');



    $(window).scroll(function () {
        var windowScrollTop = $(window).scrollTop(),
            overallTabsOffsetTop = overallTabs.offset().top,
            overallWrapperHeight = seriesTabsContentWrapper.outerHeight() + overallTabsOffsetTop;

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
				overallSpecsTabsContainer.find('.navtab[data-href="' + $(this).attr('data-id') + '"]').addClass('active');
            }
        });

    });
    $(".navtab").click(function (event) {
        event.preventDefault();
        try {
            var scrollSectionId = $(this).data('href');
            $('html,body').animate({
                scrollTop: $(scrollSectionId).offset().top - 40
            },
          'slow');
            triggerGA('Series_Page', 'Floating_Navigation_Clicked', $(this).data("lab"));
        }
        catch (e) {
            console.log(e);
        }
    });
});