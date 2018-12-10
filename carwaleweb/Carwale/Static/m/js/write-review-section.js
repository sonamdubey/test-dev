var OpenPopup = (function () {
    var readMoreLink, backBtn, popupContainer, popupActiveClass;

    function _setSelector() {
        readMoreLink = '.content__read-more';
        backBtn = '.popup__back';
        popupContainer = '.popupContainer';
        popupActiveClass = 'popup--active';
    }

	function registeEvents() {
		_setSelector();
		$(readMoreLink).on('click', function () {
			$(this).parents('.review-block').find(popupContainer).addClass(popupActiveClass);
			window.location.hash = $(this).attr("data-hash");
			Common.utils.lockPopup();
		});
		$(backBtn).on('click', function () {
            $(this).parents(popupContainer).removeClass(popupActiveClass);
			Common.utils.unlockPopup();
			window.history.back();
        })
    }

    return {
        registerEvents: registeEvents
    }

})();

$(document).ready(function () {
   OpenPopup.registerEvents();
});