var focusedMakeModel = null, focusedCity = null;
var objBikes = new Object();
var objCity = new Object();
var globalCityId = 0;
var _makeName = '';
var pqSourceId = "38";
var IsPriceQuoteLinkClicked = false;
var ga_pg_id = '0', recentSearches, navDrawer, cityArea, quotationPage, playerState = '';
var navContainer, effect = 'slide', directionLeft = { direction: 'left' }, duration = 500;
var popupHeading, popupContent, brandcitypopupContent;



if (!Array.prototype.indexOf) {
    Array.prototype.indexOf = function (elt) {
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


function triggerGA(cat, act, lab) {
    try {

        dataLayer.push({ 'event': 'Bikewale_all', 'cat': cat, 'act': act, 'lab': lab });
    }
    catch (e) {// log error   
    }
}

function triggerNonInteractiveGA(cat, act, lab) {
    try {

        dataLayer.push({ 'event': 'Bikewale_noninteraction', 'cat': cat, 'act': act, 'lab': lab });
    }
    catch (e) {// log error   
    }
}

function GetCatForNav() {
    var ret_category = "other";
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
                ret_category = "BikeWale_PQ";
                break;
            case "7":
                ret_category = "Dealer_PQ";
                break;
            case "8":
                ret_category = "Booking_Config_Page";
                break;
            case "9":
                ret_category = "Booking_Page";
                break;
            case "10":
                ret_category = "News_Page";
                break;
            case "11":
                ret_category = "News_Detail";
                break;
            case "12":
                ret_category = "Expert_Reviews_Page";
                break;
            case "13":
                ret_category = "Expert_Reviews_Detail";
                break;
            case "14":
                ret_category = "BookingSummary_New";
                break;
            case "15":
                ret_category = "SpecsAndFeature";
                break;
            case "16":
                ret_category = "Price_in_City_Page";
                break;
            case "39":
                ret_category = "BookingListing";
                break;
        }
    }
    return ret_category;
}

function pushNavMenuAnalytics(menuItem) {
    var categ = GetCatForNav();
    if (categ != null) {
        triggerGA(categ, 'Hamburger_Menu_Item_Click', menuItem);
    }
}

function navbarShow() {
    $('body').addClass('lock-browser-scroll');
    $("#nav").addClass('open').stop().animate({
        'left': '0px'
    });
    $(".blackOut-window").show();
}

