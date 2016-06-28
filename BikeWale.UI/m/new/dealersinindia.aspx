<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.DealersInIndia" EnableViewState="false" %>
<%@ Register TagPrefix="BW" TagName="MPopupWidget" Src="/m/controls/MPopupWidget.ascx" %>
<!DOCTYPE html>
<html>
<head>
    <% 
        description = objMMV.Make + " bike dealers/showrooms in India. Find new bike dealer information for more than 200 cities. Dealer information includes full address, phone numbers, email, pin code etc.";
        keywords = objMMV.Make + " bike dealers, " + objMMV.Make + " bike showrooms," + objMMV.Make + " dealers, " + objMMV.Make + " showrooms, " + objMMV.Make + " dealerships, dealerships, test drive";
        title = objMMV.Make + " Bike Dealers | " + objMMV.Make + " Bike Showrooms in India - BikeWale";
        canonical = "http://www.bikewale.com/new/" + objMMV.MakeMappingName + "-dealers/";
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
                <div class="leftfloat header-title text-bold text-white font18">Select state</div>
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
                        <h1 class="font16 text-pure-black margin-bottom10"><%=objMMV.Make %> bike dealers in India</h1>
                        <h2 class="font14 text-unbold text-xt-light-grey text-truncate padding-bottom15 margin-bottom20 border-solid-bottom"><%=countryCount %> dealers across <%=stateCount %> states</h2>
                        <div class="form-control-box">
                            <span class="bwmsprite search-icon-grey"></span>
                            <input type="text" class="form-control padding-right40" placeholder="Type to select state" id="getStateInput" />
                            <span class="fa fa-spinner fa-spin position-abt text-black"></span>
                        </div>
                    </div>
                    <ul id="listingUL" class="state-listing">
                        <asp:Repeater ID="rptState" runat="server">
                            <ItemTemplate>
                                <li>
                                    <a href="/m/<%=objMMV.MakeMappingName %>-bikes/dealers-in-<%#DataBinder.Eval(Container.DataItem,"stateMaskingName") %>-state/" data-item-id="<%#DataBinder.Eval(Container.DataItem,"stateId") %>"><%#DataBinder.Eval(Container.DataItem,"StateName") %></a>                                    
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
