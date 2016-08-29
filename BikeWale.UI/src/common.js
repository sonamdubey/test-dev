// JavaScript Document
var focusedMakeModel = null, focusedCity = null;
var objBikes = new Object();
var objCity = new Object();
var globalCityId = 0;
var _makeName = '';
var ga_pg_id = '0';
var pqSourceId = "37";
var IsPriceQuoteLinkClicked = false;
var monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"];

function triggerGA(cat, act, lab) {
    try {
        dataLayer.push({ 'event': 'Bikewale_all', 'cat': cat, 'act': act, 'lab': lab });
    }
    catch (e) {// log error   
    }
}
$('.bw-ga').click(function () {
  
    try {
        var obj = $(this);
        if (obj.attr('l') !== undefined) {
            triggerGA(obj.attr("c"), obj.attr("a"), obj.attr("l"));
        }
        else if (obj.attr('v') !== undefined) {
            triggerGA(obj.attr("c"), obj.attr("a"), window[obj.attr("v")]);
        }
        else if (obj.attr('f') !== undefined) {
            triggerGA(obj.attr("c"), obj.attr("a"), eval(obj.attr("f") + '()'));
        }
    }
    catch (e) {
    }
});

//fallback for indexOf for IE7
if (!Array.prototype.indexOf) {
    Array.prototype.indexOf = function (elt /*, from*/) {
        var len = this.length >>> 0;

        var from = Number(arguments[1]) || 0;
        from = (from < 0)
             ? Math.ceil(from)
             : Math.floor(from);
        if (from < 0)
            from += len;

        for (; from < len; from++) {
            if (from in this &&
                this[from] === elt)
                return from;
        }
        return -1;
    };
}

