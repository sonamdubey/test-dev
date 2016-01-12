<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.controls.PopupWidget" %>
<!--bw popup code starts here-->
<script type="text/javascript">
    var sourceHref = '0';
</script>
<script runat="server">
    private string staticUrl1 = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
    private string staticFileVersion1 = System.Configuration.ConfigurationManager.AppSettings["staticFileVersion"];
</script>
<link href="<%= !string.IsNullOrEmpty(staticUrl1) ? "http://st2.aeplcdn.com" + staticUrl1 : string.Empty %>/css/chosen.min.css?<%=staticFileVersion1 %>" rel="stylesheet" />

<div class="bw-popup hide bw-popup-sm" id="popupWrapper">
    <div class="popup-inner-container" stopbinding: true>
        <div class="bwsprite popup-close-btn close-btn position-abt pos-top10 pos-right10 cur-pointer"></div>
        <div class="cityPop-icon-container">
            <div class="icon-outer-container rounded-corner50 margin-bottom20">
                <div class="icon-inner-container rounded-corner50">
                    <span class="bwsprite orp-location-icon margin-top20"></span>
                </div>
            </div>
        </div>
        <p class="font20 margin-top15 text-capitalize text-center">Please Tell Us Your Location</p>
        <p class="text-light-grey margin-bottom15 margin-top15 text-capitalize text-center">Get on-road prices by just sharing your location!</p>
        <div class="padding-top10" id="popupContent">
            <div>
                <select data-placeholder="--Select City--" class="chosen-select" id="ddlCitiesPopup" tabindex="2" data-bind="options: bookingCities, value: selectedCity, optionsText: 'cityName', optionsValue: 'cityId', optionsCaption: '--Select City--', event: { change: cityChangedPopup }"></select>
                <span class="bwsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please Select City</div>
            </div>
            <div data-bind="visible: bookingAreas().length > 0" style="margin-top: 10px">
                <select data-placeholder="--Select Area--" class="chosen-select" id="ddlAreaPopup" data-bind="options: bookingAreas, value: selectedArea, optionsText: 'areaName', optionsValue: 'areaId', optionsCaption: '--Select Area--'"></select>
                <span class="bwsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please Select Area</div>
            </div>
            <input id="btnDealerPricePopup" class="action-btn text-uppercase margin-top15" style="display: block; margin-right: auto; margin-left: auto;" type="button" value="Get on road price" data-bind="event: { click: getPriceQuotePopup }">
            <div id="errMsgPopup" class="text-orange margin-top10 hide"></div>
        </div>
    </div>
</div>
</div>
<!--bw popup code ends here-->

