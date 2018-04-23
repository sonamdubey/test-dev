var $sortDiv = $("#sort-by-div"),
    applyFilter = $('#btnApplyFilters'),
    CheckBoxFilter = $('.multiSelect .unchecked'),
    multiSelect = $('.multiSelect'),
    nobikediv = $('#nobike'),
    loading = $('#loading'),
    resetButton = $('#btnReset'),
    filterName;

var sortEnum = {};

var $window = $(window);
$.totalCount = "";
$.pageNo = "";
$.nextPageUrl = "";
$.lazyLoadingStatus = true;
$.effect = 'slide';
$.options = { direction: 'right' };
$.duration = 500;
$.so = '';
$.sc = '';
var ShowReviewCount = function (reviewCount) {
    var reviewText = '';
    if (parseInt(reviewCount) > 0)
        reviewText = reviewCount + ' Reviews';
    else
        reviewText = 'Not yet rated';

    return reviewText;
};

$(document).ready(function () {
    $('html, body').scrollTop(0);
    var completeQs = location.href.split('?')[1] || "";
    completeQs = $.removeFilterFromQS('pageno');

    if (completeQs && completeQs.indexOf("pageno=") != -1) {
        completeQs = completeQs.replace(/pageno=\d+/i, "");
    }

    $.selectedValueSortTab();

    newurl = window.location.protocol + "//" + window.location.host + window.location.pathname + '?' + completeQs;
    window.history.pushState({ path: newurl }, '', newurl);

    $.pushState(completeQs);

});//-- document ready ends here 

$.selectedValueSortTab = function () {
    var node = $('#sort-by-div');
    $.so = $.getFilterFromQS('so');
    $.sc = $.getFilterFromQS('sc');

    if ($.so.length > 0 && $.sc.length > 0) {
        node.find('div[sc="' + $.sc + '"] a').addClass('text-bold');

        if (node.find('a[data-title="sort"]').hasClass('price-sort')) {
            node.find('div[sc="' + $.sc + '"]').parent().attr('so', $.so);

            if ($.so.length > 0) {
                var sortedText = $('.price-sort').find('span');
                sortedText.css('display', 'inline-block');
                sortedText.text($.so == '1' ? ": High" : ": Low");
            }
        }
    }
    else
        node.find('div[sc=""] a').addClass('text-bold');
};

//Sort by div popup
$("#sort-btn").click(function () {
    $("#sort-by-div").slideToggle('fast');
    $("html, body").animate({ scrollTop: $("header").offset().top }, 0);
});

$('#sort-by-div a[data-title="sort"]').click(function () {
    $.removePageNoParam();
    $.removeKnockouts();
    $.scrollToTop();

    $.so = "0";
    var newurl = '';
    if ($(this).hasClass('price-sort')) {
        var sortOrder = $(this).attr('so');
        var sortedText = $('.price-sort').find('span');

        if (sortOrder == undefined || sortOrder == '0')
            $.so = '0';
        else
            $.so = '1';

        $(this).attr('so', $.so);

        if (sortOrder != undefined) {
            if (sortedText.text() === ': Low')
                $.so = '1';
            else
                $.so = '0';
        }

        if ($.so.length > 0) {
            sortedText.css('display', 'inline-block');
            sortedText.text($.so == '1' ? ": High" : ": Low");
        }
    }

    $('#sort-by-div a[data-title="sort"]').removeClass('text-bold');
    $(this).addClass('text-bold');
    $(this).parent().removeClass('text-bold');

    $.sc = $(this).parent().attr('sc');

    if ($.sc == "2") $.so = "1";

    if ($.sc != '1') {
        $('.price-sort').find('span').text('');
    }

    completeQS = $.removeFilterFromQS('so');
    newurl = window.location.protocol + "//" + window.location.host + window.location.pathname + '?' + completeQS;
    window.history.pushState({ path: newurl }, '', newurl);

    completeQS = $.removeFilterFromQS('sc');
    newurl = window.location.protocol + "//" + window.location.host + window.location.pathname + '?' + completeQS;
    window.history.pushState({ path: newurl }, '', newurl);

    if ($.sc.length > 0 && $.so.length > 0) {
        if (completeQS.length > 1)
            completeQS += "&so=" + $.so + "&sc=" + $.sc;
        else
            completeQS += "so=" + $.so + "&sc=" + $.sc;
    }

    $.pushStateUrl(completeQS);
    $.lazyLoadingStatus = false;
});

//filter div popup
var filterOffset = 0;
$("#filter-btn").click(function () {
    $("#filter-div").show($.effect, $.options, $.duration, function () {
        //$.selected Filters
    });
    $(".popup-btn-filters").show(); //Filter btn 
    $.selectFiltersPresentInQS();
});

//Back button press
$("#back-btn").click(function () {
    $("html,body").removeClass("lock-browser-scroll"); //bodyLock code
    $("#filter-div").hide($.effect, $.options, $.duration);
    $(".popup-btn-filters").hide(); //Filter btn 
    if (location.hash.substring(1) == 'back') {
        history.back();
    }

    isReset = false;
});