$(document).ready(function () {
    if (ga_pg_id != '1')
    $('#globalSearch').parent().show();

    $(".lazy").lazyload({
        effect: "fadeIn"
    });
    applyLazyLoad();
    $('#newBikeList').val('').focus();
    $('#globalCityPopUp').val('');
    var blackOut = $(".blackOut-window")[0];
    
	CheckGlobalCookie();

	$("#globalCityPopUp").bw_autocomplete({
	    width: 350,
	    source: 3,
	    recordCount: 8,
	    onClear: function () {
	        objCity = new Object();
	    },
	    click: function (event, ui, orgTxt) {
	        var city = new Object();
	        city.cityId = ui.item.payload.cityId;
	        city.maskingName = ui.item.payload.cityMaskingName;
	        var cityName = ui.item.label.split(',')[0];
	        var CookieValue = city.cityId + "_" + cityName, oneYear = 365;
	        SetCookieInDays("location", CookieValue, oneYear);
	        globalCityId = city.cityId;
	        CloseCityPopUp();
	        showGlobalCity(cityName);
	        dataLayer.push({ 'event': 'Bikewale_all', 'cat': GetCatForNav(), 'act': 'City_Popup_Default', 'lab': cityName });
	    },
	    open: function (result) {
	        objCity.result = result;
	    },
	    focusout: function () {
	        if ($('li.ui-state-focus a:visible').text() != "") {
	            focusedCity = new Object();
	            focusedCity = objCity.result[$('li.ui-state-focus').index()];
	        }
	    },
	    afterfetch: function (result, searchtext) {
	        var element = $("#globalCityPopUp");
	        if (result != undefined && result.length > 0)
	            showHideMatchError(element,false);
	        else
	            showHideMatchError(element,true);
	    }
	}).autocomplete("widget").addClass("globalCity-auto-desktop").css({ 'position': 'fixed' });
	
	// nav bar code starts
	$(".navbarBtn").click(function(){
		navbarShow();
	});
	$(".blackOut-window").mouseup(function(e){
		var nav = $("#nav");
        if(e.target.id !== nav.attr('id') && !nav.has(e.target).length)		
        {
		    nav.animate({'left':'-350px'});
			unlockPopup();
        }
    }); 
	$(".navUL > li > a").click(function(){		
		if(!$(this).hasClass("open")) {
			var a = $(".navUL li a");
			a.removeClass("open").next("ul").slideUp(350);
			$(this).addClass("open").next("ul").slideDown(350);
			
			if($(this).siblings().size() === 0) {
				navbarHide();
			}
			
			$(".nestedUL > li > a").click(function(){
				$(".nestedUL li a").removeClass("open");
				$(this).addClass("open");
				navbarHide();
			});
			
		}
		else if($(this).hasClass("open")) {
			$(this).removeClass("open").next("ul").slideUp(350);
		}
	}); // nav bar code ends here
	
	// login code starts 
	$("#firstLogin").click(function(){		
	    window.location.href = "/users/login.aspx?ReturnUrl=" + window.location.href;
	});
	$(".blackOut-window").mouseup(function(e){
		var loginPopUp = $(".loginPopUpWrapper");
        if(e.target.id !== loginPopUp.attr('id') && !loginPopUp.has(e.target).length)
        {
            loginPopUp.animate({'right':'-400px'});
			unlockPopup();
        }
    });
	
	$(".loginCloseBtn").click(function () {
        unlockPopup();
        $(".loginPopUpWrapper").animate({ right: '-400px' });
        $(".loggedinProfileWrapper").animate({ right: '-280px' });
        loginSignupSwitch();
    });
    $("#forgotpass").click(function () {
        $("#forgotpassdiv").toggleClass("hide show");
    });
    $("button.loginBtnSignUp").click(function () {
        $("div.loginStage").hide();
        $("div.signUpStage").show();
    });
    $("#btnSignUpBack").click(function () {
        $("div.signUpStage").hide();
        $("div.loginStage").show();
    });
    $("#btnSignUpBack").click(function () {
        loginSignupSwitch();
    });
	
	//user logged in code
	$("#userLoggedin").click(function(){
		$(".blackOut-window").show();
		$(".loggedinProfileWrapper").animate({right:'0'});
	});
	$(".afterLoginCloseBtn").click(function(){
		unlockPopup();
		$(".loggedinProfileWrapper").animate({right:'-280px'});
		loginSignupSwitch();
	});
	$(".blackOut-window").mouseup(function(e){
		var loggedIn = $(".loggedinProfileWrapper");
        if(e.target.id !== loggedIn.attr('id') && !loggedIn.has(e.target).length)
        {
            loggedIn.animate({'right':'-280px'});
			unlockPopup();
        }
    });

	//global city popup
	$("#header div.gl-default-stage").click( function(){
		$(".blackOut-window").show();
		$(".globalcity-popup").removeClass("hide").addClass("show");
		CheckGlobalCookie();
	});
	
	$(".blackOut-window").mouseup(function(e){
		var globalLocation = $("#globalcity-popup"); 
        if(e.target.id !== globalLocation.attr('id') && !globalLocation.has(e.target).length)		
        {
            CloseCityPopUp();
        }
	});

	$(".globalcity-close-btn").click(function(){
	    CloseCityPopUp();
	});
	
	$(document).keydown(function (e) {
		// ESCAPE key pressed
		if (e.keyCode == 27) {
			CloseCityPopUp();
			navbarHideOnESC();
			loginHideOnESC();
			popupHideOnESC();
		}
	});

	$('#btnGlobalCityPopup').on('click', function () {
	    ele = $('#globalCityPopUp');
	    if (globalCityId > 0 && ($(ele).val()) != "")
	    {
	        showHideMatchError(ele, false);
	        CloseCityPopUp();
	    }
	    else
	    {
	        showHideMatchError(ele, true); 
	    } 
	    return false;
	});

    // Common BW tabs code
	$(".more-filter-item-data .bw-tabs li").live('click', function () {
	    var panel = $(this).closest(".bw-tabs-panel");
	    if (!$(this).hasClass("active")) {
	        panel.find(".bw-tabs li").removeClass("active");
	        $(this).addClass("active");
	    }
	    else {
	        $(this).removeClass("active");
	    }
	});

	$(".bw-tabs li").live('click', function () {
	    var panel = $(this).closest(".bw-tabs-panel");
	    panel.find(".bw-tabs li").removeClass("active");
	    $(this).addClass("active");
	    var panelId = $(this).attr("data-tabs");
	    panel.find(".bw-tabs-data").hide();
	    $("#" + panelId).show();
	    applyTabsLazyLoad();
	});

	$(".jcarousel-wrapper .jcarousel-control-next").on("click", function () {
	    var jcaourselDiv = $(this).parent("span").prev("div");
	    jcaourselDiv.on('jcarousel:visiblein', 'li', function (event, carousel) {
	        $(this).find("img.lazy").trigger("imgLazyLoad");
	    });
	});
    // Contents Widegt Clicked
	
	$("#ctrlNews a").on("click", function () {
	    var category = GetCatForNav();
	    if (category != null) {
	        dataLayer.push({ 'event': 'Bikewale_all', 'cat': category, 'act': 'Content_Widget_Clicked', 'lab': 'News_Clicked' });
	    }
	});
	$("#ctrlExpertReviews a").on("click", function () {
	    var category = GetCatForNav();
	    if (category != null) {
	        dataLayer.push({ 'event': 'Bikewale_all', 'cat': category, 'act': 'Content_Widget_Clicked', 'lab': 'Reviews_Clicked' });
	    }
	});
	$("#ctrlVideos a").on("click", function () {
	    var category = GetCatForNav();
	    if (category != null) {
	        dataLayer.push({ 'event': 'Bikewale_all', 'cat': category, 'act': 'Content_Widget_Clicked', 'lab': 'Videos_Clicked' });
	    }
	});
    // Brands, mileage, styles clicked
	$(".brand-type-container li").on("click", function () {
	    var bikeClicked = $(this).find(".brand-type-title").html();
	    var category = GetCatForNav();
	    if (category != null) {
	        dataLayer.push({ 'event': 'Bikewale_all', 'cat': category, 'act': 'Discover_Your_Bike_Brand', 'lab': bikeClicked });
	    }
	});

	$("#discoverBudget li a").on("click", function () {
	    var budgetClick = $(this).text().replace(/\s/g, "");
	    var category = GetCatForNav();
	    if (category != null) {
	        dataLayer.push({ 'event': 'Bikewale_all', 'cat': category, 'act': 'Discover_Your_Bike_Budget', 'lab': budgetClick });
	    }
	});

	$("#discoverMileage li a").on("click", function () {
	    var mileageClick = $(this).text().replace(/\s/g, "");
	    var category = GetCatForNav();
	    if (category != null) {
	        dataLayer.push({ 'event': 'Bikewale_all', 'cat': category, 'act': 'Discover_Your_Bike_Mileage', 'lab': mileageClick });
	    }
	});

	$("#discoverStyle li a").on("click", function () {
	    var styleClicked = $(this).text().replace(/\s/g, "");
	    var category = GetCatForNav();
	    if (category != null) {
	        dataLayer.push({ 'event': 'Bikewale_all', 'cat': category, 'act': 'Discover_Your_Bike_Style', 'lab': styleClicked });
	    }
	});
	
    // Google Analytics code for Click of Item on Nav_Bar on HP
	$(".navUL ul li").on("click", function () {
	    pushNavMenuAnalytics($(this).text());
	});
	$(".navbarTitle").on("click", function () {
	    pushNavMenuAnalytics($(this).text());
	});

	$("#bwheader-logo").on("click", function () {
	    var categ = GetCatForNav();
	    if (categ != null) {
	        dataLayer.push({ 'event': 'Bikewale_all', 'cat': categ, 'act': 'Logo', 'lab': 'Logo_Clicked' });
	    }
	});
});

