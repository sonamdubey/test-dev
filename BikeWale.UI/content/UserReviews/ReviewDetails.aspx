<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Content.ReviewDetails" Trace="false" %>
<%@ Import NameSpace="Bikewale.Common" %>
<%@ Register TagPrefix="BikeWale" TagName="DiscussIt" Src="/Controls/DiscussIt.ascx" %>
<%@ Register TagPrefix="BP" TagName="InstantBikePrice" Src="/controls/instantbikeprice.ascx" %>
<%@ Register TagPrefix="LD" TagName="LocateDealer" Src="/controls/locatedealer.ascx" %>
<%@ Register TagPrefix="PW" TagName="PopupWidget" Src="/controls/PopupWidget.ascx" %>
<%
    title 			= _title + " - A Review on " + BikeMake + " " + BikeModel + " by " + reviewerName;
	description 	= BikeMake + " User Review - " + "A review/feedback on " + BikeName + " by " + reviewerName + ". Find out what " + reviewerName + " has to say about " + BikeName + ".";
	keywords		=  BikeMake + " " + BikeModel + " review, " + BikeMake + " " + BikeModel + " user review, car review, owner feedback, consumer review";
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_New_";
    alternate = "http://www.bikewale.com/m/" + MakeMaskingName + "-bikes/" + ModelMaskingName + "/user-reviews/" + reviewerId + ".html";
 %>
