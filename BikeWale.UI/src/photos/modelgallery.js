var cacheData,
    modelColorImageCount = 0,
    modelImages = [],
    modelColorImages = [],
    videoList = null;
var pageNo = 1,pageSize = 4,eleGallery, vmModelGallery,colorIndex = 0,isIEBrowser;
var photoCount, videoCount, modelName, imageIndex, colorImageId, returnUrl, bikeModelId;
var thumbnailSwiperEvents, gallerySwiper, colorGallerySwiper, thumbnailSwiper, colorThumbnailSwiper, videoThumbnailSwiper;
var imageTypes = ["Other", "ModelImage", "ModelGallaryImage", "ModelColorImage"];

var popupGallery = {
    open: function () {
        vmModelGallery.isGalleryActive(true);
        lockPopup();

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
            unlockPopup();
        }
    },

    bindGallery: function (imageIndex) {
        triggerGA('Gallery_Page', 'Gallery_Loaded', modelName);
        popupGallery.open();
        gallerySwiper.update(true);
        thumbnailSwiperEvents.focusGallery(gallerySwiper, imageIndex);

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

function detectIEBrowser() {
    var divElement = document.createElement("div");
    divElement.innerHTML = "<!--[if lt IE 10]><i></i><![endif]-->";
    var isIELessThan10 = (divElement.getElementsByTagName("i").length == 1);
    if (isIELessThan10) {
        $('body').addClass('ie-lt-9-browser');
        return true;
    }
};

var modelGallery = function () {
    var self = this;

    self.isGalleryActive = ko.observable(false);

    self.photosTabActive = ko.observable(true);
    self.colorTabActive = ko.observable(true);
    self.photoSwiperActive = ko.observable(true);

    // footer screens
    self.screenActive = ko.observable(false);
    self.photoThumbnailScreen = ko.observable(false);
    self.colorsThumbnailScreen = ko.observable(false);
    self.modelInfoScreen = ko.observable(false);
    self.videoListScreen = ko.observable(false);

    self.activePhotoTitle = ko.observable('');
    self.activePhotoIndex = ko.observable(0);

    self.activeColorTitle = ko.observable('');
    self.activeColorIndex = ko.observable(0);
    self.colorTabActive(modelColorImageCount == 0 ? false : true);

    self.photoList = ko.observableArray(modelImages);
    self.colorPhotoList = ko.observableArray(modelColorImages);

    self.renderImage = function (hostUrl, originalImagePath, imageSize) {
        if (originalImagePath && originalImagePath != null) {
            return (hostUrl + '/' + imageSize + '/' + originalImagePath);
        }
        else {
            return ('https://imgd.aeplcdn.com/' + imageSize + '/bikewaleimg/images/noimage.png?q=70');
        }
    }

    // video
    self.activeVideoTitle = ko.observable('');
    self.activeVideoIndex = ko.observable(0);
    self.activeVideoId = ko.observable();

    self.videoList = ko.observableArray(videoList);
    self.totalVideoCount = videoCount;

    self.togglePhotoTab = function () {
        if (!self.photosTabActive()) {
            self.photosTabActive(true);
            self.activeVideoId('');
        }
        else {
            self.photosTabActive(false);
            triggerGA('Gallery_Page', 'Videos_Clicked', modelName);
            if (self.videoList().length) {
                if (!self.activeVideoId()) {
                    setVideo(0);
                }
                setVideo(self.activeVideoIndex() - 1);
            }
        }
        if (self.screenActive()) {
            self.deactivateAllScreens();
        }
    };

    // all photos tab
    self.togglePhotoThumbnailScreen = function () {
        if (!self.photoThumbnailScreen()) {
            // deactivate all other screens
            self.deactivateAllScreens();
            // activate clicked tab screen
            self.photoThumbnailScreen(true);
            self.photoSwiperActive(true);

            gallerySwiper.update(true);
            thumbnailSwiper.update(true);
            thumbnailSwiperEvents.focusThumbnail(thumbnailSwiper, vmModelGallery.activePhotoIndex(), true); // (swiperName, activeIndex, slideToFlag)
            triggerGA('Gallery_Page', 'All_Photos_Tab_Clicked_Opened', modelName);
        }
        else {
            self.photoThumbnailScreen(false);
            triggerGA('Gallery_Page', 'All_Photos_Tab_Clicked_Closed', modelName);
        }

        self.screenActive(self.photoThumbnailScreen());
    };

    self.toggleColorThumbnailScreen = function () {
        if (!self.colorsThumbnailScreen()) {
            self.deactivateAllScreens();
            self.colorsThumbnailScreen(true);
            self.photoSwiperActive(false);

            colorGallerySwiper.update(true);
            colorThumbnailSwiper.update(true);
            thumbnailSwiperEvents.focusThumbnail(colorThumbnailSwiper, self.activeColorIndex(), true);
            triggerGA('Gallery_Page', 'Colours_Tab_Clicked_Opened', modelName);
        }
        else {
            self.colorsThumbnailScreen(false);
            triggerGA('Gallery_Page', 'Colours_Tab_Clicked_Closed', modelName);
        }

        self.screenActive(self.colorsThumbnailScreen());

    };

    self.toggleModelInfoScreen = function () {
        if (!self.modelInfoScreen()) {
            self.deactivateAllScreens();
            self.modelInfoScreen(true);
            triggerGA('Gallery_Page', 'Info_Tab_Clicked', modelName);
        }
        else {
            self.modelInfoScreen(false);
        }

        self.screenActive(self.modelInfoScreen());
    };

    self.toggleVideoListScreen = function () {
        if (!self.videoListScreen()) {
            self.deactivateAllScreens();
            self.videoListScreen(true);
            videoThumbnailSwiper.update(true);
            thumbnailSwiperEvents.focusThumbnail(videoThumbnailSwiper, self.activeVideoIndex(), true);
            triggerGA('Gallery_Page', 'All_Videos_Tab_Clicked_Opened', modelName);
        }
        else {
            self.videoListScreen(false);
            triggerGA('Gallery_Page', 'All_Videos_Tab_Clicked_Closed', modelName);
        }
        self.screenActive(self.videoListScreen());
    };

    self.deactivateAllScreens = function () {
        self.photoThumbnailScreen(false);
        self.colorsThumbnailScreen(false);
        self.modelInfoScreen(false);
        self.videoListScreen(false);
    };

    self.resetGallery = function () {
        self.activeVideoId('');
        self.photosTabActive(true);
        self.photoSwiperActive(true);
        self.deactivateAllScreens();
    }

};


function setVideo(elementIndex) {
    var elementId = vmModelGallery.videoList()[elementIndex].VideoId,
        elementTitle = vmModelGallery.videoList()[elementIndex].VideoTitle;

    vmModelGallery.activeVideoId(elementId);
    vmModelGallery.activeVideoIndex(elementIndex + 1);
    vmModelGallery.activeVideoTitle(elementTitle);

    (document.getElementById("iframe-video").contentWindow || document.getElementById("iframe-video").documentWindow).location.replace('https://www.youtube.com/embed/' + vmModelGallery.activeVideoId() + '?showinfo=0');

};

var hashChange = function (e) {
    var oldUrl, oldHash;
    oldUrl = e.originalEvent.oldURL;
    if (oldUrl && (oldUrl.indexOf('#') > 0)) {
        oldHash = oldUrl.split('#')[1];
        closePopUp(oldHash);
    };
};

var closePopUp = function (state) {
    if (state == "photosGallery")
        popupGallery.close();
};

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
        isIEBrowser = detectIEBrowser();

    } catch (e) {
        console.warn(e);
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
        }
    }

}

