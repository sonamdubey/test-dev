<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.BikeBooking.DealerPriceQuote" trace="false" Async="true" %>
<%@ Register Src="~/m/controls/AlternativeBikes.ascx" TagPrefix="BW" TagName="AlternateBikes" %>
<%@ Register TagPrefix="BW" TagName="MPopupWidget" Src="/m/controls/MPopupWidget.ascx" %>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Import Namespace="Bikewale.BikeBooking" %>
<!doctype html>
<html>
<head>
<%
    title = "";
    keywords = "";
    description = "";
    canonical = "";
    AdPath = "/1017752/Bikewale_Mobile_PriceQuote";
    AdId = "1398766000399";
%>
<script>var quotationPage = true;</script>
<!-- #include file="/includes/headscript_mobile.aspx" -->
<link rel="stylesheet"  href="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/css/bw-new-style.css?<%= staticFileVersion %>" />
<link href="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/css/chosen.min.css?<%= staticFileVersion %>" type="text/css"rel="stylesheet" />

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
<style type="text/css">
    .inner-section{background:#fff; clear:both; overflow:hidden;}
    .alternatives-carousel .jcarousel li.front { border:none;}
    .discover-bike-carousel .jcarousel li { height: auto; }
    .discover-bike-carousel .front { height:auto; }
    #leadCapturePopup .leadCapture-close-btn {z-index:2;}
    #leadCapturePopup .error-icon, #leadCapturePopup .bw-blackbg-tooltip {display:none} 
</style>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <div class="bg-white box1 box-top new-line5 bot-red new-line10">

            <div class="bike-img">
                <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(objPrice.OriginalImagePath,objPrice.HostUrl,Bikewale.Utility.ImageSize._640x348) %>" alt="" title="" border="0" />
            </div>
            <h1 class="margin-top20 font18 padding-left10 padding-right10" style="margin-left: 0px;"><%= objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " + objPrice.objVersion.VersionName %> Price Quote</h1>
            <div class="<%= objColors.Count == 0 ?"hide":"hide" %>">
                <div class="full-border new-line10 selection-box">
                    <b>Color Options: </b>
                    <table width="100%">
                        <tr style="margin-bottom: 5px;">
                            <td class="break-line" colspan="2"></td>
                        </tr>
                        <asp:Repeater ID="rptColors" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td style="width: 30px;">
                                        <div style="width: 30px; height: 20px; margin: 0px 10px 0px 0px; border: 1px solid #a6a9a7; padding-top: 5px; background-color: #<%# DataBinder.Eval(Container.DataItem,"ColorCode")%>"></div>
                                    </td>
                                    <td>
                                        <div class="new-line"><%# DataBinder.Eval(Container.DataItem,"ColorName") %></div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </div>
            <div class="<%= versionList.Count>1 ?"":"hide" %> margin-top20">
                <asp:DropDownList ID="ddlVersion" CssClass="form-control" runat="server" AutoPostBack="true"></asp:DropDownList>
            </div>
            <!--Price Breakup starts here-->
            <div class="new-line15 padding-left10 padding-right10" style="margin-top: 20px;">

                <% if (!String.IsNullOrEmpty(cityArea))
                   { %>
                <h2 class="font16" style="font-weight: normal">On-road price in <%= cityArea %></h2>
                <% } %>
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="pqTable font14">

                    <asp:Repeater ID="rptPriceList" runat="server">
                        <ItemTemplate>
                            <%-- Start 102155010 --%>

                            <tr>
                                <td align="left" class="text-medium-grey"><%# DataBinder.Eval(Container.DataItem,"CategoryName") %> <%# Bikewale.common.DealerOfferHelper.HasFreeInsurance(dealerId.ToString(),"",DataBinder.Eval(Container.DataItem,"CategoryName").ToString(),Convert.ToUInt32(DataBinder.Eval(Container.DataItem,"Price").ToString()),ref insuranceAmount) ? "<img class='insurance-free-icon' alt='Free_icon' src='http://imgd1.aeplcdn.com/0x0/bw/static/free_red.png' title='Free_icon'/>" : "" %></td>
                                <td align="right" class="text-grey text-bold"><span class="fa fa-rupee"></span><%# CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"Price").ToString()) %></td>
                            </tr>
                            <%-- End 102155010 --%>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr align="left">
                        <td height="10" colspan="2" style="padding: 0;"></td>
                    </tr>
                    <tr align="left">
                        <td height="1" colspan="2" class="break-line" style="padding: 0 0 10px;"></td>
                    </tr>
                    <%-- Start 102155010 --%>

                    <%
                        if (IsInsuranceFree)
                        {
                    %>
                    <tr>
                        <td align="left" class="text-medium-grey">Total On Road Price</td>
                        <td align="right" class="text-grey text-bold">
                            <div><span class="fa fa-rupee"></span><span style="text-decoration: line-through"><%= CommonOpn.FormatPrice(totalPrice.ToString()) %></span></div>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="text-medium-grey">Minus Insurance</td>
                        <td align="right" class="text-grey text-bold">
                            <div><span class="fa fa-rupee"></span><%= CommonOpn.FormatPrice(insuranceAmount.ToString()) %></div>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="text-medium-grey">BikeWale On Road (after insurance offer)</td>
                        <td align="right" class="text-grey text-bold">
                            <div><span class="fa fa-rupee"></span><%= CommonOpn.FormatPrice((totalPrice - insuranceAmount).ToString()) %></div>

                        </td>
                    </tr>
                    <%
                        }
                        else
                        {%>
                    <tr>
                        <td align="left" class="text-grey font16">Total On Road Price</td>
                        <td align="right" class="text-grey text-bold font18">
                            <div><span class="fa fa-rupee"></span><%= CommonOpn.FormatPrice(totalPrice.ToString()) %></div>

                        </td>
                    </tr>
                    <%
                    }
                    %>
                    <%-- End 102155010 --%>
                    <tr align="left">
                        <td height="20" colspan="2" style="padding: 0;"></td>
                    </tr>
                    <tr align="left">
                        <td height="1" colspan="2" class="break-line-light" style="padding: 0;">&nbsp;</td>
                    </tr>
                </table>
                <ul class="grey-bullet hide">
                    <asp:Repeater ID="rptDisclaimer" runat="server">
                        <ItemTemplate>
                            <li><i><%# Container.DataItem %></i></li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
            <!--Price Breakup ends here-->
            <!--Exciting Offers section starts here-->
            <% if (objPrice.objOffers != null && objPrice.objOffers.Count > 0)
               { %>
            <div class="new-line10 padding-left10 padding-right10 margin-bottom15" id="divOffers" style="background: #fff;">
                <h2 class="font24 text-center text-grey"><%= IsInsuranceFree ? "BikeWale Offer" : "Get Absolutely Free"%></h2>
                <div class="new-line10">
                    <asp:Repeater ID="rptOffers" runat="server">
                        <HeaderTemplate>
                            <ul class="grey-bullet">
                        </HeaderTemplate>
                        <ItemTemplate>

                            <li><%# DataBinder.Eval(Container.DataItem,"OfferText")%></li>

                        </ItemTemplate>
                        <FooterTemplate>
                            </ul>                        
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
            <div class="padding-left10 padding-right10">
                <% if (bookingAmount > 0)
                   { %>
                <button type="button" data-role="none" id="getDealerDetails" class="btn btn-full-width btn-orange" style="margin-bottom: 20px;" onclick="dataLayer.push({ event: 'Bikewale_all', cat: 'DealerQuotation_Page - <%=MakeModel.Replace("'","") %>', act: 'Click Button Get_Dealer_Details',lab: 'Clicked on Button Get_Dealer_Details' });">Avail offer</button>
                <% }
                   else
                   { %>
                <button type="button" data-role="none" id="leadBtnBookNow" name="leadBtnBookNow" class="btn btn-full-width btn-orange" style="margin-bottom: 20px;" onclick="dataLayer.push({ event: 'Bikewale_all', cat: 'DealerQuotation_Page - <%=MakeModel.Replace("'","") %>', act: 'Click Button Book Now',lab: 'Clicked on Button Get_Dealer_Details' });">Get Dealer Details</button>
                <% } %>
                <%}
               else
               {
                   if (bookingAmount > 0)
                   { %>
                <button type="button" data-role="none" id="btnBookBike" class="btn btn-full-width btn-orange" style="margin-bottom: 20px;" onclick="dataLayer.push({ event: 'Bikewale_all', cat: 'New Bike Booking - <%=MakeModel.Replace("'","") %>', act: 'Click Button Book Now',lab: 'Clicked on Button Get_Dealer_Details' });">Book Now</button>
                <% }
                   else
                   {%>
                <button type="button" data-role="none" id="leadBtnBookNow" name="leadBtnBookNow" class="btn btn-full-width btn-orange" style="margin-bottom: 20px;" onclick="dataLayer.push({ event: 'Bikewale_all', cat: 'New Bike Booking - <%=MakeModel.Replace("'","") %>', act: 'Click Button Book Now',lab: 'Clicked on Button Get_Dealer_Details' });">Get Dealer Details</button>
                <% }
               }%>
            </div>
            <!--Exciting Offers section ends here-->
        </div>


        <section class="<%= (ctrlAlternateBikes.FetchedRecordsCount > 0) ? "" : "hide" %>">
            <div class="container margin-bottom30">
                <div class="grid-12">
                    <!-- Most Popular Bikes Starts here-->
                    <h2 class="margin-top30px margin-bottom20 text-center padding-top20"><%= objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName  %> alternatives</h2>

                    <div class="jcarousel-wrapper discover-bike-carousel alternatives-carousel">
                        <div class="jcarousel">
                            <BW:AlternateBikes ID="ctrlAlternateBikes" runat="server" />
                        </div>
                        <span class="jcarousel-control-left"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-prev"></a></span>
                        <span class="jcarousel-control-right"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-next"></a></span>
                        <p class="text-center jcarousel-pagination"></p>
                    </div>

                </div>
                <div class="clear"></div>
            </div>
        </section>


        <% if (bookingAmount <= 0)
           { %>
        <!-- lead capture popup -->
        <div id="leadCapturePopup" class="bw-popup contact-details hide">
            <div class="popup-inner-container">
                <div class="bwmsprite close-btn leadCapture-close-btn rightfloat"></div>
                <h2>Please provide us contact details</h2>

                <div class="personal-info-form-container margin-top10">
                    <div class="form-control-box">
                        <input type="text" class="form-control get-first-name" placeholder="First name" id="getFirstName" data-bind="value: firstName">
                        <span class="bwmsprite error-icon "></span>
                        <div class="bw-blackbg-tooltip errorText">Please enter your first name</div>
                    </div>
                    <div class="form-control-box margin-top20">
                        <input type="text" class="form-control get-last-name" placeholder="Last name" id="getLastName" data-bind="value: lastName">
                        <span class="bwmsprite error-icon"></span>
                        <div class="bw-blackbg-tooltip errorText">Please enter your last name</div>
                    </div>
                    <div class="form-control-box margin-top20">
                        <input type="text" class="form-control get-email-id" placeholder="Email address" id="getEmailID" data-bind="value: emailId">
                        <span class="bwmsprite error-icon"></span>
                        <div class="bw-blackbg-tooltip errorText">Please enter your email adress</div>
                    </div>
                    <div class="form-control-box margin-top20">
                        <input type="text" class="form-control get-mobile-no" maxlength="10" placeholder="Mobile no." id="getMobile" data-bind="value: mobileNo">
                        <span class="bwmsprite error-icon"></span>
                        <div class="bw-blackbg-tooltip errorText">Please enter mobile number</div>
                    </div>
                    <div class="clear"></div>
                    <a class="btn btn-full-width btn-orange margin-top20" id="user-details-submit-btn" data-bind="event: { click: submitLead }">Submit</a>
                </div>

                <div class="mobile-verification-container margin-top20 hide">
                    <p class="font12 text-center margin-bottom10 padding-left15 padding-right15">Please confirm your contact details and enter the OTP for mobile verfication</p>
                    <div class="form-control-box  padding-left15 padding-right15">
                        <input type="text" class="form-control get-otp-code text-center" placeholder="Enter OTP" id="getOTP" data-bind="value: otpCode">
                        <span class="bwmsprite error-icon hide"></span>
                        <div class="bw-blackbg-tooltip errorText hide">Please enter a valid OTP</div>
                    </div>
                    <div class="text-center padding-top10">
                        <a class="margin-left10 blue resend-otp-btn margin-top10" id="resendCwiCode" data-bind="visible: (NoOfAttempts() < 2), click: function () { regenerateOTP() }">Resend OTP</a>
                        <p class="margin-left10 blue resend-otp-btn margin-top10 otp-notify-text text-light-grey font12" data-bind="visible: (NoOfAttempts() >= 2)">
                            OTP has been already sent to your mobile
                        </p>
                    </div>

                    <div class="clear"></div>
                    <a class="btn btn-full-width btn-orange margin-top20" id="otp-submit-btn">Confirm</a>
                    <div id="processing" class="hide" style="text-align: center; font-weight: bold;">Processing Please wait...</div>
                </div>

                <input type="button" class="btn btn-full-width btn-orange hide" value="Submit" onclick="validateDetails();" class="rounded-corner5" data-role="none" id="btnSubmit" />
            </div>
        </div>
        <% } %>


        <BW:MPopupWidget runat="server" ID="MPopupWidget" />
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/chosen-jquery-min-mobile.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript">
            $('#getDealerDetails,#btnBookBike').click(function () {
                window.location.href = '/m/pricequote/bookingsummary_new.aspx';
            });

            var freeInsurance = $("img.insurance-free-icon");
            if (!freeInsurance.length) {
                cityArea = GetGlobalCityArea();
                $("table tr td.text-medium-grey:contains('Insurance')").html("Insurance  (<a href='/m/insurance/' style='position: relative; font-size: 12px; margin-top: 1px;' target='_blank' onclick=\"dataLayer.push({ event: 'Bikewale_all', cat: 'Dealer_PQ', act: 'Insurance_Clicked',lab: '<%= String.Format("{0}_{1}_{2}_",objPrice.objMake.MakeName,objPrice.objModel.ModelName,objPrice.objVersion.VersionName)%>" + cityArea + "' });\">Up to 60% off - PolicyBoss </a>)<span style='margin-left: 5px; vertical-align: super; font-size: 9px;'>Ad</span>");
            }

             <% if (bookingAmount <= 0)
                { %>
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
            var customerViewModel = new CustomerModel();

            $(function () {

                leadBtnBookNow.on('click', function () {
                    isClicked = true;
                    leadCapturePopup.show();
                    $('body').addClass('lock-browser-scroll');
                    $(".blackOut-window").show();

                    $(".leadCapture-close-btn, .blackOut-window").on("click", function () {
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
                            "source": 2
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
                        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation_Page', 'act': 'Step_1_Successful_Submit', 'lab': getCityArea });
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
                //if (lastname.val().indexOf('&') != -1) {
                if ((/&/).test(lastname.val())) {
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
                //if (firstname.val().indexOf('&') != -1) {
                if ((/&/).test(lastname.val())) {
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
                if (!isValid) { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation Page', 'act': 'Step_1_Submit_Error_Name', 'lab': getCityArea }); }
                return isValid;
            }

            function lastnameValTrue() {
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

            emailid.on("keyup keydown blur", function () {
                if (prevEmail != emailid.val().trim()) {
                    if (validateEmail()) {
                        customerViewModel.IsVerified(false);
                        detailsSubmitBtn.show();
                        otpText.val('');
                        otpContainer.removeClass("show").addClass("hide");
                        hideError(emailid);
                    }
                    $('#confirmation-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
                    $('#customize-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
                }
            });

            mobile.on("keyup keydown blur", function () {
                if (mobile.val().length < 10) {
                    $("#user-details-submit-btn").show();
                    $(".mobile-verification-container").removeClass("show").addClass("hide");
                }
                if (prevMobile != mobile.val().trim()) {
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
                if (!isValid) { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation Page', 'act': 'Step_1_Submit_Error_Email', 'lab': getCityArea }); }
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
                if (!isValid) { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation Page', 'act': 'Step_1_Submit_Error_Mobile', 'lab': getCityArea }); }
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