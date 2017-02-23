$(document).ready(function () {
    var photosLength = $('.model-grid-images li').length,
        photoLimitCount = 24;

    // add 'more photos count' if photo grid contains 24 images
    if (photosLength == photoLimitCount) {
        morePhotosOverlay(photoLimitCount);
    }
    else if ((photoCount - 1) % 8 == 1) { // remainder 1 image
        $('.model-grid-images li').eq(11).css('float', 'left');
        morePhotosOverlay(photosLength);
    }

    var windowHeight = window.innerHeight;
    if (windowHeight < 700) {
        $('body').addClass('gallery-700');
    }
    else if (windowHeight > 700 && windowHeight < 880) {
        $('body').addClass('gallery-768');
    }
    else if (windowHeight > 890 && windowHeight < 910) {
        $('body').addClass('gallery-900');
    }
    else if (windowHeight > 920 && windowHeight < 1100) {
        $('body').addClass('gallery-1024');
    }

});

function morePhotosOverlay(limitCount) {
    var lastPhoto = $('.model-grid-images li').last(),
        countOverlay = '<span class="black-overlay"><span class="font18 text-bold">+' + (photoCount - limitCount) + '</span><br /><span class="font16">images</span></span>';   
    lastPhoto.append(countOverlay);
};

$('.model-main-image').on('click', 'li', function () {
    if (photoCount > 1) {
        popupGallery.bindGallery(0);
    }
});

$('.model-grid-images').on('click', 'li', function () {
    var imageIndex = $(this).index(),
        parentGridType = $(this).closest('.model-grid-images');

    if (!parentGridType.hasClass('remainder-grid-list')) {
        imageIndex = imageIndex + 1;
    }
    else {
        imageIndex = imageIndex + $('.model-grid-images').first().find('li').length;
    }

    popupGallery.bindGallery(imageIndex);
});

$('#gallery-close-btn').on('click', function () {
    popupGallery.close();
});

$(document).keydown(function (event) {
    if (event.keyCode == 27) {
        popupGallery.close();
    }
});

var popupGallery = {
    open: function () {
        vmModelGallery.isGalleryActive(true);
        popup.lock();
    },

    close: function () {
        vmModelGallery.isGalleryActive(false);
        vmModelGallery.resetGallery();
        popup.unlock();
    },

    bindGallery: function (imageIndex) {
        getPhotos();
        popupGallery.open();
        gallerySwiper.update(true);
        thumbnailSwiperEvents.focusGallery(gallerySwiper, imageIndex);
    }
}

var modelImages = [],
    modelColorImages = [],
    modelColorImageCount = 0;

var pageNo = 1,
    pageSize = 4;

/* gallery */
function getPhotos() {
    $.ajax({
        type: "Get",
        url: "/api/model/" + ModelId + "/photos/",
        contentType: "application/json",
        dataType: 'json',
        async: false,
        success: function (response) {
            if (response) {
                modelImages = response;
                modelColorImages = filterColorImagesArray(response);
            }
            modelColorImageCount = modelColorImages.length;
        }
    });
};

getPhotos();

function filterColorImagesArray(responseArray) {
    return ko.utils.arrayFilter(responseArray, function (response) {
        return response.imageType == 3;
    });
}

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

    // video
    self.activeVideoTitle = ko.observable('');
    self.activeVideoIndex = ko.observable(0);
    self.activeVideoId = ko.observable();

    self.videoList = ko.observableArray([]);
    self.totalVideoCount = videoCount;

    self.togglePhotoTab = function () {
        if (!self.photosTabActive()) {
            self.photosTabActive(true);
            self.activeVideoId('');
        }
        else {
            self.photosTabActive(false);
            if (!self.videoList().length) {
                self.getVideos();
            }
            else {
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

            thumbnailSwiper.update(true);
            thumbnailSwiperEvents.focusThumbnail(thumbnailSwiper, vmModelGallery.activePhotoIndex(), true); // (swiperName, activeIndex, slideToFlag)
        }
        else {
            self.photoThumbnailScreen(false);
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
        }
        else {
            self.colorsThumbnailScreen(false);
        }

        self.screenActive(self.colorsThumbnailScreen());

    };

    self.toggleModelInfoScreen = function () {
        if (!self.modelInfoScreen()) {
            self.deactivateAllScreens();
            self.modelInfoScreen(true);
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
            thumbnailSwiperEvents.focusThumbnail(videoThumbnailSwiper, vmModelGallery.activeVideoIndex(), true);
        }
        else {
            self.videoListScreen(false);
        }
        self.screenActive(self.videoListScreen());
    };

    self.deactivateAllScreens = function() {
        self.photoThumbnailScreen(false);
        self.colorsThumbnailScreen(false);
        self.modelInfoScreen(false);
        self.videoListScreen(false);
    };

    self.getVideos = function () {
        try {
            if (videoCount > (pageNo - 1) * pageSize) {
                $.ajax({
                    type: 'GET',
                    url: '/api/videos/pn/' + pageNo + '/ps/' + pageSize + '/model/' + ModelId + '/',
                    dataType: 'json',
                    success: function (response) {
                        if (response) {
                            isNextPage = true;
                            pushVideoList(response.videos);
                        }
                    },
                    complete: function (xhr) {
                        if (xhr.status != 200) {
                            isNextPage = false;
                        }
                    }
                });
            }
        } catch (e) {
            console.warn("Unable to fetch Videos model gallery " + e.message);
        }
    };

    function pushVideoList(response) {
        ko.utils.arrayPushAll(self.videoList(), ko.toJS(response));
        if (!self.activeVideoId()) {
            setVideo(0);
        }
        pageNo = pageNo + 1;
        self.videoList.notifySubscribers();
        videoThumbnailSwiper.update();
    }

    self.resetGallery = function () {
        self.activeVideoId('');
        self.photosTabActive(true);
        self.photoSwiperActive(true);
        self.deactivateAllScreens();
    }

};

