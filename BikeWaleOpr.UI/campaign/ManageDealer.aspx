<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikewaleOpr.Campaign.ManageDealer" AsyncTimeout="45" Async="true" %>
<!-- #Include file="/includes/headerNew.aspx" -->
<script src="http://st1.aeplcdn.com/bikewale/src/frameworks.js?01July2016v1" type="text/javascript"></script>
<script src="/src/AjaxFunctions.js" type="text/javascript"></script>
<script type="text/ecmascript" src="/src/AjaxFunctions.js"></script>
<script src="/src/knockout.js" type="text/javascript"></script>
<link rel="stylesheet" href="/css/common.css?V1.2" type="text/css" />
<link href="http://st2.aeplcdn.com/bikewale/css/chosen.min.css?v15416" rel="stylesheet" />

<style type="text/css">
    .greenMessage {
        color: #6B8E23;
        font-size: 11px;
    }

    .redmsg {
        color: #FFCECE;
    }

    .errMessage {
        color: #FF4A4A;
    }

    .valign {
        vertical-align: top;
    }

    .progress-bar {
        width: 0;
        display: none;
        height: 2px;
        background: #16A085;
        bottom: 0px;
        left: 0;
        border-radius: 2px;
    }

    .position-abt {
        position: absolute;
    }

    .position-rel {
        position: relative;
    }

    .required {
        color: red;
    }

    .redmsg {
        border: 1px solid red;
        background: #FFCECE;
    }

    .greenMessage {
        color: #6B8E23;
        font-size: 11px;
    }

    .hide {
        display: none;
    }

    .show {
        display: block;
    }

    #pageloaddiv {
        position: fixed;
        left: 0px;
        top: 0px;
        width: 100%;
        height: 100%;
        z-index: 1000;
        background: rgb(250, 246, 246) url('http://imgd2.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif') no-repeat center center;
    }
</style>
<div>
    You are here &raquo; Edit Dealer Campaigns
</div>
<div>
    <!-- #Include file="/content/DealerMenu.aspx" -->
</div>
<div>

    <% if(isEdit)  { %>
    <h1> Edit </h1>
    <% }else {  %> 
    <h1> Add </h1>
    <% }  %> 

    <table class="margin-top10 margin-bottom10" rules="all" cellspacing="0" cellpadding="5" style="border-width: 1px; border-style: solid; width: 60%; border-collapse: collapse;">
                <tbody>
                    <tr>
                        <td style="width: 20%"><strong>Campaign Description :</strong> </td>
                        <td>
                            <asp:textbox runat="server"  maxlength="300" class="req width300" enabled="true" width="300"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%"><strong>Campaign Masking Number :</strong><b class='required'>*</b></td>
                        <td>
                            <asp:textbox runat="server" readonly="true" name="maskingNumber" id="txtMaskingNumber" maxlength="10" class="numeric width300" enabled="true" />
                            <asp:dropdownlist id="ddlMaskingNumber" runat="server" />
                            <asp:hiddenfield id="hdnOldMaskingNumber" runat="server" />
                            <a id="releaseMaskingNumber" href="javascript:void(0)">Release Masking number</a>
                        </td>
                    </tr>  
                    <tr>
                        <td style="width: 20%"><strong>Is Active :</strong><b class='required'>*</b> 
                        <asp:CheckBox runat="server" id="CheckBox1" Checked="True" /> 
                        </td>                   
                    </tr>   
                    <tr>
                        <td style="width: 20%"><strong>1. Dealer Price Quote Page Desktop</strong><b class='required'>*</b>                         
                        </td>
                        <td>
                            <asp:textbox id="textBox1" disabled="true" textmode="multiline" multiline="true" height="100" width="300" runat="server" class="req width300" />
                            <span id="spnDealerEmail" class="Required marginleft18"></span>
                            <asp:CheckBox runat="server" id="CheckBox2" Checked="True" text ="Use Default" onclick="enableDisable(this.checked, 'textBox1')"/> 
                        </td>
                    </tr>   
                    <tr>
                        <td style="width: 20%"><strong>2. Dealer Price Quote Page Mobile</strong><b class='required'>*</b>                         
                        </td>
                        <td>
                            <asp:textbox id="textBox2" textmode="multiline" disabled="true" multiline="true" height="100" width="300" runat="server" class="req width300" />
                            <span id="spnDealerEmail" class="Required marginleft18"></span>
                            <asp:CheckBox runat="server" id="CheckBox3" Checked="True" text ="Use Default" onclick="enableDisable(this.checked, 'textBox2')"/> 
                        </td>
                    </tr> 
                    <tr>
                        <td style="width: 20%"><strong>3. Model Page Desktop</strong><b class='required'>*</b>                         
                        </td>
                        <td>
                            <asp:textbox id="textBox3" disabled="true" textmode="multiline" multiline="true" height="100" width="300" runat="server" class="req width300" />
                            <span id="spnDealerEmail" class="Required marginleft18"></span>
                            <asp:CheckBox runat="server" id="CheckBox4" Checked="True" text ="Use Default" onclick="enableDisable(this.checked, 'textBox3')"/> 
                        </td>
                    </tr>  
                    <tr>
                        <td style="width: 20%"><strong>4. Model Page Mobile</strong><b class='required'>*</b>                         
                        </td>
                        <td>
                            <asp:textbox id="textBox4" disabled="true" textmode="multiline" multiline="true" height="100" width="300" runat="server" class="req width300" />
                            <span id="spnDealerEmail" class="Required marginleft18"></span>
                            <asp:CheckBox runat="server" id="CheckBox5" Checked="True" text ="Use Default" onclick="enableDisable(this.checked, 'textBox4')"/> 
                        </td>
                    </tr>             
                    <tr>
                        <td colspan="2">
                            <asp:button id="btnUpdate" onclientclick="return ValidateForm();" text="Save" runat="server" cssclass="padding10" />
                        </td>
                    </tr>
                </tbody>                         
            </table>


                <asp:label class="errMessage margin-bottom10 margin-left10 required" id="lblErrorSummary" runat="server" />
            <br />
            <asp:label class="greenMessage margin-bottom10 margin-left10" id="lblGreenMessage" runat="server" />
            <br />
           
                      

</div>

<script type="text/javascript">
    function enableDisable(bEnable, textBoxID)
    {
        $('#' + textBoxID).prop("disabled", bEnable);
        $('#lblGreenMessage').html('Please fill values');
    }

    function ValidateForm() {
        var isValid = true;
        $('#lblErrorSummary').html('');
        $('.req').each(function () {
            if ($.trim($(this).val()) == '') {
                isValid = false;
                $(this).addClass('redmsg');
            }
            else {
                $(this).removeClass('redmsg');
            }
        });

        if (!isValid) {
            $('#lblErrorSummary').html('Please fill values');
        }
        
        return isValid;
        }

</script>

<!-- #Include file="/includes/footerNew.aspx" -->
