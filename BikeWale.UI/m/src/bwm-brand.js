$("#sort-btn").click(function () {
    $("#sort-by-div").slideToggle('fast');
    $("html, body").animate({ scrollTop: $("header").offset().top }, 0);
});

$('#sort-by-div a[data-title="sort"]').click(function () {
    var dt = '';
    var list = $(".listitems> .front");
    $.scrollToTop();
    $.so = '0';
    if ($(this).hasClass('price-sort')) {
        var sortOrder = $(this).attr('so');
        var sortedText = $('.price-sort').find('span');
        if (sortOrder == undefined || sortOrder == '0') {
            $.so = '0';
        }
        else {
            $.so = '1';
        }
        $(this).attr('so', $.so);

        if (sortOrder != undefined) {
            if (sortedText.text() === ': Low') {
                $.so = '1';
                dt = sortResults(list, 'prc', false);
                pushGaTags('Price_High_to_Low');
            }
            else {
                $.so = '0';
                dt = sortResults(list, 'prc', true);
                pushGaTags('Price_Low_to_High');
            }
        }
        else {
            dt = sortResults(list, 'prc', true);
            pushGaTags('Price_Low_to_High');
        }
        if ($.so.length > 0) {
            sortedText.css('display', 'inline-block');
            sortedText.text($.so == '1' ? ": High" : ": Low");
        }
    }
    else {
        $.sc = $(this).parent().attr('sc');
        if ($.sc == '') {
            dt = sortResults(list, 'pop', true);
            pushGaTags('Popularity');
        }
        else {
            dt = sortResults(list, 'mlg', false);
            pushGaTags('Mileage_High_to_Low');
        }
        $('.price-sort').find('span').text('');
    }
    $('#sort-by-div a[data-title="sort"]').removeClass('text-bold');
    $(this).addClass('text-bold');
    $(this).parent().removeClass('text-bold');
    var htm = '';
    for (var i = 0, l = dt.length; i < l; i++) {
        htm += dt[i].outerHTML;
    }
    $(".listitems").html('')
    var ul = document.getElementById('listitems');
    ul.insertAdjacentHTML('beforeend', htm);
    applyTabsLazyLoad();
});

$.scrollToTop = function () {
    $('body,html').animate({
        scrollTop: 0
    }, 1000);
};

function sortResults(mydata, prop, asc) {
    return mydata.sort(function (a, b) {
        if (asc) return (parseInt($(a).attr(prop)) > parseInt($(b).attr(prop))) ? 1 : ((parseInt($(a).attr(prop)) < parseInt($(b).attr(prop))) ? -1 : 0);
        else return (parseInt($(b).attr(prop)) > parseInt($(a).attr(prop))) ? 1 : ((parseInt($(b).attr(prop)) < parseInt($(a).attr(prop))) ? -1 : 0);
    });
}

function applyTabsLazyLoad() {
    $("img.lazy").lazyload({
        failure_limit: 20
    });
}

function pushGaTags(label) {
    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Make_Page', 'act': 'Sort_Clicked', 'lab': label });
}

$(document).ready(function () {

    var $window = $(window),
        listitems = $('#listitems'),
        listItemsFooter = $('#listItemsFooter'),
        overallSpecsTabsContainer = $('.overall-specs-tabs-container'),
        makeOverallTabsWrapper = $('#makeOverallTabsWrapper'),
        makeSpecsFooter = $('#makeSpecsFooter'),
        topNavBarHeight = overallSpecsTabsContainer.height();

    var tabsLength = $('.overall-specs-tabs-wrapper li').length - 1;

    makeOverallTabsWrapper.find('.overall-specs-tabs-wrapper li').first().addClass('active');

    $(window).scroll(function () {
        var windowScrollTop = $window.scrollTop(),
            listItemsFooterOffsetTop = listItemsFooter.offset().top,
            makeOverallTabsOffsetTop = makeOverallTabsWrapper.offset().top,
            makeSpecsFooterOffsetTop = makeSpecsFooter.offset().top;

        if ($('header').offset().top > 90) {
            if (windowScrollTop > $('header').offset().top)
                showHeaderDiv();
        }

        else if ($('header').offset().top < 90) {
            showHeaderDiv();
        }

        if($('body').hasClass('listing-navbar-active')) {
            if (windowScrollTop > listItemsFooterOffsetTop - 120) {
                $('header, #sort-by-div').removeClass('fixed');
                $('#sort-by-div').hide();
                $('body').removeClass('listing-navbar-active');
            }
            else if (windowScrollTop == 0 || windowScrollTop < 100) {
                $('header, #sort-by-div').removeClass('fixed');
                $('body').removeClass('listing-navbar-active');
            }
        }

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
                $('#makeTabsContentWrapper .bw-mode-tabs-data').removeClass('active');
                $(this).addClass('active');
                var currentActiveTab = overallSpecsTabsContainer.find('li[data-tabs="#' + $(this).attr('id') + '"]');
                overallSpecsTabsContainer.find(currentActiveTab).addClass('active');
            }
        });

        var scrollToTab = $('#makeReviewsContent');
        if (scrollToTab.length != 0) { 
            if (windowScrollTop > scrollToTab.offset().top - 45) {
                if (!$('#overallSpecsTab').hasClass('scrolled-left')) {
                    $('.overall-specs-tabs-container').addClass('scrolled-left');
                    scrollHorizontal(300);
                }
            }

            else if (windowScrollTop < scrollToTab.offset().top) {
                if ($('#overallSpecsTab').hasClass('scrolled-left')) {
                    $('.overall-specs-tabs-container').removeClass('scrolled-left');
                    scrollHorizontal(0);
                }
            }
        }
        
    });

    function showHeaderDiv() {
        $('header, #sort-by-div').addClass('fixed');
        $('header').show();
        $('body').addClass('listing-navbar-active');
        if (!$('body').hasClass('listing-navbar-active'))
            $('#sort-by-div').hide();
    }

    function scrollHorizontal(pos) {
        $('#overallSpecsTab').animate({ scrollLeft: pos + 'px' }, 500);
    }

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

$('a.read-more-model-preview').click(function () {
    if (!$(this).hasClass('open')) {
        var self = $(this);
        $('.model-preview-main-content').hide();
        $('.model-preview-more-content').show();
        self.text(self.text() === 'Read more' ? 'Collapse' : 'Read more');
        self.addClass("open");
    }
    else if ($(this).hasClass('open')) {
        var self = $(this);
        $('.model-preview-main-content').show();
        $('.model-preview-more-content').hide();
        self.text(self.text() === 'Read more' ? 'Collapse' : 'Read more');
        self.removeClass('open');
    }
});
