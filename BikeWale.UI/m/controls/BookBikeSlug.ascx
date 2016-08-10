<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.BookBikeSlug" %>
<!-- book bike msg popup starts here -->
<div class="bw-popup bookbikeMsg hide">
    <div class="popup-inner-container">
        <div class="bwmsprite close-btn floatright"></div>
        <h1>Dealer not available</h1>
        <p class="new-line10 msg1">Oops, currently we don't have a partner dealer in your locality. We are adding dealers everyday. You can check an indicative on-road price below.
            <a href="/m/pricequote/dealerpricequote.aspx" class="bwm-btn ui-link">Get Estimated Price</a>
        </p>        
        <p class="new-line10 msg2 hide">Oops, We are unable to process your request right now. Please try again later.</p>
    </div>
</div>
<!-- book bike msg popup ends here -->
<!-- slug starts here -->
<div class="bookbike-slug">
    <div class="slug-head">
        <div class="slug-dd-box">
            <div class="bw-sprite left-curve-icon"></div>
            <div class="slug-dd">
                <p class="floatleft">Book Your Bike</p>
                <div class="floatright rounded-border">
                <span class="slug-down-arrow hide"></span>
                <span class="slug-up-arrow"></span>
                </div>
                <div class="clear"></div>
            </div>
            <div class="bw-sprite right-curve-icon"></div>
            <div class="clear"></div>
        </div>
        <div class="slug-text">Now Book Your Bike on BikeWale !</div>
        <div class="slug-text hide">Provide following details</div>
    </div>
    <div id="divInfo" class="vehicle-details-form hide">
        <p class="red-text font11 hide">Please fill all the details</p>
        <div>
            <select id="ddlCitiesSlug" data-bind="options: bookingCities, value: selectedCity, optionsText: 'cityName', optionsValue: 'cityId',
            optionsCaption: 'Select City', event: { change: slugCityChanged }"></select>
        </div>
        <div>
            <select id="ddlAreaSlug" data-bind="options: bookingAreas, value: selectedArea, optionsText: 'Text', optionsValue: 'Value',
            optionsCaption: 'Select Area', event: { change: slugAreaChanged }"></select>
        </div>
        <div>
            <select id="ddlMakeSlug" data-bind="options: bookingMakes, value: selectedMake, optionsText: 'MakeName', optionsValue: 'MakeId',
    optionsCaption: 'Select Make', event: { change: slugMakeChanged }"></select>
        </div>
        <div>
            <select id="ddlModelSlug" data-bind="options: bookingModels, value: selectedModel, optionsText: 'Text', optionsValue: 'Value',
    optionsCaption: 'Select Model', event: { change: slugModelChanged }"></select>
        </div>
        <div class="margin-top-10">                    
            <a id="btnDealerPriceSlug" href="#" class="bwm-btn">Check Final Dealer Price</a>
        </div>
    </div>
