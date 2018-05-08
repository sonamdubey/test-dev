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
        background: rgb(250, 246, 246) url('https://imgd.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif') no-repeat center center;
    }

    /* Style the list */
    ul.breadcrumb {padding: 10px 0;list-style: none;background-color: #eee;font-size: 17px;margin-top:10px;}
    /* Display list items side by side */
    ul.breadcrumb li {display: inline;}
    /* Add a slash symbol (/) before/behind each list item */
    ul.breadcrumb li+li:before {padding: 8px;color: black;content: ">\00a0";}
    /* Add a color to all links inside the list */
    ul.breadcrumb li a {color: #0275d8;text-decoration: none;}
    /* Add a color on mouse-over */
    ul.breadcrumb li a:hover {color: #01447e;}
</style>
<div class="left">
    <div>
        <ul class="breadcrumb margin-top15">
            <li><a id='backbutton' href="javascript:void(0)" title="Back to Manage Campaigns Page" style="padding:10px">Manage Campaigns (Step 1)</a></li>
            <li>Edit Dealer Campaign (Step 2)</li>
            <li><a target="_blank" rel="noopener" href="/campaign/DealersRules.aspx?campaignid=<%=campaignId %>&dealerid=<%=dealerId %>" title="Manage Models Mapping">Campaign Models (Step 3)</a></li>
            <li><a target="_blank" rel="noopener" href="/dealercampaign/servingareas/dealerid/<%= dealerId %>/campaignid/<%= campaignId %>/" title="Manage Campaign Areas Mapping">Campaign Serving Areas (Step 4)</a></li>
        </ul>
        <div id="box" class="box">
            <table class="margin-top10" rules="all" cellspacing="0" cellpadding="8" style="border-width: 1px; border-style: solid; width: 80%; border-collapse: collapse;font-size: 13px;">
                <tbody>
                    <tr>
                        <td style="width: 25%"><strong>Dealer Name :</strong> </td>
                        <td><span id="spnDealerName"><strong><%= dealerName  %></strong></span></td>
                    </tr>
                    <tr>
                        <td style="width: 22%"><strong>Campaign Name :</strong> </td>
                        <td>
                            <asp:textbox runat="server" name="maskingNumber" id="txtCampaignName" maxlength="100" class="req width300 font13" enabled="true" />
                            <span style="color: red">Please do not add area names in the campaign name.</span>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 22%"><strong>Campaign Dealer Number :</strong></td>
                        <td>
                            <asp:textbox runat="server" name="dealerNumber" id="txtDealerNumber" class="width300 font13" disabled />
                            <span style="color: red" id="dealerNumberMsg">Mapping a masking number will result in calling to both masking and dealer numbers one after another.</span>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 22%"><strong>Campaign Masking Number :</strong><b class='required'>*</b></td>
                        <td>
                            <asp:textbox runat="server" readonly="true" name="maskingNumber" id="txtMaskingNumber" maxlength="10" class="numeric width300 font13" enabled="true" />
                            <%
                                if (ddlMaskingNumber.DataSource != null)
                                { 
                            %>
                            <asp:dropdownlist id="ddlMaskingNumber" runat="server" class="font13"></asp:dropdownlist>
                            <asp:hiddenfield id="hdnOldMaskingNumber" runat="server" />
                            <% if (isCampaignPresent)
                               { %> <a id="releaseMaskingNumber" href="javascript:void(0)">Release Masking number</a><%} %>
                            <%} %>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 22%"><strong>Additional Communication No's:</strong> </td>
                        <td>
                            <asp:textbox runat="server" name="communicationNumber1" id="txtCommunicationNumber1" maxlength="10" class="numeric width300 font13" enabled="true" placeholder="Mobile 1 (Optional)" />
                            <asp:textbox runat="server" name="communicationNumber2" id="txtCommunicationNumber2" maxlength="10" class="numeric width300 font13" enabled="true" placeholder="Mobile 2 (Optional)"/>
                            <asp:textbox runat="server" name="communicationNumber3" id="txtCommunicationNumber3" maxlength="10" class="numeric width300 font13" enabled="true" placeholder="Mobile 3 (Optional)"/>
                            <asp:textbox runat="server" name="communicationNumber4" id="txtCommunicationNumber4" maxlength="10" class="numeric width300 font13" enabled="true" placeholder="Mobile 4 (Optional)"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 22%"><strong>Campaign Email ID :</strong><b class="required">*</b></td>
                        <td>
                            <asp:textbox runat="server" id="txtDealerEmail" placeholder="Enter Email id" class="req font13" type ="email" />
                            <span id="spnDealerEmail" class="Required marginleft18"></span>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 22%"><strong>Additional Communication Emails:</strong> </td>
                        <td>
                            <asp:textbox runat="server" name="communicationEmail1" id="txtCommunicationEmail1" maxlength="100" class=" width300 font13" type ="email" enabled="true" placeholder="Email 1 (Optional)"/>
                            <asp:textbox runat="server" name="communicationEmail2" id="txtCommunicationEmail2" maxlength="100" class=" width300 font13" type ="email" enabled="true" placeholder="Email 2 (Optional)"/>
                            <asp:textbox runat="server" name="communicationEmail3" id="txtCommunicationEmail3" maxlength="100" class=" width300 font13" type ="email" enabled="true" placeholder="Email 3 (Optional)"/>
                            <asp:textbox runat="server" name="communicationEmail4" id="txtCommunicationEmail4" maxlength="100" class=" width300 font13" type ="email" enabled="true" placeholder="Email 4 (Optional)"/>
                        </td>
                    </tr>
                    <tr>
                    <tr>
                        <td style="width: 22%"><strong>Daily Leads Limit :</strong></td>
                        <td>
                            <asp:textbox runat="server" id="txtLeadsLimit" placeholder="" class="numeric width300 font13" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 22%"><strong>Call to Action :</strong></td>
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
                            <asp:button id="btnUpdate" onclientclick="return ValidateForm();" text="Save Campaign" runat="server" cssclass="padding10" />
                        </td>
                    </tr>
                </tbody>
            </table>

            <asp:label class="errMessage margin-bottom10 margin-left10 required" id="lblErrorSummary" runat="server" />
            <br />
            <asp:label class="greenMessage margin-bottom10 margin-left10" id="lblGreenMessage" runat="server" />
            <br />

        </div>
    </div>
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
            if (isValid) {
                var maskingNumber = $("#txtMaskingNumber").val().trim();
                var nos = parseInt(dealerNoEle.attr("data-numberCount"));
                if (maskingNumber != "" && nos && maskingNumber != txtMaskingNumber) {
                    var r = confirm("You are mapping " + nos + " dealer numbers to 1 masking number. Are you sure you want to continue?");
                    if (!r) {
                        isValid = false;
                        alert("Please ensure that there is only one number for this dealer in DCRM. Campaign has not been saved.");
                    }

                }
                $("#pageloaddiv").show();
                
            }else {
                $('#lblErrorSummary').html('Please fill values');
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
