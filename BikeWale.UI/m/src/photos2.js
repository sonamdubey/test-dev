var cacheData,
    modelColorImageCount = 0,
    pageNo = 1,
    modelImages = [],
    pageSize = 4,
    modelColorImages = []
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

    self.photosTabActive = ko.observable(true);

    self.photoSwiperActive = ko.observable(true);

    // footer screens
    self.screenActive = ko.observable(false);
    self.photoThumbnailScreen = ko.observable(false);
    self.colorsThumbnailScreen = ko.observable(false);
    self.modelInfoScreen = ko.observable(false);
    self.videoListScreen = ko.observable(false);

    self.activePhotoTitle = ko.observable('');
    self.activePhotoIndex = ko.observable(0);

    self.photoList = ko.observableArray(modelImages);
    
    self.togglePhotoTab = function () {
        if (!self.photosTabActive()) {
            self.photosTabActive(true);
        }
        else {
            self.photosTabActive(false);
        }
    };

    // all photos tab
    self.togglePhotoThumbnailScreen = function () {
        if (!self.photoThumbnailScreen()) {
            self.deactivateAllScreens();
            self.photoThumbnailScreen(true);
            self.photoSwiperActive(true);

            gallerySwiper.update(true);
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

    self.toggleModelInfoScreen = function () {
        if (!self.modelInfoScreen()) {
            self.deactivateAllScreens();
            if (navigator.userAgent.indexOf('UCBrowser/') >= 0) {
                if (!$('#gallery-root').hasClass('uc-iframe-position')) {
                    $('#gallery-root').addClass('uc-iframe-position');
                }
            }
            self.modelInfoScreen(true);
            //triggerGA('Gallery_Page', 'Info_Tab_Clicked', modelName);
        }
        else {
            self.modelInfoScreen(false);
        }

        self.screenActive(self.modelInfoScreen());
    };

    self.deactivateAllScreens = function () {
        self.photoThumbnailScreen(false);
        self.colorsThumbnailScreen(false);
        self.modelInfoScreen(false);
        self.videoListScreen(false);
    };
};

var vmModelGallery = new modelGallery();

ko.applyBindings(vmModelGallery, document.getElementById('gallery-root'));

// thumbnail swiper events listener
$('.thumbnail-swiper .swiper-slide').on('click', function () {
    var slideIndex = $(this).index();
    thumbnailSwiperEvents.focusGallery(gallerySwiper, slideIndex);
    thumbnailSwiperEvents.focusThumbnail(thumbnailSwiper, vmModelGallery.activePhotoIndex(), false);
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
    }
}

// initialize swipers
var gallerySwiper = new Swiper('#main-photo-swiper', {
    spaceBetween: 0,
    preloadImages: false,
    lazyLoading: true,
    nextButton: '.swiper-button-next',
    prevButton: '.swiper-button-prev',
    onInit: function (swiper) {
        swiper.slideTo(vmModelGallery.activePhotoIndex());        
        thumbnailSwiperEvents.setPhotoDetails(swiper);
    },
    onClick: function () {
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
        thumbnailSwiperEvents.focusThumbnail(thumbnailSwiper, vmModelGallery.activePhotoIndex(), true);
    }
});

var thumbnailSwiper = new Swiper('#thumbnail-photo-swiper', {
    spaceBetween: 0,
    slidesPerView: 'auto',
    preloadImages: false,
    lazyLoading: true
});