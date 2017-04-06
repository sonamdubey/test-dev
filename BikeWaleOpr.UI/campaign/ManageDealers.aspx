<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikewaleOpr.Campaign.ManageDealers" AsyncTimeout="45" Async="true" %>

<%@ Import Namespace="BikeWaleOpr.Common" %>
<!-- #Include file="/includes/headerNew.aspx" -->

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
        background: rgb(250, 246, 246) url('https://imgd2.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif') no-repeat center center;
    }
</style>
<div>
    <fieldset class="margin-left10">
        <a id='backbutton' href="javascript:void(0)">Back to Manage Campaigns Page</a>
        <legend>
            <h3>Edit Dealer Campaign</h3>
        </legend>
        <div id="box" class="box">
            <table class="margin-top10 margin-bottom10" rules="all" cellspacing="0" cellpadding="5" style="border-width: 1px; border-style: solid; width: 60%; border-collapse: collapse;">
                <tbody>
                    <tr>
                        <td style="width: 20%"><strong>Dealer :</strong> </td>
                        <td><span id="spnDealerName"><%= dealerName  %></span></td>
                    </tr>
                    <tr>
                        <td style="width: 20%"><strong>Campaign Name :</strong> </td>
                        <td>
                            <asp:textbox runat="server" name="maskingNumber" id="txtCampaignName" maxlength="100" class="req width300" enabled="true" />
                            <span style="color: red">Please do not add area names in the campaign name.</span>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%"><strong>Campaign Dealer Number :</strong></td>
                        <td>
                            <asp:textbox runat="server" name="dealerNumber" id="txtDealerNumber" class="width300" disabled />
                            <span style="color: red" id="dealerNumberMsg">Mapping a masking number will result in calling to both masking and dealer numbers one after another.</span>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%"><strong>Campaign Masking Number :</strong><b class='required'>*</b></td>
                        <td>
                            <asp:textbox runat="server" readonly="true" name="maskingNumber" id="txtMaskingNumber" maxlength="10" class="numeric width300" enabled="true" />
                            <%
                                if (ddlMaskingNumber.DataSource != null)
                                { 
                            %>
                            <asp:dropdownlist id="ddlMaskingNumber" runat="server"></asp:dropdownlist>
                            <asp:hiddenfield id="hdnOldMaskingNumber" runat="server" />
                            <% if (isCampaignPresent)
                               { %> <a id="releaseMaskingNumber" href="javascript:void(0)">Release Masking number</a><%} %>
                            <%} %>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%"><strong>Campaign Email ID :</strong><b class="required">*</b></td>
                        <td>
                            <asp:textbox textmode="multiline" multiline="true" height="50" width="200" runat="server" id="txtDealerEmail" placeholder="Enter Email ids separated by comma" class="req width300" />
                            <span id="spnDealerEmail" class="Required marginleft18"></span>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%"><strong>Campaign Lead Serving radius(in kms) :</strong><b class="required">*</b></td>
                        <td>
                            <asp:textbox runat="server" id="txtdealerRadius" placeholder="" class="numeric req width300" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%"><strong>Daily Leads Limit :</strong></td>
                        <td>
                            <asp:textbox runat="server" id="txtLeadsLimit" placeholder="" class="numeric width300" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%"><strong>Call to Action :</strong></td>
                        <td>
                            <asp:checkbox runat="server" id="chkUseDefaultCallToAction" text="Use Default" autopostback="false"></asp:checkbox>
                            <asp:dropdownlist id="ddlCallToAction" autopostback="false" runat="server">                                
                            </asp:dropdownlist>
                            <br />
                            <span style="color: red">Call to action should be changed only when dealer has specifically asked for it. Inform the product team when a change in CTA is made.</span>
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



    </fieldset>

    <% if (isCampaignPresent)
       { %>
    <fieldset>
        <legend>Define Components</legend>

        <strong>Edit rules:</strong><span><a href="/campaign/DealersRules.aspx?campaignid=<%=campaignId %>&dealerid=<%=dealerId %>">Rules</a></span>

    </fieldset>
    <% } %>
