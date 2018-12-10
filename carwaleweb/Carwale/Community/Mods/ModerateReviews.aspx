<%@ Page trace="false" Inherits="Carwale.UI.Community.Mods.ModerateReviews" AutoEventWireUp="false" Language="C#" %>
<!doctype html>
<html>
<head>
<!-- #include file="/includes/global/head-script.aspx" -->
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
			var title = $("#spnTitleOfReview_" + reviewId).text();
			var car =  $("#spnCarOfReview_" + reviewId).text();

			var reviewObject={reviewId:reviewId.toString(),customerId:customerId.toString(),title:title,car:car};

			$.ajax({
			    type: "POST",
			    url: "/ajaxpro/CarwaleAjax.AjaxUserReviews,Carwale.ashx",
			    data: JSON.stringify(reviewObject),
			    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "ApproveReview"); },
			    success: function (response) {			        
			        var responseJSON = eval('(' + response + ')');
			        if (responseJSON.value == false) alert("Technical Error!!");
			        else {
			            $("tr[rowid='" + reviewId + "']").hide();
			        }
			    }
			});
	
		}
	}
	
	function fnDelete(reviewId){
		var ret = confirm("Do you want to delete the posting?");
		var customerId = $("#spnCustomerIdOfReview_" + reviewId).text();
		var title = $("#spnTitleOfReview_" + reviewId).text();
	    var car =  $("#spnCarOfReview_" + reviewId).text();
	    var reviewObject = {reviewId:reviewId.toString(),customerId:customerId.toString(),title:title,car:car};

		if(ret) {
			$.ajax({
				type: "POST",
				url: "/ajaxpro/CarwaleAjax.AjaxUserReviews,Carwale.ashx",
				data: JSON.stringify(reviewObject),
				beforeSend: function(xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "DeleteReview"); },
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
                             <li><span class="fa fa-angle-right margin-right10"></span>Unverified reviews</li>
                          </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="font30 text-black special-skin-text">List of unverified reviews</h1>
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
				                        <th colspan="4">Title</th>
			                        </tr>
			                        <tr>
				                        <th colspan="4">Ratings</th>
			                        </tr>
			                        <tr>
				                        <th colspan="4">Pros</th>
			                        </tr>
			                        <tr>
				                        <th colspan="4">Cons</th>
			                        </tr>
			                        <tr>
				                        <th colspan="4">Purchased As</th>
			                        </tr>
			                        <tr>
				                        <th colspan="4">Familarity</th>
			                        </tr>
			                        <tr>
				                        <th colspan="4">Mileage</th>
			                        </tr>
			                        <tr>
				                        <th colspan="4">Comments</th>
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
					                        <span id="spnCarOfReview_<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>" style="display:none;"><%# DataBinder.Eval(Container.DataItem,"CarOfReview")%></span>
				                        </td>
				                        <td>
					                        <%# DataBinder.Eval(Container.DataItem,"CustomerName")%>
					                        <span id="spnCustomerIdOfReview_<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>" style="display:none;"><%# DataBinder.Eval(Container.DataItem,"CustomerId")%></span>
				                        </td>	
				                        <td>
					                        <%# Convert.ToDateTime( DataBinder.Eval(Container.DataItem,"EntryDateTime") ).ToString("dd-MMM-yy HH:mm")%>
				                        </td>
			                        </tr>
			                        <tr class="item" rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                        <td colspan="4">
					                        <strong><a href="/Research/userreviews/reviewdetails.aspx?rid=<%#DataBinder.Eval(Container.DataItem,"ReviewId")%>"><%#DataBinder.Eval(Container.DataItem,"Title")%></a></strong>&nbsp;
					                        <span id="spnTitleOfReview_<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>" style="display:none;"><%# DataBinder.Eval(Container.DataItem,"Title")%></span>
				                        </td>
			                        </tr>
			                        <tr class="item" rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                        <td colspan="4">
					                        Style : <%#DataBinder.Eval(Container.DataItem,"StyleR")%>, 
					                        Comfort : <%#DataBinder.Eval(Container.DataItem,"ComfortR")%>, 
					                        Performance : <%#DataBinder.Eval(Container.DataItem,"PerformanceR")%>, 
					                        Value : <%#DataBinder.Eval(Container.DataItem,"ValueR")%>, 
					                        FuelEconomy : <%#DataBinder.Eval(Container.DataItem,"FuelEconomyR")%>, 
					                        Overall : <%#DataBinder.Eval(Container.DataItem,"OverallR")%>
				                        </td>
			                        </tr>
			                        <tr class="item" rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                        <td colspan="4">
					                        <strong>Pros : </strong><%#DataBinder.Eval(Container.DataItem,"Pros")%>&nbsp;
				                        </td>
			                        </tr>
			                        <tr class="item" rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                        <td colspan="4">
					                        <strong>Cons : </strong><%#DataBinder.Eval(Container.DataItem,"Cons")%>&nbsp;
				                        </td>
			                        </tr>
			                        <tr class="item" rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                        <td colspan="4"><strong>Purchased As : </strong> <%#DataBinder.Eval(Container.DataItem,"PurchasedAs")%>&nbsp;</td>
			                        </tr>
			                        <tr class="item" rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                        <td colspan="4"><strong>Familarity : </strong> <%#DataBinder.Eval(Container.DataItem,"Familiarity")%>&nbsp;</td>
			                        </tr>
			                        <tr class="item" rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                        <td colspan="4"><strong>Mileage : </strong>  <%#DataBinder.Eval(Container.DataItem,"Mileage")%>&nbsp;</td>
			                        </tr>
			                        <tr class="item" rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                        <td colspan="4">
					                        <%#DataBinder.Eval(Container.DataItem,"Comments")%>&nbsp;
				                        </td>
			                        </tr>
			                        <tr class="item" rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                        <td colspan="4" align="left">
					                        <a href="/research/userreviews/editreview.aspx?rid=<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>" style="padding-right:20px" data-role="click-tracking" data-event="CWNonInteractive" data-action="edit" data-cat="usermod_moderatereviews" data-label="<%# Regex.Replace((CurrentUser.Id+"_"+CurrentUser.Name+"_"+DateTime.Now.ToString()+"_"+ DataBinder.Eval(Container.DataItem,"ReviewId")),@"\s+","|")%>"><strong>Edit</strong></a>
					                        <input type="button" class="btn btn-orange" id="butApprove" data-role="click-tracking" data-event="CWNonInteractive" data-action="approve" data-cat="usermod_moderatereviews" data-label="<%# Regex.Replace((CurrentUser.Id+"_"+CurrentUser.Name+"_"+DateTime.Now.ToString()+"_"+ DataBinder.Eval(Container.DataItem,"ReviewId")),@"\s+","|")%>"  onClick='fnApprove(<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>)' value="Approve" causesvalidation="false" />
					                        <input type="button" class="btn btn-orange" id="butDelete" data-role="click-tracking" data-event="CWNonInteractive" data-action="delete" data-cat="usermod_moderatereviews" data-label="<%# Regex.Replace((CurrentUser.Id+"_"+CurrentUser.Name+"_"+DateTime.Now.ToString()+"_"+ DataBinder.Eval(Container.DataItem,"ReviewId")),@"\s+","|")%>" onClick='fnDelete(<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>)' value="Delete" causesvalidation="false" /><br>
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
					                        <asp:Label ID="lblCarOfReview" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"CarOfReview")%>' />
					                        <span id="spnCarOfReview_<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>" style="display:none;"><%# DataBinder.Eval(Container.DataItem,"CarOfReview")%></span>
				                        </td>
				                        <td>
					                        <%# DataBinder.Eval(Container.DataItem,"CustomerName")%>
					                        <asp:Label ID="lblCustomerIdOfReview" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"CustomerId")%>' />
					                        <span id="spnCustomerIdOfReview_<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>" style="display:none;"><%# DataBinder.Eval(Container.DataItem,"CustomerId")%></span>
				                        </td>	
				                        <td>
					                        <%# Convert.ToDateTime( DataBinder.Eval(Container.DataItem,"EntryDateTime") ).ToString("dd-MMM-yy HH:mm")%>
				                        </td>
			                        </tr>
			                        <tr rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                        <td colspan="4">
					                        <a href="/Research/userreviews/reviewdetails.aspx?rid=<%#DataBinder.Eval(Container.DataItem,"ReviewId")%>"><strong><%#DataBinder.Eval(Container.DataItem,"Title")%></strong></a>&nbsp;
					                        <asp:Label ID="lblTitleOfReview" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"Title")%>' />
					                        <span id="spnTitleOfReview_<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>" style="display:none;"><%# DataBinder.Eval(Container.DataItem,"Title")%></span>
				                        </td>
			                        </tr>
			                        <tr rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                        <td colspan="4">
					                        Style : <%#DataBinder.Eval(Container.DataItem,"StyleR")%>, 
					                        Comfort : <%#DataBinder.Eval(Container.DataItem,"ComfortR")%>, 
					                        Performance : <%#DataBinder.Eval(Container.DataItem,"PerformanceR")%>, 
					                        Value : <%#DataBinder.Eval(Container.DataItem,"ValueR")%>, 
					                        FuelEconomy : <%#DataBinder.Eval(Container.DataItem,"FuelEconomyR")%>, 
					                        Overall : <%#DataBinder.Eval(Container.DataItem,"OverallR")%>
				                        </td>
			                        </tr>
			                        <tr rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                        <td colspan="4">
					                        <strong>Pros : </strong><%#DataBinder.Eval(Container.DataItem,"Pros")%>&nbsp;
				                        </td>
			                        </tr>
			                        <tr rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                        <td colspan="4">
					                        <strong>Cons : </strong><%#DataBinder.Eval(Container.DataItem,"Cons")%>&nbsp;
				                        </td>
			                        </tr>
			                        <tr rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                        <td colspan="4"><strong>Purchased As : </strong> <%#DataBinder.Eval(Container.DataItem,"PurchasedAs")%>&nbsp;</td>
			                        </tr>
			                        <tr rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                        <td colspan="4"><strong>Familarity : </strong> <%#DataBinder.Eval(Container.DataItem,"Familiarity")%>&nbsp;</td>
			                        </tr>
			                        <tr rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                        <td colspan="4"><strong>Mileage : </strong>  <%#DataBinder.Eval(Container.DataItem,"Mileage")%>&nbsp;</td>
			                        </tr>
			                        <tr rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                        <td colspan="4">
					                        <%#DataBinder.Eval(Container.DataItem,"Comments")%>&nbsp;
				                        </td>
			                        </tr>
			                        <tr rowid='<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>'>
				                        <td colspan="4" align="left">
					                        <a href="/research/userreviews/editreview.aspx?rid=<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>" style="padding-right:20px" data-role="click-tracking" data-event="CWNonInteractive" data-action="edit" data-cat="usermod_moderatereviews" data-label="<%# Regex.Replace((CurrentUser.Id+"_"+CurrentUser.Name+"_"+DateTime.Now.ToString()+"_"+ DataBinder.Eval(Container.DataItem,"ReviewId")),@"\s+","|")%>"><strong>Edit</strong></a>
					                        <input type="button" class="btn btn-orange" id="butApprove" data-role="click-tracking" data-event="CWNonInteractive" data-action="approve" data-cat="usermod_moderatereviews" data-label="<%# Regex.Replace((CurrentUser.Id+"_"+CurrentUser.Name+"_"+DateTime.Now.ToString()+"_"+ DataBinder.Eval(Container.DataItem,"ReviewId")),@"\s+","|")%>"  onClick='fnApprove(<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>)' value="Approve" causesvalidation="false" />
					                        <input type="button" class="btn btn-orange" id="butDelete" data-role="click-tracking" data-event="CWNonInteractive" data-action="delete" data-cat="usermod_moderatereviews" data-label="<%# Regex.Replace((CurrentUser.Id+"_"+CurrentUser.Name+"_"+DateTime.Now+"_"+ DataBinder.Eval(Container.DataItem,"ReviewId")),@"\s+","|")%>" onClick='fnDelete(<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>)' value="Delete" causesvalidation="false" /><br>
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
