<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.DealerInCountry" EnableViewState="false" %>

<%@ Register Src="~/UI/m/controls/MUpcomingBikes.ascx" TagName="MUpcomingBikes" TagPrefix="BW" %>
<%@ Register Src="~/UI/m/controls/MNewLaunchedBikes.ascx" TagName="MNewLaunchedBikes" TagPrefix="BW" %>
<%@ Register Src="~/UI/m/controls/DealersByBrand.ascx" TagName="DealersByBrand" TagPrefix="BW" %>
<%@ Register Src="~/UI/m/controls/usedBikeModel.ascx" TagName="usedBikeModel" TagPrefix="BW" %>
<!DOCTYPE html>
<html>
<head>
    <% 
        title = string.Format("{0} Bike Showrooms in India | {0} Bike Dealers in India - BikeWale - BikeWale", objMMV.MakeName, stateName);
        keywords = string.Format("{0} bike dealers, {0} bike showrooms, {0} dealers, {0} showrooms, {0} dealerships, dealerships, test drive, {0} dealer contact number", objMMV.MakeName);
        description = string.Format("Find the nearest {0} showroom in your city. There are {1} {0} showrooms in {2} cities in India. Get contact details, address, and direction of {0} dealers.", objMMV.MakeName, DealerCount, citiesCount);
        canonical = string.Format("https://www.bikewale.com/dealer-showrooms/{0}/", objMMV.MaskingName);
        AdPath = "/1017752/Bikewale_Mobile_Model";
        AdId = "1444028976556";
        Ad_320x50 = true;
        Ad_Bot_320x50 = true;
    %>

    <!-- #include file="/UI/includes/headscript_mobile_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/UI/m/css/dealer/location.css" />
    <script type="text/javascript">
        <!-- #include file="\UI\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/UI/includes/headBW_Mobile.aspx" -->
        <% if (Ad_320x50)
           { %>
        <section>
            <div>
                <!-- #include file="/UI/ads/Ad320x50_mobile.aspx" -->
            </div>
        </section>
        <% } %>

        <section>
            <div class="container margin-bottom10">
                <div class="bg-white">
                    <h1 class="box-shadow padding-15-20"><%=objMMV.MakeName %> Showrooms in India</h1>
                    <div class="box-shadow padding-15-20 font14 text-light-grey collapsible-content">
                        <p class="main-content">BikeWale recommends to buy your <%=objMMV.MakeName %> bike only from authorized <%=objMMV.MakeName %> showrooms. We bring you a list of <%= String.Format("{0} {1}",DealerCount,objMMV.MakeName) %><%=DealerCount>1?" showrooms":" showroom"%> present in <%=citiesCount%><%=citiesCount>1?" cities":" city"%> in India. The showroom locator tool will help you find the <%=objMMV.MakeName %> showroom in your city.</p>
                        <p class="more-content"> BikeWale works with more than 200+ bike showrooms in India to provide you a hassle-free bike buying experience. Get <%=objMMV.MakeName %> showroom’s address, contact details, EMI options for your nearest dealer.</p>
                        <a href="javascript:void(0)" class="read-more-target" rel="nofollow">...Read more</a>
					   </div>
                </div>
            </div>
        </section>

        <section>
            <div class="container bg-white margin-bottom10 box-shadow">
                <h2 class="padding-15-20 border-solid-bottom"><%= String.Format("{0} {1}",DealerCount,objMMV.MakeName) %> dealers in <%=citiesCount%> cities</h2>
                <div class="content-inner-block-20">
                    <div class="form-control-box">
                        <span class="bwmsprite search-icon-grey"></span>
                        <input type="text" class="form-control padding-right40" placeholder="Type to select city" id="getCityInput" />
                        <span class="fa fa-spinner fa-spin position-abt text-black"></span>
                    </div>
                    <ul id="location-list">
                        <% foreach (Bikewale.Entities.DealerLocator.StateCityEntity st in states.stateCityList)
                           { %>
                        <li class="item-state">
                            <a data-item-id="<%=st.Id %>" data-item-name="<%=st.Name %>" data-lat="<%=st.Lat %>" data-long="<%=st.Long %>" data-dealercount="<%=st.DealerCountState%>" href="javascript:void(0)" rel="nofollow" class="type-state" data-item-id="<%=st.Id %>"><%=st.Name %></a>
                            <ul class="location-list-city">
                                <% foreach (Bikewale.Entities.Location.DealerCityEntity stcity in st.Cities)
                                   { %>

                                <li>
                                    <a data-item-id="<%=stcity.Id %>" data-item-name="<%=stcity.CityName %>" data-lat="<%=stcity.Lattitude %>" data-long="<%=stcity.Longitude %>" data-link="<%=stcity.Link %>" data-dealercount="<%=stcity.DealersCount%>" title=" <%=objMMV.MakeName%> dealer showrooms in <%=stcity.CityName %>" href="/m/dealer-showrooms/<%=makeMaskingName %>/<%=stcity.CityMaskingName %>/"><%=stcity.CityName %> (<%=stcity.DealersCount %>)</a>
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
        <% if (ctrlDealersByBrand.FetchedRecordsCount > 0)
           { %>
        <section>
            <div class="margin-bottom10">
                <BW:DealersByBrand runat="server" ID="ctrlDealersByBrand" />
            </div>
        </section>
        <%} %>
        <% if (ctrlNewLaunchedBikes.FetchedRecordsCount > 0 || ctrlUpcomingBikes.FetchedRecordsCount > 0 || (ctrlusedBikeModel.FetchCount > 0))
           { %>
        <section>
            <div class="container bg-white margin-bottom10 box-shadow padding-top15">

                <% if (ctrlNewLaunchedBikes.FetchedRecordsCount > 0)
                   { %>
                <div class="carousel-heading-content">
                <div class="swiper-heading-left-grid inline-block">
                    <h2>New <%=objMMV.MakeName %> bike launches</h2>
                </div>
                <div class="swiper-heading-right-grid inline-block text-right">
                    <a href="/m/new-<%=makeMaskingName %>-bike-launches/" title="<%=objMMV.MakeName%> Bike Launches in India" class="btn view-all-target-btn">View all</a>
                </div>
                    <div class="clear"></div>
                    </div>
                <div class="swiper-container card-container">
                    <div class="swiper-wrapper">
                        <BW:MNewLaunchedBikes runat="server" ID="ctrlNewLaunchedBikes" />
                    </div>
                </div>
                <div class="margin-top20 margin-right20 margin-left20 border-solid-bottom"></div>
                <%} %>


                <% if (ctrlUpcomingBikes.FetchedRecordsCount > 0)
                   { %>
                <div class="carousel-heading-content padding-top15">
                <div class="swiper-heading-left-grid inline-block">
                    <h2>Upcoming <%=objMMV.MakeName %> bikes</h2>
                </div>
                <div class="swiper-heading-right-grid inline-block text-right">
                    <a href="/m/<%=makeMaskingName %>-bikes/upcoming/" title="Upcoming <%=objMMV.MakeName%> Bikes in India" class="btn view-all-target-btn">View all</a>
                </div>
                    <div class="clear"></div>
                    </div>
                <div class="swiper-container card-container">
                    <div class="swiper-wrapper">
                        <BW:MUpcomingBikes runat="server" ID="ctrlUpcomingBikes" />
                    </div>
                </div>
                <div class="margin-top20 margin-right20 margin-left20 border-solid-bottom"></div>
                <%} %>
                <% if (ctrlusedBikeModel.FetchCount > 0)
                   { %>
                <div class="padding-top15">
                <BW:usedBikeModel runat="server" ID="ctrlusedBikeModel" />
                    </div>
                <% } %>
            </div>
        </section>
        <%} %>
                
        <%--<ul id="listingUL" class="city-listing">
            <asp:Repeater ID="rptCity" runat="server">
                <ItemTemplate>
                    <li>
                        <a href="/m/<%=objMMV.MaskingName %>-bikes/dealers-in-<%#DataBinder.Eval(Container.DataItem,"cityMaskingName") %>/" data-item-id="<%#DataBinder.Eval(Container.DataItem,"cityId") %>"><%#DataBinder.Eval(Container.DataItem,"CityName") %> (<%#DataBinder.Eval(Container.DataItem,"DealersCount") %>)</a>
                    </li>

                </ItemTemplate>
            </asp:Repeater>
        </ul>--%>        

        <script type="text/javascript" src="<%= staticUrl  %>/UI/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/UI/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl  %>/UI/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/UI/includes/footerscript_mobile.aspx" -->
        <script type="text/javascript" src="<%= staticUrl  %>/UI/m/src/dealer/location.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/UI/includes/fontBW_Mobile.aspx" -->
    </form>
</body>
</html>
