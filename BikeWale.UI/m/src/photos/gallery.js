// gallery variables
var modelColorImageCount = 0, MODEL_IMAGES = [], MODEL_COLOR_IMAGES = [], MODEL_VIDEO_LIST = null;

var eleGallery, vmModelGallery, colorIndex = 0;

// page variables
var PHOTO_COUNT, VIDEO_COUNT, MODEL_NAME, BIKE_MODEL_ID, IMAGE_INDEX, COLOR_IMAGE_ID, RETURN_URL, isColorImageSet = false;

// bhrighu logging
var imageTypes = ["Other", "ModelImage", "ModelGallaryImage", "ModelColorImage"];

var colorGallerySwiper, colorThumbnailSwiper;

var setPageVariables = function () {
	eleGallery = $("#pageGallery");

	try {
		if (eleGallery.length > 0 && eleGallery.data("images") != '') {
			var imageList = JSON.parse(Base64.decode(eleGallery.data("images")));
			MODEL_IMAGES = imageList;
			MODEL_COLOR_IMAGES = filterColorImagesArray(imageList);

			if (MODEL_COLOR_IMAGES)
				modelColorImageCount = MODEL_COLOR_IMAGES.length;
		}

		if (eleGallery.length > 0 && eleGallery.data("videos") != '') {
			MODEL_VIDEO_LIST = JSON.parse(Base64.decode(eleGallery.data("videos")));
		}

		PHOTO_COUNT = eleGallery.data("photoscount");
		VIDEO_COUNT = eleGallery.data("videoscount");
		IMAGE_INDEX = eleGallery.data("selectedimageid");
		COLOR_IMAGE_ID = eleGallery.data("selectedcolorimageid");
		RETURN_URL = eleGallery.data("returnurl");
		MODEL_NAME = eleGallery.data("modelname");
		BIKE_MODEL_ID = eleGallery.data("modelid");		
	} catch (e) {
		console.warn(e);
	}
}

