var modelImages = [],
    modelColorImages = [],
    modelColorImageCount = 0,
    videoList = null,
    imageList = null;

(function () {
    try {
    imageList = JSON.parse(Base64.decode(encodedImageList));
    videoList = JSON.parse(Base64.decode(encodedVideoList));

    var firstImage = JSON.parse(Base64.decode(encodedFirstImage));

    imageList.unshift(firstImage);

    modelImages = imageList;
    modelColorImages = filterColorImagesArray(imageList);

    if (modelColorImages)
            modelColorImageCount = modelColorImages.length;            
    } catch (e) {
        console.warn(e);
    }
})();

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
        if (!detectIEBrowser()) {
            popupGallery.bindGallery(0);
        }
        else {
            fallbackGallery.open();
        }
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
    if (!detectIEBrowser()) {
        popupGallery.bindGallery(imageIndex);
    }
    else {
        fallbackGallery.open();
    }
    
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

        if (colorImageId > 0) {
            vmModelGallery.toggleColorThumbnailScreen();
        }

    },

    close: function () {
        if (isModelPage === "true") {
            window.location.href = window.location.pathname.split("images/")[0];
        }
        else {
            vmModelGallery.isGalleryActive(false);
            vmModelGallery.resetGallery();
            popup.unlock();
        }
    },

    bindGallery: function (imageIndex) {
        triggerGA('Gallery_Page', 'Gallery_Loaded', modelName);
        popupGallery.open();
        gallerySwiper.update(true);
        thumbnailSwiperEvents.focusGallery(gallerySwiper, imageIndex);

        if (isModelPage === "false") {
            window.location.hash = 'photosGallery';
        }
    }
}

var pageNo = 1,
    pageSize = 4;

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
            thumbnailSwiperEvents.focusThumbnail(colorThumbnailSwiper, vmModelGallery.activeColorIndex(), true);
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
            thumbnailSwiperEvents.focusThumbnail(videoThumbnailSwiper, vmModelGallery.activeVideoIndex(), true);
            triggerGA('Gallery_Page', 'All_Videos_Tab_Clicked_Opened', modelName);
        }
        else {
            self.videoListScreen(false);
            triggerGA('Gallery_Page', 'All_Videos_Tab_Clicked_Closed', modelName);
        }
        self.screenActive(self.videoListScreen());
    };

    self.deactivateAllScreens = function() {
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

    (document.getElementById("iframe-video").contentWindow || document.getElementById("iframe-video").documentWindow).location.replace('https://www.youtube.com/embed/' + vmModelGallery.activeVideoId() + '?showinfo=0');

    };

var colorIndex = 0;

if (!detectIEBrowser()) {
    (function () {
        try {
            if (colorImageId > 0) {

                ko.utils.arrayForEach(modelColorImages, function (item, index) {
                    if (item.ColorId == colorImageId) { colorIndex = index; }
                });

                vmModelGallery.activeColorIndex(colorIndex);
            }
        } catch (e) {
            console.warn(e);
        }
    })();
}
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
        triggerGA('Gallery_Page', 'Colour_Changed', modelName);
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

$(window).on('hashchange', function (e) {
    hashChange(e);
});

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

if (!detectIEBrowser()) {
    (function () {
        try {
            if (colorImageId > 0) {
                thumbnailSwiperEvents.focusGallery(colorGallerySwiper, colorIndex);
            }
        } catch (e) {
            console.warn(e);
        }
    })();
}