//Close button//
function CloseWindow(thiswindow) {
    var popupWindow = thiswindow.attributes.popupname.value;

    if (popupWindow == "filterpopup") {
        //        setFilters();
        $("#back-btn").click();
    }

    $('.popup-btn-submit').hide(); // for hiding the button submit when model selection pop up 
    $("#main-container").show();
    // ucAllMod();
}

var checkedLen, controlWidth, hidaWidth, remainSpace, multiselWidth;

$(window).resize(function () {
    $('.dropdown').each(function () {
        var $dropDown = $(this);
        controlWidth = $dropDown.find('.form-control').width();
        hidaWidth = $dropDown.find('.hida').width();
        remainSpace = controlWidth - hidaWidth - 10;
        multiselWidth = $dropDown.find('.multiSel').width();
        $dropDown.find('.multiSel').css('max-width', remainSpace + 'px');
    });
});

$window.scroll(function () {
    //if ($sortDiv.is(':visible')) {
    if ($(window).scrollTop() > 50) {
        $('#sort-by-div,header').addClass('fixed');
    } else {
        $('#sort-by-div,header').removeClass('fixed');
    }
    //}

    var winScroll = $window.scrollTop(),
        pageHeight = $(document).height(),
        windowHeight = $window.height(),
        footerHeight = $(".bg-footer").height();

    var position = pageHeight - (windowHeight + footerHeight + 200);
    if ($.lazyLoadingStatus) {
        if ($.nextPageUrl != '' && $.nextPageUrl != undefined) {
            if (winScroll >= position) {
                $.getNextPageData();
                $.lazyLoadingStatus = false;
            }
        }
    }
});

$(".dropdown .form-control").on('click', function () {
    var $ul = $(this).parent().find('ul');
    $(".dropdown ul").slideUp('fast');
    if ($ul.is(':visible')) {
        $ul.slideUp('fast');
    }
    else {
        $ul.slideDown('fast');
    }
});

$(document).bind('click', function (e) {
    var $clicked = $(e.target);
    if (!$clicked.parents().hasClass("dropdown")) $(".dropdown ul").hide();
});

CheckBoxFilter.on('click', function () {
    var $dropDown = $(this).closest('.dropdown');
    $(this).toggleClass('checked');
    var title = $(this).closest('.multiSelect').find('span').text(),
        title = $.trim($(this).text()) + ",";

    checkedLen = $dropDown.find('.multiSelect .unchecked.checked').length;
    controlWidth = $dropDown.find('.form-control').width();
    hidaWidth = $dropDown.find('.hida').width();
    remainSpace = controlWidth - hidaWidth - 10;
    multiselWidth;

    if ($(this).hasClass('checked')) {
        var html = '<span data-title="' + title + '">' + title + '</span>';

        $dropDown.find('.multiSel').append(html);
        $dropDown.find(".hida").addClass('hide');
        multiselWidth = $dropDown.find('.multiSel').width();
        if (checkedLen > 1 && multiselWidth > remainSpace) {
            $dropDown.find('.multiSel').css('max+-width', remainSpace + 'px');
        }
    }
    else {
        $dropDown.find('span[data-title="' + title + '"]').remove();
        if (checkedLen < 1) {
            $dropDown.find(".hida").removeClass('hide');
        }
        multiselWidth = $dropDown.find('.multiSel').width();
        if (multiselWidth < remainSpace) {
            $dropDown.find('.multiSel').css('max-width', 'none');
        }
    }
});


$('.checkOption').click(function () {
    $(this).siblings().removeClass('optionSelected');
    $(this).toggleClass('optionSelected');
});

$.scrollToTop = function () {
    $('body,html').animate({
        scrollTop: 0
    }, 800);
};

//back button function
$(function () {
    var backFlag;
    var hash = location.hash.substring(1);
    if (hash != '' && hash == 'back' || hash != '' && hash == 'sellPopup') {
        history.replaceState(null, null, location.pathname + location.search);
    }
    $(window).bind('hashchange', function (e) {
        pagiFlag = false;
        hash = location.hash.substring(1);
        var filterPopname = $('div.filterBackArrow[popupname="filterpopup"]:visible');
        var popname = $('div.filterBackArrow[popupname!="filterpopup"]:visible');
        if (hash == 'back') {
            backFlag = false;

            if (location.hash.substring(1) == 'back' && !($(document).find('#filter-div,#sort-by-div').is(':visible'))) {
                history.back();
            }
        }
        else if (backFlag == false && hash != 'back' && $(document).find('#filter-div,#sort-by-div').is(':visible')) {
            popname.trigger('click');
            filterPopname.trigger('click');
            backFlag = true;
        }
    });
});

