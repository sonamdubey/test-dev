/* jCarousel custom methods */
var _target = 3;

$(function () {
    if (typeof (videoSlider) != 'undefined') {
        _target = 1
    }

    var jcarousel = $('.jcarousel').jcarousel({
        vertical: false
    });

    $('.jcarousel-control-prev').each(function () {
        var element = $(this);
        var dynamicCarousel = element.closest('.dynamic-carousel')
        var target = dynamicCarousel.length ? dynamicCarousel.attr('data-swipeCount') : _target;
        element.on('jcarouselcontrol:active', function () {
            $(this).removeClass('inactive');
        }).on('jcarouselcontrol:inactive', function () {
            $(this).addClass('inactive');
        }).jcarouselControl({
            target: '-=' + target
        });
    })

    $('.jcarousel-control-next').each(function () {
        var element = $(this);
        var dynamicCarousel = element.closest('.dynamic-carousel')
        var target = dynamicCarousel.length ? dynamicCarousel.attr('data-swipeCount') : _target;
        element.on('jcarouselcontrol:active', function () {
            $(this).removeClass('inactive');
        }).on('jcarouselcontrol:inactive', function () {
            $(this).addClass('inactive');
        }).jcarouselControl({
            target: '+=' + target
        });
    })

    $('.jcarousel-pagination').on('jcarouselpagination:active', 'a', function () {
        $(this).addClass('active');
    }).on('jcarouselpagination:inactive', 'a', function () {
        $(this).removeClass('active');
    }).on('click', function (e) {
        e.preventDefault();
    }).jcarouselPagination({
        item: function (page) {
            return '<a href="#' + page + '">' + page + '</a>';
        }
    });

    // Swipe handlers for mobile
    $(".jcarousel").swipe({
        fingers: 'all', swipeLeft: swipe1, swipeRight: swipe1, allowPageScroll: "auto"
    });
    function swipe1(event, direction, distance, duration, fingerCount) {
        if (direction == "left") {
            $(this).closest('.jcarousel-wrapper').find("a.jcarousel-control-next").click();
        }
        else if (direction == "right") {
            $(this).closest('.jcarousel-wrapper').find("a.jcarousel-control-prev").click();
        }
    }
});