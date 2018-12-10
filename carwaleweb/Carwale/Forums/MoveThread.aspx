<%@ Page Language="C#" Inherits="Carwale.UI.Forums.MoveThread" validateRequest="false" trace="false" AutoEventWireup="false" %>
<form runat="server">
<h3>Move Thread</h3>
<asp:Label ID="lblMessage" ForeColor="#FF0000" runat="server" EnableViewState="false" CssClass="error" />
<table cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td valign="top">
			<table width="100%" border="0" cellpadding="0" cellspacing="0">
				<tr>
					<td>Title : </td>
					<td><asp:TextBox ID="txtTitle" Columns="20" runat="server" /></td>
				</tr>
				<tr>
					<td>Category : </td>
					<td><asp:DropDownList ID="cmbCategories" runat="server" /></td>
				</tr>
				<tr>
					<td colspan="2" align="center">
						<asp:Button ID="butSave" runat="server" Text="Update" CssClass="buttons" />
						<input type="button" value="Cancel" onClick="window.close();" class="buttons" />
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>
</form>
<script type="text/javascript">
    Common.showCityPopup = false;
</script> 
<!-- Footer ends here -->
