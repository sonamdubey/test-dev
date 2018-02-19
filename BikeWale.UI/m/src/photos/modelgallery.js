var modelColorImageCount = 0,
    modelImages = [],
    modelColorImages = [],
    videoList = null;
var eleGallery, vmModelGallery, colorIndex = 0, galleryRoot;
var photoCount, videoCount, modelName, bikeModelId, imageIndex, colorImageId, returnUrl, isColorImageSet = false;
var thumbnailSwiperEvents, gallerySwiper, colorGallerySwiper, thumbnailSwiper, colorThumbnailSwiper, videoThumbnailSwiper, videoListEvents;
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

    self.isGalleryActive = ko.observable(false);

    self.photosTabActive = ko.observable(true);
    self.colorTabActive = ko.observable(true);
    self.photoSwiperActive = ko.observable(true);

    self.galleryTabsActive = ko.observable(true);
    self.galleryFooterActive = ko.observable(true);
    self.photoHeadingActive = ko.observable(true);
    self.fullScreenModeActive = ko.observable(false);
    self.fullscreenSupport = ko.observable(true && "orientation" in screen);

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

    self.activeVideoTitle = ko.observable('');
    self.activeVideoIndex = ko.observable(0);
    self.activeVideoId = ko.observable();

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

    self.togglePhotoTab = function () {
        if (self.screenActive()) {
            self.deactivateAllScreens();
        }

        if (!self.photosTabActive()) {
            self.photosTabActive(true);
            videoListEvents.setVideoURL(''); // empty the embedded video url
        }
        else {
            self.photosTabActive(false);
            toggleFullScreen(false);
            triggerGA('Gallery_Page', 'Videos_Clicked', modelName);
            if (self.videoList().length) {
                if (!self.activeVideoId()) {
                    videoListEvents.setVideo(0); // set 1st video from the list
                    return;
                }
                videoListEvents.setVideo(self.activeVideoIndex() - 1);
            }
        }
    };

    // all photos tab
    self.togglePhotoThumbnailScreen = function () {
        if (!self.photoThumbnailScreen()) {
            self.deactivateAllScreens();

            self.photoSwiperActive(true);
            self.photoThumbnailScreen(true);

            if (colorImageId > 0) {
                gallerySwiper.update(true);
            }

            thumbnailSwiper.update(true);
            thumbnailSwiperEvents.focusThumbnail(thumbnailSwiper, self.activePhotoIndex(), true); // (swiperName, activeIndex, slideToFlag)
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
            if (navigator.userAgent.indexOf('UCBrowser/') >= 0) {
                galleryRoot.addClass('uc-iframe-position');
            }
            self.modelInfoScreen(true);
            triggerGA('Gallery_Page', 'Info_Tab_Clicked', modelName);
        }
        else {
            self.modelInfoScreen(false);
        }

        self.screenActive(self.modelInfoScreen());
    };

    // video
    self.toggleVideoListScreen = function () {
        if (!self.videoListScreen()) {
            self.deactivateAllScreens();
            self.videoListScreen(true);

            videoListEvents.focusThumbnail($('.video-tab-list'), self.activeVideoIndex());
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
        self.photosTabActive(true);
        self.photoSwiperActive(true);
        self.deactivateAllScreens();
        videoListEvents.setVideoURL('');
    }

    self.hideGalleryTabs = function () {
        self.galleryTabsActive(false);
    };

    self.showGalleryTabs = function () {
        self.galleryTabsActive(true);
    };

    self.hideFooterTabs = function () {
        self.galleryFooterActive(false);
    };

    self.showFooterTabs = function () {
        self.galleryFooterActive(true);
    };

    self.toggleFooterTabs = function () {
        self.galleryFooterActive() ? self.hideFooterTabs() : self.showFooterTabs();
    };

    self.hidePhotoHeading = function () {
        self.photoHeadingActive(false);
    };

    self.showPhotoHeading = function () {
        self.photoHeadingActive(true);
    };

    self.togglePhotoHeading = function () {
        self.photoHeadingActive() ? self.hidePhotoHeading() : self.showPhotoHeading();
    };

    self.toggleFullScreen = function () {
        self.deactivateAllScreens();

        if (!self.fullScreenModeActive()) {
            toggleFullScreen(true);
            self.fullScreenModeActive(true);

            if ("orientation" in screen && screen.orientation.type == 'portrait-primary') {
                screen.orientation.unlock();
                screen.orientation.lock('landscape-primary');

                self.hideFooterTabs();
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
        popupGallery.open();
        gallerySwiper.update(true);
        if (colorImageId > 0) {
            thumbnailSwiperEvents.focusGallery(colorGallerySwiper, colorIndex);
        }
        else {
            thumbnailSwiperEvents.focusGallery(gallerySwiper, imageIndex);
        }

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
        vmModelGallery.hideGalleryTabs();
        vmModelGallery.hideFooterTabs();
        vmModelGallery.hidePhotoHeading();

        if (!vmModelGallery.photosTabActive()) {
            $('.main-video-iframe-content').css({ 'padding-bottom': '44%' });
        }
    }
    else {
        vmModelGallery.showGalleryTabs();
        vmModelGallery.showFooterTabs();
        vmModelGallery.showPhotoHeading();

        if (vmModelGallery.photosTabActive()) {
            $('.main-video-iframe-content').css({ 'padding-bottom': '56.25%' });
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

            label = 'modelId=' + bikeModelId + '|imageid=' + imageid + lb + '|pageid=' + (gaObj ? gaObj.id : 0);
            cwTracking.trackImagesInteraction("BWImages", "ImageViewed", label);
            triggerVirtualPageView(window.location.host, window.location.pathname, lb);
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

        setPhotoDetails: function (swiper) {
            var activeSlide = swiper.slides[swiper.activeIndex],
            activeSlideTitle = $(activeSlide).find('img').attr('alt');

            vmModelGallery.activePhotoIndex(swiper.activeIndex + 1);
            vmModelGallery.activePhotoTitle(activeSlideTitle);
        },

        setColorPhotoDetails: function (swiper) {
            var activeSlide = 0;
            if (colorImageId > 0 && !isColorImageSet) {
                activeSlide = swiper.slides[colorIndex];
                vmModelGallery.activeColorIndex(colorIndex + 1);
                isColorImageSet = true;
            }
            else {
                activeSlide = swiper.slides[swiper.activeIndex];
                vmModelGallery.activeColorIndex(swiper.activeIndex + 1);
            }

            var activeSlideTitle = $(activeSlide).find('img').attr('alt');
            vmModelGallery.activeColorTitle(activeSlideTitle);
        }
    };

    videoListEvents = {

        focusThumbnail: function (listElement, vmActiveIndex) {
            var itemElement = listElement.find('li:eq(' + (vmActiveIndex - 1) + ')');

            $(itemElement).siblings().removeClass('active');
            $(itemElement).addClass('active');
        },

        setVideo: function (elementIndex) {
            var elementId = vmModelGallery.videoList()[elementIndex].VideoId,
                elementTitle = vmModelGallery.videoList()[elementIndex].VideoTitle;

            vmModelGallery.activeVideoId(elementId);
            vmModelGallery.activeVideoIndex(elementIndex + 1);
            vmModelGallery.activeVideoTitle(elementTitle);

            videoListEvents.setVideoURL(vmModelGallery.activeVideoId());
        },

        setVideoURL: function (videoId) {
            (document.getElementById("iframe-video").contentWindow || document.getElementById("iframe-video").documentWindow).location.replace('https://www.youtube.com/embed/' + videoId + '?showinfo=0');
        }
    };

    // initialize swipers
    gallerySwiper = new Swiper('#main-photo-swiper', {
        spaceBetween: 0,
        preloadImages: false,
        lazyLoading: true,
        lazyLoadingInPrevNext: true,
        nextButton: '.gallery-type-next',
        prevButton: '.gallery-type-prev',
        onInit: function (swiper) {
            swiper.slideTo(vmModelGallery.activePhotoIndex());
            thumbnailSwiperEvents.setPhotoDetails(swiper);
        },
        onClick: function () {
            if (window.innerWidth > window.innerHeight) {
                vmModelGallery.toggleFooterTabs();
                vmModelGallery.togglePhotoHeading();
            }
            if (vmModelGallery.screenActive()) {
                vmModelGallery.deactivateAllScreens();
            }
        },
        onTouchStart: function () {
            if (vmModelGallery.screenActive()) {
                vmModelGallery.deactivateAllScreens();
            }
        },
        onSlideChangeStart: function (swiper) {
            thumbnailSwiperEvents.setPhotoDetails(swiper);
            swiper.lazy.load();
            triggerGA('Gallery_Page', 'Image_Carousel_Clicked', null);
        },
        onSlideChangeEnd: function (swiper)
        {
            logBhrighuForImage($('#main-photo-swiper .swiper-slide-active'));
        }
    });

    colorGallerySwiper = new Swiper('#main-color-swiper', {
        spaceBetween: 0,
        preloadImages: false,
        lazyLoading: true,
        lazyLoadingInPrevNext: true,
        nextButton: '.color-type-next',
        prevButton: '.color-type-prev',
        onInit: function (swiper) {
            thumbnailSwiperEvents.setColorPhotoDetails(swiper);
        },
        onClick: function () {
            if (window.innerWidth > window.innerHeight) {
                vmModelGallery.toggleFooterTabs();
                vmModelGallery.togglePhotoHeading();
            }
            if (vmModelGallery.modelInfoScreen()) {
                vmModelGallery.modelInfoScreen(false);
            }
        },
        onSlideChangeStart: function (swiper) {
            thumbnailSwiperEvents.setColorPhotoDetails(swiper);
            thumbnailSwiperEvents.focusThumbnail(colorThumbnailSwiper, vmModelGallery.activeColorIndex(), true);
            triggerGA('Gallery_Page', 'Colour_Changed', modelName);
            if (vmModelGallery.modelInfoScreen()) {
                vmModelGallery.modelInfoScreen(false);
            }
        },
        onSlideChangeEnd: function (swiper) {
            logBhrighuForImage($('#main-color-swiper .swiper-slide-active'));
        }
    });

    thumbnailSwiper = new Swiper('#thumbnail-photo-swiper', {
        spaceBetween: 0,
        slidesPerView: 'auto',
        preloadImages: false,
        lazyLoading: true,
        lazyLoadingInPrevNext: true,
        watchSlidesVisibility: true
    });

    colorThumbnailSwiper = new Swiper('#thumbnail-colors-swiper', {
        spaceBetween: 0,
        slidesPerView: 'auto'
    });

    //set imagecolor wise image
    if (colorImageId > 0) {
        ko.utils.arrayForEach(modelColorImages, function (item, index) {
            if (item.ColorId == colorImageId) { colorIndex = index; }
        });
        vmModelGallery.activeColorIndex(colorIndex);
        thumbnailSwiperEvents.focusGallery(colorGallerySwiper, colorIndex);
    }


    window.addEventListener('resize', resizeHandler, true);
    resizeHandler();


    $('#gallery-close-btn').on('click', function () {
        popupGallery.close();
    });

    // thumbnail swiper event
    $('.thumbnail-swiper .swiper-slide').on('click', function () {
        var slideIndex = $(this).index();
        thumbnailSwiperEvents.focusGallery(gallerySwiper, slideIndex);
        thumbnailSwiperEvents.focusThumbnail(thumbnailSwiper, vmModelGallery.activePhotoIndex(), true);
    });

    // color thumbnail swiper event
    $('.color-thumbnail-swiper .swiper-slide').on('click', function () {
        var slideIndex = $(this).index();
        thumbnailSwiperEvents.focusGallery(colorGallerySwiper, slideIndex);
        thumbnailSwiperEvents.focusThumbnail(colorThumbnailSwiper, vmModelGallery.activeColorIndex(), true);
    });

    // video thumbnail list event
    $('#video-tab-screen').on('click', '.video-tab-list li', function () {
        var elementIndex = $(this).index();
        videoListEvents.setVideo(elementIndex);
        videoListEvents.focusThumbnail($('.video-tab-list'), vmModelGallery.activeVideoIndex());
    });


});

