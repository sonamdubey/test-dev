var Ribbon = (function () {
	var ribbonElement;

	function _setSelectors() {
		ribbonElement = $(".ribbon-content");
	}

	function _handleWindowScroll() {
		$(document).on("scroll", function() {
			_animateRibbon();
			_translateRibbon();
		})
	}

	function _animateRibbon() {
		var screenScroll = $(document).scrollTop();
		var ribbonPosition = parseInt($(".ribbon-content").css("top"));
		var scrollTop = (screenScroll + ribbonPosition);
		var containerTop = $(".ribbon-container").offset().top;
		var ribbonHeight = ribbonElement.outerHeight();
		var offersContainerbottom = containerTop + $(".ribbon-container").outerHeight();
		if (scrollTop + ribbonHeight >= containerTop && scrollTop <= offersContainerbottom) {
			ribbonElement.hide();
		} else {
			ribbonElement.fadeIn(50);
		}
	}

	function _translateRibbon() {
		var screenScroll = $(document).scrollTop();
		var fixedValue = 85;
		var prefixes = ["-moz-", "-webkit-", "-o-", "-ms-"];
		if (screenScroll >= 0 && screenScroll < fixedValue) {
			for (var i = 0; i < prefixes.length; i++) {
				var prefix = prefixes[i] + "transform";
				ribbonElement.css(prefix, "translateX(" + (screenScroll) + "px)");
			}
			$(".ribbon-content__data").css("opacity", "1");
		} else {
			for (var i = 0; i < prefixes.length; i++) {
				var prefix = prefixes[i] + "transform";
				ribbonElement.css(prefix, "translateX(" + fixedValue + "px)");
			}

			$(".ribbon-content__data").css("opacity", "0");
		}
		for (var i = 0; i < prefixes.length; i++) {
			var prefix = prefixes[i] + "transform";
			$(".ribbon-icon__background").css(prefix, "rotate(" + screenScroll + "deg)");
		}
	}

	function _handleRibbonClick() {
		$(document).on("click", ".ribbon-content", function () {
			$("body, html").stop();
			var offersContainerTop = $(".ribbon-container").offset().top;
			$("body, html").animate({
				scrollTop: offersContainerTop - 70
			}, 1000);
		});
	}

	function registerEvents() {
		_setSelectors();
		_handleWindowScroll();
		_handleRibbonClick();
	}

	return {
		registerEvents: registerEvents
	}
})();
