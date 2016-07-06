<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.BikeBookingWidget" %>
<style type="text/css">
    .red-border { border:1px solid #c62000;}
    .inner-content {border: 1px solid #eaeaea; margin-bottom: 20px; padding: 10px;}
    .bookbike-box h2 { padding-bottom:10px; border-bottom:2px solid #c62000;}
    .bookbike-box select { width:100%; margin-bottom:10px;}
    .bookbike-hrz-box select { width:220px;}
</style>
<div class="right-grid">
    <!--book bike div stsrts here-->
    <div id="divInfo" class="inner-content bookbike-box">
        <h2 class="margin-bottom10">Book <%= String.IsNullOrEmpty(Series) ? Make : String.Format("{0} {1}",Make,Series) %> Bikes Online</h2>
        <p class="red-text font11 hide">Please select following fields</p>
            <div>                
                <select id="ddlMakes" data-bind="options: bookingMakes, value: selectedMake, optionsText: 'TEXT', optionsValue: 'Value',
                optionsCaption: 'Select Make', event: { change: makeChange }"></select>
            </div>
            <div>
                <select id="ddlModel" data-bind="options: bookingModels, value: selectedModel, optionsText: 'ModelName', optionsValue: 'ModelId',
                optionsCaption: 'Select Model', event: { change: modelChange }"></select>
            </div>
            <div>
                <select id="ddlCities" data-bind="options: bookingCities, value: selectedCity, optionsText: 'cityName', optionsValue: 'cityId',
                optionsCaption: 'Select City', event: { change: cityChange }"></select>
            </div>
            <div>
                <select id="ddlArea" data-bind="options: bookingAreas, value: selectedArea, optionsText: 'Text', optionsValue: 'Value',
                optionsCaption: 'Select Area', event: { change: areaChange }"></select>
            </div>
            <div class="center-align">                
                <input id="btnDealerPrice" class="action-btn" type="button" value="Book Now">
            </div>
        <div id="errMsg" class="red-text margin-top10 hide"></div>
    </div>
    <!--book bike div ends here-->
</div>
<script type="text/javascript">
    var makeId = '<%= MakeId %>';
    var seriesId = '<%= SeriesId %>';
    var bwHostUrl = '<%= ConfigurationManager.AppSettings["bwHostUrl"]%>';
    var isPageLoad = true;

    // knockout data binding
    var viewModel = {
        selectedMake: ko.observable(),
        bookingMakes: ko.observableArray(),
        selectedModel: ko.observable(),
        bookingModels: ko.observableArray(),
        selectedCity: ko.observable(),
        bookingCities: ko.observableArray(),
        selectedArea: ko.observable(),
        bookingAreas: ko.observableArray()        
    };
    
    function bindMakes()
    {
        // bind makes
        $.ajax({
            type: 'POST',
            url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
            data: '{"requestType":"PRICEQUOTE"}',
            dataType: 'json',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetBikeMakes"); },
            success: function (json) {                
                var jsonObj = $.parseJSON(json.value);
                viewModel.bookingMakes(jsonObj.Table);
                if (makeId != "") {
                    viewModel.selectedMake(makeId);
                    makeChange();
                }
            }
        });
    }
    
    function makeChange() {
        if (seriesId == "")
            dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'Make Page widget', 'act': 'Make changed' });
        else
            dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'Series Page widget', 'act': 'Make changed' });
        if (viewModel.selectedMake() != undefined) {
            if (seriesId != "" && isPageLoad) {                
                isPageLoad = false;
                // bind models of series
                $.ajax({
                    type: 'POST',
                    url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
                    data: '{"seriesId":"' + seriesId + '"}',
                    dataType: 'json',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetBikeModelsBySeriesId"); },
                    success: function (json) {                        
                        var objJson = JSON.parse(json.value);
                        viewModel.bookingModels(objJson);                        
                    }
                });
            }
            else {
                
                // bind models                
                $.ajax({
                    type: 'POST',
                    url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
                    data: '{"requestType":"PriceQuote", "makeId":"' + viewModel.selectedMake() + '"}',
                    dataType: 'json',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModelsNew"); },
                    success: function (json) {
                        var objJson = JSON.parse(json.value);
                        viewModel.bookingModels(objJson);                        
                    }
                });
            }
        } else {
            viewModel.bookingModels([]);
        }
    }

    function modelChange()
    {
        if (seriesId == "")
            dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'Make Page widget', 'act': 'Model changed' });
        else
            dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'Series Page widget', 'act': 'Model changed' });
        if (viewModel.selectedModel() != undefined) {
            $.ajax({
                type: "GET",
                url: bwHostUrl + "/api/DealerPriceQuote/getBikeBookingCities/?modelId=" + viewModel.selectedModel(),
                dataType: 'json',                
                success: function (response) {                                                            
                    viewModel.bookingCities(response);                    
                }
            });
        } else {
            viewModel.bookingCities([]);
        }
    }

    function cityChange()
    {
        if (seriesId == "")
            dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'Make Page widget', 'act': 'City changed' });
        else
            dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'Series Page widget', 'act': 'City changed' });
        if (viewModel.selectedCity() != undefined) {
            $.ajax({
                type: "POST",
                url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
                data: '{"cityId":"' + viewModel.selectedCity() + '"}',
                dataType: 'json',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetAreas"); },
                success: function (response) {                    
                    var jsonObj = $.parseJSON(response.value);
                    viewModel.bookingAreas(jsonObj.Table);
                }
            });
        } else {
            viewModel.bookingAreas([]);
        }
    }

    function areaChange() {
        if (seriesId == "")
            dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'Make Page widget', 'act': 'Area changed' });
        else
            dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'Series Page widget', 'act': 'Area changed' });
    }

    $("#btnDealerPrice").click(function () {
        $("#errMsg").text("");
        var category = seriesId ? 'Series Page widget' : 'Make Page widget';
        if (isValidInfo()) {
            $.ajax({
                type: 'POST',
                url: "/ajaxpro/Bikewale.Ajax.AjaxBikeBooking,Bikewale.ashx",
                data: '{"cityId":"' + viewModel.selectedCity() + '", "areaId":"' + viewModel.selectedArea() + '", "modelId":"' + viewModel.selectedModel() + '", "isMobileSource":false}',
                dataType: 'json',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "ProcessPQ"); },
                success: function (json) {
                    var jsonObj = $.parseJSON(json.value);                    
                    if (jsonObj.quoteId > 0 && jsonObj.dealerId > 0) {
                        dataLayer.push({ 'event': 'product_bw_gtm', 'cat': category, 'act': 'Successful submision - Dealer PQ', 'lab': 'Make :' + viewModel.selectedMake() + ', Model :' + viewModel.selectedModel() + ", City :" + viewModel.selectedCity() + ", Area : " + viewModel.selectedArea() + "'" });
                        window.location = "/pricequote/dealerpricequote.aspx";
                    }
                    else if (jsonObj.quoteId > 0) {
                        dataLayer.push({ 'event': 'product_bw_gtm', 'cat': category, 'act': 'Successful submision - BikeWale PQ', 'lab': 'Make :' + viewModel.selectedMake() + ', Model :' + viewModel.selectedModel() + ", City :" + viewModel.selectedCity() + ", Area : " + viewModel.selectedArea() + "'" });
                        window.location = "/pricequote/quotation.aspx";
                    } else {                        
                        $("#errMsg").text("Oops. We do not seem to have pricing for given details.").show();
                        dataLayer.push({ 'event': 'product_bw_gtm', 'cat': category, 'act': 'Error in submission' });
                    }

                },
                error: function (e) {
                    $("#errMsg").text("Oops. Some error occured. Please try again.").show();
                    dataLayer.push({ 'event': 'product_bw_gtm', 'cat': category, 'act': 'Error in submission' });
                }
            });
        } else {
            $("#errMsg").text("Please select all the details").show();
        }
    });

    function isValidInfo() {
        isValid = true;
        var errMsg = "Missing fields:";
        var category = seriesId ? 'Series Page widget' : 'Make Page widget';
        if (viewModel.selectedCity() == undefined) {
            errMsg += "City,";
            isValid = false;
        }        
        if (viewModel.selectedArea() == undefined) {
            errMsg += "Area,";
            isValid = false;
        }
        if (viewModel.selectedMake() == undefined) {
            errMsg += "Make,";
            isValid == false;
        }
        if (viewModel.selectedModel() == undefined) {
            errMsg += "Model,"
            isValid = false;
        }
        if (!isValid) {
            errMsg = errMsg.substring(0, errMsg.length - 1);
            dataLayer.push({ 'event': 'product_bw_gtm', 'cat': category, 'act': 'Error in submission', 'lab': errMsg });
        }
        return isValid;
    }


    $(document).ready(function () {
        bindMakes();
        ko.applyBindings(viewModel, document.getElementById("divInfo")); 
    });
</script>