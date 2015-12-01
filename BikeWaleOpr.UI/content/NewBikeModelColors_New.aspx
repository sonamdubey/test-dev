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

    .ismodified {
        background-color: yellow;
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


    .versionNameText {
        width: 90px;
        text-align: center;
    }

    .versionsBoxList {
        width: 950px;
        border-left: 1px solid #aaa;
    }

    #one {
        width: 50px;
        height: 50px;
        border: 1px solid #ccc;
        margin: 0 auto 10px;
    }

    #minVColor{
        width: 25px;
        height: 25px;
        border: 1px solid #ccc;
        margin: 0 auto 10px;
    }

</style>
<div class="urh">
    You are here &raquo; <a href="/content/default.aspx">Contents</a> &raquo; Add Bike Model Colors
</div>
<!-- #Include file="ContentsMenu.aspx" -->
<div class="left">

    <span id="spnError" class="error" runat="server"></span>
    <fieldset>
        <legend style="font-weight: bold">Select Model</legend>
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
        <legend style="font-weight: bold">Add New Color To Model</legend>
        Color Name:* 
		<asp:textbox runat="server" id="txtColor" maxlength="50" columns="15" runat="server" tabindex="4" />
        <br />
        <br />
        Color Codes: 
		<%--<asp:textbox runat="server" id="txtHexCode1" maxlength="6" columns="6" runat="server" tabindex="5" />
        <asp:textbox runat="server" id="txtHexCode2" maxlength="6" columns="6" runat="server" tabindex="6" />
        <asp:textbox runat="server" id="txtHexCode3" maxlength="6" columns="6" runat="server" tabindex="7" />--%>
        <div class="input_fields_wrap">
            <span>
                <input type="text" class="hexCodeText" maxlength="6" columns="6" style="width: 59px;" name="mytext[]" tabindex="5" /></span>
            <span>
                <input type="text" class="hexCodeText" maxlength="6" columns="6" style="width: 59px;" name="mytext[]" tabindex="6" /></span>
            <span>
                <input type="text" class="hexCodeText" maxlength="6" columns="6" style="width: 59px;" name="mytext[]" tabindex="7" /></span>
            <button class="add_field_button">Add New Code</button>
        </div>
        <asp:button id="btnSave" text="Save Model Color" style='margin-top: 15px' runat="server" tabindex="8" />
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
                        <asp:HiddenField id="hdnModelColorId" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />
                        <table border="0" id="one" cellspacing="0">
                            <asp:repeater id="rptColorCode" runat="server" enableviewstate="false">
                                <itemtemplate>
                                    <tr style='background:#<%#DataBinder.Eval(Container.DataItem,"HexCode")%>'><td></td></tr>
                                </itemtemplate>
                            </asp:repeater>
                        </table>
                        <p><%#DataBinder.Eval(Container.DataItem,"Name") %></p>
                        <a href="javascript:openEditColorWindow(<%#DataBinder.Eval(Container.DataItem,"Id") %>, <%= ModelId %>)" class="editBtn">Edit</a>
                        <%--<a runat="server" id="lnkDelete" href="javascript:confirmDelete(<%#DataBinder.Eval(Container.DataItem,"Id") %>" class="editBtn">Delete</a>--%>                                                
                        <asp:button id="btnDelete" text="Delete" runat="server" />
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
        <input type="hidden" id="hdnVersionColor" runat="server" />
        <input type="hidden" id="hdnHexCodes" runat="server" />
        <input type="hidden" id="hdnDeleteModelId" runat="server" />
    </fieldset>
    <%
        if (modelColorCount > 0)
        {
                
    %>
    <fieldset style="margin-top: 15px;">
        <legend style="font-weight: bold">Add color to model versions:</legend>
        <div class="addColorToVersions">
            <asp:repeater id="rptVersionColor" runat="server" enableviewstate="false">
                <ItemTemplate>
                    <div class="addColorToVersionsBox inline-block">
                        <span class="inline-block versionNameText"><%# DataBinder.Eval(Container.DataItem,"VersionName") %></span>                        
                        <asp:HiddenField id="BikeVersionId" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"VersionId") %>' />
                        <ul class="inline-block versionsBoxList">
                            <asp:Repeater ID="rptColor" runat="server" enableviewstate="false">
                                <ItemTemplate>
                                    <li>
                                        <table border="0" id="minVColor" cellspacing="0">
                                            <asp:repeater id="rptVColor" runat="server" enableviewstate="false">
                                                <itemtemplate>
                                                    <tr style='background:#<%#DataBinder.Eval(Container.DataItem,"HexCode")%>'><td></td></tr>
                                                </itemtemplate>
                                            </asp:repeater>
                                        </table>
                                        <p><%# DataBinder.Eval(Container.DataItem,"ModelColorName") %></p>
                                        <asp:CheckBox id="chkActive" runat="server" Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsActive"))%>'/>
                                        <asp:HiddenField id="hdnModelColorID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"ModelColorID") %>' />
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                        <div class="clear"></div>
                        <hr />
                    </div>
                    <div class="clear"></div>
                </ItemTemplate>
            </asp:repeater>
            <asp:button id="btnUpdateVersionColor" text="Update Version Colors" runat="server" />
        </div>
    </fieldset>
    <%} %>
