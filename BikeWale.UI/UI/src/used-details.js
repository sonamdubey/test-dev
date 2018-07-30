$(document).ready(function () {
    var makeOverallTabs = $('#makeOverallTabs'),
        overallMakeDetailsFooter = $('#overallMakeDetailsFooter'),
        makeTabsContentWrapper = $('#makeTabsContentWrapper');

    makeOverallTabs.find('.overall-specs-tabs-wrapper a').first().addClass('active');

    var mainCarousel = $("#bike-main-carousel");
    mainCarousel.find(".jcarousel").jcarousel();

    mainCarousel.find(".jcarousel-control-prev").jcarouselControl({
        target: '-=1'
    });

    mainCarousel.find(".jcarousel-control-next").jcarouselControl({
        target: '+=1'
    });


    $(window).scroll(function () {
        try {
            var windowScrollTop = $(window).scrollTop(),
                    makeOverallTabsOffsetTop = makeOverallTabs.offset().top,
                    makeDetailsFooterOffsetTop = overallMakeDetailsFooter.offset().top,
                    makeTabsContentWrapperOffsetTop = makeTabsContentWrapper.offset().top;

            if (windowScrollTop > makeOverallTabsOffsetTop) {
                makeOverallTabs.addClass('fixed-tab');
            }

            else if (windowScrollTop < makeTabsContentWrapperOffsetTop) {
                makeOverallTabs.removeClass('fixed-tab');
            }

            if (windowScrollTop > makeDetailsFooterOffsetTop - 44) { //44 height of top nav bar
                makeOverallTabs.removeClass('fixed-tab');
            }

            $('#makeTabsContentWrapper .bw-model-tabs-data').each(function () {
                var top = $(this).offset().top - makeOverallTabs.height(),
                bottom = top + $(this).outerHeight();
                if (windowScrollTop >= top && windowScrollTop <= bottom) {
                    makeOverallTabs.find('a').removeClass('active');
                    $('#makeTabsContentWrapper .bw-mode-tabs-data').removeClass('active');

                    $(this).addClass('active');
                    makeOverallTabs.find('a[href="#' + $(this).attr('id') + '"]').addClass('active');
                }
            });
        } catch (e) {

        }

    });

    $('.overall-specs-tabs-wrapper a[href^="#"]').click(function () {
        var target = $(this.hash);
        if (target.length == 0) target = $('a[name="' + this.hash.substr(1) + '"]');
        if (target.length == 0) target = $('html');
        $('html, body').animate({ scrollTop: target.offset().top - makeOverallTabs.height() }, 1000);
        return false;
    });

});

(function ($) {
    var connector = function (itemNavigation, carouselStage) {
        return carouselStage.jcarousel('items').eq(itemNavigation.index());
    };

    $(function () {
        // Setup the carousels. Adjust the options for both carousels here.
        var carouselStage = $('.carousel-stage').jcarousel();
        var carouselNavigation = $('.carousel-navigation').jcarousel();

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
                target: '-=4'
            });

        $('.photos-next-navigation')
            .on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
            })
            .on('jcarouselcontrol:active', function () {
                $(this).removeClass('inactive');
            })
            .jcarouselControl({
                target: '+=4'
            });

        $('.carousel-stage, .carousel-navigation').on('jcarousel:visiblein', 'li', function (event, carousel) {
            $(this).find("img.lazy").trigger("imgLazyLoad");
        });
    });
})(jQuery);

var photosCount = $("#bike-main-carousel ul li").length;
$('#bike-main-carousel').on('click', 'li', function () {
    if (photosCount > 1)
    {
        var imgIndex = $(this).index();
        gallery.open(imgIndex);
    }
       
});

$('.model-media-details').on('click', function () {
    $('#bike-main-carousel li').first().trigger('click');
});

$('.photos-prev-stage, .photos-next-stage').click(function () {
    getImageIndex($(this));
});

$('.carousel-navigation').on('click', 'li', function () {
    var imgIndex = $(this).index() + 1;

    setImageDetails(imgIndex);
});

$(".modelgallery-close-btn, .blackOut-window-model").click(function () {
    gallery.close();
});

$(document).on('keydown', function (e) {
    if (e.keyCode === 27) {
        if ($('#bike-gallery-popup').is(':visible')) {
            gallery.close();
        }
    }
});

var gallery = {
    container: $('#bike-gallery-popup'),

    open: function (imgIndex) {
        $('body').addClass('lock-browser-scroll');
        $('.blackOut-window-model').show();
        gallery.container.show().find('img.lazy').trigger('imgLazyLoad');
        setGalleryImage(imgIndex);
        getImageDetails();
    },

    close: function () {
        $('body').removeClass('lock-browser-scroll');
        $('.blackOut-window-model').hide();
        gallery.container.hide();
        var galleryThumbIndex = $('.carousel-navigation li.active').index();
        $('#bike-main-carousel .jcarousel').jcarousel('scroll', galleryThumbIndex);
    }
}

var setGalleryImage = function (currentImgIndex) {
    $('.carousel-stage').jcarousel('scroll', currentImgIndex);
};

var imgTotalCount;

var getImageDetails = function () {
    imgTotalCount = $('.carousel-stage li').length;
    var imgIndex = $('.carousel-navigation li.active').index();
    imgIndex = imgIndex + 1;
    setImageDetails(imgIndex);
};

var getImageIndex = function (element) {
    var imgIndex;

    if ($(element).hasClass('photos-prev-stage')) {
        imgIndex = $('.carousel-navigation li.active').prev();
    }
    else {
        imgIndex = $('.carousel-navigation li.active').next();
    }
    
    imgIndex = imgIndex.index() + 1;
    setImageDetails(imgIndex);
}

var setImageDetails = function (imgIndex) {
    if (imgIndex > 0) {
        $(".bike-gallery-count").text(imgIndex.toString() + "/" + imgTotalCount.toString());
    }
}

$('#request-media-btn').on('click', function () {
    $('html').removeClass('lock-browser-scroll');
    $('.blackOut-window').show();
});

$('#submit-request-sent-btn, .request-media-close').on('click', function () {
    $('.blackOut-window').hide();
});

$('.blackOut-window').on('click', function () {
    if ($('#request-media-popup').is(':visible')) {
        requestMediaPopup.close();
    }
});

var appendHash = function (state) {
    window.location.hash = state;
};