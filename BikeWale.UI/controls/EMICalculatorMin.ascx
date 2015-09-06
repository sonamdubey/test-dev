<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.controls.EMICalculatorMin" %>
<div class="calculate-emi-container text-center margin-bottom50">
    <div class="margin-bottom40">
        <span class="bw-circle-icon user-review-logo"></span>
    </div>
    <p class="font16 margin-bottom30">Instant calculate loan EMI</p>
    <div class="calculate-emi-search-container">
        <div class="calculate-emi-tool-search">
            <div class="loan-amount-box">
                <div class="form-control-box">
                    <input class="form-control rounded-corner0 border-no" type="text" placeholder="Enter loan amount" id="txtLoanAmount">
                    <span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black" style="display: none"></span>
                    <span class="bwsprite error-icon hide" id="sprErrLoanAmount"></span>
                    <div class="bw-blackbg-tooltip hide" id="errMsgLoanAmount"></div>
                </div>
            </div>
            <div class="interest-rate-boxSelect">
                <div class="form-control-box">
                    <input class="form-control rounded-corner0 border-no" type="text" placeholder="Rate of Interest" id="txtRateOfInterest" value="<%= rateOfInterest %>" />
                    <span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black" style="display: none"></span>
                    <span class="bwsprite error-icon hide" id="sprErrRateOfInterest"></span>
                    <div class="bw-blackbg-tooltip hide" id="errMsgRateOfInterest"></div>
                </div>
            </div>
            <div class="clear"></div>
        </div>
        <div class="calculate-btn">
            <button class="font18 btn btn-orange btn-lg rounded-corner-no-left" id="btnCalcEmi">Calculate</button>
        </div>

        <div class="clear"></div>
    </div>
</div>
<script type="text/javascript">
    $("#btnCalcEmi").click(function () {
        var re = /^[0-9]*$/;
        var reRateOfInterest = /^([0-9]{1,2}){1}(\.[0-9]{1,2})?$/;
        var loanAmt = $("#txtLoanAmount").val();
        var rateOfInterest = $("#txtRateOfInterest").val();

        if (!reRateOfInterest.test(rateOfInterest) && !(parseFloat(rateOfInterest) < 100)) {
            $("#sprErrRateOfInterest").removeClass('hide');
            $("#errMsgRateOfInterest").removeClass('hide');
            $('#errMsgRateOfInterest').html("Please enter valid rate of interest");
        }

        if (loanAmt == "" || loanAmt == "Enter loan amount") {
            $("#sprErrLoanAmount").removeClass('hide');
            $("#errMsgLoanAmount").removeClass('hide');
            $('#errMsgLoanAmount').html("Please enter valid loan amount");
            return false;
        } else if (loanAmt != "" && re.test(loanAmt) == false) {
            $("#sprErrLoanAmount").removeClass('hide');
            $("#errMsgLoanAmount").removeClass('hide');
            $('#errMsgLoanAmount').html("Please provide numeric data only for loan amount");
            return false;
        } else if (parseInt(loanAmt, 10) < 5000) {
            $("#sprErrLoanAmount").removeClass('hide');
            $("#errMsgLoanAmount").removeClass('hide');
            $('#errMsgLoanAmount').html("Please enter loan amount atleast 5000 or greater");
            return false;
        } else {
            $("#sprErrLoanAmount").addClass('hide');
            $("#errMsgLoanAmount").addClass('hide');
            if (!reRateOfInterest.test(rateOfInterest) && !(parseFloat(rateOfInterest) < 100)) {
                $("#sprErrRateOfInterest").removeClass('hide');
                $("#errMsgRateOfInterest").removeClass('hide');
                $('#errMsgRateOfInterest').html("Please enter valid rate of interest");
            }
            else {
                window.location = "/finance/emicalculator.aspx?la=" + loanAmt + "&rt=" + rateOfInterest;
            }
        }
    });
</script>
