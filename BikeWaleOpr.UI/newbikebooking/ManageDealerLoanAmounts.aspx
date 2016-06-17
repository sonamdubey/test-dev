<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikeWaleOpr.NewBikeBooking.ManageDealerLoanAmounts" Async="true" Trace="false"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manage Dealer Loan Amounts (EMI)</title>
    <script type="text/javascript" src="http://st.carwale.com/jquery-1.7.2.min.js?v=1.0" ></script>
    <link rel="stylesheet" href="/css/common.css?V1.2" type="text/css" />
    <style type="text/css">
        .errMessage {color:#FF4A4A;}
        .redmsg{
            border: 1px solid red;
            background: #FFCECE;
        }
        .greenMsg {color:#008000;}
    </style>
</head>
<body>
    <form id="myform" runat="server">
    <div class="margin-left10">
        <h1>Manage Dealer Loan Amounts</h1>
        <br />
        <fieldset class="margin-left10">
            <legend>Add Loan properties
            </legend>
            <div id="box" class="box">
                <table width="600">
                    <tr>
                        <th colspan="2" >Property
                        </th>
                        <th colspan="2">Minimum value
                        </th>
                        <th colspan="2">Maximum Value
                        </th>
                    </tr>
                    <tr>
                        <td colspan="6" class="margin10" width="30%">
                            <asp:Label ID="errorSummary" class="errMessage" runat="server" />
                            <asp:HiddenField ID="hdnLoanAmountId" Value="0" runat="server" />
                            <asp:Label ID="finishMessage" class="greenMsg" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" width="50%">Select Down Payment (%) Ex. 55.50<span class="errMessage">*</span>
                        </td>
                        <td colspan="2" class=" margin-left10">
                            <asp:TextBox ID="txtMinPayment" data-name="perc" MaxLength="10" runat="server" Width="95%" />
                        </td>
                        <td colspan="2" class=" margin-left10">
                            <asp:TextBox ID="txtMaxPayment" data-name="perc" MaxLength="10" runat="server" Width="95%" />
                        </td>

                    </tr>

                    <tr>
                        <td colspan="2" class="">Tenure (months), Ex. 12 :<span class="errMessage">* </span>
                        </td>
                        <td colspan="2" class=" margin-left10">
                            <asp:TextBox ID="txtMinTenure" class="numeric" data-name="int" MaxLength="10" pattern="\d+" runat="server" Width="95%" />
                        </td>
                        <td colspan="2" class=" margin-left10">
                            <asp:TextBox ID="txtMaxTenure" class="numeric" data-name="int" MaxLength="10" pattern="\d+" runat="server" Width="95%" />
                        </td>

                    </tr>

                    <tr>
                        <td colspan="2" class="">Rate of interest (%), Ex 12.45 :<span class="errMessage">* </span>
                        </td>
                        <td colspan="2" class=" margin-left10">
                            <asp:TextBox ID="txtMinROI" data-name="perc" MaxLength="10" runat="server" Width="95%" />
                        </td>
                        <td colspan="2" class=" margin-left10">
                            <asp:TextBox ID="txtMaxROI" data-name="perc" MaxLength="10" runat="server" Width="95%" />
                        </td>

                    </tr>

                    <tr>
                        <td colspan="2" class="">Loan To Value (LTV %), Ex. 20 :<span class="errMessage">* </span>
                        </td>
                        <td colspan="2" class=" margin-left10">
                            <asp:TextBox ID="txtMinLtv" data-name="perc" MaxLength="10" runat="server" Width="95%" />
                        </td>
                        <td colspan="2" class=" margin-left10">
                            <asp:TextBox ID="txtMaxLtv" data-name="perc" MaxLength="10" runat="server" Width="95%" />
                        </td>

                    </tr>
                    
                    <tr>
                        <td colspan="2" class="">Select Loan Provider<span class="errMessage">*</span>
                        </td>
                        <td colspan="2" class=" margin-left10">
                            <asp:TextBox ID="textLoanProvider" MaxLength="20" runat="server" Width="95%" />
                        </td>
                    </tr>

                    <tr>
                        <td colspan="2" class="">Select Processing fees Ex. 3,000<span class="errMessage">*</span>
                        </td>
                        <td colspan="2" class=" margin-left10">
                            <asp:TextBox ID="txtFees" data-name="int" class="numeric" MaxLength="10" runat="server" Width="95%" />
                        </td>
                    </tr>

                    <tr>
                        <td colspan="6" class="margin10">
                            <asp:Button ID="btnSaveEMI" runat="server" Text="Add EMI" />
                            <asp:Button ID="btnReset" Text="Reset" runat="server" />
                            <asp:Button ID="btnDelete" OnClientClick="btnDelete_Click(); return false;" Visible="false" runat="server" Text="Delete EMI" />
                        </td>
                    </tr>
                </table>
            </div>
        </fieldset>
    </div>
        <script type="text/javascript">
            
            $(document).ready(function () {

                $("#btnSaveEMI, #btnUpdateEMI").click(function () {
                    $('#finishMessage').html('');
                    $('#errorSummary').html('');
                    var isValid = true;
                    $('input[type="text"]').each(function () {
                        if ($.trim($(this).val()) == '') {
                            isValid = false;
                            $(this).addClass('redmsg');
                        }
                        else {
                            $(this).removeClass('redmsg');
                        }
                    });

                    if (isValid == false) {
                        $('#errorSummary').html('Please fill required fields');
                        return isValid;
                    }
                    else {
                        $('input[data-name="perc"]').each(function () {
                            if (!validatePer($(this))) {
                                isValid = false;
                                $(this).addClass('redmsg');
                            }
                            else {
                                $(this).removeClass('redmsg');
                            }
                        });
                    }
                    if (isValid == false) {
                        $('#errorSummary').html('Invalid input');
                        return isValid;
                    }
                    else {
                        $('input[data-name="int"]').each(function () {
                            if (isNaN($(this).val())){
                                isValid = false;
                                $(this).addClass('redmsg');
                            }
                            else {
                                $(this).removeClass('redmsg');
                            }
                        });
                    }
                    if (isValid == false) {
                        $('#errorSummary').html('Invalid input');
                        return isValid;
                    }
                })
            });

            function validatePer(val) {
                var x = val.val();
                var parts = x.split(".");
                if (typeof parts[1] == "string" && (parts[1].length == 0 || parts[1].length > 2))
                    return false;
                var n = parseFloat(x);
                if (isNaN(n))
                    return false;
                if (n < 0 || n > 100)
                    return false;
                return true;
            }

            $('.numeric').on('input', function (event) {
                this.value = this.value.replace(/[^0-9]/g, '');
            });

            function btnDelete_Click() {
                debugger;
                var loanId = '<%=loanId%>';
                var host = '<%=cwHostUrl%>';
                var acknowledge = confirm("Are you sure you want to delete this record");
                if (acknowledge) {
                    $.ajax({
                        type: "GET",
                        url: host + "api/Dealers/DeleteDealerEMI/?id=" + loanId,
                        success: function (response) {
                            $('input[type="text"]').each(function () {
                                $(this).val('');
                            });
                            $('#errorSummary').html('');
                            $('#finishMessage').html('');
                            $('#btnDelete').hide();
                            $('#errorSummary').html('data has been deleted !');
                        }
                    });
                }
            }
        </script>
    </form>

</body>
</html>
