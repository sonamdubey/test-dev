<%@ Page Language="C#" Inherits="Bikewale.New.Models" AutoEventWireUp="false" Trace="false" Debug="false" Async="true" %>
<%@ Register TagPrefix="news" TagName="NewsMin" Src="~/controls/NewsMin.ascx" %>
<%@ Register TagPrefix="uc" TagName="UserReviewsMin" Src="~/controls/UserReviewsMin.ascx" %>
<%@ Register TagPrefix="tips" TagName="TipsAdvicesMin" Src="~/controls/TipsAdvicesMin.ascx" %>
<%@ Register TagPrefix="BikeWale" TagName="FeaturedBikes" Src="~/controls/FeaturedBike.ascx" %>
<%@ Register TagPrefix="RT" TagName="RoadTest" Src="/controls/RoadTestControl.ascx" %>
<%@ Register TagPrefix="uc" TagName="UpcomingBikes" Src="~/controls/UpcomingBikesMin.ascx" %>
<%@ Register TagPrefix="BikeBooking" TagName="BookBikeWidget" Src="~/controls/BikeBookingWidget.ascx" %>
<%@ Import Namespace="Bikewale.Common" %>
<%
    title = make + " Price in India, Review, Mileage & Photos - Bikewale";
    description = make + " Price in India - " + price  + ". Check out " + make + " on road price, reviews, mileage, variants, news & photos at Bikewale.";
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_New_";
    canonical       = "http://www.bikewale.com/" + makeMaskingName + "-bikes/" ;
    alternate="http://www.bikewale.com/m/"+ makeMaskingName + "-bikes/";
    ShowTargeting = "1";
    TargetedMake = make;
%>

<!-- #include file="/includes/headNew.aspx" -->

<%@ Register TagPrefix="PW" TagName="PopupWidget" Src="/controls/PopupWidget.ascx" %>
<PW:PopupWidget runat="server" ID="PopupWidget" />