</div>
<script type="text/javascript">
    var bwHostUrl = '<%= ConfigurationManager.AppSettings["bwHostUrl"]%>';    
    $('.slug-head').click(function (e) {
        $('.vehicle-details-form').slideToggle();
    });
    $('.slug-head').click(function (e) {
        $(this).find('.slug-dd span').toggle();
        $('.slug-text').toggle();
    });

    $('.close-btn').click(function (e) {
        $('.bw-popup').hide();
    });

    // knockout data binding
    var viewModel = {
        selectedCity: ko.observable(),
        bookingCities: ko.observableArray(),
        selectedArea: ko.observable(),
        bookingAreas: ko.observableArray(),
        selectedMake: ko.observable(),
        bookingMakes: ko.observableArray(),
        selectedModel: ko.observable(),
        bookingModels: ko.observableArray()        
    };
    
    function bindCitiesSlug() {
        $.ajax({
            type: 'GET',
            url: bwHostUrl + "/api/DealerPriceQuote/getBikeBookingCities/",
            dataType: 'json',
            success: function (json) {                                      
                viewModel.bookingCities(json);
            }
        });
    }

    function slugAreaChanged() {
        dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'mSite slug', 'act': 'Area changed' });
    }

    function slugCityChanged()
    {
        dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'mSite slug', 'act': 'City changed' });
        if (viewModel.selectedCity() != undefined) {
            // bind areas
            $.ajax({
                type: 'POST',
                url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
                data: '{"cityId":"' + viewModel.selectedCity() + '"}',
                dataType: 'json',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetAreas"); },
                success: function (json) {
                    var jsonObj = $.parseJSON(json.value);
                    viewModel.bookingAreas(jsonObj.Table);
                }
            });

            // bind makes
            $.ajax({
                type: 'GET',
                url: bwHostUrl + "/api/DealerPriceQuote/GetBikeMakesInCity/?cityId=" + viewModel.selectedCity(),
                dataType: 'json',
                success: function (json) {
                    viewModel.bookingMakes(json);
                }
            });            
        }
        else {
            viewModel.bookingAreas([]);
            viewModel.bookingMakes([]);
        }        
    }

    function slugMakeChanged()
    {
        dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'mSite slug', 'act': 'Make changed' });
        if (viewModel.selectedMake() != undefined) {
            // bind models
            $.ajax({
                type: 'POST',
                url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
                data: '{"requestType":"PRICEQUOTE", "makeId":"' + viewModel.selectedMake() + '"}',
                dataType: 'json',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModels"); },
                success: function (json) {
                    var jsonObj = $.parseJSON(json.value);
                    viewModel.bookingModels(jsonObj.Table);                    
                }
            });
        } else {
            viewModel.bookingModels([]);
        }
    }    

    function slugModelChanged() {
        dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'mSite slug', 'act': 'Model changed' });
    }

    $("#btnDealerPriceSlug").click(function () {
        // Validate the data
        if (isValidInfo()) {
            // Process PQ
            var pathName = window.location.pathname;
            var objMsg = $('.bookbikeMsg');
            $.ajax({
                type: 'POST',
                url: "/ajaxpro/Bikewale.Ajax.AjaxBikeBooking,Bikewale.ashx",
                data: '{"cityId":"' + viewModel.selectedCity() + '", "areaId":"' + viewModel.selectedArea() + '", "modelId":"' + viewModel.selectedModel() + '", "isMobileSource":true}',
                dataType: 'json',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "ProcessPQ"); },
                success: function (json) {                    
                    var jsonObj = $.parseJSON(json.value);
                    if (jsonObj.quoteId > 0 && jsonObj.dealerId > 0) {
                        dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'mSite slug', 'act': 'Successful submision - Dealer PQ', 'lab': 'Path :' + pathName + ', Make :' + viewModel.selectedMake() + ', Model :' + viewModel.selectedModel() + ", City :" + viewModel.selectedCity() + ", Area : " + viewModel.selectedArea() + "'" });
                        window.location = "/m/pricequote/dealerpricequote.aspx";
                    }
                    else if (jsonObj.quoteId > 0) {
                        dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'mSite slug', 'act': 'Successful submision - BikeWale PQ', 'lab': 'Path :' + pathName + ', Make :' + viewModel.selectedMake() + ', Model :' + viewModel.selectedModel() + ", City :" + viewModel.selectedCity() + ", Area : " + viewModel.selectedArea() + "'" });
                        objMsg.find(".msg1").show();
                        objMsg.find(".msg2").hide();
                        objMsg.show();
                    } else {
                        dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'mSite slug', 'act': 'Error in submission' });
                        objMsg.find(".msg1").hide();
                        objMsg.find(".msg2").show();
                        objMsg.show();
                    }                    
                },
                error: function (e) {
                    dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'mSite slug', 'act': 'Error in submission' });
                    objMsg.find(".msg1").hide();
                    objMsg.find(".msg2").show();
                    objMsg.show();
                }
            });
        } else {
            // Show error messages
            alert("Please select all the details");
        }
    });

    function isValidInfo()
    {
        isValid = true;
        var errMsg = "Missing fields:";
        if (viewModel.selectedCity() == undefined)
        {
            errMsg += "City,";
            isValid = false;
        }

        if (viewModel.selectedArea() == undefined)
        {
            errMsg += "Area,";
            isValid = false;
        }

        if (viewModel.selectedMake() == undefined)
        {
            errMsg += "Make,";
            isValid == false;
        }

        if (viewModel.selectedModel() == undefined) {
            errMsg += "Model,"
            isValid = false;
        }
        if (!isValid) {
            errMsg = errMsg.substring(0, errMsg.length - 1);
            dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'mSite slug', 'act': 'Error in submission', 'lab': errMsg });
        }
        return isValid;
    }

    $(document).ready(function () {
        bindCitiesSlug();
        ko.applyBindings(viewModel, document.getElementById("divInfo"));
    });

</script>
<!-- slug ends here -->
