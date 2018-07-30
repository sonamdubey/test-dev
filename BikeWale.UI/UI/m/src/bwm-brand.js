ga_pg_id = '3';

docReady(function () {
    $("img.lazy").lazyload();

    $('.jcarousel-wrapper.upComingBikes .jcarousel').on('jcarousel:targetin', 'li', function () {
        $("img.lazy").lazyload({
            threshold: 300
        });
    });
    $('#sort-by-div').insertAfter('header');
    if ($("#discontinuedMore a").length > 4) {
        $('#discontinuedMore').hide();
    }
    else {
        $('#discontinuedLess').hide();
    }
    $("#spnContent").append($("#discontinuedMore a:eq(0)").clone()).append(", ").append($("#discontinuedMore a:eq(1)").clone()).append(", ").append($("#discontinuedMore a:eq(2)").clone()).append(", ").append($("#discontinuedMore a:eq(3)").clone());
    $("#spnContent").append("... <a class='f-small' id='viewall' >View All</a>");
    $('#sort-btn').removeClass('hide').addClass("show");
    $("a.read-more-btn").click(function () {
        $("div.brand-about-more-desc").slideToggle();
        $("div.brand-about-main").slideToggle();
        var a = $(this).find("span");
        a.text(a.text() === "more" ? "less" : "more");
    });
    $('#usedbikebottomlink').hide();

    
    var $window = $(window),
        listitems = $('#listitems'),
        listItemsFooter = $('#listItemsFooter'),
        overallSpecsTabsContainer = $('.overall-specs-tabs-container'),
        makeOverallTabsWrapper = $('#makeOverallTabsWrapper'),
        makeSpecsFooter = $('#makeSpecsFooter'),
        topNavBarHeight = overallSpecsTabsContainer.height();

    var tabsLength = $('.overall-specs-tabs-wrapper li').length - 1;
    if (tabsLength < 2) {
        $('.overall-specs-tabs-wrapper li').css({ 'display': 'inline-block', 'width': 'auto' });
    }

    makeOverallTabsWrapper.find('.overall-specs-tabs-wrapper li').first().addClass('active');

    $(window).on('scroll', function () {
        var windowScrollTop = $window.scrollTop(),
            listItemsFooterOffsetTop = listItemsFooter.offset().top,
            makeOverallTabsOffsetTop = makeOverallTabsWrapper.offset().top,
            makeSpecsFooterOffsetTop = makeSpecsFooter.offset().top;

        if ($('#bw-header').offset().top > 90) {
            if (windowScrollTop > $('#bw-header').offset().top)
                showHeaderDiv();
        }

        else if ($('#bw-header').offset().top < 90) {
            showHeaderDiv();
        }

        if ($('body').hasClass('listing-navbar-active')) {
            if (windowScrollTop > listItemsFooterOffsetTop - 120) {
                $('#bw-header, #sort-by-div').removeClass('fixed');
                $('#sort-by-div').hide();
                $('body').removeClass('listing-navbar-active');
            }
            else if (windowScrollTop == 0 || windowScrollTop < 100) {
                $('#bw-header, #sort-by-div').removeClass('fixed');
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

        var makeTabsContentWrapper = $('#makeTabsContentWrapper');
        var tabElementThird = makeTabsContentWrapper.find('.bw-model-tabs-data:eq(3)'),
        tabElementSixth = makeTabsContentWrapper.find('.bw-model-tabs-data:eq(6)'),
        tabElementNinth = makeTabsContentWrapper.find('.bw-model-tabs-data:eq(9)');

        if (tabElementThird.length != 0) {
            focusFloatingTab(tabElementThird, 250, 0);
        }

        if (tabElementSixth.length != 0) {
            focusFloatingTab(tabElementSixth, 500, 250);
        }

        if (tabElementNinth.length != 0) {
            focusFloatingTab(tabElementNinth, 750, 500);
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

    $("#sort-btn").click(function () {
        $("#sort-by-div").slideToggle('fast');
        $("html, body").animate({ scrollTop: $("#bw-header").offset().top }, 0);
    });
    $("#viewall").click(function () {
        $("#discontinuedLess").hide();
        $("#discontinuedMore").show();
        var xContents = $('#discontinuedMore').contents();
        xContents[xContents.length - 1].nodeValue = "";
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
                    dt = sortResults(list, 'data-price', false);
                    pushGaTags('Price_High_to_Low');
                }
                else {
                    $.so = '0';
                    dt = sortResults(list, 'data-price', true);
                    pushGaTags('Price_Low_to_High');
                }
            }
            else {
                dt = sortResults(list, 'data-price', true);
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
                dt = sortResults(list, 'data-popularity', true);
                pushGaTags('Popularity');
            }
            else {
                dt = sortResults(list, 'data-mileage', false);
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
            if ($(a).attr(prop) == "0") { return 1; }
            if ($(b).attr(prop) == "0") { return -1; }

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



    function showHeaderDiv() {
        $('#bw-header, #sort-by-div').addClass('fixed');
        $('#bw-header').show();
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
        triggerGA('Make_Page', 'Floating_Navigation_Clicked', $(this).data("lab"));
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

    $("ul#listitems li.front").click(function () {
        var cookieValue = getCookie("_bwtest"), bikeName = $(this).attr("data-bike");
        bikeName = bikeName.replace(/\s+/, '_');
        triggerGA("MakePage", "Clicked_on_ModelCard", cookieValue + "_" + bikeName);
    });

    //collapsible content

    $('.read-more-button').on('click', function () {
        var readMoreButton = $(this);
        var collapsibleContent = readMoreButton.closest('.foldable-content');
        var isDataToggle = collapsibleContent.attr('data-toggle');
        var dataTruncate = collapsibleContent.find('.truncatable-content');
        var dataLessText;
        var readLessText;
        switch (isDataToggle) {
            case 'no':
                dataTruncate.attr('data-readtextflag', '0');
                readMoreButton.hide();
                break;
            case 'yes':
                dataLessText = readMoreButton.attr('data-text');
                readLessText = !dataLessText || dataLessText.length === 0 ? 'Collapse' : dataLessText;
                dataTruncate.attr('data-readtextflag', '0');
                readMoreButton.attr('data-text', readMoreButton.text()).text(readLessText);
                collapsibleContent.attr('data-toggle', 'hide');
                break;
            case 'hide':
                dataTruncate.attr('data-readtextflag', '1');
                dataLessText = readMoreButton.attr('data-text');
                readMoreButton.attr('data-text', readMoreButton.text()).text(dataLessText);
                collapsibleContent.attr('data-toggle', 'yes');
                $("html, body").animate({ scrollTop: dataTruncate.offset().top - 35 }, 500);
                break;
        }
    });
    // For saving page in recent viewed items
    if (typeof pageData != "undefined" && pageData != null)
        recentSearches.saveRecentSearches(pageData);

});
