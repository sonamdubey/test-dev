
var Category = {
    NewLaunches: 2,
    TopSelling: 3,
    Upcoming: 1
}

var PriceStatusEnum = {
    PriceAvailable: 1,
};

var SaveUserHistory = function () {
    var _self = this;
    _self.trackUserHistory = function (modelIdArray) {
        try {
            if (isCookieExists('_userModelHistory')) {
                var userHistory = _self.getUserModelHistory();
                var userHistoryArray = userHistory.split(",");

                userHistoryArray = _self.insertIntocookie(userHistoryArray, modelIdArray);

                document.cookie = '_userModelHistory=' + userHistoryArray.join("~") + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
            } else {

                document.cookie = '_userModelHistory=' + modelIdArray.join("~") + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';

                var userHistory = _self.getUserModelHistory();
                var userHistoryArray = userHistory.split("~");

                if (userHistoryArray.length > 20) {
                    userHistoryArray.splice(0, modelArrayLen - 20);
                    document.cookie = '_userModelHistory=' + userHistoryArray.join("~") + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
                }
            }
        }
        catch (e) {
            console.log("exception in track user history");
        }
    };

    _self.insertIntocookie = function (userHistoryArray, modelIdArray) {
        try {
            var modelArrayLen = modelIdArray.length;
            var isModelInserted;

            for (var modelIndex = 0; modelIndex < modelArrayLen; modelIndex++) {
                isModelInserted = false;
                for (var userHisIndex = 0; userHisIndex < userHistoryArray.length; userHisIndex++) {
                    if (modelIdArray[modelIndex] == userHistoryArray[userHisIndex]) {
                        userHistoryArray.splice(userHisIndex, 1);
                        userHistoryArray.push(modelIdArray[modelIndex]);
                        isModelInserted = true;
                        break;
                    }
                }
                if (!isModelInserted) {
                    if (userHistoryArray.length == 20) {
                        userHistoryArray.splice(0, 1);
                    }
                    userHistoryArray.push(modelIdArray[modelIndex]);
                }
            }
            return userHistoryArray;
        }
        catch (e) {
            console.log("exceptionin insert into cookie");
        }
    };

    _self.getUserModelHistory = function () {
        if (isCookieExists('_userModelHistory')) {
            var userHistoryString = $.cookie('_userModelHistory');
            var userHistory = userHistoryString.split('~').join(',');
            return userHistory;
        } else {
            return "";
        }
    };
};

var userHistory = new SaveUserHistory();

var showHideMatchError = function (error, TargetId, errText) {
    if (error) {
        $(TargetId).siblings('.error-icon').removeClass('hide');
        $(TargetId).siblings('.cw-blackbg-tooltip').removeClass('hide').text(errText);
        $(TargetId).addClass('border-red');
    }
    else {
        $(TargetId).siblings().addClass('hide');
        $(TargetId).removeClass('border-red');
    }
}