var AppendCertificationStar = function (abStars) {
    var i, intVal, val, count = 0, certificationStar = "";
    val = abStars;
    intVal = Math.floor(val);
    for (i = 0; i < intVal ; i++) {
        certificationStar += '<img src="/image/ratings/1.png" alt="Rate">';
    }
    if (val > intVal) {
        certificationStar += '<img src="/image/ratings/half.png" alt="Rate">';
        count = (5 - intVal) - 1;
        for (i = 0; i < count; i++) {
            certificationStar += '<img src="/image/ratings/0.png" alt="Rate">';
        }
    }
    else {
        count = 5 - intVal;
        for (i = 0; i < count; i++) {
            certificationStar += '<img src="/image/ratings/0.png" alt="Rate">';
        }
    }
    return certificationStar;
}

$.hitAPI = function (searchUrl) {
    var bookingSearchURL = '/api/BikeBookingListing/?pageSize=6&cityId=' + selectedCityId + '&areaId=' + selectedAreaId + "&" + searchUrl;
    $.ajax({
        type: 'GET',
        url: bookingSearchURL,
        dataType: 'json',
        success: function (response) {
            if (response.totalCount > 0) {
                nobikediv.hide();
                $.totalCount = response.totalCount;
                $.pageNo = response.curPageNo;
                $.nextPageUrl = response.pageUrl.nextPageUrl;
                $.setCountTxt($.totalCount);
                if (!isNaN($.pageNo) && $.pageNo == 1) {
                    $.bindSearchResult(response);
                }
                else {
                    $.bindLazyListings(response);
                }
                $.lazyLoadingStatus = true;
                $('#hidePopup').click();
                loading.hide();
                $.pushGACode(searchUrl, $.totalCount);
            }
            else {
                errorNoBikes(searchUrl);
                $.pageNo = 1;
            }
        },
        error: function (error) {
            errorNoBikes(searchUrl);
            $.pageNo = 1;
        }
    });
};

function errorNoBikes(searchUrl) {
    $.totalCount = 0;
    var element = $('#divSearchResult');
    element.html('');
    ko.cleanNode(element);
    nobikediv.show();
    loading.hide();
    $('#hidePopup').click();
    $.setCountTxt($.totalCount);
    $.pushGACode(searchUrl, $.totalCount);
}


$.setCountTxt = function (totalCount) {
    var bikeCounttxt = '';
    if (totalCount == '0') bikeCounttxt = 'No bikes';
    else if (totalCount == '1') bikeCounttxt = totalCount + ' bike';
    else {
        bikeCounttxt = totalCount + ' bikes';
    }
    $('#bikecount').text(bikeCounttxt);
}

$.getSelectedQSFilterText = function () {
    var params = $.getAllParamsFromQS();
    count = 0;
    $('.bw-tabs').find('li').each(function () {
        $(this).removeClass('active');
    });
    $('.filter-select-title .default-text').each(function () {
        $(this).text($(this).prev().text());
    });

    for (var i = 0; i < params.length; i++) {
        if (params[i].length > 0) {
            var node = $('div[name=' + params[i] + ']');
            if (params[i] != 'pageno' && params[i] != 'so' && params[i] != 'sc' && params[i] != 'budget') {
                var values = $.getFilterFromQS(params[i]).replace(/ /g, '+').split('+'),
                    selText = '';

                for (var j = 0; j < values.length; j++) {
                    node.find('li[filterid=' + values[j] + ']').addClass('active');
                    selText += node.find('li[filterid=' + values[j] + ']').text() + ', ';
                }
                count++;
                node.find('ul').parent().prev(".filter-div").find('.filter-select-title .default-text').text(selText.substring(0, selText.length - 2));
            } else if (params[i] == 'budget') {
                var values = $.getFilterFromQS(params[i]).split('-');
                $.setMaxAmount(values[1]);
                $.setMinAmount(values[0]);
                count++;
            }
        }
    }
    $('.filter-counter').text(count);
};

$.getQSFromUrl = function () {
    var url = location.href.split('?')[1];
    if (url != undefined && url.length > 0)
        return url;
    else
        return "";
};

$.bindSearchResult = function (json) {
    var element;
    if ($.pageNo == 1)
        element = document.getElementById('divSearchResult');
    else
        element = document.getElementById('divSearchResult' + $.pageNo);

    ko.cleanNode(element);

    if (json.bikes.length > 0)
        ko.applyBindings(new SearchViewModel(json), element);
    else
        $('#NoBikeResults').show();
};

$.bindLazyListings = function (searchResult) {
    var koHtml = '<div id="divSearchResult' + $.pageNo + '" class="SRko" data-bind="template: { name: \'listingTemp\', foreach: bikes }">'
               + '</div>';
    if (($.pageNo - 1) > 1)
        $('#divSearchResult' + ($.pageNo - 1)).after(koHtml);
    else
        $('#divSearchResult').after(koHtml);

    ko.applyBindings(new SearchViewModel(searchResult), document.getElementById("divSearchResult" + $.pageNo));
};

var SearchViewModel = function (model) {
    ko.mapping.fromJS(model, {}, this);
};

$.getNextPageData = function () {
    $.pageNo = parseInt($.getFilterFromQS("pageno"));
    if (isNaN($.pageNo))
        $.pageNo = 2;
    else
        $.pageNo++;
    $.loadNextPage();
};

