
docReady(function () {

    $('.find-specs-tabs-wrapper .specs-btn').on('click', function () {

        var specBtn = $(this).attr('data-swipe');
        var swiperSlide = $('#rightBikeSwiper').find('.swiper-slide[data-model-index="' + specBtn + '"]');
        var indexOfSlide = swiperSlide.index();

        if (indexOfSlide >= 0)
        {
            var tag = $('#rightBikeSwiper').find('.swiper-slide .recommended-tag:not(.popular-tag)');

            tag.text('');

            if (! swiperSlide.hasClass('popular-card'))
            {      
                var recommendedTag = swiperSlide.find('.recommended-tag');
                    recommendedTag.text('Recommended Bike');
            }
            $('.right-bike-swiper.swiper-container').data('swiper').slideTo(indexOfSlide, 1000, false);
        }

    });

    //collapsible content

    $('.foldable-content .read-more-button').on('click', function () {
        var readMoreButton = $(this);
        var collapsibleContent = readMoreButton.closest('.foldable-content');
        var isDataToggle = collapsibleContent.attr('data-toggle');
        var dataTruncate = collapsibleContent.find('.truncatable-content');
        var dataLessText;
        var readLessText;
        switch (isDataToggle) {
            case 'no':
                dataTruncate.attr('data-readtextflag', '0');
                readMoreButton.hide();
                break;
            case 'yes':
                dataLessText = readMoreButton.attr('data-text');
                readLessText = !dataLessText || dataLessText.length === 0 ? 'Collapse' : dataLessText;
                dataTruncate.attr('data-readtextflag', '0');
                readMoreButton.attr('data-text', readMoreButton.text()).text(readLessText);
                collapsibleContent.attr('data-toggle', 'hide');
                break;
            case 'hide':
                dataTruncate.attr('data-readtextflag', '1');
                dataLessText = readMoreButton.attr('data-text');
                readMoreButton.attr('data-text', readMoreButton.text()).text(dataLessText);
                collapsibleContent.attr('data-toggle', 'yes');
                break;
        }
    });
	var overallSpecsTabsContainer = $('.overall-specs-tabs-container'),
	modelSpecsTabsContentWrapper = $('.overall-specs-tabs-wrapper'),
	modelSpecsFooter = $('#overallSpecsFooter'),
	topNavBarHeight = $('.overall-specs__top-content').height();
    $(window).scroll(function () {
        var windowScrollTop = $(window).scrollTop(),
            modelSpecsTabsOffsetTop = modelSpecsTabsContentWrapper.offset().top,
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
		
        $('#modelSpecsTabsContentWrapper .bw-model-tabs-data').each(function () {
            var top = $(this).offset().top - topNavBarHeight,
                bottom = top + $(this).outerHeight();
            if (windowScrollTop >= top && windowScrollTop <= bottom) {
                overallSpecsTabsContainer.find('li').removeClass('active');
                $('#modelSpecsTabsContentWrapper .bw-model-tabs-data').removeClass('active');

                $(this).addClass('active');

                var currentActiveTab = overallSpecsTabsContainer.find('li[data-tabs="#' + $(this).attr('id') + '"]');
                overallSpecsTabsContainer.find(currentActiveTab).addClass('active');
            }
        });

		
	});
    // $('.right-bike-swiper.swiper-container').on('inview', function (event, visible) {
        // if (visible == true) {
            // $('.swiper-ribbon').addClass('animate-ribbon');
        // } else {

        // }
    // });
});
