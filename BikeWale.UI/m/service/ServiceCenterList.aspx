<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Service.ServiceCenterList" EnableViewState="false" %>
<%@ Register Src="~/m/controls/UsedBikes.ascx" TagName="MostRecentusedBikes" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/MMostPopularBikes.ascx" TagName="PopularBikeMake" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/LeadCaptureControl.ascx" TagName="LeadCapture" TagPrefix="BW" %>
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
    <link href="/min/m/css/service/listing.css" rel="stylesheet" type="text/css" />
    <!--
    <style type="text/css">
        @charset "utf-8";.swiper-card,.swiper-slide:first-child{margin-left:5px}#dealersList a:hover,.swiper-card a:hover{text-decoration:none}.padding-15-20{padding:15px 20px}#dealersList{padding:0 20px 20px}#dealersList li{border-top:1px solid #e2e2e2;padding-top:15px;margin-top:20px;font-size:14px}#dealersList li:first-child{border-top:0;margin-top:0}#dealersList a{display:block}.featured-tag{width:74px;text-align:center;background:#3799a7;margin-bottom:5px;z-index:1;font-size:12px;color:#fff;line-height:20px;-webkit-border-radius:2px;-moz-border-radius:2px;-o-border-radius:2px;border-radius:2px}.vertical-top{display:inline-block;vertical-align:top}.dealership-details{width:92%}.leadcapturebtn.btn{font-size:14px;padding:9px 21px}.dealership-loc-icon{width:10px;height:14px;background-position:-40px -436px;position:relative;top:4px;margin-right:3px}.star-white{width:8px;height:8px;background-position:-174px -447px;margin-right:4px}.tel-sm-grey-icon{width:10px;height:10px;background-position:0 -437px;position:relative;top:5px;margin-right:3px}.card-container{padding-top:5px;padding-bottom:5px}.card-container .swiper-slide{width:200px}.swiper-card{width:200px;min-height:210px;border:1px solid #e2e2e2\9;background:#fff;-webkit-box-shadow:0 1px 4px rgba(0,0,0,.2);-moz-box-shadow:0 1px 4px rgba(0,0,0,.2);-ms-box-shadow:0 1px 4px rgba(0,0,0,.2);box-shadow:0 1px 4px rgba(0,0,0,.2);-webkit-border-radius:2px;-moz-border-radius:2px;-ms-border-radius:2px;border-radius:2px}.swiper-image-preview{height:95px;padding:5px 5px 0}.swiper-image-preview img{height:90px}.padding-10-15{padding:10px 15px}.btn-card{padding:6px;overflow:hidden}.text-truncate{width:100%;text-align:left;text-overflow:ellipsis;white-space:nowrap;overflow:hidden}#makeTabsContentWrapper h2{margin-bottom:13px}#makeUsedBikeContent > h2{padding-bottom:20px;}
    </style>
    -->
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
            <div class="container bg-white card-bottom-margin">
                <h1 class="card-header"><%=makeName%> service center in <%=cityName%></h1>
                <div class="card-inner-padding font14 text-light-grey">
                    <p id="service-main-content">Is your Bajaj bike due for a service or are you looking to repair your Bajaj bike? BikeWale brings you the list of all authorised Bajaj service centers in Mumbai. Bajaj has 100 authorised</p><p id="service-more-content">service centers in Mumbai. We recommend availing services only from authorised service centers.<br />Authorised Honda service centers abide by the servicing standards of Honda with an assurance of genuine Honda spare parts. BikeWale strongly recommends to use only Make genuine spare parts for your safety and durability of your bike. For more information on pick-up and drop facility, prices and service schedules get in touch with any of the below mentioned authorised make service centers in City. Do check out the maintenance tips and answers to FAQs from BikeWale experts!</p><a href="javascript:void(0)" id="read-more-target" rel="nofollow">Read more</a>
                </div>
            </div>
        </section>

        <section>
            <div class="container bg-white box-shadow card-bottom-margin">
                <h2 class="text-black text-bold padding-15-20 border-solid-bottom"><%=totalDealers %> <%=makeName%> showrooms in <%=cityName%></h2>
                <ul id="center-list">
                    <asp:Repeater ID="rptDealers" runat="server">
                        <ItemTemplate>
                            <li>
                                <a href="" title="Name of service center | Make | City">
                                    <h3 class="text-truncate margin-bottom5">
                                        <%# (DataBinder.Eval(Container.DataItem,"Name")) %>
                                    </h3>
                                    <p class="<%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"Address").ToString()))?"hide":"margin-bottom5" %>">
                                        <span class="bwmsprite dealership-loc-icon vertical-top"></span>
                                        <span class="vertical-top details-column text-light-grey"><%# DataBinder.Eval(Container.DataItem,"Address") %></span>
                                    </p>
                                    <p class="<%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString()))?"hide":"" %>">
                                        <span class="vertical-top bwmsprite tel-sm-grey-icon"></span>
                                        <span class="vertical-top details-column text-default text-bold"><%# DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString() %></span>
                                    </p>
                                    <button type="button" class="btn btn-white service-btn margin-top15">Get service center details</button>
                                </a>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </section>

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

                <% if (ctrlRecentUsedBikes.fetchedCount > 0)
                {%> 
                 <BW:MostRecentUsedBikes runat="server" ID="ctrlRecentUsedBikes" />
                <%} %>
            </div>
                </div>
            
        </section>
        <% } %>
        

        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <BW:LeadCapture ID="ctrlLeadCapture" runat="server" />
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/service/listing.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        
    </form>
</body>
</html>
