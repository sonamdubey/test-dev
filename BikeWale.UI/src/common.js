// JavaScript Document
var focusedMakeModel = null, focusedCity = null, isMakeModelRedirected = false;
var objBikes = new Object(), objCity = new Object(), globalCityId = 0, _makeName = '', ga_pg_id = '0', pqSourceId = "37";
var IsPriceQuoteLinkClicked = false, _target = 3, popup, recentSearches;
var monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"];
var bw_ObjContest;

var topCount = 5;
var trendingBikes, objSearches;
var pageName = typeof (gaObj) === 'undefined' ? 'Others' : gaObj.name;

/* landing page header */
var transparentHeader = document.querySelectorAll('.header-transparent')[0];

if (transparentHeader) {
    attachListener('scroll', window, changeHeaderBackground);
}



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

if (typeof String.prototype.contains === 'undefined') {
    String.prototype.contains = function (t, c) {
        return (!c) ? this.indexOf() != -1 : this.toLowerCase().indexOf(t.toLowerCase()) != -1;
    };
}


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
};

function triggerGA(cat, act, lab) {
    try {
        dataLayer.push({ 'event': 'Bikewale_all', 'cat': cat, 'act': act, 'lab': lab });
    }
    catch (e) {// log error   
    }
};
function triggerNonInteractiveGA(cat, act, lab) {
    try {

        dataLayer.push({ 'event': 'Bikewale_noninteraction', 'cat': cat, 'act': act, 'lab': lab });
    }
    catch (e) {// log error   
    }
}
function attachListener(event, element, functionName) {
    if (element.addEventListener) {
        element.addEventListener(event, functionName, false);
    }
    else if (element.attachEvent) {
        element.attachEvent('on' + event, functionName);
    }
};

function changeHeaderBackground() {
    if ($(window).scrollTop() > 40)
        $('.header-transparent').removeClass('header-landing').addClass('header-fixed');
    else
        $('.header-transparent').removeClass('header-fixed').addClass('header-landing');
};

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
            case "6":
                ret_category = "DealerPriceQuote_Page";
                break;
            case "7":
                ret_category = "Dealer_Locator_Page";
                break;
            case "8":
                ret_category = "Dealer_Locator_Detail_Page";
                break;
            case "9":
                ret_category = "Booking_Page";
                break;
        }
        return ret_category;
    }
}

function GetGlobalCityArea() {
    var cookieName = "location";
    var cityArea = '';
    if (isCookieExists(cookieName)) {
        cityArea = getCookie(cookieName);
        cityArea = cityArea.replace(/[0-9](_)*/g, '').replace(/-+/g, ' ');
    }
    return cityArea;
}

function GetGlobalLocationObject() {
    var locationObj = {};
    if (isCookieExists("location")) {
        var locationCookie = getCookie("location");
        var cData = locationCookie.split('_');
        if (cData.length > 0) {
            locationObj.CityId = parseInt(cData[0]);
            locationObj.CityName = cData[1];

            locationObj.AreaId = cData[2] == null ? 0 : parseInt(cData[2]);
            locationObj.AreaName = cData[3] == null ? '' : cData[3];
        }
    }
    return locationObj;
}

function applyTabsLazyLoad() {
    $("img.lazy").lazyload({
        event: "imgLazyLoad"
    });
    $("div.lazy").lazyload({
        event: "divLazyLoad"
    });
}

function applyLazyLoad() {
    $("img.lazy").lazyload({
        event: "imgLazyLoad",
        effect: "fadeIn"
    });

    $("div.lazy").lazyload({
        event: "divLazyLoad",
        effect: "fadeIn"
    });
}


// common autocomplete data call function
function dataListDisplay(availableTags, request, response) {
    var results = $.ui.autocomplete.filter(availableTags, request.term);
    response(results.slice(0, 5));
}

function setPriceQuoteFlag() {
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
        triggerGA(categ, 'Hamburger_Menu_Item_Click', menuItem);
    }
}

var getHost = function () {
    var host = document.domain;

    if (host.match("bikewale.com$"))
        host = ".bikewale.com";
    return host;
}

