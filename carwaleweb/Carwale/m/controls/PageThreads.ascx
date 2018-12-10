<%@ Control Language="C#" AutoEventWireUp="false" Inherits="MobileWeb.Controls.PageThreads" %>
<asp:Repeater ID="rptThreads" runat="server">
	<itemtemplate>
		<%--<a class="normal" href='/m/forums/viewthread-<%# DataBinder.Eval(Container.DataItem, "TopicId").ToString() %>.html' >--%>
        <a class="normal" href='/m/forums/<%# DataBinder.Eval(Container.DataItem, "TopicId").ToString() %>-<%# DataBinder.Eval(Container.DataItem, "Url").ToString() %>.html' style="text-decoration:none" >
		<div class="content-inner-block-10 content-box-shadow rounded-corner2 text-black margin-bottom10">
			<div class="sub-heading">
				<%# DataBinder.Eval(Container.DataItem, "Topic").ToString() %>&nbsp;&nbsp;<span class="arr-small">&raquo;</span>
			</div>
			<div class="lightgray">
				by <%# DataBinder.Eval(Container.DataItem, "HandleName").ToString() %>
			</div>
			<div class="lightgray">
				<%# DataBinder.Eval(Container.DataItem, "Reads").ToString() %> views | <%# Convert.ToString(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Replies").ToString())-1) %> replies
			</div>
		</div>
		</a>
	</itemtemplate>
</asp:Repeater>	
