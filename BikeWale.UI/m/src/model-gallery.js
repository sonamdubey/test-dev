$(document).ready(function () {
    $('.carousel-navigation-photos .swiper-slide').first().addClass('swiper-slide-active');
    showImgTitle(galleryTop);

    var hashValue = window.location.hash.substr(1);
    if (hashValue == 'videos') {
        $('.model-gallery-container').find('#videos-tab').trigger('click');
    }
    else { 
        $('#videos').hide();
    }
});

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

$(".gallery-close-btn").on('click', function () {
    if (document.referrer == "") {
        window.location.href = '../';
    } else {
        history.back();
    }
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
    $('.carousel-navigation-videos .swiper-slide').first().addClass('active');
    //dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Video_Tab_Clicked', 'lab': myBikeName });
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
});