function GetCatForNav() {
    var ret_category = null;
    if (ga_pg_id != null && ga_pg_id != "0") {
        switch (ga_pg_id) {
            case "1":
                ret_category = "HP";
                break;
            case "2":
                ret_category = "Model_Page";
                break;
            case "3":
                ret_category = "Make_Page";
                break;
            case "4":
                ret_category = "New_Bikes_Page";
                break;
            case "5":
                ret_category = "Search_Page";
                break;
        }
        return ret_category;
    }
}

function GetGlobalCityArea() {
    var cookieName = "location";
    var cityArea = '';
    if (isCookieExists(cookieName)) {
        var arrays = getCookie(cookieName).split(",")[0].split("_");
        if (arrays.length > 2) {
            cityArea = arrays[1] + '_' + arrays[3];
        }
        else if (arrays.length) {
            cityArea = arrays[1];
        }
        return cityArea;
    }
}

$('#newBikeList').on('keypress', function (e) {
    var id = $('#newBikeList');
    var searchVal = id.val().trim();
    var placeHolder = id.attr('placeholder');
    if (e.keyCode == 13)
        if (btnFindBikeNewNav() || searchVal == placeHolder || searchVal == "") {
            $('#errNewBikeSearch').hide();
            return false;
        }
        else {
            return false;
        }
});

$('#btnSearch').on('click', function (e) {
    var id = $('#newBikeList');
    var searchVal = id.val();
    var placeHolder = id.attr('placeholder');
    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'HP', 'act': 'Search_Not_Keyword_Present_in_Autosuggest', 'lab': searchVal });
    if (btnFindBikeNewNav() || searchVal == placeHolder || (searchVal).trim() == "") {
        return false;
    } else {
        return false;
    }

});

var popup = {
    lock: function () {
        var htmlElement = $('html'), bodyElement = $('body');
        $(".blackOut-window").show();
        if ($(document).height() > $(window).height()) {
            var windowScrollTop = htmlElement.scrollTop() ? htmlElement.scrollTop() : bodyElement.scrollTop();
            if (windowScrollTop < 0) {
                windowScrollTop = 0;
            }
            htmlElement.addClass('lock-browser-scroll').css('top', -windowScrollTop);
        }
    },

    unlock: function () {
        var htmlElement = $('html'),
            windowScrollTop = parseInt(htmlElement.css('top'));
        $(".blackOut-window").hide();
        htmlElement.removeClass('lock-browser-scroll');
        $('html, body').scrollTop(-windowScrollTop);
    }
};

/* jCarousel custom methods */
var _target = 3;
$(function () {
    if (typeof (testimonialSlider) != 'undefined') {
        _target = 1;
    }
    var jcarousel = $('.jcarousel').jcarousel({
        vertical: false
    });
    $('.jcarousel-control-prev').on('jcarouselcontrol:active', function () {
        $(this).removeClass('inactive');
    }).on('jcarouselcontrol:inactive', function () {
        $(this).addClass('inactive');
    }).jcarouselControl({
        target: '-=' + _target
    });
    $('.jcarousel-control-next').on('jcarouselcontrol:active', function () {
        $(this).removeClass('inactive');
    }).on('jcarouselcontrol:inactive', function () {
        $(this).addClass('inactive');
    }).jcarouselControl({
        target: '+=' + _target
    });
    $('.jcarousel-pagination').on('jcarouselpagination:active', 'a', function () {
        $(this).addClass('active');
    }).on('jcarouselpagination:inactive', 'a', function () {
        $(this).removeClass('active');
    }).on('click', function (e) {
        e.preventDefault();
    }).jcarouselPagination({
        item: function (page) {
            return '<a href="#' + page + '">' + page + '</a>';
        }
    });
    // Swipe handlers for mobile
    $(".jcarousel").swipe({ fingers: 'all', swipeLeft: swipe1, swipeRight: swipe1, allowPageScroll: "auto" });
    function swipe1(event, direction, distance, duration, fingerCount) {
        if (direction == "left") {
            $(this).closest('.jcarousel-wrapper').find("a.jcarousel-control-next").click();
        }
        else if (direction == "right") {
            $(this).closest('.jcarousel-wrapper').find("a.jcarousel-control-prev").click();
        }
    }
    $(".jcarousel").on('jcarousel:visiblein', 'li', function (event, carousel) {
        $(this).find("img.lazy").trigger("imgLazyLoad");
    });
});

