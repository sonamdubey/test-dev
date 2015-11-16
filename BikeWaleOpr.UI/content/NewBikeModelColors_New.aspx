<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikewaleOpr.content.NewBikeModelColors_New" EnableEventValidation="false" %>
<%@ Import Namespace="BikeWaleOpr.Common" %>
<!-- #Include file="/includes/headerNew.aspx" -->
<script language="javascript" src="/src/AjaxFunctions.js"></script>

<style>
    .doNotDisplay {
        display: none;
    }

    td, tr, table {
        border-color: white;
    }

    .savedModelColors li {
        margin-top: 10px;
        min-width: 120px;
        float: left;
        text-align: center;
    }

    .addColorToVersions li {
        min-width: 120px;
        float: left;
        text-align: center;
    }

    .inline-block {
        display: inline-block;
        vertical-align: middle;
    }

    .leftfloat {
        float: left;
    }

    .rightfloat {
        float: right;
    }

    .clear {
        clear: both;
    }

    .updateModelColor {
        display: none;
        width: 300px;
        min-height: 200px;
        background: #fff;
        border: 2px solid #222;
        margin: 0 auto;
        position: fixed;
        top: 15%;
        left: 5%;
        right: 5%;
        padding: 0 10px;
    }

        .updateModelColor .closeBtn {
            position: absolute;
            right: 10px;
            top: 10px;
        }

        .updateModelColor table tr td input[type='text'] {
            width: 80px;
            margin-right: 15px;
        }
</style>
<div class="urh">
    You are here &raquo; <a href="/content/default.aspx">Contents</a> &raquo; Add Bike Model Colors
</div>
<!-- #Include file="ContentsMenu.aspx" -->
<div class="left">

    <span id="spnError" class="error" runat="server"></span>
    <fieldset>
        <legend>Select Model</legend>
        <asp:dropdownlist id="cmbMake" runat="server" tabindex="1" />
        <asp:dropdownlist id="cmbModel" runat="server" tabindex="2">
			<asp:ListItem Value="0" Text="--Select--" />
		</asp:dropdownlist>
        <input type="hidden" id="hdn_cmbModel" runat="server" />
        <asp:button id="btnShowColors" text="Show Colors" runat="server" tabindex="3" />
        <span class="error" id="selectModel"></span>
    </fieldset>
    <br>
    <fieldset>
        <legend>Add New Color To Model</legend>
        Color Name
		<asp:textbox runat="server" id="txtColor" maxlength="50" columns="15" runat="server" tabindex="4" />
        <br />
        <br />
        Color Code
		<asp:textbox runat="server" id="txtHexCode1" maxlength="6" columns="6" runat="server" tabindex="5" />
        <asp:textbox runat="server" id="txtHexCode2" maxlength="6" columns="6" runat="server" tabindex="6" />
        <asp:textbox runat="server" id="txtHexCode3" maxlength="6" columns="6" runat="server" tabindex="7" />
        <asp:button id="btnSave" text="Save Model Color" runat="server" tabindex="8" />
        <hr />
        <%
            if (modelColorCount > 0)
            {
                
        %>
        Saved Model Colors:
        <ul class="savedModelColors">
            <asp:repeater id="rptModelColor" runat="server" enableviewstate="false">
                <itemtemplate>
                    <li>
                        <p><%#DataBinder.Eval(Container.DataItem,"Name") %></p>
                        <a href="javascript:openEditColorWindow(<%#DataBinder.Eval(Container.DataItem,"Id") %>, <%= ModelId %>)" class="editBtn">Edit</a>
                    </li>
                </itemtemplate>
            </asp:repeater>
        </ul>
        <%}
            else
            {
        %>
        <span>No content.</span>
        <%} %>
        <div class="clear"></div>
        <hr />
        <%
            if (modelColorCount > 0)
            {
                
        %>
        Add color to model versions:
        <div class="addColorToVersions">
            <asp:repeater id="rptVersionColor" runat="server" enableviewstate="false">
                <ItemTemplate>
                    <div class="addColorToVersionsBox">
                        <span class="inline-block" style="width:120px;"><%# DataBinder.Eval(Container.DataItem,"VersionName") %></span>                        
                        <asp:HiddenField id="BikeVersionId" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"VersionId") %>' />
                        <ul class="inline-block">
                            <asp:Repeater ID="rptColor" runat="server" enableviewstate="false">
                                <ItemTemplate>
                                    <li>
                                        <p><%# DataBinder.Eval(Container.DataItem,"ModelColorName") %></p>
                                        <asp:CheckBox id="chkActive" runat="server" Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsActive"))%>'/>
                                        <asp:HiddenField id="hdnModelColorID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"ModelColorID") %>' />
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                </ItemTemplate>
            </asp:repeater>
            <asp:button id="btnUpdateVersionColor" text="Update Version Colors" runat="server" />
        </div>
        <%} %>
        <input type="hidden" id="hdnVersionColor" runat="server" />
    </fieldset>
    <br>
    <br>
