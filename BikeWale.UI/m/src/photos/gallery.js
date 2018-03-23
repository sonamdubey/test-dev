// gallery variables
var modelColorImageCount = 0, MODEL_IMAGES = [], MODEL_COLOR_IMAGES = [], MODEL_VIDEO_LIST = null;
var downloadImageResolution = "1056x594";
var eleGallery, vmModelGallery, colorIndex = 0;

// page variables
var PHOTO_COUNT, VIDEO_COUNT, MODEL_NAME, MAKE_NAME, BIKE_MODEL_ID, IMAGE_INDEX, COLOR_IMAGE_ID, COLOR_INDEX, RETURN_URL, isColorImageSet = false, logBhrighu, currentPage, triggerColorImageChangeGA, triggerGalleryImageChangeGA;

// bhrighu logging
var imageTypes = ["Other", "ModelImage", "ModelGallaryImage", "ModelColorImage"];

var mainGallerySwiper, colorGallerySwiper, colorThumbnailGallerySwiper;

var buttonClicked = false, lastSlide = 0, currentSlide = 0, lastColorSlide = 0, currentColorSlide = 0;

var setPageVariables = function () {
	eleGallery = $("#pageGallery");

	try {
		PHOTO_COUNT = eleGallery.data("photoscount");
		VIDEO_COUNT = eleGallery.data("videoscount");
		IMAGE_INDEX = eleGallery.data("selectedimageid");
		COLOR_IMAGE_ID = eleGallery.data("selectedcolorimageid");
		RETURN_URL = eleGallery.data("returnurl");
		MODEL_NAME = eleGallery.data("modelname");
		BIKE_MODEL_ID = eleGallery.data("modelid");
		MAKE_NAME = eleGallery.data("makename");
		logBhrighu = true;
		triggerColorImageChangeGA = true;
		triggerGalleryImageChangeGA = true;
		currentPage = 'Model_Images_Page';
		if (eleGallery.length > 0 && eleGallery.data("images") != '') {
			var imageList = JSON.parse(Base64.decode(eleGallery.data("images")));
			MODEL_IMAGES = imageList;
			MODEL_COLOR_IMAGES = filterColorImagesArray(imageList);

			if (MODEL_COLOR_IMAGES) {
				modelColorImageCount = MODEL_COLOR_IMAGES.length;
			}

			if (COLOR_IMAGE_ID > 0) {
				ko.utils.arrayForEach(MODEL_COLOR_IMAGES, function (item, index) {
					if (item.ColorId === COLOR_IMAGE_ID) {
						COLOR_INDEX = index;
					}
				});
			}
		}

		if (eleGallery.length > 0 && eleGallery.data("videos") != '') {
			MODEL_VIDEO_LIST = JSON.parse(Base64.decode(eleGallery.data("videos")));
		}

	} catch (e) {
		console.warn(e);
	}
}

var popupGallery = {
    open: function () {

		vmModelGallery.openGalleryPopup();

		if (COLOR_INDEX) {
			vmModelGallery.colorPopup().openPopup();
			colorGallerySwiper.slideTo(COLOR_INDEX);
		}
		else if (IMAGE_INDEX) {
			mainGallerySwiper.slideTo(IMAGE_INDEX);
		}
	},

	close: function () {
		if (RETURN_URL && RETURN_URL.length > 0) {
			window.location.href = RETURN_URL;
		}
		else {
			vmModelGallery.closeGalleryPopup();
			toggleFullScreen(false);
		}
	},

	bindGallery: function () {
		popupGallery.open();
	}
}

function filterColorImagesArray(responseArray) {
	return ko.utils.arrayFilter(responseArray, function (response) {
		return response.ImageType == 3;
	});
}

function getImageDownloadUrl() {
	var activeImageIndex = vmModelGallery.activeIndex() - 1;
	if (activeImageIndex == -1)
		activeImageIndex++;
	var currImage = MODEL_IMAGES[activeImageIndex];
	return currImage.HostUrl + downloadImageResolution + currImage.OriginalImgPath;
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
	if(vmModelGallery.activePopup()) {
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

		if(colorThumbnailGallerySwiper) {
			ColorGallerySwiper.handleThumbnailSwiper(colorThumbnailGallerySwiper);
		}
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

	// popup states
	$(window).on('popstate', function () {
		GalleryState.dispatchAction();

		if (!GalleryState.count()) {
			popupGallery.close();	
		}
	});

	if(navigator.userAgent.match(/UCBrowser/g)) {
		$('body').addClass('browser--uc');
	}

	// initialize and register main gallery swiper
	mainGallerySwiper = MainGallerySwiper.init();
	MainGallerySwiper.registerEvents();

	// initialize color gallery swiper
	colorGallerySwiper = ColorGallerySwiper.initSwiper();
	colorThumbnailGallerySwiper = ColorGallerySwiper.initThumbnailSwiper();
	ColorGallerySwiper.registerEvents();

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

	function count() {
		return _actions.length;
	}

	return {
		count: count,
		dispatchAction: dispatchAction,
		subscribeAction: subscribeAction
	}
})();

