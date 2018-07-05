<%@ Control Language="C#" AutoEventWireup="False" Inherits="Bikewale.Controls.NewBikesOnRoadPrice" EnableViewState="false"%>
<style>#OnRoadContent .chosen-container {border: 0;border-radius: 0;padding: 9px; }</style>


<div class="final-price-search-container tools-price-dealer-container" id="OnRoadContent">
    <div class="final-price-search tools-price-dealer-search">
        <div class="final-price-bikeSelect">
            <div class="form-control-box">
                <input class="form-control border-no ui-autocomplete-input rounded-corner0" tabindex="1" type="text" placeholder="Search Make and Model" id="finalPriceBikeSelect" autocomplete="off" style="width: 250px;">
                <span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black" style="display: none"></span>
                <span class="bwsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please search a make</div>
            </div>
        </div>
        <div class="final-price-city-area-container">
            <div class="final-price-citySelect">
                <div class="form-control-box">
                    <select data-placeholder="Select City" class="form-control rounded-corner0" id="ddlCitiesOnRoad" tabindex="2" data-bind="options: bookingCities, value: selectedCity, optionsText: 'cityName', optionsValue: 'cityId', optionsCaption: 'Select City', event: { change: cityChangedOnRoad }, chosen: { width: '100%' }"></select>
                    <span class="fa fa-spinner fa-spin position-abt pos-right12 pos-top15 text-black bg-white" style="display: none"></span>
                    <span class="bwsprite error-icon hide"></span>
                    <div class="bw-blackbg-tooltip hide">Please select a city</div>
                </div>
            </div>
            <div class="final-price-areaSelect" data-bind="visible: bookingAreas().length > 0">
                <div class="form-control-box">
                    <select data-placeholder="Select Area" class="form-control rounded-corner0" id="ddlAreaOnRoad" tabindex="3" data-bind="options: bookingAreas, value: selectedArea, optionsText: 'areaName', optionsValue: 'areaId', optionsCaption: 'Select Area', chosen: { width: '100%' }"></select>
                    <span class="bwsprite error-icon hide"></span>
                    <div class="bw-blackbg-tooltip hide">Please select an area</div>
                </div>
            </div>
        </div>
        <div class="clear"></div>
    </div>
    <div class="get-final-price-btn">
        <button id="btnDealerPriceOnRoad" tabindex="4" class="font16 btn btn-orange btn-lg rounded-corner-no-left" type="button" data-bind="event: { click: getPriceQuoteOnRoad }">Check on-road price</button>
    </div>
    <div class="clear"></div>
</div>


