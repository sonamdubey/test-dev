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

// read more - collapse
$('.read-more-target').on('click', function () {
    var element = $(this),
        parentElemtent = element.closest('.collapsible-content');

    if (!parentElemtent.hasClass('active')) {
        parentElemtent.addClass('active');
        element.text(' Collapse');
    }
    else {
        parentElemtent.removeClass('active');
        element.text('Read more');
    }
});


// floating tabs
var makeOverallTabs = $('#makeOverallTabs'),
    overallMakeDetailsFooter = $('#overallMakeDetailsFooter'),
    makeTabsContentWrapper = $('#makeTabsContentWrapper');

makeOverallTabs.find('.overall-specs-tabs-wrapper a').first().addClass('active');

var makeDealersContent = $('#makeDealersContent');

if (makeDealersContent.length != 0) {
    makeDealersContent.removeClass('bw-model-tabs-data');
}

attachListener('scroll', window, highlightSpecTabs);

$('.overall-specs-tabs-wrapper a[href^="#"]').click(function () {
    var target = $(this.hash);
    if (target.length == 0) target = $('a[name="' + this.hash.substr(1) + '"]');
    if (target.length == 0) target = $('html');
    $('html, body').animate({ scrollTop: target.offset().top - makeOverallTabs.height() }, 1000);
    return false;
});

function highlightSpecTabs() {
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
}