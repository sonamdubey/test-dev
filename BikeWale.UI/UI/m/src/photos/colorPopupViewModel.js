var ModelColorPopupViewModel = function () {
	var self = this;

	self.modelName = MODEL_NAME;
	self.activePopup = ko.observable(false);
	self.activeLandscapeFooter = ko.observable(true);

	self.colorSwiper = ko.observable(new ModelColorSwiperViewModel());

	self.openPopup = function () {
		self.activePopup(true);
		ColorGallerySwiper.handleThumbnailSwiper(colorThumbnailGallerySwiper);

		triggerGA('Gallery_Page', 'Colours_Tab_Clicked_Opened', self.modelName);

		historyObj.addToHistory('colorPopup');

		GalleryState.subscribeAction(self.closePopup);
	}

	self.closePopup = function (isClickEvent) {
		if (self.activePopup()) {
			self.activePopup(false);
			self.resetListHeight();
			triggerGA('Gallery_Page', 'Colours_Tab_Clicked_Closed', self.modelName);

			if (isClickEvent) {
				history.back();
			}

			if ($('body').hasClass('scroll-lock--color')) {
				Scroll.unlock();
				$('body').removeClass('scroll-lock--color');
				resetFullScreenMode();
			}
			$('#galleryRoot').removeClass('color-tab-popup--active');
		}
	}

	self.setListHeight = function () {
		var colorGalleryPopup = $('#colorGalleryPopup');
		var availableHeight = colorGalleryPopup.innerHeight() - ($('#thumbnailColorSwiper').offset().top - (window.pageYOffset || document.documentElement.scrollTop));

		colorGalleryPopup.find('.color-popup__thumbnail-content').css('height', availableHeight);
	}

	self.resetListHeight = function () {
		$('#colorGalleryPopup').find('.color-popup__thumbnail-content').css('height', 'auto');
	}

	self.registerEvents = function () {
		$('#colorGalleryPopup').on('transitionend', function (event) {
			if ($(event.target).attr('id') === 'colorGalleryPopup') {
				if ($(this).hasClass('color-popup--active')) {
					self.setListHeight();
				}
			}
		})
	}

	self.registerEvents();
};

var ModelColorSwiperViewModel = function () {
	var self = this;

	self.modelName = MODEL_NAME;
	self.activeIndex = ko.observable(0);
	self.activeTitle = ko.observable('');

	self.colorList = ko.observableArray(MODEL_COLOR_IMAGES);

	self.renderImage = function (hostUrl, originalImagePath, imageSize) {
		if (originalImagePath && originalImagePath != null) {
			return (hostUrl + '/' + imageSize + '/' + originalImagePath);
		}
		else {
			return ('https://imgd.aeplcdn.com/' + imageSize + '/bikewaleimg/images/noimage.png?q=70');
		}
	}
}
