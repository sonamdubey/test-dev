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

});

function morePhotosOverlay(limitCount) {
    var lastPhoto = $('.model-grid-images li').last(),
        countOverlay = '<span class="black-overlay"><span class="font18 text-bold">+' + (photoCount - limitCount) + '</span><br /><span class="font16">images</span></span>';   
    lastPhoto.append(countOverlay);
};

var modelImages = [],
    modelColorImages = [];

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

    self.photosTabActive = ko.observable(true);

    // footer screens
    self.screenActive = ko.observable(false);
    self.photoThumbnailScreen = ko.observable(false);
    self.modelInfoScreen = ko.observable(false);

    self.activePhotoTitle = ko.observable('');
    self.activePhotoIndex = ko.observable(0);

    self.photoList = ko.observableArray(modelImages);
    self.colorPhotoList = ko.observableArray(modelColorImages);

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
            // deactivate all other screens
            self.deactivateAllScreens();
            // activate clicked tab screen
            self.photoThumbnailScreen(true);
            thumbnailSwiper.update(true);
            thumbnailSwiperEvents.focusThumbnail(thumbnailSwiper, true); // (swiperName, slideToFlag)
        }
        else {
            self.photoThumbnailScreen(false);
        }

        self.screenActive(self.photoThumbnailScreen());
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

    self.deactivateAllScreens = function() {
        self.photoThumbnailScreen(false);
        self.modelInfoScreen(false);
    };
};

var vmModelGallery = new modelGallery();

ko.applyBindings(vmModelGallery, document.getElementById('gallery-root'));

// thumbnail swiper events listener
$('.thumbnail-swiper .thumbnail-type-prev').on('click', function () {
    thumbnailSwiperEvents.slidePrev(thumbnailSwiper);
});

$('.thumbnail-swiper .thumbnail-type-next').on('click', function () {
    thumbnailSwiperEvents.slideNext(thumbnailSwiper);
});

$('.thumbnail-swiper .swiper-slide').on('click', function () {
    thumbnailSwiperEvents.focusGallery(gallerySwiper, $(this));
    thumbnailSwiperEvents.focusThumbnail(thumbnailSwiper, false);
});

// model gallery thumbnail events namespace
var thumbnailSwiperEvents = {
    slideCount: 5,

    focusGallery: function (swiper, clickedElement) {
        event.preventDefault();
        var clickedIndex = $(clickedElement).index();
        swiper.slideTo(clickedIndex);
    },

    focusThumbnail: function (swiper, slideToFlag) {
        var activeIndex = vmModelGallery.activePhotoIndex() - 1, // decrement by 1, since it was incremented by 1
            thumbnailIndex = swiper.slides[activeIndex];

        if (slideToFlag) {
            swiper.slideTo(activeIndex);
        }
        $(swiper.slides).removeClass('swiper-slide-active focus-slide');
        $(thumbnailIndex).addClass('swiper-slide-active focus-slide');
    },

    slideNext: function (swiper) {
        event.preventDefault();
        var activeIndex = swiper.activeIndex;
        swiper.slideTo(activeIndex + thumbnailSwiperEvents.slideCount);
    },

    slidePrev: function (swiper) {
        event.preventDefault();
        var activeIndex = swiper.activeIndex;
        swiper.slideTo(activeIndex - thumbnailSwiperEvents.slideCount);
    },

    setPhotoDetails: function (swiper) {
        var activeSlide = swiper.slides[swiper.activeIndex],
        activeSlideTitle = $(activeSlide).find('img').attr('title');

        vmModelGallery.activePhotoIndex(swiper.activeIndex + 1);
        vmModelGallery.activePhotoTitle(activeSlideTitle);
    }
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

var thumbnailSwiper = new Swiper('.thumbnail-swiper', {
    nextButton: '.thumbnail-type-next',
    prevButton: '.thumbnail-type-prev',
    slidesPerView: 6.8,
    preloadImages: false,
    lazyLoading: true,
    spaceBetween: 0
});