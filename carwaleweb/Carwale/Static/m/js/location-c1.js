var globalLocation = {
    toTriggermanualChangeEvent: (typeof toTriggermanualChangeEvent === 'undefined' ? false : toTriggermanualChangeEvent), //for used car search page
    toChangeGlobalCity: (typeof toChangeGlobalCity === 'undefined' ? true : toChangeGlobalCity), //for new car
    coreInstance: null,
    popup_promise: $.Deferred(),
    expectedUserInput: {
        OnlyCity: 1,
        OptionalArea: 2,
        MandatoryArea: 3
    },
    egAreaDict: { //dictionary of cityId and example areas of these cities to show in area popup placeholder
        "1": "Andheri East; Colaba",
        "10": "Connaught Place; Barakhamba",
        "2": "Koramangala;  Marathahalli",
        "12": "Swargate; Hinjewadi"
    },
    BL: function () {
        var self = this;
        globalLocation.BLInstance = self;
        if (!globalLocation.coreInstance)
        { globalLocation.coreInstance = new globalLocation.core(); globalLocation.coreInstance.registerEvents(); }
        this.openLocHint = function (locationObj, expectedUserInputObj, postcallback, precallback, reloadOnLocationChange) {
            // ios global location issue fixed
            cwTracking.trackAction("CWNonInteractive", "GlobalCityPopUp", "Popupshown", "");
            if (navigator.userAgent.match(/(iPod|iPhone|iPad)/)) {
                $('#globalcity-popup').css({ "position": "absolute" });
                $("body").css({ "position": "fixed", "width": "100%" });
            }
            if (typeof precallback == "function")
                precallback();
            if (typeof expectedUserInputObj == "undefined" || !expectedUserInputObj) expectedUserInputObj = globalLocation.expectedUserInput.OptionalArea;
            globalLocation.coreInstance.inputType = expectedUserInputObj;
            if (expectedUserInputObj == globalLocation.expectedUserInput.MandatoryArea)
                globalLocation.coreInstance.disableAreaClose = function () { globalLocation.coreInstance.locationSelectors.globalCityCloseBtn.hide(); }
            else
                globalLocation.coreInstance.disableAreaClose = undefined;
            if (globalLocation.toTriggermanualChangeEvent && typeof postcallback != "function") postcallback = globalLocation.coreInstance.manualTriggerChangeEvent;
            if (typeof postcallback != "function") postcallback = undefined;
            self.processingPostcallback(postcallback);
            if (typeof locationObj == "undefined" || !locationObj)
                locationObj = globalLocation.coreInstance.currentLocationObj;
            if (typeof reloadOnLocationChange == "undefined") reloadOnLocationChange = true;
            if (globalLocation.coreInstance.processAreacallback || globalLocation.coreInstance.processCitycallback) reloadOnLocationChange = false;
            globalLocation.coreInstance.reloadOnLocationChange = reloadOnLocationChange;
            self.processPopupConditions(locationObj);
        };
        this.processingPostcallback = function (postcallback) {
            if (!postcallback) { globalLocation.coreInstance.processCitycallback = undefined; globalLocation.coreInstance.processAreacallback = undefined; }
            if (globalLocation.coreInstance.inputType == globalLocation.expectedUserInput.OnlyCity)
                globalLocation.coreInstance.processCitycallback = postcallback;
            else
                globalLocation.coreInstance.processAreacallback = postcallback;
        };
        this.processPopupConditions = function (locationObj) {
            if (window.history) {
                window.history.pushState("globalcitypopup", "", window.location.href);
                globalLocation.coreInstance.lastState = true;
            }
            if (globalLocation.coreInstance.inputType != globalLocation.expectedUserInput.OnlyCity && $.inArray(Number(locationObj.cityId), askingAreaCityId) >= 0)
                globalLocation.coreInstance.openAreaPopup(locationObj);
            else
                globalLocation.coreInstance.openCityPopup();
        }
    },
    core: function () {
        var self = this;
        this.GACategory = "AreaLocation-abtest";
        this.lastState = false;
        this.reloadOnLocationChange = true;
        this.currentLocationObj = {
            cityId: Number($.cookie('_CustCityIdMaster')) > 0 ? $.cookie('_CustCityIdMaster') : "-1",
            cityName: Number($.cookie('_CustCityIdMaster')) > 0 ? $.cookie('_CustCityMaster') : "Select City",
            areaId: Number($.cookie('_CustAreaId')) > 0 ? $.cookie('_CustAreaId') : "-1",
            areaName: Number($.cookie('_CustAreaId')) > 0 ? $.cookie('_CustAreaName') : "Select Area",
            zoneId: Number($.cookie('_CustZoneIdMaster')) > 0 ? $.cookie('_CustZoneIdMaster') : "",
            zoneName: Number($.cookie('_CustZoneIdMaster')) > 0 ? $.cookie('_CustZoneMaster') : "Select Zone",
            cityMaskingName: ""
        };
        this.areaCityId = self.currentLocationObj.cityId;
        this.areaCityName = self.currentLocationObj.cityName;
        this.locationSelectors = {
            globalCityPopup: $("#globalcity-popup"),
            citySection: $('.global-city-section'),
            areaSection: $('.global-area-section'),
            txtPlacesQuery: $('.cityName,.areaName'),
            locationWidget: $('#loc-widget'),
            globalCityField: $("#placesQuery"),
            globalAreaField: $("#placesQueryArea"),
            globalPopupBlackout: $("#globalPopupBlackOut"),
            globalLocation: $('#global-location'),
            selectCityHeading: $('#select-heading'),
            selectedCityText: $('#selected-city-text'),
            globalCityCloseBtn: $(".globalcity-close-btn"),
        };
        this.inputType = globalLocation.expectedUserInput.OptionalArea;
        this.registerEvents = function () {
            self.locationSelectors.globalCityField.cw_easyAutocomplete({
                inputField: self.locationSelectors.globalCityField,
                resultCount: 5,
                source: ac_Source.globalCityLocation,
                click: function () {
                    self.processCities(self.locationSelectors.globalCityField.getSelectedItemData());
                },
                afterFetch: function (result, searchText) {
                    self.validateResults(result, searchText);
                }
            });
            self.locationSelectors.globalAreaField.cw_easyAutocomplete({
                inputField: self.locationSelectors.globalAreaField,
                resultCount: 5,
                beforefetch: function () {
                    this.cityId = self.areaCityId;
                },
                source: ac_Source.areaLocation,
                click: function () {
                    self.processAreas(self.locationSelectors.globalAreaField.getSelectedItemData());
                },
                afterFetch: function (result, searchText) {
                    self.validateResults(result, searchText);
                }
            });
            $(document).on('keydown', function (e) {
                var ESCKEY = 27, ENTERKEY = 13;
                if (e.keyCode == ENTERKEY)
                    e.preventDefault();
                else if (e.keyCode == ESCKEY) {
                    e.preventDefault();
                    if (self.locationSelectors.locationWidget.is(':visible')) {
                        cwTracking.trackAction("CWInteractive", self.GACategory, "popupclosedesc", "");
                        self.closeLocHint();
                    }
                }
            });
            self.locationSelectors.txtPlacesQuery.on('keydown', function (e) {
                var ENTERKEY = 13, ESCKEY = 27;
                e.stopPropagation();
                if (e.keyCode == ENTERKEY)
                    e.preventDefault();
                if (e.keyCode == ESCKEY)
                    self.closeLocHint();
            });
            $(document).on('click', 'div.globalcity-popup .globalcity-close-btn', function (e) {

                if (self.locationSelectors.locationWidget.is(':visible'))
                    cwTracking.trackAction("CWInteractive", self.locationSelectors.GACategory, "popupclosedcross", $(".cityNameWrap").text());
                self.closeLocHint();
                if (self.areaCityId != self.currentLocationObj.cityId && typeof self.processAreacallback == "function")
                    self.processAreacallback({ cityMaskingName: self.currentLocationObj.cityMaskingName });
                window.history.replaceState("", "", null);
            });
            window.onpopstate = function (e) {
                self.lastState && self.closeLocHint();
            };
            self.locationSelectors.selectedCityText.on("click", function () {
                self.showCityPopup();
            });
        };
        this.showCityPopup = function () {
            self.locationSelectors.citySection.removeClass('hide').addClass('show');
            self.locationSelectors.areaSection.removeClass('show').addClass('hide');
            self.locationSelectors.txtPlacesQuery.attr('placeholder', 'Type your city e.g. Mumbai');
            self.locationSelectors.txtPlacesQuery.val("");
            self.locationSelectors.selectCityHeading.removeClass("hide").addClass("show");
            self.locationSelectors.selectedCityText.removeClass("show").addClass("hide");
            self.locationSelectors.globalCityCloseBtn.show();
            setTimeout(function () { self.locationSelectors.txtPlacesQuery.show().focus(); }, 0);
        };
        this.showAreaPopup = function (cityObj) {
            if (typeof cityObj.cityId == "undefined")
                cityObj.cityId = $.cookie('_CustCityMaster');
            self.areaCityId = cityObj.cityId;
            if (cityObj.cityId) {
                self.locationSelectors.citySection.removeClass("show").addClass("hide");
                self.locationSelectors.areaSection.removeClass("hide").addClass("show");
                self.locationSelectors.selectCityHeading.removeClass("show").addClass("hide");
                self.locationSelectors.selectedCityText.removeClass("hide").addClass("show");
                self.locationSelectors.selectedCityText.text(cityObj.cityName).append("<span class='cwmsprite edit-city'></span>");
                self.locationSelectors.txtPlacesQuery.attr('placeholder', 'Where in ' + cityObj.cityName + '? e.g. ' + globalLocation.egAreaDict[cityObj.cityId]);
                self.locationSelectors.txtPlacesQuery.val("");
                setTimeout(function () { self.locationSelectors.txtPlacesQuery.show().focus(); }, 0);
                if (self.disableAreaClose)
                { self.disableAreaClose(); self.disableAreaClose = undefined; }
                else
                    globalLocation.coreInstance.locationSelectors.globalCityCloseBtn.show();
            }
            else self.showCityPopup();
        };
        this.openCityPopup = function () {
            self.locationSelectors.globalCityPopup.addClass('highlight');
            self.locationSelectors.globalPopupBlackout.show();
            self.showCityPopup();
            self.locationSelectors.globalCityPopup.show();
        };
        this.openAreaPopup = function (cityObj) {
            self.locationSelectors.globalCityPopup.addClass('highlight');
            self.locationSelectors.globalPopupBlackout.show();
            self.showAreaPopup(cityObj);
            self.locationSelectors.globalCityPopup.show();
        };
        this.processCities = function (item) {
            var alreadySetCity = $.cookie('_CustCityIdMaster');
            var alreadySetAreaId = Number($.cookie('_CustAreaId'));
            var alreadySetZoneId = Number($.cookie('_CustZoneIdMaster'));
            self.areaCityId = item.payload.cityId;   //for passing cityId in area autocomplete
            self.areaCityName = item.payload.cityName;
            if ($.inArray(Number(item.payload.cityId), askingAreaCityId) < 0 && !self.processCitycallback && self.processAreacallback)
            { self.processCitycallback = self.processAreacallback; self.processAreacallback = undefined; }
            if (globalLocation.toChangeGlobalCity) {
                if (Number(alreadySetCity) == Number(item.payload.cityId))
                    globalLocation.setLocationCookies(item.payload.cityId, item.payload.cityName, alreadySetAreaId > 0 ? alreadySetAreaId : -1, alreadySetAreaId > 0 ? $.cookie('_CustAreaName') : "Select Area", alreadySetZoneId > 0 ? alreadySetZoneId : "", alreadySetZoneId > 0 ? $.cookie('_CustZoneMaster') : "Select Zone");
                else {
                    globalLocation.setLocationCookies(item.payload.cityId, item.payload.cityName, -1, "Select Area", "", "Select Zone");
                    $('#global-place').text(item.payload.cityName).attr("title", item.payload.cityName);
                }
            }
            cwTracking.trackAction("CWInteractive", self.GACategory, "citySelected", item.payload.cityName + "||");
            if (self.processCitycallback) {
                self.closeLocHint();
                self.processCitycallback(item.payload);
                self.processCitycallback = undefined;
            }
            else {
                if (item.payload.isAreaAvailable && self.inputType != globalLocation.expectedUserInput.OnlyCity) {
                    self.currentLocationObj.cityMaskingName = item.payload.cityMaskingName;
                    self.showAreaPopup({ cityId: item.payload.cityId, cityName: item.payload.cityName });
                }
                else
                    self.closeLocHint();
            }
        };
        this.processAreas = function (item) {
            if (globalLocation.toChangeGlobalCity) {
                globalLocation.setLocationCookies(item.payload.cityId, item.payload.cityName, item.payload.areaId, item.payload.areaName, item.payload.zoneId, item.payload.zoneName);
                cwTracking.trackAction("CWInteractive", self.GACategory, "AreaSelected", self.getCookiesLabel({ cityId: item.payload.cityId, cityName: item.payload.cityName, areaId: item.payload.areaId, areaName: item.payload.areaName, zoneId: item.payload.zoneId, zoneName: item.payload.zoneName }));
            }
            self.closeLocHint();
            if (self.processAreacallback) {
                self.processAreacallback(item.payload);
                self.processAreacallback = undefined;
            }
        };
        this.validateResults = function (result, searchText) {
            if (result == undefined || result.length <= 0) {
                $('.cityNameWrap').addClass('errorWidget');
                cwTracking.trackAction("CWInteractive", self.GACategory, "NoResults", searchText + "_" + window.location.href);
            } else if ($('.cityNameWrap').hasClass('errorWidget')) {
                $('.cityNameWrap').removeClass('errorWidget').addClass('validWidget');
            }
        };
        this.getCookiesLabel = function (cityObj) {
            if (typeof cityObj == "undefined")
                cityObj = self.currentLocationObj;
            var label = "||";
            if (Number(cityObj.cityId) > 0) {
                label = cityObj.cityId;
                if (Number(cityObj.areaId) > 0) {
                    label += "|" + cityObj.areaId + "|" + cityObj.zoneId;
                }
                else {
                    label += "||";
                }
            }
            return label;
        };
        this.closeLocHint = function () {
            self.lastState = false;
            document.activeElement.blur();
            $('.cityNameWrap').removeClass('errorWidget');
            $('#global-location').removeClass('highlight');
            self.locationSelectors.globalCityPopup.hide();
            self.locationSelectors.globalCityPopup.removeClass("show").addClass("hide");
            Common.utils.unlockPopup();
            window.scrollTo(0, 0);
            if (self.currentLocationObj.cityId != self.areaCityId || self.currentLocationObj.areaId != $.cookie("_CustAreaId")) {
                if (self.reloadOnLocationChange && window.location.href.indexOf("ampcityId") < 0) { window.location.reload(); }
                else if (self.reloadOnLocationChange) { window.location.href = window.location.pathname; }
                var areaTxt = (Number($.cookie("_CustAreaId")) > 0 ? $.cookie('_CustAreaName') + ", " : "") + (Number($.cookie("_CustCityIdMaster")) > 0 ? $.cookie("_CustCityMaster") : "Enter your location");
                $('#global-place').text(areaTxt).attr("title", areaTxt);
            }
            // ios global location issue fixed
            if (navigator.userAgent.match(/(iPod|iPhone|iPad)/)) {
                $('#globalcity-popup, body').css({ "position": "" });
            }
        };
        this.manualTriggerChangeEvent = function (item) {
            $(document).trigger("mastercitychange", [$.cookie('_CustCityMaster'), $.cookie('_CustCityIdMaster'), item]);
        };
        // definition inherited from BL
        this.processCitycallback = undefined;
        this.processAreacallback = undefined;
        this.disableAreaClose = undefined;
    },
    setLocationCookies: function (cityId, cityName, areaId, areaName, zoneId, zoneName) {
        if (cityId > 0) $(".location-icon").addClass("city-selected");
        setCookieByNameValue('_CustCityIdMaster', cityId);
        setCookieByNameValue('_CustCityMaster', Number(cityId) > 0 ? cityName : "Select City");
        setCookieByNameValue('_CustAreaId', areaId);
        setCookieByNameValue('_CustAreaName', Number(areaId) > 0 ? areaName : "Select Area");
        setCookieByNameValue('_CustZoneIdMaster', zoneId);
        setCookieByNameValue('_CustZoneMaster', Number(zoneId) > 0 ? zoneName : "Select Zone");
    },
    registerEvents: function () {
        $("#global-location").click(function () {
            if (typeof LOCATION_EVENTS !== 'undefined' && window.location.href.indexOf("/m/used") > -1) {
                LOCATION_EVENTS.openPopup([2]);
            }
            else if (!$('#loc-widget').is(':visible')) {
                new globalLocation.BL().openLocHint(null, location.pathname.indexOf("/used/") >= 0 ? globalLocation.expectedUserInput.OnlyCity : null);
                cwTracking.trackAction("CWInteractive", globalLocation.coreInstance.GACategory, "popupdisplayed", "UserInitiated|" + globalLocation.coreInstance.getCookiesLabel());
            }
        });
        $(document).on("click", ".select-city-link", function () {
            var locationObj = null;
            var cityId = $(this).attr("data-cityid");
            var postCallback = $(this).attr("data-location-postcallback");
            var redirectUrl = $(this).attr("data-redirect-url");
            if (redirectUrl && !postCallback)
                postCallback = function () { location.href = redirectUrl };
            if (cityId)
                locationObj = { cityId: cityId, cityName: $(this).attr("data-cityname") };
            new globalLocation.BL().openLocHint(locationObj, $(this).attr("data-expectedUserInput"), eval(postCallback));
        });
        $(document).on('click', '.changeCityLink', function () {
            $("#global-location").click();
        });
    },
    // PWA global location popup
    // if any changes happen in property callback will be called
    // property:- key for store object on which you want to put watch
    // callback:- function which will execute if property value change
    // callback will get object with five params:- property: key which you have passed
    //                                 newValue: newValue of property
    //                                 oldValue: newValue of property
    //                                 newStore: updated store vaue
    //                                 oldStore: prevoius store value
    onLocationStorePropertyChange: function (property, callback) {
        if (typeof reduxWatch !== 'undefined') {
            reduxWatch(LOCATION_STORE, property, function (params) {
                console.log(params);
                callback(params);
            })
        }
    }
}

