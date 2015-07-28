<%@ Page Language="C#" Inherits="Bikewale.New.Default" AutoEventWireUp="false" Trace="false" Debug="false" EnableEventValidation="false" async="true" %>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Register TagPrefix="nl" TagName="NewBikeLaunches" Src="~/controls/NewBikeLaunches.ascx" %>
<%@ Register TagPrefix="news" TagName="NewsMin" Src="~/controls/NewsMin.ascx" %>
<%@ Register TagPrefix="tips" TagName="TipsAdvicesMin" Src="~/controls/TipsAdvicesMin.ascx" %>
<%@ Register TagPrefix="BikeWale" TagName="FeaturedBikes" Src="~/controls/FeaturedBike.ascx" %>
<%@ Register TagPrefix="uc" TagName="UpcomingBikes" Src="~/controls/UpcomingBikesMin.ascx" %>
<%@ Register TagPrefix="uc" TagName="CompareCarsMin" Src="~/controls/ComparisonMin.ascx" %>
<%@ Register TagPrefix="uc" TagName="BrowseCar" Src="~/controls/BrowseBikeByVersion.ascx" %>
<%@ Register TagPrefix="uc" TagName="BrowseUserReviews" Src="~/controls/BrowseUserReviews.ascx" %>
<%@ Register TagPrefix="BP" TagName="InstantBikePrice" Src="/controls/instantbikeprice.ascx" %>
<%@ Register TagPrefix="LD" TagName="LocateDealer" Src="/controls/locatedealer.ascx" %>
<%@ Register TagPrefix="RT" TagName="RoadTest" Src="/controls/RoadTest.ascx" %>
<%@ Register TagPrefix="NBL" TagName="NewBikeLaunches" Src="/controls/NewBikeLaunches.ascx" %>
<%
    title = "New Bikes - Bikes Reviews, Photos, Specs, Features, Tips & Advices - BikeWale";
    keywords = "new bikes, new bikes prices, new bikes comparisons, bikes dealers, on-road price, bikes research, bikes india, Indian bikes, bike reviews, bike photos, specs, features, tips & advices";
    description = "New bikes in India. Search for the right new bikes for you, know accurate on-road price and discounts. Compare new bikes and find dealers.";  
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_New_";
    canonical="http://www.bikewale.com/new/";
    alternate="http://www.bikewale.com/m/new/";
%>
<!-- #include file="/includes/headNew.aspx" -->

<%@ Register TagPrefix="PW" TagName="PopupWidget" Src="/controls/PopupWidget.ascx" %>
<PW:PopupWidget runat="server" ID="PopupWidget" />

<form runat="server">
    <div class="container_12">
        <div class="grid_12">
            <ul class="breadcrumb">
                <li>You are here: </li>
                <li><a href="/">Home</a></li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li class="current"><strong>New Bikes</strong></li>
            </ul><div class="clear"></div>
        </div>
        <div class="grid_8 margin-top10">
            <h1>New Bikes <span>India's largest and most accurate bike database</span></h1>	        
		    <div class="grid_5 alpha margin-top15">			      
				<h2>By Make</h2>
				<asp:DataList ID="dltMakes" runat="server" Width="100%" RepeatColumns="3" RepeatDirection="Vertical" CellPadding="3">
					<itemtemplate><a href="/<%#DataBinder.Eval(Container.DataItem,"MaskingName").ToString() %>-bikes/"><%# DataBinder.Eval(Container.DataItem,"Name") %></a></itemtemplate>
				</asp:DataList>
            </div>
            <div class="grid_3 omega margin-top15">
                <h2>By Body Style</h2>
                <ul class="ul-normal">
                    <li><a href="/new/search.aspx#bs=1">Cruiser</a></li>
                    <li><a href="/new/search.aspx#bs=2">Fully faired</a></li>
                    <li><a href="/new/search.aspx#bs=3">Naked</a></li>
                    <li><a href="/new/search.aspx#bs=4">Semi-faired</a></li>
                    <li><a href="/new/search.aspx#bs=5">Scooter</a></li>
                </ul>
            </div><div class="clear"></div>                    
            <div class="grid_8 margin-top10 alpha omega grey-bg">
                <div class="content-block"><uc:BrowseCar ID="ucBrowseCar" runat="server" /></div>                
            </div>           
            <div class="clear"></div>
            <div class="margin-top15">
                <h2>Featured Bikes</h2>
                <BikeWale:FeaturedBikes ID="ctrl_FeaturedBike" runat="server" TopRecords="4" ControlWidth="grid_2" />
            </div>
            <div class="clear"></div>
            
            <div class="grey-bg content-block"><uc:BrowseUserReviews ID="ucUserReviews" runat="server" /></div>

            <div class="grid_4 alpha margin-top15">
                <uc:UpcomingBikes ID="ucUpcoming" runat="server" HeaderText="Upcoming Bikes" TopRecords="2" ControlWidth="grid_2" />
            </div>
            <div class="grid_4 omega margin-top15">
                <RT:RoadTest id="ucRoadTestMin" runat="server" TopRecords="2" HeaderText="Road Test"></RT:RoadTest>
            </div><div class="clear"></div>
                                        
            <div class="grid_8 alpha omega" style="border:1px solid #E2E2E2;">
                <uc:CompareCarsMin ID="ucCompareCarsMin" runat="server" TopRecords="4" />
            </div>
           <%-- <div class="grid_4 alpha omega grey-bg margin-left10 padding-top10">
                <NBL:NewBikeLaunches ID="ctrl_NewBikeLaunches" runat="server" />
                <div class="clear"></div>
            </div>--%>
        </div>
        <div class="grid_4">
            <div>
                <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
                <!-- #include file="/ads/Ad300x250.aspx" -->
            </div>
            <div class="light-grey-bg content-block border-radius5 padding-bottom20 margin-top15">
                <BP:InstantBikePrice runat="server" ID="InstantBikePrice" />
            </div>
            <div class="light-grey-bg content-block border-radius5 margin-top10 padding-bottom20">
                <LD:LocateDealer runat="server" id="LocateDealer" />
            </div>            
            <div class="grey-bg content-block border-radius5 padding-bottom20 margin-top15">
                <NBL:NewBikeLaunches ID="ctrl_NewBikeLaunches" TopCount="3" runat="server" />
                <div class="clear"></div>
            </div>
            <div>
                <news:NewsMin id="newsMin" runat="server" posts="2"></news:NewsMin>
            </div>
            <div class="margin-top15">
                <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250_BTF -->
                <!-- #include file="/ads/Ad300x250BTF.aspx" -->
            </div>
        </div>                                    
     </div>       
</form>
<!-- #include file="/includes/footerInner.aspx" -->
