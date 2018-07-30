var appendText = $(".filter-select-title"),
    currentList = $(".filter-selection-div"),
    liList = $(".filter-selection-div ul li"),
    liToggelFilter = $(".bw-tabs li"),
    defaultText = $(".default-text"),
    sortCriteria = $('#sort'),
    sortByDiv = $(".sort-div"),
    sortListDiv = $(".sort-selection-div"),
    sortListLI = $(".sort-selection-div ul li");
var arr, a, p, minDataValue, maxDataValue;
var count = 0, counter = 0;
var pqSourceId = '39';
var searchUrl = "";
arr = [
	{ amount: "0", value: 0 },
	{ amount: "30K", value: 30000 },
	{ amount: "40K", value: 40000 },
	{ amount: "50K", value: 50000 },
	{ amount: "60K", value: 60000 },
	{ amount: "70K", value: 70000 },
	{ amount: "80K", value: 80000 },
	{ amount: "90K", value: 90000 },
	{ amount: "1L", value: 100000 },
	{ amount: "1.5L", value: 150000 },
	{ amount: "2L", value: 200000 },
	{ amount: "2.5L", value: 250000 },
	{ amount: "3L", value: 300000 },
	{ amount: "3.5L", value: 350000 },
	{ amount: "5L", value: 500000 },
	{ amount: "7.5L", value: 750000 },
	{ amount: "10L", value: 1000000 },
	{ amount: "12.5L", value: 1250000 },
	{ amount: "15L", value: 1500000 },
	{ amount: "30L", value: 3000000 },
	{ amount: "60L", value: 6000000 }
];
$.pageNo = "";
$.so = "";
$.sc = "";
$.nextPageUrl = "";
$.totalCount;
$.lazyLoadingStatus = true;
var $window = $(window),
    $menu = $('#filter-container'),
    menuTop = $menu.offset().top;


ko.bindingHandlers.CurrencyText = {
    update: function (element, valueAccessor) {
        var amount = valueAccessor();
        var formattedAmount = ko.unwrap(amount) !== null ? formatPrice(amount) : 0;
        $(element).text(formattedAmount);
    }
};

var SearchViewModel = function (model) {
    ko.mapping.fromJS(model, {}, this);
};

$(document).ready(function () {
    
    var completeQs = window.location.hash.replace('#', '');
    completeQs = $.removeFilterFromQS('pageno');
    $.pushState(completeQs,"");
});

$(".filter-div").on("click", function () {
    var allDiv = $(".filter-div");
    var clickedDiv = $(this);
    if (!clickedDiv.hasClass("open")) {
        stateChangeDown(allDiv, clickedDiv);
        //$(".more-filters-container").slideUp();
        $.sortChangeUp(sortByDiv);
    }
    else {

        stateChangeUp(allDiv, clickedDiv);
    }
});

var stateChangeDown = function (allDiv, clickedDiv) {
    allDiv.removeClass("open");
    allDiv.next(".filter-selection-div").slideUp();
    clickedDiv.addClass("open");
    clickedDiv.next(".filter-selection-div").show().css({ "overflow": "inherit" });
    clickedDiv.next(".filter-selection-div").addClass("open");
};

var stateChangeUp = function (allDiv, clickedDiv) {
    allDiv.removeClass("open");
    allDiv.next(".filter-selection-div").slideUp();
    clickedDiv.removeClass("open");
    clickedDiv.next(".filter-selection-div").slideUp();
    clickedDiv.next(".filter-selection-div").removeClass("open");
};

var defaultTextBack = function (a, textDiv) {
    if (!textDiv.children("span").hasClass("selected")) {
        a.find(defaultText).show();
        currentList.slideUp();
        $(".filter-div").removeClass("open");
    }
}

var resetBWTabs = function () {
    $(".bw-tabs li").removeClass("active");
};

$(".more-filters-btn").click(function () {
    if (!$(this).hasClass("open")) {
        $(this).addClass("open");
        $(".more-filters-container").show().css({ "overflow": "inherit" });
        var a = $(".filter-div");
        a.removeClass("open");
        a.next(".filter-selection-div").slideUp();
        a.next(".filter-selection-div").removeClass("open");
        moreLessTextChange($(this));
    }
    else {
        $(this).removeClass("open");
        $(".more-filters-container").slideUp();
        moreLessTextChange($(this));
    }
});

