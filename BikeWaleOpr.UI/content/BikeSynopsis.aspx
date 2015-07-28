<%@Page Inherits="BikeWaleOpr.Content.AddBikeSynopsis" Language="C#" AutoEventWireUp="false" Trace="false" Debug="false" validateRequest="false" EnableEventValidation="false" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <style type="text/css">
	    .exText td{font-size:9px; text-align:center; color:#969696;}
	    .successMessage { text-align:center; background-color:yellow; }
    </style>
    <script type="text/javascript" language="javascript" src="/src/AjaxFunctions.js"></script>
</head>
<body>
<div class="left">
	<h3>Add Synopsis for <%=bikeName%></h3>
    <asp:Label ID="lbl_success_msg" runat="server" CssClass="successMessage" Visible="false" />
	<form runat="server">
	  	<table width="100%" border="0" cellpadding="3" cellspacing="0">	
			<tr>
				<td colspan="3">
					<asp:Label ID="lblMessage" Visible="false" CssClass="successMessage" runat="server"></asp:Label>
				</td>
			</tr>
			<tr>
				<td>Small Description</td>
				<td>
					<asp:TextBox ID="txtSmallDesc" TextMode="MultiLine" Rows="10" Columns="50" MaxLength="8000" runat="server">
					</asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>Description</td>
				<td>
					<FTB:FreeTextBox ToolbarStyleConfiguration="office2000" 
						id="ftbDescription" Height="200" Width="480"
						JavaScriptLocation="ExternalFile" 
						ButtonImagesLocation="ExternalFile"
						ToolbarImagesLocation="ExternalFile"
						SupportFolder="~/aspnet_client/FreeTextBox/" 
						runat="Server" />
				</td>
			</tr>
			<tr>
				<td>Pros</td>
				<td>
					<asp:TextBox ID="txtPros" Columns="80" MaxLength="500" runat="server"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>Cons</td>
				<td>
					<asp:TextBox ID="txtCons" Columns="80" MaxLength="500" runat="server"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>Ratings</td>
				<td>
					<table>
						<tr>
							<td>Looks</td>
							<td>
								<asp:DropDownList ID="drpLooks" runat="server"></asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td>Performance</td>
							<td>
								<asp:DropDownList ID="drpPerformance" runat="server"></asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td>Fuel Efficiency</td>
							<td>
								<asp:DropDownList ID="drpFuel" runat="server"></asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td>Comfort</td>
							<td>
								<asp:DropDownList ID="drpComfort" runat="server"></asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td>Safety</td>
							<td>
								<asp:DropDownList ID="drpSafety" runat="server"></asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td>Interiors</td>
							<td>
								<asp:DropDownList ID="drpInteriors" runat="server"></asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td>Ride Quality</td>
							<td>
								<asp:DropDownList ID="drpRide" runat="server"></asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td>Handling</td>
							<td>
								<asp:DropDownList ID="drpHandling" runat="server"></asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td>Braking</td>
							<td>
								<asp:DropDownList ID="drpBraking" runat="server"></asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td>Overall</td>
							<td>
								<asp:DropDownList ID="drpOverall" runat="server"></asp:DropDownList>
							</td>
						</tr>
						<tr><td>&nbsp;</td></tr>
						<tr>
							<td colspan="2">
								<asp:Button ID="btnSave" Text="Save" runat="server"></asp:Button>
							</td>
						</tr>
					</table>
				</td>
			</tr>				
  		</table>
	</form>
</div>
<script language="javascript" type="text/javascript">

    $(document).ready(function () 
    {
        //Set Length of comment to 8000
        $('#txtSmallDesc').attr('maxLength', 8000);
    });

</script>
</body>
</html>