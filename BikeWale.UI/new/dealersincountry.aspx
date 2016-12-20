<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.DealersInCountry" EnableViewState="false" Trace="false" Debug="false" %>
<%@ Register Src="~/controls/NewLaunchedBikes_new.ascx" TagName="NewLaunchedBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/UpcomingBikes_new.ascx" TagName="UpcomingBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/DealersByBrand.ascx" TagName="DealersByBrand" TagPrefix="BW" %>

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
    <script src="https://maps.googleapis.com/maps/api/js?key=<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey %>&libraries=places"></script>
    <script type="text/javascript" src="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/src/new/markerwithlabel.js"></script>
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
                                <a href="/" itemprop="url">
                                    <span itemprop="title">Home</span>
                                </a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                 <a href="/new/" itemprop="url">
                                    <span itemprop="title">New Bikes</span>
                                </a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                 <a href="/dealer-showroom-locator/" itemprop="url">
                                    <span itemprop="title">Dealer Showroom locator</span>
                                </a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                    <span itemprop="title"><%=objMMV.MakeName %> Dealer Showrooms</span>
                            </li>
                            <%--<li><span class="bwsprite fa fa-angle-right margin-right10"></span>Dealer Locator</li>--%>
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
                        <div class="padding-14-20 font14 text-light-grey">
                            <p id="main-content">BikeWale recommends to buy your <%=objMMV.MakeName %> bike only from authorized <%=objMMV.MakeName %> showrooms. We bring you a list of <%=DealerCount%> <%=objMMV.MakeName %>  <%=DealerCount>1?"showrooms":"showroom"%> present in <%=citiesCount%> <%=citiesCount>1?"cities":"city"%> in India. The showroom locator tool will help you find the <%=objMMV.MakeName %> showroom in your city.</p>
					        <p id="more-content"> BikeWale works with more than 200+ bike showrooms in India to provide you a hassle-free bike buying experience. Get <%=objMMV.MakeName %> showroom’s address, contact details, EMI options for your nearest dealer.</p><a href="javascript:void(0)" id="read-more-target" rel="nofollow">...Read more</a>
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
         <% if (ctrlDealerByBrand.FetchedRecordsCount > 0)
                           { %>
        <section>
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <BW:DealersByBrand runat="server" ID="ctrlDealerByBrand" />
                    </div>
                    </div>
                <div class="clear"></div>
            </div>
        </section>
        <%} %>
                      
           <% if(ctrlNewLaunchedBikes.FetchedRecordsCount > 0 ||ctrlUpcomingBikes.FetchedRecordsCount  >0){ %>
        <section>
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div class="content-box-shadow">
                          <% if (ctrlNewLaunchedBikes.FetchedRecordsCount > 0)
                           { %>
                          <h2 class="font18 padding-18-20">Newly launched <%=objMMV.MakeName %> bikes</h2>
                        <BW:NewLaunchedBikes runat="server" ID="ctrlNewLaunchedBikes" />
                        <%} %>
                      


                        <div class="margin-top20 margin-right10 margin-left10 border-solid-top"></div>
                        <% if (ctrlUpcomingBikes.FetchedRecordsCount > 0)
           { %> <h2 class="font18 padding-18-20">Upcoming <%=objMMV.MakeName %> bikes</h2>
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
                       
                    </div>
                </div>
                <div class="clear"></div>
            </div>
            <%} %>
        </section>

        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "https://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript">
            var dealersByCity = true;
          <%--  var cityArr = JSON.parse('<%= cityArr %>');--%>
       <%--     var stateLat = '<%= (dealerCity != null && dealerCity.dealerStates != null) ? dealerCity.dealerStates.StateLatitude : string.Empty %>';
            var stateLong = '<%= (dealerCity != null && dealerCity.dealerStates != null) ? dealerCity.dealerStates.StateLongitude : string.Empty %>';--%>
        </script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/src/dealer/location.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->
    </form>
</body>
</html>
