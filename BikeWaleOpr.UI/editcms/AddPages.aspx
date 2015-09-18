<%@ Page AutoEventWireUp="false" Language="C#" Inherits="BikeWaleOpr.EditCms.AddPages"  Trace="false" Debug="false" %>
<%@ Register TagPrefix="Uc" TagName="DispBasicInfo" src="/editcms/DisplayBasicInfo.ascx" %>
<%@ Register TagPrefix="Ec" TagName="EditCmsCommon" src="/editcms/EditCmsCommon.ascx" %>
<!-- #Include file="/includes/headerNew.aspx" -->
<div class="urh">
	<a href="/default.aspx">BikeWale operations</a> &raquo; <a href="/editcms/default.aspx">Editorial Home</a> &raquo; Manage Articles
</div>
<script language="javascript" src="/src/AjaxFunctions.js"></script>
<script language="javascript" type="text/javascript">
	$(document).ready(function(){
		$("#btnUpdate").hide();
	});
	function Validate() {
		$("#lblMessage").text('');
		$("#spnPageName").text('');
		$("#spnPageNo").text('');
		if($("#txtPageName").val() == "") {
			$("#spnPageName").text('Fill Page Name');
			return false;
		}
		if($("#txtPageName").val() == "") {
			$("#spnPageNo").text('Fill Page No');
			return false;
		}
		else {
			var x=$("#txtPageNo").val();
			var anum=/(^\d+$)|(^\d+\.\d+$)/;
			if (anum.test(x))
				return true;
			else {
				$("#spnPageNo").text('Fill numeric in Page No');
				return false;
			}
		}
	}
	
	function fillData(name, number, updateId) {
		$("#txtPageName").val(name);
		$("#txtPageNo").val(number);
		$("#hdnUpdateId").val(updateId);
		$("#lblMessage").text('');
		$("#spnPageName").text('');
		$("#spnPageNo").text('');
		if(updateId == '') {
			$("#btnSave").show();
			$("#btnUpdate").hide();
		}
		else {
			$("#btnSave").hide();
			$("#btnUpdate").show();
		}
	}
	
</script>

<div style="clear:both;">
<div>
		<Ec:EditCmsCommon ID="EditCmsCommon" runat="server" />
	</div>
<div style="float:left;">
	
	<table width="100%" cellpadding="2" cellspacing="3">
			<tr>
				<td colspan="2"><span style="font-weight:bold;color:red;"><asp:Label ID="lblMessage" EnableViewState="false" CssClass="lbl" runat="server"></asp:Label></span></td>
			</tr>
			<tr>
				<td width="80px">Page Name <font color="red">*</font></td>
				<td>
					<asp:TextBox ID="txtPageName" MaxLength="100" runat="server"></asp:TextBox>
					&nbsp;&nbsp;<span style="font-weight:bold;color:red;" id="spnPageName" class="error" />
				</td>
			</tr>
			<tr>
				<td>Page Number <font color="red">*</font></td>
				<td>
					<asp:TextBox ID="txtPageNo" MaxLength="100" runat="server"></asp:TextBox>
					&nbsp;&nbsp;<span style="font-weight:bold;color:red;" id="spnPageNo" class="error" />
				</td>
			</tr>
			<tr>
				<td colspan="2">&nbsp;</td>
			</tr>
			<tr>
				<td colspan="2"><asp:Button ID="btnSave" Text="Save" runat="server" OnClientClick="return Validate()" />
				<asp:Button ID="btnUpdate" Text="Update" runat="server" onClientClick="return Validate()" />
				<input type="button" value="Cancel" onclick="javsacript:fillData('','','')" />
				</td>
				
			</tr>
			<!--<tr>
				<td colspan="2"><div align="right"><asp:LinkButton ID="lnkPhotos" runat="server">Add Photos</asp:LinkButton></div></td>
			</tr>-->
			<tr>
				<td colspan="2"><asp:DataGrid ID="dgPages" runat="server" CssClass="lstTable" 
						CellPadding="5" BorderWidth="1" AllowPaging="false" width="350px"
						PagerStyle-Mode="NumericPages" PageSize="30" AllowSorting="false" AutoGenerateColumns="false">
						<headerstyle CssClass="lstTableHeader"></headerstyle>
						<columns>
							<asp:TemplateColumn HeaderText="Page No" ItemStyle-Width="60px">
								<itemtemplate>
									<%# DataBinder.Eval( Container.DataItem, "Priority" ) %>
								</itemtemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Page Name">
								<itemtemplate>
									<a href="fillpages.aspx?pid=<%# DataBinder.Eval( Container.DataItem, "Id" ) %>&bid=<%=basicId%>"><%# DataBinder.Eval( Container.DataItem, "PageName" ) %></a>
								</itemtemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn ItemStyle-Width="30">
								<itemtemplate><a title="Click to Edit" onclick="javascript:fillData('<%# DataBinder.Eval( Container.DataItem, "PageName" ) %>','<%# DataBinder.Eval( Container.DataItem, "Priority" ) %>','<%# DataBinder.Eval( Container.DataItem, "Id" ) %>')"  style="cursor:pointer; "><img src="../images/edit.jpg" border="0" alt="edit"/></a> </itemtemplate>
							</asp:TemplateColumn>
						</columns>			
					</asp:DataGrid></td>
			</tr>
		</table>
</div>
<div style="float:right;margin-left:15px;margin-top:20px;display:none;">
	<Uc:DispBasicInfo ID="BasicInfo" runat="server" />
</div>
</div>
<input type="hidden" id="hdnUpdateId" runat="server"  />
</form>