function applyTabsLazyLoad() {
    $("img.lazy").lazyload({
        event: "imgLazyLoad"
    });
}

function applyLazyLoad() {
    $("img.lazy").lazyload({
        event: "imgLazyLoad",
        effect: "fadeIn"
    });
}

// common autocomplete data call function
function dataListDisplay(availableTags, request, response) {
    var results = $.ui.autocomplete.filter(availableTags, request.term);
    response(results.slice(0, 5));
}

function setPriceQuoteFlag()
{
    IsPriceQuoteLinkClicked = true;
}


function MakeModelRedirection(items) {
    if (!IsPriceQuoteLinkClicked) {
        var make = new Object();
        make.maskingName = items.payload.makeMaskingName;
        make.id = items.payload.makeId;
        var model = null;
        if (items.payload.modelId > 0) {
            model = new Object();
            model.maskingName = items.payload.modelMaskingName;
            model.id = items.payload.modelId;
            model.futuristic = items.payload.futuristic;
        }

        if (model != null && model != undefined) {
            window.location.href = "/" + make.maskingName + "-bikes/" + model.maskingName + "/";
            return true;
        } else if (make != null && make != undefined) {
            window.location.href = "/" + make.maskingName + "-bikes/";
            return true;
        }
    }
}

function pushNavMenuAnalytics(menuItem) {
    var categ = GetCatForNav();
    if (categ != null) {
        dataLayer.push({ 'event': 'Bikewale_all', 'cat': categ, 'act': 'Hamburger_Menu_Item_Click', 'lab': menuItem });
    }
}

(function ($) {
    $.fn.hint = function (blurClass) {
        if (!blurClass) {
            blurClass = 'blur';
        }

        return this.each(function () {
            // get jQuery version of 'this'
            var $input = $(this),

            // capture the rest of the variable to allow for reuse
              title = $input.attr('placeholder'),
              $form = $(this.form),
              $win = $(window);

            function remove() {
                if ($input.val() === title && $input.hasClass(blurClass)) {
                    $input.val('').removeClass(blurClass);
                }
            }

            // only apply logic if the element has the attribute
            if (title) {
                // on blur, set value to title attr if text is blank
                $input.blur(function () {
                    if (this.value === '') {
                        $input.val(title).addClass(blurClass);
                    }
                }).focus(remove).blur(); // now change all inputs to title

                // clear the pre-defined text when form is submitted
                $form.submit(remove);
                $win.unload(remove); // handles Firefox's autocomplete
            }
        });
    };

})(jQuery);

