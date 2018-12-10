$(document).ready(function () {

    var slideToClick = function (swiper) {
        var clickedSlide = swiper.slides[swiper.clickedIndex];
        swiper.slides.removeClass('swiper-slide-active');
        $(clickedSlide).addClass('swiper-slide-active');
        galleryTop.slideTo(swiper.clickedIndex, 500);
        window.dispatchEvent(new Event('resize'));
    };

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
        onTap: slideToClick,
        nextButton: '.navigation-photos .swiper-button-next',
        prevButton: '.navigation-photos .swiper-button-prev'
    });
    var slidegalleryThumbs = function (swiper) {
        galleryThumbs.slideTo(swiper.activeIndex, 500);
        galleryThumbs.slides.removeClass('swiper-slide-active');
        galleryThumbs.slides[swiper.activeIndex].className += ' swiper-slide-active';
    };

    var galleryTop = $('.carousel-stage-photos').swiper({
        nextButton: '.stage-photos .swiper-button-next',
        prevButton: '.stage-photos .swiper-button-prev',
        spaceBetween: 10,
        preloadImages: false,
        lazyLoading: true,
        lazyLoadingInPrevNext: true,
        watchSlidesProgress: true,
        watchSlidesVisibility: true,
        pagination: '.swiper-pagination',
        onSlideChangeEnd: slidegalleryThumbs
    });

    var calculatePos = function () {
        var diffX = Math.round(($(window).width() - $(".yt-video-iframe").width()) / 2),
        diffY = Math.round(($(window).height() - $(".yt-video-iframe").height()) / 2),
        cTop = (diffY) + scrollY,
        cLeft = diffX + scrollX;
        $(".yt-video-iframe").css({ 'top': cTop + 'px', 'left': cLeft, 'z-index': 999 });
    };

    var initYtVideo = function () {
        $(".yt-video-iframe").css("height", $(window).height() - 100);
        $(".yt-video-iframe iframe").css("height", $(window).height() - 100);
        $(".close-btn").on('click', function () {
            $(".yt-video-iframe").hide();
            Common.utils.unlockPopup();
            player.pauseVideo();
            $(".close-btn").hide();
        });
    };

    var resizeTimeout = setTimeout(function () {
        window.dispatchEvent(new Event("resize"));
        clearTimeout(resizeTimeout);
    }, 500);

    $(window).on('orientationchange', function () {
        window.dispatchEvent(new Event("resize"));
        calculatePos();
        $('.connected-carousels-photos .carousel-photos', function () {
            $(this).css({
                "height": "auto"
            });
        });
    });

    $(".video-slide").on('click', function () {
        Common.utils.lockPopup();
        $(".yt-video-iframe").show();
        calculatePos();
        player.playVideo();
        $(".close-btn").show();
    });
    $(".globalPopupBlackOut").on('click', function () {
        $(".yt-video-iframe").hide();
        Common.utils.unlockPopup();
        player.pauseVideo();
        $(".close-btn").hide();
    });

    $(".blackOut-window").on('click', function () {
        $(".yt-video-iframe").hide();
        Common.utils.unlockPopup();
        player.pauseVideo();
        $(".close-btn").hide();
    });
    dataLayer.push({
        event: 'CWNonInteractive',
        cat: 'CW_Modelpage_300 350_m',
        act: 'BMW_760li_shown',
        lab: 'BMW_760li_shown'
    });
    var act = '';
    $(".click_track").click(function () {
        label = $(this).attr('data-label');
        dataLayer.push({
            event: 'CWInteractive',
            cat: 'CW_Modelpage_300 350_m',
            act: 'BMW_760li_click' + "_" + label + "_" + ModelName,
            lab: label + "_" + ModelName
        });
    });

    initYtVideo();
});

var player;
function onYouTubeIframeAPIReady() {
    player = new YT.Player('muteYouTubeVideoPlayer', {
        videoId: 'ob_ZvRLjbUU', // YouTube Video ID
        playerVars: {
            'rel': 0, 'showinfo': 0
        },
        events: {
            onReady: function (e) {
            },
            'onStateChange': onPlayerStateChange
        }
    });
}
var onPlayerStateChange = function (event) {
    if (event.data === YT.PlayerState.ENDED) {
        $(".yt-video-iframe").hide();
        Common.utils.unlockPopup();
        player.pauseVideo();
        $(".close-btn").hide();
    }
};