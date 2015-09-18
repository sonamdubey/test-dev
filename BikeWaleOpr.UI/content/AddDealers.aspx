<%@ Page Language="C#" Inherits="BikeWaleOpr.Content.AddDealers" AutoEventWireup="false" Trace="false" Debug="false" EnableEventValidation="false" %>

<!-- #Include file="/includes/headerNew.aspx" -->
<div class="urh">
		You are here &raquo; <a href="/content/default.aspx">Contents</a> &raquo; Add Dealers
</div>
<div>
    <!-- #Include file="contentsMenu.aspx" -->
</div>
<script type="text/javascript" src="/src/AjaxFunctions.js"></script>
    <div class="left">
	<h3><asp:label id="lbl" runat="server"></asp:label></h3>
		<table width="100%" border="0" cellpadding="2" cellspacing="0">
			<tr>
				<td colspan="2" align="left"><asp:Label ID="lblMessage" CssClass="lbl" runat="server"></asp:Label></td>
			</tr>
			<tr>
				<td width="13%">Make <font color="red">*</font></td>
				<td width="87%">
					<asp:DropDownList ID="drpMake" runat="server"></asp:DropDownList>
					<span style="font-weight:bold;color:red;" id="spndrpMake" class="error" />
			  </td>
			</tr>
			<tr>
				<td>State-City <font color="red">*</font></td>
				<td>
					<asp:DropDownList ID="drpState" CssClass="drpClass" runat="server" />-
					<asp:DropDownList ID="drpCity" Enabled="false" CssClass="drpClass" runat="server">
						<asp:ListItem Text="--Select City--" Value="-1" />
					</asp:DropDownList>
					<input type="hidden" id="hdn_drpCity" runat="server" />
					<span style="font-weight:bold;color:red;" id="spndrpCity" class="error" />
				 </td>
			</tr>
			<tr>
				<td>Dealer Name</td>
				<td><asp:TextBox ID="txtName" runat="server" MaxLength="100" /></td>  									 
			</tr>
			<tr>
				<td>Address</td>
				<td><asp:TextBox TextMode="MultiLine" ID="txtAddress" runat="server" MaxLength="1000"/></td>					 				
			</tr>
			<tr>
				<td>Pincode</td>
				<td><asp:TextBox ID="txtPincode" runat="server" MaxLength="50" /></td>					  				 
			</tr>
			<tr>
				<td>Contact No</td>
				<td><asp:TextBox ID="txtContact" runat="server" MaxLength="200" /></td>					  				 
			</tr>
			<tr>
				<td>Fax No</td>
				<td><asp:TextBox ID="txtFax" runat="server" MaxLength="50" /></td>					  				 
			</tr>
			<tr>
				<td>EMail Id</td>
				<td><asp:TextBox ID="txtEmail" runat="server" MaxLength="100" /></td>					  				 
			</tr>
			<tr>
				<td>Website</td>
				<td><asp:TextBox ID="txtWebsite" runat="server" MaxLength="100" /></td>					  				 
			</tr>
			<tr>
				<td>Working Hrs.</td>
				<td><asp:TextBox ID="txtWorkingHours" runat="server" MaxLength="100" /></td>					  				 
			</tr>
            <tr>
				<td>Is NCD</td>
				<td><asp:CheckBox ID="cbxIsNcd" runat="server" /></td>					  				 
			</tr>
            <tr>
				<td>Is Active</td>
				<td><asp:CheckBox ID="cbxIsActive" runat="server" checked="true" /></td>					  				 
			</tr>
			<tr><td>&nbsp;</td></tr>
			<tr>
				<td colspan="2"><asp:button ID="btnSave" text="Add" runat="server" /></td>					 				
			</tr>
          	<tr><td>&nbsp;</td></tr>
        </table>
	
</div>

<script language="javascript">

    function btnSave_Click() {
        document.getElementById('spndrpMake').innerHTML = "";
        document.getElementById('spndrpCity').innerHTML = "";

        if (document.getElementById('drpMake').value == "-1") {
            document.getElementById('spndrpMake').innerHTML = "Select Make";
            return false;
        }

        if (document.getElementById('drpCity').value == "-1") {
            document.getElementById('spndrpCity').innerHTML = "Select City";
            return false;
        }
    }

    if (document.getElementById('btnSave'))
        document.getElementById('btnSave').onclick = btnSave_Click;

    document.getElementById("drpState").onchange = drpState_Change;

    function drpState_Change(e) {
        var stateId = document.getElementById("drpState").value;
        var response = AjaxFunctions.GetCities(stateId);

        var dependentCmbs = new Array();

        //call the function to consume this data
        FillCombo_Callback(response, document.getElementById("drpCity"), "hdn_drpCity");
    }

</script>


<!-- #Include file="/includes/footerNew.aspx" -->