var vmModelGallery = new modelGallery();
ko.applyBindings(vmModelGallery, document.getElementById('gallery-root'));

// thumbnail swiper events listener
$('.thumbnail-swiper .thumbnail-type-prev').on('click', function () {
    thumbnailSwiperEvents.slidePrev(thumbnailSwiper, 6); // (swiperName, no. of slides to scroll)
});

$('.thumbnail-swiper .thumbnail-type-next').on('click', function () {
    thumbnailSwiperEvents.slideNext(thumbnailSwiper, 6);
});

$('.thumbnail-swiper .swiper-slide').on('click', function () {
    var slideIndex = $(this).index();
    thumbnailSwiperEvents.focusGallery(gallerySwiper, slideIndex);
    thumbnailSwiperEvents.focusThumbnail(thumbnailSwiper, vmModelGallery.activePhotoIndex(), false);
});

// color thumbnail swiper events listener
$('.color-thumbnail-swiper .color-type-prev').on('click', function () {
    thumbnailSwiperEvents.slidePrev(colorThumbnailSwiper, 5);
});

$('.color-thumbnail-swiper .color-type-next').on('click', function () {
    thumbnailSwiperEvents.slideNext(colorThumbnailSwiper, 5);
});

$('.color-thumbnail-swiper .swiper-slide').on('click', function () {
    var slideIndex = $(this).index();
    thumbnailSwiperEvents.focusGallery(colorGallerySwiper, slideIndex);
});

// video thumbnail swiper events listener
$('.video-thumbnail-swiper .video-type-prev').on('click', function () {
    thumbnailSwiperEvents.slidePrev(videoThumbnailSwiper, 2);
});

$('.video-thumbnail-swiper .video-type-next').on('click', function () {
    vmModelGallery.getVideos();
    thumbnailSwiperEvents.slideNext(videoThumbnailSwiper, 2);
});

$('#video-tab-screen').on('click', '.video-thumbnail-swiper .swiper-slide', function () {
    var elementIndex = $(this).index();
    setVideo(elementIndex);
    thumbnailSwiperEvents.focusThumbnail(videoThumbnailSwiper, vmModelGallery.activeVideoIndex(), true);
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
        vmModelGallery.activePhotoTitle(activeSlideTitle);
    },

    setColorPhotoDetails: function (swiper) {
        var activeSlide = swiper.slides[swiper.activeIndex],
            activeSlideTitle = $(activeSlide).find('img').attr('alt');

        vmModelGallery.activeColorIndex(swiper.activeIndex + 1);
        vmModelGallery.activeColorTitle(activeSlideTitle);
    }
}

function setVideo(elementIndex) {
    var elementId = vmModelGallery.videoList()[elementIndex].VideoId,
        elementTitle = vmModelGallery.videoList()[elementIndex].VideoTitle;

    vmModelGallery.activeVideoId(elementId);
    vmModelGallery.activeVideoIndex(elementIndex + 1);
    vmModelGallery.activeVideoTitle(elementTitle);
};

// initialize swipers
var gallerySwiper = new Swiper('.gallery-type-swiper', {
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
    }
});

var colorGallerySwiper = new Swiper('.gallery-color-type-swiper', {
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
    },
    onTouchStart: function () {
        if (vmModelGallery.modelInfoScreen()) {
            vmModelGallery.modelInfoScreen(false);
        }
    },
    onSlideChangeStart: function (swiper) {
        thumbnailSwiperEvents.setColorPhotoDetails(swiper);
        thumbnailSwiperEvents.focusThumbnail(colorThumbnailSwiper, vmModelGallery.activeColorIndex(), true);
    }
});

var thumbnailSwiper = new Swiper('.thumbnail-swiper', {
    nextButton: '.thumbnail-type-next',
    prevButton: '.thumbnail-type-prev',
    slidesPerView: 6.8,
    preloadImages: false,
    lazyLoading: true,
    spaceBetween: 0
});

var colorThumbnailSwiper = new Swiper('.color-thumbnail-swiper', {
    nextButton: '.color-thumbnail-type-next',
    prevButton: '.color-thumbnail-type-prev',
    slidesPerView: 5.5,
    spaceBetween: 0
});

var videoThumbnailSwiper = new Swiper('.video-thumbnail-swiper', {
    nextButton: '.video-type-next',
    prevButton: '.video-type-prev',
    slidesPerView: 2.35,
    preloadImages: false,
    lazyLoading: true,
    spaceBetween: 0
});