<%@ Page AutoEventWireUp="false" Language="C#" Inherits="BikeWaleOpr.EditCms.SelectBikes" Trace="false" Debug="false" enableEventValidation="false"%>
<%@ Register TagPrefix="Uc" TagName="DispBasicInfo" src="/editcms/DisplayBasicInfo.ascx" %>
<%@ Register TagPrefix="Ec" TagName="EditCmsCommon" src="/editcms/EditCmsCommon.ascx" %>
<!-- #Include file="/includes/headerNew.aspx" -->
<link rel="stylesheet" type="text/css" href="http://www.carwale.com/css/cw-common-new.v004.css" />
<div class="urh">
	<a href="/default.aspx">Bikewale operations</a> &raquo; <a href="/editcms/default.aspx">Editorial Home</a> &raquo; Manage Articles
</div>

<script language="javascript" type="text/javascript" src="/src/AjaxFunctions.js"></script>
<script language="javascript" type="text/javascript">
$(document).ready(function(){
	
	$("#ddlMake").change(function(){		
		ddlMake_Change(this);
	});
	$("#ddlModel").change(function(){
		ddlModel_Change(this);
		var val = $(this).val();
		$("#hdn_selModel").val(val);
		$("#hdn_selModelName").val($(this).find("option[value='"+val+"']").html());
		
		$("#hdn_selVersion").val("-1");
		$("#hdn_selVersionName").val("");		
	});
	$("#ddlVersion").change(function(){
		var val = $(this).val();
		$("#hdn_selVersion").val(val);
		$("#hdn_selVersionName").val($(this).find("option[value='"+val+"']").html());		
	});
});

function ValidateCombo() {
    var make = $("#ddlMake");
    var model = $("#ddlModel");
    var version = $("#ddlVersion");
    var bikeName;

    if (make.val() <= 0) {
        alert("Select Make");
        return false;
    } else { bikeName = make.find("option[value='" + make.val() + "']").html(); }

    if (model.val() <= 0) {
        alert("Select Model");
        return false;
    } else { bikeName += " " + model.find("option[value='" + model.val() + "']").html(); }

    if (version.val() > 0) {
        bikeName += " " + version.find("option[value='" + version.val() + "']").html();
    }

    var dTable = $("#dtSelBikes").html();
    var start = dTable.indexOf(bikeName);

    if (dTable.indexOf(bikeName) > -1) {
        var temp = dTable.substring(start);
        var len = temp.indexOf("\n");
        if (bikeName.length == len) {
            alert("Bike already added");
            return false;
        }
    }
    return true;
}

//function ValidateCombo(){
//		var make = $("#ddlMake");
//		var model = $("#ddlModel"); 
//		var version = $("#ddlVersion"); 
//		var bikeName;
		
//		if(make.val() <= 0){
//			alert("Select Make");
//			return false;
//        } else { bikeName = make.find("option[value='" + make.val() + "']").html(); }
			
//		if(model.val() <= 0){
//			 alert("Select Model");
//			 return false; 
//        } else { bikeName += " " + model.find("option[value='"+model.val()+"']").html(); }

//		if(version.val() <= 0){ 
//			if($("#hdnVersionSelection").val().toString() == "True") {
//				alert("Select Version"); 
//				return false;
//			}
//		} else { bikeName += " " + version.find("option[value='"+version.val()+"']").html(); }
		
//		alert(bikeName);
//		var dTable = $("#dtSelBikes").html();
//		var start = dTable.indexOf(bikeName);