$(document).ready(
function () {
    if ($.cookie("_CustCityIdMaster") == null)
        globalLocation.setLocationCookies("-1", "Select City", "-1", "Select Area", "", "Select Zone");
    var areaText = (Number($.cookie("_CustAreaId")) > 0 ? $.cookie('_CustAreaName') + ", " : "") + (Number($.cookie("_CustCityIdMaster")) > 0 ? $.cookie("_CustCityMaster") : "Enter your location");
    $('#global-place').text(areaText);
    if ($('#hdnGlobalCity').length > 0) {
        var hdnCityId = $('#hdnGlobalCity').val();
        var cityId = Number($.cookie('_CustCityIdMaster'));
        if (Number(hdnCityId) != cityId) {
            location.reload();
        }
        $('#hdnGlobalCity').val(cityId);
    }
    var location_src = $($("#location_popup").text()).attr("src");
    if (location_src) {
        $.get(location_src).done(
        function (response) {
            $("body").append(response);
            globalLocation.registerEvents();
            $(document).trigger("locationloaded");
            globalLocation.popup_promise.resolve();
            AmpGlobalCityOpen()
        });
    }
    function AmpGlobalCityOpen() {
        var querystring = window.location.search;
        var iscityclicked = false;
        if (querystring != undefined && querystring.indexOf("=globalcity") > 0) {
            iscityclicked = true;
            window.history.replaceState('', document.title, window.location.href.split('?')[0]);
        }
        var cityFromCookie = Number($.cookie('_CustCityIdMaster'));
        if (iscityclicked && cityFromCookie <= 0) {
            new globalLocation.BL().openLocHint();
        }
    }
});
