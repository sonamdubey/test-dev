﻿$(document).ready(function () {
    var photosLength = $('.photos-grid-list').first().find('li').length;
    
    // add 'more photos count' if photo grid contains 30 images
    if (photosLength == 30) {
        var lastPhoto = $('.photos-grid-list li').eq(29),
            morePhotoCount = $('<span class="black-overlay"><span class="font14 text-white">+' + (photoCount - 29) + '<br />photos</span></span>');

        lastPhoto.append(morePhotoCount);
    }
    
});

var modelImages = [];
var modelColorImages = [];

$('.photos-grid-list').on('click', 'li', function () {
    bindGallery($(this));

});
var bindGallery = function (clickedImg)
{
    var imgIndex = clickedImg.index(),
        parentGridType = clickedImg.closest('.photos-grid-list');

    if (parentGridType.hasClass('remainder-grid-list')) {
        var gridOneLength = $('.photos-grid-list').first().find('li').length;

        imgIndex = gridOneLength + imgIndex; // (grid type 1's length + grid type remainder's index)
    }

    vmPhotosPage.imageIndex(imgIndex);
    showGallery();

};
    if (!isModelPage) {
        window.location.hash = 'photosGallery';
    }
    //appendState('gallery');

    $(document).on('click', '#gallery-close-btn', function () {
       
        if (isModelPage) {
            window.location.href = window.location.pathname.split("images/")[0];
        }
        else
            hideGallery();
    });
var modelVideos = [
    {
        'imagePathThumbnail': 'https://img.youtube.com/vi/HhOik7KWJwc/default.jpg',
        'videoId': 'HhOik7KWJwc',
        'videoTitle': 'Dominar 400 or the Classic 350, CBR 250R'
    },
    {
        'imagePathThumbnail': 'https://img.youtube.com/vi/WubzCZFId1o/default.jpg',
        'videoId': 'WubzCZFId1o',
        'videoTitle': 'All you need to know about the Bajaj Dominar 400 : PowerDrift'
    },
    {
        'imagePathThumbnail': 'https://img.youtube.com/vi/h399XRm-OcA/default.jpg',
        'videoId': 'h399XRm-OcA',
        'videoTitle': 'Honda Navi : First Impression : PowerDrift'
    },
    {
        'imagePathThumbnail': 'https://img.youtube.com/vi/jOdAplDI2FI/default.jpg',
        'videoId': 'jOdAplDI2FI',
        'videoTitle': 'Launch Alert : Bajaj Dominar 400 : PowerDrift'
    },
    {
        'imagePathThumbnail': 'https://img.youtube.com/vi/W1KOvK9_gAc/default.jpg',
        'videoId': 'W1KOvK9_gAc',
        'videoTitle': 'Indian Scout Sixty : Launch Alert : PowerDrift'
    }
];

function toggleFullScreen(goFullScreen) {
    var doc = window.document;
    var docElement = doc.documentElement;

    var requestFullScreen = docElement.requestFullscreen || docElement.mozRequestFullScreen || docElement.webkitRequestFullScreen || docElement.msRequestFullscreen;
    var cancelFullScreen = doc.exitFullscreen || doc.mozCancelFullScreen || doc.webkitExitFullscreen || doc.msExitFullscreen;

    if (goFullScreen && requestFullScreen != undefined) {
        requestFullScreen.call(docElement);
    }
    else if (cancelFullScreen != undefined) {
        cancelFullScreen.call(doc);
    }
}

function checkCacheModelPhotos(bKey) {    
    return (bwcache.get(bKey, true));
}
var cacheData;

function filterImagesArray(responseArray){
    return ko.utils.arrayFilter(responseArray, function (response) {
        return response.imageType == 3;
    });
}

function bindPhotoGallery() {
    // remove the binding and then re-apply
    ko.cleanNode(document.getElementById('gallery-root'))
    ko.applyBindings(vmPhotosPage, document.getElementById('gallery-root'))

    vmPhotosPage.activateGallery(true);
    $('body').addClass('lock-browser-scroll');
    appendState('modelGallery');
}

function showGallery() {      
    try {        
        var keyPhoto = "modelPhotos_" + modelId;

        if (!checkCacheModelPhotos(keyPhoto)) {
            $.ajax({
                type: "Get",
                url: "/api/model/" + modelId + "/photos/",
                contentType: "application/json",
                dataType: 'json',
                success: function (response) {
                    if (response) {
                        modelImages = response;
                        modelColorImages = filterImagesArray(response);
                    }                    

                    bindPhotoGallery();

                    var cacheData = JSON.stringify({
                        modelImages: modelImages,
                        modelColorImages: modelColorImages
                    });                   
                    var cachedEncodedData = Base64.encode(cacheData)
                    bwcache.set(keyPhoto, cachedEncodedData, true);
                }
            });
        }
        else {           
            var cacheData = Base64.decode(bwcache.get(keyPhoto, true));
            var cacheDecodedData = JSON.parse(cacheData);
            modelImages = cacheDecodedData.modelImages;
            modelColorImages = cacheDecodedData.modelColorImages;
            bindPhotoGallery();            
        }

    }
    catch (e) {
        console.warn(e);
    }
};

