
ga_pg_id = '3';
var dt = '';

docReady(function () {
    if ($("#discontinuedMore a") && $("#discontinuedMore a").length > 4) {
        $('#discontinuedMore').hide();
    }
    else {
        $('#discontinuedLess').hide();
    }

    var makeOverallTabs = $('#makeOverallTabs'),
        overallMakeDetailsFooter = $('#overallMakeDetailsFooter'),
        makeTabsContentWrapper = $('#makeTabsContentWrapper');

    makeOverallTabs.find('.overall-specs-tabs-wrapper span').first().addClass('active');

    var makeDealersContent = $('#makeDealersContent');

    if (makeDealersContent.length != 0) {
        makeDealersContent.removeClass('bw-model-tabs-data');
    }

    $(window).scroll(function () {
        var windowScrollTop = $(window).scrollTop(),
            makeOverallTabsOffsetTop = makeOverallTabs.offset().top,
            makeDetailsFooterOffsetTop = overallMakeDetailsFooter.offset().top,
            makeTabsContentWrapperOffsetTop = makeTabsContentWrapper.offset().top;

        if (windowScrollTop > makeOverallTabsOffsetTop) {
            makeOverallTabs.addClass('fixed-tab');
        }

        else if (windowScrollTop < makeTabsContentWrapperOffsetTop + 44) {
            makeOverallTabs.removeClass('fixed-tab');
        }

        if (windowScrollTop > makeDetailsFooterOffsetTop - 44) { //44 height of top nav bar
            makeOverallTabs.removeClass('fixed-tab');
        }

        $('#makeTabsContentWrapper .bw-model-tabs-data').each(function () {
            var top = $(this).offset().top - makeOverallTabs.height(),
            bottom = top + $(this).outerHeight();
            if (windowScrollTop >= top && windowScrollTop <= bottom) {
                makeOverallTabs.find('span').removeClass('active');
                $('#makeTabsContentWrapper .bw-mode-tabs-data').removeClass('active');

                $(this).addClass('active');
                makeOverallTabs.find('span[data-href="#' + $(this).attr('id') + '"]').addClass('active');
            }
        });


    });

    $('.overall-specs-tabs-wrapper a[href^="#"]').click(function () {
        var target = $(this.hash);
        if (target.length == 0) target = $('a[name="' + this.hash.substr(1) + '"]');
        if (target.length == 0) target = $('html');
        $('html, body').animate({ scrollTop: target.offset().top - makeOverallTabs.height() }, 1000);
        return false;
    });





    /* */
    $("img.lazy").lazyload();

    $('#user-details-submit-btn').click(function () {
        var bikeName = $('#getLeadBike :selected').text();
        if (bikeName != 'Select a bike') {
            var cityName = GetGlobalCityArea();
            triggerGA('Make_Page', 'Lead_Submitted', bikeName + "_" + cityName);
        }
    });

    $(".upcoming-brand-bikes-container").on('jcarousel:visiblein', 'li', function (event, carousel) {
        $(this).find("img.lazy").trigger("imgLazyLoad");
    });

    $("#spnContent").append($("#discontinuedMore a:eq(0)").clone()).append(", ").append($("#discontinuedMore a:eq(1)").clone()).append(", ").append($("#discontinuedMore a:eq(2)").clone()).append(", ").append($("#discontinuedMore a:eq(3)").clone());
    $("#spnContent").append("... <a class='f-small' id='viewall' >View All</a>");


    $("#sortbike li").on("click", function () {
        sortListLI.removeClass("selected");
        $(this).addClass('selected');
        var sortByText = $(this).text();
        $(".sort-by-title").find(".sort-select-btn").html(sortByText);
        $.sortChangeUp(sortByDiv);
        var id = $(this).attr('id');
        switch (id) {
            case '0':
                dt = sortResults($(".listitems li"), 'data-price', true);
                pushGaTags('Price_Low_to_High');
                break;
            case '1':
                dt = sortResults($(".listitems li"), 'data-popularity', true);
                pushGaTags('Popular');
                break;
            case '2':
                dt = sortResults($(".listitems li"), 'data-price', false);
                pushGaTags('Price_High_to_Low');
                break;
            case '3':
                dt = sortResults($(".listitems li"), 'data-mileage', false);
                pushGaTags('Mileage_High_to_Low');
                break;
        }
        var htm = '';
        for (var i = 0, l = dt.length; i < l; i++) {
            htm += dt[i].outerHTML;
        }
        $(".listitems").html('');
        var ul = document.getElementById('listitems');
        ul.insertAdjacentHTML('beforeend', htm);
        applyTabsLazyLoad();
    });

    $("#viewall").click(function () {
        $("#discontinuedMore").show();
        $("#discontinuedLess").hide();
        var xContents = $('#discontinuedMore').contents();
        xContents[xContents.length - 1].nodeValue = "";
    });

    function sortResults(mydata, prop, asc) {
        return mydata.sort(function (a, b) {           
            if ($(a).attr(prop) == "0") { return 1; }
            if ($(b).attr(prop) == "0") { return -1;}

            if (asc) return (parseInt($(a).attr(prop)) > parseInt($(b).attr(prop))) ? 1 : ((parseInt($(a).attr(prop)) < parseInt($(b).attr(prop))) ? -1 : 0);
            else return (parseInt($(b).attr(prop)) > parseInt($(a).attr(prop))) ? 1 : ((parseInt($(b).attr(prop)) < parseInt($(a).attr(prop))) ? -1 : 0);
        });
    }

    var sortByDiv = $(".sort-div"),
        sortListDiv = $(".sort-selection-div"),
        sortCriteria = $('#sort'),
        sortByDiv = $(".sort-div"),
        sortListDiv = $(".sort-selection-div"),
        sortListLI = $(".sort-selection-div ul li");

    sortByDiv.click(function () {
        if (!sortByDiv.hasClass("open"))
            $.sortChangeDown(sortByDiv);
        else
            $.sortChangeUp(sortByDiv);
    });

    $.sortChangeDown = function (sortByDiv) {
        sortByDiv.addClass("open");
        sortListDiv.show();
    };

    $.sortChangeUp = function (sortByDiv) {
        sortByDiv.removeClass("open");
        sortListDiv.slideUp();
    };

    $(document).mouseup(function (e) {
        e.stopPropagation();
        if (!$(".sort-select-btn, .sort-div #upDownArrow").is(e.target)) {
            $.sortChangeUp($(".sort-div"));
        }
    });

    function applyTabsLazyLoad() {
        $("img.lazy").lazyload({
            failure_limit: 20
        });
    }

    function pushGaTags(label) {
        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Make_Page', 'act': 'Sort_Clicked', 'lab': label });
    }

    //800X600
    if ($(window).width() < 996 && $(window).width() > 790) {
        $("#sortByContainer .sort-by-text").removeClass("margin-left50");
    }

    $('a.read-more-bike-preview').click(function () {
        if (!$(this).hasClass('open')) {
            $('.preview-main-content').hide();
            $('.preview-more-content').show();
            $(this).text($(this).text() === 'Read more' ? 'Collapse' : 'Read more');
            $(this).addClass("open");
        }
        else if ($(this).hasClass('open')) {
            $('.preview-main-content').show();
            $('.preview-more-content').hide();
            $(this).text($(this).text() === 'Read more' ? 'Collapse' : 'Read more');
            $(this).removeClass('open');
        }
    });

    $('.comparison-type-carousel').jcarousel();

    $('.comparison-type-carousel .jcarousel-control-prev')
        .on('jcarouselcontrol:active', function () {
            $(this).removeClass('inactive');
        })
        .on('jcarouselcontrol:inactive', function () {
            $(this).addClass('inactive');
        })
        .jcarouselControl({
            target: '-=2'
        });

    $('.comparison-type-carousel .jcarousel-control-next')
        .on('jcarouselcontrol:active', function () {
            $(this).removeClass('inactive');
        })
        .on('jcarouselcontrol:inactive', function () {
            $(this).addClass('inactive');
        })
        .jcarouselControl({
            target: '+=2'
        });


    
    $(".navtab").click(function () {

        try {
            var scrollSectionId = $(this).data('href');
            $('html,body').animate({
                scrollTop: $(scrollSectionId).offset().top - 40
            },
          'slow');
           triggerGA('Make_Page', 'Floating_Navigation_Clicked', $(this).data("lab"));
        }
        catch (e) {
            console.log(e);
        }
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
                break;
        }
    });

    // subfooter divider

    $('.make-subfooter .read-more-button').on('click', function () {
        var subfooter = $(this).closest('.make-subfooter');
        subfooter.find('.content__left-col').css('height', 'auto').css('height', subfooter.height());
    });

    $("ul#listitems li.front").click(function () {
        var cookieValue = getCookie("_bwtest"), bikeName = $(this).attr("data-bike");
        bikeName = bikeName.replace(/\s+/, '_');
        triggerGA("MakePage", "Clicked_on_ModelCard", cookieValue + "_" + bikeName);
    });
    // For saving page in recently viewed models/make
    if (typeof pageData != "undefined")
        recentSearches.saveRecentSearches(pageData);
});
