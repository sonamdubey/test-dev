<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.bikebooking.Default" %>

<%
    title = "Book Your Bike";
    description = "Know On-Road Price of any new bike in India. On-road price of a bike includes ex-showroom price of the bike in your city, insurance charges. road-tax, registration charges, handling charges etc. Finance option is also provided so that you can get a fair idea of EMI and down-payment.";
    keywords = "bike price, new bike price, bike prices, bike prices India, new bike price quote, on-road price, on-road prices, on-road prices India, on-road price India";
    //canonical = "http://www.bikewale.com/pricequote/";
    AdPath = "/1017752/Bikewale_Mobile_OnRoadPrice";
    AdId = "1398839030772";
    menu = "13";
%>
<!-- #include file="/includes/headermobile.aspx" -->

    <form id="form1" runat="server">

    <div class="padding5">
        <div id="br-cr"><a href="/m/new/" class="normal">New Bikes</a> &rsaquo; <span class="lightgray">Book Your Bike</span></div>
        <h1> Book Your Bike </h1>
        <div id="divBookingDetails" class="box1 new-line5">
            <div>
            <select id="ddlCities" data-bind="options: bookingCities, value: selectedCity, optionsText: 'cityName', optionsValue: 'cityId',
    optionsCaption: 'Select City', event: { change: cityChangedBooking }"></select>
        </div>
        <div>
            <select id="ddlArea" data-bind="options: bookingAreas, value: selectedArea, optionsText: 'Text', optionsValue: 'Value',
    optionsCaption: 'Select Area', event: { change: areaChangedBooking }"></select>
        </div>
        <div>
            <select id="ddlMake" data-bind="options: bookingMakes, value: selectedMake, optionsText: 'MakeName', optionsValue: 'MakeId',
    optionsCaption: 'Select Make', event: { change: makeChangedBooking }"></select>
        </div>
        <div>
            <select id="ddlModel" data-bind="options: bookingModels, value: selectedModel, optionsText: 'Text', optionsValue: 'Value',
    optionsCaption: 'Select Model', event: { change: modelChangedBooking }"></select>
        </div>
        <div class="new-line15">
		    <div><input type="checkbox" style="margin-top:3px;" id="userAgreement" checked="checked" /></div>
		    <div style="margin-left:35px; !important;"> 
			    I agree with BikeWale <a href="/visitoragreement.aspx" target="_blank">Visitor Agreement</a> and <a href="/privacypolicy.aspx" target="_blank">Privacy Policy</a>.
		    </div>
	    </div>
        <div>
            <p class="lightgray f-12 new-line10">
                We respect your privacy and will never publicly display, share or use your contact details without your authorization.  By providing your contact details to us you agree that 
				        we (and/or any of our partners including dealers, bike manufacturers,banks like ICICI bank etc) may call you on the phone number mentioned, in order to provide information or assist you in any transactions, 
				        and that we may share your details with these partners.
            </p>
        </div>
        <div class="margin-top-10">                    
            <a id="btnDealerPrice" href="#" class="bwm-btn ui-link">Show Final Price and Book</a>
        </div>
        </div>
    </div>
    <div data-role="popup" id="popupDialog" data-overlay-theme="a" data-theme="c" data-dismissible="false"  class="ui-corner-all">
        <div data-role="header" data-theme="a" class="ui-corner-top" style="background-color:#000">
            <h2 style="color:#fff;">Error !!</h2>
        </div>
        <div data-role="content" data-theme="d" class="ui-corner-bottom ui-content" style="background-color:#fff;">
            <span id="spnError" style="font-size:14px;line-height:20px;" class="error"></span>
            <a href="#" data-role="button" data-rel="back" data-theme="c" data-mini="true">OK</a>
        </div>
    </div>        
    </form>
