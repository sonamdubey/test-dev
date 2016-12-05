<%@ Page Language="C#" Inherits="Bikewale.New.BrowseNewBikeDealerDetails" AutoEventWireup="false" EnableViewState="false" %>
<%@ Register Src="~/controls/UsedBikeWidget.ascx" TagName="UsedBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/MostPopularBikes_new.ascx" TagName="MostPopularBikesMake" TagPrefix="BW" %>
<%@ Register Src="~/controls/LeadCaptureControl.ascx" TagName="LeadCapture" TagPrefix="BW" %>
<%@ Register Src="~/controls/ServiceCenterCard.ascx" TagName="ServiceCenterCard" TagPrefix="BW" %>
<!DOCTYPE html>

<html>
<head>
    <%      
        keywords = String.Format("{0} showroom {1}, {0} dealers {1}, {1} bike showroom, {1} bike dealers,{1} dealers, {1} bike showroom, bike dealers, bike showroom, dealerships", makeName, cityName);
        description = String.Format("There are {2} {0} dealer showrooms in {1}. Get in touch with {0} showroom for prices, availability, test rides, EMI options and more!", makeName, cityName, totalDealers);
        title = String.Format("{0} Showrooms in {1} | {2} {0} Bike Dealers  - BikeWale", makeName, cityName, totalDealers);
        canonical = String.Format("http://www.bikewale.com/{0}-dealer-showrooms-in-{1}/", makeMaskingName, cityMaskingName);
        alternate = String.Format("http://www.bikewale.com/m/{0}-dealer-showrooms-in-{1}/", makeMaskingName, cityMaskingName);
        AdId = "1395986297721";
        AdPath = "/1017752/BikeWale_New_";
        isAd970x90Shown = true;
        isAd970x90BottomShown = true;
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
        isHeaderFix = false;
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <style type="text/css">
        .dealer-card-target:hover,.dealer-info-tooltip a:hover{text-decoration:none}.padding-14-20{padding:14px 20px}.padding-18-20{padding:18px 20px}#listing-left-column.grid-4{padding-right:20px;padding-left:20px;width:32.333333%;box-shadow:0 0 8px #ddd;z-index:1}#listing-right-column.grid-8{width:67.666667%}#dealersList li{padding-bottom:20px;border-top:1px solid #eee}#dealersList li:first-child{border-top:0}.dealer-card-target{display:block;padding-top:18px}.dealer-card-target .dealer-name{display:block;text-align:left;text-overflow:ellipsis;white-space:nowrap;overflow:hidden}.featured-tag{width:74px;display:block;text-align:center;line-height:20px;background:#3799a7;z-index:1;font-weight:400;font-size:12px;color:#fff;border-radius:2px;position:relative;top:-4px}.vertical-top{display:inline-block;vertical-align:top}.dealership-card-details{width:92%}.dealer-map-wrapper{width:100%;height:530px;display:block;position:relative}.dealer-info-tooltip{max-width:350px}#dealersMap .dealership-card-details{width:80%}.dealer-info-tooltip a:hover p{text-decoration:underline}#used-bikes-content .grid-6{display:inline-block;vertical-align:top;width:49%;float:none}.dealership-loc-icon{width:9px;height:12px;background-position:-52px -469px;position:relative;top:4px}.phone-black-icon{width:10px;height:10px;background-position:-73px -444px;position:relative;top:5px}.star-white{width:8px;height:8px;background-position:-222px -107px;margin-right:4px}.blue-right-arrow-icon{width:6px;height:10px;background-position:-74px -469px;position:relative;top:1px;left:7px}.btn.btn-size-2{padding:9px 20px}.card{width:292px;min-height:140px;border:1px solid #f6f6f6;-webkit-box-shadow:0 1px 2px 0 rgba(0,0,0,.2);-moz-box-shadow:0 1px 2px 0 rgba(0,0,0,.2);-ms-box-shadow:0 1px 2px 0 rgba(0,0,0,.2);-o-box-shadow:0 1px 2px 0 rgba(0,0,0,.2);box-shadow:0 1px 2px 0 rgba(0,0,0,.2);float:left;margin-left:30px;margin-bottom:20px}.card:first-child{margin-left:20px}.card .card-target{min-height:140px;display:block;padding:15px 20px 0}.card .card-target:hover{text-decoration:none}.card .text-truncate{width:100%}.details-column{width:92%}@media only screen and (max-width:1024px){#city-dealer-list li{width:280px}}
    </style>
    <script src="http://maps.googleapis.com/maps/api/js?key=<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey %>&libraries=places"></script>
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
        var currentCityName = '<%= cityName %>';
        var pageUrl = '<%= pageUrl %>';
        var clientip = '<%= clientIP %>';
    </script>

