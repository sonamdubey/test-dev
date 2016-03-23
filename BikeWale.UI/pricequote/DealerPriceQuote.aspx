<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.BikeBooking.DealerPriceQuote" Trace="false" EnableEventValidation="false" %>

<%@ Register Src="~/controls/AlternativeBikes.ascx" TagName="AlternativeBikes" TagPrefix="BW" %>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Import Namespace="Bikewale.BikeBooking" %>

<!doctype html>
<html>
<head>
    <%
        title = String.Format("{0} {1} Price Quote", BikeName, versionName);
        description = String.Format("{0} {1} price quote", BikeName, versionName);
        AdId = "1395986297721";
        AdPath = "/1017752/Bikewale_PQ_";
        isAd970x90Shown = true;
        PopupWidget.Visible = true;
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/dealerpricequote.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var dealerId = '<%= dealerId%>';
        var pqId = '<%= pqId%>';
        var ABHostUrl = '<%= System.Configuration.ConfigurationManager.AppSettings["ApiHostUrl"]%>';
        var versionId = '<%= versionId%>';
        var cityId = '<%= cityId%>';
        var areaId = '<%= areaId%>';   
        var clientIP = "<%= clientIP%>";
        var pageUrl = "www.bikewale.com/quotation/dealerpricequote.aspx?versionId=" + versionId + "&cityId=" + cityId;
    </script>