var ModelCar = {

    PQ: {
        optGroup: "",
        optGroup1: "",
        cityDrp: "",

        bindZones: function (divToBind, drpCity) {
            if (ModelCar.PQ.checkForCity("1", drpCity, divToBind)) {// Mumbai
                ModelCar.PQ.removeRepeatedCities("1", drpCity);
                ModelCar.PQ.optGroup = $("<optgroup label = 'Mumbai'/>");
                ModelCar.PQ.optGroup.append("<option value=" + 1 + " ZoneId = '-1' >" + 'Mumbai' + "</option>");

                if (ModelCar.PQ.checkForCity("40", drpCity, divToBind)) { // Thane
                    ModelCar.PQ.removeRepeatedCities("40", drpCity);
                    ModelCar.PQ.optGroup.append("<option value=" + 40 + " >" + 'Thane' + "</option>");
                }

                if (ModelCar.PQ.checkForCity("13", drpCity, divToBind)) { //Navi Mumbai
                    ModelCar.PQ.removeRepeatedCities("13", drpCity);
                    ModelCar.PQ.optGroup.append("<option value=" + 13 + " >" + 'Navi Mumbai' + "</option>");
                }

                if (ModelCar.PQ.checkForCity("395", drpCity, divToBind)) { //Navi Mumbai
                    ModelCar.PQ.removeRepeatedCities("395", drpCity);
                    ModelCar.PQ.optGroup.append("<option value=" + 395 + " >" + 'Vasai-Virar' + "</option>");
                }

                if (ModelCar.PQ.checkForCity("6", drpCity, divToBind)) { // Kalyan
                    ModelCar.PQ.removeRepeatedCities("6", drpCity);
                    ModelCar.PQ.optGroup.append("<option value=" + 6 + " >" + 'Kalyan-Dombivali' + "</option>");
                }

                if (ModelCar.PQ.checkForCity("8", drpCity, divToBind)) { // Panvel
                    ModelCar.PQ.removeRepeatedCities("8", drpCity);
                    ModelCar.PQ.optGroup.append("<option value=" + 8 + " >" + 'Panvel' + "</option>");
                }
            }

            if (ModelCar.PQ.checkForCity("10", drpCity, divToBind)) {// New Delhi
                ModelCar.PQ.removeRepeatedCities("10", drpCity);
                ModelCar.PQ.optGroup1 = $("<optgroup label = 'Delhi NCR'/>");
                ModelCar.PQ.optGroup1.append("<option value=" + 10 + " ZoneId = '-1'>" + 'Delhi' + "</option>");

                if (ModelCar.PQ.checkForCity("246", drpCity, divToBind)) { // Gurgoan
                    ModelCar.PQ.removeRepeatedCities("246", drpCity);
                    ModelCar.PQ.optGroup1.append("<option value=" + 246 + " >" + 'Gurgaon' + "</option>");
                }

                if (ModelCar.PQ.checkForCity("224", drpCity, divToBind)) { // Noida
                    ModelCar.PQ.removeRepeatedCities("224", drpCity);
                    ModelCar.PQ.optGroup1.append("<option value=" + 224 + " >" + 'Noida' + "</option>");
                }

                if (ModelCar.PQ.checkForCity("225", drpCity, divToBind)) { // Ghaziabad
                    ModelCar.PQ.removeRepeatedCities("225", drpCity);
                    ModelCar.PQ.optGroup1.append("<option value=" + 225 + " >" + 'Ghaziabad' + "</option>");
                }

                if (ModelCar.PQ.checkForCity("273", drpCity, divToBind)) { // Faridabad
                    ModelCar.PQ.removeRepeatedCities("273", drpCity);
                    ModelCar.PQ.optGroup1.append("<option value=" + 273 + " >" + 'Faridabad' + "</option>");
                }
            }

            if (drpCity == 'drpPqCity') {
                ModelCar.PQ.cityDrp = $('#' + divToBind).find(drpCity);
            }
            else {
                ModelCar.PQ.cityDrp = $(drpCity);
            }
            ModelCar.PQ.cityDrp.prepend('<option value=-2>-------------</option>');
            ModelCar.PQ.cityDrp.prepend(ModelCar.PQ.optGroup1);
            ModelCar.PQ.cityDrp.prepend(ModelCar.PQ.optGroup);
        },

        checkForCity: function (cityId, drpCity, divToBind) {
            if (drpCity == '#drpPqCity') {
                if ($('#' + divToBind).find(drpCity + " option[value='" + cityId + "']").length > 0)
                    return true;
                else
                    return false;
            } else {
                if ($(drpCity + " option[value='" + cityId + "']").length > 0)
                    return true;
                else
                    return false;
            }
        },

        preselectDropDownOption: function (drpId, val, attr) {
            $('#' + drpId + ' option[value=' + val + ']').attr('selected', 'selected');
        },

        preselectPQDropDown: function (modelCity, isCityPage) {
            if (typeof (isCityPage) == "undefined")
            {
                isCityPage = false;
            }
            if (isCookieExists('_CustZoneIdMaster')) {
                ModelCar.PQ.preselectDropDownOption(modelCity, $.cookie("_CustCityIdMaster"), $.cookie('_CustZoneIdMaster'));
            }
            else {
                isCookieExists('_CustCityIdMaster') && !isCityPage ? ModelCar.PQ.preselectDropDownOption(modelCity, $.cookie("_CustCityIdMaster")) : "";
            }

        },

        removeRepeatedCities: function (cityId, drpCity) {
            $(drpCity + " option[value='" + cityId + "']").remove();
        },

        city: {
            MUMBAI: 1,
            DELHI: 10,
            BANGALORE: 2 //TODO:Bangalore Zone Refactoring
        },
        /* Ad Animation Code Starts Here */
        animateSlug: function () {
            $('.pq-data-highlight-call').addClass('flipinx').delay(1000).queue(function (next) {
                ModelCar.PQ.noAnimateSlug();
                next();
                $('.tabs li, #pqTabs .close-btn').click(function () {
                    ModelCar.PQ.noAnimateSlug();
                });
            });
        },
        noAnimateSlug: function () {
            $('.pq-data-highlight-call').removeClass("flipinx");
        },
        animateSlugPageId: [15, 16],
        stopAnimateSlugPageId: [17],
        isAnimateSlugShown: function (pageId) {
            return $.inArray(Number(pageId), ModelCar.PQ.animateSlugPageId) >= 0;
        },
        isAnimateSlugStop: function (pageId) {
            return $.inArray(Number(pageId), ModelCar.PQ.stopAnimateSlugPageId) >= 0;
        },
        /* Ad Animation Code Ends Here */
        pageId: {
            isValidPageId: function (pageId) {
                var arrPageIds = [15, 16, 36, 17, 59, 115]; // Here 115 is dealer locator pqPageId
                if ($.inArray(pageId, arrPageIds) > -1)
                    return true;
                else
                    return false;
            },

            isValidPQPageId: function (pageId) {
                var arrPageIds = [15, 16, 17, 20, 36, 59];
                if ($.inArray(pageId, arrPageIds) > -1)
                    return true;
                else
                    return false;
            }
        }
    },
    Recommendation: {
        leadClickSource: [100, 118, 119, 120, 101, 104, 109, 110, 131, 137, 138, 130, 133, 128, 129, 155, 156, 157, 158, 159, 160, 180, 212, 214, 215, 216, 220, 302, 304, 307, 341, 360], // Created for showing recommendation specific to these lead click source

        isRecommendationAvailable: function (leadClickSource) {
            return $.inArray(Number(leadClickSource), ModelCar.Recommendation.leadClickSource) >= 0;
        }
    }
}
var Tracking = {
    versionPage: {
        inBodyTestDriveSlug: function () {
            Common.utils.trackAction("DealerLeadPopUpBehaviour", "TestDriveSlugVersions", "ButtonShown");
        }
    }
}

