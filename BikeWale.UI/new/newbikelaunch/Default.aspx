﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.NewLaunchBikes" Trace="false" Async="true" %>

<%@ Register TagPrefix="uc" TagName="UpcomingBikes" Src="~/controls/UpcomingBikesMin.ascx" %>
<%@ Register TagPrefix="BikeWale" TagName="RepeaterPager" Src="/controls/LinkPagerControl.ascx" %>
<%@ Register TagPrefix="news" TagName="NewsMin" Src="~/controls/NewsMin.ascx" %>
<%@ Import Namespace="Bikewale.Common" %>
<%
    title = "New Bikes Launches in " + year + " - BikeWale";
    keywords = "new bikes 2014, new bike launches in " + year + ", just launched bikes, new bike arrivals, bikes just got launched";
    description = "List of all new bikes launched in India in " + year + " (last eight months)";
    canonical = "https://www.bikewale.com/new-bikes-launches/";
    prevPageUrl = prevPage;
    nextPageUrl = nextPage;
    alternate = "https://www.bikewale.com/m/new-bikes-launches/";
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_New_";
    isAd300x250Shown = false;
    isAd300x250_BTFShown = false;
%>

<!-- #include file="/includes/headNew.aspx" -->
<div class="container_12">
    <div class="grid_12">
        <ul class="breadcrumb">
            <li>You are here: </li>
            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                <a itemprop="url" href="/"><span itemprop="title">Home</span></a>
            </li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                <a itemprop="url" href="/new/"><span itemprop="title">New</span></a>
            </li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><strong>New Bike Launches in <%= year %></strong></li>
        </ul>
        <div class="clear"></div>
    </div>
    <div class="grid_8 margin-top10">
        <span id="spnError" runat="server"></span>
        <h1>New Bikes Launches</h1>
        <div id="alertObj" runat="server"></div>
        <asp:repeater id="rptLaunched" runat="server">						           
                    <ItemTemplate>
                        <div class="margin-bottom15">
                            <div class="grid_5 omega">
                                <div class="anchor-title">
                                    <a href="/<%# DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName") %>-bikes/<%#DataBinder.Eval(Container.DataItem,"MaskingName") %>/"><%# DataBinder.Eval(Container.DataItem,"MakeBase.MakeName") + " " + DataBinder.Eval(Container.DataItem,"ModelName") %></a>
			                    </div>
                                <div style="margin:5px 0;"><b>Lauched On : </b><%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"LaunchDate")).ToString("dd-MMM-yyyy") %></div>
			                    <div style="margin:5px 0;" >
                                    <div><b>Price : </b> Rs. <%#CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"MinPrice").ToString()) %></div>
                                    <span class="ex-showroom">Ex-showroom, <%= ConfigurationManager.AppSettings["defaultName"].ToString() %></span>
                                    <a class="buttons getquotation btn-xs" data-pqSourceId="<%= (int)Bikewale.Entities.PriceQuote.PQSourceEnum.Desktop_NewLaunchLanding %>" title="Check <%# DataBinder.Eval(Container.DataItem,"MakeBase.MakeName") + " " + DataBinder.Eval(Container.DataItem,"ModelName") %> On-road Price" href="/pricequote/default.aspx?model=<%#DataBinder.Eval(Container.DataItem,"ModelId") %>" style="margin-top:-25px; margin-right:25px; float:right;<%# (DataBinder.Eval(Container.DataItem,"MinPrice").ToString()!="0")?"":"display:none" %>" data-modelId="<%#DataBinder.Eval(Container.DataItem,"ModelId") %>">Check On-Road Price</a>
                                    </div>
			                    <div class="margin-top10 <%#DataBinder.Eval(Container.DataItem,"ReviewCount").ToString() == "0" ? "hide" : "" %>">
                                    <%# Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble(DataBinder.Eval(Container.DataItem,"ReviewRate"))) %>&nbsp;&nbsp;&nbsp;
                                    <a href="/<%# DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName") %>-bikes/<%#DataBinder.Eval(Container.DataItem,"MaskingName") %>/user-reviews/" ><%#DataBinder.Eval(Container.DataItem,"ReviewCount") %> reviews</a>
                                </div>
                                <div class="margin-top10 <%#DataBinder.Eval(Container.DataItem,"ReviewCount").ToString() == "0" ? "" : "hide" %>">
                                    Not rated yet,  <a href="/content/userreviews/writereviews.aspx?bikem=<%# DataBinder.Eval(Container.DataItem,"ModelId") %>">Be the first one to write a review</a>
                                </div>
                                <div class="margin-top15 margin-bottom10">
                                    <a title="View <%# DataBinder.Eval(Container.DataItem,"MakeBase.MakeName") + " " + DataBinder.Eval(Container.DataItem,"ModelName") %> Photos" href="/<%# DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName") %>-bikes/<%#DataBinder.Eval(Container.DataItem,"MaskingName") %>/photos/">Photos</a>
                                </div>
                            </div>
                            <div class="right-float grid_3 alpha omega">
                                <a href="/<%# DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName") %>-bikes/<%#DataBinder.Eval(Container.DataItem,"MaskingName") %>/">
                                    <img class="redirect-rt margin-top5 img-border" alt="<%# DataBinder.Eval(Container.DataItem,"MakeBase.MakeName") + " " + DataBinder.Eval(Container.DataItem,"ModelName") %>" title="<%# DataBinder.Eval(Container.DataItem,"MakeBase.MakeName") + " " + DataBinder.Eval(Container.DataItem,"ModelName") %>" src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(), DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._210x118) %>" />
                                </a>
                            </div>
                        </div><div class="clear"></div>
                        <div class="sept-dashed"></div>
                    </ItemTemplate>
	            </asp:repeater>
        <BikeWale:RepeaterPager ID="repeaterPager" runat="server" />
    </div>
    <div class="grid_4">
        <div class="grid_4 alpha margin-top15">
            <uc:UpcomingBikes ID="ctrl_UpcomingBikes" runat="server" HeaderText="Upcoming Bikes" TopRecords="2" ControlWidth="grid_2" />
            <div class="clear"></div>
        </div>
        <div class="clear"></div>
        <div>
            <news:NewsMin ID="ctrl_News" runat="server" />
            <div class="clear"></div>
        </div>
    </div>
</div>
<!-- #include file="/includes/footerInner.aspx" -->