<script type="text/javascript">

    var onRoadcity, onRoadArea, onRoadMakeModel,selectedMakeModel = { makeModelName: "", modelId: "" };
    var onCookieObj = {}, mname = "", viewModelOnRoad;
    var selectedMakeName = '', selectedCityName = '', gaLabel = '', selectedAreaName = '';

    function findCityById(vm, id) {
        return ko.utils.arrayFirst(vm.bookingCities(), function (child) {
            return child.cityId === id;
        });
    }

    function FillCitiesOnRoad(modelId) {
        showHideMatchError(onRoadcity, false);
        $.ajax({
            type: "GET",
            url: "/api/PQCityList/?modelId=" + modelId,
            dataType: 'json',
            success: function (response) {
                var cities = response.cities;
                var citySelected = null;
                if (cities) {
                    insertCitySeparator(cities);
                    nbCheckCookies();
                    viewModelOnRoad.bookingCities(cities);
                    if (!isNaN(onCookieObj.PQCitySelectedId) && onCookieObj.PQCitySelectedId > 0 && viewModelOnRoad.bookingCities() && selectElementFromArray(viewModelOnRoad.bookingCities(), onCookieObj.PQCitySelectedId)) {
                        viewModelOnRoad.selectedCity(onCookieObj.PQCitySelectedId);
                        viewModelOnRoad.hasAreas(findCityById(viewModelOnRoad, onCookieObj.PQCitySelectedId).hasAreas);
                    }
                    onRoadcity.find("option[value='0']").prop('disabled', true);
                    onRoadcity.trigger('chosen:updated');
                    cityChangedOnRoad();
                }
                else {
                    viewModelOnRoad.bookingCities([]);
                    calcWidth();
                }
            }
        });
    }

    function cityChangedOnRoad() {
        showHideMatchError(onRoadArea, false);
        if (viewModelOnRoad.selectedCity() != undefined) {
            viewModelOnRoad.hasAreas(findCityById(viewModelOnRoad, viewModelOnRoad.selectedCity()).hasAreas);
            if (viewModelOnRoad.hasAreas() != undefined && viewModelOnRoad.hasAreas()) {
                $.ajax({
                    type: "GET",
                    url: "/api/PQAreaList/?modelId=" + selectedModel + "&cityId=" + viewModelOnRoad.selectedCity(),
                    dataType: 'json',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetPriceQuoteArea"); },
                    success: function (response) {
                        areas = response.areas;
                        if (areas.length) {
                            viewModelOnRoad.bookingAreas(areas);
                            if (!isNaN(onCookieObj.PQAreaSelectedId) && onCookieObj.PQAreaSelectedId > 0 && selectElementFromArray(viewModelOnRoad.bookingAreas(), onCookieObj.PQAreaSelectedId)) {
                                viewModelOnRoad.selectedArea(onCookieObj.PQAreaSelectedId);
                            }
                            calcWidth();
                        }
                        else {
                            viewModelOnRoad.selectedArea(0);
                            viewModelOnRoad.bookingAreas([]);
                            calcWidth();
                        }
                    },
                    error: function (e) {
                        viewModelOnRoad.selectedArea(0);
                        viewModelOnRoad.bookingAreas([]);
                        calcWidth();
                    }
                });
            }
            else {
                viewModelOnRoad.bookingAreas([]);
                calcWidth();
            }
        } else {
            viewModelOnRoad.bookingAreas([]);
            calcWidth();
        }
    }

    function isValidInfoOnRoad() {
        isValid = true;
        var errMsg = "Missing fields:";
        showHideMatchError(onRoadMakeModel, false);
        showHideMatchError(onRoadcity, false);
        showHideMatchError(onRoadArea, false);
        mname = onRoadMakeModel.val();

        if (selectedModel == 0 || (mname == "" || mname.length < 2 || mname == "Search Make and Model")) {
            showHideMatchError(onRoadMakeModel, true);
            errMsg += "Make/Model,";
            isValid = false;
        }

        if (viewModelOnRoad.selectedCity() == undefined) {
            showHideMatchError(onRoadcity, true);
            errMsg += "City,";
            isValid = false;
        }


        if (viewModelOnRoad.bookingAreas().length > 0 && viewModelOnRoad.selectedArea() == undefined) {
            showHideMatchError(onRoadArea, true);
            errMsg += "Area,";
            isValid = false;
        }

        if (!isValid) {
            errMsg = errMsg.substring(0, errMsg.length - 1);
            gtmCodeAppenderWidget(pageId, "Error in submission", errMsg);
        }

        return isValid;
    }

    function getPriceQuoteOnRoad() {
        var cityId = viewModelOnRoad.selectedCity(), areaId = viewModelOnRoad.selectedArea() ? viewModelOnRoad.selectedArea() : 0;
        if (isValidInfoOnRoad()) {

            //set global cookie
            if (cityId != onCookieObj.PQCitySelectedId || areaId > 0)
                setLocationCookie($('#ddlCitiesOnRoad option:selected'), $('#ddlAreaOnRoad option:selected'));
            var obj = {
                'CityId': viewModelOnRoad.selectedCity(),
                'AreaId': viewModelOnRoad.selectedArea(),
                'ModelId': selectedModel,
                'ClientIP': '<%= ClientIP %>',
                'SourceType': '1',
                'VersionId': 0,
                'pQLeadId': '<%= PQSourceId%>',
                'deviceId': getCookie('BWC')
            };

            $.ajax({
                type: 'POST',
                url: "/api/v2/PriceQuote/",
                data: obj,
                dataType: 'json',
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('utma', getCookie('__utma'));
                    xhr.setRequestHeader('utmz', getCookie('_bwutmz'));
                },
                success: function (json) {
                    var jsonObj = json;

                    selectedCityName = $("#ddlCitiesOnRoad option:selected").text();

                    if (areaId > 0)
                        selectedAreaName = $("#ddlAreaOnRoad option:selected").text();

                    if (selectedMakeName != "" && selectedCityName != "") {
                        gaLabel = selectedMakeName + ',' + selectedCityName;

                        if (selectedAreaName != '')
                            gaLabel += ',' + selectedAreaName;
                    }

                    cookieValue = "CityId=" + viewModelOnRoad.selectedCity() + "&AreaId=" + (!isNaN(viewModelOnRoad.selectedArea()) ? viewModelOnRoad.selectedArea() : 0) + "&PQId=" + jsonObj.quoteId + "&VersionId=" + jsonObj.versionId + "&DealerId=" + jsonObj.dealerId;

                    if (jsonObj != undefined && jsonObj.quoteId > 0) {

                        if (jsonObj.dealerId > 0) {
                            gtmCodeAppender(pageId, 'Dealer_PriceQuote_Success_Submit', gaLabel);
                        }
                        else {
                            gtmCodeAppender(pageId, 'BW_PriceQuote_Success_Submit', gaLabel);
                        }

                        window.location = "/pricequote/dealer/?MPQ=" + Base64.encode(cookieValue);

                    } else {
                        gtmCodeAppenderWidget(pageId, 'BW_PriceQuote_Error_Submit', gaLabel);
                        $("#errMsgOnRoad").text("Oops. We do not seem to have pricing for given details.").show();
                    }
                },
                error: function (e) {
                    gtmCodeAppenderWidget(pageId, 'BW_PriceQuote_Error_Submit', gaLabel);
                    $("#errMsg").text("Oops. Some error occured. Please try again.").show();
                }
            });
        } else {
            gtmCodeAppenderWidget(pageId, 'BW_PriceQuote_Error_Submit', gaLabel);
            $("#errMsgOnRoad").text("Please select all the details").show();
        }
    }

    function gtmCodeAppenderWidget(pageId, action, label) {
        if (pageId != null) {
            switch (pageId) {
                case "1":
                    category = 'CheckPQ_Make';
                    break;
                case "2":
                    category = "CheckPQ_Series";
                    action = "CheckPQ_Series_" + action;
                    break;
                case "3":
                    category = "CheckPQ_Model";
                    break;
                case "4":
                    category = "New_Bikes_Page";
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

    function nbCheckCookies() {
        c = document.cookie.split('; ');
        for (i = c.length - 1; i >= 0; i--) {
            C = c[i].split('=');
            if (C[0] == "location") {
                var cData = (String(C[1])).split('_');
                onCookieObj.PQCitySelectedId = parseInt(cData[0]) || 0;
                onCookieObj.PQCitySelectedName = cData[1] || "";
                onCookieObj.PQAreaSelectedId = parseInt(cData[2]) || 0;
                onCookieObj.PQAreaSelectedName = cData[3] || "";

            }
        }
    }

    function calcWidth() {
        if (viewModelOnRoad.bookingAreas().length > 0)
            $(ele).width(161);
        else $(ele).width(322);
    }

    docReady(function () {

        onRoadcity = $('#ddlCitiesOnRoad'); onRoadArea = $('#ddlAreaOnRoad'); onRoadMakeModel = $('#finalPriceBikeSelect');

        viewModelOnRoad = {
            selectedCity: ko.observable(),
            bookingCities: ko.observableArray([]),
            selectedArea: ko.observable(),
            bookingAreas: ko.observableArray([]),
            hasAreas: ko.observable()
        };

        $.fn.hint = bwHint;
        $.fn.bw_autocomplete = bwAutoComplete;

        $("#finalPriceBikeSelect").bw_autocomplete({
            width: 250,
            source: 2,
            recordCount: 10,
            onClear: function () {
                objBikes = new Object();
            },
            click: function (event, ui, orgTxt) {
                var model = null;
                if (ui.item.payload.modelId > 0) {
                    model = new Object();
                    model.maskingName = ui.item.payload.modelMaskingName;
                    model.id = ui.item.payload.modelId;
                    selectedMakeName = ui.item.label;
                    pageId = '<%= PageId %>';
                    gtmCodeAppenderWidget(pageId, "Get_On_Road_Price_Click", selectedMakeName);
                    $("#errMsgOnRoad").empty();
                    selectedModel = model.id;
                    FillCitiesOnRoad(selectedModel);

                }
            },
            open: function (result) {
                objBikes.result = result;
            },
            focusout: function () {
                if ($('li.ui-state-focus a:visible').text() != "") {
                    focusedMakeModel = new Object();
                    focusedMakeModel = objBikes.result[$('li.ui-state-focus').index()];
                    selectedMakeModel.makeModelName = onRoadMakeModel.val();
                }
            },
            afterfetch: function (result, searchtext) {
                return false;
            }
        });

       ko.applyBindings(viewModelOnRoad, $("#OnRoadContent")[0]);

       ele = $('#OnRoadContent .final-price-citySelect')[0];

   });
</script>
