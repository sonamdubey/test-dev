var focusedMakeModel = null, focusedCity = null;
var objBikes = new Object();
var objCity = new Object();
var globalCityId = 0;
var _makeName = '';
var pqSourceId = "38";
var globalCityPQSource = "125"
var IsPriceQuoteLinkClicked = false;
var ga_pg_id = '0', recentSearches, navDrawer, cityArea, quotationPage, playerState = '';
var navContainer, effect = 'slide', directionLeft = { direction: 'left' }, duration = 500;
var popupHeading, popupContent, brandcitypopupContent;
var trendingBikes, objSearches;
var topCount = 5;
var pageName = typeof (gaObj) === 'undefined' ? 'Others' : gaObj.name;
var bhriguPageName = typeof (gaObj) === 'undefined' ? 'Others' : gaObj.bhriguPageName;
var fullShown = false, partialShown = false;

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

function triggerVirtualPageView(host, path, title) {
    try {
        dataLayer.push({
            'event': 'VirtualPageview',
            'virtualPageHost': host,
            'virtualPagePath': path,
            'virtualPageTitle': title
        })
    } catch (e) {

    }
}

function triggerNonInteractiveGA(cat, act, lab) {
    try {

        dataLayer.push({ 'event': 'Bikewale_noninteraction', 'cat': cat, 'act': act, 'lab': lab });
    }
    catch (e) {// log error   
    }
}

