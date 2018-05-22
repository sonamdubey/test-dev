var ViewPort = (function() {
	function isElementInViewportTopBottom(element) {
		if (typeof jQuery === "function" && element instanceof jQuery) {
			element = element[0];
		}
		var rect = element.getBoundingClientRect();
		return (
			rect.top >= 0 &&
			rect.bottom <= $(window).height()
		);
	}

	function isElementInViewportLeftRight(element) {
		if (typeof jQuery === "function" && element instanceof jQuery) {
			element = element[0];
		}
		var rect = element.getBoundingClientRect();
		return (
			rect.left >= 0 &&
			rect.right <= $(window).width()
		);
	}

	return {
		isElementInViewportTopBottom: isElementInViewportTopBottom,
		isElementInViewportLeftRight: isElementInViewportLeftRight
	}
})()
