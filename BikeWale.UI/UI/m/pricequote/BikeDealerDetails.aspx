<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Pricequote.BookingConfig" %>

<%@ Register Src="~/m/controls/UsersTestimonials.ascx" TagPrefix="BW" TagName="UsersTestimonials" %>
<!doctype html>
<html>
<head>
    <%
        title = bikeName + " Booking Summary";
        description = "Authorise dealer price details of a bike " + bikeName;
        keywords = bikeName + ", price, authorised, dealer,Booking ";    
    %>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <link href="<%= staticUrl  %>/m/css/bwm-bookingconfig.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->

        <section class="bg-white" id="bookingConfig" style="display: none" data-bind="visible: true">
            <h1 class="padding-top15 padding-left20 padding-right20 padding-bottom20">More details about <span data-bind="text : $root.Bike().bikeName()"></span></h1>
            <div class="container margin-bottom20">
                <div class="grid-12 box-shadow padding-bottom20">

                    <div class="booking-wrapper">
                        <div id="configTabsContainer" class="bike-to-buy-tabs booking-tabs">
                            <ul class="margin-bottom20">
                                <li class="first">
                                    <div id="customizeBikeTab" class="bike-booking-part" data-bind="click: function () { if (CurrentStep() > 1 ) CurrentStep(1); }, css: (CurrentStep() >= 1) ? 'active-tab' : ''">
                                        <div class="bike-booking-image">
                                            <span class="booking-sprite " data-bind="css: (CurrentStep() == 1) ? 'customize-icon-selected' : 'booking-tick-blue'"></span>
                                        </div>
                                    </div>
                                </li>
                                <li class="middle">
                                    <div id="financeDetailsTab" class="bike-booking-part" data-bind="click: function () { if (CurrentStep() > 2 || ActualSteps() > 1) CurrentStep(2); }, css: (CurrentStep() >= 2 || ActualSteps() > 1) ? 'active-tab' : 'disabled-tab'">
                                        <div class="bike-booking-image">
                                            <span class="booking-sprite " data-bind="css: (CurrentStep() == 2) ? 'finance-icon-selected' : (CurrentStep() > 2 || ActualSteps() > 1) ? 'booking-tick-blue' : 'finance-icon-grey'"></span>
                                        </div>
                                    </div>
                                </li>
                                <li class="last">
                                    <div id="dealerDetailsTab" class="bike-booking-part" data-bind="click: function () { if ((CurrentStep() > 3) || ActualSteps() > 2) CurrentStep(3); }, css: (CurrentStep() >= 3 || ActualSteps() > 2) ? 'active-tab' : 'disabled-tab'">
                                        <div class="bike-booking-image">
                                            <span class="booking-sprite" data-bind="css: (CurrentStep() == 3) ? 'confirmation-icon-selected' : (CurrentStep() > 3 || ActualSteps() > 2) ? 'booking-tick-blue' : 'confirmation-icon-grey'"></span>
                                        </div>
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>

                    <div id="customizeBike" data-bind="visible: CurrentStep() == 1, with: Bike">
                        <p class="font18 text-center">
                            <span class="iconTtl text-bold">Customize your bike</span>
                        </p>
                        <div class="select-version-container border-light-bottom margin-top20">
                            <h3 class="select-versionh4 margin-bottom15">Choose version</h3>
                            <ul class="select-versionUL">
                                <asp:Repeater ID="rptVarients" runat="server">
                                    <ItemTemplate>
                                        <li class="text-light-grey border-light-grey" versionid="<%#DataBinder.Eval(Container.DataItem,"MinSpec.VersionId") %>" data-bind="click: function () { getVersion(<%#DataBinder.Eval(Container.DataItem,"MinSpec.VersionId") %>); $root.ActualSteps(1); }">
                                            <span class="bwmsprite radio-btn radio-sm-unchecked margin-right5 margin-left10"></span>
                                            <span class="version-title-box"><%#DataBinder.Eval(Container.DataItem,"MinSpec.VersionName") %></span>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                            <p class="font14 margin-top20 margin-bottom5">Version features</p>

                            <ul class="version-featureUL margin-bottom10 clearfix" data-bind="with: versionSpecs ">
                                <li data-bind="visible: AlloyWheels">
                                    <span class="versionIcon alloy-wheel "></span>
                                    <span class="padding-left10">Alloy wheel</span>
                                </li>
                                <li data-bind="visible: !AlloyWheels">
                                    <span class="versionIcon spoke-wheel"></span>
                                    <span class="padding-left10">Spoke wheel</span>
                                </li>
                                <li data-bind="visible: /disc/i.test(BrakeType)">
                                    <span class="versionIcon disc-brake "></span>
                                    <span class="padding-left10">Disc brake</span>
                                </li>
                                <li data-bind="visible: /drum/i.test(BrakeType)">
                                    <span class="versionIcon drum-brake "></span>
                                    <span class="padding-left10">Drum brake</span>
                                </li>
                                <li data-bind="visible: ElectricStart">
                                    <span class="versionIcon elect-start "></span>
                                    <span class="padding-left10">Electric Start</span>
                                </li>
                                <li data-bind="visible: !ElectricStart">
                                    <span class="versionIcon kick-start "></span>
                                    <span class="padding-left10">Kick Start</span>
                                </li>
                                <li data-bind="visible: AntilockBrakingSystem">
                                    <span class="versionIcon abs "></span>
                                    <span class="padding-left10">ABS</span>
                                </li>
                            </ul>
                        </div>
                        <div class="select-color-container padding-bottom10 margin-bottom15">
                            <h3 class="select-colorh4 margin-top15 margin-bottom15">Choose color</h3>
                            <ul class="select-colorUL" data-bind="foreach: versionColors">
                                <li class="text-light-grey border-light-grey" colorid="" data-bind="attr: { colorId: $data.ColorId},click: function() { $parent.getColor($data);$root.ActualSteps(1);}">
                                    <span class="color-box " data-bind="foreach : HexCode , css : (HexCode.length >= 3)? 'color-count-three': (HexCode.length ==2)?'color-count-two':'color-count-one' ">
                                        <span data-bind="style: { 'background-color': '#' + $data }"></span>
                                    </span>
                                    <span class="color-title-box" data-bind="text: ColorName"></span>
                                    <br />
                                    <span class="color-availability-box" data-bind="BikeAvailability : {Days : NoOfDays,CustomText : 'Waiting of '}"></span>
                                </li>
                            </ul>

                            <div class="margin-left10 margin-top15 margin-bottom15" data-bind="visible : $root.Bike().selectedColorId() > 0">
                                <span class="text-bold font16 ">Availability: </span>
                                <span class="color-availability-box text-light-grey font14" data-bind="visible : $root.Bike().waitingPeriod() > 0">Waiting period of <span data-bind="    text : ($root.Bike().waitingPeriod() == 1)?$root.Bike().waitingPeriod() + ' day' : $root.Bike().waitingPeriod() + ' days'"></span></span>
                                <span class="text-green text-bold" data-bind="visible : $root.Bike().waitingPeriod() == 0"><span class='text-green text-bold'>Now available</span></span>
                                <span class="text-red text-bold" data-bind="visible : $root.Bike().waitingPeriod() < 0"><span class='text-red text-bold'>Not available</span></span>
                            </div>

                        </div>

                    </div>

                    <div id="financeDetails" data-bind="visible: CurrentStep() == 2, css: (CurrentStep() > 1) ? 'active-tab text-bold' : ''" class="margin-bottom15" style="display: none">
                        <p class="font18 text-center">
                            <span class="iconTtl text-bold">Finance details</span>
                        </p>
                        <div class="finance-options-container margin-top20 margin-bottom10">
                            <h4 class="select-financeh4">Finance options</h4>
                            <ul class="select-financeUL clearfix">
                                <li class="margin-top20  grid-6 alpha">
                                    <div class="finance-required selected-finance text-bold border-dark-grey">
                                        <span class="bwmsprite radio-btn radio-btn-checked margin-right5 margin-left10"></span>
                                        <span class="finance-title-box">Required</span>
                                    </div>
                                </li>
                                <li class="grid-6 omega">
                                    <div class="text-light-grey border-light-grey">
                                        <span class="bwmsprite radio-btn radio-btn-unchecked margin-right5 margin-left10"></span>
                                        <span class="finance-title-box">Not required</span>
                                    </div>
                                </li>
                            </ul>
                        </div>
                        <div class="finance-emi-container" data-bind="with: EMI">
                            <!-- ko if : $root.Bike().versionPrice() > 0 -->
                            <div class="finance-emi-left-box alpha">
                                <div class="emi-slider-box">
                                    <div class="emi-slider-box-left-section">
                                        <div class="clearfix">
                                            <p class="grid-8 alpha">Down payment</p>
                                            <div class="emi-slider-box-right-section font16 grid-4 omega">
                                                <span class="bwmsprite inr-xsm-icon"></span>
                                                <span id="downPaymentAmount" data-bind="text: formatPrice(downPayment())" class="text-bold" class="text-bold">50000</span>
                                            </div>
                                        </div>
                                        <div id="downPaymentSlider"
                                            data-bind="slider: downPayment, sliderOptions: { min: $.LoanAmount($root.Bike().versionPrice(), 10), max: $.LoanAmount($root.Bike().versionPrice(), 90), range: 'min', step: $.LoanAmount($root.Bike().versionPrice(), 10), value: $.LoanAmount($root.Bike().versionPrice(), 10) }"
                                            class="ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all">
                                            <div class="ui-slider-range ui-widget-header ui-corner-all ui-slider-range-min"></div>
                                            <span class="ui-slider-handle ui-state-default ui-corner-all" tabindex="0" style="left: 5%;"></span>
                                        </div>
                                        <div class="slider-range-points">
                                            <ul class="range-five-pointsUL range-pointsUL">
                                                <li class="range-points-bar"><span data-bind="text: formatPrice($.LoanAmount($root.Bike().versionPrice(), 10))">0</span></li>
                                                <li class="range-points-bar" style="left: 5%"><span data-bind="text: formatPrice($.LoanAmount($root.Bike().versionPrice(), 30))">2.5L</span></li>
                                                <li class="range-points-bar" style="left: 10%"><span data-bind="text: formatPrice($.LoanAmount($root.Bike().versionPrice(), 50))">5L</span></li>
                                                <li class="range-points-bar" style="left: 15%"><span data-bind="text: formatPrice($.LoanAmount($root.Bike().versionPrice(), 70))">7.5L</span></li>
                                                <li class="range-points-bar" style="left: 19.9%"><span data-bind="text: formatPrice($.LoanAmount($root.Bike().versionPrice(), 90))">10L</span></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <div class="emi-slider-box">
                                    <div class="emi-slider-box-left-section">
                                        <div class="clearfix">
                                            <p class="grid-8 alpha">Loan Amount</p>
                                            <div class="emi-slider-box-right-section font16 grid-4 omega">
                                                <span class="bwmsprite inr-xsm-icon"></span>
                                                <span id="loanAmount" data-bind="text: formatPrice(loan())" class="text-bold">950000</span>
                                            </div>
                                        </div>
                                        <div id="loanAmountSlider"
                                            data-bind="slider: loan, sliderOptions: { min: $.LoanAmount($root.Bike().versionPrice(), 10), max: $.LoanAmount($root.Bike().versionPrice(), 90), range: 'min', step: $.LoanAmount($root.Bike().versionPrice(), 10) }"
                                            class="ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all">
                                            <div class="ui-slider-range ui-widget-header ui-corner-all ui-slider-range-min"></div>
                                            <span class="ui-slider-handle ui-state-default ui-corner-all" tabindex="0" style="left: 95%;"></span>
                                        </div>
                                        <div class="slider-range-points">
                                            <ul class="range-five-pointsUL range-pointsUL">
                                                <li class="range-points-bar"><span data-bind="text: formatPrice($.LoanAmount($root.Bike().versionPrice(), 10))">0</span></li>
                                                <li class="range-points-bar" style="left: 5%"><span data-bind="text: formatPrice($.LoanAmount($root.Bike().versionPrice(), 30))">2.5L</span></li>
                                                <li class="range-points-bar" style="left: 10%"><span data-bind="text: formatPrice($.LoanAmount($root.Bike().versionPrice(), 50))">5L</span></li>
                                                <li class="range-points-bar" style="left: 15%"><span data-bind="text: formatPrice($.LoanAmount($root.Bike().versionPrice(), 70))">7.5L</span></li>
                                                <li class="range-points-bar" style="left: 19.9%"><span data-bind="text: formatPrice($.LoanAmount($root.Bike().versionPrice(), 90))">10L</span></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <div class="emi-slider-box">
                                    <div class="emi-slider-box-left-section">
                                        <div class="clearfix">
                                            <p class="grid-8 alpha">Tenure</p>
                                            <div class="emi-slider-box-right-section font16 grid-4 omega">
                                                <span id="tenurePeriod" data-bind="text: tenure" class="font16 text-bold">36</span>
                                                <span class="font12">Months</span>
                                            </div>
                                        </div>
                                        <div id="tenureSlider"
                                            data-bind="slider: tenure, sliderOptions: { min: 12, max:60, range: 'min', step: 6 }"
                                            class="ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all">
                                            <div class="ui-slider-range ui-widget-header ui-corner-all ui-slider-range-min"></div>
                                            <span class="ui-slider-handle ui-state-default ui-corner-all" tabindex="0" style="left: 33.3333%;"></span>
                                        </div>
                                        <div class="slider-range-points">
                                            <ul class="range-pointsUL month-range tenure-rate-interest">
                                                <li class="range-points-bar" style="left: 0"><span>12</span></li>
                                                <li class="range-points-bar" style="left: 2%"><span>18</span></li>
                                                <li class="range-points-bar" style="left: 5%"><span>24</span></li>
                                                <li class="range-points-bar" style="left: 7%"><span>30</span></li>
                                                <li class="range-points-bar" style="left: 9%"><span>36</span></li>
                                                <li class="range-points-bar" style="left: 12%"><span>42</span></li>
                                                <li class="range-points-bar" style="left: 14%"><span>48</span></li>
                                                <li class="range-points-bar" style="left: 16.8%"><span>54</span></li>
                                                <li class="range-points-bar" style="left: 20%"><span>60</span></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <div class="emi-slider-box">
                                    <div class="emi-slider-box-left-section">
                                        <div class="clearfix">
                                            <p class="grid-8 alpha">Rate of interest</p>
                                            <div class="emi-slider-box-right-section font16 grid-4 omega">
                                                <span id="rateOfInterestPercentage" data-bind="text: rateofinterest" class="text-bold">5</span>
                                                <span>%</span>
                                            </div>
                                        </div>
                                        <div id="rateOfInterestSlider"
                                            data-bind="slider: rateofinterest, sliderOptions: { min: 7, max: 20, range: 'min', step: 0.25 }"
                                            class="ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all">
                                            <div class="ui-slider-range ui-widget-header ui-corner-all ui-slider-range-min"></div>
                                            <span class="ui-slider-handle ui-state-default ui-corner-all" tabindex="0" style="left: 25%;"></span>
                                        </div>
                                        <div class="slider-range-points">
                                            <ul class="range-five-pointsUL range-pointsUL tenure-rate-interest">
                                                <li class="range-points-bar"><span>7</span></li>
                                                <li class="range-points-bar" style="left: 5%"><span>10.25</span></li>
                                                <li class="range-points-bar" style="left: 10%"><span>13.5</span></li>
                                                <li class="range-points-bar" style="left: 15%"><span>16.5</span></li>
                                                <li class="range-points-bar" style="left: 19.9%"><span>20</span></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </div>
                            <div class="finance-emi-right-box omega margin-top15">
                                <div class="clearfix">
                                    <p class="grid-8 alpha">Indicative EMI<span class="font12 text-light-grey">(per month)</span></p>
                                    <div class="indicative-emi-amount font16 text-right grid-4 omega">
                                        <span class="bwmsprite inr-xsm-icon"></span>
                                        <span class="text-bold" data-bind="text: monthlyEMI" id="emiAmount">12,000</span>
                                    </div>
                                </div>

                            </div>
                            <div class="clear"></div>
                            <!-- /ko -->
                        </div>
                    </div>

                    <div id="dealerDetails" data-bind="visible: CurrentStep() > 2, with: Dealer" style="display: none">
                        <p class="font18 text-center">
                            <span class="iconTtl text-bold">Dealer details</span>
                        </p>
                        <div class="contact-offers-container margin-top20 padding-bottom15">
                            <div class="contact-details-container">
                                <h3 class="padding-bottom10 border-light-bottom"><span class="bwmsprite contact-icon margin-right5"></span>Contact details:</h3>
                                <ul>
                                    <li>
                                        <p class="text-bold">Offers from the nearest dealers</p>
                                        <p class="text-light-grey"><%= dealerAddress %></p>
                                    </li>
                                    <li>
                                        <p class="text-bold">Availability</p>
                                        <span class="color-availability-box text-light-grey font14" data-bind="visible : $root.Bike().waitingPeriod() > 0">Waiting period of <span data-bind="    text : ($root.Bike().waitingPeriod() == 1)?$root.Bike().waitingPeriod() + ' day' : $root.Bike().waitingPeriod() + ' days'"></span></span>
                                        <span class="text-green text-bold" data-bind="visible : $root.Bike().waitingPeriod() == 0"><span class='text-green text-bold'>Now available</span></span>
                                        <span class="text-red text-bold" data-bind="visible : $root.Bike().waitingPeriod() < 0"><span class='text-red text-bold'>Not available</span></span>
                                    </li>
                                </ul>
                            </div>
                            <div class="offer-details-container border-light-top">

                                <% if (isOfferAvailable)
                                   { %>
                                <h3 class="padding-top10 padding-bottom10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() > 0"><span class="bwmsprite offers-icon margin-right5"></span>Pay <span class="bwmsprite inr-xsm-icon"></span><span class="font16" data-bind="    text : $root.Bike().bookingAmount()"></span>to book your bike and get:</h3>
                                <h3 class="padding-left5 padding-bottom10 margin-left10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() < 1"><span class="bwmsprite offers-icon margin-right5"></span>Available Offers </h3>

                                <ul>
                                    <asp:Repeater ID="rptDealerOffers" runat="server">
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
                                <script src="https://maps.googleapis.com/maps/api/js?key=<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey %>"></script>
                                <h3 class="padding-top10 padding-bottom10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() > 0"><span class="bwmsprite offers-icon margin-right5"></span>Pay <span class="bwmsprite inr-sm-icon"></span><span class="font16" data-bind="    text : $root.Bike().bookingAmount()"></span>to book your bike</h3>
                                <h3 class="padding-top10 padding-bottom10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() < 1">Dealer's Location</h3>
                                <div class="bikeModel-dealerMap-container margin-top15" style="width: 100%; min-width: 50%; height: 150px" data-bind="googlemap: { latitude: latitude(), longitude: longitude() }"></div>

                                <% } %>
                            </div>
                            <div class="clear"></div>
                        </div>
                    </div>

                    <div id="configBtnWrapper" data-bind="with: Bike">
                        <div class="customize-onRoadPrice-container border-light-top padding-top15 margin-top20">
                            <p class="text-light-grey margin-bottom5 font14">On-road price in <span class="font16 text-bold text-grey"><%= location %></span></p>
                            <div class="modelPriceContainer margin-bottom15">
                                <!-- ko if : (versionPrice() - insuranceAmount()) > 0 -->
                                <span class="bwmsprite inr-xlg-icon"></span> 
                                <span class="font30" data-bind="CurrencyText: (versionPrice() - totalDiscount())"></span>
                                <span class="font14 viewBreakupText">View breakup</span>
                                <!-- /ko -->
                                <!-- ko ifnot : (versionPrice() - insuranceAmount()) > 0 -->
                                <span class="font30">Price unavailable</span>
                                <!-- /ko -->
                            </div>
                            <div class="disclaimer-onroadprice-container" data-bind="visible : $root.CurrentStep() == 2">
                                <div class="disclaimer-container text-left">
                                    <h3 class="padding-bottom10 border-light-bottom"><span class="bwmsprite disclaimer-icon margin-left5 margin-right5"></span>Disclaimer:</h3>
                                    <ul class="disclaimerUL">
                                        <li>The EMI and downpayment amount is calculated as per the information entered by you. It does not include any other fees like processing fee, dealer handling charges that are typically charged by some banks / NBFCs / Dealers.</li>
                                        <li>The actual EMI and down payment will vary depending upon your credit profile
                                        Please get in touch with your bank / NBFC to know the exact EMI and downpayment based on your credit profile.</li>
                                    </ul>
                                </div>
                            </div>
                            <!-- ko if : (bookingAmount() > 0) && (viewModel.CurrentStep() > 2) -->
                            <input type="button" id="bookingConfigNextBtn" data-bind="click : function(data,event){ $root.UpdateVersion(data,event); return $root.bookNow(data,event);},attr:{value : ((viewModel.CurrentStep() > 2) && (bookingAmount() > 0))?'Book Now':'Next'}" type="button" value="Next" class="btn btn-orange btn-full-width" />
                            <!-- /ko -->
                            <!-- ko ifnot : (bookingAmount() > 0) && (viewModel.CurrentStep() > 2) -->
                            <input type="button" data-bind="visible : $root.CurrentStep() < 3 , click : function(data,event){ $root.UpdateVersion(data,event); return $root.bookNow(data,event);}" value="Next" class="btn btn-orange btn-full-width" />
                            <!-- /ko -->
                        </div>
                        <!-- View BreakUp Popup Starts here-->
                        <div class="breakupPopUpContainer bwm-fullscreen-popup hide" id="breakupPopUpContainer">
                            <div class="breakupCloseBtn position-abt pos-top10 pos-right10 bwmsprite cross-lg-lgt-grey cur-pointer"></div>
                            <div class="breakup-text-container padding-bottom10">
                                <h3 class="breakup-header margin-bottom5"><span data-bind="text : bikeName()"></span><span class="font14 text-light-grey ">(On road price breakup)</span></h3>
                                <table id="model-view-breakup" class="font16" width="100%">
                                    <tbody>
                                        <!-- ko foreach: versionPriceBreakUp -->
                                        <tr>
                                            <td width="60%" class="padding-bottom10" data-bind="text: ItemName"></td>
                                            <td align="right" class="padding-bottom10 text-bold"><span class="bwmsprite inr-xsm-icon"></span><span data-bind="CurrencyText: Price"></span></td>
                                        </tr>

                                        <!-- /ko -->
                                        <% if (dealerDetailEntity != null && dealerDetailEntity.objQuotation != null && dealerDetailEntity.objQuotation.discountedPriceList != null && dealerDetailEntity.objQuotation.discountedPriceList.Count > 0)
                                           {%>
                                        <tr>
                                            <td colspan="2">
                                                <div class="border-solid-top padding-bottom10"></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="padding-bottom10">Total on road price</td>
                                            <td align="right" class="padding-bottom10 text-bold" style="text-decoration: line-through;"><span class="bwmsprite inr-xsm-icon"></span><span data-bind="CurrencyText: versionPrice()"></span></td>
                                        </tr>
                                        <!-- ko foreach: discountList -->
                                        <tr>
                                            <td width="350" class="padding-bottom10" data-bind="text: 'Minus '+CategoryName"></td>
                                            <td align="right" class="padding-bottom10 text-bold"><span class="bwmsprite inr-xsm-icon"></span><span data-bind="CurrencyText: Price"></span></td>
                                        </tr>
                                        <!-- /ko -->
                                        <%} %>
                                        <%--<!-- ko if : isInsuranceFree()  && insuranceAmount() > 0 -->
                                            <tr>
                                                <td class="padding-bottom10">Minus insurance</td>
                                                <td align="right" class="padding-bottom10 text-bold"><span class="bwmsprite inr-xxsm-icon margin-right5"></span><span data-bind="CurrencyText: insuranceAmount()"></span></td>
                                            </tr>
                                            <!-- /ko -->--%>
                                        <tr>
                                            <td colspan="2">
                                                <div class="border-solid-top padding-bottom10"></div>
                                            </td>
                                        </tr>
                                        <tr>

                                            <td class="padding-bottom10 text-bold">Total on road price</td>
                                            <td align="right" class="padding-bottom10 font20 text-bold"><span class="bwmsprite inr-sm-icon"></span><span data-bind="CurrencyText: (versionPrice() - totalDiscount())"></span></td>

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
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
                <%--<div class="content-box-shadow content-inner-block-15 margin-top15 margin-bottom15 text-medium-grey text-center" data-bind="visible : $root.CurrentStep()==3">
                    <p class="text-medium-grey font14 margin-bottom10">In case of queries call us toll-free on:</p>
                    <a href="tel:1800 120 8300" class="font20 text-grey call-text-green rounded-corner2" style="text-decoration: none;"><span class="bwmsprite tel-green-icon margin-right5"></span>1800 120 8300</a>
                </div>--%>
            </div>
        </section>


        <section class="container margin-bottom30 content-box-shadow">
            <div class="lazy booking-how-it-works" data-original="https://imgd.aeplcdn.com/0x0/bw/static/sprites/m/howitworks-mobile.png?<%= staticFileVersion %>"></div>
            <div class="clear"></div>
        </section>
        <% if (ctrlUsersTestimonials.FetchedCount > 0)
           { %>
        <section class="bg-white">
            <div id="testimonialWrapper" class="content-box-shadow padding-top15 padding-bottom20 margin-bottom30"  data-bind="visible : IsUserTestimonials() ">
                <h2 class="text-bold text-center margin-bottom15 font24">What do our customers say</h2>
                <div class="swiper-container text-center">
                    <div class="swiper-wrapper padding-bottom20">
                        <BW:UsersTestimonials ID="ctrlUsersTestimonials" runat="server"></BW:UsersTestimonials>
                    </div>
                    <div class="swiper-pagination"></div>
                </div>
            </div>
        </section>
        <%                 
           }
        %>
        <input id="hdnBikeData" type="hidden" value='<%= jsonBikeVarients  %>' />

        <section>
            <div class="container margin-bottom30 margin-top20">
                <div id="faqSlug">
                    <div class="faq-slug-container content-box-shadow content-inner-block-20">
                        <div class="question-icon-container text-center leftfloat">
                            <div class="icon-outer-container rounded-corner50percent">
                                <div class="icon-inner-container rounded-corner50percent">
                                    <span class="bwmsprite question-mark-icon margin-top20"></span>
                                </div>
                            </div>
                        </div>
                        <!-- Terms and condition Popup start -->
                        <div class="termsPopUpContainer content-inner-block-20 hide" id="termsPopUpContainer">
                            <div class="fixed-close-btn-wrapper">
                                <div class="termsPopUpCloseBtn bwmsprite fixed-close-btn cross-lg-lgt-grey cur-pointer"></div>
                            </div>
                            <h3>Terms and Conditions</h3>
                            <div  class="hide" style="vertical-align: middle; text-align: center;" id="termspinner">
                                <img src="https://imgd.aeplcdn.com/0x0/bw/static/design15/old-images/d/search-loading.gif" />
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
                        <div class="question-text-container leftfloat padding-left15">
                            <p class="question-title font16 text-bold text-black">Questions?</p>
                            <p class="question-subtitle text-light-grey font14">
                                We’re here to help.<br />
                                Read our <a href="/m/faq.aspx" target="_blank" rel="noopener">FAQs</a> or <a href="mailto:contact@bikewale.com">Email Us</a><%-- or call us on <a href="tel:18001208300" class="text-dark-grey">1800 120 8300</a>--%>
                            </p>
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <input id="hdnDiscountList" type="hidden" value='<%= new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(dealerDetailEntity.objQuotation.discountedPriceList)%>' />

        <!-- all other js plugins -->
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->

        <script type="text/javascript">
            //Need to uncomment the below script
            var thisBikename = "<%= this.bikeName %>";
            //select bike version
            var bikeVersionId = '<%= versionId %>';
            var pqId = '<%= pqId%>';
            var versionList = JSON.parse(Base64.decode($("#hdnBikeData").val()));
            var preSelectedColor = <%= (objCustomer != null && objCustomer.objColor != null) ? objCustomer.objColor.ColorId : 0 %>;
            var discountDetail = JSON.parse($("#hdnDiscountList").val());
            var insFree = <%= Convert.ToString(isInsuranceFree).ToLower() %>; 
            var insAmt = '<%= insuranceAmount %>';
            var cityId = '<%= cityId%>';
            var areaId = '<%= areaId%>';
            var bwHostUrl = '<%= ConfigurationManager.AppSettings["bwHostUrl"]%>';
            var BikeDealerDetails = function () {
                var self = this;
                self.DealerId = ko.observable(<%= dealerId%>);
                self.IsInsuranceFree = ko.observable(insFree);
                self.InsuranceAmount = ko.observable(insAmt);
                self.latitude = ko.observable(<%= latitude %>);
                self.longitude = ko.observable(<%= longitude %>);
            }
            ga_pg_id = "8";
            var getCityArea = GetGlobalCityArea();
        </script>
        <script src="<%= staticUrl  %>/m/src/bwm-bookingconfig.js?<%= staticFileVersion %>" type="text/javascript"></script>

    </form>
</body>
</html>
