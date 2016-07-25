<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.BikeBookingMinWidget" %>
<div class="margin-top10 grey-bg hide" id="dvBikeBookingMinWidget" >
    <div class="content-block bookbike-hrz-box">
        <h2 class="margin-bottom10">Book <%= Model %> Online</h2>
        <p class="red-text font11 hide">Please select following fields</p>
        <ul class="ul-hrz-left">
            <li>
                <select id="ddlCities" data-bind="options: bookingCities, value: selectedCity, optionsText: 'cityName', optionsValue: 'cityId',
                optionsCaption: 'Select City', event: { change: cityChange }"></select>    
            </li>
            <li>
                <select id="ddlArea" data-bind="options: bookingAreas, value: selectedArea, optionsText: 'Text', optionsValue: 'Value',
                optionsCaption: 'Select Area', event: { change: minWidgetAreaChange }"></select>
            </li>
            <li>
                <input id="btnDealerPrice" class="action-btn" type="button" value="Book Now">
            </li>            
        </ul>
        <div class="clear"></div>
        <div id="errMsg" class="red-text margin-top10 hide"></div>
    </div>
</div>
<script type="text/javascript">
    var modelId = '<%= ModelId %>';
    var bwHostUrl = '<%= ConfigurationManager.AppSettings["bwHostUrl"]%>';

    var viewModel = {        
        selectedCity: ko.observable(),
        bookingCities: ko.observableArray(),
        selectedArea: ko.observable(),
        bookingAreas: ko.observableArray()
    };

    function minWidgetAreaChange() {        
        dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'Model Page widget', 'act': 'Area changed' });
    }

    function bindCities() {
        $.ajax({
            type: "GET",
            url: bwHostUrl + "/api/DealerPriceQuote/getBikeBookingCities/?modelId=" + modelId,
            dataType: 'json',
            success: function (response) {                
                viewModel.bookingCities(response);
                if (response == undefined || response.length == 0) {
                    $("#dvBikeBookingMinWidget").addClass("hide");
                }
            },
            error: function (request, status, error) {
                $("#dvBikeBookingMinWidget").addClass("hide");
            }
        });        
    }

    function cityChange() {
        dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'Model Page widget', 'act': 'City changed' });
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

    $("#btnDealerPrice").click(function () {
        $("#errMsg").text("");        
        // Validate the data
        if (isValidInfo()) {
            // Process PQ
            //var objMsg = $('.bookbikeMsg');            
            $.ajax({
                type: 'POST',
                url: "/ajaxpro/Bikewale.Ajax.AjaxBikeBooking,Bikewale.ashx",
                data: '{"cityId":"' + viewModel.selectedCity() + '", "areaId":"' + viewModel.selectedArea() + '", "modelId":"' + modelId + '", "isMobileSource":false}',
                dataType: 'json',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "ProcessPQ"); },
                success: function (json) {                    
                    var jsonObj = $.parseJSON(json.value);
                    if (jsonObj.quoteId > 0 && jsonObj.dealerId > 0) {
                        dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'Model Page widget', 'act': 'Successful submision - Dealer PQ', 'lab': "'City :" + viewModel.selectedCity() + ", Area : " + viewModel.selectedArea() + "'" });
                        window.location = "/pricequote/dealerpricequote.aspx";
                    }
                    else if (jsonObj.quoteId > 0) {
                        dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'Model Page widget', 'act': 'Successful submision - BikeWale PQ', 'lab': "'City :" + viewModel.selectedCity() + ", Area : " + viewModel.selectedArea() + "'" });
                        window.location = "/pricequote/quotation.aspx";
                    } else {
                        dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'Model Page widget', 'act': 'Error in submission' });
                        $("#errMsg").text("Oops. We do not seem to have pricing for given details.").show();
                    }

                },
                error: function (e) {
                    dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'Model Page widget', 'act': 'Error in submission' });
                    $("#errMsg").text("Oops. Some error occured.").show();
                }
            });
        } else {
            // Show error messages
            $("#errMsg").text("Please select all the details").show();
        }
    });

    function isValidInfo() {
        isValid = true;
        var errMsg = "Missing fields:";
        if (viewModel.selectedCity() == undefined) {
            errMsg += "City,";
            isValid = false;
        }

        if (viewModel.selectedArea() == undefined) {
            errMsg += "Area,";
            isValid = false;
        }
        
        if (modelId == "") {
            errMsg += "Model,"
            isValid = false;
        }
        if (!isValid) {
            errMsg = errMsg.substring(0, errMsg.length - 1);
            dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'Model Page widget', 'act': 'Error in submission', 'lab': errMsg });
        }
        return isValid;
    }

    $(function () {
        bindCities();
        ko.applyBindings(viewModel, $("#dvBikeBookingMinWidget")[0]);
    });
    
</script>