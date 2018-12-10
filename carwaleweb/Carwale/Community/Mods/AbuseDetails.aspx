<%@ Page trace="false" Inherits="Carwale.UI.Community.Mods.AbuseDetails" AutoEventWireUp="false" Language="C#" %>
<asp:Repeater ID="rptAbuseDetails" runat="server">
	<headertemplate>
		<table cellpadding="4" cellspacing="0" style="width:580px;border:1px solid #BAD5E9;" >
			<tr>
				<td style="background-color:#BAD5E9;width:200px;font-weight:bold;">Customer</td>
				<td style="background-color:#BAD5E9;width:380px;font-weight:bold;">Comments</td>
			</tr>
	</headertemplate>
	<itemtemplate>
			<tr>
				<td style="border-bottom:1px solid #BAD5E9;border-right:1px solid #BAD5E9;border-left:1px solid #BAD5E9;"><%# DataBinder.Eval(Container.DataItem,"name")%></td>
				<td style="border-bottom:1px solid #BAD5E9;border-right:1px solid #BAD5E9;border-left:1px solid #BAD5E9;"><%# DataBinder.Eval(Container.DataItem,"Comments")%></td>
			</tr>
	</itemtemplate>
	<footertemplate>
		</table>	
	</footertemplate>
</asp:Repeater>