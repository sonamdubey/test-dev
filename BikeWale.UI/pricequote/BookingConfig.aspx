<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Pricequote.BookingConfig" Trace="false" EnableEventValidation="false" %>

<!DOCTYPE html>
<html>
<head>
    <%
        title = bikeName + " Bookingbooking-sprite buy-icon customize-icon-grey Summary";
        description = "Authorise dealer price details of a bike " + bikeName;
        keywords = bikeName + ", price, authorised, dealer,Booking ";    
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bookingconfig.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
</head>
<body class="header-fixed-inner">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section>
            <div class="container">
                <div class="grid-12 margin-bottom5">
                    <div class="breadcrumb margin-bottom10">
                        <!-- breadcrumb code starts here -->
                        <ul>
                            <li><a href="/">Home</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/new/">New Bikes</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span>Booking Config</li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="font30 text-black margin-top10">Booking configurator - 
                	<span><%= bikeName %></span>
                    </h1>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container margin-bottom20 " id="bookingConfig" style="min-height: 500px;display: none;" data-bind="visible: true">
                <div class="grid-12">
                    <div class="content-box-shadow content-inner-block-20 rounded-corner2">
                        <div id="configTabsContainer" class="margin-bottom10">
                            <div class="horizontal-line position-rel margin-auto"></div>
                            <ul>
                                <li>
                                    <div id="customizeBikeTab" class="bike-config-part"  data-bind="click: function () { if (CurrentStep() > 1 ) CurrentStep(1); }, css: (CurrentStep() >= 1) ? 'active-tab' : ''" >
                                        <p>Customize your bike</p>
                                        <div class="config-tabs-image">
                                            <span class="booking-sprite booking-config-icon " data-bind="css: (CurrentStep() == 1) ? 'customize-icon-selected' : 'booking-tick-blue'"></span>
                                        </div>
                                    </div>
                                </li>
                                <li>
                                    <div id="financeDetailsTab" class="bike-config-part " data-bind="click: function () { if (CurrentStep() > 2 || ActualSteps() > 1) CurrentStep(2); }, css: (CurrentStep() >= 2 || ActualSteps() > 1) ? 'active-tab' : 'disabled-tab'" >
                                        <p>Finance details</p>
                                        <div class="config-tabs-image">
                                            <span class="booking-sprite booking-config-icon " data-bind="css: (CurrentStep() == 2) ? 'finance-icon-selected' : (CurrentStep() > 2 || ActualSteps() > 1) ? 'booking-tick-blue' : 'finance-icon-grey'"></span>
                                        </div>
                                    </div>
                                </li>
                                <li>
                                    <div id="dealerDetailsTab" class="bike-config-part disabled-tab" data-bind="click: function () { if ((CurrentStep() > 3) || ActualSteps() > 2) CurrentStep(3); }, css: (CurrentStep() >= 3 || ActualSteps() > 2) ? 'active-tab' : 'disabled-tab'" >
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
                                <h4 class="select-versionh4">Select version</h4>
                                <ul class="select-versionUL">
                                    <asp:Repeater ID="rptVarients" runat="server">
                                        <ItemTemplate>
                                            <li class="text-light-grey border-light-grey" versionid="<%#DataBinder.Eval(Container.DataItem,"MinSpec.VersionId") %>" data-bind="click: getVersion()  ">
                                                <span class="bwsprite radio-btn radio-sm-unchecked margin-right5 margin-left10"></span>
                                                <span class="version-title-box"><%#DataBinder.Eval(Container.DataItem,"MinSpec.VersionName") %></span>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <asp:HiddenField ID="selectedVersionId" runat="Server"></asp:HiddenField>
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
                                <h4 class="select-colorh4 margin-top15">Select color</h4>
                                <ul class="select-colorUL" data-bind="foreach: versionColors">
                                    <li class="text-light-grey border-light-grey">
                                        <span class="color-box" data-bind="style: { 'background-color': '#' + HexCode }"></span>
                                        <span class="color-title-box" data-bind="text: ColorName"></span>
                                    </li>
                                </ul>
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
                            <div class="finance-emi-container border-light-bottom padding-bottom15">
                                <!-- updated line -->
                                <div class="finance-emi-left-box alpha border-light-right">
                                    <div class="emi-slider-box">
                                        <div class="emi-slider-box-left-section">
                                            <p>Down payment</p>
                                            <div id="downPaymentSlider" class="ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all">
                                                <div class="ui-slider-range ui-widget-header ui-corner-all ui-slider-range-min" style="width: 5%;"></div>
                                                <span class="ui-slider-handle ui-state-default ui-corner-all" tabindex="0" style="left: 5%;"></span>
                                            </div>
                                            <div class="slider-range-points">
                                                <ul class="range-five-pointsUL range-pointsUL">
                                                    <li class="range-points-bar"><span>0</span></li>
                                                    <li class="range-points-bar" style="left: 5%"><span>2.5L</span></li>
                                                    <li class="range-points-bar" style="left: 10%"><span>5L</span></li>
                                                    <li class="range-points-bar" style="left: 15%"><span>7.5L</span></li>
                                                    <li class="range-points-bar" style="left: 19.9%"><span>10L</span></li>
                                                </ul>
                                            </div>
                                        </div>
                                        <div class="emi-slider-box-right-section font16">
                                            <span class="fa fa-rupee"></span>
                                            <span id="downPaymentAmount" class="text-bold">50000</span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="emi-slider-box">
                                        <div class="emi-slider-box-left-section">
                                            <p>Loan Amount</p>
                                            <div id="loanAmountSlider" class="ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all">
                                                <div class="ui-slider-range ui-widget-header ui-corner-all ui-slider-range-min" style="width: 95%;"></div>
                                                <span class="ui-slider-handle ui-state-default ui-corner-all" tabindex="0" style="left: 95%;"></span>
                                            </div>
                                            <div class="slider-range-points">
                                                <ul class="range-five-pointsUL range-pointsUL">
                                                    <li class="range-points-bar"><span>0</span></li>
                                                    <li class="range-points-bar" style="left: 5%"><span>2.5L</span></li>
                                                    <li class="range-points-bar" style="left: 10%"><span>5L</span></li>
                                                    <li class="range-points-bar" style="left: 15%"><span>7.5L</span></li>
                                                    <li class="range-points-bar" style="left: 19.9%"><span>10L</span></li>
                                                </ul>
                                            </div>
                                        </div>
                                        <div class="emi-slider-box-right-section font16">
                                            <span class="fa fa-rupee"></span>
                                            <span id="loanAmount" class="text-bold">950000</span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="emi-slider-box">
                                        <div class="emi-slider-box-left-section">
                                            <p>Tenure</p>
                                            <div id="tenureSlider" class="ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all">
                                                <div class="ui-slider-range ui-widget-header ui-corner-all ui-slider-range-min" style="width: 33.3333%;"></div>
                                                <span class="ui-slider-handle ui-state-default ui-corner-all" tabindex="0" style="left: 33.3333%;"></span>
                                            </div>
                                            <div class="slider-range-points">
                                                <ul class="range-pointsUL">
                                                    <li class="range-points-bar"><span>1</span></li>
                                                    <li class="range-points-bar" style="left: 3%"><span>2</span></li>
                                                    <li class="range-points-bar" style="left: 6%"><span>3</span></li>
                                                    <li class="range-points-bar" style="left: 8.2%"><span>4</span></li>
                                                    <li class="range-points-bar" style="left: 11%"><span>5</span></li>
                                                    <li class="range-points-bar" style="left: 13.3%"><span>6</span></li>
                                                    <li class="range-points-bar" style="left: 15.8%"><span>7</span></li>
                                                </ul>
                                            </div>
                                        </div>
                                        <div class="emi-slider-box-right-section">
                                            <span id="tenurePeriod" class="font16 text-bold">36</span>
                                            <span class="font12">Months</span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="emi-slider-box">
                                        <div class="emi-slider-box-left-section">
                                            <p>Rate of interest</p>
                                            <div id="rateOfInterestSlider" class="ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all">
                                                <div class="ui-slider-range ui-widget-header ui-corner-all ui-slider-range-min" style="width: 25%;"></div>
                                                <span class="ui-slider-handle ui-state-default ui-corner-all" tabindex="0" style="left: 25%;"></span>
                                            </div>
                                            <div class="slider-range-points">
                                                <ul class="range-five-pointsUL range-pointsUL">
                                                    <li class="range-points-bar"><span>0</span></li>
                                                    <li class="range-points-bar" style="left: 5%"><span>5</span></li>
                                                    <li class="range-points-bar" style="left: 10%"><span>10</span></li>
                                                    <li class="range-points-bar" style="left: 15%"><span>15</span></li>
                                                    <li class="range-points-bar" style="left: 19.9%"><span>20</span></li>
                                                </ul>
                                            </div>
                                        </div>
                                        <div class="emi-slider-box-right-section font16">
                                            <span id="rateOfInterestPercentage" class="text-bold">5</span>
                                            <span>%</span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </div>
                                <div class="finance-emi-right-box omega text-center">
                                    <h4 class="margin-top90 text-light-grey margin-bottom20">Indicative EMI</h4>
                                    <div class="indicative-emi-amount margin-bottom5">
                                        <span class="font28"><span class="fa fa-rupee"></span></span>
                                        <span id="emiAmount" class="font30">12,000</span>
                                    </div>
                                    <p class="font16 text-light-grey">per month</p>
                                </div>
                                <div class="clear"></div>
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
                                            <p class="text-light-grey">Waiting period of <span class="text-default"></span></p>
                                        </li>
                                    </ul>
                                </div>
                                <div class="grid-7 omega offer-details-container">

                                    <% if (isOfferAvailable)
                                       { %>
                                    <h3 class="padding-left5 padding-bottom10 margin-left10 border-light-bottom"><span class="fa fa-gift margin-right5"></span>Pay <span class="font16"><span class="fa fa-rupee"></span></span><%= bookingAmount %></span> to book your bike and get:</h3>
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
                                    <div class="bikeModel-dealerMap-container" style="width: 250px; height: 150px" data-bind="googlemap: { latitude: latitude(), longitude: longitude() }"></div>
                                    <script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDY0kkJiTPVd2U7aTOAwhc9ySH6oHxOIYM&sensor=false"></script>
                                    <% } %>
                                </div>
                                <div class="clear"></div>
                            </div>
                        </div>

                        <div id="configBtnWrapper" data-bind="with: Bike">
                            <div class="grid-8 alpha query-number-container" data-bind="visible: $root.CurrentStep() == 3">
                                <p class="font14 padding-left5"><span class="bwsprite call-icon inline-block margin-right10"></span>In case of any queries feel free to call us on <span class="text-bold font18">1800 120 8300</span></p>
                            </div>
                            <div class="disclaimer-container grid-8 text-left border-light-right" data-bind="visible : $root.CurrentStep() == 2">
                                <h3 class="padding-bottom10 margin-right20 border-light-bottom">Disclaimer:</h3>
                                <ul class="disclaimerUL">
                                    <li>The EMI amount is calculated as per the information entered by you. It does not include any other fees like processing fee that are typically charged by some banks / NBFCs</li>
                                    <li>The actual EMI and down payment will vary depending upon your credit profile Please get in touch with your bank / NBFC to know the exact EMI and downpayment based on your credit profile.</li>
                                </ul>
                            </div>
                            <div class="grid-4 omega text-right " data-bind="css: ($root.CurrentStep() != 3) ? '' : 'border-light-left'">
                                <p class="text-light-grey margin-bottom5 font14">On-road price in <span class="font16 text-bold text-grey"><%= location %></span></p>
                                <div class="modelPriceContainer margin-bottom15">
                                    <!-- ko if : (versionPrice() - insuranceAmount()) > 0 -->
                                    <span class="font28"><span class="fa fa-rupee"></span></span>
                                    <span class="font30" data-bind="CurrencyText: (versionPrice() - insuranceAmount())"></span>
                                    <span class="font14 text-light-grey viewBreakupText">View breakup</span>
                                    <!-- /ko -->
                                    <!-- ko ifnot : (versionPrice() - insuranceAmount()) > 0 -->
                                    <span class="font30">Price unavailable</span>
                                    <!-- /ko -->

                                </div>
                                <input state="customize" id="bookingConfigNextBtn" data-bind="click: $root.changedSteps" type="button" value="Next" class="btn btn-orange">
                            </div>
                            <!-- View BreakUp Popup Starts here-->
                            <div class="breakupPopUpContainer content-inner-block-20 hide" id="breakupPopUpContainer">
                                <div class="breakupCloseBtn position-abt pos-top20 pos-right20 bwsprite cross-lg-lgt-grey cur-pointer"></div>
                                <div class="breakup-text-container padding-bottom10">
                                    <h3 class="breakup-header font26 margin-bottom20"><%= bikeName %> <span class="font14 text-light-grey ">(On road price breakup)</span></h3>
                                    <table id="model-view-breakup" class="font16">
                                        <tbody>
                                            <!-- ko foreach: versionPriceBreakUp -->
                                            <tr>
                                                <td width="350" class="padding-bottom10" data-bind="text: ItemName"></td>
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
                                                <td class="padding-bottom10">Total on road price</td>
                                                <td align="right" class="padding-bottom10 text-bold" style="text-decoration: line-through;"><span class="fa fa-rupee margin-right5"></span><span data-bind="CurrencyText: versionPrice()"></span></td>
                                            </tr>

                                            <tr>
                                                <td class="padding-bottom10">Minus insurance</td>
                                                <td align="right" class="padding-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span><span data-bind="CurrencyText: insuranceAmount()"></span></td>
                                            </tr>
                                            <!-- /ko -->
                                            <tr>
                                                <td colspan="2">
                                                    <div class="border-solid-top padding-bottom10"></div>
                                                </td>
                                            </tr>
                                            <tr>

                                                <td class="padding-bottom10 text-bold">Total on road price</td>
                                                <td align="right" class="padding-bottom10 font20 text-bold"><span class="fa fa-rupee margin-right5"></span><span data-bind="CurrencyText: (versionPrice() - insuranceAmount())"></span></td>

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


        <input id="hdnBikeData" type="hidden" value='<%= new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objBookingConfig.Varients)%>' />
        <input id="hdnDealerData" type="hidden" value='<%= new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objBookingConfig.DealerQuotation)%>' />
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/bookingconfig.js?<%= staticFileVersion %>"></script>
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

                var steps = 0;
            });          

            //var versionList = ([{ "hostUrl": "http://imgd1.aeplcdn.com/", "imagePath": "/bw/models/honda-cb-trigger-standard-304.jpg?20151209184418", "onRoadPrice": 78708, "bookingAmount": 0, "noOfWaitingDays": 5, "priceList": [{ "ItemName": "Ex-showroom ", "Price": 73409, "DealerId": 11348, "ItemId": 3 }, { "ItemName": "RTO", "Price": 5299, "DealerId": 11348, "ItemId": 5 }], "minSpec": { "brakeType": "Disc", "alloyWheels": true, "electricStart": true, "antilockBrakingSystem": false, "versionId": 304, "versionName": "Standard", "modelName": "CB Trigger", "price": 78708 }, "make": { "makeId": 7, "makeName": "Honda", "maskingName": "honda", "hostUrl": null, "logoUrl": null }, "model": { "modelId": 220, "modelName": "CB Trigger", "maskingName": "cbtrigger" }, "bikeModelColors": [{ "id": 891, "modelId": 304, "colorName": "Black", "hexCode": "4f4e4e" }, { "id": 892, "modelId": 304, "colorName": "Meteor Green Metallic", "hexCode": "dbc774" }, { "id": 893, "modelId": 304, "colorName": "Peral Siena Red", "hexCode": "af1126" }, { "id": 1853, "modelId": 304, "colorName": "Pearl Sunbeam White", "hexCode": "eeeeee" }] }, { "hostUrl": "http://imgd1.aeplcdn.com/", "imagePath": "/bw/models/honda-cb-trigger-standard-304.jpg?20151209184418", "onRoadPrice": 78908, "bookingAmount": 10, "noOfWaitingDays": 10, "priceList": [{ "ItemName": "Ex-showroom ", "Price": 73609, "DealerId": 11348, "ItemId": 3 }, { "ItemName": "RTO", "Price": 5299, "DealerId": 11348, "ItemId": 5 }], "minSpec": { "brakeType": "Disc", "alloyWheels": true, "electricStart": true, "antilockBrakingSystem": false, "versionId": 467, "versionName": "DLX", "modelName": "CB Trigger", "price": 78908 }, "make": { "makeId": 7, "makeName": "Honda", "maskingName": "honda", "hostUrl": null, "logoUrl": null }, "model": { "modelId": 220, "modelName": "CB Trigger", "maskingName": "cbtrigger" }, "bikeModelColors": [{ "id": 888, "modelId": 467, "colorName": "Black", "hexCode": "4f4e4e" }, { "id": 889, "modelId": 467, "colorName": "Meteor Green Metallic", "hexCode": "dbc774" }, { "id": 890, "modelId": 467, "colorName": "Peral Siena Red", "hexCode": "af1126" }, { "id": 1852, "modelId": 467, "colorName": "Pearl Sunbeam White", "hexCode": "eeeeee" }] }, { "hostUrl": "http://imgd1.aeplcdn.com/", "imagePath": "/bw/models/honda-cb-trigger-standard-304.jpg?20151209184418", "onRoadPrice": 78808, "bookingAmount": 0, "noOfWaitingDays": 15, "priceList": [{ "ItemName": "Ex-showroom ", "Price": 73509, "DealerId": 11348, "ItemId": 3 }, { "ItemName": "RTO", "Price": 5299, "DealerId": 11348, "ItemId": 5 }], "minSpec": { "brakeType": "Disc", "alloyWheels": true, "electricStart": true, "antilockBrakingSystem": false, "versionId": 469, "versionName": "CBS", "modelName": "CB Trigger", "price": 78808 }, "make": { "makeId": 7, "makeName": "Honda", "maskingName": "honda", "hostUrl": null, "logoUrl": null }, "model": { "modelId": 220, "modelName": "CB Trigger", "maskingName": "cbtrigger" }, "bikeModelColors": [{ "id": 885, "modelId": 469, "colorName": "Black", "hexCode": "4f4e4e" }, { "id": 886, "modelId": 469, "colorName": "Meteor Green Metallic", "hexCode": "dbc774" }, { "id": 887, "modelId": 469, "colorName": "Peral Siena Red", "hexCode": "af1126" }, { "id": 1851, "modelId": 469, "colorName": "Pearl Sunbeam White", "hexCode": "eeeeee" }] }]);
            var first = true;
            var versionList = JSON.parse($("#hdnBikeData").val());
            var objDealer = JSON.parse($("#hdnDealerData").val());

            var BookingConfigViewModel = function () {
                var self = this;
                self.Bike = ko.observable(new BikeDetails);
                self.Dealer = ko.observable(new BikeDealerDetails);
                self.EMI = ko.observable(new BikeEMI);
                self.CurrentStep = ko.observable(1);
                self.SelectedVersion = ko.observable(0);
                self.SelectedColor = ko.observable(0);
                self.ActualSteps = ko.observable(1);
                self.changedSteps = function () {
                    if (self.Bike().selectedVersionId() > 0) {
                        if (self.Bike().selectedColor() > 0) {
                            self.SelectedVersion(self.Bike().selectedVersionId());
                            self.SelectedColor(self.Bike().selectedColor());
                            if (self.CurrentStep() != 3)
                            {
                              self.CurrentStep(self.CurrentStep() + 1);
                              self.ActualSteps(self.ActualSteps() + 1)
                            }
                            
                        }
                        else {
                            $("#customizeBike .select-colorh4").addClass("text-red");
                            return false;
                        }
                    }
                    else {
                        $("#customizeBike .select-versionh4").addClass("text-red");
                        return false;
                    }

                };
            }

            var BikeDealerDetails = function () {
                var self = this;
                self.Dealer = ko.observable(objDealer);
                self.DealerId = ko.observable(0);
                self.DealerDetails = ko.observable(objDealer.objDealer);
                self.DealerQuotation = ko.observable(objDealer.objQuotation);
                self.IsInsuranceFree = ko.observable(objDealer.IsInsuranceFree);
                self.InsuranceAmount = ko.observable(objDealer.InsuranceAmount);
                self.latitude = ko.observable(objDealer.objDealer.objArea.Latitude);
                self.longitude = ko.observable(objDealer.objDealer.objArea.Longitude);
            }

            var BikeEMI = function () {
                var self = this;
                self.MinPrice = ko.observable();
                self.MaxPrice = ko.observable();
            }

            var BikeDetails = function () {
                var self = this;
                self.bikeVersions = ko.observableArray(versionList);
                self.selectedVersionId = ko.observable(bikeVersionId);
                self.selectedVersion = ko.observable();
                self.versionPriceBreakUp = ko.observableArray([]);
                self.bookingAmount = ko.observable();
                self.waitingPeriod = ko.observable();
                self.selectedColor = ko.observable();
                self.isInsuranceFree = ko.observable(objDealer.IsInsuranceFree);
                self.insuranceAmount = ko.observable(objDealer.InsuranceAmount);

                self.versionPrice = ko.computed(function () {
                    var total = 0;
                    for (i = 0; i < self.versionPriceBreakUp().length; i++) {
                        total += self.versionPriceBreakUp()[i].Price;
                    }
                    return total;
                }, this);

                self.versionColors = ko.observableArray([]);
                self.priceListBreakup = ko.observableArray([]);
                self.versionSpecs = ko.observable();
                self.getVersion = function () {
                    $.each(self.bikeVersions(), function (key, value) {
                        if (self.selectedVersionId() != undefined && self.selectedVersionId() > 0 && self.selectedVersionId() == value.MinSpec.VersionId) {
                            self.versionColors(value.BikeModelColors);
                            self.versionSpecs(value.MinSpec);
                            self.versionPriceBreakUp(value.PriceList);
                            self.waitingPeriod(value.NoOfWaitingDays);
                            self.bookingAmount(value.BookingAmount);                             
                        }
                    });
                }

            }

            ko.bindingHandlers.googlemap = {
                init: function (element, valueAccessor) {
                    var
                      value = valueAccessor(),
                      latLng = new google.maps.LatLng(value.latitude, value.longitude),
                      mapOptions = {
                          zoom: 10,
                          center: latLng,
                          mapTypeId: google.maps.MapTypeId.ROADMAP
                      },
                      map = new google.maps.Map(element, mapOptions),
                      marker = new google.maps.Marker({
                          position: latLng,
                          map: map
                      });
                }
            };

            ko.bindingHandlers.CurrencyText = {
                update: function (element, valueAccessor) {
                    var amount = valueAccessor();
                    var formattedAmount = ko.unwrap(amount) !== null ? formatPrice(amount) : 0;
                    $(element).text(formattedAmount);
                }
            };

            function formatPrice(price) {
                price = price.toString();
                var lastThree = price.substring(price.length - 3);
                var otherNumbers = price.substring(0, price.length - 3);
                if (otherNumbers != '')
                    lastThree = ',' + lastThree;
                var price = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
                return price;
            }

            var viewModel = new BookingConfigViewModel;
            ko.applyBindings(viewModel, $("#bookingConfig")[0]);

        </script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->

    </form>
</body>
</html>