(function ($) {
    $.fn.bw_autocomplete = function (options) {
        return this.each(function () {
            if (options == null || options == undefined) {
                return;
            }
            else if (options.source == null || options.source == undefined || options.source == '') {
                return;
            }
            var cache = new Object();
            var reqTerm;
            var orgTerm;
            if ($(this).attr('placeholder') != undefined)
                $(this).hint();
            var result;
            $(this).focusout(function () {
                if (options.focusout != undefined)
                    options.focusout();
            })
            $(this).keyup(function () {
                if (options.keyup != undefined)
                    options.keyup();
            })
            $(this).autocomplete({
                autoFocus: true,
                source: function (request, response) {
                    orgTerm = request.term;
                    reqTerm = request.term.replace(/^\s\s*/, '').replace(/\s\s*$/, '').replace(/-/g, ' ').replace(/[^A-Za-z0-9 ]/g, '').toLowerCase().trim();

                    var year = options.year;
                    if (year != null && year != undefined && year != '')
                        year = year.val();
                    else
                        year = '';
                    cacheProp = reqTerm + '_' + year;
                    if (!(cacheProp in cache) && reqTerm.length > 0) {
                        var indexToHit = options.source;
                        var count = options.recordCount;
                        
                        var path = "/api/AutoSuggest/?source=" + indexToHit + "&inputText=" + encodeURIComponent(reqTerm) + "&noofrecords=" + count;
                        cache[cacheProp] = new Array();
                        $.ajax({
                            async: true, type: "GET", contentType: "application/json; charset=utf-8", dataType: "json",
                            url: path,
                            beforeSend : function(xhr){
                                if (options.loaderStatus != null && typeof (options.loaderStatus) == "function") options.loaderStatus(false);
                            },
                            success: function (jsonData) {
                                jsonData = jsonData.suggestionList;
                                cache[reqTerm + '_' + year] = $.map(jsonData, function (item) {
                                    return { label: item.text, payload: item.payload }
                                });
                                result = cache[cacheProp];
                                response(cache[cacheProp]);
                                if (options.afterfetch != null && typeof (options.afterfetch) == "function") options.afterfetch(result, reqTerm);
                            },
                            error: function (error) {
                                result = undefined;
                                options.afterfetch(result, reqTerm);
                                response(cache[cacheProp]);
                            },
                            complete: function(xhr,status)
                            {
                                if (options.loaderStatus != null && typeof (options.loaderStatus) == "function") options.loaderStatus(true);
                            }
                        });
                    }
                    else {
                        result = cache[cacheProp];
                        response(cache[cacheProp]);
                        if (options.afterfetch != null && typeof (options.afterfetch) == "function") options.afterfetch(result, reqTerm);
                    }
                },
                minLength: 1,
                select: function (event, ui) {
                    if (options.click != undefined)
                        options.click(event, ui, $(this).val());
                },
                open: function (event, ui) {
                    if (!isNaN(options.width)) {
                        $('.ui-menu').width(options.width);
                    }
                    if (options.open != undefined)
                        options.open(result);
                }
            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                return createAutoSuggestLinkText(ul, item, reqTerm);
            };
            function __highlight(s, t) {
                var matcher = new RegExp("(" + $.ui.autocomplete.escapeRegex(t) + ")", "ig");
                return s.replace(matcher, "<strong>$1</strong>");
            }
            function __getValue(key, value) {
                if (value != null && value != undefined && value != '')
                    return key + ':' + value + ';';
                else
                    return '';
            }
            function createAutoSuggestLinkText(ul, item, reqTerm) {          
                var ulItem = $("<li>")
                              .data("ui-autocomplete-item", item)
                              .append('<a OptionName=' + item.label.replace(/\s/g, '').toLowerCase() + '>' + __highlight(item.label, reqTerm) + '</a>');

                if (options.source == '1') {
                    if (item.payload.modelId > 0) {
                        if (item.payload.futuristic == 'True') {
                            ulItem.append('<span class="upcoming-link">coming soon</span>')
                        } else {
                            if (item.payload.isNew == 'True') {
                                ulItem.append('<a pqSourceId="' + pqSourceId + '" modelId="' + item.payload.modelId + '" class="fillPopupData target-popup-link" onclick="setPriceQuoteFlag()">Check On-Road Price</a>');
                            } else {
                                ulItem.append('<span class="upcoming-link">discontinued</span>')
                            }

                        }

                        ulItem.append('<div class="clear"></div>');
                    }
                }
                ulItem.appendTo(ul);
                return ulItem;
            }
            $(this).keyup(function (e) {
                if ($(this).val().replace(/\s/g, '').length == 0 && options.onClear != undefined) {
                    options.onClear();
                }
            });
            $(this).keypress(function (e) {
                if (e.keyCode == 13) {
                    e.preventDefault();
                }
            });
        });
    };
})(jQuery);

function SetCookie(cookieName, cookieValue) {    
    document.cookie = cookieName + "=" + cookieValue + '; path =/';
}

function SetCookie(cookieName, cookieValue) {
    document.cookie = cookieName + "=" + cookieValue + '; path =/';
}

function SetCookieInDays(cookieName, cookieValue, nDays) {
    var today = new Date();
    var expire = new Date();
    expire.setTime(today.getTime() + 3600000 * 24 * nDays);
    document.cookie = cookieName + "=" + cookieValue + ";expires=" + expire.toGMTString() + '; path =/';
}

function getCookie(key) {
    var keyValue = document.cookie.match('(^|;) ?' + key + '=([^;]*)(;|$)');
    return keyValue ? keyValue[2] : null;
}

function isCookieExists(cookiename) {
    var coockieVal = $.cookie(cookiename);
    if (coockieVal == undefined || coockieVal == null || coockieVal == "-1" || coockieVal == "")
        return false;
    return true;
}

function toggleErrorMsg(element, error, msg) {    
    if (error) {
        element.parent().find('.error-icon').removeClass('hide');
        element.parent().find('.bw-blackbg-tooltip').text(msg).removeClass('hide');        
        element.addClass('border-red')
    }
    else {
        element.parent().find('.error-icon').addClass('hide');
        element.parent().find('.bw-blackbg-tooltip').text("").addClass('hide');
        element.removeClass('border-red');
    }
}

function showHideMatchError(element, error) {    
    if (error) {
        element.parent().find('.error-icon').removeClass('hide');
        element.parent().find('.bw-blackbg-tooltip').removeClass('hide');
        element.addClass('border-red')
    }
    else {
        element.parent().find('.error-icon').addClass('hide');
        element.parent().find('.bw-blackbg-tooltip').addClass('hide');
        element.removeClass('border-red');
    }
}

function showGlobalCity(cityName) {
    $(".gl-default-stage").show();
    $('#cityName').text(cityName);
}

function CloseCityPopUp() {
    var globalLocation = $("#globalcity-popup");
    globalLocation.removeClass("show").addClass("hide");
    unlockPopup();
}

function CheckGlobalCookie()
{
    var cookieName = "location";
    if(isCookieExists(cookieName))
    {
        var locationCookie = getCookie(cookieName).split("_");
        var cityName = locationCookie[1];
        globalCityId = parseInt(locationCookie[0]);
        showGlobalCity(cityName);
        showHideMatchError($("#globalCityPopUp"), false);
        $("#globalCityPopUp").val(cityName);
    }
}

//function to attach ajax spinner
function attachAjaxLoader(element)
{
    var $loading = $(element).hide();
    $(document)
      .ajaxStart(function () {
          $loading.show();
      })
      .ajaxStop(function () {
          $loading.hide();
      });
}

//set location cookie
function setLocationCookie(cityEle, areaEle) {
    if (parseInt($(cityEle).val()) > 0) {
        cookieValue = parseInt($(cityEle).val()) + "_" + $(cityEle).text();
        if (parseInt($(areaEle).val()) > 0)
            cookieValue += "_" + parseInt($(areaEle).val()) + "_" + $(areaEle).text();
        SetCookieInDays("location", cookieValue, 365);
    }
}

//match cookie data to check city /area exists 
function selectElementFromArray(arr, id) {
    if (arr != null && (l = arr.length) > 0) {
        for (var i = 0; i < l; i++) {
            if (arr[i].cityId === id || arr[i].AreaId === id || arr[i].areaId === id || arr[i].CityId === id || arr[i].id === id)
                return true;
        }
    }
    return false;
}

$(".modelurl").click(function () {
    var array = $(this).attr('href').split('/');
    if (array.length > 2) {
        dataLayer.push({
            'event': 'Bikewale_all', 'cat': 'Make_Page', 'act': 'Model_Click', 'lab':_makeName +'_'+ array[2]
        });
    }
});

$('.dealer-details-main-content').on('click', function () {
    $(this).hide();
    $(this).next('.dealer-details-more-content').show();
});

function insertCitySeparator(response) {
    l = (response != null) ? response.length : 0;
    if (l > 0) {
        for (i = 0; i < l; i++) {
            if (!response[i].IsPopular) {
                if (i > 0)
                    response.splice(i, 0, { CityId: 0, CityName: "--------------------", CityMaskingName: "", IsPopular: false });
                break;
            }
        }
    }
}

function insertCitySeparatorNew(response) {
    l = (response != null) ? response.length : 0;
    if (l > 0) {
        for (i = 0; i < l; i++) {
            if (!response[i].IsPopular) {
                if (i > 0)
                    response.splice(i, 0, { Id: 0, Name: "--------------------", IsPopular: false,hasAreas :false });
                break;
            }
        }
    }
}

function btnFindBikeNewNav() {
    if (focusedMakeModel == undefined || focusedMakeModel == null) {
        return false;
    }
    return MakeModelRedirection(focusedMakeModel);
}

function navbarHide() {
    $("#nav").removeClass('open').animate({ 'left': '-350px' });
    $(".blackOut-window").hide();
}

function navbarHideOnESC() {
    $("#nav").removeClass('open').animate({ 'left': '-350px' });
    $(".blackOut-window").hide();
}

function navbarShow() {
    var category = GetCatForNav();
    if (category != null) {
        dataLayer.push({ 'event': 'Bikewale_all', 'cat': category, 'act': 'Hamburger_Menu_Icon', 'lab': 'Icon_Click' });
    }
    $("#nav").addClass('open').animate({ 'left': '0px' });
    $(".blackOut-window").show();
}
function loginSignupSwitch() {
    $(".loginStage").show();
    $(".signUpStage").hide();
}

$('#btnGlobalSearch').on('click', function () {
    if (focusedMakeModel != null && btnGlobalSearch != undefined)
        btnFindBikeNewNav();

});

$("#newBikeList").bw_autocomplete({
    width: 469,
    source: 1,
    recordCount: 10,
    onClear: function () {
        objBikes = new Object();
    },
    click: function (event, ui, orgTxt) {
        MakeModelRedirection(ui.item);
        var keywrd = ui.item.label + '_' + $('#newBikeList').val();
        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'HP', 'act': 'Search_Keyword_Present_in_Autosuggest', 'lab': keywrd });
    },
    open: function (result) {
        objBikes.result = result;
    },
    focusout: function () {
        if ($('li.ui-state-focus a:visible').text() != "") {
            $('#errNewBikeSearch').hide()
            focusedMakeModel = new Object();
            focusedMakeModel = objBikes.result[$('li.ui-state-focus').index()];
        }
        else {
            $('#errNewBikeSearch').hide()
        }
    },
    afterfetch: function (result, searchtext) {
        if (result != undefined && result.length > 0 && searchtext.trim() != "")
        {
            $('#errNewBikeSearch').hide()
            NewBikeSearchResult = true;
        }
        else {
            focusedMakeModel = null; NewBikeSearchResult = false;
            if (searchtext.trim() != "")
                $('#errNewBikeSearch').show()
        }
    },
    keyup: function () {
        if ($('li.ui-state-focus a:visible').text() != "") {
            focusedMakeModel = new Object();
            focusedMakeModel = objBikes.result[$('li.ui-state-focus').index()];
            $('#errNewBikeSearch').hide();
        } else {
            if ($('#newBikeList').val().trim() == '') {
                $('#errNewBikeSearch').hide();
            } 
        }

        if ($('#newBikeList').val().trim() == '' || e.keyCode == 27 || e.keyCode == 13) {
            if (focusedMakeModel == null || focusedMakeModel == undefined)
                if ($('#newBikeList').val().trim() != '')
                    $('#errNewBikeSearch').show();
            else
                $('#errNewBikeSearch').hide();
        }
    }
});