</head>
<body class="bg-light-grey header-fixed-inner">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section class="bg-light-grey padding-top10">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom10">
                        <!-- breadcrumb code starts here -->
                        <ul>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <a href="/" itemprop="url">
                                    <span itemprop="title">Home</span>
                                </a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="fa fa-angle-right margin-right10"></span>
                                <a href="/new/" itemprop="url">
                                    <span itemprop="title">New</span>
                                </a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="fa fa-angle-right margin-right10"></span>
                                <a href="/pricequote/" itemprop="url">
                                    <span itemprop="title">On-Road Price Quote</span>
                                </a>
                            </li>
                            <li><span class="fa fa-angle-right margin-right10"></span>Dealer Price Quote</li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="font26 text-default margin-bottom10">On-road price quote for <%= BikeName %> in <%= GetLocationCookie() %></h1>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section class="container">
            <div class="grid-12 margin-bottom20" id="dealerPriceQuoteContainer">
                <div class="content-box-shadow rounded-corner2">
                    <div id="pqBikeDetails" class="grid-8 alpha omega">
                        <div class="grid-6 padding-bottom20" id="PQImageVariantContainer">
                            <% if (detailedDealer != null)
                               { %>
                            <div class="pqBikeImage margin-bottom15">
                                <img alt="<%= String.Format("{0} {1}",BikeName,versionName) %> Photos" src="<%= Bikewale.Utility.Image.GetPathToShowImages(detailedDealer.OriginalImagePath,detailedDealer.HostUrl,Bikewale.Utility.ImageSize._310x174) %>" title="<%= String.Format("{0} {1}",BikeName,versionName) %> Photos" />
                            </div>
                            <% } %>

                            <div class="pqVariants">
                                <p class="margin-left10 font16 text-light-grey leftfloat margin-top7">Version:</p>
                                <% if (versionList.Count > 1)
                                   { %>
                                <div class="position-rel">
                                    <div class="variants-dropdown rounded-corner2 leftfloat">
                                        <div class="variant-selection-tab">
                                            <asp:Label runat="server" ID="defaultVariant"></asp:Label>
                                        </div>
                                        <span id="upDownArrow" class="rightfloat fa fa-angle-down position-abt pos-top10 pos-right10"></span>
                                    </div>
                                    <ul class="variants-dropdown-list">
                                        <asp:Repeater ID="rptVersion" runat="server">
                                            <ItemTemplate>
                                                <li>
                                                    <asp:Button Style="width: 100%; text-align: left" ID="btnVariant"
                                                        OnCommand="btnVariant_Command" versionid='<%# DataBinder.Eval(Container.DataItem,"VersionId") %>' CommandName='<%# DataBinder.Eval(Container.DataItem,"VersionId") %>'
                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem,"VersionName") %>' runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"VersionName") %>'></asp:Button>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <asp:HiddenField ID="hdnVariant" runat="server" />
                                    </ul>
                                </div>
                                <% }
                                   else
                                   { %>
                                <span id="versText" class="margin-left10 font16 text-light-grey leftfloat margin-top7 text-light-grey margin-right20 text-bold"><%= versionName %></span>
                                <% } %>
                            </div>

                        </div>
                        <!--Price List Section-->
                        <div class="grid-6 padding-top15 padding-bottom20 padding-right20" id="PQDetailsContainer">

                            <p class="font14 text-default text-bold margin-bottom15">On-road price - <%= dealerName %></p>

                            <div runat="server">
                                <div>
                                    <% if (detailedDealer != null)
                                       { %>
                                    <table class="font14" cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <asp:Repeater ID="rptPriceList" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td width="210" class="PQDetailsTableTitle padding-bottom15 text-light-grey">
                                                        <%# DataBinder.Eval(Container.DataItem,"CategoryName") %> 
                                                    </td>
                                                    <td align="right" class="PQDetailsTableAmount padding-bottom10 text-default ">
                                                        <span class="fa fa-rupee"></span>&nbsp;<span id="exShowroomPrice"><%#CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"Price").ToString()) %></span>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <tr>
                                            <td colspan="2">
                                                <div class="border-solid-top padding-bottom10"></div>
                                                <td>
                                        </tr>

                                        <tr>
                                            <td class="PQDetailsTableTitle font14  PQOnRoadPrice text-grey text-bold">On-road price</td>
                                            <td align="right" class="PQDetailsTableAmount font18 text-grey text-bold">
                                                <span class="fa fa-rupee"></span>&nbsp;<span><%= CommonOpn.FormatPrice(totalPrice.ToString()) %></span>

                                            </td>
                                        </tr>

                                        <tr>
                                            <td colspan="2" class="text-right padding-top5"><a class="font14 text-link " id="leadLink" name="leadLink" onclick="dataLayer.push({ event: 'Bikewale_all', cat: 'New Bike Booking - <%=BikeName.Replace("'","")%>', act: 'Click Button Get dealer details',lab: 'Clicked on Button Get_Dealer_Details' });">Get more details</a></td>
                                        </tr>
                                        <tr class="hide">
                                            <td colspan="3">
                                                <ul class="std-ul-list">
                                                    <asp:Repeater ID="rptDisclaimer" runat="server">
                                                        <ItemTemplate>
                                                            <li><i><%# Container.DataItem %></i></li>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </ul>
                                            </td>
                                        </tr>
                                    </table>
                                    <% }
                                       else
                                       { %>
                                    <div class="grey-bg border-light padding5 margin-top10 margin-bottom20">
                                        <h3>Dealer Prices for this Version is not available.</h3>
                                    </div>
                                    <% } %>
                                </div>

                            </div>

                            <div id="div_ShowErrorMsg" runat="server" class="grey-bg border-light content-block text-highlight margin-top15"></div>
                        </div>
                        <div class="clear"></div>
                        <!--offer List Section-->
                        <%if (dealerType != Bikewale.Entities.PriceQuote.DealerPackageTypes.Standard && isoffer)
                          { %>
                        <div class="grid-12 padding-right20 padding-bottom10 padding-left20 font14">
                            <p class="text-bold padding-top20 margin-bottom5 border-light-top">Exclusive offers from this dealer:</p>
                            <ul class="pricequote-benefits-list text-light-grey">
                                <asp:Repeater ID="rptOffers" runat="server">
                                    <ItemTemplate>
                                        <li>
                                            <span class="inline-block pq-benefits-image pricequote-sprite <%#  "offerIcon_" + DataBinder.Eval(Container.DataItem,"OfferId") %> margin-right10"></span>
                                            <span class="inline-block pq-benefits-title"><%#  DataBinder.Eval(Container.DataItem,"OfferText") %></span>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                            <div class="clear"></div>
                        </div>
                        <div class="clear"></div>
                        <% } %>

                        <%if (primarydealer.DealerDetails != null)
                          { %>
                        <div id="dealerAssistance">
                            <!--Lead capture form-->
                            <div class="grid-12 bg-light-grey content-inner-block-20">
                                <div id="buying-assistance-form">
                                    <p class="font14 text-bold margin-bottom20">Get buying assistance from this dealer:</p>
                                    <div class="buying-assistance-form">
                                        <div class="form-control-box margin-right10">
                                            <input type="text" class="form-control" placeholder="Name" id="assistanceGetName" data-bind="textInput: fullName">
                                            <span class="bwsprite error-icon errorIcon"></span>
                                            <div class="bw-blackbg-tooltip errorText"></div>
                                        </div>
                                        <div class="form-control-box margin-right10">
                                            <input type="text" class="form-control" placeholder="Email id" id="assistanceGetEmail" data-bind="textInput: emailId">
                                            <span class="bwsprite error-icon errorIcon"></span>
                                            <div class="bw-blackbg-tooltip errorText"></div>
                                        </div>
                                        <div class="form-control-box margin-right10 assistance-form-mobile">
                                            <p class="mobile-prefix">+91</p>
                                            <input type="text" class="form-control padding-left40" maxlength="10" placeholder="Number" id="assistanceGetMobile" data-bind="textInput: mobileNo">
                                            <span class="bwsprite error-icon errorIcon"></span>
                                            <div class="bw-blackbg-tooltip errorText"></div>
                                        </div>
                                        <a class="btn btn-orange leftfloat" id="buyingAssistBtn" data-bind="event: { click: submitLead } ">Submit</a>
                                        <div class="clear"></div>
                                    </div>
                                </div>
                                <div id="dealer-assist-msg" class="hide">
                                    <p class="font14 leftfloat">Thank you for your interest. <%= dealerName %> will get in touch shortly</p>
                                    <span class="assistance-response-close bwsprite cross-lg-lgt-grey cur-pointer rightfloat"></span>
                                    <div class="clear"></div>
                                </div>
                            </div>

                            <!-- lead capture popup start-->
                            <div id="leadCapturePopup" class="text-center rounded-corner2">
                                <div class="leadCapture-close-btn position-abt pos-top10 pos-right10 bwsprite cross-lg-lgt-grey cur-pointer"></div>
                                <!-- contact details starts here -->
                                <div id="contactDetailsPopup">
                                    <div class="icon-outer-container rounded-corner50">
                                        <div class="icon-inner-container rounded-corner50">
                                            <span class="bwsprite user-contact-details-icon margin-top25"></span>
                                        </div>
                                    </div>
                                    <p class="font20 margin-top25 margin-bottom10">Provide contact details</p>
                                    <p class="text-light-grey margin-bottom20">For you to see more details about this bike, please submit your valid contact details. It will be safe with us.</p>
                                    <div class="personal-info-form-container">
                                        <div class="form-control-box personal-info-list">
                                            <input type="text" class="form-control get-first-name" placeholder="Full name (mandatory)"
                                                id="getFullName" data-bind="textInput: fullName">
                                            <span class="bwsprite error-icon errorIcon"></span>
                                            <div class="bw-blackbg-tooltip errorText">Please enter your first name</div>
                                        </div>
                                        <div class="form-control-box personal-info-list">
                                            <input type="text" class="form-control get-email-id" placeholder="Email address (mandatory)"
                                                id="getEmailID" data-bind="textInput: emailId">
                                            <span class="bwsprite error-icon errorIcon"></span>
                                            <div class="bw-blackbg-tooltip errorText">Please enter email address</div>
                                        </div>
                                        <div class="form-control-box personal-info-list">
                                            <p class="mobile-prefix">+91</p>
                                            <input type="text" class="form-control padding-left40 get-mobile-no" placeholder="Mobile no. (mandatory)"
                                                id="getMobile" maxlength="10" data-bind="textInput: mobileNo">
                                            <span class="bwsprite error-icon errorIcon"></span>
                                            <div class="bw-blackbg-tooltip errorText">Please enter mobile number</div>
                                        </div>
                                        <div class="clear"></div>
                                        <a class="btn btn-orange margin-top10" id="user-details-submit-btn" data-bind="event: { click: submitLead }">Submit</a>
                                    </div>
                                </div>
                                <!-- contact details ends here -->
                                <!-- otp starts here -->
                                <div id="otpPopup">
                                    <div class="icon-outer-container rounded-corner50">
                                        <div class="icon-inner-container rounded-corner50">
                                            <span class="bwsprite otp-icon margin-top25"></span>
                                        </div>
                                    </div>
                                    <p class="font18 margin-top25 margin-bottom20">Verify your mobile number</p>
                                    <p class="font14 text-light-grey margin-bottom20">We have sent an OTP on the following mobile number. Please enter that OTP in the box provided below:</p>
                                    <div>
                                        <div class="lead-mobile-box lead-otp-box-container font22">
                                            <span class="fa fa-phone"></span>
                                            <span class="text-light-grey font24">+91</span>
                                            <span class="lead-mobile font24"></span>
                                            <span class="bwsprite edit-blue-icon edit-mobile-btn"></span>
                                        </div>
                                        <div class="otp-box lead-otp-box-container">
                                            <div class="form-control-box margin-bottom10">
                                                <input type="text" class="form-control" maxlength="5" placeholder="Enter your OTP" id="getOTP" data-bind="value: otpCode">
                                                <span class="bwsprite error-icon errorIcon"></span>
                                                <div class="bw-blackbg-tooltip errorText"></div>
                                            </div>
                                            <a class="resend-otp-btn margin-left10 blue rightfloat resend-otp-btn" id="resendCwiCode" data-bind="visible: (NoOfAttempts() < 2), click: function () { regenerateOTP() }">Resend OTP
                                            </a>
                                            <p class="otp-alert-text margin-left10 otp-notify-text text-light-grey font12 margin-top10" data-bind="visible: (NoOfAttempts() >= 2)">
                                                OTP has been already sent to your mobile
                                            </p>
                                            <div class="clear"></div>
                                            <input type="button" class="btn btn-orange margin-top20" value="Confirm OTP" id="otp-submit-btn">
                                        </div>
                                        <div class="update-mobile-box">
                                            <div class="form-control-box text-left">
                                                <p class="mobile-prefix">+91</p>
                                                <input type="text" class="form-control padding-left40" placeholder="Mobile no." maxlength="10" id="getUpdatedMobile" data-bind="value: mobileNo" />
                                                <span class="bwsprite error-icon errorIcon"></span>
                                                <div class="bw-blackbg-tooltip errorText"></div>
                                            </div>
                                            <input type="button" class="btn btn-orange" value="Send OTP" id="generateNewOTP" data-bind="event: { click: submitLead }" />
                                        </div>
                                    </div>
                                </div>
                                <!-- otp ends here -->
                                <div id="dealer-lead-msg" class="hide">
                                    <div class="icon-outer-container rounded-corner50">
                                        <div class="icon-inner-container rounded-corner50">
                                            <span class="bwsprite otp-icon margin-top25"></span>
                                        </div>
                                    </div>
                                    <p class="font18 margin-top25 margin-bottom20">Thank you for providing your details. <%= dealerName %>, <%= dealerArea %> will get in touch with you soon.</p>

                                    <a href="javascript:void(0)" class="btn btn-orange okay-thanks-msg">Okay</a>
                                </div>
                            </div>
                            <!-- lead capture popup End-->
                        </div>

                        <div class="clear"></div>

                        <% if (dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium)
                           { %>

                        <% if (isUSPBenfits)
                           { %>
                        <!--  Dealer Benefits starts-->
                        <div class="grid-12 padding-top20 padding-right20 padding-bottom5 padding-left20 font14">
                            <p class="text-bold margin-bottom5">Benefits of buying a bike from this dealer:</p>
                            <ul class="pricequote-benefits-list text-light-grey">
                                <asp:Repeater ID="rptUSPBenefits" runat="server">
                                    <ItemTemplate>
                                        <li>
                                            <span class="inline-block pq-benefits-image pricequote-sprite <%#  "benifitIcon_" + DataBinder.Eval(Container.DataItem,"CatId") %> margin-right10"></span>
                                            <span class="inline-block pq-benefits-title"><%#  DataBinder.Eval(Container.DataItem,"BenefitText") %></span>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                            <div class="clear"></div>
                        </div>
                        <div class="clear"></div>
                        <!--  Dealer Benefits  ends -->
                        <% } %>

                        <%if (primarydealer.IsBookingAvailable)
                          { %>
                        <!--  Booking availability starts-->
                        <div class="grid-12 padding-left20 padding-right20 padding-bottom20 font14">
                            <p class="text-bold padding-top20 margin-bottom5 border-light-top">Pay <span class="fa fa-rupee"></span>&nbsp;<%= CommonOpn.FormatPrice(bookingAmount.ToString()) %> online and book this bike:</p>
                            <ul class="pricequote-benefits-list pq-benefits-booking-list text-light-grey">
                                <li class="bullet-point">
                                    <p>Save on dealer visits</p>
                                </li>
                                <li class="bullet-point">
                                    <p>Secure online payments</p>
                                </li>
                                <li class="bullet-point">
                                    <p>Complete buyer protection</p>
                                </li>
                            </ul>
                            <div class="clear"></div>
                            <div class="grid-12 alpha omega margin-top10">
                                <div class="grid-9 alpha">
                                    <p class="font14 text-light-grey">The booking amount of <span class="fa fa-rupee"></span>&nbsp;<%= CommonOpn.FormatPrice(bookingAmount.ToString()) %> has to be paid online and balance amount of <span class="fa fa-rupee"></span>&nbsp;<%= CommonOpn.FormatPrice((totalPrice - bookingAmount).ToString()) %> has to be paid at the dealership.</p>
                                </div>
                                <div class="grid-3 omega text-right">
                                    <a href="/pricequote/bookingsummary_new.aspx?MPQ=<%= mpqQueryString %>" class="btn btn-grey btn-sm font14">Book now</a>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <div class="clear"></div>
                        <!--  Booking availability ends-->
                        <%} %>

                        <% if (primarydealer.EMIDetails != null)
                           { %>
                        <!-- EMI section starts -->
                        <div id="EMISection" data-bind="visible: true" style="display: none" class="grid-12 padding-left20 padding-right20 padding-bottom20 font14">
                            <p class="text-bold padding-top20 margin-bottom15 border-light-top">Get EMI quote from this dealer:</p>
                            <div class="finance-emi-container">
                                <div class="emi-slider-box">
                                    <div class="emi-slider-box-left-section">
                                        <p>Down payment</p>
                                        <div id="downPaymentSlider"
                                            data-bind="slider: downPayment, sliderOptions: { min: minDnPay(), max: maxDnPay(), range: 'min', step: 1, value: Math.round(((maxDnPay() - minDnPay()) / 2 ) + minDnPay()) }"
                                            class="ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all">
                                            <div class="ui-slider-range ui-widget-header ui-corner-all ui-slider-range-min"></div>
                                            <span class="ui-slider-handle ui-state-default ui-corner-all" tabindex="0"></span>
                                        </div>
                                        <div class="slider-range-points">
                                            <ul class="range-five-pointsUL range-pointsUL" data-bind="">
                                                <li class="range-points-bar"><span data-bind="text: $.valueFormatter(minDnPay())"></span></li>
                                                <!-- ko foreach: new Array(breakPoints() - 1 ) -->
                                                <li class="range-points-bar"><span data-bind="text: $.createSliderPoints($index() + 1, $parent.minDnPay(), $parent.maxDnPay(), $parent.breakPoints(),1)"></span></li>
                                                <!-- /ko -->
                                                <li class="range-points-bar" style="width: 1px; float: right; margin-top: -5px"><span data-bind="text: $.valueFormatter(maxDnPay())"></span></li>

                                            </ul>
                                        </div>
                                    </div>
                                    <div class="emi-slider-box-right-section font16">
                                        <span class="fa fa-rupee"></span>&nbsp;
                                            <span id="downPaymentAmount" class="text-bold" data-bind="text: formatPrice(Math.round(downPayment()))"></span>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <div class="emi-slider-box">
                                    <div class="emi-slider-box-left-section">
                                        <p>Loan Amount</p>
                                        <div id="loanAmountSlider"
                                            data-bind="slider: loan, sliderOptions: { min: bikePrice() - maxDnPay(), max: bikePrice() - minDnPay(), range: 'min', step: 1 }"
                                            class="ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all">
                                            <div class="ui-slider-range ui-widget-header ui-corner-all ui-slider-range-min"></div>
                                            <span class="ui-slider-handle ui-state-default ui-corner-all" tabindex="0"></span>
                                        </div>
                                        <div class="slider-range-points">
                                            <ul class="range-five-pointsUL range-pointsUL" data-bind="">
                                                <li class="range-points-bar"><span data-bind="text: $.valueFormatter(bikePrice() - maxDnPay())"></span></li>
                                                <!-- ko foreach: new Array(breakPoints() - 1 ) -->
                                                <li class="range-points-bar"><span data-bind="text: $.createSliderPoints($index() + 1, $parent.bikePrice() - $parent.maxDnPay(), $parent.bikePrice() - $parent.minDnPay(), $parent.breakPoints(),1)"></span></li>
                                                <!-- /ko -->
                                                <li class="range-points-bar" style="width: 1px; float: right; margin-top: -5px"><span data-bind="text: $.valueFormatter(bikePrice() - minDnPay())"></span></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="emi-slider-box-right-section font16">
                                        <span class="fa fa-rupee"></span>&nbsp;
                                            <span id="loanAmount" class="text-bold" data-bind="text: formatPrice(Math.round(loan()))"></span>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <div class="emi-slider-box">
                                    <div class="emi-slider-box-left-section">
                                        <p>Tenure (Months)</p>
                                        <div id="tenureSlider"
                                            data-bind="slider: tenure, sliderOptions: { min: minTenure(), max: maxTenure(), range: 'min', step: 1 }"
                                            class="ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all">
                                            <div class="ui-slider-range ui-widget-header ui-corner-all ui-slider-range-min"></div>
                                            <span class="ui-slider-handle ui-state-default ui-corner-all" tabindex="0"></span>
                                        </div>
                                        <div class="slider-range-points">
                                            <ul class="range-five-pointsUL  range-pointsUL tenure-rate-interest" data-bind="">
                                                <li class="range-points-bar"><span data-bind="text: $.valueFormatter(minTenure())"></span></li>
                                                <!-- ko foreach: new Array(breakPoints() - 1 ) -->
                                                <li class="range-points-bar"><span data-bind="text: $.createSliderPoints($index() + 1, $parent.minTenure(), $parent.maxTenure() , $parent.breakPoints(),2)"></span></li>
                                                <!-- /ko -->
                                                <li class="range-points-bar" style="width: 1px; float: right; margin-top: -5px"><span data-bind="text: $.valueFormatter(maxTenure())"></span></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="emi-slider-box-right-section">
                                        <span id="tenurePeriod" class="font16 text-bold" data-bind="text: tenure"></span>
                                        <span class="font12">Months</span>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <div class="emi-slider-box">
                                    <div class="emi-slider-box-left-section">
                                        <p>Rate of interest (Percentage)</p>
                                        <div id="rateOfInterestSlider"
                                            data-bind="slider: rateofinterest, sliderOptions: { min: minROI(), max: maxROI(), range: 'min', step: 0.25 }"
                                            class="ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all">
                                            <div class="ui-slider-range ui-widget-header ui-corner-all ui-slider-range-min"></div>
                                            <span class="ui-slider-handle ui-state-default ui-corner-all" tabindex="0"></span>
                                        </div>
                                        <div class="slider-range-points">
                                            <ul class="range-five-pointsUL range-pointsUL tenure-rate-interest.">
                                                <li class="range-points-bar"><span data-bind="text: $.valueFormatter(minROI())"></span></li>
                                                <!-- ko foreach: new Array(breakPoints() - 1 ) -->
                                                <li class="range-points-bar"><span data-bind="text:  $.createSliderPoints($index() + 1, $parent.minROI(), $parent.maxROI() , $parent.breakPoints())"></span></li>
                                                <!-- /ko -->
                                                <li class="range-points-bar" style="width: 1px; float: right; margin-top: -5px"><span data-bind="text: $.valueFormatter(maxROI())"></span></li>

                                            </ul>
                                        </div>
                                    </div>
                                    <div class="emi-slider-box-right-section font16">
                                        <span id="rateOfInterestPercentage" class="text-bold" data-bind="text: rateofinterest">5</span>
                                        <span>%</span>
                                    </div>
                                    <div class="clear"></div>
                                </div>

                                <div class="margin-top10">
                                    <div class="grid-8 alpha text-grey text-bold padding-top10">
                                        <p class="leftfloat margin-right10 position-rel pos-top3">Indicative EMI:</p>
                                        <div class="indicative-emi-amount margin-right10 leftfloat">
                                            <span class="font18"><span class="fa fa-rupee"></span>&nbsp;</span>
                                            <span id="emiAmount" class="font18" data-bind="text: monthlyEMI"></span>
                                        </div>
                                        <p class="font14 leftfloat position-rel pos-top3">per month</p>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="grid-4 omega text-right">
                                        <a id="btnEmiQuote" class="btn btn-grey btn-md font14">Get EMI quote</a>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </div>
                        </div>
                        <!-- EMI section ends  -->
                        <% } %>

                        <% } %>
                        <div class="clear"></div>
                        <%} %>
                        <div class="clear"></div>
                        <p id="disclaimerText" class="padding-left20 margin-top15 font11 text-light-grey padding-top20 padding-bottom20"><span class="bwsprite disclaimer-sm-icon"></span>On-road price <%= (dealerType != Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium)?"": "and EMI calculator" %> is provided for information. BikeWale does not own any responsibility for the same.</p>
                    </div>

                    <!--Primary Dealer Section-->
                    <div class="grid-4 alpha padding-top20 <%= primarydealer.DealerDetails != null ? "dealer-pointer" : "" %> " id="PQDealerSidebarContainer">
                        <div class="pqdealer-and-listing-container">
                            <div class="margin-left10">
                                <%if (primarydealer.DealerDetails != null)
                                  { %>
                                <div class="pqdealer-sidebar-panel position-rel">
                                    <p class="font18 text-bold text-darker-black"><%= dealerName %></p>
                                    <p class="font14 text-light-grey margin-bottom15"><%= dealerArea %></p>
                                    <% if (dealerType != Bikewale.Entities.PriceQuote.DealerPackageTypes.Standard || !String.IsNullOrEmpty(maskingNum))
                                       { %>
                                    <div class="border-solid-top padding-top15">
                                        <%if (dealerType != Bikewale.Entities.PriceQuote.DealerPackageTypes.Standard)
                                          { %>
                                        <p class="font14 text-light-grey margin-bottom10"><%= dealerAddress %></p>
                                        <%} %>
                                        <% if (!string.IsNullOrEmpty(maskingNum))
                                           { %>
                                        <p class="font16 text-bold"><span class="fa fa-phone"></span>&nbsp;<%= maskingNum %></p>
                                        <%} %>
                                        <%if (dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium)
                                          { %>
                                        <script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDY0kkJiTPVd2U7aTOAwhc9ySH6oHxOIYM"></script>
                                        <div id="dealerMap" class=" margin-top15 text-center position-rel" style="height: 100px">
                                            <img class="position-abs" src="http://imgd3.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif" />
                                        </div>
                                        <script type="text/javascript">
                                            function initializeDealerMap(element,latitude,longitude) {
                                                mapUrl  = "http://maps.google.com/?q=" + latitude + "," + longitude;
                                                latLng = new google.maps.LatLng(latitude, longitude),
                                                mapOptions = {
                                                    zoom: 13,
                                                    center: latLng,
                                                    mapTypeId: google.maps.MapTypeId.ROADMAP
                                                },
                                                map = new google.maps.Map(element, mapOptions),
                                                marker = new google.maps.Marker({
                                                    title: "Dealer's Location",
                                                    position: latLng,
                                                    map: map,
                                                    animation: google.maps.Animation.DROP
                                                });

                                                google.maps.event.addListener(marker, 'click', function() {
                                                    window.open(mapUrl, '_blank');
                                                });

                                                google.maps.event.addListener(map, 'click', function() {
                                                    window.open(mapUrl, '_blank');
                                                });
                                            }
                                            google.maps.event.addDomListener(window, 'load', initializeDealerMap($("#dealerMap")[0],<%= latitude %>,<%= longitude %>));

                                        </script>
                                        <% } %>
                                        <%if (dealerType != Bikewale.Entities.PriceQuote.DealerPackageTypes.Standard)
                                          { %>
                                        <a id="leadBtn" class="btn btn-inv-grey btn-full-width font14 margin-top15">Get offers from this dealer</a>
                                        <% } %>
                                    </div>
                                    <% } %>
                                </div>
                                <%} %>
                                <%else
                                  { %>
                                <div class="pq-no-premium-dealer font14 text-light-grey">Sorry, there are no dealers nearby</div>
                                <%} %>
                            </div>
                            <%if (detailedDealer != null && detailedDealer.SecondaryDealerCount > 0)
                              { %>
                            <div class="margin-left10 pq-sidebar-dealer-listing margin-top15 padding-right20 padding-left20">
                                <p class="padding-bottom15">Prices available from <%= detailedDealer.SecondaryDealerCount %> <%= (detailedDealer.SecondaryDealerCount > 1)?"more dealers":"more dealer" %> :</p>
                                <ul id="dealerList">
                                    <asp:Repeater ID="rptDealers" runat="server">
                                        <ItemTemplate>
                                            <li dealerid="<%# DataBinder.Eval(Container.DataItem,"dealerId") %>">
                                                <h3><a href="#" class="font18 text-bold text-darker-black margin-right20"><%# DataBinder.Eval(Container.DataItem,"Name") %></a></h3>
                                                <p class="font14 text-light-grey"><%# DataBinder.Eval(Container.DataItem,"Area") %></p>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </div>
                            <% } %>
                        </div>
                    </div>
                    <div class="clear"></div>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <section class="margin-bottom30 <%= (ctrlAlternativeBikes.FetchedRecordsCount > 0) ? string.Empty : "hide" %>">
            <div class="container">
                <div class="grid-12 alternative-section" id="alternative-bikes-section">
                    <h2 class="text-bold text-center margin-top20 margin-bottom30"><%= BikeName %> alternatives</h2>
                    <div class="content-box-shadow">
                        <div class="jcarousel-wrapper alternatives-carousel margin-top20">
                            <div class="jcarousel">
                                <ul>
                                    <BW:AlternativeBikes ID="ctrlAlternativeBikes" runat="server" />
                                </ul>
                            </div>
                            <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev"></a></span>
                            <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next"></a></span>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript">

            var bikeName = "<%= BikeName %>";
            var bikeVersionPrice = "<%= totalPrice %>";

            <% if (dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium)
               { %>  

            ko.bindingHandlers.slider = {
                init: function (element, valueAccessor, allBindingsAccessor, bindingContext) {
                    var options = allBindingsAccessor().sliderOptions || {};
                    $("#" + element.id).slider(options);
                    ko.utils.registerEventHandler("#" + element.id, "slide", function (event, ui) {
                        var observable = valueAccessor();
                        observable(ui.value);
                    });
                },
                update: function (element, valueAccessor, allBindingsAccessor, bindingContext) {
                    var options = allBindingsAccessor().sliderOptions || {};
                    $("#" + element.id).slider(options);
                    var value = ko.utils.unwrapObservable(valueAccessor());
                    if (isNaN(value)) value = 0;
                    $("#" + element.id).slider("value", value);
                }
            };

            var BikeEMI = function () {
                var self = this;
                self.breakPoints = ko.observable(5);
                self.bikePrice = ko.observable(bikeVersionPrice);
                self.minDnPay = ko.observable(<%= primarydealer.EMIDetails.MinDownPayment %> * bikeVersionPrice/100);
                self.maxDnPay = ko.observable(<%= primarydealer.EMIDetails.MaxDownPayment %> * bikeVersionPrice/100);
                self.minTenure = ko.observable(<%= primarydealer.EMIDetails.MinTenure %>);
                self.maxTenure = ko.observable(<%= primarydealer.EMIDetails.MaxTenure  %>);
                self.minROI = ko.observable(<%= primarydealer.EMIDetails.MinRateOfInterest %>);
                self.maxROI = ko.observable(<%= primarydealer.EMIDetails.MaxRateOfInterest %>);
                self.processingFees = ko.observable(<%= primarydealer.EMIDetails.ProcessingFee %>);
                self.exshowroomprice = ko.observable(bikeVersionPrice);
                self.loan = ko.observable();

                self.tenure = ko.observable((self.maxTenure() - self.minTenure())/2 + self.minTenure());
                self.rateofinterest = ko.observable((self.maxROI() - self.minROI())/2 + self.minROI());
                self.downPayment = ko.pureComputed({
                    read: function () {
                        if (self.loan() == undefined || isNaN(self.loan()) || self.loan() == null)
                            self.loan($.LoanAmount(self.exshowroomprice(), 70));
                        return (($.LoanAmount(self.exshowroomprice(), 100)) - self.loan());
                    },
                    write: function (value) {
                        self.loan((($.LoanAmount(self.exshowroomprice(), 100))) - value);
                    },
                    owner: this
                });

                self.monthlyEMI = ko.pureComputed({
                    read: function () {
                        return $.calculateEMI(self.loan(), self.tenure(), self.rateofinterest(),self.processingFees());
                    },
                    owner: this
                });
            }          


            $.calculateEMI = function (loanAmount, tenure, rateOfInterest,proFees) {
                var interest, totalRepay, finalEmi;
                try {
                    interest = (loanAmount * tenure * rateOfInterest) / (12 * 100);
                    totalRepay = loanAmount + interest;
                    finalEmi = Math.ceil((totalRepay / tenure) + proFees);
                }
                catch (e) {
                }
                return formatPrice(finalEmi);
            };

            $.createSliderPoints = function(index,min,max,breaks,sliderType)
            {   var svar = "";
                try {
                    switch(sliderType)
                    {
                        case 1: 
                            svar =  $.valueFormatter(Math.round(min + (index * (max - min)/breaks)));
                            break;
                        case 2:
                            svar =  Math.round(min + (index * (max - min)/breaks));
                            break;
                        default:
                            svar =  (min + (index * (max - min)/breaks)).toFixed(2);
                            break;
                    } 
                } catch (e) {
    
                }
                return svar;
            }

            $.LoanAmount = function (onRoadPrice, percentage) {
                var price;
                try {
                    price = (onRoadPrice * percentage) / 100;
                    price = Math.ceil(price / 100.0) * 100;
                }
                catch (e) {
                }
                return price;
            };

            $.valueFormatter = function (num) {
                if (num >= 100000) {
                    return (num / 100000).toFixed(1).replace(/\.0$/, '') + 'L';
                }
                if (num >= 1000) {
                    return (num / 1000).toFixed(1).replace(/\.0$/, '') + 'K';
                }
                return num;
            }

            var EMIviewModel = new BikeEMI;
            ko.applyBindings(EMIviewModel, $("#EMISection")[0]);

            <% } %>
            
            $("#dealer-assist-msg .assistance-response-close").on("click", function(){
                $("#dealer-assist-msg").parent().slideUp();
            });

            $("#dealer-lead-msg .okay-thanks-msg").on("click", function(){
                $(".leadCapture-close-btn").click();
            });

            
            $('#btnGetDealerDetails, #btnBikeBooking').on("click", function () {
                var cookieValue = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + versionId + "&DealerId=" + dealerId;
                window.location.href = '/pricequote/bookingsummary_new.aspx?MPQ=' + Base64.encode(cookieValue);
            });

            $("#leadBtnBookNow").on("click", function () {
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Dealer_PQ', 'act': 'Get_More_Details_Clicked_Button', 'lab': bikeName + '_' + getCityArea });
            });

            $("#leadLink").on("click", function () {
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Dealer_PQ', 'act': 'Get_More_Details_Clicked_Link', 'lab': bikeName + '_' + getCityArea });
            });

            $("input[name*='btnVariant']").on("click", function () {
                if ($(this).attr('versionid') == $('#hdnVariant').val()) {
                    return false;
                }
                $('#hdnVariant').val($(this).attr('title'));
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Version_Change', 'lab': bikeVersionLocation });
            });

            $("input[name*='switchDealer']").on("click", function () {
                if ($(this).attr('dealerId') == $('#hdnDealerId').val()) {
                    return false;
                }
                $('#hdnDealerId').val($(this).attr('title'));
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Version_Change', 'lab': bikeVersionLocation });
            });
            $("#dealerList li").on("click", function(){
                registerPQ($(this).attr('dealerId'));
            });

            function registerPQ(secondaryDealerId) {
                var obj = {
                    'cityId': cityId,
                    'areaId': areaId,
                    'clientIP': clientIP,
                    'sourceType': <%=Bikewale.Utility.BWConfiguration.Instance.SourceId%>,
                    'versionId': versionId,
                    'pQLeadId': eval("<%= Convert.ToInt16(Bikewale.Entities.BikeBooking.LeadSourceEnum.DealerPQ_Mobile) %>"),
                    'deviceId': getCookie('BWC'),
                    'dealerId': secondaryDealerId
                };
                $.ajax({
                    type: 'POST',
                    url: "/api/RegisterPQ/",
                    data: obj,
                    dataType: 'json',
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader('utma', getCookie('__utma'));
                        xhr.setRequestHeader('utmz', getCookie('__utmz'));
                    },
                    success: function (json) {
                        var jsonObj = json;                                               
                        if (jsonObj != undefined && jsonObj.quoteId > 0 && jsonObj.dealerId > 0) {
                            cookieValue = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + jsonObj.quoteId + "&VersionId=" + versionId + "&DealerId=" + secondaryDealerId;
                            window.location.href = "/pricequote/dealerpricequote.aspx?MPQ=" + Base64.encode(cookieValue);
                        }
                        else {
                            window.location.href = "/pricequote/";
                        }
                    },
                    error: function (e) {
                        window.location = "/pricequote/";
                    }
                });
            }

            function formatPrice(price) {
                price = price.toString();
                var lastThree = price.substring(price.length - 3);
                var otherNumbers = price.substring(0, price.length - 3);
                if (otherNumbers != '')
                    lastThree = ',' + lastThree;
                var price = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
                return price;
            }
        </script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/dealerpricequote.js?<%= staticFileVersion %>"></script>
    </form>
</body>
</html>