$.getFilterFromQS = function (name) {
    var hash = location.href.split('?')[1];
    var result = {
    };
    var propval, value;
    var isFound = false;
    if (hash != undefined) {
        var params = hash.split('&');
        for (var i = 0; i < params.length; i++) {
            var propval = params[i].split('=');
            filterName = propval[0];
            if (filterName == name) {
                value = propval[1];
                isFound = true;
                break;
            }
        }
    }
    if (isFound && value.length > 0) {
        if ((/\+/).test(value))
            return value.replace(/\+/g, " ");
        else
            return value;
    }
    else
        return "";
};

$.loadNextPage = function () {
    var completeQS = $.removeFilterFromQS("pageno");
    if (completeQS.length > 0)
        completeQS += "&pageno=" + $.pageNo;
    else
        completeQS = "pageno=" + $.pageNo;
    $.pushState(completeQS);
}

$.pushState = function (qs) {
    loading.show();
    if (qs && qs != "")
        history.pushState(null, null, '?' + qs);
    $.hitAPI(qs);
};

$.pushStateUrl = function (qs) {
    loading.show();
    var newurl = window.location.protocol + "//" + window.location.host + window.location.pathname + '?' + qs;
    window.history.pushState({ path: newurl }, '', newurl);
    $.hitAPI(qs);
};

$.removeFilterFromQS = function (name) {
    var url = location.href.split('?')[1];
    if (url != undefined && url.length > 0) {
        url = url.replace('?', '');
        var prefix = name + '=';
        var pars = url.split(/[&;]/g);
        for (var i = pars.length; i-- > 0;) {
            //if (pars[i].indexOf(prefix) > -1) {
            //if ((/=/).test(pars[i])) {
            if ((new RegExp(prefix, 'gi')).test(pars[i])) {
                pars.splice(i, 1);
            }
        }
        url = pars.join('&');
        return url;
    }
    else
        return "";
};

$.getAllParamsFromQS = function () {
    var completeQS = window.location.href.split('?')[1];
    var params = [];

    if (completeQS != undefined && completeQS.length > 1) {
        var tempParams = completeQS.substring(0, completeQS.length).split('&');

        if (tempParams.length > 0) {
            for (var i = 0; i < tempParams.length; i++) {
                params.push(tempParams[i].split('=')[0]);
            }
        }
    }
    return params;
};

$.removeKnockouts = function () {
    $(".SRko").each(function () {
        $(this).remove();
    });
};

$.removePageNoParam = function () {
    if ($.getFilterFromQS('pageno').length > 0) {
        var completeQS = $.removeFilterFromQS('pageno');
        $.pageNo = 1;

        var newurl = window.location.protocol + "//" + window.location.host + window.location.pathname + '?' + completeQS;
        window.history.pushState({ path: newurl }, '', newurl);
    }
};

$.appendToQS = function (temp, name, value) {
    if (temp.length > 0)
        temp += "&" + name + "=" + value;
    else
        temp = name + "=" + value;

    return temp;
};

$.AddToQS = function (name, value) {
    var temp = name + "=" + value;

    return temp;
};

$.fn.applyFilterOnButtonClick = function () {
    return $(this).click(function () {
        $.removePageNoParam();
        $.removeKnockouts();
        $.scrollToTop();
        var completeQS = '';
        var completeQSArr = new Array();
        completeQSArr.push($.applyToggelFilter());
        completeQSArr.push($.applyCheckBoxFilter());
        completeQSArr.push($.applySliderFilter($('#mSlider-range'), $('#mSlider-range').attr('name')));

        for (var i = 0; i < completeQSArr.length; i++) {
            if (completeQSArr[i].length > 1)
                completeQS += completeQSArr[i] + '&';
        }

        if (completeQS.length > 1)
            completeQS = completeQS.substring(0, completeQS.length - 1);

        if ($.sc.length > 0 && $.so.length > 0) {
            completeQS = $.appendToQS(completeQS, 'sc', $.sc);
            completeQS = $.appendToQS(completeQS, 'so', $.so);
        }

        $.removeCompleteQSFromUrl();
        $.pushStateUrl(completeQS);
    });
}

applyFilter.applyFilterOnButtonClick();

$.addParameterToString = function (name, value, completeQS) {
    if (value.length > 0) {
        if (completeQS.length > 0)
            completeQS += "&" + name + "=" + value;
        else
            completeQS = name + "=" + value;
    }
    return completeQS;
};

