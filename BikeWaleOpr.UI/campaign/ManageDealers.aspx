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
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <fieldset class="margin-left10">
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
                            <%--<textarea runat="server" id="textArea" style="height:50px;width:200px;" />--%>
                            <span id="spnDealerEmail" class="Required marginleft18"></span>
                        </td>
                    </tr>
                    <tr>
                        <td><strong>Dealer Serving radius :</strong><b class="required">*</b></td>
                        <td>
                            <asp:TextBox runat="server" id="txtdealerRadius" placeholder="" class="numeric req width300" />
                        </td>
                    </tr>
                       <%-- <tr>
                            <td><strong> Start date:</strong> <b class="required">*</b>
                            </td>
                             <td>
                                 <asp:TextBox ID="txtStartDate" class="req" runat="server" ReadOnly = "true"></asp:TextBox>
                                 <asp:HiddenField ID="hdnStartDate" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td><strong>End date: </strong><b class="required">*</b>
                            </td>
                             <td>
                                  <asp:TextBox ID="txtEndDate" class="req" runat="server" ReadOnly = "true"></asp:TextBox>
                                 <asp:HiddenField ID="hdnEndDate" runat="server" />
                            </td>
                        </tr>--%>
                         <tr>
                        <td colspan="2">
                            <asp:Button ID="btnUpdate" OnClientClick="return ValidateForm();" Text="Update" runat="server" />
                        </td>
                    </tr>
                </tbody></table>

              <asp:Label class="errMessage margin-bottom10 margin-left10 required" ID="lblErrorSummary" runat="server" />
                <br />
                <asp:Label class="margin-bottom10 margin-left10 greenMessage" ID="lblGreenMessage" runat="server" />
                <br />

            </div>
        </fieldset>
    </form>
    <script type="text/javascript">
        $('.numeric').on('input', function (event) {
            this.value = this.value.replace(/[^0-9]/g, '');
        });

        //$(function () {
        //    $("[id$=txtStartDate]").datepicker({
        //        showOn: 'button',
        //        dateFormat: 'dd-mm-yy',
        //        buttonImageOnly: true,
        //        buttonImage: '../images/calendar.png',
        //        numberOfMonths: 2,
        //        onSelect: function (selected) {
        //            var dateParts = selected.split('-');
        //            var dt = new Date(dateParts[1] + "/" + dateParts[0] + "/" + dateParts[2]);
        //            dt.setDate(dt.getDate() + 1);
        //            $("#txtEndDate").datepicker("option", "minDate", dt);
        //        }
        //    });
        //    $("[id$=txtEndDate]").datepicker({
        //        showOn: 'button',
        //        dateFormat: 'dd-mm-yy',
        //        buttonImageOnly: true,
        //        buttonImage: '../images/calendar.png',
        //        numberOfMonths: 2,
        //        onSelect: function (selected) {
        //            var dateParts = selected.split('-');
        //            var dt = new Date(dateParts[1] + "/" + dateParts[0] + "/" + dateParts[2]);
        //            dt.setDate(dt.getDate() + 1);
        //            $("#txtStartDate").datepicker("option", "maxDate", dt);
        //        }   
        //    });
        //});

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
            //if (isValid) {
            //    $('#hdnStartDate').val($('#txtStartDate').val());
            //    $('#hdnEndDate').val($('#txtEndDate').val());
            //}
            return isValid;
        }
        function isValidEmailAddress(emailAddress) {
            var pattern = new RegExp(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/);
            return pattern.test(emailAddress);
        };

        function ShowMapMaskingNumberPopup() {
            alert(1)
            var maskingCurrentAction = 'Add';
            if (maskingCurrentAction == "Add") {
                var dealerId = 11743;
                if (dealerId == "" || dealerId == null || dealerId == 0 || dealerId == -1) {
                    alert('Please select dealer first');
                    return;
                }
                $("#mapDealerMaskingIFrame").remove();
                var applyIframe = false;
                var GB_Html = '<iframe id="mapDealerMaskingIFrame" src="http://webserver:8082/DCRM/Masters/MapDealerMasking.aspx?DealerIdForMasking=' + 4 + '" style="width:99%; height:100%; display:none;"></iframe>';
                GB_show("Map Dealer Masking", "", 400, 200, applyIframe, GB_Html);
            }
            else if (maskingCurrentAction == "Remove") {
                var confirmResult = confirm('Are you sure you want to release the masked number for ' + $('#spnDealerName').text().split('(')[0]);
                if (!confirmResult) {
                    return;
                }
                $('#' + 'btnUpdateResume').hide();
                $("#txtMaskingNumber").val("");
                maskingCurrentAction = "Add";
                //$('#chkMaskingNumber').attr('disabled', false);
                //$('#mapNewMaskingNumber').text("Map new number");
                //$('#chkMaskingNumber').attr("checked", false);
            }
        }
    </script>
</body>
</html>
