var modelColorImageCount = 0,
    modelImages = [],
    modelColorImages = [],
    videoList = null;
var eleGallery, vmModelGallery, colorIndex = 0, galleryRoot;
var photoCount, videoCount, modelName, bikeModelId, imageIndex, colorImageId, returnUrl, isColorImageSet = false;
var imageTypes = ["Other","ModelImage", "ModelGallaryImage", "ModelColorImage"];

var setPageVariables = function () {
    eleGallery = $("#pageGallery");

    try {
        if (eleGallery.length > 0 && eleGallery.data("images") != '')
        {
            var imageList = JSON.parse(Base64.decode(eleGallery.data("images")));
            modelImages = imageList;
            modelColorImages = filterColorImagesArray(imageList);

            if (modelColorImages)
                modelColorImageCount = modelColorImages.length;
        }

        if (eleGallery.length > 0 && eleGallery.data("videos") != '')
        {
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

    self.activePhotoTitle = ko.observable('');
    self.activePhotoIndex = ko.observable(1);

    self.activeSwiper = ko.observable(false);

    self.HideSwiperTitle = ko.observable(false);

    self.galleryFooterActive = ko.observable(true);

    self.activeGalleryScreenRotateSlug = ko.observable(true);
    self.galleryColorFixedSlug = ko.observable(true);
    self.activeGalleryColor = ko.observable(false);
    self.ActiveLandscapeIcon = ko.observable(false);

    self.photoList = ko.observableArray(modelImages);
    self.colorPhotoList = ko.observableArray(modelColorImages);
    self.videoList = ko.observableArray(videoList);

    self.renderImage = function (hostUrl, originalImagePath, imageSize)
    {
        if(originalImagePath && originalImagePath!=null)
        {
            return (hostUrl + '/' + imageSize + '/' + originalImagePath);
        }
        else
        {
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
};

var popupGallery = {
    open: function () {
        vmModelGallery.isGalleryActive(true);
        if (navigator.userAgent.indexOf('UCBrowser/11') >= 0) {
            vmModelGallery.fullscreenSupport(false);
        }
        $('body').addClass('lock-browser-scroll');

        if (colorImageId > 0) {
            if (vmModelGallery.activeColorIndex() == 0) vmModelGallery.activeColorIndex(1);
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
	}
	else {
	    vmModelGallery.fullScreenModeActive(false);
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

            label = 'modelId=' + bikeModelId + '|imageid=' + imageid + lb + '|pageid=' + (gaObj ? gaObj.id : 0);
            cwTracking.trackImagesInteraction("BWImages", "ImageViewed", label);
            triggerVirtualPageView(window.location.host + window.location.pathname, lb);
        }
    }

}

docReady(function () {

    galleryRoot = $('#gallery-root');

    setPageVariables();

    vmModelGallery = new modelGallery();

    if (galleryRoot.length > 0) {
        ko.applyBindings(vmModelGallery, galleryRoot[0]);
    }

    var mainGallerySwiper = new Swiper("#mainPhotoSwiper", {
      spaceBetween: 0,
      preloadImages: false,
      lazyLoading: true,
      lazyLoadingInPrevNext: true,
      nextButton: ".gallery--next",
      prevButton: ".gallery--prev",
      onInit: function(swiper) {
        swiper.slideTo(vmModelGallery.activePhotoIndex() - 1);
        var activeSlide = swiper.slides[swiper.activeIndex];
        activeSlideTitle = $(activeSlide).find('img').attr('alt');
        vmModelGallery.activePhotoTitle(activeSlideTitle);

			},
			onClick: function (swiper) {
				if(swiper.activeIndex < 5) {
					gallerySlug.setColor($(swiper.slides[4]));
				}
				resizeHandler();
			},
			onTransitionStart: function (swiper) {
				var activeSlide = swiper.slides[swiper.activeIndex];

				if ($(activeSlide).hasClass('swiper-slide__slug')) {
					$(swiper.container).find('.gallery-image__footer').hide();
				}
				else {
					$('.gallery-image__footer').show();
				}

			},
      onSlideChangeEnd: function (swiper) {
        vmModelGallery.activePhotoIndex(swiper.activeIndex + 1);
        logBhrighuForImage($("#mainPhotoSwiper .swiper-slide-active"));

      	// remove embedded slug
        var activeSlide = swiper.slides[swiper.activeIndex];
        var sliderSlugSlide = $(activeSlide).prev('.swiper-slide__slug');

        if (sliderSlugSlide.length) {
					gallerySlug.removeSlug(sliderSlugSlide);
        }

        if (swiper.activeIndex === 5) {
            var element = $('.model-gallery__screen--rotate-slug .screen--rotate-slug__landscape-icon');
            var elementTopPosition = ($('.gallery-image__footer').offset().top - $('.model-gallery__screen--rotate-slug').offset().top + 10);
            var elementRightPosition = element.width() + element.offset().left - $(window).width() + $('.landscape-icon').width();
            element.css('position', 'fixed').animate({ 'top': elementTopPosition + 'px', 'right': elementRightPosition + 'px' }, 1000, "swing");
            $('.model-gallery__screen--rotate-slug').addClass('model-gallery__screen--rotate-slug--hide');
            setTimeout(function () {
                vmModelGallery.activeGalleryScreenRotateSlug(false);
                

            }, 1500);
            vmModelGallery.ActiveLandscapeIcon(true);
            //$('.landscape-icon__container').animate({ 'width': '40px' }, 800);
        }

        if (swiper.activeIndex === 6) {
            vmModelGallery.activeGalleryColor(true);
        }
        activeSlideTitle = $(activeSlide).find('img').attr('alt');
        vmModelGallery.activePhotoTitle(activeSlideTitle);
      }
    });

    window.addEventListener('resize', resizeHandler, true);
    //resizeHandler();
});

function resizeHandler() {
    if (window.innerWidth < window.innerHeight) {
        vmModelGallery.showFooterTabs();
    }
    else {
        vmModelGallery.toggleFooterTabs();
    }
};


var gallerySlug = (function() {
  function setColor(slideElement) {
		if (!slideElement.hasClass(slideElement)) {
			slideElement.addClass("swiper-slide__slug");
			slideElement.append($("#carouselColorSlug").html());
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
