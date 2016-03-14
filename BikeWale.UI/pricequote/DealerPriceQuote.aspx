<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.BikeBooking.DealerPriceQuote" Trace="false" Async="true" EnableEventValidation="false" %>

<%@ Register Src="~/controls/AlternativeBikes.ascx" TagName="AlternativeBikes" TagPrefix="BW" %>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Import Namespace="Bikewale.BikeBooking" %>

<!doctype html>
<html>
<head>
    <%
        title = objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " + objPrice.objVersion.VersionName + " Price Quote ";
        description = objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " + objPrice.objVersion.VersionName + " price quote";
        keywords = "";
        AdId = "1395986297721";
        AdPath = "/1017752/Bikewale_PQ_";
    isAd970x90Shown = true;
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
        var Customername = "", email = "", mobileNo = "";
        var CustomerId = '<%= CurrentUser.Id %>';
        if (CustomerId != '-1') {
            Customername = '<%= objCustomer.CustomerName%>', email = '<%= objCustomer.CustomerEmail%>', mobileNo = '<%= objCustomer.CustomerMobile%>';
        } else {
            Customername = '<%= CustomerDetailCookie.CustomerName%>', email = '<%= CustomerDetailCookie.CustomerEmail%>', mobileNo = '<%= CustomerDetailCookie.CustomerMobile %>';
        }
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
                    <h1 class="font26 text-default margin-bottom10">On-road price quote for Bike in Area, City</h1>
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
                            <% if (objPrice != null)
                               { %>
                            <div class="pqBikeImage margin-bottom15">
                                <img alt="<%= objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " + objPrice.objVersion.VersionName %> Photos" src="<%= Bikewale.Utility.Image.GetPathToShowImages(objPrice.OriginalImagePath,objPrice.HostUrl,Bikewale.Utility.ImageSize._310x174) %>" title="<%= objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " + objPrice.objVersion.VersionName%> Photos" />
                            </div>
                            <% } %>
                            <% if (versionList.Count > 1)
                               { %>
                            <div class="pqVariants">
                                <p class="margin-left10 font16 text-light-grey leftfloat margin-top7">Version:</p>
                                <div class="position-rel">
                                    <div class="variants-dropdown rounded-corner2 leftfloat">
                                        <div class="variant-selection-tab">
                                            <span id="defaultVariant">Variant 1</span>
                                        </div>
                                        <span id="upDownArrow" class="rightfloat fa fa-angle-down position-abt pos-top10 pos-right10"></span>
                                    </div>
                                    <ul class="variants-dropdown-list">
                                        <li><input type="submit" value="Variant 1"/></li>
                                        <li><input type="submit" value="Variant 2 Variant 2 Variant 2 Variant 2" /></li>
                                    </ul>
                                </div>
                                <%--<div class="form-control-box leftfloat">
                                    <asp:DropDownList ID="ddlVersion" CssClass="form-control" runat="server" AutoPostBack="true"></asp:DropDownList>
                                </div>--%>
                            </div>
                            <% } %>
                        </div>
                        <div class="grid-6 padding-top15 padding-bottom20 padding-right20" id="PQDetailsContainer">
                        <% if (objPrice != null)
                           { %>
                        <%--<p class="font20 text-bold margin-bottom20"><%= objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " +objPrice.objVersion.VersionName%></p>--%>
                        <% } %>
                        <% if (!String.IsNullOrEmpty(cityArea))
                           { %>
                        <p class="font14 text-default text-bold margin-bottom15">On-road price - Dealership Name</p>
                        <% } %>
                        <div runat="server">
                            <div>
                                <% if (objPrice != null)
                                   { %>
                                <table class="font14" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <asp:Repeater ID="rptPriceList" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td width="210" class="PQDetailsTableTitle padding-bottom15 text-light-grey">
                                                    <%# DataBinder.Eval(Container.DataItem,"CategoryName") %> <%# Bikewale.common.DealerOfferHelper.HasFreeInsurance(dealerId.ToString(),"",DataBinder.Eval(Container.DataItem,"CategoryName").ToString(),Convert.ToUInt32(DataBinder.Eval(Container.DataItem,"Price").ToString()),ref insuranceAmount) ? "<img class='insurance-free-icon' alt='Free_icon' src='http://imgd1.aeplcdn.com/0x0/bw/static/free_red.png' title='Free_icon'/>" : "" %>
                                                </td>
                                                <td align="right" class="PQDetailsTableAmount padding-bottom10 text-default">
                                                    <span class="fa fa-rupee"></span> <span id="exShowroomPrice"><%#CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"Price").ToString()) %></span>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <tr>
                                        <td colspan="2">
                                            <div class="border-solid-top padding-bottom10"></div>
                                        <td>
                                    </tr>
                                    <%
                                       if (IsDiscount)//if (IsInsuranceFree)
                                       {
                                    %>
                                    <%--<tr>
                                        <td class="PQDetailsTableTitle padding-bottom10">Total on road price</td>
                                        <td align="right" class="PQDetailsTableAmount text-bold padding-bottom10">
                                            <span class="fa fa-rupee"></span><span style="text-decoration: line-through;"><%= CommonOpn.FormatPrice(totalPrice.ToString()) %></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="PQDetailsTableTitle padding-bottom10">Minus insurance</td>
                                        <td align="right" class="PQDetailsTableAmount text-bold padding-bottom10">
                                            <span class="fa fa-rupee"></span><span><%= CommonOpn.FormatPrice(insuranceAmount.ToString()) %></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" class="PQDetailsTableTitle font18 text-bold PQOnRoadPrice">Total on road price</td>
                                        <td align="right" class="PQDetailsTableAmount font20 text-bold">
                                            <span class="fa fa-rupee"></span><span><%= CommonOpn.FormatPrice((totalPrice - insuranceAmount).ToString()) %></span>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td class="PQDetailsTableTitle padding-bottom10">On-road price</td>
                                        <td align="right" class="PQDetailsTableAmount padding-bottom10">
                                            <span class="fa fa-rupee"></span> <span style="text-decoration: line-through;"><%= CommonOpn.FormatPrice(totalPrice.ToString()) %></span>
                                        </td>
                                    </tr>
                                    <asp:Repeater ID="rptDiscount" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td width="210" class="PQDetailsTableTitle padding-bottom10">
                                                   Minus <%# DataBinder.Eval(Container.DataItem,"CategoryName") %> 
                                                </td>
                                                <td align="right" class="PQDetailsTableAmount padding-bottom10">
                                                    <span class="fa fa-rupee"></span> <span id="exShowroomPrice">
                                                        <%#CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"Price").ToString()) %></span>
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
                                        <td valign="middle" class="PQDetailsTableTitle text-black font14 PQOnRoadPrice">Total on-road price</td>
                                        <td align="right" class="PQDetailsTableAmount font18 text-black">
                                            <span class="fa fa-rupee"></span> <span><%= CommonOpn.FormatPrice((totalPrice - totalDiscount).ToString()) %></span>
                                        </td>
                                    </tr>
                                    <%
                                       }
                                       else
                                       {
                                    %>
                                    <tr>
                                        <td class="PQDetailsTableTitle font14 text-black PQOnRoadPrice">Total on-road price</td>
                                        <td align="right" class="PQDetailsTableAmount font18 text-black">
                                            <span class="fa fa-rupee"></span> <span><%= CommonOpn.FormatPrice(totalPrice.ToString()) %></span>

                                        </td>
                                    </tr>

                                    <% } %>

                                    <tr>
                                        <td colspan="2" class="text-right"><a class="font14 text-link" id="leadLink" name="leadLink" onclick="dataLayer.push({ event: 'Bikewale_all', cat: 'New Bike Booking - <%=BikeName.Replace("'","")%>', act: 'Click Button Get dealer details',lab: 'Clicked on Button Get_Dealer_Details' });">Get more details</a></td>
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
                        <div class="grid-12 padding-right20 padding-bottom10 padding-left20 font14">
                            <p class="text-bold padding-top20 margin-bottom5 border-light-top">Exclusive offers from this dealer:</p>
                            <ul class="pricequote-benefits-list text-light-grey">
                                <li>
                                    <span class="inline-block pq-benefits-image pricequote-sprite offers-insurance-icon margin-right10"></span>
                                    <span class="inline-block pq-benefits-title">Free bike insurance</span>
                                </li>
                                <li>
                                    <span class="inline-block pq-benefits-image pricequote-sprite offers-voucher-icon margin-right10"></span>
                                    <span class="inline-block pq-benefits-title">Flipkart voucher worth <span class="fa fa-rupee"></span>1,000</span>
                                </li>
                                <li>
                                    <span class="inline-block pq-benefits-image pricequote-sprite offers-pickup-icon margin-right10"></span>
                                    <span class="inline-block pq-benefits-title">Document pickup and bike delivery</span>
                                </li>
                            </ul>
                            <div class="clear"></div>
                        </div>
                        <div class="clear"></div>
                        <div class="grid-12 bg-light-grey content-inner-block-20">
                            <p class="font14 text-bold margin-bottom20">Get buying assistance from this dealer:</p>
                            <div class="buying-assistance-form">
                                <div class="form-control-box margin-right10">
                                    <input type="text" class="form-control" placeholder="Name" id="assistanceGetName">
                                    <span class="bwsprite error-icon errorIcon"></span>
                                    <div class="bw-blackbg-tooltip errorText"></div>
                                </div>
                                <div class="form-control-box margin-right10">
                                    <input type="text" class="form-control" placeholder="Email id" id="assistanceGetEmail">
                                    <span class="bwsprite error-icon errorIcon"></span>
                                    <div class="bw-blackbg-tooltip errorText"></div>
                                </div>
                                <div class="form-control-box margin-right10 assistance-form-mobile">
                                    <input type="text" class="form-control" placeholder="Number" id="assistanceGetMobile">
                                    <span class="bwsprite error-icon errorIcon"></span>
                                    <div class="bw-blackbg-tooltip errorText"></div>
                                </div>
                                <a class="btn btn-orange leftfloat" id="buyingAssistanceSubmitBtn">Submit</a>
                                <div class="clear"></div>
                            </div>
                        </div>
                        <div class="clear"></div>
                        <div class="grid-12 padding-top20 padding-right20 padding-bottom5 padding-left20 font14">
                            <p class="text-bold margin-bottom5">Benefits of buying a bike from this dealer:</p>
                            <ul class="pricequote-benefits-list text-light-grey">
                                <li>
                                    <span class="inline-block pq-benefits-image pricequote-sprite benefit-offers-icon margin-right10"></span>
                                    <span class="inline-block pq-benefits-title">Exclusive offers</span>
                                </li>
                                <li>
                                    <span class="inline-block pq-benefits-image pricequote-sprite benefit-assistance-icon margin-right10"></span>
                                    <span class="inline-block pq-benefits-title">Complete buying assistance</span>
                                </li>
                                <li>
                                    <span class="inline-block pq-benefits-image pricequote-sprite benefit-cancel-icon margin-right10"></span>
                                    <span class="inline-block pq-benefits-title">Easy cancellations</span>
                                </li>
                            </ul>
                            <div class="clear"></div>
                        </div>
                        <div class="clear"></div>
                        <div class="grid-12 padding-left20 padding-right20 padding-bottom20 font14">
                            <p class="text-bold padding-top20 margin-bottom5 border-light-top">Pay <span class="fa fa-rupee"></span> 5,000 online and book this bike:</p>
                            <ul class="pricequote-benefits-list pq-benefits-booking-list text-light-grey">
                                <li>
                                    <p>Save on dealer visits</p>
                                </li>
                                <li>
                                    <p>Secure online payments</p>
                                </li>
                                <li>
                                    <p>Complete buyer protection</p>
                                </li>
                            </ul>
                            <div class="clear"></div>
                            <div class="grid-12 alpha omega margin-top10">
                                <div class="grid-9 alpha">
                                    <p class="font14 text-light-grey">The booking amount of <span class="fa fa-rupee"></span> 5,000 has to be paid online and balance amount of <span class="fa fa-rupee"></span> 1,21,000 has to be paid at the dealership.</p>
                                </div>
                                <div class="grid-3 omega text-right">
                                    <a class="btn btn-grey btn-sm font14">Book now</a>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <div class="clear"></div>

                        <div class="grid-12 padding-left20 padding-right20 padding-bottom20 font14">
                            <p class="text-bold padding-top20 margin-bottom15 border-light-top">Get EMI quote from this dealer:</p>
                            <div class="finance-emi-container">
                                    <div class="emi-slider-box">
                                        <div class="emi-slider-box-left-section">
                                            <p>Down payment</p>
                                            <div id="downPaymentSlider"
                                                class="ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all">
                                                <div class="ui-slider-range ui-widget-header ui-corner-all ui-slider-range-min"></div>
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
                                            <span id="downPaymentAmount" class="text-bold"></span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="emi-slider-box">
                                        <div class="emi-slider-box-left-section">
                                            <p>Loan Amount</p>
                                            <div id="loanAmountSlider"
                                                class="ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all">
                                                <div class="ui-slider-range ui-widget-header ui-corner-all ui-slider-range-min"></div>
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
                                            <span id="loanAmount" class="text-bold"></span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="emi-slider-box">
                                        <div class="emi-slider-box-left-section">
                                            <p>Tenure (Months)</p>
                                            <div id="tenureSlider"
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
                                            <span id="rateOfInterestPercentage" class="text-bold">5</span>
                                            <span>%</span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                
                                    <div class="margin-top10">
                                        <div class="grid-8 alpha text-grey text-bold padding-top10">
                                            <p class="leftfloat margin-right10 position-rel pos-top3">Indicative EMI:</p>
                                            <div class="indicative-emi-amount margin-right10 leftfloat">
                                                <span class="font18"><span class="fa fa-rupee"></span></span>
                                                <span id="emiAmount" class="font18">55,000</span>
                                            </div>
                                            <p class="font14 leftfloat position-rel pos-top3">per month</p>
                                            <div class="clear"></div>
                                        </div>
                                        <div class="grid-4 omega text-right">
                                            <a class="btn btn-grey btn-md font14">Get EMI quote</a>
                                        </div>
                                        <div class="clear"></div>
                                        <p id="disclaimerText" class="margin-top15 font11 text-light-grey"><span class="bwsprite disclaimer-sm-icon"></span>On-road price and EMI calculator is provided for information. BikeWale does not own any responsibility for the same.</p>
                                    </div>
                            </div>
                        </div>
                    </div>
                    <div class="grid-4 padding-top20 dealer-pointer" id="PQDealerSidebarContainer"> <!-- when no premium dealer remove dealer-pointer class -->
                        <div class="pqdealer-and-listing-container">
                            <div class="pqdealer-sidebar-panel position-rel">
                                <div> <!-- when no premium dealer hide this div -->
                                    <p class="font18 text-bold text-darker-black">Dealership Name</p>
                                    <p class="font14 text-light-grey margin-bottom15">Area</p>
                                    <div class="border-solid-top padding-top15">
                                        <p class="font14 text-light-grey margin-bottom10">Vishwaroop IT Park, Sector 30, Navi Mumbai, Maharashtra, 400067</p>
                                        <p class="font16 text-bold margin-bottom15"><span class="fa fa-phone"></span> 022-6667 8888</p>
                                        <iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d30165.66210531427!2d72.98105033863713!3d19.076582232598167!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x3be7c136b519107b%3A0x8452b99754be0fc8!2sVashi%2C+Navi+Mumbai%2C+Maharashtra+400703!5e0!3m2!1sen!2sin!4v1457345328664" frameborder="0" width="100%" height="90" style="border:1px solid #e2e2e2"></iframe>
                                        <a class="btn btn-inv-grey btn-full-width font14 margin-top15">Get offers from this dealer</a>
                                    </div>
                                </div>
                                <div class="pq-no-premium-dealer font14 text-light-grey">Sorry, there are no dealers nearby</div> <!-- when no premium dealer show this div -->
                            </div>
                            <div class="pq-sidebar-dealer-listing margin-top15 padding-right20 padding-left20">
                                <p class="padding-bottom15">Prices available from 27 more dealers:</p>
                                <ul>
                                    <li>
                                        <a href="" class="font18 text-bold text-darker-black margin-right20">Dealership Name 1</a>
                                        <p class="font14 text-light-grey">Area</p>
                                    </li>
                                    <li>
                                        <a href="" class="font18 text-bold text-darker-black margin-right20">Dealership Name 2</a>
                                        <p class="font14 text-light-grey">Area</p>
                                    </li>
                                    <li>
                                        <a href="" class="font18 text-bold text-darker-black margin-right20">Dealership Name 3</a>
                                        <p class="font14 text-light-grey">Area</p>
                                    </li>
                                </ul>
                            </div>
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
                            id="getFullName" data-bind="value: fullName">
                        <span class="bwsprite error-icon errorIcon"></span>
                        <div class="bw-blackbg-tooltip errorText">Please enter your first name</div>
                    </div>
                    <div class="form-control-box personal-info-list">
                        <input type="text" class="form-control get-email-id" placeholder="Email address (mandatory)"
                            id="getEmailID" data-bind="value: emailId">
                        <span class="bwsprite error-icon errorIcon"></span>
                        <div class="bw-blackbg-tooltip errorText">Please enter email address</div>
                    </div>
                    <div class="form-control-box personal-info-list">
                        <p class="mobile-prefix">+91</p>
                        <input type="text" class="form-control padding-left40 get-mobile-no" placeholder="Mobile no. (mandatory)"
                            id="getMobile" maxlength="10" data-bind="value: mobileNo">
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
        </div>
        <!-- lead capture popup End-->

         <!-- Terms and condition Popup start -->
            <div class="termsPopUpContainer content-inner-block-20 hide" id="termsPopUpContainer">
                <div class="fixed-close-btn-wrapper">
                    <div class="termsPopUpCloseBtn fixed-close-btn bwsprite cross-lg-lgt-grey cur-pointer"></div>
                </div>
				<h3>Terms and Conditions</h3>
                <div class="hide" style="vertical-align: middle; text-align: center;" id="termspinner">
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
        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/dealerpricequote.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript">
            var bikeName = '<%= BikeName %>';
            var getCityArea = GetGlobalCityArea();
            $('#btnGetDealerDetails, #btnBikeBooking').click(function () {
                var cookieValue = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + versionId + "&DealerId=" + dealerId;                
                window.location.href = '/pricequote/bookingsummary_new.aspx?MPQ=' + Base64.encode(cookieValue);
            });

            var freeInsurance = $("img.insurance-free-icon");
            if (!freeInsurance.length) {
                cityArea = GetGlobalCityArea();
                $("table tr td.PQDetailsTableTitle:contains('Insurance')").first().append(" <br/><div style='position: relative; color: #999; font-size: 11px; margin-top: 1px;'>Save up to 60% on insurance - <a target='_blank' href='/insurance/' onclick=\"dataLayer.push({ event: 'Bikewale_all', cat: 'Dealer_PQ', act: 'Insurance_Clicked',lab: '<%= String.Format("{0}_{1}_{2}_",objPrice.objMake.MakeName,objPrice.objModel.ModelName,objPrice.objVersion.VersionName)%>" + cityArea + "' });\">PolicyBoss</a> <span style='margin-left: 8px; vertical-align: super; font-size: 9px;'>Ad</span></div>");
            }

            // JavaScript Document

            var leadBtnBookNow = $("#leadBtnBookNow,#leadLink"), leadCapturePopup = $("#leadCapturePopup");
            var fullName = $("#getFullName");
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
                    $("div#contactDetailsPopup").show();
                    $("#otpPopup").hide();
                    $('body').addClass('lock-browser-scroll');
                    $(".blackOut-window").show();

                });
                $(".leadCapture-close-btn, .blackOut-window").on("click mouseup", function () {
                    leadCapturePopup.hide();
                    $('body').removeClass('lock-browser-scroll');
                    $(".blackOut-window").hide();
                });

                $(document).on('keydown', function (e) {
                    if (e.keyCode === 27) {
                        $("#leadCapturePopup .leadCapture-close-btn").click();
                        $("div.termsPopUpCloseBtn").click();
                    }
                });
            });

            ko.applyBindings(customerViewModel, $('#leadCapturePopup')[0]);

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
                            "leadSourceId": 1,
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
                            "source": 1
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
                            $("#personalInfo").hide();
                            $("#leadCapturePopup .leadCapture-close-btn").click();                            
                            var cookieValue = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + versionId + "&DealerId=" + dealerId;
                            window.location.href = "/pricequote/BikeDealerDetails.aspx?MPQ=" + Base64.encode(cookieValue);
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
                        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Dealer_PQ', 'act': 'Lead_Submitted', 'lab': bikeName + '_' +getCityArea });
                    }
                };

                otpBtn.click(function () {
                    $('#processing').show();
                    if (!validateOTP())
                        $('#processing').hide();

                    if (validateOTP() && ValidateUserDetail()) {
                        customerViewModel.generateOTP();
                        if (customerViewModel.IsVerified()) {
                            $("#personalInfo").hide();
                            $(".booking-dealer-details").removeClass("hide").addClass("show");
                            $('#processing').hide();

                            detailsSubmitBtn.show();
                            otpText.val('');
                            otpContainer.removeClass("show").addClass("hide");

                            // OTP Success
                            dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation_Page', 'act': 'Step_1_OTP_Successful_Submit', 'lab': getCityArea });
                            $("#leadCapturePopup .leadCapture-close-btn").click();
                            
                            var cookieValue = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + versionId + "&DealerId=" + dealerId;
                            window.location.href = "/pricequote/BikeDealerDetails.aspx?MPQ=" + Base64.encode(cookieValue);
                        }
                        else {
                            $('#processing').hide();
                            otpVal("Please enter a valid OTP.");
                            // push OTP invalid
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
                var a = fullName.val().length;
                if ((/&/).test(fullName.val())) {
                    isValid = false;
                    setError(fullName, 'Invalid name');
                }
                else
                    if (a == 0) {
                        isValid = false;
                        setError(fullName, 'Please enter your first name');
                    }
                    else if (a >= 1) {
                        isValid = true;
                        nameValTrue()
                    }
                if (!isValid) { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation Page', 'act': 'Step_1_Submit_Error_Name', 'lab': getCityArea }); }
                return isValid;
            }

            function nameValTrue() {
                hideError(fullName)
                fullName.siblings("div").text('');
            };

            fullName.on("focus", function () {
                hideError(fullName);
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
                    if (validateMobile()) {
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
                if (!isValid) { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation Page', 'act': 'Step_1_Submit_Error_Email', 'lab': getCityArea }); }
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
                if (!isValid) { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation Page', 'act': 'Step_1_Submit_Error_Mobile', 'lab': getCityArea }); }
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
                    bindInsuranceText();
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
                var val = fullName.val() + '&' + emailid.val() + '&' + mobile.val();
                SetCookie("_PQUser", val);
            }

            $(".edit-mobile-btn").on("click", function () {
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
            $("#leadBtnBookNow").on("click", function () {
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Dealer_PQ', 'act': 'Get_More_Details_Clicked_Button', 'lab': bikeName + '_' + getCityArea });
            });
            $("#leadLink").on("click", function () {
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Dealer_PQ', 'act': 'Get_More_Details_Clicked_Link', 'lab': bikeName + '_' + getCityArea });
            });

            $('.tnc').on('click', function (e) {
                LoadTerms($(this).attr("id"));
            });

            function LoadTerms(offerId) {
                $("div#termsPopUpContainer").show();
                $(".blackOut-window").show();
                if (offerId != 0 && offerId != null) {
                    $(".termsPopUpContainer").css('height', '150')
                    $('#termspinner').show();
                    $('#terms').empty();
                    $.ajax({
                        type: "GET",
                        url: "/api/Terms/?offerMaskingName=&offerId=" + offerId,
                        dataType: 'json',
                        success: function (response) {
                            $('#termspinner').hide();
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
                $(".termsPopUpContainer").css('height', '500');
            }

            $(".termsPopUpCloseBtn,.blackOut-window").on('mouseup click', function (e) {
                $("div#termsPopUpContainer").hide();
                $(".blackOut-window").hide();
            });
        </script>
    </form>
</body>
</html>