docReady(function () {

    setPageVariables();


    vmModelGallery = new modelGallery();
    var eleGalleryRoot = $("#gallery-root");
    if (eleGalleryRoot.length > 0) {
        ko.applyBindings(vmModelGallery, eleGalleryRoot[0]);
    }


    // model gallery thumbnail events namespace
    thumbnailSwiperEvents = {

        focusGallery: function (swiper, elementIndex) {
            swiper.slideTo(elementIndex);
        },

        focusThumbnail: function (swiper, vmActiveIndex, slideToFlag) {
            var activeIndex = vmActiveIndex - 1, // decrement by 1, since it was incremented by 1
                thumbnailIndex = swiper.slides[activeIndex];

            if (slideToFlag) {
                swiper.slideTo(activeIndex);
            }
            $(swiper.slides).removeClass('swiper-slide-active focus-slide');
            $(thumbnailIndex).addClass('swiper-slide-active focus-slide');
        },

        slideNext: function (swiper, slideCount) {
            var activeIndex = swiper.activeIndex;
            swiper.slideTo(activeIndex + slideCount - 1); // decrement by 1, since index starts from 0
        },

        slidePrev: function (swiper, slideCount) {
            var activeIndex = swiper.activeIndex;
            swiper.slideTo(activeIndex - slideCount + 1);
        },

        setPhotoDetails: function (swiper) {
            var activeSlide = swiper.slides[swiper.activeIndex],
            activeSlideTitle = $(activeSlide).find('img').attr('alt');

            vmModelGallery.activePhotoIndex(swiper.activeIndex + 1);

            vmModelGallery.activePhotoTitle(modelName + " " + activeSlideTitle);
        },

        setColorPhotoDetails: function (swiper) {
            var activeSlide = swiper.slides[swiper.activeIndex],
                activeSlideTitle = $(activeSlide).find('img').attr('alt');

            vmModelGallery.activeColorIndex(swiper.activeIndex + 1);
            vmModelGallery.activeColorTitle(modelName + " " + activeSlideTitle);
        }
    }

    // initialize swipers
    gallerySwiper = new Swiper('.gallery-type-swiper', {
        nextButton: '.gallery-type-next',
        prevButton: '.gallery-type-prev',
        slidesPerView: 'auto',
        preloadImages: false,
        lazyLoading: true,
        centeredSlides: true,
        spaceBetween: 0,
        keyboardControl: true,
        lazyLoadingInPrevNext: true,
        mousewheelControl: true,
        onInit: function (swiper) {
            swiper.slideTo(vmModelGallery.activePhotoIndex());
            thumbnailSwiperEvents.setPhotoDetails(swiper);
        },
        onTouchStart: function () {
            if (vmModelGallery.screenActive()) {
                vmModelGallery.deactivateAllScreens();
            }
        },
        onSlideChangeStart: function (swiper) {
            thumbnailSwiperEvents.setPhotoDetails(swiper);
            thumbnailSwiperEvents.focusThumbnail(thumbnailSwiper, vmModelGallery.activePhotoIndex(), true);
        },
        onSlideChangeEnd: function (swiper) {
            logBhrighuForImage($('.gallery-type-swiper .swiper-slide-active').first());
        }
    });

    colorGallerySwiper = new Swiper('.gallery-color-type-swiper', {
        nextButton: '.color-type-next',
        prevButton: '.color-type-prev',
        slidesPerView: 'auto',
        preloadImages: false,
        lazyLoading: true,
        centeredSlides: true,
        spaceBetween: 0,
        keyboardControl: true,
        lazyLoadingInPrevNext: true,
        mousewheelControl: true,
        onInit: function (swiper) {
            thumbnailSwiperEvents.setColorPhotoDetails(swiper);
            logBhrighuForImage($('.gallery-type-swiper .swiper-slide-active').first());
        },
        onTouchStart: function () {
            if (vmModelGallery.modelInfoScreen()) {
                vmModelGallery.modelInfoScreen(false);
            }
        },
        onSlideChangeStart: function (swiper) {
            thumbnailSwiperEvents.setColorPhotoDetails(swiper);
            thumbnailSwiperEvents.focusThumbnail(colorThumbnailSwiper, vmModelGallery.activeColorIndex(), true);
            triggerGA('Gallery_Page', 'Colour_Changed', modelName);
        },
        onSlideChangeEnd: function (swiper) {
            logBhrighuForImage($('.gallery-color-type-swiper .swiper-slide-active').first());
        }
    });


    thumbnailSwiper = new Swiper('.thumbnail-swiper', {
        nextButton: '.thumbnail-type-next',
        prevButton: '.thumbnail-type-prev',
        slidesPerView: 6.8,
        preloadImages: false,
        lazyLoading: true,
        spaceBetween: 0
    });

    colorThumbnailSwiper = new Swiper('.color-thumbnail-swiper', {
        nextButton: '.color-thumbnail-type-next',
        prevButton: '.color-thumbnail-type-prev',
        slidesPerView: 5.5,
        spaceBetween: 0
    });

    videoThumbnailSwiper = new Swiper('.video-thumbnail-swiper', {
        nextButton: '.video-type-next',
        prevButton: '.video-type-prev',
        slidesPerView: 2.35,
        preloadImages: false,
        lazyLoading: true,
        spaceBetween: 0
    });



    

    $(window).on('hashchange', function (e) {
        hashChange(e);
    });

    $(document).on('click', '#gallery-close-btn', function () {
        popupGallery.close();
    });

    $(document).keydown(function (event) {
        if (event.keyCode == 27) {
            popupGallery.close();
        }
    });

    // thumbnail swiper events listener
    $(document).on('click', '.thumbnail-swiper .thumbnail-type-prev', function () {
        thumbnailSwiperEvents.slidePrev(thumbnailSwiper, 6); // (swiperName, no. of slides to scroll)
    });

    $(document).on('click', '.thumbnail-swiper .thumbnail-type-next', function () {
        thumbnailSwiperEvents.slideNext(thumbnailSwiper, 6);
    });

    $(document).on('click', '.thumbnail-swiper .swiper-slide', function () {
        var slideIndex = $(this).index();
        thumbnailSwiperEvents.focusGallery(gallerySwiper, slideIndex);
        thumbnailSwiperEvents.focusThumbnail(thumbnailSwiper, vmModelGallery.activePhotoIndex(), false);
    });

    // color thumbnail swiper events listener
    $(document).on('click', '.color-thumbnail-swiper .color-type-prev', function () {
        thumbnailSwiperEvents.slidePrev(colorThumbnailSwiper, 5);
    });

    $(document).on('click', '.color-thumbnail-swiper .color-type-next', function () {
        thumbnailSwiperEvents.slideNext(colorThumbnailSwiper, 5);
    });

    $(document).on('click', '.color-thumbnail-swiper .swiper-slide', function () {
        var slideIndex = $(this).index();
        thumbnailSwiperEvents.focusGallery(colorGallerySwiper, slideIndex);
    });

    // video thumbnail swiper events listener
    $(document).on('click', '.video-thumbnail-swiper .video-type-prev', function () {
        thumbnailSwiperEvents.slidePrev(videoThumbnailSwiper, 2);
    });

    $(document).on('click', '.video-thumbnail-swiper .video-type-next', function () {
        thumbnailSwiperEvents.slideNext(videoThumbnailSwiper, 2);
    });

    $(document).on('click', '#video-tab-screen .video-thumbnail-swiper .swiper-slide', function () {
        var elementIndex = $(this).index();
        setVideo(elementIndex);
        thumbnailSwiperEvents.focusThumbnail(videoThumbnailSwiper, vmModelGallery.activeVideoIndex(), true);
    });

    if (!isIEBrowser) {
        (function () {
            try {
                if (colorImageId > 0) {

                    ko.utils.arrayForEach(modelColorImages, function (item, index) {
                        if (item.ColorId == colorImageId) { colorIndex = index; }
                    });
                    vmModelGallery.activeColorIndex(colorIndex);
                    thumbnailSwiperEvents.focusGallery(colorGallerySwiper, colorIndex);
                }
            } catch (e) {
                console.warn(e);
            }
        })();
    }

});



