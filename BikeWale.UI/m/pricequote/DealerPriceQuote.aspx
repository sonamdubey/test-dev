<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.BikeBooking.DealerPriceQuote" Trace="false" Async="true" %>

<%@ Register Src="~/m/controls/AlternativeBikes.ascx" TagPrefix="BW" TagName="AlternateBikes" %>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Import Namespace="Bikewale.BikeBooking" %>
<!doctype html>
<html>
<head>
    <%
        title = String.Format("{0} {1} {2} Price Quote", objPriceQuote.objMake.MakeName, objPriceQuote.objModel.ModelName, objPriceQuote.objVersion.VersionName);
        description = String.Format("{0} {1} {2} price quote", objPriceQuote.objMake.MakeName, objPriceQuote.objModel.ModelName, objPriceQuote.objVersion.VersionName);
        keywords = "";
        canonical = "";
        AdPath = "/1017752/Bikewale_Mobile_PriceQuote";
        AdId = "1398766000399";
        PopupWidget.Visible = true;
    %>
    <script>var quotationPage = true;</script>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/css/dealerpricequote.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
    <script type="text/javascript">
        var dealerId = '<%= dealerId%>';
        var pqId = '<%= pqId%>';
        var ABHostUrl = '<%= System.Configuration.ConfigurationManager.AppSettings["ApiHostUrl"]%>';
        var versionId = '<%= versionId%>';
        var cityId = '<%= cityId%>';
        var Customername = "", email = "", mobileNo = "";
        var CustomerId = '<%= CurrentUser.Id %>';
        if (CustomerId != '-1') {
            Customername = '<%= objCustomer.CustomerName%>', email = '<%= objCustomer.CustomerEmail%>', mobileNo = '<%= objCustomer.CustomerMobile%>';
        } else {
            Customername = '<%= CustomerDetailCookie.CustomerName%>', email = '<%= CustomerDetailCookie.CustomerEmail%>', mobileNo = '<%= CustomerDetailCookie.CustomerMobile %>';
        }
        var clientIP = "<%= clientIP%>";
        var pageUrl = "<%= Bikewale.Utility.BWConfiguration.Instance.BwHostUrl %>" + "/quotation/dealerpricequote.aspx?versionId=" + versionId + "&cityId=" + cityId;       

    </script>
    <style type="text/css">
        
    </style>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <div class="bg-white pq-inner-block-10 bottom-shadow">
            <div class="bike-name-image-wrapper margin-top5">
                <div class="bike-img">
                    <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(objPriceQuote.OriginalImagePath,objPriceQuote.HostUrl,Bikewale.Utility.ImageSize._110x61) %>" alt="" title="" border="0" />
                </div>
                <h1 class="padding-left10 font18 text-dark-black"><%= objPriceQuote.objMake.MakeName + " " + objPriceQuote.objModel.ModelName %></h1>
            </div>
            <div class="clear"></div>

           
            <div id="versionsDropdownWrapper" class="margin-top10 padding-right10 padding-left10">
                <p class="grid-3 alpha omega version-label-text font14 text-light-grey margin-top5 leftfloat">Version:</p>
                 <%if (versionList!=null && versionList.Count > 1)
                   { %>
                <asp:DropDownList ID="ddlVersion" CssClass="form-control" runat="server" AutoPostBack="true"></asp:DropDownList>
                <%} 
                  else if(objPriceQuote.objVersion != null){ %>
                <span id='versText' class="margin-left10 font14 text-light-grey leftfloat margin-top7 text-light-grey margin-right20 text-bold"><%= objPriceQuote.objVersion.VersionName %></span>
                <%} %>
            </div>
            
            <!--Price Breakup starts here-->
            <div class="margin-top15 padding-left10 padding-right10">
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="pqTable font14">
                    <asp:Repeater ID="rptPriceList" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td align="left" width="75%" class="text-light-grey padding-bottom15"><%# DataBinder.Eval(Container.DataItem,"CategoryName") %> <%# Bikewale.common.DealerOfferHelper.HasFreeInsurance(dealerId.ToString(),"",DataBinder.Eval(Container.DataItem,"CategoryName").ToString(),Convert.ToUInt32(DataBinder.Eval(Container.DataItem,"Price").ToString()),ref insuranceAmount) ? "<img class='insurance-free-icon' alt='Free_icon' src='http://imgd1.aeplcdn.com/0x0/bw/static/free_red.png' title='Free_icon'/>" : "" %></td>
                                <td align="right" width="25%" class="padding-bottom15"><span class="bwmsprite inr-xxsm-icon"></span><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"Price").ToString()) %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr align="left">
                        <td height="1" colspan="2" class="break-line padding-bottom15"></td>
                    </tr>


                    <%
                        if (IsDiscount)
                        {
                    %>
                    <tr>
                        <td align="left" class="text-light-grey padding-bottom15">Total On Road Price</td>
                        <td align="right" class="padding-bottom15"><span class="bwmsprite inr-xxsm-icon"></span><span style="text-decoration: line-through"><%= Bikewale.Utility.Format.FormatPrice(totalPrice.ToString()) %></span></td>
                    </tr>
                    <asp:Repeater ID="rptDiscount" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td align="left" class="text-light-grey padding-bottom15">Minus <%# DataBinder.Eval(Container.DataItem,"CategoryName") %> <%# Bikewale.common.DealerOfferHelper.HasFreeInsurance(dealerId.ToString(),"",DataBinder.Eval(Container.DataItem,"CategoryName").ToString(),Convert.ToUInt32(DataBinder.Eval(Container.DataItem,"Price").ToString()),ref insuranceAmount) ? "<img class='insurance-free-icon' alt='Free_icon' src='http://imgd1.aeplcdn.com/0x0/bw/static/free_red.png' title='Free_icon'/>" : "" %></td>
                                <td align="right" class="padding-bottom15"><span class="bwmsprite inr-xxsm-icon"></span><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"Price").ToString()) %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr align="left">
                        <td height="1" colspan="2" class="break-line padding-bottom15"></td>
                    </tr>
                    <tr>
                        <td align="left" class="text-dark-black padding-bottom15">On-road price</td>
                        <td align="right" class="text-dark-black padding-bottom15">
                            <div><span class="bwmsprite inr-xxsm-icon"></span><%= Bikewale.Utility.Format.FormatPrice((totalPrice - totalDiscount).ToString()) %></div>
                        </td>
                    </tr>
                    <%
                        }
                        else
                        {%>
                    <tr>
                        <td align="left" class="text-dark-black padding-bottom15">Total On Road Price</td>
                        <td align="right" class="text-dark-black padding-bottom15">
                            <div><span class="bwmsprite inr-xxsm-icon"></span><%= Bikewale.Utility.Format.FormatPrice(totalPrice.ToString()) %></div>

                        </td>
                    </tr>
                    <%
                        }
                    %>
                </table>
            </div>
            <!--Price Breakup ends here-->

            <%if(isPrimaryDealer){ %>
            <!-- Dealer Widget starts here -->
            <div id="pqDealerDetails">
                <!-- hide this div when no premium dealer -->
                <div id="pqDealerHeader">
                    <div class="padding-top7 padding-right10 padding-left10 border-trl">
                        <h2 class="dealership-name font18 text-dark-black"><%= dealerName %></h2>
                    </div>
                </div>
                <div id="pqDealerBody" class="font14 padding-right10 padding-left10 border-rbl">
                    <p class="font14 text-light-grey padding-bottom10 margin-bottom15 border-light-bottom"><%= dealerArea %></p>
                    <% if (dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium || dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Deluxe)
                       {%>
                    <p class="text-light-grey margin-bottom5"><%= dealerAdd %></p>
                    <%} %>
                    <%if (!string.IsNullOrEmpty(maskingNum))
                      { %>
                    <p class="margin-bottom15"><span class="bwmsprite tel-sm-icon"></span><%= maskingNum %></p>
                    <%} %>
                    <%if (dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium)
                      { %>
                    <script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDY0kkJiTPVd2U7aTOAwhc9ySH6oHxOIYM"></script>
                    <div id="dealerMap" style="height: 100px; position: relative; text-align: center">
                        <img src="http://imgd3.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif" />
                    </div>
                    <script type="text/javascript">
                        function initializeDealerMap(element, latitude, longitude) {
                            try {
                                mapUrl = "http://maps.google.com/?q=" + latitude + "," + longitude;
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

                                google.maps.event.addListener(marker, 'click', function () {
                                    window.open(mapUrl, '_blank');
                                });

                                google.maps.event.addListener(map, 'click', function () {
                                    window.open(mapUrl, '_blank');
                                });

                                google.maps.event.addListenerOnce(map, 'idle', function () {
                                });
                            } catch (e) {
                                return;
                            }
                        }
                        google.maps.event.addDomListener(window, 'load', initializeDealerMap($("#dealerMap")[0],'<%= latitude %>','<%= longitude %>'));
                    </script>
                    <%} %>
                    <%if (isOfferAvailable && (dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium || dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Deluxe))
                      { %>
                    <div class="padding-top15 padding-bottom15 border-light-top">
                        <span class="font15 text-bold"><%=offerCount%> <%= offerCount == 1 ? "offer" : "offers" %> available</span>
                        <span class="text-link view-offers-target">View offers</span>
                    </div>
                    <div id="offersPopup" class="bwm-fullscreen-popup text-center padding-top30">
                        <div class="offers-popup-close-btn position-abt pos-top10 pos-right10 bwmsprite cross-lg-lgt-grey cur-pointer"></div>
                        <div class="icon-outer-container rounded-corner50percent">
                            <div class="icon-inner-container rounded-corner50percent">
                                <span class="bwmsprite offers-box-icon margin-top20"></span>
                            </div>
                        </div>
                        <p class="font16 text-bold margin-top25 margin-bottom20">Exclusive offers on this bike</p>
                        <ul class="pricequote-benefits-list text-light-grey">
                            <asp:Repeater ID="rptOffers" runat="server">
                                <ItemTemplate>
                                    <li>
                                        <span class="offer-benefit-sprite offerIcon_<%# DataBinder.Eval(Container.DataItem,"OfferCategoryId") %>"></span>
                                        <span class="pq-benefits-title padding-top5 padding-left15"><%# DataBinder.Eval(Container.DataItem,"OfferText") %></span>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <%} %>
                    <%if (isUSPAvailable && (dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium))
                      { %>
                    <div class="border-light-top padding-top15 padding-bottom15">
                        <p class="font15 text-bold margin-bottom15">Benefits of buying from this dealer:</p>
                        <ul class="pricequote-benefits-list text-light-grey text-left">
                            <asp:Repeater ID="rptBenefits" runat="server">
                                <ItemTemplate>
                                    <li>
                                        <span class="offer-benefit-sprite benifitIcon_<%# DataBinder.Eval(Container.DataItem,"CatId") %>"></span>
                                        <span class="pq-benefits-title padding-left15"><%# DataBinder.Eval(Container.DataItem,"BenefitText") %></span>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <%} %>
                    <%if (isBookingAvailable)
                      {%>
                    <div class="padding-top15 padding-bottom15 border-light-top">
                        <p class="font15 text-bold margin-bottom10">Pay <span class="bwmsprite inr-xxsm-icon"></span><%=Bikewale.Utility.Format.FormatPrice((objPriceQuote.PrimaryDealer.BookingAmount).ToString()) %> online and book bike:</p>
                        <p class="text-light-grey margin-bottom20">The booking amount of <span class="bwmsprite inr-grey-xxsm-icon"></span><%=Bikewale.Utility.Format.FormatPrice((objPriceQuote.PrimaryDealer.BookingAmount).ToString()) %> has to be paid online and balance amount of <span class="bwmsprite inr-grey-xxsm-icon"></span><%=Bikewale.Utility.Format.FormatPrice((totalPrice - objPriceQuote.PrimaryDealer.BookingAmount).ToString()) %> has to be paid at the dealership</p>
                        <a href="/m/pricequote/bookingsummary_new.aspx?MPQ=<%= mpqQueryString %>" class="btn btn-grey btn-full-width">Book now</a>
                    </div>
                    <%} %>
                    <%if (dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium){ %>
                    <div class="padding-top15 padding-bottom15 border-light-top">
                        <span class="font15 text-bold leftfloat">Get EMI quote</span>
                        <span class="text-link rightfloat calculate-emi-target">Calculate now</span>
                        <div class="clear"></div>
                    </div>
                    <%} %>
                    <div id="pqRemoveHeader"></div>
                </div>
            </div>
            <!-- show below div when no premium dealer -->
            <%} else { %>
            <div class="font14 text-light-grey border-solid padding-top20 padding-right10 padding-bottom20 padding-left10">Sorry, there are no dealers nearby</div>
            <%} %>
            <%if (isSecondaryDealer)
              {%>
            <div id="pqMoreDealers" class="padding-top15 padding-right10 padding-left10">
                <p class="font14 text-bold margin-bottom15">Prices available from <%= secondaryDealersCount == 1 ? secondaryDealersCount + " more dealer" : secondaryDealersCount + " more dealers" %> :</p>
                <ul class="pq-dealer-listing">
                    <%--bind secondary dealers--%>
                    <asp:Repeater ID="rptSecondaryDealers" runat="server">
                        <ItemTemplate>
                            <li>
                                <a href="#" dealerid="<%# DataBinder.Eval(Container.DataItem,"DealerId") %>" class="secondary-dealer font18 text-darker-black text-bold"><%# DataBinder.Eval(Container.DataItem,"Name") %></a><br />
                                <p class="font14 text-light-grey"><%# DataBinder.Eval(Container.DataItem,"Area") %></p>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
            <%} %>
            <!-- Dealer Widget ends here -->

            <!--Exciting Offers section starts here-->
            <div class="grid-12 float-button float-fixed">
                <%if (!string.IsNullOrEmpty(maskingNum))
                  { %>
                <div class="grid-6 alpha omega">
                    <a id="calldealer" class="btn btn-grey btn-full-width btn-sm rightfloat" href="tel:<%= maskingNum %>"><span class="bwmsprite tel-grey-icon margin-right5"></span>Call dealer</a>
                </div>
                <%} %>
                <div class="<%= !string.IsNullOrEmpty(maskingNum) ? "grid-6 omega" : "" %>">
                    <input type="button" data-role="none" id="leadBtnBookNow" name="leadBtnBookNow" class="btn btn-sm btn-full-width btn-orange" value="Get offers" />
                </div>
            </div>
            <div class="clear"></div>
            <!--Exciting Offers section ends here-->
        </div>
        <%if (dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium){ %>
        <div id="emiPopup" data-bind="visible: true" style="display: none" class="bwm-fullscreen-popup text-center padding-top30">
            <div class="emi-popup-close-btn position-abt pos-top10 pos-right10 bwmsprite cross-lg-lgt-grey cur-pointer"></div>
            <div class="icon-outer-container rounded-corner50percent">
                <div class="icon-inner-container rounded-corner50percent">
                    <span class="offer-benefit-sprite cal-emi-icon margin-top20"></span>
                </div>
            </div>
            <p class="font16 text-bold margin-top25 margin-bottom10">EMI Calculator</p>
            <div class="finance-emi-container">
                <div class="finance-emi-left-box alpha">
                    <div class="emi-slider-box">
                        <div class="emi-slider-box-left-section">
                            <div class="clearfix font14">
                                <p class="grid-8 alpha text-light-grey text-left">Down payment</p>
                                <div class="emi-slider-box-right-section grid-4 omega">
                                    <span class="bwmsprite inr-xxsm-icon"></span>
                                    <span id="downPaymentAmount" data-bind="text: formatPrice(Math.round(downPayment()))"></span>
                                </div>
                            </div>
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
                        <div class="clear"></div>
                    </div>

                    <div class="emi-slider-box">
                        <div class="emi-slider-box-left-section">
                            <div class="clearfix font14">
                                <p class="grid-8 alpha text-light-grey text-left">Loan Amount</p>
                                <div class="emi-slider-box-right-section grid-4 omega">
                                    <span id="loanAmount" data-bind="text: formatPrice(Math.round(loan()))"></span>
                                </div>
                            </div>
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
                                    <li class="range-points-bar"><span data-bind="text:  $.createSliderPoints($index() + 1, $parent.bikePrice() - $parent.maxDnPay(), $parent.bikePrice() - $parent.minDnPay(), $parent.breakPoints(),1)"></span></li>
                                    <!-- /ko -->
                                    <li class="range-points-bar" style="width: 1px; float: right; margin-top: -5px"><span data-bind="text: $.valueFormatter(bikePrice() - minDnPay())"></span></li>
                                </ul>
                            </div>
                        </div>
                        <div class="clear"></div>
                    </div>


                    <div class="emi-slider-box">
                        <div class="emi-slider-box-left-section">
                            <div class="clearfix font14">
                                <p class="grid-8 alpha text-light-grey text-left">Tenure</p>
                                <div class="emi-slider-box-right-section grid-4 omega">
                                    <span id="tenurePeriod" data-bind="text: tenure"></span>
                                    <span class="font12">Months</span>
                                </div>
                            </div>
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
                        <div class="clear"></div>
                    </div>


                    <div class="emi-slider-box">
                        <div class="emi-slider-box-left-section">
                            <div class="clearfix font14">
                                <p class="grid-8 alpha text-light-grey text-left">Rate of interest</p>
                                <div class="emi-slider-box-right-section grid-4 omega">
                                    <span id="rateOfInterestPercentage" data-bind="text: rateofinterest">5</span>
                                    <span>%</span>
                                </div>
                            </div>
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
                                    <li class="range-points-bar"><span data-bind="text: $.createSliderPoints($index() + 1, $parent.minROI(), $parent.maxROI() , $parent.breakPoints())"></span></li>
                                    <!-- /ko -->
                                    <li class="range-points-bar" style="width: 1px; float: right; margin-top: -5px"><span data-bind="text: $.valueFormatter(maxROI())"></span></li>

                                </ul>
                            </div>
                        </div>
                        <div class="clear"></div>
                    </div>


                </div>
                <div class="finance-emi-right-box omega margin-top15 margin-bottom25">
                    <div class="clearfix">
                        <p class="grid-8 font14 text-left alpha position-rel pos-top2">Indicative EMI<span class="font12 text-light-grey"> (per month)</span></p>
                        <div class="indicative-emi-amount text-right grid-4 omega">
                            <span class="bwmsprite inr-xsm-icon"></span>
                            <span id="emiAmount" class="font18" data-bind="text: monthlyEMI"></span>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
                <a id="btnEmiQuote" class="btn btn-orange emi-quote-btn margin-bottom20">Get EMI quote</a>
            </div>
        </div>
        <%} %>

        <section class="<%= (ctrlAlternateBikes.FetchedRecordsCount > 0) ? "" : "hide" %>">
            <div class="container margin-bottom30">
                <div class="grid-12">
                    <h2 class="margin-top30px margin-bottom20 text-center padding-top20"><%= objPriceQuote.objMake.MakeName + " " + objPriceQuote.objModel.ModelName  %> alternatives</h2>

                    <div class="swiper-container discover-bike-carousel alternatives-carousel padding-bottom60">
                        <div class="swiper-wrapper">
                            <BW:AlternateBikes ID="ctrlAlternateBikes" runat="server" />
                        </div>
                        <!-- Add Pagination -->
                        <div class="swiper-pagination"></div>
                        <!-- Navigation -->
                        <div class="bwmsprite swiper-button-next hide"></div>
                        <div class="bwmsprite swiper-button-prev hide"></div>
                    </div>

                </div>
                <div class="clear"></div>
            </div>
        </section>

        <!-- Lead Capture pop up start  -->
        <div id="leadCapturePopup" class="bw-popup bwm-fullscreen-popup contact-details hide">
            <div class="popup-inner-container text-center">
                <div class="bwmsprite close-btn leadCapture-close-btn rightfloat"></div>
                <div id="contactDetailsPopup">
                    <!-- Contact details Popup starts here -->
                    <h2 class="margin-top10 margin-bottom10">Provide contact details</h2>
                    <p class="text-light-grey margin-bottom10">Dealership will get back to you with offers</p>

                    <div class="personal-info-form-container">
                        <div class="form-control-box">
                            <input type="text" class="form-control get-first-name" placeholder="Your name" id="getFullName" data-bind="value: fullName">
                            <span class="bwmsprite error-icon "></span>
                            <div class="bw-blackbg-tooltip errorText">Please enter your name</div>
                        </div>
                        <div class="form-control-box margin-top20">
                            <input type="text" class="form-control get-email-id" placeholder="Email address" id="getEmailID" data-bind="value: emailId">
                            <span class="bwmsprite error-icon"></span>
                            <div class="bw-blackbg-tooltip errorText">Please enter your email adress</div>
                        </div>
                        <div class="form-control-box margin-top20">
                            <p class="mobile-prefix">+91</p>
                            <input type="text" class="form-control get-mobile-no" maxlength="10" placeholder="Mobile no." id="getMobile" data-bind="value: mobileNo">
                            <span class="bwmsprite error-icon"></span>
                            <div class="bw-blackbg-tooltip errorText">Please enter mobile number</div>
                        </div>
                        <div class="clear"></div>
                        <a class="btn btn-full-width btn-orange margin-top20" id="user-details-submit-btn" data-bind="event: { click: submitLead }">Submit</a>
                    </div>
                    <input type="button" class="btn btn-full-width btn-orange hide rounded-corner5" value="Submit" onclick="validateDetails();" data-role="none" id="btnSubmit" />
                </div>
                <!--thank you message starts here -->
                <div id="dealer-assist-msg" class="hide">
                    <div class="icon-outer-container rounded-corner50">
                        <div class="icon-inner-container rounded-corner50">
                            <span class="bwsprite otp-icon margin-top25"></span>
                        </div>
                    </div>
                    <p class="font18 margin-top25 margin-bottom20">Thank you for providing your details. <%= dealerName %>, <%= dealerArea %> will get in touch with you soon.</p>

                    <a href="javascript:void(0)" id="aOkayButton" class="btn btn-orange okay-thanks-msg">Okay</a>
                    <div class="clear"></div>
                </div>
                <!-- Contact details Popup ends here -->
                <div id="otpPopup">
                    <!-- OTP Popup starts here -->
                    <p class="font18 margin-top10 margin-bottom10">Verify your mobile number</p>
                    <p class="font14 text-light-grey margin-bottom10">We have sent OTP on your mobile. Please enter that OTP in the box provided below:</p>
                    <div>
                        <div class="lead-mobile-box lead-otp-box-container margin-bottom10 font22">
                            <span class="bwmsprite tel-grey-icon"></span>
                            <span class="text-light-grey font24">+91</span>
                            <span class="lead-mobile font24"></span>
                            <span class="bwmsprite edit-blue-icon edit-mobile-btn"></span>
                        </div>
                        <div class="otp-box lead-otp-box-container">
                            <div class="form-control-box margin-bottom10">
                                <input type="text" class="form-control" placeholder="Enter your OTP" id="getOTP" maxlength="5" data-bind="value: otpCode" />
                                <span class="bwmsprite error-icon errorIcon"></span>
                                <div class="bw-blackbg-tooltip errorText"></div>
                            </div>
                            <a class="margin-left10 blue resend-otp-btn margin-top10" id="resendCwiCode" data-bind="visible: (NoOfAttempts() < 2), click: function () { regenerateOTP() }">Resend OTP</a>
                            <p class="margin-left10 margin-top10 otp-notify-text text-light-grey font12" data-bind="visible: (NoOfAttempts() >= 2)">
                                OTP has been already sent to your mobile
                            </p>
                            <a class="btn btn-full-width btn-orange margin-top20" id="otp-submit-btn">Confirm</a>
                        </div>
                        <div class="update-mobile-box">
                            <div class="form-control-box text-left">
                                <p class="mobile-prefix">+91</p>
                                <input type="text" class="form-control padding-left40" placeholder="Mobile no." maxlength="10" id="getUpdatedMobile" data-bind="value: mobileNo" />
                                <span class="bwmsprite error-icon errorIcon"></span>
                                <div class="bw-blackbg-tooltip errorText"></div>
                            </div>
                            <input type="button" class="btn btn-orange margin-top20" value="Send OTP" id="generateNewOTP" data-bind="event: { click: submitLead }"/>
                        </div>
                    </div>

                </div>
                <!-- OTP Popup ends here -->
            </div>
        </div>
        <!-- Lead Capture pop up end  -->

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/dealerpricequote.js?<%= staticFileVersion %>"></script>

        <script type="text/javascript">
            var bikeName = "<%= BikeName %>";
            var bikeVersionPrice = "<%= totalPrice %>";
            var getCityArea = GetGlobalCityArea();
            var areaId = '<%= areaId %>';
            $('#getDealerDetails,#btnBookBike').click(function () {
                var cookieValue = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + versionId + "&DealerId=" + dealerId;                
                window.location.href = '/m/pricequote/bookingsummary_new.aspx?MPQ=' + Base64.encode(cookieValue);
            });

            
            var leadBtnBookNow = $("#leadBtnBookNow,#leadLink,#btnEmiQuote"), leadCapturePopup = $("#leadCapturePopup");
            var fullname = $("#getFullName");
            var emailid = $("#getEmailID");
            var mobile = $("#getMobile");
            var otpContainer = $(".mobile-verification-container");

            var detailsSubmitBtn = $("#user-details-submit-btn");
            var otpText = $("#getOTP");
            var otpBtn = $("#otp-submit-btn");

            var prevEmail = "";
            var prevMobile = "";

            var getCityArea = GetGlobalCityArea();
            var customerViewModel = new CustomerModel();

            $(function () {

                leadBtnBookNow.on('click', function () {
                    leadCapturePopup.show();
                    appendHash("dpqPopup");
                    $("div#contactDetailsPopup").show();
                    $("#otpPopup").hide(); 

                });

                $(".leadCapture-close-btn").on("click", function () {
                    leadCapturePopup.hide();
                    $("#dealer-assist-msg").hide();
                    window.history.back();
                });

                $(document).on('keydown', function (e) {
                    if (e.keyCode === 27) {
                        $("#leadCapturePopup .leadCapture-close-btn").click();
                        $("div.termsPopUpCloseBtn").click();
                    }
                });

                $("#aOkayButton").click(function () {
                    $("#leadCapturePopup .leadCapture-close-btn").click();                    
                });

            });


            function CustomerModel() {
                var arr = setuserDetails();
                var self = this;
                if (arr != null && arr.length > 0) {
                    self.fullName = ko.observable(arr[0]);
                    self.emailId = ko.observable(arr[1]);
                    self.mobileNo = ko.observable(arr[2]);
                }
                else {
                    self.fullName = ko.observable();
                    self.emailId = ko.observable();
                    self.mobileNo = ko.observable();
                }
                self.IsVerified = ko.observable(false);
                self.NoOfAttempts = ko.observable(0);
                self.IsValid = ko.computed(function () { return self.IsVerified(); }, this);
                self.otpCode = ko.observable();
                self.verifyCustomer = function () {
                    if (!self.IsVerified()) {
                        var objCust = {
                            "dealerId": dealerId,
                            "pqId": pqId,
                            "customerName": self.fullName(),
                            "customerMobile": self.mobileNo(),
                            "customerEmail": self.emailId(),
                            "clientIP": clientIP,
                            "pageUrl": pageUrl,
                            "versionId": versionId,
                            "cityId": cityId,
                            "leadSourceId": eval("<%= Convert.ToInt16(Bikewale.Entities.BikeBooking.LeadSourceEnum.DealerPQ_Mobile) %>"),
                            "deviceId": getCookie('BWC')
                        }
                        $.ajax({
                            type: "POST",
                            url: "/api/PQCustomerDetail/",
                            data: ko.toJSON(objCust),
                            beforeSend: function (xhr) {
                                xhr.setRequestHeader('utma', getCookie('__utma'));
                                xhr.setRequestHeader('utmz', getCookie('__utmz'));
                            },
                            async: false,
                            contentType: "application/json",
                            success: function (response) {
                                var obj = ko.toJS(response);
                                self.IsVerified(obj.isSuccess);
                                if (!self.IsVerified()) {
                                    self.NoOfAttempts(obj.noOfAttempts);
                                }
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                self.IsVerified(false);
                            }
                        });
                    }
                };
                self.generateOTP = function () {
                    if (!self.IsVerified()) {
                        var objCust = {
                            "pqId": pqId,
                            "customerMobile": self.mobileNo(),
                            "customerEmail": self.emailId(),
                            "cwiCode": self.otpCode(),
                            "branchId": dealerId,
                            "customerName": self.fullName(),
                            "versionId": versionId,
                            "cityId": cityId
                        }
                        $.ajax({
                            type: "POST",
                            url: "/api/PQMobileVerification/",
                            data: ko.toJSON(objCust),
                            async: false,
                            contentType: "application/json",
                            success: function (response) {
                                var obj = ko.toJS(response);
                                self.IsVerified(obj.isSuccess);

                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                self.IsVerified(false);
                            }
                        });
                    }
                };

                self.regenerateOTP = function () {
                    if (self.NoOfAttempts() <= 2 && !self.IsVerified()) {
                        var url = '/api/ResendVerificationCode/';
                        var objCustomer = {
                            "customerName": self.fullName(),
                            "customerMobile": self.mobileNo(),
                            "customerEmail": self.emailId(),
                            "source": 2
                        }
                        $.ajax({
                            type: "POST",
                            url: url,
                            async: false,
                            data: ko.toJSON(objCustomer),
                            contentType: "application/json",
                            success: function (response) {
                                self.IsVerified(false);
                                self.NoOfAttempts(response.noOfAttempts);
                                alert("You will receive the new OTP via SMS shortly.");
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                self.IsVerified(false);
                            }
                        });
                    }
                };

                self.submitLead = function () {
                    if (ValidateUserDetail()) {
                        self.verifyCustomer();
                        if (self.IsValid()) {
                            $("#contactDetailsPopup").hide();
                            $("#otpPopup").hide();
                            $("#dealer-assist-msg").show();                          
                        }
                        else {
                            $("#contactDetailsPopup").hide();
                            $("#otpPopup").show();
                            var leadMobileVal = mobile.val();
                            $("#otpPopup .lead-mobile-box").find("span.lead-mobile").text(leadMobileVal);
                            otpContainer.removeClass("hide").addClass("show");
                            nameValTrue();
                            hideError(mobile);
                            otpText.val('').removeClass("border-red").siblings("span, div").hide();
                        }
                        setPQUserCookie();
                        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Dealer_PQ', 'act': 'Lead_Submitted', 'lab': bikeName + '_' + getCityArea });
                    }

                };

                otpBtn.click(function () {
                    $('#processing').show();
                    if (!validateOTP())
                        $('#processing').hide();

                    if (validateOTP() && ValidateUserDetail()) {
                        customerViewModel.generateOTP();
                        if (customerViewModel.IsVerified()) {
                            $(".booking-dealer-details").removeClass("hide").addClass("show");
                            $('#processing').hide();

                            detailsSubmitBtn.show();
                            otpText.val('');
                            otpContainer.removeClass("show").addClass("hide");
                            dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation_Page', 'act': 'Step_1_OTP_Successful_Submit', 'lab': getCityArea });
                            $("#contactDetailsPopup").hide();
                            $("#otpPopup").hide();
                            $("#dealer-assist-msg").show();
                        }
                        else {
                            $('#processing').hide();
                            otpVal("Please enter a valid OTP.");
                            dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation Page', 'act': 'Step_1_OTP_Submit_Error', 'lab': getCityArea });
                        }
                    }
                });

            }

            function ValidateUserDetail() {
                var isValid = true;
                isValid = validateEmail();
                isValid &= validateMobile();
                isValid &= validateName();
                return isValid;
            };


            function validateName() {
                var isValid = true;
                var a = fullname.val().length;
                if ((/&/).test(fullname.val())) {
                    isValid = false;
                    setError(fullname, 'Invalid name');
                }
                else if (a == 0) {
                    isValid = false;
                    setError(fullname, 'Please enter your name');
                }
                else if (a >= 1) {
                    isValid = true;
                    nameValTrue()
                }
                return isValid;
            }

            function nameValTrue() {
                hideError(fullname)
                fullname.siblings("div").text('');
            };

            fullname.on("focus", function () {
                hideError(fullname);
            });

            emailid.on("focus", function () {
                hideError(emailid);
                prevEmail = emailid.val().trim();
            });

            mobile.on("focus", function () {
                hideError(mobile)
                prevMobile = mobile.val().trim();

            });

            emailid.on("blur", function () {
                if (prevEmail != emailid.val().trim()) {
                    if (validateEmail()) {
                        customerViewModel.IsVerified(false);
                        detailsSubmitBtn.show();
                        otpText.val('');
                        otpContainer.removeClass("show").addClass("hide");
                        hideError(emailid);
                    }
                    $('#confirmation-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
                    $('#customize-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
                }
            });

            mobile.on("blur", function () {
                if (mobile.val().length < 10) {
                    $("#user-details-submit-btn").show();
                    $(".mobile-verification-container").removeClass("show").addClass("hide");
                }
                if (prevMobile != mobile.val().trim()) {
                    if (validateMobile(getCityArea)) {
                        customerViewModel.IsVerified(false);
                        detailsSubmitBtn.show();
                        otpText.val('');
                        otpContainer.removeClass("show").addClass("hide");
                        hideError(mobile);
                    }
                    $('#confirmation-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
                    $('#customize-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
                }

            });

            function mobileValTrue() {
                mobile.removeClass("border-red");
                mobile.siblings("span, div").hide();
            };


            otpText.on("focus", function () {
                otpText.val('');
                otpText.siblings("span, div").hide();
            });

            function setError(ele, msg) {
                ele.addClass("border-red");
                ele.siblings("span, div").show();
                ele.siblings("div").text(msg);
            }

            function hideError(ele) {
                ele.removeClass("border-red");
                ele.siblings("span, div").hide();
            }
            /* Email validation */
            function validateEmail() {
                var isValid = true;
                var emailID = emailid.val();
                var reEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;

                if (emailID == "") {
                    setError(emailid, 'Please enter email address');
                    isValid = false;
                }
                else if (!reEmail.test(emailID)) {
                    setError(emailid, 'Invalid Email');
                    isValid = false;
                }
                return isValid;
            }

            function validateMobile() {
                var isValid = true;
                var reMobile = /^[0-9]{10}$/;
                var mobileNo = mobile.val();
                if (mobileNo == "") {
                    isValid = false;
                    setError(mobile, "Please enter your Mobile Number");
                }
                else if (!reMobile.test(mobileNo) && isValid) {
                    isValid = false;
                    setError(mobile, "Mobile Number should be 10 digits");
                }
                else {
                    hideError(mobile)
                }
                return isValid;
            }

            var otpVal = function (msg) {
                otpText.addClass("border-red");
                otpText.siblings("span, div").show();
                otpText.siblings("div").text(msg);
            };


            function validateOTP() {
                var retVal = true;
                var isNumber = /^[0-9]{5}$/;
                var cwiCode = otpText.val();
                customerViewModel.IsVerified(false);
                if (cwiCode == "") {
                    retVal = false;
                    otpVal("Please enter your Verification Code");
                }
                else {
                    if (isNaN(cwiCode)) {
                        retVal = false;
                        otpVal("Verification Code should be numeric");
                    }
                    else if (cwiCode.length != 5) {
                        retVal = false;
                        otpVal("Verification Code should be of 5 digits");
                    }
                }
                return retVal;
            }

            function setuserDetails() {
                var cookieName = "_PQUser";
                if (isCookieExists(cookieName)) {
                    var arr = getCookie(cookieName).split("&");
                    return arr;
                }
            }

            function setPQUserCookie() {
                var val = fullname.val() + '&' + emailid.val() + '&' + mobile.val();
                SetCookie("_PQUser", val);
            }

            $("#otpPopup .edit-mobile-btn").on("click", function () {
                var prevMobile = $(this).prev("span.lead-mobile").text();
                $(".lead-otp-box-container").hide();
                $(".update-mobile-box").show();
                $("#getUpdatedMobile").val(prevMobile).focus();
            });

            $("#generateNewOTP").on("click", function () {
                if (validateUpdatedMobile()) {
                    var updatedNumber = $(".update-mobile-box").find("#getUpdatedMobile").val();
                    $(".update-mobile-box").hide();
                    $(".lead-otp-box-container").show();
                    $(".lead-mobile-box").find(".lead-mobile").text(updatedNumber);
                }
            });

            var validateUpdatedMobile = function () {
                var isValid = true,
                    mobileNo = $("#getUpdatedMobile"),
                    mobileVal = mobileNo.val(),
                    reMobile = /^[0-9]{10}$/;
                if (mobileVal == "") {
                    setError(mobileNo, "Please enter your Mobile Number");
                    isValid = false;
                }
                else if (!reMobile.test(mobileVal) && isValid) {
                    setError(mobileNo, "Mobile Number should be 10 digits");
                    isValid = false;
                }
                else
                    hideError(mobileNo)
                return isValid;
            };
            $('#bookNowBtn').on('click', function (e) {
                window.location.href = "/m/pricequote/bookingSummary_new.aspx";
            });
            ko.applyBindings(customerViewModel, $('#leadCapturePopup')[0]);
            // GA Tags
            $("#leadBtnBookNow").on("click", function () {
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Dealer_PQ', 'act': 'Get_More_Details_Clicked_Button', 'lab': bikeName + '_' + getCityArea });
            });
            $("#leadLink").on("click", function () {
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Dealer_PQ', 'act': 'Get_More_Details_Clicked_Link', 'lab': bikeName + '_' + getCityArea });
            });
            ga_pg_id = "7";

            $('.tnc').on('click', function (e) {
                LoadTerms($(this).attr("id"));
            });

            function LoadTerms(offerId) {
                $("div#termsPopUpContainer").show();
                $(".blackOut-window").show();
                $('#terms').empty();
                if (offerId != 0 && offerId != null) {
                    $('#termspinner').show();
                    $.ajax({
                        type: "GET",
                        url: "/api/Terms/?offerMaskingName=&offerId=" + offerId,
                        dataType: 'json',
                        success: function (response) {
                            if (response != null)
                                $('#terms').html(response);
                        },
                        error: function (request, status, error) {
                            $("div#termsPopUpContainer").hide();
                            $(".blackOut-window").hide();
                        }
                    });
                }
                else {
                    $('#terms').html($("#orig-terms").html());
                }
                $('#termspinner').hide();
            }

            $(".termsPopUpCloseBtn").on('mouseup click', function (e) {
                $("div#termsPopUpContainer").hide();
                $(".blackOut-window").hide();
            });                      

            $(".secondary-dealer").click(function () {
                registerPQ($(this).attr("dealerId"));
            });

            function registerPQ(secondaryDealerId) {
                var obj = {
                    'CityId': cityId,
                    'AreaId': areaId,                    
                    'ClientIP': clientIP,
                    'SourceType': '<%=Bikewale.Utility.BWConfiguration.Instance.SourceId %>',
                    'VersionId': versionId,
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
                            window.location.href = "/m/pricequote/dealerpricequote.aspx?MPQ=" + Base64.encode(cookieValue);
                        }
                    }
                });
            }


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
                self.minDnPay = ko.observable('<%= primarydealer.EMIDetails.MinDownPayment %>' * bikeVersionPrice/100);
                self.maxDnPay = ko.observable('<%= primarydealer.EMIDetails.MaxDownPayment %>' * bikeVersionPrice/100);
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

            $.createSliderPoints = function (index, min, max, breaks, sliderType) {
                var svar = "";
                try {
                    switch (sliderType) {
                        case 1:
                            svar = $.valueFormatter(Math.round(min + (index * (max - min) / breaks)));
                            break;
                        case 2:
                            svar = Math.round(min + (index * (max - min) / breaks));
                            break;
                        default:
                            svar = (min + (index * (max - min) / breaks)).toFixed(2);
                            break;
                    }
                } catch (e) {

                }
                return svar;
            }

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
            ko.applyBindings(EMIviewModel, $("#emiPopup")[0]);  

            <% } %>

        </script>

    </form>
</body>
</html>
