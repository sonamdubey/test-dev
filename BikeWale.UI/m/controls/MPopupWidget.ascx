<%@ Control Language="C#" AutoEventWireup="true" Inherits="Bikewale.Mobile.controls.MPopupWidget" %>

<script type="text/javascript">
    lscache.flushExpired();  //remove expired
    var modelCityKey = "mc_";
    var cityAreaKey = "ca_";
    var sourceHref = '0';
    var cityClicked = false;
    var areaClicked = false;
</script>

<div id="popupWrapper">
    <div id="city-area-popup" class="bwm-fullscreen-popup">
        <div class="header-fixed fixed">
            <div class="leftfloat header-back-btn">
                <a href="javascript:void(0)" rel="nofollow"><span class="bwmsprite white-back-arrow"></span></a>
            </div>
            <div class="leftfloat header-title text-bold text-white font18">Select location</div>
            <div class="clear"></div>
        </div>
        <div class="city-area-banner"></div>
        <div id="city-area-content">
            <div id="city-menu" class="city-area-menu open">
                <div id="city-menu-tab" class="city-area-tab cursor-pointer">
                    <span class="city-area-tab-label" data-bind="text: (SelectedCity() != undefined && SelectedCity().name != '') ? 'City : ' + SelectedCity().name : 'Select your city'"></span>
                    <span class="chevron bwmsprite chevron-down"></span>
                </div>
                <div class="inputbox-list-wrapper">
                    <div class="form-control-box user-input-box">
                        <span class="bwmsprite search-icon-grey"></span>
                        <input type="text" class="form-control padding-right40" placeholder="Type to select city" id="city-menu-input" autocomplete="off" data-bind="textInput: cityFilter">
                        <span class="fa fa-spinner fa-spin position-abt text-black"></span>
                    </div>
                    <ul id="city-menu-list" data-bind="foreach: BookingCities" >
                         <li data-bind="text: name, attr: { 'cityId': id }, click: function (d, e) { $parent.selectCity(d, e); }"></li>
                    </ul>
                </div>
            </div>

            <div id="area-menu" class="city-area-menu">
                <div id="area-menu-tab" class="city-area-tab">
                    <span class="city-area-tab-label" data-bind="text: (SelectedArea() != undefined && SelectedArea().name != '') ? 'Area : ' + SelectedArea().name : 'Select your area'"></span>
                </div>
                <div class="inputbox-list-wrapper">
                    <div class="form-control-box user-input-box">
                        <span class="bwmsprite search-icon-grey"></span>
                        <input type="text" class="form-control padding-right40" placeholder="Type to select area" id="area-menu-input" autocomplete="off" data-bind="textInput: areaFilter">
                        <span class="fa fa-spinner fa-spin position-abt text-black"></span>
                    </div> 
                    <ul id="area-menu-list" data-bind="foreach: BookingAreas" >
                        <li data-bind="text: name, attr: { 'areaId': id }, click: function (d, e) { $parent.selectArea(d, e); }"></li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</div>
<!-- pricequote widget ends here-->