<script type="text/javascript">

    var preSelectedCityId = 0;
    var preSelectedCityName = "";
    popupcity = $('#ddlCitiesPopup');
    popupArea = $('#ddlAreaPopup');
    var selectedModel = 0;
    var selectedMakeName = '', selectedModelName = '', selectedCityName = '', selectedAreaName = '', gaLabel = '';
    var abHostUrl = '<%= ConfigurationManager.AppSettings["ABApiHostUrl"]%>';
    var metroCitiesIds = [40, 12, 13, 10, 224, 1, 198, 105, 246, 176, 2, 128];
    var pageId, PQSourceId;
    var onCookieObj = {};

    // knockout popupData binding
    var viewModelPopup = {
        selectedCity: ko.observable(),
        bookingCities: ko.observableArray([]),
        selectedArea: ko.observable(),
        bookingAreas: ko.observableArray([]),
        hasAreas: ko.observable()
    };

    function findCityById(vm, id) {
        return ko.utils.arrayFirst(vm.bookingCities(), function (child) {
            return child.cityId === id;
        });
    }

    function FillCitiesPopup(modelId, makeName, modelName, pageIdAttr, pqSourceId) {
        PQSourceId = pqSourceId;
        $.ajax({
            type: "GET",
            url: "/api/PQCityList/?modelId=" + modelId,
            success: function (response) {
                selectedModel = modelId;
                pageId = pageIdAttr;
                if (makeName != undefined && makeName != '')
                    selectedMakeName = makeName;

                if (modelName != undefined && modelName != '')
                    selectedModelName = modelName;

                $('.blackOut-window,#popupWrapper').fadeIn(100);
                var cities = response.cities;
                var citySelected = null;
                if (cities) {
                    insertCitySeparator(cities);
                    checkCookies();
                    viewModelPopup.bookingCities(cities);
                    if (!isNaN(onCookieObj.PQCitySelectedId) && onCookieObj.PQCitySelectedId > 0 && viewModelPopup.bookingCities() && selectElementFromArray(viewModelPopup.bookingCities(), onCookieObj.PQCitySelectedId)) {
                        viewModelPopup.selectedCity(onCookieObj.PQCitySelectedId);
                        viewModelPopup.hasAreas(findCityById(viewModelPopup, onCookieObj.PQCitySelectedId).hasAreas);
                    }
                    popupcity.find("option[value='0']").prop('disabled', true);
                    popupcity.trigger('chosen:updated');
                    cityChangedPopup();
                }
                else {
                    viewModelPopup.bookingCities([]);
                    $('#ddlCitiesPopup').trigger("chosen:updated");
                }
            }
        });
    }


    function cityChangedPopup() {
        //gtmCodeAppender(pageId, "City Selected", null);
        if (viewModelPopup.selectedCity() != undefined) {

            viewModelPopup.hasAreas(findCityById(viewModelPopup, viewModelPopup.selectedCity()).hasAreas);
            if (viewModelPopup.hasAreas() != undefined && viewModelPopup.hasAreas()) {
                $.ajax({
                    type: "GET",
                    url: "/api/PQAreaList/?modelId=" + selectedModel + "&cityId=" + viewModelPopup.selectedCity(),
                    dataType: 'json',
                    success: function (response) {
                        areas = response.areas;
                        if (areas.length) {
                            viewModelPopup.bookingAreas(areas);
                            if (!isNaN(onCookieObj.PQAreaSelectedId) && onCookieObj.PQAreaSelectedId > 0 && selectElementFromArray(areas, onCookieObj.PQAreaSelectedId)) {
                                viewModelPopup.selectedArea(onCookieObj.PQAreaSelectedId);
                                onCookieObj.PQAreaSelectedId = 0;
                            }
                            $('#ddlAreaPopup').trigger("chosen:updated");
                        }
                        else {
                            viewModelPopup.selectedArea(0);
                            viewModelPopup.bookingAreas([]);
                            $('#ddlAreaPopup').trigger("chosen:updated");
                        }
                    },
                    error: function (e) {
                        viewModelPopup.selectedArea(0);
                        viewModelPopup.bookingAreas([]);
                        $('#ddlAreaPopup').trigger("chosen:updated");
                    }
                });
            }
            else {
                viewModelPopup.bookingAreas([]);
            }
        } else {
            viewModelPopup.bookingAreas([]);
        }
    }

    //function areaChangedPopup() {
    //    gtmCodeAppender(pageId, "Area Selected", null);
    //}


    function isValidInfoPopup() {
        isValid = true;
        var errMsg = "Missing fields:";

        if (viewModelPopup.selectedCity() == undefined) {
            errMsg += "City,";
            isValid = false;
        }
        if (viewModelPopup.bookingAreas().length > 0 && viewModelPopup.selectedArea() == undefined) {
            errMsg += "Area,";
            isValid = false;
        }
        if (!isValid) {
            errMsg = errMsg.substring(0, errMsg.length - 1);
            gtmCodeAppender(pageId, "Error in submission", errMsg);
        }
        return isValid;
    }

    function getPriceQuotePopup() {
        var cityId = viewModelPopup.selectedCity(), areaId = viewModelPopup.selectedArea() ? viewModelPopup.selectedArea() : 0;
        if (isValidInfoPopup()) {
            setLocationCookie($('#ddlCitiesPopup option:selected'), $('#ddlAreaPopup option:selected'));
            if (ga_pg_id != null && ga_pg_id == 2 && sourceHref == '1') {
                window.location.reload();// = "/new/bikeModel.aspx?model=cbshine#modelDetailsContainer";
            }
            else {
                var obj = {
                    'CityId': viewModelPopup.selectedCity(),
                    'AreaId': viewModelPopup.selectedArea(),
                    'ModelId': selectedModel,
                    'ClientIP': '',
                    'SourceType': '1',
                    'VersionId': 0,
                    'pQLeadId': PQSourceId,
                    'deviceId': getCookie('BWC')
                };

                $.ajax({
                    type: 'POST',
                    url: "/api/PriceQuote/",
                    data: obj,
                    dataType: 'json',
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader('utma', getCookie('__utma'));
                        xhr.setRequestHeader('utmz', getCookie('__utmz'));
                    },
                    success: function (json) {
                        var jsonObj = json;

                        selectedCityName = $("#ddlCitiesPopup option:selected").text();

                        if (areaId > 0)
                            selectedAreaName = $("#ddlAreaPopup option:selected").text();

                        if (selectedMakeName != "" && selectedModelName != "" && selectedCityName != "") {
                            gaLabel = selectedMakeName + ',' + selectedModelName + ',' + selectedCityName;

                            if (selectedAreaName != '')
                                gaLabel += ',' + selectedAreaName;
                        }

                        cookieValue = "CityId=" + viewModelPopup.selectedCity() + "&AreaId=" + (!isNaN(viewModelPopup.selectedArea()) ? viewModelPopup.selectedArea() : 0) + "&PQId=" + jsonObj.quoteId + "&VersionId=" + jsonObj.versionId + "&DealerId=" + jsonObj.dealerId;
                        //SetCookie("_MPQ", cookieValue);

                        if (jsonObj != undefined && jsonObj.quoteId > 0 && jsonObj.dealerId > 0) {
                            gtmCodeAppender(pageId, 'Dealer_PriceQuote_Success_Submit', gaLabel);
                            window.location = "/pricequote/dealerpricequote.aspx" + "?MPQ=" + Base64.encode(cookieValue);
                        }
                        else if (jsonObj != undefined && jsonObj.quoteId > 0) {
                            gtmCodeAppender(pageId, 'BW_PriceQuote_Success_Submit', gaLabel);
                            window.location = "/pricequote/quotation.aspx" + "?MPQ=" + Base64.encode(cookieValue);
                        } else {
                            gtmCodeAppender(pageId, 'BW_PriceQuote_Error_Submit', gaLabel);
                            $("#errMsgPopup").text("Oops. We do not seem to have pricing for given details.").show();
                        }
                    },
                    error: function (e) {
                        gtmCodeAppender(pageId, 'BW_PriceQuote_Error_Submit', gaLabel);
                        $("#errMsg").text("Oops. Some error occured. Please try again.").show();
                    }
                });
            }
        } else {
            gtmCodeAppender(pageId, 'BW_PriceQuote_Error_Submit', gaLabel);
            $("#errMsgPopup").text("Please select all the details").show();
        }
    }

    function gtmCodeAppender(pageId, action, label) {
        var category = '';
        if (pageId != null) {
            switch (pageId) {
                case "1":
                    category = 'Make_Page';
                    action = action;
                    break;
                case "2":
                    category = "CheckPQ_Series";
                    action = "CheckPQ_Series_" + action;
                    break;
                case "3":
                    category = "CheckPQ_Model";
                    action = "CheckPQ_Model_" + action;
                    break;
                case "4":
                    category = "Search_Page";
                    break;
                case "5":
                    category = "New_Bikes_Page";
                    break;
                case "6":
                    category = "HP";
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

    $(document).ready(function () {
        $('body').on('click', 'a.fillPopupData', function (e) {
            if (ga_pg_id != null & ga_pg_id == 2) {
                var attr = $(this).attr('ismodel');
                if (typeof attr !== typeof undefined && attr !== false) {
                    $('html, body').animate({
                        scrollTop: $("#breadcrumb").offset().top
                    }, 10);
                    sourceHref = '1';
                }
            }
            pageIdAttr = $(this).attr('pageCatId');
            e.preventDefault();
            $("#errMsgPopUp").empty();
            var str = $(this).attr('modelId');
            var makeName = $(this).attr('makeName'), modelName = $(this).attr('modelName');
            var modelIdPopup = parseInt(str, 10);
            PQSourceId = $(this).attr("pqSourceId");
            FillCitiesPopup(modelIdPopup, makeName, modelName, pageIdAttr, PQSourceId);
            gtmCodeAppender(pageIdAttr, "Get_On_Road_Price_Click", modelName);
        });

        $('#popupWrapper .close-btn,.blackOut-window').mouseup(function () {
            $('.blackOut-window,#popupWrapper').fadeOut(100);
        });

        $("#ddlCitiesPopup").chosen({ no_results_text: "No matches found!!" });
        $("#ddlAreaPopup").chosen({ no_results_text: "No matches found!!" });
        $('.chosen-container').attr('style', 'width:100%;');

        ko.applyBindings(viewModelPopup, $("#popupContent")[0]);
    });


</script>
<script type="text/javascript" src="<%= !string.IsNullOrEmpty(staticUrl1) ? "http://st2.aeplcdn.com" + staticUrl1 : string.Empty %>/src/common/chosen.jquery.min.js?<%= staticFileVersion1 %>"></script>
