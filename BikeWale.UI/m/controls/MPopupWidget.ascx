<%@ Control Language="C#" AutoEventWireup="true" Inherits="Bikewale.Mobile.controls.MPopupWidget" %>
<script type="text/javascript">
    var sourceHref = '0';
</script>
<!--bw popup code starts here-->
<div class="bw-city-popup bwm-fullscreen-popup hide bw-popup-sm text-center" id="popupWrapper">
    <div class="popup-inner-container">
        <div class="bwmsprite onroad-price-close-btn close-btn position-abt pos-top10 pos-right10 cur-pointer"></div>
        <div id="popupHeading" class="content-inner-block-20">
            <p class="font18 margin-bottom5 text-capitalize">Please Tell Us Your Location</p>
            <div class="text-light-grey margin-bottom5"><span class="red">*</span>Get on-road prices by just sharing your location!</div>
            <div id="citySelection" class="form-control text-left input-sm position-rel margin-bottom10">
                <div class="selected-city" data-bind="text: (SelectedCity() != undefined && SelectedCity().cityName != '') ? SelectedCity().cityName : 'Select City'"></div>
                <span class="fa fa-angle-right position-abt pos-top10 pos-right10"></span>
            </div>

            <div id="areaSelection" class="form-control text-left input-sm position-rel margin-bottom10" data-bind="visible: BookingAreas().length > 0 && SelectedAreaId() > 0">
                <div class="selected-area" data-bind="text: (SelectedArea() != undefined && SelectedArea().areaName != '') ? SelectedArea().areaName : 'Select Area'">Select Area</div>
                <span class="fa fa-angle-right position-abt pos-top10 pos-right10"></span>
            </div>

            <div class="center-align margin-top20 text-center">
                <a id="btnDealerPricePopup" class="btn btn-orange btn-full-width font18" data-bind=" click: getPriceQuote ">Get on road price</a>
                <div id="errMsgPopup" class="red-text margin-top10 hide"></div>
            </div>
        </div>
        <div id="popupContent" class="bwm-city-area-popup-wrapper">
            <div class="bw-city-popup-box bwm-city-area-box city-list-container form-control-box text-left">
                <div class="user-input-box">
                    <span class="back-arrow-box">
                        <span class="bwmsprite back-long-arrow-left"></span>
                    </span>
                    <input class="form-control" type="text" id="popupCityInput" placeholder="Select City" data-bind="attr: { value: (SelectedCity() != undefined) ? SelectedCity().cityName : '' }" />
                </div>
                <ul id="popupCityList" class="margin-top40" data-bind="foreach: BookingCities">
                    <li data-bind="text: cityName, attr: { 'cityId': cityId }, css: (isPopular) ? 'isPopular' : '', click: function (data, event) { $parent.selectCity(data, event); }"></li>
                </ul>
                <div class="margin-top30 font24 text-center margin-top60 "><span class="fa fa-spinner fa-spin text-black" style="display: none;"></span><span id="popupLoader"></span></div>
            </div>

            <div class="bw-area-popup-box bwm-city-area-box area-list-container form-control-box text-left" data-bind="visible: BookingAreas().length > 0">
                <div class="user-input-box">
                    <span class="back-arrow-box">
                        <span class="bwmsprite back-long-arrow-left"></span>
                    </span>
                    <input class="form-control" type="text" id="popupAreaInput" placeholder="Select Area" data-bind="attr: { value: (SelectedArea() != undefined) ? SelectedArea().areaName : '' }" />
                </div>
                <ul id="popupAreaList" class="margin-top40" data-bind="foreach: BookingAreas, visible: BookingAreas().length > 0 ">
                    <li data-bind="text: areaName, attr: { 'areaId': areaId }, click: function (data, event) { $parent.selectArea(data, event); }"></li>
                </ul>

            </div>
        </div>
    </div>
</div>
<!--bw popup code ends here-->

