<%@ Control Language="C#" AutoEventWireUp="false" Inherits="Carwale.UI.Controls.RepeaterPagerAdvanced" %>
<style>
	.dgNavDiv 
	{ 
		background-color:#F3F3F3; 
		font-weight:bold;
	}
	.dgNavDiv td { padding:3px; font-size:11px;}
	.dgNavDiv td option {font-size:11px;} 
	.dgNavDiv a { font-weight:bold;font-size:11px;}
	.dgNavDiv a:link, .dgNavDiv a:visited, .dgNavDiv a:active {font-weight:bold;font-size:11px;}
	.headers { color:#666666;font-weight:bold;line-height:30px;font-size:12px;}
	.headers span { color:#FF6600; }
	.disabledLinkPager {color:#999999;text-decoration:none;}
	
	.pg {padding:2px;}
	.pgSel {padding:2px;color:#777777}
</style>	
	<table border="0" width="100%" cellpadding="0" cellspacing="0">
		<tr class="dgNavDiv">
			<td>
				<asp:label CssClass="headers" ID="lblRecords" runat="server" />
			</td>
			<td align="right" colspan="3">
				<asp:label CssClass="headers" ID="lblPages" runat="server" />
			</td>
		</tr>
		<tr class="dgNavDiv" id="TopPager" runat="server">
			<td nowrap width="250">Show 
				<asp:DropDownList ID="cmbPageSize" onchange="changePageSize(this)" runat="server">
					<asp:ListItem Text="10" Value="10" />
					<asp:ListItem Text="20" Value="20" Selected="true" />
					<asp:ListItem Text="30" Value="30" />
					<asp:ListItem Text="40" Value="40" />
					<asp:ListItem Text="50" Value="50" />
				</asp:DropDownList> 
				<%=ResultName%> Per Page
			</td>
			<td align="left"><div id="divFirstNav" runat="server"></div></td>
			<td align="center">
				<div id="divPages" runat="server" align="center"></div>
			</td>
			<td align="right"><div id="divLastNav" runat="server"></div></td>
		</tr>
		<tr>
			<td colspan="4">
				<asp:Panel ID="pnlGrid" runat="server">
				
				</asp:Panel>
			</td>
		</tr>
		<tr class="dgNavDiv" id="BottomPager" runat="server">
			<td nowrap width="250">Show 
				<asp:DropDownList ID="cmbPageSize1" onchange="changePageSize(this)" runat="server">
					<asp:ListItem Text="10" Value="10" />
					<asp:ListItem Text="20" Value="20" Selected="true" />
					<asp:ListItem Text="30" Value="30" />
					<asp:ListItem Text="40" Value="40" />
					<asp:ListItem Text="50" Value="50" />
				</asp:DropDownList> 
				<%=ResultName%> Per Page
			</td>
			<td align="left"><div id="divFirstNav1" runat="server"></div></td>
			<td align="center"><div id="divPages1" runat="server" align="center"></div></td>
			<td align="right"><div id="divLastNav1" runat="server"></div></td>
		</tr>
	</table>
<script language="javascript">
	function changePageSize(e){
		var baseUrl = '<%= baseUrlForPs%>';
		var val = e.value;
		location.href = baseUrl + "&ps=" + val + "&pn=1";
	}
</script>	