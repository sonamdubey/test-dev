<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.BrowseBikes" %>
<div class="small-width left-float margin-top5"><h2><asp:Literal runat="server" ID="ltrTitel"></asp:Literal></h2></div>
<div class="icon-sprite arrow-right margin-top15 left-float"></div>
<div class="grid_2 omega margin-top5">
    <asp:DropDownList ID="ddlMake" runat="server" CssClass="brand">
        <asp:ListItem Selected="true" Text="--Select Make--" Value="0"></asp:ListItem>
    </asp:DropDownList>
</div>
<div class="icon-sprite light-divider margin-top5 left-float"></div>
<div class="grid_2 omega margin-top5">
    <asp:DropDownList ID="ddlModel" runat="server" CssClass="brand"  Enabled="false">
        <asp:ListItem Selected="true" Text="--Select Model--" Value="0"></asp:ListItem>
    </asp:DropDownList>
</div>
<%=VersionRequired == false ? "<div class=\"hide\">":"" %>
<div class="icon-sprite light-divider margin-top5 left-float"></div>
<div class="grid_2 omega margin-top5">
    <input type="hidden" id="hdn_Model" runat="server" />
    <asp:DropDownList ID="ddlVersion" runat="server" CssClass="brand" Enabled="false">
        <asp:ListItem Selected="true" Text="--Select Version--" Value="0"></asp:ListItem>
    </asp:DropDownList>
</div>
<input type="hidden" id="hdn_Version" runat="server"  />  
<%=VersionRequired == false ? "</div>":"" %>
<div class="margin-top5">
    <asp:Button ID="btn_browseBikes" runat="server" CssClass="action-btn" Text="Search" /> 
</div>
<input type="hidden" id="hdn_SelectedModel" runat="server" Value="" />
<input type="hidden" id="hdn_SelectedVersion" runat="server" value="" />
<script type="text/javascript">
    var ddlMakeId = '<%= drpMake_Id%>';
    var ddlModelId = '<%= drpModel_Id%>';  
    var hdnSelectedModel_Id = '<%= hdn_SelectedModel_Id%>';
    var hdnSelectedVersion_Id = '<%= hdn_SelectedVersion_Id%>';
    var btnSearch_Id = '<%= btn_browseBikes_Id%>';
    var version = '<%= VersionRequired %>';

    $("#" + ddlMakeId).change(function () {
        ddlMake_Change($(this));
    });

    function ddlMake_Change(e)
    {
        var requestType = "NEW";
        var makeId = e.val();
        $("#" + hdnSelectedModel_Id).val("");
        $("#" + hdnSelectedVersion_Id).val("");
        makeId = makeId.split('_')[0];
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
    }

    $("#" + ddlModelId).change(function () {
        var requestType = "NEW";
        var modelId = $(this).val();     
        $("#" + hdnSelectedVersion_Id).val("");
        if (modelId != 0) {
            $("#" + hdnSelectedModel_Id).val(modelId + "|" + $(this).find("option:selected").text());             
        } 
    });

    $("#" + btnSearch_Id).click(function () {
        
        if ($("#" + ddlMakeId).val() == 0) {
            alert("Please Select Make");
            return false;
        }
    });

    $(document).ready(function () {
        if ($("#" + ddlMakeId).val() > 0)
        {
            ddlMake_Change($("#" + ddlMakeId));
        }
    });

</script>
