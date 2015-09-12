<%@Page validateRequest="false" Inherits="BikeWaleOpr.Content.FeaturedListing" Language="C#" AutoEventWireUp="false" Trace="false" Debug="false" EnableEventValidation="false" %>
<%@ Import Namespace="BikeWaleOpr" %>
<!-- #Include file="/includes/headerNew.aspx" -->
<style>
	.exText td{font-size:9px; text-align:center; color:#969696;}
    .divImgShow {border:1px solid #808080; float:left; padding:5px;}
    .detailShow {float:left; padding-left:20px;}
    .txtWidth { width:30px;}
</style>
<div class="urh">
	You are here &raquo; <a href="/Content/default.aspx">Contents</a> &raquo; Add Featured Listing
</div>
<!-- #Include file="ContentsMenu.aspx" -->
<script language="javascript" src="/src/AjaxFunctions.js"></script>
<div class="left">
	<h3>Add Featured Listing</h3>
	
	  	<table width="100%" border="0" cellpadding="3" cellspacing="0">	
			<tr>
				<td colspan="2">
					<asp:Label ID="lblMessage" CssClass="errorMessage" runat="server"></asp:Label>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Label ID="lblResult" runat="server" Text="" style="color:red;" />
				</td>
			</tr>
    		<tr>
				<td>Bike Name <span style="color: #ff0000">*</span> </td>
				<td nowrap="nowrap">
					<asp:DropDownList ID="drpMake" CssClass="drpClass" runat="server"></asp:DropDownList>
					-
					<asp:DropDownList ID="drpModel" CssClass="drpClass" Enabled="false" runat="server">
						<asp:ListItem Text="--Select--" Value="0" />				
					</asp:DropDownList>
					<input type="hidden" id="hdn_drpModel" runat="server" />
                    <input type="hidden" id="hdn_SelectedModel" runat="server" value="" />
					-
					<asp:DropDownList ID="drpVersion" CssClass="drpClass" Enabled="false" runat="server">
						<asp:ListItem Text="--Select--" Value="0" />				
					</asp:DropDownList>
					<input type="hidden" id="hdn_drpVersion" runat="server" />&nbsp;
                    <input type="hidden" id="hdn_SelectedVersion" runat="server" value="" />
					
					<span id="spnBikeName" style="color:#FF3300; font-weight:bold;"/>&nbsp;
			  	</td>
			</tr>
			<tr>
				<td>Bike Description</td>
				<td>
					<asp:TextBox ID="txtDescription" MaxLength="1000" TextMode="MultiLine" Rows="10" Columns="50" runat="server"></asp:TextBox>
				</td>
			</tr>
			<%--<tr>
				<td>Select Test Drive</td>
				<td>
					<asp:RadioButton GroupName="gdDrive" ID="rdFD" Text="First Drive" runat="server"></asp:RadioButton>
					<asp:RadioButton GroupName="gdDrive" ID="rdRT" Text="RoadTest" runat="server"></asp:RadioButton><br />
					Link <asp:TextBox ID="txtLink" MaxLength="500" Columns="100" runat="server"></asp:TextBox>
				</td>
			</tr>--%>
			<tr>
				<td>Select Photo</td>
				<td>
					<table style="border-collapse:collapse;">
						<tr>
							<td valign="top">
                                <div class="divImgShow">
                                    <% if(!String.IsNullOrEmpty(originalImgPath)){ %>
                                    <img id="imgFLPhoto" src="<%=BikeWaleOpr.ImagingOperations.GetPathToShowImages(hostURL,"227X128",originalImgPath) %>" />
                                    <%} else { %>
                                    <img src="http://img.carwale.com/bikewaleimg/common/nobike.jpg" width="140" height="80"/>
                                    <% } %>
                                </div>
								<div class="detailShow"><input type="file" id="flphoto" accept="image/jpeg" runat="server" />
								<br>Maximum file-size: 1Mb.
                                <br /><span style="color:#f00">Image Size : 200x150</span>
								<br><span id="spnflphoto" class="error" style="color:#f00" /></div>
                                <div class="clear"></div>
							</td>
						</tr>
					</table>
				</td>	
			</tr>
			<tr>
				<td colspan="2"> 
					<asp:CheckBox ID="chkIsModel" Text="Is Model?" Checked="true" runat="server"></asp:CheckBox>&nbsp;
					<asp:CheckBox ID="chkIsVisible" Text="Is Visible?" Checked="true" runat="server"></asp:CheckBox>&nbsp;
					<asp:CheckBox ID="chkIsActive" Text="Is Active?" Checked="true" runat="server"></asp:CheckBox>&nbsp;
					<%--<asp:CheckBox ID="chkIsResearch" Text="Show Research?" Checked="true" runat="server"></asp:CheckBox>&nbsp;
					<asp:CheckBox ID="chkIsPrice" Text="Show OnRoad Price?" Checked="true" runat="server"></asp:CheckBox>&nbsp;--%>
				</td>
			</tr>
			<tr>
			  	<td>
					<asp:Button ID="btnSave" Text="Save" runat="server"></asp:Button>
					<asp:Button ID="btnUpdate" Text="Update" Enabled="false" runat="server"></asp:Button>
                    <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><input type="button" value="Update Priorities" id="btnPriority" />
				</td>
			</tr>
			<tr>
				<td colspan="2">
					<br />
					<div style="color:#FF0000; font-weight:bold; background-color:#FFFFCC; height:25px; font-size:14px;">
						Total no of visible featured listings : <%=visibleCount%>
					</div>
					<hr />
				</td>
			</tr>
			<tr>
				<td colspan="2">
					<asp:DataGrid ID="dtgrdFeaturedListing" runat="server" CssClass="lstTable" 
							
						CellPadding="5" 
						BorderWidth="1" 
						AllowPaging="true" 
						width="100%"
						PagerStyle-Mode="NumericPages" 
						PageSize="20" 
						AllowSorting="false" 
						AutoGenerateColumns="false">
						<itemstyle></itemstyle>
						<headerstyle CssClass="lstTableHeader"></headerstyle>
						<alternatingitemstyle></alternatingitemstyle>
						<edititemstyle></edititemstyle>
						<columns>
							<asp:TemplateColumn HeaderText="SNo">
									<itemtemplate>
										<%= ++serialNo%>
									</itemtemplate>
							</asp:TemplateColumn>
							
							<asp:TemplateColumn HeaderText="Bike Name">
								<itemtemplate>
									<%# DataBinder.Eval( Container.DataItem, "BikeName" ) %>
								</itemtemplate>
							</asp:TemplateColumn>
							
							<asp:TemplateColumn HeaderText="Is Model">
								<itemtemplate>
									<%#GetString(DataBinder.Eval(Container.DataItem,"IsModel").ToString())%> 
								</itemtemplate>
							</asp:TemplateColumn>
							
							<asp:TemplateColumn HeaderText="Is Visible">
								<itemtemplate>
									<%#GetString(DataBinder.Eval(Container.DataItem,"IsVisible").ToString())%> 
								</itemtemplate>
							</asp:TemplateColumn>
							
							<asp:TemplateColumn HeaderText="Is Active">
								<itemtemplate>
									<%#GetString(DataBinder.Eval(Container.DataItem,"IsActive").ToString())%> 
								</itemtemplate>
							</asp:TemplateColumn>
							
							<asp:TemplateColumn HeaderText="Image">
								<itemtemplate>
                                    <img class="<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsReplicated")) ? "" : "checkImage" %>" image-id='<%# DataBinder.Eval( Container.DataItem, "ID" ) %>' src="<%# ImagingOperations.GetPathToShowImages( DataBinder.Eval(Container.DataItem,"HostURL").ToString(),"144X81",DataBinder.Eval(Container.DataItem,"OriginalImagePath").ToString())%>" width="144" alt="<%# DataBinder.Eval( Container.DataItem, "BikeName" ) %>" />
									<%--<img src="<%# GetImage( DataBinder.Eval(Container.DataItem,"Id").ToString())%>" width="75" height="50" alt="<%# DataBinder.Eval( Container.DataItem, "BikeName" ) %>" />--%>
								</itemtemplate>
							</asp:TemplateColumn>
							
                            <asp:TemplateColumn HeaderText="Priority">
								<itemtemplate>
                                    <input class="txtWidth priority" maxlength="4" type="text" id="txtPriority" value='<%# DataBinder.Eval(Container.DataItem,"DisplayPriority").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem,"DisplayPriority") %>' featureId='<%# DataBinder.Eval(Container.DataItem,"ID") %>' />
								</itemtemplate>
							</asp:TemplateColumn>

							<asp:TemplateColumn ItemStyle-Width="30">
								<itemtemplate> <a title="Click to Edit" href="FeaturedListing.aspx?UpdateId=<%# DataBinder.Eval( Container.DataItem, "ID" ) %>" style="cursor:pointer; "><img src="http://opr.carwale.com/images/edit.jpg" border="0" alt="edit"/></a> </itemtemplate>
							</asp:TemplateColumn>
								
						</columns>			
					</asp:DataGrid>	
				</td>
			</tr>
			<%--<tr>
				<td colspan="2">
					<div style="border:1px solid;padding:10px;">
						Clicking the below button will update recent featured Bike on the home page of BikeWale.com.<br/>
						So make sure you really want to update featured bike on home page before clicking it.<br/>
						<asp:Button ID="btnUpdateFeaturedBike" runat="server" Text="Update featured bike to home page" /><br/>
					</div>
				</td>
			</tr>--%>
  		</table>
		
	
