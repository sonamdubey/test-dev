var ModelVideoPopupViewModel = function () {
	var self = this;

	self.activePopup = ko.observable(false);
	self.browserCustomPlayer = ko.observable(false);
	if (navigator.userAgent.match(/UCBrowser/g)) {
		self.browserCustomPlayer(true);
	}
	self.videoData = ko.observable(new ModelVideoViewModel());
	self.videoData().getVideos();
	self.modelName = MODEL_NAME;

	self.openPopup = function () {
		self.activePopup(true);
		self.setBodyHeight();
		self.handleSwiperResize();
		self.videoSwiper.update(true);
		triggerGA('Gallery_Page', 'All_Videos_Tab_Clicked_Opened', self.modelName);
		historyObj.addToHistory('videosPopup');

		GalleryState.subscribeAction(self.closePopup);
	}

	self.closePopup = function (isClickEvent) {
		if (self.activePopup()) {
			self.activePopup(false);
			triggerGA('Gallery_Page', 'All_Videos_Tab_Clicked_Closed', self.modelName);
			SwiperYT.YouTubeApi.videoPause();

			if (isClickEvent) {
				history.back();
			}
		}
	}

	self.setBodyHeight = function () {
		var availableHeight = $('#videoGalleryPopup').outerHeight() - $('.video-popup__head').outerHeight();

		$('.video-popup__body').css('height', availableHeight);
	}

	self.videoSwiper = new Swiper('#mainVideoSwiper', {
		direction: 'vertical',
		freeMode: true,
		spaceBetween: 0,
		slidesPerView: 'auto',
		onTap: function (swiper) {
			if (window.innerWidth > window.innerHeight) {
				swiper.slideTo(swiper.clickedIndex);
			}
		},
		onSlideChangeStart: function (swiper) {
			SwiperYT.slideChangeStart();
			if ((swiper.activeIndex + 1) % 4 === 0) {
				self.videoData().getVideos();
				self.videoSwiper.update(true);
			}
		}
	})

	self.handleSwiperResize = function () {
		var swiperElement = $(self.videoSwiper.container[0]);

		if (window.innerWidth > window.innerHeight) {
			self.videoSwiper.params.freeMode = false;
			self.videoSwiper.params.direction = 'horizontal';
			self.videoSwiper.params.initialSlide = self.videoSwiper.slides.length;
			swiperElement.removeClass('swiper-container-vertical');
			self.setSlidesDimension();
		}
		else {
			self.videoSwiper.params.freeMode = true;
			self.videoSwiper.params.direction = 'vertical';
			self.videoSwiper.params.initialSlide = 0;
			swiperElement.addClass('swiper-container-vertical');
			self.resetSlidesDimension();
		}

		self.videoSwiper.destroy(false);
		self.videoSwiper.init();
		self.videoSwiper.update(true);

		if (window.innerWidth > window.innerHeight) {
			self.videoSwiper.slideTo(0, 1000);

			setTimeout(function () {
				self.videoSwiper.update(true);
			}, 1000);
		}
	}

	self.setSlidesDimension = function () {
		var swiperElement = $(self.videoSwiper.container[0]);

		swiperElement.find('.swiper-slide').css('min-height', swiperElement.height() - 15); // decrement 15px of offset space between swiper and footer
	}

	self.resetSlidesDimension = function () {
		$(self.videoSwiper.container[0]).find('.swiper-slide').css('min-height', '250px'); // set minimum value to override the value that was set in landscape mode
	}

	self.registerEvents = function () {
		$(window).on('resize', function () {
			if (self.activePopup()) {
				self.setBodyHeight();
				self.handleSwiperResize();
			}
		});
	}

	self.registerEvents();
}

var ModelVideoViewModel = function () {
	var self = this;

	self.defaultVideoCount = ko.observable(10);
	self.listTypeIFrame = true;

	if(navigator.userAgent.match(/UCBrowser/g)) {
		self.listTypeIFrame = false;
	}

	self.videoList = ko.observable([]);

	self.getVideos = function () {
		if (MODEL_VIDEO_LIST) {
			if (self.videoList().length !== MODEL_VIDEO_LIST.length) {
				var list = MODEL_VIDEO_LIST.slice(self.videoList().length, self.defaultVideoCount() + self.videoList().length);

				self.videoList(self.videoList().concat(list));
			}
		}
	}

}
