<%@ Page Language="C#" Inherits="BikeWaleOpr.Content.BikeComparisonList" AutoEventWireup="false" Trace="false" Debug="false" EnableEventValidation="false" %>

<!-- #Include file="/includes/headerNew.aspx" -->
<div class="urh">
		You are here &raquo; <a href="/content/default.aspx">Contents</a> &raquo; Bike Comparison List
</div>
<div>
    <!-- #Include file="contentsMenu.aspx" -->
</div>
<style>
    .Font_11 { font-size :11px;}
    .txtWidth { width:30px;}
    #imgCompPhoto { border:1px solid #000000; padding:5px; float:left; margin-top:5px;}
    .ddlWidth { width:145px;}
    .border { border: 1px solid #000000;}
    .topMargin { margin-top:20px;}
    .centreAlign { text-align:center;}
</style>
<script type="text/javascript" src="/src/AjaxFunctions.js"></script>
    <div class="left">
	<h3>Bike Comparison List</h3><br />

            <fieldset style="white-space:nowrap;width:634px; float:left;">
			<legend>Add Bikes to Compare</legend>
		<table style="height:180px;">
			<tr>
				<td>Select Bike 1 <font color="red">*</font></td>
				<td>
					<asp:DropDownList class="ddlWidth" ID="drpMake1" runat="server"></asp:DropDownList>
					<span style="font-weight:bold;color:red;" id="spndrpMake1" class="error" />
			    </td>
                <td>
					<asp:DropDownList ID="drpModel1" Enabled="false" CssClass="drpClass ddlWidth" runat="server">
						<asp:ListItem Text="--Select Model--" Value="-1" />
					</asp:DropDownList>
					<input type="hidden" id="hdn_drpModel1" runat="server" />
                    <span style="font-weight:bold;color:red;" id="spndrpModel1" class="error" />
			    </td>
                <td>
					<asp:DropDownList ID="drpVersion1" Enabled="false" CssClass="drpClass ddlWidth" runat="server">
						<asp:ListItem Text="--Select Version--" Value="-1" />
					</asp:DropDownList>
					<input type="hidden" id="hdn_drpVersion1" runat="server" />
                    <span style="font-weight:bold;color:red;" id="spndrpVersion1" class="error" />
			    </td>
            </tr>
            <tr>
				<td>Select Bike 2 <font color="red">*</font></td>
				<td>
					<asp:DropDownList class="ddlWidth" ID="drpMake2" runat="server"></asp:DropDownList>
					<span style="font-weight:bold;color:red;" id="spndrpMake2" class="error" />
			    </td>
                <td>
					<asp:DropDownList ID="drpModel2" Enabled="false" CssClass="drpClass ddlWidth" runat="server">
						<asp:ListItem Text="--Select Model--" Value="-1" />
					</asp:DropDownList>
					<input type="hidden" id="hdn_drpModel2" runat="server" />
                    <span style="font-weight:bold;color:red;" id="spndrpModel2" class="error" />
			    </td>
                <td>
					<asp:DropDownList ID="drpVersion2" Enabled="false" CssClass="drpClass ddlWidth" runat="server">
						<asp:ListItem Text="--Select Version--" Value="-1" />
					</asp:DropDownList>
					<input type="hidden" id="hdn_drpVersion2" runat="server" />
                    <span style="font-weight:bold;color:red;" id="spndrpVersion2" class="error" />
			    </td>
            </tr>
            <tr>
                <td></td>
                <td><input class="Font_11" type="checkbox" id="chkIsActive" text="IsActive" checked="true" runat="server" /> IsActive</td>
            </tr>
            <tr>
                <td><asp:button ID="btnSave"  text="Save" runat="server" /></td>
                <td><asp:button ID="btnCancel" text="Cancel" runat="server" visible="false"/></td>
                <td><span style="font-weight:bold;color:red;" id="spnbtnErr" class="error" /></td>
			</tr>
        </table></fieldset>
            <fieldset>
                <legend>Upload Compare Bike Photo</legend>
                <table>
                    <tr>
                        <td colspan="2">
                            <img id="imgCompPhoto" src="<%= String.IsNullOrEmpty(originalImgPath) ? "http://img.carwale.com/bikewaleimg/common/nobike.jpg" : BikeWaleOpr.ImagingOperations.GetPathToShowImages(hostUrl,"310X174",originalImgPath)%>" height="110px"/>
                        </td>
                    </tr>
                    <tr>
                        <td>Upload Image : </td>
                        <td ><input type="file" id="filPhoto" runat="server" /></td>
                    </tr>
                    <tr>
                        <td ">
                            Maximum Image Size : 
                        </td>
                        <td style="color:red;">
                            300 x 100 px
                        </td>
                    </tr>
                </table>
            </fieldset>
        </form><br /><br />
        <div class="clear"></div>
        <asp:Repeater id="MyRepeater" runat="server">       
    <HeaderTemplate>
        <table id="tblBikeList" border="1" style="border-collapse:collapse;" cellpadding ="5">
               <tr style="background-color:#D4CFCF;">
                    <th>S.No.</th>  
                    <th>Image</th>                                         
                    <th>Bike1</th>                                            
                    <th>Bike2</th>                                            
                    <th>Entry Date</th>                                            
                    <th>Edit</th>  
                   <th>Priority<br /><a id="lnkPriorities" style="cursor:pointer;text-decoration:underline;">Update</a></th>
                   <th>IsActive</th>   
                   <th>Delete</th>                                       
               </tr>
    </HeaderTemplate>
    <ItemTemplate>
               <tr class="Font_11" id="delete_<%#DataBinder.Eval(Container.DataItem,"ID") %>">
                    <td height="20px"><%--<%# DataBinder.Eval(Container.DataItem,"ID") %>--%><%#Container.ItemIndex+1 %></td>   
                    <td><img id="imgPhoto" image-id='<%#DataBinder.Eval(Container.DataItem,"ID") %>' class='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsReplicated")) ? "": "checkImage" %>' src='<%# BikeWaleOpr.ImagingOperations.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"HostURL").ToString(),"310X174",DataBinder.Eval(Container.DataItem,"OriginalImagePath").ToString()) %>' runat="server" /> </td>                                         
                    <td><%# DataBinder.Eval(Container.DataItem,"Bike1") %></td>                
                    <td><%# DataBinder.Eval(Container.DataItem,"Bike2") %></td>                                          
                    <td><%# DataBinder.Eval(Container.DataItem,"EntryDate") %></td>                                                         
                    <td class="centreAlign"><a href='bikecomparisonlist.aspx?id=<%# DataBinder.Eval(Container.DataItem,"ID")%>'><img border=0 src=http://opr.carwale.com/images/edit.jpg /></a></td> 
                   <td><input class="txtWidth priority" type="text" id="txtPriority" value='<%# DataBinder.Eval(Container.DataItem,"DisplayPriority") %>' compId='<%# DataBinder.Eval(Container.DataItem,"ID") %>' runat="server" /></td>
                   <td class="centreAlign"><span id="status"><%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsActive")) ? "Active" : "Inactive" %></span></td>
                   <td class="centreAlign"><a class="delete" Id='<%# DataBinder.Eval( Container.DataItem, "ID" ) %>' style="cursor:pointer;" ><img src="http://opr.carwale.com/images/icons/delete.ico" border="0"/></a></td>                              
              </tr> 
    </ItemTemplate>
    <FooterTemplate>
         </Table>
    </FooterTemplate>
    </asp:Repeater>
</div>
<script type="text/javascript">
    var refreshTime = 2000;

        if (document.getElementById('btnSave'))
            document.getElementById('btnSave').onclick = btnSave_Click;
        if (document.getElementById('drpMake1'))
            document.getElementById('drpMake1').onchange = drpMake1_Change;
        if (document.getElementById('drpModel1'))
            document.getElementById('drpModel1').onchange = drpModel1_Change;
        if (document.getElementById('drpMake2'))
            document.getElementById('drpMake2').onchange = drpMake2_Change;
        if (document.getElementById('drpModel2'))
            document.getElementById('drpModel2').onchange = drpModel2_Change;

        function btnSave_Click() {
            document.getElementById('spndrpMake1').innerHTML = "";
            document.getElementById('spndrpModel1').innerHTML = "";
            document.getElementById('spndrpVersion1').innerHTML = "";
            document.getElementById('spndrpMake2').innerHTML = "";
            document.getElementById('spndrpModel2').innerHTML = "";
            document.getElementById('spndrpVersion2').innerHTML = "";
            $("#spnbtnErr").text("");

            if (document.getElementById('drpMake1').value == "-1") {
                document.getElementById('spndrpMake1').innerHTML = "Select Make";
                return false;
            }
            if (document.getElementById('drpModel1').value == "-1") {
                document.getElementById('spndrpModel1').innerHTML = "Select Model";
                return false;
            }
            if (document.getElementById('drpVersion1').value == "-1") {
                document.getElementById('spndrpVersion1').innerHTML = "Select Version";
                return false;
            }
            if (document.getElementById('drpMake2').value == "-1") {
                document.getElementById('spndrpMake2').innerHTML = "Select Make";
                return false;
            }
            if (document.getElementById('drpModel2').value == "-1") {
                document.getElementById('spndrpModel2').innerHTML = "Select Model";
                return false;
            }
            if (document.getElementById('drpVersion2').value == "-1") {
                document.getElementById('spndrpVersion2').innerHTML = "Select Version";
                return false;
            }
            if ($("#drpVersion2").val() == $("#drpVersion1").val())
            {
                $("#spnbtnErr").text("Select Different Versions");
                return false;
            }
        }

        function drpMake1_Change(e) {
            var makeId1 = document.getElementById("drpMake1").value;
            var requestType = 'NEW';
            if (makeId1 > 0) {
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                    data: '{"requestType":"' + requestType + '", "makeId":"' + makeId1 + '"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModels"); },
                    success: function (response) {
                        var responseJSON = eval('(' + response + ')');
                        var resObj = eval('(' + responseJSON.value + ')');
                        bindDropDownList(resObj, $("#drpModel1"), "hdn_drpModel1", "--Select Model--");
                    }
                });
            }
            else {
                $("#drpModel1").val("-1").attr("disabled", true);
                $("#drpVersion1").val("-1").attr("disabled", true);
            }
        }

        function drpModel1_Change(e) {
            var requestType = 'NEW';
            var modelId1 = document.getElementById("drpModel1").value;
            if (modelId1 > 0) {
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                    data: '{"requestType":"' + requestType + '", "modelId":"' + modelId1 + '"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetVersions"); },
                    success: function (response) {
                        var responseJSON = eval('(' + response + ')');
                        var resObj = eval('(' + responseJSON.value + ')');
                        bindDropDownList(resObj, $("#drpVersion1"), "hdn_drpVersion1", "--Select Version--");
                    }
                });
            }
            else {
                $("#drpVersion1").val("-1").attr("disabled", true);
            }
        }

        function drpMake2_Change(e) {
            var requestType = 'NEW';
            var makeId2 = document.getElementById("drpMake2").value;
            if (makeId2 > 0) {
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                    data: '{"requestType":"' + requestType + '", "makeId":"' + makeId2 + '"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModels"); },
                    success: function (response) {
                        var responseJSON = eval('(' + response + ')');
                        var resObj = eval('(' + responseJSON.value + ')');
                        bindDropDownList(resObj, $("#drpModel2"), "hdn_drpModel2", "--Select Model--");
                    }
                });
            }
            else {
                $("#drpModel2").val("-1").attr("disabled", true);
                $("#drpVersion2").val("-1").attr("disabled", true);
            }
        }

        function drpModel2_Change(e) {
            var requestType = 'NEW';
            var modelId2 = document.getElementById("drpModel2").value;
            if (modelId2 > 0) {
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                    data: '{"requestType":"' + requestType + '", "modelId":"' + modelId2 + '"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetVersions"); },
                    success: function (response) {
                        var responseJSON = eval('(' + response + ')');
                        var resObj = eval('(' + responseJSON.value + ')');
                        bindDropDownList(resObj, $("#drpVersion2"), "hdn_drpVersion2", "--Select Version--");
                    }
                });
            }
            else {
                $("#drpVersion2").val("-1").attr("disabled", true);
            }
        }
        
        var deleteId;
        $(document).ready(function () {
            $(".delete").click(function () {
                deleteId = $(this).attr("Id");
                confirmDelete();
            });            

            $("#lnkPriorities").click(function () {
                var isNull = CheckNullText();
                var status = checkDuplicate();

                if (!status && !isNull)
                    updatePriorities();
            });

            setInterval(UpdatePendingMainImage, refreshTime)
        });
    
        function confirmDelete() {
            var conf = confirm("Are you ready to delete this Record ?");
            if (conf == true)
            {
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                    data: '{"deleteId":"' + deleteId + '"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "DeleteCompBikeData"); },
                    success: function (response) {
                        $("#delete_" + deleteId).html("<td colspan='9'>This Record is Deleted Successfully.</td>").addClass("yellow");;
                    }
                });
                alert("Record Deleted Sucessfully...");
            }
        }

        function updatePriorities()
        {
            var priorityList = "";

            $(":text.priority").each(function () {
                    priorityList += $(this).attr("compid") + ":" + $.trim($(this).val()) + ",";
            });
            updatePrioritiesInDb(priorityList);
        }

        function checkDuplicate() {
            var status = false;
            $(":text.priority").each(function () {
                var text = $(this).val();
                $(":text.priority").not($(this)).each(function () {
                    if (text == $(this).val()) {
                        status = true;
                    }
                });
            });
            if (status == true)
            alert("Priorities are same for more than one record.");
            return status;
        }

        function CheckNullText() {
            var IsNull = false;
            $(":text.priority").each(function () {
                if ($(this).val() == '' || $(this).val() == 0) {
                    IsNull = true;
                }
            });
            if (IsNull)
            {
                alert("Please enter valid priority.");
            }
            return IsNull;
        }

        function updatePrioritiesInDb(priorityList)
        {
            $.ajax({
                type: "POST",
                url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                data: '{"prioritiesList":"' + priorityList + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "UpdatePriorities"); },
                success: function (response) {
                    alert("Priorities updated Succcessfully...");
                    window.location.replace("/content/bikecomparisonlist.aspx");
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
            var category = 'BIKECOMPARISIONLIST';
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
                                var imgUrlLarge = obj_response.Table[i].HostUrl + "/310X174/" + obj_response.Table[i].OriginalImagePath;

                                event.attr('src', imgUrlLarge);
                            }

                        }
                    }
                });
            }

        }
</script>
<!-- #Include file="/includes/footerNew.aspx" -->