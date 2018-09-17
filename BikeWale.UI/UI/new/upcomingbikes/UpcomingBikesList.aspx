<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.UpcomingBikesList" Trace="false" EnableViewState="false" Debug="false" %>

<%@ Register TagPrefix="BikeWale" TagName="RepeaterPager" Src="~/UI/controls/RepeaterPager.ascx" %>
<%@ Register TagPrefix="BikeWale" TagName="UpcomingBikeSearch" Src="~/UI/controls/UpcomingBikeSearch.ascx" %>
<%@ Register TagPrefix="NBL" TagName="NewBikeLaunches" Src="/UI/controls/NewBikeLaunches.ascx" %>
<%@ Import Namespace="Bikewale.Common" %>
<%
    if (meta != null)
    {
        title = meta.Title;
        keywords = meta.Keywords;
        description = meta.Description;
        canonical = meta.CanonicalUrl;
        alternate = meta.AlternateUrl;
    }
    prevPageUrl = prevUrl;
    nextPageUrl = nextUrl;
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_New_";
%>

<!-- #include file="/UI/includes/headNew.aspx" -->
<div class="container_12">
    <div class="grid_12">
        <ul class="breadcrumb">
            <li>You are here: </li>
            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                <a itemprop="url" href="/"><span itemprop="title">Home</span></a>
            </li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                <a itemprop="url" href="/new-bikes-in-india/"><span itemprop="title">New</span></a>
            </li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><strong>Upcoming Bikes</strong></li>
        </ul>
        <div class="clear"></div>
    </div>
    <div class="grid_8 margin-top10">
        <span id="spnError" runat="server"></span>
        <h1><%= pageTitle %> </h1><span>Latest information on expected new bike launches in India.</span>
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
                            <div style="margin:5px 0;"><b>When to expect:</b> <%# String.IsNullOrEmpty(Convert.ToString(DataBinder.Eval(Container.DataItem, "ExpectedLaunch"))) ? "N/A" : Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "ExpectedLaunch")).ToString("MMMM yyyy") %></div>
			                <div style="margin:5px 0;"><b>Estimated Price:</b> Rs. <%# GetFormattedPrice(DataBinder.Eval(Container.DataItem, "EstimatedPriceMin").ToString()) +"-" + GetFormattedPrice(DataBinder.Eval(Container.DataItem, "EstimatedPriceMax").ToString()) %></div>
			                <div class="desc <%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Description").ToString()) ? "hide" : "" %>"><%# DataBinder.Eval(Container.DataItem, "Description").ToString().Length >= 300 ? DataBinder.Eval(Container.DataItem, "Description").ToString().Substring(0,300) : DataBinder.Eval(Container.DataItem, "Description").ToString() %>...</div>
			                <div class="margin-top10 readmore"><a href="/<%# DataBinder.Eval(Container.DataItem,"MakeMaskingName").ToString()%>-bikes/<%#DataBinder.Eval(Container.DataItem,"ModelMaskingName").ToString() %>/">Read More</a></div>                            
                        </div><div class="clear"></div>
                        <div class="sept-dashed"></div>
                    </ItemTemplate>
	            </asp:repeater>
        </BikeWale:RepeaterPager>
    </div>
    <div class="grid_4">
        <div class="margin-top15">
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/UI/ads/Ad300x250.aspx" -->
        </div>
        <div class="grey-bg content-block border-radius5 padding-bottom20 margin-top15">
            <NBL:NewBikeLaunches ID="ctrl_NewBikeLaunches" TopCount="3" runat="server" />
            <div class="clear"></div>
        </div>
        <div class="margin-top15">
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/UI/ads/Ad300x250BTF.aspx" -->
        </div>
    </div>
</div>
<!-- #include file="/UI/includes/footerInner.aspx" -->
