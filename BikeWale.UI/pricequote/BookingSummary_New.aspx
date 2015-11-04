<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.BikeBooking.BookingSummary_New" %>

<!DOCTYPE html>
<html>
<head>
    <%
        title = bikeName + " Bookingbooking-sprite buy-icon customize-icon-grey Summary";
        description = "Authorise dealer price details of a bike " + bikeName;
        keywords = bikeName + ", price, authorised, dealer,Booking ";    
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/booking.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
</head>
<body class="header-fixed-inner">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section class="bg-light-grey padding-top10">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul>
                            <li><a href="/">Home</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span>New Bikes</li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="font30 text-black margin-top10 margin-bottom10">Avail great offers in 3 simple steps</h1>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <section class="container">
            <!--  Discover bikes section code starts here -->
            <div class="grid-12">
                <div class="content-box-shadow content-inner-block-10">
                    <!-- ko with: viewModel.SelectedVarient() -->
                    <div class="grid-4 bikeModel-image-container inline-block">
                        <div class="imageWrapper margin-top10">
                            <%--<img src="http://imgd8.aeplcdn.com/227x128//bikewaleimg/models/490b.jpg?20140909123254" alt="<%= bikeName %>" title="<%= bikeName %>">--%>
                            <img data-bind="attr: { src: imageUrl, alt: bikeName, title: bikeName }" />
                        </div>
                    </div>
                    <div class="grid-4 bikeModel-details-table inline-block">
                        <h3 class="margin-bottom15" data-bind="text: bikeName"></h3>
                        <div class="font14">
                            <table>
                                <tbody>
                                    <tr>
                                        <td width="200" class="padding-bottom10">On road price:</td>
                                        <td align="right" class="padding-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span><span data-bind="CurrencyText: onRoadPrice"></span></td>
                                    </tr>
                                    <tr>
                                        <td>Advance booking:</td>
                                        <td align="right" class="text-bold"><span class="fa fa-rupee margin-right5"></span><span data-bind="CurrencyText: bookingAmount"></span></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="padding-bottom10"><a id="cancellation-box" href="#">Hassle free cancellation policy</a></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="border-solid-top padding-bottom10"></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Balance amount:</td>
                                        <td align="right" class="font18 text-bold finalBalanceAmount"><span class="fa fa-rupee margin-right5"></span><span data-bind="CurrencyText: remainingAmount"></span></td>
                                    </tr>
                                    <tr>
                                        <td class="font12 bikeModel-balance-text" colspan="2">*Balance amount payable at the dealership</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <!-- /ko -->

                    <div class="grid-4 inline-block border-solid-left">
                        <!-- ko if: viewModel.Dealer() -->
                        <!-- ko with: viewModel.Dealer() -->
                        <div class="booking-dealer-details">
                            <h3 class="font18 margin-bottom15" data-bind="text: organization()"></h3>
                            <p class="font14 text-light-grey margin-bottom10" data-bind="text: address1() + ' ' + address2() + ', ' + area() + ', ' + city() + ', ' + state() + ', ' + pincode()"></p>
                            <p class="font14 margin-bottom10"><span class="fa fa-phone margin-right5"></span><span data-bind="text: phoneNo()"></span></p>
                            <div>
                                <%--<iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3770.9863262686094!2d72.99639100000005!3d19.06433880000001!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x3be7c136b2c080cb%3A0x225353b221740ef0!2sCarWale!5e0!3m2!1sen!2sin!4v1441706839659" width="290" height="90" frameborder="0" style="border: 0" allowfullscreen></iframe>--%>
                                <%--<div id="divMap" data-bind="style: { width: showMap ? '290px' : 0, height: showMap ? '90px' : 0 }"></div>--%>
                                <div class="bikeModel-dealerMap-container" style="width: 290px; height: 90px" data-bind="googlemap: { latitude: lattitude(), longitude: longitude() }"></div>
                            </div>
                        </div>
                        <!-- /ko -->
                        <!-- /ko -->
                        <!-- ko if: !viewModel.Dealer() -->
                        <div class="call-for-queries text-center">
                            <div class="query-call-pic bg-white text-center">
                                <div class="bookingcomforts-sprite buying-asst-icon"></div>
                            </div>
                            <p class="font14 margin-bottom10">In case of queries call us on:</p>
                            <p class="font18 text-bold">1800 120 8300</p>
                        </div>
                        <!-- /ko -->
                    </div>
                    <div class="clear"></div>
                </div>
            </div>
        </section>

        <section class="container">
            <!--  Don't know which bike to buy section code starts here -->
            <div class="grid-12">
                <div class="booking-wrapper text-center content-box-shadow margin-top20">
                    <div class="bike-to-buy-tabs booking-tabs">
                        <div class="horizontal-line position-rel margin-auto"></div>
                        <ul class="margin-bottom20">
                            <li>
                                <div id="personal-info-tab" class="bike-booking-part active-tab text-bold" data-tabs-buy="personalInfo">
                                    <div class="bike-booking-title">
                                        <p class="font16">Personal Information</p>
                                    </div>
                                    <div class="bike-booking-image">
                                        <span class="booking-sprite buy-icon personalInfo-icon-selected"></span>
                                    </div>
                                </div>
                            </li>
                            <li>
                                <div id="customize-tab" class="bike-booking-part disabled-tab" data-tabs-buy="customize">
                                    <div class="bike-booking-title">
                                        <p class="font16">Customize</p>
                                    </div>
                                    <div class="bike-booking-image">
                                        <span class="booking-sprite buy-icon customize-icon-grey"></span>
                                    </div>
                                </div>
                            </li>
                            <li>
                                <div id="confirmation-tab" class="bike-booking-part disabled-tab" data-tabs-buy="confirmation">
                                    <div class="bike-booking-title">
                                        <p class="font16">Confirmation</p>
                                    </div>
                                    <div class="bike-booking-image">
                                        <span class="booking-sprite buy-icon confirmation-icon-grey"></span>
                                    </div>
                                </div>
                            </li>
                        </ul>
                    </div>

                    <div id="personalInfo" class="">
                        <p class="font16">Please provide us contact details for your booking</p>
                        <div class="personal-info-form-container">
                            <div class="form-control-box personal-info-list">
                                <input type="text" class="form-control get-first-name" placeholder="First name (mandatory)"
                                    id="getFirstName" data-bind="value: viewModel.CustomerVM().firstName">
                                <span class="bwsprite error-icon errorIcon"></span>
                                <div class="bw-blackbg-tooltip errorText">Please enter your first name</div>
                            </div>
                            <div class="form-control-box personal-info-list">
                                <input type="text" class="form-control get-last-name" placeholder="Last name"
                                    id="getLastName" data-bind="value: viewModel.CustomerVM().lastName">
                                <span class="bwsprite error-icon errorIcon"></span>
                                <div class="bw-blackbg-tooltip errorText">Please enter your last name</div>
                            </div>
                            <div class="form-control-box personal-info-list">
                                <input type="text" class="form-control get-email-id" placeholder="Email address (mandatory)"
                                    id="getEmailID" data-bind="value: viewModel.CustomerVM().emailId">
                                <span class="bwsprite error-icon errorIcon"></span>
                                <div class="bw-blackbg-tooltip errorText">Please enter email address</div>
                            </div>
                            <div class="form-control-box personal-info-list">
                                <input type="text" class="form-control get-mobile-no" placeholder="Mobile no. (mandatory)"
                                    id="getMobile" maxlength="10" data-bind="value: viewModel.CustomerVM().mobileNo">
                                <span class="bwsprite error-icon errorIcon"></span>
                                <div class="bw-blackbg-tooltip errorText">Please enter mobile number</div>
                            </div>
                            <div class="clear"></div>
                            <a class="btn btn-orange margin-top30" id="user-details-submit-btn">Next</a>
                        </div>
                        <div class="mobile-verification-container hide">
                            <div class="input-border-bottom"></div>
                            <div class="margin-top20">
                                <p class="font16 confirm-otp-text leftfloat">Please confirm your contact details and enter the OTP for mobile verfication</p>
                                <div class="form-control-box">
                                    <input type="text" class="form-control get-otp-code rightfloat" placeholder="Enter OTP" id="getOTP" data-bind="value: viewModel.CustomerVM().otpCode">
                                    <span class="bwsprite error-icon errorIcon hide"></span>
                                    <div class="bw-blackbg-tooltip errorText hide"></div>
                                </div>

                                <div class="clear"></div>
                            </div>
                            <a class="margin-left10 blue rightfloat resend-otp-btn margin-top10" id="resendCwiCode" data-bind="visible: (viewModel.CustomerVM().NoOfAttempts() < 2), click: function () { viewModel.CustomerVM().regenerateOTP() }">Resend OTP</a>
                            <p class="margin-left10 rightfloat otp-notify-text text-light-grey font12 margin-top10" data-bind="visible: (viewModel.CustomerVM().NoOfAttempts() >= 2)">
                             OTP has been already sent to your mobile
                            </p>
                            <div class="clear"></div>
                            <br />
                                <a class="btn btn-orange margin-top30" id="otp-submit-btn">Confirm OTP</a>
                                <div style="margin-right:70px;" id="processing" class="hide"><b>Processing Please wait...</b></div>
                        </div>
                    </div>
                    <div id="customize" class="hide" data-bind="with: viewModel">
                        <p class="font16 margin-bottom20 varient-heading-text">Choose your variant</p>
                        <!-- ko if: viewModel.SelectedVarient() -->
                        <ul class="varientsList" data-bind="foreach: viewModel.Varients()">
                            <li>
                                <div class="grid-6 text-left">
                                    <div data-bind="attr: { class: (minSpec().versionId() == $parent.SelectedVarient().minSpec().versionId()) ? 'selected border-dark varient-item border-solid content-inner-block-10 rounded-corner2' : 'varient-item border-solid content-inner-block-10 rounded-corner2' }, click: function (data, event) { $parent.selectVarient($data, event); }">
                                        <div class="grid-8 alpha">
                                            <h3 class="font16 margin-bottom10" data-bind="text: minSpec().versionName"></h3>
                                            <p class="font14" data-bind="text: minSpec().displayMinSpec"></p>
                                        </div>
                                        <div class="grid-4 omega">
                                            <p class="font18 margin-bottom10"><span class="fa fa-rupee margin-right5"></span><span data-bind="CurrencyText: onRoadPrice"></span></p>
                                            <span data-bind="html: availText"></span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </div>
                            </li>
                        </ul>
                        <!-- /ko -->
                        <div class="clear"></div>
                        <div class="border-solid-top margin-bottom20"></div>
                        <div class="booking-available-colors">
                            <ul data-bind="foreach: viewModel.ModelColors()">
                                <li>
                                    <div class="booking-color-box" data-bind="style: { 'background-color': '#' + hexCode() }, click: function (data,event) { $parent.selectModelColor($data, event); }">
                                        <span class="ticked hide"></span>
                                    </div>
                                    <p class="font16 margin-top20" data-bind="text: colorName"></p>
                                </li>
                            </ul>
                            <div class="clear"></div>
                        </div>
                        <p class="font12 margin-bottom20">* Colours are subject to availabilty, can be selected at the dealership</p>
                        <a class="customize-submit-btn btn btn-orange margin-bottom20" data-bind="click: function () { viewModel.generatePQ(); }">Next</a>
                        <div class="clear"></div>
                        <div class="btn btn-link customizeBackBtn">Back</div>
                    </div>

                    <div id="confirmation" class="hide">
                        <div class="feedback-container">
                            <p class="text-bold font16">Congratulations!</p>
                            <p>Hi <span data-bind="text: viewModel.CustomerVM().fullName"></span></p>
                            <p>you can now book your bike by just paying</p>
                            <!-- ko with: viewModel.SelectedVarient() -->
                            <p class="font20"><span class="fa fa-rupee margin-right5"></span><span class="text-bold font22" data-bind="CurrencyText: bookingAmount"></span></p>
                            <!-- /ko -->
                            <p>
                                You can pay that booking amount using a Credit Card/Debit Card/Net Banking. 