var CompareCars = {
    hotCompareNextCount: 1,
    hotComparePrevCount: 1,
    hotCompareNextPageNo: 1,
    _pageSize: 6,
    isAdBlockDetecter: false,

    registerEvents: function () {
        $('#hotComparison .jcarousel-control-prev').click(function () {
            CompareCars.hotComparePrevCount += 1;
        });
        var url = window.location.href.toLowerCase();
        var isComparePage = url.indexOf("/comparecars") > 0;
        if ((window.adblockDetecter === undefined && !isComparePage) || (typeof isSBIAd !== 'undefined' && isSBIAd)) {
            CompareCars.isAdBlockDetecter = true;
            $('.adblockCompare').removeClass('grid-8').addClass('grid-12');
            $('#nextComparison, #prevComparison').attr('data-swipeCount', '3');
            $('#moreComparison, .jcarousel-wrapper.compare-carousel').css('width', '943px');
            $('.compareAdDiv').addClass('hide');
        }

        $('#hotComparison .jcarousel-control-next').on('click', function () {
            CompareCars.hotCompareNextCount += 1;
            if (((!CompareCars.isAdBlockDetecter && ((CompareCars.hotCompareNextCount + 1) % 3 == 0)) || (CompareCars.isAdBlockDetecter && CompareCars.hotCompareNextCount % 2 == 0)) && CompareCars.hotCompareNextCount > CompareCars.hotComparePrevCount) {

                CompareCars.hotCompareNextPageNo = CompareCars.hotCompareNextPageNo + 1;
                var _pageNo = CompareCars.hotCompareNextPageNo;
                var url = '/hotComparison/?pageno=' + _pageNo + '&pagesize=' + CompareCars._pageSize;
                $.when(Common.utils.ajaxCall(url)).done(function (data) {
                    if (data != null && $.trim(data) != "") {
                        $('#hotComparison ul').append(data);
                        BindCarouselEvent('#hotComparison');
                        UNTLazyLoad();
                    }
                });
            }
            $('#hotComparison').find('img').trigger("UNT");
        });
        
        $(document).on('hover', '.compare-carousel .year-info-icon', function (e) {
            e.stopPropagation();
            $(this).siblings('.price-availability').toggleClass('hide');
        });
    }
}

