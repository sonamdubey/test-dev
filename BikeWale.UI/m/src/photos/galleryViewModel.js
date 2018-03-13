﻿var ModelGalleryViewModel = function () {
	var self = this;

	// model image gallery
	self.activeTitle = ko.observable('');
	self.activeIndex = ko.observable(0);
	self.activePopup = ko.observable(false);

	// model gallery footer: color option
	self.activeFooterColorOption = ko.observable(false);

	// model color gallery
	self.colorPopup = ko.observable(new ModelColorPopupViewModel());

	self.fullScreenModeActive = ko.observable(false);
	self.fullScreenSupport = ko.observable(true);

	if (navigator.userAgent.indexOf('UCBrowser/11') >= 0) {
		self.fullScreenSupport(false);
	}
	self.floatingLandscapeSlugVisibilityThreshold = ko.observable(IMAGE_INDEX + 2);

	self.activeSwiperTitle = ko.observable(true);
	self.galleryFooterActive = ko.observable(true);

	self.activeFloatingRotateScreenOption = ko.observable(true);
	self.rotateScreenOption = ko.observable(false);
	self.activeLandscapeIcon = ko.observable(false);

	self.modelName = MODEL_NAME;
	self.photoList = ko.observableArray(MODEL_IMAGES);

	// share
	self.activeSharePopup = ko.observable(false);
	self.activeGalleryFooterShare = ko.observable(false);

	// continue slug
	self.activeContinueSlug = ko.observable(false);

	// video
	self.videoPopup = ko.observable(new ModelVideoPopupViewModel());

	// in between slug
	self.colorSlug = ko.observable(new ColorSlugViewModel(MODEL_COLOR_IMAGES));
	self.colorSlug().visibilityThreshold(IMAGE_INDEX + 5);
	self.isColorSlugEligible = MODEL_COLOR_IMAGES.length > 1 ? true : false;
	self.activeFloatingColorSlug = ko.observable(false);

	self.videoSlug = ko.observable(new VideoSlugViewModel(MODEL_VIDEO_LIST));
	self.videoSlug().description('Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.');
	self.videoSlug().visibilityThreshold(IMAGE_INDEX + 10);

	self.renderImage = function (hostUrl, originalImagePath, imageSize) {
		if (originalImagePath && originalImagePath != null) {
			return (hostUrl + '/' + imageSize + '/' + originalImagePath);
		}
		else {
			return ('https://imgd.aeplcdn.com/' + imageSize + '/bikewaleimg/images/noimage.png?q=70');
		}
	}

	self.hideFooterTabs = function () {
		if (self.activePopup()) {
			self.galleryFooterActive(false);
		}
	};

	self.showFooterTabs = function () {
		if (self.activePopup()) {
			self.galleryFooterActive(true);
		}
	};

	self.toggleFooterTabs = function () {
		self.galleryFooterActive() ? self.hideFooterTabs() : self.showFooterTabs();
	};

	// images popup events
	self.openGalleryPopup = function () {
		self.activeSwiperTitle(false);
		self.activePopup(true);
		self.setRotateScreenOption();
		self.setColorOption();

		MainGallerySwiper.calculateCenter();
		Scroll.lock();
		setTimeout(function () {
			self.activeSwiperTitle(true);
		}, 300);

		historyObj.addToHistory('imagesPopup');
		GalleryState.subscribeAction(self.closeGalleryPopup);
	};

	self.closeGalleryPopup = function () {
		if (self.activePopup()) {
			$('.model-gallery__container').animate({
				top: 0
			}, 300, 'swing');

			self.activeSwiperTitle(false);
			self.activePopup(false);
			self.setRotateScreenOption();
			self.setColorOption();

			$('.model-gallery-section').removeClass('gallery-color-slug--active');
			self.activeContinueSlug(false);

			Scroll.unlock();
			setTimeout(function () {
				vmModelGallery.activeSwiperTitle(true);
			}, 300);
		}
	};

	self.toggleFullScreen = function () {
		if (!self.fullScreenModeActive()) {
			toggleFullScreen(true);
			self.fullScreenModeActive(true);

			if ("orientation" in screen && screen.orientation.type == 'portrait-primary') {
				screen.orientation.unlock();
				screen.orientation.lock('landscape-primary');
			}
		}
		else {
			toggleFullScreen(false);
			self.fullScreenModeActive(false);

			if ("orientation" in screen && screen.orientation.type == 'landscape-primary') {
				screen.orientation.unlock();
				screen.orientation.lock('portrait-primary');
			}
		}
	};

	// share popup
	self.openSharePopup = function (event) {
		if (!self.activePopup() || self.fullScreenModeActive() || self.colorPopup().activePopup()) {
			self.activeSharePopup(true);
			historyObj.addToHistory('sharePopup');

			GalleryState.subscribeAction(self.closeSharePopup);
		}
		else {
			self.activeGalleryFooterShare(true);
		}

		Scroll.lock();
	}

	self.closeSharePopup = function () {
		if (!self.activePopup() || self.fullScreenModeActive() || self.colorPopup().activePopup()) {
			self.activeSharePopup(false);
		}
		else {
			self.activeGalleryFooterShare(false);
		}

		Scroll.unlock();
	}

	self.toggleSharePopup = function () {
		if (!self.activePopup() || self.fullScreenModeActive() || self.colorPopup().activePopup()) {
			self.activeSharePopup() ? self.closeSharePopup() : self.openSharePopup();
		}
		else {
			self.activeGalleryFooterShare() ? self.closeSharePopup() : self.openSharePopup();
		}
	}

	self.resetSharePopup = function () {
		self.activeSharePopup(false);
		self.activeGalleryFooterShare(false);
	}

	// rotate screen
	self.setRotateScreenOption = function () {
		if (self.activePopup()) {
			if (self.activeFloatingRotateScreenOption()) {
				self.rotateScreenOption(true);

				if (self.activeIndex() > self.floatingLandscapeSlugVisibilityThreshold()) {
					self.rotateScreenOption(false);
					self.activeFloatingRotateScreenOption(false);
				}
			}
			else {
				self.rotateScreenOption(false);
			}
		}
		else {
			self.rotateScreenOption(false);
		}
	}

	// gallery footer color button
	self.setColorOption = function () {
		if (self.isColorSlugEligible) {
			if (self.activePopup()) {
				if (self.colorSlug().slugShown() && !self.videoSlug().activeSlug()) {
					if (!self.fullScreenModeActive()) {
						self.activeFooterColorOption(false);
						self.activeFloatingColorSlug(true);
					}
					else {
						self.activeFooterColorOption(true);
						self.activeFloatingColorSlug(false);
					}
				}
			}
			else {
				self.activeFooterColorOption(false);
				self.activeFloatingColorSlug(false);
			}
		}
	}

	self.setLandscapeIcon = function () {
		var element = $('.gallery__landscape-slug .screen--rotate-slug__landscape-icon');

		var topPosition = $('.model-gallery__container .gallery-footer').offset().top - $('.gallery__landscape-slug').offset().top + 8;

		var rightPosition =  -(element.width());

		if (!self.fullScreenModeActive()) {
			element.css('position', 'fixed').animate({ 'top': topPosition + 'px', 'right': rightPosition + 'px' }, 1000, "swing");
			$('.gallery__landscape-slug').addClass('landscape-slug--inactive');
			setTimeout(function () {
				self.setRotateScreenOption();
			}, 1400);
			self.activeLandscapeIcon(true);
		}
		else {
			element.css({
				'position': 'fixed',
				'top': topPosition + 'px',
				'right': rightPosition + 'px'
			});
			$('.gallery__landscape-slug').addClass('landscape-slug--inactive');
			self.setRotateScreenOption();
			self.activeLandscapeIcon(true);
		}
	}

	// handle slug visibility
	self.setColorSlug = function(activeIndex) {
		if (self.isColorSlugEligible) {
			if (self.colorSlug().visibilityThreshold() === activeIndex) {
				self.colorSlug().activeSlug(true);
				self.activeContinueSlug(true);
			}
			else {
				self.colorSlug().activeSlug(false);
				if (!self.videoSlug().activeSlug()) {
					self.activeContinueSlug(false);
				}

				if (activeIndex > self.colorSlug().visibilityThreshold()) {
					self.colorSlug().slugShown(true);
				}
			}
		}
		else {
			if (self.colorSlug().visibilityThreshold() === activeIndex) {
				self.colorSlug().activeSingleColorMessage(true);
			}
			else {
				self.colorSlug().activeSingleColorMessage(false);
			}
		}
	}

	self.setVideoSlug = function (activeIndex) {
		if (self.videoSlug().visibilityThreshold() === activeIndex) {
			self.videoSlug().activeSlug(true);
			self.activeContinueSlug(true);
			self.activeFloatingColorSlug(false);
		}
		else {
			self.videoSlug().activeSlug(false);
			if (!self.colorSlug().activeSlug()) {
				self.activeContinueSlug(false);
			}
		}
	}

	self.handleGalleryContinueClick = function () {
		$('.model-gallery__container .gallery__next').trigger('click');
	}
};

