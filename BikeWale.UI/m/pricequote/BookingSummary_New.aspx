﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.PriceQuote.BookingSummary_New" %>

<!doctype html>
<html>
<head>
    <%
        title = bikeName + " Booking Summary";
        description = "Authorise dealer price details of a bike " + bikeName;
        keywords = bikeName + ", price, authorised, dealer,Booking ";    
    %>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-bookingflow.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <script type="text/javascript">$("header").find(".leftfloat .navbarBtn").hide();$("header").find(".rightfloat ").hide();</script>
        <section class="bg-white" id="bookingFlow" style="display: none;" data-bind="visible: true">
            <div class="container margin-bottom20 padding-top20">
                <div class="grid-12 box-shadow padding-bottom20">

                    <div class="booking-wrapper">
                        <div id="configTabsContainer" class="bike-to-buy-tabs booking-tabs">
                            <ul class="margin-bottom20">
                                <li class="first">
                                    <div id="summaryDeatilsTab" class="bike-booking-part active-tab text-bold" data-bind="click: function () { if (CurrentStep() > 1 ) CurrentStep(1); }, css: (CurrentStep() >= 1) ? 'active-tab' : ''">
                                        <div class="bike-booking-image">
                                            <span class="booking-sprite" data-bind="css: (CurrentStep() == 1) ? 'personal-details-icon-selected' : 'booking-tick-blue'"></span>
                                        </div>
                                    </div>
                                </li>
                                <li class="middle">
                                    <div id="deliveryDetailsTab" class="bike-booking-part" data-bind="click: function () { if (CurrentStep() > 2 || ActualSteps() > 1) CurrentStep(2); }, css: (CurrentStep() >= 2 || ActualSteps() > 1) ? 'active-tab' : 'disabled-tab'">
                                        <div class="bike-booking-image">
                                            <span class="booking-sprite" data-bind="css: (CurrentStep() == 2) ? 'summary-icon-selected' : (CurrentStep() > 2 || ActualSteps() > 1) ? 'booking-tick-blue' : 'summary-icon-grey'"></span>
                                        </div>
                                    </div>
                                </li>
                                <li class="last">
                                    <div id="payDetailsTab" class="bike-booking-part disabled-tab" data-bind="click: function () { if ((CurrentStep() < -1) || ActualSteps() < -1) CurrentStep(-1); }, css: (CurrentStep() < -1 || ActualSteps() < -1) ? 'active-tab' : 'disabled-tab'">
                                        <div class="bike-booking-image">
                                            <span class="booking-sprite" data-bind="css: (CurrentStep() == -1) ? 'payment-icon-selected' : (CurrentStep() < -1 || ActualSteps() < -1) ? 'booking-tick-blue' : 'payment-icon-grey'"></span>
                                        </div>
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>

                    <div id="bikeSummary" data-bind="visible: CurrentStep() == 2, with: Bike" style="display: block">
                        <p class="font18 text-center">
                            <span class="iconTtl text-bold">Summary</span>
                        </p>

                        <h1 class="padding-top15 padding-bottom10 padding-right20" data-bind="text : bikeName()"></h1>

                        <div class="imageWrapper">
                            <img data-bind="attr:{src : bikeImageUrl()}">
                        </div>

                        <div class="select-version-container border-light-bottom margin-top20">

                            <div class="clearfix margin-bottom10">
                                <div class="font14 text-medium-grey alpha omega grid-2 margin-top5">Variant:</div>
                                <div class="leftfloat grid-10 omega">
                                    <div class="variant-dropdown margin-bottom10">
                                        <div class="select-dropdown rounded-corner2">
                                            <div class="variant-selected-box">
                                                <span class="leftfloat select-btn font14" data-bind="text : selectedVersion().MinSpec.VersionName,attr:{versionId:selectedVersion().MinSpec.VersionId}"></span>
                                                <span class="clear"></span>
                                            </div>
                                            <span class="upDownArrow rightfloat fa fa-angle-down position-abt pos-right10"></span>
                                        </div>
                                        <div class="select-dropdown-list hide">
                                            <ul>
                                                <asp:Repeater ID="rptVarients" runat="server">
                                                    <ItemTemplate>
                                                        <li versionid="<%#DataBinder.Eval(Container.DataItem,"MinSpec.VersionId") %>" data-bind="click: function () { getVersion(<%#DataBinder.Eval(Container.DataItem,"MinSpec.VersionId") %>); $root.ActualSteps(1); }">
                                                            <p><%#DataBinder.Eval(Container.DataItem,"MinSpec.VersionName") %> </p>
                                                        </li>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="clearfix">
                                <div class="font14 text-medium-grey alpha omega grid-2 margin-top5">Colour:</div>
                                <div class="leftfloat grid-10 omega">
                                    <div class="colour-dropdown">
                                        <div class="select-dropdown rounded-corner2">
                                            <div class="colour-selected-box">
                                                <span class="leftfloat select-color-box rounded-corner2" data-bind="style:{'background-color':('#'+selectedColor().HexCode)}"></span>
                                                <span class="leftfloat select-btn font14" data-bind="text:selectedColor().ColorName"></span>
                                                <span class="clear"></span>
                                            </div>
                                            <span class="upDownArrow rightfloat fa fa-angle-down position-abt pos-right10"></span>
                                        </div>
                                        <div class="select-dropdown-list hide">
                                            <ul data-bind="foreach: versionColors">
                                                <li class="text-light-grey" colorid="" data-bind="attr: { colorId: $data.Id},click: function() { $parent.getColor($data);$root.ActualSteps(1);}">
                                                    <span class="select-color-box rounded-corner2" data-bind="style: { 'background-color': '#' + HexCode}"></span>
                                                    <p data-bind="text: ColorName"></p>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>

                                </div>
                            </div>

                            <div class="font14 text-medium-grey margin-top20 margin-bottom10">
                                <span data-bind="text : displayMinSpec"></span>
                            </div>

                        </div>

                        <div class="customize-onRoadPrice-container margin-top15 clearfix">
                            <div class="grid-12 alpha omega margin-bottom15">
                                <p class="grid-8 text-medium-grey font14 alpha">On-road price<span class="font12 viewBreakupText text-blue text-link">(View breakup)</span>:</p>
                                <p class="grid-4 alpha omega text-right font14">
                                    <!-- ko if : versionPrice() > 0 -->
                                    <span class="fa fa-rupee"></span>
                                    <strong class="font16" data-bind="CurrencyText: (versionPrice()-totalDiscount())"></strong>
                                    <!-- /ko -->
                                    <!-- ko ifnot : (versionPrice() > 0) -->
                                    <strong class="font16">Price unavailable</strong>
                                    <!-- /ko -->
                                </p>
                            </div>

                            <div class="grid-12 alpha omega margin-bottom15">
                                <p class="grid-8 text-medium-grey font14 alpha">Booking amount:</p>
                                <p class="grid-4 alpha omega text-right font14">
                                    <span class="fa fa-rupee"></span>
                                    <strong class="font16" data-bind="CurrencyText: ($root.Bike().bookingAmount()> 0)?$root.Bike().bookingAmount():'Price unavailable'"></strong>
                                </p>
                                <a id="cancellationLink" class="blue-text font12">Hassle-free cancellation</a>
                            </div>

                            <div class="grid-12 alpha omega margin-bottom15">
                                <p class="grid-8 text-medium-grey font14 alpha">Balance amount payable:</p>
                                <p class="grid-4 alpha omega text-right font14">
                                    <span class="fa fa-rupee"></span>
                                    <strong data-bind="CurrencyText: remainingAmount()"></strong>
                                </p>
                            </div>

                            <!-- View BreakUp Popup Starts here-->
                            <div class="breakupPopUpContainer content-inner-block-20 hide" id="breakupPopUpContainer">
                                <div class="breakupCloseBtn position-abt pos-top20 pos-right20 bwmsprite cross-lg-lgt-grey cur-pointer"></div>
                                <div class="breakup-text-container padding-bottom10">
                                    <h3 class="breakup-header font26 margin-bottom20"><span data-bind="text : bikeName()"></span><span class="font14 text-light-grey ">(On road price breakup)</span></h3>
                                    <table id="model-view-breakup" class="font16" width="100%">
                                        <tbody>
                                            <!-- ko foreach: versionPriceBreakUp -->
                                            <tr>
                                                <td width="60%" class="padding-bottom10" data-bind="text: ItemName"></td>
                                                <td align="right" class="padding-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span><span data-bind="CurrencyText: Price"></span></td>
                                            </tr>

                                            <!-- /ko -->
                                            <%if (dealerDetailEntity.objQuotation.discountedPriceList != null && dealerDetailEntity.objQuotation.discountedPriceList.Count > 0)
                                              { %>
                                            <tr>
                                                <td colspan="2">
                                                    <div class="border-solid-top padding-bottom10"></div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="padding-bottom10">Total on road price</td>
                                                <td align="right" class="padding-bottom10 text-bold" style="text-decoration: line-through;"><span class="fa fa-rupee margin-right5"></span><span data-bind="CurrencyText: versionPrice()"></span></td>
                                            </tr>
                                            <!-- ko foreach: discountList -->
                                            <tr>
                                                <td width="350" class="padding-bottom10" data-bind="text: 'Minus ' + CategoryName"></td>
                                                <td align="right" class="padding-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span><span data-bind="CurrencyText: Price"></span></td>
                                            </tr>
                                            <!-- /ko -->
                                            <%} %>
                                            <%--<!-- ko if : isInsuranceFree()  && insuranceAmount() > 0 -->
                                            

                                            <tr>
                                                <td class="padding-bottom10">Minus insurance</td>
                                                <td align="right" class="padding-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span><span data-bind="CurrencyText: insuranceAmount()"></span></td>
                                            </tr>
                                            <!-- /ko -->--%>
                                            <tr>
                                                <td colspan="2">
                                                    <div class="border-solid-top padding-bottom10"></div>
                                                </td>
                                            </tr>
                                            <tr>

                                                <td class="padding-bottom10 text-bold">Total on road price</td>
                                                <td align="right" class="padding-bottom10 font20 text-bold"><span class="fa fa-rupee margin-right5"></span><span data-bind="CurrencyText: (versionPrice() - totalDiscount())"></span></td>

                                            </tr>
                                            <tr>
                                                <td width="60%" colspan="2">
                                                    <div class="border-solid-top padding-bottom10"></div>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <!-- /ko -->

                                </div>
                            </div>
                            <!--View Breakup popup ends here-->

                        </div>

                        <div class="contact-details-container margin-top30">
                            <h3 class="padding-bottom10 border-light-bottom"><span class="fa fa-map-marker text-red"></span> Contact details:</h3>
                            <ul>
                                <li class="text-black">Offers from the nearest dealers<br />
                                    <span class="text-light-grey"><%= dealerAddress %></span>
                                </li>
                                <li class="text-black">Availability<br />
                                    <span class="text-light-grey" data-bind="visible : $root.Bike().waitingPeriod() > 0">Waiting period of <span class="text-default" data-bind="    text : ($root.Bike().waitingPeriod() == 1)?$root.Bike().waitingPeriod() + ' day' : $root.Bike().waitingPeriod() + ' days'"></span></span>
                                    <span class="text-green text-bold" data-bind="visible : $root.Bike().waitingPeriod() < 1">Now available</span>
                                </li>
                            </ul>
                        </div>
                        <div class="offers-container margin-top15 clearfix">
                            <% if (isOfferAvailable)
                               { %>
                            <h3 class="padding-top10 padding-bottom10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() < 1"><span class="bwmsprite offers-icon"></span>Available Offers </h3>
                            <h3 class="padding-top10 padding-bottom10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() > 0"><span class="bwmsprite offers-icon"></span>Pay <span class="fa fa-rupee" style="font-size: 15px"></span><span class="font16" data-bind="    text : $root.Bike().bookingAmount()"></span> to book your bike and get:</h3>

                            <ul class="pay-and-get-ul">
                                <asp:Repeater ID="rptDealerOffers" runat="server">
                                    <ItemTemplate>
                                        <li class="offertxt"><%#DataBinder.Eval(Container.DataItem,"OfferText") %>
                                            <%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "isOfferTerms")) ==  true ? "<span class='tnc' id='"+ DataBinder.Eval(Container.DataItem, "offerId") +"' ><a class='viewterms'>View terms</a></span>" : "" %>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                            <%}
                               else
                               {%>
                            <h3 class="padding-top10 padding-bottom10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() > 0"><span class="bwmsprite offers-icon margin-right5"></span>Pay <span class="fa fa-rupee" style="font-size: 15px"></span><span class="font16" data-bind="    text : $root.Bike().bookingAmount()"></span> to book your bike</h3>
                            <h3 class="padding-top10 padding-bottom10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() < 1"><span class="fa fa-map-marker text-red margin-right5"></span>Dealer's Location</h3>
                            <div class="bikeModel-dealerMap-container margin-left5 margin-top15" style="width: 100%; height: 150px; margin: 10px 0;" data-bind="googlemap: { latitude: $root.Dealer().latitude(), longitude: $root.Dealer().longitude() }"></div>
                            <script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDY0kkJiTPVd2U7aTOAwhc9ySH6oHxOIYM"></script>
                            <% } %>
                        </div>

                        <div class="clear margin-top20">
                            <input type="submit" runat="server" value="Make payment" class="btn btn-orange btn-full-width" id="deliveryDetailsNextBtn" data-bind="click : function(data,event){return $root.bookNow(data,event);}">
                        </div>

                    </div>

                    <div id="deliveryDetails" data-bind="visible: CurrentStep() == 1, css: (CurrentStep() > 1) ? 'active-tab' : ''" class="margin-bottom15" style="display: none">
                        <p class="font18 text-center">
                            <span class="iconTtl text-bold">Personal Details</span>
                        </p>

                        <div class="margin-top20" data-bind="with : Customer">
                            <h3 class="padding-bottom10 border-light-bottom"><span class="bwmsprite personal-info-form-icon"></span>Personal details</h3>
                            <div class="form-control-box margin-top20">
                                <input type="text" class="form-control" placeholder="Name" id="getLeadName" data-bind="textInput : Name">
                                <span class="bwmsprite error-icon" style="display: none;"></span>
                                <div class="bw-blackbg-tooltip errorText" style="display: none;">Please enter your first name</div>
                            </div>

                            <div class="form-control-box margin-top20">
                                <input type="text" class="form-control" placeholder="Email id" id="getEmailID" data-bind="textInput : EmailId">
                                <span class="bwmsprite error-icon" style="display: none;"></span>
                                <div class="bw-blackbg-tooltip errorText" style="display: none;">Please enter email id</div>
                            </div>

                            <div class="form-control-box margin-top20">
                                <span class="mobile-prefix">+91</span>
                                <input type="text" class="form-control padding-left40" maxlength="10" placeholder="Mobile no." id="getMobile" data-bind="textInput : MobileNo">
                                <span class="bwmsprite error-icon" style="display: none;"></span>
                                <div class="bw-blackbg-tooltip errorText" style="display: none;">Please enter mobile number</div>
                            </div>
                        </div>

                        <div class="offers-container margin-top30 margin-bottom30">
                            <% if (isOfferAvailable)
                               { %>
                            <h3 class="padding-top10 padding-bottom10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() < 1"><span class="fa fa-gift margin-right5 font-24"></span>Available Offers </h3>
                            <h3 class="padding-top10 padding-bottom10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() > 0"><span class="bwmsprite offers-icon margin-right5 text-red font-24"></span>Pay <span class="fa fa-rupee" style="font-size: 15px"></span><span class="font16" data-bind="    text : $root.Bike().bookingAmount()"></span> to book your bike and avail </h3>

                            <ul>
                                <asp:Repeater ID="rptDealerFinalOffers" runat="server">
                                    <ItemTemplate>
                                        <li class="offertxt"><%#DataBinder.Eval(Container.DataItem,"OfferText") %>
                                            <%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "isOfferTerms")) ==  true ? "<span class='tnc' id='"+ DataBinder.Eval(Container.DataItem, "offerId") +"' ><a class='viewterms'>View terms</a></span>" : "" %>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                            <%}
                               else
                               {%>
                            <h3 class="padding-top10 padding-bottom10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() > 0"><span class="bwmsprite offers-icon margin-right5 font-24"></span>Pay <span class="fa fa-rupee" style="font-size: 15px"></span><span class="font16" data-bind="    text : $root.Bike().bookingAmount()"></span> to book your bike and avail</h3>
                            <h3 class="padding-top10 padding-bottom10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() < 1"><span class="fa fa-map-marker text-red margin-right5"></span>Get following details on the bike</h3>
                            <ul class="bike-details-ul" style="margin-left: 0; list-style: none">
                                <li>
                                    <span>Offers from the nearest dealers</span>
                                </li>
                                <li>
                                    <span>Waiting period on this bike</span>
                                </li>
                                <li>
                                    <span>Nearest dealership from your place</span>
                                </li>
                            </ul>
                            <% } %>
                        </div>

                        <div id="otpPopup" class="rounded-corner2 bwm-fullscreen-popup text-center" style="display: none;" data-bind="with : Customer">
                            <div class="otpPopup-close-btn position-abt pos-top15 pos-right15 bwmsprite cross-lg-lgt-grey cur-pointer"></div>

                            <p class="font18 margin-top10 margin-bottom5">Verify your mobile number</p>
                            <p class="font14 text-light-grey margin-bottom10">We have sent an OTP on the following mobile number. Please enter that OTP in the box provided below:</p>
                            <div>
                                <div class="lead-mobile-box lead-otp-box-container margin-bottom10 font22" style="display: none;">
                                    <span class="fa fa-phone"></span>
                                    <span class="text-light-grey">+91</span>
                                    <span class="lead-mobile font24" data-bind="text : MobileNo"></span>
                                    <span class="bwmsprite edit-blue-icon edit-mobile-btn"></span>
                                </div>
                                <div class="otp-box lead-otp-box-container" style="display: none;">
                                    <div class="form-control-box margin-bottom10">
                                        <input type="text" class="form-control" placeholder="Enter your OTP" data-bind="textInput : OtpCode" id="getOTP">
                                        <span class="bwmsprite error-icon errorIcon"></span>
                                        <div class="bw-blackbg-tooltip errorText"></div>
                                    </div>
                                    <p class="resend-otp-btn margin-bottom5 text-right" data-bind="visible: (OtpAttempts() < 2), click:regenerateOTP">Resend OTP</p>
                                    <p class=" otp-notify-text text-light-grey font12 " data-bind="visible: (OtpAttempts() >= 2)">
                                        OTP has been already sent to your mobile
                                    </p>
                                    <input type="button" class="btn btn-orange margin-top10" value="Submit OTP" data-bind="click : function(data,event){return validateOTP(data,event);}" runat="server" id="processOTP">
                                </div>
                                <div class="update-mobile-box" style="display: none;">
                                    <div class="form-control-box text-left">
                                        <span class="mobile-prefix">+91</span>
                                        <input type="text" class="form-control padding-left40" placeholder="Mobile no." maxlength="10" id="getUpdatedMobile" data-bind="textInput : MobileNo">
                                        <span class="bwmsprite error-icon errorIcon" style="display: none;"></span>
                                        <div class="bw-blackbg-tooltip errorText" style="display: none;"></div>
                                    </div>
                                    <input type="button" class="btn btn-orange margin-top20" value="Send OTP" data-bind="click : function(data,event){return $root.verifyCustomer(data,event);}" id="generateNewOTP">
                                </div>
                            </div>
                        </div>

                        <div class="clear"></div>
                        <input type="button" value="Next" class="btn btn-orange btn-full-width" id="bikeSummaryNextBtn" data-bind="click : function(data,event){return $root.verifyCustomer(data,event);}" />
                    </div>

                    <div id="payDetails" data-bind="visible: CurrentStep() > 2" style="display: none">
                        <p class="font18 text-center">
                            <span class="iconTtl text-bold">Payment</span>
                        </p>

                    </div>
                </div>
                <div class="clear"></div>

            </div>
        </section>
        <div class="clear"></div>

        <div class="content-box-shadow content-inner-block-15 margin-top15 margin-bottom15 text-medium-grey text-center">
            <p class="text-medium-grey font14 margin-bottom10">In case of queries call us toll-free on:</p>
            <a href="tel:1800 457 9781" class="font20 text-grey call-text-green rounded-corner2" style="text-decoration: none;"><span class="fa fa-phone text-green margin-right5"></span>1800 457 9781</a>
        </div>

        <section class="container margin-bottom30 lazy content-box-shadow booking-how-it-works" data-original="http://img.aeplcdn.com/bikewaleimg/m/images/howItWorks-mobile.png?<%= staticFileVersion %>">
            <div class="grid-12"></div>
            <div class="clear"></div>
        </section>
        <!-- Terms and condition Popup start -->
        <div class="termsPopUpContainer content-inner-block-20 hide" id="termsPopUpContainer">
            <h3>Terms and Conditions</h3>
            <div style="vertical-align: middle; text-align: center;" id="termspinner">
                <%--<span class="fa fa-spinner fa-spin position-abt text-black bg-white" style="font-size: 50px"></span>--%>
                <img src="/images/search-loading.gif" />
            </div>
            <div class="termsPopUpCloseBtn position-abt pos-top10 pos-right10 bwmsprite  cross-lg-lgt-grey cur-pointer"></div>
            <div id="terms" class="breakup-text-container padding-bottom10 font14">
            </div>
        </div>
        <!-- Terms and condition Popup Ends -->
        <!--Cancellation & refund policy popup starts here-->
    <div class="bw-popup cancellation-popup hide">
    	<div class="popup-inner-container">
            <div class="cancellation-close-btn position-abt pos-top10 pos-right10 bwmsprite cross-lg-lgt-grey cur-pointer"></div>
            <h1>Cancellation & Refund Policy</h1>
            <div class="lower-alpha-list">
                <ol>
                    <li><strong>a. </strong>Cancellation must be requested <strong>within 15 calendar days of booking the vehicle.</strong></li>
		            <li><strong>b. </strong>Please email your <strong>‘Booking Cancellation Request’</strong> to <a href="mailto:contact@bikewale.com" class="blue-text">contact@bikewale.com</a> with a valid reason for cancellation, clearly stating the <strong>booking reference number, your mobile number and email address (that you used while booking).</strong></li> 	
                    <li><strong>c. Cancellation will not be possible if you and dealership have proceeded further with purchase of the vehicle.</strong> These conditions include payment of additional amount directly to the dealership, submitting any documents, procurement of vehicle by the dealership etc.</li>
                    <li><strong>d. </strong>If the dealer has initiated the procurement of the bike upon customer’s booking, cancellation will not be possible.</li>
                    <li><strong>e. </strong>For all valid requests, we will process the refund of full booking amount to customer’s account within 7 working days.</li>
                </ol>
            </div>
        </div>
    </div>
    <!--Cancellation & refund policy popup ends here-->
        <section>
            <div class="container margin-top10 margin-bottom30">
                <div id="faqSlug" class="grid-12">
                    <div class="faq-slug-container content-box-shadow content-inner-block-20">
                        <div class="question-icon-container text-center leftfloat">
                            <div class="icon-outer-container rounded-corner50percent">
                                <div class="icon-inner-container rounded-corner50percent">
                                    <span class="bwmsprite question-mark-icon margin-top20"></span>
                                </div>
                            </div>
                        </div>
                        <div class="question-text-container leftfloat padding-left15">
                            <p class="question-title font16 text-bold text-black">Questions?</p>
                            <p class="question-subtitle text-light-grey font14">We’re here to help.<br />Read our <a href="/m/faq.aspx" target="_blank">FAQs</a>, <a href="mailto:contact@bikewale.com">email</a> or call us on <a href="tel:18001208300" class="text-dark-grey">1800 120 8300</a></p>
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <input id="hdnBikeData" type="hidden" value='<%= jsonBikeVarients  %>' />
        <input id="hdnDiscountList" type="hidden" value='<%= new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(dealerDetailEntity.objQuotation.discountedPriceList)%>' />
        <!-- all other js plugins -->
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <script type="text/javascript">
            var pqId = '<%= pqId %>'
            var verId = '<%= versionId %>';
            var cityId = '<%= cityId%>';
            var dealerId = '<%= dealerId%>';
            var clientIP = '<%= clientIP %>';
            var pageUrl = '<%= pageUrl%>';
            var areaId = '<%= areaId%>';
            var abHostUrl = '<%= ConfigurationManager.AppSettings["ABApiHostUrl"]%>';
        </script>
        <script type="text/javascript">
            //Need to uncomment the below script
            var thisBikename = '<%= this.bikeName %>';
            var clientIP = '<%= clientIP %>'; 
            var pageUrl = '<%= pageUrl %>';
            //select bike version
            var bikeVersionId = <%= (objCustomer!=null && objCustomer.SelectedVersionId > 0)?objCustomer.SelectedVersionId:versionId %>;
            $(function () {
                var versionTab = $('#customizeBike');
                $('#customizeBike ul.select-versionUL li').each(function () {
                    if (bikeVersionId === $(this).attr('versionId')) {
                        $(this).removeClass("text-light-grey border-light-grey").addClass("selected-version text-bold border-dark-grey").find("span.radio-btn").removeClass("radio-sm-unchecked").addClass("radio-sm-checked");
                    }
                });
            });

            var versionList = JSON.parse(Base64.decode($("#hdnBikeData").val()));
            var discountDetail = JSON.parse($("#hdnDiscountList").val());
            var preSelectedColor = <%= (objCustomer != null && objCustomer.objColor != null) ? objCustomer.objColor.ColorId : 0 %>;
            var insFree = <%= Convert.ToString(isInsuranceFree).ToLower() %>;          
            var insAmt = <%= insuranceAmount %>;
            var BikeDealerDetails = function () {
                var self = this;
                self.DealerId = ko.observable(<%= dealerId %>);
                self.PQId = ko.observable(<%= pqId %>);
                self.CityId = ko.observable(<%= cityId %>);
                self.IsInsuranceFree = ko.observable(insFree);
                self.InsuranceAmount = ko.observable(insAmt);
                self.latitude = ko.observable(<%= latitude %>);
                self.longitude = ko.observable(<%= longitude %>);
            }
            var getCityArea = GetGlobalCityArea();
        </script>
        <script src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/bwm-bookingflow.js?<%= staticFileVersion %>" type="text/javascript"></script>

        <script type="text/javascript">
            <% if (objCustomer != null && objCustomer.objCustomerBase != null && !String.IsNullOrEmpty(objCustomer.objCustomerBase.CustomerName))
               { %>
            viewModel.Customer().Name('<%= (objCustomer!=null && objCustomer.objCustomerBase!=null &&  !String.IsNullOrEmpty(objCustomer.objCustomerBase.CustomerName))?objCustomer.objCustomerBase.CustomerName:String.Empty %>');
            viewModel.Customer().EmailId('<%= (objCustomer!=null && objCustomer.objCustomerBase!=null &&  !String.IsNullOrEmpty(objCustomer.objCustomerBase.CustomerEmail))?objCustomer.objCustomerBase.CustomerEmail:String.Empty %>');
            viewModel.Customer().MobileNo('<%= (objCustomer!=null && objCustomer.objCustomerBase!=null &&  !String.IsNullOrEmpty(objCustomer.objCustomerBase.CustomerMobile))?objCustomer.objCustomerBase.CustomerMobile:String.Empty %>');
            <% }
               else
               {%>
            var arr = setuserDetails();
            if (arr != null && arr.length > 0) {
                viewModel.Customer().Name(arr[0]);
                viewModel.Customer().EmailId(arr[1]);
                viewModel.Customer().MobileNo(arr[2]);
            }
            <% } %>

            function viewMore(id){
                $(id).closest('li').nextAll('li').toggleClass('hide');
                $(id).text($(id).text() == '(view more)' ? '(view less)' : '(view more)');
            };

        </script>

    </form>
</body>
</html>
