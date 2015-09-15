<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.PriceQuote.BookingSummary_New" %>

<!doctype html>
<html>
<head>
    <%
        title = bikeName + " Booking Summary";
        description = "Authorise dealer price details of a bike " + bikeName;
        keywords = bikeName + ", price, authorised, dealer,Booking ";    
    %>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-booking.css?15sept20151" rel="stylesheet" type="text/css" />
        <section class="container bg-white box-shadow padding-bottom20 margin-bottom10 clearfix">
            <!--  Discover bikes section code starts here -->
            <div class="grid-12">
                <!-- ko with: viewModel.SelectedVarient() -->
                <div class="imageWrapper margin-top10">
                    <img data-bind="attr: { src: imageUrl, alt: bikeName, title: bikeName }">
                </div>
                <div class="margin-top10">
                    <h3 class="margin-bottom15" data-bind="text: bikeName"></h3>
                    <div class="font14">
                        <table>
                            <tbody>
                                <tr>
                                    <td width="200" class="padding-bottom10">On road price:</td>
                                    <td align="right" class="padding-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span><span data-bind="text: onRoadPrice"></span></td>
                                </tr>
                                <tr>
                                    <td>Advance booking:</td>
                                    <td align="right" class="text-bold"><span class="fa fa-rupee margin-right5"></span><span data-bind="text: bookingAmount"></span></td>
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
                                    <td align="right" class="font18 text-bold"><span class="fa fa-rupee margin-right5"></span><span data-bind="text: remainingAmount"></span></td>
                                </tr>
                                <tr>
                                    <td class="font12 text-medium-grey">*Balance amount payable at the dealership</td>
                                    <td></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="clear"></div>
                <!-- /ko -->
            </div>
        </section>

        <section id="offerSection" class="container bg-white box-shadow margin-bottom20 clearfix">
            <!--  Don't know which car to buy section code starts here -->
            <div class="grid-12">
                <h2 class="margin-top30 margin-bottom20 text-center">Avail great offers in 3 simple steps</h2>
                <div class="booking-wrapper">

                    <div class="bike-to-buy-tabs booking-tabs">
                        <ul class="margin-bottom20">
                            <li class="first">
                                <div id="personal-info-tab" class="bike-booking-part active-tab text-bold" data-tabs-buy="personalInfo">
                                    <div class="bike-booking-image">
                                        <span class="booking-sprite buy-icon personalInfo-icon-selected"></span>
                                    </div>
                                </div>
                            </li>
                            <li class="middle">
                                <div id="customize-tab" class="bike-booking-part disabled-tab" data-tabs-buy="customize">
                                    <div class="bike-booking-image">
                                        <span class="booking-sprite buy-icon customize-icon-grey"></span>
                                    </div>
                                </div>
                            </li>
                            <li class="last">
                                <div id="confirmation-tab" class="bike-booking-part disabled-tab" data-tabs-buy="confirmation">
                                    <div class="bike-booking-image">
                                        <span class="booking-sprite buy-icon confirmation-icon-grey"></span>
                                    </div>
                                </div>
                            </li>
                        </ul>
                    </div>

                    <div id="personalInfo" class="padding-bottom20">
                        <p class="font18 text-center">
                            <span class="iconTtl text-bold">Personal Information</span>
                        </p>
                        <p class="font14 text-center margin-top20">Please provide us contact details for your booking</p>
                        <div class="personal-info-form-container margin-top10">
                            <div class="form-control-box personal-info-list">
                                <input type="text" class="form-control get-first-name" placeholder="First name"
                                    id="getFirstName" data-bind="value: viewModel.CustomerVM().firstName">
                                <span class="bwmsprite error-icon"></span>
                                <div class="bw-blackbg-tooltip errorText">Please enter your first name</div>
                            </div>
                            <div class="form-control-box personal-info-list">
                                <input type="text" class="form-control get-last-name" placeholder="Last name"
                                    id="getLastName" data-bind="value: viewModel.CustomerVM().lastName">
                                <span class="bwmsprite error-icon"></span>
                                <div class="bw-blackbg-tooltip errorText">Please enter your last name</div>
                            </div>
                            <div class="form-control-box personal-info-list">
                                <input type="text" class="form-control get-email-id" placeholder="Email address"
                                    id="getEmailID" data-bind="value: viewModel.CustomerVM().emailId">
                                <span class="bwmsprite error-icon"></span>
                                <div class="bw-blackbg-tooltip errorText"></div>
                            </div>
                            <div class="form-control-box personal-info-list">
                                <input type="text" class="form-control get-mobile-no" maxlength="10" placeholder="Mobile no."
                                    id="getMobile" data-bind="value: viewModel.CustomerVM().mobileNo">
                                <span class="bwmsprite error-icon"></span>
                                <div class="bw-blackbg-tooltip errorText">Please enter mobile number</div>
                            </div>
                            <div class="clear"></div>
                            <a class="btn btn-full-width btn-orange margin-top20" id="user-details-submit-btn">Next</a>
                        </div>
                        <div class="mobile-verification-container margin-top20 hide">
                            <p class="font12 text-center margin-bottom10 padding-left15 padding-right15">Please confirm your contact details and enter the OTP for mobile verfication</p>
                            <div class="form-control-box  padding-left15 padding-right15">
                                <input type="text" class="form-control get-otp-code text-center" placeholder="Enter OTP" id="getOTP" data-bind="value: viewModel.CustomerVM().otpCode">
                                <span class="bwmsprite error-icon"></span>
                                <div class="bw-blackbg-tooltip errorText">Please enter a valid OTP</div>
                            </div>
                            <div class="text-center padding-top10">
                                <a class="margin-left10 blue resend-otp-btn margin-top10" id="resendCwiCode" href="#" data-bind="click: function () { viewModel.CustomerVM().regenerateOTP() }">Resend OTP</a><br />
                            </div>
                            <div class="clear"></div>
                            <a class="btn btn-full-width btn-orange margin-top20" id="otp-submit-btn">Confirm</a>
                        </div>
                    </div>

                    <div id="customize" class="hide">
                        <p class="font18 text-center">
                            <span class="iconTtl text-bold">Customize</span>
                        </p>
                        <p class="font14 text-center margin-top20 varient-heading-text">Choose your variant</p>
                        <ul class="varientsList margin-top10" data-bind="foreach: viewModel.Varients()">
                            <li>
                                <div class="clear text-left">
                                    <div class="varient-item border-solid content-inner-block-10 rounded-corner2" data-bind="attr: { class: (minSpec().versionId() == $parent.SelectedVarient().minSpec().versionId()) ? 'selected border-dark varient-item border-solid content-inner-block-10 rounded-corner2' : 'varient-item border-solid content-inner-block-10 rounded-corner2' }, click: function () { $parent.selectVarient($data, event); }">
                                        <div class="grid-12 alpha margin-bottom10">
                                            <h3 class="font16" data-bind="text: minSpec().versionName"></h3>
                                            <p class="font14" data-bind="text: minSpec().displayMinSpec"></p>
                                        </div>
                                        <div class="grid-12 alpha omega">
                                            <p class="font18"><span class="fa fa-rupee margin-right5"></span><span data-bind="text: onRoadPrice"></span></p>
                                            <p class="font12 text-green" data-bind="html: availText"></p>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </div>
                            </li>
                        </ul>
                        <div class="clear"></div>
                        <div class="border-solid-top margin-bottom20"></div>

                        <!-- colours code starts here -->
                        <div class="colours-wrap margin-bottom20">
                            <h2 class="margin-top30 margin-bottom20 text-center">Colours</h2>
                            <div class="jcarousel-wrapper">
                                <div class="jcarousel">
                                    <ul class="text-center" data-bind="foreach: viewModel.ModelColors()">
                                        <li class="available-colors">
                                            <div class="color-box" data-bind="style: { 'background-color': '#' + hexCode() }, click: function () { $parent.selectModelColor($data, event); }"><span class="ticked hide"></span></div>
                                            <p class="font16 text-medium-grey" data-bind="text: colorName"></p>
                                        </li>
                                    </ul>
                                </div>
                                <span class="jcarousel-control-left"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-prev"></a></span>
                                <span class="jcarousel-control-right"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-next"></a></span>
                                <p class="text-center jcarousel-pagination"></p>

                            </div>
                        </div>

                        <p class="font12 margin-bottom20">* Colours are subject to availabilty, can be selected at the dealership</p>
                        <button class="customize-submit-btn btn btn-full-width btn-orange margin-bottom20" data-bind="click: function () { viewModel.generatePQ(); }">Next</button>
                        <div class="clear"></div>
                        <!--<div class="btn btn-link btn-full-width customizeBackBtn">Back</div>-->
                    </div>

                    <div id="confirmation" class="hide">
                        <p class="font18 text-center">
                            <span class="iconTtl text-bold">Confirmation</span>
                        </p>
                        <!--<p class="font14 text-center margin-top20">Choose your variant</p>-->
                        <div class="feedback-container margin-top30">
                            <p class="text-bold font16">Congratulations!</p>
                            <p class="margin-bottom20">Hi <span data-bind="text: viewModel.CustomerVM().fullName"></span></p>
                            <!-- ko with: viewModel.SelectedVarient() -->
                            <p class="margin-bottom15" style="line-height: 22px;">you can now book your bike by just paying <span class="font22"><span class="fa fa-rupee margin-right5"></span><span class="text-bold font24" data-bind="text: bookingAmount"></span></span></p>
                            <!-- /ko -->
                            <p class="margin-bottom15">You can pay that booking amount using a Credit Card/Debit Card/Net Banking.</p>
                            <p>
                                Book your bike online at BikeWale and make the balance payment at the dealership to avail great offers.
                            </p>
                            <p>For further assistance call toll free on <span class="text-bold">1800 456 7890.</span></p>
                        </div>
                        <asp:Button ID="btnMakePayment" class="btn btn-full-width btn-orange margin-top20 margin-bottom10" Text="Pay Now" runat="server" />
                        <div class="clear"></div>
                        <!--<div class="btn btn-full-width btn-link confirmationBackBtn">Back</div>-->
                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <section>
            <!--  toll-free code starts here -->

            <div class="grid-12">
                <!-- ko if : viewModel.Dealer() -->
                <!-- ko with: viewModel.Dealer() -->
                <div class="content-box-shadow content-inner-block-10 bg-white clearfix margin-top20">
                    <div class="font18 text-bold text-black padding-bottom10" data-bind="text: organization()"></div>
                    <div class="font14" data-bind="text: address1() + ' ' + address2() + ', ' + area() + ', ' + city() + ', ' + state() + ', ' + pincode()">
                    </div>
                    <div class="font14 padding-top10"><span class="fa fa-phone"></span><span data-bind="text: phoneNo()"></span></div>
                    <div id="divMapWindow">
                        <div style="width: 290px; height: 90px" data-bind="googlemap: { latitude: lattitude(), longitude: longitude() }"></div>
                    </div>
                </div>
                <!-- /ko -->
                <!-- /ko -->
                <!-- ko if: !viewModel.Dealer() -->
                <div class="container">
                    <div class="grid-12">
                        <div class="content-box-shadow content-inner-block-15 margin-bottom15 text-medium-grey text-center">
                            <a href="tel:02267398888" class="font20 text-grey call-text-green" style="text-decoration: none;"><span class="fa fa-phone text-green margin-right5"></span>1800 457 9781</a>
                        </div>
                    </div>
                </div>
                <!-- /ko -->
            </div>
            <div class="clear"></div>
        </section>

        <section>
            <!--  What next code starts here -->
            <div class="container">
                <div class="grid-12">
                    <h2 class="text-center margin-top30 margin-bottom20">What next!</h2>
                    <div class="what-next-box content-box-shadow content-inner-block-10 margin-bottom15 font16">
                        <ul>
                            <li>
                                <div class="inner-thumb table-cell">
                                    <div class="whatnext-sprite get-dealership"></div>
                                </div>
                                <p class="table-cell text-bold padding-left10">Get in touch with dealership</p>
                            </li>
                            <li>
                                <div class="inner-thumb table-cell">
                                    <div class="whatnext-sprite show-id"></div>
                                </div>
                                <p class="table-cell text-bold padding-left10">Show them your Booking Id</p>
                            </li>
                            <li>
                                <div class="inner-thumb table-cell">
                                    <div class="whatnext-sprite submit-list"></div>
                                </div>
                                <p class="table-cell text-bold padding-left10"><a href="#" id="required-document">Submit list of required documents</a></p>
                            </li>
                            <li>
                                <div class="inner-thumb table-cell">
                                    <div class="whatnext-sprite pay-balance"></div>
                                </div>
                                <p class="table-cell text-bold padding-left10">Pay balance amount & get best deals</p>
                            </li>
                        </ul>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        
        <!--Cancellation & refund policy popup starts here-->
    <div class="bwm-popup cancellation-popup hide">
    	<div class="popup-inner-container">
            <div class="cancel-policy-close-btn position-abt pos-top10 pos-right10 bwmsprite cross-lg-lgt-grey cur-pointer"></div>
            <h3>Cancellation & Refund Policy</h3>
            <div class="lower-alpha-list cancellation-list content-inner-block-10">
                <ol>
                    <li>Cancellation must be requested <strong>within 15 calendar days of pre-booking the vehicle.</strong></li>
		            <li>Please email your <strong>‘Pre-Booking Cancellation Request’</strong> to <a href="mailto:contact@bikewale.com" class="blue-text">contact@bikewale.com</a> with a valid reason for cancellation, clearly stating the <strong>pre-booking reference number, your mobile number and email address (that you used while pre-booking).</strong></li> 	
                    <li><strong>Cancellation will not be possible if you and dealership have proceeded further with purchase of the vehicle.</strong> These conditions include payment of additional amount directly to the dealership, submitting any documents, procurement of vehicle by the dealership etc.</li>
                    <li>If the dealer has initiated the procurement of the bike upon customer’s pre-booking, cancellation will not be possible.</li>
                   
                    <li>For all valid requests, we will process the refund of full pre-booking amount to customer’s account within 7 working days.</li>
                </ol>
            </div>
        </div>
    </div>
    <!--Cancellation & refund policy popup ends here-->
    
    <!--Documents popup starts here-->
    <div class="bwm-popup required-doc hide">
    	<div class="popup-inner-container">
            <div class="req-document-close-btn position-abt pos-top10 pos-right10 bwmsprite cross-lg-lgt-grey cur-pointer"></div>
            <h3 class="margin-bottom20">List of Required Documents</h3>
            <div class="f-bold margin-top-10 text-bold">Mandatory Documents:</div>
            <div class="doc-list">
                <ul>
                    <li>Two Color Photographs.</li>
                    <li>PAN Card.</li>
                </ul>
            </div>
            <div class="f-bold margin-top10 text-bold">Identity Proof:</div>
            <div class="doc-list">
                <ul>
                    <li>Passport / Voter ID / Driving License.</li>
                </ul>
            </div>
            <div class="f-bold margin-top10 text-bold">Additional Documents for Loan:</div>
            <div class="doc-list">
                <ul>
                    <li>Last 6 Months Bank Statement.</li>
                    <li>Salary Slip / Latest I.T. Return.</li>
                </ul>
            </div>
            <div class="f-bold margin-top20 margin-bottom10  text-bold">Residential Address Proof:</div>
            <div class="lightgray">(Self-Owned House)</div>
            <div class="doc-list">
                <ul>
                    <li>Light Bil / Passport.</li>
                    <li>Ration Card (Relation Proof).</li>
                </ul>
            </div>
            <div class="lightgray margin-top15">(Rented House)</div>
            <div class="doc-list">
                <ul>
                    <li>Registered Rent Agreement + Police N.O.C..</li>
                    <li>Rent Home Electricity Bill.</li>
                    <li>Permanent Address Proof.</li>
                    <li>Ration Card (Relation Proof).</li>
                </ul>
            </div>
        </div>
    </div>
    <!--Documents popup ends here-->


        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDY0kkJiTPVd2U7aTOAwhc9ySH6oHxOIYM&sensor=false"></script>
        <script src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/framework/knockout.js" type="text/javascript"></script>
        <script type="text/javascript">
            var pqId = '<%= pqId %>'
            var verId = '<%= versionId %>';
            var cityId = '<%= cityId%>';
            var dealerId = '<%= dealerId%>';
            var clientIP = '<%= clientIP %>';
            var pageUrl = '<%= pageUrl%>';
            var areaId = '<%= areaId%>';
        </script>
        <script src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/bwm-booking.js?sept201515" type="text/javascript"></script>
        <script language="javascript" type="text/javascript">
            window.onload = function () {
                var btnRelease = document.getElementById('<%= clientIP %>');
                //Find the button set null value to click event and alert will not appear for that specific button
                function setGlobal() {
                    window.onbeforeunload = null;
                }
                $(btnRelease).click(setGlobal);

                // Alert will not appear for all links on the page
                $('a').click(function () {
                    window.onbeforeunload = null;

                });
                window.onbeforeunload = function () {
                    return "";
                };
            };


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

            });

        </script>
    </form>
</body>
</html>