$("#globalSearch").bw_autocomplete({
    width: 420,
    source: 1,
    recordCount: 10,
    onClear: function () {
        objBikes = new Object();
    },
    click: function (event, ui, orgTxt) {
        var keywrd = ui.item.label + '_' + $('#globalSearch').val();
        var category = GetCatForNav();
        dataLayer.push({ 'event': 'Bikewale_all', 'cat': category, 'act': 'Search_Keyword_Present_in_Autosuggest', 'lab': keywrd });
        MakeModelRedirection(ui.item);
    },
    loaderStatus : function(status)
    {
        if(!status)
        {
            $("#btnGlobalSearch").removeClass('bwsprite');
            $("#globalSearch").siblings('.fa-spinner').show();
            if (focusedMakeModel == null) $('#errGlobalSearch').hide();
        }
        else {
            $("#btnGlobalSearch").addClass('bwsprite');
            $("#globalSearch").siblings('.fa-spinner').hide();
        }
    },
    open: function (result) {
        objBikes.result = result;
    },
    focusout: function () {
        if ($('li.ui-state-focus a:visible').text() != "") {
            focusedMakeModel = new Object();
            focusedMakeModel = objBikes.result[$('li.ui-state-focus').index()];
        }
        else {
            $('#errGlobalSearch').hide();
        }
    },
    afterfetch: function (result, searchtext) {

        if (result != undefined && result.length > 0 && searchtext.trim() != '') {
            $('#errGlobalSearch').hide();
            globalSearchResult = true;
        }
        else {
            focusedMakeModel = null; globalSearchResult = false;
            if (searchtext.trim() != '')
                $('#errGlobalSearch').show();
            var keywrd = $('#globalSearch').val();
            var category = GetCatForNav();
            dataLayer.push({ 'event': 'Bikewale_all', 'cat': category, 'act': 'Search_Keyword_Not_Present_in_Autosuggest', 'lab': keywrd });
        }
    },
    keyup: function () {
    if ($('li.ui-state-focus a:visible').text() != "") {
        focusedMakeModel = new Object();
        focusedMakeModel = objBikes.result[$('li.ui-state-focus').index()];
        $('#errGlobalSearch').hide();
    } else {
        if ($('#globalSearch').val().trim() == '') {
            $('#errGlobalSearch').hide();
        } 
    }

    if ($('#globalSearch').val().trim() == '' || e.keyCode == 27 || e.keyCode == 13) {
        if (focusedMakeModel == null || focusedMakeModel == undefined) {
            if ($('#globalSearch').val().trim() != '')
                $('#errGlobalSearch').show();
        }
        else
            $('#errGlobalSearch').hide();
    }
}
}).keydown(function (e) {
    if (e.keyCode == 13)
    {
        $('#btnGlobalSearch').click();
    }
        
});

