<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.BikeBooking.DealerPriceQuote" Trace="false" Async="true" EnableEventValidation="false"%>
<%@ Register Src="~/controls/AlternativeBikes.ascx" TagName="AlternativeBikes" TagPrefix="BW" %>
<%@ Register TagPrefix="PW" TagName="PopupWidget" Src="/controls/PopupWidget.ascx" %>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Import Namespace="Bikewale.BikeBooking" %>

<!doctype html>
<html>
<head>
<%
    title =  objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " + objPrice.objVersion.VersionName + " Price Quote ";
	description =  objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " + objPrice.objVersion.VersionName + " price quote";
    keywords = "";
    AdId = "1395986297721";
    AdPath = "/1017752/Bikewale_PriceQuote_";
    isAd970x90Shown = true;
%>
<!-- #include file="/includes/headscript.aspx" -->
<style type="text/css">
    #PQImageVariantContainer img { width:100%; }
    .PQDetailsTableTitle { color:#82888b; }
    .PQDetailsTableAmount, .PQOnRoadPrice { color:#4d5057; }
    .PQOffersUL { margin-left:18px; list-style:disc; }
    .PQOffersUL li { padding-bottom:15px; }
    .pqVariants .form-control-box { width:92%; }
    .form-control-box select.form-control { color:#4d5057; }

    .jcarousel-wrapper.alternatives-carousel { width: 974px; }
    .alternatives-carousel .jcarousel li {height: auto; margin-right:18px;}
    .alternatives-carousel .jcarousel li.front { border:none;}
    .alternative-section .jcarousel-control-left { left:-24px; }
    .alternative-section .jcarousel-control-right { right:-24px; }
    .alternative-section .jcarousel-control-left, .alternative-section .jcarousel-control-right { top:50%; }
    .newBikes-latest-updates-container .grid-4 { padding-left:10px;}
    .available-colors { display:inline-block; width:150px; text-align:center; margin-bottom: 20px; padding:0 5px; vertical-align: top; }
    .available-colors .color-box {width:60px; height:60px; margin:0 auto 15px; border-radius:3px; background:#f00; border:1px solid #ccc;}

    /* lead capture popup */
    #leadCapturePopup { display:none; width:650px; min-height:250px; background:#fff; margin:0 auto; position:fixed; top:15%; right:5%; left:5%; z-index:10; padding: 30px 20px 30px; }
    .personal-info-form-container { margin: 10px auto; width: 600px; min-height: 100px; }
    .personal-info-form-container .personal-info-list { margin:0 auto; width:270px; float:left; margin-left:15px; margin-right:15px; margin-bottom:25px; border-radius:0; }
    .personal-info-list .errorIcon, .personal-info-list .errorText { display:none; }
    .mobile-verification-container .errorIcon, .mobile-verification-container .errorText { display:none; }
    .mobile-verification-container { width: 610px; min-height:50px; margin:0 auto; padding:0 10px; }
    .mobile-verification-container p.confirm-otp-text { height:40px; width: 400px; text-align:left; }
    .mobile-verification-container p.otp-alert-text { min-height:20px; width: 240px; }
    .get-otp-code { width: 160px; }
    .resend-otp-btn:hover { cursor:pointer; }
    .input-border-bottom { border-bottom:1px solid #ccc; }

</style>
<script type="text/javascript">
    var dealerId = '<%= dealerId%>';
    var pqId = '<%= pqId%>';
    var ABHostUrl = '<%= System.Configuration.ConfigurationManager.AppSettings["ApiHostUrl"]%>';
    var versionId = '<%= versionId%>';
    var cityId = '<%= cityId%>';
    var Customername = "", email = "", mobileNo = "";
    var CustomerId = '<%= CurrentUser.Id %>';
    if (CustomerId != '-1') {
        Customername = '<%= objCustomer.CustomerName%>', email = '<%= objCustomer.CustomerEmail%>', mobileNo = '<%= objCustomer.CustomerMobile%>';
    } else {
        Customername = '<%= CustomerDetailCookie.CustomerName%>', email = '<%= CustomerDetailCookie.CustomerEmail%>', mobileNo = '<%= CustomerDetailCookie.CustomerMobile %>';
    }
    var clientIP = "<%= clientIP%>";
    var pageUrl = "www.bikewale.com/quotation/dealerpricequote.aspx?versionId=" + versionId + "&cityId=" + cityId;
</script>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section class="bg-light-grey padding-top10">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul>
                            <li><a href="/">Home</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/new/">New</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/pricequote/">On-Road Price Quote</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span>Dealer Price Quote</li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="font30 text-black margin-top10 margin-bottom10">On-road price quote</h1>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section class="container">
            <div class="grid-12 margin-bottom20" id="dealerPriceQuoteContainer">
                <div class="content-box-shadow content-inner-block-20 rounded-corner2">
                    <div class="grid-3 alpha" id="PQImageVariantContainer">
                        <% if (objPrice != null)
                           { %>
                        <div class="pqBikeImage margin-bottom20 margin-top5">
                            <img alt="<%= objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " + objPrice.objVersion.VersionName %> Photos" src="<%= Bikewale.Utility.Image.GetPathToShowImages(objPrice.OriginalImagePath,objPrice.HostUrl,Bikewale.Utility.ImageSize._210x118) %>" title="<%= objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " + objPrice.objVersion.VersionName%> Photos" />
                        </div>
                        <% } %>
                        <div class="hide">
                            <div class="<%= objColors.Count == 0 ? "hide" : "" %>" style="float: left; margin-right: 3px; padding-top: 3px;">Color: </div>
                            <div style="overflow: hidden;">
                                <ul class="colours <%= objColors.Count == 0 ? "hide" : ""%>">

                                    <asp:Repeater ID="rptColors" runat="server">
                                        <ItemTemplate>
                                            <li>
                                                <div title="<%#DataBinder.Eval(Container.DataItem,"ColorName") %>" style="background-color: #<%# DataBinder.Eval(Container.DataItem,"ColorCode")%>; height: 15px; width: 15px; margin: 5px; border: 1px solid #a6a9a7;"></div>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                                <div class="clear"></div>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <% if (versionList.Count > 1)
                           { %>
                        <div class="pqVariants">
                            <p class="font14 margin-bottom5">Variants</p>
                            <div class="form-control-box">
                                <asp:DropDownList ID="ddlVersion" CssClass="form-control" runat="server" AutoPostBack="true"></asp:DropDownList>
                            </div>
                        </div>
                        <% } %>
                    </div>
                    <div class="grid-5 padding-right20 <%= (objPrice.objOffers != null && objPrice.objOffers.Count > 0) ? "border-solid-right" : string.Empty%>" id="PQDetailsContainer">
                        <% if (objPrice != null)
                           { %>
                        <p class="font20 text-bold margin-bottom20"><%= objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " +objPrice.objVersion.VersionName%></p>
                        <% } %>
                        <% if (!String.IsNullOrEmpty(cityArea))
                           { %>
                        <p class="font16 margin-bottom15">On-road price in <%= cityArea%></p>
                        <% } %>
                        <div runat="server">
                            <div>
                                <% if (objPrice != null)
                                   { %>
                                <table class="font14" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <asp:Repeater ID="rptPriceList" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td width="245" class="PQDetailsTableTitle padding-bottom10">
                                                    <%# DataBinder.Eval(Container.DataItem,"CategoryName") %> <%# Bikewale.common.DealerOfferHelper.HasFreeInsurance(dealerId.ToString(),"",DataBinder.Eval(Container.DataItem,"CategoryName").ToString(),Convert.ToUInt32(DataBinder.Eval(Container.DataItem,"Price").ToString()),ref insuranceAmount) ? "<img alt='Free_icon' src='http://imgd1.aeplcdn.com/0x0/bw/static/free_red.png' title='Free_icon'/>" : "" %>
                                                </td>
                                                <td align="right" class="PQDetailsTableAmount text-bold padding-bottom10">
                                                    <span class="fa fa-rupee margin-right5"></span><span id="exShowroomPrice"><%#CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"Price").ToString()) %></span>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <tr>
                                        <td colspan="2">
                                            <div class="border-solid-top padding-bottom10"></div>
                                            <td>
                                    </tr>
                                    <%
                                       if (IsInsuranceFree)
                                       {
                                    %>
                                    <tr>
                                        <td class="PQDetailsTableTitle padding-bottom10">Total on road price</td>
                                        <td align="right" class="PQDetailsTableAmount text-bold padding-bottom10">
                                            <span class="fa fa-rupee"></span><span style="text-decoration: line-through;"><%= CommonOpn.FormatPrice(totalPrice.ToString()) %></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="PQDetailsTableTitle padding-bottom10">Minus insurance</td>
                                        <td align="right" class="PQDetailsTableAmount text-bold padding-bottom10">
                                            <span class="fa fa-rupee"></span><span><%= CommonOpn.FormatPrice(insuranceAmount.ToString()) %></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" class="PQDetailsTableTitle font18 text-bold PQOnRoadPrice">Total on road price</td>
                                        <td align="right" class="PQDetailsTableAmount font20 text-bold">
                                            <span class="fa fa-rupee"></span><span><%= CommonOpn.FormatPrice((totalPrice - insuranceAmount).ToString()) %></span>
                                        </td>
                                    </tr>
                                    <%
                                       }
                                       else
                                       {
                                    %>
                                    <tr>
                                        <td class="PQDetailsTableTitle font18 text-bold PQOnRoadPrice padding-bottom10">Total on road price</td>
                                        <td align="right" class="PQDetailsTableAmount padding-bottom10 font20 text-bold">
                                            <span class="fa fa-rupee margin-right5"></span><span><%= CommonOpn.FormatPrice(totalPrice.ToString()) %></span>
                                        </td>
                                    </tr>

                                    <% } %>
                                    <% if (!(objPrice.objOffers != null && objPrice.objOffers.Count > 0))
                                       {
                                           if (bookingAmount > 0)
                                           { %>
                                                <tr>
                                                    <td colspan="2" class="border-solid-top" align="right"><a class="margin-top15 btn btn-orange" id="btnBikeBooking" name="btnSavePriceQuote" onclick="dataLayer.push({ event: 'Bikewale_all', cat: 'New Bike Booking - <%=BikeName.Replace("'","")%>', act: 'Click Button Book Now',lab: 'Clicked on Button Get_Dealer_Details' });">Book Now</a></td>
                                                </tr>
                                          <% } else {%>
                                                <tr>
                                                    <td colspan="2" class="border-solid-top" align="right"><a class="margin-top15 btn btn-orange" id="leadBtnBookNow" name="leadBtnBookNow">Get Dealer Details</a></td>
                                                </tr>

                                    <% } } %>
                                    <tr class="hide">
                                        <td colspan="3">
                                            <ul class="std-ul-list">
                                                <asp:Repeater ID="rptDisclaimer" runat="server">
                                                    <ItemTemplate>
                                                        <li><i><%# Container.DataItem %></i></li>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ul>
                                        </td>
                                    </tr>
                                </table>
                                <% }
                                   else
                                   { %>
                                <div class="grey-bg border-light padding5 margin-top10 margin-bottom20">
                                    <h3>Dealer Prices for this Version is not available.</h3>
                                </div>
                                <% } %>
                            </div>

                        </div>

                        <div id="div_ShowErrorMsg" runat="server" class="grey-bg border-light content-block text-highlight margin-top15"></div>
                    </div>
                    <div class="grid-4 omega padding-left20" id="PQOffersContainer">
                        <!--Exciting offers div starts here-->
                        <% if (objPrice.objOffers != null && objPrice.objOffers.Count > 0)
                           { %>
                        <div id="divOffers">
                            <p class="font20 text-bold margin-bottom10 border-solid-bottom padding-bottom5"><%= IsInsuranceFree ? "BikeWale Offer" : "Available Offer"%></p>
                            <div>
                                <asp:Repeater ID="rptOffers" runat="server">
                                    <HeaderTemplate>
                                        <ul class="font14 text-light-grey PQOffersUL">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <li><%# DataBinder.Eval(Container.DataItem,"OfferText")%></li>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </ul>
                                    </FooterTemplate>
                                </asp:Repeater>
                                <div class="margin-top10">
                                    <a class="btn btn-orange" id="btnGetDealerDetails" name="btnSavePriceQuote" onclick="dataLayer.push({ event: 'Bikewale_all', cat: 'New Bike Booking - <%=BikeName.Replace("'","")%>', act: 'Click Button Get_Dealer_Details',lab: 'Clicked on Button Get_Dealer_Details' });">Avail offer</a>
                                </div>
                            </div>
                        </div>
                        <%}%>

                        <!--Exciting offers div ends here-->

                    </div>
                    <div class="clear"></div>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <section class="margin-bottom30 <%= (ctrlAlternativeBikes.FetchedRecordsCount > 0) ? string.Empty : "hide" %>">
            <div class="container">
                <div class="grid-12 alternative-section" id="alternative-bikes-section">
                    <h2 class="text-bold text-center margin-top20 margin-bottom30"><%= BikeName %> alternatives</h2>
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

        <% if(bookingAmount <= 0) { %>
        <!-- lead capture popup -->
        <div id="leadCapturePopup" class="text-center rounded-corner2"  >
            <div class="leadCapture-close-btn position-abt pos-top10 pos-right10 bwsprite cross-lg-lgt-grey cur-pointer"></div>
            <p class="font20 margin-bottom10">Provide Contact Details</p>
            <p class="text-light-grey margin-bottom20">For you to see BikeWale Dealer pricing and get a printable Certificate, we need your valid contact details. We promise to keep this information confidential and not use for any other purpose.</p>
            <div class="personal-info-form-container">
                <div class="form-control-box personal-info-list">
                    <input type="text" class="form-control get-first-name" placeholder="First name (mandatory)"
                        id="getFirstName" data-bind="value: firstName">
                    <span class="bwsprite error-icon errorIcon"></span>
                    <div class="bw-blackbg-tooltip errorText">Please enter your first name</div>
                </div>
                <div class="form-control-box personal-info-list">
                    <input type="text" class="form-control get-last-name" placeholder="Last name"
                        id="getLastName" data-bind="value: lastName">
                    <span class="bwsprite error-icon errorIcon"></span>
                    <div class="bw-blackbg-tooltip errorText">Please enter your last name</div>
                </div>
                <div class="form-control-box personal-info-list">
                    <input type="text" class="form-control get-email-id" placeholder="Email address (mandatory)"
                        id="getEmailID" data-bind="value: emailId">
                    <span class="bwsprite error-icon errorIcon"></span>
                    <div class="bw-blackbg-tooltip errorText">Please enter email address</div>
                </div>
                <div class="form-control-box personal-info-list">
                    <input type="text" class="form-control get-mobile-no" placeholder="Mobile no. (mandatory)"
                        id="getMobile" maxlength="10" data-bind="value: mobileNo">
                    <span class="bwsprite error-icon errorIcon"></span>
                    <div class="bw-blackbg-tooltip errorText">Please enter mobile number</div>
                </div>
                <div class="clear"></div>
                <a class="btn btn-orange margin-top20" id="user-details-submit-btn" data-bind="event: { click: submitLead() }">Submit</a>
            </div>
            <div class="mobile-verification-container hide">
                <div class="input-border-bottom"></div>
                <div class="margin-top20">
                    <p class="font14 confirm-otp-text leftfloat">Please confirm your contact details and enter the OTP for mobile verfication</p>
                    <div class="form-control-box">
                        <input type="text" class="form-control get-otp-code rightfloat" placeholder="Enter OTP" id="getOTP" data-bind="value: otpCode">
                        <span class="bwsprite error-icon errorIcon hide"></span>
                        <div class="bw-blackbg-tooltip errorText hide"></div>
                    </div>

                    <div class="clear"></div>
                </div>
                <a class="margin-left10 blue rightfloat resend-otp-btn margin-top10" id="resendCwiCode" data-bind="visible: (NoOfAttempts() < 2), click: function () { regenerateOTP() }">Resend OTP</a>
                <p class="otp-alert-text margin-left10 rightfloat otp-notify-text text-light-grey font12 margin-top10" data-bind="visible: (NoOfAttempts() >= 2)">
                    OTP has been already sent to your mobile
                </p>
                <div class="clear"></div>
                <br />
                <a class="btn btn-orange" id="otp-submit-btn">Confirm OTP</a>
                <div style="margin-right: 70px;" id="processing" class="hide"><b>Processing Please wait...</b></div>
            </div>
        </div>
        <% } %>
        <PW:PopupWidget runat="server" ID="PopupWidget" />
        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript">

            $('#btnGetDealerDetails, #btnBikeBooking').click(function () {
                window.location.href = '/pricequote/bookingsummary_new.aspx';
            });

            <% if(bookingAmount <= 0) { %>
            var isClicked = false;
            var leadBtnBookNow = $("#leadBtnBookNow"), leadCapturePopup = $("#leadCapturePopup");
            var firstname = $("#getFirstName");
            var lastname = $("#getLastName");
            var emailid = $("#getEmailID");
            var mobile = $("#getMobile");
            var otpContainer = $(".mobile-verification-container");

            var detailsSubmitBtn = $("#user-details-submit-btn");
            var otpText = $("#getOTP");
            var otpBtn = $("#otp-submit-btn");

            var prevEmail = "";
            var prevMobile = "";

            var getCityArea = GetGlobalCityArea();
            var customerViewModel =   new CustomerModel();

            $(function () {              

                leadBtnBookNow.on('click', function () {
                    isClicked = true;
                    leadCapturePopup.show();
                    $('body').addClass('lock-browser-scroll');
                    $(".blackOut-window").show();                     

                    $(".leadCapture-close-btn, .blackOut-window").on("click mouseup", function () {
                        leadCapturePopup.hide();
                        $('body').removeClass('lock-browser-scroll');
                        $(".blackOut-window").hide();
                    });

                    $(document).on('keydown', function (e) {
                        if (e.keyCode === 27) {
                            $("#leadCapturePopup .leadCapture-close-btn").click();
                        }
                    });

                });

            });

            ko.applyBindings(customerViewModel, $('#leadCapturePopup')[0]);

            function CustomerModel() {
                var arr = setuserDetails();
                var self = this;
                if (arr != null && arr.length > 0) {
                    self.firstName = ko.observable(arr[0]);
                    self.lastName = ko.observable(arr[1]);
                    self.emailId = ko.observable(arr[2]);
                    self.mobileNo = ko.observable(arr[3]);
                }
                else {
                    self.firstName = ko.observable();
                    self.lastName = ko.observable();
                    self.emailId = ko.observable();
                    self.mobileNo = ko.observable();
                }
                self.IsVerified = ko.observable(false);
                self.NoOfAttempts = ko.observable(0);
                self.IsValid = ko.computed(function () { return self.IsVerified(); }, this);
                self.otpCode = ko.observable();
                self.fullName = ko.computed(function () {
                    var _firstName = self.firstName() != undefined ? self.firstName() : "";
                    var _lastName = self.lastName() != undefined ? self.lastName() : "";
                    return _firstName + ' ' + _lastName;
                }, this);

                self.verifyCustomer = function () {
                    if (!self.IsVerified()) {
                        var objCust = {
                            "dealerId": dealerId,
                            "pqId": pqId,
                            "customerName": self.fullName,
                            "customerMobile": self.mobileNo,
                            "customerEmail": self.emailId,
                            "clientIP": clientIP,
                            "pageUrl": pageUrl,
                            "versionId": versionId,
                            "cityId": cityId
                        }
                        $.ajax({
                            type: "POST",
                            url: "/api/PQCustomerDetail/",
                            data: ko.toJSON(objCust),
                            async: false,
                            contentType: "application/json",
                            success: function (response) {
                                var obj = ko.toJS(response);
                                self.IsVerified(obj.isSuccess);
                                if (!self.IsVerified()) {
                                    self.NoOfAttempts(obj.noOfAttempts);
                                }
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                self.IsVerified(false);
                            }
                        });
                    }
                };
                self.generateOTP = function () {
                    if (!self.IsVerified()) {
                        var objCust = {
                            "pqId": pqId,
                            "customerMobile": self.mobileNo,
                            "customerEmail": self.emailId,
                            "cwiCode": self.otpCode,
                            "branchId": dealerId,
                            "customerName": self.fullName,
                            "versionId": versionId,
                            "cityId": cityId
                        }
                        $.ajax({
                            type: "POST",
                            url: "/api/PQMobileVerification/",
                            data: ko.toJSON(objCust),
                            async: false,
                            contentType: "application/json",
                            success: function (response) {
                                var obj = ko.toJS(response);
                                self.IsVerified(obj.isSuccess);

                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                self.IsVerified(false);
                            }
                        });
                    }
                };

                self.regenerateOTP = function () {
                    if (self.NoOfAttempts() <= 2 && !self.IsVerified()) {
                        var url = '/api/ResendVerificationCode/';
                        var objCustomer = {
                            "customerName": self.fullName(),
                            "customerMobile": self.mobileNo(),
                            "customerEmail": self.emailId(),
                            "source": 1
                        }
                        $.ajax({
                            type: "POST",
                            url: url,
                            async: false,
                            data: ko.toJSON(objCustomer),
                            contentType: "application/json",
                            success: function (response) {
                                self.IsVerified(false);
                                self.NoOfAttempts(response.noOfAttempts);
                                alert("You will receive the new OTP via SMS shortly.");
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                self.IsVerified(false);
                            }
                        });
                    }
                };

                self.fullName = ko.computed(function () {
                    var _firstName = self.firstName() != undefined ? self.firstName() : "";
                    var _lastName = self.lastName() != undefined ? self.lastName() : "";
                    return _firstName + ' ' + _lastName;
                }, this);

                self.submitLead = function () {
                    if (isClicked && ValidateUserDetail()) {
                        self.verifyCustomer();
                        if (self.IsValid()) {                             
                            $("#personalInfo").hide();
                            $("#leadCapturePopup .leadCapture-close-btn").click();
                            window.location.href = "/pricequote/detaileddealerquotation.aspx";
                        }
                        else {
                            otpContainer.removeClass("hide").addClass("show");
                            detailsSubmitBtn.hide();
                            nameValTrue();
                            hideError(mobile);
                            otpText.val('').removeClass("border-red").siblings("span, div").hide();
                        }
                        setPQUserCookie();
                        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Step_1_Successful_Submit', 'lab': getCityArea });
                    }
                    
                };                

                otpBtn.click(function () {
                    $('#processing').show();
                    if (!validateOTP())
                        $('#processing').hide();

                    if (validateOTP() && ValidateUserDetail()) {
                        customerViewModel.generateOTP(); 
                        if (customerViewModel.IsVerified()) {
                            // $.customizeState();
                            $("#personalInfo").hide();
                            $(".booking-dealer-details").removeClass("hide").addClass("show");
                            $('#processing').hide();

                            detailsSubmitBtn.show();
                            otpText.val('');
                            otpContainer.removeClass("show").addClass("hide");

                            // OTP Success
                            dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation_Page', 'act': 'Step_1_OTP_Successful_Submit', 'lab': getCityArea });

                            $("#leadCapturePopup .leadCapture-close-btn").click();

                            window.location.href = "/pricequote/detaileddealerquotation.aspx";

                        }
                        else {
                            $('#processing').hide();
                            otpVal("Please enter a valid OTP.");
                            // push OTP invalid
                            dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation Page', 'act': 'Step_1_OTP_Submit_Error', 'lab': getCityArea });
                        }
                    }
                });

            }

            function ValidateUserDetail() {
                var isValid = true;
                isValid = validateEmail();
                isValid &= validateMobile();
                isValid &= validateName();
                isValid &= validateLastName();
                return isValid;
            };

            function validateLastName() {
                var isValid = true;
                if (lastname.val().indexOf('&') != -1) {
                    isValid = false;
                    setError(lastname, 'Invalid name');
                }
                else {
                    isValid = true;
                    lastnameValTrue();
                }
                return isValid;
            }


            function validateName() {
                var isValid = true;
                var a = firstname.val().length;
                if (firstname.val().indexOf('&') != -1) {
                    isValid = false;
                    setError(firstname, 'Invalid name');
                }
                else if (a == 0) {
                    isValid = false;
                    setError(firstname, 'Please enter your first name');
                }
                else if (a >= 1) {
                    isValid = true;
                    nameValTrue()
                }
                if (!isValid) { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation Page', 'act': 'Step_1_Submit_Error_Name', 'lab': cityArea }); }
                return isValid;
            }

            function  lastnameValTrue() {
                hideError(lastname)
                lastname.siblings("div").text('');
            };
            function nameValTrue() {
                hideError(firstname)
                firstname.siblings("div").text('');
            };

            firstname.on("focus", function () {
                hideError(firstname);
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
                    var getCityArea = GetGlobalCityArea();
                    if (validateEmail(getCityArea)) {
                        customerViewModel.IsVerified(false);
                        detailsSubmitBtn.show();
                        otpText.val('');
                        otpContainer.removeClass("show").addClass("hide");
                        hideError(emailid);
                    }
                    $('#confirmation-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
                    $('#customize-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
                }
                else
                    customerViewModel.IsVerified(true);
            });

            mobile.on("blur", function () {
                if (prevMobile != mobile.val().trim()) {
                    var getCityArea = GetGlobalCityArea();
                    if (validateMobile(getCityArea)) {
                        customerViewModel.IsVerified(false);
                        detailsSubmitBtn.show();
                        otpText.val('');
                        otpContainer.removeClass("show").addClass("hide");
                        hideError(mobile);
                    }
                    $('#confirmation-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
                    $('#customize-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
                }
                else
                    customerViewModel.IsVerified(true);

            });

            function mobileValTrue() {
                mobile.removeClass("border-red");
                mobile.siblings("span, div").hide();
            };


            otpText.on("focus", function () {
                otpText.val('');
                otpText.siblings("span, div").hide();
            });

            function setError(ele, msg) {
                ele.addClass("border-red");
                ele.siblings("span, div").show();
                ele.siblings("div").text(msg);
            }

            function hideError(ele) {
                ele.removeClass("border-red");
                ele.siblings("span, div").hide();
            }
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
                if (!isValid) { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation Page', 'act': 'Step_1_Submit_Error_Email', 'lab': cityArea }); }
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
                if (!isValid) { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation Page', 'act': 'Step_1_Submit_Error_Mobile', 'lab': cityArea }); }
                return isValid;
            }

            var otpVal = function (msg) {
                otpText.addClass("border-red");
                otpText.siblings("span, div").show();
                otpText.siblings("div").text(msg);
            };


            function validateOTP() {
                var retVal = true;
                var isNumber = /^[0-9]{5}$/;
                var cwiCode = otpText.val();
                customerViewModel.IsVerified(false);
                if (cwiCode == "") {
                    retVal = false;
                    otpVal("Please enter your Verification Code");
                }
                else {
                    if (isNaN(cwiCode)) {
                        retVal = false;
                        otpVal("Verification Code should be numeric");
                    }
                    else if (cwiCode.length != 5) {
                        retVal = false;
                        otpVal("Verification Code should be of 5 digits");
                    }
                }
                return retVal;

            }

            function setuserDetails() {
                var cookieName = "_PQUser";
                if (isCookieExists(cookieName)) {
                    var arr = getCookie(cookieName).split("&");
                    return arr;
                }
            }

            function setPQUserCookie() {
                var val = firstname.val() + '&' + lastname.val() + '&' + emailid.val() + '&' + mobile.val();
                SetCookie("_PQUser", val);
            }

            <% } %>

        </script>
    </form>
</body>
</html>
