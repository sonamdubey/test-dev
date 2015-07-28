<%@ Page trace="false" Inherits="BikeWaleOpr.Content.NewBikeVersionColors" AutoEventWireUp="false" Language="C#" %>
<%@ Import Namespace="BikeWaleOpr.Common" %> 
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link rel="stylesheet" href="/css/Common.css?V1.1" type="text/css" />
    <style type="text/css">
        .doNotDisplay { display:none; }
        td, tr, table { border-color:white; }
        .padding-10 { padding:10px; }
    </style>
</head>
<body>
<div class="padding-10">
	<form runat="server">
		<h3>Add Bike Colors(Step2) For <asp:Label ID="lblBike" CssClass="errorMessage" runat="server"></asp:Label> </h3>
		<span id="spnError" class="errorMessage" runat="server"></span>
		<br>
		<table border="0">
			<tr>
				<td valign="top">
					<fieldset>
						<legend>Add Color</legend>
						Color
						<asp:TextBox ID="txtColor" MaxLength="50" Columns="15" runat="server" /><br />
						Company Code
						<asp:TextBox ID="txtCode" MaxLength="50" Columns="10" runat="server" /><br />
						Hex Code
						<asp:TextBox ID="txtHexCode" MaxLength="6" Columns="6" runat="server" />
					</fieldset><br><br>
					<asp:Button ID="btnSave" Text="Add Color(s)" runat="server" />
				</td>
				<td valign="top">
					<fieldset>
						<legend>Copy Common Colors</legend>	
						<asp:CheckBoxList ID="chkModelColors" runat="server" />
					</fieldset>	
				</td>
			</tr>
		</table>
		<br>
		<br>
	
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
				<asp:TemplateColumn HeaderText="Color" SortExpression="Name" ItemStyle-Width="300">
					<itemtemplate>
						<%# DataBinder.Eval( Container.DataItem, "Color" ) %>
					</itemtemplate>
					<edititemtemplate>
						<asp:TextBox ID="txtColor" MaxLength="50" Columns="15" Text='<%# DataBinder.Eval( Container.DataItem, "Color" ) %>' runat="server" />
						
					</edititemtemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Company Code" SortExpression="Name" ItemStyle-Width="300">
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
							<img src='<%# CommonOpn.ImagePath%>spacer.gif' height="1" width="20" /> 
						</span>
					</itemtemplate>
				</asp:TemplateColumn>
				<asp:EditCommandColumn EditText="Edit" CancelText="Cancel" UpdateText="Update" />
				<asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
			</columns>
		</asp:DataGrid>
	</form>
</div>
</body>
</html>