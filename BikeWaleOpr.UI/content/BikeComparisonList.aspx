<%@ Page Language="C#" Inherits="BikeWaleOpr.Content.BikeComparisonList" AutoEventWireup="false" Trace="false" Debug="false" EnableEventValidation="false" %>

<!-- #Include file="/includes/headerNew.aspx" -->
<div>
    <!-- #Include file="contentsMenu.aspx" -->
</div>
 <style>
    #comparision-bike-selection{position: fixed; right: 20px;}
     .model-img-content {
    width: 110px;
    height: 61px;
    padding: 5px;
    display:inline-block
}
     
    

    table#tblBikeList tr td img:nth-child(even) {
    border-right: 1px solid #ccc;
}

    .image_wrapper{
        width:250px;
        position:relative
    }
     .image_wrapper .vs-image{
         position:absolute;
         left: 112px;
        top: 25px;
    }


    @media screen and (max-width: 600px) {
    div#comparision-bike-selection{
        position:static;
    }

}
 </style>
<div class="left">
    <h1>Bike Comparison List</h1>

    <div id="comparision-bike-selection" class="margin-top10">
        <fieldset >
            <legend><b>Add Bikes to Compare</b></legend>
            <div class="margin-top10 text-bold">
                Select Bike 1 <span style="color: red">*</span>
            </div>
            <div class="margin-top10 margin-bottom10 form-control-box">
                <asp:dropdownlist class="ddlWidth" id="drpMake1" runat="server"></asp:dropdownlist>
                <span class="bwsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please Select Make</div>
            </div>
            <div class="margin-top10 margin-bottom10 form-control-box">
                <asp:dropdownlist id="drpModel1" enabled="false"  runat="server">
                    <asp:ListItem Text="--Select Model--" Value="-1" />
                </asp:dropdownlist>
                <input type="hidden" id="hdn_drpModel1" runat="server" />
                <span class="bwsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please Select Model</div>
            </div>
            <div class="margin-top10 margin-bottom10 form-control-box">
                <asp:dropdownlist id="drpVersion1" enabled="false"  runat="server">
						<asp:ListItem Text="--Select Version--" Value="-1" />
					</asp:dropdownlist>
                <input type="hidden" id="hdn_drpVersion1" runat="server" />
                <span class="bwsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please Select Version</div>
            </div>
            <hr />
            <div class="margin-top10 text-bold">
                Select Bike 2 <span style="color: red">*</span>
            </div>
            <div class="margin-top10 margin-bottom10 form-control-box">
                <asp:dropdownlist class="ddlWidth" id="drpMake2" runat="server"></asp:dropdownlist>
                <span class="bwsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please Select Make</div>
            </div>
            <div class="margin-top10 margin-bottom10 form-control-box">
                <asp:dropdownlist id="drpModel2" enabled="false"  runat="server">
						<asp:ListItem Text="--Select Model--" Value="-1" />
					</asp:dropdownlist>
                <input type="hidden" id="hdn_drpModel2" runat="server" />
                <span class="bwsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please Select Model</div>
            </div>
            <div class="margin-top10 margin-bottom10 form-control-box">
                <asp:dropdownlist id="drpVersion2" enabled="false"  runat="server">
						<asp:ListItem Text="--Select Version--" Value="-1" />
					</asp:dropdownlist>
                <input type="hidden" id="hdn_drpVersion2" runat="server" />
                <span class="bwsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please Select Version</div>
            </div>
            <hr />
            <div class="margin-top10 margin-bottom10">
                <div class="margin-top10 margin-bottom10">
                    <input type="checkbox" id="chkIsActive" text="IsActive" checked="checked" runat="server" /> IsActive
                </div>

                    <asp:button id="btnSave" text="Save" runat="server" /> 

                    <asp:button class="cancel" id="btnCancel" text="Cancel" runat="server" visible="false" />
                

                <div><span style="font-weight: bold; color: red;" id="spnbtnErr" class="error" />
            </div>
        </fieldset>
    </div>
     
    <div class="margin-top10 floatLeft" style="width: 850px; display: inline-block;">
        <table id="tblBikeList" class="table-bordered" cellspacing="0" cellpadding="5">
            <tr class="dtHeader">
                <th>S.No.</th>
                <th>Image</th>
                <th>Bike1</th>
                <th>Bike2</th>
                <th>Entry Date</th>
                <th>Edit</th>
                <th>Priority<br />
                    <a id="lnkPriorities" style="cursor: pointer; text-decoration: underline;color:orange">Update</a></th>
                <th>IsActive</th>
                <th>Delete</th>
            </tr>
            <% ushort index = 0; foreach (var bike in objBikeComps) { %>
               <tr style="text-align:center" class="Font_11" id="delete_<%= bike.ComparisionId %>">
                    <td><%= ++index %></td>   
                    <td >
                        <div class="image_wrapper">
                            <img class="vs-image" src="http://imgd2.aeplcdn.com/0x0/bw/static/design15/comparison-divider.png" /> 
                            <img class="model-img-content" src="<%= BikeWaleOpr.ImagingOperations.GetPathToShowImages(bike.HostUrl1,"110x61",bike.OriginalImagePath1) %>" /> 
                            <img class="model-img-content"  src="<%= BikeWaleOpr.ImagingOperations.GetPathToShowImages(bike.HostUrl2,"110x61",bike.OriginalImagePath2) %>" /> 
                        </div>
                    </td>                                         
                    <td><%= bike.Bike1 %></td>                
                    <td><%= bike.Bike2 %></td>                                          
                    <td><%= bike.EntryDate %></td>                                                         
                    <td class="centreAlign"><a href='bikecomparisonlist.aspx?id=<%= bike.ComparisionId %>'><img border=0 src=http://opr.carwale.com/images/edit.jpg /></a></td> 
                   <td><input class="txtWidth priority" type="text" style="width:25px" id="txtPriority" value="<%= (bike.PriorityOrder > 0) ? bike.PriorityOrder.ToString() : string.Empty %>" compId='<%= bike.ComparisionId %>' /></td>
                   <td class="centreAlign"><span id="status"><%= bike.IsActive ? "Active" : "Inactive" %></span></td>
                   <td class="centreAlign"><a class="delete" Id='<%= bike.ComparisionId %>' style="cursor:pointer;" ><img src="http://opr.carwale.com/images/icons/delete.ico" border="0"/></a></td>                              
              </tr> 
        <% } %>
        </table>
    </div>

