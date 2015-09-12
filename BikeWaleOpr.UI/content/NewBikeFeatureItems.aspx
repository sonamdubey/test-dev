<%@ Page Inherits="BikeWaleOpr.Content.NewBikeFeatureItems" AutoEventWireUp="false" Language="C#" trace="false" Debug="false" enableEventValidation="false" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <style type="text/css">
	.doNotDisplay { display:none; }
</style>
</head>
<body>
<div class="left">
	<h3>New Bike Feature Items</h3>
	<form runat="server">
		<span id="spnError" class="error" runat="server"></span>
		<fieldset>
			<asp:DropDownList ID="cmbMake" runat="server" tabindex="1"/>
			<asp:DropDownList ID="cmbModel" Enabled="false" runat="server" tabindex="2">
				<asp:ListItem Value="0" Text="--Select--" />
			</asp:DropDownList><span class="error" id="selectModel"></span>
			<asp:Button ID="btnFind" Text="Show Existing Features" runat="server" tabindex="3"/>
			
			
			
		</fieldset>	<br>
		<fieldset>
			<legend>Add New Features</legend>
			Category: <asp:DropDownList ID="cmbCategories" runat="server" tabindex="1"/>
			<span class="error" id="spnCategory"></span>
			<br>
			<asp:TextBox ID="txtItems" TextMode="MultiLine" Columns="70" Rows="10" runat="server" tabindex="2" />
			<span class="error" id="writeItems"></span>
			<br><asp:Button ID="btnSave" Text="Add Items" runat="server" tabindex="3" />
		</fieldset><br>
		<asp:DataGrid ID="dtgrdMembers" runat="server" 
				DataKeyField="ID" 
				CellPadding="5" 
				BorderWidth="1" 
				width="100%"
				AllowPaging="false"
				AllowSorting="true" 
				AutoGenerateColumns="false">
			<itemstyle CssClass="dtItem"></itemstyle>
			<headerstyle CssClass="dtHeader"></headerstyle>
			<alternatingitemstyle CssClass="dtAlternateRow"></alternatingitemstyle>
			<edititemstyle CssClass="dtEditItem"></edititemstyle>
			<columns>
				<asp:TemplateColumn HeaderText="Feature" SortExpression="Feature" ItemStyle-Width="400">
					<itemtemplate>
						<%# DataBinder.Eval( Container.DataItem, "Feature" ) %>
					</itemtemplate>
					<edititemtemplate>
						<asp:TextBox ID="txtItem" Text='<%# DataBinder.Eval( Container.DataItem, "Feature" ) %>' runat="server" />
						
					</edititemtemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Category" SortExpression="Category">
					<itemtemplate>
						<%# DataBinder.Eval( Container.DataItem, "Category" ) %>
					</itemtemplate>
					<edititemtemplate>
						<select id="cmbGridCategory" name="cmbGridCategory"></select>
					</edititemtemplate>
				</asp:TemplateColumn>
				
				<asp:TemplateColumn ItemStyle-CssClass="doNotDisplay" HeaderStyle-CssClass="doNotDisplay">
					<itemtemplate>
						<%# DataBinder.Eval( Container.DataItem, "CategoryId" ) %>
					</itemtemplate>
				</asp:TemplateColumn>
				<asp:EditCommandColumn EditText="Edit" CancelText="Cancel" UpdateText="Update" />
				<asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
			</columns>
		</asp:DataGrid>
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
	<%}%>
	
	function checkFind( e )
	{
		if ( document.getElementById('cmbModel').options[0].selected ) 
		{
			document.getElementById('selectModel').innerHTML = "Select Model First"; 
			return false;
		}
		else document.getElementById('selectModel').innerHTML = ""; 
	}
	
	function checkSave( e )
	{
		if ( document.getElementById('cmbModel').options[0].selected ) 
		{
			document.getElementById('selectModel').innerHTML = "Select Model First"; 
			return false;
		}
		else document.getElementById('selectModel').innerHTML = ""; 
		
		if ( document.getElementById('cmbCategories').options[0].selected )
		{
			document.getElementById('spnCategory').innerHTML = "Select Category First!";
			return false;
		}	
		else document.getElementById('spnCategory').innerHTML = "";
		
		if ( document.getElementById('txtItems').value == "" )
		{
			document.getElementById('writeItems').innerHTML = "<br>Write at least one Item!";
			return false;
		}	
		else document.getElementById('writeItems').innerHTML = "";
		
	}
	
	var grdSeg = document.getElementById('cmbGridCategory');
	if ( grdSeg )
	{
		var seg = document.getElementById('cmbCategories');
		for ( var i=1; i<seg.options.length; i++ )
		{
			grdSeg.options[i - 1] = new Option( seg.options[i].text, seg.options[i].value );
			if ( parseInt(grdSeg.parentNode.parentNode.getElementsByTagName('td')[2].innerHTML) == seg.options[i].value )
				grdSeg.options[i - 1].selected = true;
		}
	}
	
	document.getElementById('btnFind').onclick = checkFind;
	document.getElementById('btnSave').onclick = checkSave;
</script>
</body>
</html>
