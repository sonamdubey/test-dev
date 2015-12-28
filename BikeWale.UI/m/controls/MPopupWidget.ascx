<%@ Control Language="C#" AutoEventWireup="true" Inherits="Bikewale.Mobile.controls.MPopupWidget" %>  
<script type="text/javascript">
    var sourceHref = '0';
</script>
<!--bw popup code starts here-->
<div class="bw-city-popup bwm-fullscreen-popup hide bw-popup-sm text-center" id="popupWrapper">
	<div class="popup-inner-container">
        
    	<div class="bwmsprite close-btn position-abt pos-top10 pos-right10 cur-pointer"></div>
        <div class="icon-outer-container rounded-corner50percent margin-bottom15">
            <div class="icon-inner-container rounded-corner50percent">
                <span class="bwmsprite orp-location-icon margin-top20"></span>
            </div>
        </div>
    	<p class="font18 margin-bottom10 text-capitalize">Please Tell Us Your Location</p>
        <div class="padding-top5" id="popupContent">
            <div class="text-light-grey margin-bottom15"><span class="red">*</span>Get on-road prices by just sharing your location!</div>
         <div>
                <select id="ddlCitiesPopup" class="form-control chosen-select" tabindex="2" data-bind="options: bookingCities, value: selectedCity, optionsText: 'CityName', optionsValue: 'CityId', optionsCaption: '--Select City--', event: { change: cityChangedPopup }, chosen: { width: '100%' }" ></select> 
                <span class="bwsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please Select City</div>   
         </div>
            <div  data-bind="visible: bookingAreas().length > 0" class="margin-top15">
                <select  class="form-control chosen-select" id="ddlAreaPopup" data-bind="options: bookingAreas, value: selectedArea, optionsText: 'AreaName', optionsValue: 'AreaId', optionsCaption: '--Select Area--', chosen: { width: '100%' }"></select>
                <span class="bwsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please Select Area</div>
            </div> 
            <div class="center-align margin-top20 text-center">                
                <a id="btnDealerPricePopup" class="btn btn-orange btn-full-width font18" data-bind="event: { click: getPriceQuotePopup }">Get on road price</a>
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
var onCookieObj = {};
var selectedMakeName = '', selectedModelName = '', selectedCityName = '', selectedAreaName = '', gaLabel = '';

// knockout popupData binding
var viewModelPopup = {
    selectedCity: ko.observable(),
    bookingCities: ko.observableArray([]),
    selectedArea: ko.observable(),
    bookingAreas: ko.observableArray([])
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

function FillCitiesPopup(modelId, makeName, modelName, pageIdAttr) {
    $.ajax({
        type: "POST",
        url: "/ajaxpro/Bikewale.Ajax.AjaxPriceQuote,Bikewale.ashx",
        data: '{"modelId":"' + modelId + '"}',
        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetPriceQuoteCitiesNew"); },
        success: function (response) {
            selectedModel = modelId;
            pageId = pageIdAttr;
            if (makeName != undefined && makeName != '')
                selectedMakeName = makeName;

            if (modelName != undefined && modelName != '')
                selectedModelName = modelName;
            
            $('#popupWrapper').fadeIn(100);
            $('body').addClass('lock-browser-scroll');
            $(".blackOut-window").show();
            appendHash("onroadprice");
            var obj = JSON.parse(response);
            var cities = JSON.parse(obj.value);
            var citySelected = null;
            if (cities)
            {
                insertCitySeparator(cities);
                checkCookies();
                viewModelPopup.bookingCities(cities);
                if (!isNaN(onCookieObj.PQCitySelectedId) && onCookieObj.PQCitySelectedId > 0 && viewModelPopup.bookingCities() && selectElementFromArray(viewModelPopup.bookingCities(), onCookieObj.PQCitySelectedId)) {
                    viewModelPopup.selectedCity(onCookieObj.PQCitySelectedId);
                }
                popupcity.find("option[value='0']").prop('disabled', true);
                popupcity.trigger('chosen:updated');
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
                    if (!isNaN(onCookieObj.PQAreaSelectedId) && onCookieObj.PQAreaSelectedId > 0 && selectElementFromArray(areas, onCookieObj.PQAreaSelectedId)) {
                        viewModelPopup.selectedArea(onCookieObj.PQAreaSelectedId);
                    }
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
        //set global cookie
        setLocationCookie($('#ddlCitiesPopup option:selected'), $('#ddlAreaPopup option:selected'));       
        if (ga_pg_id != null && ga_pg_id == 2) {
            window.location.reload();
        }
        else {
            $.ajax({
                type: 'POST',
                url: "/ajaxpro/Bikewale.Ajax.AjaxBikeBooking,Bikewale.ashx",
                data: '{"cityId":"' + cityId + '", "areaId":"' + areaId + '", "modelId":"' + selectedModel + '", "isMobileSource":true}',
                dataType: 'json',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "ProcessPQ"); },
                success: function (json) {
                    var jsonObj = $.parseJSON(json.value);
                    selectedCityName = $("#ddlCitiesPopup option:selected").text();

                    if (areaId > 0)
                        selectedAreaName = $("#ddlAreaPopup option:selected").text();

                    if (selectedMakeName != "" && selectedModelName != "" && selectedCityName != "") {
                        gaLabel = selectedMakeName + ',' + selectedModelName + ',' + selectedCityName;

                        if (selectedAreaName != '')
                            gaLabel += ',' + selectedAreaName;
                    }
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


$(document).ready(function () {
    $('#popupWrapper .close-btn,.blackOut-window').click(function () {
        $('.bw-city-popup').fadeOut(100);
        $('body').removeClass('lock-browser-scroll');
        $(".blackOut-window").hide();
        $('a.fillPopupData').removeClass('ui-btn-active');
    });

    ko.applyBindings(viewModelPopup, $("#popupContent")[0]);
});

$("a.fillPopupData").on("click", function (e) {
    //if (ga_pg_id != null & ga_pg_id == 2) {
    //    var attr = $(this).attr('ismodel');
    //    if (typeof attr !== typeof undefined && attr !== false) {
    //        $('html, body').animate({
    //            scrollTop: $("#modelDetailsContainer").offset().top
    //        }, 10);
    //        sourceHref = '1';
    //    }
    //}
    e.stopPropagation();    
    $("#errMsgPopUp").empty();
    var str = $(this).attr('modelId');
    var pageIdAttr = $(this).attr('pagecatid');
    var makeName = $(this).attr('makeName'), modelName = $(this).attr('modelName');
    var modelIdPopup = parseInt(str, 10);
    gtmCodeAppender(pageIdAttr, "Get_On_Road_Price_Click", modelName);
    FillCitiesPopup(modelIdPopup,makeName,modelName,pageIdAttr);
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
</script>
