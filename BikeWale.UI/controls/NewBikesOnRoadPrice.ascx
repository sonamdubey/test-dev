<%@ Control Language="C#" AutoEventWireup="False" Inherits="Bikewale.controls.NewBikesOnRoadPrice" %>
<style>
    
/*PopupWidget Styling*/
#OnRoadContent .chosen-container { border:0;border-radius:0;padding:12px }
.minifyWidth{width:50%;}
</style>


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
            <div class="final-price-citySelect" >
                <div class="form-control-box">
                    <select data-placeholder="Select City" class="form-control rounded-corner0" id="ddlCitiesOnRoad" tabindex="2" data-bind="options: bookingCities, value: selectedCity, optionsText: 'CityName', optionsValue: 'CityId', optionsCaption: 'Select City', event: { change: cityChangedOnRoad }"></select> 
                    <span class="fa fa-spinner fa-spin position-abt pos-right12 pos-top15 text-black bg-white" style="display:none"></span>
                    <span class="bwsprite error-icon hide"></span>
                    <div class="bw-blackbg-tooltip hide">Please select a city</div>
                </div>
            </div>
            <div class="final-price-areaSelect" data-bind="visible: bookingAreas().length > 0">
                <div class="form-control-box">
                    <select data-placeholder="Select Area" class="form-control rounded-corner0" id="ddlAreaOnRoad" tabindex="3" data-bind="options: bookingAreas, value: selectedArea, optionsText: 'AreaName', optionsValue: 'AreaId', optionsCaption: 'Select Area', event: { change: areaChangedOnRoad }"></select>
                    <span class="bwsprite error-icon hide"></span>
                    <div class="bw-blackbg-tooltip hide">Please select an area</div>
                </div>
            </div>
        </div>
        <div class="clear"></div>
    </div>
    <div class="get-final-price-btn">
        <button id="btnDealerPriceOnRoad" tabindex="4" class="font18 btn btn-orange btn-lg rounded-corner-no-left" type="button" value="Get On-road Price" data-bind="event: { click: getPriceQuoteOnRoad }">Get On-road Price</button>
    </div>
    <div class="clear"></div>
</div>


<script type="text/javascript">

    onRoadcity = $('#ddlCitiesOnRoad');
    onRoadArea = $('#ddlAreaOnRoad');
    onRoadMakeModel = $('#finalPriceBikeSelect');
    selectedMakeModel = { makeModelName: "", modelId: "" };
    mname = "";
    var onCookieObj = {};
    var selectedMakeName = '', selectedCityName = '', gaLabel = '', selectedAreaName = '';

    // knockout OnRoadData binding
    var viewModelOnRoad = {
        selectedCity: ko.observable(),
        bookingCities: ko.observableArray([]),
        selectedArea: ko.observable(),
        bookingAreas: ko.observableArray([])
    };


    function FillCitiesOnRoad(modelId) {
        toggleErrorMsg(onRoadcity, false);
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
                    nbCheckCookies();
                    var initIndex = 0;
                    for (var i = 0; i < cities.length; i++) { 

                        if (onCookieObj.PQCitySelectedId == cities[i].CityId) {
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

                    if(citySelected!=null)
                    {
                        viewModelOnRoad.selectedCity(citySelected.CityId);
                        calcWidth();
                    }
                        

                    $("#ddlCitiesOnRoad option[value=0]").prop("disabled", "disabled");
                    if ($("#ddlCitiesOnRoad option:last-child").val() == "0") {
                        $("#ddlCitiesOnRoad option:last-child").remove();
                    }
                    if ($("#ddlCitiesOnRoad option:first-child").next().val() == "0") {
                        $("#ddlCitiesOnRoad option[value=0]").remove();
                    }
                    $('#ddlCitiesOnRoad').trigger("chosen:updated");                        

                    cityChangedOnRoad();
                }
                else {
                    viewModelOnRoad.bookingCities([]);
                    $('#ddlCitiesOnRoad').trigger("chosen:updated");
                }
            }
        });
    }


    function cityChangedOnRoad() {
        gtmCodeAppenderWidget(pageId, "City Selected", null);
        toggleErrorMsg(onRoadArea, false);
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
                        if (onCookieObj.PQAreaSelectedId != 0 && selectElementFromArray(areas, onCookieObj.PQAreaSelectedId)) {
                            viewModelOnRoad.selectedArea(onCookieObj.PQAreaSelectedId);
                            onCookieObj.PQAreaSelectedId = 0;
                        }
                        $('#ddlAreaOnRoad').trigger("chosen:updated");
                        calcWidth();
                    }
                    else {
                        viewModelOnRoad.selectedArea(0);
                        viewModelOnRoad.bookingAreas([]);
                        $('#ddlAreaOnRoad').trigger("chosen:updated");
                        calcWidth();
                    }
                }
            });
        } else {
            viewModelOnRoad.bookingAreas([]);
        }
    }

    function areaChangedOnRoad() {
        gtmCodeAppenderWidget(pageId, "Area Selected", null);
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
            setLocationCookie($('#ddlCitiesOnRoad option:selected'), $('#ddlAreaOnRoad option:selected'));

            $.ajax({
                type: 'POST',
                url: "/ajaxpro/Bikewale.Ajax.AjaxBikeBooking,Bikewale.ashx",
                data: '{"cityId":"' + cityId + '", "areaId":"' + areaId + '", "modelId":"' + selectedModel + '", "isMobileSource":false}',
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
                        gtmCodeAppenderWidget(pageId, 'Dealer_PriceQuote_Success_Submit', gaLabel);
                        window.location = "/pricequote/dealerpricequote.aspx";
                    }
                    else if (jsonObj != undefined && jsonObj.quoteId > 0) {
                        gtmCodeAppenderWidget(pageId, 'BW_PriceQuote_Success_Submit', gaLabel);
                        window.location = "/pricequote/quotation.aspx";
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
                onCookieObj.PQCitySelectedId = parseInt(cData[0]);
                onCookieObj.PQCitySelectedName = cData[1];
                onCookieObj.PQAreaSelectedId = parseInt(cData[2]);
                onCookieObj.PQAreaSelectedName = cData[3];

            }
        }
    }

   function calcWidth()
    {
        if (viewModelOnRoad.bookingAreas().length > 0 )
            $(ele).width(161);
        else $(ele).width(322);
    }

   $(function () {

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
                    gtmCodeAppenderWidget(pageId, "Button Clicked", null);
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
        
        $("#ddlCitiesOnRoad").chosen({ no_results_text: "No matches found!!" });
        $("#ddlAreaOnRoad").chosen({ no_results_text: "No matches found!!" });

        ko.applyBindings(viewModelOnRoad, $("#OnRoadContent")[0]);

        ele = $('#OnRoadContent .final-price-citySelect')[0];

    });
</script>