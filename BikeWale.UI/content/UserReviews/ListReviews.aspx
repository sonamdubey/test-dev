<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Content.ListReviews" Trace="false" %>
<%@ Import NameSpace="Bikewale.Common" %>
<%@ Register TagPrefix="BW" TagName="BikeRating" src="/Controls/BikeRatings.ascx" %>
<%@ Register TagPrefix="BW" TagName="RepeaterPagerReviews" src="/content/userreviews/RepeaterPagerReviews.ascx" %>
<%@ Register TagPrefix="BP" TagName="InstantBikePrice" Src="/controls/instantbikeprice.ascx" %>
<%@ Register TagPrefix="LD" TagName="LocateDealer" Src="/controls/locatedealer.ascx" %>
<%@ Register TagPrefix="PW" TagName="PopupWidget" Src="/controls/PopupWidget.ascx" %>
<%
    title = "User Reviews: " + BikeName;
	description = BikeName + " User Reviews - Read first-hand reviews of actual " + BikeName + " owners. Find out what buyers of " + BikeName + " have to say about the bike.";
	keywords = BikeName + " reviews, " + BikeName + " Users Reviews, " + BikeName + " customer reviews, " + BikeName + " customer feedback, " + BikeName + " owner feedback, user bike reviews, owner feedback, consumer feedback, buyer reviews";
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_New_";
    alternate = "http://www.bikewale.com" + "/m/" + MakeMaskingName + "-bikes/" + ModelMaskingName + "/user-reviews/";
 %>
<!-- #include file="/includes/headnew.aspx" -->



<style type="text/css">
    .action-btn.write-review-btn { padding:8px 12px !important; }
</style>