function CloseCityPopUp() {
    var globalLocation = $("#globalcity-popup");
    globalLocation.removeClass("show").addClass("hide");
    unlockPopup();
    if (!isCookieExists("location"))
        SetCookieInDays("location", "0", 365);
}

function popupHideOnESC() {
    $('.bw-popup').fadeOut(100);
    unlockPopup();
}

function headerOnScroll() {
    if ($(window).scrollTop() > 40) {
        $('#header').addClass('header-fixed-with-bg');
    } else {
        $('#header').removeClass('header-fixed-with-bg');
    }
}

function loginHideOnESC() {
    $(".loginPopUpWrapper").animate({ right: '-400px' });
    $(".loggedinProfileWrapper").animate({
        right: '-280px'
    });
}

function lockPopup() {
    $('body').addClass('lock-browser-scroll');
    $(".blackOut-window").show();
}

function unlockPopup() {
    $('body').removeClass('lock-browser-scroll');
    $(".blackOut-window").hide();
}

$.fn.shake = function (options) {
    var settings = {
        'shakes': 2,
        'distance': 10,
        'duration': 400
    };
    if (options) {
        $.extend(settings, options);
    }
    var pos;
    return this.each(function () {
        $this = $(this);
        pos = $this.css('position');
        if (!pos || pos === 'static') {
            $this.css('position', 'relative');
        }
        for (var x = 1; x <= settings.shakes; x++) {
            $this.animate({ left: settings.distance * -1 }, (settings.duration / settings.shakes) / 4)
                .animate({ left: settings.distance }, (settings.duration / settings.shakes) / 2)
                .animate({ left: 0 }, (settings.duration / settings.shakes) / 4);
        }
    });
};

