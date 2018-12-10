<%@ Page trace="false" Inherits="Carwale.UI.Community.Mods.ModerateAbusedReviews" AutoEventWireUp="false" Language="C#" %>
<!doctype html>
<html>
<head>
<!-- #include file="/includes/global/head-script.aspx" -->
<script  type="text/javascript"  src="/static/src/graybox.js" ></script>

<style>
	.inquiries { border-collapse:collapse; border-color:#eeeeee; }
	.inquiries th { text-align:left;white-space:nowrap;background-color:#777777;color:#ffffff;padding:4px 2px 4px 2px; }
	.inquiries .item { background-color:#f3f3f3; }
	.inquiries .altItem { background-color:#ffffff; }
	.sendMail { background-color:#FFF0E1; border:1px solid orange;padding:5px; }
	
</style>
<script language="javascript">
	function fnApprove(reviewId){
		var ret = confirm("Do you want to Approve the Posting?");
		if(ret) {
			var customerId = $("#spnCustomerIdOfReview_" + reviewId).text();
			
			$.ajax({
				type: "POST",
				url: "/ajaxpro/CarwaleAjax.AjaxUserReviews,Carwale.ashx",
				data: '{"reviewId":"'+ reviewId +'"}',
				beforeSend: function(xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "ApproveAbusedReview"); },
				success: function(response) {											
					var responseJSON = eval('('+ response +')');
					if(responseJSON.value == false) alert("Technical Error!!"); 
					else {
						$("tr[rowid='" + reviewId + "']").hide();
					}
				}
			});
			
		}
	}
	
	function fnDelete(reviewId){
		var ret = confirm("Do you want to delete the posting?");
		if(ret) {
			$.ajax({
				type: "POST",
				url: "/ajaxpro/CarwaleAjax.AjaxUserReviews,Carwale.ashx",
				data: '{"reviewId":"'+ reviewId +'"}',
				beforeSend: function(xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "DeleteAbusedReview"); },
				success: function(response) {											
					var responseJSON = eval('('+ response +')');
					if(responseJSON.value == false) alert("Technical Error!!"); 
					else  {
						$("tr[rowid='" + reviewId + "']").hide();
					}			
				}
			});
		}
	}

	function showAbusedDetails(node)
	{
	    var title=node.data().title;
	    var reviewId=node.data().reviewid;
		var caption = "Abuse details for : " + title;
		var url = "AbuseDetails.aspx?reviewId="+ reviewId;	 
		var applyIframe = false;
		var GB_Html = "";
		  
		GB_show( caption, url, 250, 600, applyIframe, GB_Html );
	}
</script>
</head>
<body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color">
    <form runat="server">
        <!-- #include file="/includes/header.aspx" -->
        <input type="hidden" id="hdnIsPageFromCache" runat="server" />
        <section class="container">
            <div class="grid-12">
                <div class="padding-bottom15 text-center">
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 0, 90, 0, 0, true, 2) %>
                </div>
            </div>
        </section>
        <div class="clear"></div>
        <section class="bg-light-grey padding-top10 padding-bottom20 no-bg-color">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul class="special-skin-text">
                            <li><a href="/">Home</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/community/">Community</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="default.aspx">Moderator's Home</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span>Abused reviews</li>
                           </ul>
                        <div class="clear"></div>
                    </div>
                    

                    <h1 class="font30 text-black special-skin-text">List of abused reviews</h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
                <div class="clear"></div>
                <div class="grid-12">
                    <div class="content-box-shadow content-inner-block-10">
						<span id="spnError" class="error" runat="server"></span>		
	                    <asp:Repeater ID="dtgrdForums" runat="server">
		                    <headertemplate>
		                    <table class="inquiries" width="100%" cellspacing="0" cellpadding="3" border="1">
			                    <tr>
				                    <th rowspan="9">S. No.</th>
				                    <th>Car</th>
				                    <th>Posted By</th>
				                    <th width="100">Date Time</th>
			                    </tr>
			                    <tr>
				                    <th colspan="3">Title</th>
			                    </tr>
			                    <tr>
				                    <th colspan="3">Ratings</th>
			                    </tr>
			                    <tr>
				                    <th colspan="3">Pros</th>
			                    </tr>
			                    <tr>
				                    <th colspan="3">Cons</th>
			                    </tr>
			                    <tr>
				                    <th colspan="3">Purchased As</th>
			                    </tr>
			                    <tr>
				                    <th colspan="3">Familarity</th>
			                    </tr>
			                    <tr>
				                    <th colspan="3">Mileage</th>
			                    </tr>
			                    <tr>
				                    <th colspan="3">Comments</th>
			                    </tr>		
		                    </headertemplate>	
		                    <itemtemplate>
			                    <tr class="item" rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                    <td valign="top" rowspan="9" nowrap="nowrap">
					                    <%# ++recordNo %> 
					                    <span id="spnId_<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>" style="display:none;"><%# DataBinder.Eval(Container.DataItem,"ReviewId")%></span>
				                    </td>
				                    <td>
					                    <%# DataBinder.Eval(Container.DataItem,"Car")%>
				                    </td>
				                    <td>
					                    <%# DataBinder.Eval(Container.DataItem,"CustomerName")%>
				                    </td>	
				                    <td>
					                    <%# Convert.ToDateTime( DataBinder.Eval(Container.DataItem,"EntryDateTime") ).ToString("dd-MMM-yy HH:mm")%>
				                    </td>
			                    </tr>
			                    <tr class="item" rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                    <td colspan="1">
					                    <strong><a href="/Research/userreviews/reviewdetails.aspx?rid=<%#DataBinder.Eval(Container.DataItem,"ReviewId")%>"><%#DataBinder.Eval(Container.DataItem,"Title")%></a></strong>&nbsp;
				                    </td>
				                    <td colspan="2">
					                    <span data-title="<%#DataBinder.Eval(Container.DataItem,"Title")%>" data-reviewid="<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>" onclick="showAbusedDetails($(this));" style="font-weight:bold;color:red;text-decoration:underline;cursor:pointer;">No of abuses : <%#DataBinder.Eval(Container.DataItem,"NoOfAbuses")%></span>
				                    </td>
			                    </tr>
			                    <tr class="item" rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                    <td colspan="3">
					                    Style : <%#DataBinder.Eval(Container.DataItem,"StyleR")%>, 
					                    Comfort : <%#DataBinder.Eval(Container.DataItem,"ComfortR")%>, 
					                    Performance : <%#DataBinder.Eval(Container.DataItem,"PerformanceR")%>, 
					                    Value : <%#DataBinder.Eval(Container.DataItem,"ValueR")%>, 
					                    FuelEconomy : <%#DataBinder.Eval(Container.DataItem,"FuelEconomyR")%>, 
					                    Overall : <%#DataBinder.Eval(Container.DataItem,"OverallR")%>
				                    </td>
			                    </tr>
			                    <tr class="item" rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                    <td colspan="3">
					                    <strong>Pros : </strong><%#DataBinder.Eval(Container.DataItem,"Pros")%>&nbsp;
				                    </td>
			                    </tr>
			                    <tr class="item" rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                    <td colspan="3">
					                    <strong>Cons : </strong><%#DataBinder.Eval(Container.DataItem,"Cons")%>&nbsp;
				                    </td>
			                    </tr>
			                    <tr class="item" rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                    <td colspan="3"><strong>Purchased As : </strong> <%#DataBinder.Eval(Container.DataItem,"PurchasedAs")%>&nbsp;</td>
			                    </tr>
			                    <tr class="item" rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                    <td colspan="3"><strong>Familarity : </strong> <%#DataBinder.Eval(Container.DataItem,"Familiarity")%>&nbsp;</td>
			                    </tr>
			                    <tr class="item" rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                    <td colspan="3"><strong>Mileage : </strong>  <%#DataBinder.Eval(Container.DataItem,"Mileage")%>&nbsp;</td>
			                    </tr>
			                    <tr class="item" rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                    <td colspan="3">
					                    <%#DataBinder.Eval(Container.DataItem,"Comments")%>&nbsp;
				                    </td>
			                    </tr>
			                    <tr class="item" rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                    <td colspan="2"></td>
				                    <td colspan="2"  align="right">
					                    <a href="/research/userreviews/editreview.aspx?rid=<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>" data-role="click-tracking" data-event="CWNonInteractive" data-action="edit" data-cat="usermod_moderateabusedreviews" data-label="<%# Regex.Replace((CurrentUser.Id+"_"+CurrentUser.Name+"_"+DateTime.Now.ToString()+"_"+ DataBinder.Eval(Container.DataItem,"ReviewId")),@"\s+","|")%>"><input type="button" class="btn btn-orange" id="butEdit"  value="Edit" causesvalidation="false" /></a>
					                    <input type="button" class="btn btn-orange" id="butApprove" data-role="click-tracking" data-event="CWNonInteractive" data-action="approve" data-cat="usermod_moderateabusedreviews" data-label="<%# Regex.Replace((CurrentUser.Id+"_"+CurrentUser.Name+"_"+DateTime.Now.ToString()+"_"+ DataBinder.Eval(Container.DataItem,"ReviewId")),@"\s+","|")%>" onClick='fnApprove(<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>)' value="Approve" causesvalidation="false" />
					                    <input type="button" class="btn btn-orange" id="butDelete" data-role="click-tracking" data-event="CWNonInteractive" data-action="delete" data-cat="usermod_moderateabusedreviews" data-label="<%# Regex.Replace((CurrentUser.Id+"_"+CurrentUser.Name+"_"+DateTime.Now.ToString()+"_"+ DataBinder.Eval(Container.DataItem,"ReviewId")),@"\s+","|")%>" onClick='fnDelete(<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>)' value="Delete" causesvalidation="false" /><br>
				                    </td>
			                    </tr>
		                    </itemtemplate>
		                    <alternatingitemtemplate>
			                    <tr rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                    <td valign="top" rowspan="9" nowrap="nowrap">
					                    <%# ++recordNo %> 
					                    <asp:Label ID="lblId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>' />
				                    </td>
				                    <td>
					                    <%# DataBinder.Eval(Container.DataItem,"Car")%>
				                    </td>
				                    <td>
					                    <%# DataBinder.Eval(Container.DataItem,"CustomerName")%>
				                    </td>	
				                    <td>
					                    <%# Convert.ToDateTime( DataBinder.Eval(Container.DataItem,"EntryDateTime") ).ToString("dd-MMM-yy HH:mm")%>
				                    </td>
			                    </tr>
			                    <tr rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                    <td colspan="1">
					                    <a href="/Research/userreviews/reviewdetails.aspx?rid=<%#DataBinder.Eval(Container.DataItem,"ReviewId")%>"><strong><%#DataBinder.Eval(Container.DataItem,"Title")%></strong></a>&nbsp;
				                    </td>
				                    <td colspan="2">
					                    <span  data-title="<%#DataBinder.Eval(Container.DataItem,"Title")%>" data-reviewid="<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>" onclick='showAbusedDetails($(this));' style="font-weight:bold;color:red;text-decoration:underline;cursor:pointer;">No of abuses : <%#DataBinder.Eval(Container.DataItem,"NoOfAbuses")%></span>
				                    </td>
			                    </tr>
			                    <tr rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                    <td colspan="3">
					                    Style : <%#DataBinder.Eval(Container.DataItem,"StyleR")%>, 
					                    Comfort : <%#DataBinder.Eval(Container.DataItem,"ComfortR")%>, 
					                    Performance : <%#DataBinder.Eval(Container.DataItem,"PerformanceR")%>, 
					                    Value : <%#DataBinder.Eval(Container.DataItem,"ValueR")%>, 
					                    FuelEconomy : <%#DataBinder.Eval(Container.DataItem,"FuelEconomyR")%>, 
					                    Overall : <%#DataBinder.Eval(Container.DataItem,"OverallR")%>
				                    </td>
			                    </tr>
			                    <tr rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                    <td colspan="3">
					                    <strong>Pros : </strong><%#DataBinder.Eval(Container.DataItem,"Pros")%>&nbsp;
				                    </td>
			                    </tr>
			                    <tr rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                    <td colspan="3">
					                    <strong>Cons : </strong><%#DataBinder.Eval(Container.DataItem,"Cons")%>&nbsp;
				                    </td>
			                    </tr>
			                    <tr rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                    <td colspan="3"><strong>Purchased As : </strong> <%#DataBinder.Eval(Container.DataItem,"PurchasedAs")%>&nbsp;</td>
			                    </tr>
			                    <tr rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                    <td colspan="3"><strong>Familarity : </strong> <%#DataBinder.Eval(Container.DataItem,"Familiarity")%>&nbsp;</td>
			                    </tr>
			                    <tr rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                    <td colspan="3"><strong>Mileage : </strong>  <%#DataBinder.Eval(Container.DataItem,"Mileage")%>&nbsp;</td>
			                    </tr>
			                    <tr rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                    <td colspan="3">
					                    <%#DataBinder.Eval(Container.DataItem,"Comments")%>&nbsp;
				                    </td>
			                    </tr>
			                    <tr rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                    <td colspan="2"></td>
				                    <td colspan="2" align="right">
					                    <a href="/research/userreviews/editreview.aspx?rid=<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>" data-role="click-tracking" data-event="CWNonInteractive" data-action="edit" data-cat="usermod_moderateabusedreviews" data-label="<%# Regex.Replace((CurrentUser.Id+"_"+CurrentUser.Name+"_"+DateTime.Now.ToString()+"_"+ DataBinder.Eval(Container.DataItem,"ReviewId")),@"\s+","|")%>"><input type="button" class="btn btn-orange" id="butEdit"  value="Edit" causesvalidation="false" /></a>
					                    <input type="button" class="btn btn-orange" id="butApprove" data-role="click-tracking" data-event="CWNonInteractive" data-action="approve" data-cat="usermod_moderateabusedreviews" data-label="<%# Regex.Replace((CurrentUser.Id+"_"+CurrentUser.Name+"_"+DateTime.Now.ToString()+"_"+ DataBinder.Eval(Container.DataItem,"ReviewId")),@"\s+","|")%>" onClick='fnApprove(<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>)' value="Approve" causesvalidation="false" />
					                    <input type="button" class="btn btn-orange" id="butDelete" data-role="click-tracking" data-event="CWNonInteractive" data-action="delete" data-cat="usermod_moderateabusedreviews" data-label="<%# Regex.Replace((CurrentUser.Id+"_"+CurrentUser.Name+"_"+DateTime.Now.ToString()+"_"+ DataBinder.Eval(Container.DataItem,"ReviewId")),@"\s+","|")%>" onClick='fnDelete(<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>)' value="Delete" causesvalidation="false" /><br>
				                    </td>
			                    </tr>
		                    </alternatingitemtemplate>
		                    <footertemplate>
			                    </table>
		                    </footertemplate>
	                    </asp:Repeater>
					</div>
				</div>
        
				<div class="clear"></div>
            </div>
        </section>
        <div class="clear"></div>
        <!-- #include file="/includes/footer.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/global/footer-script.aspx" -->
</form>
</body>
</html>
