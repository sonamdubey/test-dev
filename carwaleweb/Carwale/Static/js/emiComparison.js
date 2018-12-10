var emiCompare = {
    loanAmount1: 0,
    loanAmount2: 0,
    emi1: 0,
    emi2: 0,
    tenure1: 3,
    tenure2: 3,
    isCompareCall: false,
    pageLoad: function () {
        emiCompare.registerEvents();
    },

    registerEvents: function () {
        ko.applyBindings(emiCompare.koViewModel, $('#interestRate')[0]);
        $('#compare').on('click', function (e) {
            emiCompare.compare();
        });       

        $('#drpTenure1,#drpTenure2').change(function () {
            if(emiCompare.isCompareCall)
                emiCompare.compare();
        });        
    },

    compare: function () {
        emiCompare.isCompareCall = true;
        emiCompare.loanAmount1 = $('#txtLoanAmount1').val().trim();
        emiCompare.loanAmount2 = $('#txtLoanAmount2').val().trim();
        emiCompare.emi1 = $('#txtEMI1').val().trim();
        emiCompare.emi2 = $('#txtEMI2').val().trim();
        emiCompare.tenure1 = $('#drpTenure1').val();
        emiCompare.tenure2 = $('#drpTenure2').val();

        if (emiCompare.checkAllValidations())
        {
            emiCompare.hideErrorMessage($('#txtEMI1'));
            emiCompare.hideErrorMessage($('#txtEMI2'));
            $('#lblInterest1,#lblInterest2').removeClass('text-bold')
            emiCompare.emiComp();
        }
    },
    
    checkAllValidations: function(){
        var isValid = true;

        isValid = emiCompare.validateInput($('#txtLoanAmount1'), emiCompare.loanAmount1, "loan amount.");
        isValid = (emiCompare.validateInput($('#txtLoanAmount2'), emiCompare.loanAmount2, "loan amount.") && isValid);
        isValid = (emiCompare.validateEmi(emiCompare.emi1, emiCompare.tenure1, emiCompare.loanAmount1, $('#txtEMI1')) && isValid);
        isValid = (emiCompare.validateEmi(emiCompare.emi2, emiCompare.tenure2, emiCompare.loanAmount2, $('#txtEMI2')) && isValid);

        return isValid;
    },

    validateInput: function (field, value, error) {
        var re = /^[0-9]*.?[0-9]*$/;
        var isValid = true;
        
        if (value == "" || value == undefined) {
            emiCompare.showCustomeErrorMessage(field, "Please enter " + error);
            isValid = false;
        }
        else if (!re.test(value)) {
            emiCompare.showCustomeErrorMessage(field, "Please enter valid " + error);
            isValid = false;
        }
        else
            emiCompare.hideErrorMessage(field);
        return isValid;
    },
    
    validateEmi: function (emi, tenure, loanAmt, field) {
        var isValid = true;
        var re = /^[0-9]*.?[0-9]*$/;

        if (field.val() == "" || field.val() == undefined) {
            emiCompare.showCustomeErrorMessage(field, "Please enter EMI.");
            isValid = false;
        }
        else if (!re.test(field.val())) {
            emiCompare.showCustomeErrorMessage(field, "Please enter valid EMI.");
            isValid = false;
        }
        else if ((emi * (tenure * 12)) < loanAmt) {
            emiCompare.showCustomeErrorMessage(field, "Cannot pay the loan with this EMI.");
            isValid = false;
        }
        else
            emiCompare.hideErrorMessage(field);
        return isValid;
    },

    showCustomeErrorMessage: function (field, errMsg) {
        field.addClass('border-red').siblings('.error-icon,.cw-blackbg-tooltip').removeClass('hide');
        field.siblings('.cw-blackbg-tooltip').text(errMsg);
    },

    hideErrorMessage: function (field) {
        field.removeClass('border-red').siblings('.error-icon,.cw-blackbg-tooltip').addClass('hide');
    },

    emiComp: function () {
        var interest1 = 0;
        var interest2 = 0;
        var lowestAmount = 0;        

        if (emiCompare.loanAmount1 != "" && emiCompare.emi1 != "" && emiCompare.loanAmount1 != undefined && emiCompare.emi1 != undefined) {
            interest1 = (Math.round(emiCompare.calculateInterestRate(emiCompare.loanAmount1, emiCompare.emi1, emiCompare.tenure1)*100))/100;

            if (interest1 != 0) {
                emiCompare.koViewModel.roi1(interest1);
            }
            else if ($('#txtEMI1').siblings('.error-icon,.cw-blackbg-tooltip').hasClass('hide')) {
                emiCompare.showCustomeErrorMessage($('#txtEMI1'), "EMI is more than Max EMI possible.");
            }
        }

        if (emiCompare.loanAmount2 != "" && emiCompare.emi2 != "" && emiCompare.loanAmount2 != undefined && emiCompare.emi2 != undefined) {
            interest2 = (Math.round(emiCompare.calculateInterestRate(emiCompare.loanAmount2, emiCompare.emi2, emiCompare.tenure2)*100))/100;

            if (interest2 != 0) {
                emiCompare.koViewModel.roi2(interest2);
            }
            else if ($('#txtEMI2').siblings('.error-icon,.cw-blackbg-tooltip').hasClass('hide')) {
                emiCompare.showCustomeErrorMessage($('#txtEMI2'), "EMI is more than Max EMI possible.");
            }
        }

        if (interest2 != 0 && interest1 != 0)
            $('#interestResult').removeClass('hide');

        if (emiCompare.koViewModel.roi1() != "" && emiCompare.koViewModel.roi2 != "")
        {
            emiCompare.koViewModel.roi2() >= emiCompare.koViewModel.roi1() ? $('#lblInterest1').addClass('text-black font14 text-bold') : $('#lblInterest2').addClass('text-black font14 text-bold');
        }
    },
    
    calculateInterestRate: function (principleAmount, EMI, tenure) {
        var lowerInterestRate = 0;
        var higherInterestRate = 100;

        var lowerEMI = emiCompare.calculateEmi(principleAmount, lowerInterestRate, tenure);
        var higherEMI = emiCompare.calculateEmi(principleAmount, higherInterestRate, tenure);
        var midInterestRate = 0;
        var midEMI = 0;
        if (EMI >= lowerEMI && EMI <= higherEMI) {
            do {
                midInterestRate = (lowerInterestRate + higherInterestRate) / 2;
                midEMI = emiCompare.calculateEmi(principleAmount, midInterestRate, tenure);

                if (EMI <= midEMI) {
                    higherInterestRate = midInterestRate;
                }
                if (EMI >= midEMI) {
                    lowerInterestRate = midInterestRate;
                }
            } while (!(Math.abs(EMI - midEMI) < 1))
            return midInterestRate;
        }
        else {
            return 0;
        }
    },

    calculateEmi: function (principleAmount, interestRate, tenure) {
        var calculatedAmount = 0.0;
        var monthlyInterestRate = (interestRate / 1200);
        var monthlyTenure = (tenure * 12);

        var calculatePower = Math.pow((1 + monthlyInterestRate), monthlyTenure);
            
        calculatedAmount = calculatePower == 1 ? 0 : (principleAmount * monthlyInterestRate * calculatePower) / (calculatePower - 1);
        return calculatedAmount;
    },

    koViewModel: {
        roi1: ko.observable(),
        roi2: ko.observable()
    },

}

$(document).ready(function () {
    emiCompare.pageLoad();
});

