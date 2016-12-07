<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.NewBikeDealerList" EnableViewState="false" %>
<%@ Register Src="~/m/controls/UsedBikes.ascx" TagName="MostRecentusedBikes" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/MMostPopularBikes.ascx" TagName="PopularBikeMake" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/LeadCaptureControl.ascx" TagName="LeadCapture" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/ServiceCenterCard.ascx" TagName="ServiceCenterCard" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/BrandCityPopUp.ascx" TagName="BrandCity" TagPrefix="BW" %>
<!DOCTYPE html>
<html>
<head>

    <% 
        keywords = String.Format("{0} showroom {1}, {0} dealers {1}, {1} bike showroom, {1} bike dealers,{1} dealers, {1} bike showroom, bike dealers, bike showroom, dealerships", makeName, cityName);
        description = String.Format("There are {2} {0} dealer showrooms in {1}. Get in touch with {0} showroom for prices, availability, test rides, EMI options and more!", makeName, cityName,totalDealers);
        title = String.Format("{0} Showrooms in {1} | {2} {0} Bike Dealers  - BikeWale", makeName, cityName,totalDealers);
        canonical = String.Format("http://www.bikewale.com/{0}-dealer-showrooms-in-{1}/", makeMaskingName, cityMaskingName);
        Ad_320x50 = true;
        Ad_Bot_320x50 = true;
        
    %>

    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <style type="text/css">
        @charset "utf-8";.swiper-card,.swiper-slide:first-child{margin-left:5px}#dealersList a:hover,.swiper-card a:hover{text-decoration:none}.padding-15-20{padding:15px 20px}#dealersList{padding:0 20px 20px}#dealersList li{border-top:1px solid #e2e2e2;padding-top:15px;margin-top:20px;font-size:14px}#dealersList li:first-child{border-top:0;margin-top:0}#dealersList a{display:block}.featured-tag{width:74px;text-align:center;background:#3799a7;margin-bottom:5px;z-index:1;font-size:12px;color:#fff;line-height:20px;-webkit-border-radius:2px;-moz-border-radius:2px;-o-border-radius:2px;border-radius:2px}.vertical-top{display:inline-block;vertical-align:top}.dealership-details{width:92%}.leadcapturebtn.btn{font-size:14px;padding:9px 21px}.dealership-loc-icon{width:10px;height:14px;background-position:-40px -436px;position:relative;top:4px;margin-right:3px}.star-white{width:8px;height:8px;background-position:-174px -447px;margin-right:4px}.tel-sm-grey-icon{width:10px;height:10px;background-position:0 -437px;position:relative;top:5px;margin-right:3px}.card-container{padding-top:5px;padding-bottom:5px}.card-container .swiper-slide{width:200px}.swiper-card{width:200px;min-height:210px;border:1px solid #e2e2e2\9;background:#fff;-webkit-box-shadow:0 1px 4px rgba(0,0,0,.2);-moz-box-shadow:0 1px 4px rgba(0,0,0,.2);-ms-box-shadow:0 1px 4px rgba(0,0,0,.2);box-shadow:0 1px 4px rgba(0,0,0,.2);-webkit-border-radius:2px;-moz-border-radius:2px;-ms-border-radius:2px;border-radius:2px}.swiper-image-preview{height:95px;padding:5px 5px 0}.swiper-image-preview img{height:90px}.padding-10-15{padding:10px 15px}.btn-card{padding:6px;overflow:hidden}.text-truncate{width:100%;text-align:left;text-overflow:ellipsis;white-space:nowrap;overflow:hidden}.bw-horizontal-swiper.card-container .swiper-slide{width:308px}.bw-horizontal-swiper .swiper-card{width:308px;font-size:14px;min-height:140px}.bw-horizontal-swiper .swiper-card a{display:block;padding:15px 10px 10px}.details-column{width: 92%}@media only screen and (max-width:320px){.bw-horizontal-swiper .swiper-card,.bw-horizontal-swiper.card-container .swiper-slide{width:275px}}.edit-blue { width: 16px; height: 16px; background-position: -83px -257px; }
    </style>
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->

        var makeName = "<%=makeName%>";
        var makeMaskingName = "<%=makeMaskingName%>";
        var makeId = "<%=makeId%>";
        var cityName = "<%=cityName%>";
        var cityId = "<%= cityId%>";
        var cityMaskingName = "<%= cityMaskingName%>";
        var clientIP = "<%= clientIP %>";
        var pageUrl = "<%= pageUrl%>";
        var pqSrcId = "<%= Convert.ToUInt16(Bikewale.Entities.PriceQuote.PQSourceEnum.Mobile_DealerLocator_Listing) %>";
        var pageSrcId = eval("<%= Bikewale.Utility.BWConfiguration.Instance.SourceId %>");
        var googleMapAPIKey = "<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey %>";
    </script>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->

        <section>
            <div class="container margin-bottom10">
                <div class="bg-white">
                    <h1 class="box-shadow padding-15-20"><%=makeName%> Dealer Showrooms in <%=cityName%></h1>
                    <div class="box-shadow font14 text-light-grey padding-15-20">
                       <%=makeName%> has <%=totalDealers %> authorized dealers in <%=cityName%>. BikeWale recommends buying bikes only from authorized <%=makeName%> showroom in <%=cityName%>. 
                            For information on prices, offers, EMI options , test rides etc. you may get in touch with any of the below mentioned authorized <%=makeName%> dealers in <%=cityName%>.
                    </div>
                </div>
            </div>
        </section>

        <section>
            <div class="container bg-white box-shadow margin-bottom10">
                <h2 class="font16 text-black text-bold padding-15-20 border-solid-bottom"><%=totalDealers %> <%=makeName%> showrooms in <%=cityName%>
                    <a href="Javascript:void(0)" rel="nofollow" id="changeOptions"><span class="margin-left5 bwmsprite edit-blue"></span>change</a>
                </h2>
                <ul id="dealersList">
                    <asp:Repeater ID="rptDealers" runat="server">
                        <ItemTemplate>
                            <li>
                                <a href="/m/<%=makeMaskingName %>-dealer-showrooms-in-<%=cityMaskingName %>/<%# DataBinder.Eval(Container.DataItem,"DealerId") %>-<%# Bikewale.Utility.UrlFormatter.RemoveSpecialCharUrl(DataBinder.Eval(Container.DataItem,"Name").ToString()) %>/" title="<%# DataBinder.Eval(Container.DataItem,"Name") %>">
                                    <div class="<%# ((DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "3") || (DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "2"))? "" : "hide" %> featured-tag">
                                        <span class="bwmsprite star-white"></span>Featured
                                    </div>
                                    <h3 class="text-truncate margin-bottom5">
                                        <%# (DataBinder.Eval(Container.DataItem,"Name")) %>
                                    </h3>
                                    <p class="<%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"Address").ToString()))?"hide":"margin-bottom5" %>">
                                        <span class="bwmsprite dealership-loc-icon vertical-top"></span>
                                        <span class="vertical-top dealership-details text-light-grey"><%# DataBinder.Eval(Container.DataItem,"Address") %></span>
                                    </p>
                                    <div class="<%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString()))?"hide":"" %>">
                                        <a href="tel:<%#DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString() %>" class="text-default text-bold maskingNumber">
                                            <span class="vertical-top bwmsprite tel-sm-grey-icon"></span>
                                            <span class="vertical-top dealership-details"><%# DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString() %></span>
                                        </a>
                                    </div>
                                </a>
                                <input data-leadsourceid="20" data-item-id="<%# DataBinder.Eval(Container.DataItem,"DealerId") %>" data-item-type="<%# (DataBinder.Eval(Container.DataItem,"DealerType")) %>" data-item-name="<%# DataBinder.Eval(Container.DataItem,"Name") %>" data-item-area="<%# DataBinder.Eval(Container.DataItem,"Name") %>" data-campid="<%# DataBinder.Eval(Container.DataItem,"CampaignId") %>" data-pqsourceid="<%= (int) Bikewale.Entities.PriceQuote.PQSourceEnum.Desktop_DealerLocator_GetOfferButton %>" type="button" class="btn btn-white margin-top10 leadcapturebtn <%# ((DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "3") || (DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "2"))? "" : "hide" %>" value="Get offers from dealer">
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </section>

        <% if (ctrlPopoularBikeMake.FetchedRecordsCount > 0 || ctrlRecentUsedBikes.fetchedCount > 0) {%>
        <section>
            <div class="container bg-white box-shadow margin-bottom15">
                
             <div class="container bg-white box-shadow margin-bottom15">
                <% if (ctrlPopoularBikeMake.FetchedRecordsCount > 0)
                 {%> 
                 <BW:PopularBikeMake runat="server" ID="ctrlPopoularBikeMake" />
                <%} %>

                <div class="padding-top10 text-center">
                    <!-- #include file="/ads/Ad300x250_mobile.aspx" -->
                </div>
                <div class="margin-right10 margin-left10 border-solid-bottom"></div>
                    <% if (ctrlServiceCenterCard.showWidget)
                   { %>
                    <BW:ServiceCenterCard runat="server" ID="ctrlServiceCenterCard" />
                        <div class="margin-right10 margin-left10 border-solid-bottom"></div>
                <% }  %>
                <% if (ctrlRecentUsedBikes.fetchedCount > 0)
                {%> 
                 <BW:MostRecentUsedBikes runat="server" ID="ctrlRecentUsedBikes" />
                <%} %>
            </div>
                </div>
            
        </section>
        <% } %>
         
        <section>
            <div class="container margin-bottom30">
                <div class="grid-12 font12">
                    <span class="font14"><strong>Disclaimer:</strong></span> The above mentioned information about <%=makeName %> dealership showrooms in <%=cityName %> is furnished to the best of our knowledge. 
                        All <%=makeName %> bike models and colour options may not be available at each of the <%=makeName %> dealers. 
                        We recommend that you call and check with your nearest <%=makeName %> dealer before scheduling a showroom visit.
                </div>
                <div class="clear"></div>
            </div>
        </section>
      
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <BW:BrandCity runat="server" ID="ctrlBrandCity" />
        <BW:LeadCapture ID="ctrlLeadCapture" runat="server" />
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/common.min.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        <script type="text/javascript">
            $(".leadcapturebtn").click(function (e) {
                ele = $(this);
                var leadOptions = {
                    "dealerid": ele.attr('data-item-id'),
                    "dealername": ele.attr('data-item-name'),
                    "dealerarea": ele.attr('data-item-area'),
                    "campid": ele.attr('data-campid'),
                    "leadsourceid": ele.attr('data-leadsourceid'),
                    "pqsourceid": ele.attr('data-pqsourceid'),
                    "isdealerbikes": true,
                    "pageurl": window.location.href,
                    "isregisterpq": true,
                    "clientip": clientIP
                };
                dleadvm.setOptions(leadOptions);
            });
        </script>
    </form>
</body>
</html>
