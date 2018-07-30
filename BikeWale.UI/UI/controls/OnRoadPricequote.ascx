<%@ Control Language="C#" AutoEventWireup="False" Inherits="Bikewale.Controls.OnRoadPricequote" %>
<div class="container">
    <div class="grid-5 leftfloat">
        <div class="bg-white content-inner-block-15 light-box-shadow rounded-corner2 margin-top70" id="OnRoadContent">
            <h2 class="text-bold margin-bottom20 font22">On-road price</h2> 
            <!-- On road pricequote control -->              
            <div class="form-control-box margin-bottom20">
                <input value="" class="form-control ui-autocomplete-input" type="text" placeholder="Search Make and Model" id="makemodelFinalPrice" tabindex="1" autocomplete="off">
                <span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black" style="display: none"></span>
                <span class="bwsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please enter make/model name</div>
            </div>
            <div class="form-control-box margin-bottom20 finalPriceCitySelect " >
                <select data-placeholder="--Select City--" class="form-control" id="ddlCitiesOnRoad" tabindex="2" data-bind="options: bookingCities, value: selectedCity, optionsText: 'cityName', optionsValue: 'cityId', optionsCaption: '--Select City--', event: { change: cityChangedOnRoad },chosen: { width: '100%' }"></select>
                <span class="bwsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please Select City</div>
            </div>
            <div class="form-control-box margin-bottom20 finalPriceAreaSelect " data-bind="visible: bookingAreas().length > 0">
                <select data-placeholder="--Select Area--" class="form-control" id="ddlAreaOnRoad" tabindex="3" data-bind="options: bookingAreas, value: selectedArea, optionsText: 'areaName', optionsValue: 'areaId', optionsCaption: '--Select Area--', chosen: { width: '100%' }"></select>
                <span class="bwsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please Select Area</div>
            </div>
            <button id="btnDealerPriceOnRoad" tabindex="4" class="btn btn-orange margin-bottom20" type="button" data-bind="event: { click: getPriceQuoteOnRoad }">Check on-road price</button>
            <p class="margin-bottom5">Its private, no need to share your number and email</p>    
        </div>             
        <!-- Onroad price quote ends here -->            
        </div>
</div>

