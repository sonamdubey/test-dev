<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Content.ReviewDetails" %>
<%
    title = objReview.ReviewEntity.ReviewTitle + " - A Review on " + objReview.BikeEntity.MakeEntity.MakeName + " " + objReview.BikeEntity.ModelEntity.ModelName + " by " + objReview.ReviewEntity.WrittenBy;
    description = objReview.BikeEntity.MakeEntity.MakeName + " User Review - " + "A review/feedback on " + objReview.BikeEntity.MakeEntity.MakeName + " " + objReview.BikeEntity.ModelEntity.ModelName + " by " + objReview.ReviewEntity.WrittenBy + ". Find out what " + objReview.ReviewEntity.WrittenBy + " has to say about " + objReview.BikeEntity.MakeEntity.MakeName + " " + objReview.BikeEntity.ModelEntity.ModelName + ".";
    keywords = objReview.BikeEntity.MakeEntity.MakeName + " " + objReview.BikeEntity.ModelEntity.ModelName + " review, " + objReview.BikeEntity.MakeEntity.MakeName + " " + objReview.BikeEntity.ModelEntity.ModelName + " user review, car review, owner feedback, consumer review";
    canonical = "http://www.bikewale.com/" + objReview.BikeEntity.MakeEntity.MaskingName + "-bikes/" + objReview.BikeEntity.MakeEntity.MaskingName + objReview.BikeEntity.ModelEntity.MaskingName + "/user-reviews/" + objReview.ReviewEntity.ReviewId + ".html";
    AdPath = "/1017752/Bikewale_Mobile_Model";
    AdId = "1398837216327";
    menu = "9";
%>
<!-- #include file="/includes/headermobile.aspx" -->
<style>
    img {border:none;}