var trueValues = [30000, 40000, 50000, 60000, 70000, 80000, 90000, 100000, 150000, 200000, 250000, 300000, 350000, 500000, 750000, 1000000, 1250000, 1500000, 3000000, 6000000];
var values = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20];
$("#mSlider-range").slider({
    range: true,
    min: 1,
    max: 20,
    values: [1, 20],
    step: 1,
    slide: function (event, ui) {
        var includeLeft = event.keyCode != $.ui.keyCode.RIGHT;
        var includeRight = event.keyCode != $.ui.keyCode.LEFT;
        var value = $.findNearest(includeLeft, includeRight, ui.value);
        if (ui.value == ui.values[0]) {
            $(this).slider('values', 0, value);
        }
        else {
            $(this).slider('values', 1, value);
        }

        var budgetminValue = $.valueFormatter($.getRealValue(ui.values[0])) == '30000' ? 0 : $.valueFormatter($.getRealValue(ui.values[0]));
        var budgetmaxValue = $.valueFormatter($.getRealValue(ui.values[1]));

        if (ui.values[0] == 0 && ui.values[1] == 20) {
            $("#rangeAmount").html('<span class="bw-m-sprite rupee"></span> 0 -' + ' ' + '<span class="bw-m-sprite rupee"></span> Any value');
        } else {
            $("#rangeAmount").html('<span class="bw-m-sprite rupee"></span>' + ' ' + budgetminValue + ' ' + '-' + ' ' + '<span class="bw-m-sprite rupee"></span>' + ' ' + budgetmaxValue);
        }
    }
});

$.setSliderRangeQS = function (element, start, end) {
    element.slider("values", 0, start);
    if (end != '')
        element.slider("values", 1, end);
};

$.setSliderRangeQS($('#mSlider-range'), 0, 20);

$.applyToggelFilter = function () {
    var checked = 'optionSelected';
    var tempQS = '',
        completeQS = '';
    $('.checkOption').each(function () {
        if ($(this).hasClass(checked)) {
            var name = $(this).parent().attr('name'),
               value = $(this).attr('filterid');
            tempQS = $.AddToQS(name, value);
            completeQS += tempQS + '&';
        }
        else {
            tempQS = $.removeFilterFromQS(name);
        }
    });

    if (completeQS.length > 1)
        completeQS = completeQS.substring(0, completeQS.length - 1);
    return completeQS;
}

$.applyCheckBoxFilter = function () {
    var selected = 'checked',
        completeQS = '',
        tempArray = new Array();
    multiSelect.each(function () {
        var curCheckboxList = $(this),
            tempQS = '',
            name = curCheckboxList.attr('name'),
            value = '';

        curCheckboxList.find('ul li').each(function () {
            if ($(this).hasClass(selected)) {
                var filterId = $(this).attr('filterid');
                value += filterId + '+';
            }
        });
        if (value.length > 1) {
            value = value.substring(0, value.length - 1);
            tempQS = $.AddToQS(name, value);
            tempArray.push(tempQS);
        }
    });

    if (tempArray.length > 0) {
        for (var i = 0; i < tempArray.length; i++)
            completeQS += tempArray[i] + '&';
    }
    if (completeQS.length > 1)
        completeQS = completeQS.substring(0, completeQS.length - 1);
    return completeQS;
};

$.applySliderFilter = function (element, name) {
    var minValue = $.getRealValue(element.slider('values', 0)) == '30000' ? 0 : $.getRealValue(element.slider('values', 0)),
        maxValue = $.getRealValue(element.slider('values', 1)),
        completeQS = '';
    return name + "=" + minValue + '-' + maxValue;
};

$.findNearest = function (includeLeft, includeRight, value) {
    var nearest = null;
    var diff = null;
    for (var i = 0; i < values.length; i++) {
        if ((includeLeft && values[i] <= value) || (includeRight && values[i] >= value)) {
            var newDiff = Math.abs(value - values[i]);
            if (diff == null || newDiff < diff) {
                nearest = values[i];
                diff = newDiff;
            }
        }
    }
    return nearest;
}

$.getSliderValue = function (budgetValue) {
    for (var i = 0; i < trueValues.length; i++)
        if (trueValues[i] == budgetValue)
            return values[i];
}

$.getRealValue = function (sliderValue) {
    for (var i = 0; i < values.length; i++) {
        if (values[i] >= sliderValue) {
            return trueValues[i];
        }
    }
    return 0;
}

$.removeCompleteQSFromUrl = function () {
    history.pushState(null, null, '');
};

$.fn.resetAll = function () {
    return $(this).click(function () {
        $.resetFilterUI();
        $.removePageNoParam();
        $.removeKnockouts();
        $.scrollToTop();
        $.pageNo = 1;
        $.pushStateUrl('');
        $.lazyLoadingStatus = false;
    });
};

resetButton.resetAll();

$.resetFilterUI = function () {
    var params = ['bike', 'ridestyle', 'budget'];
    for (var i = 0; i < params.length; i++) {
        if (params[i].length > 0) {
            var node = $('div[name=' + params[i] + ']');
            if (params[i] == 'bike' || params[i] == 'ridestyle') {
                node.prev().find('.hida').removeClass('hide');
                node.prev().find('.multiSel').html('');
                node.find('li').each(function () {
                    $(this).removeClass('checked');
                });
            } else if (params[i] == 'budget') {
                $.setSliderRangeQS($('#mSlider-range'), 1, 20);
                $("#rangeAmount").html('<span class="bw-m-sprite rupee"></span> 0 -' + ' ' + '<span class="bw-m-sprite rupee"></span> Any value');
            }
        }
    }
};

