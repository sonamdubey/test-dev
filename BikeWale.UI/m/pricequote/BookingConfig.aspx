<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Pricequote.BookingConfig" %>

<!doctype html>
<html>
<head>
    <%
        //title = bikeName + " Booking Summary";
        //description = "Authorise dealer price details of a bike " + bikeName;
        //keywords = bikeName + ", price, authorised, dealer,Booking ";    
    %>
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-bookingconfig.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
    <!-- #include file="/includes/headscript_mobile.aspx" -->
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->

        <section class="bg-white" id="bookingConfig"  style="display:none" data-bind="visible: true">
            <h1 class="padding-top15 padding-left20 padding-right20 padding-bottom20">More details about <span data-bind="text : $root.Bike().bikeName()"></span></h1>
            <div class="container margin-bottom20">
                <div class="grid-12 box-shadow padding-bottom20">

                    <div class="booking-wrapper" >
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

                    <div id="customizeBike" data-bind="visible: CurrentStep() == 1, with: Bike" >
                        <p class="font18 text-center">
                            <span class="iconTtl text-bold">Customize your bike</span>
                        </p>
                        <div class="select-version-container border-light-bottom margin-top20">
                            <h3 class="select-versionh4 margin-bottom15">Choose version</h3>
                            <ul class="select-versionUL">
                                <asp:Repeater ID="rptVarients" runat="server">
                                    <ItemTemplate>
                                        <li class="text-light-grey border-light-grey" versionid="<%#DataBinder.Eval(Container.DataItem,"MinSpec.VersionId") %>" data-bind="click: function () { getVersion(<%#DataBinder.Eval(Container.DataItem,"MinSpec.VersionId") %>); $root.ActualSteps(1); }">
                                            <span class="bwsprite radio-btn radio-sm-unchecked margin-right5 margin-left10"></span>
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
                                <li class="text-light-grey border-light-grey" colorid="" data-bind="attr: { colorId: $data.Id},click: function() { $parent.getColor($data);$root.ActualSteps(1);}">
                                    <span class="color-box" data-bind="style: { 'background-color': '#' + HexCode }"></span>
                                    <span class="color-title-box" data-bind="text: ColorName"></span>
                                </li>
                            </ul>
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
                                                <span class="fa fa-rupee"></span>
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
                                                <span class="fa fa-rupee"></span>
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
                                                <li class="range-points-bar"><span  data-bind="text: formatPrice($.LoanAmount($root.Bike().versionPrice(), 10))" >0</span></li>
                                                <li class="range-points-bar" style="left: 5%"><span  data-bind="text: formatPrice($.LoanAmount($root.Bike().versionPrice(), 30))" >2.5L</span></li>
                                                <li class="range-points-bar" style="left: 10%"><span  data-bind="text: formatPrice($.LoanAmount($root.Bike().versionPrice(), 50))" >5L</span></li>
                                                <li class="range-points-bar" style="left: 15%"><span  data-bind="text: formatPrice($.LoanAmount($root.Bike().versionPrice(), 70))" >7.5L</span></li>
                                                <li class="range-points-bar" style="left: 19.9%"><span  data-bind="text: formatPrice($.LoanAmount($root.Bike().versionPrice(), 90))" >10L</span></li>
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
                                            <ul class="range-pointsUL month-range">
                                                <li class="range-points-bar" style="left:0"><span>12</span></li>
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
                                                <span id="rateOfInterestPercentage"  data-bind="text: rateofinterest" class="text-bold">5</span>
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
                                            <ul class="range-five-pointsUL range-pointsUL">
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
                                        <span class="font14"><span class="fa fa-rupee"></span></span>
                                        <span class="text-bold"  data-bind="text: monthlyEMI" id="emiAmount">12,000</span>
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
                                <h3 class="padding-bottom10 border-light-bottom"><span class="fa fa-map-marker text-red"></span> Contact details:</h3>
                                <ul>
                                    <li>
                                        <p class="text-bold">Offers from the nearest dealers</p>
                                        <p class="text-light-grey"><%= dealerAddress %></p>
                                    </li>
                                    <li>
                                        <p class="text-bold">Availability</p>
                                        <p class="text-light-grey" data-bind="visible : $root.Bike().waitingPeriod() > 0">Waiting period of <span class="text-default" data-bind="    text : ($root.Bike().waitingPeriod() == 1)?$root.Bike().waitingPeriod() + ' day' : $root.Bike().waitingPeriod() + ' days'"></span></p>
                                        <p class="text-green text-bold" data-bind="visible : $root.Bike().waitingPeriod() < 1">Now available</p>
                                    </li>
                                </ul>
                            </div>
                            <div class="offer-details-container border-light-top">

                                <% if (isOfferAvailable)
                                   { %>
                                <h3 class="padding-top10 padding-bottom10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() > 0"><span class="fa fa-gift margin-right5 text-red font-24"></span>Pay <span class="fa fa-rupee"></span><span class="font16" data-bind="    text : $root.Bike().bookingAmount()"></span> to book your bike and get:</h3>
                                <h3 class="padding-left5 padding-bottom10 margin-left10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() < 1"><span class="fa fa-gift margin-right5 text-red font-24"></span>Available Offers </h3>

                                <ul>
                                    <asp:Repeater ID="rptDealerOffers" runat="server">
                                        <ItemTemplate>
                                            <li><span class="fa fa-star text-red font12 margin-right5"></span><%#DataBinder.Eval(Container.DataItem,"OfferText") %></li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                                <%}
                                   else
                                   {%>
                                <h3 class="padding-top10 padding-bottom10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() > 0"><span class="fa fa-gift margin-right5 text-red font-24"></span>Pay <span class="fa fa-rupee"></span><span class="font16" data-bind="    text : $root.Bike().bookingAmount()"></span> to book your bike</h3>
                                <h3 class="padding-top10 padding-bottom10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() < 1">Dealer's Location</h3>
                                <div class="bikeModel-dealerMap-container margin-top15" style="width: 100%;min-width:50%; height: 150px" data-bind="googlemap: { latitude: latitude(), longitude: longitude() }"></div>
                                <script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDY0kkJiTPVd2U7aTOAwhc9ySH6oHxOIYM&sensor=false"></script>
                                <% } %>
                            </div>
                            <div class="clear"></div>
                        </div>
                    </div>

                    <div id="configBtnWrapper" data-bind="with: Bike">
                        
                        <div class="customize-onRoadPrice-container border-light-top padding-top15 margin-top20" >
                            <p class="text-light-grey margin-bottom5 font14">On-road price in <span class="font16 text-bold text-grey"><%= location %></span></p>
                            <div class="modelPriceContainer margin-bottom15">
                                <!-- ko if : (versionPrice() - insuranceAmount()) > 0 -->
                                <span class="font28"><span class="fa fa-rupee"></span></span>
                                <span class="font30" data-bind="CurrencyText: (versionPrice() - insuranceAmount())"></span>
                                <span class="font14 viewBreakupText">View breakup</span>
                                <div class="font12 text-light-grey " data-bind="text : priceBreakupText"></div>
                                <!-- /ko -->
                                <!-- ko ifnot : (versionPrice() - insuranceAmount()) > 0 -->
                                <span class="font30">Price unavailable</span>
                                <!-- /ko -->

                            </div>

                            <div class="disclaimer-onroadprice-container" data-bind="visible : $root.CurrentStep() == 2">
                            <div class="disclaimer-container text-left">
                                <h3 class="padding-bottom10 border-light-bottom">Disclaimer:</h3>
                                <ul class="disclaimerUL">
                                    <li>The EMI and downpayment amount is calculated as per the information entered by you. It does not include any other fees like processing fee, dealer handling charges that are typically charged by some banks / NBFCs / Dealers.</li>
                                    <li>The actual EMI and down payment will vary depending upon your credit profile
                                        Please get in touch with your bank / NBFC to know the exact EMI and downpayment based on your credit profile.</li>
                                </ul>
                            </div>
                        </div>

                            <input state="customize" id="bookingConfigNextBtn" data-bind="visible : (bookingAmount() > 0),click : function(data,event){return $root.bookNow(data,event);},attr:{value : ((viewModel.ActualSteps() > 2) && (bookingAmount() > 0))?'Book Now':'Next'}" type="button"type="button" value="Next" class="btn btn-orange btn-full-width">

                            <div class="grid-12 alpha omega query-number-container text-center margin-top10" data-bind="visible: $root.CurrentStep() == 3">
                            <p class="font14 padding-left5"><span class="bwsprite call-icon inline-block margin-right10"></span>In case of any queries feel free to call us on <span class="text-bold font18">1800 120 8300</span></p>
                        </div>

                        </div>

                        
                        <!-- View BreakUp Popup Starts here-->
                        <div class="breakupPopUpContainer content-inner-block-20 hide" id="breakupPopUpContainer">
                            <div class="breakupCloseBtn position-abt pos-top20 pos-right20 bwmsprite cross-lg-lgt-grey cur-pointer"></div>
                            <div class="breakup-text-container padding-bottom10">
                                <h3 class="breakup-header font26 margin-bottom20" ><span data-bind="text : bikeName()"></span> <span class="font14 text-light-grey ">(On road price breakup)</span></h3>
                                <table id="model-view-breakup" class="font16" width="100%" >
                                    <tbody>
                                        <!-- ko foreach: versionPriceBreakUp -->
                                        <tr>
                                            <td width="60%" class="padding-bottom10" data-bind="text: ItemName"></td>
                                            <td align="right" class="padding-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span><span data-bind="CurrencyText: Price"></span></td>
                                        </tr>

                                        <!-- /ko -->
                                        <!-- ko if : isInsuranceFree()  && insuranceAmount() > 0 -->
                                        <tr>
                                            <td colspan="2">
                                                <div class="border-solid-top padding-bottom10"></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="60%" class="padding-bottom10">Total on road price</td>
                                            <td align="right" class="padding-bottom10 text-bold" style="text-decoration: line-through;"><span class="fa fa-rupee margin-right5"></span><span data-bind="CurrencyText: versionPrice()"></span></td>
                                        </tr>

                                        <tr>
                                            <td width="60%" class="padding-bottom10">Minus insurance</td>
                                            <td align="right" class="padding-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span><span data-bind="CurrencyText: insuranceAmount()"></span></td>
                                        </tr>
                                        <!-- /ko -->
                                        <tr>
                                            <td width="60%" colspan="2">
                                                <div class="border-solid-top padding-bottom10"></div>
                                            </td>
                                        </tr>
                                        <tr>

                                            <td width="60%" class="padding-bottom10 text-bold">Total on road price</td>
                                            <td align="right" class="padding-bottom10 font20 text-bold"><span class="fa fa-rupee margin-right5"></span><span data-bind="CurrencyText: (versionPrice() - insuranceAmount())"></span></td>

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
            </div>
        </section>
        <input id="hdnBikeData" type="hidden" value='<%= new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objBookingPageDetails.Varients)%>' />
        <!-- all other js plugins -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->

        <script type="text/javascript">
            //Need to uncomment the below script
            var thisBikename = '<%= this.bikeName %>';
            //select bike version
            var bikeVersionId = '<%= versionId %>';
            $(function () {
                var versionTab = $('#customizeBike');
                $('#customizeBike ul.select-versionUL li').each(function () {
                    if (bikeVersionId === $(this).attr('versionId')) {
                        $(this).removeClass("text-light-grey border-light-grey").addClass("selected-version text-bold border-dark-grey").find("span.radio-btn").removeClass("radio-sm-unchecked").addClass("radio-sm-checked");
                    }
                });
            });

            var pqId = '<%= pqId%>';
            var versionList = JSON.parse($("#hdnBikeData").val());
            var insFree = <%= Convert.ToString(isInsuranceFree).ToLower() %>; 
            var insAmt = '<%= insuranceAmount %>';
            var cityId = '<%= cityId%>';
            var areaId = '<%= areaId%>';
            var BikeDealerDetails = function () {
                var self = this;
                // self.Dealer = ko.observable(objDealer);
                self.DealerId = ko.observable(<%= dealerId%>);
                // self.DealerDetails = ko.observable(objDealer.objDealer);
                // self.DealerQuotation = ko.observable(objDealer.objQuotation);
                self.IsInsuranceFree = ko.observable(insFree);
                self.InsuranceAmount = ko.observable(insAmt);
                self.latitude = ko.observable(<%= latitude %>);
                self.longitude = ko.observable(<%= longitude %>);
            }

        </script>
        <script src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/bwm-bookingconfig.js?<%= staticFileVersion %>" type="text/javascript"></script>

    </form>
</body>
</html>
