var NavigationTabs = (function () {
	var overallContainer, overallTabsListContent, overallNavBarHeight;
	var _activeTabClassName = 'tab--active';

	function _setSelectores() {
		overallContainer = $('#overallContainer');
		overallTabsListContent = overallContainer.find('.overall-tabs__content');
		overallNavBarHeight = overallTabsListContent.height();
	}

	function _setFallback() {
		if (navigator.userAgent.match(/OPR/gi)) {
			$('.overall-tabs__content').addClass('overall-tabs--fallback');
		}
	}

	function setTab(tabId) {
		overallContainer.find('.' + _activeTabClassName).removeClass(_activeTabClassName);
		overallContainer.find('[data-id]').addClass('hide');

		if (tabId) {
			overallContainer.find('[data-tabs="' + tabId + '"]').addClass(_activeTabClassName);
			overallContainer.find('[data-id="' + tabId + '"]').removeClass('hide');
		}
		else {
			overallContainer.find('.overall-tabs-list__item').first().addClass(_activeTabClassName);
			overallContainer.find('[data-id]').first().removeClass('hide');
		}
	}

	function _handleClickEvent() {
		$('.overall-tabs__list li').on('click', function () {
			var panelId = $(this).attr('data-tabs');
			var panel = $(this).closest('.overall-tabs__panel');
			var panelContent = panel.find('[data-id="' + panelId + '"]');

			_pauseYoutubeVideo();

			$(this).addClass(_activeTabClassName).siblings('.' + _activeTabClassName).removeClass(_activeTabClassName);

			if (panel.attr('data-panel-type') === 'toggle') {		
				panel.find('[data-id]').hide();
				panelContent.show();

				$('html, body').animate({
					scrollTop: panel.offset().top
				})
			}

			if (panelContent.find('.swiper-container').length) {
				panelContent.find('.swiper-container').each(function() {
					$(this).data('swiper').update(true);
				})
			}

		});
	}

	function _handleScrollEvent() {
		$(window).scroll(function () {
			var windowScrollTop = $(window).scrollTop(),
				overallTabsOffsetTop = overallContainer.find('.overall-tabs__placeholder').offset().top,
				overallContainerHeight = overallContainer.outerHeight();

			if (windowScrollTop > overallTabsOffsetTop) {
				overallTabsListContent.addClass('fixed-tab-nav');
			}

			else if (windowScrollTop < overallTabsOffsetTop) {
				overallTabsListContent.removeClass('fixed-tab-nav');
			}

			if (overallTabsListContent.hasClass('fixed-tab-nav')) {
				if (windowScrollTop > Math.ceil(overallContainerHeight) - (overallNavBarHeight * 2) + overallTabsOffsetTop) {
					overallTabsListContent.removeClass('fixed-tab-nav');
				}
			}
		});
	}

	function _pauseYoutubeVideo() {
		if (typeof SwiperYT !== 'undefined') {
			SwiperYT.YouTubeApi.videoPause();
		}
	}

	function registerEvents() {
		_setSelectores();
		_setFallback();

		// register events
		_handleClickEvent();
		_handleScrollEvent();
	}

	return {
		registerEvents: registerEvents,
		setTab: setTab
	}
})();