$.selectFiltersPresentInQS = function () {
    $.resetFilterUI();
    var params = $.getAllParamsFromQS();
    for (var i = 0; i < params.length; i++) {
        if (params[i].length > 0) {
            var node = $('div[name=' + params[i] + ']');
            if (params[i] == 'bike' || params[i] == 'ridestyle') {
                var values = $.getFilterFromQS(params[i]).replace(/ /g, '+').split('+');
                var html = '';
                for (var j = 0; j < values.length; j++) {
                    node.find('li[filterid=' + values[j] + ']').addClass('checked');
                    var title = node.find('li[filterid=' + values[j] + ']').text() + ',';
                    html += '<span data-title="' + title + '">' + title + '</span>';
                }

                node.prev().find('.hida').addClass('hide');
                node.prev().find('.multiSel').html(html);

            } else if (params[i] == 'budget') {
                var values = $.getFilterFromQS(params[i]).split('-');
                values[0] = (values[0] == '0' ? '30000' : values[0]);
                values[1] = values[1] == '' ? '6000000' : values[1];
                var minValue = $.getSliderValue(values[0]), maxValue = $.getSliderValue(values[1])

                $.setSliderRangeQS($('#mSlider-range'), minValue, maxValue);

                var budgetminValue = $.valueFormatter(values[0]);
                var budgetmaxValue = $.valueFormatter(values[1]);

                if (values[0] == 1 && values[1] == 20) {
                    $("#rangeAmount").html('<span class="bw-m-sprite rupee"></span> 0 -' + ' ' + '<span class="bw-m-sprite rupee"></span> Any value');
                } else {
                    $("#rangeAmount").html('<span class="bw-m-sprite rupee"></span>' + ' ' + budgetminValue + ' ' + '-' + ' ' + '<span class="bw-m-sprite rupee"></span>' + ' ' + budgetmaxValue);
                }
            } else if (params[i] == 'sc') {
                $.sc = $.getFilterFromQS('sc');
                $.so = $.getFilterFromQS('so');
            }
        }
    }
}

$.valueFormatter = function (num) {
    if (num >= 100000) {
        return (num / 100000).toFixed(1).replace(/\.0$/, '') + 'L';
    }
    if (num >= 1000) {
        return (num / 1000).toFixed(1).replace(/\.0$/, '') + 'K';
    }
    return num;
}

$.pushGACode = function (qs, noOfRecords) {
    var params = $.getAllParamsFromQS();
    for (var i = 0; i < params.length; i++) {
        if (params[i].length > 0) {
            if (params[i] != "pageno" && params[i] != "so" && params[i] != "sc" && params[i] != "budget") {
                $.pushGTACode(noOfRecords, params[i]);
            } else if (params[i] == "sc") {
                var sc = $.getFilterFromQS('sc'), so = $.getFilterFromQS('so');

                filterName = "";
                switch (sc) {
                    case '0':
                        filterName = "Popular";
                        break;
                    case '1':
                        filterName = so == '0' ? "Price :Low to High" : "Price :High to Low";
                        break;
                }
                $.pushGTACode(noOfRecords, filterName);
            } else if (params[i] == "budget") {
                var budget = $.getFilterFromQS('budget').split('-');
                if (!(budget[0] == '0' && budget[1] == '6000000'))
                    $.pushGTACode(noOfRecords, filterName);
            }

        }
    }
};

$.pushGTACode = function (noOfRecords, filterName) {
    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'BookingListing_Page', 'act': 'Filter_Select_' + noOfRecords, 'lab': filterName });
};

$.ModelClickGaTrack = function (modelName, modelUrl) {
    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'BookingListing_Page', 'act': 'Model_Click', 'lab': modelName });
    location.href = modelUrl;
};

$.PricePopUpClickGA = function (makeName) {
    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'BookingListing_Page', 'act': 'Get_On_Road_Price_Click', 'lab': makeName });
};

(function ($, ko) {
    'use strict';
    // TODO: Hook into image load event before loading others...
    function KoLazyLoad() {
        var self = this;

        var updatebit = ko.observable(true).extend({ throttle: 50 });

        var handlers = {
            img: updateImage
        };

        function flagForLoadCheck() {
            updatebit(!updatebit());
        }

        $(window).on('scroll', flagForLoadCheck);
        $(window).on('resize', flagForLoadCheck);
        $(window).on('load', flagForLoadCheck);

        function isInViewport(element) {
            var rect = element.getBoundingClientRect();
            return rect.bottom > 0 && rect.right > 0 &&
              rect.top < (window.innerHeight || document.documentElement.clientHeight) &&
              rect.left < (window.innerWidth || document.documentElement.clientWidth);
        }

        function updateImage(element, valueAccessor, allBindings, viewModel, bindingContext) {
            var value = ko.unwrap(valueAccessor());
            if (isInViewport(element)) {
                element.src = value;
                $(element).data('kolazy', true);
            }
        }

        function init(element, valueAccessor, allBindings, viewModel, bindingContext) {
            var initArgs = arguments;
            updatebit.subscribe(function () {
                update.apply(self, initArgs);
            });
        }

        function update(element, valueAccessor, allBindings, viewModel, bindingContext) {
            var $element = $(element);

            if ($element.is(':hidden') || $element.css('visibility') == 'hidden' || $element.data('kolazy')) {
                return;
            }

            var handlerName = element.tagName.toLowerCase();
            if (handlers.hasOwnProperty(handlerName)) {
                return handlers[handlerName].apply(this, arguments);
            } else {
                throw new Error('No lazy handler defined for "' + handlerName + '"');
            }
        }

        return {
            handlers: handlers,
            init: init,
            update: update
        }
    }

    ko.bindingHandlers.lazyload = new KoLazyLoad();

})(jQuery, ko);

