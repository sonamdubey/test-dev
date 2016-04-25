<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.controls.PopupWidget" %>
<script runat="server">
    private string staticUrl1 = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
    private string staticFileVersion1 = System.Configuration.ConfigurationManager.AppSettings["staticFileVersion"];
</script>
<script src="<%= staticUrl1 != "" ? "http://st2.aeplcdn.com" + staticUrl1 : "" %>/src/lscache.min.js?<%= staticFileVersion1%>"></script>
<!--bw popup code starts here-->
<script type="text/javascript">
    lscache.flushExpired();  //remove expired
    var modelCityKey = "mc_";
    var cityAreaKey = "ca_";
    var sourceHref = '0';
    var cityClicked = false;
    var areaClicked = false;
</script>
<link href="<%= !string.IsNullOrEmpty(staticUrl1) ? "http://st2.aeplcdn.com" + staticUrl1 : string.Empty %>/css/chosen.min.css?<%=staticFileVersion1 %>" rel="stylesheet" />
<style type="text/css">
.progress-bar {width:0;height:2px;background:#16A085;bottom:0px;left:0;border-radius:2px;}
.btn-loader {background-color:#822821;}
.btnSpinner {right:22px;top:10px;z-index:9;background:rgb(255, 255, 255);}
#popupWrapper .form-control-box { height:40px; }
#popupWrapper .form-control, #popupWrapper .chosen-container { border:none; }
#divCityLoader, #divAreaLoader { border:1px solid #ccc; border-radius:4px; }
</style>
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
            <div id="divCityLoader" class="margin-top10 form-control-box">
                <div class="placeholder-loading-text form-control">Loading Cities..<span class="fa fa-spinner fa-spin position-abt text-black btnSpinner"></span></div>
                <div data-bind="visible: bookingCities().length > 0">
                    <select data-placeholder="--Select City--" class="chosen-select" id="ddlCitiesPopup" tabindex="2" data-bind="options: bookingCities(), value: selectedCity, optionsText: 'name', optionsValue: 'id', optionsCaption: '--Select City--', event: { change: cityChangedPopup }"></select>
                    <span class="bwsprite error-icon error-tooltip-siblings"></span>
                    <div class="bw-blackbg-tooltip error-tooltip-siblings"></div>
                </div>
                <span class="position-abt progress-bar"></span>
            </div>  
                        
            <div id="divAreaLoader" class="hide margin-top10 form-control-box">
                <div class="placeholder-loading-text form-control">Loading Areas..<span class="fa fa-spinner fa-spin position-abt text-black btnSpinner"></span></div>
                <div data-bind="visible: bookingAreas().length > 0">                              
                    <select data-placeholder="--Select Area--" class="chosen-select" id="ddlAreaPopup" data-bind="options: bookingAreas(), value: selectedArea, optionsText: 'name', optionsValue: 'id', optionsCaption: '--Select Area--', event: { change: isValidInfoPopup }"></select>                
                    <span class="bwsprite error-icon error-tooltip-siblings"></span>                
                    <div class="bw-blackbg-tooltip error-tooltip-siblings"></div>
                </div>
                <span class="position-abt progress-bar"></span>
            </div>           
                        
            <input id="btnDealerPricePopup" class="action-btn margin-top15 margin-left70" style="display: block;" type="button" value="Show On-Road Price" data-bind="click: getPriceQuotePopup, enable: (!hasAreas() && bookingCities().length > 0) || (hasAreas && bookingAreas().length > 0)">
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
        hasAreas: ko.observable(),
        getPriceQuoteButtonCliked: ko.observable(false)
    };

    function findCityById(vm, id) {
        return ko.utils.arrayFirst(vm.bookingCities(), function (child) {
            return (child.id === id || child.cityId === id);
        });
    }


    function FillCitiesPopup(modelId, makeName, modelName, pageIdAttr, pqSourceId) {        
        PQSourceId = pqSourceId;
        var isAborted = false;
        if (viewModelPopup.bookingCities().length < 1 || (selectedModel != modelId)) {            
            modelCityKey = "mc_" + modelId;
            $.ajax({
                type: "GET",
                url: "/api/v2/PQCityList/?modelId=" + modelId,
                beforeSend: function (xhr) {
                    viewModelPopup.bookingCities([]);
                    viewModelPopup.bookingAreas([]);
                    //viewModelPopup.selectedCity(0);
                    //preSelectedCityId = 0;
                    //viewModelPopup.selectedArea(0);
                    startLoading($("#divCityLoader"));
                    $("#divCityLoader .placeholder-loading-text").show();
                    if (data = lscache.get(modelCityKey)) {
                        var cities = ko.toJS(data);                        
                        if (cities) {
                            selectedModel = modelId;
                            insertCitySeparatorNew(cities);
                            checkCookies();
                            stopLoading($("#divCityLoader"));                            
                            $("#divCityLoader .placeholder-loading-text").hide();
                            viewModelPopup.bookingCities(data);
                            isAborted = true;
                            xhr.abort();
                        }
                        else {
                            viewModelPopup.bookingCities([]);
                        }                        
                    }
                },
                success: function (response) {                   
                    selectedModel = modelId;
                    pageId = pageIdAttr;
                    if (makeName != undefined && makeName != '')
                        selectedMakeName = makeName;

                    if (modelName != undefined && modelName != '')
                        selectedModelName = modelName;

                    lscache.set(modelCityKey, response.cities, 60);
                    var cities = response.cities;                    
                    if (cities) {                        
                        stopLoading($("#divCityLoader"));
                        $("#divCityLoader .placeholder-loading-text").hide();
                        insertCitySeparatorNew(cities);
                        checkCookies();
                        viewModelPopup.bookingCities(cities);                        
                    }
                    else {
                        viewModelPopup.bookingCities([]);
                        $('#ddlCitiesPopup').trigger("chosen:updated");
                    }
                },
                complete: function () {
                    completeCityPopup();
                }
            });

            if (isAborted)
            {                
                completeCityPopup();
            }
        }
    }

    function completeCityPopup()
    {
        if (!isNaN(onCookieObj.PQCitySelectedId) && onCookieObj.PQCitySelectedId > 0 && viewModelPopup.bookingCities() && selectElementFromArray(viewModelPopup.bookingCities(), onCookieObj.PQCitySelectedId)) {
            viewModelPopup.selectedCity(onCookieObj.PQCitySelectedId);
            viewModelPopup.hasAreas(findCityById(viewModelPopup, onCookieObj.PQCitySelectedId).hasAreas);
        }
        popupcity.find("option[value='0']").prop('disabled', true);
        popupcity.trigger('chosen:updated');
        cityChangedPopup();

        ev = $._data($('ul.chosen-results')[0], 'events');
        if (!(ev && ev.click)) {
            $($('ul.chosen-results')[0]).on('click', 'li', function (e) {
                if (cityClicked == false) {
                    if (ga_pg_id != null && ga_pg_id == 2) {
                        var bkVersionLocn = myBikeName + '_' + getBikeVersion() + '_' + $('#ddlCitiesPopup option:selected').html();
                        if (viewModelPopup.hasAreas()) {
                            dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'City_Selected_Has_Area', 'lab': getBikeVersion() + '_' + $('#ddlCitiesPopup option:selected').html() });
                        }
                        else {
                            dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'City_Selected_Doesnt_Have_Area', 'lab': bkVersionLocn });
                        }
                        cityClicked = true;
                    }
                }
            });
        }
    }

    function cityChangedPopup() {
        var isAborted = false;        
        if (viewModelPopup.selectedCity() != undefined) {
            viewModelPopup.hasAreas(findCityById(viewModelPopup, viewModelPopup.selectedCity()).hasAreas);            
            if (viewModelPopup.hasAreas() != undefined && viewModelPopup.hasAreas() && selectedModel > 0) {
                cityAreaKey = "ca_" + viewModelPopup.selectedCity().toString();
                $.ajax({
                    type: "GET",
                    url: "/api/v2/PQAreaList/?modelId=" + selectedModel + "&cityId=" + viewModelPopup.selectedCity(),
                    dataType: 'json',
                    beforeSend: function (xhr) {
                        viewModelPopup.bookingAreas([]);
                        //viewModelPopup.selectedArea(0);
                        $("#divAreaLoader").show();
                        $("#divAreaLoader .placeholder-loading-text").show();
                        startLoading($("#divAreaLoader"));                        
                        if (data = lscache.get(cityAreaKey)) {
                            var areas = ko.toJS(data);                            
                            if (areas) {                                
                                stopLoading($("#divAreaLoader"));
                                $("#divAreaLoader .placeholder-loading-text").hide();
                                viewModelPopup.bookingAreas(data);
                                isAborted = true;
                                xhr.abort();
                            }
                        }
                    },
                    success: function (response) {
                        var areas = response.areas;                        
                        lscache.set(cityAreaKey, areas, 60);
                        if (areas.length) {                            
                            stopLoading($("#divAreaLoader"));
                            $("#divAreaLoader .placeholder-loading-text").hide();
                            viewModelPopup.bookingAreas(areas);                                                      
                        }
                        else {                            
                            viewModelPopup.bookingAreas([]);
                            $("#divAreaLoader").hide();
                            $('#ddlAreaPopup').trigger("chosen:updated");                           
                        }
                    },
                    error: function (e) {                        
                        viewModelPopup.bookingAreas([]);
                        $('#ddlAreaPopup').trigger("chosen:updated");
                    },
                    complete: function () {                        
                        completeAreaPopup();
                    }
                });
            }
            else {
                viewModelPopup.bookingAreas([]);
                $("#divAreaLoader").hide();
            }
        } else {
            viewModelPopup.bookingAreas([]);
            $("#divAreaLoader").hide();
        }

        if (isAborted)
        {
            completeAreaPopup();
        }    
        isValidInfoPopup();
    }

    function completeAreaPopup() {

        if (!isNaN(onCookieObj.PQAreaSelectedId) && onCookieObj.PQAreaSelectedId > 0 && selectElementFromArray(viewModelPopup.bookingAreas(), onCookieObj.PQAreaSelectedId)) {
            viewModelPopup.selectedArea(onCookieObj.PQAreaSelectedId);
            onCookieObj.PQAreaSelectedId = 0;
        }
        $('#ddlAreaPopup').trigger("chosen:updated");

        ev = $._data($('ul.chosen-results')[1], 'events');
        if (!(ev && ev.click)) {
            $($('ul.chosen-results')[1]).on('click', 'li', function (e) {
                if (areaClicked == false) {
                    if (ga_pg_id != null && ga_pg_id == 2) {
                        var bkVersionLocn = myBikeName + '_' + getBikeVersion() + '_' + $('#ddlCitiesPopup option:selected').html() + '_' + $('#ddlAreaPopup option:selected').html();
                        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Area_Selected', 'lab': bkVersionLocn });
                        areaClicked = true;
                    }
                }
            });
        }       
    }

    function isValidInfoPopup() {
        isValid = true;
        var errMsg = "",
            errMsgParent;

        if (viewModelPopup.selectedCity() == undefined && viewModelPopup.getPriceQuoteButtonCliked()) {
            errMsgParent = $('#divCityLoader.form-control-box');
            errMsg += "City,";
            isValid = false;
            showCityAreaError(errMsgParent,errMsg);
        }

        else {
            removeCityAreaError($('#divCityLoader.form-control-box'));
        }

        if (viewModelPopup.bookingAreas().length > 0 && viewModelPopup.selectedArea() == undefined && viewModelPopup.getPriceQuoteButtonCliked()) {
            errMsgParent = $('#divAreaLoader.form-control-box');
            errMsg += "Area,";
            isValid = false;
            showCityAreaError(errMsgParent, errMsg);
        }

        else {
            removeCityAreaError($('#divAreaLoader.form-control-box'));
        }
       
        return isValid;
    }

    function showCityAreaError(errMsgParent, errMsg)
    {
        errMsgParent.find('.error-tooltip-siblings').show();
        errMsgParent.css({ 'border-color': 'red' });
        errMsg = errMsg.substring(0, errMsg.length - 1);
        errMsgParent.find('.bw-blackbg-tooltip').text("Please select " + errMsg);
    }

    function removeCityAreaError(errMsgParent) {
        errMsgParent.css({ 'border-color': '#ccc' });
        errMsgParent.find('.error-tooltip-siblings').hide();
        errMsgParent.find('.bw-blackbg-tooltip').text("");
    }

    function getPriceQuotePopup() {
        var cityId = viewModelPopup.selectedCity(), areaId = viewModelPopup.selectedArea() ? viewModelPopup.selectedArea() : 0;
        viewModelPopup.getPriceQuoteButtonCliked(true);
        if (isValidInfoPopup()) {
            //$("#errMsgPopup").text("");
            setLocationCookie($('#ddlCitiesPopup option:selected'), $('#ddlAreaPopup option:selected'));
            if (ga_pg_id != null && ga_pg_id == 2 && sourceHref == '1') {
                try {
                    var selArea = '';
                    if ($('#ddlAreaPopup option:selected').index() > 0) {
                        selArea = '_' + $('#ddlAreaPopup option:selected').html();
                    }
                    bikeVersionLocation = myBikeName + '_' + getBikeVersion() + '_' + $('#ddlCitiesPopup option:selected').html() + selArea;
                    if (bikeVersionLocation != null) {
                        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Show_On_Road_Price_Selected', 'lab': bikeVersionLocation });
                    }
                }
                catch (err) { }
                window.location.reload();
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
            gtmCodeAppender(pageId, "Error in submission", $("#errMsgPopup").text().replace("Please select", "Missing fields :"));           
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

    function startLoading(ele) {        
        try {
           var _self = $(ele).find(".progress-bar").css({'width':'0'}).show();
            _self.animate({ width: '100%' }, 7000);
        }
        catch (e) {  return };
    }

    function stopLoading(ele) {
        try {
            var _self = $(ele).find(".progress-bar");
            _self.stop(true, true).css({'width':'100%'}).fadeOut(1000);
        }
        catch (e) { return };
    }

    $(document).ready(function () {
        $('body').on('click', 'a.fillPopupData', function (e) {
            $('.blackOut-window,#popupWrapper').fadeIn(100);

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
