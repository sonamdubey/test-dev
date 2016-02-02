<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Pricequote.BookingConfig" %>

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
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bookingconfig.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />

</head>
<body class="header-fixed-inner">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <div id="bookingConfig" style="display: none" data-bind="visible: true">
        <section>
            <div class="container">
                <div class="grid-12 margin-bottom5 margin-top15">
                    <div class="breadcrumb margin-bottom10">
                        <!-- breadcrumb code starts here -->
                        <ul>
                            <li><a href="/">Home</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><%= makeUrl %></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><%= modelUrl %></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><span data-bind="text : $root.Bike().selectedVersion().MinSpec.VersionName"></span></li>
                            <li><span class="fa fa-angle-right margin-right10"></span>Dealer Details</li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="font30 text-black margin-top10">More details about   
                	<span data-bind="text : $root.Bike().bikeName()"></span>
                    </h1>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section >
            <div class="container" style="min-height: 500px;">
                <div class="grid-12">
                    <div class="content-box-shadow content-inner-block-20 rounded-corner2">
                        <div id="configTabsContainer" class="margin-bottom10">
                            <div class="horizontal-line position-rel margin-auto"></div>
                            <ul>
                                <li>
                                    <div id="customizeBikeTab" class="bike-config-part" data-bind="click: function () { if (CurrentStep() > 1 ) CurrentStep(1); }, css: (CurrentStep() >= 1) ? 'active-tab' : ''">
                                        <p>Customize your bike</p>
                                        <div class="config-tabs-image">
                                            <span class="booking-sprite booking-config-icon " data-bind="css: (CurrentStep() == 1) ? 'customize-icon-selected' : 'booking-tick-blue'"></span>
                                        </div>
                                    </div>
                                </li>
                                <li>
                                    <div id="financeDetailsTab" class="bike-config-part " data-bind="click: function () { if (CurrentStep() > 2 || ActualSteps() > 1) CurrentStep(2); }, css: (CurrentStep() >= 2 || ActualSteps() > 1) ? 'active-tab' : 'disabled-tab'">
                                        <p>Finance details</p>
                                        <div class="config-tabs-image">
                                            <span class="booking-sprite booking-config-icon " data-bind="css: (CurrentStep() == 2) ? 'finance-icon-selected' : (CurrentStep() > 2 || ActualSteps() > 1) ? 'booking-tick-blue' : 'finance-icon-grey'"></span>
                                        </div>
                                    </div>
                                </li>
                                <li>
                                    <div id="dealerDetailsTab" class="bike-config-part " data-bind="click: function () { if ((CurrentStep() > 3) || ActualSteps() > 2) CurrentStep(3); }, css: (CurrentStep() >= 3 || ActualSteps() > 2) ? 'active-tab' : 'disabled-tab'">
                                        <p>Dealer details</p>
                                        <div class="config-tabs-image">
                                            <span class="booking-sprite booking-config-icon " data-bind="css: (CurrentStep() == 3) ? 'confirmation-icon-selected' : (CurrentStep() > 3 || ActualSteps() > 2) ? 'booking-tick-blue' : 'confirmation-icon-grey'"></span>
                                        </div>
                                    </div>
                                </li>
                            </ul>
                        </div>
                        <div id="customizeBike" data-bind="visible: CurrentStep() == 1, with: Bike" style="display: block">
                            <div class="select-version-container border-light-bottom ">
                                <h4 class="select-versionh4">Choose version</h4>
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
                                <p class="font14 margin-top5">Features</p>

                                <ul class="version-featureUL margin-bottom10" data-bind="with: versionSpecs ">
                                    <li data-bind="visible: AlloyWheels">
                                        <span class="booking-sprite ver-feature-img alloy-wheel-icon inline-block"></span>
                                        <span class="inline-block">Alloy wheel</span>
                                    </li>
                                    <li data-bind="visible: !AlloyWheels">
                                        <span class="booking-sprite ver-feature-img spoke-wheel-icon inline-block"></span>
                                        <span class="inline-block">Spoke wheel</span>
                                    </li>
                                    <li data-bind="visible: /disc/i.test(BrakeType)">
                                        <span class="booking-sprite ver-feature-img disc-brake-icon inline-block"></span>
                                        <span class="inline-block">Disc brake</span>
                                    </li>
                                    <li data-bind="visible: /drum/i.test(BrakeType)">
                                        <span class="booking-sprite ver-feature-img drum-brake-icon inline-block"></span>
                                        <span class="inline-block">Drum brake</span>
                                    </li>
                                    <li data-bind="visible: ElectricStart">
                                        <span class="booking-sprite ver-feature-img electric-start-icon inline-block"></span>
                                        <span class="inline-block">Electric Start</span>
                                    </li>
                                    <li data-bind="visible: !ElectricStart">
                                        <span class="booking-sprite ver-feature-img kick-start-icon inline-block"></span>
                                        <span class="inline-block">Kick Start</span>
                                    </li>
                                    <li data-bind="visible: AntilockBrakingSystem">
                                        <span class="booking-sprite ver-feature-img abs-icon inline-block"></span>
                                        <span class="inline-block">ABS</span>
                                    </li>
                                </ul>
                            </div>
                            <div class="select-color-container border-light-bottom padding-bottom10 margin-bottom15">
                                <h4 class="select-colorh4 margin-top15">Choose colour</h4>
                                <ul class="select-colorUL" data-bind="foreach: versionColors">
                                    <li class="text-light-grey border-light-grey" colorid="" data-bind="attr: { colorId: $data.ColorId},click: function() { $parent.getColor($data);$root.ActualSteps(1);}">
                                        <span class="color-box " data-bind="foreach : HexCode , css : (HexCode.length >= 3)? 'color-count-three': (HexCode.length ==2)?'color-count-two':'color-count-one' ">
                                            <span data-bind="style: { 'background-color': '#' + $data }"></span>
                                        </span>
                                        <span class="color-title-box" data-bind="text: ColorName"></span>
                                        <span class="color-availability-box" data-bind="BikeAvailability : {Days : NoOfDays,CustomText : 'Waiting of '}"></span>
                                    </li>
                                </ul>

                                <div class="margin-left10 margin-top15 margin-bottom15" data-bind="visible : $root.Bike().selectedColorId() > 0">
                                    <span class="text-bold font16 ">Availability: </span>
                                    <span class="color-availability-box text-light-grey font14" data-bind="visible : $root.Bike().waitingPeriod() > 0">Waiting period of <span data-bind="text : ($root.Bike().waitingPeriod() == 1)?$root.Bike().waitingPeriod() + ' day' : $root.Bike().waitingPeriod() + ' days'"></span></span>
                                    <span class="text-green text-bold" data-bind="visible : $root.Bike().waitingPeriod() == 0"><span class='text-green text-bold'>Now available</span></span>
                                    <span class="text-red text-bold" data-bind="visible : $root.Bike().waitingPeriod() < 0"><span class='text-red text-bold'>Not available</span></span>
                                </div>
                            </div>


                        </div>

                        <div id="financeDetails" data-bind="visible: CurrentStep() == 2, css: (CurrentStep() > 1) ? 'active-tab text-bold' : ''" class="margin-bottom15" style="display: none">
                            <!-- updated line -->
                            <div class="finance-options-container">
                                <h4 class="select-financeh4">Finance options</h4>
                                <ul class="select-financeUL">
                                    <li class="finance-required selected-finance text-bold border-dark-grey">
                                        <span class="bwsprite radio-btn radio-sm-checked margin-right5 margin-left10"></span>
                                        <span class="finance-title-box">Required</span>
                                    </li>
                                    <li class="text-light-grey border-light-grey">
                                        <span class="bwsprite radio-btn radio-sm-unchecked margin-right5 margin-left10"></span>
                                        <span class="finance-title-box">Not required</span>
                                    </li>
                                </ul>
                            </div>

                            <div class="finance-emi-container border-light-bottom padding-bottom15" data-bind="with: EMI">
                                <!-- ko if : $root.Bike().versionPrice() > 0 -->
                                <!-- updated line -->
                                <div class="finance-emi-left-box alpha border-light-right">
                                    <div class="emi-slider-box">
                                        <div class="emi-slider-box-left-section">
                                            <p>Down payment</p>
                                            <div id="downPaymentSlider"
                                                data-bind="slider: downPayment, sliderOptions: { min: $.LoanAmount($root.Bike().versionPrice(), 10), max: $.LoanAmount($root.Bike().versionPrice(), 90), range: 'min', step: $.LoanAmount($root.Bike().versionPrice(), 10), value: $.LoanAmount($root.Bike().versionPrice(), 30) }"
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
                                        <div class="emi-slider-box-right-section font16">
                                            <span class="fa fa-rupee"></span>
                                            <span id="downPaymentAmount" class="text-bold" data-bind="text: formatPrice(downPayment())"></span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="emi-slider-box">
                                        <div class="emi-slider-box-left-section">
                                            <p>Loan Amount</p>
                                            <div id="loanAmountSlider"
                                                data-bind="slider: loan, sliderOptions: { min: $.LoanAmount($root.Bike().versionPrice(), 10), max: $.LoanAmount($root.Bike().versionPrice(), 90), range: 'min', step: $.LoanAmount($root.Bike().versionPrice(), 10) }"
                                                class="ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all">
                                                <div class="ui-slider-range ui-widget-header ui-corner-all ui-slider-range-min"></div>
                                                <span class="ui-slider-handle ui-state-default ui-corner-all" tabindex="0" style="left: 95%;"></span>
                                            </div>
                                            <div class="slider-range-points">
                                                <ul class="range-five-pointsUL range-pointsUL">
                                                    <li class="range-points-bar"><span data-bind="text: formatPrice($.LoanAmount($root.Bike().versionPrice(), 10))">0</span></li>
                                                    <li class="range-points-bar" style="left: 5%"><span data-bind="text: formatPrice($.LoanAmount($root.Bike().versionPrice(), 30))"></span></li>
                                                    <li class="range-points-bar" style="left: 10%"><span data-bind="text: formatPrice($.LoanAmount($root.Bike().versionPrice(), 50))">5L</span></li>
                                                    <li class="range-points-bar" style="left: 15%"><span data-bind="text: formatPrice($.LoanAmount($root.Bike().versionPrice(), 70))">7.5L</span></li>
                                                    <li class="range-points-bar" style="left: 19.9%"><span data-bind="text: formatPrice($.LoanAmount($root.Bike().versionPrice(), 90))">10L</span></li>
                                                </ul>
                                            </div>
                                        </div>
                                        <div class="emi-slider-box-right-section font16">
                                            <span class="fa fa-rupee"></span>
                                            <span id="loanAmount" class="text-bold" data-bind="text: formatPrice(loan())"></span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="emi-slider-box">
                                        <div class="emi-slider-box-left-section">
                                            <p>Tenure (Months)</p>
                                            <div id="tenureSlider"
                                                data-bind="slider: tenure, sliderOptions: { min: 12, max:60, range: 'min', step: 6 }"
                                                class="ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all">
                                                <div class="ui-slider-range ui-widget-header ui-corner-all ui-slider-range-min"></div>
                                                <span class="ui-slider-handle ui-state-default ui-corner-all" tabindex="0" style="left: 33.3333%;"></span>
                                            </div>
                                            <div class="slider-range-points">
                                                <ul class="range-pointsUL tenure-rate-interest">
                                                    <li class="range-points-bar"><span>12</span></li>
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
                                        <div class="emi-slider-box-right-section">
                                            <span id="tenurePeriod" class="font16 text-bold" data-bind="text: tenure">36</span>
                                            <span class="font12">Months</span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="emi-slider-box">
                                        <div class="emi-slider-box-left-section">
                                            <p>Rate of interest (Percentage)</p>
                                            <div id="rateOfInterestSlider"
                                                data-bind="slider: rateofinterest, sliderOptions: { min: 7, max: 20, range: 'min', step: 0.25 }"
                                                class="ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all">
                                                <div class="ui-slider-range ui-widget-header ui-corner-all ui-slider-range-min"></div>
                                                <span class="ui-slider-handle ui-state-default ui-corner-all" tabindex="0" style="left: 25%;"></span>
                                            </div>
                                            <div class="slider-range-points">
                                                <ul class="range-five-pointsUL range-pointsUL tenure-rate-interest.">
                                                    <li class="range-points-bar"><span>7</span></li>
                                                    <li class="range-points-bar" style="left: 5%"><span>10.25</span></li>
                                                    <li class="range-points-bar" style="left: 10%"><span>13.5</span></li>
                                                    <li class="range-points-bar" style="left: 15%"><span>16.5</span></li>
                                                    <li class="range-points-bar" style="left: 19.9%"><span>20</span></li>
                                                </ul>
                                            </div>
                                        </div>
                                        <div class="emi-slider-box-right-section font16">
                                            <span id="rateOfInterestPercentage" class="text-bold" data-bind="text: rateofinterest">5</span>
                                            <span>%</span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </div>
                                <div class="finance-emi-right-box omega text-center">
                                    <h4 class="margin-top90 text-light-grey margin-bottom20">Indicative EMI</h4>
                                    <div class="indicative-emi-amount margin-bottom5">
                                        <span class="font28"><span class="fa fa-rupee"></span></span>
                                        <span id="emiAmount" class="font30" data-bind="text: monthlyEMI">12,000</span>
                                    </div>
                                    <p class="font16 text-light-grey">per month</p>
                                </div>
                                <div class="clear"></div>
                                <!-- /ko -->
                            </div>
                            <div class="clear"></div>
                        </div>

                        <div id="dealerDetails" class="margin-bottom15" data-bind="visible: CurrentStep() > 2, with: Dealer" style="display: none">
                            <div class="contact-offers-container border-light-bottom padding-bottom15">
                                <div class="grid-5 alpha contact-details-container border-light-right">
                                    <h3 class="padding-bottom10 padding-left5 margin-right20 border-light-bottom"><span class="bwsprite contact-icon margin-right5"></span>Contact details:</h3>
                                    <ul>
                                        <li>
                                            <p class="text-bold">Offers from the nearest dealers</p>
                                            <p class="text-light-grey"><%= dealerAddress %></p>
                                        </li>
                                        <li>
                                            <p class="text-bold">Availability</p>
                                            <span class="color-availability-box text-light-grey font14" data-bind="visible : $root.Bike().waitingPeriod() > 0">Waiting period of <span data-bind="text : ($root.Bike().waitingPeriod() == 1)?$root.Bike().waitingPeriod() + ' day' : $root.Bike().waitingPeriod() + ' days'"></span></span>
                                            <span class="text-green text-bold" data-bind="visible : $root.Bike().waitingPeriod() == 0"><span class='text-green text-bold'>Now available</span></span>
                                            <span class="text-red text-bold" data-bind="visible : $root.Bike().waitingPeriod() < 0"><span class='text-red text-bold'>Not available</span></span>
                                        </li>

                                    </ul>
                                </div>
                                <div class="grid-7 omega offer-details-container">

                                    <% if (isOfferAvailable)
                                       { %>
                                    <h3 class="padding-left5 padding-bottom10 margin-left10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() < 1"><span class="bwsprite offers-icon margin-right5"></span>Available Offers </h3>
                                    <h3 class="padding-left5 padding-bottom10 margin-left10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() > 0"><span class="bwsprite offers-icon margin-right5"></span>Pay <span class="fa fa-rupee" style="font-size: 15px"></span><span class="font16" data-bind="    text : $root.Bike().bookingAmount()"></span>to book your bike and get:</h3>

                                    <ul>
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
                                    <script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDY0kkJiTPVd2U7aTOAwhc9ySH6oHxOIYM"></script>
                                    <h3 class="padding-left5 padding-bottom10 margin-left10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() > 0"><span class="fa fa-gift margin-right5 text-red font-24"></span>Pay <span class="fa fa-rupee" style="font-size: 15px"></span><span class="font16" data-bind="    text : $root.Bike().bookingAmount()"></span>to book your bike</h3>
                                    <h3 class="padding-bottom10 padding-left5 margin-right20 border-light-bottom margin-bottom20" data-bind="visible : $root.Bike().bookingAmount() < 1"><span class="fa fa-map-marker text-red margin-right5"></span>Dealer's Location</h3>
                                    <div class="bikeModel-dealerMap-container margin-left5 margin-top15" style="width: 400px; height: 150px" data-bind="googlemap: { latitude: latitude(), longitude: longitude() }"></div>

                                    <% } %>
                                </div>
                                <div class="clear"></div>
                            </div>
                        </div>

                        <div id="configBtnWrapper" data-bind="with: Bike">
                            <div class="grid-8 alpha query-number-container" data-bind="visible: $root.CurrentStep() == 3">
                                <p class="font14 padding-left5"><span class="bwsprite call-icon inline-block margin-right10"></span>In case of any queries feel free to call us on <span class="text-bold font18">1800 120 8300</span></p>
                            </div>
                            <div class="disclaimer-container grid-8 text-left border-light-right alpha" data-bind="visible : $root.CurrentStep() == 2">
                                <h3 class="padding-bottom10 margin-right20 border-light-bottom"><span class="bwsprite disclaimer-icon margin-left5 margin-right5"></span>Disclaimer:</h3>
                                <ul class="disclaimerUL">
                                    <li>The EMI and downpayment amount is calculated as per the information entered by you. It does not include any other fees like processing fee, dealer handling charges that are typically charged by some banks / NBFCs / Dealers.</li>
                                    <li>The actual EMI and down payment will vary depending upon your credit profile Please get in touch with your bank / NBFC to know the exact EMI and downpayment based on your credit profile.</li>
                                </ul>
                            </div>
                            <div class="grid-4 omega text-right " data-bind="css: ($root.CurrentStep() != 3) ? '' : 'border-light-left'">
                                <p class="text-light-grey margin-bottom5 font14">On-road price in <span class="font16 text-bold text-grey"><%= location %></span></p>
                                <div class="modelPriceContainer margin-bottom15">
                                    <!-- ko if : (versionPrice() - insuranceAmount()) > 0 -->
                                    <span class="font28"><span class="fa fa-rupee"></span></span>
                                    <span class="font30" data-bind="CurrencyText: (versionPrice() - totalDiscount())"></span>
                                    <span class="font14 text-light-grey viewBreakupText">View breakup</span>
                                    <!-- /ko -->
                                    <!-- ko ifnot : (versionPrice() - insuranceAmount()) > 0 -->
                                    <span class="font30">Price unavailable</span>
                                    <!-- /ko -->

                                </div>
                                <!-- ko if : (bookingAmount() > 0) && (viewModel.CurrentStep() > 2) -->
                                <input type="button" id="bookingConfigNextBtn" data-bind="click : function(data,event){ $root.UpdateVersion(data,event); return $root.bookNow(data,event);},attr:{value : ((viewModel.CurrentStep() > 2) && (bookingAmount() > 0))?'Book Now':'Next'}" type="button" value="Next" class="btn btn-orange" />
                                <!-- /ko -->
                                <!-- ko ifnot : (bookingAmount() > 0) && (viewModel.CurrentStep() > 2) -->
                                <input type="button" data-bind="visible : $root.CurrentStep() < 3 , click : function(data,event){$root.UpdateVersion(data,event); return $root.bookNow(data,event);}" value="Next" class="btn btn-orange" />
                                <!-- /ko -->
                                <span class="select-color-warning-tooltip leftfloat">Please select a colour</span>
                                <span class="clear"></span>
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
                                            <% if (dealerDetailEntity != null && dealerDetailEntity.objQuotation != null
                                                   && dealerDetailEntity.objQuotation.discountedPriceList != null && dealerDetailEntity.objQuotation.discountedPriceList.Count > 0)
                                               {%>
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
                                                <td width="350" class="padding-bottom10" data-bind="text: 'Minus '+CategoryName"></td>
                                                <td align="right" class="padding-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span><span data-bind="CurrencyText: Price"></span></td>
                                            </tr>
                                            <!-- /ko -->
                                            <%} %>

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
                            <div class="clear"></div>
                        </div>

                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>  
        </div>
        <!-- Terms and condition Popup start -->
        <div class="termsPopUpContainer content-inner-block-20 hide" id="termsPopUpContainer">
            <h3>Terms and Conditions</h3>
            <div style="vertical-align: middle; text-align: center;" id="termspinner">
                <img src="/images/search-loading.gif" />
            </div>
            <div class="termsPopUpCloseBtn position-abt pos-top20 pos-right20 bwsprite cross-lg-lgt-grey cur-pointer"></div>
            <div id="terms" class="breakup-text-container padding-bottom10 font14">
            </div>
        </div>
        <!-- Terms and condition Popup Ends -->

        <section class="container margin-top30 lazy content-box-shadow booking-how-it-works" data-original="http://img.aeplcdn.com/bikewaleimg/images/howItWorks.png?<%= staticFileVersion %>">
            <div class="grid-12"></div>
            <div class="clear"></div>
        </section>

        <input id="hdnBikeData" type="hidden" value='<%= jsonBikeVarients  %>' />

        <% if (ctrlUsersTestimonials.FetchedCount > 0)
           { %>
        <section>
            <div id="testimonialWrapper" class="container margin-bottom30">
                <div class="grid-12 <%= ctrlUsersTestimonials.FetchedCount > 0 ? "" : "hide" %>">
                    <h2 class="text-bold text-center margin-top50 margin-bottom30 font28">Testimonials</h2>
                    <BW:UsersTestimonials ID="ctrlUsersTestimonials" runat="server"></BW:UsersTestimonials>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <%
           }
           else
           {
        %>
        <section>
            <div class="container margin-bottom30">
                <div class="grid-12">
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <%           
           }
        %>

        <section>
            <div class="container margin-bottom30">
                <div class="grid-12">
                    <div class="content-box-shadow content-inner-block-20">
                        <div class="inline-block text-center margin-right30">
                            <div class="icon-outer-container rounded-corner50">
                                <div class="icon-inner-container rounded-corner50">
                                    <span class="bwsprite question-mark-icon margin-top25"></span>
                                </div>
                            </div>
                        </div>
                        <div class="inline-block">
                            <h3 class="margin-bottom10">Questions?</h3>
                            <p class="text-light-grey font14">We’re here to help. Read our <a href="/faq.aspx" target="_blank">FAQs</a>, <a href="mailto:contact@bikewale.com">email</a> or call us on <span class="text-dark-grey">1800 120 8300</span></p>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <input id="hdnDiscountList" type="hidden" value='<%= new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(dealerDetailEntity.objQuotation.discountedPriceList)%>' />

        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->

        <script type="text/javascript">
            $(document).ready(function() {
                applyLazyLoad();
            });
            var thisBikename = "<%= this.bikeName %>";
            var bikeVersionId = "<%= versionId %>";
            var pqId = '<%= pqId%>';
            var versionList = JSON.parse(Base64.decode($("#hdnBikeData").val()));
            var discountDetail = JSON.parse($("#hdnDiscountList").val());
            var preSelectedColor = '<%= (objCustomer != null && objCustomer.objColor != null) ? objCustomer.objColor.ColorId : 0 %>';
            var insFree = <%= Convert.ToString(isInsuranceFree).ToLower() %>; 
            var insAmt = '<%= insuranceAmount %>';
            var cityId = '<%= cityId%>';
            var areaId = '<%= areaId%>';
            var BikeDealerDetails = function () {
                var self = this;
                self.DealerId = ko.observable(<%= dealerId%>);
                self.IsInsuranceFree = ko.observable(insFree);
                self.InsuranceAmount = ko.observable(insAmt);
                self.latitude = ko.observable(<%= latitude %>);
                self.longitude = ko.observable(<%= longitude %>);
            }
            var getCityArea = GetGlobalCityArea();
            var abHostUrl = '<%= ConfigurationManager.AppSettings["ABApiHostUrl"]%>';
        </script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/bookingconfig.js?<%= staticFileVersion %>"></script>


    </form>
</body>
</html>
