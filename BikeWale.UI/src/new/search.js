var appendText = $(".filter-select-title"),
    currentList = $(".filter-selection-div"),
    liList = $(".filter-selection-div ul li"),
    liToggelFilter = $(".bw-tabs li"),
    defaultText = $(".default-text"),
    sortCriteria = $('#sort'),
    sortByDiv = $(".sort-div"),
    sortListDiv = $(".sort-selection-div"),
    sortListLI = $(".sort-selection-div ul li");

var count = 0;
var searchUrl = "";
$.pageNo = "";
$.budget = "";
$.Displacement = "";
$.Bike = "";
$.Mileage = "";
$.RideStyle = "";
$.ABS = "";
$.brack = "";
$.AlloyWheel = "";
$.StartType = "";
$.so = "";
$.sc = "";
$.queryString = "";
$.nextPageUrl = "";
$.totalCount;
$.lazyLoadingStatus = true;
var $window = $(window),
    $menu = $('#filter-container'),
    menuTop = $menu.offset().top;

var SearchViewModel = function (model) {
    ko.mapping.fromJS(model, {}, this);
};

$(document).ready(function () {
    
    var completeQs = window.location.hash.replace('#', '');
    completeQs = $.removeFilterFromQS('pageno');
    $.pushState(completeQs,"");
});

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
            if (elementId != "minMaxContainer" && elementClass != "budget-box")
                $('#minMaxContainer').trigger('click');
        }
    }

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
    }
    else {
        $(this).removeClass("open");
        $(".more-filters-container").slideUp();
    }
});

$(".filter-done-btn").click(function () {
    $(".more-filters-container").slideUp();
    stateChangeUp($('.filter-div'), $('.filter-div'));
});

