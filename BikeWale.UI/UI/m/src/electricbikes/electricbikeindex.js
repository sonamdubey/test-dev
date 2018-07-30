$(document).ready(function () {

    $(".nav-tab").click(function () {

        try {
            triggerGA('Electric_Bikes', 'Floating_Navigation_Clicked', $(this).data("lab"));
        }
        catch (e) {
            console.log(e);
        }
    });

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

        var makeTabsContentWrapper = $('#makeTabsContentWrapper');
        var tabElementThird = makeTabsContentWrapper.find('.bw-model-tabs-data:eq(3)'),
        tabElementSixth = makeTabsContentWrapper.find('.bw-model-tabs-data:eq(6)'),
        tabElementEighth = makeTabsContentWrapper.find('.bw-model-tabs-data:eq(8)');

        if (tabElementThird.length != 0) {
            focusFloatingTab(tabElementThird, 250, 0);
        }

        if (tabElementSixth.length != 0) {
            focusFloatingTab(tabElementSixth, 500, 250);
        }

        if (tabElementEighth.length != 0) {
            focusFloatingTab(tabElementEighth, 750, 500);
        }

        function focusFloatingTab(element, startPosition, endPosition) {
            if (windowScrollTop > element.offset().top - 45) {
                if (!$('#overallSpecsTab').hasClass('scrolled-left-' + startPosition)) {
                    $('.overall-specs-tabs-container').addClass('scrolled-left-' + startPosition);
                    scrollHorizontal(startPosition);
                }
            }

            else if (windowScrollTop < element.offset().top) {
                if ($('#overallSpecsTab').hasClass('scrolled-left-' + startPosition)) {
                    $('.overall-specs-tabs-container').removeClass('scrolled-left-' + startPosition);
                    scrollHorizontal(endPosition);
                }
            }
        };

    });

    function scrollHorizontal(pos) {
        $('#overallSpecsTab').animate({ scrollLeft: pos + 'px' }, 500);
    }

    $('.overall-specs-tabs-wrapper li').click(function () {
        var target = $(this).attr('data-tabs');
        $('html, body').animate({ scrollTop: $(target).offset().top - overallSpecsTabsContainer.height() }, 1000);
        centerItVariableWidth($(this), '.overall-specs-tabs-container');
        triggerGA('Electric_Bikes', 'Floating_NavigationClicked', $(this).data("lab"));
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