<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.m.controls.MOnRoadPricequote" %>

    <div class="container" id="OnRoadContent">
        	<div class="grid-12">
                	<h2 class="margin-top30 margin-bottom20 text-white text-center">On Road Price</h2>

            <!-- On road pricequote control-->
            <div class="form-control-box margin-bottom20">
                <input value="" class="form-control rounded-corner2 ui-autocomplete-input" type="text" placeholder="Search Make and Model" id="getFinalPrice" />
                 <span style="display: none;" class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black hide"></span>
                  <span class="bwmsprite error-icon hide"></span>
                 <div class="bw-blackbg-tooltip hide">Please enter make/model name</div>
            </div>
            <div class="form-control-box margin-bottom20 finalPriceCitySelect ">
                <select data-placeholder="--Select City--" class="form-control" id="ddlCitiesOnRoad" tabindex="2" data-bind="options: bookingCities, value: selectedCity, optionsText: 'CityName', optionsValue: 'CityId', optionsCaption: '--Select City--', event: { change: cityChangedOnRoad }"></select>
                <span class="bwmsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please Select City</div>
            </div>
            <div class="form-control-box margin-bottom20 finalPriceAreaSelect " data-bind="visible: bookingAreas().length > 0">
                <select data-placeholder="--Select Area--" class="form-control" id="ddlAreaOnRoad" data-bind="options: bookingAreas, value: selectedArea, optionsText: 'AreaName', optionsValue: 'AreaId', optionsCaption: '--Select Area--', event: { change: areaChangedOnRoad }"></select>
                <span class="bwsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please Select Area</div>
            </div>
            <button id="btnDealerPriceOnRoad" class="btn btn-orange btn-full-width margin-bottom30" type="button" value="Get On-road Price" data-bind="event: { click: getPriceQuoteOnRoad }">Get On-road Price</button>
            <!-- Onroad price quote ends here-->

            <p class="text-white text-center padding-left30 padding-right30">Its private, no need to share your number and email</p>
        </div>
    </div>
    <div class="clear"></div>

