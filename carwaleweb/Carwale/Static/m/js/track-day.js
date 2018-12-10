$(document).ready(function () {
    $('.carousel-navigation-photos .swiper-slide').first().addClass('swiper-slide-active');
});

var slideToClick = function (swiper) {
    var clickedSlide = swiper.slides[swiper.clickedIndex];
    $(clickedSlide).addClass('swiper-slide-active');
    galleryTop.slideTo(swiper.clickedIndex, 500);
};
var landingPageSwiper = $('.landing-page-banner .swiper-container').swiper({
    spaceBetween: 10,
    preloadImages: false,
    lazyLoading: true,
    lazyLoadingInPrevNext: true,
    nextButton: '.landing-page-banner .swiper-button-next',
    prevButton: '.landing-page-banner .swiper-button-prev'
});

var galleryThumbs = $('.carousel-navigation-photos').swiper({
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
};

var galleryTop = $('.carousel-stage-photos').swiper({
    nextButton: '#trackdayGallery .swiper-button-next',
    prevButton: '#trackdayGallery .swiper-button-prev',
    spaceBetween: 10,
    preloadImages: false,
    lazyLoading: true,
    lazyLoadingInPrevNext: true,
    watchSlidesProgress: true,
    watchSlidesVisibility: true,
    onSlideChangeEnd: slidegalleryThumbs
});
$('.stage-photos').on('click', '.swiper-slide', function () {
    if (!$('body').hasClass('gallery-active')) {
        gallery.open();
        appendState('popupGallery');
        window.dispatchEvent(new Event('resize'));
    }
});

$(window).resize(function () {
    if ($('body').hasClass('gallery-active')) {
        gallery.setPosition();
    }  
});
$(window).on('orientationchange', function () {
    $('.connected-carousels-photos .carousel-photos', function () {
        $(this).css({ "height": "auto" });
    });
});
$('#gallery-close-btn').on('click', function () {
    gallery.close();
});

var gallery = {
    open: function () {
        $('body').addClass('gallery-active');
        popup.lock('.gallery-blackOut-window');
        gallery.setPosition();
    },
    close: function () {
        $('body').removeClass('gallery-active');
        popup.unlock('.gallery-blackOut-window');
        gallery.resetPosition();
    },
    setPosition: function () {
        var topPosition = ($(window).height() / 2) - (($('.stage-photos').height() + $('.navigation-photos').height()) / 2);
        $('.connected-carousels-photos').css({
            'top': topPosition
        });
    },
    resetPosition: function () {
        $('.connected-carousels-photos').css({
            'top': 0
        });
    }
};
var popup = {
    lock: function (blackOutWindow) {
        var htmlElement = $('html'), bodyElement = $('body');
        $(blackOutWindow).show();
        if ($(document).height() > $(window).height()) {
            var windowScrollTop = htmlElement.scrollTop() ? htmlElement.scrollTop() : bodyElement.scrollTop();
            if (windowScrollTop < 0) {
                windowScrollTop = 0;
            }
            htmlElement.addClass('lock-browser-scroll').css('top', -windowScrollTop);
        }
    },
    unlock: function (blackOutWindow) {
        var htmlElement = $('html'),
            windowScrollTop = parseInt(htmlElement.css('top'));

        $(blackOutWindow).hide();
        htmlElement.removeClass('lock-browser-scroll');
        $('html, body').scrollTop(-windowScrollTop);
    }
};

/* popup state */
var appendState = function (state) {
    window.history.pushState(state, '', '');
};

$(window).on('popstate', function (event) {
    if ($('#gallery-blackOut-window').is(':visible')) {
        gallery.close();
    }
});