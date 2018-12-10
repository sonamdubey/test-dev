<%@ Control Language="C#" AutoEventWireUp="false" Inherits="Carwale.UI.Controls.RepeaterPagerDealerStock" Debug="false" %>
<table id="tbl_res" border="0" width="100%" cellpadding="0" cellspacing="0">
	<%--<tr class="dgNavDivTop" id="TopPager" runat="server">
		<td colspan="2"><asp:label CssClass="headers" ID="lblRecords" runat="server" /></td>        
	</tr>--%>
	<tr><td colspan="2"><asp:Panel ID="pnlGrid" runat="server"></asp:Panel></td></tr>	
	<tr class="dgNavDivTop" id="BottomPager" runat="server">
		<td><asp:label CssClass="headers" ID="lblRecordsFooter" runat="server" /></td>
		<td id="pgBot" align="right"><span id="divFirstNav1" runat="server"></span><span id="divPages1" runat="server" align="center"></span><span id="divLastNav1" runat="server"></span></td>				
	</tr>
</table>