</style>
<form id="form1" runat="server">
    <div class="padding5">
       <div id="br-cr">
            <a href="/m/new/" class="normal">New Bikes</a> &rsaquo;  
            <a href="/m/<%=objReview.BikeEntity.MakeEntity.MaskingName %>-bikes/" class="normal"><%=objReview.BikeEntity.MakeEntity.MakeName %></a> &rsaquo; 
            <a href="/m/<%=objReview.BikeEntity.MakeEntity.MaskingName %>-bikes/<%= objReview.BikeEntity.ModelEntity.MaskingName %>/" class="normal"><%= objReview.BikeEntity.ModelEntity.ModelName %></a> &rsaquo;
            <a href="/m/<%=objReview.BikeEntity.MakeEntity.MaskingName %>-bikes/<%= objReview.BikeEntity.ModelEntity.MaskingName %>/user-reviews/" class="normal">User Reviews</a>  &rsaquo;
            <span class="lightgray"><%= objReview.ReviewEntity.ReviewTitle %></span>
        </div>
        <h1><%= objReview.ReviewEntity.ReviewTitle %></h1>
        <div class="new-line5 f-12"> <%= Bikewale.Common.CommonOpn.GetDisplayDate(objReview.ReviewEntity.ReviewDate.ToString()) %> | By <%=  objReview.ReviewEntity.WrittenBy %></div>
        <div id="divModel" class="box1 new-line5 f-12">
	        <div class="normal f-12" style="text-decoration:none;">
		        <table style="width:100%;" cellpadding="0" cellspacing="0">
				    <tr>
					    <td style="width:100px;vertical-align:top;margin-left:5px;">
                            <%--<img alt="<%=  objReview.BikeEntity.MakeEntity.MakeName + " " + objReview.BikeEntity.ModelEntity.ModelName %> Reviews" title=" <%=  objReview.BikeEntity.MakeEntity.MakeName + " " + objReview.BikeEntity.ModelEntity.ModelName %> Reviews" src="<%= Bikewale.Common.MakeModelVersion.GetModelImage( objReview.HostUrl, "/bikewaleimg/models/" + objReview.LargePicUrl) %>" width="100">--%>
                            <img alt="<%=  objReview.BikeEntity.MakeEntity.MakeName + " " + objReview.BikeEntity.ModelEntity.ModelName %> Reviews" title=" <%=  objReview.BikeEntity.MakeEntity.MakeName + " " + objReview.BikeEntity.ModelEntity.ModelName %> Reviews" src="<%= Bikewale.Common.MakeModelVersion.GetModelImage( objReview.HostUrl, objReview.OriginalImagePath,Bikewale.Utility.ImageSize._110x61) %>" width="100">
                            <div class="darkgray"><b><%=!objReview.New && objReview.Used ? "Last Recorded Price: " : "Starts At: " %></b></div>
                            <div class="darkgray"><b>Rs. <%= Bikewale.Common.CommonOpn.FormatNumeric(objReview.BikeEntity.Price.ToString())%></b></div>
                        </td>
					    <td valign="top">
                            <table style="width:100%">
                                   <tr><td class="darkgray" colspan="2"><b> <%=  objReview.ReviewEntity.WrittenBy %>'s Ratings</b></td></tr>
                                    <tr>
                                        <td style="width:110px;" class="darkgray"><span style="position:relative;top:2px;">Overall Average</span></td>
                                        <td style="font-size:0px;"> 
                                            <%= Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble(objReview.ReviewRatingEntity.OverAllRating))%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="darkgray new-line5"><span style="position:relative;top:2px;">Looks</span></td>
                                        <td style="font-size:0px;">
                                            <%= Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble(objReview.ReviewRatingEntity.StyleRating))%>
                                        </td>
                                    </tr>
                                        <tr>
                                        <td class="darkgray new-line"><span style="position:relative;top:2px;">Performance</span></td>
                                        <td style="font-size:0px;">
                                             <%= Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble(objReview.ReviewRatingEntity.PerformanceRating))%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="darkgray new-line"><span style="position:relative;top:2px;">Space/Comfort</span></td>
                                        <td style="font-size:0px;">
                                             <%= Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble(    objReview.ReviewRatingEntity.ComfortRating))%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="darkgray new-line"><span style="position:relative;top:2px;">Fuel Economy</span></td>
                                        <td style="font-size:0px;">
                                               <%= Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble( objReview.ReviewRatingEntity.FuelEconomyRating))%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="darkgray new-line"><span style="position:relative;top:2px;">Value For Money</span></td>
                                        <td style="font-size:0px;">
                                               <%= Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble(objReview.ReviewRatingEntity.ValueRating))%>
                                        </td>
                                    </tr>
                            </table>
                        </td>
				    </tr>
		        </table>
	        </div>
        </div> 
        <div class="box1 new-line5 f-12">
            <div class="container10" style="text-decoration:none;">
                <table style=" width:100%; vertical-align: top;">
                    <tr class="darkgray new-line" style="vertical-align:top;">
                        <td style="width:15%"><b>Good : </b></td>
                        <td><%=  objReview.ReviewEntity.Pros %></td>
                    </tr>
                    <tr class="darkgray new-line" style="vertical-align:top;">
                        <td style="width:10%"><b>Bad : </b></td>
                        <td><%=  objReview.ReviewEntity.Cons %></td>
                    </tr>
                </table>
            </div>
        </div>
        <h2 class="new-line5">Full Review</h2>
        <div class="box1 new-line10" style="font-size:14px; line-height:18px;">
             <%= objReview.ReviewEntity.Comments %>
        </div>
<%--         <%if( !String.IsNullOrEmpty(nextPageUrl)) {%>
        <a href="<%= nextPageUrl%>" class="normal">
            <div class="box1 new-line5">
                <div class="rightArrowContainer">
                    <div style="text-align:right;">
                        <div style="font-weight:bold;">Next Review</div>
                    </div>
                </div>
            </div>
        </a>
    <%} %>

    <%if( !String.IsNullOrEmpty(prevPageUrl)) {%>
        <a href="<%= prevPageUrl %>" class="normal">
            <div class="box1 new-line5">
                <div class="leftArrowContainer">
                    <div>
                        <div style="font-weight:bold;">Previous Review</div>
                    </div>
                </div>
            </div>
        </a>
    <%} %>--%>
    <table style="width:100%; margin-top:10px;" cellspacing="0" cellpadding="0" border="0" class="new-line5">
	    <tr> 
		    <td>
			    <%if (!String.IsNullOrEmpty(prevPageUrl))
                {%>
				    <a class="normal" href="<%= prevPageUrl %>">
				        <span><span class="arr-big">&laquo;</span>&nbsp;Prev</span>
				    </a>
			    <%}%>
		    </td>
		    <td align="right">
			    <%if (!String.IsNullOrEmpty(nextPageUrl))
                {%>
				    <a class="normal" href="<%= nextPageUrl %>">
				        <span>Next<span class="arr-big">&nbsp;&raquo;</span></span>
				    </a>
			    <%}%>
		    </td>
	    </tr>
    </table>

</div>
</form>
<!-- #include file="/includes/footermobile.aspx" -->