<style type="text/css">  
.tbl-series { width:100%; border-collapse:collapse; }
.tbl-series td { padding:10px 0;}
.tbl-header {text-align:left; background-color:#e5e4e4; padding:5px 0;}
.series-row {border-top:1px solid #e5e4e4;}
.td-border {border-bottom:1px solid #e5e4e4;}
</style>
<div class="container_12">
<form runat="server">
    <div class="grid_12">
        <ul class="breadcrumb">
            <li>You are here: </li>
            <li itemtype="http://data-vocabulary.org/Breadcrumb"><a itemprop='url' href="/"><span itemprop="title">Home</span></a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li itemtype="http://data-vocabulary.org/Breadcrumb"><a itemprop='url' href="/new/"><span itemprop="title">New Bikes</span></a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li itemtype="http://data-vocabulary.org/Breadcrumb" class="current"><span itemprop="title"><strong><%= make %> Bikes</strong></span></li>
        </ul><div class="clear"></div>
    </div>
    <div class="grid_8  margin-top10"><!--    Left Container starts here -->
	    <h1 class="hd1"><%= make %> Bikes</h1>
	    <div id="divModels" runat="server" class="mid-box">
		    <table class="tbl-series margin-top5">
			    <tr>
				    <th width="150" class="tbl-header" >&nbsp;</th>
				    <th width="250" class="tbl-header">Bike Model</th>
				    <th class="tbl-header">Ex-Showroom Price (<asp:Literal id="ltrDefaultCityName" runat="server"></asp:Literal>)</th>
			    </tr>
	            <%--<asp:Repeater ID="rptModels" runat="server">
		            <itemtemplate>
			            <tr>
				            <td>
					            <a href='/<%# previousUrl + DataBinder.Eval(Container.DataItem,"MakeMaskingName").ToString()%>-bikes/<%# DataBinder.Eval(Container.DataItem,"ModelMaskingName").ToString() %>/photos/'>
					            <img title="<%=make%> <%# DataBinder.Eval(Container.DataItem,"Model") %> Photos" border="0" src='<%# MakeModelVersion.GetModelImage(DataBinder.Eval(Container.DataItem, "HostURL").ToString(),"/bikewaleimg/models/" + DataBinder.Eval(Container.DataItem,"SmallPic")) %>' alt='<%=make%> <%# DataBinder.Eval(Container.DataItem,"Model") %> Photos' /></a>
				            </td>
				            <td>
					            <a class="href-title" href='/<%# previousUrl + DataBinder.Eval(Container.DataItem,"MakeMaskingName").ToString()%>-bikes/<%# DataBinder.Eval(Container.DataItem,"ModelMaskingName").ToString() %>/'><%= make %> <%# DataBinder.Eval(Container.DataItem,"Model") %></a>
					            <p><%# GetRateImage( Convert.ToDouble(DataBinder.Eval(Container.DataItem,"ReviewRate"))) %> 
                                    <a title='<%= make %> <%# DataBinder.Eval(Container.DataItem,"Model") %> User Reviews' class="href-grey <%# DataBinder.Eval(Container.DataItem,"ReviewCount").ToString() == "0" ? "hide" : "" %>" href="/<%#DataBinder.Eval(Container.DataItem,"MakeMaskingName").ToString()%>-bikes/<%#DataBinder.Eval(Container.DataItem,"ModelMaskingName").ToString() %>/user-reviews/"><%# DataBinder.Eval(Container.DataItem,"ReviewCount") %> User Reviews</a>
                                    <a class="<%# DataBinder.Eval(Container.DataItem,"ReviewCount").ToString() == "0" ? "show" : "hide" %>" href="/content/userreviews/writereviews.aspx?bikem=<%# DataBinder.Eval(Container.DataItem,"Id") %>">Write a review</a>
					            </p>
				            </td>
				            <td>
					            <strong>Rs. <%# CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"MinPrice").ToString(),DataBinder.Eval(Container.DataItem,"MaxPrice").ToString()) %></strong>
					            <p class="<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"MinPrice").ToString()) ? "hide" : "" %>"><a href='/pricequote/default.aspx?model=<%# DataBinder.Eval(Container.DataItem,"ID") %>' title='<%=make%> <%# DataBinder.Eval(Container.DataItem,"Model") %> On Road Price'>Check on-road price</a></p>
				            </td>
			            </tr>
		            </itemtemplate>
	            </asp:Repeater>--%>
                <asp:Repeater ID="rptSeries" runat="server">
		            <itemtemplate>
			         <%--<%# DataBinder.Eval(Container.DataItem, "SeriesRank").ToString().Equals("1") ? GetSeriesRow(DataBinder.Eval(Container.DataItem, "SeriesRank").ToString(), DataBinder.Eval(Container.DataItem, "SeriesId").ToString(), DataBinder.Eval(Container.DataItem, "Series").ToString(),DataBinder.Eval(Container.DataItem, "Model").ToString(),  MakeModelVersion.GetFormattedPrice( DataBinder.Eval(Container.DataItem, "MinPrice").ToString()), DataBinder.Eval(Container.DataItem,"HostUrl").ToString(), DataBinder.Eval(Container.DataItem,"SmallPicUrl").ToString(), DataBinder.Eval(Container.DataItem,"ReviewCount").ToString(), DataBinder.Eval(Container.DataItem,"ModelMaskingName").ToString(), DataBinder.Eval(Container.DataItem,"MakeMaskingName").ToString(),DataBinder.Eval(Container.DataItem,"SeriesMaskingName").ToString(),DataBinder.Eval(Container.DataItem,"ModelCount").ToString(),DataBinder.Eval(Container.DataItem,"ModelId").ToString(),DataBinder.Eval(Container.DataItem,"ReviewRate").ToString()) : ""%>--%>
                        <%# DataBinder.Eval(Container.DataItem, "SeriesRank").ToString().Equals("1") ? GetSeriesRow(DataBinder.Eval(Container.DataItem, "SeriesRank").ToString(), DataBinder.Eval(Container.DataItem, "SeriesId").ToString(), DataBinder.Eval(Container.DataItem, "Series").ToString(),DataBinder.Eval(Container.DataItem, "Model").ToString(),  MakeModelVersion.GetFormattedPrice( DataBinder.Eval(Container.DataItem, "MinPrice").ToString()), DataBinder.Eval(Container.DataItem,"HostUrl").ToString(), DataBinder.Eval(Container.DataItem,"OriginalImagePath").ToString(), DataBinder.Eval(Container.DataItem,"ReviewCount").ToString(), DataBinder.Eval(Container.DataItem,"ModelMaskingName").ToString(), DataBinder.Eval(Container.DataItem,"MakeMaskingName").ToString(),DataBinder.Eval(Container.DataItem,"SeriesMaskingName").ToString(),DataBinder.Eval(Container.DataItem,"ModelCount").ToString(),DataBinder.Eval(Container.DataItem,"ModelId").ToString(),DataBinder.Eval(Container.DataItem,"ReviewRate").ToString()) : ""%>
			            <tr class='<%# DataBinder.Eval(Container.DataItem, "ModelCount").ToString().Equals("1") ? "hide" : "" %>'>			            
                            <td class="td-border" style="padding-left:10px;"><div><a class="href-grey" href="/<%#  DataBinder.Eval(Container.DataItem, "MakeMaskingName").ToString() +"-bikes/" + DataBinder.Eval(Container.DataItem,"ModelMaskingName").ToString() + "/" %>"><span class="text-grey"><%# DataBinder.Eval(Container.DataItem, "Model")  %></span></a><br><span class='<%# Convert.ToDouble(DataBinder.Eval(Container.DataItem,"ReviewRate")) <= 0 ? "hide" : ""%>'><%# CommonOpn.GetRateImage( Convert.ToDouble(DataBinder.Eval(Container.DataItem,"ReviewRate"))) %> | </span><a href="/<%#DataBinder.Eval(Container.DataItem,"MakeMaskingName").ToString()%>-bikes/<%#DataBinder.Eval(Container.DataItem,"ModelMaskingName").ToString() %>/user-reviews/" class="href-grey reviewLink <%# DataBinder.Eval(Container.DataItem,"ReviewCount").ToString() == "0" ? "hide" : "" %>" ><%# DataBinder.Eval(Container.DataItem, "ReviewCount") %> User Reviews</a><a href="/content/userreviews/writereviews.aspx?bikem=<%# DataBinder.Eval(Container.DataItem, "ModelId")%>" class="reviewLink <%# DataBinder.Eval(Container.DataItem,"ReviewCount").ToString() == "0" ? "" : "hide" %>" rel="nofollow">Write  a review</a></div></td>
				            <td class="td-border" valign="top">
                                <strong> Starts At Rs. <%# MakeModelVersion.GetFormattedPrice( DataBinder.Eval(Container.DataItem, "MinPrice").ToString() ) %></strong>
                                <div class="<%# DataBinder.Eval(Container.DataItem,"MinPrice").ToString().Equals("0")? "hide" : "" %>"><a href='/pricequote/default.aspx?model=<%# DataBinder.Eval(Container.DataItem,"ModelId") %>' title='<%=make%> <%# DataBinder.Eval(Container.DataItem,"Model") %> On Road Price' class='fillPopupData' pageCatId="1" modelId="<%# DataBinder.Eval(Container.DataItem,"ModelId") %>">Check on-road price</a></div>
				            </td>
                        </tr>
		            </itemtemplate>
	            </asp:Repeater>
		    </table>
	    </div>      
        <div class="margin-top20">   
            <RT:RoadTest id="ucRoadTestMin" runat="server" TopRecords="4" ControlWidth="grid_2" ></RT:RoadTest>
        </div><div class="clear"></div>
        <div class="margin-top15 grey-bg">
            <div class="content-block"><uc:UserReviewsMin id="ucUserReviewsMin" runat="server" TopRecords="5"></uc:UserReviewsMin></div>
        </div><div class="clear"></div>
        <div class="margin-top15">
            <h2>Featured Bikes</h2>
            <BikeWale:FeaturedBikes ID="ctrl_FeaturedBike" runat="server" TopRecords="4" />
        </div>
    </div><!-- Left Container ends here -->
    <div class="grid_4"><!--    Right Container starts here -->
        <div class="marging-top15">
            <BikeBooking:BookBikeWidget ID="ctrBikeBooking" runat="server"/>
        </div>
        <div class="margin-top15">
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/ads/Ad300x250.aspx" -->
        </div>
        <div class="margin-top15">
            <uc:UpcomingBikes ID="ucUpcoming" runat="server" HeaderText="Upcoming Bikes" TopRecords="2" ControlWidth="grid_2" />
        </div><div class="clear"></div>
        
        <div><news:NewsMin id="newsMin" runat="server"></news:NewsMin></div>
        <div class="margin-top15"><tips:TipsAdvicesMin id="tipsAdvices" runat="server" /></div>
        <div class="margin-top15">
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250_BTF -->
            <!-- #include file="/ads/Ad300x250BTF.aspx" -->
        </div>
    </div><!--    Right Container ends here -->
</form>    
</div>
<!-- #include file="/includes/footerInner.aspx" -->