<script type="text/javascript">
    var selectedModel = 0;
    var preSelectedCityName = "";
    var onCookieObj = {};
    var selectedMakeName = '', selectedModelName = '', selectedCityName = '', selectedAreaName = '', gaLabel = '', isModelPage = false;
    var PQSourceId;
    var opBrowser = false;


    $('#popupWrapper .close-btn,.blackOut-window').click(function () {
        $('.bw-city-popup').fadeOut(100);
        $('body').removeClass('lock-browser-scroll');
        $(".blackOut-window").hide();
        $('a.fillPopupData').removeClass('ui-btn-active');
    });

    $('body').on("click", "a.fillPopupData", function (e) {

        e.stopPropagation();
        $("#errMsgPopUp").empty();
        var str = $(this).attr('modelId');
        var pageIdAttr = $(this).attr('pagecatid');
        selCityId = $(this).attr('selCityId');
        PQSourceId = $(this).attr('pqSourceId');
        var makeName = $(this).attr('makeName'), modelName = $(this).attr('modelName');
        var modelIdPopup = parseInt(str, 10);
        gtmCodeAppender(pageIdAttr, "Get_On_Road_Price_Click", modelName);
        MPopupViewModel.MakeName = makeName;
        MPopupViewModel.ModelName = modelName;
        MPopupViewModel.PageCatId = pageIdAttr;
        selectedModel = modelIdPopup;
        isModelPage = $(this).attr('ismodel');
        if (MPopupViewModel.SelectedModelId() != selectedModel) {
            MPopupViewModel.SelectedModelId(selectedModel);
            MPopupViewModel.getCities();
        }
        $('#popupWrapper').fadeIn(10);
        appendHash("onRoadPrice");
    });


    var mPopup = function () {
        var self = this;
        self.MakeName = "";
        self.ModelName = "";
        self.PageCatId = "";
        self.SelectedModelId = ko.observable();
        self.SelectedCity = ko.observable();
        self.SelectedArea = ko.observable();
        self.SelectedCityId = ko.observable(0);
        self.SelectedAreaId = ko.observable(0);
        self.BookingCities = ko.observableArray([]);
        self.BookingAreas = ko.observableArray([]);
        self.oBrowser = ko.observable(opBrowser);
        self.hasAreas = ko.observable();
        self.getCities = function () {
            var isAborted = false;
            $("#citySelection div.selected-city").text("Loading Cities..");
            $("#popupLoader").text("Loading cities..").show().prev().show();
            self.BookingCities([]);
            $("#areaSelection").hide();
            if (self.SelectedModelId() != undefined && self.SelectedModelId() > 0) {
                self.SelectedCityId(0);
                modelCityKey = "mc_" + self.SelectedModelId();
                $.ajax({
                    type: "GET",
                    url: "/api/v2/PQCityList/?modelId=" + self.SelectedModelId(),
                    dataType: 'json',
                    beforeSend: function (xhr) {
                        startLoading($("#citySelection"));
                        $("#popupContent").show();
                        $("#citySelection div.selected-city").text("Loading Cities..").next().show();
                        $("#popupLoader").text("Loading cities..").show().prev().show();
                        if (data = lscache.get(modelCityKey)) {
                            var cities = ko.toJS(data);
                            var citySelected = null;
                            if (cities) {
                                self.BookingCities(data);
                            }
                            else {
                                self.BookingCities([]);
                            }
                            isAborted = true;
                            xhr.abort();
                        }
                    },
                    success: function (response) {
                        var _gZippedCitiesParse = ko.toJS(response);
                        lscache.set(modelCityKey, _gZippedCitiesParse.cities, 60);
                        var cities = ko.toJS(_gZippedCitiesParse.cities);
                        var citySelected = null;
                        if (cities) {
                            self.BookingCities(cities);
                        }
                        else {
                            self.BookingCities([]);
                        }
                    },
                    complete: function (xhr) {
                        completeCityOp(self);
                    }
                });
            }

            if (isAborted) {
                completeCityOp(self);
            }
        };

        self.selectCity = function (data, event) {
            var isAborted = false;
            $(".bwm-city-area-popup-wrapper .back-arrow-box").click();
            if (!self.oBrowser()) {
                self.SelectedCity(data);
                self.SelectedCityId(data.id);
            }
            else {
                self.SelectedCity(findCityById(self.SelectedCityId()));
            }

            if (self.SelectedModelId() != undefined && self.SelectedModelId() > 0 && self.SelectedCity() != undefined) {
                self.hasAreas(findCityById(self.SelectedCity().id).hasAreas);
                if (self.hasAreas()) {
                    cityAreaKey = "ca_" + self.SelectedCityId().toString();
                    self.BookingAreas([]);
                    self.SelectedArea(undefined);
                    self.SelectedAreaId(0);
                    $("#areaSelection").show();
                    $("#areaSelection div.selected-area").text("Loading areas..");
                    $("#areaPopupLoader").text("Loading areas..").show().prev().show();

                    $.ajax({
                        type: "GET",
                        url: "/api/v2/PQAreaList/?modelId=" + self.SelectedModelId() + "&cityId=" + self.SelectedCity().id,
                        dataType: 'json',
                        beforeSend: function (xhr) {
                            startLoading($("#areaSelection"));
                            $("#areaSelection div.selected-area").text("Loading areas..").next().show();
                            $("#areaPopupLoader").text("Loading areas..").show().prev().show();
                            if (data = lscache.get(cityAreaKey)) {
                                var areas = ko.toJS(data);
                                var areaSelected = null;
                                if (areas) {
                                    self.BookingAreas(data);
                                }
                                else {
                                    self.BookingAreas([]);
                                }
                                isAborted = true;
                                xhr.abort();
                            }
                        },
                        success: function (response) {
                            var gArea = ko.toJS(response.areas);
                            lscache.set(cityAreaKey, gArea, 60);
                            var areas = ko.toJS(gArea);
                            var areaSelected = null;
                            if (areas) {
                                self.BookingAreas(areas);
                            }

                        },
                        complete: function (xhr) {
                            completeAreaOp(self, xhr);
                        }

                    });
                }
                else {
                    $(".bwm-city-area-popup-wrapper .back-arrow-box").click();
                    self.BookingAreas([]);
                    self.SelectedArea(undefined);
                    self.SelectedAreaId(0);
                }

                if (isAborted) {
                    if (self.BookingCities().length > 0)
                        completeAreaOp(self, ko.toJS({ "status": 200 }));
                    else completeAreaOp(self, ko.toJS({ "status": 404 }));
                }
            }

            ev = $._data($('ul#popupCityList')[0], 'events');
            if (!(ev && ev.click)) {
                $('ul#popupCityList').on('click', 'li', function (e) {
                    if (ga_pg_id != null && ga_pg_id == 2 && cityClicked == false) {
                        var actText = '';
                        if (self.SelectedCity().hasAreas) {
                            actText = 'City_Selected_Has_Area';
                        }
                        else {
                            actText = 'City_Selected_Doesnt_Have_Area';
                        }
                        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': actText, 'lab': getBikeVersion() + '_' + self.SelectedCity().name });
                        cityClicked = true;
                    }
                });
            }

        };

        self.selectArea = function (data, event) {
            if (!self.oBrowser()) {
                self.SelectedArea(data);
                self.SelectedAreaId(data.id);
                $(".bwm-city-area-popup-wrapper .back-arrow-box").click();
            }
            else {
                self.SelectedArea(findAreaById(self.SelectedAreaId()));
            }
            if (ga_pg_id != null && ga_pg_id == 2 && areaClicked == false) {
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Area_Selected', 'lab': myBikeName + '_' + getBikeVersion() + '_' + self.SelectedCity().name + '_' + self.SelectedArea().name });
                areaClicked = true;
            }
        };

        self.verifyDetails = function (data, event) {
            isValid = true;
            var errMsg = "Missing fields:";

            if (self.SelectedCityId() == undefined) {
                errMsg += "City,";
                isValid = false;
            }
            if (self.BookingAreas().length > 0 && self.SelectedArea() == undefined && (self.SelectedAreaId() == undefined || self.SelectedAreaId() == 0)) {
                errMsg += "Area,";
                isValid = false;
            }
            if (!isValid) {
                errMsg = errMsg.substring(0, errMsg.length - 1);
                gtmCodeAppender(pageId, "Error in submission", errMsg);
            }
            return isValid;
        };

        self.getPriceQuote = function (data, event) {
            var cityId = self.SelectedCityId(), areaId = self.SelectedAreaId() ? self.SelectedAreaId() : 0;
            pageId = self.PageCatId;

            cookieValue = self.SelectedCity().id + "_" + self.SelectedCity().name;
            if (self.SelectedArea() != undefined) {
                cookieValue += ("_" + self.SelectedArea().id + "_" + self.SelectedArea().name);
            }
            SetCookieInDays("location", cookieValue, 365);

            if (self.verifyDetails()) {

                if (isModelPage && ga_pg_id != null && ga_pg_id == 2) {
                    try {
                        startLoading($("#btnPriceLoader"));
                        var selArea = '';
                        if (self.SelectedArea() != undefined) {
                            selArea = '_' + self.SelectedArea().name;
                        }
                        bikeVersionLocation = myBikeName + '_' + getBikeVersion() + '_' + self.SelectedCity().name + selArea;
                    }
                    catch (err) { }
                    window.location.reload();

                    stopLoading($("#btnPriceLoader"));

                }
                else {
                    startLoading($("#btnPriceLoader"));

                    var obj = {
                        'CityId': self.SelectedCityId(),
                        'AreaId': self.SelectedAreaId(),
                        'ModelId': (self.SelectedModelId() != undefined) ? self.SelectedModelId() : selectedModel,
                        'ClientIP': '<%= ClientIP %>',
                        'SourceType': '2',
                        'VersionId': 0,
                        'pQLeadId': PQSourceId,
                        'deviceId': getCookie('BWC'),
                        'refPQId': typeof pqId != 'undefined' ? pqId : ''
                    };
                    $.ajax({
                        type: 'POST',
                        url: "/api/PriceQuote/",
                        data: obj,
                        dataType: 'json',
                        beforeSend: function (xhr) {

                            xhr.setRequestHeader('utma', getCookie('__utma'));
                            xhr.setRequestHeader('utmz', getCookie('_bwutmz'));
                        },
                        success: function (json) {
                            var jsonObj = ko.toJS(json);
                            selectedCityName = self.SelectedCity().name;

                            if (self.MakeName != undefined || self.ModelName != undefined)
                                gaLabel = self.MakeName + ',' + self.ModelName + ',';

                            gaLabel += selectedCityName;

                            if (self.SelectedArea() != undefined) {
                                selectedAreaName = self.SelectedArea().name;
                                gaLabel += ',' + selectedAreaName;
                            }

                            cookieValue = "CityId=" + self.SelectedCityId() + "&AreaId=" + (!isNaN(self.SelectedAreaId()) ? self.SelectedAreaId() : 0) + "&PQId=" + jsonObj.quoteId + "&VersionId=" + jsonObj.versionId + "&DealerId=" + jsonObj.dealerId;

                            if (jsonObj != undefined && jsonObj.quoteId > 0 && jsonObj.dealerId > 0) {
                                gtmCodeAppender(pageId, 'Dealer_PriceQuote_Success_Submit', gaLabel);
                                window.location = "/m/pricequote/dealerpricequote.aspx" + "?MPQ=" + Base64.encode(cookieValue);
                            }

                            else if (jsonObj != undefined && jsonObj.dealerId == 0 && jsonObj.isDealerAvailable && jsonObj.quoteId > 0) {
                                gtmCodeAppender(pageId, 'Dealer_PriceQuote_Success_Submit', gaLabel);
                                window.location = "/m/pricequote/dealerpricequote.aspx" + "?MPQ=" + Base64.encode(cookieValue);
                            }
                            else if (jsonObj != undefined && jsonObj.dealerId == 0 && jsonObj.quoteId > 0 && !jsonObj.isDealerAvailable) {
                                gtmCodeAppender(pageId, 'BW_PriceQuote_Success_Submit', gaLabel);
                                window.location = "/m/pricequote/quotation.aspx" + "?MPQ=" + Base64.encode(cookieValue);
                            }
                            else {
                                gtmCodeAppender(pageId, 'BW_PriceQuote_Error_Submit', gaLabel);
                                $("#errMsgPopup").text("Oops. We do not seem to have pricing for given details.").show();
                            }

                            //window.history.back();
                        },
                        complete: function (e) {
                            stopLoading($("#btnPriceLoader"));
                            if (e.status == 404 || e.status == 204) {
                                $("#errMsg").text("Oops. Some error occured. Please try again.").show();
                                gtmCodeAppender(pageId, 'BW_PriceQuote_Error_Submit', gaLabel);
                            }



                        }
                    });
                }
            } else {
                $("#errMsgPopup").text("Please select all the details").show();
                gtmCodeAppender(pageId, 'BW_PriceQuote_Error_Submit', gaLabel);
            }


        };

    }

    function completeCityOp(self) {
        $("#popupLoader").text("Loading cities..").hide().prev().hide();
        if (self.BookingCities() != null && self.BookingCities().length > 0) {
            $("#citySelection div.selected-city").text("Select City").next().hide();
            $("#popupCityList li.isPopular").last().addClass("border-last-bottom");
        } else {
            $("#citySelection div.selected-city").text("No cities Found").next().hide();
            lscache.set(modelCityKey, null, 60);
        }
        stopLoading($("#citySelection"));
        checkCookies();
        var cityFound = null;

        if (selCityId == null) {
            if (!$.isEmptyObject(onCookieObj) && onCookieObj.PQCitySelectedId > 0 && selectElementFromArray(self.BookingCities(), onCookieObj.PQCitySelectedId)) {
                cityFound = findCityById(onCookieObj.PQCitySelectedId);

                if (cityFound != null) {
                    self.SelectedCity(ko.toJS({ 'id': onCookieObj.PQCitySelectedId, 'name': onCookieObj.PQCitySelectedName }));
                    self.SelectedCityId(onCookieObj.PQCitySelectedId);
                    self.hasAreas(cityFound.hasAreas);
                }
                if (!self.oBrowser()) {
                    $("ul#popupCityList li[cityId='" + onCookieObj.PQCitySelectedId + "']").click();
                }
                else {
                    self.selectCity(self, null);
                }

            }
        }
        else {
            _cityid = parseInt(selCityId);
            cityFound = findCityById(_cityid);
            if (cityFound != null) {
                self.SelectedCityId(_cityid);
                self.SelectedCity(ko.toJS({ 'id': cityFound.id, 'name': cityFound.name }));
                self.hasAreas(cityFound.hasAreas);
            }
            if (!self.oBrowser()) {
                $("ul#popupCityList li[cityId='" + _cityid + "']").click();
            }
            else {
                self.selectCity(self, null);
            }

        }


    }

    function completeAreaOp(self, xhr) {
        $("#areaSelection div.selected-area").next().hide();
        $("#areaPopupLoader").text("Loading areas..").hide().prev().hide();
        if (xhr.status == 404 || xhr.status == 204) {
            $(".bwm-city-area-popup-wrapper .back-arrow-box").click();
            self.BookingAreas([]);
            self.SelectedArea(undefined);
            self.SelectedAreaId(0);
            $("#areaSelection div.selected-area").text("No areas Found");
            lscache.set(cityAreaKey, null, 60);

        }
        else {
            if (self.BookingAreas().length > 0) {
                $("#areaSelection div.selected-area").text("Select Area");
            } else {
                $("#areaSelection div.selected-area").text("No areas available");
            }
            self.SelectedArea(undefined);
            self.SelectedAreaId(0);
        }

        stopLoading($("#areaSelection"));

        if (!$.isEmptyObject(onCookieObj) && onCookieObj.PQCitySelectedId > 0 && onCookieObj.PQAreaSelectedId > 0 && selectElementFromArray(self.BookingAreas(), onCookieObj.PQAreaSelectedId)) {
            if (!self.oBrowser()) {
                $("ul#popupAreaList li[areaId='" + onCookieObj.PQAreaSelectedId + "']").click();
            }
            else {
                self.selectArea(self, null);
            }
        }


    }

    function findAreaById(id) {
        return ko.utils.arrayFirst(MPopupViewModel.BookingAreas(), function (child) {
            return (child.id === id || child.areaId === id);
        });
    }

    function findCityById(id) {
        return ko.utils.arrayFirst(MPopupViewModel.BookingCities(), function (child) {
            return (child.id === id || child.cityId === id);
        });
    }

    function startLoading(ele) {
        try {
            var _self = $(ele).find(".progress-bar").css({ 'width': '0' }).show();
            _self.animate({ width: '100%' }, 7000);
        }
        catch (e) { return };
    }

    function stopLoading(ele) {
        try {
            var _self = $(ele).find(".progress-bar");
            _self.stop(true, true).css({ 'width': '100%' }).fadeOut(1000);
        }
        catch (e) { return };
    }

    function gtmCodeAppender(pageId, action, label) {
        var categoty = '';
        if (pageId != null) {
            switch (pageId) {
                case "1":
                    category = 'Make_Page';
                    break;
                case "2":
                    category = "CheckPQ_Series";
                    action = "CheckPQ_Series_" + action;
                    break;
                case "3":
                    category = "Model_Page";
                    action = "CheckPQ_Model_" + action;
                    break;
                case '4':
                    category = 'New_Bikes_Page';
                    break;
                case '5':
                    category = 'HP';
                    break;
                case '6':
                    category = 'Search_Page';
                    break;
            }
            if (label) {
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': category, 'act': action, 'lab': label });
            }
            else {
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': category, 'act': action });
            }
        }

    }

    function checkCookies() {
        c = document.cookie.split('; ');
        for (i = c.length - 1; i >= 0; i--) {
            C = c[i].split('=');
            if (C[0] == "location") {
                var cData = (String(C[1])).split('_');
                onCookieObj.PQCitySelectedId = parseInt(cData[0]);
                onCookieObj.PQCitySelectedName = cData[1];
                onCookieObj.PQAreaSelectedId = parseInt(cData[2]);
                onCookieObj.PQAreaSelectedName = cData[3];

            }
        }
    }

    MPopupViewModel = new mPopup;
    ko.applyBindings(MPopupViewModel, $("#popupWrapper")[0]);

</script>
