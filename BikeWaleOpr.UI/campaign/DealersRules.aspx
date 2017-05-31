<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikewaleOpr.Campaign.DealersRules" EnableEventValidation="false" %>

<!-- #Include file="/includes/headerNew.aspx" -->
    <h1 class="margin-left10"><%= dealerName %> campaign models</h1>
<div class="margin-top10 floatLeft" style="width: 850px; display: inline-block;">
    <asp:Repeater ID="rptRules" runat="server">
                    <HeaderTemplate>                        
                        <table border="1" style="border-collapse: collapse;" cellpadding="5" class="margin-left10 font13">
                            <tr style="background-color: #D4CFCF;">
                                <th>
                                    <div>Select All</div>
                                    <div>
                                        <input type="checkbox" runat="server" id="chkAll" />
                                    </div>
                                </th>
								<th>Make</th>
                                <th>Model</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                    <tr>
                           <td align="center">
                                <input type="checkbox" class="checkboxAll" id="chkOffer_<%# DataBinder.Eval(Container.DataItem,"Id") %>" RuleId="<%# DataBinder.Eval(Container.DataItem,"Id") %>" />
                           </td>
                            <td><%# Eval("MakeName").ToString() %></td>
                            <td><%# Eval("ModelName").ToString() %></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
        <asp:HiddenField ID="hdnCheckedRules" runat="server" Value="" />
</div>
<div class="margin-top10" style="position: fixed; right: 20px;">
    <fieldset class="margin-left10 font13">            
            <legend>Add Campaign Models</legend>
            <div id="box" class="box">                
                    <div class="margin-top10">
                        <asp:DropDownList ID="ddlMake" runat="server" Width="100%" class="font13" />
                    </div>
                    <div class="margin-top10">
                        <asp:DropDownList ID="ddlModel" multiple="multiple" runat="server" style="width:100%;height: 100px;" class="font13"/>
                            <asp:HiddenField ID="hdnSelectedModel" runat="server" />                        
                    </div>
                    <div class="margin-top10"><asp:Button runat="server" ID="btnSaveRule" OnClientClick="return ValidateForm();" Text="Save Models" /></div>                
            </div>
        <div class="margin-top10"><asp:Label class="redmsg errMessage margin-bottom10 margin-left10 greenMessage" ID="lblErrorSummary" runat="server" /></div>
        <div class="margin-top10"><asp:Label class="greenMessage margin-bottom10 margin-left10" ID="lblGreenMessage" runat="server" /></div>
        </fieldset>

        <% 
            if(rptRules.DataSource!= null){ 
        %>
        <fieldset class="margin-left10 margin-top20">            
            <legend>Remove Campaign Models</legend>
        <asp:Button runat="server" OnClientClick="return deleteRules();" class="margin-bottom10 margin-left10 margin-top10" ID="btnDelete" Text="Delete Models" />
        </fieldset>
        <br />
        <% 
            } 
        %>
</div>
        
        
        <script type="text/javascript">
            $(document).ready(function () {
                if ($("#ddlMake").val() > 0) {
                    GetModels($("#ddlMake"));
                }
                $("#ddlModel").append("<option value='0' title=''> -- Select Models --</option>");                
                $('#ddlModel').prop('disabled', true);                
            });

            $("#ddlMake").change(function () {
                $("#lblSaved").text("");
                $("#ddlModel").val("0").attr("disabled", "disabled");
                GetModels(this);
                $('#hdnSelectedModel').val('');
            });

            $("#rptRules_chkAll").click(function () {
                if ($(this).is(":checked")) {
                    $('.checkboxAll').each(function () { this.checked = true; });
                }
                else {
                    $('.checkboxAll').each(function () { this.checked = false; });
                }
            });

            function ValidateForm() {
                $('#lblErrorSummary').html('');
                if ($("#ddlModel").val() == null){
                    alert('Please select bike models');
                    return false;
                }
                else if ($("#ddlMake").val() == 0 || $("#ddlModel").val() == 0) {
                    alert('Please select bike models')
                    return false;
                }
                else {
                    $('#hdnSelectedModel').val($("#ddlModel").val());
                    return true;
                }
            }
            function deleteRules () {
                var isSuccess = false;
                var ruleIds = '';
                $('.checkboxAll').each(function () {
                    if ($(this).is(":checked")) {
                        ruleIds += $(this).attr('RuleId') + ',';
                    }
                });
                if (ruleIds.length > 1) {
                    ruleIds = ruleIds.substring(0, ruleIds.length - 1);
                    isSuccess = true;
                }

                if (isSuccess) {
                    $('#hdnCheckedRules').val(ruleIds);
                    return true;
                }
                else {
                    alert("please select rules to delete.");
                    return false;
                }
            }

            function GetModels(e) {
                var makeId = $(e).val();
                var reqType = 'NEW';
                if (makeId > 0) {
                    $.ajax({
                        type: "POST",
                        url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                        data: '{"makeId":"' + makeId + '" , "requestType":"' + reqType + '"}',
                        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModels"); },
                        success: function (response) {
                            var responseJSON = eval('(' + response + ')');
                            var resObj = eval('(' + responseJSON.value + ')');
                            bindDropDownList(resObj, $("#ddlModel"), "", "");
                        }
                    });
                } else {
                    $("#ddlModel").val("0").attr("disabled", "disabled");
                }
            }

            function bindDropDownList(response, cmbToFill, viewStateId, selectString) {
                if (response.Table != null) {                    
                    $(cmbToFill).empty();
                    $(cmbToFill).prop('disabled', false);
                    if (selectString != '') {
                        $(cmbToFill).append("<option value=\"0\" title='" + selectString + "'>" + selectString + "</option>");
                    }
                    var hdnValues = "";

                    for (var i = 0; i < response.Table.length; i++) {
                        $(cmbToFill).append("<option value=" + response.Table[i].Value + " title='" + response.Table[i].Text + "'>" + response.Table[i].Text + "</option>");
                        if (hdnValues == "")
                            hdnValues += response.Table[i].Text + "|" + response.Table[i].Value;
                        else
                            hdnValues += "|" + response.Table[i].Text + "|" + response.Table[i].Value;
                    }
                    if (viewStateId) $("#" + viewStateId).val(hdnValues);
                }
            }
        </script>
<!-- #Include file="/includes/footerNew.aspx" -->