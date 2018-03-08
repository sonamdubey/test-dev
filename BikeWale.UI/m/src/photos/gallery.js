var modelColorImageCount = 0,
	modelImages = [],
	modelColorImages = [],
	videoList = null;
var eleGallery, vmModelGallery, colorIndex = 0, galleryRoot;
var photoCount, videoCount, modelName, bikeModelId, imageIndex, colorImageId, returnUrl, isColorImageSet = false;
var imageTypes = ["Other", "ModelImage", "ModelGallaryImage", "ModelColorImage"];

var gallerySwiper, colorGallerySwiper, thumbnailSwiper, colorThumbnailSwiper, videoThumbnailSwiper, videoListEvents;

var setPageVariables = function () {
	eleGallery = $("#pageGallery");

	try {
		if (eleGallery.length > 0 && eleGallery.data("images") != '') {
			var imageList = JSON.parse(Base64.decode(eleGallery.data("images")));
			modelImages = imageList;
			modelColorImages = filterColorImagesArray(imageList);

			if (modelColorImages)
				modelColorImageCount = modelColorImages.length;
		}

		if (eleGallery.length > 0 && eleGallery.data("videos") != '') {
			videoList = JSON.parse(Base64.decode(eleGallery.data("videos")));
		}

		photoCount = eleGallery.data("photoscount");
		videoCount = eleGallery.data("videoscount");
		imageIndex = eleGallery.data("selectedimageid");
		colorImageId = eleGallery.data("selectedcolorimageid");
		returnUrl = eleGallery.data("returnurl");
		modelName = eleGallery.data("modelname");
		bikeModelId = eleGallery.data("modelid");


	} catch (e) {
		console.warn(e);
	}
}

