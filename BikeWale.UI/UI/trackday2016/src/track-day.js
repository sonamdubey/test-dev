(function ($) {

    var connector = function (itemNavigation, carouselStage) {
        return carouselStage.jcarousel('items').eq(itemNavigation.index());
    };
    $(function () {
        var carouselStage = $('.carousel-stage-photos').jcarousel();
        var carouselNavigation = $('.carousel-navigation-photos').jcarousel();

        carouselNavigation.jcarousel('items').each(function () {
            var item = $(this);
            var target = connector(item, carouselStage);
            item
                .on('jcarouselcontrol:active', function () {
                    carouselNavigation.jcarousel('scrollIntoView', this);
                    item.addClass('active');
                })
                .on('jcarouselcontrol:inactive', function () {
                    item.removeClass('active');
                })
                .jcarouselControl({
                    target: target,
                    carousel: carouselStage
                });
        });

        $('.photos-prev-stage')
            .on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
            })
            .on('jcarouselcontrol:active', function () {
                $(this).removeClass('inactive');
            })
            .jcarouselControl({
                target: '-=1'
            });
        $('.photos-next-stage')
            .on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
            })
            .on('jcarouselcontrol:active', function () {
                $(this).removeClass('inactive');
            })
            .jcarouselControl({
                target: '+=1'
            });
        $('.photos-prev-navigation')
            .on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
            })
            .on('jcarouselcontrol:active', function () {
                $(this).removeClass('inactive');
            })
            .jcarouselControl({
                target: '-=7'
            });
        $('.photos-next-navigation')
            .on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
            })
            .on('jcarouselcontrol:active', function () {
                $(this).removeClass('inactive');
            })
            .jcarouselControl({
                target: '+=7'
            });
        $(".carousel-stage-photos, .carousel-navigation-photos").on('jcarousel:visiblein', 'li', function (event, carousel) {
            $(this).find("img.lazy").trigger("imgLazyLoad");
        });
    });
})(jQuery);

$('.stage-photos').on('click', 'li', function () {
    if (!$('body').hasClass('gallery-active')) {
        gallery.open();
        appendState('popupGallery');
    }
});

$('#gallery-close-btn').on('click', function () {
    gallery.close();
});

$(document).on('keydown', function (event) {
    if ($('body').hasClass('gallery-active')) {
        if (event.keyCode === 27) {
            gallery.close();
        }
        if (event.keyCode === 37) {
            $(".photos-prev-stage").click();
        }
        if (event.keyCode === 39) {
            $(".photos-next-stage").click();
        }
    }
});

$(document).ready(function () {
    var visibleThumbnails = $('.carousel-navigation-photos').jcarousel('visible');
    visibleThumbnails.each(function () {
        $(this).find('img.lazy').trigger('imgLazyLoad');
    });

    var trackDayVideo = $('#td-iframe-video'),
        trackDayVideoSource = trackDayVideo.attr('src');

    $('#track-day-video').on('click', '.video-overlay', function (event) {
        $(this).hide();
        trackDayVideo.attr('src', trackDayVideoSource + '?autoplay=1');        
    });
});

var gallery = {

    open: function () {
        $('body').addClass('gallery-active');
        popup.lock('.gallery-blackOut-window');
        gallery.setPosition();
        $('.carousel-stage-photos').jcarousel('reload', {
            animation: 'slow'
        });
    },

    close: function () {
        $('body').removeClass('gallery-active');
        popup.unlock('.gallery-blackOut-window');
        gallery.resetPosition();
        $('.carousel-stage-photos').jcarousel('reload', {
            animation: 'slow'
        });
    },

    setPosition: function () {
        var windowHeight = $(window).height(),
            galleryWidth = (windowHeight * 16) / 9,
            galleryHeight = (galleryWidth * 9) / 16;

        $('.gallery-active .carousel-stage-media li, .gallery-active .stage-media, .gallery-active .navigation-media').css({
            width: galleryWidth - 200
        });

        //var topPosition = ($(window).height() - galleryHeight) / 2;
        $('.connected-carousels-photos').css({
            'top': 10
        });
    },

    resetPosition: function () {
        var windowWidth = $(window).width();

        if (windowWidth >= 1024) {
            $('.carousel-stage-media li, .stage-media, .navigation-media').css({
                width: 976
            });
        }
        else {
            $('.carousel-stage-media li, .stage-media, .navigation-media').css({
                width: 940
            });
        }

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
