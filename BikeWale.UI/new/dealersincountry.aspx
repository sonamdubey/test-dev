<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.DealersInCountry" EnableViewState="false" Trace="false" Debug="false" %>
<%@ Register Src="~/controls/NewLaunchedBikes_new.ascx" TagName="NewLaunchedBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/UpcomingBikes_new.ascx" TagName="UpcomingBikes" TagPrefix="BW" %>

<%@ Import Namespace="Bikewale.Common" %>

<!doctype html>
<html>
<head>
    <% 
        title = string.Format("{0} Bike Showrooms in India | {0} Bike Dealers in India - BikeWale - BikeWale", objMMV.MakeName, stateName);
        keywords = string.Format("{0} bike dealers, {0} bike showrooms, {0} dealers, {0} showrooms, {0} dealerships, dealerships, test drive, {0} dealer contact number", objMMV.MakeName);
        description = string.Format("{0} bike dealer showrooms in India. Find {0} dealer showroom information for more than {1} dealers in {2} cities", objMMV.MakeName, DealerCount, citiesCount);
        canonical = string.Format("http://www.bikewale.com/{0}-dealer-showrooms-in-india/", objMMV.MaskingName);
        alternate = string.Format("http://www.bikewale.com/m/{0}-dealer-showrooms-in-india/", objMMV.MaskingName);
        isAd970x90Shown = false;
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
        isAd970x90BottomShown = false;
        isHeaderFix = false;
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <style type="text/css">
        @charset "utf-8";.padding-14-20{padding:14px 20px}.padding-18-20{padding:18px 20px}#listing-left-column.grid-4{padding-right:20px;padding-left:20px;width:32.333333%;box-shadow:0 0 8px #ddd;z-index:1}#listing-right-column.grid-8{width:67.666667%;overflow:hidden}#filter-input{margin-top:20px;background:#fff}#filter-input .search-icon-grey{position:absolute;right:10px;top:10px;cursor:pointer;z-index:2}#filter-input .fa-spinner{display:none;right:14px;top:12px}#filter-input .errorIcon,#filter-input .errorText{display:none}#location-list .item-state{border-top:1px solid #f1f1f1}#location-list .item-state:first-child{border-top:0}#location-list .type-state,#location-list a{color:#4d5057;font-size:14px;display:block;padding-top:13px;padding-bottom:13px}#location-list .type-state,#no-result{font-size:16px}#no-result,.gm-style-iw+div,.location-list-city{display:none}#location-list li .type-state:hover,.dealer-location-tooltip .type-state:hover,.dealer-location-tooltip a:hover{color:#2a2a2a;text-decoration:none}#location-list .location-list-city a{color:#82888b;padding-top:10px;padding-bottom:10px}#location-list .location-list-city a:hover{color:#4d5057;text-decoration:none}#no-result{padding:13px 0;color:#82888b}.labels{top:-14px;left:-12px}.labels a{color:#4d5057;background-color:#fff;font-size:10px;font-weight:700;text-align:center;width:24px;height:24px;display:block;border-radius:50%;white-space:nowrap;padding-top:4px;border:2px solid #82888b}.labels a:hover{border:2px solid #ef3f30;text-decoration:none}
    </style>
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
    <script src="http://maps.googleapis.com/maps/api/js?key=<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey %>&libraries=places"></script>
    <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/new/markerwithlabel.js"></script>
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
                            <h1><%=objMMV.MakeName %> dealer showrooms in India</h1>
                        </div>
                        <p class="font14 text-light-grey content-inner-block-20">
                            <%=objMMV.MakeName %> sells bikes through a vast network of dealer showrooms.The network consists of <%=DealerCount%> authorized <%=objMMV.MakeName %> showrooms spread across <%=citiesCount%> cities in India. The <%=objMMV.MakeName%> dealer showroom locator will help you find the nearest authorized dealer in your city. In case, there are no <%=objMMV.MakeName %> showrooms in your city, you can get in touch with an authorized dealer in your nearby city.
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
                        <h2 class="font18 bg-white padding-18-20"><%=DealerCount%> <%=objMMV.MakeName %> dealer showrooms in <%=citiesCount%> cities</h2>
                        <div id="listing-left-column" class="grid-4">
                            <div id="filter-input" class="form-control-box">
                                <span class="bwsprite search-icon-grey"></span>
                                <input type="text" class="form-control padding-right40" placeholder="Type to search city" id="getCityInput" />
                                <span class="fa fa-spinner fa-spin position-abt text-black"></span>
                                <span class="bwsprite error-icon errorIcon"></span>
                                <div class="bw-blackbg-tooltip errorText"></div>
                            </div>
                            <ul id="location-list">
                                  <% foreach (Bikewale.Entities.DealerLocator.StateCityEntity st in stateList)
                       { %>
                                <li  class="item-state">
                                    <p data-item-id="<%=st.Id %>" data-item-name="<%=st.Name %>" data-lat="<%=st.Lat %>" data-long ="<%=st.Long %>" data-dealercount="<%=st.DealerCountState%>" class="type-state" data-item-id="<%=st.Id %>"><%=st.Name %></p>
                                                 <ul class="location-list-city">
                                                     <% foreach (Bikewale.Entities.Location.DealerCityEntity stcity in st.Cities)
                       { %>
                                    
                                        <li>
                                            <a data-item-id="<%=stcity.Id %>" data-item-name="<%=stcity.CityName %>" data-lat="<%=stcity.Lattitude %>" data-long ="<%=stcity.Longitude %>" data-link="<%=stcity.Link %>" data-dealercount="<%=stcity.DealersCount%>" title=" <%=objMMV.MakeName%> dealer showrooms in <%=stcity.CityName %>" href="/<%=makeMaskingName %>-dealer-showrooms-in-<%=stcity.CityMaskingName %>/"><%=stcity.CityName %> (<%=stcity.DealersCount %>)</a>
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

        <section>
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div class="content-box-shadow">
                          <% if (ctrlNewLaunchedBikes.FetchedRecordsCount > 0)
                           { %>
                          <h2 class="font18 padding-18-20">Newly launched <%=objMMV.MakeName %> bikes</h2>
                        <BW:NewLaunchedBikes runat="server" ID="ctrlNewLaunchedBikes" />
                        <%} %>
                      


                        <div class="margin-right10 margin-left10 border-solid-top"></div>
                        <% if (ctrlUpcomingBikes.FetchedRecordsCount > 0)
           { %> <h2 class="font18 padding-18-20">Upcoming <%=objMMV.MakeName %> bikes</h2>
                         <div class="jcarousel-wrapper inner-content-carousel">
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
        </section>

        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript">
            var dealersByCity = true;
          <%--  var cityArr = JSON.parse('<%= cityArr %>');--%>
            var stateLat = '<%= (dealerCity != null && dealerCity.dealerStates != null) ? dealerCity.dealerStates.StateLatitude : string.Empty %>';
            var stateLong = '<%= (dealerCity != null && dealerCity.dealerStates != null) ? dealerCity.dealerStates.StateLongitude : string.Empty %>';
        </script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/dealer/location.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->
    </form>
</body>
</html>
