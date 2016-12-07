<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.BikeBooking.DealerPriceQuote" Trace="false" EnableEventValidation="false" %>
<%@ Register Src="~/controls/NewAlternativeBikes.ascx" TagName="AlternativeBikes" TagPrefix="BW" %>
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
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
        
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/css/dealerpricequote.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->

        var dealerId = '<%= dealerId%>';
        var pqId = '<%= pqId%>';
        var ABHostUrl = '<%= System.Configuration.ConfigurationManager.AppSettings["ApiHostUrl"]%>';
        var versionId = '<%= versionId%>';
        var cityId = '<%= cityId%>';
        var areaId = '<%= areaId%>';   
        var clientIP = "<%= clientIP%>";
        var versionName = "<%= versionName%>";
        var pageUrl = "www.bikewale.com/pricequote/dealerpricequote.aspx?versionId=" + versionId + "&cityId=" + cityId;       
    </script>
</head>
<body class="bg-light-grey header-fixed-inner">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section>
            <div class="container padding-top10">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <a href="/" itemprop="url">
                                    <span itemprop="title">Home</span>
                                </a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                <a href="/new/" itemprop="url">
                                    <span itemprop="title">New</span>
                                </a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                <a href="/pricequote/" itemprop="url">
                                    <span itemprop="title">On-Road Price Quote</span>
                                </a>
                            </li>
                            <li><span class="bwsprite fa-angle-right margin-right10"></span>Dealer Price Quote</li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container" id="modelDetailsContainer">
                <div class="grid-12 margin-bottom20">
                    <div class="content-box-shadow">
                        <div class="content-box-shadow padding-14-20">
                            <h1 class="inline-block margin-right15">Detailed price quote for <%= BikeName %> in <%= GetLocationCookie() %></h1>
                        </div>
                        <div class="padding-top15 padding-right20 padding-bottom20 padding-left20">
                            <h2 class="font18 margin-bottom15"><%= BikeName %></h2>
                            <div id="pq-image-column" class="grid-5 alpha">
                                <% if (detailedDealer != null)
                                { %>
                                <div id="model-image">
                                    <img alt="<%= String.Format("{0} {1}",BikeName,versionName) %> Photos" src="<%= Bikewale.Utility.Image.GetPathToShowImages(detailedDealer.OriginalImagePath,detailedDealer.HostUrl,Bikewale.Utility.ImageSize._360x202) %>" title="<%= String.Format("{0} {1}",BikeName,versionName) %> Photos" width="100%" />
                                </div>
                                <% } %>
                                <ul id="version-specs-list">
                                    <li>Alloy Wheel</li>
                                    <li>Electric Start</li>
                                    <li>Disc Brake</li>
                                    <li>ABS</li>
                                </ul>
                            </div>
                            <div id="pq-table-column" class="grid-7 alpha omega">
                                <% if (versionList.Count > 1)
                                    { %>
                                <div id="model-version-dropdown" class="margin-right40 vertical-top">
                                    <div class="select-box select-box-no-input done size-small">
                                        <p class="select-label">Version</p>
                                        <select class="chosen-select" data-title="Version">
                                            <option value></option>
                                            <option value="1" selected>Electric Start/Drum/Alloy</option>
                                            <option value="2">Electric Start/Disc/Alloy</option>
                                            <option value="3">CBS</option>
                                        </select>
                                    </div>
                                </div>
                                <% }
                                   else { %>
                                <div class="single-version-content position-rel top-minus5 vertical-top margin-right40">
                                    <p class="font12 text-light-grey">Version</p>
                                    <p class="font14 text-bold single-version-value"><%= versionName %></p>
                                </div>
                                <% } %>

                                <div class="position-rel top-minus5 vertical-top">
                                    <p class="font12 text-light-grey">Location</p>
                                    <p class="font14 text-bold block position-rel pos-top2"><%= GetLocationCookie() %> <a class="margin-left5"><span class="bwsprite loc-change-blue-icon"></span></a></p>
                                </div>

                                <div runat="server">
                                    <div id="pq-table" class="margin-top15">
                                    <% if (primaryPriceList != null && primaryPriceList.Count() > 0)
                                       { %>
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <asp:Repeater ID="rptPriceList" runat="server">
                                            <ItemTemplate>
                                                <tr class="row-with-padding">
                                                    <td width="300" class="font14">
                                                        <%# DataBinder.Eval(Container.DataItem,"CategoryName") %> 
                                                    </td>
                                                    <td width="160" align="right">
                                                        <span class="bwsprite inr-md"></span>&nbsp;<span id="exShowroomPrice" class="font16 text-bold"><%#CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"Price").ToString()) %></span>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <tr>
                                            <td colspan="2" class="padding-bottom5">
                                                <div class="border-light-bottom"></div>
                                            <td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <p class="font14 text-bold">On-road price in <%= GetLocationCookie() %></p>
                                            </td>
                                            <td align="right">
                                                <span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font22 text-bold"><%= CommonOpn.FormatPrice(totalPrice.ToString()) %></span>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <p class="font12 text-light-grey text-truncate position-rel top-minus5">powered by Dealer</p>
                                            </td>
                                            <td align="right">
                                                <a href="javascript:void(0)" class="font14 bw-ga" leadSourceId="8" id="leadLink" name="leadLink" c="Dealer_PQ" a="Get_more_details_below_price_clicked" f="GetBikeVerLoc" rel="nofollow">Get more details</a>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td colspan="2" class="text-right padding-top5">
                                                </td>
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

                                       else if (objQuotation != null && objQuotation.ExShowroomPrice > 0)
                                       {%>                                           
                                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                <tr class="row-with-padding">
                                                    <td width="300" class="font14">
                                                        Ex-Showroom (<%= objQuotation.City %>)
                                                    </td>
                                                    <td width="160" align="right">
                                                        <span class="bwsprite inr-md"></span>&nbsp;<span id="exShowroomPrice" class="font16 text-bold"><%= CommonOpn.FormatNumeric( objQuotation.ExShowroomPrice.ToString() ) %></span>
                                                    </td>
                                                </tr>
                                                <tr class="row-with-padding">
                                                    <td class="font14">RTO</td>
                                                    <td align="right">
                                                        <span class="bwsprite inr-md"></span>&nbsp;<span class="font16 text-bold"><%= CommonOpn.FormatNumeric( objQuotation.RTO.ToString() ) %></span>
                                                    </td>
                                                </tr>
                                                <tr class="row-with-padding">
                                                    <td class="font14">Insurance (Comprehensive)<%--<br />
                                                        <div style="position: relative; color: #999; font-size: 11px; margin-top: 1px;">Save up to 60% on insurance - <a onclick="dataLayer.push({ event: 'Bikewale_all', cat: 'BW_PQ', act: 'Insurance_Clicked',lab: '<%= (objQuotation!=null)?(objQuotation.MakeName + "_" + objQuotation.ModelName + "_" + objQuotation.VersionName + "_" + objQuotation.City):string.Empty %>' });" target="_blank" href="/insurance/">PolicyBoss</a>
                                                            <span style="margin-left: 8px; vertical-align: super; font-size: 9px;">Ad</span>  
                                                        </div>--%>
                                                    </td>
                                                    <td align="right">
                                                        <span class="bwsprite inr-md"></span>&nbsp;<span class="font16 text-bold"><%= CommonOpn.FormatNumeric(  objQuotation.Insurance.ToString()  ) %></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" class="padding-bottom5">
                                                        <div class="border-light-bottom"></div>
                                                    <td>
                                                </tr>
                                                <tr>
                                                    <td><p class="font14 text-bold">On-road price in <%= GetLocationCookie() %></p></td>
                                                    <td align="right">
                                                        <span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font22 text-bold"><%= CommonOpn.FormatNumeric( objQuotation.OnRoadPrice.ToString()  ) %></span>
                                                    </td>
                                                </tr>	
                                            </table>
                      
                                       <%}
                                       else
                                       { %>
                                    <div>
                                        <p class="font16 text-bold">Price for this bike is not available in this city.</p>
                                    </div>
                                    <% } %>
                                </div>
                                </div>

                                <div id="div_ShowErrorMsg" runat="server" class="grey-bg border-light content-block text-highlight"></div>

                                
                                <% if (versionList.Count > 1)
                                   { %>
                                <div class="position-rel hide">
                                    <div class="variants-dropdown rounded-corner2 leftfloat">
                                        <div class="variant-selection-tab">
                                            <asp:Label runat="server" ID="defaultVariant"></asp:Label>
                                        </div>
                                        <span id="upDownArrow" class="rightfloat fa-angle-down position-abt pos-top15 pos-right10"></span>
                                    </div>
                                    <ul class="variants-dropdown-list" id="ulVersions">
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
                                <% } %>
                                
                            </div>
                            <div class="clear"></div>

                            <%if (primarydealer.DealerDetails != null)
                              { %>
                            <div class="inner-card-shadow margin-top20">
                                <div class="content-inner-block-20">
                                    <div id="pq-dealer-name" class="inline-block">
                                        <div class="inline-block margin-right10">
                                            <span class="pq-sprite partner-dealer"></span>
                                        </div>
                                        <div class="inline-block dealer-name-content">
                                            <h3 class="font18 text-black margin-bottom5"><%= dealerName %></h3>
                                            <p class="font12 text-light-grey">BikeWale partner dealer</p>
                                        </div>
                                    </div>
                                    <div id="dealer-offers-label" class="inline-block">
                                        <p class="font14">Get in touch with this dealer for:</p>
                                        <ul id="offers-label-list">
                                            <li>Best offers</li>
                                            <li>Test rides</li>
                                            <li>Exchange benefits</li>
                                        </ul>
                                    </div>
                                    <%if (dealerType != Bikewale.Entities.PriceQuote.DealerPackageTypes.Standard)
                                      { %>
                                    <div id="get-offers-btn-content" class="inline-block">
                                        <a href="javascript:void(0)" id="leadBtn" leadSourceId="9" class="btn btn-orange pq-get-dealer-offers" rel="nofollow">Get offers</a>
                                    </div>
                                    <% } %>
                                    <div class="clear"></div>                                    
                                </div>

                                <div class="margin-right20 margin-left20 border-solid-bottom"></div>
                                <div id="dealer-contact-details" class="content-inner-block-20 <%= (dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium) ? "" : "map-absent" %>"><!-- if no map, add 'map-absent' class -->
                                    <div class="alpha font14 <%= (dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium) ? "grid-6" : "grid-12 omega" %>"><!-- if no map, replace grid-6 with grid-12 -->
                                        <p class="text-bold margin-bottom15">Dealer contact details</p>

                                        <%if (dealerType != Bikewale.Entities.PriceQuote.DealerPackageTypes.Standard)
                                          { %>
                                        <div class="margin-bottom10">
                                            <span class="bwsprite dealership-loc-icon vertical-top margin-right5"></span>
                                            <span class="vertical-top text-light-grey details-column"><%= dealerAddress %></span>
                                        </div>
                                        <%} %>

                                        <% if (!string.IsNullOrEmpty(maskingNum))
                                           { %>
                                        <div class="dealer-details-item">
                                            <span class="bwsprite phone-black-icon vertical-top"></span>
                                            <span class="font15 vertical-top text-bold details-column"><%= maskingNum %></span>
                                        </div>
                                        <%} %>

                                        <div class="dealer-details-item">
                                            <span class="bwsprite mail-grey-icon vertical-top"></span>
                                            <a href="mailto:dealer@gmail.com" target="_blank" class="vertical-top text-light-grey" rel="nofollow">
                                                <span class="dealership-card-details">dealer@gmail.com</span>
                                            </a>
                                        </div>

                                        <div class="dealer-details-item">
                                            <span class="bwsprite clock-icon vertical-top"></span>
                                            <span class="vertical-top text-light-grey details-column">Working hours: 9.00 am - 6.00 pm</span>
                                        </div>

                                    </div>
                                    <%if (dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium)
                                      { %>
                                    <div class="grid-6 omega">
                                        <script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDY0kkJiTPVd2U7aTOAwhc9ySH6oHxOIYM&callback=initializeDealerMap" async defer></script>
                                        
                                        <div id="dealerMap" style="width:438px;height:202px;border:1px solid #f5f5f5;"></div>
                                        <div id="get-direction-button" title="get directions">
                                            <a href="" target="_blank">
                                                <span class="bwsprite get-direction-icon"></span>
                                            </a>
                                        </div>
                                        <script type="text/javascript">
                                            function initializeDealerMap() {
                                                var element = document.getElementById('dealerMap');
                                                var latitude = '<%= latitude %>';
                                                var longitude = '<%= longitude %>';

                                                try {
                                                    mapUrl = "http://maps.google.com/?q=" + latitude + "," + longitude;
                                                    latLng = new google.maps.LatLng(latitude, longitude),
                                                    mapOptions = {
                                                        zoom: 13,
                                                        center: latLng,
                                                        scrollwheel: false,
                                                        navigationControl: false,
                                                        draggable: false,
                                                        mapTypeId: google.maps.MapTypeId.ROADMAP,
                                                        disableDefaultUI: true
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
                                        </script>
                                    </div>
                                    <% } %>
                                    <div class="clear"></div>
                                </div>
                                
                                <% if (dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium &&  primarydealer.EMIDetails != null)
                                   { %>
                                <div class="margin-right20 margin-left20 border-solid-bottom"></div>
                                <div class="content-inner-block-20">
                                    <p class="font14 text-bold margin-bottom15">EMI calculator</p>
                                    <div id="EMISection" data-bind="visible: true" style="display: none" class="font14">
                                        <div class="grid-8 alpha border-light-right padding-bottom5 margin-bottom10">
                                            <div class="emi-slider-box">
                                                <p class="text-light-grey leftfloat">Down payment</p>
                                                <p class="rightfloat margin-right15"><span class="bwsprite inr-sm"></span>&nbsp;<span id="downPaymentAmount" class="text-bold" data-bind="text: formatPrice(Math.round(downPayment()))"></span></p>
                                                <div class="clear"></div>

                                                <div id="downPaymentSlider"
                                                    data-bind="slider: downPayment, sliderOptions: { min: minDnPay(), max: maxDnPay(), range: 'min', step: 1, value: Math.round(((maxDnPay() - minDnPay()) / 2 ) + minDnPay()) }"
                                                    class="ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all">
                                                    <div class="ui-slider-range ui-widget-header ui-corner-all ui-slider-range-min"></div>
                                                    <span class="ui-slider-handle ui-state-default ui-corner-all" tabindex="0"></span>
                                                </div>
                                            </div>

                                            <div class="emi-slider-box">
                                                <p class="text-light-grey leftfloat">Loan Amount</p>
                                                <p class="rightfloat margin-right15"><span class="bwsprite inr-sm"></span>&nbsp;<span id="loanAmount" class="text-bold" data-bind="text: formatPrice(Math.round(loan()))"></span></p>
                                                <div class="clear"></div>
                                                    
                                                <div id="loanAmountSlider"
                                                    data-bind="slider: loan, sliderOptions: { min: bikePrice() - maxDnPay(), max: bikePrice() - minDnPay(), range: 'min', step: 1 }"
                                                    class="ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all">
                                                    <div class="ui-slider-range ui-widget-header ui-corner-all ui-slider-range-min"></div>
                                                    <span class="ui-slider-handle ui-state-default ui-corner-all" tabindex="0"></span>
                                                </div>                                                
                                            </div>

                                            <div class="emi-slider-box">
                                                <p class="text-light-grey leftfloat">Tenure (Months)</p>
                                                <p class="rightfloat text-bold margin-right15"><span id="tenurePeriod" data-bind="text: tenure"></span> Months</p>
                                                <div class="clear"></div>
                                                    
                                                <div id="tenureSlider"
                                                    data-bind="slider: tenure, sliderOptions: { min: minTenure(), max: maxTenure(), range: 'min', step: 1 }"
                                                    class="ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all">
                                                    <div class="ui-slider-range ui-widget-header ui-corner-all ui-slider-range-min"></div>
                                                    <span class="ui-slider-handle ui-state-default ui-corner-all" tabindex="0"></span>
                                                </div>
                                            </div>

                                            <div class="emi-slider-box">
                                                <p class="text-light-grey leftfloat">Rate of interest (Percentage)</p>
                                                <p class="rightfloat text-bold margin-right15"><span id="rateOfInterestPercentage" class="text-bold" data-bind="text: rateofinterest">5</span>%</p>
                                                <div class="clear"></div>

                                                <div id="rateOfInterestSlider"
                                                    data-bind="slider: rateofinterest, sliderOptions: { min: minROI(), max: maxROI(), range: 'min', step: 0.25 }"
                                                    class="ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all">
                                                    <div class="ui-slider-range ui-widget-header ui-corner-all ui-slider-range-min"></div>
                                                    <span class="ui-slider-handle ui-state-default ui-corner-all" tabindex="0"></span>
                                                </div>                                               
                                            </div>
                                        </div>
                                        <div class="grid-4 padding-left20 omega">
                                            <div class="margin-left10 margin-bottom15">
                                                <p class="font14 text-light-grey margin-bottom5">Total amount (Payable+Interest)</p>
                                                <div>
                                                    <span class="bwsprite inr-md"></span>
                                                    <span class="font16 text-bold">999</span>                                            
                                                </div>
                                            </div>
                                            <div class="border-light-bottom margin-bottom15"></div>
                                            <div class="margin-left10">
                                                <p class="font14 text-light-grey margin-bottom5">Indicative EMI</p>
                                                <div class="margin-bottom15">
                                                    <span class="bwsprite inr-lg"></span>
                                                    <span class="font18 text-bold">
                                                        <span id="emiAmount" data-bind="text: monthlyEMI"></span> per month
                                                    </span>                                            
                                                </div>
                                                <a id="btnEmiQuote" leadSourceId="11" class="btn btn-grey btn-md font14">Get EMI quote</a>
                                            </div>
                                            
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <% } %>

                                <%if ((dealerType != Bikewale.Entities.PriceQuote.DealerPackageTypes.Standard && isoffer) || (dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium && isUSPBenfits))
                                  { %>
                                <div class="margin-right20 margin-left20 border-solid-bottom"></div>
                                <div class="content-inner-block-20 font14">
                                    <% if (dealerType != Bikewale.Entities.PriceQuote.DealerPackageTypes.Standard && isoffer) { %>
                                    <!-- offers -->
                                    <div class="alpha <%= (dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium && isUSPBenfits) ? "grid-6 border-light-right" : "grid-12 omega offers-or-benefits" %>">
                                        <p class="text-bold margin-bottom5">Offers from this dealer:</p>
                                        <ul class="dealership-benefit-list">
                                            <asp:Repeater ID="rptOffers" runat="server">
                                                <ItemTemplate>
                                                    <li>
                                                        <span class="inline-block benefit-list-image pq-sprite <%#  "offerIcon_" + DataBinder.Eval(Container.DataItem,"OfferCategoryId") %>"></span>
                                                        <span class="inline-block benefit-list-title"><%#  DataBinder.Eval(Container.DataItem,"OfferText") %><span class="tnc font9 margin-left5 <%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsOfferTerms"))? string.Empty: "hide" %>" id="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "OfferId")) %>">View terms</span></span>
                                                        </span>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                        <div class="clear"></div>
                                    </div>
                                    <% } %>
                                    <% if (dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium && isUSPBenfits) { %>
                                    <!-- benefits -->
                                    <div class="omega <%= (dealerType != Bikewale.Entities.PriceQuote.DealerPackageTypes.Standard && isoffer) ? "grid-6 padding-left30" : "grid-12 alpha offers-or-benefits" %>">
                                        <p class="text-bold margin-bottom5">Benefits offered by this dealer</p>
                                        <ul class="dealership-benefit-list">
                                            <asp:Repeater ID="rptUSPBenefits" runat="server">
                                                <ItemTemplate>
                                                    <li>
                                                        <span class="inline-block benefit-list-image pq-sprite <%#  "benifitIcon_" + DataBinder.Eval(Container.DataItem,"CatId") %>"></span>
                                                        <span class="inline-block benefit-list-title"><%#  DataBinder.Eval(Container.DataItem,"BenefitText") %></span>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                        <div class="clear"></div>
                                    </div>
                                    <% } %>
                                    <div class="clear"></div>
                                </div>
                                <% } %>

                                <%if (primarydealer.IsBookingAvailable)
                                  { %>
                                <div class="margin-right20 margin-left20 border-solid-bottom"></div>
                                <div class="content-inner-block-20 font14">
                                    <p class="text-bold margin-bottom15">Book your bike from this dealer</p>
                                    <div class="grid-8 alpha">
                                        <p class="margin-bottom10">Pay <span class="bwsprite inr-sm-dark"></span><%= Bikewale.Utility.Format.FormatPrice(bookingAmount.ToString()) %> to book online and balance amount of <span class="bwsprite inr-sm-dark"></span><%= Bikewale.Utility.Format.FormatPrice((totalPrice - bookingAmount).ToString()) %> at the dealership</p>
                                        <ul id="booking-benefits-list">
                                            <li>Save on dealer visits</li>
                                            <li>Secure online payments</li>
                                            <li>Complete buyer protection</li>
                                        </ul>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="grid-4 padding-top5">
                                        <a href="/pricequote/bookingsummary_new.aspx?MPQ=<%= mpqQueryString %>" class="btn btn-teal book-now-btn">Book bike</a>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <%} %>
                                <p id="disclaimerText" class="<%= primarydealer.DealerDetails != null ? "" : "hide" %> padding-left20 font11 padding-right20 padding-bottom20 text-x-light">
                                    <span id="read-less">
                                        <%if (dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium)
                                            { %>
                                        The bike prices and EMI quote mentioned here are indicative and are provided by their authorized dealerships. BikeWale takes utmost care in gathering precise
                                        <% }
                                        else{ %>
                                        The bike prices mentioned here are indicative and are provided by their authorized dealerships. BikeWale takes utmost care in gathering precise and accurate
                                        <% } %>
                                          <a id="readmore" class="text-link">read more</a>
                                    </span>
                                    <span id="read-more"></span>
                                </p>
                            </div>
                            <% } %>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <%if (detailedDealer != null && detailedDealer.SecondaryDealers != null && detailedDealer.SecondaryDealers.Count() > 0 )
          { %>
        <section>
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div class="content-box-shadow padding-bottom20">
                        <h2 class="font18 padding-top20 padding-left20">Prices from <%= detailedDealer.SecondaryDealers.Count() %> <%= (detailedDealer.SecondaryDealers.Count() > 1)?"more partner dealers":"more partner dealer" %></h2>
                        <div class="jcarousel-wrapper inner-content-carousel small-card-carousel">
                            <div class="jcarousel margin-top20 margin-bottom20">
                                <ul class="more-dealers-list">
                                <asp:Repeater ID="rptDealers" runat="server">
                                    <ItemTemplate>
                                        <li dealerid="<%# DataBinder.Eval(Container.DataItem,"dealerId") %>">
                                            <a href="javascript:void(0)" title="<%# DataBinder.Eval(Container.DataItem,"Name") %>" class="top-block-target">
                                                <div class="grid-10 alpha margin-bottom5">
                                                    <h3 class="text-black text-truncate"><%# DataBinder.Eval(Container.DataItem,"Name") %></h3>
                                                </div>
                                                <div class="grid-2 dealer-distance alpha omega font12 text-light-grey text-right pos-top2">99 kms</div>
                                                <div class="clear"></div>

                                                <div class="margin-bottom5">
                                                    <span class="bwsprite dealership-loc-icon vertical-top margin-right5"></span>
                                                    <span class="vertical-top details-column font14 text-light-grey"><%# DataBinder.Eval(Container.DataItem,"Area") %></span>
                                                </div>
                                            
                                                <div>
                                                    <span class="bwsprite phone-black-icon vertical-top margin-right5"></span>
                                                    <span class="vertical-top details-column font14 text-default text-bold">9876543210</span>
                                                </div>

                                                <div class="margin-top10">
                                                    <div class="grid-5 alpha omega">
                                                        <p class="font12 text-light-grey margin-bottom2">On-road price</p>
                                                        <span class="bwsprite inr-md"></span>&nbsp;<span class="font16 text-default text-bold">99,999</span>
                                                    </div>
                                                    <div class="grid-7 border-solid-left padding-top10 padding-bottom10 padding-left20 omega ">
                                                        <span class="bwsprite offers-sm-box"></span>
                                                        <span class="font14 text-default text-bold">4</span>
                                                        <span class="font12 text-light-grey">Offers available</span>
                                                    </div>
								                    <div class="clear"></div>
							                    </div>
                                            </a>
                                            <div class="bottom-block-button margin-top15">
                                                <a href="" class="btn btn-white partner-dealer-offers-btn">Get offers from dealer</a>
                                            </div>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                                </ul>
                            </div>
                            <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev" rel="nofollow"></a></span>
                            <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <% } %>


        <section class="container">
            <div class="grid-12 margin-bottom20" id="dealerPriceQuoteContainer">
                <div class="content-box-shadow rounded-corner2">
                    <div id="pqBikeDetails" class="grid-8 alpha omega bg-white">
                        <div class="grid-6 padding-bottom20" id="PQImageVariantContainer">
                            <% if (detailedDealer != null)
                               { %>
                            <div class="pqBikeImage margin-bottom15">
                                <img alt="<%= String.Format("{0} {1}",BikeName,versionName) %> Photos" src="<%= Bikewale.Utility.Image.GetPathToShowImages(detailedDealer.OriginalImagePath,detailedDealer.HostUrl,Bikewale.Utility.ImageSize._310x174) %>" title="<%= String.Format("{0} {1}",BikeName,versionName) %> Photos" />
                            </div>
                            <% } %>                           

                        </div>
                        <!--Price List Section-->
                        <div class="grid-6 padding-top15 padding-bottom20 padding-right20" id="PQDetailsContainer">                         

                            
                        </div>
                        <div class="clear"></div>
                        

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
                                        <a class="btn btn-orange leftfloat" leadSourceId="10" id="buyingAssistBtn" data-bind="event: { click: submitLead } ">Submit</a>
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
                                    <p class="text-light-grey margin-bottom20">Dealership will get back to you with offers, EMI quotes, exchange benefits and much more!</p>
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
                                        <div class="lead-mobile-box lead-otp-box-container">
                                            <span class="bwsprite phone-grey-icon"></span>
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

                        <!--  Dealer Benefits starts-->
                        <!--  Dealer Benefits  ends -->

                        <!--  Booking availability starts-->
                        <!--  Booking availability ends-->

                        <!-- EMI section starts -->                        
                        <!-- EMI section ends  -->

                        <%} %>
                        <div class="clear"></div>

                        <!-- disclaimer text -->
                        
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
                                        <p class="font14 text-light-grey margin-bottom10">Dealership contact details:</p>
                                        <%if (dealerType != Bikewale.Entities.PriceQuote.DealerPackageTypes.Standard)
                                          { %>
                                        <p class="font14 margin-bottom10"><span class="bwsprite dealership-loc-icon vertical-top margin-right10"></span><span class="vertical-top dealership-address"><%= dealerAddress %></span></p>
                                        <%} %>
                                        <% if (!string.IsNullOrEmpty(maskingNum))
                                           { %>
                                        <p class="font16 text-bold"><span class="bwsprite phone-black-icon"></span>&nbsp;<%= maskingNum %></p>
                                        <%} %>
                                        <%if (dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium)
                                          { %>
                                        <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyDY0kkJiTPVd2U7aTOAwhc9ySH6oHxOIYM&callback=initializeDealerMap" async defer></script>
                                        <div id="dealerMap" class=" margin-top15 text-center position-rel" style="height: 100px">
                                            <img class="position-abs" src="https://imgd3.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif" />
                                        </div>
                                        <script type="text/javascript">
                                            function initializeDealerMap() {
                                                var element = document.getElementById('dealerMap');
                                                var latitude = '<%= latitude %>';
                                                var longitude = '<%= longitude %>';

                                                try {
                                                    mapUrl = "https://maps.google.com/?q=" + latitude + "," + longitude;
                                                    latLng = new google.maps.LatLng(latitude, longitude),
                                                    mapOptions = {
                                                        zoom: 13,
                                                        center: latLng,
                                                        scrollwheel: false,
                                                        navigationControl: false,
                                                        draggable: false,
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
                                        </script>--%>
                                        <% } %>
                                        <%if (dealerType != Bikewale.Entities.PriceQuote.DealerPackageTypes.Standard)
                                          { %>
                                        <%--<a id="leadBtn" leadSourceId="9" class="btn btn-teal btn-full-width font14 margin-top15">Get offers from this dealer</a>--%>
                                        <% } %>
                                    </div>
                                    <% } %>
                                </div>
                                <%} %>
                                <%else
                                  { %>
                                <div class="pq-no-premium-dealer font14 text-light-grey">Sorry, there are no partner dealers nearby</div>
                                <%} %>
                            </div>
                            
                        </div>
                    </div>
                    <div class="clear"></div>
                </div>
            </div>
            <div class="clear"></div>
        </section>
        <section>
            <div class="container margin-bottom30 <%= (ctrlAlternativeBikes.FetchedRecordsCount > 0) ? string.Empty : "hide" %>">
                <div class="grid-12">
                    <BW:AlternativeBikes ID="ctrlAlternativeBikes" runat="server" />
                </div>
                <div class="clear"></div>
            </div>
        </section>
         <!-- Terms and condition Popup start -->
        <div class="termsPopUpContainer content-inner-block-20 hide" id="termsPopUpContainer">
            <div class="fixed-close-btn-wrapper">
                <div id="termsPopUpCloseBtn" class="termsPopUpCloseBtn fixed-close-btn bwsprite cross-lg-lgt-grey cur-pointer"></div>
            </div>
            <h3>Terms and conditions</h3>
            <div class="hide" style="vertical-align: middle; text-align: center;" id="termspinner">
                    <img class="lazy" data-original="https://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif"  src="" />
            </div>
            <div id="terms" class="breakup-text-container padding-bottom10 font14">
            </div>
            <div id='orig-terms' class='hide'>
            </div>
        </div>
        <!-- Terms and condition Popup Ends -->

        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>

        <!-- #include file="/includes/footerBW.aspx" -->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/common.min.js?<%= staticFileVersion %>"></script>
        
        <script type="text/javascript">

            var bikeName = "<%= BikeName %>";
            var bikeVersionPrice = "<%= totalPrice %>";
            var leadSourceId;
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
                <%--self.processingFees = ko.observable(<%= primarydealer.EMIDetails.ProcessingFee %>);--%>
                self.processingFees = ko.observable(0);
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
                    totalRepay = loanAmount + interest + proFees;
                    finalEmi = Math.ceil((totalRepay / tenure));
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
                if(isNaN(num))
                {
                    if (num >= 100000) {
                        return (num / 100000).toFixed(1).replace(/\.0$/, '') + 'L';
                    }
                    if (num >= 1000) {
                        return (num / 1000).toFixed(1).replace(/\.0$/, '') + 'K';
                    }
                }
              
                return num;
            }

            var EMIviewModel = new BikeEMI;
            ko.applyBindings(EMIviewModel, $("#EMISection")[0]);

            <% } %>

            $( document ).ready(function() {
                <%if(detailedDealer != null && detailedDealer.SecondaryDealers != null && detailedDealer.SecondaryDealers.Count() > 0){%>
                triggerGA('Dealer_PQ', 'Secondary_Dealer_Card_Shown', '<%= string.Format("{0}_{1}_{2}_{3}", makeName,modelName,CityArea.City,CityArea.Area)%>');
                 <%}%>
            });

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
                dataLayer.push({ "event": "Bikewale_all", "cat": "Dealer_PQ", "act": "Get_More_Details_Clicked_Button", "lab": GetBikeVerLoc() });
            });

            $("#leadLink").on("click", function () {
                getMoreDetailsClick = true;
            });

            $("input[name*='btnVariant']").on("click", function () {
                if ($(this).attr('versionid') == $('#hdnVariant').val()) {
                    return false;
                }
                $('#hdnVariant').val($(this).attr('title'));
                dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Version_Change", "lab": GetBikeVerLoc() });
            });

            $("input[name*='switchDealer']").on("click", function () {
                if ($(this).attr('dealerId') == $('#hdnDealerId').val()) {
                    return false;
                }
                $('#hdnDealerId').val($(this).attr('title'));
                dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Version_Change", "lab": GetBikeVerLoc() });
            });
            $("#dealerList li").on("click", function(){
                registerPQ($(this).attr('dealerId'));
            });

            function registerPQ(secondaryDealerId) {
                var obj = {
                    'cityId': cityId,
                    'areaId': areaId,
                    'clientIP': clientIP,
                    'sourceType': <%=Bikewale.Utility.BWConfiguration.Instance.SourceId %>,
                    'versionId': versionId,
                    'pQLeadId': leadSourceId,
                    'deviceId': getCookie('BWC'),
                    'dealerId': secondaryDealerId,
                    'refPQId' : pqId
                };
                $.ajax({
                    type: 'POST',
                    url: "/api/RegisterPQ/",
                    data: obj,
                    dataType: 'json',
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader('utma', getCookie('__utma'));
                        xhr.setRequestHeader('utmz', getCookie('_bwutmz'));
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

            function GetBikeVerLoc() {
              
                return bikeName + "_" + versionName + "_" + getCityArea;
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
            $(function(){
                if ($('.pricequote-benefits-list li').length % 2 == 0) {
                    $('.pricequote-benefits-list').addClass("pricequote-two-benefits");
                }
            });
            $("#readmore").on("click", function () {
                var dealerType = '<%=dealerType %>';
                loadDisclaimer(dealerType);
            });
            $('.blackOut-window').on("click", function () {
                $("div#termsPopUpContainer").hide();
                $(".blackOut-window").hide();
            });
        </script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/src/dealerpricequote.js?<%= staticFileVersion %>"></script>
        <link href="https://fonts.googleapis.com/css?family=Open+Sans:400,600,700" rel="stylesheet" type="text/css" />
        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->
    </form>
</body>
</html>