var NewCar_Common = {
    setLocation: function (locationObj) {
        var areaId = typeof locationObj.areaId !== "undefined" ? locationObj.areaId : "-1";
        var areaName = typeof locationObj.areaName !== "undefined" ? locationObj.areaName : "Select Area";
        Location.globalSearch.setLocationCookies(locationObj.cityId, locationObj.cityName, areaId, areaName);
    },

    redirectToModelPage: function (makeMaskingName, modelMaskingName) {
        var modelPageURL = '/' + makeMaskingName + '-cars/' + modelMaskingName + '/';
        window.location.href = modelPageURL;
    },

    redirectToVersionPage: function (location) {
        var makeMaskingName = location.selector().data('makemaskingname');
        var modelMaskingName = location.selector().data('modelmaskingname');
        var versionMaskingName = location.selector().data('versionmaskingname');
        var versionPageURL = '/' + makeMaskingName + '-cars/' + modelMaskingName + '/' + versionMaskingName + '/';
        window.location.href = versionPageURL;
    },
}

CompareCars.registerEvents();

upcomingLeadCity = {};
upcomingCityTargetId = "";

var PrefilCityUpcoming = function () {
    if (Number($.cookie("_CustCityIdMaster")) > 0 && $.cookie("_CustCityMaster") != null && $.trim($.cookie("_CustCityMaster")) != "" && $.trim($.cookie("_CustCityMaster")) != "Select City") {
        $(".upcomingCity").val($.cookie("_CustCityMaster"));
        upcomingLeadCity.name = $.trim($.cookie("_CustCityMaster"));
        upcomingLeadCity.id = $.trim($.cookie("_CustCityIdMaster"));
    }
}

var ValidateCityUpcoming = function (targetId) {
    var cityTargetId = "#upcomingCityid_" + targetId;
    var cityVal = Common.utils.getSplitCityName($(cityTargetId).val());
    if (cityVal == $.cookie("_CustCityMaster") && typeof (upcomingLeadCity) != "undefined" && Number(upcomingLeadCity.id) > 0 && upcomingLeadCity.id == $.cookie("_CustCityIdMaster")) {
        showHideMatchError(false, cityTargetId);
        return true;
    }
    else if (cityVal == "" || $(cityTargetId).hasClass('border-red') ||
                (
                    ($('li.ui-menu-item a:visible:eq(0)').text() != cityVal && cityVal != "") &&
                    (typeof (upcomingLeadCity) == "undefined" || typeof (upcomingLeadCity.name) == "undefined" || upcomingLeadCity.name.toLowerCase() != cityVal.toLowerCase())
                )
           ) {
        showHideMatchError(true, cityTargetId, "Please Enter City");
        return false;
    }
    return true;
}

function bindCityAutoSuggest() {
    $('.upcomingCity').each(function (counter, element) {
        if (!element.isProcessed) {
            $(element).cw_autocomplete({
                resultCount: 5,
                source: ac_Source.allCarCities,
                click: function (event, ui, orgTxt) {
                    upcomingLeadCity.name = Common.utils.getSplitCityName(ui.item.label);
                    upcomingLeadCity.id = ui.item.id;
                    ui.item.value = upcomingLeadCity.name;
                },
                open: function (result) {
                    upcomingLeadCity.result = result;
                },
                focusout: function () {
                    var cityVal = $('#' + upcomingCityTargetId).val();
                    if ((Common.utils.getSplitCityName($('li.ui-state-focus a:visible').text().toLowerCase()) == cityVal.toLowerCase() || $('li.ui-state-focus a:visible').text().toLowerCase() == cityVal.toLowerCase()) && typeof (this.result) == "object") {
                        var focused = this.result[$('li.ui-state-focus').index()];
                        if (upcomingLeadCity == undefined) upcomingLeadCity = new Object();
                        if (focused != undefined) {
                            upcomingLeadCity.name = Common.utils.getSplitCityName(focused.label);
                            upcomingLeadCity.id = focused.id;
                        }
                    }
                },
                afterfetch: function (result, searchtext) {
                    this.result = result;
                    if (typeof result == "undefined" || result.length <= 0)
                        showHideMatchError(true, '#' + upcomingCityTargetId, "No city Match");
                    else
                        showHideMatchError(false, '#' + upcomingCityTargetId);

                },
            }).focus(function (event) {
                upcomingCityTargetId = event.target.id;
            })
            element.isProcessed = true;
        }
    });
    $('#upComingCars .card').on('click', function () {
        if ($('.upcomingCity').is(':focus'))
            $('#' + upcomingCityTargetId).blur();
    });

    $('#upComingCars .upcomingCity').on('click', function (e) { e.stopPropagation() });
}
bindCityAutoSuggest();