var ModelColorPopupViewModel = function () {
	var self = this;

	self.activePopup = ko.observable(false);
	self.activeLandscapeFooter = ko.observable(true);

	self.colorSwiper = ko.observable(new ModelColorSwiperViewModel());

	self.openPopup = function () {
		self.activePopup(true);
		self.setListHeight();

		historyObj.addToHistory('colorPopup');

		GalleryState.subscribeAction(self.closePopup);
	}

	self.closePopup = function () {
		if (self.activePopup()) {
			self.activePopup(false);
		}
	}

	self.setListHeight = function () {
		var thumbnailSwiper = $('#thumbnailColorSwiper');
		var thumbnailSwiperList = thumbnailSwiper.find('.model-color__list');
		var availableHeight = $('#colorGalleryPopup').innerHeight() - $('#thumbnailColorSwiper').offset().top;

		if (thumbnailSwiperList.height() > availableHeight) {
			thumbnailSwiperList.css('height', availableHeight)
		}
	}
};

var ModelColorSwiperViewModel = function () {
	var self = this;

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

var ModelVideoPopupViewModel = function () {
	var self = this;

	self.activePopup = ko.observable(false);
	self.videoData = ko.observable(new ModelVideoViewModel());
	self.videoData().getVideos();

	self.openPopup = function () {
		self.activePopup(true);
		self.setBodyHeight();
		self.handleSwiperResize();
		self.videoSwiper.update(true);

		historyObj.addToHistory('videosPopup');

		GalleryState.subscribeAction(self.closePopup);
	}

	self.closePopup = function () {
		if (self.activePopup()) {
			self.activePopup(false);
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
			if((swiper.activeIndex + 1) % 4 === 0) {
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
		}
		else {
			self.videoSwiper.params.freeMode = true;
			self.videoSwiper.params.direction = 'vertical';
			self.videoSwiper.params.initialSlide = 0;
			swiperElement.addClass('swiper-container-vertical');
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
	self.videoList = ko.observable([]);

	self.getVideos = function () {
		if (self.videoList().length !== MODEL_VIDEO_LIST.length) {
			var list = MODEL_VIDEO_LIST.slice(self.videoList().length, self.defaultVideoCount() + self.videoList().length);

			self.videoList(self.videoList().concat(list));
		}
	}
}

var ColorSlugViewModel = function (colorPhotoList) {
	var self = this;

	self.visibilityThreshold = ko.observable(5);
	self.previewCount = 3;

	self.activeSlug = ko.observable(false);
	self.activeSingleColorMessage = ko.observable(false);
	self.slugShown = ko.observable(false);
	self.modelName = MODEL_NAME;
	self.colorList = colorPhotoList.slice(0, self.previewCount);
	self.remainingCount = colorPhotoList.length - self.previewCount;

	self.registerEvents = function () {
		$(document).on('click', '.model-gallery__color-slide .color-slide-list__item', function () {
			var clickedIndex = $(this).index();

			vmModelGallery.colorPopup().openPopup();
			colorGallerySwiper.slideTo(clickedIndex);
		});
	}

	self.registerEvents();
}

var VideoSlugViewModel = function (videoList) {
	var self = this;

	self.activeSlug = ko.observable(false);
	self.visibilityThreshold = ko.observable(5);
	self.modelName = MODEL_NAME;
	self.videoList = videoList;
	self.videoCount = videoList.length;
	self.isPlaying = ko.observable(false);
	self.description = ko.observable('');

	self.playVideo = function(event) {
		var targetElement = event.currentTarget;

		self.isPlaying(true);
		$(targetElement).closest('.model-gallery__video-slide').find('.iframe-overlay').trigger('click');
	};
}