function btnFindBikeNewNav() {
    if (focusedMakeModel == undefined || focusedMakeModel == null) {
        return false;
    }
    return MakeModelRedirection(focusedMakeModel);
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
                    $(".fa-spinner").show();
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
                            $(".fa-spinner").hide();
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
            minLength: options.minLength || 1,
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
                          .append('<span class="bwmsprite ui-search-icon"></span><a OptionName=' + item.label.replace(/\s/g, '').toLowerCase() + '>' + __highlight(item.label, reqTerm) + '</a>');

            if (options.source == '1') {
                if (item.payload.modelId > 0) {
                    if (item.payload.futuristic == 'True') {
                        ulItem.append('<span class="upcoming-link">coming soon</span>')
                    } else {
                        if (item.payload.isNew == 'True') {
                            ulItem.append('<a href="javascript:void(0)" data-pqSourceId="' + pqSourceId + '" data-modelId="' + item.payload.modelId + '" class="getquotation target-popup-link" onclick="setPriceQuoteFlag()">Check On-Road Price</a>');
                        }
                        else {
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

function slideChangeStart() {
    //console.log('slideChangeStart');
    if (playerState == 'playing' || playerState == 'buffering') {
        //console.log(playerState);
        try {
            videoPause();
        } catch (e) { console.log(e.toString()); }
    }
};

var getHost = function () {
    var host = document.domain;
    if (host.match("bikewale.com$"))
        host = ".bikewale.com";
    return host;
}

function SetCookie(cookieName, cookieValue) {
    if (/MSIE (\d+\.\d+);/.test(navigator.userAgent) || /Trident\//.test(navigator.userAgent))
        document.cookie = cookieName + "=" + cookieValue + '; path =/';
    else
        document.cookie = cookieName + "=" + cookieValue + ';domain=' + getHost() + '; path =/';
}

function SetCookieInDays(cookieName, cookieValue, nDays) {
    var today = new Date();
    var expire = new Date();
    expire.setTime(today.getTime() + 3600000 * 24 * nDays);
    cookieValue = cookieValue.replace(/\s+/g, '-');
    if (/MSIE (\d+\.\d+);/.test(navigator.userAgent) || /Trident\//.test(navigator.userAgent))
        document.cookie = cookieName + "=" + cookieValue + ";expires=" + expire.toGMTString() + '; path =/';
    else
        document.cookie = cookieName + "=" + cookieValue + ";expires=" + expire.toGMTString() + ';domain=' + getHost() + '; path =/';

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

function CheckGlobalCookie() {
    var cookieName = "location";
    if (isCookieExists(cookieName)) {
        var locationCookie = getCookie(cookieName);
        locationCookie = (locationCookie.replace('-', ' ')).split("_");
        var cityName = locationCookie[1];
        globalCityId = parseInt(locationCookie[0]);
        showGlobalCity(cityName);
        showHideMatchError($("#globalCityPopUp"), false);
        $(".fa-spinner").hide();
        $("#globalCityPopUp").val(cityName);
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
function lockPopup() {
    $('body').addClass('lock-browser-scroll');
    $(".blackOut-window").show();
}
function unlockPopup() {
    $('body').removeClass('lock-browser-scroll');
    $(".blackOut-window").hide();
}

$(window).resize(function () {
    var newwidth = 98 + '%';
    $(".ui-autocomplete").width(newwidth);
});

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
function selectElementFromArray(dataArray, id) {
    if (dataArray != null && (l = dataArray.length) > 0) {
        for (var i = 0; i < l; i++) {
            if (dataArray[i].cityId === id || dataArray[i].AreaId === id || dataArray[i].areaId === id || dataArray[i].CityId === id || dataArray[i].id === id)
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

var hashChange = function (e) {
    var oldUrl, oldHash;
    oldUrl = e.originalEvent.oldURL;
    if (oldUrl && (oldUrl.indexOf('#') > 0)) {
        oldHash = oldUrl.split('#')[1];
        closePopUp(oldHash);
    };
};

var appendHash = function (state) {
    window.location.hash = state;
}

var closePopUp = function (state) {
    switch (state) {
        case "globalCity":
            CloseCityPopUp();
            break;
        case "onRoadPrice":
            closeOnRoadPricePopUp();
            break;
        case "contactDetails":
            leadPopupClose();
            break;
        case "viewBreakup":
            viewBreakUpClosePopup();
            break;
        case "otp":
            otpPopupClose();
            break;
        case "dpqPopup":
            dpqLeadCaptureClosePopup();
            break;
        case "bookingsearch":
            bookingSearchClose();
            break;
        case "listingPopup":
            listingLocationPopupClose();
            break;
        case "offersPopup":
            offersPopupClose($('div#offersPopup'));
            break;
        case "emiPopup":
            emiPopupClose($('#emiPopup'));
            break;
        case "assistancePopup":
            assistancePopupClose($('#leadCapturePopup'));
            break;
        case "locatorsearch":
            locatorSearchClose();
            break;
        case "moreDealers":
            popupDiv.close($('#more-dealers-popup'));
            break;
        case "dealerOffers":
            popupDiv.close($('#dealer-offers-popup'));
            break;
        case "sellerDealers":
            getSellerDetailsPopup.close();
            break;
        case "requestMedia":
            requestMediaPopup.close();
            break;
        case "termsConditions":
            popupDiv.close($('#termsPopUpContainer'));
            break;
        case "sellerDealers":
            getSellerDetailsPopup.close();
            break;
        case "photosGallery":
            popupGallery.close();
            break;
        default:
            return true;
    }
};

function centerItVariableWidth(target, outer) {
    var out = $(outer);
    var tar = target;
    var x = out.width();
    var y = tar.outerWidth(true);
    var z = tar.index();
    var q = 0;
    var m = out.find('li');
    //Just need to add up the width of all the elements before our target. 
    for (var i = 0; i < z; i++) {
        q += $(m[i]).outerWidth(true);
    }
    out.animate({ scrollLeft: Math.max(0, q - (x - y) / 2) }, 500, 'swing');
}
function applyTabsLazyLoad() {
    $("img.lazy").lazyload({
        event: "imgLazyLoad"
    });
    $("div.lazy").lazyload({
        event: "divLazyLoad"
    });
}

function CloseCityPopUp() {
    $("#globalcity-popup").hide();
    $(".blackOut-window").hide();
}

function closeOnRoadPricePopUp() {
    $("#popupWrapper").hide();
    $(".bwm-city-area-popup-wrapper .back-arrow-box").click();
};

function dpqLeadCaptureClosePopup() {
    var leadCapturePopup = $("#leadCapturePopup");
    leadCapturePopup.hide();
}

var locationFilter = function (filterContent, wrapperMenu) {

    var inputText = $(filterContent).val();
    inputText = inputText.toLowerCase();
    var inputTextLength = inputText.length;
    if (inputTextLength > 0) {

        wrapperMenu.find("li").each(function () {
            var locationName = $(this).text().toLowerCase();
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
        $(filterContent).parent("div.user-input-box").siblings("ul").find("li").show();
    }
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

};

function setPriceQuoteFlag() {
    $('#global-search-popup').hide();
    unlockPopup();
    IsPriceQuoteLinkClicked = true;
}
function formatPrice(x) { try { x = x.toString(); var lastThree = x.substring(x.length - 3); var otherNumbers = x.substring(0, x.length - 3); if (otherNumbers != '') lastThree = ',' + lastThree; var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree; return res; } catch (err) { } }

// common autocomplete data call function
function dataListDisplay(availableTags, request, response) {
    var results = $.ui.autocomplete.filter(availableTags, request.term);
    response(results.slice(0, 5));
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

        recentSearches.saveRecentSearches(items);

        if (model != null && model != undefined) {
            window.location.href = "/m/" + make.maskingName + "-bikes/" + model.maskingName + "/";
            return true;
        } else if (make != null && make != undefined) {
            window.location.href = "/m/" + make.maskingName + "-bikes/";
            return true;
        }
    }
}

function LoadTerms(offerId) {
    $("div#termsPopUpContainer").show();
    $(".blackOut-window").show();
    if (offerId != 0 && offerId != null) {
        $('#termspinner').show();
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
    $('#termspinner').hide();
}


function CloseCityPopUp() {
    $("#globalcity-popup").hide();
    unlockPopup();
}

var appendState = function (state) {
    window.history.pushState(state, '', '');
};

docReady(function () {

    (function ($) {
        $.fn.hint = bwHint;
    })(jQuery);

    (function ($) {
        $.fn.bw_autocomplete = bwAutoComplete;
    })(jQuery);

    if (quotationPage) {
        $('header .bw-logo, header .navbarBtn, header .global-location, .ae-logo-border, .ae-sprite.ae-logo').hide();
        $('.headerTitle,.white-back-arrow').show();

        $('#book-back').on('click', function () {
            window.history.back();
        });
    }

    //Scroll To Top function

    $(window).scroll(function () {
        if ($(this).scrollTop() > 180) {
            $('.back-to-top').fadeIn(500);
        } else {
            $('.back-to-top').fadeOut(500);
        }
    });

    $('.back-to-top').click(function (event) {
        $('html, body').stop().animate({ scrollTop: 0 }, 600);
        event.preventDefault();
    });

    popupHeading = $("#popupHeading"), popupContent = $("#popupContent"), brandcitypopupContent = $("#brandcitypopupContent");

    //App Banner
    var isSafari = Object.prototype.toString.call(window.HTMLElement).indexOf('Constructor') > 0;
    var appbanner = getCookie("AppBanner");
    if ((appbanner == null || appbanner == "show") && !isSafari) {        
        var hideBannerPages = ['7', '14'];
        if (ga_pg_id == 2) {
            if (typeof isDealerPQ !== 'undefined') {
                if (isDealerPQ === 'False') {
                    $("#appBanner").slideDown();
                }
            }
        } else if (hideBannerPages.indexOf(ga_pg_id) == -1) {
            $("#appBanner").slideDown();
            SetCookie("AppBanner", "show");
            dataLayer.push({ 'event': 'Bikewale_noninteraction', 'cat': GetCatForNav(), 'act': 'App_Download_Banner_Shown' });
        }
    }

    /* Swiper custom methods */

    var iOS = /iPad|iPhone|iPod/.test(navigator.userAgent) && !window.MSStream;

    var slideCount = function (swiper) {
        try {
            imgTotalCount = swiper.slides.length;
            $(document).find('.bike-model-gallery-count').text(swiper.activeIndex + 1 + " of " + imgTotalCount.toString());
        } catch (e) { }
    };

    $('.swiper-container:not(".noSwiper")').each(function (index, element) {
        $(this).addClass('sw-' + index);
        $('.sw-' + index).swiper({
            effect: 'slide',
            speed: 300,
            //autoplay: 3000,
            nextButton: $(this).find('.swiper-button-next'),
            prevButton: $(this).find('.swiper-button-prev'),
            pagination: $(this).find('.swiper-pagination'),
            slidesPerView: 'auto',
            paginationClickable: true,
            spaceBetween: 10,
            //freeMode: true,
            //freeModeSticky: true,
            preloadImages: false,
            lazyLoading: true,
            lazyLoadingInPrevNext: true,
            watchSlidesProgress: true,
            watchSlidesVisibility: true,
            onSlideChangeStart: slideChangeStart,
            onSlideChangeEnd: slideCount
        });

    });

    //Load the visible images
    $(".swiper-slide-visible img,.swiper-slide-active img").each(function () {
        var src = $(this).attr("data-src");
        $(this).attr("src", src);
        if (!$(this).error)
            $(this).parent().find('.swiper-lazy-preloader').remove();
    });

    if ($('#bikeBannerImageCarousel')[0] != undefined) {
        var bikeModelSwiper = $('#bikeBannerImageCarousel')[0].swiper;

        var currentMainStageActiveImage;
        bikeModelSwiper.on('slideChangeStart', function () {
            currentMainStageActiveImage = $('#bikeBannerImageCarousel .stage').find(".swiper-slide.swiper-slide-active img");
            $('#bikeBannerImageCarousel').css({ 'height': currentMainStageActiveImage.height() });
        });
    }

    $('#newBikeList').val('').focus();
    $('#globalCityPopUp').val('');

    $(".fa-spinner").hide();

    CheckGlobalCookie();

    $('.globalcity-close-btn').click(function () {
        CloseCityPopUp();
        window.history.back();
    });

    $(".onroad-price-close-btn").click(function () {
        closeOnRoadPricePopUp();
        window.history.back();
    });


    $("#globalCity").autocomplete({
        source: function (request, response) {
            dataListDisplay(availableTags, request, response);
        }, minLength: 1
    }).css({ 'width': '179px' });


    $("#gs-close").click(function () {
        $(".global-search-popup").hide();
        unlockPopup();
    });

    $("#gs-text-clear").click(function () {
        $("#globalSearch").val("").change();
        $("#globalSearch").focus();
        $(this).hide();
    });
    		
    $("#newBikeList").bw_autocomplete({		
        recordCount: 5,		
        source: 1,		
        onClear: function () {		
            objBikes = new Object();		
        },		
        click: function (event, ui, orgTxt) {		
            MakeModelRedirection(ui.item);		
            // GA code		
            var keywrd = ui.item.label + '_' + $('#newBikeList').val();		
            dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'HP', 'act': 'Search_Keyword_Present_in_Autosuggest', 'lab': keywrd });		
        },		
		
        open: function (result) {		
            objBikes.result = result;		
            if (navigator.userAgent.match(/(iPod|iPhone|iPad)/)) {		
                $('.ui-autocomplete').off('menufocus hover mouseover');		
            }		
        },		
        focus: function () {		
            if ($('#newBikeList').val().trim() == '') {		
                recentSearches.showRecentSearches();		
            }		
        },		
        focusout: function () {		
            if ($('li.ui-state-focus a:visible').text() != "") {		
                focusedMakeModel = new Object();		
                focusedMakeModel = objBikes.result ? objBikes.result[$('li.ui-state-focus').index()] : null;		
            }		
            else {		
                $('#errNewBikeSearch').hide();		
                recentSearches.hideRecentSearches();		
            }		
        },		
        afterfetch: function (result, searchtext) {		
            if (result != undefined && result.length > 0 && searchtext.trim()) {		
                $('#errNewBikeSearch').hide();		
                recentSearches.hideRecentSearches();		
                NewBikeSearchResult = true;		
            }		
            else {		
                focusedMakeModel = null; NewBikeSearchResult = false;		
                if (searchtext.trim() != '')		
                {		
                    $('#errNewBikeSearch').show();		
                    recentSearches.hideRecentSearches();		
                }		
                  		
            }		
        },		
        keyup: function () {		
            if ($('li.ui-state-focus a:visible').text() != "") {		
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
                    if ($('#newBikeList').val().trim() != '')		
                    {		
                         $('#errNewBikeSearch').show();		
                    recentSearches.hideRecentSearches();		
                    }		
                      		
                }		
                else		
                {		
                    $('#errNewBikeSearch').hide();		
                    recentSearches.hideRecentSearches();		
                }		
                    		
            }		
        }		
    }).css({ 'width': '100%' });		
		
		
    $('#newBikeList').on('keypress', function (e) {		
        var id = $('#newBikeList');		
        var searchVal = id.val();		
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
        var searchVal = id.val().trim();		
        var placeHolder = id.attr('placeholder');		
        triggerGA('HP', 'Search_Not_Keyword_Present_in_Autosuggest', searchVal);		
        if (btnFindBikeNewNav() || searchVal == placeHolder || (searchVal).trim() == "") {		
            $('#errNewBikeSearch').hide();		
            return false;		
        } else {		
            $('#errNewBikeSearch').show();		
        }		
    });

    // global city popup autocomplete
    $("#globalCityPopUp").bw_autocomplete({
        recordCount: 5,
        source: 3,
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
            CloseCityPopUp();
            showGlobalCity(ui.item.label);
            dataLayer.push({ 'event': 'Bikewale_all', 'cat': GetCatForNav(), 'act': 'City_Popup_Default', 'lab': cityName });
            if (city.cityId) {
                location.reload();
            }
        },
        open: function (result) {
            objCity.result = result;
            if (navigator.userAgent.match(/(iPod|iPhone|iPad)/)) {
                $('.ui-autocomplete').off('menufocus hover mouseover');
            }
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
    }).autocomplete("widget").addClass("globalCity-autocomplete").css({ 'z-index': '11', 'font-weight': 'normal', 'text-align': 'left' });



    $("#citySelectionFinalPrice").autocomplete({
        source: function (request, response) {
            dataListDisplay(availableTags, request, response);
        }, minLength: 1
    }).css({ 'width': '365px' });



    $(".blackOut-window").mouseup(function (e) {
        var globalSearchPopup = $(".global-search-popup");
        if (e.target.id !== globalSearchPopup.attr('id') && !globalSearchPopup.has(e.target).length) {
            globalSearchPopup.hide();
            unlockPopup();
        }
    });

    $(".global-location").click(function () {
        $("#globalcity-popup").show();
        lockPopup();
        CheckGlobalCookie();
        appendHash("globalCity");
    });

    $(".blackOut-window").mouseup(function (e) {
        var globalLocation = $("#globalcity-popup");
        if (e.target.id !== globalLocation.attr('id') && !globalLocation.has(e.target).length) {
            globalLocation.hide();
            unlockPopup();
        }
    });

    //making card clickable 
    $('li.card').click(function () {
        $(this).find('a')[0].click();
    });

    $(".navUL > li > a").click(function (e) {

        if (!$(this).hasClass("open")) {
            var a = $(".navUL li a");
            a.removeClass("open").next("ul").slideUp(350);
            $(this).addClass("open").next("ul").slideDown(350);

            if ($(this).siblings().size() == 0) {
                navDrawer.close();
            }

            $(".nestedUL > li > a").click(function () {
                $(".nestedUL li a").removeClass("open");
                $(this).addClass("open");
                navDrawer.close();
            });
        }
        else if ($(this).hasClass("open")) {
            $(this).removeClass("open").next("ul").slideUp(350);
        }
    }); // nav bar code ends here

    // Common BW tabs code
    $(".bw-tabs li").on('click', function () {
        var panel = $(this).closest(".bw-tabs-panel");
        panel.find(".bw-tabs li").removeClass("active");
        $(this).addClass("active");
        var panelId = $(this).attr("data-tabs");
        panel.find(".bw-tabs-data").hide();
        $("#" + panelId).show();
        applyTabsLazyLoad();
        $("#" + panelId).find(".swiper-slide-active img.swiper-lazy").each(function (index) {
            var src = $(this).attr("data-src");
            $(this).attr("src", src);
            $(this).parent().find('.swiper-lazy-preloader').remove();
        });
        centerItVariableWidth($(this), '.bw-tabs');

        var swiperContainer = $('#' + panelId + " .swiper-container");
        if (swiperContainer.length > 0) {
            var sIndex = swiperContainer.attr('class');
            var regEx = /sw-([0-9]+)/i;
            try {
                var index = regEx.exec(sIndex)[1]
                $('.sw-' + index).data('swiper').update(true);
            } catch (e) { }
        }

    }); // ends
    // Common CW select box tabs code
    $(".bw-tabs select").change(function () {
        var panel = $(this).closest(".bw-tabs-panel");
        var panelId = $(this).val();
        panel.find(".bw-tabs-data").hide();
        $('#' + panelId).show();
    }); // ends

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
        var budgetClick = $(this).text();
        var category = GetCatForNav();
        if (category != null) {
            dataLayer.push({ 'event': 'Bikewale_all', 'cat': category, 'act': 'Discover_Your_Bike_Budget', 'lab': budgetClick });
        }
    });

    $("#discoverMileage li a").on("click", function () {
        var mileageClick = $(this).text();
        var category = GetCatForNav();
        if (category != null) {
            dataLayer.push({ 'event': 'Bikewale_all', 'cat': category, 'act': 'Discover_Your_Bike_Mileage', 'lab': mileageClick });
        }
    });

    $("#discoverStyle li a").on("click", function () {
        var styleClicked = $(this).text();
        var category = GetCatForNav();
        if (category != null) {
            dataLayer.push({ 'event': 'Bikewale_all', 'cat': category, 'act': 'Discover_Your_Bike_Style', 'lab': styleClicked });
        }
    });


    $("#bwheader-logo").on("click", function () {
        var categ = GetCatForNav();
        if (categ != null) {
            dataLayer.push({ 'event': 'Bikewale_all', 'cat': categ, 'act': 'Logo', 'lab': 'Logo_Clicked' });
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
            //unlockPopup();
        }
        return false;
    });

    // global-search code 
    $("#global-search").click(function () {
        $("#global-search-popup").show();
        $("#global-search-popup input").focus()
        lockPopup();
    });


    $("#globalSearch").bw_autocomplete({
        source: 1,
        recordCount: 5,
        onClear: function () {
            objBikes = new Object();
        },
        click: function (event, ui, orgTxt) {
            var keywrd = ui.item.label + '_' + $('#globalSearch').val();
            var category = GetCatForNav();
            dataLayer.push({ 'event': 'Bikewale_all', 'cat': category, 'act': 'Search_Keyword_Present_in_Autosuggest', 'lab': keywrd });

            MakeModelRedirection(ui.item);
        },
        loaderStatus: function (status) {
            if (!status) {
                $("#globalSearch").siblings('.fa-spinner').show();
            }
            else {
                $("#globalSearch").siblings('.fa-spinner').hide();
                $("#gs-text-clear").show();
            }
        },
        open: function (result) {
            objBikes.result = result;
        },
        focus: function () {
            if ($('#globalSearch').val().trim() == '') {
                recentSearches.showRecentSearches();
            }
        },
        focusout: function () {
            if ($('li.ui-state-focus a:visible').text() != "") {
                focusedMakeModel = new Object();
                focusedMakeModel = objBikes.result ? objBikes.result[$('li.ui-state-focus').index()] : null;
            }
            else {
                $('#errGlobalSearch').hide();
                recentSearches.hideRecentSearches();
            }
        },
        afterfetch: function (result, searchtext) {
            if (result != undefined && result.length > 0 && searchtext.trim() != "") {
                $('#errGlobalSearch').hide();
                recentSearches.hideRecentSearches();
            }
            else {
                focusedMakeModel = null;
                if (searchtext.trim() != "") {
                    $('#errGlobalSearch').show();
                    recentSearches.hideRecentSearches();
                }

                var keywrd = $('#globalSearch').val();
                var category = GetCatForNav();
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': category, 'act': 'Search_Keyword_Not_Present_in_Autosuggest', 'lab': keywrd });
            }
        },
        keyup: function () {
            if ($('li.ui-state-focus a:visible').text() != "") {
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
                    if ($('#globalSearch').val().trim() != '')
                        $('#errGlobalSearch').show();
                    else
                        $('#errGlobalSearch').hide();
                }
                else {
                    $('#errGlobalSearch').hide();
                    recentSearches.showRecentSearches();
                }


                $("#gs-text-clear").hide();
            }
        }
    }).keydown(function (e) {
        if (e.keyCode == 13) {
            if (focusedMakeModel != null && btnGlobalSearch != undefined)
                btnFindBikeNewNav();
        }

    });

    $("#navbarBtn").on("click", function () {
        var categ = GetCatForNav();
        if (categ != null) {
            triggerGA(categ, 'Hamburger_Menu_Icon', 'Icon_Click');
        }
    });

    // Google Analytics code for Click of Item on Nav_Bar on HP
    $(".navUL ul li").on("click", function () {
        pushNavMenuAnalytics($(this).text());
    });
    $(".navbarTitle").on("click", function () {
        pushNavMenuAnalytics($(this).text());
    });


    $(".lazy").lazyload({
        effect: "fadeIn"
    });

    $("#citySelection").on("click", function () {
        $("#popupContent .bw-city-popup-box").show().siblings("div.bw-area-popup-box").hide();
        popupContent.addClass("open").stop().animate({ 'left': '0px' }, 500);
        $(".user-input-box").stop().animate({ 'left': '0px' }, 500);

    });

    $("#areaSelection").on("click", function () {
        $("#popupContent .bw-city-popup-box").hide().siblings("div.bw-area-popup-box").show();
        popupContent.addClass("open").stop().animate({ 'left': '0px' }, 500);
        $(".user-input-box").stop().animate({ 'left': '0px' }, 500);
    });


    $("#makeSelection").on("click", function () {
        $("#brandcitypopupContent .bw-city-popup-box").show().siblings("div.bw-area-popup-box").hide();
        brandcitypopupContent.addClass("open").stop().animate({ 'left': '0px' }, 500);
        $(".user-input-box").stop().animate({ 'left': '0px' }, 500);

    });

    $("#citiesSelection").on("click", function () {
        $("#brandcitypopupContent .bw-city-popup-box").hide().siblings("div.bw-area-popup-box").show();
        brandcitypopupContent.addClass("open").stop().animate({ 'left': '0px' }, 500);
        $(".user-input-box").stop().animate({ 'left': '0px' }, 500);
    });

    $(".bwm-city-area-popup-wrapper .back-arrow-box").on("click", function () {
        popupContent.removeClass("open").stop().animate({ 'left': '100%' }, 500);
        $(".user-input-box").stop().animate({ 'left': '100%' }, 500);
    });
    $(".bwm-city-area-popup-wrapper .back-arrow-box").on("click", function () {
        brandcitypopupContent.removeClass("open").stop().animate({ 'left': '100%' }, 500);
        $(".user-input-box").stop().animate({ 'left': '100%' }, 500);
    });

});

docReady(function () {

    $('#city-area-popup .white-back-arrow').on('click', function () {
        cityArea.close();
        window.history.back();
    });

    $('#city-area-content').on('click', '#city-menu-tab', function () {
        var tab = $(this),
            tabParent = tab.parent('.city-area-menu'),
            cityAreaContent = $('#city-area-content');

        if (cityAreaContent.hasClass('city-selected')) {
            var areaMenu = $('#area-menu');

            if (!tabParent.hasClass('open')) {
                cityArea.openList(tabParent);
                cityArea.closeList(areaMenu);
                areaMenu.hide();
            }
            else {
                cityArea.closeList(tabParent);
                areaMenu.show();
                cityArea.openList(areaMenu);
            }
        }
    });

    if ($('.swiper-wrapper iframe').length > 0 /*&& iOS != true*/) {

        var tag = document.createElement('script');
        tag.src = "https://www.youtube.com/iframe_api";
        var firstScriptTag = document.getElementsByTagName('script')[0];
        firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);

        // This function takes the existing <iframe> (and YouTube player)
        // with id 'player1' and adds an event listener for state changes.
        var player = new Array(), id, count, countArray = []

        window.onYouTubeIframeAPIReady = function () {
            var i = 1;
            $('.swiper-wrapper iframe').each(function () {
                id = $(this).attr('id');
                //console.log('ids: ' + id);
                player[i] = new YT.Player(id, {
                    events: {
                        'onStateChange': onPlayerStateChange,
                        "onReady": onPlayerReady,
                        "onError": onPlayerError
                    }
                });
                //console.log(player[i]);
                i++;
            });
        }

        function onPlayerStateChange(event) {
            switch (event.data) {
                case YT.PlayerState.UNSTARTED:
                    playerState = 'unstarted';
                    break;
                case YT.PlayerState.ENDED:
                    playerState = 'ended';
                    $('.yt-iframe-preview .overlay').show();
                    break;
                case YT.PlayerState.PLAYING:
                    playerState = 'playing';
                    break;
                case YT.PlayerState.PAUSED:
                    $('.yt-iframe-preview .overlay').show();
                    playerState = 'paused';
                    break;
                case YT.PlayerState.BUFFERING:
                    playerState = 'buffering';
                    break;
                case YT.PlayerState.CUED:
                    playerState = 'cued';
                    break;
            }
        }
        function onPlayerReady(event) { }
        function onPlayerError(event) { alert('error!'); }

        var targetClick, targetOverlay;
        $(document).on('click tap', '.swiper-slide', function (event) {
            targetClick = $(event.target).attr('class');
            //console.log("targetClick: " + targetClick);
            if (targetClick == 'overlay') {
                targetOverlay = $(this).find('span.overlay');
                videoPlay();
            }
        });

        $('.yt-iframe-preview').append('<span class="overlay" />');
        function videoPlay() {
            //console.log(targetOverlay);
            count = targetOverlay.prev().attr('id').replace('video_', '');
            //console.log('count: ' + count);
            player[count].playVideo();
            $('.swiper-slide-active .overlay').hide();
            countArray.push(count);
        };

        function videoPause() {
            for (var j = 0; j < countArray.length; j++) {
                player[countArray[j]].pauseVideo();
            }
            $('.swiper-slide-active .overlay').show();
            countArray = [];
        };


    }

    navDrawer = {
        open: function () {
            navContainer.show(effect, directionLeft, duration, function () {
                navContainer.addClass('drawer-active');
            });
            lockPopup();
        },

        close: function () {
            navContainer.removeClass('drawer-active');
            navContainer.hide(effect, directionLeft, duration, function () { });
            unlockPopup();
        }
    };

    cityArea = {
        popup: $('#city-area-popup'),

        open: function () {
            cityArea.popup.show();
            $('body, html').addClass('lock-browser-scroll');
        },

        close: function () {
            cityArea.popup.hide();
            $('body, html').removeClass('lock-browser-scroll');
        },

        openList: function (wrapper) {
            wrapper.find('.inputbox-list-wrapper').slideDown();
            wrapper.addClass('open');
        },

        closeList: function (wrapper) {
            wrapper.find('.inputbox-list-wrapper').slideUp();
            wrapper.removeClass('open');
        },

        setSelection: function (item) {
            var selectionText = item.text(),
                wrapper = item.closest('.city-area-menu');

            wrapper.find('li').removeClass('active');
            item.addClass('active');
            cityArea.setLabel(selectionText, wrapper);
        },

        setLabel: function (itemText, wrapper) {
            var tabLabel = wrapper.find('.city-area-tab-label');

            if (wrapper.attr('id') == 'city-menu') {
                var areaMenu = $('#area-menu');
                $('#city-area-content').addClass('city-selected');
                tabLabel.text('City: ' + itemText);
                cityArea.closeList(wrapper);
                cityArea.resetLabel('Select your area', areaMenu);
                areaMenu.show();
                cityArea.openList(areaMenu);
                areaMenu.find('li').removeClass('active');
            }
            else {
                tabLabel.text('Area: ' + itemText);
                $('#city-area-popup .white-back-arrow').trigger('click');
            }
        },

        resetLabel: function (message, wrapper) {
            wrapper.find('.city-area-tab-label').text(message);
        },
    };

    $("#btnCrossApp").click(function () {
        $("#appBanner").slideUp();
        SetCookieInDays("AppBanner", "hide", 30);
        dataLayer.push({ 'event': 'Bikewale_all', 'cat': GetCatForNav(), 'act': 'Close_Clicked_App_Download_Banner' });
    });

    $("#btnInstallApp").click(function () {
        $("#appBanner").slideUp();
        SetCookieInDays("AppBanner", "hide", 30);
        dataLayer.push({ 'event': 'Bikewale_all', 'cat': GetCatForNav(), 'act': 'InstallApp_Clicked_App_Download_Banner' });
    });
    $(window).on('hashchange', function (e) {
        hashChange(e);
    });

    navContainer = $("#nav");

    $('#city-menu-input').on('focus click', function (event) {
        event.stopPropagation();
        $("#city-area-popup").animate({ scrollTop: 147 });
    });

    $('#area-menu-input').on('focus click', function (event) {
        event.stopPropagation();
        $("#city-area-popup").animate({ scrollTop: 190 });
    });

    $('#navbarBtn').on('click', function () {
        navDrawer.open();
        appendState('filter');
    });

    $(".blackOut-window").mouseup(function (event) {
        if (event.target.id !== navContainer.attr('id') && !navContainer.has(event.target).length && navContainer.hasClass('drawer-active')) {
            history.back();
            navDrawer.close();
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

    $(window).on('popstate', function (event) {
        if ($('#nav').is(':visible')) {
            navDrawer.close();
        }
        if ($('#leadCapturePopup').is(':visible')) {
            $('#leadCapturePopup').find('.leadCapture-close-btn').trigger('click');
        }
    });


    recentSearches =
    {
        searchKey: "recentsearches",
        options: {
            homeSearchEle: $('#newBikeList'),
            bikeSearchEle: $('#globalSearch'),
            recentSearchesEle: $("#new-global-recent-searches").length ? $("#new-global-recent-searches") : $("#global-recent-searches"),
            recentSearchesLoaded: false
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
                if (objSearches.searches.length > 5)
                    objSearches.searches.pop();
                objSearches["noOfSearches"] = objSearches.searches.length;
                bwcache.set(this.searchKey, objSearches);
            }
        },
        showRecentSearches: function () {
            if (!this.options.recentSearchesLoaded) {
                var objSearches = bwcache.get(this.searchKey);
                if (objSearches && objSearches.searches) {
                    var html = "", bikename, url;
                    var i = 0;
                    for (var item in objSearches.searches) {
                        item = objSearches.searches[item];
                        bikename = item.name || '';
                        if (bikename != '' && $("#global-recent-searches li[data-modelid='" + item.modelId + "']").length == 0 && i < 3) {
                            html += '<li data-makeid="' + item.makeId + '" data-modelid="' + item.modelId + '" class="ui-menu-item" tabindex="' + i++ + '"><span class="bwmsprite history-icon"></span><a href="javascript:void(0)" data-href="/m/' + item.makeMaskingName + '-bikes/' + item.modelMaskingName + '" optionname="' + bikename.toLowerCase().replace(' ', '') + '">' + bikename + '</a>';
                            if (item.modelId > 0) {
                                if (item.futuristic == 'True') {
                                    html += '<span class="upcoming-link">coming soon</span>';
                                } else {
                                    if (item.isNew == 'True') {
                                        html += '<a data-pqSourceId="' + pqSourceId + '" data-modelId="' + item.modelId + '" class="getquotation target-popup-link" onclick="setPriceQuoteFlag()">Check On-Road Price</a>';
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
                        this.options.recentSearchesEle.find("li:first-child").addClass("ui-state-focus").siblings().removeClass("ui-state-focus");
                    }

                }
            }
            this.options.recentSearchesEle.slideDown(100);
            if (this.options.recentSearchesEle.is(":visible")) {
                var rsele = this.options.recentSearchesEle.find("li.ui-state-focus");
                if (event.keyCode == 27) {
                    this.hideRecentSearches();
                }
            }
            return false;

        },
        hideRecentSearches: function () {
            this.options.recentSearchesEle.slideUp(100).find("li:first-child").addClass("ui-state-focus").siblings().removeClass("ui-state-focus");

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
                window.location.href = $(this).find('a').first().attr('data-href');
            }

            recentSearches.hideRecentSearches();

        } catch (e) {
            console.log(e.message);
        }

    });

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
                            //request.abort();
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
                // errorLog(error);
            } catch (e) {
                return false;
            }
        };

        window.errorLog = errorLog;
    })();

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

    // more brand - collapse
    $('.view-brandType').click(function () {
        var element = $(this),
            elementParent = element.closest('.collapsible-brand-content'),
            moreBrandContainer = elementParent.find('.brandTypeMore');

        if (!moreBrandContainer.is(':visible')) {
            moreBrandContainer.slideDown();
            element.attr('href', 'javascript:void(0)');
            element.addClass('active').find('.btn-label').text('View less brands');
        }
        else {
            element.attr('href', '#brand-type-container');
            moreBrandContainer.slideUp();
            element.removeClass('active').find('.btn-label').text('View more brands');
        }
    });

    $(".brand-collapsible-present li").click(function () {
        var tabsPanel = $(this).closest('.bw-tabs-panel'),
            collapsibleBrand = tabsPanel.find('.collapsible-brand-content'),
            moreBrandContainer = collapsibleBrand.find('.brandTypeMore'),
            viewMoreBtn = collapsibleBrand.find('.view-brandType');

        viewMoreBtn.attr('href', '#brand-type-container');
        moreBrandContainer.slideUp();
        viewMoreBtn.removeClass('active').find('.btn-label').text('View more brands');
    });

    // read more - collapse
    $(document).on('click','.read-more-target', function () {
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

});