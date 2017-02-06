<%@ Page Language="C#" Inherits="Bikewale.New.BrowseNewBikeDealerDetails" AutoEventWireup="false" EnableViewState="false" %>
<%@ Register Src="~/controls/UsedBikeWidget.ascx" TagName="UsedBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/UsedPopularModelsInCity.ascx" TagName="UsedPopularModels" TagPrefix="BW" %>
<%@ Register Src="~/controls/MostPopularBikes_new.ascx" TagName="MostPopularBikesMake" TagPrefix="BW" %>
<%@ Register Src="~/controls/LeadCaptureControl.ascx" TagName="LeadCapture" TagPrefix="BW" %>
<%@ Register Src="~/controls/ServiceCenterCard.ascx" TagName="ServiceCenterCard" TagPrefix="BW" %>
<%@ Register Src="~/controls/BrandCityPopUp.ascx" TagName="BrandCity" TagPrefix="BW" %>
<%@ Register Src="~/controls/DealersInNearByCities.ascx" TagName="DealersCount" TagPrefix="BW" %>
<!DOCTYPE html>

<html>
<head>
    <%      
        keywords = String.Format("{0} showroom {1}, {0} dealers {1}, {1} bike showroom, {1} bike dealers,{1} dealers, {1} bike showroom, bike dealers, bike showroom, dealerships", makeName, cityName);
        description = String.Format("Find address, contact details and direction for {2} {0} showrooms in {1}. Contact {0} showroom near you for prices, EMI options, and availability of {0} bike", makeName, cityName, totalDealers);
        title = String.Format("{0} showroom in {1} | {2} {0} bike dealers- BikeWale", makeName, cityName, totalDealers);
        canonical = String.Format("https://www.bikewale.com/{0}-dealer-showrooms-in-{1}/", makeMaskingName, cityMaskingName);
        alternate = String.Format("https://www.bikewale.com/m/{0}-dealer-showrooms-in-{1}/", makeMaskingName, cityMaskingName);
        AdId = "1395986297721";
        AdPath = "/1017752/BikeWale_New_";
        isAd970x90Shown = true;
        isAd970x90BottomShown = true;
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
        isHeaderFix = false;
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->

    <link rel="stylesheet" href="/css/dealer/listing.css" type="text/css" />
    <script src="https://maps.googleapis.com/maps/api/js?key=<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey %>&libraries=places&callback=initializeMap" async defer></script>
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
                                <span class="bwsprite fa-angle-right margin-right10"></span><a itemprop="url" href="/dealer-showroom-locator/"><span itemprop="title">Showroom Locator</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span><a itemprop="url" href="/<%=makeMaskingName %>-dealer-showrooms-in-india/"><span itemprop="title"><%=makeName%> Showrooms</span></a>
                            </li>
                            <li class="current"><span class="bwsprite fa-angle-right margin-right10"></span><%=makeName%> Showrooms in <%=cityName %></li>
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
                            <h1><%=makeName%> Showroom<%=(totalDealers > 1 )?"s":"" %> in <%=cityName%></h1>
                             </div>
                         <div class="padding-14-20 font14 text-light-grey">
                        <p id="dealer-main-content" >Showroom experience has always played an important role while buying a new bike. BikeWale brings you the address, contact details and directions of <%=makeName%> Showroom to improve your buying experience. There  <%=totalDealers>1?"are":"is"%> <%=totalDealers %>  <%=makeName%> <%=totalDealers>1?"showrooms":"showroom"%> in  <%=cityName%>. BikeWale recommends buying bikes from authorized <%=makeName%> showroom in  <%=cityName%></p>
                            <p id="dealer-more-content"> For information on prices, offers, EMI options and test rides you may get in touch with below mentioned <%=makeName%> dealers in  <%=cityName%>.</p>
                            <a href="javascript:void(0)" id="read-more-target" rel="nofollow">...Read more</a>
                         </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <section>
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <div class="padding-18-20">
                            <h2 class="font18 text-black text-bold bg-white dealer-heading inline-block margin-right10"><%=totalDealers %> <%=makeName %> showroom<%=(totalDealers>1)?"s":"" %> in <%=cityName %> </h2>
                            <div class="inline-block">
                                <span class="edit-blue-link" id="brandSelect" ><span class="bwsprite edit-blue text-link"></span> <span class="change text-link">change</span></span>
                            </div>
                            <div class="clear"></div>
                        </div>
                        
                        <div id="listing-left-column" class="grid-4">
                            <ul id="dealersList">
                                <asp:Repeater ID="rptDealers" runat="server">
                                    <ItemTemplate>                                     
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
                                                    <span class="vertical-top dealership-card-details"><%#(String.IsNullOrEmpty(Convert.ToString(DataBinder.Eval(Container.DataItem,"objArea.PinCode"))))?string.Format("{0}, {1}",DataBinder.Eval(Container.DataItem,"Address"),cityName):string.Format("{0}, {1}, {2}",DataBinder.Eval(Container.DataItem,"Address"),DataBinder.Eval(Container.DataItem,"objArea.PinCode"),cityName)%></span>
                                                
                                                </p>
                                                <p class="<%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString()))?"hide":"" %>">
                                                    <span class="bwsprite phone-black-icon vertical-top margin-right5"></span>
                                                    <span class="vertical-top text-bold text-default dealership-card-details"><%# DataBinder.Eval(Container.DataItem,"MaskingNumber") %></span>
                                                </p>
                                            </a>
                                            <div class="<%# ((DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "3") || (DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "2"))? "margin-top20" : "hide" %>">
                                                <a data-item-id="<%# DataBinder.Eval(Container.DataItem,"DealerId") %>" href="Javascript:void(0)" data-leadsourceid="14"
                                                    data-item-name="<%# DataBinder.Eval(Container.DataItem,"Name") %>" data-item-area="" data-campid="<%# DataBinder.Eval(Container.DataItem,"CampaignId") %>" data-pqsourceid="<%= (int) Bikewale.Entities.PriceQuote.PQSourceEnum.Desktop_DealerLocator_GetOfferButton %>" class="btn btn-white btn-full-width font14 leadcapturebtn"><%# DataBinder.Eval(Container.DataItem,"DisplayTextLarge").ToString() %></a>
                                            </div>
                                        </li>                                    
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

        <% if(ctrlDealerCount.FetchedRecordsCount > 0) { %>
        <BW:DealersCount ID="ctrlDealerCount" runat="server" />
        <% } %>

        <% if (ctrlPopoularBikeMake.FetchedRecordsCount > 0 || ctrlUsedModels.FetchedRecordsCount > 0 || ctrlServiceCenterCard.showWidget)
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
                          <BW:ServiceCenterCard runat="server" ID="ctrlServiceCenterCard" />
                          <div class="margin-left10 margin-right10 border-solid-bottom"></div>
                        <% } %>
                        <% if (ctrlUsedModels.FetchedRecordsCount > 0)
                           { %>
                        <BW:UsedPopularModels runat="server" ID="ctrlUsedModels" />
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

        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <BW:LeadCapture ID="ctrlLeadCapture" runat="server" />
        <BW:BrandCity ID="ctrlBrandCity" runat="server" />
        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/src/dealer/listing.js?<%= staticFileVersion %>"></script>
        
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
        <!-- #include file="/includes/fontBW.aspx" -->
    </form>
</body>
</html>