$(document).on('click', "#newLaunches .infoBtn,#topSelling .infoBtn, #upComingCars .infoBtn", function () {
    //tracking
    var divId = $(this).parents().eq(6).attr('id');
    trackTNU(divId, divId + '-Flip', divId);

    $(this).parents("li").flip(true).siblings().flip(false);
    var upcomingCars = $(this).parents().eq(6).attr("id");
    if (upcomingCars != "upComingCars") {
        var drpCity = $(this).parents('li').find('.formWrapper .formContent input');
        preFillPqWidget(drpCity);
    }
});

$(document).on('click', '.topSellingChkOrpBtn,.newLaunchesChkOrpBtn', function () {
    var divId = $(this).attr("topSelling") ? "topSelling" : "newLaunches";
    var drpCity = $(this).parent().find('input');
    var cityId = drpCity.data("cityid");
    var carModelId = $(this).data("modelid");
    var versionId = $(this).data("versionid");
    if (Number(cityId) > 0) {

        if (drpCity.val() != "" && drpCity.val() != "Select City") {
            var cityId = drpCity.data("cityid");
            var cityName = drpCity.data("cityname");
            var zoneId = drpCity.data("zoneid");
            var areaId = drpCity.data("areaid");
            var areaName = drpCity.data("areaname");
            var locationObj;
            if (areaId > 0)
                locationObj = { cityId: cityId, cityName: cityName, zoneId: zoneId, areaId: areaId, areaName: areaName };
            else
                locationObj = { cityId: cityId, cityName: cityName };
        }

        trackTNU(divId, 'Successful-submit', cityId + "-" + carModelId);

        PriceBreakUp.Quotation.RedirectToPQ({
            'modelId': carModelId, 'versionId': versionId, 'location': locationObj, 'pageId': carouselPqPageId($(this))
        });
    }
    else {
        ShakeFormView($(".op-city-selection"));
        showCityError(drpCity);
        trackTNU(divId, 'Unsuccessful-submit', cityId + "-" + carModelId);
        return false;
    }
});

var preFillPqWidget = function (selector) {
    var cityId = $.cookie('_CustCityIdMaster');
    var cityName = $.cookie('_CustCityMaster');
    var zoneId = $.cookie('_CustZoneIdMaster');
    var areaId = $.cookie('_CustAreaId');
    var areaName = $.cookie('_CustAreaName');
    if (cityId > 0) {
        if (areaId > 0) {
            selector.attr('value', areaName + ', ' + cityName);
            selector.data('cityid', cityId);
            selector.data('cityname', cityName);
            selector.data('zoneid', zoneId);
            selector.data('areaid', areaId);
            selector.data('areaname', areaName);
        }
        else {
            if ($.inArray(Number(cityId), askingAreaCityId) < 0) {
                selector.attr('value', cityName);
                selector.data('cityid', cityId);
                selector.data('cityname', cityName);
            }
        }
    }
}

