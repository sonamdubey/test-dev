<%@ Page Language="C#" Inherits="Bikewale.New.bikeseries" AutoEventWireUp="false" Trace="false" Debug="false" async="true"%>
<%@ Register TagPrefix="news" TagName="NewsMin" Src="~/controls/NewsMin.ascx" %>
<%@ Register TagPrefix="uc" TagName="UserReviewsMin" Src="~/controls/UserReviewsMin.ascx" %>
<%@ Register TagPrefix="tips" TagName="TipsAdvicesMin" Src="~/controls/TipsAdvicesMin.ascx" %>
<%@ Register TagPrefix="BikeWale" TagName="FeaturedBikes" Src="~/controls/FeaturedBike.ascx" %>
<%@ Register TagPrefix="RT" TagName="RoadTest" Src="/controls/RoadTestControl.ascx" %>
<%@ Register TagPrefix="uc" TagName="UpcomingBikes" Src="~/controls/UpcomingBikesMin.ascx" %>
<%@ Register TagPrefix="BikeBooking" TagName="BookBikeWidget" Src="~/controls/BikeBookingWidget.ascx" %>
<%@ Import Namespace="Bikewale.Common" %>
<%
    title = make+ " " + series +" Price in India, Review, Mileage,Photos & More - BikeWale";
    keywords = make +" bikes, "+  make + " " + series +" India, "+ make + " " + series + " bike prices, buy "+ make + " " + series + " Bikes, "+ make + " " + series + " reviews, bike reviews, bike news";
    description = make + " " + series +" price in India" +price+ ". Check out "+ make + " " + series + " on road price, reviews, mileage, variants, news & photos at Bikewale";  
    canonical   = "http://www.bikewale.com/" + makeMaskingName + "-bikes/" + seriesMaskingName + "-series/";
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_New_";
    alternate= "http://www.bikewale.com/m/" + makeMaskingName + "-bikes/" + seriesMaskingName + "-series/";
    ShowTargeting = "1";
    TargetedSeries = series.Trim();
%>
<!-- #include file="/includes/headNew.aspx" -->

<%@ Register TagPrefix="PW" TagName="PopupWidget" Src="/controls/PopupWidget.ascx" %>
<PW:PopupWidget runat="server" ID="PopupWidget" />


<div class="container_12">
    <div class="grid_12">
        <ul class="breadcrumb" itemscope itemtype="http://data-vocabulary.org/Breadcrumb">
            <li>You are here: </li>
            <li><a itemprop="url" href="/"><span itemprop="title">Home</span></a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li><a itemprop="url" href="/new/"><span itemprop="title">New Bikes</span></a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li><a itemprop="url" href='/<%=makeMaskingName %>-bikes/'><span itemprop="title"><%=make %> Bikes</span></a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><span itemprop="title"><strong><%= series%></strong></span></li>
        </ul><div class="clear"></div>
    </div>
    <div class="grid_8  margin-top10"><!--    Left Container starts here -->
	    <h1 class="hd1"><%= make + " " + series%> Bikes</h1>
        <div class="margin-top10 margin-bottom20">
            <asp:Label id="lblSeriesDesc" runat="server"></asp:Label>
        </div>
	    <div id="divModels" runat="server" class="mid-box">
		    <table class="tbl-std margin-top5">
			    <tr>
				    <th width="150">&nbsp;</th>
				    <th width="250">Bike Model </th>
				    <th>Ex-Showroom Price (<asp:Literal id="ltrDefaultCityName" runat="server"></asp:Literal>)</th>
			    </tr>
	            <asp:Repeater ID="rptModels" runat="server">
		            <itemtemplate>
			            <tr>
				            <td>
					            <a href='/<%# previousUrl + DataBinder.Eval(Container.DataItem,"MakeMaskingName").ToString()%>-bikes/<%# DataBinder.Eval(Container.DataItem,"ModelMaskingName").ToString() %>/photos/'>
					            <%--<img title="<%=make%> <%# DataBinder.Eval(Container.DataItem,"Model") %> Photos" border="0" src='<%# MakeModelVersion.GetModelImage(DataBinder.Eval(Container.DataItem, "HostURL").ToString(),"/bikewaleimg/models/" + DataBinder.Eval(Container.DataItem,"SmallPic")) %>' alt='<%=make%> <%# DataBinder.Eval(Container.DataItem,"Model") %> Photos' /></a>--%>
                                    <img title="<%=make%> <%# DataBinder.Eval(Container.DataItem,"Model") %> Photos" border="0" src='<%# MakeModelVersion.GetModelImage(DataBinder.Eval(Container.DataItem, "HostURL").ToString(),DataBinder.Eval(Container.DataItem,"OriginalImagePath").ToString(),Bikewale.Utility.ImageSize._110x61) %>' alt='<%=make%> <%# DataBinder.Eval(Container.DataItem,"Model") %> Photos' /></a>
				            </td>
				            <td>
					            <a class="href-title" href='/<%# previousUrl + DataBinder.Eval(Container.DataItem,"MakeMaskingName").ToString()%>-bikes/<%# DataBinder.Eval(Container.DataItem,"ModelMaskingName").ToString() %>/'><%= make %> <%# DataBinder.Eval(Container.DataItem,"Model") %></a>
					            <p><span class='<%# DataBinder.Eval(Container.DataItem,"ReviewCount").ToString() == "0" ? "hide" : "" %>'><%# CommonOpn.GetRateImage( Convert.ToDouble(DataBinder.Eval(Container.DataItem,"ReviewRate"))) %></span>
                                    <a title='<%= make %> <%# DataBinder.Eval(Container.DataItem,"Model") %> User Reviews' class="href-grey <%# DataBinder.Eval(Container.DataItem,"ReviewCount").ToString() == "0" ? "hide" : "" %>" href="/<%#DataBinder.Eval(Container.DataItem,"MakeMaskingName").ToString()%>-bikes/<%#DataBinder.Eval(Container.DataItem,"ModelMaskingName").ToString() %>/user-reviews/"><%# DataBinder.Eval(Container.DataItem,"ReviewCount") %> User Reviews</a>
                                    <a class="<%# DataBinder.Eval(Container.DataItem,"ReviewCount").ToString() == "0" ? "show" : "hide" %>" href="/content/userreviews/writereviews.aspx?bikem=<%# DataBinder.Eval(Container.DataItem,"Id") %>">Write a review</a>
					            </p>
				            </td>
				            <td>
					            <strong>Start At Rs. <%# CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"MinPrice").ToString()) %></strong>
					            <p class="<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"MinPrice").ToString()) ? "hide" : "" %>"><a href='/pricequote/default.aspx?model=<%# DataBinder.Eval(Container.DataItem,"ID") %>' title='<%=make%> <%# DataBinder.Eval(Container.DataItem,"Model") %> On Road Price' class="fillPopupData" pageCatId="2" modelId="<%#  DataBinder.Eval(Container.DataItem,"ID") %>">Check on-road price</a></p>
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
</div>
<!-- #include file="/includes/footerInner.aspx" -->
