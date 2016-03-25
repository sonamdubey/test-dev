<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikewaleOpr.Campaign.DealersRules" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dealer Rules</title>
    <style type="text/css">
        .greenMessage {
            color:#6B8E23;
            font-size: 11px;
        }
        .redmsg{
            color: #FFCECE;
        }
        .errMessage {color:#FF4A4A;}
    </style>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <link rel="stylesheet" href="/css/common.css?V1.2" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <fieldset class="margin-left10">
            <legend>Add a New Rule</legend>
            <div id="box" class="box">
                <table>
                    <tr>
                        <td class="margin-left20">Make:
                            <asp:DropDownList ID="ddlMake" runat="server" Width="100%" />
                        </td>
                        <td class="margin-left20">Model:
                            <asp:DropDownList ID="ddlModel" runat="server" Width="100%" />
                            <asp:HiddenField ID="hdnSelectedModel" runat="server" />
                        </td>
                        <td class="margin-left20">State:
                            <asp:DropDownList ID="ddlState" runat="server" Width="100%" />
                        </td>
                        <td class="margin-left20">City:
                            <asp:DropDownList ID="ddlCity" runat="server" Width="100%" />
                            <asp:HiddenField ID="hdnSelectedCity" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="margin-left20">
                            <asp:Button runat="server" ID="btnSaveRule" OnClientClick="return ValidateForm();" Text="Save" />
                        </td>
                    </tr>
                </table>
            </div>
        </fieldset>
        <asp:Label class="redmsg errMessage margin-bottom10 margin-left10 greenMessage" ID="lblErrorSummary" runat="server" />
        <br />
        <asp:Label class="greenMessage margin-bottom10 margin-left10" ID="lblGreenMessage" runat="server" />
        <br />
        <% 
            if(rptRules.DataSource!= null){ 
        %>
        <asp:Button runat="server" OnClientClick="return deleteRules();" class="margin-bottom10 margin-left10" ID="btnDelete" Text="Delete" />
        <br />
        <% 
            } 
        %>
        <asp:Repeater ID="rptRules" runat="server">
                    <HeaderTemplate>
                        <h1>Added Rule(s) :</h1>
                        <br />
                        <table border="1" style="border-collapse: collapse;" cellpadding="5" class="margin-left10">
                            <tr style="background-color: #D4CFCF;">
                                <th>
                                    <div>Select All</div>
                                    <div>
                                        <input type="checkbox" runat="server" id="chkAll" />
                                    </div>
                                </th>
								<th>Make</th>
                                <th>Model</th>
                                <th>State</th>
                                <th>City</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                    <tr>
                           <td align="center">
                                <input type="checkbox" class="checkboxAll" id="chkOffer_<%# DataBinder.Eval(Container.DataItem,"Id") %>" RuleId="<%# DataBinder.Eval(Container.DataItem,"Id") %>" />
                           </td>
                            <td><%# Eval("MakeName").ToString() %></td>
                            <td><%# Eval("ModelName").ToString() %></td>
                            <td><%# Eval("StateName").ToString() %></td>
                            <td><%# Eval("CityName").ToString() %></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>

        <asp:HiddenField ID="hdnCheckedRules" runat="server" Value="" />
        <script type="text/javascript">
            $(document).ready(function () {
                if ($("#ddlMake").val() > 0) {
                    GetModels($("#ddlMake"));
                }
                if ($("#ddlState").val() > 0) {
                    loadStateCities();
                }
            });

            $("#ddlMake").change(function () {
                $("#lblSaved").text("");
                $("#ddlModel").val("0").attr("disabled", "disabled");
                GetModels(this);
                $('#hdnSelectedModel').val('');
            });

            $("#ddlState").change(function () {
                loadStateCities();
                $('#hdnSelectedCity').val('');
            });

            $("#ddlModel").change(function () {
                $('#hdnSelectedModel').val($("#ddlModel").val());
            });

            $("#ddlCity").change(function () {
                $('#hdnSelectedCity').val($("#ddlCity").val());
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
                if ($("#ddlCity").val() == null || $("#ddlModel").val() == null){
                    alert('Select values from List');
                    return false;
                }
                else if ($("#ddlMake").val() == 0 || $("#ddlState").val() == 0 || $("#ddlCity").val() == 0 ||  $("#ddlModel").val() == 0) {
                    alert('Select values from List')
                    return false;
                }
                else {
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
                            bindDropDownList(resObj, $("#ddlModel"), "", "--Select Model--");
                        }
                    });
                } else {
                    $("#ddlModel").val("0").attr("disabled", "disabled");
                }
            }
            function loadStateCities() {
                var stateId = $("#ddlState").val();
                if (stateId > 0) {
                    var requestType = "ALL";
                    $("#hdnCities").val("");
                    $.ajax({
                        type: "POST",
                        url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                        data: '{"requestType":"' + requestType + '", "stateId":"' + stateId + '"}',
                        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetCWCities"); },
                        success: function (response) {
                            var responseJSON = eval('(' + response + ')');
                            var resObj = eval('(' + responseJSON.value + ')');
                            bindDropDownList(resObj, $("#ddlCity"), "", "--Select city--");
                        }
                    });
                } else {
                    $("#ddlCity").val("0").attr("disabled", "disabled");
                }
            }
            function bindDropDownList(response, cmbToFill, viewStateId, selectString) {
                if (response.Table != null) {
                    if (!selectString || selectString == '') selectString = "--Select--";
                    $(cmbToFill).empty().append("<option value=\"0\" title='" + selectString + "'>" + selectString + "</option>").removeAttr("disabled");
                    var hdnValues = "";
                    // Add select all option for Models
                    if (($(cmbToFill).attr('id') == 'ddlModel')) {
                        $(cmbToFill).append("<option value=\"-1\" title='-- Select all --'>" + '-- Select all --' + "</option>");
                    }
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
    </form>
</body>
</html>
