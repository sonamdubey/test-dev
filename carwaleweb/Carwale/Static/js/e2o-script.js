var Electric = {
    personLeadCity: {},
    newWidth: null,

    responsive: {

        responsiveCarousel: function () {
            var jcarousel = $('.vehicles .jcarousel');
            jcarousel
                .on('jcarousel:reload jcarousel:create', function () {
                    var carousel = $(this);
                    if (Electric.newWidth == null) {
                        Electric.newWidth = carousel.innerWidth();
                    }
                    carousel.jcarousel('items').css('width', Math.ceil(Electric.newWidth) + 'px');
                })
                .jcarousel();
        },

        responsiveFB:function()
        {
            var container_width = $('#pageFbContainer').width();
            $('#pageFbContainer').html('<div class="fb-page" data-href="https://www.facebook.com/carwale" data-width="' + container_width + '" data-height="430" data-small-header="false" data-adapt-container-width="true" data-hide-cover="false" data-show-facepile="false" data-show-posts="true"><div class="fb-xfbml-parse-ignore"><blockquote cite="https://www.facebook.com/carwale"><a href="https://www.facebook.com/carwale">Facebook</a></blockquote></div></div>');
            FB.XFBML.parse();
        },

        registerEvent: function () {

            $('.full-width-carousel .jcarousel').jcarousel('scroll', 0);
            $(".full-width-carousel .jcarousel ul li").css("width", $(window).outerWidth());
            var activeTab = $("#mahindra-cw-tabs li").first();
            activeTab.trigger("click");
            $('.vehicles .jcarousel').jcarousel('scroll', 0);
            Electric.newWidth = null;
            Electric.responsive.responsiveCarousel();
            
            $("#mahindra-cw-tabs li").click(function () {
                if (!$(this).hasClass('active')) {
                    var label = $(this).attr('data-tabs');
                    Electric.leadForm.TrackLinks("mahindra-cw-tabs", label);
                }
            });
        }
    },

    lazyImg: {

        callEventLazy: function () {
            $("img.lazy").lazyload();
            $(window).load(function () {
                Electric.lazyImg.applyLazyLoad();
            });
        },

        applyLazyLoad: function () {
            try {
                var lazyImg = $(document).find('.card-jcarousel .jcarousel img.lazy');
                for (var i = 0; i < lazyImg.length; i++) {
                    var $lazyImg = $(lazyImg[i]);
                    var dataOriginal = $lazyImg.attr('data-original');
                    var dataSrc = $lazyImg.attr('src');
                    if (dataSrc == '' || dataSrc == undefined || dataSrc == null) {
                        $lazyImg.attr('src', dataOriginal);
                    }
                }
            } catch (e) { }
        }
    }

};

var appendState = function (state) {
    window.history.pushState(state, '', '');
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
        window.dispatchEvent(new Event('resize'));
        $(blackOutWindow).hide();
        htmlElement.removeClass('lock-browser-scroll');
        $('html, body').scrollTop(-windowScrollTop);
    }
};


var windowWidth, resizeId;

var galleryTop1 = $('.carousel-stage-photos1').swiper({
    nextButton: '.wd-btn-next',
    prevButton: '.wd-btn-prev',
    spaceBetween: 10,
    preloadImages: false,
    lazyLoading: true,
    lazyLoadingInPrevNext: true,
    watchSlidesProgress: true,
    watchSlidesVisibility: true,
    lazyLoadingOnTransitionStart: true
});

var windowWidth, resizeId;

var e2oArticles = $('.carousel-stage-article').swiper({
    nextButton: '.feature-btn-next',
    prevButton: '.feature-btn-prev',
    spaceBetween: 0,
    slidesPerView: 'auto',
    preloadImages: false,
    lazyLoading: true,
    lazyLoadingInPrevNext: true,
    watchSlidesProgress: true,
    watchSlidesVisibility: true,
    lazyLoadingOnTransitionStart: true
});
var gallerySwipper = $('.gallery-carousel').swiper({
	nextButton: '.gallery-next-btn',
	prevButton: '.gallery-prev-btn',
	spaceBetween: 0,
	slidesPerView: 'auto',
	preloadImages: false,
	lazyLoading: true,
	lazyLoadingInPrevNext: true,
	watchSlidesProgress: true,
	watchSlidesVisibility: true,
	lazyLoadingOnTransitionStart: true
});
$(document).ready(function () {

    $("body").removeClass("rsz-lyt");
    windowWidth = $(window).width();

    Electric.responsive.registerEvent();
    Electric.lazyImg.callEventLazy();
    //  Electric.responsive.responsiveCarousel();

    $(".e2o-main .jcarousel").on('jcarousel:visiblein', 'li', function (event, carousel) {
        $(this).find('img').trigger("UNT");
        UNTLazyLoad();
    });
   
    $(".edirve-cw-tabs li").on('click', function () {
        var resizeTimeout = setTimeout(function () {
            $(window).trigger('resize');
            clearTimeout(resizeTimeout);
        }, 50);
    });
    $('.social-media__nav-btn li').click(function () {
    	$('.social__facebook-block').toggle();
    	$('.social__twitter-block').toggle();
    	$('.fb-nav-tab').toggleClass('fb-nav-tab-inactive');
    	$('.twitter-nav-tab').toggleClass('twitter-nav-tab-active');

    });

    $('.gallery__swipper__block img').on('click', function () {
    	var getIamgeSource = $(this).attr('src'),
    		newGetImgURL,
    		windowsize = $(window).width();
    	if (windowsize > 800) {
    		newGetImgURL = getIamgeSource.replace('.png', '-large.png');
    	}
    	else {
    		newGetImgURL = getIamgeSource.replace('.png', '-large-m.png');
    	}
    	$('#largePopupImage').attr('src', newGetImgURL);
    	$('.gallery-image__popup').toggle();
    	$('.gallery-btn-block').find('.gallery-next-btn, .gallery-prev-btn').hide();
    
    	
    });
    $('.image__popup__close-btn').on('click', function () {
    	var newGetImgURL = "";
    	$('.gallery-image__popup').toggle();
    	$('.gallery-btn-block').find('.gallery-next-btn, .gallery-prev-btn').show();

    });
});

$(window).on('orientationchange', function () {
    $('.connected-carousels-photos .carousel-photos', function () {
        $(this).css({ "height": "auto" });
    });
});



function UNTLazyLoad() {
    $("img.lazy").lazyload({
        event: "UNT"
    });
}

$(window).resize(function () {
    clearTimeout(resizeId);
    if ($(window).width() != windowWidth) {
        windowWidth = $(window).width();
        resizeId = setTimeout(Electric.responsive.registerEvent, 500);
        Electric.responsive.responsiveFB();
    }
    
});


