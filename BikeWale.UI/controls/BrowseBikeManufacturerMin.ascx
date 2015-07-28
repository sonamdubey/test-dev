<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.BrowseBikeManufacturerMin" %>
<%@ Import Namespace="Bikewale.Common" %>
<div id="footer-makes" class="<%=GridClass %>">
    <h2><%=HeaderText %></h2>
    <div class="margin-top10">
        <asp:DataList ID="dltMakes" runat="server" Width="100%" RepeatDirection="Vertical" CellPadding="0">
	        <itemtemplate><div class="grid_2 alpha"><a href="/<%# DataBinder.Eval(Container.DataItem,"MaskingName").ToString() %>-bikes/"><%# DataBinder.Eval(Container.DataItem,"Name") %></a></div></itemtemplate>
        </asp:DataList>
    </div>
</div>
