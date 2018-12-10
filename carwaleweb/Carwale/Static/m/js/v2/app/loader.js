
Loader = {
	isDesktop: window.innerWidth > 768,
    showOxygenLoaderOnSection: function (div) {
        $('.oxygen-loader').removeClass('hide');
        div.html($('.oxygenLoaderContainer__js').html());
    },

    hideOxygenLoaderFromSection: function (div) {
        $('.oxygen-loader').addClass('hide');
        div.html('');
    },

	showFullPageLoader: function () {
		Loader.isDesktop ? Common.utils.lockPopup() : lockPopup();
        $('.speedometer__container').removeClass('hide');
    },

    hideFullPageLoader: function () {
        $('.speedometer__container').addClass('hide');
		Loader.isDesktop ? Common.utils.unlockPopup() : unlockPopup();
    }
}