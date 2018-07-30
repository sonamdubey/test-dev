<%@ Control Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Controls.BrowseBikes" %>
<h2><asp:Literal runat="server" ID="ltrTitel"></asp:Literal></h2>
<ul class="ul-hrz-left margin-top10">  
    <li><asp:DropDownList ID="ddlMake" runat="server" CssClass="brand">
        <asp:ListItem Selected="true" Text="--Select Make--" Value="0"></asp:ListItem>
    </asp:DropDownList></li>
    <li> <asp:DropDownList ID="ddlModel" runat="server" CssClass="brand"  Enabled="false">
        <asp:ListItem Selected="true" Text="--Select Model--" Value="0"></asp:ListItem>
    </asp:DropDownList></li>
    <li> <input type="hidden" id="hdn_Model" runat="server" />
    <asp:DropDownList ID="ddlVersion" runat="server" CssClass="brand" Enabled="false">
        <asp:ListItem Selected="true" Text="--Select Version--" Value="0"></asp:ListItem>
    </asp:DropDownList></li>
    <li><asp:Button ID="btn_browseBikes" runat="server" CssClass="action-btn" Text="Search" /></li>
</ul>
<div class="clear"></div>
<input type="hidden" id="hdn_Version" runat="server"  />
<input type="hidden" id="hdn_SelectedModel" runat="server" Value="" />
<input type="hidden" id="hdn_SelectedVersion" runat="server" value="" />

<script type="text/javascript">
    var ddlMakeId = '<%= drpMake_Id%>';
    var ddlModelId = '<%= drpModel_Id%>';
    var ddlVersionId = '<%= drpVersion_Id%>';
    var hdnSelectedModel_Id = '<%= hdn_SelectedModel_Id%>';
    var hdnSelectedVersion_Id = '<%= hdn_SelectedVersion_Id%>';
    var btnSearch_Id = '<%= btn_browseBikes_Id%>';
    var version = '<%= VersionRequired %>';
    //var makeId = "";
    $("#" + ddlMakeId).change(function () {        
        var requestType = "NEW";
         var makeId = $(this).val().split('_')[0];
        //alert(makeId);
        $("#" + hdnSelectedModel_Id).val("");
        $("#" + hdnSelectedVersion_Id).val("");

        if (makeId != 0) {
            $.ajax({
                type: "POST",
                url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
                data: '{"requestType":"' + requestType + '", "makeId":"' + makeId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModelsWithMappingName"); },
                success: function (response) {
                    var responseJSON = eval('(' + response + ')');
                    var resObj = eval('(' + responseJSON.value + ')');

                    var dependentCmbs = new Array();
                    bindDropDownList(resObj, $("#" + ddlModelId), "hdn_ddlModel", dependentCmbs, "--Select Model--");
                }
            });
        } else {
            $("#" + ddlModelId).val("0").attr("disabled", true);
        }
    });

    $("#" + ddlModelId).change(function () {
        //alert(makeId);
        var requestType = "NEW";
        var modelId = $(this).val().split('_')[0];
        //alert(modelId);
        $("#" + hdnSelectedVersion_Id).val("");


        if (modelId != 0 && version == "True") {
            $.ajax({
                type: "POST",
                url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
                data: '{"requestType":"' + requestType + '", "modelId":"' + modelId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetVersions"); },
                success: function (response) {
                    var responseJSON = eval('(' + response + ')');
                    var resObj = eval('(' + responseJSON.value + ')');

                    var dependentCmbs = new Array();
                    bindDropDownList(resObj, $("#" + ddlVersionId), "hdn_ddlVersion", dependentCmbs, "--Select Version--");
                }
            });

            $("#" + hdnSelectedModel_Id).val(modelId + "|" + $(this).find("option:selected").text());
        } else {
            $("#" + ddlVersionId).val("0").attr("disabled", true);
            $("#" + hdnSelectedModel_Id).val("");
        }
    });

    $("#" + ddlVersionId).change(function () {
        if ($(this).val() == 0) {
            $("#" + hdnSelectedModel_Id).val("");
            $("#" + hdnSelectedVersion_Id).val("");
        } else {
            $("#" + hdnSelectedModel_Id).val($("#" + ddlModelId).val() + "|" + $("#" + ddlModelId).find("option:selected").text());
            $("#" + hdnSelectedVersion_Id).val($("#" + ddlVersionId).val() + "|" + $("#" + ddlVersionId).find("option:selected").text());
        }
    });

    $("#" + btnSearch_Id).click(function () {
        if ($("#" + ddlMakeId).val() == 0) {
            alert("Please Select Make");
            return false;
        }
    });
</script>
