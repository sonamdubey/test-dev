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

		if($('body').hasClass('fullscreen-mode--iframe')) {
			$('.video-popup__body').css('height', window.innerHeight);
		}
		else {
			$('.video-popup__body').css('height', availableHeight);
		}
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
			self.videoSwiper.params.initialSlide = self.videoSwiper.activeIndex;
			swiperElement.removeClass('swiper-container-vertical');
			self.setSlidesDimension();
		}
		else {
			self.videoSwiper.params.freeMode = true;
			self.videoSwiper.params.direction = 'vertical';
			self.videoSwiper.params.initialSlide = self.videoSwiper.activeIndex;
			swiperElement.addClass('swiper-container-vertical');
			self.resetSlidesDimension();
		}

		self.videoSwiper.destroy(false);
		self.videoSwiper.init();
		self.videoSwiper.update(true);

		if (window.innerWidth > window.innerHeight) {
			var timeout;
			clearTimeout(timeout);
			timeout = setTimeout(function () {
				self.videoSwiper.update(true);
			}, 1000);

			if(typeof handleFullscreenAnchestor !== 'undefined') {
				handleFullscreenAnchestor();
			}
		}
	}

	self.setSlidesDimension = function () {
		var gallery = $("#videoGalleryPopup");
		var swiperElement = $(self.videoSwiper.container[0]);
		var sliderHeight = window.innerHeight - (gallery.find(".video-popup__head").height() * 1.5) - gallery.find(".video-list__back-btn").height();
		var sliderWidth = sliderHeight * 16 / 9;
		var swiperPadding = window.innerWidth - sliderWidth + 10; // 10px offset padding from left

		swiperElement.find(".swiper-slide").css("width", sliderWidth);
		$("#mainVideoSwiper").css("padding-right", swiperPadding);
	}

	self.resetSlidesDimension = function () {
		$(self.videoSwiper.container[0]).find(".swiper-slide").css("width", "100%");
		$("#mainVideoSwiper").css("padding-right", 0);
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
