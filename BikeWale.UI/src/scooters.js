var transparentHeader = document.querySelectorAll('.header-transparent')[0];

if (transparentHeader) {
    attachListener('scroll', window, changeHeaderBackground);
}

function attachListener(event, element, functionName) {
    if (element.addEventListener) {
        element.addEventListener(event, functionName, false);
    }
    else if (element.attachEvent) {
        element.attachEvent('on' + event, functionName);
    }
};

function changeHeaderBackground() {
    if ($(window).scrollTop() > 40)
        $('.header-transparent').removeClass('header-landing').addClass('header-fixed');
    else
        $('.header-transparent').removeClass('header-fixed').addClass('header-landing');
};

// more brand - collapse
$('.view-brandType').click(function (e) {
    var element = $(this),
        elementParent = element.closest('.collapsible-brand-content'),
        moreBrandContainer = elementParent.find('.brandTypeMore');

    if (!moreBrandContainer.is(':visible')) {
        moreBrandContainer.slideDown();
        element.attr('href', 'javascript:void(0)');
        element.text('View less brands');
    }
    else {
        element.attr('href', '#brand-type-container');
        moreBrandContainer.slideUp();
        element.text('View more brands');
    }

    e.preventDefault();
    e.stopPropagtion();

});

$(document).ready(function () {
    var comparisonCarousel = $("#comparisonCarousel");
    comparisonCarousel.find(".jcarousel").jcarousel();

    comparisonCarousel.find(".jcarousel-control-prev").jcarouselControl({
        target: '-=2'
    });

    comparisonCarousel.find(".jcarousel-control-next").jcarouselControl({
        target: '+=2'
    });
});