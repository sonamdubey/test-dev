<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.DealersInState" EnableViewState="false" %>
<%@ Register TagPrefix="BW" TagName="MPopupWidget" Src="/m/controls/MPopupWidget.ascx" %>
<!DOCTYPE html>
<html>
<head>
     <% 
         description = string.Format("{0} bike dealers/showrooms in {1}. Find dealer information for more than {2} dealers in {3} cities. Dealer information includes full address, phone numbers, email, pin code etc.", objMMV.Make, stateName, DealerCount, citiesCount);
        keywords = string.Format("{0} bike dealers, {0} bike showrooms, {0} dealers, {0} showrooms, {0} dealerships, dealerships, test drive, {0} dealer contact number", objMMV.Make);
        title = string.Format("{0} Bike Dealers in {1} | {0} Bike Showrooms in {1} - BikeWale", objMMV.Make, stateName);
        canonical = string.Format("http://www.bikewale.com/{0}-bikes/dealers-in-{1}-state/", objMMV.MakeMappingName, stateMaskingName);
        AdPath = "/1017752/Bikewale_Mobile_Model";
        AdId = "1444028976556";
        Ad_320x50 = true;
        Ad_Bot_320x50 = true;
    %>

    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-dealersbylocation.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
</head>
<body class="bg-light-grey">
    <form runat="server">                  
        <header>
            <div class="header-fixed fixed">
                <div class="leftfloat header-back-btn">
                    <a href="javascript:history.back()"><span class="bwmsprite fa-arrow-back"></span></a>
                </div>
                <div class="leftfloat header-title text-bold text-white font18">Select city</div>
                <div class="clear"></div>
            </div>
        </header>
         <% if(Ad_320x50){ %>
                <section>            
                    <div>
                        <!-- #include file="/ads/Ad320x50_mobile.aspx" -->
                    </div>
                </section>
            <% } %>
        <section class="container margin-top10 margin-bottom30">
            <div class="grid-12">
                <div id="listingWrapper" class="box-shadow padding-top15">
                    <div id="listingHeader" class="padding-right20 padding-left20">
                        <h1 class="font16 text-pure-black margin-bottom10"><%=objMMV.Make %> bike dealers in <%= stateName %></h1>
                        <h2 class="font14 text-unbold text-xt-light-grey text-truncate padding-bottom15 margin-bottom20 border-solid-bottom"><%=DealerCount %> dealers across <%=citiesCount %> cities in <%= stateName %></h2>
                        <div class="form-control-box">
                            <span class="bwmsprite search-icon-grey"></span>
                            <input type="text" class="form-control padding-right40" placeholder="Type to select city" id="getCityInput" />
                            <span class="fa fa-spinner fa-spin position-abt text-black"></span>
                        </div>
                         <% if(dealerCity!=null && dealerCity.dealerStates!=null){ %>
                            <div class="font16 text-black selected-state-label"><%= dealerCity.dealerStates.StateName %></div>
                         <% } %>
                    </div>
                    <ul id="listingUL" class="city-listing">
                        <asp:Repeater ID="rptCity" runat="server">
                            <ItemTemplate>
                                <li>
                                    <a href="/m/<%=objMMV.MakeMappingName %>-bikes/dealers-in-<%#DataBinder.Eval(Container.DataItem,"cityMaskingName") %>/" data-item-id="<%#DataBinder.Eval(Container.DataItem,"cityId") %>"><%#DataBinder.Eval(Container.DataItem,"CityName") %> (<%#DataBinder.Eval(Container.DataItem,"DealersCount") %>)</a>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/dealersbylocation.js?<%= staticFileVersion %>"></script>
    </form>
</body>
</html>