<!-- #include file="/includes/headnew.aspx" --> 
    <div class="container_12">
        <div class="grid_12">
            <ul class="breadcrumb">
                <li>You are here: </li>
                <li><a href="/">Home</a></li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li><a href="/new/">New</a></li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li><a href="/<%= MakeMaskingName%>-bikes/"><%= BikeMake%></a></li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li><a href="/<%= MakeMaskingName%>-bikes/<%= ModelMaskingName%>/"><%= BikeModel%></a></li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li><a href="/<%= MakeMaskingName%>-bikes/<%= ModelMaskingName%>/user-reviews/"><%=BikeModel%> Reviews</a></li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li class="current"><strong>User Reviews</strong></li>
            </ul><div class="clear"></div>
        </div>
        <div class="grid_8 margin-top10">
	    <h1><%= _title %></h1>	
	    <div>A user review on <a href="/<%= MakeMaskingName%>-Bikes/<%= ModelMaskingName%>/"><%= BikeMake %> <%= BikeModel %></a>. Written by
        <%if(handleName != ""){%> 
		    <%=handleName%> 
		    <%}else{%>
		    <%=reviewerName%>
		    <%}%>
		    on <%= entryDate %></div>      
        <div class="grid_3 alpha margin-bottom15 margin-top15">
            <%--<img src="<%= Bikewale.Common.MakeModelVersion.GetModelImage(HostUrl, "/bikewaleimg/models/" + LargePic) %>" title="<%=BikeMake%> <%=BikeModel%>" />--%>
            <img src="<%= Bikewale.Common.MakeModelVersion.GetModelImage(HostUrl,OriginalImagePath,Bikewale.Utility.ImageSize._210x118) %>" title="<%=BikeMake%> <%=BikeModel%>" />
		    <div class="margin-top10">
			    <span class="text-highlight"><%=!IsNew && IsUsed ? "Last Recorded Price Rs: " : "Start at Rs: " %> <%= CommonOpn.FormatPrice( ModelStartPrice ) %></span><br />
			    <span><a title="<%= BikeMake %> <%= BikeModel %> details" href='/<%= MakeMaskingName%>-Bikes/<%= ModelMaskingName %>/'><%=BikeModel%> Details</a><% if(IsNew && IsUsed) { %><span class="text-grey"> | </span><a href="/pricequote/default.aspx?model=<%= ModelId %>" class="fillPopupData" modelId="<%= ModelId %>">Check on Road Price</a><% } %></span>
		    </div>
        </div>                  
        <div class="grid_5 omega margin-top10">
            <div class="grid_3 alpha">
                <h3><%if(handleName != ""){%><%=handleName%><%}else{%><%=reviewerName%><%}%>'s Ratings</h3>
                <table width="100%" cellpadding="2" cellspacing="0" border="0" class="margin-top5">
                    <tr>
                        <td width="130">Overall</td>
                        <td><%= CommonOpn.GetRateImage(overallR) %></td>
                    </tr>
                    <tr>
                        <td>Looks</td>
                        <td><%= CommonOpn.GetRateImage(styleR) %></td>
                    </tr>
                    <tr>
                        <td>Performance</td>
                        <td><%= CommonOpn.GetRateImage(performanceR) %></td>
                    </tr>
                    <tr>
                        <td>Space/Comfort</td>
                        <td><%= CommonOpn.GetRateImage(comfortR) %></td>
                    </tr>
                    <tr>
                        <td>Fuel Economy</td>
                        <td><%= CommonOpn.GetRateImage(fuelEconomyR) %></td>
                    </tr>
                    <tr>
                        <td>Value For Money</td>
                        <td><%= CommonOpn.GetRateImage(valueR) %></td>
                    </tr>
                </table>
            </div>
            <div class="grid_2 omega margin-top20">
                Own a <%= BikeName %>? Help others make an informed buying decision. <a href="/content/userreviews/writereviews.aspx?bikem=<%= ModelId %>" class="margin-top5 action-btn btn-xs">Write a review</a>
            </div>                
        </div>               
        <div class="clear"></div>       
	    <table class="margin-top15" width="100%" border="0" cellpadding="2" cellspacing="0">	          
            <tr id="trVerReviewed" runat="server" class="hide">
			    <td><span>Version Reviewed</span></td>			  
			    <td><%= BikeVersion %></td>
			</tr>            		   		  
            <tr>
			    <td width="80"><span class="price2 black-text">Good :</span></td>			   
			    <td><%= pros %></td>
		    </tr>
		    <tr>
			    <td><span class="price2 black-text">Bad : </span></td>			  
			    <td><%= cons %></td>
		    </tr>
	    </table>
		
	    <h3 class="margin-top10">Full Review <span><%= userLoggedIn ? "<a class='f-small' href='/userreviews/editreview.aspx?rid=" + reviewId + "'>Edit</a>" : "" %></span></h3>		
	    <div class="margin-top15 format-content"><%= comments %></div>	    
	    <div class="hr-dotted"></div>	
        <div class="margin-top10 content-block grey-bg border-light margin-bottom15">
            <div class="grid_4 alpha ">
			<div class="text-grey"><%=viewed%> Views, <span id="spnLiked"><%=liked%></span> of <span id="spnDisliked"><%= liked + disliked %></span> people found this review useful</div>										
			<div id="divHelpful" class="readmore black-text">Was this review helpful to you? <a onClick='javascript:helpfulClick(<%= reviewId %>, "1")' class=" pointer">Yes</a> | <a onClick='javascript:helpfulClick(<%= reviewId %>, "0")' class=" pointer">No</a></div>
		</div>										
		<div class="grid_3 omega readmore">
			<p id="divAbuse"> Inappropriate Review? <a onclick='javascript:abuseClick(<%= reviewId %>)' class=" pointer">Report Abuse</a> </p>
		</div><div class="clear"></div>
        </div>			   
	    <BikeWale:DiscussIt id="ucDiscuss" runat="server" />
	    <div class="mid-box hide">
		    <div class="left-grid-lt" style="width:300px;"><%= Prev %></div>
		    <div class="left-grid-rt" align="right" style="width:300px;"><%= Next %></div>
	    </div><div class="clear"></div>
	    <div class="gray-block hide">
		    <h2 class="hd2">More Reviews on <%= BikeMake %> <%= BikeModel %></h2>
		    <asp:Repeater ID="rptMoreUserReviews" runat="server">
			    <itemtemplate>
				    <div class="mar-top-5 hr-dotted" style="padding-bottom:5px;">
					    <span style="display:none;"><%# CommonOpn.GetRateImage( Convert.ToDouble( DataBinder.Eval(Container.DataItem, "OverallR") ) ) %><br /></span>
					    <a href="/<%# MakeMaskingName%>-Bikes/<%# ModelMaskingName%>/user-reviews/<%# DataBinder.Eval(Container.DataItem, "ReviewId") %>.html"><%# DataBinder.Eval(Container.DataItem, "Title") %></a> <span class="text-grey">by <%# DataBinder.Eval(Container.DataItem, "CustomerName") %> on <%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "EntryDateTime")).ToString("dd MMM, yyyy") %></span>					
				    </div>
			    </itemtemplate>
		    </asp:Repeater>
		    <a href="/<%= MakeMaskingName%>-Bikes/<%= ModelMaskingName%>/user-reviews/">All <%= BikeMake %> <%= BikeModel %> user reviews</a>
		    <span class="icon-sheet more-link"></span>
		    <div class="clear"></div>
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
<div id="report-abuse" class="hide">
    <span>Comments</span>
    <p><textarea type="text" id="txtAbuseComments" rows="6" cols="35"></textarea></p>
    <span id="spnAbuseComments" class="error"></span><br />
    <a id="btnReportReviewAbuse" class="buttons" onclick="javascript:reportAbuse()">Report</a>
