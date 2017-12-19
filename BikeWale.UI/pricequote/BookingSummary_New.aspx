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
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
        isAd970x90BottomShown = false;
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%= staticUrl  %>/css/booking.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
</head>
<body class="header-fixed-inner" data-contestslug="true">
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
                                <h3 class="padding-left5 padding-bottom10 margin-left10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() > 0"><span class="bwsprite offers-icon margin-right5 text-red font-24"></span>Pay <span class="bwsprite inr-lg"></span>&nbsp;<span class="font16 margin-right5" data-bind="    text : $root.Bike().bookingAmount()"></span>&nbsp;to book your bike and avail</h3>

                                <ul>
                                    <asp:Repeater ID="rptDealerFinalOffers" runat="server">
                                        <ItemTemplate>
                                            <li class="offertxt"><%#DataBinder.Eval(Container.DataItem,"OfferText") %>
                                                <%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "isOfferTerms")) ? "<span class='tnc font9' id='"+ DataBinder.Eval(Container.DataItem, "offerId") +"' >View terms</span>" : string.Empty %>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                                <%}
                                   else
                                   {%>
                                <h3 class="padding-left5 padding-bottom10 margin-left10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() < 1"><span class="fa fa-gift margin-right5 font-24"></span>Book online and avail </h3>
                                <h3 class="padding-left5 padding-bottom10 margin-left10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() > 0"><span class="bwsprite offers-icon margin-right5 text-red font-24"></span>Pay <span class="bwsprite inr-lg"></span>&nbsp;<span class="font16 margin-right5" data-bind="    text : $root.Bike().bookingAmount()"></span>&nbsp;to book your bike and avail</h3>
                                <ul>
                                    <li>Offers from the nearest dealers</li>
                                    <li>Waiting period on this bike at the dealership</li>
                                    <li>Nearest dealership from your place</li>
                                </ul>
                                <% } %>
                            </div>
                            <div class="clear"></div>

                            

                        </div>
                        <div class="grid-12 alpha margin-top15 query-number-container">
                            <input type="button" value="Next" class="btn btn-orange rightfloat" id="bikeSummaryNextBtn" leadsourceid="16" data-bind="click : function(data,event){return $root.verifyCustomer(data,event);}" />
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
                                            <span class="upDownArrow rightfloat fa fa-angle-down position-abt pos-top13 pos-right10"></span>
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
                                            <span class="upDownArrow rightfloat fa fa-angle-down position-abt pos-top13 pos-right10"></span>
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
                                            <span class="bwsprite inr-lg"></span>
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
                                            <span class="bwsprite inr-lg"></span>
                                            <span data-bind="CurrencyText: ($root.Bike().bookingAmount()> 0)?$root.Bike().bookingAmount():'Price unavailable'"></span>
                                        </div>
                                        <a class='viewBreakupText blue' id="cancellation-box" href="#">Hassle-free cancellation</a>
                                    </li>
                                    <li>
                                        <p>Balance amount payable:</p>
                                        <div>
                                            <span class="bwsprite inr-lg"></span>
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
                                    <h3 class="breakup-header font22 margin-bottom20"><span data-bind="text : bikeName()"></span><span class="font14 text-light-grey ">&nbsp;(On road price breakup)</span></h3>
                                    <table id="model-view-breakup" class="font16" width="100%">
                                        <tbody>
                                            <!-- ko foreach: versionPriceBreakUp -->
                                            <tr>
                                                <td width="450" class="padding-bottom10" data-bind="text: ItemName"></td>
                                                <td align="right" class="padding-bottom10 text-bold"><span class="bwsprite inr-lg"></span>&nbsp;<span data-bind="CurrencyText: Price"></span></td>
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
                                                <td align="right" class="padding-bottom10 text-bold" style="text-decoration: line-through;"><span class="bwsprite inr-lg"></span>&nbsp;<span data-bind="CurrencyText: versionPrice()"></span></td>
                                            </tr>
                                            <!-- ko foreach: discountList -->
                                            <tr>
                                                <td width="450" class="padding-bottom10" data-bind="text: 'Minus ' + CategoryName"></td>
                                                <td align="right" class="padding-bottom10 text-bold"><span class="bwsprite inr-lg"></span>&nbsp;<span data-bind="CurrencyText: Price"></span></td>
                                            </tr>
                                            <!-- /ko -->
                                            <% } %>
                                            <tr>
                                                <td colspan="2">
                                                    <div class="border-solid-top padding-bottom10"></div>
                                                </td>
                                            </tr>
                                            <tr>

                                                <td class="padding-bottom10 text-bold">Total on road price</td>
                                                <td align="right" class="padding-bottom10 font20 text-bold"><span class="bwsprite inr-lg"></span>&nbsp;<span data-bind="CurrencyText: (versionPrice() - totalDiscount())"></span></td>

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
                                            <p class="text-bold">Nearest Dealership Details</p>
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
                                    <h3 class="padding-left5 padding-bottom10 margin-left10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() > 0"><span class="bwsprite offers-icon margin-right5 text-red font-24"></span>Pay <span class="bwsprite inr-lg"></span>&nbsp;<span class="font16" data-bind="    text : $root.Bike().bookingAmount()"></span>&nbsp;to book your bike and get:</h3>

                                    <ul>
                                        <asp:Repeater ID="rptDealerOffers" runat="server">
                                            <ItemTemplate>
                                                <li class="offertxt"><%#DataBinder.Eval(Container.DataItem,"OfferText") %>
                                                    <%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "isOfferTerms")) ? "<span class='tnc font9' id='"+ DataBinder.Eval(Container.DataItem, "offerId") +"' >View terms</span>" : string.Empty %>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                    <%}
                                       else
                                       {%>
                                    <h3 class="padding-left5 padding-bottom10 margin-left10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() < 1"><span class="fa fa-gift margin-right5 font-24"></span>Book online and avail </h3>
                                    <h3 class="padding-left5 padding-bottom10 margin-left10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() > 0"><span class="bwsprite offers-icon margin-right5 text-red font-24"></span>Pay <span class="bwsprite inr-lg"></span>&nbsp;<span class="font16 margin-right5" data-bind="    text : $root.Bike().bookingAmount()"></span>&nbsp;to book your bike and avail</h3>
                                    <ul>
                                        <li>Offers from the nearest dealers</li>
                                        <li>Waiting period on this bike at the dealership</li>
                                        <li>Nearest dealership from your place</li>
                                    </ul>
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
                <img src="https://imgd.aeplcdn.com/0x0/bw/static/design15/old-images/d/search-loading.gif" />
            </div>
            <div id="terms" class="breakup-text-container padding-bottom10 font14">
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

        <section class="container margin-top10 lazy content-box-shadow booking-how-it-works" data-original="https://imgd.aeplcdn.com/0x0/bw/static/sprites/d/howItWorks.png?<%= staticFileVersion %>">
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
                        <p class="font14 text-light-grey">To book a bike, you have to pay a fixed booking amount online mentioned against the vehicle of your interest. This amount will be adjusted...<a href="/faq.aspx#2" target="_blank" rel="noopener">read more</a></p>
                    </div>
                    <div class="grid-4 content-box-shadow content-inner-block-20 margin-left20 margin-right20">
                        <p class="font16 margin-bottom20">Where do I have to pay the balance amount? How much will it be?</p>
                        <p class="font14 text-light-grey">You will pay the balance amount directly to the assigned dealership during your visit to the showroom. The...<a href="/faq.aspx#14" target="_blank" rel="noopener">read more</a></p>
                    </div>
                    <div class="grid-4 content-box-shadow content-inner-block-20">
                        <p class="font16 margin-bottom20">How will I get the benefits of the offers?</p>
                        <p class="font14 text-light-grey">Depending upon the offer, you will get the benefit of some offers directly at the dealership, while taking...<a href="/faq.aspx#16" target="_blank" rel="noopener">read more</a></p>
                    </div>
                    <div class="clear"></div>
                    <div class="margin-top20 font14 text-center">
                        <p>We’re here to help. Read our <a href="/faq.aspx" target="_blank" rel="noopener" class="text-blue">FAQs</a> or <a href="mailto:contact@bikewale.com" target="_blank" rel="noopener">email</a> us</p>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <input id="hdnBikeData" type="hidden" value='<%= bikesData %>' />
        <input id="hdnDiscountList" type="hidden" value='<%= discountedPriceList %>' />
        <!-- #include file="/includes/footerscript.aspx" -->
        <!-- #include file="/includes/footerBW.aspx" -->

        <script type="text/javascript">
            var leadSourceId;
            $(document).ready(function() {
                applyLazyLoad();
            });

            var pqId = '<%= pqId %>';
            var verId = '<%= versionId %>';
            var cityId = '<%= cityId%>';
            var dealerId = '<%= dealerId%>';
            var areaId = '<%= areaId%>';
            //Need to uncomment the below script
            var thisBikename = "<%= this.bikeName %>";
            var clientIP = "<%= clientIP %>"; 
            var pageUrl = "<%= pageUrl %>";
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
            };
            var ga_pg_id= '14';
            var gaObj = { 'id': '<%= (int)Bikewale.Entities.Pages.GAPages.Booking_Page%>', 'name': '<%= Bikewale.Entities.Pages.GAPages.Booking_Page%>' };
        </script>

        <script type="text/javascript" src="<%= staticUrl  %>/src/booking.js?<%= staticFileVersion %>"></script>

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
                if(arr[1]!="undefined")
                    viewModel.Customer().EmailId(arr[1]);
                else
                    viewModel.Customer().EmailId();
                viewModel.Customer().MobileNo(arr[2]);
            }
            <% } %>                    
            var getCityArea = GetGlobalCityArea();
        </script>
    </form>
</body>
</html>
