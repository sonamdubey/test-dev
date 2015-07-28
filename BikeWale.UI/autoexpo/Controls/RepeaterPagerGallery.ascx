<%@ Control Language="C#" AutoEventWireUp="false" Inherits="AutoExpo.RepeaterPagerGallery" %>
<style>	
	span.pg{padding:2px 5px; border:1px solid #A3B5D9; margin:0 2px;}
	span.pgSel{background-color:#CCDBF8; padding:2px 5px; border:1px solid #A3B5D9; margin:0 2px; color:#5B5B5B; font-weight:bold;}
	span.pgEnd{padding:2px 5px; border:1px solid gray; margin:0 2px; color:gray; cursor:default;}
	.dgNavDiv td{padding:5px;}
</style>
	<asp:Panel ID="pnlGrid" runat="server"></asp:Panel>

	<div style="padding:10px 0 20px 10px; background-color:#fff;width:100%;text-align:center;">
		<span id="divFirstNav" runat="server"></span>
		<span id="divPages" runat="server" align="center"></span>
		<span id="divLastNav" runat="server"></span>
	</div>