</div>

<script language="javascript">
    var refreshTime = 2000;

	if(document.getElementById("btnSave"))
	{
		document.getElementById("btnSave").onclick = btnSave_Click
	}
	if(document.getElementById("btnUpdate"))
	{
		document.getElementById("btnUpdate").onclick = btnSave_Click
	}
	
	document.getElementById("drpMake").onchange = drpMake_Change;
	document.getElementById("drpModel").onchange = drpModel_Change;
	
	$('#btnSave').click(function () {
	    if (isValidImg)
	        return isValidImg;
	    else {
	        alert("Image max size should be 1 MB");
	        return false;
	    }
	})
	$('#btnUpdate').click(function () {
	    if (isValidImg)
	        return isValidImg;
	    else {
	        alert("Image max size should be 1 MB");
	        return false;
	    }
	})
	$('#flphoto').change(function (e) {
	    isValidImg = true;
	    $("#divAlertMsg").text("");
	    $("#divAlertMsg").visible = false;
	    var f = this.files[0];

	    if (f.size > 1000141 || f.fileSize > 1000141) {
	        $("#spnflphoto").visible = true;
	        $("#spnflphoto").text("Maximum image size exceeded.");
	        isValidImg = false;
	        return false;
	    }
	    else
	        return true;
	})

	function btnSave_Click()
	{
		var isError = false;
		
		if( document.getElementById("drpModel").selectedIndex == 0 )
		{
			document.getElementById("spnBikeName").innerHTML = "Select Model";
			isError = true;
		}
		else
			document.getElementById("spnBikeName").innerHTML = "";
			
		if( document.getElementById("chkIsModel").checked == false )
		{
			if( document.getElementById("drpVersion").selectedIndex == 0 )
			{
				document.getElementById("spnBikeName").innerHTML = "Select Version";
				isError = true;
			}
			else
				document.getElementById("spnBikeName").innerHTML = "";
		}

		//if ($("#flphoto").val() == "") {
		//    $("#spnflphoto").text("Plase select Photo");
		//    isError = true;
		//} else {
		//    $("#spnflphoto").text("");
		//}
		
		if(isError)
			return false;
	}
	
	function drpMake_Change(e)
	{
		document.getElementById("drpModel").options[0].text = "--Loading--";
		
		if($("#drpMake").val() == "0")
		{
			$("#hdn_SelectedModel").val("");
            $("#hdn_SelectedVersion").val("");
		}

		var makeId = document.getElementById("drpMake").value;
		var response = AjaxFunctions.GetModels(makeId);

		var dependentCmbs = new Array();
		//add the name of the dependent combos on this combo
		
		dependentCmbs[0] = "drpVersion";
		
		//call the function to consume this data
		FillCombo_Callback(response, document.getElementById("drpModel"), "hdn_drpModel" , dependentCmbs);
	}
	
	function drpModel_Change(e)
	{
		document.getElementById("drpVersion").options[0].text = "--Loading--";
		
		var modelId = document.getElementById("drpModel").value;
		var response = AjaxFunctions.GetVersions(modelId);
        
		var objModel = $("#drpModel");
		if (objModel.val() != "0") {
		    $("#hdn_SelectedModel").val(objModel.val() + "|" + objModel.find("option:selected").text());
		} else {
		    $("#hdn_SelectedModel").val("");
		    $("#hdn_SelectedVersion").val("");
		}

		var dependentCmbs = new Array();
		//add the name of the dependent combos on this combo
		
		dependentCmbs[0] = "drpVersion";
		
		//call the function to consume this data
		FillCombo_Callback(response, document.getElementById("drpVersion"), "hdn_drpVersion" , dependentCmbs);
	}

	$("#drpVersion").change(function () {
	    var objVersion = $(this);
	    if (objVersion.val() != "0") {
	        $("#hdn_SelectedVersion").val(objVersion.val() + "|" + objVersion.find("option:selected").text());
	    } else {
	        $("#hdn_SelectedVersion").val("");
	    }
	});
	
	function ConfirmUpdateFeaturedBike()
	{
		var resUpdateFeaturedBike = confirm("Are you sure want to update featured bike to home page ?");
		return 	resUpdateFeaturedBike;
	}

	$(document).ready(function () {
	    var objModel = $("#drpModel");
	    var objVersion = $("#drpVersion");

	    if (objModel.val() != "0") {
	        $("#hdn_SelectedModel").val(objModel.val() + "|" + objModel.find("option:selected").text());
	    } else {
	        $("#hdn_SelectedModel").val("");
	    }

	    if (objVersion.val() != "0") {
	        $("#hdn_SelectedVersion").val(objVersion.val() + "|" + objVersion.find("option:selected").text());
	    } else {
	        $("#hdn_SelectedVersion").val("");
	    }

	    $("#btnPriority").click(function () {
	        var status = checkDuplicate();

	        if (!status)
	            updatePriorities();
	    });

	    setInterval(UpdatePendingMainImage, refreshTime)
	});

	function updatePriorities() {
	    var priorityList = "";

	    $(":text.priority").each(function () {
	        priorityList += $(this).attr("featureId") + ":" + $.trim($(this).val()) + ",";
	    });
	    updatePrioritiesInDb(priorityList);
	}

	function checkDuplicate() {
	    var status = false;
	    var PrioList = '<%=priorityList%>';
	    var count = '<%=count%>';
	    priority = PrioList.split('_');
	    $(":text.priority").each(function () {
	        var text = $(this).val();
	        $(":text.priority").not($(this)).each(function () {
	            if ($(this).val() != "" && $(this).val() != "0") {
	                if (text == $(this).val()) {
	                    status = true;
	                }
	            }
	            for (var i = 0; i < count-1 ; i++) {
	                for (var j = i + 1; j > count; j++) {
	                    if (PrioList.split(',')[i] == PrioList.split(',')[j])
	                        status = true;
	                }
	            }
	        });
	    });
	        if (status == true)
	            alert("Priorities are same for more than one record.");
	        
	        return status;
	}
	    


	function updatePrioritiesInDb(priorityList) {
	    $.ajax({
	        type: "POST",
	        url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
	        data: '{"prioritiesList":"' + priorityList + '"}',
	        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "SetFeaturedBikePriorities"); },
	        success: function (response) {
	            if (response)
	                alert("Priorities updated Succcessfully...");
	            else
	                alert("Failed to update priorities");
	            window.location.replace("/content/featuredlisting.aspx");
	        }
	    });
	}

	function UpdatePendingMainImage() {
	    var event = $(".checkImage");
	    var id = event.attr('image-id');
	    //alert(id);
	    CheckMainImageStatus(event, id);
	}

	function CheckMainImageStatus(event, mainImageId) {
	    var category = 'FEATUREDLISTING';
	    if (mainImageId != undefined) {
	        $.ajax({
	            type: "POST", url: "/AjaxPro/BikeWaleOpr.Common.Ajax.ImageReplication,BikewaleOpr.ashx",
	            data: '{"imageId":"' + mainImageId + '","Category":"' + category + '"}',
	            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "CheckImageStatusByCategory"); },
	            success: function (response) {
	                var ret_response = eval('(' + response + ')');
	                //alert(ret_response.value);
	                var obj_response = eval('(' + ret_response.value + ')');
	                if (obj_response.Table.length > 0) {
	                    for (var i = 0; i < obj_response.Table.length; i++) {
	                        var imgUrlLarge = obj_response.Table[i].HostUrl + "/144X81/" + obj_response.Table[i].OriginalImagePath;

	                        event.attr('src', imgUrlLarge);
	                    }

	                }
	            }
	        });
	    }
	}
	
</script>
<!-- #Include file="/includes/footerNew.aspx" -->