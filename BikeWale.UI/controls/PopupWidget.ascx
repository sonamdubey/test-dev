<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.controls.PopupWidget" %>
<!--bw popup code starts here-->
<link href="/css/chosen.min.css" rel="stylesheet" />

<div class="bw-popup hide bw-popup-sm" id="popupWrapper">
    <div class="popup-inner-container" stopBinding: true>
        <div class="bwsprite popup-close-btn close-btn position-abt pos-top10 pos-right10 cur-pointer"></div>
        <div class="cityPop-icon-container">
            <div class="cityPopup-box rounded-corner50 margin-bottom20">
                <span class="bwsprite cityPopup-icon margin-top10"></span>
            </div>
        </div>
        <p class="font20 margin-top15 text-capitalize text-center">Please Tell Us Your Location</p>
        <p class="text-light-grey margin-bottom15 margin-top15 text-capitalize text-center">Get on-road prices by just sharing your location!</p>
        <div class="padding-top10" id="popupContent">
            <div>
                <select data-placeholder="--Select City--" class="chosen-select"  id="ddlCitiesPopup" tabindex="2" data-bind="options: bookingCities, value: selectedCity, optionsText: 'CityName', optionsValue: 'CityId', optionsCaption: '--Select City--', event: { change: cityChangedPopup }" ></select> 
                 <span class="bwsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please Select City</div>
            </div>
            <div data-bind="visible: bookingAreas().length > 0" style="margin-top:10px">
                <select data-placeholder="--Select Area--" class="chosen-select" id="ddlAreaPopup" data-bind="options: bookingAreas, value: selectedArea, optionsText: 'AreaName', optionsValue: 'AreaId', optionsCaption: '--Select Area--', event: { change: areaChangedPopup }"></select>
                <span class="bwsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please Select Area</div>
            </div>
                <input id="btnDealerPricePopup" class="action-btn text-uppercase margin-top15" style="display:block;margin-right:auto;margin-left:auto;" type="button" value="Confirm Location" data-bind="event: { click: getPriceQuotePopup }">
                <div id="errMsgPopup" class="text-orange margin-top10 hide"></div>
            </div>
        </div> 
    </div>
</div>
<!--bw popup code ends here-->