</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section>
            <div class="container padding-top10">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <a itemprop="url" href="/"><span itemprop="title">Home</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span><a itemprop="url" href="/new/"><span itemprop="title">New Bikes</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span><a itemprop="url" href="/dealer-showroom-locator/"><span itemprop="title">Dealer Showroom Locator</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span><a itemprop="url" href="/<%=makeMaskingName %>-dealer-showrooms-in-india/"><span itemprop="title"><%=makeName%> Dealer Showrooms</span></a>
                            </li>
                            <li class="current"><span class="bwsprite fa-angle-right margin-right10"></span><%=makeName%> Bikes Dealer Showroom in <%=cityName %></li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <div class="content-box-shadow padding-14-20">
                            <h1><%=makeName%> Dealer Showrooms in <%=cityName%></h1>
                        </div>
                        <p class="font14 text-light-grey content-inner-block-20">
                            <%=makeName%> has <%=totalDealers %> authorized dealers in <%=cityName%>. BikeWale recommends buying bikes only from authorized <%=makeName%> showroom in <%=cityName%>. 
                            For information on prices, offers, EMI options , test rides etc. you may get in touch with any of the below mentioned authorized <%=makeName%> dealers in <%=cityName%>.

                        </p>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <section>
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <h2 class="font18 text-black text-bold bg-white padding-18-20"><%=totalDealers %> <%=makeName %> showrooms in <%=cityName %> </h2>
                        <div id="listing-left-column" class="grid-4">
                            <ul id="dealersList">
                                <asp:Repeater ID="rptDealers" runat="server">
                                    <ItemTemplate>
                                     <a href="<%# Bikewale.Utility.UrlFormatter.GetDealerUrl(makeMaskingName, cityMaskingName,DataBinder.Eval(Container.DataItem,"Name").ToString(),Convert.ToInt32(DataBinder.Eval(Container.DataItem,"DealerId"))) %>"">
                                        <li data-item-type="<%# (DataBinder.Eval(Container.DataItem,"DealerType")) %>" data-item-id="<%# DataBinder.Eval(Container.DataItem,"DealerId") %>" data-item-inquired="false" data-item-number="<%# DataBinder.Eval(Container.DataItem,"MaskingNumber") %>" data-lat="<%# DataBinder.Eval(Container.DataItem,"objArea.Latitude") %>" data-log="<%# DataBinder.Eval(Container.DataItem,"objArea.Longitude") %>" data-address="<%# DataBinder.Eval(Container.DataItem,"Address") %>" 
                                            data-campid="<%# DataBinder.Eval(Container.DataItem,"CampaignId") %>" data-item-url="<%# Bikewale.Utility.UrlFormatter.RemoveSpecialCharUrl(DataBinder.Eval(Container.DataItem,"Name").ToString()) %>" >
                                            <a href="/<%=makeMaskingName %>-dealer-showrooms-in-<%=cityMaskingName %>/<%# DataBinder.Eval(Container.DataItem,"DealerId") %>-<%# Bikewale.Utility.UrlFormatter.RemoveSpecialCharUrl(DataBinder.Eval(Container.DataItem,"Name").ToString()) %>/" title="<%# DataBinder.Eval(Container.DataItem,"Name") %>" class="dealer-card-target font14">
                                                <div class="margin-bottom5">
                                                    <div class="<%# ((DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "3") || (DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "2"))? "" : "hide" %>">
                                                        <span class="featured-tag margin-bottom5"><span class="bwsprite star-white"></span>Featured</span>
                                                    </div>
                                                    <h3 class="dealer-name text-black text-bold"><%# DataBinder.Eval(Container.DataItem,"Name") %></h3>
                                                </div>
                                                <p class="<%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"Address").ToString()))?"hide":"text-light-grey margin-bottom5" %>">
                                                    <span class="bwsprite dealership-loc-icon vertical-top margin-right5"></span>
                                                    <span class="vertical-top dealership-card-details"><%# DataBinder.Eval(Container.DataItem,"Address") %></span>
                                                </p>
                                                <p class="<%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString()))?"hide":"" %>">
                                                    <span class="bwsprite phone-black-icon vertical-top margin-right5"></span>
                                                    <span class="vertical-top text-bold text-default dealership-card-details"><%# DataBinder.Eval(Container.DataItem,"MaskingNumber") %></span>
                                                </p>
                                            </a>
                                            <div class="<%# ((DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "3") || (DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "2"))? "margin-top20" : "hide" %>">
                                                <a data-item-id="<%# DataBinder.Eval(Container.DataItem,"DealerId") %>" href="Javascript:void(0)" data-leadsourceid="14"
                                                    data-item-name="<%# DataBinder.Eval(Container.DataItem,"Name") %>" data-item-area="" data-campid="<%# DataBinder.Eval(Container.DataItem,"CampaignId") %>" data-pqsourceid="<%= (int) Bikewale.Entities.PriceQuote.PQSourceEnum.Desktop_DealerLocator_GetOfferButton %>" class="btn btn-white btn-full-width font14 leadcapturebtn">Get offers from dealer</a>
                                            </div>
                                        </li>
                                    </a>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </div>
                        <div id="listing-right-column" class="grid-8 alpha omega">
                            <div class="dealer-map-wrapper">
                                <div id="dealerMapWrapper" style="width: 661px; height: 530px;">
                                    <div id="dealersMap" style="width: 661px; height: 530px;"></div>
                                </div>
                            </div>
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
            <div id="listing-footer"></div>
        </section>
        <% if (ctrlPopoularBikeMake.FetchedRecordsCount > 0 || ctrlRecentUsedBikes.FetchedRecordsCount > 0 || ctrlServiceCenterCard.showWidget)
           { %>
        <section>
            <div class="container">
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <% if (ctrlPopoularBikeMake.FetchedRecordsCount > 0)
                           { %>
                          <h2 class="font18 padding-18-20">Popular <%=makeName %> bikes in <%=cityName %></h2>
                        <BW:MostPopularBikesMake runat="server" ID="ctrlPopoularBikeMake" />
                        <%} %>
                        <div class="margin-left10 margin-right10 border-solid-bottom"></div>
                        <!-- Used bikes widget -->
                  <% if(ctrlServiceCenterCard.showWidget){ %>
            <section>
                <div class="container section-bottom-margin">
                   <div class="grid-12">
                    <div class="content-box-shadow padding-bottom20">
                      <BW:ServiceCenterCard runat="server" ID="ctrlServiceCenterCard" />
                   </div>
                 </div>
                <div class="clear"></div>
               </div>
            </section>
        <% } %>
                        
                        <% if (ctrlRecentUsedBikes.FetchedRecordsCount > 0)
                           { %>
                        <BW:UsedBikes runat="server" ID="ctrlRecentUsedBikes" />
                        <%} %>

                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <% } %>
        <section>
            <div class="container margin-top10 margin-bottom30">
                <div class="grid-12 font12">
                    <span class="font14"><strong>Disclaimer</strong>:</span> The above mentioned information about <%=makeName%> dealership showrooms in <%=cityName%> is furnished to the best of our knowledge. 
                        All <%=makeName%> bike models and colour options may not be available at each of the <%=makeName%> dealers. 
                        We recommend that you call and check with your nearest <%=makeName%> dealer before scheduling a showroom visit.
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <BW:LeadCapture ID="ctrlLeadCapture" runat="server" />
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/dealer/listing.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->
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
                    "clientip": clientip
                };
                dleadvm.setOptions(leadOptions);
            });
        </script>
    </form>
</body>
</html>