</div>
<script type="text/javascript">

    document.getElementById("cmbMake").onchange = cmbMake_Change;

    function cmbMake_Change(e) {
        var makeId = document.getElementById("cmbMake").value;
        var response = AjaxFunctions.GetNewModels(makeId);

        var dependentCmbs = new Array;
        dependentCmbs[0] = "cmbModel";
        //call the function to consume this data
        FillCombo_Callback(response, document.getElementById("cmbModel"), "hdn_cmbModel", dependentCmbs);
    }


    function checkFind(e) {
        if (document.getElementById('cmbMake').options[0].selected) {
            document.getElementById('selectModel').innerHTML = "Select Make First";
            return false;

        }
        else if (document.getElementById('cmbModel').options[0].selected) {
            document.getElementById('selectModel').innerHTML = "Select Model First";
            return false;
        }
        else document.getElementById('selectModel').innerHTML = "";
    }

    //document.getElementById('btnFind').onclick = checkFind;

    $(".editBtn").live("click", function () {
        $(".updateModelColor").show();
    });
    $(".closeBtn").live("click", function () {
        $(".updateModelColor").hide();
    });
    $(":checkbox").live("click", function isChecked(e) {
        $(this).attr("isModified", "true");
    });

    $("#btnUpdateVersionColor").live("click", function () {
        var versionCount = $(".addColorToVersionsBox").length;
        var bvId = 0;
        var str = '';
        var colorCount = $(".addColorToVersionsBox").first().find("ul li").length;
        for (var i = 0; i < versionCount; i++) {
            bvId = $("#rptVersionColor_BikeVersionId_" + i).val();
            str += bvId + ':';
            for (var j = 0; j < colorCount; j++) {
                if ($("#rptVersionColor_rptColor_" + i + "_chkActive_" + j + "[ismodified]").attr("ismodified")) {
                    str += $("#rptVersionColor_rptColor_" + i + "_hdnModelColorID_" + j).val() + "_" + ($("#rptVersionColor_rptColor_" + i + "_chkActive_" + j).prop('checked') ? 1 : 0) + ',';
                }
            }
            str = str.substring(0, str.length - 1);
            str += '|';
        }
        str = str.substring(0, str.length - 1);
        $("#hdnVersionColor").val(str);
    });
    $("#btnShowColors").live("click", function () {
        if ($("#cmbMake").val() > 0 && $("#cmbModel").val() > 0) {
            return true;
        }
        else {
            alert("Please select make and model");
            return false;
        }
    });

    $("#btnSave").live("click", function () {
        var isValid = true;
        if ($("#txtColor").val().length > 0 && $("#cmbMake").val() > 0 && $("#cmbModel").val() > 0) {
            if ($("#txtHexCode1").val().length > 0 || $("#txtHexCode2").val().length > 0 || $("#txtHexCode3").val().length > 0) {
                if ($("#txtHexCode1").val().length > 0)
                    if (!validateHexColorCode($("#txtHexCode1").val()))
                        isValid = false;
                if ($("#txtHexCode2").val().length > 0)
                    if (!validateHexColorCode($("#txtHexCode2").val()))
                        isValid = false;
                if ($("#txtHexCode3").val().length > 0)
                    if (!validateHexColorCode($("#txtHexCode3").val()))
                        isValid = false;
            }
            else {
                alert("Please enter a valid hexcode.");
                isValid = false;
            }
        }
        else {            
            isValid = false;
        }
        if (!isValid) {
            alert("Invalid input.");
        }
        return isValid;
    });

    function validateHexColorCode(val) {
        var patt = new RegExp("^(?:[0-9a-fA-F]{3}){1,2}$");
        return patt.test(val);
    }
    function openEditColorWindow(modelColorId,modelId) {
        window.open('/content/ManageNewModelColor.aspx?modelColorId=' + modelColorId + '&modelId=' + modelId + '', 'mywin', 'scrollbars=yes,left=0,top=0,width=1350,height=600');
    }
</script>
<!-- #Include file="/includes/footerNew.aspx" -->
