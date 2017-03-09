var cacheData,
    modelColorImageCount = 0,
    modelImages = [],
    modelColorImages = [],
    videoList = null;

(function () {
    try {
        var imageList = JSON.parse(Base64.decode(encodedImageList));

        videoList = JSON.parse(Base64.decode(encodedVideoList));
        modelImages = imageList;
        modelColorImages = filterColorImagesArray(imageList);

        if (modelColorImages)
            modelColorImageCount = modelColorImages.length;
    } catch (e) {
        console.warn(e);
    }
})();

var popupGallery = {
    open: function () {
        vmModelGallery.isGalleryActive(true);
        if (navigator.userAgent.indexOf('UCBrowser/11') >= 0) {
            vmModelGallery.fullscreenSupport(false);
        }
        $('body').addClass('lock-browser-scroll');
    },

    close: function () {
        if (isModelPage) {
            window.location.href = window.location.pathname.split("images/")[0];
        }
        else {
            vmModelGallery.isGalleryActive(false);
            vmModelGallery.resetGallery();
            $('body').removeClass('lock-browser-scroll');
            toggleFullScreen(false);
        }
    },

    bindGallery: function (imageIndex) {
        //triggerGA('Gallery_Page', 'Gallery_Loaded', modelName);
        popupGallery.open();
        gallerySwiper.update(true);
        thumbnailSwiperEvents.focusGallery(gallerySwiper, imageIndex);

        if (!isModelPage) {
            window.location.hash = 'photosGallery';
        }
    }
}

var galleryRoot = $('#gallery-root');

$('.photos-grid-list').on('click', 'li', function () {
    if (photoCount > 1) {
        galleryRoot.find('.gallery-loader-placeholder').show();

        var imageIndex = $(this).index(),
            parentGridType = $(this).closest('.photos-grid-list');

        if (parentGridType.hasClass('remainder-grid-list')) {
            var gridOneLength = $('.photos-grid-list').first().find('li').length;

            imageIndex = gridOneLength + imageIndex; // (grid type 1's length + grid type remainder's index)
        }

        popupGallery.bindGallery(imageIndex);
        galleryRoot.find('.gallery-loader-placeholder').hide();
    }
});

$('#gallery-close-btn').on('click', function () {
    popupGallery.close();
});

$('.photos-grid-list img.lazy').on('load', function () {
    resizePortraitImage($(this));
});

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

$(document).ready(function () {
    var photosLength = $('.photos-grid-list').first().find('li').length,
        photosLimit = 30,
        lastPhotoIndex = photosLimit - 1;

    // add 'more photos count' if photo grid contains 30 images
    if (photosLength == photosLimit) {
        var lastPhoto = $('.photos-grid-list li').eq(lastPhotoIndex),
            morePhotoCount = $('<span class="black-overlay"><span class="font14 text-white">+' + (photoCount - lastPhotoIndex) + '<br />images</span></span>');
        lastPhoto.append(morePhotoCount);
    }
});

function filterColorImagesArray(responseArray) {
    return ko.utils.arrayFilter(responseArray, function (response) {
        return response.ImageType == 3;
    });
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
    self.fullscreenSupport = ko.observable(true);

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
            //triggerGA('Gallery_Page', 'Videos_Clicked', modelName);
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

            //gallerySwiper.update(true);
            thumbnailSwiper.update(true);
            thumbnailSwiperEvents.focusThumbnail(thumbnailSwiper, vmModelGallery.activePhotoIndex(), true); // (swiperName, activeIndex, slideToFlag)
            //triggerGA('Gallery_Page', 'All_Photos_Tab_Clicked_Opened', modelName);
        }
        else {
            self.photoThumbnailScreen(false);
            //triggerGA('Gallery_Page', 'All_Photos_Tab_Clicked_Closed', modelName);
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
            thumbnailSwiperEvents.focusThumbnail(colorThumbnailSwiper, vmModelGallery.activeColorIndex(), true);
            //triggerGA('Gallery_Page', 'Colours_Tab_Clicked_Opened', modelName);
        }
        else {
            self.colorsThumbnailScreen(false);
            //triggerGA('Gallery_Page', 'Colours_Tab_Clicked_Closed', modelName);
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
            //triggerGA('Gallery_Page', 'Info_Tab_Clicked', modelName);
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

            videoListEvents.focusThumbnail($('.video-tab-list'), vmModelGallery.activeVideoIndex());
            //triggerGA('Gallery_Page', 'All_Videos_Tab_Clicked_Opened', modelName);
        }
        else {
            self.videoListScreen(false);
            //triggerGA('Gallery_Page', 'All_Videos_Tab_Clicked_Closed', modelName);
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
        fadeOutFooterTabs();
        self.deactivateAllScreens();
        if (!self.fullScreenModeActive()) {
            toggleFullScreen(true);
            self.fullScreenModeActive(true);
        }
        else {
            toggleFullScreen(false);
            self.fullScreenModeActive(false);
        }
    };
};

var vmModelGallery = new modelGallery();

ko.applyBindings(vmModelGallery, document.getElementById('gallery-root'));

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

// model gallery thumbnail events namespace
var thumbnailSwiperEvents = {

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
        var activeSlide = swiper.slides[swiper.activeIndex],
            activeSlideTitle = $(activeSlide).find('img').attr('alt');

        vmModelGallery.activeColorIndex(swiper.activeIndex + 1);
        vmModelGallery.activeColorTitle(activeSlideTitle);
    }
};

var videoListEvents = {

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
var gallerySwiper = new Swiper('#main-photo-swiper', {
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
    }
});

var colorGallerySwiper = new Swiper('#main-color-swiper', {
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
        //triggerGA('Gallery_Page', 'Colour_Changed', modelName);
        if (vmModelGallery.modelInfoScreen()) {
            vmModelGallery.modelInfoScreen(false);
        }
    }
});

var thumbnailSwiper = new Swiper('#thumbnail-photo-swiper', {
    spaceBetween: 0,
    slidesPerView: 'auto',
    preloadImages: false,
    lazyLoading: true,
    lazyLoadingInPrevNext: true,
    watchSlidesVisibility: true
});

var colorThumbnailSwiper = new Swiper('#thumbnail-colors-swiper', {
    spaceBetween: 0,
    slidesPerView: 'auto'
});

window.addEventListener('resize', resizeHandler, true);
resizeHandler();

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

function fadeOutFooterTabs() {
    $('#gallery-footer').hide();
    setTimeout(function () {
        $('#gallery-footer').show();
    }, 500);
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