<script type="text/javascript">

    var preSelectedCityId =0;
    var preSelectedCityName = "";
    popupcity = $('#ddlCitiesPopup');
    popupArea = $('#ddlAreaPopup');
    var selectedModel = 0;
    var abHostUrl = '<%= ConfigurationManager.AppSettings["ABApiHostUrl"]%>';
    var metroCitiesIds = [40, 12, 13, 10, 224, 1, 198, 105, 246, 176, 2, 128];
    var pageId;


    // knockout popupData binding
    var viewModelPopup = {
        selectedCity: ko.observable(),
        bookingCities: ko.observableArray([]),
        selectedArea: ko.observable(),
        bookingAreas: ko.observableArray([])
    };


    function FillCitiesPopup(modelId) {
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
                        if (metroCitiesIds.indexOf(cities[i].CityId) > -1) {
                            var currentCity = cities[i];
                            cities.splice(cities.indexOf(currentCity), 1);
                            cities.splice(initIndex++, 0, currentCity);
                        }

                        if(preSelectedCityId == cities[i].CityId )
                        {
                            citySelected = cities[i];
                        }
                            
                    }

                    cities.splice(initIndex, 0, { CityId: 0, CityName: "---------------", CityMaskingName: null }); 

                    viewModelPopup.bookingCities(cities);

                    if(citySelected!=null)
                    {
                        viewModelPopup.selectedCity(citySelected.CityId);
                    }
                        

                    $("#ddlCitiesPopup option[value=0]").prop("disabled", "disabled");
                    if ($("#ddlCitiesPopup option:last-child").val() == "0") {
                        $("#ddlCitiesPopup option:last-child").remove();
                    }
                    if ($("#ddlCitiesPopup option:first-child").next().val() == "0") {
                        $("#ddlCitiesPopup option[value=0]").remove();
                    }
                    $('#ddlCitiesPopup').trigger("chosen:updated");                        

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
        gtmCodeAppender(pageId, "City Selected", null);
        if (viewModelPopup.selectedCity() != undefined) {
            $.ajax({
                type: "POST",
                url: "/ajaxpro/Bikewale.Ajax.AjaxPriceQuote,Bikewale.ashx",
                data: '{"cityId":"' + viewModelPopup.selectedCity() + '","modelId":"' + selectedModel + '"}',
                dataType: 'json',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetPriceQuoteArea"); },
                success: function (response) {
                    areas = $.parseJSON(response.value);
                    if (areas.length) {
                        viewModelPopup.bookingAreas(areas);
                        $('#ddlAreaPopup').trigger("chosen:updated");
                    }
                    else {
                        viewModelPopup.selectedArea(0);
                        viewModelPopup.bookingAreas([]);
                        $('#ddlAreaPopup').trigger("chosen:updated");
                    }
                }
            });
        } else {
            viewModelPopup.bookingAreas([]);
        }
    }

    function areaChangedPopup() {
        gtmCodeAppender(pageId, "Area Selected", null);
    }


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

            //set global cookie
            if (cityId > 0) {
                cityName = $(popupcity).find("option[value=" + cityId + "]").text();
                cookieValue = cityId + "_" + cityName;
                SetCookieInDays("location", cookieValue, 365);
            }

            $.ajax({
                type: 'POST',
                url: "/ajaxpro/Bikewale.Ajax.AjaxBikeBooking,Bikewale.ashx",
                data: '{"cityId":"' + cityId + '", "areaId":"' + areaId + '", "modelId":"' + selectedModel + '", "isMobileSource":false}',
                dataType: 'json',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "ProcessPQ"); },
                success: function (json) {
                    var jsonObj = $.parseJSON(json.value);
                    if (jsonObj != undefined && jsonObj.quoteId > 0 && jsonObj.dealerId > 0) {
                        gtmCodeAppender(pageId, "Successful submission - DealerPQ", "Model : " + selectedModel + ', City : ' + viewModelPopup.selectedCity() + ', Area : ' + viewModelPopup.selectedArea());
                        window.location = "/pricequote/dealerpricequote.aspx";
                    }
                    else if (jsonObj != undefined && jsonObj.quoteId > 0) {
                        gtmCodeAppender(pageId, "Successful submission - BikeWalePQ", "Model : " + selectedModel + ', City : ' + viewModelPopup.selectedCity() + ', Area : ' + viewModelPopup.selectedArea());
                        window.location = "/pricequote/quotation.aspx";
                    } else {
                        gtmCodeAppender(pageId, "Error in submission", null);
                        $("#errMsgPopup").text("Oops. We do not seem to have pricing for given details.").show();
                    }
                },
                error: function (e) {
                    gtmCodeAppender(pageId, "Error in submission", null);
                    $("#errMsg").text("Oops. Some error occured. Please try again.").show();
                }
            });
        } else {
            gtmCodeAppender(pageId, "Error in submission", null);
            $("#errMsgPopup").text("Please select all the details").show();
        }
    }

    function gtmCodeAppender(pageId, action, label) {
        if (pageId != null) {
            switch (pageId) {
                case "1":
                    category = 'CheckPQ_Make';
                    action = "CheckPQ_Make_" + action;
                    break;
                case "2":
                    category = "CheckPQ_Series";
                    action = "CheckPQ_Series_" + action;
                    break;
                case "3":
                    category = "CheckPQ_Model";
                    action = "CheckPQ_Model_" + action;
                    break;
            }
            if (label) {
                dataLayer.push({ 'event': 'product_bw_gtm', 'cat': category, 'act': action, 'lab': label });
            }
            else {
                dataLayer.push({ 'event': 'product_bw_gtm', 'cat': category, 'act': action });
            }
        }

    }

    function checkCookies()
    {
        c = document.cookie.split('; ');
        for(i=c.length-1; i>=0; i--)
        {
            C = c[i].split('=');
            if(C[0]=="location")
            {
                var cData = (String(C[1])).split('_');
                preSelectedCityId = parseInt(cData[0]);
                preSelectedCityName = cData[1];
            }
        } 
    }

    $(function () {
        $("a.fillPopupData").click(function (e) {
            pageId = $(this).attr('pageCatId');
            gtmCodeAppender(pageId, "Button Clicked", null);
            e.preventDefault();
            $("#errMsgPopUp").empty();
            var str = $(this).attr('modelId');
            var modelIdPopup = parseInt(str, 10);
            selectedModel = modelIdPopup;
            $('.blackOut-window,#popupWrapper').fadeIn(100);
            FillCitiesPopup(modelIdPopup);
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
<script type="text/javascript" src="/src/common/chosen.jquery.min.js"></script>