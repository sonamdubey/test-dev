
docReady(function () {
    //find bike carousel
    var specsTabsWrapper = $('.find-specs-tabs-wrapper');
    $('.find-specs-tabs-wrapper .specs-btn').on('click', function () {

        var specBtn = $(this).attr('data-swipe');
        var swiperSlide = $('#rightBikeSwiper').find('.rightbike__swiper-card[data-model-index="' + specBtn + '"]');
        var indexOfSlide = swiperSlide.closest('.swiper-slide').index();

        $(this).closest('.find-specs-tabs-wrapper').find('.active').removeClass('active');
        $(this).addClass('active');
        $('html, body').animate({ scrollTop: $('.find-right-bike').offset().top - 70 }, 1000);
        if (indexOfSlide >= 0)
        {
            var tag = $('#rightBikeSwiper').find('.rightbike__swiper-card .recommended-tag:not(.popular-tag)');
            var popTag = $('#rightBikeSwiper').find('.rightbike__swiper-card .popular-tag.recommended-tag');

            tag.text('');

            var recommendedTag = swiperSlide.find('.recommended-tag');

            if (recommendedTag.hasClass('popular-tag')) {
                recommendedTag.text('Recommended Bike');
            }
            else {

                recommendedTag.text('Recommended Bike');
                popTag.text('Most Popular');
            }
            $('.right-bike-swiper.swiper-container').data('swiper').slideTo(indexOfSlide, 1000, false);
			$('#rightBikeSwiper').find('.rightbike__swiper-card.featured-card').removeClass('featured-card');
			swiperSlide.addClass('featured-card');
			centerItVariableWidth($(this).closest('li'), '.find-specs__tabs-container');

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
	modelSpecsTabsContentWrapper = $('#modelLatestUpdatesWrapper'),
	modelSpecsFooter = $('#overallSpecsFooter');
	function scrollHorizontal(pos) {
		$('#overallUpdatesTab').animate({ scrollLeft: pos - 15 + 'px' }, 500);
}
	$(window).scroll(function () {
        var windowScrollTop = $(window).scrollTop(),
            modelSpecsTabsOffsetTop = modelSpecsTabsContentWrapper.offset().top,
            modelSpecsFooterOffsetTop = modelSpecsFooter.offset().top,
            topNavBarHeight = $('.overall-specs__top-content').height();

        if (windowScrollTop > modelSpecsTabsOffsetTop) {
            overallSpecsTabsContainer.addClass('fixed-tab-nav');
        }

        else if (windowScrollTop < modelSpecsTabsOffsetTop) {
            overallSpecsTabsContainer.removeClass('fixed-tab-nav');
        }

        if (overallSpecsTabsContainer.hasClass('fixed-tab-nav')) {
            if (windowScrollTop > Math.ceil(modelSpecsFooterOffsetTop) - (topNavBarHeight)) {
                overallSpecsTabsContainer.removeClass('fixed-tab-nav');
            }
        }
		
        $('#modelLatestUpdatesWrapper .bw-model-tabs-data').each(function () {
            var top = $(this).offset().top - topNavBarHeight,
                bottom = top + $(this).outerHeight();
            if (windowScrollTop >= top && windowScrollTop <= bottom) {
                overallSpecsTabsContainer.find('li').removeClass('active');
                $('#modelLatestUpdatesWrapper .bw-model-tabs-data').removeClass('active');

                $(this).addClass('active');

                var currentActiveTab = overallSpecsTabsContainer.find('li[data-tabs="#' + $(this).attr('id') + '"]');
                overallSpecsTabsContainer.find(currentActiveTab).addClass('active');
            }
		});
		var tabElementThird = modelSpecsTabsContentWrapper.find('.bw-model-tabs-data:eq(3)'),
			tabElementSixth = modelSpecsTabsContentWrapper.find('.bw-model-tabs-data:eq(6)'),
			tabElementNinth = modelSpecsTabsContentWrapper.find('.bw-model-tabs-data:eq(9)');

		if (tabElementThird.length != 0) {
			focusFloatingTab(tabElementThird, 250, 0);
		}

		if (tabElementSixth.length != 0) {
			focusFloatingTab(tabElementSixth, 500, 250);
		}

		if (tabElementNinth.length != 0) {
			focusFloatingTab(tabElementNinth, 750, 500);
		}
		function focusFloatingTab(element, startPosition, endPosition) {
            if (windowScrollTop > element.offset().top - 45) {
				if (!$('#overallUpdatesTab').hasClass('scrolled-left-' + startPosition)) {
                    $('.overall-specs-tabs-container').addClass('scrolled-left-' + startPosition);
                    scrollHorizontal(startPosition);
                }
            }

            else if (windowScrollTop < element.offset().top) {
                if ($('#overallUpdatesTab').hasClass('scrolled-left-' + startPosition)) {
                    $('.overall-specs-tabs-container').removeClass('scrolled-left-' + startPosition);
                    scrollHorizontal(endPosition);
                }
            }
        };

		
		
	});
    $('.overall-specs-tabs-wrapper li').click(function () {
        var target = $(this).attr('data-tabs');
        $('html, body').animate({ scrollTop: Math.ceil($(target).offset().top) - topNavBarHeight }, 1000);
        centerItVariableWidth($(this), '.overall-specs-tabs-container');        
    });
    $('.right-bike-swiper.swiper-container').on('inview', function (event, visible) {
        if (visible == true) {
            $('.swiper-ribbon').addClass('animate-ribbon');
        } else {

        }
	});
	$(".navtab").click(function (event) {
		
		try {
			triggerGA('Series_Page', 'Floating_Navigation_Clicked', $(this).data("lab"));
		}
		catch (e) {
			console.log(e);
		}
	});

	
});
