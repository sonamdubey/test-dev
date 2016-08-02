<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.BikeBooking.DealerPriceQuote" Trace="false" Async="true" %>

<%@ Register Src="~/m/controls/AlternativeBikes.ascx" TagPrefix="BW" TagName="AlternateBikes" %>
<%@ Register Src="~/m/controls/LeadCaptureControl.ascx" TagName="LeadCapture" TagPrefix="BW" %>

<%@ Import Namespace="Bikewale.Common" %>
<%@ Import Namespace="Bikewale.BikeBooking" %>
<!doctype html>
<html>
<head>
	<%
		title = String.Format("{0} {1} {2} Price Quote", objPriceQuote.objMake.MakeName, objPriceQuote.objModel.ModelName, objPriceQuote.objVersion.VersionName);
		description = String.Format("{0} {1} {2} price quote", objPriceQuote.objMake.MakeName, objPriceQuote.objModel.ModelName, objPriceQuote.objVersion.VersionName);
		keywords = string.Empty;
		canonical = string.Empty;
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

		var campaignId = "<%= objExQuotation != null ? objExQuotation.CampaignId : 0 %>";
		var manufacturerId = "<%= objExQuotation != null ? objExQuotation.ManufacturerId : 0 %>";

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
				<h1 class="padding-left10"><%= objPriceQuote.objMake.MakeName + " " + objPriceQuote.objModel.ModelName %></h1>
			</div>
			<div class="clear"></div>


			<div id="versionsDropdownWrapper" class="margin-top10 padding-right10 padding-left10">
				<p class="grid-3 alpha omega version-label-text font14 text-light-grey margin-top5 leftfloat">Version:</p>
				<%if (versionList != null && versionList.Count > 1)
				  { %>
				<asp:DropDownList ID="ddlVersion" CssClass="form-control" runat="server" AutoPostBack="true"></asp:DropDownList>
				<%}
				  else if (objPriceQuote.objVersion != null)
				  { %>
				<span id='versText' class="font14 margin-bottom10 text-default leftfloat margin-top5 margin-right20"><%= objPriceQuote.objVersion.VersionName %></span>
				<%} %>
			</div>

			<!--Price Breakup starts here-->
			<div class="margin-top15 padding-left10 padding-right10">
				<%if (isPriceAvailable)
				  { %>
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
						<td align="left" class="text-dark-black padding-bottom5">Total On Road Price</td>
						<td align="right" class="text-dark-black padding-bottom5">
							<div><span class="bwmsprite inr-xxsm-icon"></span><%= Bikewale.Utility.Format.FormatPrice(totalPrice.ToString()) %></div>
						</td>
					</tr>
					<tr>
						<td colspan="2" align="right" class="text-light-grey padding-bottom15">
						   <a id='getMoreDetails' leadSourceId="23" class="get-offer-link bw-ga leadcapturebtn" c="Dealer_PQ" a="Get_more_details_below_price_clicked" f="GetBikeVerLoc" >Get more details</a>
						</td>
					</tr>
					<%
						}
					%>
				</table>
				 <%} else if (objExQuotation != null && objExQuotation.ExShowroomPrice > 0)
				   {%>
				<table width="100%" border="0" cellspacing="0" cellpadding="0" class="pqTable font14">
					<tr>
						<td class="text-light-grey padding-bottom15" width="75%" align="left">Ex-Showroom Price</td>
						<td class="padding-bottom15" width="25%" align="right"><span class="bwmsprite inr-xxsm-icon"></span><%= CommonOpn.FormatPrice(objExQuotation.ExShowroomPrice.ToString()) %></td>
					</tr>
					<tr>
						<td class="text-light-grey padding-bottom15" align="left">RTO</td>
						<td class="padding-bottom15" align="right"><span class="bwmsprite inr-xxsm-icon"></span><%= CommonOpn.FormatPrice(objExQuotation.RTO.ToString()) %></td>
					</tr>
					<tr>
						<td class="text-light-grey padding-bottom15" align="left">Insurance <%--(<a target="_blank" onclick="dataLayer.push({ event: 'Bikewale_all', cat: 'BW_PQ', act: 'Insurance_Clicked',lab: '<%= (objExQuotation!=null)?(objExQuotation.MakeName + "_" + objExQuotation.ModelName + "_" + objExQuotation.VersionName + "_" + objExQuotation.City):string.Empty %>' });" href="/m/insurance/" style="display: inline-block; position: relative; font-size: 11px; margin-top: 1px;">
								Up to 60% off - PolicyBoss                                
						</a>)<span style="margin-left: 5px; vertical-align: super; font-size: 9px;">Ad</span>--%>
						</td>
						<td class="padding-bottom15" align="right"><span class="bwmsprite inr-xxsm-icon"></span><%=CommonOpn.FormatPrice(objExQuotation.Insurance.ToString()) %></td>
					</tr>
					
					<tr align="left">
						<td height="1" colspan="2" class="break-line padding-bottom10"></td>
					</tr>
					<tr>
						<td class="text-dark-black padding-bottom15" align="left">On-road price</td>
						<td class="text-dark-black padding-bottom15" align="right"><span class="bwmsprite inr-xxsm-icon"></span><%=CommonOpn.FormatPrice(objExQuotation.OnRoadPrice.ToString()) %></td>
					</tr>
				</table>
				<%}
				 else
				   {%>
				<div class="margin-top-10 padding5" style="background: #fef5e6;">Price for this bike is not available in this city.</div>
				<%} %>
			</div>
			<!--Price Breakup ends here-->

			<%if (isPrimaryDealer)
			  { %>
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
					
					<% if (dealerType != Bikewale.Entities.PriceQuote.DealerPackageTypes.Standard || !String.IsNullOrEmpty(maskingNum))
					{ %>
					<p class="text-light-grey margin-bottom10">Dealership contact details:</p>
				
					<p class="margin-bottom5"><span class="bwmsprite dealership-loc-icon vertical-top margin-right10"></span><span class="vertical-top dealership-address"><%= dealerAdd %></span></p>
					
					<%if (!string.IsNullOrEmpty(maskingNum))
					  { %>
					<p class="margin-bottom10"><span class="bwmsprite tel-sm-icon"></span><a id="aDealerNumber" href="tel:<%= maskingNum %>" class="font16 text-default text-bold"><%= maskingNum %></a></p>
					<%} %>

					<%if (!string.IsNullOrEmpty(contactHours))
					  { %>
					<p class="margin-bottom10"><span class="bwmsprite tel-sm-icon"></span><a id="aContactHours" class="font16 text-default text-bold"><%= contactHours %></a></p>
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
						google.maps.event.addDomListener(window, 'load', initializeDealerMap($("#dealerMap")[0], '<%= latitude %>', '<%= longitude %>'));
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
					<%if (dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium || dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Deluxe)
					  { %>
					<div class="padding-top15 padding-bottom15 border-light-top">
						<span class="font15 text-bold leftfloat">Get EMI quote</span>
						<span class="text-link rightfloat calculate-emi-target">Calculate now</span>
						<div class="clear"></div>
					</div>
					<%} %>
				  <% } %>
					<div id="pqRemoveHeader"></div>
				</div>               
			</div>
			<!-- show below div when no premium dealer -->
			<%}
			  else if (isSecondaryDealer)
			  { %>
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
								<a href="javascript:void(0)" dealerid="<%# DataBinder.Eval(Container.DataItem,"DealerId") %>" class="secondary-dealer font18 text-darker-black text-bold"><%# DataBinder.Eval(Container.DataItem,"Name") %></a><br />
								<p class="font14 text-light-grey"><%# DataBinder.Eval(Container.DataItem,"Area") %></p>
							</li>
						</ItemTemplate>
					</asp:Repeater>
				</ul>
			</div>
			<%} %>
			<!-- Dealer Widget ends here -->

			<!--Dealer Campaign starts here -->
			<div class="city-unveil-offer-container <%= (objExQuotation != null && objExQuotation.CampaignId > 0) ? "" : "hide" %>">
				<h4 class="border-solid-bottom padding-bottom5 margin-bottom10"><span class="bwmsprite disclaimer-icon margin-right5"></span>                   
						Get following details from <%=objVersionDetails.MakeBase.MakeName %>:                   
				</h4>
				<ul class="bike-details-list-ul">
					<li>
						<span class="show">Offers from the nearest dealers</span>
					</li>
					<li>
						<span class="show">Waiting period on this bike at the dealership</span>
					</li>
					<li>

						<span class="show">Nearest dealership from your place</span>
					</li>
					<li>
						<span class="show">Finance options on this bike</span>
					</li>
				</ul>
			</div>
			<div class="grid-12 float-button float-fixed clearfix <%= (objExQuotation != null && objExQuotation.CampaignId > 0) ? "" : "hide" %>">
				<input type="button" value="Get more details" leadSourceId="29" class="btn btn-full-width btn-sm margin-right10 leftfloat btn-orange leadcapturebtn" id="getMoreDetailsBtnCampaign" />
			</div>

			<!--Dealer Campaign ends here -->

			<!--Exciting Offers section starts here-->
			<div class="grid-12 float-button float-fixed <%= (objExQuotation != null && objExQuotation.CampaignId > 0) ? "hide" : "" %>">
				<%if (!string.IsNullOrEmpty(maskingNum))
				  { %>
				<div class="grid-6 alpha omega padding-right5">
					<input type="button" data-role="none" id="leadBtnBookNow" leadSourceId="17" name="leadBtnBookNow" class="btn btn-sm btn-full-width btn-white leadcapturebtn" value="Get offers" />
				</div>
				<%}

					if (isPrimaryDealer){ %>

				<div class="<%= !string.IsNullOrEmpty(maskingNum) ? "grid-6 omega padding-left5" : "" %>">
					<a id="calldealer" class="btn btn-sm btn-full-width btn-orange rightfloat" href="tel:<%= maskingNum %>">
						<span class="bwmsprite tel-white-icon margin-right5"></span>Call dealer</a>
				</div>
				<%} %>
			</div>
			<div class="clear"></div>
			<!--Exciting Offers section ends here-->
		</div>
		<%if (dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium || dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Deluxe)
		  { %>
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
								data-bind="slider: downPayment, sliderOptions: { min: minDnPay(), max: maxDnPay(), range: 'min', step: 1, value: Math.round(((maxDnPay() - minDnPay()) / 2) + minDnPay()) }"
								class="ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all">
								<div class="ui-slider-range ui-widget-header ui-corner-all ui-slider-range-min"></div>
								<span class="ui-slider-handle ui-state-default ui-corner-all" tabindex="0"></span>
							</div>
							<div class="slider-range-points">
								<ul class="range-five-pointsUL range-pointsUL" data-bind="">
									<li class="range-points-bar"><span data-bind="text: $.valueFormatter(minDnPay())"></span></li>
									<!-- ko foreach: new Array(breakPoints() - 1 ) -->
									<li class="range-points-bar"><span data-bind="text: $.createSliderPoints($index() + 1, $parent.minDnPay(), $parent.maxDnPay(), $parent.breakPoints(), 1)"></span></li>
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
									<li class="range-points-bar"><span data-bind="text: $.createSliderPoints($index() + 1, $parent.bikePrice() - $parent.maxDnPay(), $parent.bikePrice() - $parent.minDnPay(), $parent.breakPoints(), 1)"></span></li>
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
									<li class="range-points-bar"><span data-bind="text: $.createSliderPoints($index() + 1, $parent.minTenure(), $parent.maxTenure(), $parent.breakPoints(), 2)"></span></li>
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
									<li class="range-points-bar"><span data-bind="text: $.createSliderPoints($index() + 1, $parent.minROI(), $parent.maxROI(), $parent.breakPoints())"></span></li>
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
				<a id="btnEmiQuote" leadSourceId="18" class="btn btn-orange emi-quote-btn margin-bottom20 leadcapturebtn">Get EMI quote</a>
			</div>
		</div>
		<%} %>

		<section class="<%= (ctrlAlternateBikes.FetchedRecordsCount > 0) ? "" : "hide" %>">
			<div class="container margin-bottom30">
				<div class="grid-12">
					<h2 class="font18 margin-top20px margin-bottom20 text-center padding-top20"><%= objPriceQuote.objMake.MakeName + " " + objPriceQuote.objModel.ModelName  %> alternatives</h2>

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
        <BW:LeadCapture ID="ctrlLeadCapture" runat="server" />
		<!-- Lead Capture pop up end  -->

		<!-- #include file="/includes/footerBW_Mobile.aspx" -->
		<!-- all other js plugins -->
		<!-- #include file="/includes/footerscript_Mobile.aspx" -->
		<script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/dealerpricequote.js?<%= staticFileVersion %>"></script>

		<script type="text/javascript">
			var leadSourceId;
			var bikeName = "<%= BikeName %>";
			var bikeVersionPrice = "<%= totalPrice %>";
			var getCityArea = GetGlobalCityArea();
			var areaId = '<%= areaId %>';
			var versionName = "<%= objPriceQuote.objVersion.VersionName %>";

			$('#getDealerDetails,#btnBookBike').click(function () {
				var cookieValue = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + versionId + "&DealerId=" + dealerId;
				window.location.href = '/m/pricequote/bookingsummary_new.aspx?MPQ=' + Base64.encode(cookieValue);
			});
			
			var fullname = $("#getFullName");
			var emailid = $("#getEmailID");
			var mobile = $("#getMobile");
			var otpContainer = $(".mobile-verification-container");

			var detailsSubmitBtn = $("#user-details-submit-btn");
			var otpText = $("#getOTP");
			var otpBtn = $("#otp-submit-btn");

			var prevEmail = "";
			var prevMobile = "";

			var getOffersClicked = false;
			var getEMIClicked = false;
			var getMoreDetailsClicked = false;

			var getCityArea = GetGlobalCityArea();			

			$(".leadcapturebtn").click(function (e) {
			    ele = $(this);
			    var leadOptions = {
			        "dealerid": campaignId > 0 ? manufacturerId : dealerId,
			        "dealername": campaignId > 0 ? '<%= objPriceQuote.objMake.MakeName %>' : '<%= dealerName %>',
			        "dealerarea": '<%= dealerArea %>',
			        "versionid": versionId,
			        "leadsourceid": ele.attr('leadSourceId'),
			        "pqsourceid": ele.attr('pqsourceid'),
			        "pageurl": pageUrl,
			        "clientip": clientIP,
			        "isregisterpq": true,
			        "campid": campaignId
			    };
			    
			    dleadvm.setOptions(leadOptions);			    
			});			
			
			//// GA Tags
			$("#leadBtnBookNow").on("click", function () {
				leadSourceId = $(this).attr("leadSourceId");
				dataLayer.push({ "event": "Bikewale_all", "cat": "Dealer_PQ", "act": "Get_More_Details_Clicked_Button", "lab": bikeName + "_" + getCityArea });
			});
			$("#leadLink").on("click", function () {
				dataLayer.push({ "event": "Bikewale_all", "cat": "Dealer_PQ", "act": "Get_More_Details_Clicked_Link", "lab": bikeName + "_" + getCityArea });
			});
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
					'SourceType': '<%=Bikewale.Utility.BWConfiguration.Instance.MobileSourceId  %>',
					'VersionId': versionId,
					'pQLeadId': eval("<%= Convert.ToInt16(Bikewale.Entities.BikeBooking.LeadSourceEnum.DealerPQ_Mobile) %>"),
					'deviceId': getCookie('BWC'),
					'dealerId': secondaryDealerId,
					'refPQId': pqId
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
				self.minDnPay = ko.observable('<%= primarydealer.EMIDetails.MinDownPayment %>' * bikeVersionPrice / 100);
				self.maxDnPay = ko.observable('<%= primarydealer.EMIDetails.MaxDownPayment %>' * bikeVersionPrice / 100);
				self.minTenure = ko.observable(<%= primarydealer.EMIDetails.MinTenure %>);
				self.maxTenure = ko.observable(<%= primarydealer.EMIDetails.MaxTenure  %>);
				self.minROI = ko.observable(<%= primarydealer.EMIDetails.MinRateOfInterest %>);
				self.maxROI = ko.observable(<%= primarydealer.EMIDetails.MaxRateOfInterest %>);
				<%--self.processingFees = ko.observable(<%= primarydealer.EMIDetails.ProcessingFee %>);--%>
				self.processingFees = ko.observable(0);
				self.exshowroomprice = ko.observable(bikeVersionPrice);
				self.loan = ko.observable();

				self.tenure = ko.observable((self.maxTenure() - self.minTenure()) / 2 + self.minTenure());
				self.rateofinterest = ko.observable((self.maxROI() - self.minROI()) / 2 + self.minROI());
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
						return $.calculateEMI(self.loan(), self.tenure(), self.rateofinterest(), self.processingFees());
					},
					owner: this
				});
			}


			$.calculateEMI = function (loanAmount, tenure, rateOfInterest, proFees) {
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
				if (isNaN(num))
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
			ko.applyBindings(EMIviewModel, $("#emiPopup")[0]);

			<% } %>
			function GetBikeVerLoc() {
				return bikeName + "_" + versionName + "_" + getCityArea;
			}
			ga_pg_id = "7";
		</script>

	</form>
</body>
</html>
