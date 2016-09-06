<%@ Page Language="C#" validateRequest = "false" AutoEventWireup="false" Inherits="BikewaleOpr.manufacturecampaign.ManageDealer" AsyncTimeout="45" EnableEventValidation="false" Async="true" %>
<!-- #Include file="/includes/headerNew.aspx" -->
<style type="text/css">
    .greenMessage {color: #6B8E23;font-size: 11px;}
    .errMessage {color: #FF4A4A;}
    .valign {vertical-align: top;}
    .progress-bar {width: 0;display: none;height: 2px;background: #16A085;bottom: 0px;left: 0;border-radius: 2px;}
    .position-abt {position: absolute;}
    .position-rel {position: relative;}
    .required {color: red;}
    .redmsg {border: 1px solid red;background: #FFCECE;}
    .greenMessage {color: #6B8E23;font-size: 11px;}
    .hide {display: none;}
    .show {display: block;}
    #pageloaddiv {position: fixed;left: 0px;top: 0px;width: 100%;height: 100%;z-index: 1000;background: rgb(250, 246, 246) url('http://imgd2.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif') no-repeat center center;}
</style>
<div>
    <!-- #Include file="/content/DealerMenu.aspx" -->
</div>
<div class="left min-height600">
    <h1><%= (isEdit ? "Edit" : "Add") %>&nbsp;<%= manufacturerName %> Campaign</h1>
    <asp:label class="greenMessage margin-bottom10 margin-left10" id="lblGreenMessage" runat="server" />    
    <table class="margin-top10 margin-bottom10 tblSimple" rules="all" cellspacing="0" cellpadding="5">
        <tbody>
            <tr>
                <th>Campaign Description :<span class="errorMessage">*</span> </th>
                <td><asp:TextBox runat="server" id="campaignDescription" class="req" enabled="true" width="300"/></td>
            </tr>
            <tr>
                <th>Campaign Masking Number :<span class="errorMessage">*</span></th>
                <td>
                    <asp:TextBox runat="server" id="txtMaskingNumber" maxlength="10" class="numeric"  disabled="disabled" />
                    <asp:dropdownlist id="ddlMaskingNumber" runat="server" />
                    <asp:hiddenfield id="hdnOldMaskingNumber" runat="server" />                          
                    <% if (isEdit) { %> <a id="releaseMaskingNumber" href="javascript:void(0)">Release Masking number</a><%} %>
                </td>
            </tr>  
            <tr>
                <th>Start / Stop Campaign :</th> 
                <td><asp:CheckBox runat="server" id="isActive" Checked="True" /></td>                  
            </tr>   
            <tr>
                <th>Dealer Price Quote Page Desktop Template<span class="errorMessage">*</span></th>
                <td>
                    <asp:textbox id="textBox1" textmode="multiline" multiline="true" height="100" width="300" runat="server" />                           
                    <asp:CheckBox runat="server" id="CheckBox1" Checked="True" text ="Use Default Template" onclick="enableDisableTextbox(this.checked, 'textBox1')"/> 
                    <asp:hiddenfield id="Hiddenfield1" runat="server" />
                </td>
            </tr>   
            <tr>
                <th>Dealer Price Quote Page Mobile Template<span class="errorMessage">*</span></th>
                <td>
                    <asp:textbox id="textBox2" textmode="multiline" multiline="true" height="100" width="300" runat="server" />                        
                    <asp:CheckBox runat="server" id="CheckBox2" Checked="True" text ="Use Default Template" onclick="enableDisableTextbox(this.checked, 'textBox2')"/> 
                    <asp:hiddenfield id="Hiddenfield2" runat="server" />
                </td>
            </tr> 
            <tr>
                <th>Model Page Desktop Template<span class="errorMessage">*</span></th>
                <td>
                    <asp:textbox id="textBox3" textmode="multiline" multiline="true" height="100" width="300" runat="server"  />                           
                    <asp:CheckBox runat="server" id="CheckBox3" Checked="True" text ="Use Default Template" onclick="enableDisableTextbox(this.checked, 'textBox3')"/> 
                    <asp:hiddenfield id="Hiddenfield3" runat="server" />
                </td>
            </tr>  
            <tr>
                <th>Model Page Mobile Template<span class="errorMessage">*</span></th>
                <td>
                    <asp:textbox id="textBox4" textmode="multiline" multiline="true" height="100" width="300" runat="server" />                            
                    <asp:CheckBox runat="server" id="CheckBox4" Checked="True" text ="Use Default Template" onclick="enableDisableTextbox(this.checked, 'textBox4')"/> 
                    <asp:hiddenfield id="Hiddenfield4" runat="server" />
                </td>
            </tr>             
            <tr>
                <td colspan="2"><asp:button id="btnUpdate" onclientclick="return ValidateForm();"  runat="server" cssclass="padding10" /></td>
            </tr>  
            <% if (isEdit) { %>                      
            <tr>
                <td colspan="2"><a href="/manufacturecampaign/ManufacturerCampaignRules.aspx?campaignid=<%=campaignId%>&dealerid=<%=dealerId%>&manufactureName=<%=manufacturerName%>">manage rules for the campaign</a></td>                         
            </tr>  
             <%} %>                 
        </tbody>                                                                    
    </table>
            <asp:label class="errMessage margin-bottom10 margin-left10 required" id="lblErrorSummary" runat="server" />
</div>

<script type="text/javascript">
    var BwOprHostUrl = '<%= BwOprHostUrl%>';
    $(document).ready(function () {

        if ('<%= isEdit %>' == 'False')
        { 
            $('#textBox1').prop("disabled", true);
            $('#textBox2').prop("disabled", true);
            $('#textBox3').prop("disabled", true);
            $('#textBox4').prop("disabled", true);
            $("#btnUpdate").val("Save Campaign");
        }
        else
        {        
            $("#btnUpdate").val("Update Campaign");
            
        }

        bindMaskingNumber(<%=dealerId%>);
        //bindMaskingNumber(21079);

    });

    $("#ddlMaskingNumber option[Value='True']").each(function () {
        $(this).prop("disabled", true);
        if ($(this).text() == txtMaskingNumber) {
            $('#txtMaskingNumber').val($(this).text());
            
        }
    });

    $("#ddlMaskingNumber").change(function () {
        $('#txtMaskingNumber').val($(this).find("option:selected").text());
        $('#hdnOldMaskingNumber').val($(this).find("option:selected").text());
    });

    $("#releaseMaskingNumber").click(function () {
        var maskingNumber = $("#txtMaskingNumber").val();
        if (maskingNumber.length > 0) {
            releaseMaskingNumber(maskingNumber);
        }
        return false;
    });


    function releaseMaskingNumber(maskingNumber) {
        try {
            if (confirm("Do you want to release the number?")) {
                $.ajax({
                    
                    type: "POST",
                    url: BwOprHostUrl + "/api/ManufacturerCampaign/ReleaseNumber/?dealerId=" + <%= dealerId %> + "&campaignId=" + <%= campaignId %> + "&maskingNumber=" +  maskingNumber  + "&userId=" + <%= userId %>,
                    datatype: "json",

                    success: function (response) {
                        if (response) {
                            $("#txtMaskingNumber").val('');
                            //bindMaskingNumber(dealerId);                                
                            alert("Masking Number is released successful.");
                            
                        }
                        else {
                            alert("There was error while releasing masking number. Please contact System Administrator for more details.");
                        }
                    }

                });
            }
        } catch (e) {
            alert("An error occured. Please contact System Administrator for more details.");
        }
    }

    function bindMaskingNumber(dealerId) {
        try {

            $.ajax({
                type: "GET",
                url: BwOprHostUrl + "/api/ManufacturerCampaign/GetDealerMaskingNumbers/?dealerId=" + <%= dealerId %>,
                datatype: "json",
                success: function (response) {
                    var res = response;
                    if (res) {
                        $('#ddlMaskingNumber').empty();
                        $.each(res, function (index, value) {
                            $('#ddlMaskingNumber').append($('<option>').text(value.number).attr('value', value.isAssigned));
                        });
                    }
                }

            });
        } catch (e) {
            alert("An error occured. Please contact System Administrator for more details.");
        }
    }


    function enableDisableTextbox(bEnable, textBoxID)
    {
        $('#' + textBoxID).prop("disabled", bEnable);
        $('#lblGreenMessage').html('Please fill values');
    }

    function modifyHtmlTemplate(ele)
    {
        try {
            if (ele) {
                var el = $("<section></section>");
                d = el.html(ele.val());
                $(d).find("#mfg_name").text("{1}");
                var maskingNum = $(d).find("#mfg_number");
                $(maskingNum).text("{2}").attr("class","{10}");
                $(maskingNum).prev().attr("class","{10}");
                $(d)
                var leadBtn = $(d).find(".leadcapturebtn");
                if(leadBtn)
                {
                    leadBtn.attr("data-item-mfg-campid","{0}");
                    leadBtn.attr("data-item-id","{3}");
                    leadBtn.attr("data-item-area","{4}");
                    leadBtn.attr("data-leadsourceid","{5}");
                    leadBtn.attr("data-pqsourceid","{6}");
                    leadBtn.attr("a","{7}");
                    leadBtn.attr("c","{8}");
                    leadBtn.attr("l","{9}");
                }
                ele.val(el.html());
            }
            
        } catch (e) {
            
        }
    }

    function ValidateForm() {
        var isValid = true;
        
        $('.req').each(function () {
            if ($.trim($(this).val()) == '') {
                isValid = false;
                $(this).addClass('redmsg');
            }
            else {
                $(this).removeClass('redmsg');
            }
        });

        
        
        $("textarea").each(function(){
            var ele = $(this);
            if(!ele.next().prop("checked"))
                modifyHtmlTemplate(ele);
        })
        
        
        $("textarea").each(function(){
            var ele = $(this);
            if(!ele.next().prop("checked") && $.trim($(this).val()) == ''){
                $(this).addClass('redmsg');
                isValid = false;
            }
            else {
                $(this).removeClass('redmsg');
            }

                
        })

        if (!isValid) {
            $('#lblErrorSummary').html('Please fill values');
            
        }

        return isValid;
        }

</script>

<!-- #Include file="/includes/footerNew.aspx" -->
