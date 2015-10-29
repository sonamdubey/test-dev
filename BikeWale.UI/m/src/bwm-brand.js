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
            }
            else {
                $.so = '0';
                dt = sortResults(list, 'prc', true);
            }
        }
        else {
            dt = sortResults(list, 'prc', true);
        }
        if ($.so.length > 0) {
            sortedText.css('display', 'inline-block');
            sortedText.text($.so == '1' ? ": High" : ": Low");
        }
    }
    else {
        $.sc = $(this).parent().attr('sc');
        if ($.sc == '')
            dt = sortResults(list, 'ind', true);
        else
            dt = sortResults(list, 'mlg', false);
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
    }, 800);
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

$(window).scroll(function () {
    if ($(window).scrollTop() > 50)
        $('#sort-by-div,header').addClass('fixed');
    else
        $('#sort-by-div,header').removeClass('fixed');
});