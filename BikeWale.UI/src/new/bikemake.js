var dt = '';

$("#sortbike li").on("click", function () {
    sortListLI.removeClass("selected");
    $(this).addClass('selected');
    var sortByText = $(this).text();
    $(".sort-by-title").find(".sort-select-btn").html(sortByText);
    $.sortChangeUp(sortByDiv);
    var id = $(this).attr('id');
    switch (id) {
        case '0':
            dt = sortResults($(".listitems li"), 'prc', true);
            pushGaTags('Price_Low_to_High');
            break;
        case '1':
             dt = sortResults($(".listitems li"), 'pop', true);
            pushGaTags('Popular');
            break;
        case '2':
            dt = sortResults($(".listitems li"), 'prc', false);
            pushGaTags('Price_High_to_Low');
            break;
        case '3':
            dt = sortResults($(".listitems li"), 'mlg', false);
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


function sortResults(mydata, prop, asc) {
    return mydata.sort(function (a, b) {
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

$(document).ready(function () {
    var makeOverallTabs = $('#makeOverallTabs'),
        overallMakeDetailsFooter = $('#overallMakeDetailsFooter'),
        makeTabsContentWrapper = $('#makeTabsContentWrapper');

    makeOverallTabs.find('.overall-specs-tabs-wrapper a').first().addClass('active');

    $(window).scroll(function () {
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


    });

    $('.overall-specs-tabs-wrapper a[href^="#"]').click(function () {
        var target = $(this.hash);
        if (target.length == 0) target = $('a[name="' + this.hash.substr(1) + '"]');
        if (target.length == 0) target = $('html');
        $('html, body').animate({ scrollTop: target.offset().top - makeOverallTabs.height() }, 1000);
        return false;
    });

});