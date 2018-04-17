var ModelGalleryViewModel = function () {
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

	var requestFullScreen = checkFullScreenSupport();
	if (typeof requestFullScreen === 'undefined') {
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
	self.colorSlug().visibilityThreshold(IMAGE_INDEX + 3);
	self.isColorSlugEligible = MODEL_COLOR_IMAGES.length > 1 ? true : false;
	self.activeFloatingColorSlug = ko.observable(false);

	self.videoSlug = ko.observable(new VideoSlugViewModel(MODEL_VIDEO_LIST));
	self.videoSlug().description('');
	if(self.videoSlug().slugSupport()) {
		self.videoSlug().visibilityThreshold(IMAGE_INDEX + 10);
	}
	else {
		self.videoSlug().visibilityThreshold(-1);
	}

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
	    currentPage = 'Gallery_Page';
		self.activeSwiperTitle(false);
		self.activePopup(true);
		gaObj.id = galleryId;
		triggerGA('Gallery_Page', 'Gallery_Loaded', self.modelName);
		if (logBhrighu) {
		    logBhrighuForImage($("#mainPhotoSwiper .swiper-slide-active"));
		}
		logBhrighu = true;
		resizeHandler();

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
			currentPage = 'Model_Images_Page';
			gaObj.id = lastPageId;
			self.activeSwiperTitle(false);
			self.showFooterTabs();
			self.activePopup(false);
			self.setRotateScreenOption();
			self.setColorOption();

			self.resetSlug();
			self.activeContinueSlug(false);

			Scroll.unlock();
			setTimeout(function () {
				vmModelGallery.activeSwiperTitle(true);
			}, 300);
		}
	};

	self.toggleFullScreen = function () {
		if (screenfull.enabled) {
			if (!screenfull.isFullscreen) {
				document.documentElement.style.backgroundColor = '#000';
			}
			else {
				document.documentElement.style.backgroundColor = '';
			}
			screenfull.toggle();
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
			self.activeFloatingColorSlug(false);
			self.rotateScreenOption(false);
		}

		Scroll.lock();
	}

	self.closeSharePopup = function () {
		if (!self.activePopup() || self.fullScreenModeActive() || self.colorPopup().activePopup()) {
			self.activeSharePopup(false);
		}
		else {
			self.activeGalleryFooterShare(false);
			self.setColorOption();
			self.setRotateScreenOption();
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
			if (self.activeFloatingRotateScreenOption() && !self.fullScreenModeActive()) {
				self.rotateScreenOption(true);
				self.activeLandscapeIcon(false);

				if (self.activeIndex() > self.floatingLandscapeSlugVisibilityThreshold()) {
					self.rotateScreenOption(false);
					self.activeLandscapeIcon(true);
					self.activeFloatingRotateScreenOption(false);
				}
			}
			else {
				self.activeLandscapeIcon(true);
				self.rotateScreenOption(false);
			}
		}
		else {
			self.activeLandscapeIcon(false);
			self.rotateScreenOption(false);
		}
	}

	// gallery footer color button
	self.setColorOption = function () {
		if (self.isColorSlugEligible) {
			if (self.activePopup()) {
				if (!self.activeFloatingRotateScreenOption() && self.colorSlug().slugShown() && !self.videoSlug().activeSlug()) {
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

	self.resetSlug = function() {
		self.colorSlug().resetSlug();
		self.videoSlug().resetSlug();
	}

	self.setLandscapeIcon = function () {
		if (!self.activeLandscapeIcon()) {
			var element = $('.model-gallery-section .gallery__landscape-slug .screen--rotate-slug__landscape-icon');

			var topPosition = $('.model-gallery__container .gallery-footer').offset().top - $('.model-gallery-section .gallery__landscape-slug').offset().top + 8;

			var rightPosition = -(window.innerWidth - ($('.model-gallery-section .screen--rotate-slug__landscape-container').offset().left + $('.model-gallery-section .screen--rotate-slug__landscape-container').width()) - 25);

			$('.model-gallery-section .gallery__landscape-slug').css('z-index', 10);

			if (!self.fullScreenModeActive()) {
				element.css('position', 'fixed').animate({ 'top': topPosition + 'px', 'right': rightPosition + 'px' }, 1000, "swing");
				setTimeout(function () {
					self.setRotateScreenOption();
				}, 1100);
				self.activeLandscapeIcon(true);
			}
			else {
				element.css({
					'position': 'fixed',
					'top': topPosition + 'px',
					'right': rightPosition + 'px'
				});
				self.setRotateScreenOption();
				self.activeLandscapeIcon(true);
			}
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
		if (self.videoSlug().visibilityThreshold() === activeIndex && self.videoSlug().videoCount) {
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

var ColorSlugViewModel = function (colorPhotoList) {
	var self = this;

	self.colorCount = colorPhotoList.length;
	self.visibilityThreshold = ko.observable(5);
	self.previewCount = 3;

	self.activeSlug = ko.observable(false);
	self.activeSingleColorMessage = ko.observable(false);
	self.slugShown = ko.observable(false);
	self.modelName = MODEL_NAME;
	self.colorList = colorPhotoList.slice(0, self.previewCount);
	self.remainingCount = colorPhotoList.length - self.previewCount;

	self.resetSlug = function() {
		self.activeSlug(false);
	}

	self.registerEvents = function () {
		$(document).on('click', '.model-gallery__color-slide .color-slide-list__item', function () {
			var clickedIndex = $(this).index();

			vmModelGallery.colorPopup().openPopup();
			triggerColorImageChangeGA = false;
			colorGallerySwiper.slideTo(clickedIndex);
		});
	}

	self.registerEvents();
}

var VideoSlugViewModel = function (videoList) {
	var self = this;

	self.activeSlug = ko.observable(false);
	self.slugSupport = ko.observable(true);
	if (navigator.userAgent.match(/UCBrowser/g)) {
		self.slugSupport(false);
	}
	self.visibilityThreshold = ko.observable(5);
	self.modelName = MODEL_NAME;
	if (videoList) {
		self.videoList = videoList;
	}
	else {
		self.videoList = [];
	}
	self.videoCount = self.videoList.length;
	self.isPlaying = ko.observable(false);
	self.description = ko.observable('');

	self.resetSlug = function () {
		self.activeSlug(false);
	}

	self.playVideo = function(event) {
		var targetElement = event.currentTarget;

		self.isPlaying(true);
		$(targetElement).closest('.model-gallery__video-slide').find('.iframe-overlay').trigger('click');
	};
}
function formatDate(dateString) {
    var date = new Date(dateString);
    var monthNames = [
"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
    ];

    var dd = date.getDate();
    var mm = date.getMonth();
    var yyyy = date.getFullYear();

    if (dd < 10) {
        dd = '0' + dd
    }

    var formattedDate = monthNames[mm].substr(0, 3) + ' ' + dd + ', ' + yyyy;
    return formattedDate;
}

function formatNumeric(numberToFormat) {

    numberToFormat = numberToFormat.toString();
    var formatted = '';
    var breakPoint = 3, noOfCommas = 3;
    for (var i = numberToFormat.length - 1; i >= 0; i--) {
        formatted = numberToFormat[i] + formatted;
        if ((numberToFormat.length - i) == breakPoint && numberToFormat.length > breakPoint && noOfCommas > 0) {
            formatted = "," + formatted;
            breakPoint += 2;
            noOfCommas--;
        }
    }
    return formatted;
}