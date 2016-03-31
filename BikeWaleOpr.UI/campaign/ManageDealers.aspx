<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikewaleOpr.Campaign.ManageDealers" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manage Dealers</title>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js" type="text/javascript"></script>
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" rel="Stylesheet" type="text/css" />
    <link rel="stylesheet" href="/css/common.css?V1.2" type="text/css" />
    <script type="text/javascript" src="/src/greybox.js"></script>
    <style>
        .required {color: red;}
        .redmsg{ border: 1px solid red; background: #FFCECE; }
        .greenMessage { color:#6B8E23;font-size: 11px;}
        .hide { display: none; }
        .show { display: block; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <fieldset class="margin-left10">
         <a id='backbutton' href="javascript:void(0)">Back to contract page</a>
         <legend>Manage Dealer Campaigns</legend>
          <div id="box" class="box">

              <table class="table-default-style">
                    <tbody><tr>
                        <td><strong>Dealer :</strong> </td>
                        <td><span id="spnDealerName"><%= dealerName  %></span></td>
                    </tr>
                    <tr>
                        <td><strong>Dealer Masking Number :</strong><b class='required'>*</b></td>
                        <td>
                            <asp:TextBox runat="server" id="txtMaskingNumber" MaxLength="10" class="req numeric width300" Enabled="true" />
                            <span id="spnMaskingNumber" class="errorMessage"></span>
                            <span id="mapNewMaskingNumber" class="link" onclick="ShowMapMaskingNumberPopup()">Release number</span>
                        </td>
                    </tr>
                    <tr>
                        <td><strong>Dealer Email ID :</strong><b class="required">*</b></td>
                        <td>
                            <asp:TextBox TextMode="multiline" multiline="true" Height="50" Width="200" runat="server" id="txtDealerEmail" placeholder="Enter Email ids separated by comma" class="req width300" />
                            <span id="spnDealerEmail" class="Required marginleft18"></span>
                        </td>
                    </tr>
                    <tr>
                        <td><strong>Dealer Serving radius :</strong><b class="required">*</b></td>
                        <td>
                            <asp:TextBox runat="server" id="txtdealerRadius" placeholder="" class="numeric req width300" />
                        </td>
                    </tr>
                         <tr>
                        <td colspan="2">
                            <asp:Button ID="btnUpdate" OnClientClick="return ValidateForm();" Text="Update" runat="server" />
                        </td>
                    </tr>
                </tbody></table>

              <asp:Label class="errMessage margin-bottom10 margin-left10 required" ID="lblErrorSummary" runat="server" />
                <br />
                <asp:Label class="greenMessage margin-bottom10 margin-left10" ID="lblGreenMessage" runat="server" />
                <br />

            </div>
        </fieldset>
        <% if(isCampaignPresent){ %>
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
            }
            return isValid;
        }
        function isValidEmailAddress(emailAddress) {
            var pattern = new RegExp(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/);
            return pattern.test(emailAddress);
        };

        function ShowMapMaskingNumberPopup() {
            var maskingCurrentAction = 'Add';
            if (maskingCurrentAction == "Add") {
                var dealerId = 11743;
                if (dealerId == "" || dealerId == null || dealerId == 0 || dealerId == -1) {
                    alert('Please select dealer first');
                    return;
                }
                $("#mapDealerMaskingIFrame").remove();
                var applyIframe = false;
                //var GB_Html = '<iframe id="mapDealerMaskingIFrame" src="http://webserver:8082/DCRM/Masters/MapDealerMasking.aspx?DealerIdForMasking=' + 4 + '" style="width:99%; height:100%; display:none;"></iframe>';
                //var GB_Html = '<iframe id="mapDealerMaskingIFrame" src="http://www.google.com" style="width:99%; height:100%; display:none;"></iframe>';
                //GB_show("Map Dealer Masking", "", 400, 200, applyIframe, GB_Html);
                //var src = "http://http://localhost:8084/";
                var src = 'http://webserver:8082/DCRM/Masters/MapDealerMasking.aspx?DealerIdForMasking=4';
                var title = 'Map a masking number'
                var width = 1100;
                var height = 600;
                var iframe = $('<iframe id="mapDealerMaskingIFrame" onload="mapDealerMaskingIFrameLoad()" frameborder="0" marginwidth="0" marginheight="0" allowfullscreen></iframe>');
                dialog = $("<div></div>").append(iframe).appendTo("body").dialog({
                    autoOpen: false,
                    modal: true,
                    resizable: false,
                    width: "auto",
                    height: "auto",
                    close: function () {
                        iframe.attr("src", "");
                    }
                });

                iframe.attr({
                    width: +width,
                    height: +height,
                    src: src
                });
                dialog.dialog("option", "title", title).dialog("open");
            }
            else if (maskingCurrentAction == "Remove") {
                var confirmResult = confirm('Are you sure you want to release the masked number for ' + $('#spnDealerName').text().split('(')[0]);
                if (!confirmResult) {
                    return;
                }
                $('#' + 'btnUpdateResume').hide();
                $("#txtMaskingNumber").val("");
                maskingCurrentAction = "Add";
            }
        }

        function updateMaskingNumber(maskingNumber, userMobileNumber, dealerType, ncdBrandId, maskingNumberId) {
            alert(maskingNumber);
            $('#txtMaskingNumber').val(maskingNumber);
            dialog.dialog('close');
        }

        function mapDealerMaskingIFrameLoad() {
            //Hiding all items that are not required in popup - Header, Footer, etc..
            var iFrameContents = $("#mapDealerMaskingIFrame").contents();
            iFrameContents.find(".header").css('display', 'none');
            iFrameContents.find(".right").css('display', 'none');
            iFrameContents.find(".footer").css('display', 'none');
            iFrameContents.find("#breadcrumbsDiv").css('display', 'none');
            iFrameContents.find("fieldset").css('border', 'none');
            iFrameContents.find("legend").css('display', 'none');
            iFrameContents.find("html").css('margin-top', '0px');
            iFrameContents.find("#drpStateValue").text(iFrameContents.find("#drpState :selected").text());
            iFrameContents.find("#drpState").css('display', 'none');
            iFrameContents.find("#drpCityValue").text(iFrameContents.find("#drpCity :selected").text());
            iFrameContents.find("#drpCity").css('display', 'none');
            iFrameContents.find("#drpDealerValue").text(iFrameContents.find("#drpDealer :selected").text());
            iFrameContents.find("#drpDealer").css('display', 'none');
            iFrameContents.find("#btnSave").css('display', 'none');

            //Initially width and height is set to 1% each so that
            //first above things get completed and then iFrame is displayed.
            $("#mapDealerMaskingIFrame").css('display', 'block');
        }
        $("#backbutton").on("click", function () {
            window.location.href = '/campaign/MapCampaign.aspx?contractid='+ '<%= contractId %>';
        });
        
    </script>
</body>
</html>
