<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.NewsMin" %>
<div id="divControl" runat="server" class="hide">
    <div class="<%= RecordCount > 0 ? "grey-bg content-block border-radius5 padding-bottom20 margin-top15" : "hide" %>">
    <h2>Bike News</h2>
    <div class="margin-bottom10" style="border-bottom:2px solid #fff; padding-bottom:10px;">
        <div class="margin-bottom10"><a href="/news/<%=basicId %>-<%=url%>.html"><b><%=title%></b></a></div>
        <a class="<%= RecordCount > 0 ? "" : "hide" %>" href="/news/<%=basicId %>-<%=url%>.html"><img class="margin-bottom10" src="<%= Bikewale.Utility.Image.GetPathToShowImages(imagePathCustom,hostUrl,Bikewale.Utility.ImageSize._210x118) %>" width="278" style="border:1px solid #E5E4E4;"/></a>
        <% if(isExpandable == "1") {%>
			<p>
				<%= TruncateDesc(description) %> <span class="text-grey"> <%= GetPubDate(displayDate) %></span>
			</p>	
		<%}%>
    </div>
    <ul class="ul-normal margin-top10">
        <asp:Repeater runat="server" id="rptCarNews">	 
	        <itemtemplate>
		        <li>
			        <a href="/news/<%# DataBinder.Eval(Container.DataItem,"BasicId") %>-<%# DataBinder.Eval(Container.DataItem,"ArticleUrl") %>.html"><b><%# DataBinder.Eval(Container.DataItem, "Title") %></b></a>
			        <% if(isExpandable == "1") {%>
			        <p>
				        <%# TruncateDesc(DataBinder.Eval(Container.DataItem, "Description").ToString()) %> <span class="text-grey"> <%# GetPubDate(DataBinder.Eval(Container.DataItem, "DisplayDate").ToString()) %></span>
			        </p>	
			        <%}%>
		        </li>
	        </itemtemplate>
        </asp:Repeater>
    </ul>
    <asp:Label ID="lblNotFound" runat="server"></asp:Label>
    <div class="readmore right-float"><a href="/news/">More Bike News</a></div>
	<div class="clear"></div>
        </div>
</div>