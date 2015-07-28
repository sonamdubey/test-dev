<%@ Control Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Controls.BikeVideos" %>
<div>
    <asp:Repeater ID="rptVideos" runat="server">
        <itemtemplate>
            <div class="margin-top10"><h3><%# DataBinder.Eval(Container.DataItem,"VideoTitle") %></h3></div>
            <iframe class="youtube-player margin-top10" type="text/html" src="<%# DataBinder.Eval(Container.DataItem,"VideoSrc") %>" width="560" height="315" frameborder="0"></iframe>
        </itemtemplate>
    </asp:Repeater>
</div>