var articleSwiper, videosThumbs, galleryThumbs, articleGallery;
var currentStagePhoto, currentStageActiveImage, galleryTop;

docReady(function () {
    articleSwiper = new Swiper('.article-photos-swiper', {
        slideActiveClass: '',
        spaceBetween: 10,
        slidesPerView: 'auto',
        slideToClickedSlide: true,
        preloadImages: false,
        lazyLoading: true,
        lazyLoadingInPrevNext: true,
        watchSlidesProgress: true,
        watchSlidesVisibility: true,
        nextButton: '.swiper-button-next',
        prevButton: '.swiper-button-prev'
    });

    videosThumbs = new Swiper('.carousel-navigation-videos', {
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

    galleryThumbs = new Swiper('.carousel-navigation-photos', {
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

    galleryTop = new Swiper('.carousel-stage-photos', {
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

    articleGallery = {
        open: function () {
            lockPopup();
            $('.model-gallery-container').show();
            $('body').addClass('article-gallery-active');
        },

        close: function () {
            unlockPopup();
            $('.model-gallery-container').hide();
            $('body').removeClass('article-gallery-active');
        }
    };

    $('.share-btn').click(function () {
        var str = $(this).attr('data-attr');
        var cururl = window.location.href;
        switch (str) {
            case 'fb':
                url = 'https://www.facebook.com/sharer/sharer.php?u=';
                break;
            case 'tw':
                url = 'https://twitter.com/home?status=';
                break;
            case 'gp':
                url = 'https://plus.google.com/share?url=';
                break;
            case 'wp':
                var text = document.getElementsByTagName("title")[0].innerHTML;
                var message = encodeURIComponent(text) + " - " + encodeURIComponent(cururl);
                var whatsapp_url = "whatsapp://send?text=" + message;
                url = whatsapp_url;
                window.open(url, '_blank');
                return;
        }
        url += cururl;
        window.open(url, '_blank');
    });

    $('.article-photos-swiper').on('click', '.swiper-slide', function () {
        var clickedImg = $(this),
            imgIndex = clickedImg.index();

        articleGallery.open();
        window.dispatchEvent(new Event('resize'));
        appendState('gallery');

        var clickedSlide = $('.carousel-navigation-photos .swiper-slide')[imgIndex];
        $('.carousel-navigation-photos .swiper-slide').removeClass('swiper-slide-active');
        $(clickedSlide).addClass('swiper-slide-active');
        galleryThumbs.slideTo(imgIndex, 500);
        galleryTop.slideTo(imgIndex, 500);
    });

    $(".gallery-close-btn").on('click', function () {
        articleGallery.close();
        history.back();
    });

    $(window).on('popstate', function (event) {
        if ($('.model-gallery-container').is(':visible')) {
            articleGallery.close();
        }
    });
});

var slideToClick = function (swiper) {
    var clickedSlide = swiper.slides[swiper.clickedIndex];
    $('.carousel-navigation-photos .swiper-slide').removeClass('swiper-slide-active');
    $(clickedSlide).addClass('swiper-slide-active');
    galleryTop.slideTo(swiper.clickedIndex, 500);
};


var slidegalleryThumbs = function (swiper) {
    galleryThumbs.slideTo(swiper.activeIndex, 500);
    galleryThumbs.slides.removeClass('swiper-slide-active');
    galleryThumbs.slides[swiper.activeIndex].className += ' swiper-slide-active';

    showImgTitle(galleryTop);
};


function showImgTitle(swiper) {
    imgTitle = $(galleryTop.slides[swiper.activeIndex]).find('img').attr('title');
    imgTotalCount = galleryThumbs.slides.length;
    $(".media-title").text(imgTitle);
    $(".gallery-count").text(swiper.activeIndex + 1 + " of " + imgTotalCount.toString());
    currentStagePhoto = $(".connected-carousels-photos .stage-photos");
    currentStageActiveImage = currentStagePhoto.find(".swiper-slide.swiper-slide-active img");
    currentStagePhoto.find('.carousel-stage-photos').css({ 'height': currentStageActiveImage.height() });
}

var appendState = function (state) {
    window.history.pushState(state, '', '');
};
