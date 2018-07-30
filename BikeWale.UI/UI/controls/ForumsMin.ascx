<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.ForumsMin" %>
<h2>Popular Discussions</h2>
<p class="black-text">Discuss it with BikeWale community members</p>
<div class="dotted-line margin-top5"></div>                
<div class="margin-top10 margin-bottom15">
    <asp:Repeater ID="rptForums" runat="server">
        <HeaderTemplate>
            <ul class="no-std-ul-list">
        </HeaderTemplate>
        <ItemTemplate>        
            <li><a href="/forums/threads.aspx?forum=<%# DataBinder.Eval( Container.DataItem, "ID" ) %>"><%# FormatedTopic(DataBinder.Eval( Container.DataItem, "Topic" ).ToString()) %></a><span class="discussion-link"><%# DataBinder.Eval( Container.DataItem, "ReplyCount" ) %>&nbsp;Replies</span></li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>                
</div>
<div class="dotted-line margin-bottom15"></div>
<div class="margin-top15 margin-bottom15 readmore">
    <a href="/forums/">More</a>
</div>