// main gallery swiper
var MainGallerySwiper = (function() {
    function init() {
		var swiper = new Swiper("#mainPhotoSwiper", {
			initialSlide: vmModelGallery.activeIndex(),
			spaceBetween: 0,
			preloadImages: false,
			lazyLoading: true,
			lazyLoadingInPrevNext: true,
			nextButton: ".gallery__next",
			prevButton: ".gallery__prev",
			onInit: function (swiper) {
				SwiperEvents.setDetails(swiper, vmModelGallery);
			},

			onTap: function (swiper, event) {
				if (!$(event.target).hasClass('gallery__arrow-btn')) {
					if (vmModelGallery.fullScreenModeActive()) {
						vmModelGallery.toggleFooterTabs();
					}
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

				if(vmModelGallery.videoSlug().activeSlug()) {
					SwiperYT.YouTubeApi.videoPause();
				}
			},

			onSlideChangeStart: function (swiper) {
				if (vmModelGallery.activePopup()) {
					vmModelGallery.setColorSlug(swiper.activeIndex);
					vmModelGallery.setVideoSlug(swiper.activeIndex);
					

				}
				lastSlide = vmModelGallery.activeIndex();
			},

			onSlideChangeEnd: function (swiper) {

				if (swiper.activeIndex >= vmModelGallery.floatingLandscapeSlugVisibilityThreshold()) {
					vmModelGallery.setLandscapeIcon();
				}

				vmModelGallery.setColorOption();

				SwiperEvents.setDetails(swiper, vmModelGallery);

				currentSlide = vmModelGallery.activeIndex();
				if (triggerGalleryImageChangeGA) {
				    if (!buttonClicked) {
				        if (currentSlide != lastSlide) {
				            currentSlide > lastSlide ? triggerGA(currentPage, 'Swipe_Right', MAKE_NAME + "_" + MODEL_NAME) : triggerGA(currentPage, 'Swipe_Left', MAKE_NAME + "_" + MODEL_NAME);
				        }
				    }
				    else {
				        triggerGA(currentPage, 'Image_Carousel_Clicked', MAKE_NAME + "_" + MODEL_NAME);
				    }
				}
				triggerGalleryImageChangeGA = true;
				logBhrighuForImage($("#mainPhotoSwiper .swiper-slide-active"));
				buttonClicked = false;
			}
		})

		return swiper;
	}

	function registerEvents() {
		$('.swiper__image').on('click', '.swiper-slide', function (e) {
			var container = $(this).closest('.model-gallery__container');

			if (!container.hasClass('gallery-popup--active')) {
				vmModelGallery.openGalleryPopup();
				calculateCenter();				
			}
		});

		$(window).on('resize', function () {
			if ($('.gallery-popup--active').is(':visible')) {
				calculateCenter();
			}
		});

		$('.swiper--next').on('click', function (e) {
		    buttonClicked = true;
		});

		$('.swiper--prev').on('click', function (e) {
		    buttonClicked = true;
		});
	};

	function calculateCenter() {
		var centerPosition,
			element = $('.model-gallery__container'),
			elementScrollTop = element.closest('.model-gallery-section').offset().top,
			widowScrollTop = $(window).scrollTop(),
			windowHeight = $(window).height() / 2;

		centerPosition = windowHeight - ($('.model-gallery-section').height() / 2)
		if(window.innerWidth > window.innerHeight) {
			$('.model-gallery__container').css('top', '0');
		}
		else {
			$('.model-gallery__container').css('top', centerPosition);
		}
	};

	return {
		init: init,
		registerEvents: registerEvents,
		calculateCenter: calculateCenter
	}
})();

var ColorGallerySwiper = (function () {
	function initSwiper() {
		var swiper = new Swiper('#mainColorSwiper', {
			initialSlide: vmModelGallery.colorPopup().colorSwiper().activeIndex(),
			spaceBetween: 0,
			preloadImages: false,
			lazyLoading: true,
			lazyLoadingInPrevNext: true,
			nextButton: '#mainColorSwiper .color-type-next',
			prevButton: '#mainColorSwiper .color-type-prev',
			onInit: function (swiper) {
				SwiperEvents.setDetails(swiper, vmModelGallery.colorPopup().colorSwiper());
			},

			onTap: function (swiper, event) {
				if (!$(event.target).hasClass('color__arrow-btn')) {
					if (vmModelGallery.fullScreenModeActive()) {
						vmModelGallery.colorPopup().activeLandscapeFooter(!vmModelGallery.colorPopup().activeLandscapeFooter());
					}
				}
			},

			onSlideChangeStart: function (swiper) {
			    lastColorSlide = vmModelGallery.colorPopup().colorSwiper().activeIndex();
			    SwiperEvents.setDetails(swiper, vmModelGallery.colorPopup().colorSwiper());
			    if (!vmModelGallery.fullScreenModeActive()) {
					SwiperEvents.focusThumbnail(colorThumbnailGallerySwiper, vmModelGallery.colorPopup().colorSwiper().activeIndex(), false);
				}
				else {
					SwiperEvents.focusThumbnail(colorThumbnailGallerySwiper, vmModelGallery.colorPopup().colorSwiper().activeIndex(), true);
				}

				var activeElement = $(colorThumbnailGallerySwiper.slides[swiper.activeIndex]);
				colorBox.scrollIntoView(activeElement);
				colorBox.setColorCode(activeElement);
				
			},

			onSlideChangeEnd: function (swiper) {

			    currentColorSlide = vmModelGallery.colorPopup().colorSwiper().activeIndex();
			    if (triggerColorImageChangeGA) {
			        if (!buttonClicked) {
			            if (currentColorSlide != lastColorSlide) {
			                currentColorSlide > lastColorSlide ? triggerGA(currentPage, 'Swipe_Right_Colours_Tab', MAKE_NAME + "_" + MODEL_NAME) : triggerGA(currentPage, 'Swipe_Left_Colours_Tab', MAKE_NAME + "_" + MODEL_NAME);
			            }
			        }
			        else {
			            triggerGA(currentPage, 'Image_Carousel_Clicked_Colours_Tab', MAKE_NAME + "_" + MODEL_NAME);
			        }
			    }
			    logBhrighuForImage($('#mainColorSwiper .swiper-slide-active'));
			    triggerColorImageChangeGA = true;
			    buttonClicked = false;
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
			SwiperEvents.focusThumbnail(swiper, vmModelGallery.colorPopup().colorSwiper().activeIndex(), false);
		}
		else {
			swiper.update(true);
			swiper.attachEvents();
			SwiperEvents.focusThumbnail(swiper, vmModelGallery.colorPopup().colorSwiper().activeIndex(), true);
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

	function registerEvents() {
	    $('.color-box__content').on('click', function () {
	        triggerGA(currentPage, 'Colour_Changed', MAKE_NAME + ' ' + MODEL_NAME);
	        logBhrighuForImage($('#mainColorSwiper .swiper-slide-active'));
	    })
	}

	return {
	    initSwiper: initSwiper,
	    initThumbnailSwiper: initThumbnailSwiper,
	    handleThumbnailSwiper: handleThumbnailSwiper,
	    registerEvents: registerEvents
	}
})();

var SwiperEvents = (function () {
	function focusThumbnail(swiper, vmActiveIndex, slideToFlag) {
		var activeIndex = vmActiveIndex - 1; // decrement by 1, since it was incremented by 1
		var thumbnailIndex = swiper.slides[activeIndex];

		if (slideToFlag) {
			swiper.slideTo(activeIndex);
		}

		$(swiper.slides).removeClass('swiper-slide-active slide--focus');
		$(thumbnailIndex).addClass('swiper-slide-active slide--focus');
	}

	function setDetails(swiper, viewModel) {
		var activeSlideIndex = swiper.activeIndex;
		var activeSlideTitle = $(swiper.slides[activeSlideIndex]).find('img').attr('alt');

		viewModel.activeIndex(activeSlideIndex + 1);
		viewModel.activeTitle(activeSlideTitle);
	}

	return {
		setDetails: setDetails,
		focusThumbnail: focusThumbnail
	}
})();