var modelGallery = function () {
	var self = this;

	self.fullScreenModeActive = ko.observable(false);
	self.floatingSlugVisibilityThreshold = 3;

	// model image gallery
	self.activePhotoTitle = ko.observable('');
	self.activePhotoIndex = ko.observable(1);
	self.activeGalleryPopup = ko.observable(false);

	// model gallery footer: color option
	self.activeGalleryFooterColor = ko.observable(false);

	// model color gallery
	self.colors = ko.observable(new modelColorSwiper());
	self.activeColorPopup = ko.observable(false);
	self.colorsFooterActive = ko.observable(true);

	self.HideSwiperTitle = ko.observable(false);

	self.galleryFooterActive = ko.observable(true);

	self.rotateScreenOption = ko.observable(true);
	self.galleryColorFixedSlug = ko.observable(false);
	self.activeLandscapeIcon = ko.observable(false);

	self.photoList = ko.observableArray(modelImages);

	// share
	self.activeSharePopup = ko.observable(false);
	self.activeGalleryFooterShare = ko.observable(false);

	// floating slug: color
	self.activeColorFloatingSlug = ko.observable(false);

	//
	//var colorList = self.colorPhotoList().concat(self.colorPhotoList());
	//self.colorPhotoList(colorList.concat(colorList));
	//self.colorPhotoList(self.colorPhotoList().slice(0, 3))
	//

	var vmcolorSlugViewModel = new colorSlugViewModel(self.colors().colorPhotoList());
	ko.applyBindings(vmcolorSlugViewModel, document.getElementById('carouselColorSlug'));

	self.videoList = ko.observableArray(videoList);

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
		self.activeGalleryPopup(true);
		historyObj.addToHistory('imagesPopup');
	};

	self.closeGalleryPopup = function (isClickEvent) {
		if (self.activeGalleryPopup()) {
			self.activeGalleryPopup(false);
			$('.black-window').trigger('click');

			if (isClickEvent) {
				history.back();
			}
		}
	};

	// color popup events
	self.openColorPopup = function () {
		self.activeColorPopup(true);
		setColorListHeight();
		setColorGalleryFooter();
		historyObj.addToHistory('colorsPopup');
	}

	self.closeColorPopup = function (isClickEvent) {
		if (self.activeColorPopup()) {
			self.activeColorPopup(false);

			if (isClickEvent) {
				history.back();
			}
		}
	}

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
		if (!self.activeGalleryPopup() || self.fullScreenModeActive()) {
			self.activeSharePopup(true);
			historyObj.addToHistory('sharePopup');
		}
		else {
			self.activeGalleryFooterShare(true);
		}

		Scroll.lock();
	}

	self.closeSharePopup = function(isClickEvent) {
		if (!self.activeGalleryPopup() || self.fullScreenModeActive()) {
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
		if (!self.activeGalleryPopup() || self.fullScreenModeActive()) {
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
		if (self.fullScreenModeActive()) {
			self.rotateScreenOption(false);
		}
		else if (self.activePhotoIndex() >= 5) {
			self.rotateScreenOption(false);
		}
		else {
			self.rotateScreenOption(true);
		}
	}

	// gallery footer color button
	self.setGalleryFooterColor = function() {
		if(self.activeGalleryPopup() && self.activePhotoIndex() > self.floatingSlugVisibilityThreshold) {
			if(self.fullScreenModeActive()) {
				self.activeGalleryFooterColor(true);
				self.activeColorFloatingSlug(false);
			}
			else {
				self.activeGalleryFooterColor(false);
				self.activeColorFloatingSlug(true);
			}
		}
	}

};

function colorSlugViewModel(colorPhotoList) {
	var self = this;

	self.colorCount = 3;
	self.colorPhotoList = colorPhotoList.slice(0, self.colorCount);
	self.moreColorCount = colorPhotoList.length - self.colorCount;

	self.colorData = {
		colorPhotoList: self.colorPhotoList,
		moreColorCount: self.moreColorCount
	};
}

var popupGallery = {
	open: function () {
		vmModelGallery.isGalleryActive(true);
		if (navigator.userAgent.indexOf('UCBrowser/11') >= 0) {
			vmModelGallery.fullscreenSupport(false);
		}
		$('body').addClass('lock-browser-scroll');

		if (colorImageId > 0) {
			if (vmModelGallery.colors().activeColorIndex() == 0) vmModelGallery.colors().activeColorIndex(1);
			vmModelGallery.toggleColorThumbnailScreen();
		}
	},

	close: function () {
		if (returnUrl && returnUrl.length > 0) {
			window.location.href = returnUrl;
		}
		else {
			vmModelGallery.isGalleryActive(false);
			vmModelGallery.resetGallery();
			$('body').removeClass('lock-browser-scroll');
			toggleFullScreen(false);
		}
	},

	bindGallery: function (imageIndex) {
		triggerGA('Gallery_Page', 'Gallery_Loaded', modelName);
		//popupGallery.open();
		//gallerySwiper.update(true);

		if (returnUrl.length <= 0) {
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
	var currImage = modelImages[activeImageIndex];
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
	vmModelGallery.setGalleryFooterColor();
	vmModelGallery.resetSharePopup();
	setColorListHeight();
	setColorGalleryFooter();
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

			label = 'modelId=' + bikeModelId + '|imageid=' + imageid + lb + '|pageid=' + (gaObj ? gaObj.id : 0);
			cwTracking.trackImagesInteraction("BWImages", "ImageViewed", label);
			triggerVirtualPageView(window.location.host + window.location.pathname, lb);
		}
	}

}

docReady(function () {

	galleryRoot = $('#galleryRoot');

	setPageVariables();

	vmModelGallery = new modelGallery();
	vmModelColorSwiper = new modelColorSwiper();

	if (galleryRoot.length > 0) {
		ko.applyBindings(vmModelGallery, galleryRoot[0]);
		ko.applyBindings(vmModelColorSwiper, document.getElementById('vmcolorSwiper'));
	}

	// popup states
	$(window).on('popstate', function () {
		if (history.state) {
			switch (history.state.currentState) {
				case 'imagesPopup':
					vmModelGallery.closeColorPopup();

				default:
					break;
			}
		}
		else {
			vmModelGallery.closeGalleryPopup();
		}

		if ($('#sharePopup').hasClass('share-popup--active')) {
			vmModelGallery.closeSharePopup();
		}

	});

	var mainGallerySwiper = new Swiper("#mainPhotoSwiper", {
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

			if (swiper.activeIndex < 5) {
				gallerySlug.setColor($(swiper.slides[1]));
			}
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
					vmModelGallery.rotateScreenOption(false);
				}, 1500);
				vmModelGallery.activeLandscapeIcon(true);
			}

			vmModelGallery.setGalleryFooterColor();

			activeSlideTitle = $(activeSlide).find('img').attr('alt');
			vmModelGallery.activePhotoTitle(activeSlideTitle);
		}
	});

	colorGallerySwiper = new Swiper('#mainColorSwiper', {
		spaceBetween: 0,
		preloadImages: false,
		lazyLoading: true,
		lazyLoadingInPrevNext: true,
		nextButton: '.color-type-next',
		prevButton: '.color-type-prev',
		onInit: function (swiper) {
			vmModelGallery.colors().activeColorIndex(1);
			//thumbnailSwiperEvents.setColorPhotoDetails(swiper);
		},
		onTap: function (swiper, event) {
			if(vmModelGallery.fullScreenModeActive()) {
				vmModelGallery.colorsFooterActive(!vmModelGallery.colorsFooterActive());
			}
		},
		onSlideChangeStart: function (swiper) {
			thumbnailSwiperEvents.setColorPhotoDetails(swiper);
			if(!vmModelGallery.fullScreenModeActive()) {
				thumbnailSwiperEvents.focusThumbnail(colorThumbnailSwiper, vmModelGallery.colors().activeColorIndex(), false);
			}
			else {
				thumbnailSwiperEvents.focusThumbnail(colorThumbnailSwiper, vmModelGallery.colors().activeColorIndex(), true);
			}

			var activeElement = $(colorThumbnailSwiper.slides[swiper.activeIndex]);
			colorBox.scrollIntoView(activeElement);
			colorBox.setColorCode(activeElement);
		}
	});

	colorThumbnailSwiper = new Swiper('#thumbnailColorSwiper', {
		spaceBetween: 0,
		slidesPerView: 'auto',
		onInit: function (swiper) {
			if (!vmModelGallery.fullScreenModeActive()) {
				swiper.destroy(false);
				thumbnailSwiperEvents.attachColorEvents(swiper);
				thumbnailSwiperEvents.focusThumbnail(swiper, vmModelGallery.colors().activeColorIndex(), false);
			}
			else {
				swiper.update();
				swiper.attachEvents();
				thumbnailSwiperEvents.focusThumbnail(swiper, vmModelGallery.colors().activeColorIndex(), true);
			}

			colorBox.setColorCode($(swiper.slides[vmModelGallery.colors().activeColorIndex() - 1]));
		}
	});

	window.addEventListener('resize', resizeHandler, true);
	resizeHandler();
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