var Base64 = {
    _keyStr: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=",
    encode: function (input) {
        var output = "";
        var chr1, chr2, chr3, enc1, enc2, enc3, enc4;
        var i = 0;

        input = Base64._utf8_encode(input);

        while (i < input.length) {

            chr1 = input.charCodeAt(i++);
            chr2 = input.charCodeAt(i++);
            chr3 = input.charCodeAt(i++);

            enc1 = chr1 >> 2;
            enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
            enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
            enc4 = chr3 & 63;

            if (isNaN(chr2)) {
                enc3 = enc4 = 64;
            } else if (isNaN(chr3)) {
                enc4 = 64;
            }

            output = output + this._keyStr.charAt(enc1) + this._keyStr.charAt(enc2) + this._keyStr.charAt(enc3) + this._keyStr.charAt(enc4);

        }

        return output;
    },
    decode: function (input) {
        var output = "";
        var chr1, chr2, chr3;
        var enc1, enc2, enc3, enc4;
        var i = 0;

        input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");

        while (i < input.length) {

            enc1 = this._keyStr.indexOf(input.charAt(i++));
            enc2 = this._keyStr.indexOf(input.charAt(i++));
            enc3 = this._keyStr.indexOf(input.charAt(i++));
            enc4 = this._keyStr.indexOf(input.charAt(i++));

            chr1 = (enc1 << 2) | (enc2 >> 4);
            chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
            chr3 = ((enc3 & 3) << 6) | enc4;

            output = output + String.fromCharCode(chr1);

            if (enc3 != 64) {
                output = output + String.fromCharCode(chr2);
            }
            if (enc4 != 64) {
                output = output + String.fromCharCode(chr3);
            }

        }

        output = Base64._utf8_decode(output);

        return output;

    },
    _utf8_encode: function (string) {
        string = string.replace(/\r\n/g, "\n");
        var utftext = "";

        for (var n = 0; n < string.length; n++) {

            var c = string.charCodeAt(n);

            if (c < 128) {
                utftext += String.fromCharCode(c);
            }
            else if ((c > 127) && (c < 2048)) {
                utftext += String.fromCharCode((c >> 6) | 192);
                utftext += String.fromCharCode((c & 63) | 128);
            }
            else {
                utftext += String.fromCharCode((c >> 12) | 224);
                utftext += String.fromCharCode(((c >> 6) & 63) | 128);
                utftext += String.fromCharCode((c & 63) | 128);
            }

        }

        return utftext;
    },
    _utf8_decode: function (utftext) {
        var string = "";
        var i = 0;
        var c = c1 = c2 = 0;

        while (i < utftext.length) {

            c = utftext.charCodeAt(i);

            if (c < 128) {
                string += String.fromCharCode(c);
                i++;
            }
            else if ((c > 191) && (c < 224)) {
                c2 = utftext.charCodeAt(i + 1);
                string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
                i += 2;
            }
            else {
                c2 = utftext.charCodeAt(i + 1);
                c3 = utftext.charCodeAt(i + 2);
                string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
                i += 3;
            }

        }

        return string;
    }
}

function formatPrice(x) { try { x = x.toString(); var lastThree = x.substring(x.length - 3); var otherNumbers = x.substring(0, x.length - 3); if (otherNumbers != '') lastThree = ',' + lastThree; var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree; return res; } catch (err) { } }

ko.bindingHandlers.formateDate = {
    update: function (element, valueAccessor) {
        var date = new Date(valueAccessor());
        var formattedDate = monthNames[date.getUTCMonth()] + ' ' + date.getUTCDate() + ', ' + date.getUTCFullYear();
        $(element).text(formattedDate);
    }
};

(function () {

    $(document).ajaxError(function (event, request, settings) { 
        try {
            error = {};
            error.ErrorType = event.type || "";
            error.SourceFile = (event.target) ? event.target.URL || "" : "";
            error.Trace = JSON.stringify({
                "API": settings.url || "",
                "Error Occured": request.status || "" + request.statusText || "",
                "Response Text": request.responseText || ""
            });
            error.Message = "Ajax Error Occured";
            //errorLog(error);
        } catch (e) {
            return false;
        }
    });


    'use strict';
    var errorLog = function (error) {
        try {
            if (error) {
                $.ajax({ type:"POST",url:"/api/JSException/",data:error,
                    error : function(event,request,settings)
                    {
                       // request.abort();
                        return false;
                    }
                });
            }
        } catch (e) {
            return false;
        }
    }

    window.onerror = function (message, filename, lineno, colno, err) {
        error = {};
        try {
            error.Message = err.message || message || "";
            error.SourceFile = err.fileName || filename || "";
            error.ErrorType = err.name || "Uncatched Exception";
            error.LineNo = lineno || "Unable to trace";
            error.Trace = (err.stack.toString() || '-');
            //errorLog(error);
        } catch (e) {
            return false;
        }
    };

    window.errorLog = errorLog;
})();