<script type="text/javascript">

    var preSelectedCityId = 0,preSelectedCityName = "", selectedMakeName = '', selectedCityName = '', gaLabel = '', selectedAreaName = '';
    var selectedModel = 0, pageId, onRoadcity, onRoadArea, onRoadMakeModel, viewModelOnRoad;
    var onCookieObj = {}, mname = "";


    function RfindCityById(vm, id) {
        return ko.utils.arrayFirst(vm.bookingCities(), function (child) {
            return (child.cityId === id || child.id === id);
        });
    }

    function FillCitiesOnRoad(modelId) {
        $.ajax({
            type: "GET",
            url: "/api/PQCityList/?modelId=" + modelId,
            dataType: 'json',
            success: function (response) {
                var cities = response.cities;
                var citySelected = null; 
                if (cities) {
                    insertCitySeparator(cities);
                    checkCookies();
                    viewModelOnRoad.bookingCities(cities);
                    if (!isNaN(onCookieObj.PQCitySelectedId) && onCookieObj.PQCitySelectedId > 0 && viewModelOnRoad.bookingCities() && selectElementFromArray(viewModelOnRoad.bookingCities(), onCookieObj.PQCitySelectedId)) {
                        viewModelOnRoad.selectedCity(onCookieObj.PQCitySelectedId);
                        viewModelOnRoad.hasAreas(RfindCityById(viewModelOnRoad, onCookieObj.PQCitySelectedId).hasAreas);
                    }
                    onRoadcity.find("option[value='0']").prop('disabled', true);
                    onRoadcity.trigger('chosen:updated');
                    cityChangedOnRoad();                    
                    
                }
                else {
                    viewModelOnRoad.bookingCities([]);
                    $('.chosen - drop').hide();
                }
            }
        });
    }

    function cityChangedOnRoad() {
        if (viewModelOnRoad.selectedCity() != undefined) {
            viewModelOnRoad.hasAreas(RfindCityById(viewModelOnRoad, viewModelOnRoad.selectedCity()).hasAreas);
            if (viewModelOnRoad.hasAreas() != undefined && viewModelOnRoad.hasAreas()) {
                $.ajax({
                    type: "GET",
                    url: "/api/PQAreaList/?modelId=" + selectedModel + "&cityId=" + viewModelOnRoad.selectedCity(),
                    dataType: 'json',
                    success: function (response) {
                        areas = response.areas;
                        if (areas.length) {
                            viewModelOnRoad.bookingAreas(areas);
                            if (!isNaN(onCookieObj.PQAreaSelectedId) && onCookieObj.PQAreaSelectedId > 0 && selectElementFromArray(areas, onCookieObj.PQAreaSelectedId)) {
                                viewModelOnRoad.selectedArea(onCookieObj.PQAreaSelectedId);
                            }
                        }
                        else {
                            viewModelOnRoad.selectedArea(0);
                            viewModelOnRoad.bookingAreas([]);
                        }

                        onRoadArea.trigger('chosen:updated');
                    },
                    error: function (e) {
                        viewModelOnRoad.selectedArea(0);
                        viewModelOnRoad.bookingAreas([]);
                        onRoadArea.trigger('chosen:updated');
                    }
                });
            }
            else {
                viewModelOnRoad.selectedArea(0);
                viewModelOnRoad.bookingAreas([]);
            }
        } else {
            viewModelOnRoad.bookingAreas([]);
        }

        onRoadArea.trigger('chosen:updated');
    }

    function isValidInfoOnRoad() {
        isValid = true;
        var errMsg = "Missing fields:";
        showHideMatchError(onRoadMakeModel, false);
        showHideMatchError(onRoadcity, false);
        showHideMatchError(onRoadArea, false);
        $(onRoadcity).next().removeClass("chosenError");
        mname = onRoadMakeModel.val();

        if (selectedModel <= 0 || (mname == "" || mname.length < 2 || mname == "Search Make and Model"))
        {
            showHideMatchError(onRoadMakeModel, true);
            errMsg += "Make/Model,";
            isValid = false;
            $(onRoadcity).next().addClass("chosenError");
        }

        if (viewModelOnRoad.selectedCity() == undefined ) {
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

            if (cityId != onCookieObj.PQCitySelectedId || areaId > 0)
                setLocationCookie($('#ddlCitiesOnRoad option:selected'), $('#ddlAreaOnRoad option:selected'));
            var obj = {
                'CityId': viewModelOnRoad.selectedCity(),
                'AreaId': viewModelOnRoad.selectedArea(),
                'ModelId': selectedModel,
                'ClientIP': '<%= ClientIP %>',
                'SourceType': '1',
                'VersionId': 0,
                'pQLeadId' : '<%= PQSourceId%>',
                'deviceId' : getCookie('BWC')
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

                    if (selectedMakeName!="" && selectedCityName != "") {
                        gaLabel = selectedMakeName + ',' + selectedCityName;

                        if (selectedAreaName != '')
                            gaLabel += ',' + selectedAreaName;
                    }

                    cookieValue = "CityId=" + viewModelOnRoad.selectedCity() + "&AreaId=" + (!isNaN(viewModelOnRoad.selectedArea()) ? viewModelOnRoad.selectedArea() : 0) + "&PQId=" + jsonObj.quoteId + "&VersionId=" + jsonObj.versionId + "&DealerId=" + jsonObj.dealerId;

                    if (jsonObj != undefined && jsonObj.quoteId > 0) {

                       
                        if (jsonObj.dealerId > 0)
                        {
                            gtmCodeAppender(pageId, 'Dealer_PriceQuote_Success_Submit', gaLabel);
                        }
                        else {
                            gtmCodeAppender(pageId, 'BW_PriceQuote_Success_Submit', gaLabel);
                        }
                        
                        window.location = "/pricequote/dealer/?MPQ=" + Base64.encode(cookieValue);

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
        if (pageId != null) {
            switch (pageId) {
                case "1":
                    category = 'HP';
                    break;
                case "2":
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

    function checkCookies()
    {
        c = document.cookie.split('; ');
        for (i = c.length - 1; i >= 0; i--) {
            C = c[i].split('=');
            if (C[0] == "location") {
                var cData = (String(C[1])).split('_');
                onCookieObj.PQCitySelectedId = parseInt(cData[0]) || 0;
                onCookieObj.PQCitySelectedName = cData[1] ? cData[1].replace(/-/g, ' ') : "";
                onCookieObj.PQAreaSelectedId = parseInt(cData[2]) || 0;
                onCookieObj.PQAreaSelectedName = cData[3] ? cData[3].replace(/-/g, ' ') : "";

            }
        }
    }

    docReady(function () {

        viewModelOnRoad = {
            selectedCity: ko.observable(),
            bookingCities: ko.observableArray([]),
            selectedArea: ko.observable(),
            bookingAreas: ko.observableArray([]),
            hasAreas: ko.observable()
        };

        onRoadcity = $('#ddlCitiesOnRoad'), onRoadArea = $('#ddlAreaOnRoad'), onRoadMakeModel = $('#makemodelFinalPrice');

        $.fn.hint = bwHint;
        $.fn.bw_autocomplete = bwAutoComplete;

      $("#makemodelFinalPrice").bw_autocomplete({
            width: 365,
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
                    pageId = '<%= PageId %>';
                    selectedMakeName = ui.item.label;
                    gtmCodeAppender(pageId, "Get_On_Road_Price_Click", selectedMakeName);
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
                }
            },
            afterfetch: function (result, searchtext) {
                return false;
            }
      });
        
      $("#ddlCitiesOnRoad").chosen({ no_results_text: "No matches found!!" });
      $("#ddlAreaOnRoad").chosen({ no_results_text: "No matches found!!" });

      ko.applyBindings(viewModelOnRoad, $("#OnRoadContent")[0]);

    });
</script>