function SetCookie(cookieName, cookieValue) {
    if (/MSIE (\d+\.\d+);/.test(navigator.userAgent) || /Trident\//.test(navigator.userAgent))
    { document.cookie = cookieName + "=" + cookieValue + '; path =/'; }
    else
    { document.cookie = cookieName + "=" + cookieValue + ';domain=' + getHost() + '; path =/'; }
}

function SetCookieInDays(cookieName, cookieValue, nDays) {
    var today = new Date();
    var expire = new Date();
    expire.setTime(today.getTime() + 3600000 * 24 * nDays);
    cookieValue = cookieValue.replace(/\s+/g, '-');
    if (/MSIE (\d+\.\d+);/.test(navigator.userAgent) || /Trident\//.test(navigator.userAgent))
    { document.cookie = cookieName + "=" + cookieValue + ";expires=" + expire.toGMTString() + '; path =/'; }
    else
    { document.cookie = cookieName + "=" + cookieValue + ";expires=" + expire.toGMTString() + ';domain=' + getHost() + '; path =/'; }

    bwcache.remove("userchangedlocation", true);
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

function triggerVirtualPageView(url, title) {
    try {
        dataLayer.push({
            'event': 'VirtualPageview',
            'virtualPageURL': url,
            'virtualPageTitle': title
        })
    } catch (e) {

    }
}

function CheckGlobalCookie() {
    var cookieName = "location";
    if (isCookieExists(cookieName)) {
        var locationCookie = getCookie(cookieName);
        locationCookie = (locationCookie.replace('-', ' ')).split("_");
        var cityName = locationCookie[1];
        globalCityId = parseInt(locationCookie[0]);
        showGlobalCity(cityName);
        showHideMatchError($("#globalCityPopUp"), false);
        $("#globalCityPopUp").val(cityName);
    }
}

//function to attach ajax spinner
function attachAjaxLoader(element) {
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
                    response.splice(i, 0, { Id: 0, Name: "--------------------", IsPopular: false, hasAreas: false });
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
        triggerGA(category, 'Hamburger_Menu_Icon', 'Icon_Click');
    }
    $("#nav").addClass('open').animate({ 'left': '0px' });
    $(".blackOut-window").show();
}

function loginSignupSwitch() {
    $(".loginStage").show();
    $(".signUpStage").hide();
}

function CloseCityPopUp() {
    var globalLocation = $("#globalcity-popup");
    globalLocation.removeClass("show").addClass("hide");
    unlockPopup();
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

function formatPrice(x) {
    try { x = x.toString(); var lastThree = x.substring(x.length - 3); var otherNumbers = x.substring(0, x.length - 3); if (otherNumbers != '') lastThree = ',' + lastThree; var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree; return res; } catch (err) { }
}

var bwAutoComplete = function (options) {
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
        $(this).focus(function () {
            if (options.focus != undefined)
                options.focus();
        })
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
                        beforeSend: function (xhr) {
                            if (options.loaderStatus != null && typeof (options.loaderStatus) == "function") options.loaderStatus(false);
                        },
                        success: function (jsonData) {
                            jsonData = jsonData.suggestionList;

                            if (options.source == '7' && $('#nonUpcomingBikes').attr('data-contentTab') == "expertReview")
                            {
                                var arr = new Array();
                                for (i = 0; i < jsonData.length; i++) {
                                    if (jsonData[i].payload.expertReviewsCount > 0)
                                        arr.push(jsonData[i])
                                }
                                for (i = 0; i < jsonData.length; i++) {
                                    if (jsonData[i].payload.expertReviewsCount == 0)
                                        arr.push(jsonData[i])
                                }
                                jsonData = arr;
                            }                            

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
                        complete: function (xhr, status) {
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
                          .append('<span class="bwsprite ui-search-icon"></span><a OptionName=' + item.label.replace(/\s/g, '').toLowerCase() + '>' + __highlight(item.label, reqTerm) + '</a>');

            if (options.source == '5')
            {
                ulItem.append(' <span class="rightfloat margin-left10 font14">(' + item.payload.userRatingsCount + ' Ratings)</span>')
            }
            if (options.source == '7' && $('#nonUpcomingBikes').attr('data-contentTab') == "userReview" && parseInt(item.payload.userRatingsCount) > 0) {
                ulItem.append(' <span class="rightfloat margin-left10 font14">(' + item.payload.userRatingsCount + ' Ratings)</span>')
            }
            else if (options.source == '7' && $('#nonUpcomingBikes').attr('data-contentTab') == "expertReview" && parseInt(item.payload.expertReviewsCount) > 0) {
                ulItem.append(' <a href="javascript:void(0)"  class="target-popup-link" rel="nofollow">Read review</a>')
            }
            else if (options.source == '7') {
                ulItem.closest('li').addClass('event-none');
                ulItem.append(' <span class="rightfloat margin-left10 font14 text-grey">Not reviewed yet</span>')
            }
            if (options.source == '1') {
                if (item.payload.modelId > 0) {
                    if (item.payload.futuristic == 'True') {
                        ulItem.append('<span class="upcoming-link">coming soon</span>')
                    } else {
                        if (item.payload.isNew == 'True') {
                            ulItem.append('<a href="javascript:void(0)" data-pqSourceId="' + pqSourceId + '" data-modelId="' + item.payload.modelId + '" class="getquotation target-popup-link" onclick="setPriceQuoteFlag()">Check On-Road Price</a>');
                        } else {
                            ulItem.append('<span class="upcoming-link">discontinued</span>')
                        }

                    }
                }
			}
			ulItem.append('<div class="clear"></div>');
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

var bwHint = function (blurClass) {
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

/* jCarousel custom methods */
docReady(function () {
    if (typeof (testimonialSlider) != 'undefined') {
        _target = 1;
    }
    if (typeof (slideCountOne) != 'undefined') {
        _target = 1;
    }
    if (typeof (slideCountTwo) != 'undefined') {
        _target = 2;
    }
    
    var jcarousel = $('.jcarousel').jcarousel({
        vertical: false
    });

    $('.jcarousel').each(function () {
        var dataSwipe = $(this).attr('data-swipe');
        var carouselWrapper = $(this).closest('.jcarousel-wrapper');
        carouselWrapper.find('.jcarousel-control-next').on('jcarouselcontrol:active', function () {
            $(this).removeClass('inactive');
        }).on('jcarouselcontrol:inactive', function () {
            $(this).addClass('inactive');
        }).jcarouselControl(
            {
                target: (dataSwipe ? '+=' + dataSwipe : '+=' + _target)

            });

        carouselWrapper.find('.jcarousel-control-prev').on('jcarouselcontrol:active', function () {
            $(this).removeClass('inactive');
        }).on('jcarouselcontrol:inactive', function () {
            $(this).addClass('inactive');
        }).jcarouselControl(
    {
        target: (dataSwipe ? '-=' + dataSwipe : '-=' + _target)
    }
    );

    })
    
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
        $(this).find("div.lazy").trigger("divLazyLoad");
    });

    if ($(window).width() < 996 && $(window).width() > 790)
        $("#bg-footer .grid-6").addClass("padding-left30 padding-right30");

    $('.jcarousel').jcarousel({
        animation: {
			duration: 400,
			easing: 'linear',
            complete: function () {
            }
        }
    });

});

docReady(function () {
    trendingBikes = JSON.parse(localStorage.getItem("bwc_trendingbikes", trendingBikes) || null);
    if (!trendingBikes) {
        $.ajax({
            type: "GET",
            url: "/api/popularbikes/?topCount=" + topCount,
            dataType: 'json',
            success: function (response) {
                if (response != null) {
                    trendingBikes = response;
                    localStorage.setItem("bwc_trendingbikes", JSON.stringify(trendingBikes));
                }
            }
        });
    }
    if (!bw_ObjContest) {
        bw_ObjContest = {}
        bw_ObjContest.visited = [];
        bw_ObjContest.visible = true;
        bw_ObjContest.count = 0;

    }
    var contestSlug = function () {
        var self = this;
        // function for bike contest 
       self.init= function () {
            try {
                var url = window.location.pathname;
                if (bw_ObjContest) {
                    if (bw_ObjContest.count < 3) {

                        if (bw_ObjContest.visited.indexOf(url) == -1) {
                            bw_ObjContest.visited.push(url);
                            bw_ObjContest.count++;
                        }

                        bwcache.set("showContestSlug", bw_ObjContest, true); 
                    }
                    if (bw_ObjContest.visible && bw_ObjContest.count >= 3) {
                        if (!document.getElementsByTagName("BODY")[0].getAttribute("data-contestslug")) {
                            var hrefStr = Base64.encode("csrc=12");
                            $('#bg-footer').before("<div id='contestSlideInSlug' class='review-contest-slidein-slug'><span id='contestSlideInCloseBtn' class='bwsprite slidein-slug__close-icon'></span><span class='slidein-slug__trophy-icon'></span><a href='/bike-review-contest/?q=" + hrefStr + "'class='slidein-slug__target bw-ga' c='Other' a='Contest_Slug_Clicked_Participate_CTA' l=''><span class='slidein-slug__target-title'>Bike Review Contest</span><span class='btn slidein-slug__target-btn'>Win &#x20B9;" + $("body").data('pricemoney') + "<span class='bwsprite slidein-slug__btn-arrow'></span></span></a></div>")
                            triggerGA("Other", "Contest_Slug_Appeared", "");
                        }
                     

                    }
                }
            } catch (e) {

            }
        };

    }
    var vmContestSlug = new contestSlug();

    (vmContestSlug.init());
    
    // review contest slide-in slug
    var contestSlideInSlug = $('#contestSlideInSlug'),
		contestSlugEndPoint;

    if (contestSlideInSlug.length > 0) {
		contestSlugEndPoint = ($(window).height() * 30) / 100; // 30 percent of window height
        attachListener('scroll', window, positionContestSlug);
        contestSlideInSlug.addClass('slidein-slug--visible');
    }

    function positionContestSlug() {
        if ($(window).scrollTop() > 600) {
            if (!contestSlideInSlug.hasClass('slug--position-absolute')) {
                contestSlideInSlug.addClass('slug--position-absolute');
                contestSlideInSlug.css({
                    'top': 600 + contestSlugEndPoint
                });
            }
        }
        else {
            if (contestSlideInSlug.hasClass('slug--position-absolute')) {
                contestSlideInSlug.removeClass('slug--position-absolute');
                contestSlideInSlug.css({
                    'top': '30%'
                });
            }
        }
    }

    $('#contestSlideInCloseBtn').on('click', function () {
        contestSlideInSlug.remove();
        bw_ObjContest.visible = false;
        bwcache.set("showContestSlug", bw_ObjContest, true);
        triggerGA("Other", "Contest_Slug_Clicked_On_Cross", "")
    });


    $.fn.hint = bwHint;

    $.fn.bw_autocomplete = bwAutoComplete;

    /* bikes search starts autocomplete */
    $('#btnGlobalSearch').on('click', function () {
        if (focusedMakeModel != null && btnGlobalSearch != undefined)
            btnFindBikeNewNav();

    });

    /*
        default width is 469,
        with campaign banner width is 415
    */
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
            isMakeModelRedirected = true;
        },
        open: function (result) {
            objBikes.result = result;
        },
        focus: function () {
            if ($('#newBikeList').val().trim() == '') {
                $('#errNewBikeSearch').hide();
                recentSearches.showRecentSearches();
                // showRecentSearches captures recentSearchesLoaded if any searchdata avaliable in local Storage
                var label = "Recently_Viewed_Bikes_" + (recentSearches.options.recentSearchesLoaded ? "Present" : "Not_Present");
                triggerGA('HP', 'Search_Bar_Clicked', label );
            }

        },
        focusout: function () {
            if ($('#newBikeList').find('li.ui-state-focus a:visible').text() != "") {
                focusedMakeModel = new Object();
                focusedMakeModel = objBikes.result ? objBikes.result[$('li.ui-state-focus').index()] : null;
            }
            else {
                $('#errNewBikeSearch').hide();
                var container = $('#new-global-search-section');
                if (container.is(':visible')) {
                    if (!container.is(event.relatedTarget) && container.has(event.relatedTarget).length === 0) {
                        recentSearches.hideRecentSearches();
                    }
                }
            }
        },
        afterfetch: function (result, searchtext) {
            if (result != undefined && result.length > 0 && searchtext.trim() != "") {
                $('#errNewBikeSearch').hide();
                recentSearches.hideRecentSearches();
                NewBikeSearchResult = true;
            }
            else {
                focusedMakeModel = null; NewBikeSearchResult = false;
                if (searchtext.trim() != "") {
                    $('#errNewBikeSearch').show();
                    recentSearches.hideRecentSearches();
                }
            }
        },
        keyup: function () {
            if ($('#newBikeList').val().trim() != '' && $('li.ui-state-focus a:visible').text() != "") {
                focusedMakeModel = new Object();
                focusedMakeModel = objBikes.result ? objBikes.result[$('li.ui-state-focus').index()] : null;
                $('#errNewBikeSearch').hide();
                recentSearches.hideRecentSearches();
            } else {
                if ($('#newBikeList').val().trim() == '') {
                    $('#errNewBikeSearch').hide();
                    recentSearches.showRecentSearches();
                }
            }

            if ($('#newBikeList').val().trim() == '' || e.keyCode == 27 || e.keyCode == 13) {
                if (focusedMakeModel == null || focusedMakeModel == undefined) {
                    if ($('#newBikeList').val().trim() != '') {
                        $('#errNewBikeSearch').show();
                    }
                }
                else {
                    $('#errNewBikeSearch').hide();
                    recentSearches.showRecentSearches();
                }

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
            isMakeModelRedirected = true;

        },
        loaderStatus: function (status) {
            if (!status) {
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
        focus: function () {
            if ($('#globalSearch').val().trim() == '') {
                $('#errGlobalSearch').hide();
                recentSearches.showRecentSearches();
                // showRecentSearches captures recentSearchesLoaded if any searchdata avaliable in local Storage
                var label = "Recently_Viewed_Bikes_" + (recentSearches.options.recentSearchesLoaded ? "Present" : "Not_Present");
                triggerGA(pageName, 'Search_Bar_Clicked', label);
            }
        },
        focusout: function () {
            if ($('#globalSearch').find('li.ui-state-focus a:visible').text() != "") {
                focusedMakeModel = new Object();
                focusedMakeModel = objBikes.result ? objBikes.result[$('li.ui-state-focus').index()] : null;
            }
            else {
                $('#errGlobalSearch').hide();
                var container = $('#global-search-section');
                if (container.is(':visible')) {
                    if (!container.is(event.relatedTarget) && container.has(event.relatedTarget).length === 0) {
                        recentSearches.hideRecentSearches();
                    }
                }
            }
        },
        afterfetch: function (result, searchtext) {

            if (result != undefined && result.length > 0 && searchtext.trim() != '') {
                $('#errGlobalSearch').hide();
                recentSearches.hideRecentSearches();
                globalSearchResult = true;
            }
            else {
                focusedMakeModel = null; globalSearchResult = false;
                if (searchtext.trim() != '') {
                    $('#errGlobalSearch').show();
                    recentSearches.hideRecentSearches();
                }
                var keywrd = $('#globalSearch').val();
                var category = GetCatForNav();
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': category, 'act': 'Search_Keyword_Not_Present_in_Autosuggest', 'lab': keywrd });
            }
        },
        keyup: function () {
            if ($('#globalSearch').val().trim() != '' && $('li.ui-state-focus a:visible').text() != "") {
                focusedMakeModel = new Object();
                focusedMakeModel = objBikes.result ? objBikes.result[$('li.ui-state-focus').index()] : null;
                $('#errGlobalSearch').hide();
                recentSearches.hideRecentSearches();
            } else {
                if ($('#globalSearch').val().trim() == '') {
                    $('#errGlobalSearch').hide();
                    recentSearches.showRecentSearches();
                }
            }

            if ($('#globalSearch').val().trim() == '' || e.keyCode == 27 || e.keyCode == 13) {
                if (focusedMakeModel == null || focusedMakeModel == undefined) {
                    if ($('#globalSearch').val().trim() != '') {
                        $('#errGlobalSearch').show();
                    }
                }
                else {
                    $('#errGlobalSearch').hide();
                    recentSearches.showRecentSearches();
                }

            }
        }
    })
        .keydown(function (e) {
            if (e.keyCode == 13) {
                if (!isMakeModelRedirected)
                    $('#btnGlobalSearch').click();
                else
                    isMakeModelRedirected = false;
            }

        });

    /* bikes search ends autocomplete */

    /* recent searches code starts here */
    recentSearches =
    {
        searchKey: "recentsearches",
        options: {
            homeSearchEle: $('#newBikeList'),
            bikeSearchEle: $('#globalSearch'),
            globalSearchSection: $('#new-global-search-section').length ? $('#new-global-search-section') : $('#global-search-section'),
            recentSearchesEle: $("#new-global-recent-searches").length ? $("#new-global-recent-searches") : $("#global-recent-searches"),
            trendingSearchesEle: $("#new-trending-bikes").length ? $("#new-trending-bikes") : $("#trending-bikes"),
            recentSearchDiv: $('#new-history-search').length ? $('#new-history-search') : $('#history-search'),
            trendingSearchDiv: $('#new-trending-search').length ? $('#new-trending-search') : $('#trending-search'),
            recentSearchesLoaded: false,
            trendingSearchesLoaded: false
        },
        saveRecentSearches: function (opt) {
            if (opt && opt.payload && opt.payload.makeId > 0) {
                var objSearches = bwcache.get(this.searchKey) || {};
                opt.payload["name"] = opt.label;
                objSearches.searches = objSearches.searches || [];
                eleIndex = this.objectIndexOf(objSearches.searches, opt.payload);
                if (objSearches.searches != null && eleIndex > -1) objSearches.searches.splice(eleIndex, 1);
                objSearches.searches.unshift(opt.payload);

                objSearches["lastModified"] = new Date().getTime();
                if (objSearches.searches.length > 3)
                    objSearches.searches.pop();
                objSearches["noOfSearches"] = objSearches.searches.length;
                bwcache.set(this.searchKey, objSearches);
            }
        },
        showRecentSearches: function () {
            var html = "";
            if (!this.options.recentSearchesLoaded) {
                objSearches = bwcache.get(this.searchKey);
                if (objSearches && objSearches.searches) {
                    var bikename, url;
                    var i = 0;
                    for (var item in objSearches.searches) {
                        item = objSearches.searches[item];
                        bikename = item.name || '';
                        if (bikename != '' && $("#global-recent-searches li[data-modelid='" + item.modelId + "']").length == 0) {
                            html += '<li data-makeid="' + item.makeId + '" data-modelid="' + item.modelId + '" class="" tabindex="' + i++ + '"><span class="bwsprite history-icon"></span><a href="javascript:void(0)" data-href="/' + item.makeMaskingName + '-bikes/' + item.modelMaskingName + '" optionname="' + bikename.toLowerCase().replace(' ', '') + '">' + bikename + '</a>';
                            if (item.modelId > 0) {
                                if (item.futuristic == 'True') {
                                    html += '<span class="upcoming-link">coming soon</span>';
                                } else {
                                    if (item.isNew == 'True') {
                                        html += '<a href="javascript:void(0)" data-pqSourceId="' + pqSourceId + '" data-modelId="' + item.modelId + '" class="getquotation target-popup-link" onclick="setPriceQuoteFlag()">Check On-Road Price</a>';
                                    } else {
                                        html += '<span class="upcoming-link">discontinued</span>';
                                    }
                                }
                                html += '<div class="clear"></div>';
                            }
                            html += "</li>";
                        }
                    }
                    if (html != "") {
                        this.options.recentSearchesEle.append(html);
                        this.options.recentSearchesLoaded = true;
                    }
                }
            }

            if (!this.options.trendingSearchesLoaded) {
                if (trendingBikes) {
                    html = '<li class="bw-ga" data-cat="' + pageName + '" data-act="AutoExpo_2018_Link Clicked" data-lab="Trending_Searches_Autosuggest_Clicked">\<span class="trending-searches"></span><a href="https://www.bikewale.com/autoexpo2018/" data-href="https://www.bikewale.com/autoexpo2018/">Auto Expo 2018</a>';
                    for (var index in trendingBikes) {
                        item = trendingBikes[index];
                        html += '<li data-makeid="' + item.objMake.makeId + '" data-modelid="' + item.objModel.modelId + '" class="bw-ga" data-cat="' + pageName + '" data-act="Trending_Searches_Search_Bar_Clicked" data-lab="' + item.BikeName + '"><span class="trending-searches"></span><a href="javascript:void(0)" data-href="/'
                            + item.objMake.maskingName + '-bikes/' + item.objModel.maskingName + '" optionname="' + item.BikeName.toLowerCase().replace(' ', '') + '">' + item.BikeName + '</a>';
                        if (item.objModel.modelId > 0) {
                            html += '<a href="javascript:void(0)" data-pqSourceId="' + pqSourceId + '" data-modelId="' + item.objModel.modelId + '" class="getquotation target-popup-link" onclick="setPriceQuoteFlag()">Check On-Road Price</a><div class="clear"></div>';
                        }
                        html += "</li>";
                    }

                    if (html != "") {
                        this.options.trendingSearchesEle.append(html);
                        this.options.trendingSearchesLoaded = true;
                    }
                }
            }

            if (!trendingBikes) {
                this.options.trendingSearchDiv.addClass('hide');
            } else {
                this.options.trendingSearchDiv.removeClass('hide');
            }

            if (!(objSearches && objSearches.searches)) {
                this.options.recentSearchDiv.addClass('hide');
            } else {
                this.options.recentSearchDiv.removeClass('hide');
            }

            var rsele = this.options.recentSearchesEle.find("li");
            var trele = this.options.trendingSearchesEle.find("li");
            if (rsele.length > 0 || trele.length > 0) {
                this.options.globalSearchSection.removeClass('hide');
                this.handleKeyEvents();
            }
        },
        hideRecentSearches: function () {
            this.options.globalSearchSection.addClass('hide');
        },
        handleKeyEvents: function () {
            var rsele = this.options.recentSearchesEle.find("li.focus-state");
            if (event.keyCode == 40) {
                rsele.next().addClass("focus-state").siblings().removeClass("focus-state");
                curElement = rsele.next();
                return false;
            } else if (event.keyCode == 38) {
                rsele.prev().addClass("focus-state").siblings().removeClass("focus-state");
                curElement = rsele.prev();
                return false;
            }
            else if (event.keyCode == 27) {
                this.hideRecentSearches();
            }
            else if (event.keyCode == 13) {
                rsele.trigger('click');
            }
        },
        objectIndexOf: function (arr, opt) {
            var makeId = opt.makeId, modelId = opt.modelId;
            for (var i = 0, len = arr.length; i < len; i++)
                if (arr[i]["makeId"] === opt.makeId && arr[i]["modelId"] === opt.modelId) return i;
            return -1;
        }
    };

    recentSearches.options.recentSearchesEle.on('click', 'li', function () {
        try {
            if (!$(event.target).hasClass('getquotation')) {

                var objSearches = bwcache.get(recentSearches.searchKey) || {}, mkId = this.getAttribute("data-makeid"), moId = this.getAttribute("data-modelid"),
                    eleIndex = recentSearches.objectIndexOf(objSearches.searches, { makeId: mkId, modelId: moId }),
                    obj = objSearches.searches[eleIndex];
                if (objSearches.searches != null && eleIndex > -1) objSearches.searches.splice(eleIndex, 1);
                objSearches.searches.unshift(obj);
                bwcache.set(recentSearches.searchKey, objSearches);
                triggerGA(pageName, ' Recently_View_Search_Bar_Clicked', this.textContent);
                window.location.href = $(this).find('a').first().attr('data-href');
            }

            recentSearches.hideRecentSearches();

        } catch (e) {
            console.log(e.message);
        }
    });

    recentSearches.options.trendingSearchesEle.on('click', 'li', function () {
        try {
            if (!$(event.target).hasClass('getquotation')) {
                window.location.href = $(this).find('a').first().attr('data-href');
            }
            recentSearches.hideRecentSearches();
        } catch (e) {
            console.log(e.message);
        }
    });

    var elements = recentSearches.options.recentSearchesEle.find("li");
    var curElement = recentSearches.options.recentSearchesEle.find("li").first();

    $(document).on('mouseenter', '.recent-searches-dropdown li', function () {
        $(this).addClass("focus-state");
        curElement = $(this);
    });
    $(document).on('mouseleave', '.recent-searches-dropdown li', function () {
        $(this).removeClass("focus-state");
    });

    $(document).on('mouseleave', '.recent-searches-dropdown', function () {
        curElement.addClass("focus-state").siblings().removeClass("focus-state");
    });

    /* recent searches ends starts here */

    popup = {
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


    window.validateMobileNo = function (mobileNo, self) {
        if (self != null) {
            var regPhone = /^[6-9][0-9]{9}$/;
            var isValid = true;
            if (mobileNo == "") {
                self.msg = "Please enter your mobile no.";
                isValid = false;
            }
            else if (!regPhone.test(mobileNo) && isValid) {
                self.msg = "Please enter a valid mobile no.";
                isValid = false;
            }
            else
                self.msg = "";
            return isValid;
        }
        else {
            return false;
        }
    };

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
        if (btnFindBikeNewNav() === false || searchVal === placeHolder || (searchVal).trim() === "") {
            $('#errNewBikeSearch').show();
            return false;
        } else {
            return true;
        }

    });

   // Stop default achor propgation and redirect to custom link
    $(".redirect-url").click(function (ev) {
        ev.preventDefault();
        window.location.href = $(this).attr('data-url');
    });

    $(document).on("click", ".bw-ga", function () {
        try {
            var obj = $(this);
            var category = obj.attr("data-cat") || obj.attr("c") || $('body').data('page-name');
            var action = obj.attr("data-act") || obj.attr("a");
            var label = obj.attr("data-lab") || obj.attr("l");
            var variable = obj.attr("data-var") || obj.attr("v");
            var funct = obj.attr("data-func") || obj.attr("f");
            if (label !== undefined) {
                triggerGA(category, action, label);
            }
            else if (variable !== undefined) {
                triggerGA(category, action, window[variable]);
            }
            else if (funct !== undefined) {
                triggerGA(category, action, eval(funct + '()'));
            }
        }
        catch (e) {
        }
    });

    $(".lazy").lazyload({
        effect: "fadeIn"
    });
    applyLazyLoad();
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
            if (city.cityId != globalCityId) {
                SetCookieInDays("location", city.cityId + "_" + cityName, 365);
                bwcache.set("userchangedlocation", window.location.href, true);
            }
            globalCityId = city.cityId;
            CloseCityPopUp();
            showGlobalCity(cityName);
            dataLayer.push({ 'event': 'Bikewale_all', 'cat': GetCatForNav(), 'act': 'City_Popup_Default', 'lab': cityName });
            dataLayer.push({ 'GlobalCity': cityName });
            ga('set', 'dimension3', cityName);
            if (city.cityId) {
                location.reload();
            }           
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
                showHideMatchError(element, false);
            else
                showHideMatchError(element, true);
        }
    }).autocomplete("widget").addClass("globalCity-auto-desktop").css({ 'position': 'fixed' });

    // nav bar code starts
    $(".navbarBtn").click(function () {
        navbarShow();
    });
    $(".blackOut-window").mouseup(function (e) {
        var nav = $("#nav");
        if (e.target.id !== nav.attr('id') && !nav.has(e.target).length) {
            nav.animate({ 'left': '-350px' });
            unlockPopup();
        }
    });
    $(".navUL > li > a").click(function () {
        if (!$(this).hasClass("open")) {
            var a = $(".navUL li a");
            a.removeClass("open").next("ul").slideUp(350);
            $(this).addClass("open").next("ul").slideDown(350);

            if ($(this).siblings().size() === 0) {
                navbarHide();
            }

            $(".nestedUL > li > a").click(function () {
                $(".nestedUL li a").removeClass("open");
                $(this).addClass("open");
                navbarHide();
            });

        }
        else if ($(this).hasClass("open")) {
            $(this).removeClass("open").next("ul").slideUp(350);
        }
    }); // nav bar code ends here

    // login code starts 
    $("#firstLogin").click(function () {
        window.location.href = "/users/login.aspx?ReturnUrl=" + window.location.pathname;
    });
    $(".blackOut-window").mouseup(function (e) {
        var loginPopUp = $(".loginPopUpWrapper");
        if (e.target.id !== loginPopUp.attr('id') && !loginPopUp.has(e.target).length) {
            loginPopUp.animate({ 'right': '-400px' });
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
    $("#userLoggedin").click(function () {
        $(".blackOut-window").show();
        $(".loggedinProfileWrapper").animate({ right: '0' });
    });
    $(".afterLoginCloseBtn").click(function () {
        unlockPopup();
        $(".loggedinProfileWrapper").animate({ right: '-280px' });
        loginSignupSwitch();
    });
    $(".blackOut-window").mouseup(function (e) {
        var loggedIn = $(".loggedinProfileWrapper");
        if (e.target.id !== loggedIn.attr('id') && !loggedIn.has(e.target).length) {
            loggedIn.animate({ 'right': '-280px' });
            unlockPopup();
        }
    });

    //global city popup
    $("#header div.gl-default-stage").click(function () {
        var changeCityPopup = $('#globalchangecity-popup');
        if (typeof changeCityPopup !== 'undefined' && $(changeCityPopup).is(':visible')) { // ie lt 10 fix
            event.preventDefault();
        }
        else {
            $(".blackOut-window").show();
            CheckGlobalCookie();
            $(".globalcity-popup").removeClass("hide").addClass("show");
        }
    });

    $(".blackOut-window").mouseup(function (e) {
        var globalLocation = $("#globalcity-popup");
        if (e.target.id !== globalLocation.attr('id') && !globalLocation.has(e.target).length) {
            CloseCityPopUp();
        }
        globalLocation = $("#globalchangecity-popup");
        if (globalLocation && e.target.id !== globalLocation.attr('id') && !globalLocation.has(e.target).length) {
            globalLocation.hide();
            unlockPopup();
        }
    });

    $(".globalcity-close-btn").click(function () {
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
        if (globalCityId > 0 && ($(ele).val()) != "") {
            showHideMatchError(ele, false);
            CloseCityPopUp();
        }
        else {
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
            $(this).find("div.lazy").trigger("divLazyLoad");
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
            triggerGA(categ, 'Logo', 'Logo_Clicked');
        }
    });

    // more brand - collapse
    $('.view-brandType').click(function (event) {
        var element = $(this),
            elementParent = element.closest('.collapsible-brand-content'),
            moreBrandContainer = elementParent.find('.brandTypeMore');

        if (!moreBrandContainer.is(':visible')) {
            moreBrandContainer.slideDown();
            element.addClass('active').find('.btn-label').text('View less brands');
        }
        else {
            moreBrandContainer.slideUp();
            element.removeClass('active').find('.btn-label').text('View more brands');
        }

        event.preventDefault();
        event.stopPropagation();
    });

    $(".brand-collapsible-present li").click(function () {
        var tabsPanel = $(this).closest('.bw-tabs-panel'),
            collapsibleBrand = tabsPanel.find('.collapsible-brand-content'),
            moreBrandContainer = collapsibleBrand.find('.brandTypeMore'),
            viewMoreBtn = collapsibleBrand.find('.view-brandType');

        moreBrandContainer.slideUp();
        viewMoreBtn.removeClass('active').find('.btn-label').text('View more brands');
    });

    // read more - collapse
    $(document).on('click', '.read-more-target', function () {
        var element = $(this),
            parentElemtent = element.closest('.collapsible-content');

        if (!parentElemtent.hasClass('active')) {
            parentElemtent.addClass('active');
            element.text(' Collapse');
        }
        else {
            parentElemtent.removeClass('active');
            element.text('...Read more');
        }
    });


    $(".modelurl").click(function () {
        var array = $(this).attr('href').split('/');
        if (array.length > 2) {
            dataLayer.push({
                'event': 'Bikewale_all', 'cat': 'Make_Page', 'act': 'Model_Click', 'lab': _makeName + '_' + array[2]
            });
        }
    });

    $('.dealer-details-main-content').on('click', function () {
        $(this).hide();
        $(this).next('.dealer-details-more-content').show();
    });

    ko.bindingHandlers.formateDate = {
        update: function (element, valueAccessor) {
            var date = new Date(valueAccessor());
            var formattedDate = monthNames[date.getUTCMonth()] + ' ' + date.getUTCDate() + ', ' + date.getUTCFullYear();
            $(element).text(formattedDate);
        }
    };

    //log javascript errors
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
                    $.ajax({
                        type: "POST", url: "/api/JSException/", data: error,
                        error: function (event, request, settings) {
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
});