<script type="text/javascript">

    // knockout data binding
    var viewModelBikeBooking = {
        selectedCity: ko.observable(),
        bookingCities: ko.observableArray(),
        selectedArea: ko.observable(),
        bookingAreas: ko.observableArray(),
        selectedMake: ko.observable(),
        bookingMakes: ko.observableArray(),
        selectedModel: ko.observable(),
        bookingModels: ko.observableArray()
    };

    

    //bind cities
    function bindBookingCities() {
        $.ajax({
            type: 'GET',
            url: abHostUrl + "/api/DealerPriceQuote/getBikeBookingCities/",
            dataType: 'json',
            success: function (json) {
                //for insite bikebooking
                viewModelBikeBooking.bookingCities(json);
            }
        });
    }

    function areaChangedBooking() {
        dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'mSite_Bkg_Menu', 'act': 'mSiteBkg_Area selected' });
    }

    function modelChangedBooking()
    {
        dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'mSite_Bkg_Menu', 'act': 'mSiteBkg_Model selected' });
    }

    function cityChangedBooking() {
        dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'mSite_Bkg_Menu', 'act': 'mSiteBkg_City selected' });
        if (viewModelBikeBooking.selectedCity() != undefined) {
            // bind areas
            $.ajax({
                type: 'POST',
                url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
                data: '{"cityId":"' + viewModelBikeBooking.selectedCity() + '"}',
                dataType: 'json',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetAreas"); },
                success: function (json) {
                    var jsonObj = $.parseJSON(json.value);
                    viewModelBikeBooking.bookingAreas(jsonObj.Table);
                }
            });

            // bind makes
            $.ajax({
                type: 'GET',
                url: abHostUrl + "/api/DealerPriceQuote/GetBikeMakesInCity/?cityId=" + viewModelBikeBooking.selectedCity(),
                dataType: 'json',
                success: function (json) {
                    viewModelBikeBooking.bookingMakes(json);
                }
            });
        }
        else {
            viewModelBikeBooking.bookingAreas([]);
            viewModelBikeBooking.bookingMakes([]);
        }
    }

    function makeChangedBooking() {
        dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'mSite_Bkg_Menu', 'act': 'mSiteBkg_Make selected' });
        if (viewModelBikeBooking.selectedMake() != undefined) {
            // bind models
            $.ajax({
                type: 'POST',
                url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
                data: '{"requestType":"PRICEQUOTE", "makeId":"' + viewModelBikeBooking.selectedMake() + '"}',
                dataType: 'json',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModels"); },
                success: function (json) {
                    var jsonObj = $.parseJSON(json.value);
                    viewModelBikeBooking.bookingModels(jsonObj.Table);
                }
            });
        } else {
            viewModelBikeBooking.bookingModels([]);
        }
    }

    $("#btnDealerPrice").click(function () {        
        // Validate the data
        if (isValidBookingInfo()) {
            // Process PQ
            var pathName = window.location.pathname;
            var objMsg = $('.bookbikeMsg');
            $.ajax({
                type: 'POST',
                url: "/ajaxpro/Bikewale.Ajax.AjaxBikeBooking,Bikewale.ashx",
                data: '{"cityId":"' + viewModelBikeBooking.selectedCity() + '", "areaId":"' + viewModelBikeBooking.selectedArea() + '", "modelId":"' + viewModelBikeBooking.selectedModel() + '", "isMobileSource":true}',
                dataType: 'json',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "ProcessPQ"); },
                success: function (json) {
                    var jsonObj = $.parseJSON(json.value);
                    if (jsonObj.quoteId > 0 && jsonObj.dealerId > 0) {
                        dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'mSite_Bkg_Menu', 'act': 'mSiteBkg_Successful submission - Dealer PQ', 'lab': 'Path :' + pathName + ', Make :' + viewModelBikeBooking.selectedMake() + ', Model :' + viewModelBikeBooking.selectedModel() + ", City :" + viewModelBikeBooking.selectedCity() + ", Area : " + viewModelBikeBooking.selectedArea() + "'" });
                        window.location = "/m/pricequote/dealerpricequote.aspx";
                    }
                    else if (jsonObj.quoteId > 0) {
                        dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'mSite_Bkg_Menu', 'act': 'mSiteBkg_Successful submission - BikeWale PQ', 'lab': 'Path :' + pathName + ', Make :' + viewModelBikeBooking.selectedMake() + ', Model :' + viewModelBikeBooking.selectedModel() + ", City :" + viewModelBikeBooking.selectedCity() + ", Area : " + viewModelBikeBooking.selectedArea() + "'" });
                        objMsg.find(".msg1").show();
                        objMsg.find(".msg2").hide();
                        objMsg.show();
                       
                    } else {
                        dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'mSite_Bkg_Menu', 'act': 'mSiteBkg_Error in submission' });
                        objMsg.find(".msg1").hide();
                        objMsg.find(".msg2").show();
                        objMsg.show();
                    }
                },
                error: function (e) {
                    dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'mSite_Bkg_Menu', 'act': 'mSiteBkg_Error in submission' });
                    objMsg.find(".msg1").hide();
                    objMsg.find(".msg2").show();
                    objMsg.show();
                    alert(viewModelBikeBooking.selectedCity() + " " + viewModelBikeBooking.selectedMake() + " " + viewModelBikeBooking.selectedModel() + " " + viewModelBikeBooking.selectedArea());
         
                }
            });
        } else {
            // Show error messages
            //alert("Please select all the details");
            return false;
        }
    });

    function isValidBookingInfo() {
        isValid = true;
        var errMsg = "Missing fields:";        
        if (viewModelBikeBooking.selectedCity() == undefined) {
            errMsg += "City,";
            isValid = false;
        }

        if (viewModelBikeBooking.selectedArea() == undefined) {
            errMsg += "Area,";
            isValid = false;
        }

        if (viewModelBikeBooking.selectedMake() == undefined) {
            errMsg += "Make,";
            isValid == false;
        }

        if (viewModelBikeBooking.selectedModel() == undefined) {
            errMsg += "Model,"
            isValid = false;
        }

        if (!$("input#userAgreement").is(":checked")) {
            retVal = false;
            errMsg += "<br>Visitor Agreement ";            
        }

        if (!isValid) {
            errMsg = errMsg.substring(0, errMsg.length - 1);
            dataLayer.push({ 'event': 'product_bw_gtm', 'cat': 'mSite_Bkg_Menu', 'act': 'mSiteBkg_Error in submission', 'lab': errMsg });

            $("#spnError").html(errMsg);
            $("#popupDialog").popup("open");
        }
        return isValid;
    }    

    $(function () {      
        //apply ko bindings for the  data Binds
        bindBookingCities();
        ko.applyBindings(viewModelBikeBooking, document.getElementById("divBookingDetails"));
    });
    
</script>

<!-- #include file="/includes/footermobile.aspx" -->
