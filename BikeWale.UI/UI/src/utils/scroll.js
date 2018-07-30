var Scroll = (function() {
	function lock() {
		var htmlElement = $('html'), bodyElement = $('body');
		if ($(document).height() > $(window).height()) {
			var windowScrollTop = htmlElement.scrollTop() ? htmlElement.scrollTop() : bodyElement.scrollTop();
			if (windowScrollTop < 0) {
				windowScrollTop = 0;
			}
			htmlElement.addClass('lock-browser-scroll').css('top', -windowScrollTop);
		}
	}

	function unlock() {
		var htmlElement = $('html'),
			windowScrollTop = parseInt(htmlElement.css('top'));
		htmlElement.removeClass('lock-browser-scroll');
		$('html, body').scrollTop(-windowScrollTop);
	}

	return {
		lock: lock,
		unlock: unlock
	}

})();
