<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.BikeBooking.BookingSummary_New" %>

<%@ Register Src="~/controls/UsersTestimonials.ascx" TagPrefix="BW" TagName="UsersTestimonials" %>
<!DOCTYPE html>
<html>
<head>
    <%
        title = bikeName + " Bookingbooking-sprite buy-icon customize-icon-grey Summary";
        description = "Authorise dealer price details of a bike " + bikeName;
        keywords = bikeName + ", price, authorised, dealer,Booking ";
        isAd970x90Shown = false;     
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/booking.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
</head>
<body class="header-fixed-inner">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <script type="text/javascript">$("#header").find(".leftfloat .navbarBtn").hide();$("#header").find(".rightfloat ").hide();</script>
        <section class="bg-light-grey padding-top10">
            <div class="container">
                <div class="grid-12">
                    <%--<h1 class="font30 text-black margin-top10 margin-bottom10">Avail great offers in 3 simple steps</h1>--%>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section class="container" id="bookingFlow" style="display: none;" data-bind="visible: true">
            <div class="grid-12 margin-bottom20">
                <div class="content-box-shadow content-inner-block-20 rounded-corner2">
                    <div id="bookingTabsContainer" class="margin-bottom15">
                        <div class="horizontal-line position-rel margin-auto"></div>
                        <ul>
                            <li>
                                <div id="bikeSummaryTab" class="bike-book-step leftfloat" data-bind="click: function () { if (CurrentStep() > 1 ) CurrentStep(1); }, css: (CurrentStep() >= 1) ? 'active-tab' : ''">
                                    <p>Personal Details</p>
                                    <div class="booking-tabs-image">
                                        <span class="booking-flow-sprite booking-tab-icon" data-bind="css: (CurrentStep() == 1) ? 'personal-details-icon-selected' : 'booking-tick-blue'"></span>
                                    </div>
                                </div>
                            </li>
                            <li>
                                <div id="deliveryDetailsTab" class="bike-book-step" data-bind="click: function () { if (CurrentStep() > 2 || ActualSteps() > 1) CurrentStep(2); }, css: (CurrentStep() >= 2 || ActualSteps() > 1) ? 'active-tab' : 'disabled-tab'">
                                    <p>Summary</p>
                                    <div class="booking-tabs-image">
                                        <span class="booking-flow-sprite booking-tab-icon " data-bind="css: (CurrentStep() == 2) ? 'summary-icon-selected' : (CurrentStep() > 2 || ActualSteps() > 1) ? 'booking-tick-blue' : 'summary-icon-grey'"></span>
                                    </div>
                                </div>
                            </li>
                            <li>
                                <div id="bikePaymentTab" class="bike-book-step rightfloat" data-bind="click: function () { if ((CurrentStep() < -1) || ActualSteps() < -1) CurrentStep(-1); }, css: (CurrentStep() < -1 || ActualSteps() < -1) ? 'active-tab' : 'disabled-tab'">
                                    <p>Payment</p>
                                    <div class="booking-tabs-image">
                                        <span class="booking-flow-sprite booking-tab-icon " data-bind="css: (CurrentStep() == -1) ? 'payment-icon-selected' : (CurrentStep() < -1 || ActualSteps() < -1) ? 'booking-tick-blue' : 'payment-icon-grey'"></span>
                                    </div>
                                </div>
                            </li>
                        </ul>
                    </div>

                    <div id="deliveryDetails" data-bind="visible: CurrentStep() == 1" style="display: block">
                        <div class="contact-offers-container border-light-bottom padding-bottom20">
                            <div class="grid-6 alpha" data-bind="with : Customer">
                                <h3 class="padding-bottom10 margin-right10 border-light-bottom"><span class="bwsprite personal-details-icon margin-right5"></span>Personal details:</h3>
                                <div class="person-details-form-wrapper">
                                    <div class="form-control-box margin-top20 margin-bottom20">
                                        <input type="text" class="form-control" placeholder="Name" id="getLeadName" data-bind="textInput : Name">
                                        <span class="bwsprite error-icon errorIcon" style="display: none;"></span>
                                        <div class="bw-blackbg-tooltip errorText" style="display: none;">Please enter your name</div>
                                    </div>
                                    <div class="form-control-box email-box margin-right20 leftfloat">
                                        <input type="text" class="form-control" placeholder="Email id" id="getEmailID" data-bind="textInput : EmailId">
                                        <span class="bwsprite error-icon errorIcon" style="display: none;"></span>
                                        <div class="bw-blackbg-tooltip errorText" style="display: none;">Please enter your email</div>
                                    </div>
                                    <div class="form-control-box mobile-box leftfloat">
                                        <span class="mobile-prefix">+91</span>
                                        <input type="text" class="form-control padding-left40" placeholder="Mobile no." maxlength="10" id="getMobile" data-bind="textInput : MobileNo" />
                                        <span class="bwsprite error-icon errorIcon" style="display: none;"></span>
                                        <div class="bw-blackbg-tooltip errorText" style="display: none;">Please enter your mobile no.</div>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </div>
                            <div class="grid-6 omega border-light-left contact-details-container">
                                <% if (isOfferAvailable)
                                   { %>
                                <h3 class="padding-left5 padding-bottom10 margin-left10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() < 1"><span class="fa fa-gift margin-right5 font-24"></span>Book online and avail </h3>
                                <h3 class="padding-left5 padding-bottom10 margin-left10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() > 0"><span class="bwsprite offers-icon margin-right5 text-red font-24"></span>Pay <span class="fa fa-rupee margin-right5" style="font-size: 15px"></span><span class="font16 margin-right5" data-bind="    text : $root.Bike().bookingAmount()"></span>to book your bike and avail</h3>

                                <ul>
                                    <asp:Repeater ID="rptDealerFinalOffers" runat="server">
                                        <ItemTemplate>
                                            <li class="offertxt"><%#DataBinder.Eval(Container.DataItem,"OfferText") %>
                                                <%--<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "isOfferTerms")) ==  true ? "<span class='tnc' id='"+ DataBinder.Eval(Container.DataItem, "offerId") +"' ><a class='viewterms'>View terms</a></span>" : "" %>--%>
                                                <%# "<span class='tnc' id='"+ DataBinder.Eval(Container.DataItem, "offerId") +"' ><a class='viewterms'>View terms</a></span>"  %>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                                <%}
                                   else
                                   {%>
                                <h3 class="padding-left5 padding-bottom10 margin-left10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() > 0"><span class="bwsprite offers-icon margin-right5 font-24"></span>Pay <span class="fa fa-rupee margin-right5" style="font-size: 15px"></span><span class="font16 margin-right5" data-bind="    text : $root.Bike().bookingAmount()"></span>to book your bike</h3>
                                <h3 class="padding-bottom10 padding-left5 margin-right20 border-light-bottom margin-bottom20" data-bind="visible : $root.Bike().bookingAmount() < 1"><span class="fa fa-map-marker text-red margin-right5"></span>Get following details on the bike</h3>
                                <ul>
                                    <li>Offers from the nearest dealers</li>
                                    <li>Waiting period on this bike at the dealership</li>
                                    <li>Nearest dealership from your place</li>
                                </ul>
                                <% } %>
                            </div>
                            <div class="clear"></div>

                            <div id="otpPopup" class="rounded-corner2 text-center" style="display: none;" data-bind="with : Customer">
                                <div class="otpPopup-close-btn position-abt pos-top10 pos-right10 bwsprite cross-lg-lgt-grey cur-pointer"></div>
                                <div class="margin-top10 margin-bottom20"><span class="booking-flow-sprite otp-icon"></span></div>
                                <p class="font18 margin-bottom20">Verify your mobile number</p>
                                <p class="font14 text-light-grey margin-bottom20">We have sent an OTP on the following mobile number. Please enter that OTP in the box provided below:</p>
                                <div>
                                    <div class="lead-mobile-box lead-otp-box-container font22" style="display: block;">
                                        <span class="fa fa-phone"></span>
                                        <span class="text-light-grey font24">+91</span>
                                        <span class="lead-mobile font24" data-bind="text : MobileNo"></span>
                                        <span class="bwsprite edit-blue-icon edit-mobile-btn"></span>
                                    </div>
                                    <div class="otp-box lead-otp-box-container" style="display: block;">
                                        <div class="form-control-box margin-bottom10">
                                            <input type="text" class="form-control" placeholder="Enter your OTP" data-bind="textInput : OtpCode" id="getOTP" />
                                            <span class="bwsprite error-icon errorIcon"></span>
                                            <div class="bw-blackbg-tooltip errorText"></div>
                                        </div>
                                        <p class="resend-otp-btn margin-bottom5" data-bind="visible: (OtpAttempts() < 2), click:regenerateOTP">Resend OTP</p>
                                        <p class=" otp-notify-text text-light-grey font12 " data-bind="visible: (OtpAttempts() >= 2)">
                                            OTP has been already sent to your mobile
                                        </p>
                                        <input type="button" class="btn btn-orange margin-top20" value="Submit OTP" data-bind="click : function(data,event){return validateOTP(data,event);}" id="processOTP">
                                    </div>
                                    <div class="update-mobile-box" style="display: none;">
                                        <div class="form-control-box text-left">
                                            <span class="mobile-prefix">+91</span>
                                            <input type="text" class="form-control padding-left40" placeholder="Mobile no." maxlength="10" id="getUpdatedMobile" data-bind="textInput : MobileNo" />
                                            <span class="bwsprite error-icon errorIcon" style="display: none;"></span>
                                            <div class="bw-blackbg-tooltip errorText" style="display: none;">Please enter your Mobile Number</div>
                                        </div>
                                        <input type="button" class="btn btn-orange" value="Send OTP" data-bind="click : function(data,event){return $root.verifyCustomer(data,event);}" id="generateNewOTP">
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="grid-12 alpha margin-top15 query-number-container">
                            <%--<p class="font14 padding-left5 leftfloat"><span class="bwsprite call-icon inline-block margin-right10"></span>In case of any queries feel free to call us on <span class="text-bold font18">1800 120 8300</span></p>--%>
                            <%-- <input type="button" value="Next" class="btn btn-orange rightfloat" id="bikeSummaryNextBtn" data-bind="click: $root.changedSteps">--%>
                            <input type="button" value="Next" class="btn btn-orange rightfloat" id="bikeSummaryNextBtn" data-bind="click : function(data,event){return $root.verifyCustomer(data,event);}" />
                        </div>
                        <div class="clear"></div>

                    </div>

                    <div id="bikeSummary" data-bind="with: Bike,visible: CurrentStep() == 2, css: (CurrentStep() > 1) ? 'active-tab' : ''" class="margin-bottom15" style="display: none">
                        <div class="grid-5 text-center bike-image-wrapper">
                            <img data-bind="attr:{src : bikeImageUrl()}">
                        </div>
                        <div class="grid-7 bike-summary-wrapper omega">
                            <p class="font24 bike-title text-black text-bold margin-bottom10" data-bind="text : bikeName()"></p>
                            <div class="variant-color-wrapper padding-bottom10 border-light-bottom">
                                <div class="bike-variant-wrapper grid-6 alpha omega">
                                    <p class="variant-text text-light-grey margin-right10">Version:</p>
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
                                    <div class="clear"></div>
                                    <p class="variants-feature font14 text-light-grey" data-bind="text : displayMinSpec"></p>
                                </div>
                                <div class="bike-color-wrapper grid-6 alpha omega">
                                    <p class="colour-text text-light-grey margin-left5 margin-right10">Colour:</p>
                                    <div class="colour-dropdown">
                                        <div class="select-dropdown rounded-corner2">
                                            <div class="colour-selected-box">
                                                <span class="leftfloat select-color-box rounded-corner2 " data-bind="foreach : selectedColor().HexCode , css : (selectedColor().HexCode.length >= 3)? 'color-count-three': (selectedColor().HexCode.length ==2)?'color-count-two':'color-count-one' ">
                                                    <span data-bind="style: { 'background-color': '#' + $data }"></span>
                                                </span>
                                                <span class="leftfloat select-btn font14" data-bind="text:selectedColor().ColorName"></span>
                                                <span class="clear"></span>
                                            </div>
                                            <span class="upDownArrow rightfloat fa fa-angle-down position-abt pos-right10"></span>
                                        </div>
                                        <div class="select-dropdown-list hide">
                                            <ul data-bind="foreach: versionColors">
                                                <li class="text-light-grey border-light-grey" colorid="" data-bind="attr: { colorId: $data.ColorId},click: function() { $parent.getColor($data);$root.ActualSteps(1);}">
                                                    <span class="select-color-box rounded-corner2 " data-bind="foreach : HexCode , css : (HexCode.length >= 3)? 'color-count-three': (HexCode.length ==2)?'color-count-two':'color-count-one' ">
                                                        <span data-bind="style: { 'background-color': '#' + $data }"></span>
                                                    </span>
                                                    <p class="color-title-box" data-bind="text: ColorName"></p>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="onRoad-price-wrapper padding-top10">
                                <ul>
                                    <li>
                                        <p>On road price <span class="viewBreakupText text-blue text-link">(View breakup)</span></p>
                                        <div>
                                            <!-- ko if : versionPrice() > 0 -->
                                            <span class="fa fa-rupee"></span>
                                            <span data-bind="CurrencyText: versionPrice() - totalDiscount()"></span>
                                            <!-- /ko -->
                                            <!-- ko ifnot : (versionPrice() > 0) -->
                                            <span>Price unavailable</span>
                                            <!-- /ko -->
                                        </div>
                                    </li>
                                    <li>
                                        <p>Booking amount:</p>
                                        <div>
                                            <span class="fa fa-rupee"></span>
                                            <span data-bind="CurrencyText: ($root.Bike().bookingAmount()> 0)?$root.Bike().bookingAmount():'Price unavailable'"></span>
                                        </div>
                                        <a class='viewBreakupText blue' id="cancellation-box" href="#">Hassle-free cancellation</a>
                                    </li>
                                    <li>
                                        <p>Balance amount payable:</p>
                                        <div>
                                            <span class="fa fa-rupee"></span>
                                            <span data-bind="CurrencyText: remainingAmount()"></span>
                                        </div>
                                    </li>
                                </ul>
                                <div class="clear"></div>
                            </div>


                            <!-- View BreakUp Popup Starts here-->
                            <div class="breakupPopUpContainer content-inner-block-20 hide" id="breakupPopUpContainer">
                                <div class="breakupCloseBtn position-abt pos-top20 pos-right20 bwsprite cross-lg-lgt-grey cur-pointer"></div>
                                <div class="breakup-text-container padding-bottom10">
                                    <h3 class="breakup-header font26 margin-bottom20"><span data-bind="text : bikeName()"></span><span class="font14 text-light-grey ">(On road price breakup)</span></h3>
                                    <table id="model-view-breakup" class="font16">
                                        <tbody>
                                            <!-- ko foreach: versionPriceBreakUp -->
                                            <tr>
                                                <td width="350" class="padding-bottom10" data-bind="text: ItemName"></td>
                                                <td align="right" class="padding-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span><span data-bind="CurrencyText: Price"></span></td>
                                            </tr>
                                            <!-- /ko -->
                                            <%if (dealerDetailEntity != null && dealerDetailEntity.objQuotation != null
                                                   && dealerDetailEntity.objQuotation.discountedPriceList != null && dealerDetailEntity.objQuotation.discountedPriceList.Count > 0)
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
                                            <% } %>
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
                                                <td colspan="2">
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
                        <div class="clear"></div>

                        <div id="dealerDetails" class="border-light-top margin-top10 padding-top20">
                            <div class="contact-offers-container border-light-bottom padding-bottom15">
                                <div class="grid-5 alpha contact-details-container border-light-right">
                                    <h3 class="padding-bottom10 padding-left5 margin-right10 border-light-bottom"><span class="bwsprite contact-icon margin-right5"></span>Dealership details:</h3>
                                    <ul>
                                        <li>
                                            <p class="text-bold">Offers from the nearest dealers</p>
                                            <p class="text-light-grey"><%= dealerAddress %></p>
                                        </li>
                                        <li>
                                            <p class="text-bold">Availability</p>
                                            <p class="text-light-grey" data-bind="visible : $root.Bike().waitingPeriod() > 0">Waiting period of <span class="text-default" data-bind="    text : ($root.Bike().waitingPeriod() == 1)?$root.Bike().waitingPeriod() + ' day' : $root.Bike().waitingPeriod() + ' days'"></span></p>
                                            <p class="text-green text-bold" data-bind="visible : $root.Bike().waitingPeriod() == 0">Now available</p>
                                            <p class="text-red text-bold" data-bind="visible : $root.Bike().waitingPeriod() < 0">Not available</p>
                                        </li>
                                    </ul>
                                </div>
                                <div class="grid-7 omega offer-details-container">
                                    <% if (isOfferAvailable)
                                       { %>
                                    <h3 class="padding-left5 padding-bottom10 margin-left10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() < 1"><span class="fa fa-gift margin-right5 font-24"></span>Available Offers </h3>
                                    <h3 class="padding-left5 padding-bottom10 margin-left10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() > 0"><span class="bwsprite offers-icon margin-right5 text-red font-24"></span>Pay <span class="fa fa-rupee" style="font-size: 15px"></span><span class="font16" data-bind="    text : $root.Bike().bookingAmount()"></span>to book your bike and get:</h3>

                                    <ul>
                                        <asp:Repeater ID="rptDealerOffers" runat="server">
                                            <ItemTemplate>
                                                <li class="offertxt"><%#DataBinder.Eval(Container.DataItem,"OfferText") %>
                                                    <%--offers<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "isOfferTerms")) ==  true ? "<span class='tnc' id='"+ DataBinder.Eval(Container.DataItem, "offerId") +"' ><a class='viewterms'> View terms</a></span>" : "" %>--%>
                                                    <%# "<span class='tnc' id='"+ DataBinder.Eval(Container.DataItem, "offerId") +"' ><a class='viewterms'>View terms</a></span>"  %>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                    <%}
                                       else
                                       {%>
                                    <script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDY0kkJiTPVd2U7aTOAwhc9ySH6oHxOIYM"></script>
                                    <h3 class="padding-left5 padding-bottom10 margin-left10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() > 0"><span class="bwsprite offers-icon margin-right5 font-24"></span>Pay <span class="fa fa-rupee" style="font-size: 15px"></span><span class="font16" data-bind="    text : $root.Bike().bookingAmount()"></span>to book your bike</h3>
                                    <h3 class="padding-bottom10 padding-left5 margin-right20 border-light-bottom margin-bottom20" data-bind="visible : $root.Bike().bookingAmount() < 1"><span class="fa fa-map-marker text-red margin-right5"></span>Dealer's Location</h3>
                                    <div class="bikeModel-dealerMap-container margin-left5 margin-top15" style="width: 400px; height: 150px" data-bind="googlemap: { latitude: $root.Dealer().latitude(), longitude: $root.Dealer().longitude() }"></div>

                                    <% } %>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="grid-12 alpha margin-top15 query-number-container">
                                <%--<p class="font14 padding-left5 leftfloat"><span class="bwsprite call-icon inline-block margin-right10"></span>In case of any queries feel free to call us on <span class="text-bold font18">1800 120 8300</span></p>--%>
                                <%--<input type="button" value="Next" class="btn btn-orange rightfloat" id="bikeSummaryNextBtn" data-bind="click: $root.changedSteps">--%>
                                <input type="submit" runat="server" value="Make payment" class="btn btn-orange rightfloat" id="deliveryDetailsNextBtn" data-bind="click : function(data,event){return $root.bookNow(data,event);}">
                            </div>
                            <div class="clear"></div>
                        </div>
                    </div>

                    <div id="bikePayment" data-bind="visible: CurrentStep() > 5" style="display: none">
                    </div>

                </div>
            </div>
            <div class="clear"></div>


        </section>
        <!-- Terms and condition Popup start -->
        <div class="termsPopUpContainer content-inner-block-20 hide" id="termsPopUpContainer">
            <div class="fixed-close-btn-wrapper">
                    <div class="termsPopUpCloseBtn fixed-close-btn bwsprite cross-lg-lgt-grey cur-pointer"></div>
                </div>
				<h3>Terms and Conditions</h3>
            <div class="hide" class="hide" style="vertical-align: middle; text-align: center;" id="termspinner">
                <img src="/images/search-loading.gif" />
            </div>
            <div id="terms" class="breakup-text-container padding-bottom10 font14">
            </div>
            <div id='orig-terms' class='hide'>
					<h1>Offers and Gifts Promotion Terms and Conditions</h1>
                    <p><strong>Definitions:</strong></p>
                    <p>"BikeWale" refers to Automotive Exchange Private Limited, a private limited company having its head office at 12<sup>th</sup> Floor, Vishwaroop IT Park, Sector 30A, Vashi, Navi Mumbai 400705, India, who owns and operates www.bikewale.com, one of India's leading automotive web portals.</p>
                    <p>"Bike Manufacturer" or "manufacturer" refers to the company that manufactures and / or markets and sells bikes in India through authorised dealers.</p>
                    <p>"Dealership" or "dealer" refers to companies authorised by a Bike Manufacturer to sell their bikes. Each Bike Manufacturer many have more than one Dealership and / or Dealer.</p>
                    <p>"Offer" refers to the promotions, discounts and gifts that are available as displayed on BikeWale.</p>
                    <p>"Buyer" or "user" or "participant" refers to the individual who purchases a Bike and / or avails any of the offers.</p>
                    <p><strong>Offers from Bike Manufacturers and Dealers</strong></p>
                    <p>1. All offers are from Bike manufacturers and / or their dealers, and BikeWale makes no representation or warranty regarding the accuracy, truth, quality, suitability or reliability of such information.</p>
                    <p>2. These terms and conditions are to be read in conjunction with the terms and conditions of the manufacturers / dealers. Please refer to the manufacturers and / or their dealers' websites for a detailed list of terms and conditions that apply to these offers.</p>
                    <p>3. In the event of any discrepancy between the manufacturers / dealers' offer terms and conditions, and the terms and conditions mentioned herewith, the manufacturers / dealers' terms and conditions will apply.</p>
                    <p>4. All questions, clarifications, complaints and any other communication pertaining to these offers should be addressed directly to the manufacturer and / or their dealers. BikeWale will not be able to entertain any communication in this regard.</p>
                    <p>5. The offers may be modified and / or withdrawn by manufacturers and / or their dealers without notice, and buyers are strongly advised to check the availability and detailed terms and conditions of the offer before making a booking.</p>
                    <p>6. Buyers are strongly advised to verify the offer details with the manufacturer and / or the nearest dealer before booking the bike.</p>
                    <p>7. Any payments made towards purchase of the Bike are governed by the terms and conditions agreed between the buyer and the manufacturer and / or the dealer. BikeWale is in no way related to the purchase transaction and cannot be held liable for any refunds, financial loss or any other liability that may arise directly or indirectly out of participating in this promotion.</p>
                    <p><strong>Gifts from BikeWale</strong></p>
                    <p>8. In select cases, BikeWale may offer a limited number of free gifts to buyers, for a limited period only, over and above the offers from Bike manufacturers and / or their dealers. The quantity and availability period (also referred to as 'promotion period' hereafter) will be displayed prominently along with the offer and gift information on www.bikewale.com.</p>
                    <p>9. These free gifts are being offered solely by BikeWale, and entirely at BikeWale's own discretion, without any additional charges or fees to the buyer.</p>
                    <p>10. In order to qualify for the free gift, the buyer must fulfil the following:</p>
                    <div class="margin-left20 margin-top10">
                        <p>a. Be a legally recognised adult Indian resident, age eighteen (18) years or above as on 01 Dec 2014, and be purchasing the Bike in their individual capacity</p>
                        <p>b. Visit www.bikewale.com and pay the booking amount online against purchase of selected vehicle from BikeWale’s assigned dealer.</p>
                        <p>c. Complete all payment formalities and take delivery of the bike from the same dealership. </p>
                        <p>d. Inform BikeWale through any of the means provided about the completion of the delivery of the bike.</p>
                        
                    </div>
                    <p>11. By virtue of generating an offer code and / or providing BikeWale with Bike booking and / or delivery details, the buyer agrees that s/he is:</p>
                    <div class="margin-left20 margin-top10">
                        <p>a. Confirming his/her participation in this promotion; and</p>
                        <p>b. Actively soliciting contact from BikeWale and / or Bike manufacturers and / or dealers; and</p>
                        <p>c. Expressly consenting for BikeWale to share the information they have provided, in part or in entirety, with Bike manufacturers and / or dealers, for the purpose of being contacted by them to further assist in the Bike buying process; and</p>
                        <p>d. Expressly consenting to receive promotional phone calls, emails and SMS messages from BikeWale, Bike manufacturers and / or dealers; and</p>
                        <p>e. Expressly consenting for BikeWale to take photographs and record videos of the buyer and use their name, photographs, likeness, voice and comments for advertising, promotional or any other purposes on any media worldwide and in any way as per BikeWale's discretion throughout the world in perpetuity without any compensation to the buyer whatsoever; and</p>
                        <p>f. Confirming that, on the request of BikeWale, s/he shall also make arrangements for BikeWale to have access to his / her residence, work place, favourite hangouts, pets etc. and obtain necessary permissions from his / her parents, siblings, friends, colleagues to be photographed, interviewed and to record or take their photographs, videos etc. and use this content in the same manner as described above; and</p>
                        <p>g. Hereby agreeing to fully indemnify BikeWale against any claims for expenses, damages or any other payments of any kind, including but not limited to that arising from his / her actions or omissions or arising from any representations, misrepresentations or concealment of material facts; and</p>
                        <p>h. Expressly consenting that BikeWale may contact the Bike manufacturer and / or dealer to verify the booking and / or delivery details provided by the buyer; and</p>
                        <p>i. Waiving any right to raise disputes and question the process of allocation of gifts</p>
                    </div>
                    <p>12. Upon receiving complete booking and delivery details from the buyer, BikeWale may at its own sole discretion verify the details provided with the Bike manufacturer and / or dealer. The buyer will be eligible for the free gift only if the details can be verified as matching the records of the manufacturer and / or dealer.</p>
                    <p>13. The gifts will be allocated in sequential order at the time of receiving confirmed booking details. Allocation of a gift merely indicates availability of that specific gift for the selected Bike at that specific time, and does not guarantee, assure or otherwise entitle the buyer in any way whatsoever to receive the gift. Allocation of gifts will be done entirely at BikeWale's own sole discretion. BikeWale may change the allocation of gifts at their own sole discretion without notice and without assigning a reason.</p>
                    <p>14. The quantity of gifts available, along with the gift itself, varies by Bike and city. The availability of gifts displayed on www.bikewale.com is indicative in nature. Buyers are strongly advised to check availability of gifts by contacting BikeWale via phone before booking the bike.</p>
                    <p>15. The gift will be despatched to buyers only after the dealer has confirmed delivery of the bike.</p>
                    <p>16. Gifts will be delivered to addresses in India only. In the event that delivery is not possible at certain locations, BikeWale may at its own sole discretion, accept an alternate address for delivery, or arrange for the gift to be made at the nearest convenient location for the buyer to collect.</p>
                    <p>17. Ensuring that the booking and / or delivery information reaches BikeWale in a complete and timely manner is entirely the responsibility of the buyer, and BikeWale, Bike manufacturers, dealers and their employees and contracted staff cannot be held liable for incompleteness of information and / or delays of any nature under any circumstances whatsoever.</p>
                    <p>18. The buyer must retain the offer code, booking confirmation form, invoice of the bike, and delivery papers provided by the dealer, and provide any or all of the same on demand along with necessary identity documents and proof of age. BikeWale may at its own sole discretion declare a buyer ineligible for the free gift in the event the buyer is not able to provide / produce any or all of the documents as required.</p>
                    <p>19. In the event of cancellation of a booking, or if the buyer fails to take delivery of the Bike for any reason, the buyer becomes ineligible for the gift.</p>
                    <p>20. BikeWale's sole decision in all matters pertaining to the free gift, including the choice and value of product, is binding and non-contestable in all respects.</p>
                    <p>21. The buyer accepts and agrees that BikeWale, Bike manufacturers, dealers and other associates of BikeWale, including agencies and third parties contracted by BikeWale, and / or their directors, employees, officers, affiliates or subsidiaries, cannot be held liable for any damage or loss, including but not limited to lost opportunity, lost profit, financial loss, bodily harm, injuries or even death, directly or indirectly, arising out of the use or misuse of the gift, or a defect of any nature in the gift, or out of participating in this promotion in any way whatsoever.</p>
                    <p>22. The buyer specifically agrees not to file in person / through any family member and / or any third party any applications, criminal and/or civil proceedings in any courts or forum in India against BikeWale, Bike manufacturers, dealers and other associates of BikeWale, including agencies and third parties contracted by BikeWale, and/or their directors, employees, officers, affiliates or subsidiaries, and / or their directors, employees, officers, affiliates or subsidiaries to claim any damages or relief in connection with this promotion.</p>
                    <p>23. All gifts mentioned, including the quantity available, are indicative only. Pictures are used for representation purposes only and may not accurately depict the actual gift.</p>
                    <p>24. BikeWale reserves the right to substitute any gift with a suitable alternative or provide gift vouchers of an equivalent value to the buyer, without assigning a reason for the same. Equivalent value of the gift shall be determined solely by BikeWale, irrespective of the market / retail / advertised prices or Maximum Retail Price (MRP) of the product at the time of despatch of the gift. An indicative “gift value” table is provided below.</p>
                    <p>25. Delivery of the product shall be arranged through a third party logistics partner and BikeWale is in no way or manner liable for any damage to the product during delivery.</p>
                    <p>26. Warranty on the gift, if any, will be provided as per the gift manufacturer's terms and directly by the gift manufacturer.</p>
                    <p>27. Gifts cannot be transferred or redeemed / exchanged for cash.</p>
                    <p>28. Income tax, gift tax and / or any other statutory taxes, duties or levies as may be applicable from time to time, arising out of the free gifts, shall be payable entirely by the buyer on his/her own account.</p>
                    <p>29. BikeWale makes no representation or warranties as to the quality, suitability or merchantability of any of the gifts whatsoever, and no claim or request, whatsoever, in this respect shall be entertained.</p>
                    <p>30. Certain gifts may require the buyer to incur additional expenses such as installation expenses or subscription fees or purchasing additional services, etc. The buyer agrees to bear such expenses entirely on their own account.</p>
                    <p>31. Availing of the free gift and offer is purely voluntary. The buyer may also purchase the Bike without availing the free gift and / or the offer.</p>
                    <p>32. For the sake of clarity it is stated that the Bike manufacturer and / or dealer shall not be paid any consideration by BikeWale to display their offers and / or offer free gifts for purchasing bikes from them. Their only consideration will be the opportunity to sell a Bike to potential Bike buyers who may discover their offer on www.bikewale.com.</p>
                    <p>33. Each buyer is eligible for only one free gift under this promotion, irrespective of the number of bikes they purchase.</p>
                    <p>34. This promotion cannot be used in conjunction with any other offer, promotion, gift or discount scheme.</p>
                    <p>35. In case of any dispute, BikeWale's decision will be final and binding and non-contestable. The existence of a dispute, if any, does not constitute a claim against BikeWale.</p>
                    <p>36. This promotion shall be subject to jurisdiction of competent court/s at Mumbai alone.</p>
                    <p>37. Employees of BikeWale and their associate / affiliate companies, and their immediate family members, are not eligible for any free gifts under this promotion.</p>
                    <p>38. This promotion is subject to force majeure circumstances i.e. Act of God or any circumstances beyond the reasonable control of BikeWale.</p>
                    <p>39. Any and all information of the buyers or available with BikeWale may be shared with the government if any authority calls upon BikeWale / manufacturers / dealers to do so, or as may be prescribed under applicable law.</p>
                    <p>40. In any case of any dispute, inconvenience or loss, the buyer agrees to indemnify BikeWale, its representing agencies and contracted third parties without any limitation whatsoever.</p>
                    <p>41. The total joint or individual liability of BikeWale, its representing agencies and contracted third parties, along with Bike manufacturers and dealers, will under no circumstances exceed the value of the free gift the buyer may be eligible for.</p>
                    <p>42. BikeWale reserves the right to modify any and all of the terms and conditions mentioned herein at its own sole discretion, including terminating this promotion, without any notice and without assigning any reason whatsoever, and the buyers agree not to raise any claim due to such modifications and / or termination.</p>
                    <p>By participating in this promotion, the buyer / user agrees to the terms and conditions above in toto.</p>
					</div>
        </div>
        <!-- Terms and condition Popup Ends -->
        <!--cancellation popup starts here-->
            <div class="bw-popup bw-popup-lg cancellation-popup hide">
                <div class="popup-inner-container">
                    <div class="termsPopUpCloseBtn position-abt pos-top20 pos-right20 bwsprite cross-lg-lgt-grey cur-pointer"></div>
                    <h2>Cancellation & Refund Policy</h2>
                    <div class="popup-inner-content cancellation-list">
                        <ul>
                            <li><strong>a.</strong> Cancellation must be requested <strong>within 15 days of booking the vehicle.</strong></li>
                            <li><strong>b.</strong> To cancel the booking, you will have to reach out to the dealership and inform about the cancellation request mentioning booking reference number and your mobile number (that you used while booking).</li>
                            <li><strong>c.</strong> <strong>Cancellation will not be possible if you and dealership have proceeded further with purchase 
                                    of the vehicle.</strong> These conditions include payment of additional amount directly to the dealership, 
                                    submitting any documents, procurement of the vehicle by the dealership etc.
                            </li>
                            <li><strong>d.</strong> If the dealer has initiated the procurement of the bike upon customer’s booking, cancellation will not be possible.</li>

                            <li><strong>e.</strong> For all valid cancellation requests, full booking amount will be refunded back to you by the dealership within 15 working days.</li>
                            <li><strong>f.</strong> Should you have any concerns regarding cancelling your booking, please feel free to write to us at <a href="mailto:contact@bikewale.com">contact@bikewale.com</a>.</li>
                        </ul>
                    </div>
                </div>
            </div>
        <!--cancellation popup ends here-->

        <section class="container margin-top10 lazy content-box-shadow booking-how-it-works" data-original="http://imgd3.aeplcdn.com/0x0/bw/static/sprites/d/howItWorks.png?<%= staticFileVersion %>">
            <div class="grid-12"></div>
            <div class="clear"></div>
        </section>

        <input id="hdnBikeData" type="hidden" value='<%= jsonBikeVarients  %>' />          

        <section>
            <div id="faqsWraper" class="container margin-bottom30">
                <div class="grid-12">
                    <h2 class="text-bold text-center margin-top40 margin-bottom20 font28">FAQs</h2>
                    <div class="grid-4 content-box-shadow content-inner-block-20">
                        <p class="font16 margin-bottom20">How can I book a bike on BikeWale?</p>
                        <p class="font14 text-light-grey">To book a bike, you have to pay a fixed booking amount online mentioned against the vehicle of your interest. This amount will be adjusted...<a href="/faq.aspx#2" target="_blank">read more</a></p>
                    </div>
                    <div class="grid-4 content-box-shadow content-inner-block-20 margin-left20 margin-right20">
                        <p class="font16 margin-bottom20">Where do I have to pay the balance amount? How much will it be?</p>
                        <p class="font14 text-light-grey">You will pay the balance amount directly to the assigned dealership during your visit to the showroom. The...<a href="/faq.aspx#14" target="_blank">read more</a></p>
                    </div>
                    <div class="grid-4 content-box-shadow content-inner-block-20">
                        <p class="font16 margin-bottom20">How will I get the benefits of the offers?</p>
                        <p class="font14 text-light-grey">Depending upon the offer, you will get the benefit of some offers directly at the dealership, while taking...<a href="/faq.aspx#16" target="_blank">read more</a></p>
                    </div>
                    <div class="clear"></div>
                    <div class="margin-top20 font14 text-center">
                        <p>We’re here to help. Read our <a href="/faq.aspx" target="_blank" class="text-blue">FAQs</a> or <a href="mailto:contact@bikewale.com" target="_blank" >email</a> us</p>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <input id="hdnBikeData" type="hidden" value='<%= new System.Web.Script.Serialization.JavaScriptSerializer().Serialize((objBooking.Varients))%>' />
        <input id="hdnDiscountList" type="hidden" value='<%= new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(dealerDetailEntity.objQuotation.discountedPriceList)%>' />
        <!-- #include file="/includes/footerscript.aspx" -->
        <!-- #include file="/includes/footerBW.aspx" -->

        <script type="text/javascript">
            $(document).ready(function() {
                applyLazyLoad();
            });

            var pqId = '<%= pqId %>'
            var verId = '<%= versionId %>';
            var cityId = '<%= cityId%>';
            var dealerId = '<%= dealerId%>';
            var areaId = '<%= areaId%>';
            //Need to uncomment the below script
            var thisBikename = "<%= this.bikeName %>";
            var clientIP = "<%= clientIP %>"; 
            var pageUrl = "<%= pageUrl %>";
            var abHostUrl = '<%= ConfigurationManager.AppSettings["ABApiHostUrl"]%>';
            
            //select bike version
            var bikeVersionId = "<%= (objCustomer!=null && objCustomer.SelectedVersionId > 0)?objCustomer.SelectedVersionId:versionId %>";
            var versionList = JSON.parse(Base64.decode($("#hdnBikeData").val()));
            var discountDetail = JSON.parse($("#hdnDiscountList").val());
            var preSelectedColor = '<%= (objCustomer != null && objCustomer.objColor != null) ? objCustomer.objColor.ColorId : 0 %>';
            var insFree = <%= Convert.ToString(isInsuranceFree).ToLower() %>;          
            var insAmt = '<%= insuranceAmount %>';
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

        </script>

        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/booking.js?<%= staticFileVersion %>"></script>

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
            var getCityArea = GetGlobalCityArea();
        </script>
    </form>
</body>
</html>
