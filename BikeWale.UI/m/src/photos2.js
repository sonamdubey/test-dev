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
};

var vmModelGallery = new modelGallery();

ko.applyBindings(vmModelGallery, document.getElementById('gallery-root'));

// model gallery thumbnail events namespace
var thumbnailSwiperEvents = {

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
    onSlideChangeStart: function (swiper) {
        thumbnailSwiperEvents.setPhotoDetails(swiper);
    }
});