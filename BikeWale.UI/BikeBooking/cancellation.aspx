﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cancellation.aspx.cs" Inherits="Bikewale.BikeBooking.cancellation" %>

<!DOCTYPE html>

<html>
<head>
    <title></title>
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/cancellation.css?<%= staticFileVersion%>" rel="stylesheet" type="text/css"/>
    <%
        isAd970x90Shown = false;
         %>
</head>
<body class="bg-light-grey">
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <header class="booking-cancellation-banner">    	
            <div class="container">
                <div class="welcome-box">
                    <h1 class="font30 text-uppercase margin-bottom10">Cancellation policy</h1>
                    <p class="font20">Hassle-free process to cancel your booking</p>
                </div>
            </div>
        </header>
    
        <section>
            <div id="cancellationPage" class="container margin-top20 margin-bottom20">
                <div class="grid-12">
                    <div id="processResponse" class="content-box-shadow content-inner-block-20 margin-bottom20">
                        <div class="inline-block margin-right30">
                            <div class="icon-outer-container text-center rounded-corner50">
                                <div class="icon-inner-container rounded-corner50">
                                    <span class="bwsprite cancel-policy-lg-icon margin-top25"></span>
                                </div>
                            </div>
                        </div>
                        <div class="inline-block">
                            <h3 class="margin-bottom10">We are sorry. We cannot process your cancellation.</h3>
                            <p class="font14">Kindly refer to our <a href="" class="text-blue">cancanellation policy</a> for more details regarding your bike booking.</p>
                        </div>
                    </div>
                    <div id="cancellationStepsWrapper" class="content-box-shadow content-inner-block-20">
                        <div class="margin-bottom15">
                            <div class="horizontal-line position-rel margin-auto"></div>
                            <ul>
                                <li>
                                    <div id="userBikeDetailsTab" class="booking-cancel-step leftfloat" data-bind="click: function () { if (CurrentStep() > 1) CurrentStep(1) }, css: (CurrentStep() >= 1) ? 'active-tab' : ''">
                                        <p>Enter your details</p>
                                        <div class="cancellation-tabs-image">
                                            <span class="cancellation-sprite booking-config-icon " data-bind="css: (CurrentStep() == 1) ? 'user-details-icon-selected' : 'cancellation-tick-blue'"></span>
                                        </div>
                                    </div>
                                </li>
                                <li>
                                    <div id="cancelBookingDetailsTab" class="booking-cancel-step" data-bind="click: function () { if (CurrentStep() > 2 || ActualSteps() > 1) CurrentStep(2); }, css: (CurrentStep() >= 2 || ActualSteps() > 1) ? 'active-tab' : 'disabled-tab'">
                                        <p>Cancel your booking</p>
                                        <div class="cancellation-tabs-image">
                                            <span class="cancellation-sprite booking-config-icon " data-bind="css: (CurrentStep() == 2) ? 'cancellation-icon-selected' : (CurrentStep() > 2 || ActualSteps() > 1) ? 'cancellation-tick-blue' : 'cancellation-icon-grey'"></span>
                                        </div>
                                    </div>
                                </li>
                                <li>
                                    <div id="cancelConfirmationTab" class="booking-cancel-step rightfloat" data-bind="click: function () { if ((CurrentStep() > 3) || ActualSteps() > 2) CurrentStep(3); }, css: (CurrentStep() >= 3 || ActualSteps() > 2) ? 'active-tab' : 'disabled-tab'">
                                        <p>Confirmation</p>
                                        <div class="cancellation-tabs-image">
                                            <span class="cancellation-sprite booking-config-icon " data-bind="css: (CurrentStep() == 3) ? 'confirmation-icon-selected' : (CurrentStep() > 3 || ActualSteps() > 2) ? 'cancellation-tick-blue' : 'confirmation-icon-grey'"></span>
                                        </div>
                                    </div>
                                </li>
                            </ul>
                        </div>
                
                        <div id="userBikeDetails" data-bind="visible: CurrentStep() == 1">
                            <div class="form-control-box margin-bottom20">
                                <input type="text" class="form-control" placeholder="Enter your booking ID" id="getBikeBookingId">
                                <span class="bwsprite error-icon errorIcon"></span>
                                <div class="bw-blackbg-tooltip errorText">Please enter your booking ID</div>
                            </div>
                            <div class="form-control-box margin-bottom30">
                                <span class="mobile-prefix">+91</span>
                                <input type="text" class="form-control padding-left40" placeholder="Enter your registered mobile no." id="getUserRegisteredNum" maxlength="10">
                                <span class="bwsprite error-icon errorIcon"></span>
                                <div class="bw-blackbg-tooltip errorText">Please enter your registered mobile no.</div>
                            </div>
                            <div class="text-center">
                                <input id="userBikeDetailsBtn" type="button" class="btn btn-orange margin-bottom10" data-bind="click: function (data, event) { return $root.userBikeDetails(data, event); }" value="Submit">
                            </div>
                            <div id="otpPopup" class="rounded-corner2 text-center">
                                <div class="otpPopup-close-btn position-abt pos-top10 pos-right10 bwsprite cross-lg-lgt-grey cur-pointer"></div>
                                <div class="icon-outer-container text-center rounded-corner50">
                                    <div class="icon-inner-container rounded-corner50">
                                        <span class="bwsprite cancel-policy-lg-icon margin-top25"></span>
                                    </div>
                                </div>
                                <p class="font18 margin-top20 margin-bottom20">Cancellation initiated</p>
                                <p class="font14 text-light-grey margin-bottom20">You have initiated cancellation against your booking. We request you confirm your action</p>
                                <div>
                                    <div class="lead-mobile-box lead-otp-box-container font22" style="display: block;">
                                        <span class="fa fa-phone"></span>
                                        <span class="text-light-grey font24">+91</span>
                                        <span class="lead-mobile font24"></span>
                                        <span class="bwsprite edit-blue-icon edit-mobile-btn"></span>
                                    </div>
                                    <div class="otp-box lead-otp-box-container">
                                        <div class="form-control-box margin-bottom10">
                                            <input type="text" class="form-control" placeholder="Enter your OTP" id="getOTP">
                                            <span class="bwsprite error-icon errorIcon"></span>
                                            <div class="bw-blackbg-tooltip errorText"></div>
                                        </div>
                                        <p class="resend-otp-btn margin-bottom5">Resend OTP</p>
                                        <p class="clear"></p>
                                        <p class="otp-notify-text text-light-grey font12">
                                            OTP has been already sent to your mobile
                                        </p>
                                        <input type="button" class="btn btn-orange margin-top20" value="Submit OTP" id="processOTP" data-bind="click: function (data, event) { return $root.processOTP(data, event); }">
                                    </div>
                                    <div class="update-mobile-box">
                                        <div class="form-control-box text-left">
                                            <span class="mobile-prefix">+91</span>
                                            <input type="text" class="form-control padding-left40" placeholder="Mobile no." maxlength="10" id="getUpdatedMobile">
                                            <span class="bwsprite error-icon errorIcon"></span>
                                            <div class="bw-blackbg-tooltip errorText">Please enter your Mobile Number</div>
                                        </div>
                                        <input type="button" class="btn btn-orange" value="Send OTP" id="generateNewOTP">
                                    </div>
                                </div>
                            </div>
                        </div>
                    
                        <div id="cancelBookingDetails" data-bind="visible: CurrentStep() == 2">
                            <div class="grid-6 padding-left20 padding-right20">
                                <p class="font16 text-bold margin-bottom20">Booking information</p>
                                <p class="font14 margin-bottom15 text-light-grey">Booking ID:<span class="margin-left5 text-default font18 text-bold">BWAB123456</span></p>
                                <p class="font14 margin-bottom10 text-light-grey">Selected bike:<span class="margin-left5 text-default text-bold">TVS Apache RTR 180 ABS</span></p>
                                <p class="font14 text-light-grey">Booking date:<span class="margin-left5 text-default text-bold">2 Jul, 2015</span></p>
                            </div>
                            <div class="grid-6 padding-left40 padding-right20 border-light-left">
                                <p class="font16 text-bold margin-bottom20">Personal information</p>
                                <p class="font14 margin-bottom20 text-light-grey">Name:<span class="margin-left5 text-default text-bold">John Doe</span></p>
                                <p class="font14 margin-bottom20 text-light-grey">Email ID:<span class="margin-left5 text-default text-bold">johndoe@gmail.com</span></p>
                                <p class="font14 text-light-grey">Mobile no:<span class="margin-left5 text-default text-bold">9876543210</span></p>
                            </div>
                            <div class="clear"></div>
                            <div class="text-center margin-top25">
                                <input id="cancelBookingBtn" type="button" class="btn btn-orange margin-bottom10" data-bind="click: function (data, event) { return $root.cancelBooking(data, event); }" value="Cancel my booking">
                            </div>
                        </div>
                    
                        <div id="cancelConfirmation" class="text-center" data-bind="visible: CurrentStep() == 3">
                            <p class="margin-top10 margin-bottom20 font14 text-light-grey">We have intiatied your cancellation process. You will be informed shortly on the status of your refund</p>
                            <textarea runat="server" id="FeedBackText" placeholder="Tell us about your experience and help us imporve."></textarea>
                            <div class="text-center margin-top20">
                                <asp:button runat="server" ID="feedbackBtn" class="btn btn-orange margin-bottom10" OnClick="feedbackBtn_Click" Text="Done" />
                                <%--<input id="feedbackBtn" type="button" class="btn btn-orange margin-bottom10" data-bind="click: function (data, event) { return $root.sendFeedback(data, event); }" value="Done">--%>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
    
        <section>
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div class="cancellation-policy-content content-box-shadow content-inner-block-20">
                        <p class="padding-bottom10 border-light-bottom">
                            <span class="bwsprite cancel-policy-sm-icon margin-left5 inline-block"></span>
                            <span class="inline-block"><strong>Our cancellation policy</strong></span>
                        </p>
                        <ul>
                            <li>Cancellation must be requested within <span class="text-bold text-default">15 calendar days of booking the vehicle</span>.</li>
                            <li>Please provide us with your <span class="text-bold text-default">Booking ID and your mobile number</span> (that you used while booking) to proceed with cancellation).</li>
                            <li><span class="text-bold text-default">Cancellation will not be possible if you and dealership have proceeded further with purchase of the vehicle.</span> These conditions include payment of additional amount directly to the dealership, submitting any documents, procurement of the vehicle by the dealership etc.</li>
                            <li>If the dealer has initiated the procurement of the bike upon customer’s booking, cancellation will not be possible.</li>
                            <li>For all valid requests, we will process the refund of full booking amount to customer's account within 7 working days.</li>
                        </ul>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
    
        <section>
            <div class="container margin-bottom30">
                <div class="grid-12">
                    <div class="content-box-shadow content-inner-block-20">
                        <p class="font14 padding-left5"><span class="bwsprite call-icon inline-block margin-right10"></span>In case of any queries feel free to call us on <span class="text-bold font18">1800 120 8300</span></p>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st.aeplcdn.com" + staticUrl : "" %>/src/cancellation.js?<%= staticFileVersion %>"></script>
    </form>
</body>
</html>