<script type="text/javascript">
    var selectedModel = 0;
    var abHostUrl = '<%= ConfigurationManager.AppSettings["ABApiHostUrl"]%>';
    var preSelectedCityId = 0;
    var preSelectedCityName = "";
    var onCookieObj = {};
    var selectedMakeName = '', selectedModelName = '', selectedCityName = '', selectedAreaName = '', gaLabel = '', isModelPage = false;

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

        self.getCities = ko.computed(function (data, event) {
            if (self.SelectedModelId() != undefined && self.SelectedModelId() > 0) {
                $.ajax({
                    type: "GET",
                    url: "/api/PQCityList/?modelId=" + self.SelectedModelId(),
                    beforeSend: function () {
                        $("#popupContent").show();
                        $("#citySelection div.selected-city").text("Loading Cities..");
                        $("#popupLoader").text("Loading cities..").show().prev().show();
                    },
                    success: function (response) {
                        var cities = ko.toJS(response.cities);
                        var citySelected = null;
                        if (cities) {
                            self.BookingCities(cities);
                        }
                        else {
                            self.BookingCities([]);
                        }
                    },
                    complete: function (xhr) {
                        $("#popupLoader").text("Loading cities..").hide().prev().hide();
                        if (self.BookingCities().length > 0) {
                            $("#citySelection div.selected-city").text("Select City");
                            $("#popupCityList li.isPopular").last().addClass("border-last-bottom");
                        } else {
                            $("#citySelection div.selected-city").text("No cities Found");
                        }

                        checkCookies();
                        if (!$.isEmptyObject(onCookieObj) && onCookieObj.PQCitySelectedId > 0) {
                            MPopupViewModel.SelectedCity(ko.toJS({ 'cityId': onCookieObj.PQCitySelectedId, 'cityName': onCookieObj.PQCitySelectedName }));
                            MPopupViewModel.SelectedCityId(onCookieObj.PQCitySelectedId);
                            $("ul#popupCityList li[cityId='" + onCookieObj.PQCitySelectedId + "']").click();
                        }
                    }
                });
            }
        });

        self.selectCity = function (data, event) {
            self.SelectedCity(data);
            self.SelectedCityId(data.cityId);

            $("div.bw-city-area-popup-wrapper .back-arrow-box").click();
            if (self.SelectedModelId() != undefined && self.SelectedModelId() > 0 && self.SelectedCity() != undefined) {
                $.ajax({
                    type: "GET",
                    url: "/api/PQAreaList/?modelId=" + self.SelectedModelId() + "&cityId=" + self.SelectedCity().cityId,
                    beforeSend: function () {
                        $("#areaSelection div.selected-area").text("Loading areas..");
                        $("#popupLoader").text("Loading areas..").show().prev().show();
                    },
                    success: function (response) {
                        var areas = ko.toJS(response.areas);
                        var areaSelected = null;
                        if (areas) {
                            self.BookingAreas(areas);

                        }

                    },
                    complete: function (xhr) {
                       
                        $("#popupLoader").text("Loading areas..").hide().prev().hide();
                        if (xhr.status == 404 || xhr.status == 204) {
                            $(".bwm-city-area-popup-wrapper .back-arrow-box").click();
                            self.BookingAreas([]);
                            self.SelectedArea([]);
                            self.SelectedAreaId(0);
                            $("#areaSelection div.selected-area").text("No areas Found");

                        }
                        else {
                            if (self.BookingAreas().length > 0) {
                                $("#areaSelection div.selected-area").text("Select Area");
                            } else {
                                $("#areaSelection div.selected-area").text("No areas available");
                            } 
                            //$("#areaSelection").click();

                        }

                        if (!$.isEmptyObject(onCookieObj) && onCookieObj.PQCitySelectedId > 0 && onCookieObj.PQAreaSelectedId > 0) {
                            //MPopupViewModel.SelectedArea(ko.toJS({ 'areaId': onCookieObj.PQAreaSelectedId, 'areaName': onCookieObj.PQAreaSelectedName }));
                            MPopupViewModel.SelectedAreaId(onCookieObj.PQAreaSelectedId);
                            $("ul#popupAreaList li[areaId='" + onCookieObj.PQAreaSelectedId + "']").click();
                            
                        }

                        
                    }
                });
            }
        }

        self.selectArea = function (data, event) {
            self.SelectedArea(data);
            self.SelectedAreaId(data.areaId);
            $(".bwm-city-area-popup-wrapper .back-arrow-box").click();
        };

        self.verifyDetails = function (data, event) {
            isValid = true;
            var errMsg = "Missing fields:";

            if (self.SelectedCityId() == undefined) {
                errMsg += "City,";
                isValid = false;
            }
            if (self.BookingAreas().length > 0 && self.SelectedAreaId() == undefined) {
                errMsg += "Area,";
                isValid = false;
            }
            if (!isValid) {
                errMsg = errMsg.substring(0, errMsg.length - 1);
                gtmCodeAppender(pageId, "Error in submission", errMsg);
            }
            return isValid;
        }

        self.getPriceQuote = function (data, event) {
            var cityId = self.SelectedCityId(), areaId = self.SelectedAreaId() ? self.SelectedAreaId() : 0;
            pageId = self.PageCatId;

            cookieValue = self.SelectedCity().cityId + "_" + self.SelectedCity().cityName + ((self.SelectedAreaId() == undefined || self.SelectedAreaId() < 1) ? "" : ("_" + self.SelectedArea().areaId + "_" + self.SelectedArea().areaName));
            SetCookieInDays("location", cookieValue, 365);

            if (self.verifyDetails()) {

                if (isModelPage && ga_pg_id != null && ga_pg_id == 2) {
                    window.location.reload();
                }

                else {
                    var obj = {
                        'CityId': self.SelectedCityId(),
                        'AreaId': self.SelectedAreaId(),
                        'ModelId': (self.SelectedModelId() != undefined) ? self.SelectedModelId() : selectedModel,
                        'ClientIP': '',
                        'SourceType': '2',
                        'VersionId': 0
                    };
                    $.ajax({
                        type: 'POST',
                        url: "/api/PriceQuote/",
                        data: obj,
                        dataType: 'json',
                        //beforeSend: function (xhr) { return; },
                        success: function (json) {
                            var jsonObj = ko.toJS(json);
                            selectedCityName = self.SelectedCity().cityName;

                            if (self.MakeName != undefined || self.ModelName != undefined)
                                gaLabel = self.MakeName + ',' + self.ModelName + ',';

                            gaLabel += selectedCityName;

                            if (self.SelectedArea() != undefined) {
                                selectedAreaName = self.SelectedArea().areaName;
                                gaLabel += ',' + selectedAreaName;
                            }

                            cookieValue = "CityId=" + self.SelectedCityId() + "&AreaId=" + self.SelectedAreaId() + "&PQId=" + jsonObj.quoteId + "&VersionId=" + jsonObj.versionId + "&DealerId=" + jsonObj.dealerId;
                            SetCookie("_MPQ", cookieValue);

                            if (jsonObj.quoteId > 0 && jsonObj.dealerId > 0) {
                                gtmCodeAppender(pageId, 'Dealer_PriceQuote_Success_Submit', gaLabel);
                                window.location = "/m/pricequote/dealerpricequote.aspx";
                            }
                            else if (jsonObj.quoteId > 0) {
                                gtmCodeAppender(pageId, 'BW_PriceQuote_Success_Submit', gaLabel);
                                window.location = "/m/pricequote/quotation.aspx";
                            } else {
                                gtmCodeAppender(pageId, 'BW_PriceQuote_Error_Submit', gaLabel);
                                $("#errMsgPopup").text("Oops. We do not seem to have pricing for given details.").show();
                            }
                        },
                        error: function (e) {
                            $("#errMsg").text("Oops. Some error occured. Please try again.").show();
                            gtmCodeAppender(pageId, 'BW_PriceQuote_Error_Submit', gaLabel);
                        }
                    });
                }
            } else {
                $("#errMsgPopup").text("Please select all the details").show();
                gtmCodeAppender(pageId, 'BW_PriceQuote_Error_Submit', gaLabel);
            }
        }

    };


    $(document).ready(function () {
        $('#popupWrapper .close-btn,.blackOut-window').click(function () {
            $('.bw-city-popup').fadeOut(100);
            $('body').removeClass('lock-browser-scroll');
            $(".blackOut-window").hide();
            $('a.fillPopupData').removeClass('ui-btn-active');
        });

        $("a.fillPopupData").on("click", function (e) {
            e.stopPropagation();
            $("#errMsgPopUp").empty();
            var str = $(this).attr('modelId');
            var pageIdAttr = $(this).attr('pagecatid');

            var makeName = $(this).attr('makeName'), modelName = $(this).attr('modelName');
            var modelIdPopup = parseInt(str, 10);
            gtmCodeAppender(pageIdAttr, "Get_On_Road_Price_Click", modelName);
            MPopupViewModel.MakeName = makeName;
            MPopupViewModel.ModelName = modelName;
            MPopupViewModel.PageCatId = pageIdAttr;
            selectedModel = modelIdPopup;
            isModelPage = $(this).attr('ismodel');

            //checkCookies();
            //if(onCookieObj!=null && onCookieObj.PQCitySelectedId > 0)
            //{
            //    MPopupViewModel.SelectedCity(ko.toJS({ 'cityId': onCookieObj.PQCitySelectedId, 'cityName': onCookieObj.PQCitySelectedName }));
            //    MPopupViewModel.SelectedCityId(onCookieObj.PQCitySelectedId);

            //    if(onCookieObj.PQSelectedId > 0)
            //    {
            //        MPopupViewModel.SelectedArea(ko.toJS({ 'areaId': onCookieObj.PQAreaSelectedId, 'areaName': onCookieObj.PQAreaSelectedName }));
            //        MPopupViewModel.SelectedAreaId(onCookieObj.PQAreaSelectedId);
            //    }
            //}      

            MPopupViewModel.SelectedModelId(selectedModel);

            $('#popupWrapper').fadeIn(10);
            appendHash("onRoadPrice");
        });

    });

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