function hideGallery() {
    vmPhotosPage.activateGallery(false);
    $('body').removeClass('lock-browser-scroll');
};

var photosPage = function () {
    var self = this;

    self.activateGallery = ko.observable(false);
    self.imageIndex = ko.observable(0);
};

var vmPhotosPage = new photosPage();
var modelGallery = function () {
    var self = this;

    self.photosTabActive = ko.observable(true);

    self.galleryTabsActive = ko.observable(true);
    self.galleryFooterActive = ko.observable(true);
    self.photoSwiperActive = ko.observable(true);
    self.fullScreenModeActive = ko.observable(false);

    // footer screens
    self.screenActive = ko.observable(false);
    self.photoThumbnailScreen = ko.observable(false);
    self.colorsThumbnailScreen = ko.observable(false);
    self.modelInfoScreen = ko.observable(false);
    self.isScreenRotated = ko.observable(false);

    // swiper
    self.photoHeadingActive = ko.observable(true);

    self.activePhotoTitle = ko.observable('');
    self.activePhotoIndex = ko.observable(vmPhotosPage.imageIndex());

    self.activeColorTitle = ko.observable('');
    self.activeColorIndex = ko.observable(0);

    // video
    self.activeVideoTitle = ko.observable('');
    self.activeVideoIndex = ko.observable(0);

    self.activeVideoId = ko.observable();

    self.videoListScreen = ko.observable(false);

    self.photoList = ko.observableArray(modelImages);
    self.colorPhotoList = ko.observableArray(modelColorImages);
    self.videoList = ko.observable(modelVideos);

    self.afterRender = function () {
        if (!self.mainSwiper) {
            self.mainSwiper = $('#main-photo-swiper').swiper({
                spaceBetween: 0,
                preloadImages: false,
                lazyLoading: true,
                onInit: function (swiper) {
                    swiper.slideTo(self.activePhotoIndex());
                    setPhotoDetails(swiper);
                },
                onClick: function () {
                    if (window.innerWidth > window.innerHeight) {
                        self.toggleFooterTabs();
                        self.togglePhotoHeading();
                    }
                    if (self.screenActive()) {
                        deactivateAllScreens();
                    }
                },
                onTouchStart: function () {
                    if (self.screenActive()) {
                        deactivateAllScreens();
                    }
                },
                onSlideChangeStart: function (swiper) {
                    setPhotoDetails(swiper);
                }
            });
        };

        if (!self.mainColorSwiper) {
            self.mainColorSwiper = $('#main-color-swiper').swiper({
                spaceBetween: 0,
                preloadImages: false,
                lazyLoading: true,
                onInit: function (swiper) {
                    setColorPhotoDetails(swiper);
                },
                onClick: function () {
                    if (window.innerWidth > window.innerHeight) {
                        self.toggleFooterTabs();
                        self.togglePhotoHeading();
                    }
                    if (self.modelInfoScreen()) {
                        self.modelInfoScreen(false);
                    }
                },
                onTouchStart: function () {
                    if (self.modelInfoScreen()) {
                        self.modelInfoScreen(false);
                    }
                },
                onSlideChangeStart: function (swiper) {
                    setColorPhotoDetails(swiper);
                }
            });
        };
    };

    self.togglePhotoTab = function () {
        if (!self.photosTabActive()) {
            self.photosTabActive(true);
            self.activeVideoId('');
        }
        else {
            self.photosTabActive(false);
            toggleFullScreen(false);
            setVideoDetails(self.activeVideoIndex());
        }
        if (self.screenActive()) {
            deactivateAllScreens();
        }
    };

    // all photos tab
    self.togglePhotoThumbnailScreen = function () {
        if (!self.photoThumbnailScreen()) {
            // deactivate all other screens
            deactivateAllScreens();
            // activate clicked tab screen
            self.photoThumbnailScreen(true);
            self.photoSwiperActive(true);
            self.mainSwiper.update(true);
            self.initiatePhotoThumbnailSwiper();
        }
        else {
            self.photoThumbnailScreen(false);
        }

        self.screenActive(self.photoThumbnailScreen());
    };

    self.toggleColorThumbnailScreen = function () {
        if (!self.colorsThumbnailScreen()) {
            deactivateAllScreens();
            self.colorsThumbnailScreen(true);
            self.photoSwiperActive(false);
            self.mainColorSwiper.update(true);
            self.initiateColorThumbnailSwiper();
        }
        else {
            self.colorsThumbnailScreen(false);
        }

        self.screenActive(self.colorsThumbnailScreen());

    };

    self.toggleModelInfoScreen = function () {
        if (!self.modelInfoScreen()) {
            deactivateAllScreens();
            self.modelInfoScreen(true);
        }
        else {
            self.modelInfoScreen(false);
        }

        self.screenActive(self.modelInfoScreen());
    };

    self.initiatePhotoThumbnailSwiper = function () {
        self.thumbnailSwiper = $('#thumbnail-photo-swiper').swiper({
            slidesPerView: 'auto',
            spaceBetween: 0,
            slideActiveClass: '',
            slideToClickedSlide: true,
            onClick: function (swiper) {
                focusGalleryPhoto(swiper);
            }
        });

        focusThumbnail(self.thumbnailSwiper);
    };

    self.initiateColorThumbnailSwiper = function () {
        self.colorThumbnailSwiper = $('#thumbnail-colors-swiper').swiper({
            slidesPerView: 'auto',
            spaceBetween: 0,
            slideActiveClass: '',
            slideToClickedSlide: true,
            onClick: function (swiper) {
                focusColorGalleryPhoto(swiper);
            }
        });

        focusColorThumbnail(self.colorThumbnailSwiper);
    };

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
        if (!self.fullScreenModeActive()) {
            toggleFullScreen(true);
            self.fullScreenModeActive(true);
        }
        else {
            toggleFullScreen(false);
            self.fullScreenModeActive(false);
        }
    };

    // video
    self.toggleVideoListScreen = function () {
        if (!self.videoListScreen()) {
            deactivateAllScreens();
            self.videoListScreen(true);
        }
        else {
            self.videoListScreen(false);
        }

        self.screenActive(self.videoListScreen());
    };

    self.videoSelection = function (data, event) {
        var element = $(event.currentTarget);

        if (!element.hasClass('active')) {
            var elementIndex = element.index();

            setVideoDetails(elementIndex);
        }
    };

    function setVideoDetails(elementIndex) {
        var element = $('.video-tab-list li')[elementIndex],
            elementId = self.videoList()[elementIndex].videoId,
            elementTitle = self.videoList()[elementIndex].videoTitle;

        self.activeVideoId(elementId);
        self.activeVideoIndex(elementIndex);
        self.activeVideoTitle(elementTitle);

        $(element).siblings().removeClass('active');
        $(element).addClass('active');

    };

    window.addEventListener('resize', resizeHandler, true);
    resizeHandler();

    function resizeHandler() {
        if (window.innerWidth > window.innerHeight) {
            self.hideGalleryTabs();
            self.hideFooterTabs();
            self.hidePhotoHeading();
        }
        else {
            self.showGalleryTabs();
            self.showFooterTabs();
            self.showPhotoHeading();
        }
    };

    // swiper
    function setPhotoDetails(swiper) {
        var activeSlide = swiper.slides[swiper.activeIndex],
            activeSlideTitle = $(activeSlide).find('img').attr('title');

        self.activePhotoIndex(swiper.activeIndex + 1);
        self.activePhotoTitle(activeSlideTitle);
    };

    function setColorPhotoDetails(swiper) {
        var activeSlide = swiper.slides[swiper.activeIndex],
            activeSlideTitle = $(activeSlide).find('img').attr('title');

        self.activeColorIndex(swiper.activeIndex + 1);
        self.activeColorTitle(activeSlideTitle);

        if (self.colorThumbnailSwiper) {
            focusColorThumbnail(self.colorThumbnailSwiper);
        }
    };

    // focus gallery to clicked thumbnail index
    function focusGalleryPhoto(swiper) {
        var thumbnailIndex = swiper.clickedIndex;

        self.mainSwiper.slideTo(thumbnailIndex);
        focusThumbnail(swiper);
    };

    function focusColorGalleryPhoto(swiper) {
        var thumbnailIndex = swiper.clickedIndex;

        self.mainColorSwiper.slideTo(thumbnailIndex);
        focusColorThumbnail(swiper);
    };

    // focus thumbnail to main photo index
    function focusThumbnail(swiper) {
        var activeIndex = self.activePhotoIndex() - 1, // decrement by 1, since it was incremented by 1
            thumbnailIndex = swiper.slides[activeIndex];

        swiper.slideTo(activeIndex);
        $(swiper.slides).removeClass('swiper-slide-active');
        $(thumbnailIndex).addClass('swiper-slide-active');
    };

    function focusColorThumbnail(swiper) {
        var activeIndex = self.activeColorIndex() - 1,
            thumbnailIndex = swiper.slides[activeIndex];

        swiper.slideTo(activeIndex);
        $(swiper.slides).removeClass('swiper-slide-active');
        $(thumbnailIndex).addClass('swiper-slide-active');
    };

    function deactivateAllScreens() {
        self.photoThumbnailScreen(false);
        self.colorsThumbnailScreen(false);
        self.modelInfoScreen(false);
        self.videoListScreen(false);
    };
}

ko.components.register("gallery-component", {
    viewModel: modelGallery,
    template: { element: "gallery-template-wrapper" }
});

ko.applyBindings(vmPhotosPage, document.getElementById('gallery-root'));


$(window).on('popstate', function (event) {
    if ($('#gallery-container').is(':visible')) {
        if (isModelPage) {
            window.location.href = window.location.pathname.split("images/")[0];
        }
        else
            hideGallery();
       
    }
});