var moreLessTextChange = function (p) {
    var morelessFilter = $("#more-less-filter-text");
    var q = p.find(morelessFilter);
    q.text(q.text() === "More" ? "Less" : "More");
};

$(".filter-done-btn").click(function () {
    $(".more-filters-container").slideUp();
    stateChangeUp($('.filter-div'), $('.filter-div'));
    var a = $(".more-filters-btn");
    moreLessTextChange(a);
    $(".more-filters-btn").removeClass("open");
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

var ShowReviewCount = function (reviewCount) {
    var reviewText='';
    if (parseInt(reviewCount) > 0)
        reviewText = reviewCount + ' Reviews';
    else
        reviewText = 'Not yet rated';

    return reviewText;
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
var d;
$.hitAPI = function (searchUrl, filterName) {
    var bookingSearchURL = '/api/BikeBookingListing/?pageSize=6&cityId=' + selectedCityId + '&areaId=' + selectedAreaId + "&" + searchUrl;
    $.ajax({
        type: 'GET',
        url: bookingSearchURL ,
        dataType: 'json',
        success: function (response) {             
            if (response.totalCount > 0)
            {
                $.totalCount = response.totalCount;
                $.pageNo = response.curPageNo;
                if (response.pageUrl != null)
                    $.nextPageUrl = response.pageUrl.nextPageUrl;
                else $.nextPageUrl = "";

                $('#bikecount').text(($.totalCount > 1) ? $.totalCount + ' Bikes' : $.totalCount + ' Bike');

                if (!isNaN($.pageNo) && $.pageNo == 1) {
                    $.bindSearchResult(response);
                }
                else {
                    $.bindLazyListings(response);
                }
                $.selectFiltersPresentInQS();
                $.getSelectedQSFilterText();

                $.lazyLoadingStatus = true;
                $('#NoBikeResults').hide();
                $('#loading').hide();
            }
            else {
                errorNoBikes();
            }
           
        },
        error: function (error) {
            errorNoBikes();
        }
    });
};

function errorNoBikes()
{
    $.totalCount = 0;
    var element = $('#divSearchResult');
    element.html('');
    ko.cleanNode(element);
    $('#loading').hide();
    $('#NoBikeResults').show();
    $('#bikecount').text('No bikes found');
    $.selectFiltersPresentInQS();
    $.getSelectedQSFilterText();
}


$.bindLazyListings = function (searchResult) {
    var koHtml = '<div class="grid-12 alpha omega">'
                        + '<ul id="divSearchResult' + $.pageNo + '" class="SRko" data-bind="template: { name: \'listingTemp\', foreach: bikes }">'
                        + '</ul>'
                    + '</div>';
    if (($.pageNo - 1) > 1)
        $('#divSearchResult' + ($.pageNo - 1)).parent().after(koHtml);
    else
        $('#divSearchResult').parent().after(koHtml);

    ko.applyBindings(new SearchViewModel(searchResult), document.getElementById("divSearchResult" + $.pageNo));
};

$.removeKnockouts = function () {
    $(".SRko").each(function () {
        $(this).parent().remove();
    });
};

$.loadNextPage = function () {
    var completeQS = $.removeFilterFromQS("pageno");
    if (completeQS.length > 0)
        completeQS += "&pageno=" + $.pageNo;
    else
        completeQS = "pageno=" + $.pageNo;
    $.pushState(completeQS, "");
};

$.removeFilterFromQS = function (name) {
    var url = window.location.hash.replace('#', '');
    if (url.length > 0) {
        var prefix = name + '=';
        var pars = url.split(/[&;]/g);
        for (var i = pars.length; i-- > 0;) {
            if (pars[i].indexOf(prefix) > -1) {
                pars.splice(i, 1);
            }
        }
        url = pars.join('&');
        return url;
    }
    else
        return "";
};

$.pushState = function (qs, filterName) {
    $('#NoBikeResults').hide();
    $('#loading').show();
    window.location.hash = qs;
    $.hitAPI(qs, filterName);
};

$.getFilterFromQS = function (name) {
    var hash = window.location.hash.replace('#', '');
    var params = hash.split('&');
    var result = {};
    var propval, filterName, value;
    var isFound = false;
    for (var i = 0; i < params.length; i++) {
        var propval = params[i].split('=');
        filterName = propval[0];
        if (filterName == name) {
            value = propval[1];
            isFound = true;
            break;
        }
    }
    if (isFound && value.length > 0) {
        if (value.indexOf('+') > 0)
            return value.replace(/\+/g, " ");
        else
            return value;
    }
    else
        return "";
};

$window.scroll(function () {
    $menu.toggleClass('stick', $window.scrollTop() > menuTop);

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

$.getAllParamsFromQS = function () {
    var completeQS = window.location.hash.replace('#', '');
    var tempParams = completeQS.substring(0, completeQS.length).split('&');
    var params = [];
    if (tempParams.length > 0) {
        for (var i = 0; i < tempParams.length; i++) {
            params.push(tempParams[i].split('=')[0]);
        }
    }
    return params;
};

$.addFilterInQS = function (name, value) {
    var hash = window.location.hash.replace('#', '');
    if (hash.length > 0)
        return hash.substring(0, hash.length) + "&" + name + "=" + value;
    else
        return name + "=" + value;
}

$.updateCheckBoxFilterInQS = function (name, value, curValue, toAdd) {
    var completeQS = '';
    if (toAdd == true)
        completeQS = $.addValueToCheckBoxFilterInQS(name, value, curValue);
    else
        completeQS = $.removeValueFromCheckBoxInQS(name, value, curValue);

    return completeQS;
};

$.addValueToCheckBoxFilterInQS = function (name, value, curValue) {
    var completeQS = $.removeFilterFromQS(name);
    var values = curValue.split('+');
    var temp = name + "=" + value;
    for (var i = 0; i < values.length; i++)
        temp = temp + "+" + values[i];
    if (completeQS.length > 0)
        completeQS = completeQS + "&" + temp;
    else
        completeQS = temp;
    return completeQS;
};

$.removeValueFromCheckBoxInQS = function (name, value, curValue) {
    var completeQS = $.removeFilterFromQS(name);
    var values = curValue.split('+');
    var temp = '';
    for (var i = 0; i < values.length; i++) {
        if (values[i] != value) {
            if (temp.length > 0)
                temp = temp + "+" + values[i];
            else
                temp = values[i];
        }
    }
    var param = '';
    if (temp.length > 0)
        param = name + "=" + temp;

    if (completeQS.length > 0 && param.length > 0)
        completeQS = completeQS + "&" + param;
    else if (param.length > 0)
        completeQS = param;

    return completeQS;
};

$.applyCheckBoxFilter = function (name, value, node) {
    $.removePageNoParam();
    $.removeKnockouts();
    var checked = 'active';
    var completeQS = '';
    var curValue = $.getFilterFromQS(name).replace(/ /g, '+');
    if (node.hasClass(checked)) {
        node.removeClass(checked);
        completeQS = $.updateCheckBoxFilterInQS(name, value, curValue, false);
    } else {
        node.addClass(checked);
        if (curValue.length > 0) {
            completeQS = $.updateCheckBoxFilterInQS(name, value, curValue, true);
        }
        else {
            completeQS = $.addFilterInQS(name, value);
        }
    }
    $.pushState(completeQS,name);
};

$.applyToggelFilter = function (name, value, node) {
    $.removePageNoParam();
    $.removeKnockouts();
    var checked = 'active';
    var tempQS = '';
    var curValue = $.getFilterFromQS(name).replace(/ /g, '+');

    if (!node.find('li[filterid=' + value + ']').hasClass(checked)) {
        node.removeClass(checked);
        tempQS = $.removeFilterFromQS(name);
        tempQS = $.appendToQS(tempQS, name, value);
    }
    else {
        tempQS = $.removeFilterFromQS(name);
    }
    
    $.pushState(tempQS,name);
};

$.removePageNoParam = function () {
    if ($.getFilterFromQS('pageno').length > 0) {
        var completeQS = $.removeFilterFromQS('pageno');
        $.pageNo = 1;
        window.location.hash = completeQS;
    }
};

$.selectFiltersPresentInQS = function () {
    var params = $.getAllParamsFromQS();
    for (var i = 0; i < params.length; i++) {
        if (params[i].length > 0) {
            var node = $('div[name=' + params[i] + ']');
            if (params[i] != 'pageno' && params[i] != 'so' && params[i] != 'sc') {
                var values = $.getFilterFromQS(params[i]).replace(/ /g, '+').split('+');
                for (var j = 0; j < values.length; j++) {
                    node.find('li[filterid=' + values[j] + ']').addClass('active');
                }
            }

            $.so = $.getFilterFromQS('so');
            $.sc = $.getFilterFromQS('sc');
            if ($.so.length > 0 && $.sc.length > 0) {
                sortListLI.removeClass("selected");
                sortListLI.each(function () {
                    if ($(this).attr('sortqs') == ('so=' + $.so + '&sc=' + $.sc)) {
                        $(this).addClass('selected')
                        var sortByText = $(this).text();
                        $(".sort-by-title").find(".sort-select-btn").html(sortByText);
                    }
                });
            }
        }
    }
};

$.appendToQS = function (temp, name, value) {
    if (temp.length > 0)
        temp += "&" + name + "=" + value;
    else
        temp += name + "=" + value;

    return temp;
};

$.fn.resetAll = function () {
    return $(this).click(function () {
        $("span.selected").remove();
        $(".filter-selection-div li").each(function () {
            $(this).removeClass("active").addClass("uncheck");
        });
        //$('.filter-select-title').text($('.filter-select-title .default-text').prev().find('.hide').text())
        $('.filter-select-title .default-text').each(function () {
            $(this).text($(this).prev().text());
        });
        $('#minInput').val('').attr("data-value",'');
        $('#maxInput').val('').attr("data-value",'');
        minAmount.text('');
        maxAmount.text('');
        defaultText.show();
        count = 0;
        resetBWTabs();
        var a = $(".more-filters-btn");
        if (a.hasClass("open"))
            moreLessTextChange(a);
        $(".more-filters-btn").removeClass("open");
        $(".more-filters-container").slideUp();
        $('.filter-counter').text(count);
        $.pageNo = 1;
        var so = $.getFilterFromQS('so');
        var sc = $.getFilterFromQS('sc');
        var completeQS = '';
        if (so.length > 0 && sc.length > 0)
            completeQS = "sc=" + sc + "&so=" + so;
        $.pushState(completeQS,"resetButton");
    });
};

$("#btnReset").resetAll();

$.fn.onCheckBoxClick = function () {
    return this.click(function () {
        var clickedLI = $(this);
        var name = $(this).parents().find('.filter-selection-div').attr('name');
        var clickedLIText = clickedLI.text();
        var fid = clickedLI.attr("filterId");
        var a = clickedLI.parent().parent().prev(".filter-div");
        $.updateFilters(clickedLI, name, fid, 1);
    });
}

$.fn.onToggelFilterClick = function () {
    return this.click(function () {
        var panel = $(this).closest(".bw-tabs-panel");

        $.updateFilters(panel, $(this).parent().parent().parent().parent().attr('name'), $(this).attr('filterid'), 2);
    });
};

$.getNextPageData = function () {
    $.pageNo = parseInt($.getFilterFromQS("pageno"));
    if (isNaN($.pageNo))
        $.pageNo = 2;
    else
        $.pageNo++;
    $.loadNextPage();
};

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

$.updateFilters = function (node, name, value, type) {
    if (type == 1)
        $.applyCheckBoxFilter(name, value, node);
    else if (type == 2)
        $.applyToggelFilter(name, value, node);
    else if (type == 5)
        $.applyMinMaxFilter(name, value, node);
};

liList.onCheckBoxClick();

liToggelFilter.onToggelFilterClick();

$.valueFormatter = function (num) {
    if (num >= 100000) {
        return (num / 100000).toFixed(1).replace(/\.0$/, '') + 'L';
    }
    if (num >= 1000) {
        return (num / 1000).toFixed(1).replace(/\.0$/, '') + 'K';
    }
    return num;
}

$.generateMaxList = function (dataValue) {
    counter = 0;
    maxList.empty();
    dataValue = parseInt(dataValue);
    for (p in arr) {
        var a = arr[p].value;
        if (dataValue < a && counter <= 8) {
            maxList.append("<li data-value=" + arr[p].value + ">" + arr[p].amount + "</li>");
            counter++;
        }
    }
};

$.applyMinMaxFilter = function (name, value, node) {

    if ((/undefined/g).test(value) || (/NaN/g).test(value)) return;

    $.removePageNoParam();
    $.removeKnockouts();
    var tempQS = '';
    var curValue = $.getFilterFromQS(name).replace(/ /g, '+');

    if (value != '' && value != undefined) {
        tempQS = $.removeFilterFromQS(name);
        tempQS = $.appendToQS(tempQS, name, value);
    }
    else {
        tempQS = $.removeFilterFromQS(name);
    }

    $.pushState(tempQS,name);
};

$.pushGTACode = function (noOfRecords, filterName) {
   // dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Search_Page', 'act': 'Filter_Select_'+ noOfRecords, 'lab': filterName });
};

$.PricePopUpClickGA = function (makeName) {
   // dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Search_Page', 'act': 'Get_On_Road_Price_Click', 'lab': makeName });
};

sortByDiv.click(function(){
	if(!sortByDiv.hasClass("open"))
		$.sortChangeDown(sortByDiv);
	else
		$.sortChangeUp(sortByDiv);
});

$.sortChangeDown = function(sortByDiv){
	sortByDiv.addClass("open");
	sortListDiv.show();
};

$.sortChangeUp = function(sortByDiv){
	sortByDiv.removeClass("open");
	sortListDiv.slideUp();	
};

$.fn.applySortFilter = function () {
    return $(this).click(function () {
        $.removeKnockouts();
        $.removePageNoParam();
        
        var node = $(this);
        var completeQS = $.removeFilterFromQS('so');
        window.location.hash = completeQS;
        completeQS = $.removeFilterFromQS('sc');
        window.location.hash = completeQS;

        sortListLI.removeClass('selected');
        node.addClass('selected');
        var sortByText = $(this).text();
        $(".sort-by-title").find(".sort-select-btn").html(sortByText);
        $.sortChangeUp(sortByDiv);

        var so = node.attr('so');
        var sc = node.attr('sc');
        if (completeQS.length > 0) {
            if (so.length > 0 && sc.length > 0)
                completeQS = completeQS + "&so=" + so + "&sc=" + sc;
        }
        else {
            if (so.length > 0 && sc.length > 0)
                completeQS = "so=" + so + "&sc=" + sc;
        }
        $.pushState(completeQS, sortByText);
    });
}

sortListLI.applySortFilter();

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

var minAmount = $(".minAmount"),
    maxAmount = $(".maxAmount");
    maxInputVal = "",
    minList = $("#minList"),
    maxList = $("#maxList"),
    minInput = $("#minInput"),
    maxInput = $("#maxInput"),
    budgetListContainer = $("#budgetListContainer");

$(document).mouseup(function (e) {

    var filterDivContainer = $(".filter-div");
    var filterDivTitle = $(".filter-select-title");
    var filterSelectedText = $(".filter-select-title span.selected");
    var filterArrow = $(".fa-angle-down");
    var filterSelectBtn = $(".filter-select-btn");
    var filterSelectionDiv = $(".filter-selection-div");
    var filterSelectionUL = $(".filter-selection-div ul");
    var filterSelectionList = $(".filter-selection-div ul li");
    var filterSelectionLIText = filterSelectionList.find("span");
    if (filterSelectionDiv.hasClass("open")) {
        if (!filterDivContainer.is(e.target) && !filterDivTitle.is(e.target) && !filterSelectedText.is(e.target) && !filterArrow.is(e.target) && !filterSelectBtn.is(e.target) && !filterSelectionUL.is(e.target) && !filterSelectionList.is(e.target) && !filterSelectionLIText.is(e.target)) {
            filterSelectionDiv.slideUp();
            filterDivContainer.removeClass("open");
            filterSelectionDiv.removeClass("open");
        }
    }

    var container = $("#budgetListContainer");
    if (container.hasClass('show') && $("#budgetListContainer").is(":visible")) {
        if (!container.is(e.target) && container.has(e.target).length === 0) {
            var elementId = $('#' + e.target.id).parent().attr('id');
            var elementClass = $('#' + e.target.id).parent().attr('class');
            minDataValue = parseInt(minInput.attr("data-value")) || 0;
            maxDataValue = parseInt(maxInput.attr("data-value")) || 0;

            if (elementId != "minMaxContainer" && elementClass != "budget-box" && (minDataValue <= maxDataValue || maxDataValue == 0)) {
                $('#minMaxContainer').trigger('click');
                maxList.hide();
                maxInput.removeClass("border-red");
                $(".bw-blackbg-tooltip-max").hide();
                container.removeClass("invalid show").addClass("hide");
                
                $.inputValFormatting(minInput);
                $.inputValFormatting(maxInput);
            }
            else if (minDataValue > maxDataValue) {
                maxInput.addClass("border-red");
                $(".bw-blackbg-tooltip-max").show();
            }
        }
    }

});

$.validateInputValue = function () {
    minDataValue = parseInt(minInput.attr("data-value")) || 0;
    maxDataValue = parseInt(maxInput.attr("data-value")) || 0;
    var isValid = false;
    if (minDataValue <= maxDataValue) {
        maxInput.removeClass("border-red");
        $(".bw-blackbg-tooltip-max").hide();
        isValid = true;
    }
    else if (maxDataValue > 0 && minDataValue > maxDataValue) {
        maxInput.addClass("border-red");
        $(".bw-blackbg-tooltip-max").show();
        isValid = false;
    }
    else if (maxDataValue == 0)
        isValid = true;
    return isValid;
}

$(".budget-box").click(function () {
    minDataValue = parseInt(minInput.attr("data-value")) || 0;
    maxDataValue = parseInt(maxInput.attr("data-value")) || 0;

    if (minDataValue <= maxDataValue || maxDataValue == 0) {
        $("#minMaxContainer").toggleClass("open");
        budgetListContainer.toggleClass("hide show");
        minList.show();
        maxList.hide();
        maxInput.removeClass("border-red");
        $(".bw-blackbg-tooltip-max").hide();
    }
    else if (minDataValue > 1)
        $.validateInputValue();
});

for (p in arr) {
    if (counter <= 8) {
        minList.append("<li data-value=" + arr[p].value + ">" + arr[p].amount + "</li>");
        counter++;
    }
}

minInput.on("click focus", function () {
    minList.show();
    maxList.hide();

    $.inputValFormatting(maxInput);

    minInputVal = $(this).val();
    if (minInputVal == "")
        minInput.val("");
    else {
        var userMinInput = minInput.attr("data-value");
        minInput.val($.formatPrice(userMinInput));
    }

    minInput.on("keyup", function () {
        maxInputVal = maxInput.val();
        userMinAmount = $(this).val();
        if (userMinAmount == "") {
            minInput.val("");
            minInput.attr("data-value", "");
            minAmount.html("0");
        }
        else {
            $("#budgetBtn").hide();
            minInput.attr("data-value", parseInt(userMinAmount.replace(/\D/g, ''), 10)).val($.formatPrice(userMinAmount.replace(/\D/g, ''), 10));
            formattedValue = $.newUserInputPrice(userMinAmount);
            minAmount.html(formattedValue);
            if (maxInputVal == "")
                maxAmount.html(" - MAX");
        }
        if ($("#budgetBtn").is(':visible'))
            minAmount.html("");
    });
});

maxInput.on("click focus", function () {
    maxInput.removeClass("border-red");
    $(".bw-blackbg-tooltip-max").hide();

    if (!maxList.hasClass("refMinList")) {
        var defaultValue = 30000;
        $.generateMaxList(defaultValue);
    }

    minList.hide();
    maxList.show();

    $.inputValFormatting(minInput);

    var userMaxAmount = $(this).val();
    if (userMaxAmount == "")
        maxInput.val("");
    else {
        var userMaxInput = maxInput.attr("data-value");
        maxInput.val($.formatPrice(userMaxInput));
    }

    maxInput.on("keyup", function (e) {
        userMaxAmount = $(this).val();
        /* when the user deletes the last digit left in the input field */
        if (e.keyCode == 8 && userMaxAmount.length == 0)
            maxAmount.html(" - MAX");

        if (userMaxAmount.length == 0 && $('#budgetBtn').not(':visible')) {
            maxInput.val("");
            maxInput.attr("data-value", "");
        }
        else {
            $("#budgetBtn").hide();
            userMinAmount = minInput.val();
            if (userMinAmount == "")
                minAmount.html("0");

            formattedValue = $.newUserInputPrice(userMaxAmount);
            maxInput.attr("data-value", parseInt(userMaxAmount.replace(/\D/g, ''), 10)).val($.formatPrice(userMaxAmount.replace(/\D/g, ''), 10));
            maxAmount.html("- " + formattedValue);
        }
    });

    /* based on user input value generate max list */
    var userInputMin = minInput.val();
    var userInputDataValue = minInput.attr("data-value");
    if (userInputMin.length != 0)
        $.generateMaxList(userInputDataValue);
});

minInput.on('focusout', function () {
    if ($.validateInputValue())
        $.applyMinMaxFilter('budget', minInput.attr('data-value') + '-' + (maxInput.attr('data-value') == 0 || maxInput.attr('data-value') == undefined ? '' : maxInput.attr('data-value')), $(this));
});

maxInput.on('focusout', function () {
    if ($.validateInputValue())
        $.applyMinMaxFilter('budget', minInput.attr('data-value') + '-' + (maxInput.attr('data-value') == 0 || maxInput.attr('data-value') == undefined ? '' : maxInput.attr('data-value')), $(this));
});

$.setMinAmount = function (userMinAmount) {
    if (userMinAmount == "") {
        minInput.val("").attr("data-value", "");
        minAmount.html("0");
    }
    else {
        $("#budgetBtn").hide();
        var formattedValue = $.newUserInputPrice(userMinAmount);
        minAmount.text(formattedValue);
        minInput.val(formattedValue).attr('data-value', userMinAmount);
    }
    if ($("#budgetBtn").is(':visible'))
        minAmount.html("");
};

$.setMaxAmount = function (userMaxAmount) {
    if (e.keyCode == 8 && userMaxAmount.length == 0)
        maxAmount.html(" - MAX");

    if (userMaxAmount.length == 0 && $('#budgetBtn').not(':visible')) {
        maxInput.val("").attr("data-value", "");
        maxAmount.html("- MAX");
    }
    else {
        $("#budgetBtn").hide();
        var userMinAmount = minInput.val();
        if (userMinAmount == "")
            minAmount.html("0");

        var formattedValue = $.newUserInputPrice(userMaxAmount);
        maxAmount.html("- " + formattedValue);
        maxInput.val(formattedValue).attr('data-value', userMaxAmount);
    }
};

/* allow only digits in the input field */
$.isNumberKey = function (evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

$.fn.validateKeyPress = function ()
{
    return this.keypress(function () {
        return $.isNumberKey(event);
    });
};
minInput.validateKeyPress();
maxInput.validateKeyPress();

minList.delegate("li", "click", function () {
    var clickedLI = $(this);
    var amount = clickedLI.text();
    var dataValue = clickedLI.attr("data-value");
    var prevVal = minInput.attr('data-value');
    minInput.attr('data-value', dataValue);
    maxInputVal = maxInput.val();
    if (maxInputVal == "")
        $(".maxAmount").html("- MAX");
    $("#budgetBtn").hide();
    $.generateMaxList(dataValue);
    $.setMinAmount(dataValue);

    minList.hide();
    maxList.show().addClass("refMinList");
    if (parseInt(dataValue) <= parseInt(maxInput.attr('data-value')))
        $.applyMinMaxFilter('budget', dataValue + '-' + (maxDataValue == 0 ? '' : maxDataValue), clickedLI);
});

maxList.delegate("li", "click", function () {
    var clickedLI = $(this);
    var amount = clickedLI.text();
    var dataValue = clickedLI.attr("data-value");
    maxInput.attr('data-value', dataValue);

    if (minInput.val() == 0) {
        $("#budgetBtn").hide();
        minInput.val(0);
        minInput.attr("data-value", 0);
        minAmount.html("0");
    }
    
    $("#minMaxContainer").removeClass("open");
    if ($.validateInputValue()) {
        maxList.hide();
        budgetListContainer.removeClass('show').addClass('hide');
        $.applyMinMaxFilter('budget', minInput.attr('data-value') + '-' + dataValue, clickedLI);
    }
});

$.newUserInputPrice = function (newMinMaxValue) {
    var newAmount = parseInt(newMinMaxValue.replace(/\D/g, ''), 10);
    var formattedValue = $.valueFormatter(newAmount);
    return formattedValue;
}

/* convert the comma seperated price into INR format*/
$.inputValFormatting = function (priceInput) {
    var inputVal = priceInput.val();
    var str = '';
    var regex = new RegExp(',', 'g');
    if (inputVal.length > 0) {
        str = new String(priceInput.attr("data-value"));
        str = str.replace(regex, '');
        priceInput.val($.valueFormatter(str));
    }
};

/* priceFormatter */
$.formatPrice = function (price) {
    var thMatch = /(\d+)(\d{3})$/;
    var thRest = thMatch.exec(price);
    if (!thRest) return price;
    return (thRest[1].replace(/\B(?=(\d{2})+(?!\d))/g, ",") + "," + thRest[2]);
}

/* booking listing js */
$("#searchBikeList").on("click", "span.view-offers-target", function () {
    var offersDiv = $(this).parents("div.bike-book-now-wrapper").next("div#offersPopup");
    offersPopupOpen(offersDiv);
});

$("#searchBikeList").on("click", "div.offers-popup-close-btn", function () {
    var offersDiv = $(this).parents("div#offersPopup");
    offersPopupClose(offersDiv);
});

$(".blackOut-window").on("click", function () {
    closeVisiblePopup();
});

var offersPopupOpen = function (offersDiv) {
    offersDiv.show();
    $(".blackOut-window").show();
};

var offersPopupClose = function (offersDiv) {
    offersDiv.hide();
    $(".blackOut-window").hide();
};

$(document).keydown(function (e) {
    if (e.keyCode == 27)
        closeVisiblePopup();
});

var closeVisiblePopup = function () {
    if (listingLocationPopup.is(":visible"))
        listingLocationPopupClose();
    if ($("div#offersPopup").is(":visible"))
        offersPopupClose($("div#offersPopup"));
};

$(".change-city-area-target").on("click", function () {
    listingLocationPopupOpen();
});


$(".location-popup-close-btn").on("click", function () {
    listingLocationPopupClose();
});

var listingLocationPopup = $("#listingLocationPopup");

var listingLocationPopupOpen = function () {
    listingLocationPopup.show();
    $(".blackOut-window").show();
};

var listingLocationPopupClose = function () {
    listingLocationPopup.hide();
    $(".blackOut-window").hide();
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

function registerPQ(myData) {
    var obj = {
        'CityId': selectedCityId,
        'AreaId': selectedAreaId,
        'ModelId': myData.modelEntity.modelId(),
        'ClientIP': clientIP,
        'SourceType': '1',
        'VersionId': myData.versionEntity.versionId(),
        'pQLeadId': pqSourceId,
        'deviceId': getCookie('BWC'),
        'dealerId': myData.dealerId()
    };
    $.ajax({
        type: 'POST',
        url: "/api/v1/registerpq/",
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
            if (jsonObj != undefined && jsonObj.quoteId != "" && jsonObj.dealerId > 0) {
               // gtmCodeAppenderWidget(pageId, 'Dealer_PriceQuote_Success_Submit', gaLabel);
                window.location = "/pricequote/bookingsummary_new.aspx?MPQ=" + Base64.encode(cookieValue);
            }
            else {
               // gtmCodeAppenderWidget(pageId, 'BW_PriceQuote_Error_Submit', gaLabel);
                //$("#errMsgOnRoad").text("Oops. We do not seem to have pricing for given details.").show();
            }
        },
        error: function (e) {
            alert('oops !')
            //gtmCodeAppenderWidget(pageId, 'BW_PriceQuote_Error_Submit', gaLabel);
           // $("#errMsg").text("Oops. Some error occured. Please try again.").show();
        }
    });
}


$('#termsPopUpCloseBtn').on("click", function () {
    $(".blackOut-window").hide();
    $("div#termsPopUpContainer").hide()
    $("div#offersPopup").hide();
});

function LoadTerms(offerId) {
    $("div#offersPopup").hide();
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
                $(".termsPopUpContainer").css('height', '450')
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