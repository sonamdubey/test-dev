<%@ Page Inherits="BikeWaleOpr.Content.NewBikeModelColors" AutoEventWireUp="false" Language="C#" trace="false" Debug="false" EnableEventValidation="false" %>
<%@ Import Namespace="BikeWaleOpr.Common" %> 
<!-- #Include file="/includes/headerNew.aspx" -->
<script language="javascript" src="/src/AjaxFunctions.js"></script>

<style>
	.doNotDisplay { display:none; }
	td, tr, table { border-color:white; }
</style>
<div class="urh">
	You are here &raquo; <a href="/content/default.aspx">Contents</a> &raquo; Add Bike Model Colors
</div>
<!-- #Include file="ContentsMenu.aspx" -->
<div class="left">
<form runat="server">
	<span id="spnError" class="error" runat="server"></span>
	<fieldset>
		<legend>Select Model</legend>
		<asp:DropDownList ID="cmbMake" runat="server" tabindex="1"/>
		<asp:DropDownList ID="cmbModel" runat="server" tabindex="2">
			<asp:ListItem Value="0" Text="--Select--" />
		</asp:DropDownList>
		<input type="hidden" id="hdn_cmbModel" runat="server"/>
		<asp:Button ID="btnFind" Text="Show Existing Colors" runat="server" tabindex="3" />
		<span class="error" id="selectModel"></span>
	</fieldset>
	<br>
	<fieldset>
		<legend>Add Color</legend>
		Color
		<asp:TextBox ID="txtColor" MaxLength="50" Columns="15" runat="server" tabindex="1" />
		Company Code
		<asp:TextBox ID="txtCode" MaxLength="50" Columns="10" runat="server" tabindex="2" /> 
		Code
		<asp:TextBox ID="txtHexCode" MaxLength="6" Columns="6" runat="server" tabindex="3" /> 
		<asp:Button ID="btnSave" Text="Add Color" runat="server" tabindex="4"/>
	</fieldset>	<br><br>

	<asp:DataGrid ID="dtgrdColors" runat="server" 
			DataKeyField="ID" 
			CellPadding="5" 
			BorderWidth="1" 
			width="100%"
			PagerStyle-Mode="NumericPages" 
			AutoGenerateColumns="false">
		<itemstyle CssClass="dtItem"></itemstyle>
		<headerstyle CssClass="dtHeader"></headerstyle>
		<alternatingitemstyle CssClass="dtAlternateRow"></alternatingitemstyle>
		<edititemstyle CssClass="dtEditItem"></edititemstyle>
		<columns>
			<asp:TemplateColumn HeaderText="Color" ItemStyle-Width="300">
				<itemtemplate>
					<%# DataBinder.Eval( Container.DataItem, "Color" ) %>
				</itemtemplate>
				<edititemtemplate>
					<asp:TextBox ID="txtColor" MaxLength="50" Columns="15" Text='<%# DataBinder.Eval( Container.DataItem, "Color" ) %>' runat="server" />
					<asp:RequiredFieldValidator ControlToValidate="txtColor" ErrorMessage="Required" runat="server" />
				</edititemtemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Company Code" ItemStyle-Width="300">
				<itemtemplate>
					<%# DataBinder.Eval( Container.DataItem, "Code" ) %>
				</itemtemplate>
				<edititemtemplate>
					<asp:TextBox MaxLength="50" Columns="10" ID="txtCode" Text='<%# DataBinder.Eval( Container.DataItem, "Code" ) %>' runat="server" />
				</edititemtemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Hex-Code" ItemStyle-Width="300">
				<itemtemplate>
					<%# DataBinder.Eval( Container.DataItem, "HexCode" ) %>
				</itemtemplate>
				<edititemtemplate>
					<asp:TextBox MaxLength="6" Columns="6" ID="txtHexCode" Text='<%# DataBinder.Eval( Container.DataItem, "HexCode" ) %>' runat="server" />
				</edititemtemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn>
				<itemtemplate>
					<span style='background-color:#<%# DataBinder.Eval( Container.DataItem, "HexCode" )%>'>
						<img src='http://img.carwale.com/images/spacer.gif' height="20" width="20" /> 
					</span>
				</itemtemplate>
			</asp:TemplateColumn>
			<asp:EditCommandColumn EditText="Edit" CancelText="Cancel" UpdateText="Update" />
			<asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
		</columns>
	</asp:DataGrid>
</form>
</div>
<script type="text/javascript" language="javascript">

    document.getElementById("cmbMake").onchange = cmbMake_Change;

    function cmbMake_Change(e) {
        var makeId = document.getElementById("cmbMake").value;
        var response = AjaxFunctions.GetNewModels(makeId);
        
        var dependentCmbs = new Array;
        dependentCmbs[0] = "cmbModel";
        //call the function to consume this data
        FillCombo_Callback(response, document.getElementById("cmbModel"), "hdn_cmbModel", dependentCmbs);
    }

	
	function checkFind( e )
	{
		if ( document.getElementById('cmbMake').options[0].selected ) 
		{
			document.getElementById('selectModel').innerHTML = "Select Make First"; 
			return false;

        }
        else if (document.getElementById('cmbModel').options[0].selected) 
        {
            document.getElementById('selectModel').innerHTML = "Select Model First";
            return false;
        }
		else document.getElementById('selectModel').innerHTML = ""; 
	}
	
	document.getElementById('btnFind').onclick = checkFind;
 </script>
<!-- #Include file="/includes/footerNew.aspx" -->