function initTopSellingPQInstance() {
    var div = $('.topsellingCityInput');
    if (typeof topSellingHomeLocation !== "undefined") {
        topSellingHomeLocation.destroy();
        topSellingHomeLocation = null;
    }

    topSellingHomeLocation = new LocationSearch(div, {
        showCityPopup: true,
        callback: function (locationObj) {
            var carModelId = topSellingHomeLocation.selector().data('modelid');
            var carVersionId = topSellingHomeLocation.selector().data('versionid') || -1;
            PriceBreakUp.Quotation.RedirectToPQ({
                'modelId': carModelId, 'versionId': carVersionId, 'location': locationObj, 'pageId': carouselPqPageId(topSellingHomeLocation.selector())
            });
        }
    });
}

function initJustLaunchesPQInstance() {
    var div = $('.justLaunchesCityInput');
    if (typeof newLaunchesHomeLocation !== "undefined") {
        newLaunchesHomeLocation.destroy();
        newLaunchesHomeLocation = null;
    }

    newLaunchesHomeLocation = new LocationSearch(div, {
        showCityPopup: true,
        callback: function (locationObj) {
            var carModelId = newLaunchesHomeLocation.selector().data('modelid');
            var carVersionId = newLaunchesHomeLocation.selector().data('versionid') || -1;
            PriceBreakUp.Quotation.RedirectToPQ({
                'modelId': carModelId, 'versionId': carVersionId, 'location': locationObj, 'pageId': carouselPqPageId(newLaunchesHomeLocation.selector())
        });
        }
    });
}

function BindData(apiUrl, ulId, category) {
    $.ajax({
        type: 'GET',
        url: apiUrl,
        success: function (result) {
            $(ulId + ' ul').append(result);
            BindCarouselEvent(ulId);
            bindCityAutoSuggest();
            PrefilCityUpcoming();
            BindFlipEvents();
            if (category === Category.TopSelling) {
                initTopSellingPQInstance();
            }
            if (category === Category.NewLaunches) {
                initJustLaunchesPQInstance();
            }
        }
    }).done(function () {
        UNTLazyLoad();
        $("img.lazy").trigger("UNT");
    });
}

function showCityError(selector) {
    selector.parent().find('.city-error-icon').show();
    selector.parent().find('.city-error-msg').show();
    selector.addClass('border-red');
}


function BindCarouselEvent(divId) {
    $(divId + ' .jcarousel').jcarousel('reload');
}

function BindFlipEvents() {
    $(".card").flip({
        axis: 'y',
        trigger: 'manual',
        reverse: true
    });
}

function carouselPqPageId(clickSelector)
{
    var pageId;
    if (window.location.pathname === "/new/") {
        if (clickSelector.data("topselling")) {
                pageId = 41;
            } else {
                pageId = 42;
            }
        } else {
        if (clickSelector.data("newlaunches")) {
                pageId = 24;
            } else {
                pageId = 25;
            }
    }
    return pageId;
}

// The parallax plugin code
(function ($) {
    $.fn.parallax = function (options) {
        var $$ = $(this);
        offset = $$.offset();
        var defaults = {
            'start': 0,
            'stop': 0,
            'coeff': 0.95
        };
        if ($$.height()) {
            defaults.stop = offset.top + $$.height();
        }

        var opts = $.extend(defaults, options);
        return this.each(function () {
            $(window).bind('scroll', function () {
                windowTop = $(window).scrollTop();
                if ((windowTop >= opts.start) && (windowTop <= opts.stop)) {
                    newCoord = windowTop * opts.coeff;
                    $$.css({
                        'background-position': '0 ' + newCoord + 'px'
                    });
                }
            });
        });
    };
})(jQuery);

$(document).ready(function () {
	if ($("#parallax-container").length) {
		$(window).scroll(function () {
			parallaxEffect();
		});
	}

	function parallaxEffect() {
		var top_of_element = $("#parallax-container").offset().top;
		var bottom_of_element = $("#parallax-container").offset().top + $("#parallax-container").outerHeight();
		var bottom_of_screen = $(window).scrollTop() + window.innerHeight;
		var top_of_screen = $(window).scrollTop();

		if ((bottom_of_screen > bottom_of_element) && (top_of_screen < bottom_of_element)) {
			$('.swift-scroll-property').parallax({ 'coeff': 0.05 })
		} else if ((top_of_screen < top_of_element)) {
			$('.swift-scroll-property').parallax({ 'coeff': 0.0 });
		}
		else {
			$('.bgimg-2').parallax({ 'coeff': 0.0 });
		}
	}
});