</div>
<script type="text/javascript">

    var ddlMake1 = $("#drpMake1"), ddlMake2 = $("#drpMake2");
    var ddlModel1 = $("#drpModel1"), ddlModel2 = $("#drpModel2");
    var ddlVersion1 = $("#drpVersion1"), ddlVersion2 = $("#drpVersion2");

    $('select').each(function () {
        $(this).chosen({ width: "180px", no_results_text: "No matches found!!", search_contains: true });
    })

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

        var isValid = true;

        showHideMatchError(ddlMake1, false); showHideMatchError(ddlMake2, false);
        showHideMatchError(ddlModel1, false); showHideMatchError(ddlModel2, false);
        showHideMatchError(ddlVersion1, false); showHideMatchError(ddlVersion2, false);

        $("#spnbtnErr").text("");

        if (ddlMake1.val() <= 0) {
            isValid = false;
            showHideMatchError(ddlMake1, true);
        }
        else if (ddlModel1.val() <= 0) {
            isValid = false;
            showHideMatchError(ddlModel1, true);
        }
        else if (ddlVersion1.val() <= 0) {
            isValid = false;
            showHideMatchError(ddlVersion1, true);
        }

        if (ddlMake2.val() <= 0) {
            isValid = false;
            showHideMatchError(ddlMake2, true);
        }
        else if (ddlModel2.val() <= 0) {
            isValid = false;
            showHideMatchError(ddlModel2, true);
        }
        else if (ddlVersion2.val() <= 0) {
            isValid = false;
            showHideMatchError(ddlVersion2, true);
        }

        if (ddlVersion1.val()>=0 && ddlVersion1.val() == ddlVersion2.val()) {
            $("#spnbtnErr").text("Select Different Versions");
            isValid =  false;
        }

        return isValid;
    }

    function drpMakeChange(drpMake,drpModel,drpVersion,hdnModelSpan)
    {
        showHideMatchError(drpMake, false);
        var makeId = drpMake.val();
        var requestType = 'NEW';
        if (makeId > 0) {
            $.ajax({
                type: "POST",
                url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                data: '{"requestType":"' + requestType + '", "makeId":"' + makeId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModels"); },
                success: function (response) {
                    var responseJSON = eval('(' + response + ')');
                    var resObj = eval('(' + responseJSON.value + ')');
                    bindDropDownList(resObj, drpModel, hdnModelSpan, "--Select Model--");
                    drpVersion.val("-1").attr("disabled", true);
                    drpModel.trigger('chosen:updated');
                    drpVersion.trigger('chosen:updated');
                }
            });
        }
        else {
            drpModel.val("-1").attr("disabled", true);
            drpVersion.val("-1").attr("disabled", true);
            drpModel.trigger('chosen:updated');
            drpVersion.trigger('chosen:updated');
        }
    }

    function drpMake1_Change(e) {
        drpMakeChange(ddlMake1, ddlModel1, ddlVersion1, "hdn_drpModel1");
    }
    function drpMake2_Change(e) {

        drpMakeChange(ddlMake2, ddlModel2, ddlVersion2, "hdn_drpModel2");
    }

    function drpModelChange(drpModel,drpVersion,hdnVersionSpan)
    {
        showHideMatchError(drpModel, false);
        var requestType = 'NEW';
        var modelId = drpModel.val();
        if (modelId > 0) {
            $.ajax({
                type: "POST",
                url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                data: '{"requestType":"' + requestType + '", "modelId":"' + modelId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetVersions"); },
                success: function (response) {
                    var responseJSON = eval('(' + response + ')');
                    var resObj = eval('(' + responseJSON.value + ')');
                    bindDropDownList(resObj, drpVersion, hdnVersionSpan, "--Select Version--");
                    drpVersion.trigger('chosen:updated');
                }
            });
        }
        else {
            drpVersion.val("-1").attr("disabled", true);
            drpVersion.trigger('chosen:updated');
        }
    }

    function drpModel1_Change(e) {
        drpModelChange(ddlModel1, ddlVersion1, "hdn_drpVersion1");
    }

    

    function drpModel2_Change(e) {
        drpModelChange(ddlModel2, ddlVersion2, "hdn_drpVersion2");
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
    });

    function confirmDelete() {
        var conf = confirm("Are you ready to delete this Record ?");
        if (conf == true) {
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

    function updatePriorities() {
        var priorityList = "";

        $(":text.priority").each(function () {
            priorityList += $(this).attr("compid") + ":" + $.trim($(this).val()) + ",";
        });
        updatePrioritiesInDb(priorityList);
    }

    function checkDuplicate() {
        var objPriorities = {};
        var status = false;
        $(":text.priority").each(function () {
            var text = $(this).val(); 
            if (objPriorities[text])
                status = true;
            else objPriorities[text] = true;

        });
        if (status)
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
        if (IsNull) {
            alert("Please enter valid priority.");
        }
        return IsNull;
    }

    function updatePrioritiesInDb(priorityList) {
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
</script>
<!-- #Include file="/includes/footerNew.aspx" -->