</div>
<script type="text/javascript">

    var campaignId = "<%= campaignId %>";
    var dealerId = "<%= dealerId %>";
    var dialog;
    var dealerNoEle = $('#txtDealerNumber');
    var txtMaskingNumber = "<%= oldMaskingNumber %>";



    function ValidateForm() {
        var isValid = true;
        try {
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

            if (isValid) {
                if ($('#txtdealerRadius').val() == '0') {
                    var r = confirm("By selecting dealer radius as 0 KM, You are allocating a dealer to entire city. Do you confirm ?");
                    if (!r)
                        isValid = false;
                }
                $("#pageloaddiv").show();
            }

            if (isValid) {
                var maskingNumber = $("#txtMaskingNumber").val().trim();
                var nos = parseInt(dealerNoEle.attr("data-numberCount"));
                if (nos && maskingNumber != txtMaskingNumber && maskingNumber != "") {
                    var r = confirm("You are mapping " + nos + " dealer numbers to 1 masking number. Are you sure you want to continue?");
                    if (!r) {
                        isValid = false;
                        alert("Please ensure that there is only one number for this dealer in DCRM. Campaign has not been saved.");
                    }

                }
                $("#pageloaddiv").show();

            }


        } catch (e) {
            console.warn(e.message);
            isValid = false;
        }
        return isValid;
    }

    function isValidEmailAddress(emailAddress) {
        var pattern = new RegExp(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/);
        return pattern.test(emailAddress);
    };

    function releaseMaskingNumber(maskingNumber) {
        try {
            if (confirm("Do you want to release the number?")) {
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                    data: '{"dealerId":"' + dealerId + '","campaignId":"' + campaignId + '", "maskingNumber":"' + maskingNumber + '"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "ReleaseNumber"); },
                    success: function (response) {
                        if (JSON.parse(response).value) {
                            $("#txtMaskingNumber").val('');
                            //bindMaskingNumber(dealerId);                                
                            alert("Masking Number is released successful.");
                            location.reload();
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
                type: "POST",
                url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                data: '{"dealerId":"' + dealerId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetDealerMaskingNumbers"); },
                success: function (response) {
                    var res = JSON.parse(response);
                    if (res) {
                        $('#ddlMaskingNumber').empty();
                        $.each(res.value, function (index, value) {
                            ('#ddlMaskingNumber').append($('<option>').text(value.Number).attr('value', value.IsAssigned));
                        });
                    }
                }

            });
        } catch (e) {
            alert("An error occured. Please contact System Administrator for more details.");
        }
    }

    function handleMaskingNumber() {
        try {
            $("#ddlMaskingNumber option[Value='True']").each(function () {
                var ele = $(this), txt = ele.text();
                if (txt == txtMaskingNumber) {
                    $('#txtMaskingNumber').val(txt);
                    $(this).prop("selected", true);
                }
                ele.text(ele.text() + " (Assigned)");
            });

            //show dealernumber message
            var nos = (dealerNoEle.val().match(/,/g) || []).length;
            dealerNoEle.attr("data-numberCount", ++nos);
            if (nos > 1) {
                $("#dealerNumberMsg").text("There are " + nos + " numbers for this dealer. Mapping a masking number will result in calls going to both numbers one after another.");
            }
        } catch (e) {
            console.warn("Unable to set masking numbers : " + e.message);
        }
    }

    $(function () {

        handleMaskingNumber();

        $("#ddlMaskingNumber").chosen({ width: "150px", no_results_text: "No matches found!!", search_contains: true });

        $("#pageloaddiv").hide();

        $("#releaseMaskingNumber").on("click", function () {
            var maskingNumber = $("#txtMaskingNumber").val().trim();
            if (maskingNumber.length > 0) {
                releaseMaskingNumber(maskingNumber);
            }
            return false;
        });

        $("#ddlMaskingNumber").change(function () {
            var ele = $(this);
            if (ele.val() && ele.val() != "") {
                var val = $(this).find("option:selected").text();
                val = val.replace(" (Assigned)", "");
                $('#txtMaskingNumber').val(val);
            }
            else $('#txtMaskingNumber').val("");
        });

        $("#backbutton").on("click", function () {
            window.location.href = '/campaign/MapCampaign.aspx?dealerId=' + '<%= dealerId %>' + '&contractid=' + '<%=  contractId %>';
        });

        $("#chkUseDefaultCallToAction").change(function () {
            if ($(this).is(":checked")) {
                $("#ddlCallToAction").hide();
            }
            else {
                $("#ddlCallToAction").show();
            }
        });

        $(document).on("keyup", ".numeric", function (event) {
            this.value = this.value.replace(/[^0-9]/g, '');
        });


    });

</script>
<!-- #Include file="/includes/footerNew.aspx" -->
