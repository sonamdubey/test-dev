<%@ Import Namespace="Carwale.UI.NewCars.RecommendCars" %>
<%@ Register TagPrefix="uc" TagName="BlackBerryDownload" Src="/m/Controls/BlackBerryDownload.ascx" %>
<uc:BlackBerryDownload ID="ucBlackBerryDownload" runat="server" />

<div id="divOverlay" style="display:none;"></div>
<!--Header starts here-->
<div class="header">
    <div class="menu-div floatleft">
        <a href="#menu-panel">
            <span class="cw-m-sprite menu"></span>
        </a>
    </div>
    <div class="logo-div">
        <a href="/m/" class="cw-m-sprite logo"></a>
    </div>
    <a href="#"  id="citychange" class="cw-m-sprite city-position-inactive"></a>
    <div class="select-city-text hide" id="tooltip">
        Select City
    </div>
    <div class="clear"></div>
</div>      
<!--Header ends here-->
<div id="cityPopUp"></div>
<!--Add banner starts here-->
<script language="c#" runat="server">private bool IsShowAd = true;</script>
<%if(IsShowAd){ %>
<div class="banner-div">
    <div id="divAdBar" class="ad-div">
        <!-- Mobile_CarWale/Mobile_ROS_320x50 -->
       <%-- <%if(IsNotHomePage){%>--%>
            <!-- CarWale_Mobile_ROS_320x50 -->
                <div id='div-gpt-ad-1419227721763-0' style='width:320px;'>
                    <script type='text/javascript'>
                        googletag.cmd.push(function () { googletag.display('div-gpt-ad-1419227721763-0'); });
                    </script>
                </div>
        <%--<%} %>--%>
        <%--<%else{ %>
            <!-- CarWale_Mobile_HP_HP_320x50 -->
                <div id='div-gpt-ad-1400919004715-0' style='width:320px; height:50px;'>
                    <script type='text/javascript'>
                        googletag.cmd.push(function () { googletag.display('div-gpt-ad-1400919004715-0'); });
                    </script>
                </div>
        <%} %>--%>
    </div>
</div>
<%} %>
<!--Add banner ends here-->
        
<!--Menu panel starts here-->
<div data-role="panel" id="menu-panel" data-position="left" data-display="overlay" data-theme="a">
    <ul>
        <li><a href="/m/" >Home</a></li>
        <li><a href="/m/new/" >New Cars</a></li>
        <li><a href="/m/used/" >Used Cars</a></li>
        <li><a href="/m/comparecars/" >Compare Cars</a></li>
        <li><a href="/quotation/landing/" >On Road Price</a></li>
        <li><a href="/m/research/locatedealerpopup.aspx" onclick="dataLayer.push({ event: 'locate_dealer_section', cat: 'global_left_panel', act: 'locate_dealer_link'})" >Locate Dealer</a></li>
        <li><a href="/m/upcoming-cars/" >Upcoming Cars</a></li>
        <li><a href="/m/offers/" >Offers</a></li>
        <li><a href="/used/sell/" >Sell Car</a></li>
        <li><a href="/m/news/" >News</a></li>       
        <li><a href="/m/forums/" >Forums</a></li>
        <li><a href="/m/insurance/" >Insurance</a></li>
    </ul>
</div>
<!--Menu panel ends here-->
