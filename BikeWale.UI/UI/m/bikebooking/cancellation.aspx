<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="cancellation.aspx.cs" Inherits="Bikewale.Mobile.bikebooking.Cancellation" %>

<!DOCTYPE html>

<html>
<head>
     <title>Hassle-free process to cancel your booked bike | BikeWale</title>
    <!-- #include file="/UI/includes/headscript_mobile.aspx" -->
    <link href="<%= staticUrl  %>/m/css/bwm-cancellation.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
</head>
<body class="bg-light-grey">
    <form id="form1" runat="server">
        <!-- #include file="/UI/includes/headBW_Mobile.aspx" -->
        <section class="clearfix">
            <div class="container text-center">
                <div class="booking-cancel-banner">
                    <h2 class="text-white padding-top25 font24">Cancellation policy</h2>
                    <p class="padding-top5 font16 text-white">Hassle-free process to
                        <br />
                        cancel your booking</p>
                </div>
            </div>
        </section>

        <section id="cancellationPage">
            <div id="processResponse" class="bg-white container margin-bottom20">
                <div class="grid-12 box-shadow">
                    <div class="process-icon-container">
                        <div class="icon-outer-container text-center rounded-corner50percent">
                            <div class="icon-inner-container rounded-corner50percent">
                                <span class="bwmsprite cancel-policy-lg-icon margin-top20"></span>
                            </div>
                        </div>
                    </div>
                    <div class="process-text-container">
                        <p class="font18 margin-bottom10">We are sorry.<br />
                            We cannot process your cancellation.</p>
                        <p class="font14">Kindly refer to our <a href="" class="text-blue">cancellation policy</a> for more details regarding your bike booking.</p>
                    </div>
                </div>
                <div class="clear"></div>
            </div>

            <div id="cancellationStepsWrapper" class="bg-white container margin-bottom20">
                <div class="grid-12 box-shadow">
                    <div class="cancellation-tabs">
                        <ul class="margin-bottom20">
                            <li class="first">
                                <div id="userBikeDetailsTab" class="bike-cancel-part" data-bind="css: (CurrentStep() >= 1) ? 'active-tab' : ''">
                                    <div class="bike-cancel-image">
                                        <span class="cancellation-sprite" data-bind="css: (CurrentStep() == 1) ? 'user-details-icon-selected' : 'cancellation-tick-blue'"></span>
                                    </div>
                                </div>
                            </li>
                            <li class="middle">
                                <div id="cancelBookingDetailsTab" class="bike-cancel-part" data-bind="css: (CurrentStep() >= 2 || ActualSteps() > 1) ? 'active-tab' : 'disabled-tab'">
                                    <div class="bike-cancel-image">
                                        <span class="cancellation-sprite" data-bind="css: (CurrentStep() == 2) ? 'cancellation-icon-selected' : (CurrentStep() > 2 || ActualSteps() > 1) ? 'cancellation-tick-blue' : 'cancellation-icon-grey'"></span>
                                    </div>
                                </div>
                            </li>
                            <li class="last">
                                <div id="cancelConfirmationTab" class="bike-cancel-part" data-bind="css: (CurrentStep() >= 3 || ActualSteps() > 2) ? 'active-tab' : 'disabled-tab'">
                                    <div class="bike-cancel-image">
                                        <span class="cancellation-sprite" data-bind="css: (CurrentStep() == 3) ? 'confirmation-icon-selected' : (CurrentStep() > 3 || ActualSteps() > 2) ? 'cancellation-tick-blue' : 'confirmation-icon-grey'"></span>
                                    </div>
                                </div>
                            </li>
                        </ul>
                    </div>

                    <div id="userBikeDetails" data-bind=" with: User, visible: CurrentStep() == 1">
                        <p class="font18 text-center margin-bottom20">Enter your details</p>
                        <div class="form-control-box margin-bottom20">
                            <input type="text" class="form-control" placeholder="Enter your booking ID" id="getBikeBookingId" data-bind="textInput: BookingId">
                            <span class="bwmsprite error-icon errorIcon"></span>
                            <div class="bw-blackbg-tooltip errorText">Please enter your booking ID</div>
                        </div>
                        <div class="form-control-box margin-bottom20">
                            <span class="mobile-prefix">+91</span>
                            <input type="text" class="form-control padding-left40" placeholder="Enter your registered mobile no." id="getUserRegisteredNum" maxlength="10" data-bind="textInput: Mobile">
                            <span class="bwmsprite error-icon errorIcon"></span>
                            <div class="bw-blackbg-tooltip errorText">Please enter your registered mobile no.</div>
                        </div>

                        <div id="otpPopup" class="rounded-corner2 bwm-fullscreen-popup text-center">
                            <div class="otpPopup-close-btn position-abt pos-top15 pos-right15 bwmsprite cross-lg-lgt-grey cur-pointer"></div>
                            <p class="font18 margin-top10 margin-bottom5">Verify your mobile number</p>
                            <p class="font14 text-light-grey margin-bottom10">We have sent an OTP on the following mobile number. Please enter that OTP in the box provided below:</p>
                            <div>
                                <div class="lead-mobile-box lead-otp-box-container margin-bottom10 font22">
                                    <span class="bwmsprite tel-grey-icon"></span>
                                    <span class="text-light-grey">+91</span>
                                    <span class="lead-mobile font24" data-bind="text: Mobile()"></span>
                                </div>
                                <div class="otp-box lead-otp-box-container">
                                    <div class="form-control-box margin-bottom10">
                                        <input type="text" class="form-control" placeholder="Enter your OTP" id="getOTP" data-bind="textInput: OTPCode">
                                        <span class="bwmsprite error-icon errorIcon"></span>
                                        <div class="bw-blackbg-tooltip errorText"></div>
                                    </div>
                                    <%-- <p class="resend-otp-btn margin-bottom5 text-right">Resend OTP</p>
                                    <p class="otp-notify-text text-light-grey font12">
                                        OTP has been already sent to your mobile
                                    </p>--%>
                                    <input type="button" class="btn btn-orange margin-top10" value="Submit OTP" data-bind="click: function (data, event) { return verifyOTP(data, event); }" id="processOTP">
                                </div>
                            </div>
                        </div>

                        <input id="userBikeDetailsBtn" type="button" class="btn btn-orange btn-full-width margin-bottom10" data-bind="click: function (data, event) { return $root.User().verifyBooking(data, event); }" value="Submit">
                        <div class="text-red font14 margin-top5 text-bold" data-bind="visible: !IsValidBooking(), text: ErrMessage"></div>

                    </div>

                    <div id="cancelBookingDetails" data-bind=" with: User, visible: CurrentStep() == 2">
                        <p class="font18 margin-bottom20 text-center">Cancellation</p>
                        <div class="border-light-bottom margin-bottom15">
                            <p class="font16 text-bold margin-bottom15">Booking information</p>
                            <p class="font14 margin-bottom10 text-light-grey">
                                Booking ID:<span class="margin-left5 text-default font18 text-bold" data-bind="text: BookingId()"></span>
                            <p class="font14 text-light-grey">Selected bike:</p>
                            <p class="margin-bottom10 text-default text-bold" data-bind="text: BikeName()"></p>
                            <p class="font14 text-light-grey">Booking date:</p>
                            <p class="margin-bottom15 text-default text-bold" data-bind="text: BookingDate()"></p>
                        </div>

                        <div class="margin-bottom15">
                            <p class="font16 text-bold margin-bottom15">Personal information</p>
                            <p class="font14 text-light-grey">Name:</p>
                            <p class="margin-bottom15 text-default text-bold" data-bind="text: Name()"></p>
                            <p class="font14 text-light-grey">Email ID:</p>
                            <p class="margin-bottom15 text-default text-bold" data-bind="text: Email()"></p>
                            <p class="font14 text-light-grey">Mobile no:</p>
                            <p class="margin-bottom15 text-default text-bold" data-bind="text: MobileNo()"></p>
                        </div>

                        <input id="cancelBookingBtn" class="btn btn-orange btn-full-width margin-bottom10" type="button" data-bind="click: function (data, event) { return cancelBooking(data, event); }" value="Cancel my booking">
                    </div>

                    <div id="cancelConfirmation" data-bind="visible: CurrentStep() == 3">
                        <p class="font18 margin-bottom20 text-center">Confirmation</p>
                        <p class="font14 text-light-grey margin-bottom20">We have initiated your cancellation process. You will be informed shortly on the status of your refund</p>
                        <textarea runat="server" id="FeedBackText" placeholder="Tell us about your experience and help us imporve."></textarea>
                        <asp:Button runat="server" ID="feedbackBtn" class="btn btn-orange margin-bottom10" Text="Done" />
                        <%--<input id="feedbackBtn" type="button" class="btn btn-orange margin-top20 btn-full-width margin-bottom10" data-bind="click : function(data,event){return $root.sendFeedback(data,event);}" value="Done">--%>
                    </div>

                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div id="cancellationPolicyContent" class="bg-white container margin-bottom20">
                <div class="grid-12 box-shadow">
                    <p class="padding-bottom10 border-light-bottom">
                        <span class="bwmsprite cancel-policy-sm-icon margin-left5 inline-block"></span>
                        <span class="inline-block"><strong>Our cancellation policy</strong></span>
                    </p>
                    <ul>
                        <li>Cancellation must be requested <strong>within 15 days of booking the vehicle.</strong></li>
                        <li>To cancel the booking, you will have to reach out to the dealership and inform about the cancellation request mentioning booking reference number and your mobile number (that you used while booking).</li>
                        <li><strong>Cancellation will not be possible if you and dealership have proceeded further with purchase 
                                    of the vehicle.</strong> These conditions include payment of additional amount directly to the dealership, 
                                    submitting any documents, procurement of the vehicle by the dealership etc.
                        </li>
                        <li>If the dealer has initiated the procurement of the bike upon customer’s booking, cancellation will not be possible.</li>

                        <li>For all valid cancellation requests, full booking amount will be refunded back to you by the dealership within 15 working days.</li>
                        <li>Should you have any concerns regarding cancelling your booking, please feel free to write to us at <a href="mailto:contact@bikewale.com">contact@bikewale.com</a>.</li>
                    </ul>
                </div>
                <div class="clear"></div>
            </div>
        </section>

       <%-- <section>
            <div class="content-box-shadow content-inner-block-15 margin-bottom20 text-medium-grey text-center">
                <p class="text-medium-grey font14 margin-bottom10">In case of queries call us toll-free on:</p>
                <a href="#" class="font20 text-grey call-text-green rounded-corner2" style="text-decoration: none;"><span class="bwmsprite tel-green-icon text-green margin-right5"></span>1800 457 9781</a>
            </div>
        </section>--%>
        <asp:HiddenField ID="hdnBwid" Value="" runat="server" />
        <!-- #include file="/UI/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/UI/includes/footerscript_Mobile.aspx" -->
        <script type="text/javascript" src="<%= staticUrl  %>/UI/m/src/bwm-cancellation.js?<%= staticFileVersion %>"></script>

    </form>
</body>
</html>