<div class="container_12">
    <div class="grid_12">
        <ul class="breadcrumb">
            <li>You are here: </li>
            <li><a href="/">Home</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li><a href="/new/">New</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li><a href="/<%= MakeMaskingName %>-bikes/"><%= MakeName%></a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li><a href="/<%=  MakeMaskingName %>-bikes/<%= ModelMaskingName %>/"><%= ModelName%></a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><strong>User Reviews</strong></li>
        </ul><div class="clear"></div>
    </div>
    <div class="grid_8  margin-top10">  
        <h1><a href="/<%=  MakeMaskingName %>-bikes/<%= ModelMaskingName %>/"><%= BikeName %> </a>User Reviews</h1>     
		<div class="grid_3 alpha margin-top10 margin-top15">
			<%--<img src="<%= Bikewale.Common.MakeModelVersion.GetModelImage(HostUrl, "/bikewaleimg/models/" + LargePic) %>" title="<%= BikeName %>"/>--%>
            <img src="<%= Bikewale.Common.MakeModelVersion.GetModelImage(HostUrl, OriginalImagePath,Bikewale.Utility.ImageSize._210x118) %>" title="<%= BikeName %>"/>
			<div style="margin-top:5px; line-height:17px;">
				<span class="text-highlight"><%=!IsNew && IsUsed ? "Last Recorded Price Rs.":"Starts at Rs." %> <%= CommonOpn.FormatPrice( ModelStartPrice ) %></span><br />
				<span class="margin-top5"><a title="<%= MakeName%> <%= ModelName%> details" href='/<%= MakeMaskingName %>-bikes/<%= ModelMaskingName %>/'><%= ModelName %> Details</a><% if (IsNew && IsUsed) { %><span class="text-grey"> | </span><a class="fillPopupData" href="/pricequote/default.aspx?model=<%= modelId %>" modelId="<%= modelId %>">On Road Price</a><% } %></span>
			</div>	
		</div>
		<div class="grid_5 omega margin-top10">			
            <div class="grid_3 alpha">
			    <h2 class="margin-bottom5"><%= BikeName %> Ratings</h2>			    				
				<div class="right-float"><%= CommonOpn.GetRateImage(RatingOverall) %></div><div class="left-float">Overall</div><div class="clear"></div>
				<div class="right-float"><%= CommonOpn.GetRateImage(RatingLooks) %></div><div class="left-float">Looks</div><div class="clear"></div>
				<div class="right-float"><%= CommonOpn.GetRateImage(RatingPerformance) %></div><div class="left-float">Performance</div><div class="clear"></div>
				<div class="right-float"><%= CommonOpn.GetRateImage(RatingComfort) %></div><div class="left-float">Space/Comfort</div><div class="clear"></div>
				<div class="right-float"><%= CommonOpn.GetRateImage(RatingFuelEconomy) %></div><div class="left-float">Fuel Economy</div><div class="clear"></div>
				<div class="right-float"><%= CommonOpn.GetRateImage(RatingValueForMoney) %></div><div class="left-float">Value For Money</div><div class="clear"></div>			    
            </div>
            <div class="grid_2 omega margin-top20">
				<p class="f-small">Own a <span class="price2"><%= BikeName %>?</span> Help others make an informed buying decision.</p>
				<div class="action-btn write-review-btn margin-top5"><a href="/content/userreviews/writereviews.aspx?bikem=<%= modelId %>">Write a Review</a></div>
			</div>
		</div>				
	    <div class="clear"></div>
	    <h2 class="margin-top10"><%= totalReviewCount %> <%= BikeName %> User Reviews Available</h2>	
	    <div class="gray-block-top">
            <div class="grey-bg content-block margin-top5">
                <div class="left-float">Show reviews for  <asp:DropDownList ID="drpVersions" runat="server" AutoPostBack="true"></asp:DropDownList></div>
                <div class="right-float">
                    Sort by <asp:DropDownList ID="drpSort" runat="server" AutoPostBack="true">
					            <asp:ListItem Selected="true" Text="Most Helpful" Value="1" />
					            <asp:ListItem Text="Most Read" Value="2" />
					            <asp:ListItem Text="Most Recent" Value="3" />
					            <asp:ListItem Text="Most Rated" Value="4" />
				            </asp:DropDownList>
                </div><div class="clear"></div>               
            </div>
		    
		    <BW:RepeaterPagerReviews id="rpgReviews" PageSize="10" ShowHeadersVisible="true" PagerPosition="Bottom" runat="server">
			    <asp:Repeater ID="rptReviews" runat="server">			
				    <itemtemplate>
					    <div class="margin-bottom15">	
                            <div class="anchor-title">				
						        <a href='/<%= MakeMaskingName %>-bikes/<%= ModelMaskingName %>/user-reviews/<%# DataBinder.Eval(Container.DataItem, "ReviewId")%>.html'><%# DataBinder.Eval(Container.DataItem, "Title") %></a>
                            </div>						
						    <div class="grid_5 alpha">
							    <%# CommonOpn.GetRateImage(Convert.ToDouble(DataBinder.Eval(Container.DataItem, "OverallR"))) %> by <%# HandleName(DataBinder.Eval(Container.DataItem, "CustomerName").ToString(), DataBinder.Eval(Container.DataItem, "HandleName").ToString())%>, <%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "EntryDateTime")).ToString("dd-MMM-yyyy") %> <a class="f-small hide" href="/forums/viewthread-<%# DataBinder.Eval(Container.DataItem, "ThreadId") %>.html"><%# DataBinder.Eval(Container.DataItem, "Comments") %> Comments</a>
						    </div>
						     <%# DataBinder.Eval(Container.DataItem, "Pros").ToString() == "" ? "" : "<br><strong>Good: </strong>" + DataBinder.Eval(Container.DataItem, "Pros").ToString() %>
						     <%# DataBinder.Eval(Container.DataItem, "Cons").ToString() == "" ? "" : "<br><strong>Bad: </strong>" + DataBinder.Eval(Container.DataItem, "Cons").ToString() %>
						    <div class="margin-top10">
							    <span id="spnComments" runat="server"><%# GetComments(DataBinder.Eval(Container.DataItem, "SubComments").ToString()) %></span>...
							    <a href='/<%= MakeMaskingName %>-bikes/<%= ModelMaskingName %>/user-reviews/<%# DataBinder.Eval(Container.DataItem, "ReviewId")%>.html'>read complete review</a>
						     </div>
					     </div>	
					     <div class="sept-dashed"></div>
				    </itemtemplate>			
			    </asp:Repeater>
		    </BW:RepeaterPagerReviews>
	    </div>
    </div>
    <div class="grid_4">
        <div class="margin-top15">
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/ads/Ad300x250.aspx" -->
        </div>
        <div class="light-grey-bg content-block border-radius5 padding-bottom20 margin-top15">
            <BP:InstantBikePrice runat="server" ID="InstantBikePrice" />
        </div>
        <div class="light-grey-bg content-block border-radius5 margin-top10 padding-bottom20">
            <LD:LocateDealer runat="server" id="LocateDealer" />
        </div>
        <div class="margin-top15">
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/ads/Ad300x250BTF.aspx" -->
        </div>
    </div>
</div>
<PW:PopupWidget runat="server" ID="PopupWidget" />
<!-- #include file="/includes/footerinner.aspx" -->
