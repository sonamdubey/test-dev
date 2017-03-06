<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.BikeBooking.DealerPriceQuote" Trace="false" EnableEventValidation="false" %>

<%@ Register Src="~/controls/NewAlternativeBikes.ascx" TagName="AlternativeBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/DealerCard.ascx" TagName="Dealers" TagPrefix="BW" %>
<%@ Register Src="~/controls/LeadCaptureControl.ascx" TagName="LeadCapture" TagPrefix="BW" %>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Import Namespace="Bikewale.BikeBooking" %>

<!doctype html>
<html>
<head>
    <%
        title = String.Format("{0} {1} Price Quote", bikeName, versionName);
        description = String.Format("{0} {1} price quote", bikeName, versionName);
        AdId = "1395986297721";
        AdPath = "/1017752/Bikewale_PQ_";
        isAd970x90Shown = dealerId <= 0;
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
        var bikeVerLocation;
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
                                <a href="/new-bikes-in-india/" itemprop="url">
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
                            <h1 class="inline-block margin-right15">Detailed price quote for <%= bikeName %> in <%= location %></h1>
                        </div>
                        <div class="padding-top15 padding-right20 padding-bottom20 padding-left20">
                            <h2 class="font18 margin-bottom15"><%= bikeName %></h2>
                            <div id="pq-image-column" class="grid-5 alpha">
                                <% if (detailedDealer != null)
                                   { %>
                                <div id="model-image">
                                    <img alt="<%= bikeVersionName %> Images" src="<%= Bikewale.Utility.Image.GetPathToShowImages(detailedDealer.OriginalImagePath,detailedDealer.HostUrl,Bikewale.Utility.ImageSize._360x202) %>" title="<%= bikeVersionName %> Images" alt="<%= bikeVersionName %> Images" width="100%" />
                                </div>
                                <% } %>
                                <%= minspecs %>
                            </div>
                            <div id="pq-table-column" class="grid-7 alpha omega">
                                <% if (versionList.Count > 1)
                                   { %>
                                <div id="model-version-dropdown" class="margin-right40 vertical-top">
                                    <div class="select-box select-box-no-input done size-small">
                                        <p class="select-label">Version</p>
                                        <asp:DropDownList AutoPostBack="true" runat="server" ID="ddlVersion" CssClass="chosen-select" data-title="Version" />
                                    </div>
                                </div>
                                <% }
                                   else
                                   { %>
                                <div class="single-version-content position-rel top-minus5 vertical-top margin-right40">
                                    <p class="font12 text-light-grey">Version</p>
                                    <p class="font14 text-bold single-version-value"><%= versionName %></p>
                                </div>
                                <% } %>
                                <asp:HiddenField ID="hdnVariant" runat="server" />

                                <div class="position-rel top-minus5 vertical-top getquotation" data-persistent="true" data-modelid="<%= modelId %>">
                                    <p class="font12 text-light-grey">Location</p>
                                    <p class="font14 text-bold block position-rel pos-top2">
                                        <%= location %> <span class="margin-left5">
                                            <span class="bwsprite loc-change-blue-icon"></span></span>
                                    </p>
                                </div>
                                <div runat="server">
                                    <div id="pq-table" class="margin-top15">
                                        <% if (primaryPriceList != null && primaryPriceList.Count() > 0)
                                           { %>
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <% foreach (var item in primaryPriceList)
                                               { %>
                                            <tr class="row-with-padding">
                                                <td width="300" class="font14">
                                                    <%=item.CategoryName %> 
                                                </td>
                                                <td width="170" align="right">
                                                    <span class="bwsprite inr-md"></span>&nbsp;<span id="exShowroomPrice" class="font16 text-bold"><%= CommonOpn.FormatPrice(item.Price.ToString()) %></span>
                                                </td>
                                            </tr>
                                            <% } %>
                                            <tr>
                                                <td colspan="2" class="padding-bottom5">
                                                    <div class="border-light-bottom"></div>
                                                    <td>
                                            </tr>

                                            <tr>
                                                <td>
                                                    <p class="font14 text-bold">On-road price in <%= location %></p>
                                                </td>
                                                <td align="right">
                                                    <span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font22 text-bold"><%= CommonOpn.FormatPrice(totalPrice.ToString()) %></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <% if (isPrimaryDealer)
                                                       { %>
                                                    <% if(!primarydealer.DealerDetails.IsDSA){ %>
                                                    <p class="font12 text-light-grey text-truncate position-rel top-minus5"> powered by <%= primarydealer.DealerDetails.Organization %>, <%= primarydealer.DealerDetails.objArea.AreaName %></p>
                                                    <%} else { %>
                                                    <p class="font12 text-light-grey text-truncate position-rel top-minus5">*actual pricing may vary</p>
                                                    <%} %>
                                                    <% } %>
                                                </td>
                                                <td align="right">
                                                    <% if (isPrimaryDealer)
                                                       { %>
                                                    <a href="javascript:void(0)" class="font14 bw-ga" leadsourceid="8" data-dealerid="<%=dealerId %>" id="leadLink" name="leadLink" c="Dealer_PQ" a="Get_more_details_below_price_clicked" f="GetBikeVerLoc" rel="nofollow">Get more details</a>
                                                    <% } %>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" class="text-right padding-top5"></td>
                                            </tr>
                                            <tr class="hide">
                                                <td colspan="3"></td>
                                            </tr>
                                        </table>
                                        <% }

                                           else if (objQuotation != null && objQuotation.ExShowroomPrice > 0)
                                           {%>
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr class="row-with-padding">
                                                <td width="300" class="font14">Ex-Showroom (<%= objQuotation.City %>)
                                                </td>
                                                <td width="170" align="right">
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
                                                <td class="font14">Insurance (Comprehensive)
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
                                                <td>
                                                    <p class="font14 text-bold">On-road price in <%= location %></p>
                                                </td>
                                                <td align="right">
                                                    <span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font22 text-bold"><%= CommonOpn.FormatNumeric( objQuotation.OnRoadPrice.ToString()  ) %></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <% if(!isPrimaryDealer)
                                                       { %>
                                                <td align="right" colspan="2"><p class="position-rel"><span class="position-abt neg-right15 pos-top0 font8">Ad</span><a target="_blank" href="https://www.bankbazaar.com/personal-loan.html?variant=slide&headline=HEADLINE_PL_MelaSale&WT.mc_id=bb01|BW|PL|PriceQuote&utm_source=bb01&utm_medium=display&utm_campaign=bb01|BW|PL|PriceQuote&variantOptions=mobileRequired" class="font14 bw-ga text-bold" c="Dealer_PQ" a="Get_personal_loan_offers_clicked" f="GetBikeVerLoc" rel="nofollow">Get personal loan offers</a></p></td>
                                                <% } %>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <% if (isPrimaryDealer)
                                                       { %>
                                                    <p class="font12 text-light-grey text-truncate position-rel top-minus5">powered by <%= primarydealer.DealerDetails.Organization %>, <%= primarydealer.DealerDetails.objArea.AreaName %></p>
                                                    <% } %>
                                                </td>
                                                <td align="right">
                                                    <% if (isPrimaryDealer)
                                                       { %>
                                                    <a href="javascript:void(0)" class="font14 bw-ga" leadsourceid="8" data-dealerid="<%=dealerId %>" id="leadLink" name="leadLink" c="Dealer_PQ" a="Get_more_details_below_price_clicked" f="GetBikeVerLoc" rel="nofollow">Get more details</a>
                                                    <% } %>
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
                            </div>
                            <div class="clear"></div>

                            <%if (primarydealer.DealerDetails != null && primarydealer.DealerDetails.objArea != null)
                              { %>
                            <div class="inner-card-shadow margin-top20">
                                <div class="content-inner-block-20 position-rel">
                                    <div id="pq-dealer-name" class="inline-block">
                                        <div class="inline-block margin-right10">
                                            <span class="pq-sprite partner-dealer"></span>
                                        </div>
                                        <div class="inline-block dealer-name-content">
                                            <h3 class="font18 text-black margin-bottom5 inline-block"><%= primarydealer.DealerDetails.Organization %></h3>
                                        <% if (!string.IsNullOrEmpty(primarydealer.DealerDetails.MaskingNumber))
                                           { %>
                                        <div class="margin-left10 inline-block">
                                            <span class="bwsprite phone-black-icon vertical-top"></span>
                                            <span class="font15 vertical-top text-bold "><%= primarydealer.DealerDetails.MaskingNumber %></span>
                                        </div>
                                        <%} %>
                                            <p class="font12 text-light-grey"><%= (!primarydealer.DealerDetails.IsDSA ? "Authorized Dealer in " : "Multi-brand Dealer in ") %><%= primarydealer.DealerDetails.objArea.AreaName %></p>
                                        </div>
                                    </div>
                                    <div id="dealer-offers-label" class="inline-block">
                                        <p class="font14">Get in touch with this dealer for:</p>
                                        <ul id="offers-label-list">
                                            <li>Best offers</li>
                                            <li>Test rides</li>
                                            <li>EMI options</li>
                                        </ul>
                                    </div>
                                    <div id="get-offers-btn-content" class="inline-block <%= primarydealer.DealerDetails.IsDSA ? "rightfloat" :"" %>">
                                        <a href="javascript:void(0)" id="leadBtn" leadsourceid="9"  data-item-id="<%= dealerId %>" data-item-name="<%= primarydealer.DealerDetails.Organization %>" data-item-area="<%= primarydealer.DealerDetails.objArea.AreaName %>" class="btn btn-orange pq-get-dealer-offers leadcapturebtn" c="DealerPriceQoute_Page" a="Get_Offers_Clicked" v="bikeVerLocation" rel="nofollow"><%= leadBtnLargeText %></a>
                                    </div>
                                    <div class="clear"></div>
                                    <% if(primarydealer.DealerDetails.IsDSA){ %>
                                    <div class="bw-tooltip pq-multi-brand-tooltip tooltip-bottom slideUp-tooltip">
                                        <p class="bw-tooltip-text position-rel font14">This dealer sells bikes of multiple brands. Above price is not final and may vary at the dealership.</p>
                                        <span class="position-abt pos-top15 pos-right15 bwsprite cross-sm-dark-grey cur-pointer close-bw-tooltip"></span>
                                    </div>
                                    <%} %>
                                    <div class="margin-top20 border-solid-bottom"></div>
                                </div>
                                <div id="dealer-contact-details" class="padding-right20 padding-bottom20 padding-left20 <%= isPremium ? "" : "map-absent" %>">
                                    <!-- if no map, add 'map-absent' class -->
                                    <div class="alpha font14 <%= isPremium ? "grid-6" : "grid-12 omega" %>">
                                        <!-- if no map, replace grid-6 with grid-12 -->
                                        <% if (!string.IsNullOrEmpty(primarydealer.DealerDetails.Address))
                                           { %>
                                        <p class="text-bold margin-bottom15">Dealer contact details</p>
                                        <div class="margin-bottom10">
                                            <span class="bwsprite dealership-loc-icon vertical-top margin-right5"></span>
                                            <span class="vertical-top text-light-grey details-column"><%= primarydealer.DealerDetails.Address %></span>
                                        </div>
                                        <% } %>

                                        <% if (!string.IsNullOrEmpty(primarydealer.DealerDetails.MaskingNumber))
                                           { %>
                                        <div class="dealer-details-item">
                                            <span class="bwsprite phone-black-icon vertical-top"></span>
                                            <span class="font15 vertical-top text-bold details-column"><%= primarydealer.DealerDetails.MaskingNumber %></span>
                                        </div>
                                        <%} %>
                                        <%if (!string.IsNullOrEmpty(primarydealer.DealerDetails.EmailId))
                                          {%>
                                        <div class="dealer-details-item">
                                            <span class="bwsprite mail-grey-icon vertical-top"></span>
                                            <a href="mailto:<%= primarydealer.DealerDetails.EmailId %>" target="_blank" class="vertical-top text-light-grey" rel="nofollow">
                                                <span class="dealership-card-details"><%= primarydealer.DealerDetails.EmailId %></span>
                                            </a>
                                        </div>
                                        <% } %>
                                        <%if (!string.IsNullOrEmpty(primarydealer.DealerDetails.WorkingTime))
                                          {%>
                                        <div class="dealer-details-item">
                                            <span class="bwsprite clock-icon vertical-top"></span>
                                            <span class="vertical-top text-light-grey details-column">Working hours: <%= primarydealer.DealerDetails.WorkingTime %></span>
                                        </div>
                                        <% } %>
                                    </div>
                                    <%if (isPremium)
                                      { %>
                                    <div class="grid-6 omega">
                                        <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyDY0kkJiTPVd2U7aTOAwhc9ySH6oHxOIYM&callback=initializeDealerMap" async defer></script>

                                        <div id="dealerMap" style="width: 438px; height: 202px; border: 1px solid #f5f5f5;"></div>
                                        <div id="get-direction-button" title="get directions">
                                            <a href="https://maps.google.com/?saddr=&daddr=<%= primarydealer.DealerDetails.objArea.Latitude%>,<%= primarydealer.DealerDetails.objArea.Longitude%>" target="_blank">
                                                <span class="bwsprite get-direction-icon"></span>
                                            </a>
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

                                <!-- EMI calculator starts -->
                                <% if (_objEMI != null)
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
                                                <p class="rightfloat text-bold margin-right15"><span id="tenurePeriod" data-bind="text: tenure"></span>Months</p>
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
                                                <p class="font14 text-light-grey margin-bottom5">Total amount (Principal + Interest)</p>
                                                <div>
                                                    <span class="bwsprite inr-md"></span>
                                                    <span class="font16 text-bold" data-bind="text: formatPrice(Math.round(totalPayable()))"></span>
                                                </div>
                                            </div>
                                            <div class="border-light-bottom margin-bottom15"></div>
                                            <div class="margin-left10">
                                                <p class="font14 text-light-grey margin-bottom5">Indicative EMI</p>
                                                <div class="margin-bottom15">
                                                    <span class="bwsprite inr-lg"></span>
                                                    <span class="font18 text-bold">
                                                        <span id="emiAmount" data-bind="text: formatPrice(monthlyEMI())"></span> per month
                                                    </span>
                                                </div>
                                                <a id="btnEmiQuote" leadsourceid="11" data-item-id="<%=dealerId %>" class="btn btn-grey btn-md font14 leadcapturebtn">Get EMI quote</a>
                                            </div>

                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <% } %>
                                <!-- EMI calculator ends -->

                                <%if ((!isStandard && isoffer) || (isPremium && isUSPBenfits))
                                  { %>
                                <div class="margin-right20 margin-left20 border-solid-bottom"></div>
                                <div class="content-inner-block-20 font14">
                                    <% if (!isStandard && isoffer)
                                       { %>
                                    <!-- offers -->
                                    <div class="alpha <%= (isPremium && isUSPBenfits) ? "grid-6 border-light-right" : "grid-12 omega offers-or-benefits" %>">
                                        <p class="text-bold margin-bottom5">Offers from this dealer:</p>
                                        <ul class="dealership-benefit-list">
                                            <% foreach (var offer in primarydealer.OfferList)
                                               {
                                            %>
                                            <li class="<%= offerCount == 1 ? "single-offer" :"" %>">
                                                <span class="inline-block benefit-list-image pq-sprite <%=string.Format("offerIcon_{0}", offer.OfferCategoryId) %>"></span>
                                                <span class="inline-block benefit-list-title"><%= offer.OfferText %>
                                                    <% if (offer.IsOfferTerms)
                                                       { %>
                                                    <span class="tnc font9 margin-left5" id="<%= offer.OfferId %>">View terms
                                                    </span>
                                                    <% } %>
                                                </span>
                                            </li>
                                            <% } %>
                                        </ul>
                                        <div class="clear"></div>
                                    </div>
                                    <% } %>
                                    <% if (isPremium && isUSPBenfits)
                                       { %>
                                    <!-- benefits -->
                                    <div class="omega <%= (!isStandard && isoffer) ? "grid-6 padding-left30" : "grid-12 alpha offers-or-benefits" %>">
                                        <p class="text-bold margin-bottom5">Benefits offered by this dealer</p>
                                        <ul class="dealership-benefit-list">
                                            <% foreach (var benefit in primarydealer.Benefits)
                                               { %>
                                            <li>
                                                <span class="inline-block benefit-list-image pq-sprite <%= string.Format("benifitIcon_{0}", benefit.CatId) %>"></span>
                                                <span class="inline-block benefit-list-title"><%= benefit.BenefitText %></span>
                                            </li>
                                            <% } %>
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
                                <% if (primarydealer.DealerDetails != null)
                                   { %>
                                <p id="disclaimerText" class="padding-left20 font11 padding-right20 padding-bottom20 text-x-light">
                                    <span id="read-less">
                                        <%if (isPremium)
                                          { %>
                                        The bike prices and EMI quote mentioned here are indicative and are provided by their authorized dealerships. BikeWale takes utmost care in gathering precise
                                        <% }
                                          else
                                          { %>
                                        The bike prices mentioned here are indicative and are provided by their authorized dealerships. BikeWale takes utmost care in gathering precise and accurate
                                        <% } %>
                                        <a id="readmore" class="text-link">read more</a>
                                    </span>
                                    <span id="read-more"></span>
                                </p>
                                <% } %>
                            </div>
                            <% } %>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>


        <%  if (dealerId == 0 && detailedDealer != null && detailedDealer.SecondaryDealerCount == 0 && objQuotation != null && string.IsNullOrEmpty(objQuotation.ManufacturerAd))
            { %>
        <section>
            <div class="container">
                <div class="grid-12 margin-bottom20">
                    <div class="content-box-shadow">
                        <div class="margin-left10 margin-right10"></div>
                        <BW:Dealers ID="ctrlDealers" runat="server" />
                    </div>
                </div>
            </div>
        </section>
        <% } %>


        <% if (objQuotation != null && !string.IsNullOrEmpty(objQuotation.ManufacturerAd) && detailedDealer.PrimaryDealer.DealerDetails == null && detailedDealer.SecondaryDealerCount == 0)
           {
               dealerName = objQuotation.ManufacturerName; %>
        <section>
            <%= string.Format(objQuotation.ManufacturerAd) %>
        </section>
        <%} %>

        <%if (detailedDealer != null && detailedDealer.SecondaryDealers != null && detailedDealer.SecondaryDealerCount > 0)
          { %>
        <!-- Secondary dealer section -->
        <section>
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div class="content-box-shadow padding-bottom20">
                        <h2 class="font18 padding-top20 padding-left20">Prices from <%= detailedDealer.SecondaryDealerCount %> <%= (detailedDealer.SecondaryDealerCount > 1)?"more partner dealers":"more partner dealer" %></h2>
                        <div class="jcarousel-wrapper inner-content-carousel small-card-carousel">
                            <div class="jcarousel margin-top20 margin-bottom20">
                                <ul id="dealerList" class="more-dealers-list">
                                    <% foreach (var dealer in detailedDealer.SecondaryDealers)
                                       { %>
                                    <li dealerid="<%= dealer.DealerId %>" style="min-height:191px">
                                        <a href="javascript:void(0)" title="<%= dealer.Name%>" class="top-block-target" onclick="secondarydealer_Click(<%= dealer.DealerId %>)">
                                            <div class="grid-10 alpha margin-bottom5">
                                                <h3 class="text-black text-truncate"><%= dealer.Name%></h3>
                                            </div>
                                            <div class="grid-2 dealer-distance alpha omega font12 text-light-grey text-right pos-top2"><%= Math.Truncate(dealer.Distance) %> kms</div>
                                            <div class="clear"></div>

                                            <div class="margin-bottom5">
                                                <span class="bwsprite dealership-loc-icon vertical-top margin-right5"></span>
                                                <span class="vertical-top details-column font14 text-light-grey"><%= dealer.Area%></span>
                                            </div>
                                            <div class="margin-top10">
                                                <div class="grid-5 alpha omega">
                                                    <p class="font12 text-light-grey margin-bottom2">On-road price</p>
                                                    <span class="bwsprite inr-md"></span>&nbsp;<span class="font16 text-default text-bold"><%=Bikewale.Utility.Format.FormatPrice(dealer.SelectedVersionPrice.ToString())%></span>
                                                </div>
                                                <% if (dealer.DealerPackageType != Bikewale.Entities.PriceQuote.DealerPackageTypes.Standard && dealer.OfferCount > 0)
                                                   { %>
                                                <div class="grid-7 border-solid-left padding-top10 padding-bottom10 padding-left20 omega ">
                                                    <span class="bwsprite offers-sm-box"></span>
                                                    <span class="font14 text-default text-bold"><%=dealer.OfferCount %></span>
                                                    <span class="font12 text-light-grey"><%= dealer.OfferCount > 1 ? "Offers available" :"Offer available" %> </span>
                                                </div>
                                                <% } %>
                                                <div class="clear"></div>
                                            </div>
                                        </a>
                                        <div class="bottom-block-button margin-top15">
                                            <a href="javascript:void(0)" onclick="secondarydealer_Click(<%= dealer.DealerId %>)" class="btn btn-white partner-dealer-offers-btn" rel="nofollow">View detailed price</a>
                                        </div>
                                    </li>
                                    <% } %>
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
        <!-- Secondary dealer section ends -->
        <% } %>

        <% if (ctrlAlternativeBikes.FetchedRecordsCount > 0)
           { %>
        <!-- Alternate bikes section starts-->
        <section>
            <div class="container margin-bottom30">
                <div class="grid-12">
                    <BW:AlternativeBikes ID="ctrlAlternativeBikes" runat="server" />
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <!-- Alternate bikes section ends -->
        <% } %>

        <!-- Terms and condition Popup start -->
        <div class="termsPopUpContainer content-inner-block-20 hide" id="termsPopUpContainer">
            <div class="fixed-close-btn-wrapper">
                <div id="termsPopUpCloseBtn" class="termsPopUpCloseBtn fixed-close-btn bwsprite cross-lg-lgt-grey cur-pointer"></div>
            </div>
            <h3>Terms and conditions</h3>
            <div class="hide" style="vertical-align: middle; text-align: center;" id="termspinner">
                <img class="lazy" data-original="https://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif" src="" />
            </div>
            <div id="terms" class="breakup-text-container padding-bottom10 font14">
            </div>
            <div id='orig-terms' class='hide'>
            </div>
        </div>
        <!-- Terms and condition Popup Ends -->

        
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>

        <!-- #include file="/includes/footerBW.aspx" -->
        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />

         <BW:LeadCapture ID="ctrlLeadCapture" runat="server" />

        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript">

            var bikeName = "<%= bikeName %>";
            var bikeVersionPrice = "<%= totalPrice %>";
            var leadSourceId = '<%= (int)Bikewale.Entities.PriceQuote.PQSourceEnum.Desktop_DPQ_SecondaryDealers %>';
            <% if (isPrimaryDealer)
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

           <% if (_objEMI != null)
              { %>
            var BikeEMI = function () {
                var self = this;
                self.breakPoints = ko.observable(5);
                self.bikePrice = ko.observable(bikeVersionPrice);
                self.minDnPay = ko.observable(<%= _objEMI.MinDownPayment %> * bikeVersionPrice/100);
                self.maxDnPay = ko.observable(<%= _objEMI.MaxDownPayment %> * bikeVersionPrice/100);
                self.minTenure = ko.observable(<%= _objEMI.MinTenure %>);
                self.maxTenure = ko.observable(<%= _objEMI.MaxTenure  %>);
                self.minROI = ko.observable(<%= _objEMI.MinRateOfInterest %>);
                self.maxROI = ko.observable(<%= _objEMI.MaxRateOfInterest %>);
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
                self.totalPayable = ko.pureComputed({
                    read: function () {
                        return (self.downPayment() + (self.monthlyEMI() * self.tenure()));
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
                return finalEmi;
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
            <% } %>

            $(document).ready(function() {
                bikeVerLocation = GetBikeVerLoc();
                <%if (detailedDealer != null && detailedDealer.SecondaryDealers != null && detailedDealer.SecondaryDealerCount > 0)
                  {%>               
                    triggerGA('Dealer_PQ', 'Secondary_Dealer_Card_Shown', bikeVerLocation);
                <%}%>
                <% if (objQuotation != null && objQuotation.ExShowroomPrice > 0) {%>
                triggerGA('Dealer_PQ', 'Get_personal_loan_offers_shown', GetBikeVerLoc());
                <% }%>
            });

            $("input[name*='btnVariant']").on("click", function () {
                if ($(this).attr('versionid') == $('#hdnVariant').val()) {
                    return false;
                }
                $('#hdnVariant').val($(this).attr('title'));
                triggerGA('Dealer_PQ', 'Version_Change', bikeVerLocation);
            });

            function secondarydealer_Click(dealerID) {
                triggerGA('Dealer_PQ', 'Secondary_Dealer_Card_Clicked', bikeVerLocation);
                try {
                    var isSuccess = false;

                    var objData = {
                        "dealerId": dealerID,
                        "modelId": <%= modelId%>,
                        "versionId": versionId,
                        "cityId": cityId,
                        "areaId": areaId,
                        "clientIP": clientIP,
                        "pageUrl": pageUrl,
                        "sourceType": 1,
                        "pQLeadId": pqSourceId,
                        "deviceId": getCookie('BWC')
                    };

                    isSuccess = dleadvm.registerPQ(objData);

                    if (isSuccess) {
                        var rediurl = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + dleadvm.pqId() + "&VersionId=" + versionId + "&DealerId=" + dealerID;
                        window.location.href = "/pricequote/dealerpricequote.aspx?MPQ=" + Base64.encode(rediurl);
                    }
                } catch (e) {
                    console.warn("Unable to create pricequote : " + e.message);
                }
            }
            function openLeadCaptureForm(dealerID) {
                triggerGA('Dealer_PQ', 'Secondary_Dealer_Get_Offers_Clicked', bikeVerLocation);
                event.stopPropagation();
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
                $(".leadcapturebtn").click(function (e) {
                    ele = $(this);
                    var leadOptions = {
                        "dealerid": ele.attr('data-item-id'),
                        "dealername": ele.attr('data-item-name'),
                        "dealerarea": ele.attr('data-item-area'),
                        "versionid": versionId,
                        "leadsourceid": ele.attr('data-leadsourceid'),
                        "pqsourceid": ele.attr('data-pqsourceid'),
                        "isleadpopup": ele.attr('data-isleadpopup'),
                        "mfgCampid": ele.attr('data-mfgcampid'),
                        "pqid": pqId,
                        "pageurl": pageUrl,
                        "clientip": clientIP,
                        "dealerHeading" : ele.attr('data-item-heading'), 
                        "dealerMessage" : ele.attr('data-item-message'), 
                        "dealerDescription" : ele.attr('data-item-description'), 
                        "pinCodeRequired":ele.attr("data-ispincodrequired"),
                        "gaobject": {
                            cat: ele.attr("c"),
                            act: ele.attr("a"),
                            lab: bikeVerLocation
                        }

                    };
                    dleadvm.setOptions(leadOptions);
                });
        </script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : ""%>/src/dealerpricequote.js?<%= staticFileVersion%>"></script>
        <link href="https://fonts.googleapis.com/css?family=Open+Sans:400,600,700" rel="stylesheet" type="text/css" />
        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->
    </form>
</body>
</html>
