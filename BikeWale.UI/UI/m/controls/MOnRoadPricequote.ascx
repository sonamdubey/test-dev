<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.MOnRoadPricequote" %>

<div class="container" id="OnRoadContent">
    <div class="grid-12">
        <h2 class="font18 margin-top20 margin-bottom20 text-white text-center">On road price</h2>

        <!-- On road pricequote control-->
        <div class="form-control-box margin-bottom20">
            <input value="" class="form-control rounded-corner2 ui-autocomplete-input" type="text" placeholder="Search Make and Model" id="getFinalPrice" />
            <span style="display: none;" class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black hide"></span>
            <span class="bwmsprite error-icon hide"></span>
            <div class="bw-blackbg-tooltip hide">Please enter make/model name</div>
        </div>
        <div class="form-control-box margin-bottom20 finalPriceCitySelect ">
            <select data-placeholder="--Select City--" class="form-control" id="ddlCitiesOnRoad" tabindex="2" data-bind="options: bookingCities, value: selectedCity, optionsText: 'cityName', optionsValue: 'cityId', optionsCaption: '--Select City--', chosen: { width: '100%' }, event: { change: cityChangedOnRoad }"></select>
            <span class="bwmsprite error-icon hide"></span>
            <div class="bw-blackbg-tooltip hide">Please Select City</div>
        </div>
        <div class="form-control-box margin-bottom20 finalPriceAreaSelect " data-bind="visible: bookingAreas().length > 0">
            <select data-placeholder="--Select Area--" class="form-control" id="ddlAreaOnRoad" data-bind="options: bookingAreas, value: selectedArea, optionsText: 'areaName', optionsValue: 'areaId', optionsCaption: '--Select Area--', chosen: { width: '100%' }"></select>
            <span class="bwsprite error-icon hide"></span>
            <div class="bw-blackbg-tooltip hide">Please Select Area</div>
        </div>
        <button id="btnDealerPriceOnRoad" class="btn btn-orange btn-full-width margin-bottom30" type="button" value="Check On-Road Price" data-bind="event: { click: getPriceQuoteOnRoad }">Check on-road price</button>
        <!-- Onroad price quote ends here-->

        <p class="text-white text-center padding-left30 padding-right30">Its private, no need to share your number and email</p>
    </div>
</div>
<div class="clear"></div>


<script type="text/javascript">

    var selectedModel = 0,pageId,selectedMakeName = '', selectedCityName = '', gaLabel = '', selectedAreaName = '';
    var onRoadcity ,onRoadArea ,onRoadMakeModel ,mname = "";
    var onCookieObj = {}, viewModelOnRoad;

    
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
            //data: '{"modelId":"' + modelId + '"}',
            //beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetPriceQuoteCitiesNew"); },
            success: function (response) {
                //var obj = JSON.parse(response);
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
                }
            }
        });
    }

    function cityChangedOnRoad() {
        //gtmCodeAppender(pageId, "City Selected", null);
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
                            if (!isNaN(onCookieObj.PQAreaSelectedId) && onCookieObj.PQAreaSelectedId > 0 && onCookieObj.PQAreaSelectedId != 0 && selectElementFromArray(areas, onCookieObj.PQAreaSelectedId)) {
                                viewModelOnRoad.selectedArea(onCookieObj.PQAreaSelectedId);
                            }
                        }
                        else {
                            viewModelOnRoad.selectedArea(0);
                            viewModelOnRoad.bookingAreas([]);
                        }
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
            if (cityId != onCookieObj.PQCitySelectedId || areaId > 0)
                setLocationCookie($('#ddlCitiesOnRoad option:selected'), $('#ddlAreaOnRoad option:selected'));

            var obj = {
                'CityId': viewModelOnRoad.selectedCity(),
                'AreaId': viewModelOnRoad.selectedArea(),
                'ModelId': selectedModel,
                'ClientIP': '<%= ClientIP %>',
                'SourceType': '2',
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
                    //SetCookie("_MPQ", cookieValue);

                    if (jsonObj != undefined && jsonObj.quoteId > 0) {
                        gtmCodeAppender(pageId, 'Dealer_PriceQuote_Success_Submit', gaLabel);
                        window.location = "/m/pricequote/dealer/?MPQ=" + Base64.encode(cookieValue);
                    }                    
                    else {
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
                onCookieObj.PQCitySelectedId = parseInt(cData[0]) || 0;
                onCookieObj.PQCitySelectedName = cData[1] ? cData[1].replace(/-/g, ' ') : "";
                onCookieObj.PQAreaSelectedId = parseInt(cData[2]) || 0;
                onCookieObj.PQAreaSelectedName = cData[3] ? cData[3].replace(/-/g, ' ') : "";

            }
        }
    }

    docReady(function () {

        onRoadcity = $('#ddlCitiesOnRoad'), onRoadArea = $('#ddlAreaOnRoad'), onRoadMakeModel = $('#getFinalPrice');

        // knockout OnRoadData binding
        viewModelOnRoad = {
            selectedCity: ko.observable(),
            bookingCities: ko.observableArray([]),
            selectedArea: ko.observable(),
            bookingAreas: ko.observableArray([]),
            hasAreas: ko.observable()
        };

        //for jquery chosen 
        ko.bindingHandlers.chosen = {
            init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                var $element = $(element);
                var options = ko.unwrap(valueAccessor());
                if (typeof options === 'object')
                    $element.chosen(options);

                ['options', 'selectedOptions', 'value'].forEach(function (propName) {
                    if (allBindings.has(propName)) {
                        var prop = allBindings.get(propName);
                        if (ko.isObservable(prop)) {
                            prop.subscribe(function () {
                                $element.trigger('chosen:updated');
                            });
                        }
                    }
                });
            }
        };

        // Chosen touch support.
        if ($('.chosen-container').length > 0) {
            $('.chosen-container').on('touchstart', function (e) {
                e.stopPropagation(); e.preventDefault();
                // Trigger the mousedown event.
                $(this).trigger('mousedown');
            });
        }

        $.fn.hint = bwHint;
        $.fn.bw_autocomplete = bwAutoComplete;

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
                    selectedModel = model.id;
                    selectedMakeName = ui.item.label;
                    FillCitiesOnRoad(selectedModel);
                    gtmCodeAppender(pageId, "Get_On_Road_Price_Click", selectedMakeName);
                }
            },
            open: function (result) {
                objBikes.result = result;
                if (navigator.userAgent.match(/(iPod|iPhone|iPad)/)) {

                    $('.ui-autocomplete').off('menufocus hover mouseover');
                }
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
