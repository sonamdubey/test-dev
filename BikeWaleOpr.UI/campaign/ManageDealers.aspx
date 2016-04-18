<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikewaleOpr.Campaign.ManageDealers" AsyncTimeout="45" Async="true" %>

<!DOCTYPE html>
<!-- #Include file="/includes/headerWithoutForm.aspx" -->
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manage Dealers</title>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js" type="text/javascript"></script>
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" rel="Stylesheet" type="text/css" />
    <link rel="stylesheet" href="/css/common.css?V1.2" type="text/css" />
    <script type="text/javascript" src="/src/greybox.js"></script>
    <style>
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
</head>
<body>
    <div id="pageloaddiv" class="hide"></div>
    <form id="form1" runat="server">
        <fieldset class="margin-left10">
            <a id='backbutton' href="javascript:void(0)">Back to contract page</a>
            <legend>Manage Dealer Campaigns</legend>
            <div id="box" class="box">

                <table class="table-default-style">
                    <tbody>
                        <tr>
                            <td><strong>Dealer :</strong> </td>
                            <td><span id="spnDealerName"><%= dealerName  %></span></td>
                        </tr>
                        <tr>
                            <td><strong>Campaign Name :</strong> </td>
                            <td>
                                <asp:TextBox runat="server" name="maskingNumber" ID="txtCampaignName" MaxLength="100" class="req width300" Enabled="true" /></td>
                        </tr>
                        <tr>
                            <td><strong>Dealer Masking Number :</strong><b class='required'>*</b></td>
                            <td>
                                <asp:TextBox runat="server" ReadOnly="true" name="maskingNumber" ID="txtMaskingNumber" MaxLength="10" class="req numeric width300" Enabled="true" />
                                <asp:DropDownList ID="ddlMaskingNumber" runat="server" />
                                <asp:HiddenField ID="hdnOldMaskingNumber" runat="server" />
                                <%--<a id="mapNewMaskingNumber" href="javascript:void(0)" onclick="ShowMapMaskingNumberPopup()">Map new Masking number</a>--%>
                            </td>
                        </tr>
                        <tr>
                            <td><strong>Dealer Email ID :</strong><b class="required">*</b></td>
                            <td>
                                <asp:TextBox TextMode="multiline" multiline="true" Height="50" Width="200" runat="server" ID="txtDealerEmail" placeholder="Enter Email ids separated by comma" class="req width300" />
                                <span id="spnDealerEmail" class="Required marginleft18"></span>
                            </td>
                        </tr>
                        <tr>
                            <td><strong>Dealer Serving radius :</strong><b class="required">*</b></td>
                            <td>
                                <asp:TextBox runat="server" ID="txtdealerRadius" placeholder="" class="numeric req width300" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnUpdate" OnClientClick="return ValidateForm();" Text="Update" runat="server" />
                            </td>
                        </tr>
                    </tbody>
                </table>

                <asp:Label class="errMessage margin-bottom10 margin-left10 required" ID="lblErrorSummary" runat="server" />
                <br />
                <asp:Label class="greenMessage margin-bottom10 margin-left10" ID="lblGreenMessage" runat="server" />
                <br />

            </div>
        </fieldset>
        <% if (isCampaignPresent)
           { %>
        <fieldset class="margin-left10">
            <legend>Define Components</legend>
            <table class="table-default-style">
                <tbody>
                    <tr>
                        <td><strong>Edit rules:</strong> </td>
                        <td><a href="/campaign/DealersRules.aspx?campaignid=<%=campaignId %>&dealerid=<%=dealerId %>">Rules</a></td>
                    </tr>
                </tbody>
            </table>
        </fieldset>
        <% } %>
    </form>
    <script type="text/javascript">
        var dialog;
        $('.numeric').on('input', function (event) {
            this.value = this.value.replace(/[^0-9]/g, '');
        });

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
            if (isValid) {
                if ($('#txtdealerRadius').val() == '0') {
                    var r = confirm("By selecting dealer radius as 0 KM, You are allocating a dealer to entire city. Do you confirm ?");
                    if (!r)
                        isValid = false;
                }
                $("#pageloaddiv").show();
            }
            return isValid;
        }
        function isValidEmailAddress(emailAddress) {
            var pattern = new RegExp(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/);
            return pattern.test(emailAddress);
        };

        $("#ddlMaskingNumber").change(function () {
            $('#txtMaskingNumber').val($(this).find("option:selected").text());
        });

        $("#backbutton").on("click", function () {
            window.location.href = '/campaign/MapCampaign.aspx?contractid=' + '<%= contractId %>';
        });
            $(window).ready(function () {
                $("#pageloaddiv").hide();
            });
    </script>
</body>
</html>