/* booking listing js */
$("#searchBikeList").on("click", "span.view-offers-target", function () {
    var offersDiv = $(this).parent().next("div#offersPopup");
    offersPopupOpen(offersDiv);
    appendHash("offersPopup");
});

$("#searchBikeList").on("click", "div.offers-popup-close-btn", function () {
    var offersDiv = $(this).parent("div#offersPopup");
    offersPopupClose(offersDiv);
    window.history.back();
});

var offersPopupOpen = function (offersDiv) {
    $('#sort-by-div').hide();
    offersDiv.show();
};

var offersPopupClose = function (offersDiv) {
    offersDiv.hide();
};

$(".change-city-area-target").on("click", function () {
    listingLocationPopupOpen();
    appendHash("listingPopup");
});

$(".location-popup-close-btn").on("click", function () {
    listingLocationPopup.hide();
    window.history.back();
});

var listingLocationPopup = $("#listingLocationPopup");

var listingLocationPopupOpen = function () {
    listingLocationPopup.show();
};

/* city area listing popup */
var popupHeading = $("#listingPopupHeading")
popupContent = $("#listingPopupContent");

$("#listingCitySelection").on("click", function () {
    $("#listingPopupContent .bw-city-popup-box").show().siblings("div.bw-area-popup-box").hide();
    $("#listingPopupContent").addClass("open").animate({ 'left': '0px' }, 500);
    $(".user-input-box").animate({ 'left': '0px' }, 500);

});

$("#listingAreaSelection").on("click", function () {
    $("#listingPopupContent .bw-city-popup-box").hide().siblings("div.bw-area-popup-box").show();
    $("#listingPopupContent").addClass("open").animate({ 'left': '0px' }, 500);
    $(".user-input-box").animate({ 'left': '0px' }, 500);
});

$(".bwm-city-area-popup-wrapper .back-arrow-box").on("click", function () {
    $("#listingPopupContent").removeClass("open").stop().animate({ 'left': '100%' }, 500);
    $(".user-input-box").stop().animate({ 'left': '100%' }, 500);
});

var locationFilter = function (filterContent) {
    var inputText = $(filterContent).val();
    inputText = inputText.toLowerCase();
    var inputTextLength = inputText.length;
    if (inputText != "") {
        $(filterContent).parent("div.user-input-box").siblings("ul").find("li").each(function () {
            var locationName = $(this).text().toLowerCase().trim();
            if (/\s/.test(locationName))
                var splitlocationName = locationName.split(" ")[1];
            else
                splitlocationName = "";

            if ((inputText == locationName.substring(0, inputTextLength)) || inputText == splitlocationName.substring(0, inputTextLength))
                $(this).show();
            else
                $(this).hide();
        });
    }
    else {
        $(this).parent("div.user-input-box").siblings("ul").find("li").each(function () {
            $(this).show();
        });
    }
};

$("#listingPopupCityInput, #listingPopupAreaInput").on("keyup", function () {
    locationFilter($(this));
});
var listingLocationPopupClose = function () {
    listingLocationPopup.hide();
};

$("#listingPopupCityList").on("click", "li", function () {
    var userselection = getSelectedLocationLI($(this));
    $("#listingCitySelection .selected-city").text(userselection.trim());
    //$("#listingAreaSelection").empty();

    $("#listingAreaSelection .selected-area").text("Select Area");
    $('#listingPopupAreaInput').val('');
    onChangeCity($(this));
});

$("#listingPopupAreaList").on("click", "li", function () {
    var userselection = getSelectedLocationLI($(this));
    $("#listingAreaSelection .selected-area").text(userselection.trim());
    selectedAreaId = $(this).attr('areaid');
    hideError($("#listingAreaSelection .selected-area"));
});

var getSelectedLocationLI = function (selection) {
    var selectedLI = selection.text();
    var userInputBox = selection.parent().siblings("div.user-input-box");
    userInputBox.find("input[type=text]").val(selectedLI);
    $(".bwm-city-area-popup-wrapper .back-arrow-box").click();
    return selectedLI;
};

