<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.NewBikeDealerList" EnableViewState="false" %>
<%@ Register Src="~/UI/m/controls/MMostPopularBikes.ascx" TagName="PopularBikeMake" TagPrefix="BW" %>
<%@ Register Src="~/UI/m/controls/LeadCaptureControl.ascx" TagName="LeadCapture" TagPrefix="BW" %>
<%@ Register Src="~/UI/m/controls/ServiceCenterCard.ascx" TagName="ServiceCenterCard" TagPrefix="BW" %>
<%@ Register Src="~/UI/m/controls/BrandCityPopUp.ascx" TagName="BrandCity" TagPrefix="BW" %>
<%@ Register Src="~/UI/m/controls/DealersInNearByCities.ascx" TagName="DealersCount" TagPrefix="BW" %>
<%@ Register Src="~/UI/m/controls/usedBikeModel.ascx" TagName="usedBikeModel" TagPrefix="BW" %>
<!DOCTYPE html>
<html>
<head>

    <% 
        keywords = String.Format("{0} showroom {1}, {0} dealers {1}, {1} bike showroom, {1} bike dealers,{1} dealers, {1} bike showroom, bike dealers, bike showroom, dealerships", makeName, cityName);
        description = String.Format("Find address, contact details and direction for {2} {0} showrooms in {1}. Contact {0} showroom near you for prices, EMI options, and availability of {0} bike", makeName, cityName, totalDealers);
        title = String.Format("{0} showroom in {1} | {2} {0} bike dealers- BikeWale", makeName, cityName, totalDealers);
        canonical = String.Format("https://www.bikewale.com/dealer-showrooms/{0}/{1}/", makeMaskingName, cityMaskingName);
        Ad_320x50 = true;
        Ad_Bot_320x50 = true;
        
    %>
    <!-- #include file="/UI/includes/headscript_mobile_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/UI/m/css/dealer/listing.css" />
    <script type="text/javascript">
        <!-- #include file="\UI\includes\gacode_mobile.aspx" -->

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
        <!-- #include file="/UI/includes/headBW_Mobile.aspx" -->

        <section>
            <div class="container margin-bottom10">
                <div class="bg-white">
                    <h1 class="box-shadow padding-15-20"><%=makeName%> Showroom<%=(totalDealers > 1 )?"s":"" %> in <%=cityName%></h1>                  							
                    <div class="box-shadow padding-15-20 font14 text-light-grey collapsible-content">
                        <p class="main-content">Showroom experience has always played an important role while buying a new bike. BikeWale brings you the address, contact details and directions of <%=makeName%> Showroom to improve your buying experience. There <%=totalDealers>1?"are ":"is "%><%=totalDealers %><%= String.Format(" {0}", makeName)%><%=totalDealers>1?" showrooms":" showroom"%> in  <%=cityName%></p>
					    <p class="more-content">BikeWale recommends buying bikes from authorized <%=makeName%> showroom in <%=cityName%>. For information on prices, offers, EMI options and test rides you may get in touch with below mentioned <%=makeName%> dealers in <%=cityName%>. </p><a href="javascript:void(0)" class="read-more-target" rel="nofollow">...Read more</a>
                    </div>	
                </div>
            </div>
        </section>

        <section>
            <div class="container bg-white box-shadow margin-bottom10">
                <div class="border-solid-bottom padding-15-20">
                    <h2 class="inline-block text-bold dealer-heading padding-right10"><%= String.Format("{0} {1}",totalDealers, makeName) %> showroom<%=(totalDealers>1)?"s":"" %> in <%=cityName%></h2>
                    <div class="inline-block text-center">
                        <span class="edit-blue-link" id="changeOptions" ><span class="bwmsprite edit-blue margin-right5"></span><span class="change-text text-link">change</span></span>
                    </div>
                </div>

                <ul id="dealersList">
                    <asp:Repeater ID="rptDealers" runat="server">
                        <ItemTemplate>
                            <li>
                                <a href="/m/dealer-showrooms/<%=makeMaskingName %>/<%=cityMaskingName %>/dealer-name-<%# DataBinder.Eval(Container.DataItem,"DealerId") %>-<%# Bikewale.Utility.UrlFormatter.RemoveSpecialCharUrl(DataBinder.Eval(Container.DataItem,"Name").ToString()) %>/" title="<%# DataBinder.Eval(Container.DataItem,"Name") %>">
                                    <div class="<%# ((DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "3") || (DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "2"))? "" : "hide" %> featured-tag">
                                        <span class="bwmsprite star-white"></span>Featured
                                    </div>
                                    <h3 class="text-truncate margin-bottom5">
                                        <%# (DataBinder.Eval(Container.DataItem,"Name")) %>
                                    </h3>
                                    <p class="<%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"Address").ToString()))?"hide":"margin-bottom5" %>">
                                        <span class="bwmsprite dealership-loc-icon vertical-top"></span>
                                        <span class="vertical-top dealership-details text-light-grey"><%#String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"objArea.PinCode").ToString())?string.Format("{0}, {1}",DataBinder.Eval(Container.DataItem,"Address"),cityName):string.Format("{0}, {1}, {2}",DataBinder.Eval(Container.DataItem,"Address"),DataBinder.Eval(Container.DataItem,"objArea.PinCode"),cityName)%></span>
                                    </p>
                                    <div class="<%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString()))?"hide":"" %>">
                                        <a href="tel:<%#DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString() %>" class="text-default text-bold maskingNumber">
                                            <span class="vertical-top bwmsprite tel-sm-grey-icon"></span>
                                            <span class="vertical-top dealership-details"><%# DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString() %></span>
                                        </a>
                                    </div>
                                </a>
                                <input data-leadsourceid="20" data-item-id="<%# DataBinder.Eval(Container.DataItem,"DealerId") %>" data-item-type="<%# (DataBinder.Eval(Container.DataItem,"DealerType")) %>" data-item-name="<%# DataBinder.Eval(Container.DataItem,"Name") %>" data-item-area="<%# DataBinder.Eval(Container.DataItem,"Name") %>" data-campaignid="<%# DataBinder.Eval(Container.DataItem,"CampaignId") %>" data-pqsourceid="<%= (int) Bikewale.Entities.PriceQuote.PQSourceEnum.Desktop_DealerLocator_GetOfferButton %>" type="button" class="btn btn-white margin-top10 leadcapturebtn <%# ((DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "3") || (DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "2"))? "" : "hide" %>" value="<%# DataBinder.Eval(Container.DataItem,"DisplayTextLarge") %>">
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </section>

         <% if(ctrlDealerCount.FetchedRecordsCount > 0) { %>
        <BW:DealersCount ID="ctrlDealerCount" runat="server" />
        <% } %>

        <div class="margin-right10 margin-left10 border-solid-bottom"></div>

        <% if (ctrlPopoularBikeMake.FetchedRecordsCount > 0 || ctrlusedBikeModel.FetchCount > 0)
           {%>
        <section>
            <div class="container bg-white box-shadow margin-bottom15">
                <% if (ctrlPopoularBikeMake.FetchedRecordsCount > 0)
                 {%> 
                 <BW:PopularBikeMake runat="server" ID="ctrlPopoularBikeMake" />
                <%} %>

                <div class="padding-top10 text-center">
                    <!-- #include file="/UI/ads/Ad300x250_mobile.aspx" -->
                </div>
                <div class="margin-right10 margin-left10 border-solid-bottom"></div>
                    <% if (ctrlServiceCenterCard.showWidget)
                   { %>
                 <div class="margin-bottom10"></div>
                    <BW:ServiceCenterCard runat="server" ID="ctrlServiceCenterCard" />
                        <div class="margin-right10 margin-left10 border-solid-bottom"></div>
                
                     <% }  %>
               
                       <% if (ctrlusedBikeModel.FetchCount>0)
                       { %>
                     <div class="padding-top15">
                    <BW:usedBikeModel runat="server" ID="ctrlusedBikeModel" />
                        </div>
                    <% } %> 
                 
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
      
        <script type="text/javascript" src="<%= staticUrl  %>/UI/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/UI/includes/footerBW_Mobile.aspx" -->
        <BW:BrandCity runat="server" ID="ctrlBrandCity" />
        <BW:LeadCapture ID="ctrlLeadCapture" runat="server" />
        <link href="<%= staticUrl  %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/UI/includes/footerscript_mobile.aspx" -->
        <!-- #include file="/UI/includes/fontBW_Mobile.aspx" -->
        <script type="text/javascript">
            
            $(".leadcapturebtn").click(function (e) {
                ele = $(this);
                var leadOptions = {
                    "dealerid": ele.attr('data-item-id'),
                    "dealername": ele.attr('data-item-name'),
                    "dealerarea": ele.attr('data-item-area'),
                    "campaignId": ele.attr('data-campaignid'),
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
