var $sortDiv = $("#sort-by-div"),
    applyFilter = $('#btnApplyFilters'),
    mileage = $('.mileage'),
    CheckBoxFilter = $('.multiSelect .unchecked'),
    multiSelect = $('.multiSelect'),
    nobikediv = $('#nobike'),
    loading = $('#loading'),
    resetButton=$('#btnReset');

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
    var completeQs = location.href.split('?')[1];
    completeQs = $.removeFilterFromQS('pageno');

    $.selectedValueSortTab();

    $.pushState(completeQs);
});//-- document ready ends here 

$.selectedValueSortTab = function () {
    var node=$('#sort-by-div');
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
    $("#sort-by-div").toggle($.effect, $.options, $.duration);
    $("html, body").animate({ scrollTop: $("header").offset().top }, 0);
});

$('#sort-by-div a[data-title="sort"]').click(function () {
    $.removePageNoParam();
    $.removeKnockouts();
    $.scrollToTop();

    $.so = '0';
    var newurl = '';
    if ($(this).hasClass('price-sort')) {
        var sortOrder=$(this).attr('so');
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

    if ($.sc != '1')
    {
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

$(window).resize(function(){
	$('.dropdown').each(function () { 
  	  var $dropDown = $(this);
	  controlWidth = $dropDown.find('.form-control').width();
	  hidaWidth = $dropDown.find('.hida').width();
	  remainSpace = controlWidth - hidaWidth - 10;
	  multiselWidth = $dropDown.find('.multiSel').width();
	  $dropDown.find('.multiSel').css('max-width',remainSpace+'px');
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
	  if($ul.is(':visible')){
	  	$ul.slideUp('fast');
	  }
	  else{
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
		 if( checkedLen > 1 && multiselWidth > remainSpace ){
			$dropDown.find('.multiSel').css('max+-width',remainSpace+'px');
		 }
	  } 
	  else {
		  $dropDown.find('span[data-title="' + title + '"]').remove();
		  if(checkedLen < 1){
		  	$dropDown.find(".hida").removeClass('hide');
		  }
		  multiselWidth = $dropDown.find('.multiSel').width();
		  if(multiselWidth < remainSpace){
			$dropDown.find('.multiSel').css('max-width','none');
		  }
	  }
  });
  
  /* Mileage */
  mileage.click(function(){
	  $(this).toggleClass('optionSelected');
  });
  
  $('.checkOption').click(function(){
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

$.hitAPI = function (searchUrl) {
    $.ajax({
        type: 'GET',
        url: '/api/NewBikeSearch/?' + searchUrl,
        dataType: 'json',
        success: function (response) {
            nobikediv.hide();
            $.totalCount = response.totalCount;
            $.pageNo = response.curPageNo;
            $.nextPageUrl = response.pageUrl.nextUrl;
            $('#bikecount').text($.totalCount + ' Bikes');
            if (!isNaN($.pageNo) && $.pageNo == 1) {
                $.bindSearchResult(response);
            }
            else {
                $.bindLazyListings(response);
            }
            $.lazyLoadingStatus = true;
            $('#hidePopup').click();
            loading.hide();
        },
        error: function (error) {
            $.totalCount = 0;
            var element = $('#divSearchResult');
            element.html('');
            ko.cleanNode(element);
            nobikediv.show();
            loading.hide();
            $('#hidePopup').click();
            $('#bikecount').text($.totalCount + ' bikes found');
        }
    });
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

    if (json.searchResult.length > 0)
        ko.applyBindings(new SearchViewModel(json), element);
    else
        $('#NoBikeResults').show();
};

$.bindLazyListings = function (searchResult) {
    var koHtml = '<div id="divSearchResult' + $.pageNo + '" class="SRko" data-bind="template: { name: \'listingTemp\', foreach: searchResult }">'
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
    var result = {};
    var propval, filterName, value;
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
        if (value.indexOf('+') > 0)
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

$.getAllParamsFromQS = function () {
    var completeQS = window.location.href.split('?')[1];
    var params = [];

    if (completeQS.length > 1) {
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
        var completeQS='';
        var completeQSArr = new Array();
        completeQSArr.push($.applyToggelFilter());
        completeQSArr.push($.applyMileageFilter());
        completeQSArr.push($.applyCheckBoxFilter());
        completeQSArr.push($.applySliderFilter($('#mSlider-range'), $('#mSlider-range').attr('name')));

        for(var i=0;i<completeQSArr.length;i++)
        {
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
        }else 
        {
            $("#rangeAmount").html('<span class="bw-m-sprite rupee"></span>' + ' ' + budgetminValue +  ' ' + '-' + ' ' + '<span class="bw-m-sprite rupee"></span>' + ' ' + budgetmaxValue);
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

$.applyMileageFilter = function () {
    var selected = 'optionSelected',
        curValue = "",
        completeQS = "",
        name = "",
        value = "";
    mileage.each(function () {
        name = $(this).parent().parent().attr('name');
        if ($(this).hasClass(selected)) {
            var filterId = $(this).attr('filterid');
            value += filterId + '+';
        }
    });
    if (value.length > 1) {
        value = value.substring(0, value.length - 1);
        completeQS = $.AddToQS(name, value);
    }
    else
        completeQS = "";
    return completeQS;
};

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
            if ($(this).hasClass(selected))
            {
                var filterId = $(this).attr('filterid');
                value += filterId + '+';
            }
        });
        if (value.length > 1) {
            value = value.substring(0, value.length - 1);
            tempQS = $.AddToQS( name, value);
            tempArray.push(tempQS);
        }
    });

    if (tempArray.length > 0)
    {
        for (var i = 0; i < tempArray.length; i++)
            completeQS += tempArray[i] + '&';
    }
    if (completeQS.length > 1)
        completeQS = completeQS.substring(0, completeQS.length - 1);
    return completeQS;
};

$.applySliderFilter = function (element,name) {
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

$.getSliderValue=function(budgetValue)
{
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
    var params = ['bike', 'displacement', 'ridestyle', 'AntiBreakingSystem', 'braketype', 'alloywheel', 'starttype', 'budget', 'mileage'];
    for (var i = 0; i < params.length; i++) {
        if (params[i].length > 0) {
            var node = $('div[name=' + params[i] + ']');
            if (params[i] == 'bike' || params[i] == 'displacement' || params[i] == 'ridestyle') {
                node.prev().find('.hida').removeClass('hide');
                node.prev().find('.multiSel').html('');
                node.find('li').each(function () {
                    $(this).removeClass('checked');
                });
            } else if (params[i] == 'AntiBreakingSystem' || params[i] == 'braketype' || params[i] == 'alloywheel' || params[i] == 'starttype') {
                node.children().each(function () {
                    $(this).removeClass('optionSelected');
                });
            } else if (params[i] == 'budget') {
                $.setSliderRangeQS($('#mSlider-range'), 1, 20);
                $("#rangeAmount").html('<span class="bw-m-sprite rupee"></span> 0 -' + ' ' + '<span class="bw-m-sprite rupee"></span> Any value');
            } else if (params[i] == 'mileage') {
                node.each(function () {
                    $(this).find('span').removeClass('optionSelected');
                });
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
            if (params[i] == 'bike' || params[i] == 'displacement' || params[i] == 'ridestyle') {
                var values = $.getFilterFromQS(params[i]).replace(/ /g, '+').split('+');
                var html='';
                for (var j = 0; j < values.length; j++) {
                    node.find('li[filterid=' + values[j] + ']').addClass('checked');
                    var title = node.find('li[filterid=' + values[j] + ']').text() + ',';
                    html += '<span data-title="' + title + '">' + title + '</span>';
                }

                node.prev().find('.hida').addClass('hide');
                node.prev().find('.multiSel').html(html);

            } else if (params[i] == 'AntiBreakingSystem' || params[i] == 'braketype' || params[i] == 'alloywheel' || params[i] == 'starttype') {
                var values = $.getFilterFromQS(params[i]);

                for (var j = 0; j < values.length; j++) {
                    node.find('span[filterid=' + values[j] + ']').addClass('optionSelected');
                }
            } else if (params[i] == 'budget') {
                var values = $.getFilterFromQS(params[i]).split('-');
                values[0] = (values[0] == '0' ? '30000' : values[0]);
                var minValue = $.getSliderValue(values[0]), maxValue = $.getSliderValue(values[1])

                $.setSliderRangeQS($('#mSlider-range'), minValue, maxValue);

                var budgetminValue = $.valueFormatter(values[0]);
                var budgetmaxValue = $.valueFormatter(values[1]);

                if (values[0] == 1 && values[1] == 20) {
                    $("#rangeAmount").html('<span class="bw-m-sprite rupee"></span> 0 -' + ' ' + '<span class="bw-m-sprite rupee"></span> Any value');
                } else {
                    $("#rangeAmount").html('<span class="bw-m-sprite rupee"></span>' + ' ' + budgetminValue + ' ' + '-' + ' ' + '<span class="bw-m-sprite rupee"></span>' + ' ' + budgetmaxValue);
                }
            } else if (params[i] == 'mileage') {
                var values = $.getFilterFromQS(params[i]).replace(/ /g, '+').split('+');

                for (var j = 0; j < values.length; j++) {
                    node.find('span[filterid=' + values[j] + ']').addClass('optionSelected');
                }
            }else if(params[i]=='sc')
            {
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