<script type="text/javascript">

    var preSelectedCityId = 0;
    var preSelectedCityName = "";
    var selectedModel = 0;
    var abHostUrl = '<%= ConfigurationManager.AppSettings["ABApiHostUrl"]%>';
    var metroCitiesIds = [40, 12, 13, 10, 224, 1, 198, 105, 246, 176, 2, 128];
    var pageId;
    var selectedMakeName = '', selectedCityName = '', gaLabel = '', selectedAreaName = '';
    onRoadcity = $('#ddlCitiesOnRoad');
    onRoadArea = $('#ddlAreaOnRoad');
    onRoadMakeModel = $('#getFinalPrice');
    mname = "";

    // knockout OnRoadData binding
    var viewModelOnRoad = {
        selectedCity: ko.observable(),
        bookingCities: ko.observableArray([]),
        selectedArea: ko.observable(),
        bookingAreas: ko.observableArray([])
    };


    function FillCitiesOnRoad(modelId) {
        $.ajax({
            type: "POST",
            url: "/ajaxpro/Bikewale.Ajax.AjaxPriceQuote,Bikewale.ashx",
            data: '{"modelId":"' + modelId + '"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetPriceQuoteCitiesNew"); },
            success: function (response) {
                var obj = JSON.parse(response);
                var cities = JSON.parse(obj.value);
                var citySelected = null;
                if (cities) {
                    checkCookies();
                    var initIndex = 0;
                    for (var i = 0; i < cities.length; i++) {
                        if (preSelectedCityId == cities[i].CityId) {
                            citySelected = cities[i];
                        }
                        if (metroCitiesIds.indexOf(cities[i].CityId) > -1) {
                            var currentCity = cities[i];
                            cities.splice(cities.indexOf(currentCity), 1);
                            cities.splice(initIndex++, 0, currentCity);
                        }                       

                    }

                    cities.splice(initIndex, 0, { CityId: 0, CityName: "---------------", CityMaskingName: null });

                    viewModelOnRoad.bookingCities(cities);

                    if (citySelected != null) {
                        viewModelOnRoad.selectedCity(citySelected.CityId);
                    }

                    $("#ddlCitiesOnRoad option[value=0]").prop("disabled", "disabled");
                    if ($("#ddlCitiesOnRoad option:last-child").val() == "0") {
                        $("#ddlCitiesOnRoad option:last-child").remove();
                    }
                    if ($("#ddlCitiesOnRoad option:first-child").next().val() == "0") {
                        $("#ddlCitiesOnRoad option[value=0]").remove();
                    }

                    cityChangedOnRoad();
                }
                else {
                    viewModelOnRoad.bookingCities([]);
                }
            }
        });
    }

    function cityChangedOnRoad() {
        gtmCodeAppender(pageId, "City Selected", null);
        if (viewModelOnRoad.selectedCity() != undefined) {
            $.ajax({
                type: "POST",
                url: "/ajaxpro/Bikewale.Ajax.AjaxPriceQuote,Bikewale.ashx",
                data: '{"cityId":"' + viewModelOnRoad.selectedCity() + '","modelId":"' + selectedModel + '"}',
                dataType: 'json',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetPriceQuoteArea"); },
                success: function (response) {
                    areas = $.parseJSON(response.value);
                    if (areas.length) {
                        viewModelOnRoad.bookingAreas(areas);
                    }
                    else {
                        viewModelOnRoad.selectedArea(0);
                        viewModelOnRoad.bookingAreas([]);
                    }
                }
            });
        } else {
            viewModelOnRoad.bookingAreas([]);
        }
    }

    function areaChangedOnRoad() {
        gtmCodeAppender(pageId, "Area Selected", null);
    }


    function isValidInfoOnRoad() {
        isValid = true;
        var errMsg = "Missing fields:";
        showHideMatchError(onRoadMakeModel, false);
        showHideMatchError(onRoadcity, false);
        showHideMatchError(onRoadArea, false);
        mname = onRoadMakeModel.val();

        if (selectedModel <= 0 || (mname == "" || mname.length < 2 || mname == "Search Make and Model")) {
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
            gtmCodeAppender(pageId, "Error in submission", errMsg);
        }

        return isValid;
    }

    function getPriceQuoteOnRoad() {
        var cityId = viewModelOnRoad.selectedCity(), areaId = viewModelOnRoad.selectedArea() ? viewModelOnRoad.selectedArea() : 0;
        if (isValidInfoOnRoad()) {
            //set global cookie
            if (cityId > 0) {
                cityName = $(onRoadcity).find("option[value=" + cityId + "]").text();
                cookieValue = cityId + "_" + cityName;
                SetCookieInDays("location", cookieValue, 365);
            }

            $.ajax({
                type: 'POST',
                url: "/ajaxpro/Bikewale.Ajax.AjaxBikeBooking,Bikewale.ashx",
                data: '{"cityId":"' + cityId + '", "areaId":"' + areaId + '", "modelId":"' + selectedModel + '", "isMobileSource":true}',
                dataType: 'json',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "ProcessPQ"); },
                success: function (json) {
                    var jsonObj = $.parseJSON(json.value);

                    selectedCityName = $("#ddlCitiesOnRoad option:selected").text();

                    if (areaId > 0)
                        selectedAreaName = $("#ddlAreaOnRoad option:selected").text();

                    if (selectedMakeName != "" && selectedCityName != "") {
                        gaLabel = selectedMakeName + ',' + selectedCityName;

                        if (selectedAreaName != '')
                            gaLabel += ',' + selectedAreaName;
                    }

                    if (jsonObj != undefined && jsonObj.quoteId > 0 && jsonObj.dealerId > 0) {
                        gtmCodeAppender(pageId, 'Dealer_PriceQuote_Success_Submit', gaLabel);
                        window.location = "/m/pricequote/dealerpricequote.aspx";
                    }
                    else if (jsonObj != undefined && jsonObj.quoteId > 0) {
                        gtmCodeAppender(pageId, 'BW_PriceQuote_Success_Submit', gaLabel);
                        window.location = "/m/pricequote/quotation.aspx";
                    } else {
                        gtmCodeAppender(pageId, 'BW_PriceQuote_Error_Submit', gaLabel);
                        $("#errMsgOnRoad").text("Oops. We do not seem to have pricing for given details.").show();
                    }
                },
                error: function (e) {
                    gtmCodeAppender(pageId, 'BW_PriceQuote_Error_Submit', gaLabel);
                    $("#errMsg").text("Oops. Some error occured. Please try again.").show();
                }
            });
        } else {
            gtmCodeAppender(pageId, 'BW_PriceQuote_Error_Submit', gaLabel);
            $("#errMsgOnRoad").text("Please select all the details").show();
        }
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
                preSelectedCityId = parseInt(cData[0]);
                preSelectedCityName = cData[1];
            }
        }
    }

    $(function () {

        $("#getFinalPrice").bw_autocomplete({

            source: 2,
            recordCount: 5,
            onClear: function () {
                objBikes = new Object();
            },
            click: function (event, ui, orgTxt) {
                var model = null;
                if (ui.item.payload.modelId > 0) {
                    model = new Object();
                    model.maskingName = ui.item.payload.modelMaskingName;
                    model.id = ui.item.payload.modelId;
                    pageId = '<%= PageId %>';
                    gtmCodeAppender(pageId, "Button Clicked", null);
                    selectedModel = model.id;
                    selectedMakeName = ui.item.label;
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
                }
            },
            afterfetch: function (result, searchtext) {
                return false;
            }
        });

        ko.applyBindings(viewModelOnRoad, $("#OnRoadContent")[0]);

    });
</script>
