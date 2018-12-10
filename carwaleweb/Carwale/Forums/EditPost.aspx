<%@ Page Language="C#" Inherits="Carwale.UI.Forums.EditPost" validateRequest="false" trace="false" AutoEventWireup="false" %>
<%@ Register TagPrefix="Vspl" TagName="RTE" src="/Controls/RichTextEditor.ascx" %>
<style type="text/css">
	.bdr {border:1px #D5D5D5 solid;}
	.bdr td{border-right:1px #D5D5D5 solid;}
	.bdr td{border-bottom:1px #D5D5D5 solid;}	
</style>

<form runat="server">
<h3>Edit Forum Post</h3>
<asp:Label ID="lblMessage" ForeColor="#FF0000" runat="server" EnableViewState="false" CssClass="error" />
<table cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td valign="top">
			<table width="100%" border="0" cellpadding="0" cellspacing="0">
				<tr>
					<td class="dtHeader">
						<Vspl:RTE id="rteET" Rows="15" Cols="60" runat="server" />
						<span id="spnDesc"></span><span id="spnDescription" style="color::#CC0000" class="error"></span>
					</td>
				</tr>
				<tr>
					<td class="dtHeader" align="center">
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
	document.getElementById("butSave").onclick = verifyForm;
		
	function verifyForm(e)
	{
		var isError = false;
		
		var desc = tinyMCE.get('rteET_txtContent').getContent();
			
		if( desc == "" )
		{
			isError = true;
			document.getElementById("spnDescription").innerHTML = "&nbsp;Why blank reply? Please do write something!";
		}
		
		else
			document.getElementById("spnDescription").innerHTML = "";
		
		if(isError == true)
			return false;
	}	
	
	function showCharactersLeft(e)
	{
		alert("As");
		var maxSize = 4000;
		var size = tinyMCE.get('rteET_txtContent').getContent().length;
		
		document.getElementById("spnDesc").innerHTML = "Characters Left : " + (maxSize - size);
	}
</script>
<!-- Footer ends here -->
