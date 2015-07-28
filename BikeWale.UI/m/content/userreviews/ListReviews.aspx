<%@ Page Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Mobile.Content.ListReviews" %>
<%@ Register TagPrefix="Pager" TagName="Pager" Src="/m/controls/ListPagerControl.ascx" %>
<%  title = "User Reviews: " + objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName;
    description = objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + " User Reviews - Read first-hand reviews of actual " + objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + " owners. Find out what buyers of " + objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + " have to say about the bike.";
    keywords = objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + " reviews, " + objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + " Users Reviews, " + objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + " customer reviews, " + objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + " customer feedback, " + objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + " owner feedback, user bike reviews, owner feedback, consumer feedback, buyer reviews";
    canonical = "http://www.bikewale.com/" + objModelEntity.MakeBase.MaskingName + "-bikes/" + objModelEntity.MaskingName + "/user-reviews";
    relPrevPageUrl = String.IsNullOrEmpty(prevPageUrl) ? "" : "http://www.bikewale.com" + prevPageUrl;
    relNextPageUrl = String.IsNullOrEmpty(nextPageUrl) ? "" : "http://www.bikewale.com" + nextPageUrl;
    AdPath = "/1017752/Bikewale_Mobile_Model";
    AdId = "1398837216327";
    menu = "9";
  %>
<!-- #include file="/includes/headermobile.aspx" -->
<style type="text/css">
    img {border:none!important;margin:0px !important;padding:0px !important;}
</style>
<form id="form1" runat="server">
    <div class="padding5">
        <div id="br-cr">
            <a href="/m/new/" class="normal">New Bikes</a> &rsaquo;  
            <a href="/m/<%=objModelEntity.MakeBase.MaskingName %>-bikes/" class="normal"><%=objModelEntity.MakeBase.MakeName %></a> &rsaquo; 
            <a href="/m/<%=objModelEntity.MakeBase.MaskingName %>-bikes/<%= objModelEntity.MaskingName %>/" class="normal"><%= objModelEntity.ModelName %></a> &rsaquo; 
            <span class="lightgray">User Reviews</span>
        </div>
        <div class="new-line5">
              <h1><%= objModelEntity.MakeBase.MakeName  + " " + objModelEntity.ModelName%>  User Reviews (<%=totalReviews %>)</h1>
        </div>
        <div id="divModel" class="box1 new-line5">
	        <div class="normal f-12" style="text-decoration:none;">
		        <table style="width:100%;" cellpadding="0" cellspacing="0">
				    <tr>
					    <td style="width:100px;vertical-align:top;margin-left:5px;">
                            <img alt="<%= objModelEntity.MakeBase.MaskingName + " " + objModelEntity.ModelName %> Reviews" title="<%= objModelEntity.MakeBase.MaskingName + " " + objModelEntity.ModelName %> Reviews" src="<%= Bikewale.Common.MakeModelVersion.GetModelImage( objModelEntity.HostUrl, "/bikewaleimg/models/" + objModelEntity.LargePicUrl) %>" width="100">
                            <div class="darkgray"><b><%=!objModelEntity.New && objModelEntity.Used ? "Last Recorded Price: " : "Starts At: " %> </b></div>
                            <div class="darkgray"><b>Rs. <%=Bikewale.Common.CommonOpn.FormatPrice(objModelEntity.MinPrice.ToString()) %></b></div>
                        </td>
					    <td valign="top">
                            <table style="width:100%">
                                    <tr>
                                        <td style="width:110px;" class="darkgray"><span style="position:relative;top:2px;"><b>Overall Average</b></span></td>
                                        <td style="font-size:0px;"> 
                                            <%= Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble(objRating.OverAllRating))%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="darkgray new-line5"><span style="position:relative;top:2px;">Looks</span></td>
                                        <td style="font-size:0px;">
                                            <%= Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble(objRating.StyleRating))%>
                                        </td>
                                    </tr>
                                        <tr>
                                        <td class="darkgray new-line"><span style="position:relative;top:2px;">Performance</span></td>
                                        <td style="font-size:0px;">
                                            <%= Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble(objRating.PerformanceRating))%>     
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="darkgray new-line"><span style="position:relative;top:2px;">Space/Comfort</span></td>
                                        <td style="font-size:0px;">
                                            <%= Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble(objRating.ComfortRating))%>    
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="darkgray new-line"><span style="position:relative;top:2px;">Fuel Economy</span></td>
                                        <td style="font-size:0px;">
                                            <%= Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble(objRating.FuelEconomyRating))%>   
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="darkgray new-line"><span style="position:relative;top:2px;">Value For Money</span></td>
                                        <td style="font-size:0px;">
                                            <%= Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble(objRating.ValueRating))%>   
                                        </td>
                                   </tr>
                            </table>
                        </td>
				    </tr>
		        </table>
	        </div>
        </div>
        <%if(totalReviews > 0) { %>   
        <div>
            <div class="new-line5">
                <h2>All Reviews (<%= totalReviews %>)</h2>
            </div>
            <div id="allReviews" class="box new-line5" style="padding:0px 5px;">
                <asp:Repeater id="rptUserReviews" runat="server">
                    <itemtemplate>
                        <a href='/m/<%= objModelEntity.MakeBase.MaskingName %>-bikes/<%= objModelEntity.MaskingName %>/user-reviews/<%#DataBinder.Eval(Container.DataItem,"ReviewId")%>.html' class="normal f-12">   
                            <div class="container">
                                <div class="sub-heading">
				                    <b><%#DataBinder.Eval(Container.DataItem,"ReviewTitle") %></b> &nbsp;&nbsp;<span class="arr-small">&raquo;</span>
			                    </div>    
	                            <div class="darkgray new-line5">
		                            <%# Bikewale.Common.CommonOpn.GetDisplayDate(DataBinder.Eval(Container.DataItem,"ReviewDate").ToString()) %> | By <%# DataBinder.Eval(Container.DataItem,"WrittenBy") %>
	                            </div>
                                <div class="new-line5" style="font-size:0px;">
                                    <%# Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble(DataBinder.Eval(Container.DataItem,"OverAllRating.OverAllRating"))) %>            
                                </div>
                            </div>
                        </a>                
                    </itemtemplate>
                </asp:Repeater>
            </div>
        </div>
        <%} %>
        <Pager:Pager id="listPager" runat="server"></Pager:Pager>
    </div>
</form>
<!-- #include file="/includes/footermobile.aspx" -->

