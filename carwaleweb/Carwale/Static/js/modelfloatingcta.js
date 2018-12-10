//floating CTA on model page
var floatingCta = (function () {

	var headerContainer, headerHeight, floatingCtaContainer, detailsBoxContainer, $window;

	function _setSelectors() {
		headerContainer = $('#header');
		headerHeight = $('#header').height();
		floatingCtaContainer = $('.floating-cta');
		detailsBoxContainer = $('.detailsBox');
		$window = $(window);
	}

	function registerEvents() {
		_setSelectors();
		$window.scroll(function () {
			_toggleFloatingCTA();
		});
	}

	function _toggleFloatingCTA() {
		var detailsBoxContainerOffsetBottom = detailsBoxContainer.closest('div').offset().top + detailsBoxContainer.closest('div').height(),
			windowScrollTop = $window.scrollTop();

		if ((detailsBoxContainerOffsetBottom < windowScrollTop + headerHeight)) {
			headerContainer.addClass('floating-cta__hide-header');
			floatingCtaContainer.addClass('floating-cta__show-cta');
			headerContainer.removeClass('floating-cta__show-header');
		}
		else {
			headerContainer.removeClass('floating-cta__hide-header');
			headerContainer.addClass('floating-cta__show-header');
			floatingCtaContainer.removeClass('floating-cta__show-cta');
		}
	}

	return {
		registerEvents: registerEvents
	}
})();
$(document).ready(function () {
	floatingCta.registerEvents();//initialize  floating cta
});