function setColorListHeight() {
	if ($('#thumbnailColorSwiper .model-color__list').height() > $('#colorGalleryPopup').innerHeight() - $('#thumbnailColorSwiper').offset().top) {
		$('#thumbnailColorSwiper .model-color__list').css({
			height: $('#colorGalleryPopup').innerHeight() - $('#thumbnailColorSwiper').offset().top
		})
	}
}

function setColorGalleryFooter() {
	if (colorThumbnailSwiper) {
		colorThumbnailSwiper.init();
	}

	if(!vmModelGallery.fullScreenModeActive()) {
		var availableHeight = window.innerHeight - $('.color-popup__thumbnail-content').offset().top;

		if ($('.model-color__list').height() > availableHeight) {
			$('.model-color__list').css('height', availableHeight + 'px');
		}
	}
}

var gallerySlug = (function () {
	function _registerColorSlugEvents() {
		$(document).on('click', '.model-gallery__color-slide .swiper-slide__more-btn', function () {
			vmModelGallery.openColorPopup();
		});

		$(document).on('click', '.model-gallery__color-slide .color-slide-list__item', function () {
			var clickedIndex = $(this).index();

			vmModelGallery.openColorPopup();
			colorGallerySwiper.slideTo(clickedIndex);
		});
	}

	function setColor(slideElement) {
		if (!slideElement.hasClass(slideElement)) {
			slideElement.addClass("swiper-slide__slug");
			slideElement.append($("#carouselColorSlug").html());
			_registerColorSlugEvents();
		}
	}

	function removeSlug(slideElement) {
		if (slideElement.length) {
			slideElement.removeClass('swiper-slide__slug').find(".gallery__slide-slug").remove();
		}
	}

	return {
		setColor: setColor,
		removeSlug: removeSlug
	};
})();

var thumbnailSwiperEvents = (function () {
	function attachColorEvents(swiper) {
		var elements = swiper.container[0].querySelectorAll('.swiper-slide');

		for (var i = 0; i < elements.length; i++) {
			elements[i].addEventListener('click', function (event) {
				var clickedIndex = $(event.currentTarget).index();

				colorGallerySwiper.slideTo(clickedIndex);
			});
		}
	}

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

		vmModelGallery.colors().activeColorIndex(swiper.activeIndex + 1);
		vmModelGallery.colors().activeColorTitle(activeSlideTitle);
	}

	return {
		focusThumbnail: focusThumbnail,
		setColorPhotoDetails: setColorPhotoDetails,
		attachColorEvents: attachColorEvents
	}
})();

var colorBox = (function () {
	function setColorCode(element) {
		var gradient = '';

		element.find('span').each(function (index) {
			var color = $(this).attr('style').split(':')[1];
			var colorCode = color.trim().split(';')[0];

			if (index) {
				gradient += ', '
			}

			gradient += colorCode;
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