Book your bike online at BikeWale and make the balance payment at the dealership to avail great offers.
For further assistance call on <span class="text-bold">1800 120 8300</span>
                            </p>
                        </div>
                        <asp:Button runat="server" ID="btnMakePayment" class="btn btn-orange margin-top20 margin-bottom10" Text="Pay Now" />
                        <div class="clear"></div>
                        <div class="btn btn-link confirmationBackBtn">Back</div>
                    </div>

                    <button state="personalInfo" id="btnRecommend" class="hide margin-top30 budget-price-next-btn btn btn-orange text-uppercase margin-bottom30">next</button>

                </div>
            </div>
            <div class="clear"></div>
        </section>
        <!--cancellation popup starts here-->
        <div class="bw-popup bw-popup-lg hide cancellation-popup">
            <div class="popup-inner-container">

                <div class="cancel-policy-close-btn position-abt pos-top10 pos-right10 bwsprite cross-lg-lgt-grey cur-pointer"></div>
                <h2>Cancellation & Refund Policy</h2>
                <div class="popup-inner-content cancellation-list margin-top10">
                    <ul>
                        <li><strong>a.</strong> Cancellation must be requested <strong>within 15 calendar days of pre-booking the vehicle.</strong> </li>
                        <li><strong>b.</strong> Please email your <strong>Pre-Booking Cancellation Request'</strong> to <a class="blue" href="mailto:contact@bikewale.com">contact@bikewale.com</a> with a valid reason for cancellation, clearly stating <strong>the booking reference number, your mobile number and email address (that you used while pre-booking).</strong></li>

                        <li><strong>c.</strong> <strong>Cancellation will not be possible if you and dealership have proceeded further with purchase 
                                of the vehicle.</strong> These conditions include payment of additional amount directly to the dealership, 
                                submitting any documents, procurement of the vehicle by the dealership etc.
                        </li>
                        <li><strong>d.</strong> If the dealer has initiated the procurement of the bike upon customer’s pre-booking, cancellation will not be possible.</li>

                        <li><strong>e.</strong> For all valid requests, we will process the refund of full pre-booking amount to customer's account within 7 working days.</li>
                    </ul>
                </div>
            </div>
        </div>
        <!--cancellation popup ends here-->
        <!--required documents popup ends here-->
        <div class="bw-popup bw-popup-lg hide required-doc">
            <div class="popup-inner-container">

                <div class="req-document-close-btn position-abt pos-top10 pos-right10 bwsprite cross-lg-lgt-grey cur-pointer"></div>
                <h2>Required Documents for Registration / Loan</h2>
                <div class="popup-inner-content margin-top10">

                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tbody>
                            <tr>
                                <td valign="top" width="310">
                                    <div class="grey-bullets">
                                        <p><strong>Mandatory Documents:</strong></p>
                                        <ul>
                                            <li>Two Color Photographs.</li>
                                            <li>PAN Card.</li>
                                        </ul>
                                        <p class="margin-top20"><strong>Identity Proof:</strong></p>
                                        <ul>
                                            <li>Passport / Voter ID / Driving License.</li>
                                        </ul>
                                        <p class="margin-top20"><strong>Additional Documents for Loan:</strong></p>
                                        <ul>
                                            <li>Last 6 Months Bank Statement.</li>
                                            <li>Salary Slip / Latest I.T. Return</li>
                                        </ul>
                                    </div>
                                </td>
                                <td>
                                    <div class="grey-bullets res-proof">
                                        <p><strong>Residential Address Proof:</strong></p>
                                        <p class="margin-top10">(Self-Owned House)</p>
                                        <ul>
                                            <li>Light Bil / Passport.</li>
                                            <li>Ration Card (Relation Proof).</li>
                                        </ul>
                                        <p class="margin-top10">(Rented House)</p>
                                        <ul>
                                            <li>Registered Rent Agreement + Police N.O.C.</li>
                                            <li>Rent Home Electricity Bill.</li>
                                            <li>Permanent Address Proof.</li>
                                            <li>Ration Card (Relation Proof).</li>
                                        </ul>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <!--required documents popup ends here-->
        <section>
            <div class="container margin-bottom20">
                <div class="grid-12 alternative-section">
                    <h2 class="text-bold text-center margin-top50 margin-bottom20">What's next!</h2>
                    <div class="content-box-shadow content-inner-block-20">
                        <div class="next-step-box">
                            <img src="../images/next-steps-thumb.jpg" usemap="#nextSteps">
                            <map name="nextSteps">
                                <area shape="rect" coords="424,23,587,72" href="#" target="_blank" id="required-document">
                            </map>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDY0kkJiTPVd2U7aTOAwhc9ySH6oHxOIYM&sensor=false"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/booking.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript">
            //Need to uncomment the below script
            var thisBikename = '<%= this.bikeName %>';
            window.onload = function () {
                var btnRelease = document.getElementById('');
                //Find the button set null value to click event and alert will not appear for that specific button
                function setGlobal() {
                    window.onbeforeunload = null;
                }
                $(btnRelease).click(setGlobal);

                // Alert will not appear for all links on the page
                $("a").click(function () {
                    window.onbeforeunload = null;
                });
                window.onbeforeunload = function () {
                    return "";
                };
            };
        </script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript">
            var viewModel;
            var pqId = '<%= pqId %>'
            var verId = '<%= versionId %>';
            var cityId = '<%= cityId%>';
            var dealerId = '<%= dealerId%>';
            var clientIP = '<%= clientIP %>';
            var pageUrl = '<%= pageUrl%>';
            var areaId = '<%= areaId%>';


            function pageViewModel() {
                var self = this;
                self.page = ko.observable();
            }

            function BookingPageVMModel() {
                var self = this;
                self.Dealer = ko.observable();
                self.SelectedVarient = ko.observable();
                self.ModelColors = ko.observableArray([]);
                self.Varients = ko.observableArray([]);
                self.CustomerVM = ko.observable(new CustomerModel());
                self.SelectedModelColor = ko.observable();
                self.selectModelColor = function (model, event) {
                    var curElement = $(event.currentTarget);
                    self.SelectedModelColor(model);
                    if (!curElement.find('span.ticked').hasClass("selected")) {
                        $('.booking-color-box').find('span.ticked').hide();
                        $('.booking-color-box').find('span.ticked').removeClass("selected");
                        curElement.find('span.ticked').show();
                        curElement.find('span.ticked').addClass("selected");
                    }
                    else {
                        curElement.find('span.ticked').hide();
                        curElement.find('span.ticked').removeClass("selected");
                    }
                }
                self.selectVarient = function (varient, event) {
                    self.SelectedVarient(varient);
                    $(".varient-item").removeClass("border-dark selected");
                    $(event.currentTarget).addClass("border-dark selected");
                    $(".varient-heading-text").removeClass("text-orange");
                }

                self.selectedVersionsCss = ko.computed(function () {
                    var _selectedVersion = verId;
                    var _versionId = ko.utils.arrayFilter(self.Varients(), function (varient) {
                        return varient.minSpec().versionId() == _selectedVersion;
                    });
                    return _versionId ? 'varient-item border-solid content-inner-block-10 border-dark selected' : 'varient-item border-solid content-inner-block-10';

                }, this);
                self.getBookingPage = function () {
                    var bookPage = null;
                    $.getJSON('/api/BikeBookingPage/?versionId=' + verId + '&cityId=' + cityId + '&dealerId=' + dealerId)
                    .done(function (data) {
                        if (data) {
                            bookPage = ko.toJS(data);
                            $.each(bookPage.modelColors, function (key, value) {
                                self.ModelColors.push(new ModelColorsModel(value.id, value.colorName, value.hexCode, value.modelId));
                            });
                            $.each(bookPage.varients, function (key, value) {
                                
                                if (parseInt(verId) == value.minSpec.versionId) {
                                    self.SelectedVarient(
                                        new VarientModel(
                                        value.bookingAmount,
                                        value.hostUrl,
                                        new MakeMdl(value.make.makeId, value.make.makeName, value.make.maskingName),
                                        new VersionMinSpecModel(
                                            value.minSpec.alloyWheels,
                                            value.minSpec.antilockBrakingSystem,
                                            value.minSpec.brakeType,
                                            value.minSpec.electricStart,
                                            value.minSpec.modelName,
                                            value.minSpec.versionName,
                                            value.minSpec.price,
                                            value.minSpec.versionId),
                                        new ModelMdl(value.model.modelId, value.model.modelName, value.model.maskingName),
                                        value.imagePath,
                                        value.noOfWaitingDays,
                                        value.onRoadPrice,
                                        priceList
                                    ));
                                }
                                self.Varients.push(
                                    new VarientModel(
                                        value.bookingAmount,
                                        value.hostUrl,
                                        new MakeMdl(value.make.makeId, value.make.makeName, value.make.maskingName),
                                        new VersionMinSpecModel(
                                            value.minSpec.alloyWheels,
                                            value.minSpec.antilockBrakingSystem,
                                            value.minSpec.brakeType,
                                            value.minSpec.electricStart,
                                            value.minSpec.modelName,
                                            value.minSpec.versionName,
                                            value.minSpec.price,
                                            value.minSpec.versionId),
                                        new ModelMdl(value.model.modelId, value.model.modelName, value.model.maskingName),
                                        value.imagePath,
                                        value.noOfWaitingDays,
                                        value.onRoadPrice,
                                        priceList
                                    ));

                                var priceList = [];
                                $.each(value.priceList, function (key, value) {
                                    priceList.push(new PriceListModel(value.DealerId, value.ItemId, value.ItemName, value.Price));
                                });
                            });                            
                        }
                    });
                }

                self.generatePQ = function () {
                    // Push GA Analytics
                    var cityArea = GetGlobalCityArea();
                    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Booking_Page', 'act': 'Step_2', 'lab': thisBikename + '_' + cityArea });
                    var objPQ =
                    {                        
                        "pqId": pqId,
                        "versionId": self.SelectedVarient().minSpec().versionId()
                    }                    
                    var isSameVersion = getCookie("_MPQ").indexOf("&VersionId=" + self.SelectedVarient().minSpec().versionId() + "&") > 0 ? true : false;
                    if (!isSameVersion) {
                        var cookieValue = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + self.SelectedVarient().minSpec().versionId() + "&DealerId=" + dealerId;
                        SetCookie("_MPQ", cookieValue);
                        $.ajax({
                            type: "POST",
                            url: "/api/UpdatePQ/",
                            data: ko.toJSON(objPQ),
                            async: false,
                            contentType: "application/json",
                            success: function (response) {
                                                                
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                alert("Some error has occured while registering new price quote.");
                                return false;
                            }
                        });
                    }
                    if (self.SelectedModelColor()) {
                        self.updateColor(pqId, self.SelectedModelColor().id());
                    }
                };

                self.updateColor = function (pqId,colorId) {
                    var objPQColor = {
                        "pqId": pqId,
                        "colorId": colorId
                    }
                    $.ajax({
                        type: "POST",
                        url: "/api/PQBikeColor/",
                        async: false,
                        data: ko.toJSON(objPQColor),
                        contentType: "application/json",
                        success: function (response) {
                            var obj = ko.toJS(response);
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            alert("Some error has occured while updating the Bike color.");
                            return false;
                        }
                    });
                }
            }

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
                self.IsVerified = ko.observable();
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
                            "customerName": self.fullName(),
                            "customerMobile": self.mobileNo(),
                            "customerEmail": self.emailId(),
                            "clientIP": clientIP,
                            "pageUrl": pageUrl,
                            "versionId": verId,
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
                                if (self.IsVerified()) {                                    
                                    if (obj.isSuccess && obj.dealer) {
                                        viewModel.Dealer(new DealerModel(
                                            obj.dealer.address1,
                                            obj.dealer.address2,
                                            obj.dealer.area,
                                            obj.dealer.city,
                                            obj.dealer.contactHours,
                                            obj.dealer.emailid,
                                            obj.dealer.faxNo,
                                            obj.dealer.firstName,
                                            obj.dealer.id,
                                            obj.dealer.lastName,
                                            obj.dealer.lattitude,
                                            obj.dealer.longitude,
                                            obj.dealer.mobileNo,
                                            obj.dealer.organization,
                                            obj.dealer.phoneNo,
                                            obj.dealer.pincode,
                                            obj.dealer.state,
                                            obj.dealer.websiteUrl));
                                    }
                                }
                                else {
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
                            "customerMobile": self.mobileNo(),
                            "customerEmail": self.emailId(),
                            "cwiCode": self.otpCode(),
                            "branchId": dealerId,
                            "customerName": self.fullName(),
                            "versionId": verId,
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
                                if (obj.isSuccess && obj.dealer) {
                                    viewModel.Dealer(new DealerModel(
                                        obj.dealer.address1,
                                        obj.dealer.address2,
                                        obj.dealer.area,
                                        obj.dealer.city,
                                        obj.dealer.contactHours,
                                        obj.dealer.emailid,
                                        obj.dealer.faxNo,
                                        obj.dealer.firstName,
                                        obj.dealer.id,
                                        obj.dealer.lastName,
                                        obj.dealer.lattitude,
                                        obj.dealer.longitude,
                                        obj.dealer.mobileNo,
                                        obj.dealer.organization,
                                        obj.dealer.phoneNo,
                                        obj.dealer.pincode,
                                        obj.dealer.state,
                                        obj.dealer.websiteUrl));
                                }
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                self.IsVerified(false);
                            }
                        });
                    }
                }
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
                }
                self.fullName = ko.computed(function () {
                    var _firstName = self.firstName() != undefined ? self.firstName() : "";
                    var _lastName = self.lastName() != undefined ? self.lastName() : "";
                    return _firstName + ' ' + _lastName;
                }, this);
            }

            function DealerModel(address1, address2, area, city, contactHours, emailId, faxNo, firstName, id, lastName, lattitude, longitude, mobileNo, organization, phoneNo, pincode, state, websiteUrl) {
                var self = this;
                self.address1 = ko.observable(address1);
                self.address2 = ko.observable(address2);
                self.area = ko.observable(area);
                self.city = ko.observable(city);
                self.contactHours = ko.observable(contactHours);
                self.emailId = ko.observable(emailId);
                self.faxNo = ko.observable(faxNo);
                self.firstName = ko.observable(firstName);
                self.id = ko.observable(id);
                self.lastName = ko.observable(lastName);
                self.lattitude = ko.observable(lattitude);
                self.longitude = ko.observable(longitude);
                self.mobileNo = ko.observable(mobileNo);
                self.organization = ko.observable(organization);
                self.phoneNo = ko.observable(phoneNo);
                self.pincode = ko.observable(pincode);
                self.state = ko.observable(state);
                self.websiteUrl = ko.observable(websiteUrl);                
            }

            function DisclaimerModel(disclaimers) {
                var self = this;
                self.disclaimers = ko.observableArray(disclaimers);
            }

            function ModelColorsModel(id, colorName, hexCode, modelId) {
                var self = this;
                self.id = ko.observable(id);
                self.colorName = ko.observable(colorName);
                self.hexCode = ko.observable(hexCode);
                self.modelId = ko.observable(modelId);
            }

            function OfferModel(id, text, type, value) {
                var self = this;
                self.id = ko.observable(id);
                self.text = ko.observable(text);
                self.type = ko.observable(type);
                self.value = ko.observable(value);
            }

            function VarientModel(bookingAmount, hostUrl, make, minSpec, model, imagePath, noOfWaitingDays, onRoadPrice, priceList) {
                var self = this;
                self.bookingAmount = ko.observable(bookingAmount);
                self.hostUrl = ko.observable(hostUrl);
                self.make = ko.observable(make);
                self.minSpec = ko.observable(minSpec);
                self.model = ko.observable(model);
                self.imagePath = ko.observable(imagePath);
                self.noOfWaitingDays = ko.observable(noOfWaitingDays);
                self.onRoadPrice = ko.observable(onRoadPrice);
                self.priceList = ko.observableArray(priceList);

                self.availText = ko.computed(function () {
                    var _days = self.noOfWaitingDays();
                    var _availText = _days ? '<p class="font12 text-light-grey">Waiting of ' + _days + ' days</p>' : '<p class="font12 text-green text-bold">Now available</p>';
                    return _availText;
                }, this);

                self.bikeName = ko.computed(function () {
                    var _bikeName = '';
                    _bikeName = self.make().makeName() + ' ' + self.model().modelName() + ' ' + self.minSpec().versionName();
                    return _bikeName;
                }, this);
                self.imageUrl = ko.computed(function () {
                    var _imageUrl = '';
                    _imageUrl = self.hostUrl() + '/310x174/' + self.imagePath();
                    return _imageUrl;
                }, this);
                self.remainingAmount = ko.computed(function () {
                    var _remainingAmount = self.onRoadPrice() - self.bookingAmount();
                    return _remainingAmount;
                }, this);
            }

            function VersionMinSpecModel(alloyWheels, antilockBrakingSystem, brakeType, electricStart, modelName, versionName, price, versionId) {
                var self = this;
                self.alloyWheels = ko.observable(alloyWheels);
                self.antilockBrakingSystem = ko.observable(antilockBrakingSystem);
                self.brakeType = ko.observable(brakeType);
                self.electricStart = ko.observable(electricStart);
                self.modelName = ko.observable(modelName);
                self.price = ko.observable(price);
                self.versionId = ko.observable(versionId);
                self.versionName = ko.observable(versionName);
                self.displayMinSpec = ko.computed(function () {
                    var spec = '';
                    spec += (self.alloyWheels() ? "Alloy" : "Spoke") + ' Wheels, ';
                    if (self.brakeType()) {
                        spec += self.brakeType() + ' Brake' + ', ';
                    }
                    spec += ' ' + (self.electricStart() ? ' Electric' : 'Kick') + ' Start, ';
                    if (self.antilockBrakingSystem()) {
                        spec += ' ' + 'ABS' + ', ';
                    }
                    if (spec) {
                        return spec.substring(0, spec.length - 2);
                    }
                    return '';
                }, this);
            }

            function MakeMdl(makeId, makeName, maskingName) {
                var self = this;
                self.makeId = ko.observable(makeId);
                self.makeName = ko.observable(makeName);
                self.maskingName = ko.observable(maskingName);
            }

            function ModelMdl(modelId, modelName, maskingName) {
                var self = this;
                self.maskingName = ko.observable(maskingName);
                self.modelId = ko.observable(modelId);
                self.modelName = ko.observable(modelName);
            }

            function PriceListModel(DealerId, ItemId, ItemName, Price) {
                var self = this;
                self.DealerId = ko.observable(DealerId);
                self.ItemId = ko.observable(ItemId);
                self.ItemName = ko.observable(ItemName);
                self.Price = ko.observable(Price);
            }

            ko.bindingHandlers.googlemap = {
                init: function (element, valueAccessor) {
                    var
                      value = valueAccessor(),
                      latLng = new google.maps.LatLng(value.latitude, value.longitude),
                      mapOptions = {
                          zoom: 10,
                          center: latLng,
                          mapTypeId: google.maps.MapTypeId.ROADMAP
                      },
                      map = new google.maps.Map(element, mapOptions),
                      marker = new google.maps.Marker({
                          position: latLng,
                          map: map
                      });
                }
            };

            ko.bindingHandlers.CurrencyText = {
                update: function (element, valueAccessor) {
                    var amount = valueAccessor();
                    var formattedAmount = ko.unwrap(amount) !== null ? formatPrice(amount()) : 0;
                    $(element).text(formattedAmount);
                }
            };

            function formatPrice(price) {
                price = price.toString();
                var lastThree = price.substring(price.length - 3);
                var otherNumbers = price.substring(0, price.length - 3);
                if (otherNumbers != '')
                    lastThree = ',' + lastThree;
                var price = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
                return price;
            }

            $(document).ready(function () {
                $(document).delegate("#cancellation-box", "click", function (e) {
                    e.preventDefault();
                    $(".blackOut-window").show();
                    $('.cancellation-popup').show();
                });

                $(document).delegate(".cancel-policy-close-btn", "click", function () {
                    ClosePolicyPopUp();
                });

                function ClosePolicyPopUp() {
                    $(".cancellation-popup").hide();
                    unlockPopup();
                }

                $(document).delegate("#required-document", "click", function (e) {
                    e.preventDefault();
                    $(".blackOut-window").show();
                    $('.required-doc').show();
                });

                $(document).delegate(".req-document-close-btn", "click", function () {
                    CloseDocumentPopUp();
                });

                function CloseDocumentPopUp() {
                    $(".required-doc").hide();
                    unlockPopup();
                }

                function unlockPopup() {
                    $('body').removeClass('lock-browser-scroll');
                    $(".blackOut-window").hide();
                }

                $(document).delegate(".blackOut-window", "click", function () {
                    $(".cancellation-popup").hide();
                    $('.required-doc').hide();
                });

                viewModel = new BookingPageVMModel();
                viewModel.getBookingPage();
                ko.applyBindings(viewModel);
            });

        </script>
    </form>
</body>
</html>
