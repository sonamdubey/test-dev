<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManufacturerCampaignRules.aspx.cs" Inherits="BikewaleOpr.newbikebooking.ManufacturerCampaignRules" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<!-- #Include file="/includes/headerWithoutForm.aspx" -->
<head runat="server">
    <title>Campaign Rules</title>
      <style type="text/css">
        .greenMessage {
            color:#6B8E23;
            font-size: 11px;
        }
        .redmsg{
            color: #FFCECE;
        }
        .errMessage {color:#FF4A4A;}
        .valign { vertical-align: top;}
    </style>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.4.2/chosen.jquery.js"></script>
    <script type="text/javascript">
        $(function () {
            $(".chosen-select").chosen();
        });
    </script>
    <link rel="stylesheet" href="/css/common.css?V1.2" type="text/css" /> 
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.4.2/chosen.css"/>

</head>
    
<body>
    
   <form id="form1" runat="server">
       <div class="margin-left20">
        <!-- #Include file="/content/DealerMenu.aspx" -->
    </div>
        <fieldset class="margin-left10" style="width:1100px">
            <legend>Add a New Rule</legend>
            <div id="box" class="box">
                <table>
                    <tr>
                        <td class="valign margin-left20">Make:
                            <asp:DropDownList ID="ddlMake" runat="server" Width="100%" />
                        </td>
                        <td class="valign margin-left20">Model:
                            <%--<asp:DropDownList ID="ddlModel" runat="server" Width="100%" />--%>
                            <asp:DropDownList ID="ddlModel" multiple="multiple" runat="server" style="width:100%;height: 100px;" />
                            <asp:HiddenField ID="hdnSelectedModel" runat="server" />
                        </td>
                        <td class="valign margin-left20">All India:
                            <asp:CheckBox ID="selAllIndia" runat="server" Width="100%" />
                        </td>
                        <td class="valign margin-left20">City:
                          <asp:DropDownList class="chosen-select" runat="server" multiple="true" name="cities" id="ddlCity" Width="100%"/>
                              
                         
                            <%--<asp:DropDownList ID="ddlCity" runat="server" Width="100%" />
                            <asp:HiddenField ID="hdnSelectedCity" runat="server" />--%>
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
                                <th>Location</th>
                                
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                    <tr>
                           <td align="center">
                                <input type="checkbox" class="checkboxAll" id="chkOffer_<%# DataBinder.Eval(Container.DataItem,"Id") %>" RuleId="<%# DataBinder.Eval(Container.DataItem,"Id") %>" />
                           </td>
                            <td><%# Eval("MakeName").ToString() %></td>
                            <td><%# Eval("ModelName").ToString() %></td>
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
                $("#ddlModel").append("<option value='0' title=''> -- Select Models --</option>");
                $("#ddlCity").append("<option value='0' title=''> -- Select City --</option>");
                $('#ddlModel').prop('disabled', true);
                $('#ddlCity').prop('disabled', true);
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
                    //if (!selectString || selectString == '') selectString = "--Select--";
                    $(cmbToFill).empty();
                    $(cmbToFill).prop('disabled', false);
                    if (selectString != '') {
                        $(cmbToFill).append("<option value=\"0\" title='" + selectString + "'>" + selectString + "</option>");
                    }
                    var hdnValues = "";
                    // Add select all option for Models
                    //if (($(cmbToFill).attr('id') == 'ddlModel')) {
                    //    $(cmbToFill).append("<option value=\"-1\" title='-- Select all --'>" + '-- Select all --' + "</option>");
                    //}
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
