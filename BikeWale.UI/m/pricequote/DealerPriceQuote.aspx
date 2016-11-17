﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.BikeBooking.DealerPriceQuote" Trace="false" Async="true" %>

<%@ Register Src="~/m/controls/LeadCaptureControl.ascx" TagPrefix="BW" TagName="LeadCapture" %>
<%@ Register Src="~/m/controls/NewAlternativeBikes.ascx" TagPrefix="BW" TagName="AlternateBikes" %>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Import Namespace="Bikewale.BikeBooking" %>
<%@ Import Namespace="System.Linq" %>
<!doctype html>
<html>
<head>
	<%
		title = String.Format("{0} {1} {2} Price Quote", objPriceQuote.objMake.MakeName, objPriceQuote.objModel.ModelName, objPriceQuote.objVersion.VersionName);
		description = String.Format("{0} {1} {2} price quote", objPriceQuote.objMake.MakeName, objPriceQuote.objModel.ModelName, objPriceQuote.objVersion.VersionName);
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
		var pageUrl = window.location.href;
		var dealerName = '<%= dealerName%>';
		var campaignId = "<%= objExQuotation != null ? objExQuotation.CampaignId : 0 %>";
		var manufacturerId = "<%= objExQuotation != null ? objExQuotation.ManufacturerId : 0 %>";
		$(document).ready( function(){
			if (dealerName != "") {
				$("#header-dealername").text(dealerName);
			}
			else {
				$("#headerText").removeClass("font12").addClass("font20 margin-top5");
			}
		});

	</script>  
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
				<div class="dropdown-select-wrapper margin-left5">
					<asp:DropDownList class="dropdown-select" id="ddlVersion" runat="server" AutoPostBack="true"></asp:DropDownList>                    
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
					<%= (!string.IsNullOrEmpty(currentArea) ? string.Format("{0}, {1}",currentArea.Replace('-', ' '),currentCity.Replace('-', ' ')) : currentCity.Replace('-', ' ')) %>
					<a href="javascript:void(0)" rel="nofollow" data-pqSourceId="<%= (int)Bikewale.Entities.PriceQuote.PQSourceEnum.Mobile_DPQ_Quotation %>" data-modelId="<%= objPriceQuote.objModel.ModelId %>" class="getquotation" data-persistent="true"><span class="bwmsprite loc-change-blue-icon"></span></a>
				</p>

			</div>
			<script type="text/javascript">                
				var dropdown = {
					setDropdown: function () {
						var selectDropdown = $('.dropdown-select');

						selectDropdown.each(function () {
							dropdown.setMenu($(this));
						});
					},

					setMenu: function (element) {
						$('<div class="dropdown-menu"></div>').insertAfter(element);
						dropdown.setStructure(element);
					},

					setStructure: function (element) {
						var elementText = element.find('option:selected').text(),
							menu = element.next('.dropdown-menu');

						menu.append('<p class="dropdown-label">' + elementText + '</p><div class="dropdown-list-wrapper"><p class="dropdown-selected-item">' + elementText + '</p><ul class="dropdown-menu-list dropdown-with-select"></ul></div>');

						dropdown.setOption(element);
					},

					setOption: function (element) {
						var selectedIndex = element.find('option:selected').index(),
							menu = element.next('.dropdown-menu'),
							menuList = menu.find('ul'),
							i;

						element.find('option').each(function (index) {
							if (selectedIndex == index) {
								menuList.append('<li class="active" data-option-value="' + $(this).val() + '">' + $(this).text() + '</li>');
							}
							else {
								menuList.append('<li data-option-value="' + $(this).val() + '">' + $(this).text() + '</li>');
							}
						});
					},

					active: function (label) {
						$('.dropdown-select-wrapper').find('.dropdown-menu').removeClass('dropdown-active');
						label.closest('.dropdown-menu').addClass('dropdown-active');
					},

					inactive: function () {
						$('.dropdown-select-wrapper').find('.dropdown-menu').removeClass('dropdown-active');
					},

					selectItem: function (element) {
						var elementText = element.text(),
							menu = element.closest('.dropdown-menu'),
							dropdownLabel = menu.find('.dropdown-label'),
							selectedItem = menu.find('.dropdown-selected-item');

						element.siblings('li').removeClass('active');
						element.addClass('active');
						selectedItem.text(elementText);
						dropdownLabel.text(elementText);
					},

					selectOption: function (element) {
						var elementValue = element.attr('data-option-value'),
							wrapper = element.closest('.dropdown-select-wrapper'),
							selectDropdown = wrapper.find('.dropdown-select');

						selectDropdown.val(elementValue).trigger('change');

					}
				}
				dropdown.setDropdown();
			</script>
			<!--Price Breakup starts here-->
			<div class="padding-left15 padding-right15">
				<%if (isPriceAvailable)
				  { %>
				<div class="break-line margin-bottom15"></div>
				<table width="100%" border="0" cellspacing="0" cellpadding="0" class="pqTable font14">
					<asp:Repeater ID="rptPriceList" runat="server">
						<ItemTemplate>
							<tr>
								<td align="left" width="65%" class="padding-bottom15"><%# DataBinder.Eval(Container.DataItem,"CategoryName") %></td>
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
							<a id='getMoreDetails' leadsourceid="23" class="get-offer-link bw-ga leadcapturebtn" data-item-registerpq="false" data-leadsourceid="23" data-item-id="<%= dealerId %>" data-item-name="<%= dealerName %>" data-item-area="<%= dealerArea %>" data-pqsourceid="<%= Convert.ToUInt16(Bikewale.Entities.PriceQuote.PQSourceEnum.Mobile_DPQ_Quotation) %>" c="Dealer_PQ" a="Get_more_details_below_price_clicked" f="GetBikeVerLoc">Get more details</a>
						</td>
					</tr>
					<%
						}
					%>
				</table>
				<%}
				  else if (objExQuotation != null && objExQuotation.ExShowroomPrice > 0)
				  {%>
				<table width="100%" border="0" cellspacing="0" cellpadding="0" class="pqTable font14">
					<tr>
						<td class="text-light-grey padding-bottom15" width="65%" align="left">Ex-Showroom Price</td>
						<td class="padding-bottom15" width="35%" align="right"><span class="bwmsprite inr-xxsm-icon"></span><%= CommonOpn.FormatPrice(objExQuotation.ExShowroomPrice.ToString()) %></td>
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

			 <% if (objExQuotation != null && objExQuotation.CampaignId > 0 && objPriceQuote != null && dealerId==0 && objPriceQuote.SecondaryDealerCount == 0 && !string.IsNullOrEmpty(objExQuotation.ManufacturerAd))
				{ %>
				<section>
					<%=String.Format(objExQuotation.ManufacturerAd) %>
				</section>
			<%} %>
			
		<style type="text/css">
			#campaign-container .tel-sm-icon{top:0}#campaign-offer-list li{width:50%;display:inline-block;vertical-align:middle;margin-bottom:20px}#campaign-offer-list li span{display:inline-block;vertical-align:middle}.campaign-offer-label{width:80%;font-size:13px;font-weight:bold;padding-right:5px}#campaign-button-container .btn{padding-right:0;padding-left:0}#campaign-button-container .grid-6.hide + .grid-6{width:100%;padding-right:0}.campaign-offer-1,.campaign-offer-2,.campaign-offer-3,.campaign-offer-4{width:22px;height:22px;margin-right:5px}.campaign-offer-1{background-position:0 -387px}.campaign-offer-2{background-position:0 -418px}.campaign-offer-3{background-position:-28px -387px}.campaign-offer-4{background-position:-56px -387px}
		</style>

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
					<p><span class="bwmsprite inr-md-icon"></span>&nbsp;<span class="font22 text-bold"><%= Bikewale.Utility.Format.FormatPrice(!(totalPrice.ToString() == "" || totalPrice.ToString() == "0") ? totalPrice.ToString() : (objExQuotation != null ? objExQuotation.OnRoadPrice.ToString() : "")) %></span></p>
					<%if (dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium || dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Deluxe)
					  { %>
					<p class="text-light-grey margin-top5">EMI&nbsp;<span class="bwmsprite inr-xxsm-icon"></span><span id="spnEMIAmount" class="text-default"></span>&nbsp;onwards.&nbsp;<a href="javascript:void(0)" class="calculate-emi-target">Calculate Now</a></p>
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
									<span class="offers-sprite offerIcon_<%# DataBinder.Eval(Container.DataItem,"OfferCategoryId") %>"></span>
									<span class="pq-benefits-title padding-left10"><%# DataBinder.Eval(Container.DataItem,"OfferText") %><span class="margin-left10 tnc font9 <%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsOfferTerms"))? string.Empty: "hide" %>" id="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "OfferId")) %>">View terms</span></span>
									
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
					</div>
					<p class="booknow-label font11 line-height-1-5 text-xx-light padding-left10 vertical-top">
						Pay <span class="bwmsprite inr-grey-xxxsm-icon"></span><%=Bikewale.Utility.Format.FormatPrice((objPriceQuote.PrimaryDealer.BookingAmount).ToString()) %> to book online and<br />
						balance amount of <span class="bwmsprite inr-grey-xxxsm-icon"></span><%=Bikewale.Utility.Format.FormatPrice((totalPrice - objPriceQuote.PrimaryDealer.BookingAmount).ToString()) %> at dealership
					</p>
				</div>
				<%} %>               
				<div class="border-solid margin-right5 margin-left5 margin-bottom15">

					<%if (dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium)
					  { %>
					<script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDY0kkJiTPVd2U7aTOAwhc9ySH6oHxOIYM&callback=initializeDealerMap" async defer></script>
					<div id="dealerMap" style="height: 94px; position: relative; text-align: center">
						<img src="http://imgd3.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif" />
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
						
					</script>
					<%} %>

					<div class="padding-15-20">
						
						<p class="margin-bottom10">
							<span class="bwmsprite dealership-loc-icon inline-block margin-right15"></span>
							<span class="inline-block dealership-address"><%= dealerAdd %></span>
						</p>
						
						<%if (!string.IsNullOrEmpty(maskingNum))
						  { %>
						<p class="margin-bottom10">
							<span class="bwmsprite tel-grey-sm-icon inline-block margin-right15"></span>
							<a id="aDealerNumber" href="tel:<%= maskingNum %>" class="inline-block text-default"><%= maskingNum %></a>
						</p>
						<%} %>
						<%if (!string.IsNullOrEmpty(contactHours))
						{ %>
						<div>
							<span class="bwmsprite clock-icon vertical-top margin-right15"></span>
							<span class="inline-block"><%= contactHours %></span>
						</div>
						<%} %>
					</div>

				</div>

				<%if (isUSPAvailable && (dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium))
				  { %>
				<div class="padding-right15 padding-left15">
					<p class="text-bold margin-bottom15">Benefits from this dealer:</p>
					<ul class="pricequote-benefits-list">
						<asp:Repeater ID="rptBenefits" runat="server">
							<ItemTemplate>
								<li>
									<span class="offers-sprite benifitIcon_<%# DataBinder.Eval(Container.DataItem,"CatId") %>"></span>
									<span class="pq-benefits-title padding-left15"><%# DataBinder.Eval(Container.DataItem,"BenefitText") %></span>
								</li>
							</ItemTemplate>
						</asp:Repeater>
					</ul>
				</div>
				<%} %>               
			</div>
			<%} %>
			<!-- Dealer Widget ends here -->

			<!--Dealer Campaign starts here -->
		
			<!--Dealer Campaign ends here -->
		  <%if(isPrimaryDealer){ %>
			<div id="pricequote-floating-button-wrapper" class="grid-12 alpha omega">
				<div class="float-button float-fixed">                  
					<div class="grid-<%= !String.IsNullOrEmpty(maskingNum) ? "6" : "12" %> alpha omega padding-right5">
						<input type="button" data-role="none" id="leadBtnBookNow" data-pqsourceid="<%= Convert.ToUInt16(Bikewale.Entities.PriceQuote.PQSourceEnum.Mobile_DPQ_Quotation) %>" data-leadsourceid="17" leadsourceid="17" data-item-registerpq="false" data-item-id="<%= dealerId %>" data-item-name="<%= dealerName %>" data-item-area="<%= dealerArea %>" name="leadBtnBookNow" class="btn btn-full-width btn-orange leadcapturebtn" value="Get offers" />
					</div>                  
					<%if (!String.IsNullOrEmpty(maskingNum))
					  { %>
					<div class="<%= !string.IsNullOrEmpty(maskingNum) ? "grid-6 omega padding-left5" : "" %>">
						<a id="calldealer" class="btn btn-full-width btn-green rightfloat" href="tel:<%= maskingNum %>">
							<span class="bwmsprite tel-white-icon margin-right5"></span>Call dealer
						</a>
					</div>
					<%} %>
				</div>
			</div>
			<%} %>
		
			<div class="clear"></div>
		</div>

		<%if (isSecondaryDealer)
		  {%>
		<div id="pq-secondary-dealer" class="bg-white padding-top15 padding-bottom15 bottom-shadow">
			<p class="font18 text-bold text-black padding-right20 padding-left20 margin-bottom10">Prices from <%= secondaryDealersCount == 1 ? secondaryDealersCount + " more dealer" : secondaryDealersCount + " more dealers" %></p>
			<div class="swiper-container pq-secondary-dealer-swiper">
				<div class="swiper-wrapper padding-top5 padding-bottom5">
					<%--bind secondary dealers--%>
					<asp:Repeater ID="rptSecondaryDealers" runat="server">
						<ItemTemplate>
							<div class="swiper-slide secondary-dealer-card">
								<a href="javascript:void(0)" class="secondary-dealer bw-ga text-default" c="Dealer_PQ" a="Secondary_Dealer_Card_Clicked" l="<%= BikeName + "_" + currentCity + "_" +currentArea %>" dealerid="<%# DataBinder.Eval(Container.DataItem,"DealerId") %>">
									<div class="margin-bottom15">
										<span class="grid-9 alpha omega font14 text-default text-bold"><%# DataBinder.Eval(Container.DataItem,"Name") %></span>
										<span class="grid-3 omega text-light-grey text-right"><%# String.Format("{0:0.0}",DataBinder.Eval(Container.DataItem,"Distance")) %> kms</span>
										<div class="clear"></div>
										<span class="font12 text-light-grey"><%# DataBinder.Eval(Container.DataItem,"Area") %></span>
										<div class="margin-top15">
											<div class="grid-4 alpha omega <%# (Convert.ToUInt32(DataBinder.Eval(Container.DataItem,"OfferCount")) > 0 && (Convert.ToString(DataBinder.Eval(Container.DataItem,"DealerPackageType")) != Convert.ToString(Bikewale.Entities.PriceQuote.DealerPackageTypes.Standard))) ?  "border-solid-right" : "" %>">
												<p class="font12 text-light-grey margin-bottom5">On-road price</p>                                                
												<span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="font16 text-default text-bold"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"SelectedVersionPrice").ToString()) %></span>
											</div>
											<%# (Convert.ToUInt32(DataBinder.Eval(Container.DataItem,"OfferCount")) > 0 && (Convert.ToString(DataBinder.Eval(Container.DataItem,"DealerPackageType")) != Convert.ToString(Bikewale.Entities.PriceQuote.DealerPackageTypes.Standard))) ? 
											"<div class=\"grid-8 padding-top10 padding-left20 omega\"><span class=\"bwmsprite offers-sm-box-icon\"></span><span class=\"font14 text-default text-bold\">" + DataBinder.Eval(Container.DataItem,"OfferCount") + "</span><span class=\"font12 text-light-grey\"> Offer" + (Convert.ToUInt32(DataBinder.Eval(Container.DataItem,"OfferCount")) > 1 ? "s" : "") + " available</span></div>" : "" %>
											<div class="clear"></div>
										</div>
									</div>
								</a>
								<div>
									<a href="javascript:void(0)" data-pqsourceid="<%= Convert.ToUInt16(Bikewale.Entities.PriceQuote.PQSourceEnum.Mobile_DPQ_Quotation) %>" data-leadsourceid="17" leadsourceid="17" data-item-registerpq="true" data-item-id="<%# DataBinder.Eval(Container.DataItem,"DealerId")  %>" data-item-name="<%# DataBinder.Eval(Container.DataItem,"Name") %>" data-item-area="<%# DataBinder.Eval(Container.DataItem,"Area") %>" class="btn btn-white btn-sm-1 margin-right5 inline-block leadcapturebtn bw-ga" c="Dealer_PQ" a="Get_Offers_Clicked" l="Secondary Dealer List_<%=BikeName %>_<%= currentCity %>_<%= currentArea%>" data-ga-cat="Dealer_PQ" data-ga-act="Lead_Submitted" data-ga-lab="Secondary Dealer List_<%=BikeName %>_<%= currentCity %>_<%= currentArea%>">Get offers from dealer</a>
									<%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString()) ? 
									("<a href='tel:" + DataBinder.Eval(Container.DataItem,"MaskingNumber") + "' class=\"inline-block bw-ga\" c=\"Dealer_PQ\" a=\"Dealer_Number_Clicked\" l=\"Secondary Dealer List_" + BikeName + "_" + Bikewale.Utility.GlobalCityArea.GetGlobalCityArea().City + "_" +Bikewale.Utility.GlobalCityArea.GetGlobalCityArea().Area + "\"><span class=\"bwmsprite tel-sm-icon\"></span><span class=\"font14 text-default text-bold\">" + DataBinder.Eval(Container.DataItem,"MaskingNumber") + "</span></a>") : "" %>
								</div>
							</div>
						</ItemTemplate>
					</asp:Repeater>
				</div>
			</div>
		</div>
		<%} %>

		<section class="<%= (ctrlAlternateBikes.FetchedRecordsCount > 0) ? "" : "hide" %>">
			<div class="bg-white bottom-shadow margin-top20 margin-bottom20 padding-bottom10">
				<BW:AlternateBikes ID="ctrlAlternateBikes" runat="server" />
			</div>
		</section>

		<%if (dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium || dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Deluxe)
		  { %>
		<div id="emiPopup" data-bind="visible: true" style="display: none" class="bwm-fullscreen-popup text-center padding-top30">
			<div class="emi-popup-close-btn position-abt pos-top10 pos-right10 bwmsprite cross-lg-lgt-grey cur-pointer"></div>
			<div class="icon-outer-container rounded-corner50percent">
				<div class="icon-inner-container rounded-corner50percent">
					<span class="offers-sprite cal-emi-icon margin-top15"></span>
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
				<a id="btnEmiQuote" leadsourceid="18" class="btn btn-orange text-bold emi-quote-btn leadcapturebtn"  data-pqsourceid="<%= Convert.ToUInt16(Bikewale.Entities.PriceQuote.PQSourceEnum.Mobile_DPQ_Quotation) %>" data-leadsourceid="18" data-item-registerpq="false" data-item-id="<%= dealerId %>" data-item-name="<%= dealerName %>" data-item-area="<%= dealerArea %>" data-ga-cat="Dealer_PQ" data-ga-act="Lead_Submitted" data-ga-lab="EMI_Calculator_<%=BikeName %>_<%= currentCity %>_<%= currentArea%>">Get EMI quote from dealer</a>
			</div>
		</div>
		<%} %>
		<!-- Lead Capture pop up start  -->               
		<BW:LeadCapture ID="ctrlLeadCapture" runat="server" />
		 <!-- Lead Capture pop up end  -->
		<!-- Terms and condition Popup start -->
		<div class="termsPopUpContainer content-inner-block-20 hide" id="termsPopUpContainer">
			<div class="fixed-close-btn-wrapper">
				<div id="termsPopUpCloseBtn" class="termsPopUpCloseBtn bwmsprite fixed-close-btn cross-lg-lgt-grey cur-pointer"></div>
			</div>
			<h3>Terms and Conditions</h3>
			<div class="hide" style="vertical-align: middle; text-align: center;" id="termspinner">
				<img src="http://imgd2.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif" />
			</div>
			<div id="terms" class="breakup-text-container padding-bottom10 font14">
			</div>
			<div id='orig-terms' class="hide">
			</div>
		</div>
		<!-- Terms and condition Popup end -->
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

			$(".leadcapturebtn").click(function (e) {
				ele = $(this);
				var leadOptions = {
					"dealerid": ele.attr('data-item-id'),
					"dealername": ele.attr('data-item-name'),
					"dealerarea": ele.attr('data-item-area'),
					"versionid": '<%= versionId %>',
					"leadsourceid": ele.attr('data-leadsourceid'),
					"pqsourceid": ele.attr('data-pqsourceid'),
					"pageurl": pageUrl,
					"clientip": clientIP,
					"isregisterpq": ele.attr('data-item-registerpq') == "true" ? true : false,
					"mfgCampid": ele.attr('data-mfgcampid'),
					"pqid": pqId,
					"gaobject" : {
						cat : ele.attr('data-ga-cat'),
						act: ele.attr('data-ga-act'),
						lab: ele.attr('data-ga-lab')
								}
				};

				dleadvm.setOptions(leadOptions);

			});           

			$('#getDealerDetails,#btnBookBike').click(function () {
				var cookieValue = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + versionId + "&DealerId=" + dealerId;
				window.location.href = '/m/pricequote/bookingsummary_new.aspx?MPQ=' + Base64.encode(cookieValue);
			});

			// GA Tags
			$("#leadBtnBookNow").on("click", function () {
				leadSourceId = $(this).attr("leadSourceId");
				dataLayer.push({ "event": "Bikewale_all", "cat": "Dealer_PQ", "act": "Get_More_Details_Clicked_Button", "lab": bikeName + "_" + getCityArea });
			});
			$("#leadLink").on("click", function () {
				dataLayer.push({ "event": "Bikewale_all", "cat": "Dealer_PQ", "act": "Get_More_Details_Clicked_Link", "lab": bikeName + "_" + getCityArea });
			});
			$(document).ready(function () {
			    <%if (objPriceQuote.SecondaryDealers != null && objPriceQuote.SecondaryDealers.Count() > 0){%>
                <%foreach(var dealer in objPriceQuote.SecondaryDealers){%>
			    triggerGA('Dealer_PQ', ' Secondary_Dealer_Card_Shown', '<%= string.Format("{0}_{1}_{2}_{3}", objPriceQuote.objMake.MakeName,objPriceQuote.objModel.ModelName,currentCity,dealer.Area)%>');
			    <%}%>
                <%}%>
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
						url: "/api/Terms/?offerId=" + offerId,
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
					$("#terms").load("/statichtml/tnc.html");
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
					'pQLeadId': eval("<%= Convert.ToInt16(Bikewale.Entities.PriceQuote.PQSourceEnum.Mobile_DPQ_Quotation) %>"),
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


			<% if (dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium || dealerType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Deluxe)
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
						var calculatedEMI = $.calculateEMI(self.loan(), self.tenure(), self.rateofinterest(), self.processingFees());                        
						if (calculatedEMI != "0")
							$("#spnEMIAmount").text(calculatedEMI);                            
						else {
							$("#spnEMIAmount").parent().addClass("hide");
						}
						return calculatedEMI;
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
				if (isNaN(num)) {
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