var ModelGalleryViewModel = function () {
	var self = this;

	// model image gallery
	self.activePhotoTitle = ko.observable('');
	self.activePhotoIndex = ko.observable(1);
	self.activePopup = ko.observable(false);

	// model gallery footer: color option
	self.activeFooterColorOption = ko.observable(false);
	self.activeFloatingColorSlug = ko.observable(false);

	// model color gallery
	self.colorPopup = ko.observable(new ModelColorPopupViewModel());

	self.fullScreenModeActive = ko.observable(false);
	self.floatingSlugVisibilityThreshold = 3;

	self.HideSwiperTitle = ko.observable(false);
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
	self.activeColorSlug = ko.observable(false);
	self.colorSlug = new ColorSlugViewModel(MODEL_COLOR_IMAGES);

	self.activeVideoSlug = ko.observable(true);
	self.videoSlug = new VideoSlugViewModel(MODEL_VIDEO_LIST);

	self.renderImage = function (hostUrl, originalImagePath, imageSize) {
		if (originalImagePath && originalImagePath != null) {
			return (hostUrl + '/' + imageSize + '/' + originalImagePath);
		}
		else {
			return ('https://imgd.aeplcdn.com/' + imageSize + '/bikewaleimg/images/noimage.png?q=70');
		}
	}

	self.hideFooterTabs = function () {
		self.galleryFooterActive(false);
	};

	self.showFooterTabs = function () {
		self.galleryFooterActive(true);
	};

	self.toggleFooterTabs = function () {
		self.galleryFooterActive() ? self.hideFooterTabs() : self.showFooterTabs();
	};

	// images popup events
	self.openGalleryPopup = function () {
		self.activePopup(true);
		self.setRotateScreenOption();
		self.setColorOption();

		historyObj.addToHistory('imagesPopup');
		GalleryState.subscribeAction(self.closeGalleryPopup);
	};

	self.closeGalleryPopup = function (isClickEvent) {
		if (self.activePopup()) {
			self.activePopup(false);
			self.setRotateScreenOption();
			self.setColorOption();

			$('.model-gallery-section').removeClass('gallery-color-slug--active');
			self.activeContinueSlug(false);
			
			$('.black-window').trigger('click');

			if (isClickEvent) {
				history.back();
			}
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

	self.closeSharePopup = function (isClickEvent) {
		if (!self.activePopup() || self.fullScreenModeActive() || self.colorPopup().activePopup()) {
			self.activeSharePopup(false);

			if(isClickEvent) {
				history.back();
			}
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
	self.setRotateScreenOption = function() {
		if(self.activePopup()) {
			if (self.activeFloatingRotateScreenOption()) {
				self.rotateScreenOption(true);

				if(self.activePhotoIndex() > 2) {
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
	self.setColorOption = function() {
		if (self.activePopup()) {
			if (!self.activeFloatingRotateScreenOption()) {
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

	self.closePopup = function (isClickEvent) {
		if (self.activePopup()) {
			self.activePopup(false);

			if(isClickEvent) {
				history.back();
			}
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
	self.videoList = ko.observableArray(MODEL_VIDEO_LIST);

	self.openPopup = function () {
		self.activePopup(true);
		self.setBodyHeight();
		self.videoSwiper.update(true);

		historyObj.addToHistory('videosPopup');

		GalleryState.subscribeAction(self.closePopup);
	}

	self.closePopup = function (isClickEvent) {
		if (self.activePopup()) {
			self.activePopup(false);

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
		}
	})

	self.handleSwiperResize = function () {
		var swiperElement = $(self.videoSwiper.container[0]);

		if (window.innerWidth > window.innerHeight) {
			self.videoSwiper.params.freeMode = false;
			self.videoSwiper.params.direction = 'horizontal';
			self.videoSwiper.params.initialSlide = 4;
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
			self.videoSwiper.slideTo(0, 500);

			setTimeout(function() {
				self.videoSwiper.update(true);
			}, 500);
		}
	}

	self.registerEvents = function () {
		$(window).on('resize', function () {
			if(self.activePopup()) {
				self.setBodyHeight();
				self.handleSwiperResize();
			}
		});
	}

	self.registerEvents();
}

var ModelVideoViewModel = function () {
	var self = this;

	self.videoList = ko.observable(MODEL_VIDEO_LIST);
}

var ColorSlugViewModel = function (colorPhotoList) {
	var self = this;

	self.previewCount = 3;
	self.modelName = MODEL_NAME;
	self.colorList = colorPhotoList.slice(0, self.previewCount);
	self.remainingCount = colorPhotoList.length - self.previewCount;
}

var VideoSlugViewModel = function (videoList) {
	var self = this;

	self.modelName = MODEL_NAME;
	self.videoList = videoList;

	self.registerEvents = function() {
		$(document).on('click', '.model-gallery__video-slide .iframe-overlay', function() {
			$(this).closest('.model-gallery__video-slide').addClass('video--active');
		});
	}

	self.registerEvents();
}

var popupGallery = {
	open: function () {
		vmModelGallery.isGalleryActive(true);
		if (navigator.userAgent.indexOf('UCBrowser/11') >= 0) {
			vmModelGallery.fullscreenSupport(false);
		}
		$('body').addClass('lock-browser-scroll');

		if (COLOR_IMAGE_ID > 0) {
			if (vmModelGallery.colorPopup().colorSwiper().activeIndex() == 0) vmModelGallery.colorPopup().colorSwiper().activeIndex(1);
			vmModelGallery.toggleColorThumbnailScreen();
		}
	},

	close: function () {
		if (RETURN_URL && RETURN_URL.length > 0) {
			window.location.href = RETURN_URL;
		}
		else {
			vmModelGallery.isGalleryActive(false);
			vmModelGallery.resetGallery();
			$('body').removeClass('lock-browser-scroll');
			toggleFullScreen(false);
		}
	},

	bindGallery: function (imageIndex) {
		triggerGA('Gallery_Page', 'Gallery_Loaded', MODEL_NAME);
		//popupGallery.open();
		//gallerySwiper.update(true);

		if (RETURN_URL.length <= 0) {
			window.location.hash = 'photosGallery';
		}
	}
}

function filterColorImagesArray(responseArray) {
	return ko.utils.arrayFilter(responseArray, function (response) {
		return response.ImageType == 3;
	});
}

function getImageDownloadUrl() {
	var activeImageIndex = vmModelGallery.activePhotoIndex() - 1;
	if (activeImageIndex == -1)
		activeImageIndex++;
	var currImage = MODEL_IMAGES[activeImageIndex];
	return currImage.HostUrl + "0x0" + currImage.OriginalImgPath;
}

function resizePortraitImage(element) {
	element.hide();

	var imageElement = new Image();
	imageElement.src = element.attr('data-original') || element.attr('src');

	if ((imageElement.width / imageElement.height) < 1.5) {
		var elementParent = element.parent();
		element.css({
			'width': 'auto',
			'height': elementParent.innerHeight() + 'px'
		});

		elementParent.css('background', '#fff');
	}
	element.show();
}

function resizeHandler() {
	if (window.innerWidth > window.innerHeight) {
		vmModelGallery.fullScreenModeActive(true);
		vmModelGallery.hideFooterTabs();
	}
	else {
		vmModelGallery.fullScreenModeActive(false);
		vmModelGallery.showFooterTabs();
	}

	vmModelGallery.setRotateScreenOption();
	vmModelGallery.setColorOption();
	vmModelGallery.resetSharePopup();

	if(colorThumbnailSwiper) {
		ColorGallerySwiper.handleThumbnailSwiper(colorThumbnailSwiper);
	}
};

function toggleFullScreen(goFullScreen) {
	var doc = window.document;
	var docElement = doc.documentElement;

	var requestFullScreen = docElement.requestFullscreen || docElement.mozRequestFullScreen || docElement.webkitRequestFullScreen || docElement.msRequestFullscreen;
	var cancelFullScreen = doc.exitFullscreen || doc.mozCancelFullScreen || doc.webkitExitFullscreen || doc.msExitFullscreen || doc.webkitCancelFullScreen;

	if (goFullScreen && requestFullScreen != undefined) {
		docElement.style.backgroundColor = '#2a2a2a';
		requestFullScreen.call(docElement);
	}
	else if (cancelFullScreen != undefined) {
		cancelFullScreen.call(doc);
		docElement.style.backgroundColor = '';
	}
}

function logBhrighuForImage(item) {
	if (item) {
		var imageid = item.attr("data-imgid"), imgcat = item.attr("data-imgcat"), imgtype = item.attr("data-imgtype");
		if (imageid) {
			var lb = "";
			if (imgcat) {
				lb += "|category=" + imgcat;
			}

			if (imgtype) {
				lb += "|type=" + imageTypes[imgtype];
			}

			label = 'modelId=' + BIKE_MODEL_ID + '|imageid=' + imageid + lb + '|pageid=' + (gaObj ? gaObj.id : 0);
			cwTracking.trackImagesInteraction("BWImages", "ImageViewed", label);
			triggerVirtualPageView(window.location.host + window.location.pathname, lb);
		}
	}

}

docReady(function () {

	setPageVariables();

	var galleryRoot = $('#galleryRoot');
	vmModelGallery = new ModelGalleryViewModel();

	if (galleryRoot.length) {
		ko.applyBindings(vmModelGallery, galleryRoot[0]);
	}

	var videoTab = $('#videoTab');
	vmModelVideo = new ModelVideoViewModel();

	if(videoTab.length) {
		ko.applyBindings(vmModelGallery, videoTab[0]);
	}

	// popup states
	// TODO: update popstate logic
	$(window).on('popstate', function () {
		GalleryState.dispatchAction();
	});

	// initialize and register main gallery swiper
	MainGallerySwiper.init();
	MainGallerySwiper.registerEvents();

	// initialize color gallery swiper
	colorGallerySwiper = ColorGallerySwiper.initSwiper();
	colorThumbnailSwiper = ColorGallerySwiper.initThumbnailSwiper();

	window.addEventListener('resize', resizeHandler, true);
	resizeHandler();

	SwiperYT.YouTubeApi.addApiScript();
});

function isInViewport(element) {
	var rect = element.getBoundingClientRect();
	var html = document.documentElement;
	return (
		rect.top >= 0 &&
		rect.left >= 0 &&
		rect.bottom <= (window.innerHeight || html.clientHeight) &&
		rect.right <= (window.innerWidth || html.clientWidth)
	);
}

var thumbnailSwiperEvents = (function () {
	function focusThumbnail(swiper, vmActiveIndex, slideToFlag) {
		var activeIndex = vmActiveIndex - 1; // decrement by 1, since it was incremented by 1
		var thumbnailIndex = swiper.slides[activeIndex];

		if (slideToFlag) {
			swiper.slideTo(activeIndex);
		}

		$(swiper.slides).removeClass('swiper-slide-active slide--focus');
		$(thumbnailIndex).addClass('swiper-slide-active slide--focus');
	}

	function setColorPhotoDetails(swiper) {
		var activeSlide = swiper.slides[swiper.activeIndex];
		var activeSlideTitle = $(activeSlide).find('img').attr('alt');

		vmModelGallery.colorPopup().colorSwiper().activeIndex(swiper.activeIndex + 1);
		vmModelGallery.colorPopup().colorSwiper().activeTitle(activeSlideTitle);
	}

	return {
		focusThumbnail: focusThumbnail,
		setColorPhotoDetails: setColorPhotoDetails
	}
})();

var colorBox = (function () {
	function setColorCode(element) {
		var colorBoxElement = element.find('.color-box');
		var gradient = '';

		colorBoxElement.find('.color-box__item').each(function (index) {
			var color = $(this).attr('style').split(':')[1];
			var colorCode = color.trim().split(';')[0];

			if (index) {
				gradient += ', '
			}

			gradient += colorCode;

			if (colorBoxElement.attr('data-color-count') === '1') {
				gradient += ', ' + gradient;
			}

		});

		element.siblings('.swiper-slide').find('.color-box--border').attr('style', '');
		element.find('.color-box--border').attr('style', 'background-image: linear-gradient(white, white), linear-gradient(to right, ' + gradient + ')');
	}

	function scrollIntoView(element) {
		var elementTopPosition = element.offset().top;
		var elementBottomPosition = elementTopPosition + element.outerHeight(true);

		var listTopPosition = $('.model-color__list').offset().top;
		var listBottomPosition = listTopPosition + $('.model-color__list').outerHeight(true);

		if ((listBottomPosition > elementBottomPosition) && (listTopPosition < elementBottomPosition)) {
		}
		else {
			$('.model-color__list').animate({
				scrollTop: (elementTopPosition + $('.model-color__list').scrollTop()) - listTopPosition - 15
			})
		}

		if (listTopPosition > elementBottomPosition - 20) {
			$('.model-color__list').animate({
				scrollTop: (elementTopPosition + $('.model-color__list').scrollTop()) - listTopPosition - 15
			})
		}
	}

	return {
		setColorCode: setColorCode,
		scrollIntoView: scrollIntoView
	}
})();


var historyObj = (function () {
	function addToHistory(currentState, title, url) {
		history.pushState({
			currentState: currentState
		}, title, url);
	}

	return {
		addToHistory: addToHistory
	}
})();

// handle gallery popup's browser back state
var GalleryState = (function() {
	var _actions = [];

	function subscribeAction(action) {
		_actions.push(action);
	}

	function dispatchAction() {
		var action = _actions[_actions.length - 1];

		if(action) {
			action();
			_actions.pop();
		}
	}

	return {
		subscribeAction: subscribeAction,
		dispatchAction: dispatchAction
	}
})();

// main gallery swiper
var MainGallerySwiper = (function() {
	function init() {
		var swiper = new Swiper("#mainPhotoSwiper", {
			spaceBetween: 0,
			preloadImages: false,
			lazyLoading: true,
			lazyLoadingInPrevNext: true,
			nextButton: ".gallery__next",
			prevButton: ".gallery__prev",
			onInit: function (swiper) {
				swiper.slideTo(vmModelGallery.activePhotoIndex() - 1);
				var activeSlide = swiper.slides[swiper.activeIndex];
				activeSlideTitle = $(activeSlide).find('img').attr('alt');
				vmModelGallery.activePhotoTitle(activeSlideTitle);
				resizeHandler();
			},
			onTap: function (swiper) {
				if (vmModelGallery.fullScreenModeActive()) {
					vmModelGallery.toggleFooterTabs();
				}
			},
			onTransitionStart: function (swiper) {
				var activeSlide = swiper.slides[swiper.activeIndex];

				if ($(activeSlide).hasClass('swiper-slide__slug')) {
					$(swiper.container).find('.gallery-image__footer').hide();
				}
				else {
					$('.gallery-image__footer').show();
				}

				if (vmModelGallery.activeGalleryFooterShare()) {
					vmModelGallery.closeSharePopup();
				}

				if(vmModelGallery.activeVideoSlug) {
					SwiperYT.YouTubeApi.videoPause();
				}
			},
			onSlideChangeEnd: function (swiper) {
				vmModelGallery.activePhotoIndex(swiper.activeIndex + 1);
				logBhrighuForImage($("#mainPhotoSwiper .swiper-slide-active"));

				// remove embedded slug
				var activeSlide = swiper.slides[swiper.activeIndex];
				var sliderSlugSlide = $(activeSlide).prev('.swiper-slide__slug');

				if (swiper.activeIndex === 2) {
					var element = $('.model-gallery__screen--rotate-slug .screen--rotate-slug__landscape-icon');
					var elementTopPosition = ($('.gallery-image__footer').offset().top - $('.model-gallery__screen--rotate-slug').offset().top + 10);
					var elementRightPosition = element.width() + element.offset().left - $(window).width() + $('.landscape-icon').width();

					element.css('position', 'fixed').animate({ 'top': elementTopPosition + 'px', 'right': elementRightPosition + 'px' }, 1000, "swing");
					$('.model-gallery__screen--rotate-slug').addClass('model-gallery__screen--rotate-slug--hide');
					setTimeout(function () {
						vmModelGallery.setRotateScreenOption();
					}, 1500);
					vmModelGallery.activeLandscapeIcon(true);
				}

				vmModelGallery.setColorOption();

				if (vmModelGallery.activePopup()) {
					if (swiper.activeIndex == 1) {
						$('.model-gallery-section').addClass('gallery-color-slug--active');
						vmModelGallery.activeContinueSlug(true);
					}
					else {
						$('.model-gallery-section').removeClass('gallery-color-slug--active');
						vmModelGallery.activeContinueSlug(false);
					}
				}

				activeSlideTitle = $(activeSlide).find('img').attr('alt');
				vmModelGallery.activePhotoTitle(activeSlideTitle);
			}
		})

		return swiper;
	}

	function registerEvents() {
		$('.image-grid__list').on('click', '.image-grid-list__item', function () {

			var imageNumber;
			var container = $('.model-gallery__container');
			($(this).closest('#imageGridTop').length !== 0) ? imageNumber = $(this).index() : imageNumber = $(this).index() + 7;

			if (!container.hasClass('model-gallery--relative')) {
				container.hide();
				vmModelGallery.HideSwiperTitle(true);
				_calculateCenter();
				container.fadeIn(100);
				Scroll.lock();
				vmModelGallery.activePopup(true);

				$('#mainPhotoSwiper').data('swiper').slideTo((imageNumber));
				container.closest('.model-gallery-section').css('height', 'auto');
			}
		});

		$('.swiper__image').on('click', '.swiper-slide', function (e) {
			var container = $(this).closest('.model-gallery__container')
			if (!container.hasClass('model-gallery--relative')) {
				vmModelGallery.HideSwiperTitle(true);
				_calculateCenter();
				vmModelGallery.openGalleryPopup();
				Scroll.lock();
				resizeHandler();
				setTimeout(function () {
					vmModelGallery.HideSwiperTitle(false);
				}, 300);

			}
		});

		$(document).on('click', '.black-window', function (e) {
			vmModelGallery.HideSwiperTitle(true);
			$('.model-gallery__container').animate({
				top: 0
			}, 300, "swing");

			vmModelGallery.closeGalleryPopup();

			Scroll.unlock();

			setTimeout(function () {
				vmModelGallery.HideSwiperTitle(false);
			}, 300);
		});

		$(window).on('resize', function () {
			if ($('.model-gallery--relative').is(':visible')) {
				_calculateCenter();
			}

		});
	};

	function _calculateCenter() {
		var CenterPosition,
			element = $('.model-gallery__container'),
			ElementScrollTop = element.closest('.model-gallery-section').offset().top,
			$window = $(window),
			WidowScrollTop = $window.scrollTop(),
			WindowHeight = $window.height() / 2;

		if (WidowScrollTop > ElementScrollTop) {
			CenterPosition = ((WidowScrollTop - ElementScrollTop) + (WindowHeight) - (element.height() / 2));
		}
		else {
			CenterPosition = (((WindowHeight) - element.height() / 2) - (ElementScrollTop - WidowScrollTop));
		}
		$('.model-gallery__container').animate({ top: CenterPosition }, 300, "swing");
	};

	return {
		init: init,
		registerEvents: registerEvents
	}
})();

var ColorGallerySwiper = (function () {
	function initSwiper() {
		var swiper = new Swiper('#mainColorSwiper', {
			spaceBetween: 0,
			preloadImages: false,
			lazyLoading: true,
			lazyLoadingInPrevNext: true,
			nextButton: '.color-type-next',
			prevButton: '.color-type-prev',
			onInit: function (swiper) {
				vmModelGallery.colorPopup().colorSwiper().activeIndex(1);
				//thumbnailSwiperEvents.setColorPhotoDetails(swiper);
			},
			onTap: function (swiper, event) {
				if (vmModelGallery.fullScreenModeActive()) {
					vmModelGallery.colorPopup().activeLandscapeFooter(!vmModelGallery.colorPopup().activeLandscapeFooter());
				}
			},
			onSlideChangeStart: function (swiper) {
				thumbnailSwiperEvents.setColorPhotoDetails(swiper);
				if (!vmModelGallery.fullScreenModeActive()) {
					thumbnailSwiperEvents.focusThumbnail(colorThumbnailSwiper, vmModelGallery.colorPopup().colorSwiper().activeIndex(), false);
				}
				else {
					thumbnailSwiperEvents.focusThumbnail(colorThumbnailSwiper, vmModelGallery.colorPopup().colorSwiper().activeIndex(), true);
				}

				var activeElement = $(colorThumbnailSwiper.slides[swiper.activeIndex]);
				colorBox.scrollIntoView(activeElement);
				colorBox.setColorCode(activeElement);
			}
		});

		return swiper;
	}

	function initThumbnailSwiper() {
		var swiper = new Swiper('#thumbnailColorSwiper', {
			spaceBetween: 0,
			slidesPerView: 'auto',
			onInit: function (swiper) {
				handleThumbnailSwiper(swiper);
			}
		});

		return swiper;
	}

	function handleThumbnailSwiper(swiper) {
		if (!vmModelGallery.fullScreenModeActive()) {
			swiper.destroy(false);
			$(swiper.container[0]).find('.swiper-wrapper').css({
				'transform': 'translate3d(0, 0, 0)'
			});
			_attachColorEvents(swiper);
			thumbnailSwiperEvents.focusThumbnail(swiper, vmModelGallery.colorPopup().colorSwiper().activeIndex(), false);
		}
		else {
			swiper.update(true);
			swiper.attachEvents();
			thumbnailSwiperEvents.focusThumbnail(swiper, vmModelGallery.colorPopup().colorSwiper().activeIndex(), true);
		}

		colorBox.setColorCode($(swiper.slides[vmModelGallery.colorPopup().colorSwiper().activeIndex() - 1]));
	}

	function _attachColorEvents(swiper) {
		var elements = swiper.container[0].querySelectorAll('.swiper-slide');

		for (var i = 0; i < elements.length; i++) {
			elements[i].addEventListener('click', function (event) {
				var clickedIndex = $(event.currentTarget).index();

				colorGallerySwiper.slideTo(clickedIndex);
			});
		}
	}

	return {
		initSwiper: initSwiper,
		initThumbnailSwiper: initThumbnailSwiper,
		handleThumbnailSwiper: handleThumbnailSwiper
	}
})();


// Gallery slugs
// color and video
var GallerySlug = (function () {
	function _registerColorSlugEvents() {
		$(document).on('click', '.model-gallery__color-slide .color-slide-list__item', function () {
			var clickedIndex = $(this).index();

			vmModelGallery.colorPopup().openPopup();
			colorGallerySwiper.slideTo(clickedIndex);
		});
	}

	function setColor(container) {
		container.append($('#swiperColorSlug').html());
		_registerColorSlugEvents();
	}

	return {
		setColor: setColor
	};
})();
