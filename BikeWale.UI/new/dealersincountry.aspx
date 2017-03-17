<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.DealersInCountry" EnableViewState="false" Trace="false" Debug="false" %>
<%@ Register Src="~/controls/NewLaunchedBikes_new.ascx" TagName="NewLaunchedBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/UpcomingBikes_new.ascx" TagName="UpcomingBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/DealersByBrand.ascx" TagName="DealersByBrand" TagPrefix="BW" %>
<%@ Register Src="~/controls/usedBikeModel.ascx" TagName="usedBikeModel" TagPrefix="BW" %>

<%@ Import Namespace="Bikewale.Common" %>

<!doctype html>
<html>
<head>
    <% 
        title = string.Format("{0} Bike Showrooms in India | {0} Bike Dealers in India - BikeWale", objMMV.MakeName, stateName);
        keywords = string.Format("{0} bike dealers, {0} bike showrooms, {0} dealers, {0} showrooms, {0} dealerships, dealerships, test drive, {0} dealer contact number", objMMV.MakeName);
        description = string.Format("Find the nearest {0} showroom in your city. There are {1} {0} showrooms in {2} cities in India. Get contact details, address, and direction of {0} dealers.", objMMV.MakeName, DealerCount, citiesCount);
        canonical = string.Format("https://www.bikewale.com/{0}-dealer-showrooms-in-india/", objMMV.MaskingName);
        alternate = string.Format("https://www.bikewale.com/m/{0}-dealer-showrooms-in-india/", objMMV.MaskingName);
        isAd970x90Shown = false;
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
        isAd970x90BottomShown = false;
        isHeaderFix = false;
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/css/dealer/location.css" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section>
            <div class="container padding-top10">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <ul>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <a itemprop="url" href="/"><span itemprop="title">Home</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span><a itemprop="url" href="/dealer-showroom-locator/"><span itemprop="title">Showroom Locator</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span><%=objMMV.MakeName %> Showrooms in India
                            </li>
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
                            <h1><%=objMMV.MakeName %> Showrooms in India</h1>
                        </div>
                        <div class="padding-14-20 font14 text-light-grey collapsible-content">
                            <p class="main-content">BikeWale recommends to buy your <%=objMMV.MakeName %> bike only from authorized <%=objMMV.MakeName %> showrooms. We bring you a list of <%=DealerCount%> <%=objMMV.MakeName %>  <%=DealerCount>1?"showrooms":"showroom"%> present in <%=citiesCount%> <%=citiesCount>1?"cities":"city"%> in India. The showroom locator tool will help you find the <%=objMMV.MakeName %> showroom in your city.</p>
					        <p class="more-content"> BikeWale works with more than 200+ bike showrooms in India to provide you a hassle-free bike buying experience. Get <%=objMMV.MakeName %> showroom’s address, contact details, EMI options for your nearest dealer.</p><a href="javascript:void(0)" class="read-more-target" rel="nofollow">...Read more</a>
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
                        <h2 class="font18 bg-white padding-18-20"><%=DealerCount%> <%=objMMV.MakeName %> dealers in <%=citiesCount%> cities</h2>
                        <div id="listing-left-column" class="grid-4">
                            <div id="filter-input" class="form-control-box">
                                <span class="bwsprite search-icon-grey"></span>
                                <input type="text" class="form-control padding-right40" placeholder="Type to search city" id="getCityInput" />
                                <span class="fa fa-spinner fa-spin position-abt text-black"></span>
                                <span class="bwsprite error-icon errorIcon"></span>
                                <div class="bw-blackbg-tooltip errorText"></div>
                            </div>
                            <ul id="location-list">
                                  <% foreach (Bikewale.Entities.DealerLocator.StateCityEntity st in states.stateCityList)
                       { %>
                                <li  class="item-state">
                                    <p data-item-id="<%=st.Id %>" data-item-name="<%=st.Name %>" data-lat="<%=st.Lat %>" data-long ="<%=st.Long %>" data-dealercount="<%=st.DealerCountState%>" class="type-state cur-pointer" data-item-id="<%=st.Id %>"><%=st.Name %></p>
                                                 <ul class="location-list-city">
                                                     <% foreach (Bikewale.Entities.Location.DealerCityEntity stcity in st.Cities)
                       { %>
                                    
                                        <li>
                                            <a data-item-id="<%=stcity.CityId %>" data-item-name="<%=stcity.CityName %>" data-lat="<%=stcity.Lattitude %>" data-long ="<%=stcity.Longitude %>" data-link="<%=stcity.Link %>" data-dealercount="<%=stcity.DealersCount%>" title=" <%=objMMV.MakeName%> dealer showrooms in <%=stcity.CityName %>" href="/<%=makeMaskingName %>-dealer-showrooms-in-<%=stcity.CityMaskingName %>/"><%=stcity.CityName %> (<%=stcity.DealersCount %>)</a>
                                        </li>
                                      <%}%>
                                    </ul>
                                   
                                </li>
                              <%}%>
                                
                            </ul>
                            <div id="no-result"></div>
                        </div>
                        <div id="listing-right-column" class="grid-8 alpha omega">
                            <div class="dealer-map-wrapper">
                                <div id="dealerMapWrapper" style="width: 661px; height: 530px; background: #fff url(https://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif) no-repeat center;">
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
         <% if (ctrlDealerByBrand.FetchedRecordsCount > 0)
                           { %>
        <section>
            <BW:DealersByBrand runat="server" ID="ctrlDealerByBrand" />
        </section>
        <%} %>

        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>
                      
           <% if (ctrlNewLaunchedBikes.FetchedRecordsCount > 0 || ctrlUpcomingBikes.FetchedRecordsCount > 0 || ctrlusedBikeModel.FetchCount > 0)
              { %>
        <section>
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div class="content-box-shadow">
                          <% if (ctrlNewLaunchedBikes.FetchedRecordsCount > 0)
                           { %>
                        <div class="carousel-heading-content padding-top20">
                            <div class="swiper-heading-left-grid inline-block">
                                <h2>New <%=objMMV.MakeName %> bike launches</h2>
                            </div><div class="swiper-heading-right-grid inline-block text-right">
                                <a href="/new-<%= objMMV.MaskingName %>-bike-launches/" title="<%= objMMV.MakeName %> Bike Launches in India" class="btn view-all-target-btn">View all</a>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <BW:NewLaunchedBikes runat="server" ID="ctrlNewLaunchedBikes" />
                        <%} %>
                        <div class="margin-top20 margin-right10 margin-left10 border-solid-top"></div>
                        <% if (ctrlUpcomingBikes.FetchedRecordsCount > 0)
                           { %>
                        <div class="carousel-heading-content padding-top20">
                            <div class="swiper-heading-left-grid inline-block">
                                <h2>Upcoming <%= objMMV.MakeName %> Bikes</h2>
                            </div><div class="swiper-heading-right-grid inline-block text-right">
                                <a href="/<%= objMMV.MaskingName %>-bikes/upcoming/" title="Upcoming <%= objMMV.MakeName %> Bikes in India" class="btn view-all-target-btn">View all</a>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <div class="jcarousel-wrapper inner-content-carousel padding-bottom20">
                            <div class="jcarousel">
                                <ul>
                                    <BW:UpcomingBikes runat="server" ID="ctrlUpcomingBikes" />
                                </ul>
                            </div>
                            <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev inactive" rel="nofollow"></a></span>
                            <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
                        </div>
         <%} %>
                         <div class="margin-top20 margin-right10 margin-left10 border-solid-top"></div>
                  <% if (ctrlusedBikeModel.FetchCount>0)
                       { %>
                 
                    <BW:usedBikeModel runat="server" ID="ctrlusedBikeModel" />
                        
                    <% } %>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
            <%} %>
        </section>

        <!-- #include file="/includes/footerBW.aspx" -->
        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript.aspx" -->
        <script src="https://maps.googleapis.com/maps/api/js?key=<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey %>&libraries=places"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/src/new/markerwithlabel.js"></script>
        <script type="text/javascript">
            var dealersByCity = true;
        </script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/src/dealer/location.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/fontBW.aspx" -->
    </form>
</body>
</html>
