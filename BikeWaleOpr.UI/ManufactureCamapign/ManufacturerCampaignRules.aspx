<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikewaleOpr.newbikebooking.ManufacturerCampaignRules" enableEventValidation="false" %>

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
    <script type="text/javascript" src="/BikeWale.UI/src/frameworks.js"></script>
    <script src="/src/chosen.jquery.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $(".chosen-select").chosen({
                placeholder_text_multiple: "Select Cities"
            });
        });
    </script>
    <link rel="stylesheet" href="/css/common.css?V1.2" type="text/css" /> 
   <link rel="stylesheet" href="/css/chosen.min.css"/>

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
                            <asp:DropDownList ID="ddlMake" name="make" runat="server" Width="100%" />
                        </td>
                        <td class="valign margin-left20">Model:
                            <asp:ListBox ID="ddlModel" SelectionMode="multiple" runat="server" style="width:100%;height: 100px;" />
                            <asp:HiddenField ID="hdnSelectedModel" runat="server" />
                        </td>
                        <td class="valign margin-left20">All India:
                            <asp:CheckBox ID="selAllIndia" runat="server"  Width="100%" />
                        </td>
                        <td class="valign margin-left20">City:
                          <asp:DropDownList class="chosen-select" runat="server" multiple="true" name="cities" id="ddlCity" Width="100%"/>
                              <asp:HiddenField ID="hdnSelectedCities" runat="server" />
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
       <div style="margin-left:205px;margin-top:10px;margin-bottom:10px">
        <asp:Label class="redmsg errMessage margin-bottom10 margin-left10 greenMessage" ID="lblErrorSummary" runat="server" />
        <br />
        <asp:Label class="greenMessage margin-bottom10 margin-left10" ID="lblGreenMessage" runat="server" />
        <br />
           </div>
       <div style="margin-left:210px">
        <% 
            if(rptRules.DataSource!= null){ 
        %>
       
        <asp:Button runat="server" OnClientClick="return deleteRules();" class="margin-bottom10 margin-left10" ID="btnDeleteRules" Text="Delete" />
        <br />
        <% 
            } 
        %>
        <asp:Repeater ID="rptRules" runat="server">
                    <HeaderTemplate>
                        <h1 >Added Rule(s) :</h1>
                        <br />
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
                                <input type="checkbox" class="checkboxAll" id="chkOffer_<%# DataBinder.Eval(Container.DataItem,"CampaignRuleId") %>" RuleId="<%# DataBinder.Eval(Container.DataItem,"CampaignRuleId") %>" />
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
           </div>
        <asp:HiddenField ID="hdnCheckedRules" runat="server" Value="" />
        <script type="text/javascript">
            $(document).ready(function () {
             $("#ddlModel").append("<option value='0' title=''> -- Select Models --</option>");
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

            function ValidateForm() {
                var isAllIndia = $("#selAllIndia").prop("checked");
               
                $('#lblErrorSummary').html('');
                if (isAllIndia) {
                    if ($("#ddlModel").val() == null) {
                        alert('Select values from List');
                        return false;
                    }
                    else if ($("#ddlMake").val() == 0 || $("#ddlModel").val() == 0) {
                        alert('Select values from List')
                        return false;
                    }
                    else {
                        $('#hdnSelectedModel').val($("#ddlModel").val());
                        return true;
                    }

                }
                else {
                    if ($("#ddlCity").val() == null || $("#ddlModel").val() == null) {
                        alert('Select values from List');
                        return false;
                    }
                    else if ($("#ddlMake").val() == 0 || $("#ddlCity").val() == 0 || $("#ddlModel").val() == 0) {
                        alert('Select values from List')
                        return false;
                    }
                    else {
                        $('#hdnSelectedModel').val($("#ddlModel").val());
                        $('#hdnSelectedCities').val($("#ddlCity").val());
                        return true;
                    }
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
    </form>  
  
</body>
</html>
