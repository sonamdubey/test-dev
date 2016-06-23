<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.ListNewBikeDealersByState_New" Trace="false" Debug="false" %>
<%@ Register TagPrefix="NBL" TagName="NewBikeLaunches" Src="/controls/NewBikeLaunches.ascx" %>
<%@ Register TagPrefix="TIP" TagName="TipsAdvicesMin" Src="/controls/TipsAdvicesMin.ascx" %>
<%@ Register TagPrefix="FM" TagName="ForumsMin" Src="/controls/forumsmin.ascx" %>
<%@ Import Namespace="Bikewale.Common" %>

<!doctype html>
<html>
<head> 
    <% 
        description = objMMV.Make + " bike dealers/showrooms in India. Find new bike dealer information for more than 200 cities. Dealer information includes full address, phone numbers, email, pin code etc.";
        keywords = objMMV.Make + " bike dealers, " + objMMV.Make + " bike showrooms," + objMMV.Make + " dealers, " + objMMV.Make + " showrooms, " + objMMV.Make + " dealerships, dealerships, test drive";
        title = objMMV.Make + " Bike Dealers | " + objMMV.Make + " Bike Showrooms in India - BikeWale";
        canonical = "http://www.bikewale.com/new/" + objMMV.MakeMappingName + "-dealers/";
        AdId = "1395986297721";
        AdPath = "/1017752/BikeWale_New_";
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/dealersbylocation.css?<%= staticFileVersion%>" rel="stylesheet" type="text/css">
    <script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDjG8tpNdQI86DH__-woOokTaknrDQkMC8" type="text/javascript"></script>    
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
                            <h1 id="sidebarHeader" class="font16 margin-bottom10">Royal Enfield dealers in India</h1>
                            <h2 class="text-unbold font14 text-xt-light-grey border-solid-bottom padding-bottom15">225 dealers across 29 states</h2>
                        </div>
                        <div class="form-control-box">
                            <span class="bwsprite search-icon-grey"></span>
                            <input type="text" class="form-control padding-right40" placeholder="Type to search state" id="getStateInput" />
                            <span class="fa fa-spinner fa-spin position-abt text-black"></span>
                        </div>
                    </div>
                    <% if (1 == 1) { } %>
                    <ul id="listingSidebarList">
                        <asp:Repeater ID="rptState" runat="server">
                            <ItemTemplate>
                                <li>
                                    <a href="andhra-pradesh-link" data-item-id="1" >Andhra Pradesh</a>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                       <%-- <li>
                            <a href="andhra-pradesh-link" data-state-id="1" data-state-lat="16.5000" data-state-long="80.6400">Andhra Pradesh</a>
                        </li>
                        <li>
                            <a href="arunachal-pradesh-link" data-state-id="2" data-state-lat="27.0600" data-state-long="93.3700">Arunachal Pradesh</a>
                        </li>
                        <li>
                            <a href="gujarat-link" data-state-id="3" data-state-lat="30.7500" data-state-long="76.7800">Gujarat</a>
                        </li>
                        <li>
                            <a href="goa-link" data-state-id="4" data-state-lat="15.4989" data-state-long="73.8278">Goa</a>
                        </li>
                        <li>
                            <a href="maharashtra-link" data-state-id="5" data-state-lat="18.9600" data-state-long="72.8200">Maharashtra</a>
                        </li>--%>
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
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/dealersbylocation.js?<%= staticFileVersion %>"></script>

       
    
    </form>
</body>
</html>