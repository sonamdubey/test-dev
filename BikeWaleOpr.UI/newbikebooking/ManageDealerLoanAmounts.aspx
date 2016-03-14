<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikeWaleOpr.NewBikeBooking.ManageDealerLoanAmounts" Async="true" Trace="false"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manage Dealer Loan Amounts (EMI)</title>
    <script type="text/javascript" src="http://st.carwale.com/jquery-1.7.2.min.js?v=1.0" ></script>
    <%--<link rel="stylesheet" href="/css/common.css?V1.2" type="text/css" />--%>
    <style type="text/css">
        .errMessage {color:#FF4A4A;}
        .errors{
            border: 1px solid red;
            background: #FFCECE;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
        });
</script>
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
                <table>
                    <tr>
                        <th colspan="2">Property
                        </th>
                        <th colspan="2">Minimum value
                        </th>
                        <th colspan="2">Maximum Value
                        </th>
                    </tr>
                    <tr>
                        <td colspan="6" class="margin10">
                            <span id="errorSummary" class="errMessage" runat="server"></span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="floatLeft">Select Down Payment (%) Ex. 55.50<span class="errMessage">*</span>
                        </td>
                        <td colspan="2" class="floatLeft margin-left10">
                            <asp:TextBox ID="txtMinPayment" data-name="perc" MaxLength="10" runat="server" Width="95%" />
                        </td>
                        <td colspan="2" class="floatLeft margin-left10">
                            <asp:TextBox ID="txtMaxPayment" data-name="perc" MaxLength="10" runat="server" Width="95%" />
                        </td>

                    </tr>

                    <tr>
                        <td colspan="2" class="floatLeft">Tenure (months), Ex. 12 :<span class="errMessage">* </span>
                        </td>
                        <td colspan="2" class="floatLeft margin-left10">
                            <asp:TextBox ID="txtMinTenure" MaxLength="10" runat="server" Width="95%" />
                        </td>
                        <td colspan="2" class="floatLeft margin-left10">
                            <asp:TextBox ID="txtMaxTenure" MaxLength="10" runat="server" Width="95%" />
                        </td>

                    </tr>

                    <tr>
                        <td colspan="2" class="floatLeft">Rate of interest (%), Ex 12.45 :<span class="errMessage">* </span>
                        </td>
                        <td colspan="2" class="floatLeft margin-left10">
                            <asp:TextBox ID="txtMinROI" data-name="perc" MaxLength="10" runat="server" Width="95%" />
                        </td>
                        <td colspan="2" class="floatLeft margin-left10">
                            <asp:TextBox ID="txtMaxROI" data-name= MaxLength="10" runat="server" Width="95%" />
                        </td>

                    </tr>

                    <tr>
                        <td colspan="2" class="floatLeft">Loan To Value (LTV %), Ex. 20 :<span class="errMessage">* </span>
                        </td>
                        <td colspan="2" class="floatLeft margin-left10">
                            <asp:TextBox ID="txtMinLtv" data-name="perc" MaxLength="10" runat="server" Width="95%" />
                        </td>
                        <td colspan="2" class="floatLeft margin-left10">
                            <asp:TextBox ID="txtMaxLtv" data-name="perc" MaxLength="10" runat="server" Width="95%" />
                        </td>

                    </tr>
                    
                    <tr>
                        <td colspan="2" class="floatLeft">Select Loan Provider<span class="errMessage">*</span>
                        </td>
                        <td colspan="2" class="floatLeft margin-left10">
                            <asp:TextBox ID="textLoanProvider" MaxLength="10" runat="server" Width="95%" />
                        </td>
                    </tr>

                    <tr>
                        <td colspan="2" class="floatLeft">Select Processing fees Ex. 3,000<span class="errMessage">*</span>
                        </td>
                        <td colspan="2" class="floatLeft margin-left10">
                            <asp:TextBox ID="txtFees" data-name="perc" MaxLength="10" runat="server" Width="95%" />
                        </td>
                    </tr>

                    <tr>
                        <td colspan="6" class="margin10">
                            <asp:Button ID="btnSaveEMI" runat="server" Text="Add EMI" />
                            <asp:Button ID="btnUpdateEMI" runat="server" Text="Update EMI" />
                            <%--<asp:Button ID="btnAdd" Text="Save benefit" OnClientClick="return btnAdd_Click();" runat="server" />--%>
                            <asp:Button ID="btnReset" Text="Reset" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </fieldset>
    </div>

    <%--Tenure (months), Ex.  12 :
    <asp:TextBox ID="txtTenure" runat="server" /><span id="spntxtValidTenure" class="errMessage"></span><br />
    <br />
    Rate of interest (%), Ex 12 :
    <asp:TextBox ID="txtROI" runat="server" /><span id="spntxtValidROI" class="errMessage"></span><br />
    <br />
    Loan To Value (LTV %), Ex. 20 :
    <asp:TextBox ID="txtLTV" runat="server" /><span id="spntxtValidLTV" class="errMessage"></span><br />
    <br />
    Loan Provider :
    <asp:TextBox ID="txtloanProvider" runat="server" /><br />
    <br />--%>
    <%--<div>
        <table>
            <tr>
                <th>Tenure</th>
                <th>Rate Of Interest</th>
                <th>Loan To Value (LTV)</th>
            </tr>
            <tr>
                <td><%= tenure %></td>
                <td><%= loanToValue %></td>
                <td><%= rateOfInterest %></td>
            </tr>            
        </table>
    </div>--%>
        <script type="text/javascript">
            
            $(document).ready(function () {

                $("#btnSaveEMI, #btnUpdateEMI").click(function () {
                    debugger;
                    var isValid = true;
                    $('input[type="text"]').each(function () {
                        if ($.trim($(this).val()) == '') {
                            isValid = false;                            alert($(this).val());                            $(this).css({
                                "border": "1px solid red",                                "background": "#FFCECE"
                            });
                        }                        else {
                            $(this).css({
                                "border": "",                                "background": ""
                            });
                        }
                    });                    if (isValid == false) {
                        $('#errorSummary').html('Please fill required fields');
                        return isValid;
                    }                    else {
                        //alert('Thank you for submitting');
                        $('input[data-name="perc"]').each(function () {
                            if (!validatePer($(this))) {
                                isValid = false;                                $(this).css({
                                    "border": "1px solid red",                                    "background": "#FFCECE"
                                });
                            }
                            else {
                                $(this).css({
                                    "border": "",                                    "background": ""
                                });
                            }

                        });
                    }
                    if (isValid == false) {
                        $('#errorSummary').html('Invalid input');
                        return isValid;
                    }
                    //$("#spntxtValid").text("");
                    //$("#spntxtValidTenure").text("");
                    //$("#spntxtValidROI").text("");
                    //$("#spntxtValidLTV").text("");
                    //$("#spntxtSuccess").text("");

                    //var isValid = true;
                    ////var re = /^[0-9]*$/;
                    //var re = /^\d{0,2}(\.\d{0,3}){0,1}$/;
                    //var tenure= $("#txtTenure").val();
                    //var roi = $("#txtROI").val();
                    //var ltv = $("#txtLTV").val();
                    
                    //if (roi == "" || tenure == "" || ltv == "")
                    //{
                    //    $("#spntxtValid").text("All fields are mandetory.");
                    //    isValid = false;
                    //}
                    //else if (!re.test(tenure)) {
                    //    $("#spntxtValidTenure").text("Please Enter Only Numeric Values");
                    //    isValid = false;
                    //}
                    //else if (!re.test(roi)) {
                    //    $("#spntxtValidROI").text("Please Enter Only Numeric Values");
                    //    isValid = false;
                    //}
                    //else if (!re.test(ltv)) {
                    //    $("#spntxtValidLTV").text("Please Enter Only Numeric Values");
                    //    isValid = false;
                    //}
                    //return isValid;
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
        </script>
    </form>

</body>
</html>
