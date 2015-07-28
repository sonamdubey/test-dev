<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.DiscussIt" %>
<%@ Import Namespace="Bikewale.Common" %>
<div id="divThread" runat="server">
	<div class="gray-block">
		<h2 class="hd2">Comments <span>Latest 10 are shown</span></h2>
		<div class="mid-box">
			<asp:Repeater ID="rptDiscussIt"  runat="server">
				<itemtemplate>
					<p><span class="price2"><%# DataBinder.Eval(Container.DataItem, "PostedBy") %></span> <%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "MsgDateTime")).ToString("on dd MMM, yyyy a't' hh:mm tt") %></p>					
					<p><%# FormatMessage( DataBinder.Eval(Container.DataItem, "Message").ToString() ) %></p>
					<div class="hr-dotted"></div>
				</itemtemplate>
			</asp:Repeater>
		</div>		
		<div class="mid-box"><a class="redirect-rt" href="/forums/viewthread-<%=varThreadid%>.html">Read all comments</a></div><div class="clear"></div>
	</div>
</div>