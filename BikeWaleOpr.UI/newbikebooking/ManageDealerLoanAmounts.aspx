<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikeWaleOpr.NewBikeBooking.ManageDealerLoanAmounts" Async="true" Trace="false"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manage Dealer Loan Amounts (EMI)</title>
    <script type="text/javascript" src="http://st.carwale.com/jquery-1.7.2.min.js?v=1.0" ></script>
    <style type="text/css">
        .errMessage {color:#FF4A4A; margin-left:60px}

    </style>
</head>
<body>
    <form runat="server">
    <div>
        <h1>Manage Dealer Loan Amounts</h1>
        Tenure (months), Ex.  12 : <asp:TextBox ID="txtTenure" runat="server" /><span id="spntxtValidTenure" class="errMessage"></span><br /><br />
        Rate of interest (%), Ex 12 : <asp:TextBox ID="txtROI" runat="server" /><span id="spntxtValidROI" class="errMessage"></span><br /><br />
        Loan To Value (LTV %), Ex. 20 : <asp:TextBOx ID="txtLTV" runat="server" /><span id="spntxtValidLTV" class="errMessage"></span><br /><br />
        Loan Provider : <asp:TextBOx ID="txtloanProvider" runat="server" /><br /><br />
        <asp:Button ID="btnSaveEMI" runat="server" Text="Add EMI" />
        <asp:Button ID="btnUpdateEMI" runat="server" Text="Update EMI" />
        <div  class="floatLeft margin-left10">
            <span id="spntxtValid" class="errMessage"></span>
        </div>
    </div>
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
                    $("#spntxtValid").text("");
                    $("#spntxtValidTenure").text("");
                    $("#spntxtValidROI").text("");
                    $("#spntxtValidLTV").text("");
                    $("#spntxtSuccess").text("");

                    var isValid = true;
                    //var re = /^[0-9]*$/;
                    var re = /^\d{0,2}(\.\d{0,3}){0,1}$/;
                    var tenure= $("#txtTenure").val();
                    var roi = $("#txtROI").val();
                    var ltv = $("#txtLTV").val();
                    
                    if (roi == "" || tenure == "" || ltv == "")
                    {
                        $("#spntxtValid").text("All fields are mandetory.");
                        isValid = false;
                    }
                    else if (!re.test(tenure)) {
                        $("#spntxtValidTenure").text("Please Enter Only Numeric Values");
                        isValid = false;
                    }
                    else if (!re.test(roi)) {
                        $("#spntxtValidROI").text("Please Enter Only Numeric Values");
                        isValid = false;
                    }
                    else if (!re.test(ltv)) {
                        $("#spntxtValidLTV").text("Please Enter Only Numeric Values");
                        isValid = false;
                    }
                    return isValid;
                })
            });
        </script>
    </form>
</body>
</html>