//		if(dTable.indexOf(bikeName) > -1) { 
//			var temp = dTable.substring(start);
//		    var len = temp.indexOf("\n");
//			if(bikeName.length == len) {
//				alert("Bike already added");
//				return false;
//			}
//		}
//		return true;
//	}
</script>
<form runat="server">
<div style="clear:both;">
<!--	<div style="float:left;">-->
		<div>
			<Ec:EditCmsCommon ID="EditCmsCommon" runat="server" />
		</div>
		<div id="divBikeSelection" runat="server">Category <%=catName%> requires you to select minimum <%= hdnMinBikeSel.Value.ToString()%> Bike(s)</div><br />
		<table cellpadding="5" cellspacing="0" width="1000">
		<tr>
			<td style="width:75px;">Make</td>
			<td style="width:285px;">
				<asp:DropDownList ID="ddlMake" runat="server" style="width:150px;" />
			</td>
			<td style="width:40px;">Tags</td>
			<td rowspan="3" valign="top" style="width:600px;">			
				<asp:TextBox Columns="60" ID="txtTags" Rows="5" runat="server" TextMode="MultiLine"></asp:TextBox>
			</td>
		</td>
		</tr>
		<tr>
			<td style="width:75px;">Model</td>
			<td>
				<asp:DropDownList ID="ddlModel" runat="server" style="width:150px;">
					<asp:ListItem Value="-1" Text="Select Model" />
				</asp:DropDownList>
				<input type="hidden" id="hdn_drpModel" runat="server" />
				<input type="hidden" id="hdn_selModel" runat="server" value="-1" />
				<input type="hidden" id="hdn_selModelName" runat="server" value="" />				
			</td>
			<td></td>
		</td>
		</tr>
		<tr>
			<td style="width:75px;">Version</td>
			<td>
				<asp:DropDownList ID="ddlVersion" runat="server" style="width:150px;">
				<asp:ListItem Value="-1" Text="Select Version" />
				</asp:DropDownList>
				<input type="hidden" id="hdn_drpVersion" runat="server" />
				<input type="hidden" id="hdn_selVersion" runat="server" value="-1" />
				<input type="hidden" id="hdn_selVersionName" runat="server" value="" />
			</td>
			<td></td>
		</tr>
		<tr>
			<td></td>
			<td colspan="2"><asp:Button ID="btnAdd" runat="server" Text="Add bike" OnClientClick="Javascript:return ValidateCombo()" ></asp:Button></td>
			<td>
				<asp:Button ID="btnSaveTags" runat="server" Text="Save Tags"></asp:Button>
				<span> ( Please enter comma (,) seperated values )</span><br />
				<i><span id="lblTags" runat="server"></span></i>
			</td>
		</tr>
		</table>
		<div style="margin-top:40px;">
			<asp:DataGrid ID="dtSelBikes" runat="server" 
			CellPadding="5" 
			BorderWidth="1" 
			DataKeyField="Id"
			AllowPaging="false" 
			width="350"			
			AllowSorting="true" 
			AutoGenerateColumns="false">
			<itemstyle CssClass="dtItem"></itemstyle>
			<headerstyle CssClass="dtHeader"></headerstyle>
			<alternatingitemstyle CssClass="dtAlternateRow"></alternatingitemstyle>
			<edititemstyle CssClass="dtEditItem"></edititemstyle>
			<columns>
				<asp:TemplateColumn HeaderText="Bike Selected" ItemStyle-Width="300px">
					<itemtemplate>
						<%# DataBinder.Eval( Container.DataItem, "MakeName" ) %><%# " " + DataBinder.Eval( Container.DataItem, "ModelName" ) %><%# DataBinder.Eval( Container.DataItem, "VersionName" ) == "" ? "" : " " + DataBinder.Eval( Container.DataItem, "VersionName" ) %>
					</itemtemplate>
				</asp:TemplateColumn>				
				<asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete"  />
			</columns>
			</asp:DataGrid>
		</div>
		<br />
		<div>
			<a href="otherinfo.aspx?bid=<%=basicId%>"><asp:Button ID="btnContinue" runat="server" Text="Continue"></asp:Button></a>	
		</div>
	<input type="hidden" runat="server"  id="hdnMaxBikeSel" value="0" />
	<input type="hidden" runat="server" id="hdnMinBikeSel" value="0"/>
	<input type="hidden" runat="server"  id="hdnVersionSelection" value="false" />
</div>
<div style="min-height:200px;">&nbsp;</div>
</form>

<!-- #Include file="/includes/footerNew.aspx" -->
<script type="text/javascript" language="javascript">
	function ddlMake_Change(ddl){
	    ddl_Make = ddl;
		AjaxFunctions.GetModels(ddl.value, modelCallback);
	}
	
	function ddlModel_Change(ddl){
		ddl_Model = ddl;		
		AjaxFunctions.GetVersions(ddl.value, versionCallback);
	}

	function modelCallback(response) {
		var table = ddl_Make.parentNode.parentNode.parentNode;
		var ddls = table.getElementsByTagName("select");
		dependentCmbs = new Array();
		dependentCmbs[0] = "";
		FillCombo_Callback(response, ddls[1], "hdn_drpModel" , dependentCmbs, "--Select--");
		ddls[2].innerHTML = "<option value='-1'>--Select--</option>";
	}
	
	function versionCallback(response){
		var table = ddl_Model.parentNode.parentNode.parentNode;
		var ddls = table.getElementsByTagName("select");
		dependentCmbs1 = new Array();
		dependentCmbs1[0] = "";
		FillCombo_Callback(response, ddls[2], "hdn_drpVersion" , dependentCmbs1, "--Select--");
	}
	
	
</script>

