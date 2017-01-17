﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.ModelPricesInCity" EnableViewState="false" %>

<%@ Register Src="/controls/ModelPriceInNearestCities.ascx" TagPrefix="BW" TagName="ModelPriceInNearestCities" %>
<%@ Register Src="~/controls/NewAlternativeBikes.ascx" TagName="AlternativeBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/DealerCard.ascx" TagName="Dealers" TagPrefix="BW" %>
<%@ Register Src="~/controls/LeadCaptureControl.ascx" TagName="LeadCapture" TagPrefix="BW" %>
<%@ Register Src="~/controls/UsedBikes.ascx" TagName="UsedBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/ServiceCenterCard.ascx" TagName="ServiceCenterCard" TagPrefix="BW" %>
<%@ Import Namespace="Bikewale.Common" %>
<!doctype html>
<html>
<head>

	<%
		title = string.Format("{0} price in {1} - Check On Road Price & Dealer Info. - BikeWale", bikeName, cityName);
		if (firstVersion != null && !isDiscontinued)
			description = string.Format("{0} price in {1} - Rs. {2} (On road price). Get its detailed on road price in {1}. Check your nearest {0} Dealer in {1}", bikeName, cityName, firstVersion.OnRoadPrice);
		else if(firstVersion != null)
			description = string.Format("{0} price in {1} - Rs. {2} (Ex-Showroom). Get prices for all the versions of and check out the nearest {0} Dealer in {1}", bikeName, cityName, firstVersion.ExShowroomPrice);
		
		keywords = string.Format("{0} price in {1}, {0} on-road price, {0} bike, buy {0} bike in {1}, new {2} price", bikeName, cityName, modelName);
		canonical = string.Format("https://www.bikewale.com/{0}-bikes/{1}/price-in-{2}/", makeMaskingName, modelMaskingName, cityMaskingName);
		alternate = string.Format("https://www.bikewale.com/m/{0}-bikes/{1}/price-in-{2}/", makeMaskingName, modelMaskingName, cityMaskingName);
		ogImage = modelImage;
		AdId = "1442913773076";
		AdPath = "/1017752/Bikewale_NewBike_";
		isAd970x90Shown = true;
		isAd970x90BTFShown = false;
		isAd970x90BottomShown = true;
		isAd300x250Shown = false;
		isAd300x250BTFShown = false;
				   
	%>
	<!-- #include file="/includes/headscript.aspx" -->
	<link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/css/new/modelprice-in-city.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
