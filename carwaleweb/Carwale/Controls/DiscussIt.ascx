<%@ Control Language="C#" AutoEventWireUp="false" Inherits="Carwale.UI.Controls.DiscussIt"%>
<%@ Import Namespace="Carwale.UI.Common" %>
<div id="divThread" runat="server">
	<div class="gray-block content-inner-block-10 border-solid">
		<h2 class="hd2">Comments <span class="font11 text-light-text">Latest 10 are shown</span></h2>
		<div class="mid-box margin-top20">
			<asp:Repeater ID="rptDiscussIt"  runat="server">
				<itemtemplate>
					<p><span class="price2"><%# DataBinder.Eval(Container.DataItem, "PostedBy") %></span> <%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "MsgDateTime")).ToString("on dd MMM, yyyy a't' hh:mm tt") %></p>					
					<p><%# FormatMessage( DataBinder.Eval(Container.DataItem, "Message").ToString() ) %></p>
					<div class="hr-dotted"></div>
				</itemtemplate>
			</asp:Repeater>
		</div>		
		<div class="mid-box"><a class="redirect-rt" href="/forums/<%=varThreadid%>-<%= ThreadUrl %>.html">Read all comments</a></div><div class="clear"></div>
	</div>
</div>