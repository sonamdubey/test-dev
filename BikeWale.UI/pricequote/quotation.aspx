<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.PriceQuote.Quotation" Trace="false" Debug="false" %>
<%@ Register TagPrefix="uc" TagName="UserReviewsMin" Src="~/controls/UserReviewsMin.ascx" %>
<%@ Register TagPrefix="LD" TagName="LocateDealer" Src="/controls/locatedealer.ascx" %>
<%@ Register Src="~/controls/AlternativeBikes.ascx" TagName="AlternativeBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/UpcomingBikes_new.ascx" TagName="UpcomingBikes" TagPrefix="BW" %>
<%@ Import Namespace="Bikewale.Common" %>
<!doctype html>
<html>
<head>
<%
    title = "Instant Free New Bike Price Quote";
    description = "Bikewale.com New bike free price quote.";
    ShowTargeting = "1";
    TargetedModel = mmv.Model;
    AdId = "1395986297721";
    AdPath = "/1017752/Bikewale_PQ_";
%>
<!-- #include file="/includes/headscript.aspx" -->

<style type="text/css">
    #PQImageVariantContainer img{width:100%}
    .PQDetailsTableTitle{color:#82888b}
    .PQDetailsTableAmount,.PQOnRoadPrice{color:#4d5057}
    .text-dark-black { color:#1a1a1a; }
    .PQOffersUL{margin-left:18px;list-style:disc}
    .PQOffersUL li{padding-bottom:15px}
    .margin-top7 { margin-top:7px; }
    .pqVariants .variants-dropdown{ width:210px; height: 40px; margin-left: 12px; color: #555; position: relative; cursor: pointer; background: #fff;}
    .variant-selection-tab { width:90%; display: block; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; text-align: left; }
    #upDownArrow.fa-angle-down { transition: all 0.5s ease-in-out 0s; font-size: 20px;}
    .variants-dropdown .fa-angle-down { transition: transform .3s; -moz-transition: transform .3s; -webkit-transition: transform .3s; -o-transition: transform .3s; -ms-transition: transform .3s; }
    .variants-dropdown.open .fa-angle-down { -moz-transform: rotateZ(180deg); -webkit-transform: rotateZ(180deg); -o-transform: rotateZ(180deg); -ms-transform: rotateZ(180deg); transform: rotateZ(180deg); }
    .variants-dropdown-list { display:none; width:210px; overflow:hidden; background: #fff; border: 1px solid #ccc; position: absolute; top:40px; left:82px; z-index: 2;}
    .variants-dropdown-list li { margin-bottom:5px; margin-top:5px; font-size:14px; }
    .variants-dropdown-list li.selected { font-weight:bold; }
    .variants-dropdown-list li input{ width:100%; text-align:left;overflow: hidden; text-overflow: ellipsis; white-space: nowrap;background: #fff;color:rgb(77, 80, 87); padding:2px 0 2px 8px;}
    .variants-dropdown-list li:hover input{ padding:2px 0 2px 8px; cursor:pointer; background:#82888b; color:#fff; }
    .jcarousel-wrapper.alternatives-carousel{width:974px}
    .alternatives-carousel .jcarousel li{height:auto;margin-right:18px}
    .alternatives-carousel .jcarousel li.front{border:none}
    .alternative-section .jcarousel-control-left{left:-24px}
    .alternative-section .jcarousel-control-right{right:-24px}
    .alternative-section .jcarousel-control-left,.alternative-section .jcarousel-control-right{top:50%}
    .newBikes-latest-updates-container .grid-4{padding-left:10px}
    .available-colors{display:inline-block;width:150px;text-align:center;margin-bottom:20px;padding:0 5px;vertical-align:top}
    .available-colors .color-box{width:60px;height:60px;margin:0 auto 15px;border-radius:3px;background:#f00;border:1px solid #ccc}
    .upcoming-brand-bikes-container li.front{border:none}
    .upcoming-brand-bikes-container li .imageWrapper{width:303px;height:174px}
    .upcoming-brand-bikes-container li .imageWrapper a{width:303px;height:174px;display:block;background:url('http://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif') no-repeat center center}
    .upcoming-brand-bikes-container li{width:303px;height:auto;margin-right:12px}
    .upcoming-brand-bikes-container li .imageWrapper a img{width:303px;height:174px}
    .upcoming-brand-bikes-container .jcarousel{width:934px;overflow:hidden;left:20px}
    .modelGetDetails h3 { border-bottom:1px solid #ecedee; }
    .modelGetDetails ul { list-style:disc; color:#82888b; margin-left:25px; font-size:14px; }
    .modelGetDetails ul li { padding-top: 12px;padding-right: 12px; width:100% !important; float: left; }
    #leadCapturePopup { display:none; width:450px; min-height:470px; background:#fff; margin:0 auto; position:fixed; top:10%; right:5%; left:5%; z-index:10; padding: 30px 40px; }
    .personal-info-form-container { margin: 10px auto; width: 300px; min-height: 100px; }
    .personal-info-form-container .personal-info-list { margin:0 auto; width:280px; float:left; margin-bottom:20px; border-radius:0; }
    .personal-info-list .errorIcon, .personal-info-list .errorText { display:none; }
    .user-contact-details-icon { width:36px; height:44px; background-position: 0 -391px; }
    .mobile-prefix { position: absolute; padding: 10px 13px 13px; color: #999; }
     #leadCapturePopup .error-icon, #leadCapturePopup .bw-blackbg-tooltip {display:none}
</style>

<script type="text/javascript">

    var pqId = '<%= objQuotation.PriceQuoteId%>';
    var versionId = '<%= objQuotation.VersionId%>';
    var cityId = '<%= cityId%>';      
    var bikeVersionLocation = '';
    var campaignId = "<%= objQuotation.CampaignId%>";
    var manufacturerId = "<%= objQuotation.ManufacturerId%>";
    var versionName = "<%= objQuotation.VersionName%>";
    var myBikeName = "<%=mmv.BikeName%>";

</script>
</head>
<body class="bg-light-grey header-fixed-inner">
<form runat="server">
    <!-- #include file="/includes/headBW.aspx" -->	
    <section class="bg-light-grey padding-top10">
        <div class="container">
            <div class="grid-12">
                <div class="breadcrumb margin-bottom10"><!-- breadcrumb code starts here -->
                    <ul>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <a href="/" itemprop="url">
                                <span itemprop="title">Home</span>
                            </a>
                        </li>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <span class="fa fa-angle-right margin-right10"></span>
                            <a href="/new/" itemprop="url">
                                <span itemprop="title">New</span>
                            </a>
                        </li>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <span class="fa fa-angle-right margin-right10"></span>
                            <a href="/pricequote/" itemprop="url">
                                <span itemprop="title">On-road price</span>
                            </a>
                        </li>
                        <li>
                            <span class="fa fa-angle-right margin-right10"></span><%= mmv.BikeName %>
                        </li>
                    </ul>
                    <div class="clear"></div>
                </div>
                <h1 class="font26 margin-bottom10">On-road price for <%= mmv.BikeName %> in <%= objQuotation.City %></h1>
                <div class="clear"></div>
            </div>
            <div class="clear"></div>
        </div>
    </section>
    
    <section class="container">
        <div class="grid-12 margin-bottom20" id="dealerPriceQuoteContainer">
            <div class="content-box-shadow padding-top20 padding-bottom20 rounded-corner2">
                <div class="grid-4 padding-right10 padding-left20" id="PQImageVariantContainer">
        	        <div class="pqBikeImage margin-bottom15">
                        <img alt=" <%= mmv.BikeName %> Photos" src="<%=imgPath%>" title="<%= mmv.BikeName %> Photos">
                    </div>
                    <div class="pqVariants <%=(versionList.Count > 1)?"":"hide" %>">
                        <p class="margin-left10 font16 text-light-grey leftfloat margin-top7">Version:</p>
                        <div class="form-control-box position-rel variants-dropdown rounded-corner2 leftfloat">
                            <asp:DropDownList id="ddlVersion" class="form-control" runat="server" AutoPostBack="true"></asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="grid-4 padding-right15 padding-left10" id="PQDetailsContainer">
                    <%if (objQuotation != null && objQuotation.ExShowroomPrice > 0)
                      { %>
                    <%--<p class="font16 margin-bottom15">On-road price in 
                        <span><%= (String.IsNullOrEmpty(objQuotation.Area))?objQuotation.City:(objQuotation.Area + ", " + objQuotation.City) %></span>
                    </p>--%>
                    <% } %>
                    <div>
                        <%if (objQuotation != null && objQuotation.ExShowroomPrice > 0)
                          {%>
                            <table class="font14" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td width="200" class="PQDetailsTableTitle padding-bottom15">
                                        Ex-Showroom (<%= objQuotation.City %>)
                                    </td>
                                    <td align="right" class="PQDetailsTableAmount padding-bottom15">
                                        <span class="fa fa-rupee margin-right5"></span><span id="exShowroomPrice"><%= CommonOpn.FormatNumeric( objQuotation.ExShowroomPrice.ToString() ) %></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="PQDetailsTableTitle padding-bottom15">RTO</td>
                                    <td align="right" class="PQDetailsTableAmount padding-bottom15">
                                        <span class="fa fa-rupee margin-right5"></span><span><%= CommonOpn.FormatNumeric( objQuotation.RTO.ToString() ) %></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="PQDetailsTableTitle padding-bottom15">Insurance (Comprehensive)<br />
                                        <div style="position: relative; color: #999; font-size: 11px; margin-top: 1px;">Save up to 60% on insurance - <a onclick="dataLayer.push({ event: 'Bikewale_all', cat: 'BW_PQ', act: 'Insurance_Clicked',lab: '<%= (objQuotation!=null)?(objQuotation.MakeName + "_" + objQuotation.ModelName + "_" + objQuotation.VersionName + "_" + objQuotation.City):string.Empty %>' });" target="_blank" href="/insurance/">PolicyBoss</a>
                                            <span style="margin-left: 8px; vertical-align: super; font-size: 9px;">Ad</span>  
                                        </div>
                                    </td>
                                    <td align="right" class="PQDetailsTableAmount padding-bottom15">
                                        <span class="fa fa-rupee margin-right5"></span><span><%= CommonOpn.FormatNumeric(  objQuotation.Insurance.ToString()  ) %></span>
                                    </td>
                                </tr>
                                <tr><td colspan="2" class="border-solid-top padding-bottom15" align="right"></tr>
                                <tr>
                                    <td class="PQDetailsTableTitle PQOnRoadPrice padding-bottom15 text-dark-black">On-road price</td>
                                    <td align="right" class="PQDetailsTableAmount font18 padding-bottom15 text-dark-black">
                                        <span class="fa fa-rupee margin-right5"></span><span><%= CommonOpn.FormatNumeric( objQuotation.OnRoadPrice.ToString()  ) %></span>
                                    </td>
                                </tr>	
                            </table>
                        <%}
                          else
                          { %>
                        <table class="font14" cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tr>
                                <td colspan="2" style="vertical-align:central">
                                    <div id="div_ShowErrorMsg"  class="grey-bg border-light content-block text-highlight margin-top15" style="background:#fef5e6;">Price for this bike is not available in this city.</div>   
                                </td>
                            </tr>
                        </table>
                            <%} %>
                    </div>
                </div>
                <div class="grid-4 padding-right20 padding-left15">
                    <%if (objQuotation.CampaignId == 0){ %>
                    <LD:LocateDealer ID="ucLocateDealer" runat="server" />
                    <%}
                      else if ((objQuotation.CampaignId > 0)){ %>
                    <div class="modelGetDetails padding-right20">
						<h3 class="padding-bottom10">                                                   
                                Get following details from <%=objQuotation.MakeName %>:                     
						</h3>
						<ul>
							<li>Offers from the nearest dealers</li>
							<li>Waiting period on this bike at the dealership</li>
							<li>Nearest dealership from your place</li>
							<li>Finance options on this bike</li>
						</ul>
					</div>
                    <div class="grid-3 leftfloat noOffers margin-top20">
                        <input type="button" value="Get more details" class="btn btn-orange margin-right20 leftfloat" id="getMoreDetailsBtnCampaign">
					</div>
                    <div class="blackOut-window"></div>
                    <%} %>

                </div>
                <div class="clear"></div>
            </div>
        </div>
        <div class="clear"></div>
        <!-- lead capture popup start-->    
		<div id="leadCapturePopup" class="text-center rounded-corner2">
			<div class="leadCapture-close-btn position-abt pos-top10 pos-right10 bwsprite cross-lg-lgt-grey cur-pointer"></div>
			<!-- contact details starts here -->
			<div id="contactDetailsPopup">
				<div class="icon-outer-container rounded-corner50">
					<div class="icon-inner-container rounded-corner50">
						<span class="bwsprite user-contact-details-icon margin-top25"></span>
					</div>
				</div>
				<p class="font20 margin-top25 margin-bottom10">Get more details on this bike</p>
				<p class="text-light-grey margin-bottom20">Please provide contact info to see more details</p>
				<div class="personal-info-form-container">
					<div class="form-control-box personal-info-list">
						<input type="text" class="form-control get-first-name" placeholder="Full name (mandatory)"
							id="getFullName" data-bind="value: fullName">
						<span class="bwsprite error-icon errorIcon"></span>
						<div class="bw-blackbg-tooltip errorText">Please enter your first name</div>
					</div>
					<div class="form-control-box personal-info-list">
						<input type="text" class="form-control get-email-id" placeholder="Email address (mandatory)"
							id="getEmailID" data-bind="value: emailId">
						<span class="bwsprite error-icon errorIcon"></span>
						<div class="bw-blackbg-tooltip errorText">Please enter email address</div>
					</div>
					<div class="form-control-box personal-info-list">
						<p class="mobile-prefix">+91</p>
						<input type="text" class="form-control padding-left40 get-mobile-no" placeholder="Mobile no. (mandatory)"
							id="getMobile" maxlength="10" data-bind="value: mobileNo">
						<span class="bwsprite error-icon errorIcon"></span>
						<div class="bw-blackbg-tooltip errorText">Please enter mobile number</div>
					</div>
					<div class="clear"></div>
					<a class="btn btn-orange margin-top10" id="user-details-submit-btn" data-bind="event: { click: submitCampaignLead }">Submit</a>                         
				</div>                   				
			</div>
			<!-- contact details ends here -->
            <!-- thank you message starts here -->
            <div id="notify-response" class="hide margin-top10 content-inner-block-20 text-center">
                    <div class="icon-outer-container rounded-corner50">
					    <div class="icon-inner-container rounded-corner50">
						    <span class="bwsprite user-contact-details-icon margin-top25"></span>
					    </div>
				    </div>
                    <p class="font18 text-bold margin-bottom20">Thank you <span class="notify-leadUser"></span></p>
                    <p class="font16 margin-bottom40"><%=objQuotation.MakeName%> Company would get back to you shortly with additional information.</p>
                    <input type="button" id="notifyOkayBtn" class="btn btn-orange" value="Okay" />
            </div>
			<!-- thank you message ends here -->			
		</div>
			<!-- lead capture popup End-->
        <div class="clear"></div>
    </section>

    <section class="margin-bottom20 <%= (ctrlAlternativeBikes.FetchedRecordsCount > 0) ? "" : "hide" %>">
        <div class="container">
        <div class="grid-12 alternative-section" id="alternative-bikes-section">
            <h2 class="text-bold text-center margin-top20 margin-bottom30"><%= mmv.Make + " " + mmv.Model %> alternatives</h2>
            <div class="content-box-shadow">
                <div class="jcarousel-wrapper alternatives-carousel margin-top20">
                    <div class="jcarousel">
                        <ul>
                            <BW:AlternativeBikes ID="ctrlAlternativeBikes" runat="server" />
                        </ul>
                    </div>
                    <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev"></a></span>
                    <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next"></a></span>
                </div>
            </div>
        </div>
        <div class="clear"></div>
        </div>
    </section>

    <section class="margin-bottom20 <%= (ctrlUpcomingBikes.FetchedRecordsCount > 0) ? "" : "hide" %>">
        <!-- Upcoming bikes from brands -->
        <div class="container">
            <div class="grid-12">
                <h2 class="text-bold text-center margin-top20 margin-bottom30">Upcoming bikes from <%= mmv.Make %></h2>
                <div class="content-box-shadow rounded-corner2">
                    <div class="jcarousel-wrapper upcoming-brand-bikes-container margin-top20">
                        <div class="jcarousel">
                            <ul>
                                <BW:UpcomingBikes runat="server" ID="ctrlUpcomingBikes" />
                            </ul>
                        </div>
                        <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev"></a></span>
                        <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next"></a></span>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </div>
    </section>
    <!-- #include file="/includes/footerBW.aspx" -->
    <!-- #include file="/includes/footerscript.aspx" -->
    <script type="text/javascript">
    
    var variantsDropdown = $(".variants-dropdown"),
    variantSelectionTab = $(".variant-selection-tab"),
    variantUL = $(".variants-dropdown-list"),
    variantListLI = $(".variants-dropdown-list li");

    $.variantChangeDown = function (variantsDropdown) {
        variantsDropdown.addClass("open");
        variantUL.show();
    };

    $.variantChangeUp = function (variantsDropdown) {
        variantsDropdown.removeClass("open");
        variantUL.slideUp();
    };

    variantsDropdown.click(function (e) {
        if (!variantsDropdown.hasClass("open"))
            $.variantChangeDown(variantsDropdown);
        else
            $.variantChangeUp(variantsDropdown);
    });

    //TODO show the selected variant from dropdown

    $(document).mouseup(function (e) {
        if (!$(".variants-dropdown, .variant-selection-tab, .variant-selection-tab #upDownArrow").is(e.target)) {
            $.variantChangeUp($(".variants-dropdown"));
        }
    });
    </script>
    <script type="text/javascript">
    $(document).ready(function () {

        makeMapName = '<%= mmv.MakeMappingName%>';
        modelMapName = '<%= mmv.ModelMappingName%>';
        <%--$("#version_" + '<%= versionId%>').html("<b>this bike</b>");
        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Make_Page', 'act': 'Get_On_Road_Price_Click', 'lab': selectedModel });--%>
    });

    var fullName = $("#getFullName");
    var emailid = $("#getEmailID");
    var mobile = $("#getMobile");
    var prevEmail = "";
    var prevMobile = "";

    $("#getMoreDetailsBtnCampaign").on("click", function () {
        $("#leadCapturePopup").show();
        $('body').addClass('lock-browser-scroll');
        $(".blackOut-window").show();
        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Get_More_Details_Clicked', 'lab': bikeVersionLocation });
    });

    $(".leadCapture-close-btn, .blackOut-window").on("click mouseup", function () {
        $("#leadCapturePopup").hide();
        $('body').removeClass('lock-browser-scroll');
        $(".blackOut-window").hide();
    });

    $(".leadCapture-close-btn, .blackOut-window-model, #notifyOkayBtn").on("click", function () {
        $("#leadCapturePopup").hide();
        $('body').removeClass('lock-browser-scroll');
        $(".blackOut-window").hide();
        $("#contactDetailsPopup").show();
        $("#notify-response").hide();
    });

    if (bikeVersionLocation == '') {
        bikeVersionLocation = getBikeVersionLocation();
    }

    function getBikeVersionLocation() {        
        var loctn = GetGlobalCityArea();
        if (loctn != null) {
            if (loctn != '')
                loctn = '_' + loctn;
        }
        else {
            loctn = '';
        }
        var bikeVersionLocation = myBikeName + '_' + versionName + loctn;
        return bikeVersionLocation;
    }

    function setuserDetails() {
        var cookieName = "_PQUser";
        if (isCookieExists(cookieName)) {
            var arr = getCookie(cookieName).split("&");
            return arr;
        }
    }

    function setPQUserCookie() {
        var val = fullName.val() + '&' + emailid.val() + '&' + mobile.val();
        SetCookie("_PQUser", val);
    }

    function hideError(ele) {
        ele.removeClass("border-red");
        ele.siblings("span, div").hide();
    }


    function nameValTrue() {
        hideError(fullName)
        fullName.siblings("div").text('');
    };

    function ValidateUserDetail() {
        var isValid = true;
        isValid = validateEmail();
        isValid &= validateMobile();
        isValid &= validateName();
        return isValid;
    };


    /* Email validation */
    function validateEmail() {
        var isValid = true;
        var emailID = emailid.val();
        var reEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;

        if (emailID == "") {
            setError(emailid, 'Please enter email address');
            isValid = false;
        }
        else if (!reEmail.test(emailID)) {
            setError(emailid, 'Invalid Email');
            isValid = false;
        }
        return isValid;
    }

    function validateMobile() {
        var isValid = true;
        var reMobile = /^[0-9]{10}$/;
        var mobileNo = mobile.val();
        if (mobileNo == "") {
            isValid = false;
            setError(mobile, "Please enter your Mobile Number");
        }
        else if (!reMobile.test(mobileNo) && isValid) {
            isValid = false;
            setError(mobile, "Mobile Number should be 10 digits");
        }
        else {
            hideError(mobile)
        }
        return isValid;
    }

    function validateName() {
        var isValid = true;
        var a = fullName.val().length;
        if ((/&/).test(fullName.val())) {
            isValid = false;
            setError(fullName, 'Invalid name');
        }
        else
            if (a == 0) {
                isValid = false;
                setError(fullName, 'Please enter your first name');
            }
            else if (a >= 1) {
                isValid = true;
                nameValTrue()
            }
        return isValid;
    }

    function setError(ele, msg) {
        ele.addClass("border-red");
        ele.siblings("span, div").show();
        ele.siblings("div").text(msg);
    }


    fullName.on("focus", function () {
        hideError(fullName);
    });

    emailid.on("focus", function () {
        hideError(emailid);
        prevEmail = emailid.val().trim();
    });

    mobile.on("focus", function () {
        hideError(mobile)
        prevMobile = mobile.val().trim();

    });

    emailid.on("blur", function () {
        if (prevEmail != emailid.val().trim()) {
            if (validateEmail()) {               
                hideError(emailid);
            }
        }
    });

    mobile.on("blur", function () {
        if (mobile.val().length < 10) {
            $("#user-details-submit-btn").show();            
        }
        if (prevMobile != mobile.val().trim()) {
            if (validateMobile()) {
                hideError(mobile);
            }
        }
    });

    var customerViewModel = new CustomerModel();    
    ko.applyBindings(customerViewModel, $('#leadCapturePopup')[0]);

    function CustomerModel() {
        var arr = setuserDetails();
        var self = this;
        if (arr != null && arr.length > 0) {
            self.fullName = ko.observable(arr[0]);
            self.emailId = ko.observable(arr[1]);
            self.mobileNo = ko.observable(arr[2]);
        }
        else {
            self.fullName = ko.observable();
            self.emailId = ko.observable();
            self.mobileNo = ko.observable();
        }


        self.submitCampaignLead = function () {

            var isValidCustomer = ValidateUserDetail();

            if (isValidCustomer && campaignId > 0) {

                $('#processing').show();
                var objCust = {
                    "dealerId": manufacturerId,
                    "pqId": pqId,
                    "name": self.fullName(),
                    "mobile": self.mobileNo(),
                    "email": self.emailId(),
                    "versionId": versionId,
                    "cityId": cityId,
                    "leadSourceId": 3,
                    "deviceId": getCookie('BWC')
                }
                $.ajax({
                    type: "POST",
                    url: "/api/ManufacturerLead/",
                    data: ko.toJSON(objCust),
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader('utma', getCookie('__utma'));
                        xhr.setRequestHeader('utmz', getCookie('__utmz'));
                    },
                    async: false,
                    contentType: "application/json",
                    success: function (response) {
                        //var obj = ko.toJS(response);
                        $("#personalInfo,#otpPopup").hide();
                        $('#processing').hide();

                        //validationSuccess($(".get-lead-mobile"));
                        $("#contactDetailsPopup").hide();
                        $('#notify-response .notify-leadUser').text(self.fullName());
                        $('#notify-response').show();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        $('#processing').hide();
                        $("#contactDetailsPopup, #otpPopup").hide();
                        var leadMobileVal = mobile.val();
                        nameValTrue();
                        hideError(self.mobileNo());
                    }
                });

                setPQUserCookie();
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Lead_Submitted', 'lab': bikeVersionLocation });
            }
        };
    }
</script>
</form>
</body>
</html>