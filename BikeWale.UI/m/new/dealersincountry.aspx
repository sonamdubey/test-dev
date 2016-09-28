<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.DealerInCountry" EnableViewState="false" %>
<%@ Register Src="~/m/controls/MUpcomingBikes.ascx" TagName="MUpcomingBikes" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/MNewLaunchedBikes.ascx" TagName="MNewLaunchedBikes" TagPrefix="BW" %>
<!DOCTYPE html>
<html>
<head>
     <% 
         title = string.Format("{0} Bike Showrooms in India | {0} Bike Dealers in India - BikeWale - BikeWale", objMMV.MakeName, stateName);
         keywords = string.Format("{0} bike dealers, {0} bike showrooms, {0} dealers, {0} showrooms, {0} dealerships, dealerships, test drive, {0} dealer contact number", objMMV.MakeName);
         description = string.Format("{0} bike dealers showrooms in India. Find {0} dealer showroom information for more than {1} dealers in {2} cities", objMMV.MakeName, DealerCount, citiesCount);
         canonical = string.Format("http://www.bikewale.com/{0}-dealer-showrooms-in-india/", objMMV.MaskingName);
        AdPath = "/1017752/Bikewale_Mobile_Model";
        AdId = "1444028976556";
        Ad_320x50 = true;
        Ad_Bot_320x50 = true;
    %>

    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <style type="text/css">
        @charset "utf-8";#no-result,.location-list-city{display:none}.padding-15-20{padding:15px 20px}.form-control-box .search-icon-grey{position:absolute;right:10px;top:10px;cursor:pointer;z-index:2;background-position:-34px -275px}.form-control-box .fa-spinner{display:none;right:14px;top:12px;z-index:3}#location-list .item-state{border-top:1px solid #f1f1f1}#location-list .item-state:first-child{border-top:0}#location-list a{color:#4d5057;font-size:14px;display:block;padding-top:13px;padding-bottom:13px}#location-list .type-state,#no-result{font-size:16px}#location-list li .type-state:hover{color:#2a2a2a;text-decoration:none}#location-list .type-state.active{padding-bottom:8px}#location-list .location-list-city a{color:#82888b;padding-top:9px;padding-bottom:9px}#location-list .location-list-city a:hover{color:#4d5057;text-decoration:none}#no-result{padding:13px 0;color:#82888b}.text-truncate{width:100%;text-align:left;text-overflow:ellipsis;white-space:nowrap;overflow:hidden}
    </style>
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body class="bg-light-grey">
    <form runat="server">                  
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
         <% if(Ad_320x50){ %>
            <section>            
                <div>
                    <!-- #include file="/ads/Ad320x50_mobile.aspx" -->
                </div>
            </section>
        <% } %>

        <section>
            <div class="container margin-bottom10">
                <div class="bg-white">
                    <h1 class="box-shadow padding-15-20">Bajaj dealer showrooms in India</h1>
                    <div class="box-shadow font14 text-light-grey padding-15-20">
                       <%=objMMV.MakeName %> sells bikesthrough a vast network of dealer showrooms.The network consists of <%=DealerCount%> authorized <%=objMMV.MakeName %> showrooms spread across <%=citiesCount%> cities in India. The <%=objMMV.MakeName%> dealer showroom locator will help you find the nearest authorized dealer in your city. In case, there are no <%=objMMV.MakeName %> showrooms in your city, you can get in touch with an authorized dealer in your nearby city.
                    </div>
                </div>
            </div>
        </section>

        <section>
            <div class="container bg-white margin-bottom10 box-shadow">
                <h2 class="padding-15-20 border-solid-bottom"><%=DealerCount%> <%=objMMV.MakeName %> dealer showrooms in <%=citiesCount%> cities</h2>
                <div class="content-inner-block-20">
                    <div class="form-control-box">
                        <span class="bwmsprite search-icon-grey"></span>
                        <input type="text" class="form-control padding-right40" placeholder="Type to select city" id="getCityInput" />
                        <span class="fa fa-spinner fa-spin position-abt text-black"></span>
                    </div>
                    <ul id="location-list">
                                  <% foreach (Bikewale.Entities.DealerLocator.StateCityEntity st in stateList)
                       { %>
                                <li  class="item-state">
                                    <a data-item-id="<%=st.Id %>" data-item-name="<%=st.Name %>" data-lat="<%=st.Lat %>" data-long ="<%=st.Long %>" data-dealercount="<%=st.DealerCountState%>"  href="" class="type-state" data-item-id="<%=st.Id %>"><%=st.Name %></a>
                                                 <ul class="location-list-city">
                                                     <% foreach (Bikewale.Entities.Location.DealerCityEntity stcity in st.Cities)
                       { %>
                                    
                                        <li>
                                            <a data-item-id="<%=stcity.Id %>" data-item-name="<%=stcity.CityName %>" data-lat="<%=stcity.Lattitude %>" data-long ="<%=stcity.Longitude %>" data-link="<%=stcity.Link %>" data-dealercount="<%=stcity.DealersCount%>" href="/m/<%=makeMaskingName %>-dealer-showrooms-in-<%=stcity.CityMaskingName %>/"><%=stcity.CityName %> (<%=stcity.DealersCount %>)</a>
                                        </li>
                                      <%}%>
                                    </ul>
                                   
                                </li>
                              <%}%>
                                
                            </ul>
                    <div id="no-result"></div>
                </div>
            </div>
        </section>

        <section>
            <div class="container bg-white margin-bottom10 box-shadow">
               <% if (ctrlNewLaunchedBikes.FetchedRecordsCount > 0)
                           { %>
                          <h2 class="font18 padding-15-20">Newly launched <%=objMMV.MakeName %> bikes</h2>
                <div class="swiper-container card-container">
                    <div class="swiper-wrapper">
                        <BW:MNewLaunchedBikes runat="server" ID="ctrlNewLaunchedBikes" />
                    </div>
                </div>
                        <%} %>

                <div class="margin-top20 margin-right20 margin-left20 border-solid-bottom"></div>
                 <% if (ctrlUpcomingBikes.FetchedRecordsCount > 0)
           { %> <h2 class="font18 padding-15-20">Upcoming <%=objMMV.MakeName %> bikes</h2>
                <div class="swiper-container card-container">
                    <div class="swiper-wrapper">
                        <BW:MUpcomingBikes runat="server" ID="ctrlUpcomingBikes" />
                    </div>
                </div>
        <%} %>
            </div>
        </section>
        
        <!--
        <ul id="listingUL" class="city-listing">
            <asp:Repeater ID="rptCity" runat="server">
                <ItemTemplate>
                    <li>
                        <a href="/m/<%=objMMV.MaskingName %>-bikes/dealers-in-<%#DataBinder.Eval(Container.DataItem,"cityMaskingName") %>/" data-item-id="<%#DataBinder.Eval(Container.DataItem,"cityId") %>"><%#DataBinder.Eval(Container.DataItem,"CityName") %> (<%#DataBinder.Eval(Container.DataItem,"DealersCount") %>)</a>
                    </li>

                </ItemTemplate>
            </asp:Repeater>
        </ul>
        -->
    
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/dealer/location.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
    </form>
</body>
</html>
