/* Swiper script starts here */
var Swiper = {
    //Initialize Function
    Initialize: function () {
        $('.swiper-container:not(".noSwiper")').each(function (index, element) {
            var currentSwiper = $(this);
            currentSwiper.addClass('sw-' + index).swiper({
                nextButton: currentSwiper.find('.swiper-button-next'),
                prevButton: currentSwiper.find('.swiper-button-prev'),
                pagination: currentSwiper.find('.swiper-pagination'),
                slidesPerView: 'auto',
                paginationClickable: true,
                spaceBetween: 10,
                watchSlidesVisibility: true,
                onSlideChangeStart: Swiper.slideChangeStart,
                onTransitionEnd: Swiper.transitionEnd,
                onInit: Swiper.initSwiper,
                preloadImages: false,
                lazyLoading: true,
                lazyLoadingInPrevNext: true
            });
        });
        $(".swiper-slide img[data-original]").each(function () {
            $(this).closest('.imageWrapper').addClass('swiper-imgLoader');
        });
    },

    //applyLazyLoad Function
    applyLazyLoad: function (swipebox) {
        try {
            var lazyImg = swipebox.container.find('li.swiper-slide-visible img.lazy');
            for (var i = 0; i < lazyImg.length; i++) {
                var $lazyImg = $(lazyImg[i]);
                var dataOriginal = $lazyImg.attr('data-original');
                var dataSrc = $lazyImg.attr('src');
                if (dataSrc.indexOf("grey.gif") > 0 || dataSrc.indexOf("no-cars.jpg") > 0 || dataSrc == '' || dataSrc == undefined || dataSrc == null) {
                    $lazyImg.attr('src', dataOriginal);
                }
            }
        } catch (e) { console.log(e); }
    },

    //slideChangeStart Function
    slideChangeStart: function () {
        if (Swiper.YouTubeApi.playerState == 'playing' || Swiper.YouTubeApi.playerState == 'buffering') {
            try {
                Swiper.YouTubeApi.videoPause();
            } catch (e) {
                console.log(e);
            }
        }
    },

    //transitionEnd Function
    transitionEnd: function (swiper) {
        Swiper.applyLazyLoad(swiper);
    },

    //paginationLoad Function
    paginationLoad: function (swiper) {
        if (swiper.slides.length == 1) {
            swiper.slides.addClass('fullWidth')
        }
        else if (swiper.slides.length > 1) { swiper.slides.removeClass('fullWidth') };
    },

    //initSwiper Function
    initSwiper: function (swiper) {
        setTimeout(function () { Swiper.paginationLoad(swiper); }, 300);
        $(window).resize(function () { swiper.update(true); })
    },

    //YouTubeApi Function
    YouTubeApi: {
        player: new Array(),
        id: '',
        count: 0,
        countArray: [],
        playerState: '',
        targetClick: '',
        targetOverlay: '',
        videoPos: '',
        addApiScript: function () {
            var tag = document.createElement('script');
            tag.src = "https://www.youtube.com/iframe_api";
            var firstScriptTag = document.getElementsByTagName('script')[0];
            firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
            Swiper.YouTubeApi.ApiCommon();
        },

        ApiCommon: function () {
            window.onYouTubeIframeAPIReady = function () {
                var i = 1;
                $('.swiper-wrapper iframe').each(function () {
                    Swiper.YouTubeApi.id = $(this).attr('id');
                    Swiper.YouTubeApi.videoPos = $(this).position();
                    Swiper.YouTubeApi.player[i] = new YT.Player(Swiper.YouTubeApi.id, {
                        events: {
                            'onStateChange': Swiper.YouTubeApi.onPlayerStateChange,
                            "onReady": Swiper.YouTubeApi.onPlayerReady,
                            "onError": Swiper.YouTubeApi.onPlayerError
                        }
                    });
                    i++;
                });
            }
            $('.yt-iframe-preview').append('<span class="overlay" />');

            //play video on click
            $(document).on('click', '.swiper-slide', function (event) {
                Swiper.YouTubeApi.targetClick = $(event.target).attr('class');
                if (Swiper.YouTubeApi.targetClick == 'overlay') {
                    Swiper.YouTubeApi.targetOverlay = $(this).find('span.overlay');
                    Swiper.YouTubeApi.videoPlay();
                }
            });

            //pause youtube video on scroll when video is not in viewport
            var videoElement = $("#Videos");
            var inViewPortTopBtm = '', inViewPortLeftRight = '', videoFrame = '';
            var handler = function () {
                try {
                    if (videoElement.is(':visible') && videoElement.find('iframe.current').length > 0) {
                        videoFrame = videoElement.find('iframe.current');
                        inViewPortTopBtm = Common.utils.isElementInViewportTopBottom(videoFrame);
                        inViewPortLeftRight = Common.utils.isElementInViewportLeftRight(videoFrame);
                        if (!inViewPortTopBtm && (Swiper.YouTubeApi.playerState == 'playing' || Swiper.YouTubeApi.playerState == 'buffering'))
                            Swiper.YouTubeApi.videoPause();
                        else if (inViewPortTopBtm && inViewPortLeftRight && Swiper.YouTubeApi.playerState == 'paused') Swiper.YouTubeApi.videoPlay();
                    }
                } catch (e) { }
            };

            $(window).on('resize scroll', handler);

        },

        //you tube player state change event
        onPlayerStateChange: function (event) {
            switch (event.data) {
                case YT.PlayerState.UNSTARTED:
                    Swiper.YouTubeApi.playerState = 'unstarted';
                    break;
                case YT.PlayerState.ENDED:
                    Swiper.YouTubeApi.playerState = 'ended';
                    $('.yt-iframe-preview .overlay').show();
                    break;
                case YT.PlayerState.PLAYING:
                    Swiper.YouTubeApi.playerState = 'playing';
                    break;
                case YT.PlayerState.PAUSED:
                    Swiper.YouTubeApi.playerState = 'paused';
                    break;
                case YT.PlayerState.BUFFERING:
                    Swiper.YouTubeApi.playerState = 'buffering';
                    break;
                case YT.PlayerState.CUED:
                    Swiper.YouTubeApi.playerState = 'cued';
                    break;
            }
        },

        onPlayerReady: function (event) { },

        onPlayerError: function (event) { console.log('onPlayerError:error!'); },

        videoPlay: function () {
            if (Swiper.YouTubeApi.playerState == 'playing' || Swiper.YouTubeApi.playerState == 'buffering') {
                Swiper.YouTubeApi.videoPause();
            }
            $('.swiper-wrapper iframe').removeClass('current');
            Swiper.YouTubeApi.targetOverlay.prev().addClass('current');
            Swiper.YouTubeApi.count = Swiper.YouTubeApi.targetOverlay.siblings('iframe.current').attr('id').replace('video_', '');
            Swiper.YouTubeApi.player[Swiper.YouTubeApi.count].playVideo();
            $('#video_' + Swiper.YouTubeApi.count + '.current').siblings('span.overlay').hide();
            Swiper.YouTubeApi.countArray.push(Swiper.YouTubeApi.count);
        },

        videoPause: function () {
            for (var j = 0; j < Swiper.YouTubeApi.countArray.length; j++) {
                Swiper.YouTubeApi.player[Swiper.YouTubeApi.countArray[j]].pauseVideo();
            }
            $('.swiper-slide .overlay:not(":visible")').show();
            Swiper.YouTubeApi.countArray = [];
        },
    }
};
/* Swiper script ends here */

//initialize swiper
$(document).ready(function () {
    setTimeout(function () { if (Swiper.Initialize) Swiper.Initialize(); }, 0);
});