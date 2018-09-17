<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="cancellation.aspx.cs" Inherits="Bikewale.BikeBooking.Cancellation" %>

<!DOCTYPE html>

<html>
<head>
    <title>Hassle-free process to cancel your booked bike | BikeWale</title>
    <%
        isAd970x90Shown = false;
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
        isAd970x90BottomShown = false;
         %>
    <!-- #include file="/UI/includes/headscript.aspx" -->
    <link href="<%= staticUrl  %>/UI/css/cancellation.css?<%= staticFileVersion%>" rel="stylesheet" type="text/css"/>
    
</head>
<body class="bg-light-grey" data-contestslug="true">
    <form id="form1" runat="server">
        <!-- #include file="/UI/includes/headBW.aspx" -->
        <header class="booking-cancellation-banner">    	
            <div class="container">
                <div class="welcome-box">
                    <h1 class="font30 text-uppercase margin-bottom10">Cancellation policy</h1>
                    <p class="font20">Hassle-free process to cancel your booking</p>
                </div>
            </div>
        </header>
    
        <section>
            <div id="cancellationPage" class="container margin-top20 margin-bottom20" style="display: none;" data-bind="visible: true">
                <div class="grid-12">
                    <div id="processResponse" class="content-box-shadow content-inner-block-20 margin-bottom20" <%--data-bind="visible: $root.User().IsCancelled"--%>>
                        <div class="inline-block margin-right30">
                            <div class="icon-outer-container text-center rounded-corner50">
                                <div class="icon-inner-container rounded-corner50">
                                    <span class="bwsprite cancel-policy-lg-icon margin-top25"></span>
                                </div>
                            </div>
                        </div>
                        <div class="inline-block">
                            <h3 class="margin-bottom10">We are sorry. We cannot process your cancellation.</h3>
                            <p class="font14">Kindly refer to our <a href="#" class="text-blue">cancellation policy</a> for more details regarding your bike booking.</p>
                        </div>
                    </div>
                    <div id="cancellationStepsWrapper" class="content-box-shadow content-inner-block-20" <%--data-bind="visible: !$root.User().IsCancelled"--%>>
                        <div class="margin-bottom15">
                            <div class="horizontal-line position-rel margin-auto"></div>
                            <ul>
                                <li>
                                    <div id="userBikeDetailsTab" class="booking-cancel-step leftfloat" data-bind= "css: (CurrentStep() >= 1) ? 'active-tab' : ''">
                                        <p>Enter your details</p>
                                        <div class="cancellation-tabs-image">
                                            <span class="cancellation-sprite booking-config-icon " data-bind="css: (CurrentStep() == 1) ? 'user-details-icon-selected' : 'cancellation-tick-blue'"></span>
                                        </div>
                                    </div>
                                </li>
                                <li>
                                    <div id="cancelBookingDetailsTab" class="booking-cancel-step" data-bind=" css: (CurrentStep() >= 2 || ActualSteps() > 1) ? 'active-tab' : 'disabled-tab'">
                                        <p>Cancel your booking</p>
                                        <div class="cancellation-tabs-image">
                                            <span class="cancellation-sprite booking-config-icon " data-bind="css: (CurrentStep() == 2) ? 'cancellation-icon-selected' : (CurrentStep() > 2 || ActualSteps() > 1) ? 'cancellation-tick-blue' : 'cancellation-icon-grey'"></span>
                                        </div>
                                    </div>
                                </li>
                                <li>
                                    <div id="cancelConfirmationTab" class="booking-cancel-step rightfloat" data-bind=" css: (CurrentStep() >= 3 || ActualSteps() > 2) ? 'active-tab' : 'disabled-tab'">
                                        <p>Confirmation</p>
                                        <div class="cancellation-tabs-image">
                                            <span class="cancellation-sprite booking-config-icon " data-bind="css: (CurrentStep() == 3) ? 'confirmation-icon-selected' : (CurrentStep() > 3 || ActualSteps() > 2) ? 'cancellation-tick-blue' : 'confirmation-icon-grey'"></span>
                                        </div>
                                    </div>
                                </li>
                            </ul>
                        </div>
                
                        <div id="userBikeDetails" data-bind="with:User,visible: CurrentStep() == 1">
                            <div class="form-control-box margin-bottom20">
                                <input type="text" class="form-control" placeholder="Enter your booking ID" id="getBikeBookingId" data-bind="textInput: BookingId">
                                <span class="bwsprite error-icon errorIcon"></span>
                                <div class="bw-blackbg-tooltip errorText">Please enter your booking ID</div>
                            </div>
                            <div class="form-control-box margin-bottom30">
                                <span class="mobile-prefix">+91</span>
                                <input type="text" class="form-control padding-left40" placeholder="Enter your registered mobile no." id="getUserRegisteredNum" maxlength="10" data-bind="textInput: Mobile">
                                <span class="bwsprite error-icon errorIcon"></span>
                                <div class="bw-blackbg-tooltip errorText">Please enter your registered mobile no.</div>
                            </div>
                            <div class="text-center">
                                <input id="userBikeDetailsBtn" type="button" class="btn btn-orange margin-bottom10" data-bind="click: function (data, event) { return $root.User().verifyBooking(data, event); }" value="Submit">
                                <div class="text-red font14 margin-top5 text-bold" data-bind="visible: !IsValidBooking(), text: ErrMessage"></div>
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
                                        <span class="lead-mobile font24" data-bind="text: Mobile()"></span>
                                    </div>
                                    <div class="otp-box lead-otp-box-container">
                                        <div class="form-control-box margin-bottom10">
                                            <input type="text" class="form-control" placeholder="Enter your OTP" id="getOTP" data-bind="textInput: OTPCode">
                                            <span class="bwsprite error-icon errorIcon"></span>
                                            <div class="bw-blackbg-tooltip errorText"></div>
                                        </div>
                                        <p class="clear"></p>
                                       <%-- <p class="resend-otp-btn margin-bottom5" data-bind="visible: (OtpAttempts() < 2), click: regenerateOTP">Resend OTP</p>
                                        <p class=" otp-notify-text text-light-grey font12 " data-bind="visible: (OtpAttempts() >= 2)">
                                            OTP has been already sent to your mobile
                                        </p>--%>
                                        <input type="button" class="btn btn-orange margin-top20" value="Submit OTP" data-bind="click: function (data, event) { return verifyOTP(data, event); }" id="processOTP">
                                    </div>
                                </div>
                            </div>
                        </div>
                    
                        <div id="cancelBookingDetails" data-bind="with: User, visible: CurrentStep() == 2">
                            <div class="grid-6 padding-left20 padding-right20">
                                <p class="font16 text-bold margin-bottom20">Booking information</p>
                                <p class="font14 margin-bottom15 text-light-grey">Booking ID:<span class="margin-left5 text-default font18 text-bold" data-bind="text : BookingId()"></span></p>
                                <p class="font14 margin-bottom10 text-light-grey">Selected bike:<span class="margin-left5 text-default text-bold" data-bind="text: BikeName()"></span></p>
                                <p class="font14 text-light-grey">Booking date:<span class="margin-left5 text-default text-bold" data-bind="text: BookingDate()"></span></p>
                            </div>
                            <div class="grid-6 padding-left40 padding-right20 border-light-left">
                                <p class="font16 text-bold margin-bottom20">Personal information</p>
                                <p class="font14 margin-bottom20 text-light-grey">Name:<span class="margin-left5 text-default text-bold" data-bind="text: Name()"></span></p>
                                <p class="font14 margin-bottom20 text-light-grey">Email ID:<span class="margin-left5 text-default text-bold" data-bind="text: Email()"></span></p>
                                <p class="font14 text-light-grey">Mobile no:<span class="margin-left5 text-default text-bold" data-bind="text: MobileNo()"></span></p>
                            </div>
                            <div class="clear"></div>
                            <div class="text-center margin-top25">
                                <input id="cancelBookingBtn" type="button" class="btn btn-orange margin-bottom10" data-bind="click: function (data, event) { return cancelBooking(data, event); }" value="Cancel my booking">
                            </div>
                        </div>
                    
                        <div id="cancelConfirmation" class="text-center" data-bind="visible: CurrentStep() == 3">
                            <p class="margin-top10 margin-bottom20 font14 text-light-grey">We have initiated your cancellation process. You will be informed shortly on the status of your refund</p>
                            <textarea runat="server" id="FeedBackText" placeholder="Tell us about your experience and help us imporve."></textarea>
                            <div class="text-center margin-top20">
                               <asp:button runat="server" ID="feedbackBtn" class="btn btn-orange margin-bottom10" Text="Done" />
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
                            <li>Cancellation must be requested <strong>within 15 days of booking the vehicle.</strong></li>
                            <li>To cancel the booking, you will have to reach out to the dealership and inform about the cancellation request mentioning booking reference number and your mobile number (that you used while booking).</li>
                            <li> <strong>Cancellation will not be possible if you and dealership have proceeded further with purchase 
                                    of the vehicle.</strong> These conditions include payment of additional amount directly to the dealership, 
                                    submitting any documents, procurement of the vehicle by the dealership etc.
                            </li>
                            <li>If the dealer has initiated the procurement of the bike upon customer’s booking, cancellation will not be possible.</li>

                            <li>For all valid cancellation requests, full booking amount will be refunded back to you by the dealership within 15 working days.</li>
                            <li>Should you have any concerns regarding cancelling your booking, please feel free to write to us at <a href="mailto:contact@bikewale.com">contact@bikewale.com</a>.</li>
                        </ul>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
    
        <%--<section>
            <div class="container margin-bottom30">
                <div class="grid-12">
                    <div class="content-box-shadow content-inner-block-20">
                        <p class="font14 padding-left5"><span class="bwsprite call-icon inline-block margin-right10"></span>In case of any queries feel free to call us on <span class="text-bold font18">(022) 6739 8888</span></p>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>--%>
        <asp:HiddenField ID="hdnBwid" Value="" runat="server" />
        <!-- #include file="/UI/includes/footerBW.aspx" -->
        <!-- #include file="/UI/includes/footerscript.aspx" -->
        <script type="text/javascript" src="<%= staticUrl %>/UI/src/cancellation.js?<%= staticFileVersion %>"></script>
    </form>
</body>
</html>
