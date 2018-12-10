<%@ Page trace="false" Inherits="Carwale.UI.MyCarwale.MyInquiries.RenewCar" AutoEventWireUp="false" Language="C#" %>

<style>
	<!--
	html, body { font-family:Arial, Helvetica, sans-serif; font-size:12px; }
	-->
</style>
<script language="javascript">
	if ('<%=IsPostBack%>'.toLowerCase() == 'true' && '<%=errorPanel.Visible%>'.toLowerCase() == "false" )
	{
		window.close();
		opener.location.href = opener.location.href;
	}
</script>
<!-- Header ends here -->
<form runat="server">
	<div style="background-color:#E9E9D1;border:1px solid #B4B461;padding:10px;">
		<asp:Label ID="lblMsg" runat="server" /><br />
	</div>
	<p align="center">
		<asp:button ID="btnSave" text="Renew Ad" runat="server" />
		<input type="button" value="Cancel" onclick="javascript:window.close()" />
	</p>
    <asp:Panel Id="errorPanel" runat="server" visible="false">
        <p align="center" style="color:red;margin-top:20px;font-size:16px;">
            Unable to renew!
        </p>
        Looks like you already have a free listing on CarWale. We're sorry, but as per our Terms & Conditions, you can have only one free listing live on CarWale.
        <br />
        If you wish, you can delete the existing listing and then create a new one.
    </asp:Panel>
</form>