</head>
<body class="bg-light-grey header-fixed-inner">
	<form runat="server">
		<!-- #include file="/includes/headBW.aspx" -->
		<section class="bg-light-grey padding-top10">
			<div class="container">
				<div class="grid-12">
					<div class="breadcrumb margin-bottom15">
						<!-- breadcrumb code starts here -->
						<ul>
							<li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb"><a href="/" itemprop="url">
								<span itemprop="title">Home</span></a>
							</li>
							<li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
								<span class="bwsprite fa-angle-right margin-right10"></span>
								<a href="/<%= makeMaskingName %>-bikes/" itemprop="url">
									<span itemprop="title"><%=makeName %> Bikes</span>
								</a></li>
							<li><span class="bwsprite fa-angle-right margin-right10"></span>
								<a href="/<%= makeMaskingName %>-bikes/<%= modelMaskingName %>/" itemprop="url">
									<span><%=makeName %> <%= modelName %></span>
								</a>
							</li>
							<li><span class="bwsprite fa-angle-right margin-right10"></span>
								<span>Price in <%=cityName %></span>
							</li>
						</ul>
						<div class="clear"></div>
					</div>
					<h1 class="font22 text-default margin-bottom20"><%=bikeName %> price in <%=cityName %></h1>
				</div>
				<div class="clear"></div>
			</div>
		</section>

		<section>
            <div id="versionPriceInCityWrapper" class="container margin-bottom25">
			    <div class="grid-12 font14">
				<div class="content-box-shadow">
					<p class="padding-top20 padding-right20 padding-bottom5 padding-left20 text-light-grey">
					   <%=pageDescription %></p>
					<div id='versions' class="model-versions-tabs-wrapper">
						<asp:Repeater ID="rpVersioNames" runat="server">
							<ItemTemplate>
								<a class="<%# (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "VersionId")) != versionId)?string.Empty:"active" %>" id="<%# DataBinder.Eval(Container.DataItem, "VersionId").ToString() %>" href="javascript:void(0)"><%# DataBinder.Eval(Container.DataItem, "VersionName").ToString() %></a>
							</ItemTemplate>
						</asp:Repeater>
					</div>
					<div class="border-divider"></div>

					<div id="modelVersionDetailsWrapper" class="text-light-grey padding-bottom20">
						<div class="grid-4 padding-top10">
							<div class="model-version-image-content">
								<%if(isDiscontinued) { %><span class="discontinued-text-label font16 position-abt text-center">Discontinued</span> <% } %>
								<img src="<%=modelImage %>" title="<%= title %>" alt="<%= title %>" />
							</div>
						</div>
						<div class="grid-4 padding-top15">
							<asp:Repeater ID="rprVersionPrices" runat="server">
								<ItemTemplate>
									<% if (!isDiscontinued)
									   { %>
									<div class="priceTable <%# (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "VersionId")) != versionId)?"hide":string.Empty %>" id="<%# DataBinder.Eval(Container.DataItem, "VersionId").ToString() %>">
										<table cellspacing="0" cellpadding="0" width="100%" border="0">
											<tr>
												<td width="200" class="padding-bottom15">Ex-showroom</td>
												<td align="right" class="padding-bottom15 text-default"><span class="bwsprite inr-sm"></span>
													<%# CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"ExShowroomPrice").ToString()) %>                                                     
												</td>
											</tr>
											<tr>
												<td class="padding-bottom15">RTO</td>
												<td align="right" class="padding-bottom15 text-default"><span class="bwsprite inr-sm"></span>
													<%#CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"RTO").ToString()) %>
												</td>
											</tr>
											<tr>
												<td class="padding-bottom15">Insurance</td>
												<td align="right" class="padding-bottom15 text-default"><span class="bwsprite inr-sm"></span>
													<%#CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"Insurance").ToString()) %>
												</td>
											</tr>
											<tr>
												<td class="text-bold text-default padding-top15 border-divider"><%= modelName %> On-road price in <%= cityName %></td>
												<td align="right" class="font16 text-bold text-default padding-top15 border-divider"><span class="bwsprite inr-lg"></span>
													<%#CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"OnRoadPrice").ToString()) %>
												</td>
											</tr>
										</table>
									</div>
									<%}
									   else
									   { %>
											<div class="priceTable <%# (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "VersionId")) != versionId)?"hide":string.Empty %>" id="<%# DataBinder.Eval(Container.DataItem, "VersionId").ToString() %>">
												<p class="text-x-grey margin-bottom15"><%=bikeName %> is now discontinued in India.</p>
												<div class="padding-bottom15 text-default text-bold">
													<span class="margin-right5">Last known Ex-showroom price</span>
													<span class="bwsprite inr-sm"></span>
													<%# CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"ExShowroomPrice").ToString()) %>                                                     
												</div> 
											</div>
									<% } %>
								</ItemTemplate>
							</asp:Repeater>
						</div>

						<div class="grid-4 padding-top15 padding-left15">
                            <% if(isAreaSelected) { %>
                            <p class="text-black">Get in touch with dealer for:</p>
							<ul class="selectAreaToGetList margin-bottom20">
								<li class="bullet-point">
									<p>Best offers</p>
								</li>
								<li class="bullet-point">
									<p>Test rides</p>
								</li>
								<li class="bullet-point">
									<p>Complete buying assistance</p>
								</li>
							</ul>
                            <a id="dealerDetails" href="javascript:void(0)" data-pqsourceid="<%= (int) Bikewale.Entities.PriceQuote.PQSourceEnum.Desktop_PriceInCity_Dealer_Detail_Click %>" f="getBikeVersionName" c="Price_in_City_Page" a="View_dealer_details_Clicked" data-preselcity="<%=cityId %>"  data-modelid="<%=modelId %>" class="btn btn-orange btn-xxlg font14 bw-ga" rel="nofollow">View dealer details</a>
                            <% }
                               else if (isAreaAvailable && !isDiscontinued)
							   { %>
							<p class="text-black">Please select your area to get:</p>
							<ul class="selectAreaToGetList margin-bottom20">
								<li class="bullet-point">
									<p>Nearest dealership details</p>
								</li>
								<li class="bullet-point">
									<p>Exclusive offers</p>
								</li>
								<li class="bullet-point">
									<p>Complete buying assistance</p>
								</li>
							</ul>
							<a href="javascript:void(0)" data-pqsourceid="<%= (int) Bikewale.Entities.PriceQuote.PQSourceEnum.Desktop_PriceInCity_SelectAreas %>" f="getBikeVersionName" c="Price_in_City_Page" a="Select_Area_Clicked" data-preselcity="<%=cityId %>" data-persistent="true" data-modelid="<%=modelId %>" class="btn btn-orange btn-xxlg font14 getquotation changeCity bw-ga" rel="nofollow">Select your area</a>
							<%}
							   else
							   { %>
							<script type='text/javascript' src='https://www.googletagservices.com/tag/js/gpt.js'>
							  googletag.pubads().definePassback('/1017752/Bikewale_PQ_300x250', [300, 250]).display();
							</script>
							<% } %>
						</div>
						<div class="clear"></div>
					</div>

					<div class="margin-left10 margin-right10 border-solid-bottom"></div>

					<BW:Dealers ID="ctrlDealers" runat="server" />

                     <% if(ctrlServiceCenterCard.showWidget){ %>
                    <BW:ServiceCenterCard runat="server" ID="ctrlServiceCenterCard" />
                  <% } %>

					<%if (ctrlTopCityPrices.showWidget) {%>
					<BW:ModelPriceInNearestCities ID="ctrlTopCityPrices" runat="server" />
					<%} %>
				</div>
			    </div>
			    <div class="clear"></div>
            </div>
		</section>

		<section>
            <div class="container margin-bottom20">
			<div class="grid-12">
				<div class="content-box-shadow">
                     
					<% if (ctrlAlternativeBikes.FetchedRecordsCount > 0)
					   { %>
					<!-- Alternative reviews ends -->
					<BW:AlternativeBikes ID="ctrlAlternativeBikes" runat="server" />
            		<!-- Alternative reviews ends -->
					<% } %>
                            <%if(bikeRankObj!=null){ %>
                    <%if(bikeRankObj.Rank>0){ %>
                      <div class="margin-left20 margin-right20 padding-bottom20 <%= (ctrlAlternativeBikes.FetchedRecordsCount > 0)?string.Empty:"padding-top20"%>">
                        <div class="content-inner-block-15 border-solid font14">
                            <div class="grid-9 alpha">
                                <div class="inline-block">
                                    <span class="item-rank">#<%=bikeRankObj.Rank%></span>
                                </div>
                                <p class="inline-block checkout-list-slug-label"><%=modelName%> is the <%=bikeRankObj.Rank>1?rankText:"" %> most popular <%=bikeType.ToLower() %>. Check out other <%=styleName.ToLower() %> which made it to Top 10 list.</p>
                            </div>
                            <div class="grid-3 text-right position-rel pos-top5">
                                <a href="<%=Bikewale.Utility.UrlFormatter.FormatGenericPageUrl(bikeRankObj.BodyStyle) %>" title="Best <%=styleName %> in India">Check out the list now<span class="bwsprite blue-right-arrow-icon"></span></a>
                            </div>
                            <div class="clear"></div>
                        </div>
                    </div>
                    <%} %>
                    <%else{ %>
                    <div class="margin-left20 margin-right20 padding-bottom20 <%= (ctrlAlternativeBikes.FetchedRecordsCount > 0)?string.Empty:"padding-top20"%>">
                        <div class="content-inner-block-15 border-solid font14">
                            <div class="grid-9 alpha">
                                <div class="inline-block icon-red-bg">
                                    <span class="bwsprite rank-graph"></span>
                                </div>
                                <p class="inline-block checkout-list-slug-label">Not sure what to buy? List of Top 10 <%=styleName.ToLower() %> can come in handy.</p>
                            </div>
                            <div class="grid-3 text-right position-rel pos-top5">
                                <a href="<%=Bikewale.Utility.UrlFormatter.FormatGenericPageUrl(bikeRankObj.BodyStyle) %>" title="Best <%=styleName %> in India">Check out the list now<span class="bwsprite blue-right-arrow-icon"></span></a>
                            </div>
                            <div class="clear"></div>
                        </div>
                    </div>
                    <%} %>
                    <%} %>
                    <div id="makeTabsContentWrapper" class="margin-bottom20">
				        <div class="content-box-shadow">
					        <div id="makeOverallTabsWrapper">
						        <div id="makeOverallTabs" class="overall-floating-tabs">
			                    <% if (ctrlRecentUsedBikes.FetchedRecordsCount > 0)
					                { %>
                                    <div class="border-solid-top margin-right10 margin-left10"></div>
					                <BW:UsedBikes runat="server" ID="ctrlRecentUsedBikes" />
					            <%} %>
						        </div>
			                </div>
					    </div>
				    </div>
                </div>
			    <div class="clear"></div>
            </div>
		</section>

		<BW:LeadCapture ID="ctrlLeadCapture" runat="server" />
		 <section>
			<div class="container margin-top10 margin-bottom30">
				<div class="grid-12 font12">
					<span class="font14"><strong>Disclaimer</strong>:</span><p> BikeWale takes utmost care in gathering precise and accurate information about <%=makeName %> <%=modelName %> price in <%=cityName %>.  However, this information is only indicative and may not reflect the final price you may pay. For more information please read <a target="_blank" href="/TermsConditions.aspx">Terms & Condition</a> ,<a target="_blank" href="/visitoragreement.aspx">Visitor Agreement</a> and  <a target="_blank" href="/privacypolicy.aspx">Privacy Policy</a>. </p>
                    </div>
				<div class="clear"></div>
			</div>
		</section>

		<!-- #include file="/includes/footerBW.aspx" -->
		<!-- #include file="/includes/footerscript.aspx" -->
		
		<script type="text/javascript">
			var modelId = <%= modelId %>;
			var clientIP = "<%= clientIP%>";
			var pageUrl = window.location.href; 
			var bikeName='<%=bikeName%>';
			var CityArea=GetGlobalCityArea()||"";
			ga_pg_id=16;
				$("#btnDealerPricePopup").click(function () {
					var selArea = '';
					if ($('#ddlAreaPopup option:selected').index() > 0) {
						selArea = '_' + $('#ddlAreaPopup option:selected').text();
					}
					triggerGA('Price_in_City_Page', 'Show_On_Road_Price_Clicked', "<%= string.Format("{0}_", bikeName)%>"+ $('#versions .active').text() + '_' + $('#ddlCitiesPopup option:selected').text() + selArea);
			
				});
			$(".leadcapturebtn").click(function(e){
				ele = $(this);
				var leadOptions = {
					"dealerid" : ele.attr('data-item-id'),
					"dealername" : ele.attr('data-item-name'),
					"dealerarea"  : ele.attr('data-item-area'),
					"versionid" : $("#versions a.active").attr("id") ,
					"leadsourceid" : ele.attr('data-leadsourceid'),
					"pqsourceid" : ele.attr('data-pqsourceid'),
					"pageurl" : pageUrl,
					"clientip" : clientIP,
					"isregisterpq" : true,
					"gaobject" : {
						cat : 'Price_in_City_Page',
						act: 'Lead_Submitted',
						lab: '<%= string.Format("{0}_", bikeName)%>'+ CityArea
					}
				};
				dleadvm.setOptions(leadOptions);
			});
		    $("#dealerDetails").click(function(e){
		       ele = $(this);
		       vmquotation.CheckCookies();
		       vmquotation.IsLoading(true);
		       $('#priceQuoteWidget,#popupContent,.blackOut-window').show();
		        var options = {
		            "modelId": "<%= modelId %>",
		            "cityId": onCookieObj.PQCitySelectedId,
		            "areaId": onCookieObj.PQAreaSelectedId,
		            "city": (onCookieObj.PQCitySelectedId > 0) ? { 'id': onCookieObj.PQCitySelectedId, 'name': onCookieObj.PQCitySelectedName } : null,
		            "area": (onCookieObj.PQAreaSelectedId > 0) ? { 'id': onCookieObj.PQAreaSelectedId, 'name': onCookieObj.PQAreaSelectedName } : null,
		        };
		        vmquotation.setOptions(options);
		    });
			$('.model-versions-tabs-wrapper a').on('click', function () {
				var verid = $(this).attr('id');
				showTab(verid);
			});
			function showTab(version) {
				$('.model-versions-tabs-wrapper a').removeClass('active');
				$('.model-versions-tabs-wrapper a[id="' + version + '"]').addClass('active');
				$('.priceTable').hide();
				$('.priceTable[id="' + version + '"]').show();
			}  
			function getBikeVersionName()
			{
				var bikeVersion=$('#versions .active').text();
				var bikeNameVersion='<%=bikeName%>'+'_'+ bikeVersion;
				return bikeNameVersion;
			}
		</script>
	</form>
</body>
</html>
