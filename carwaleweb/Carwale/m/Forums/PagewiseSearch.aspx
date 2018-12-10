<%@ Page Language="C#" ContentType="text/html" Inherits="MobileWeb.Forums.PagewiseSearch" ResponseEncoding="iso-8859-1" %>
<asp:Repeater ID="rptThreads" runat="server">
	<itemtemplate>
		<%--<a class="normal" href='/m/forums/viewthread-<%# DataBinder.Eval(Container.DataItem, "TopicId").ToString() %>.html' >--%>
        <a class="normal" href='/m/forums/<%# DataBinder.Eval(Container.DataItem, "TopicId").ToString() %>-<%# DataBinder.Eval(Container.DataItem, "PostUrl").ToString() %>/.html' style="text-decoration:none">
		<div class="content-inner-block-10 content-box-shadow rounded-corner2 margin-top10 text-black">
			<div class="sub-heading">
				<%# DataBinder.Eval(Container.DataItem, "Topic").ToString() %>&nbsp;&nbsp;<span class="arr-small">&raquo;</span>
			</div>
			<div class="lightgray new-line">
				by <%# DataBinder.Eval(Container.DataItem, "HandleName").ToString() %>
			</div>
			<div class="lightgray new-line">
				<%# DataBinder.Eval(Container.DataItem, "Reads").ToString() %> views | <%# Convert.ToString(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Replies").ToString())-1) %> replies
			</div>
		</div>
		</a>
	</itemtemplate>
</asp:Repeater>	
<script language="javascript" type="text/javascript">
    Common.showCityPopup = false;
	$(document).ready(function(){
		$("#divLoader").hide();
		$("#pagesContainer div[type='page']").hide();
		$("#pagesContainer div[id='page"+ selPage +"']").show();
	});
</script>
