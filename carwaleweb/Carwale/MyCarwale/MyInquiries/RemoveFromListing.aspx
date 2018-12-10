<%@ Page trace="false" Inherits="Carwale.UI.MyCarwale.MyInquiries.RemoveFromListing" AutoEventWireUp="false" Language="C#" %>

<style>
	<!--
	html, body { font-family:Arial, Helvetica, sans-serif; font-size:12px; }
	-->
</style>
<script language="javascript">
	if ( '<%=IsPostBack%>'.toLowerCase() == "true" )
	{
		window.close();
		opener.location.href = opener.location.href;
	}
</script>

<form runat="server">
	<p><asp:Label ID="lblMsg" runat="server" /></p>
	
	<asp:DropDownList ID="drpStatus" runat="server" />
	Comments if any:<br />
	<asp:TextBox TextMode="MultiLine" ID="txtComments" Rows="5" Columns="30" runat="server"	/>
	<p align="center">
		<asp:button ID="btnSave" text="Remove My Car" runat="server" />
		<input type="button" value="Cancel" onclick="javascript:window.close()" />
	</p>
</form>
<script language="javascript">
	document.getElementById('btnSave').onclick = form_Submit;
	
	function form_Submit( e )
	{
		if ( document.getElementById('drpStatus').options[0].selected ) 
		{
			alert("Please choose a reason to continue!");
			document.getElementById('drpStatus').focus();
			return false;
		}
	}	
 </script>