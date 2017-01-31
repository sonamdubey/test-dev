$(document).ready(function () {
    var photosLength = $('.photos-grid-list').first().find('li').length;
    
    // add 'more photos count' if photo grid contains 30 images
    if (photosLength == 30) {
        var lastPhoto = $('.photos-grid-list li').eq(29),
            morePhotoCount = $('<span class="black-overlay"><span class="font14 text-white">+'+ (photoCount - 29) +'<br />photos</span></span>');

        lastPhoto.append(morePhotoCount);
    }

    $('.carousel-navigation-photos .swiper-slide').first().addClass('swiper-slide-active');
    showImgTitle(galleryTop);
    $('#videos').hide();
    
});

$('.photos-grid-list').on('click', 'li', function () {
    bindGallery();
});
var bindGallery = function ()
{
    var clickedImg = $(this),
        imgIndex = clickedImg.index(),
        parentGridType = clickedImg.closest('.photos-grid-list');

    if (parentGridType.hasClass('remainder-grid-list')) {
        var gridOneLength = $('.photos-grid-list').first().find('li').length;

        imgIndex = gridOneLength + imgIndex; // (grid type 1's length + grid type remainder's index)
    }

    gallery.open();
    window.dispatchEvent(new Event('resize'));

    if (!isModelPage) {
        window.location.hash = 'photosGallery';
    }
    //appendState('gallery');

    $("#photos-tab").trigger('click');
    var clickedSlide = $('.carousel-navigation-photos .swiper-slide')[imgIndex];
    $('.carousel-navigation-photos .swiper-slide').removeClass('swiper-slide-active');
    $(clickedSlide).addClass('swiper-slide-active');
    galleryThumbs.slideTo(imgIndex, 500);
    galleryTop.slideTo(imgIndex, 500);
}
var slideToClick = function (swiper) {
    var clickedSlide = swiper.slides[swiper.clickedIndex];
    $('.carousel-navigation-photos .swiper-slide').removeClass('swiper-slide-active');
    $(clickedSlide).addClass('swiper-slide-active');
    galleryTop.slideTo(swiper.clickedIndex, 500);
};

var videosThumbs = new Swiper('.carousel-navigation-videos', {
    slideActiveClass: '',
    spaceBetween: 0,
    slidesPerView: 'auto',
    slideToClickedSlide: true,
    preloadImages: false,
    lazyLoading: true,
    lazyLoadingInPrevNext: true,
    watchSlidesProgress: true,
    watchSlidesVisibility: true
});

var galleryThumbs = new Swiper('.carousel-navigation-photos', {
    slideActiveClass: '',
    spaceBetween: 0,
    slidesPerView: 'auto',
    slideToClickedSlide: true,
    preloadImages: false,
    lazyLoading: true,
    lazyLoadingInPrevNext: true,
    watchSlidesProgress: true,
    watchSlidesVisibility: true,
    onTap: slideToClick
});

var slidegalleryThumbs = function (swiper) {
    galleryThumbs.slideTo(swiper.activeIndex, 500);
    galleryThumbs.slides.removeClass('swiper-slide-active');
    galleryThumbs.slides[swiper.activeIndex].className += ' swiper-slide-active';

    showImgTitle(galleryTop);
};

var galleryTop = new Swiper('.carousel-stage-photos', {
    nextButton: '.swiper-button-next',
    prevButton: '.swiper-button-prev',
    spaceBetween: 10,
    preloadImages: false,
    lazyLoading: true,
    lazyLoadingInPrevNext: true,
    watchSlidesProgress: true,
    watchSlidesVisibility: true,
    onSlideChangeEnd: slidegalleryThumbs
});


var currentStagePhoto, currentStageActiveImage;
function showImgTitle(swiper) {
    imgTitle = $(galleryTop.slides[swiper.activeIndex]).find('img').attr('title');
    imgTotalCount = galleryThumbs.slides.length;
    $(".media-title").text(imgTitle);
    $(".gallery-count").text(swiper.activeIndex + 1 + " of " + imgTotalCount.toString());
    currentStagePhoto = $(".connected-carousels-photos .stage-photos");
    currentStageActiveImage = currentStagePhoto.find(".swiper-slide.swiper-slide-active img");
    currentStagePhoto.find('.carousel-stage-photos').css({ 'height': currentStageActiveImage.height() });
}

var videoiFrame = document.getElementById("video-iframe");

$("#photos-tab, #videos-tab").click(function () {
    firstVideo();
});

$("#videos-tab").click(function () {
    $('.carousel-navigation-videos .swiper-slide').removeClass('active');
    $('.carousel-navigation-videos .swiper-slide').first().addClass('active');
});

var firstVideo = function () {
    var a = $(".carousel-navigation-videos .swiper-wrapper").first(".swiper-slide");
    var newSrc = a.find("img").attr("iframe-data");
    videoiFrame.setAttribute("src", newSrc);
};

var navigationVideosLI = $(".carousel-navigation-videos .swiper-slide");
navigationVideosLI.click(function () {
    navigationVideosLI.removeClass("active");
    $(this).addClass("active");
    var newSrc = $(this).find("img").attr("iframe-data");
    videoiFrame.setAttribute("src", newSrc);
    window.dispatchEvent(new Event('resize'));
});

var appendState = function (state) {
    window.history.pushState(state, '', '');
    
};

var gallery = {
    open: function () {
        lockPopup();
        $('.model-gallery-container').show();
        $('body').addClass('gallery-popup-active');
    },

    close: function () {
        unlockPopup();
        $('.model-gallery-container').hide();
        $('body').removeClass('gallery-popup-active');
    },

    gotoModelPage: function () {
        window.location.href = window.location.pathname.split("images/")[0];
    }
};