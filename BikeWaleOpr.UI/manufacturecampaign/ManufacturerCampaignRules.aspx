<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikewaleOpr.manufacturecampaign.ManufacturerCampaignRules" EnableEventValidation="false" %>
<!-- #Include file="/includes/headerNew.aspx" -->
        <div class="left min-height600"> 
            <h1>Manage Campaign Rules For <%=manufactureName %></h1>           
            <fieldset class="margin-top10">
                <legend>Add New Campaign Rules</legend>
                <div id="box" class="box">
                    <table>
                        <tr>
                            <td class="valign margin-left20">Make:
                                <asp:DropDownList ID="ddlMake" name="make" runat="server" Width="100%" />
                            </td>
                            <td class="valign margin-left20">Model:
                                <asp:ListBox ID="ddlModel" SelectionMode="multiple" runat="server" Style="width: 100%; height: 100px;" />
                                <asp:HiddenField ID="hdnSelectedModel" runat="server" />
                            </td>
                            <td class="valign margin-left20">All India:
                                <asp:CheckBox ID="chkAllIndia" runat="server" Width="100%" />
                            </td>
                            <td class="valign margin-left20">City:
                              <asp:DropDownList class="chosen-select" runat="server" multiple="true" name="cities" ID="ddlCity" Width="100%" />
                                <asp:HiddenField ID="hdnSelectedCities" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="margin-left20">
                                <asp:Button runat="server" ID="btnSaveRule" OnClientClick="return validateMfgRuleForm();" Text="Save Rules" />
                            </td>
                        </tr>
                    </table>
                </div>
            </fieldset>
            <div>
                <asp:Label class="redmsg errMessage margin-bottom10 margin-left10 greenMessage" ID="lblErrorSummary" runat="server" />                
                <asp:Label class="greenMessage margin-bottom10 margin-left10" ID="lblGreenMessage" runat="server" />                
            </div>
            <div>
                <% if (rptRules.DataSource != null)
                   { %>
                <h3>Existing Rules for <%=manufactureName %></h3>
                <asp:Button runat="server" OnClientClick="return deleteRules();" class="margin-bottom10 margin-top20" ID="btnDeleteRules" Text="Delete Rules" />                
                <asp:Repeater ID="rptRules" runat="server">
                    <HeaderTemplate>                        
                        <table border="1" style="border-collapse: collapse;" cellpadding="5">
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
                                <input type="checkbox" class="checkboxAll" id="chkOffer_<%# DataBinder.Eval(Container.DataItem,"CampaignRuleId") %>" ruleid="<%# DataBinder.Eval(Container.DataItem,"CampaignRuleId") %>" />
                            </td>
                            <td><%# Eval("MakeName").ToString() %></td>
                            <td><%# Eval("ModelName").ToString() %></td>
                            <td><%# Eval("StateName").ToString()==""?Eval("CityName").ToString():string.Format("{0},{1}",Eval("CityName").ToString(),Eval("StateName").ToString()) %></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                <% } %>
            </div>
            <asp:HiddenField ID="hdnCheckedRules" runat="server" Value="" />
        </div>
        
        <script type="text/javascript">
            $(document).ready(function () {
              //  $("#ddlModel").append("<option value='0' title=''> -- Select Models --</option>");
            });

            $("#ddlMake").change(function () {
                $("#lblSaved").text("");
                $("#ddlModel").val("0").attr("disabled", "disabled");
                if ($("#ddlMake").val() > 0) {
                    GetModels(this);
                }
                $('#hdnSelectedModel').val('');
            });


            $("#ddlCity").change(function () {
                $('#hdnSelectedCities').val($("#ddlCity").val());
            });

            $("#rptRules_chkAll").click(function () {
                if ($(this).is(":checked")) {
                    $('.checkboxAll').each(function () { this.checked = true; });
                }
                else {
                    $('.checkboxAll').each(function () { this.checked = false; });
                }
            });

            function validateMfgRuleForm() {
                var isAllIndia = $("#chkAllIndia").prop("checked");

                $('#lblErrorSummary').html('');
                if (isAllIndia) {
                    if ($("#ddlModel").val() == null) {
                        alert('Please select bike models');
                        return false;
                    }
                    else if ($("#ddlMake").val() == 0 || $("#ddlModel").val() == 0) {
                        alert('Please select bike make')
                        return false;
                    }
                    else {
                        $('#hdnSelectedModel').val($("#ddlModel").val());
                        return true;
                    }

                }
                else {
                    if ($("#ddlCity").val() == null || $("#ddlModel").val() == null) {
                        alert('Please select cities');
                        return false;
                    }
                    else if ($("#ddlMake").val() == 0 || $("#ddlCity").val() == 0 || $("#ddlModel").val() == 0) {
                        alert('Please select make, model and city')
                        return false;
                    }
                    else {
                        $('#hdnSelectedModel').val($("#ddlModel").val());
                        $('#hdnSelectedCities').val($("#ddlCity").val());
                        return true;
                    }
                }
            }

            function deleteRules() {
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
                    $('.checkboxAll').prop("checked") = false;
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