var AppendCertificationStar = function (abStars) {
    var i, intVal, val, count = 0, certificationStar = "";
    val = abStars;
        intVal = Math.floor(val);
        for (i = 0; i < intVal ; i++) {
            certificationStar += '<img src="/images/ratings/1.png" alt="Rate">';
        }
        if (val > intVal) {
            certificationStar += '<img src="/images/ratings/half.png" alt="Rate">';
            count = (5 - intVal) - 1;
            for (i = 0; i < count; i++) {
                certificationStar += '<img src="/images/ratings/0.png" alt="Rate">';
            }
        }
        else {
            count = 5 - intVal;
            for (i = 0; i < count; i++) {
                certificationStar += '<img src="/images/ratings/0.png" alt="Rate">';
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

    if (json.searchResult.length > 0)
        ko.applyBindings(new SearchViewModel(json), element);
    else
        $('#NoBikeResults').show();
};

$.hitAPI = function (searchUrl,filterName) {
    $.ajax({
        type: 'GET',
        url: '/api/NewBikeSearch/?' + searchUrl,
        dataType: 'json',
        success: function (response) {
            $.totalCount = response.totalCount;
            $.pageNo = response.curPageNo;
            $.nextPageUrl = response.pageUrl.nextUrl;
            $('#bikecount').text($.totalCount+' Bikes');
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
            if (filterName != "" && filterName != undefined)
                $.pushGTACode($.totalCount, filterName);
        },
        error: function (error) {
            $.totalCount = 0;
            var element = $('#divSearchResult');
            element.html('');
            ko.cleanNode(element);
            $('#loading').hide();
            $('#NoBikeResults').show();
            $('#bikecount').text('No bikes found');
            $.selectFiltersPresentInQS();
            $.getSelectedQSFilterText();
            if (filterName != "" && filterName != undefined)
                $.pushGTACode($.totalCount, filterName);
        }
    });
};

$.bindLazyListings = function (searchResult) {
    var koHtml = '<div class="grid-12 margin-top20 margin-bottom10">'
                        + '<ul id="divSearchResult' + $.pageNo + '" class="SRko" data-bind="template: { name: \'listingTemp\', foreach: searchResult }">'
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
        $(".selected").remove();
        $(".filter-selection-div li").each(function () {
            $(this).removeClass("active").addClass("uncheck");
        });
        //$('.filter-select-title').text($('.filter-select-title .default-text').prev().find('.hide').text())
        $('.filter-select-title .default-text').each(function () {
            $(this).text($(this).prev().text());
        });
        $('#minInput').val('');
        $('#maxInput').val('');
        defaultText.show();
        count = 0;
        resetBWTabs();
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
                var min = $('#minInput'), max = $('#maxInput');
                var values = $.getFilterFromQS(params[i]).split('-');
                min.val(values[0]);
                max.val(values[1]);
                $.formatPrice(min, max);
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
        $.applyMinMaxFilter(name, value,node);
};

liList.onCheckBoxClick();

liToggelFilter.onToggelFilterClick();

$(".budget-box").click(function (e) {
    $("#minMaxContainer").toggleClass("open");
    $("#budgetListContainer, #minPriceList").toggleClass("hide show");
});

$('.priceBox').keyup(function () {
    $.formatPrice($('#minInput'), $('#maxInput'));
});

$.formatPrice = function (minId,maxId) {
    var minValue = '', maxValue = '';
    var minBudget = minId.val();
    var maxBudget = maxId.val();

    minValue = minBudget != '' && minBudget != undefined ? $.valueFormatter(minBudget) : '';
    maxValue = maxBudget != '' && maxBudget != undefined ? $.valueFormatter(maxBudget) : '';

    $("#budgetBtn").html("<span class='minbudgetvalue'>" + minValue + "</span> - <span class='maxbudgetvalue'> " + maxValue + "</span>");

    if (minBudget == '' && maxBudget == '')
        $("#budgetBtn").text("Select budget");
};

$.valueFormatter=function(num) {
    if (num >= 100000) {
        return (num / 100000).toFixed(1).replace(/\.0$/, '') + 'L';
    }
    if (num >= 1000) {
        return (num / 1000).toFixed(1).replace(/\.0$/, '') + 'K';
    }
    return num;
}

$.fn.applyBudgetFilter = function () {
    return $(this).focusout(function () {
        var min = $('#minInput'), max = $('#maxInput');
        if ($.budgetValidator(min.val(), max.val())) {
            var budgetValue = (min.val() == '' ? 0 : min.val()) + '-' + max.val();
            $.updateFilters($(this), $(this).parent().parent().attr('name'), budgetValue, 5);
            $("#minPriceList").removeClass("hide").addClass("show");
            $("#maxPriceList").css("display", "none");
        }
        else {
            completeQs = $.removeFilterFromQS('budget');
            $.pushState(completeQs,"budget");
        }
    });
};

$.budgetValidator = function (minValue, maxValue) {
    var isValid = true;
    if (maxValue != '') {
        if (parseInt(maxValue) <= parseInt(minValue)) {
            $(".bw-blackbg-tooltip.bw-blackbg-tooltip-max").removeClass("hide").addClass("show");
            isValid = false;
        }
        else {
            $(".bw-blackbg-tooltip.bw-blackbg-tooltip-max").addClass("hide").removeClass("show");
            isValid = true;
        }
    } else if (minValue == '' && maxValue == '') {
        isValid = false;
    }
    return isValid;
};

$.applyMinMaxFilter = function (name, value, node) {
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

$('.priceBox').applyBudgetFilter();

$.pushGTACode = function (noOfRecords, filterName) {
    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Search_Page', 'act': 'Filter_Select_'+ noOfRecords, 'lab': filterName });
};

$.PricePopUpClickGA = function (makeName) {
    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Search_Page', 'act': 'Get_On_Road_Price_Click', 'lab': makeName });
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
        var node = $(this);
        var completeQS = $.removeFilterFromQS('so');
        window.location.hash = completeQS;
        completeQS = $.removeFilterFromQS('sc');
        window.location.hash = completeQS;

        $.removePageNoParam();
        $.removeKnockouts();

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