<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.BikeBooking.DealerPriceQuote_v2" Trace="false" Async="true" %>

<%@ Register Src="~/m/controls/AlternativeBikes.ascx" TagPrefix="BW" TagName="AlternateBikes" %>
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
		<div class="bg-white padding-10-5 bottom-shadow margin-bottom10">
			<div class="bike-details margin-bottom15">
				<h1><%= objPriceQuote.objMake.MakeName + " " + objPriceQuote.objModel.ModelName %></h1>
				<p class="font12 text-light-grey padding-left15 padding-right5">Version:</p>
				<%if (versionList != null && versionList.Count > 1)
				{ %>
				<div class="dropdown-menu margin-left5">
					<p class="dropdown-label">Standard</p>
					<div class="dropdown-list-wrapper">
						<p class="dropdown-selected-item">Standard</p>
						<ul class="dropdown-menu-list dropdown-with-select">
							<li>Standard</li>
							<li>Platina 100 Alloy Wheel KS</li>
							<li>Kick Drum Start/Alloy</li>
						</ul>
					</div>
				</div>
				<%}
				else if (objPriceQuote.objVersion != null)
				{ %>
				<p class="single-version-label font14 margin-left5"><%= objPriceQuote.objVersion.VersionName %></p>
				<%} %>
				<div class="text-center">
					<img src="<%= Bikewale.Utility.Image.GetPathToShowImages(objPriceQuote.OriginalImagePath,objPriceQuote.HostUrl,Bikewale.Utility.ImageSize._210x118) %>" alt="<%= objPriceQuote.objMake.MakeName + " " + objPriceQuote.objModel.ModelName %>" title="<%= objPriceQuote.objMake.MakeName + " " + objPriceQuote.objModel.ModelName %>" border="0" />
				</div>
				<p class="font12 text-light-grey padding-left15 padding-right15">Location:</p>
				<p class="font16 text-bold padding-left15 padding-right15">
					<span>Andheri</span>,&nbsp;<span>Mumbai</span>
					<a href="javascript:void(0)" rel="nofollow" modelid="<%= objVersionDetails.ModelBase.ModelId %>" class="getquotation" data-popup-state="true"><span class="bwmsprite loc-change-blue-icon "></span></a>
				</p>
			</div>

			<!--Price Breakup starts here-->
			<div class="padding-left15 padding-right15">
				<%if (isPriceAvailable)
				  { %>
				<div class="break-line margin-bottom15"></div>
				<table width="100%" border="0" cellspacing="0" cellpadding="0" class="pqTable font14">
					<asp:Repeater ID="rptPriceList" runat="server">
						<ItemTemplate>
							<tr>
								<td align="left" width="65%" class="padding-bottom15"><%# DataBinder.Eval(Container.DataItem,"CategoryName") %> <%# Bikewale.common.DealerOfferHelper.HasFreeInsurance(dealerId.ToString(),"",DataBinder.Eval(Container.DataItem,"CategoryName").ToString(),Convert.ToUInt32(DataBinder.Eval(Container.DataItem,"Price").ToString()),ref insuranceAmount) ? "<img class='insurance-free-icon' alt='Free_icon' src='http://imgd1.aeplcdn.com/0x0/bw/static/free_red.png' title='Free_icon'/>" : "" %></td>
								<td align="right" width="35%" class="padding-bottom15 text-bold"><span class="bwmsprite inr-xxsm-icon"></span><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"Price").ToString()) %></td>
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
						<td align="left" class="padding-bottom15">Total on-road price</td>
						<td align="right" class="padding-bottom15 text-bold"><span class="bwmsprite inr-xxsm-icon"></span>&nbsp;<span style="text-decoration: line-through"><%= Bikewale.Utility.Format.FormatPrice(totalPrice.ToString()) %></span></td>
					</tr>
					<asp:Repeater ID="rptDiscount" runat="server">
						<ItemTemplate>
							<tr>
								<td align="left" class="padding-bottom15">Minus <%# DataBinder.Eval(Container.DataItem,"CategoryName") %> <%# Bikewale.common.DealerOfferHelper.HasFreeInsurance(dealerId.ToString(),"",DataBinder.Eval(Container.DataItem,"CategoryName").ToString(),Convert.ToUInt32(DataBinder.Eval(Container.DataItem,"Price").ToString()),ref insuranceAmount) ? "<img class='insurance-free-icon' alt='Free_icon' src='http://imgd1.aeplcdn.com/0x0/bw/static/free_red.png' title='Free_icon'/>" : "" %></td>
								<td align="right" class="padding-bottom15 text-bold"><span class="bwmsprite inr-xxsm-icon"></span><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"Price").ToString()) %></td>
							</tr>
						</ItemTemplate>
					</asp:Repeater>
					<tr align="left">
						<td height="1" colspan="2" class="break-line padding-bottom15"></td>
					</tr>
					<tr>
						<td align="left" class="font16 padding-bottom15">On-road price</td>
						<td align="right" class="font18 text-bold padding-bottom15"><span class="bwmsprite inr-sm-icon"></span>&nbsp;<%= Bikewale.Utility.Format.FormatPrice((totalPrice - totalDiscount).ToString()) %></td>
					</tr>
					<%
						}
						else
						{%>
					<tr>
						<td align="left" class="font16 padding-bottom5">On-road price</td>
						<td align="right" class="font18 text-bold padding-bottom5"><span class="bwmsprite inr-sm-icon"></span>&nbsp;<%= Bikewale.Utility.Format.FormatPrice(totalPrice.ToString()) %></td>
					</tr>
					<tr>
						<td colspan="2" align="right" class="text-light-grey padding-bottom15">
						   <a id='getMoreDetails' leadSourceId="23" class="get-offer-link bw-ga" c="Dealer_PQ" a="Get_more_details_below_price_clicked" f="GetBikeVerLoc" >Get more details</a>
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
						<td class="font16 padding-bottom15" align="left">On-road price</td>
						<td class="font18 text-bold padding-bottom15" align="right"><span class="bwmsprite inr-sm-icon"></span>&nbsp;<%=CommonOpn.FormatPrice(objExQuotation.OnRoadPrice.ToString()) %></td>
					</tr>
				</table>
				<%}
				 else
				   {%>
				<div class="margin-top10 padding5" style="background: #fef5e6;">Price for this bike is not available in this city.</div>
				<%} %>
			</div>
			<!--Price Breakup ends here-->

			<!-- Dealer Widget starts here -->
			<%if (isPrimaryDealer)
			  { %>
			<div id="pq-dealer-details" class="font14">
				<div class="dealer-title-card margin-bottom15">
					<h2 class="font18 text-white"><%= dealerName %></h2>
					<p><%= dealerArea %></p>
				</div>

				<div class="padding-left15 padding-right15 margin-bottom15">
					<p class="text-light-grey">On-road price</p>
					<p><span class="bwmsprite inr-md-icon"></span>&nbsp;<span class="font22 text-bold"><%= Bikewale.Utility.Format.FormatPrice(totalPrice.ToString()) %></span></p>
					<%if (dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium)
					  { %>
					<p class="text-light-grey margin-top5">EMI&nbsp;<span class="bwmsprite inr-xxsm-icon"></span><span class="text-default">Amount</span>&nbsp;onwards.&nbsp;<a href="javascript:void(0)" class="calculate-emi-target">Calculate Now</a></p>
					<%} %>
				</div>

				<%if (isOfferAvailable && (dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium || dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Deluxe))
				  { %>
				<div class="margin-right10 margin-bottom15 margin-left10 break-line"></div>
				<div class="padding-left15 padding-right15 margin-bottom10">
					<p class="text-bold margin-bottom15">Offers from this dealer:</p>
					<ul class="pricequote-benefits-list">
						<asp:Repeater ID="rptOffers" runat="server">
							<ItemTemplate>
								<li>
									<span class="offer-benefit-sprite offerIcon_<%# DataBinder.Eval(Container.DataItem,"OfferCategoryId") %>"></span>
									<span class="pq-benefits-title padding-top5 padding-left10"><%# DataBinder.Eval(Container.DataItem,"OfferText") %></span>
								</li>
							</ItemTemplate>
						</asp:Repeater>
					</ul>
				</div>
				<%} %>

				<%if (isBookingAvailable)
				  {%>
				<div class="padding-right5 padding-left5 margin-bottom20">
					<div class="vertical-top">
						<a href="/m/pricequote/bookingsummary_new.aspx?MPQ=<%= mpqQueryString %>" class="btn btn-teal btn-sm-0">Book Now</a>
					</div><p class="booknow-label font11 line-height-1-5 text-xx-light padding-left10 vertical-top">
						Pay <span class="bwmsprite inr-grey-xxxsm-icon"></span><%=Bikewale.Utility.Format.FormatPrice((objPriceQuote.PrimaryDealer.BookingAmount).ToString()) %> to book online and<br />
						balance amount of <span class="bwmsprite inr-grey-xxxsm-icon"></span><%=Bikewale.Utility.Format.FormatPrice((totalPrice - objPriceQuote.PrimaryDealer.BookingAmount).ToString()) %> at dealership
					</p>
				</div>
				<%} %>
				
				<% if (dealerType != Bikewale.Entities.PriceQuote.DealerPackageTypes.Standard || !String.IsNullOrEmpty(maskingNum))
					{ %>
					<div class="border-solid margin-right5 margin-left5 margin-bottom15">

					<%if (dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium)
					  { %>
						<script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDY0kkJiTPVd2U7aTOAwhc9ySH6oHxOIYM"></script>
						<div id="dealerMap" style="height: 94px; position: relative; text-align: center">
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

						<div class="padding-15-20">
							<% if (dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium || dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Deluxe)
							   {%>
								<p class="margin-bottom10">
									<span class="bwmsprite dealership-loc-icon inline-block margin-right15"></span>
									<span class="inline-block dealership-address"><%= dealerAdd %></span>
								</p>
							<%} %>
							<%if (!string.IsNullOrEmpty(maskingNum))
							  { %>
								<p class="margin-bottom10">
									<span class="bwmsprite tel-grey-sm-icon inline-block margin-right15"></span>
									<a id="aDealerNumber" href="tel:<%= maskingNum %>" class="inline-block text-default"><%= maskingNum %></a>
								</p>
							<%} %>
							<div>
								<span class="bwmsprite clock-icon vertical-top margin-right15"></span>
								<span class="inline-block">10am to 6pm</span>
							</div>
						</div>
					
					</div>

					<%if (isUSPAvailable && (dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium))
					  { %>
					<div class="padding-right15 padding-left15 padding-bottom10">
						<p class="text-bold margin-bottom15">Benefits from this dealer:</p>
						<ul class="pricequote-benefits-list">
							<asp:Repeater ID="rptBenefits" runat="server">
								<ItemTemplate>
									<li>
										<span class="offer-benefit-sprite benifitIcon_<%# DataBinder.Eval(Container.DataItem,"CatId") %>"></span>
										<span class="pq-benefits-title padding-left15"><%# DataBinder.Eval(Container.DataItem,"BenefitText") %></span>
									</li>
								</ItemTemplate>
							</asp:Repeater>
						</ul>
					</div>
					<%} %>
					
				  <% } %>
			</div>
			<%} %>
			<!-- Dealer Widget ends here -->

			<div id="pricequote-floating-button-wrapper" class="grid-12 alpha omega">
				<div class="float-button float-fixed">
					<%if (!string.IsNullOrEmpty(maskingNum))
					  { %>
					<div class="grid-7 alpha omega padding-right5">
						<input type="button" data-role="none" id="leadBtnBookNow" leadSourceId="17" name="leadBtnBookNow" class="btn btn-full-width btn-orange" value="Get offers" />
					</div>
					<%} %>

					<%if (isPrimaryDealer)
					  { %>
					<div class="<%= !string.IsNullOrEmpty(maskingNum) ? "grid-5 omega padding-left5" : "" %>">
						<a id="calldealer" class="btn btn-full-width btn-green rightfloat" href="tel:<%= maskingNum %>">
							<span class="bwmsprite tel-white-icon margin-right5"></span>Call dealer
						</a>
					</div>
					<%} %>
				</div>
			</div>
			<div class="clear"></div>
		</div>

		<%if (isSecondaryDealer)
			{%>
		<div id="pq-secondary-dealer" class="bg-white padding-top15 padding-bottom15 bottom-shadow">
			<p class="font18 text-bold text-black padding-right20 padding-left20 margin-bottom10">Prices from <%= secondaryDealersCount == 1 ? secondaryDealersCount + " more dealer" : secondaryDealersCount + " more dealers" %></p>
			<div class="swiper-container">
				<div class="swiper-wrapper padding-top5 padding-bottom5">
					<%--bind secondary dealers--%>
					<asp:Repeater ID="rptSecondaryDealers" runat="server">
						<ItemTemplate>
							<div class="swiper-slide secondary-dealer-card">
								<a href="javascript:void(0)" dealerid="<%# DataBinder.Eval(Container.DataItem,"DealerId") %>">
									<div class="margin-bottom15">
										<span class="grid-9 alpha omega font14 text-default text-bold"><%# DataBinder.Eval(Container.DataItem,"Name") %></span>
										<span class="grid-3 omega text-light-grey text-right">5.4 kms</span>
										<div class="clear"></div>
										<span class="font12 text-light-grey"><%# DataBinder.Eval(Container.DataItem,"Area") %></span>
										<div class="margin-top15">
											<div class="grid-4 alpha omega border-solid-right">
												<p class="font12 text-light-grey margin-bottom5">On-road price</p>
												<span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="font16 text-default text-bold">1,02,887</span>
											</div>
											<div class="grid-8 padding-top10 padding-left20 omega">
												<span class="bwmsprite offers-sm-box-icon"></span>
												<span class="font14 text-default text-bold">2</span>
												<span class="font12 text-light-grey">Offers available</span>
											</div>
											<div class="clear"></div>
										</div>
									</div>
								</a>
								<div>
									<a href="javascript:void(0)" class="btn btn-white btn-sm-1 margin-right5 inline-block">Get offers from dealer</a>
									<a href="tel:9876543210" class="inline-block">
										<span class="bwmsprite tel-sm-icon"></span>
										<span class="font14 text-default text-bold">9876543210</span>
									</a>
								</div>
							</div>
						</ItemTemplate>
					</asp:Repeater>
				</div>
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


		<%if (dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium)
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
									<span id="downPaymentAmount" data-bind="text: formatPrice(Math.round(downPayment()))" class="text-bold"></span>
								</div>
							</div>
							<div id="downPaymentSlider"
								data-bind="slider: downPayment, sliderOptions: { min: minDnPay(), max: maxDnPay(), range: 'min', step: 1, value: Math.round(((maxDnPay() - minDnPay()) / 2) + minDnPay()) }"
								class="ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all">
								<div class="ui-slider-range ui-widget-header ui-corner-all ui-slider-range-min"></div>
								<span class="ui-slider-handle ui-state-default ui-corner-all" tabindex="0"></span>
							</div>
						</div>
						<div class="clear"></div>
					</div>

					<div class="emi-slider-box">
						<div class="emi-slider-box-left-section">
							<div class="clearfix font14">
								<p class="grid-8 alpha text-light-grey text-left">Loan amount</p>
								<div class="emi-slider-box-right-section grid-4 omega">
									<span class="bwmsprite inr-xxsm-icon"></span>
									<span id="loanAmount" data-bind="text: formatPrice(Math.round(loan()))" class="text-bold"></span>
								</div>
							</div>
							<div id="loanAmountSlider"
								data-bind="slider: loan, sliderOptions: { min: bikePrice() - maxDnPay(), max: bikePrice() - minDnPay(), range: 'min', step: 1 }"
								class="ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all">
								<div class="ui-slider-range ui-widget-header ui-corner-all ui-slider-range-min"></div>
								<span class="ui-slider-handle ui-state-default ui-corner-all" tabindex="0"></span>
							</div>
						</div>
						<div class="clear"></div>
					</div>

					<div class="emi-slider-box">
						<div class="emi-slider-box-left-section">
							<div class="clearfix font14">
								<p class="grid-8 alpha text-light-grey text-left">Tenure</p>
								<div class="emi-slider-box-right-section grid-4 omega text-bold">
									<span id="tenurePeriod" data-bind="text: tenure"></span>&nbsp;Months
								</div>
							</div>
							<div id="tenureSlider"
								data-bind="slider: tenure, sliderOptions: { min: minTenure(), max: maxTenure(), range: 'min', step: 1 }"
								class="ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all">
								<div class="ui-slider-range ui-widget-header ui-corner-all ui-slider-range-min"></div>
								<span class="ui-slider-handle ui-state-default ui-corner-all" tabindex="0"></span>
							</div>
						</div>
						<div class="clear"></div>
					</div>

					<div class="emi-slider-box">
						<div class="emi-slider-box-left-section">
							<div class="clearfix font14">
								<p class="grid-8 alpha text-light-grey text-left">Interest</p>
								<div class="emi-slider-box-right-section grid-4 omega text-bold">
									<span id="rateOfInterestPercentage" data-bind="text: rateofinterest">5</span>&nbsp;%
								</div>
							</div>
							<div id="rateOfInterestSlider"
								data-bind="slider: rateofinterest, sliderOptions: { min: minROI(), max: maxROI(), range: 'min', step: 0.25 }"
								class="ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all">
								<div class="ui-slider-range ui-widget-header ui-corner-all ui-slider-range-min"></div>
								<span class="ui-slider-handle ui-state-default ui-corner-all" tabindex="0"></span>
							</div>
						</div>
						<div class="clear"></div>
					</div>

				</div>
				<div class="finance-emi-right-box omega margin-top5 margin-bottom25 padding-right15 padding-left15">
					<div class="clearfix">
						<p class="grid-5 font16 text-left text-light-grey alpha position-rel pos-top2">Indicative EMI</p>
						<div class="indicative-emi-amount text-right grid-7 omega font18 text-bold">
							<span class="bwmsprite inr-sm-icon"></span>
							<span id="emiAmount" data-bind="text: monthlyEMI"></span>&nbsp;per month
						</div>
					</div>
				</div>
				<div class="clear"></div>
				<a id="btnEmiQuote" leadSourceId="18" class="btn btn-orange text-bold emi-quote-btn">Get EMI quote from dealer</a>
			</div>
		</div>
		<%} %>
		<%--<!-- Lead Capture pop up start  -->
		<div id="leadCapturePopup" class="bw-popup bwm-fullscreen-popup contact-details hide">
			<div class="popup-inner-container text-center">
				<div class="bwmsprite close-btn leadCapture-close-btn rightfloat"></div>
				<div id="contactDetailsPopup">
					<!-- Contact details Popup starts here -->
					<p class="font18 margin-top10 margin-bottom10">Provide contact details</p>
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
					<div class="icon-outer-container rounded-corner50percent">
						<div class="icon-inner-container rounded-corner50percent">
							<span class="bwmsprite thankyou-icon margin-top25"></span>
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
							<input type="button" class="btn btn-orange margin-top20" value="Send OTP" id="generateNewOTP" data-bind="event: { click: submitLead }" />
						</div>
					</div>

				</div>
				<!-- OTP Popup ends here -->
			</div>
		</div>
		<!-- Lead Capture pop up end  -->--%>

		<!-- #include file="/includes/footerBW_Mobile.aspx" -->
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


			var leadBtnBookNow = $("#leadBtnBookNow,#leadLink,#btnEmiQuote,#getMoreDetails"), leadCapturePopup = $("#leadCapturePopup");
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
			var customerViewModel = new CustomerModel();

			$(function () {
				leadBtnBookNow.on('click', function () {
					leadSourceId = $(this).attr("leadSourceId");
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
							"leadSourceId": leadSourceId,
							"deviceId": getCookie('BWC')
						}
						$.ajax({
							type: "POST",
							url: "/api/PQCustomerDetail/",
							data: ko.toJSON(objCust),
							beforeSend: function (xhr) {
								xhr.setRequestHeader('utma', getCookie('__utma'));
								xhr.setRequestHeader('utmz', getCookie('_bwutmz'));
							},
							async: false,
							contentType: "application/json",
							dataType: 'json',
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
							dataType: 'json',
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
							dataType: 'json',
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
							if (getOffersClicked) {
								dataLayer.push({ "event": "Bikewale_all", "cat": "Dealer_PQ", "act": "Lead_Submitted", "lab": "Main_Form_" + bikeName + "_" + versionName + "_" + getCityArea });
								getOffersClicked = false;
							}

							else if (getEMIClicked) {
								dataLayer.push({ "event": "Bikewale_all", "cat": "Dealer_PQ", "act": "Lead_Submitted", "lab": "Get_EMI_Quote_" + bikeName + "_" + versionName + "_" + getCityArea });
								getEMIClicked = false;
							}
							else if (getMoreDetailsClicked) {
								triggerGA('Dealer_PQ', 'Lead_Submitted', 'Get_more_details_' + GetBikeVerLoc());
								getMoreDetailsClicked = false;
							}
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
							if (getMoreDetailsClicked) {
								triggerGA('Dealer_PQ', 'Lead_Submitted', 'Get_more_details_' + GetBikeVerLoc());
								getMoreDetailsClicked = false;
							}
							$("#contactDetailsPopup").hide();
							$("#otpPopup").hide();
							$("#dealer-assist-msg").show();
						}
						else {
							$('#processing').hide();
							otpVal("Please enter a valid OTP.");
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

			//ko.applyBindings(customerViewModel, $('#leadCapturePopup')[0]);
			// GA Tags
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
