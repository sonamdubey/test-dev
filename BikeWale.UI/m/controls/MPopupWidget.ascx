<%@ Control Language="C#" AutoEventWireup="true" Inherits="Bikewale.Mobile.controls.MPopupWidget" %>  

<!--bw popup code starts here-->

<div class="bw-popup hide bw-popup-sm text-center" id="popupWrapper">
	<div class="popup-inner-container">
        
    	<div class="bwmsprite close-btn position-abt pos-top10 pos-right10 cur-pointer"></div>
        <div class="cityPopup-box rounded-corner50percent margin-bottom20">
            	<span class="bwmsprite cityPopup-icon margin-top10"></span>
            </div>
    	<p class="font20 margin-bottom10 text-capitalize">Please Tell Us Your Location</p>
        <div class="padding-top5" id="popupContent">
            <div class="text-light-grey margin-bottom15"><span class="red">*</span>Get on-road prices by just sharing your location!</div>
         <div>
                <select id="ddlCitiesPopup" class="form-control" tabindex="2" data-bind="options: bookingCities, value: selectedCity, optionsText: 'CityName', optionsValue: 'CityId', optionsCaption: '--Select City--', event: { change: cityChangedPopup }" ></select> 
                <span class="bwsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please Select City</div>   
         </div>
            <div  data-bind="visible: bookingAreas().length > 0" class="margin-top15">
                <select  class="form-control chosen-select" id="ddlAreaPopup" data-bind="options: bookingAreas, value: selectedArea, optionsText: 'AreaName', optionsValue: 'AreaId', optionsCaption: '--Select Area--'"></select>
                <span class="bwsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please Select Area</div>
            </div> 
            <div class="center-align margin-top20 text-center">                
                <a id="btnDealerPricePopup" class="btn btn-orange btn-full-width font18" data-bind="event: { click: getPriceQuotePopup }">Confirm Location</a>
                <div id="errMsgPopup" class="red-text margin-top10 hide"></div>
            </div>            
        </div>
    </div>
</div>

<!--bw popup code ends here-->
<script type="text/javascript">
var selectedModel = 0;
var abHostUrl = '<%= ConfigurationManager.AppSettings["ABApiHostUrl"]%>';
var metroCitiesIds = [40, 12, 13, 10, 224, 1, 198, 105, 246, 176, 2, 128];
var preSelectedCityId = 0;
var preSelectedCityName = "";
popupcity = $('#ddlCitiesPopup');
popupArea = $('#ddlAreaPopup');

// knockout popupData binding
var viewModelPopup = {
    selectedCity: ko.observable(),
    bookingCities: ko.observableArray([]),
    selectedArea: ko.observable(),
    bookingAreas: ko.observableArray([])
};

function checkCookies(cookieName) {
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

function FillCitiesPopup(modelId) {
    $.ajax({
        type: "POST",
        url: "/ajaxpro/Bikewale.Ajax.AjaxPriceQuote,Bikewale.ashx",
        data: '{"modelId":"' + modelId + '"}',
        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetPriceQuoteCitiesNew"); },
        success: function (response) {
            $('.blackOut-window,#popupWrapper').fadeIn(100);
            var obj = JSON.parse(response);
            var cities = JSON.parse(obj.value);
            if (cities)
            {
                checkCookies();
                var initIndex = 0;
                for (var i = 0; i < cities.length; i++) {
                    if (metroCitiesIds.indexOf(cities[i].CityId) > -1) {
                        var currentCity = cities[i];
                        cities.splice(cities.indexOf(currentCity), 1);
                        cities.splice(initIndex++, 0, currentCity);
                    }
                }
                cities.splice(initIndex, 0, { CityId: 0, CityName: "---------------", CityMaskingName: null });
                viewModelPopup.bookingCities(cities);
                $("#ddlCitiesPopup option[value=0]").prop("disabled", "disabled");
                if ($("#ddlCitiesPopup option:last-child").val() == "0") {
                    $("#ddlCitiesPopup option:last-child").remove();
                }
                if ($("#ddlCitiesPopup option:first-child").next().val() == "0") {
                    $("#ddlCitiesPopup option[value=0]").remove();
                }
                cityChangedPopup();
            }
            else {
                viewModelPopup.bookingCities([]);
            }
        }      
    });
}


function cityChangedPopup() {
    if (viewModelPopup.selectedCity() != undefined) {
        $.ajax({
            type: "POST",
            url: "/ajaxpro/Bikewale.Ajax.AjaxPriceQuote,Bikewale.ashx",
            data: '{"cityId":"' + viewModelPopup.selectedCity() + '","modelId":"' + selectedModel + '"}',
            dataType: 'json',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetPriceQuoteArea"); },
            success: function (response) {
                areas=$.parseJSON(response.value);
                if (areas.length) {
                    viewModelPopup.bookingAreas(areas);
                }
                else {
                    viewModelPopup.selectedArea = ko.observable(0);
                    viewModelPopup.bookingAreas([]);
                }
            }
        });
    } else {
        viewModelPopup.bookingAreas([]);
    }
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
        $.ajax({
            type: 'POST',
            url: "/ajaxpro/Bikewale.Ajax.AjaxBikeBooking,Bikewale.ashx",
            data: '{"cityId":"' + cityId + '", "areaId":"' + areaId + '", "modelId":"' + selectedModel + '", "isMobileSource":true}',
            dataType: 'json',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "ProcessPQ"); },
            success: function (json) {
                var jsonObj = $.parseJSON(json.value);
                if (jsonObj.quoteId > 0 && jsonObj.dealerId > 0) {
                    window.location = "/m/pricequote/dealerpricequote.aspx";
                }
                else if (jsonObj.quoteId > 0) {
                    window.location = "/m/pricequote/quotation.aspx";
                } else {
                    $("#errMsgPopup").text("Oops. We do not seem to have pricing for given details.").show();
                }
            },
            error: function (e) {
                $("#errMsg").text("Oops. Some error occured. Please try again.").show();
                dataLayer.push({ 'event': 'product_bw_gtm', 'cat': category, 'act': 'Error in submission' });
            }
        });
    } else {       
        $("#errMsgPopup").text("Please select all the details").show();
    }
}


$(document).ready(function () {
    $('#popupWrapper .close-btn,.blackOut-window').click(function () {
        $('.blackOut-window,.bw-popup').fadeOut(100);
        $('a.fillPopupData').removeClass('ui-btn-active');
    });

    ko.applyBindings(viewModelPopup, $("#popupContent")[0]);
});


$("a.fillPopupData").delegate("click", function (e) {
    e.preventDefault();
    $("#errMsgPopUp").empty();
    var str = $(this).attr('modelId');
    var modelIdPopup = parseInt(str, 10);
    selectedModel = modelIdPopup;
    FillCitiesPopup(modelIdPopup);
});
</script>
