﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.UpcomingBikesList" Trace="false" Debug="true" %>
<%@ Register TagPrefix="BikeWale" TagName="RepeaterPager" Src="~/controls/RepeaterPager.ascx" %>
<%@ Register TagPrefix="BikeWale" TagName="UpcomingBikeSearch" Src="~/controls/UpcomingBikeSearch.ascx" %>
<%@ Register TagPrefix="NBL" TagName="NewBikeLaunches" Src="/controls/NewBikeLaunches.ascx" %>
<%@ Import Namespace="Bikewale.Common" %>
<%
    title = "Upcoming Bikes in India - Expected Launches in 2012";
    keywords = "Find out upcoming new bikes in 2012 in India. From small to super-luxury, from announced to highly speculated models, from near future to end of year, know about every upcoming bike launch in India this year.";
    description = "upcoming bikes, new upcoming launches, upcoming bike launches, upcoming models, future bikes, future bike launches, 2012 bikes, speculated launches, futuristic models";
    canonical= "http://www.bikewale.com/upcoming-bikes/" ;
    prevPageUrl     = prevUrl;
    nextPageUrl     = nextUrl;
    alternate = "http://www.bikewale.com/m/upcoming-bikes/";
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_New_";
%>
<!-- #include file="/includes/headNew.aspx" -->
<%@ Register TagPrefix="PW" TagName="PopupWidget" Src="/controls/PopupWidget.ascx" %>
<PW:PopupWidget runat="server" ID="PopupWidget" />

<div class="container_12">
    <div class="grid_12">
        <ul class="breadcrumb">
            <li>You are here: </li>
            <li><a href="/">Home</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li><a href="/new/">New</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><strong>Upcoming Bikes</strong></li>
        </ul><div class="clear"></div>
    </div>
    <div class="grid_8 margin-top10">        
        <span id="spnError" runat="server"></span>
        <h1>Upcoming Bikes in India <span>Latest information on expected new bike launches in India.</span></h1>                   
        <BikeWale:UpcomingBikeSearch ID="UpcomingBikeSearch" runat="server" />           
        <div id="alertObj" runat="server"></div>
            <BikeWale:RepeaterPager ID="rpgUpcomingBikes" PageSize="10" PagerPageSize="10" runat="server">
                <asp:repeater id="rptLaunches" runat="server">						           
                    <ItemTemplate>
                        <div class="margin-bottom15">
                            <div class="right-float">
                                <%--<img class="redirect-rt margin-top5 img-border" alt="<%# DataBinder.Eval(Container.DataItem, "MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelName").ToString() %>" title="<%# DataBinder.Eval(Container.DataItem, "MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelName").ToString() %>" src="<%# Bikewale.Common.ImagingFunctions.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "LargePicImagePath").ToString(), DataBinder.Eval(Container.DataItem, "HostURL").ToString()) %>" />--%>
                                <img class="redirect-rt margin-top5 img-border" alt="<%# DataBinder.Eval(Container.DataItem, "MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelName").ToString() %>" title="<%# DataBinder.Eval(Container.DataItem, "MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelName").ToString() %>" src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(), DataBinder.Eval(Container.DataItem, "HostURL").ToString(),Bikewale.Utility.ImageSize._210x118) %>" />
                            </div>
                            <div class="anchor-title">
                                <span><%#DataBinder.Eval(Container.DataItem, "RowN").ToString() %>. </span><a href="/<%# DataBinder.Eval(Container.DataItem,"MakeMaskingName").ToString()%>-bikes/<%# DataBinder.Eval(Container.DataItem,"ModelMaskingName").ToString() %>/"><%# DataBinder.Eval(Container.DataItem, "MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelName").ToString() %></a>
			                </div>
                            <div style="margin:5px 0;"><b>When to expect:</b> <%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "ExpectedLaunch").ToString()) ? "N/A" : DataBinder.Eval(Container.DataItem, "ExpectedLaunch") %></div>
			                <div style="margin:5px 0;"><b>Estimated Price:</b> Rs. <%# GetFormattedPrice(DataBinder.Eval(Container.DataItem, "EstimatedPriceMin").ToString()) +"-" + GetFormattedPrice(DataBinder.Eval(Container.DataItem, "EstimatedPriceMax").ToString()) %></div>
			                <div class="desc <%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Description").ToString()) ? "hide" : "" %>"><%# DataBinder.Eval(Container.DataItem, "Description").ToString().Length >= 300 ? DataBinder.Eval(Container.DataItem, "Description").ToString().Substring(0,300) : DataBinder.Eval(Container.DataItem, "Description").ToString() %>...</div>
			                <div class="margin-top10 readmore"><a href="/<%# DataBinder.Eval(Container.DataItem,"MakeMaskingName").ToString()%>-bikes/<%#DataBinder.Eval(Container.DataItem,"ModelMaskingName").ToString() %>/">Read More</a></div>                            
                        </div><div class="clear"></div>
                        <div class="sept-dashed"></div>
                    </ItemTemplate>
	            </asp:Repeater>
            </BikeWale:RepeaterPager>
        </div>
    <div class="grid_4">
        <div class="margin-top15">
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/ads/Ad300x250.aspx" -->
        </div>
        <div class="grey-bg content-block border-radius5 padding-bottom20 margin-top15">
            <NBL:NewBikeLaunches ID="ctrl_NewBikeLaunches" TopCount="3" runat="server" />                    
            <div class="clear"></div>
        </div>
        <div class="margin-top15">
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/ads/Ad300x250BTF.aspx" -->
        </div>
    </div>
</div>
<!-- #include file="/includes/footerInner.aspx" -->
