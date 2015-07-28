<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Content.RepeaterPagerReviews" %>
<style>	
	span.pg{padding:2px 5px; border:1px solid #A3B5D9; margin:0 2px;}
	span.pgSel{background-color:#CCDBF8; padding:2px 5px; border:1px solid #A3B5D9; margin:0 2px; color:#5B5B5B; font-weight:bold;}
	span.pgEnd{padding:2px 5px; border:1px solid gray; margin:0 2px; color:gray; cursor:default;}
	.dgNavDiv td{padding:10px 0 0;}
</style>
<table border="0" width="100%" cellpadding="0" cellspacing="0">
	<tr class="dgNavDiv" id="TopPager" runat="server">		
		<td width="130"><asp:label CssClass="headers" ID="lblRecords" runat="server" /></td>
		<td id="pgTop" align="right">			
			<span id="divFirstNav" runat="server"></span><span id="divPages" runat="server" align="center"></span><span id="divLastNav" runat="server"></span>
		</td>		
	</tr>
	<tr>
		<td colspan="4"><asp:Panel ID="pnlGrid" runat="server"></asp:Panel></td>
	</tr>
	<tr class="dgNavDiv" id="BottomPager" runat="server">
		<td><asp:label CssClass="headers" ID="lblRecordsFooter" runat="server" /></td>
		<td id="pgBot" align="right"><span id="divFirstNav1" runat="server"></span><span id="divPages1" runat="server" align="center"></span><span id="divLastNav1" runat="server"></span></td>		
	</tr>
</table>
<script language="javascript">
    function changePageSize(e) {
        var baseUrl = '<%= baseUrlForPs%>';
        var val = e.value;
        location.href = baseUrl + "&ps=" + val + "&pn=1";
    }
</script>	
