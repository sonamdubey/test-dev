<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.EMICalculatorMin" %>
<div class="calculate-emi-container text-center">
    <div class="margin-bottom40">
        <span class="bw-circle-icon emi-calc-logo"></span>
    </div>
    <p class="font16 margin-bottom30">Instant calculate loan EMI</p>
    <div class="calculate-emi-search-container">
        <div class="calculate-emi-tool-search">
            <div class="loan-amount-box">
                <div class="form-control-box">
                    <input autocomplete="off" class="form-control rounded-corner0 border-no" type="text" maxlength="8" placeholder="Enter loan amount" id="txtLoanAmount" tabindex="1">
                    <span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black" style="display: none"></span>
                    <span class="bwsprite error-icon hide"></span>
                    <div class="bw-blackbg-tooltip hide"></div>
                </div>
            </div>
            <div class="interest-rate-boxSelect">
                <div class="form-control-box">
                    <input autocomplete="off" class="form-control rounded-corner0 border-no" type="text" maxlength="5" placeholder="Rate of Interest eg. 12.5" id="txtRateOfInterest" tabindex="2" <%--value="<%= rateOfInterest %>"--%> />
                    <span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black" style="display: none"></span>
                    <span class="bwsprite error-icon hide" ></span>
                    <div class="bw-blackbg-tooltip hide"></div>
                </div>
            </div>
            <div class="clear"></div>
        </div>
        <div class="calculate-btn">
            <a href="#" class="font16 btn btn-orange btn-lg rounded-corner-no-left" id="btnCalcEmi" tabindex="3">Calculate</a>
        </div>

        <div class="clear"></div>
    </div>
</div>
<script type="text/javascript">
    $(function () {
        $loanAmount = $('#txtLoanAmount');
        $rateOfInterest = $('#txtRateOfInterest');
    
        $("#btnCalcEmi").click(function () {

        toggleErrorMsg($rateOfInterest, false);
        toggleErrorMsg($loanAmount, false);
        var re = /^[0-9]*$/;
        var reRateOfInterest = /^([0-9]{1,2}){1}(\.[0-9]{1,2})?$/;
        var loanAmt = $loanAmount.val();
        var rateOfInterestVal = $rateOfInterest.val();
        var isValid = true;

        if (isValid && !reRateOfInterest.test(rateOfInterestVal) && !(parseFloat(rateOfInterestVal) < 30)) {
            toggleErrorMsg($rateOfInterest, true, "Please enter valid rate of interest");
            isValid = false;
        }

        if (isValid && loanAmt == "" || loanAmt == "Enter loan amount") {
            toggleErrorMsg($loanAmount, true, "Please enter valid loan amount");
            isValid =  false;
        }
        else if (isValid && loanAmt != "" && re.test(loanAmt) == false) {
            toggleErrorMsg($loanAmount, true, "Please provide numeric data only for loan amount");
            isValid = false;
        }
        else if (isValid && parseInt(loanAmt, 10) < 5000) {
            toggleErrorMsg($loanAmount, true, "Please enter loan amount atleast 5000 or greater");
            isValid = false;
        }
      
        if (isValid && !reRateOfInterest.test(rateOfInterestVal) && isNaN(rateOfInterestVal) && !(parseFloat(rateOfInterestVal) > 30) && parseFloat(rateOfInterestVal) <= 0) {
            toggleErrorMsg($rateOfInterest, true, "Please enter valid rate of interest");
            isValid = false;
        }

        if (isValid)
        {
            path = "/finance/emicalculator.aspx?la=" + loanAmt + "&rt=" + rateOfInterestVal;
           window.location = path;
        }
        else {
            return isValid;
        }        
        
    });

});
</script>
