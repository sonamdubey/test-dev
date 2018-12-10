<%@ Control Language="C#" Inherits="MobileWeb.Controls.PagePosts" AutoEventWireUp="false"%>
<asp:Repeater ID="rptPosts" runat="server">
	<itemtemplate>
		<div class="box new-line5 bot-rad-0" id="post<%# DataBinder.Eval(Container.DataItem, "ID").ToString() %>">
			<div><span class="sub-heading"><%# DataBinder.Eval(Container.DataItem, "PostedBy").ToString() %></span><%# DataBinder.Eval(Container.DataItem, "City").ToString() != "" ? ", " + DataBinder.Eval(Container.DataItem, "City").ToString() : "" %></div>
			<div class="new-line lightgray"><%# GetUserTitle( DataBinder.Eval(Container.DataItem, "Role").ToString(), DataBinder.Eval(Container.DataItem, "Posts").ToString(), DataBinder.Eval(Container.DataItem, "BannedCust").ToString() ) %></div>
			<div class="new-line lightgray"><%# DataBinder.Eval(Container.DataItem, "Posts").ToString() %> Posts | <%# DataBinder.Eval(Container.DataItem, "ThanksReceived").ToString() %> Likes</div>
		</div>	
		<div class="box-bot" style="padding:10px;">
			<div><%# GetDateTime( DataBinder.Eval(Container.DataItem, "MsgDateTime").ToString()) %></div>
			<div class="darkgray" style="padding-top:10px;padding-bottom:10px;" type="PostMessage">
				<%# GetMessage(DataBinder.Eval(Container.DataItem, "Message").ToString()) %>	
			</div>
			<%# DataBinder.Eval(Container.DataItem, "LastUpdatedHandle").ToString() != "anonymous" ? "<div>Last Updated: <br/>" + GetDateTime( DataBinder.Eval(Container.DataItem, "LastUpdatedTime").ToString() ) + ", by " + DataBinder.Eval(Container.DataItem, "LastUpdatedHandle").ToString() + "</div>" : "" %>
			<%# DataBinder.Eval(Container.DataItem, "PostThanksCount").ToString() != "0" ? "<div class='new-line5'>" + DataBinder.Eval(Container.DataItem, "PostThanksCount").ToString() + " member(s) liked the post</div>" : "" %>
			<div class="new-line10">
				<div style="width:32px;height:32px;background-image: url('../images/icons.png');background-position: 2px -158px;float:left;display:none;"></div>
				<div style="width:32px;height:32px;background-image: url('../images/icons.png');background-position: 2px -278px;float:left;margin-left:10px;display:none;"></div>
				<a class="normal" href='/forums/replythread.aspx?thread=<%=Request.QueryString["thread"].ToString()%>&quote=<%# DataBinder.Eval(Container.DataItem, "ID").ToString() %>'>
				<div style="width:32px;height:32px;background-image: url('../images/icons-sheet.png');background-position: -2px -379px;float:left;">&nbsp;</div>
				</a>
				<div style="width:32px;height:32px;background-image: url('../images/icons.png');background-position: 2px -238px;float:left;margin-left:10px;display:none;">&nbsp;</div>
				<div style="clear:both;"></div>
			</div>
		</div>
	</itemtemplate>
</asp:Repeater>
<script language="javascript" type="text/javascript">
	$(document).ready(function(){
		$("div[type='PostMessage'] a").each(function(){
			$(this).removeAttr("href");
			$(this).html($(this).text());
		});
		$("div[type='PostMessage'] img").remove();
	});
</script>