function triggerGAAnimateCTA() {
    var container = $('.cta-animation-container');
    var campaignContainer = $('.campaign-with-animation');
    var campaignContainerHeight = campaignContainer.outerHeight();
    var windowScrollTop = $(window).scrollTop() + $(window).innerHeight();
    var containerScrollTop = container.offset().top + container.height() + campaignContainerHeight;
    var sellingPitchText = campaignContainer.find('#topSellingBlock').text().trim();
    if ($(window).scrollTop() === 0 || windowScrollTop < containerScrollTop) {
        if (gaObj.id == gaEnum.Model_Page) {
            triggerNonInteractiveGA("Model_Page", "FloatingLeadCTA_FullWidth_Shown", "");
        }
        cwTracking.trackCustomData(bhriguPageName, "ES_FloatingLeadCTA_FullWidthShown", "versionId=" + versionId + (sellingPitchText == undefined || sellingPitchText == "" ? "" : "|LeadSupportingText=" + sellingPitchText));
        fullShown = true;
    }
    else if ($('.campaign-with-animation').hasClass("animated")) {
        if (gaObj.id == gaEnum.Model_Page) {
            triggerNonInteractiveGA("Model_Page", "FloatingLeadCTA_Partial_GetBestOffers_Shown", "");
        }
        cwTracking.trackCustomData(bhriguPageName, "ES_FloatingLeadCTA_AnimatedShown", "versionId=" + versionId + (sellingPitchText == undefined || sellingPitchText == "" ? "" : "|LeadSupportingText=" + sellingPitchText));
        partialShown = true;
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

                            if (options.source == '7' && $('#nonUpcomingBikes').attr('data-contentTab') == "expertReview") {
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
                          .append('<span class="ui-search-icon"></span><a OptionName=' + item.label.replace(/\s/g, '').toLowerCase() + '>' + __highlight(item.label, reqTerm) + '</a>');
            if (options.source == '5') {
                ulItem.append(' <span class="ui-menu-item-info">(' + item.payload.userRatingsCount + ' Ratings)</span>')
            }
            if (options.source == '7' && $('#nonUpcomingBikes').attr('data-contentTab') == "userReview" && parseInt(item.payload.userRatingsCount) > 0) {
                ulItem.append(' <span class="ui-menu-item-info">(' + item.payload.userRatingsCount + ' Ratings)</span>')
            }
            else if (options.source == '7' && $('#nonUpcomingBikes').attr('data-contentTab') == "expertReview" && parseInt(item.payload.expertReviewsCount) > 0) {
                ulItem.append(' <a href="javascript:void(0)"  class="target-popup-link" rel="nofollow">Read review</a>')
            }
            else if (options.source == '7') {
                ulItem.closest('li').addClass('event-none');
                ulItem.append(' <span class="ui-menu-item-info text-grey">Not reviewed yet</span>')
            }
            if (options.source == '8') {
                var suffixText = (parseInt(item.payload.photosCount) > 1) ? ' Photos' : ' Photo';
                ulItem.append(' <span class="ui-menu-item-info">(' + item.payload.photosCount.toString() + suffixText + ')</span>')
            }
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
        $("#terms").load("/UI/statichtml/tnc.html");
    }
    $('#termspinner').hide();
}


function CloseCityPopUp() {
    $("#globalcity-popup").hide();
    unlockPopup();
}

function areaSelectionClick() {
  $("#popupContent .bw-city-popup-box").hide().siblings("div.bw-area-popup-box").show();
  popupContent.addClass("open").stop().animate({ 'left': '0px' }, 500);
  $(".user-input-box").stop().animate({ 'left': '0px' }, 500);
};

var appendState = function (state) {
    window.history.pushState(state, '', '');
};

docReady(function () {
    $(".bw-anchor-link").click(function () {
        window.location = $(this).data("href");
    });

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
            if (typeof (vmquotation) != "undefined" && vmquotation.ExitUrl() != "")
            {
                window.location.href = vmquotation.ExitUrl();
            }
            else
            {
                window.history.back();
            }
        });
    }

    //Scroll To Top function
    var scrollToTop = document.querySelectorAll('.back-to-top')[0];

    if (scrollToTop) {
        attachListener('scroll', window, toggleScrollToTopBtn);
    }

    function attachListener(event, element, functionName) {
        if (element.addEventListener) {
            element.addEventListener(event, functionName, false);
        }
        else if (element.attachEvent) {
            element.attachEvent('on' + event, functionName);
        }
    };

    function toggleScrollToTopBtn() {
        if ($(this).scrollTop() > 180) {
            $('.back-to-top').fadeIn(500);
        } else {
            $('.back-to-top').fadeOut(500);
        }
    };

    $('.back-to-top').click(function (event) {
        $('html, body').stop().animate({ scrollTop: 0 }, 600);
        event.preventDefault();
    });

    popupHeading = $("#popupHeading"), popupContent = $("#popupContent"), brandcitypopupContent = $("#brandcitypopupContent");



    /* Swiper custom methods */

    var iOS = /iPad|iPhone|iPod/.test(navigator.userAgent) && !window.MSStream;

    var slideCount = function (swiper) {
        try {
            imgTotalCount = swiper.slides.length;
            $(document).find('.bike-model-gallery-count').text(swiper.activeIndex + 1 + " of " + imgTotalCount.toString());
        } catch (e) { }
    };

    $('.swiper-container:not(".noSwiper")').each(function (index, element) {
    	var currentSwiper = $(this);
    	var spaceBetween = currentSwiper.attr('data-spacebetween');
    	var spaceBetweenValue = 10;

    	if (spaceBetween) {
    		spaceBetweenValue = isNaN(spaceBetween) ? spaceBetweenValue : Number(spaceBetween);
    	}

    	currentSwiper.addClass('sw-' + index).swiper({
    		effect: 'slide',
    		speed: 300,
    		nextButton: currentSwiper.find('.swiper-button-next'),
    		prevButton: currentSwiper.find('.swiper-button-prev'),
    		pagination: currentSwiper.find('.swiper-pagination'),
    		slidesPerView: 'auto',
    		paginationClickable: true,
    		spaceBetween: spaceBetweenValue,
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

    //$('#newBikeList').val('').focus();
    $('#globalCityPopUp').val('');

    $(".fa-spinner").hide();

    CheckGlobalCookie();

    $('.globalcity-close-btn').click(function () {
        CloseCityPopUp();
        window.history.back();
    });

    $(".onroad-price-close-btn").click(function () {
        var rUrl = $(this).data("returnurl");
        if (typeof (vmquotation) != "undefined" && vmquotation.isLeadPopupFlow())
        {
            closeOnRoadPricePopUp();
        }
        else {            
            if (rUrl && rUrl != "")
                window.location = rUrl;
            else {
                closeOnRoadPricePopUp();
                window.history.back();
            }
        }            
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

    $('#newBikeList').on('click', function () {
    	$('#global-search').trigger('click');
    });

	/*
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
            $("ul.ui-menu").width($('#newBikeList').innerWidth());
            if (navigator.userAgent.match(/(iPod|iPhone|iPad)/)) {
                $('.ui-autocomplete').off('menufocus hover mouseover');
            }
        },
        focus: function () {
            if ($('#newBikeList').val().trim() == '') {
                $('#errNewBikeSearch').hide();
                recentSearches.showRecentSearches();
                // showRecentSearches captures recentSearchesLoaded if any searchdata avaliable in local Storage
                var label = "Recently_Viewed_Bikes_" + (recentSearches.options.recentSearchesLoaded ? "Present" : "Not_Present");
                triggerGA('HP', 'Search_Bar_Clicked', label);
			}

			$('html, body').animate({
				scrollTop: $('#newBikeList').offset().top - 20 // 20px offset value
			})
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
            if (result != undefined && result.length > 0 && searchtext.trim()) {
                $('#errNewBikeSearch').hide();
                recentSearches.hideRecentSearches();
                NewBikeSearchResult = true;
            }
            else {
                focusedMakeModel = null; NewBikeSearchResult = false;
                if (searchtext.trim() != '') {
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
                    if ($('#newBikeList').val().trim() != '') {
                        $('#errNewBikeSearch').show();
                        recentSearches.hideRecentSearches();
                    }

                }
                else {
                    $('#errNewBikeSearch').hide();
                    recentSearches.hideRecentSearches();
                }

            }
        }
    }).css({ 'width': '100%' });
	*/

	/*
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
        if (id.length > 0) {
            var searchVal = id.val().trim();
            var placeHolder = id.attr('placeholder').trim();
            triggerGA('HP', 'Search_Not_Keyword_Present_in_Autosuggest', searchVal);
            if (btnFindBikeNewNav() === false || searchVal === placeHolder || searchVal === "") {
                $('#errNewBikeSearch').show();
                return false;
            } else {
                return true;
            }
        }
    });
	*/

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
            dataLayer.push({ 'GlobalCity': cityName });
            ga('set', 'dimension3', cityName);
            if (city.cityId) {
                if (typeof (gaObj) != "undefined" && gaObj.id == 3) // Model page
                {
                    window.location.href = "?versionId=" + versionId + "&pqsourceid=" + globalCityPQSource;
                }
                else {
                    location.reload();
                }
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
            for(i=0; i<swiperContainer.length; i++)
            {
                var sIndex = $(swiperContainer[i]).attr('class');
                var regEx = /sw-[0-9]+/i;
                try {
                    var index = regEx.exec(sIndex)
                    $('.' + index).data('swiper').update(true);
                } catch (e) { }
            }

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
            $("ul.ui-menu").width($('#globalSearch').innerWidth());
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
            if ($('li.ui-state-focus a:visible').text() != "") {
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

    $("#areaSelection").on("click", areaSelectionClick);


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

    $(".bwm-city-area-popup-wrapper .back-arrow-box, #popupCityList li, #popupAreaList li").on("click", function () {
      brandcitypopupContent.removeClass("open").stop().animate({ 'left': '100%' }, 500);
      popupContent.removeClass("open").stop().animate({ 'left': '100%' }, 500);
      $(".user-input-box").stop().animate({ 'left': '100%' }, 500);
    });


    (function () {
        var elemList = document.querySelectorAll(".js-ga-non-interactive");
        var len = elemList.length;
        for (var i = 0 ; i < len ; i++) {
            triggerNonInteractiveGA(elemList[i].getAttribute("data-cat"), elemList[i].getAttribute("data-act"), elemList[i].getAttribute("data-lab"));
        }
    })();

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
      var windowState =  window.history.state;
        if ($('#nav').is(':visible')) {
            navDrawer.close();
        }
        if ($('#leadCapturePopup').is(':visible') && windowState !== "leadCapture" && windowState !== "fetchCampaign") {
            $('#leadCapturePopup').find('.leadCapture-close-btn').trigger('click');
        }
        if ($(".city-popup-wrapper").is(':visible') && windowState !== "fetchCampaign" && windowState !== "onRoadPrice") {
            $('#popupWrapper .close-btn').trigger('click');
        }
    });


    recentSearches =
    {
        searchKey: "recentsearches",
        options: {
            homeSearchEle: $('#newBikeList'),
            bikeSearchEle: $('#globalSearch'),
            globalSearchSection: $('#new-global-search-section').length ? $('#new-global-search-section') : $('#global-search-section'),
            recentSearchesEle: $("#new-global-recent-searches").length ? $("#new-global-recent-searches") : $("#global-recent-searches"),
            trendingSearchesEle: $("#new-trending-bikes").length ? $("#new-trending-bikes") : $("#trending-bikes"),
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
        showRecentSearches: function (event) {
            var html = "";
            if (!this.options.recentSearchesLoaded) {
                objSearches = bwcache.get(this.searchKey);
                if (objSearches && objSearches.searches) {
                    var bikename, url;
                    var i = 0;
                    for (var item in objSearches.searches) {
                        item = objSearches.searches[item];
                        bikename = item.name || '';
                        if (bikename != '' && $("#global-recent-searches li[data-modelid='" + item.modelId + "']").length == 0 && i < 3) {
                            html += '<li data-makeid="' + item.makeId + '" data-modelid="' + item.modelId + '" class="ui-menu-item" tabindex="' + i++ + '"><span class="recent-clock"></span><a href="javascript:void(0)" data-href="/m/' + item.makeMaskingName + '-bikes/' + item.modelMaskingName + '" optionname="' + bikename.toLowerCase().replace(' ', '') + '">' + bikename + '</a>';
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
                    }
                }
            }

            if (!this.options.trendingSearchesLoaded) {
                if (trendingBikes) {
                    var html = "";
                    for (var index in trendingBikes) {
                        item = trendingBikes[index];
                        html += '<li data-makeid="' + item.objMake.makeId + '" data-modelid="' + item.objModel.modelId + '" class="ui-menu-item bw-ga" data-cat="' + pageName + '" data-act="Trending_Searches_Search_Bar_Clicked" data-lab="' + item.BikeName
                                + '"><span class="trending-searches"></span><a href="javascript:void(0)" data-href="/m/' + item.objMake.maskingName + '-bikes/' + item.objModel.maskingName + '" optionname="' + item.BikeName.toLowerCase().replace(' ', '') + '">' + item.BikeName + '</a>';
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
                $('#trending-search').addClass('hide');
            } else {
                $('#trending-search').removeClass('hide');
            }

            if (!(objSearches && objSearches.searches)) {
                $('#history-search').addClass('hide');
            } else {
                $('#history-search').removeClass('hide');
            }

            this.options.globalSearchSection.removeClass('hide');
            this.options.recentSearchesEle.find("li:first-child").addClass("ui-state-focus").siblings().removeClass("ui-state-focus");
            this.options.trendingSearchesEle.find("li:first-child").addClass("ui-state-focus").siblings().removeClass("ui-state-focus");
            if (this.options.recentSearchesEle.is(":visible")) {
                var rsele = this.options.recentSearchesEle.find("li.ui-state-focus");
                if (typeof event !== "undefined" && event.keyCode == 27) {
                    this.hideRecentSearches();
                }
            }
            return false;

        },
        hideRecentSearches: function () {
            this.options.globalSearchSection.addClass('hide');
        },
        objectIndexOf: function (arr, opt) {
            var makeId = opt.makeId, modelId = opt.modelId;
            for (var i = 0, len = arr.length; i < len; i++)
                if (arr[i]["makeId"] === opt.makeId && arr[i]["modelId"] === opt.modelId) return i;
            return -1;
        }
    };

    recentSearches.options.recentSearchesEle.on('click', 'li', function (event) {
        try {
            if (!$(event.target).hasClass('getquotation')) {

                var objSearches = bwcache.get(recentSearches.searchKey) || {}, mkId = this.getAttribute("data-makeid"), moId = this.getAttribute("data-modelid"),
                    eleIndex = recentSearches.objectIndexOf(objSearches.searches, { makeId: mkId, modelId: moId }),
                    obj = objSearches.searches[eleIndex];
                if (objSearches.searches != null && eleIndex > -1) objSearches.searches.splice(eleIndex, 1);
                objSearches.searches.unshift(obj);
                bwcache.set(recentSearches.searchKey, objSearches);
                triggerGA(pageName, 'Recently_View_Search_Bar_Clicked', this.textContent);
                window.location.href = $(this).find('a').first().attr('data-href');
            }

            recentSearches.hideRecentSearches();

        } catch (e) {
            console.log(e.message);
        }

    });

    recentSearches.options.trendingSearchesEle.on('click', 'li', function (event) {
        try {
            if (!$(event.target).hasClass('getquotation')) {
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
            var log_source = new RegExp(["aeplcdn", "bikewale"].join('|'));
            try {
                if (filename && filename.match(log_source)) {
                    error.Details = err.message || message || "";
                    error.SourceFile = err.fileName || filename || "";
                    error.ErrorType = err.name || "Uncatched Exception";
                    error.LineNo = lineno || "Unable to trace";
                    error.Trace = (err.stack.toString() || '-');
                    errorLog(error);
                }
            }
            catch (e) {
                return false;
            }
        };

        window.errorLog = errorLog;
    })();
   
    // Stop default achor propgation and redirect to custom link
    $(".redirect-url").click(function (ev) {
        ev.preventDefault();
        window.location.href = $(this).attr('data-url');
    });

    $(document).on("click", ".bw-ga", function () {
        try {
            var obj = $(this);
            var category = obj.attr("data-cat") || obj.attr("c") || $('body').data('page-name') || pageName;
            var action = obj.attr("data-act") || obj.attr("a");
            var label = obj.attr("data-lab") !== undefined ? obj.attr("data-lab") : obj.attr("l");
            var variable = obj.attr("data-var") !== undefined ? obj.attr("data-var") : obj.attr("v");
            var funct = obj.attr("data-func") !== undefined ? obj.attr("data-func") : obj.attr("f"); 
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

    $(document).on("click", ".bw-bhrigu", function () {
        try {
            var obj = $(this);
            var category = bhriguPageName || obj.attr("data-bhrigucat");
            var action = obj.attr("data-bhriguact");
            var label = obj.attr("data-bhrigulab") || "NA";
            
            cwTracking.trackCustomData(category, action, label);   
        }
        catch (e) {
            console.error(e);
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

    function modelCountDown() {
        var remainingTime = setModelCountdownTime();

        // check if 1 minute is remaining, since we are not showing 'seconds' in countdown
        if (remainingTime > 60000) {
            $(".model-countdown").addClass("countdown--active");

            // Update the count down every 1 second
            var timer = setInterval(function () {
                var remainingTime = setModelCountdownTime();
                if (remainingTime < 60000) {
                    clearInterval(timer);
                    $(".model-countdown").removeClass("countdown--active");
                }
            }, 1000);

            $(".model-countdown__close").click(function () {
                $(".model-countdown").removeClass("countdown--active");
            });

            triggerNonInteractiveGA('Ticker', 'Ticker_RE Twins 650_shown', 'Royal Enfield Twins 650');
        }
    }

    function setModelCountdownTime() {
        // Set the date we're counting down to
        // date = date + 1 minute, since we are not showing 'seconds' in countdown
        var countDownDate = new Date("Nov 14, 2018 18:31:00").getTime();

        // Get todays date and time
        var now = new Date().getTime();

        // Find the distance between now an the count down date
        var distance = countDownDate - now;

        // Time calculations for days, hours, minutes and seconds
        var days = Math.floor(distance / (1000 * 60 * 60 * 24));
        var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));

        // Output the result in an element with id="demo"
        document.getElementById("days").innerHTML = days;
        document.getElementById("hours").innerHTML = hours;
        document.getElementById("minutes").innerHTML = minutes;

        return distance;
    }

    if ($('#modelCountdownContainer').length) {
        modelCountDown();
    }
});

//body lock with same position
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

String.isNullOrEmpty = function (str) {
    return (!str || str.length === 0);
};

var trackElementVisiblity = function (element) {
  if (element != null && element.dataset != null) {
    var bhriguCategory = element.dataset["bhrigucat"];
    var bhriguAction = element.dataset["bhrigushownact"];
    var bhriguLabel = element.dataset["bhrigulab"] || "NA";
    cwTracking.trackCustomData(bhriguCategory, bhriguAction, bhriguLabel);
  }
};
var triggerNonInteractiveGAWithinViewport = function (elem) {
    

    for (var i = 0 ; i < elem.length ; i++)
    {
        var rect = elem[i].getBoundingClientRect();
        if (elem[i].isGATriggered == false && rect.top >= 0 && (rect.top + rect.height) <= window.innerHeight)
        {
            triggerNonInteractiveGA(elem[i].getAttribute("data-cat"), elem[i].getAttribute("data-act"), elem[i].getAttribute("data-lab"));
            elem[i].isGATriggered = true;        
        }
    }
    
}

var trackPQSources = function (pqId, pqSourceId, platformId, bikeVersionId) {
    var category = "BWPriceQuote";
    var action = "PQTaken";
    var label = "pqId=" + pqId + "|pqSourceId=" + pqSourceId + "|platformId=" + platformId + "|versionId=" + bikeVersionId;

    cwTracking.trackCustomData(category, action, label);

}

var getFilterFromQS = function (name) {
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
        //if (value.indexOf('+') > 0)
        if ((/\+/).test(value))
            return value.replace(/\+/g, " ");
        else
            return value;
    }
    else
        return "";
};


