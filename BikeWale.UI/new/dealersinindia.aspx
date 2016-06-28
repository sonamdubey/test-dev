<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.DealersInIndia" Trace="false" Debug="false" %>
<%@ Import Namespace="Bikewale.Common" %>

<!doctype html>
<html>
<head> 
    <% 
        title = string.Format("{0} Bike Dealers in India | {0} Bike Showrooms in India - BikeWale", objMMV.MakeName);
        keywords = string.Format("{0} bike dealers, {0} bike showrooms, {0} dealers, {0} showrooms, {0} dealerships, dealerships, test drive, {0} dealer contact number", objMMV.MakeName);
        description = string.Format("{0} bike dealers/showrooms in India. Find dealer information for more than {1} dealers in {2} states. Dealer information includes full address, phone numbers, email, pin code etc.", objMMV.MakeName, countryCount, stateCount);
        canonical = "http://www.bikewale.com/new/" + objMMV.MaskingName + "-dealers/";
        isAd970x90Shown = false;
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
                            <h1 id="sidebarHeader" class="font16 margin-bottom10"><%=objMMV.MakeName %> bike dealers in India</h1>
                            <h2 class="text-unbold font14 text-xt-light-grey border-solid-bottom padding-bottom15"><%=countryCount %> dealer<%= countryCount > 1? "s": "" %> across <%=stateCount %> state<%= stateCount > 1? "s": "" %></h2>
                        </div>
                        <div class="form-control-box">
                            <span class="bwsprite search-icon-grey"></span>
                            <input type="text" class="form-control padding-right40" placeholder="Type to search state" id="getStateInput" />
                            <span class="fa fa-spinner fa-spin position-abt text-black"></span>
                            <span class="bwsprite error-icon errorIcon"></span>
                            <div class="bw-blackbg-tooltip errorText"></div>
                        </div>
                    </div>
                      <ul id="listingSidebarList" class="state-sidebar-list" >
                        <asp:Repeater ID="rptState" runat="server">
                            <ItemTemplate>
                                <li>
                                   <a href="/<%=objMMV.MaskingName %>-bikes/dealers-in-<%#DataBinder.Eval(Container.DataItem,"stateMaskingName") %>-state/" data-item-id="<%#DataBinder.Eval(Container.DataItem,"stateId") %>"><%#DataBinder.Eval(Container.DataItem,"StateName") %></a>
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
            var stateArr = JSON.parse('<%= stateArray %>');
        </script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/dealersbylocation.js?<%= staticFileVersion %>"></script>
    </form>
</body>
</html>