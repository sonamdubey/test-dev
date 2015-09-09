//on scroll sort fixed
var $sortDiv = $("#sort-by-div");
$.pageSize = 10;
$.infiniteScrollPageLimit = 10; // Not used for now
var pagiFlag = false;
var $window = $(window);
$.totalCount = "";
$.pageNo = "";
$.nextPageUrl = "";
$.lazyLoadingStatus = true;
// Set the effect type
$.effect = 'slide';
// Set the options for the effect type chosen
$.options = { direction: 'right' };
// Set the duration (default: 400 milliseconds)
$.duration = 500;

$(document).ready(function () {
    var completeQs = window.location.hash.replace('#', '');
    completeQs = $.removeFilterFromQS('pageno');
    $.pushState(completeQs);
});//-- document ready ends here 

//Sort by div popup
$("#sort-btn").click(function () {
    $("#sort-by-div").toggle($.effect, $.options, $.duration);
    $("html, body").animate({ scrollTop: $("header").offset().top }, 0);
});

$('#sort-by-div a[data-title="sort"]').click(function () {
    $('.sort-arw').hide();
    $(this).find('.sort-arw').css('display', 'inline-block');
    $(this).find('.sort-arw').toggleClass('fa-long-arrow-up');
});

//filter div popup
var filterOffset = 0;
$("#filter-btn").click(function () {
    $("#filter-div").show($.effect, $.options, $.duration, function () {
        //$.selected Filters
    });
    $(".popup-btn-filters").show(); //Filter btn 
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


/** multiple select dropdown **/

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
    if ($sortDiv.is(':visible')) {
        if ($(window).scrollTop() > 50) {
            $('#sort-by-div').addClass('fixed');
        } else {
            $('#sort-by-div').removeClass('fixed');
        }
    }

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

  /*$(".dropdown ul li").on('click', function () {
	  $(".dropdown ul").hide();
  });*/

  $(document).bind('click', function (e) {
	  var $clicked = $(e.target);
	  if (!$clicked.parents().hasClass("dropdown")) $(".dropdown ul").hide();
  });

  $('.mutliSelect .unchecked').on('click', function () { //debugger;
  	  var $dropDown = $(this).closest('.dropdown');
	  $(this).toggleClass('checked');
	  var title = $(this).closest('.mutliSelect').find('span').text(),
		  title = $.trim($(this).text()) + ",";
	
	  checkedLen = $dropDown.find('.mutliSelect .unchecked.checked').length;
	  controlWidth = $dropDown.find('.form-control').width();
	  hidaWidth = $dropDown.find('.hida').width();
	  remainSpace = controlWidth - hidaWidth - 10;
	  multiselWidth;
	  
	  if ($(this).hasClass('checked')) {
		  var html = '<span data-title="' + title + '">' + title + '</span>';
		  
		  $dropDown.find('.multiSel').append(html);
		  $dropDown.find(".hida").addClass('shiftRight');
	  	  multiselWidth = $dropDown.find('.multiSel').width();
		 if( checkedLen > 1 && multiselWidth > remainSpace ){
			$dropDown.find('.multiSel').css('max-width',remainSpace+'px');
		 }
	  } 
	  else {
		  $dropDown.find('span[data-title="' + title + '"]').remove();
		  if(checkedLen < 1){
		  	$dropDown.find(".hida").removeClass('shiftRight');
		  }
		  multiselWidth = $dropDown.find('.multiSel').width();
		  if(multiselWidth < remainSpace){
			$dropDown.find('.multiSel').css('max-width','none');
		  }
	  }
  });
  
  /* Mileage */
  $('.mileage').click(function(){
	  $(this).toggleClass('optionSelected');
  });
  
  $('.checkOption').click(function(){
	  $(this).siblings().removeClass('optionSelected');
	  $(this).toggleClass('optionSelected');
  });
  
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
        //console.log('#'+hash);
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
        },
        error: function (error) {
            $.totalCount = 0;
            var element = $('#divSearchResult');
            element.html('');
            ko.cleanNode(element);
            $('#NoBikeResults').show();
            $('#bikecount').text($.totalCount + ' bikes found');
            //$.selectFiltersPresentInQS();
            //$.getSelectedQSFilterText();
        }
    });
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

$.loadNextPage = function () {
    var completeQS = $.removeFilterFromQS("pageno");
    if (completeQS.length > 0)
        completeQS += "&pageno=" + $.pageNo;
    else
        completeQS = "pageno=" + $.pageNo;
    $.pushState(completeQS);
}

$.pushState = function (qs) {
    window.location.hash = qs;
    $.hitAPI(qs);
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

$.removeKnockouts = function () {
    $(".SRko").each(function () {
        $(this).remove();
    });
};

$.applyToggelFilter = function (name, value, node) {
    $.removePageNoParam();
    $.removeKnockouts();
    var checked = 'optionSelected';
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

    $.pushState(tempQS);
};
