<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.DealersInState" EnableViewState="false" Trace="false" Debug="false" %>
<%@ Import Namespace="Bikewale.Common" %>

<!doctype html>
<html>
<head>
    <% 
        title = string.Format("{0} Bike Dealers in {1} | {0} Bike Showrooms in {1} - BikeWale", objMMV.Make, stateName);
        keywords = string.Format("{0} bike dealers, {0} bike showrooms, {0} dealers, {0} showrooms, {0} dealerships, dealerships, test drive, {0} dealer contact number", objMMV.Make);
        description = string.Format("{0} bike dealers/showrooms in {1}. Find dealer information for more than {2} dealers in {3} cities. Dealer information includes full address, phone numbers, email, pin code etc.", objMMV.Make,stateName, DealerCount, citiesCount);
        canonical = string.Format("http://www.bikewale.com/{0}-bikes/dealers-in-{1}-state/", objMMV.MakeMappingName, stateMaskingName);
        alternate = string.Format("http://www.bikewale.com/m/{0}-bikes/dealers-in-{1}-state/", objMMV.MakeMappingName, stateMaskingName);
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/dealersbylocation.css?<%= staticFileVersion%>" rel="stylesheet" type="text/css">
    <script src="http://maps.googleapis.com/maps/api/js?key=<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey %>&libraries=places"></script>
    <script type="text/javascript" src="/src/new/markerwithlabel.js"></script>
</head>
<body class="bg-light-grey padding-top50">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section>
            <div class="opacity0 grid-12 padding-right20 padding-left20">
                <div class="breadcrumb">
                    <ul>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <a href="/" itemprop="url">
                                <span itemprop="title">Home</span>
                            </a>
                        </li>
                        <li><span class="bwsprite fa fa-angle-right margin-right10"></span>Dealer Locator</li>
                    </ul>
                    <div class="clear"></div>
                </div>
            </div>
        </section>

        <section>
            <div class="grid-12 alpha omega">
                <div id="listingSidebar" class="bg-white position-abt pos-right0">
                    <div id="listingSidebarHeading" class="padding-top15 padding-right20 padding-left20">
                        <div class="margin-bottom20">
                            <h1 id="sidebarHeader" class="font16 margin-bottom10"><%=objMMV.Make %> bike dealers in <%= stateName %></h1>
                            <h2 class="text-unbold font14 text-xt-light-grey border-solid-bottom padding-bottom15"><%=DealerCount %> dealer<%= DealerCount > 1? "s": "" %> across <%=citiesCount %> cit<%= citiesCount > 1? "ies": "y" %> in <%= stateName %></h2>
                        </div>
                        <div class="form-control-box">
                            <span class="bwsprite search-icon-grey"></span>
                            <input type="text" class="form-control padding-right40" placeholder="Type to search city" id="getCityInput" />
                            <span class="fa fa-spinner fa-spin position-abt text-black"></span>
                            <span class="bwsprite error-icon errorIcon"></span>
                            <div class="bw-blackbg-tooltip errorText"></div>
                        </div>
                        <% if(dealerCity!=null && dealerCity.dealerStates!=null){ %>
                        <div class="padding-top10 padding-bottom10">
                            <a href="/new/<%= objMMV.MakeMappingName %>-dealers/" class="inline-block"><span class="bwsprite back-icon"></span></a>
                            <span class="font16 text-black inline-block"><%= dealerCity.dealerStates.StateName %></span>
                        </div>
                        <% } %>
                    </div>
                    <ul id="listingSidebarList" class="city-sidebar-list">
                        <asp:Repeater ID="rptCity" runat="server">
                            <ItemTemplate>
                                <li>
                                   <a href="/<%=objMMV.MakeMappingName %>-bikes/dealers-in-<%#DataBinder.Eval(Container.DataItem,"cityMaskingName") %>/" data-item-id="<%#DataBinder.Eval(Container.DataItem,"cityId") %>"><%#DataBinder.Eval(Container.DataItem,"CityName") %> (<%#DataBinder.Eval(Container.DataItem,"DealersCount") %>)</a>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <section class="bg-white">
            <div class="grid-12 alpha omega">
                <div class="dealer-map-wrapper">
                    <div id="dealersMapWrapper" style="position: fixed; top: 50px; width: 100%; height: 530px;">
                        <div id="dealersMap" style="width: 100%; height: 530px;">
                        </div>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript">
            var dealersByCity = true;
            var cityArr = JSON.parse('<%= cityArr %>');
            var stateLat = '<%= (dealerCity != null && dealerCity.dealerStates != null) ? dealerCity.dealerStates.StateLatitude : string.Empty %>';
            var stateLong = '<%= (dealerCity != null && dealerCity.dealerStates != null) ? dealerCity.dealerStates.StateLongitude : string.Empty %>';
        </script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/dealersbylocation.js?<%= staticFileVersion %>"></script>
    </form>
</body>
</html>