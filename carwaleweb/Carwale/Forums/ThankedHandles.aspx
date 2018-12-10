<%@ Page Language="C#" Inherits="Carwale.UI.Forums.ThankedHandles" AutoEventWireup="false" trace="false" %>
<asp:Repeater ID="rptThankedHandles" runat="server">
	<headertemplate>
		<table cellpadding="4" cellspacing="0" style="width:380px;border:1px solid #BAD5E9;" >
			<tr>
				<td style="background-color:#BAD5E9;width:200px;font-weight:bold;">Member(s)</td>
			</tr>
	</headertemplate>
	<itemtemplate>
			<tr>
				<td style="border-bottom:1px solid #BAD5E9;border-right:1px solid #BAD5E9;border-left:1px solid #BAD5E9;"><a href='/community/members/<%# DataBinder.Eval(Container.DataItem, "HandleName") %>.html'><%# DataBinder.Eval(Container.DataItem,"HandleName")%></a></td>
			</tr>
	</itemtemplate>
	<footertemplate>
		</table>	
	</footertemplate>
</asp:Repeater>