</div>
<script type="text/javascript">

    document.getElementById("cmbMake").onchange = cmbMake_Change;
    var max_fields = 10; //maximum input boxes allowed
    var wrapper = $(".input_fields_wrap"); //Fields wrapper
    var add_button = $(".add_field_button"); //Add button ID

    var x = 1; //initlal text box count
    $(document).ready(function () {
        $(add_button).click(function (e) { //on add input button click
            e.preventDefault();
            if (x < max_fields) { //max input box allowed
                x++; //text box increment
                //$(this).parent().find('span').last().append('<span><input style="width:59px;"  type="text" maxlength="6" columns="6" name="mytext[]"/><a href="#" class="remove_field">Remove</a></span>'); //add input box
                $('<span style=\'padding-left: 4px;\'><input style="width:59px;" class="hexCodeText" type="text" maxlength="6" columns="6" name="mytext[]" tabindex="' + eval(8 + x) + '"/><a href="#" class="remove_field">Remove</a></span>').insertAfter($(this).parent().find('span').last());
            }
        });
    });

    $(".remove_field").live("click", function (e) { //user click on remove text
        if (x >= 2) {
            $(this).parent('span').remove(); x--;
        }
        return false;
    })

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

    function confirmDelete(modelColorId) {
        if (modelColorId) {
            if (confirm("Do you want to delete the model color?")) {
                $("#hdnDeleteModelId").val(modelColorId);
                return true;
            }
        }
        return false;
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
        $(this).parent().first().addClass("ismodified");
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
            if ($(".hexCodeText").length > 0) {
                $('#hdnHexCodes').val('');
                $(".hexCodeText").each(function () {
                    if (($(this).val().length > 0) && (!validateHexColorCode($(this).val()))) {
                        isValid = false;
                    }
                })
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
        else {
            $('#hdnHexCodes').val($('#hdnHexCodes').val().substring(0, $('#hdnHexCodes').val().length - 1));
        }
        return isValid;
    });

    function validateHexColorCode(val) {
        var patt = new RegExp("^(?:[0-9a-fA-F]{3}){1,2}$");
        var retVal = patt.test(val);
        if (retVal) {
            $('#hdnHexCodes').val(val + ',' + $('#hdnHexCodes').val());
        }
        return retVal;
    }

    $("input[id*='rptModelColor_btnDelete']").live("click", function () {
        confirmDelete($(this).parent().find(":hidden").val());
    });

    function openEditColorWindow(modelColorId, modelId) {
        window.open('/content/ManageNewModelColor.aspx?modelColorId=' + modelColorId + '&modelId=' + modelId + '', 'mywin', 'scrollbars=yes,left=0,top=0,width=1350,height=600');
    }
</script>
<!-- #Include file="/includes/footerNew.aspx" -->