$("#btnBookingListingPopup").on("click", function () {
    var areaName = $('#listingAreaSelection .selected-area'),
        cityName = $('#listingCitySelection .selected-city');
    if (cityName.text().trim() == "Select City") {
        return;
    }
    if (areaName.text().trim() == "Select Area") {
        setError($('.selected-area'), "Please select area !");
        return;
    }

    listingLocationPopupClose();
    var CookieValue = selectedCityId + "_" + cityName.text().trim() + '_' + selectedAreaId + "_" + areaName.text().trim();
    SetCookieInDays("location", CookieValue, 365);
    window.location.href = "/m/bikebooking/bookinglisting.aspx";
    //    $('#Userlocation').text(cityName.text().trim() + ', ' + areaName.text().trim());
    //$.hitAPI("");
});

ko.bindingHandlers.CurrencyText = {
    update: function (element, valueAccessor) {
        var amount = valueAccessor();
        var formattedAmount = ko.unwrap(amount) !== null ? formatPrice(amount) : 0;
        $(element).text(formattedAmount);
    }
};

function formatPrice(price) {
    price = price.toString();
    var lastThree = price.substring(price.length - 3);
    var otherNumbers = price.substring(0, price.length - 3);
    if (otherNumbers != '')
        lastThree = ',' + lastThree;
    var price = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
    return price;
}

function loadTerms(offerId) {
    $('#offersPopup').hide();
    LoadTerms(offerId);
}
function LoadTerms(offerId) {
    $("div#termsPopUpContainer").show();
    $(".blackOut-window").show();
    if (offerId != 0 && offerId != null) {
        $('#terms').empty();
        $.ajax({
            type: "GET",
            url: "/api/Terms/?offerId=" + offerId,
            dataType: 'json',
            success: function (response) {
                if (response != null)
                    $('#terms').html(response);
            },
            error: function (request, status, error) {
                $("div#termsPopUpContainer").hide();
                $(".blackOut-window").hide();
            }
        });
    }
    else {
        $("#terms").load("/statichtml/tnc.html");
    }
}
function registerPQ(myData) {
    var obj = {
        'CityId': selectedCityId,
        'AreaId': selectedAreaId,
        'ModelId': myData.modelEntity.modelId(),
        'ClientIP': clientIP,
        'SourceType': '2',
        'VersionId': myData.versionEntity.versionId(),
        'pQLeadId': pqLeadId,
        'deviceId': getCookie('BWC'),
        'dealerId': myData.dealerId()
    };
    $.ajax({
        type: 'POST',
        url: "/api/RegisterPQ/",
        data: obj,
        dataType: 'json',
        beforeSend: function (xhr) {
            xhr.setRequestHeader('utma', getCookie('__utma'));
            xhr.setRequestHeader('utmz', getCookie('_bwutmz'));
        },
        success: function (json) {
            var jsonObj = json;
            cookieValue = "CityId=" + selectedCityId + "&AreaId=" + selectedAreaId + "&PQId=" + jsonObj.quoteId + "&VersionId=" + myData.versionEntity.versionId() + "&DealerId=" + myData.dealerId();
            //SetCookie("_MPQ", cookieValue);
            if (jsonObj != undefined && jsonObj.quoteId > 0 && jsonObj.dealerId > 0) {
                // gtmCodeAppenderWidget(pageId, 'Dealer_PriceQuote_Success_Submit', gaLabel);

                window.location = "/m/pricequote/bookingsummary_new.aspx?MPQ=" + Base64.encode(cookieValue);
            }
            else {
                // gtmCodeAppenderWidget(pageId, 'BW_PriceQuote_Error_Submit', gaLabel);
                //$("#errMsgOnRoad").text("Oops. We do not seem to have pricing for given details.").show();
            }
        },
        error: function (e) {
            //gtmCodeAppenderWidget(pageId, 'BW_PriceQuote_Error_Submit', gaLabel);
            // $("#errMsg").text("Oops. Some error occured. Please try again.").show();
        }
    });
}

function onChangeCity(objCity) {
    selectedCityId = !isNaN(parseInt(objCity.attr('cityid'))) ? parseInt(objCity.attr('cityid')) : 0;
    $('#listingPopupAreaList').empty();
    if (selectedCityId > 0) {
        if (!checkCacheCityAreas(selectedCityId)) {
            $('#listingAreaSelection .selected-area').text('Loading Areas ....');
            $.ajax({
                type: "GET",
                url: "/api/BBAreaList/?cityId=" + selectedCityId,
                dataType: 'json',
                beforeSend: function () {

                },
                success: function (data) {
                    lscache.set(key + selectedCityId.toString(), data.areas, 30);
                    setOptions(data.areas);
                    $('#listingAreaSelection .selected-area').text('Select Area');
                },
                complete: function (xhr) {
                    if (xhr.status == 404 || xhr.status == 204) {
                        lscache.set(key + selectedCityId.toString(), null, 30);
                        setOptions(null);
                    }
                }
            });
        }
        else {
            data = lscache.get(key + selectedCityId.toString());
            setOptions(data);
        }

    }

}

var setError = function (element, msg) {
    element.addClass("border-red").siblings("span.errorIcon, div.errorText").show();
    element.siblings("div.errorText").text(msg);
};

var hideError = function (element) {
    element.removeClass("border-red").siblings("span.errorIcon, div.errorText").hide();
};