</div>   
<script type="text/javascript" src="/src/graybox.js?v=1.0"></script>
    <script language="javascript">
        function helpfulClick(reviewId, helpful) {
            //alert(reviewId + "," + helpful);
            $.ajax({
                type: "POST",
                url: "/ajaxpro/Bikewale.Ajax.AjaxUserReviews,Bikewale.ashx",
                data: '{"reviewId":"' + reviewId + '","helpful":"' + helpful + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "UpdateReviewHelpful"); },
                success: function (response) {
                    var responseObj = eval('(' + response + ')');
                    if (responseObj.value) {
                        var likedOrig = Number(document.getElementById("spnLiked").innerHTML);
                        var disliked = Number(document.getElementById("spnDisliked").innerHTML);

                        if (helpful == "1") {
                            document.getElementById("spnLiked").innerHTML = likedOrig + 1;
                        }

                        document.getElementById("spnDisliked").innerHTML = disliked + 1;
                        document.getElementById("divHelpful").innerHTML = "This review was " + (helpful == true ? "helpful" : "not helpful") + " to me.";
                    } else {
                        document.getElementById("divHelpful").innerHTML = "You have already voted for this review.";
                    }
                }
            });
        }

        function abuseClick(reviewId) {
            $.ajax({
                type: "POST",
                url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
                data: {},
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetCurrentUserId"); },
                success: function (response) {
                    var userId = eval('(' + response + ')');
                    //alert(userId.value);
                    if (userId.value != "-1") {
                        var caption = "Why do you want to report it abuse?";
                        var url = "reviewcomments.aspx";
                        var applyIframe = false;

                        GB_show(caption, url, 180, 350, applyIframe, $("#report-abuse").html());
                    } else {
                        location.href = "/users/login.aspx?returnUrl=/content/userreviews/reviewdetails.aspx?rid=" + reviewId;
                    }
                }
            });
        }

        function reportAbuse() {
            var isError = false;

            if ($("#txtAbuseComments").val().trim() == "") {
                $("#spnAbuseComments").html("Comments is required");
                isError = true;
            } else {
                $("#spnAbuseComments").html("");
            }

            if (!isError) {
                var commentsForAbuse = $("#txtAbuseComments").val().trim();
                var reviewId = '<%= reviewId %>';

                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/Bikewale.Ajax.AjaxUserReviews,Bikewale.ashx",
                    data: '{"reviewId":"' + reviewId + '","comments":"' + commentsForAbuse + '"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "UpdateReviewAbuse"); },
                    success: function (response) {
                        var responseObj = eval('(' + response + ')');
                        if (responseObj.value == true) {
                            GB_hide();
                            document.getElementById("divAbuse").innerHTML = "Your request has been sent to the administrator.";
                        }
                    }
                });
            }
        }
</script>
<PW:PopupWidget runat="server" ID="PopupWidget" />
<!-- #include file="/includes/footerinner.aspx" -->