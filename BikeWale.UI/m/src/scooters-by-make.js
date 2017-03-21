
$(document).ready(function () {

    var $window = $(window),
        overallSpecsTabsContainer = $('.overall-specs-tabs-container'),
        makeOverallTabsWrapper = $('#makeOverallTabsWrapper'),
        makeSpecsFooter = $('#makeSpecsFooter'),
        topNavBarHeight = overallSpecsTabsContainer.height();

    var tabsLength = $('.overall-specs-tabs-wrapper li').length - 1;
    if (tabsLength < 2) {
        $('.overall-specs-tabs-wrapper li').css({ 'display': 'inline-block', 'width': 'auto' });
    }

    makeOverallTabsWrapper.find('.overall-specs-tabs-wrapper li').first().addClass('active');

    var makeDealersContent = $('#makeDealersContent');

    if (makeDealersContent.length != 0) {
        makeDealersContent.removeClass('bw-model-tabs-data');
    }

    $(window).scroll(function () {
        var windowScrollTop = $window.scrollTop(),
            makeOverallTabsOffsetTop = makeOverallTabsWrapper.offset().top,
            makeSpecsFooterOffsetTop = makeSpecsFooter.offset().top;

        if (windowScrollTop > makeOverallTabsOffsetTop) {
            overallSpecsTabsContainer.addClass('fixed-tab-nav');
        }
        else if (windowScrollTop < makeOverallTabsOffsetTop) {
            overallSpecsTabsContainer.removeClass('fixed-tab-nav');
        }
        if (overallSpecsTabsContainer.hasClass('fixed-tab-nav')) {
            if (windowScrollTop > makeSpecsFooterOffsetTop - topNavBarHeight)
                overallSpecsTabsContainer.removeClass('fixed-tab-nav');
        }

        $('#makeTabsContentWrapper .bw-model-tabs-data').each(function () {
            var top = $(this).offset().top - overallSpecsTabsContainer.height(),
                bottom = top + $(this).outerHeight();

            if (windowScrollTop >= top && windowScrollTop <= bottom) {
                overallSpecsTabsContainer.find('li').removeClass('active');
                $('#makeTabsContentWrapper .bw-model-tabs-data').removeClass('active');
                $(this).addClass('active');
                var currentActiveTab = overallSpecsTabsContainer.find('li[data-tabs="#' + $(this).attr('id') + '"]');
                overallSpecsTabsContainer.find(currentActiveTab).addClass('active');
            }
        });

    });

    $('.overall-specs-tabs-wrapper li').click(function () {
        var target = $(this).attr('data-tabs');
        $('html, body').animate({ scrollTop: $(target).offset().top - overallSpecsTabsContainer.height() }, 1000);
        centerItVariableWidth($(this), '.overall-specs-tabs-container');
        return false;
    });

    function centerItVariableWidth(target, outer) {
        var out = $(outer);
        var tar = target;
        var x = out.width();
        var y = tar.outerWidth(true);
        var z = tar.index();
        var q = 0;
        var m = out.find('li');
        for (var i = 0; i < z; i++) {
            q += $(m[i]).outerWidth(true);
        }
        out.animate({ scrollLeft: Math.max(0, q - (x - y) / 2) }, 500, 'swing');
    }

});