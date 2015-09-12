<%@ Page Inherits="BikeWaleOpr.Content.NewBikeFeatures" AutoEventWireUp="false" Language="C#" trace="false" Debug="false" enableEventValidation="false" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <style type="text/css">
	<!--
	#prices { border-collapse:collapse; border-color:#cccccc; }
	#prices td { text-align:center; }
	#prices th { padding:4px; background-color:#DDEEFF; }
	#prices input { background-color:#f3f3f3; border:1px solid #dddddd; }
	.vasi { background-color:#DDEEFF; }
	.met{background-color:#FFFF66;}
	-->
</style>
</head>
<body>
<script language="javascript" src="/src/AjaxFunctions.js"></script>
<div class="left">
	<form runat="server">
	<span id="spnError" class="error" runat="server"></span>
	<fieldset>
		<legend>Select Bike</legend>
		<asp:DropDownList ID="cmbMake" runat="server" tabindex="1"/>
		<asp:DropDownList ID="cmbModel" Enabled="false" runat="server" tabindex="2">
			<asp:ListItem Value="0" Text="--Select--" />
		</asp:DropDownList>
		<asp:DropDownList ID="cmbVersion" Enabled="false" runat="server" tabindex="3">
			<asp:ListItem Value="0" Text="--Select--" />
		</asp:DropDownList>
		<asp:Button ID="btnFind" Text="Show" runat="server" tabindex="4" /> 
	</fieldset>	<br><br>

	<asp:Repeater ID="rptFeatures" runat="server" >
		<headertemplate><br>
			<ul>
		</headertemplate>
		<itemtemplate>
				<li><%# DataBinder.Eval( Container.DataItem, "Category" ) %></li>
				<asp:DataGrid
					ID="dgItems" 
					BorderWidth="0" 
					BackColor="#eeeeee" 
					Width="100%"
					DataKeyField="ID"
					runat="server" AutoGenerateColumns="false"
					ShowHeader="false">
					<columns>
						<asp:TemplateColumn ItemStyle-HorizontalAlign="center" ItemStyle-Width="30px">
							<itemtemplate>
								<asp:CheckBox ID="chkFeature" Checked='<%# DataBinder.Eval( Container.DataItem, "IsAvailable" ).ToString().Length > 0 ? true : false %>' runat="server" />
							</itemtemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn>
							<itemtemplate>
								<%# DataBinder.Eval( Container.DataItem, "Feature" ) %>
							</itemtemplate>
						</asp:TemplateColumn>
					</columns>
				</asp:DataGrid>
		</itemtemplate>
		<footertemplate>
			</ul>
		</footertemplate>
	</asp:Repeater><br>
<br>
<asp:Button ID="btnSave" Visible="false" Text="Add Item" runat="server" />
</form>
</div>
<script language="javascript">
	make = document.getElementById('cmbMake');
	model = document.getElementById('cmbModel');
	version = document.getElementById('cmbVersion');
	
	<% if ( IsPostBack ) 
	{ %>
	for ( var i = 0; i < make.options.length; i++ )
	{
		if ( make.options[ i ].value == <%=cmbMake.SelectedValue%> ) make.options[ i ].selected = true;
	}
	cmbMake_OnChange();
	for ( var i = 0; i < model.options.length; i++ )
	{
		if ( model.options[ i ].value == '<%=Request.Form["cmbModel"]%>' ) model.options[ i ].selected = true;
	}
	cmbModel_OnChange();
	for ( var i = 0; i < version.options.length; i++ )
	{
		if ( version.options[ i ].value == '<%=Request.Form["cmbVersion"]%>' ) version.options[ i ].selected = true;
	}
	<%}%>
 </script>
</body>
</html>
