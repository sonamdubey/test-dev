var emiCalculator = {
    loanAmount: 0,
    rateOfInterest: 0,
    months: 0,
    emiType: 0,
    emiList: [],
    empty: false,
    pageLoad: function () {
        emiCalculator.registerEvents();
        emiCalculator.calculateAllEmi();
    },

    registerEvents: function () {
        ko.applyBindings(emiCalculator.koEmiModel, $('#displayEmi')[0]);

        $('#loanAmount').live('change', function () {
            emiCalculator.loanAmount = $('#loanAmount').val().trim();
            if (emiCalculator.validateInputs())
                emiCalculator.hideErrorMessage($('#loanAmount'));
        });

        $('#interestRate').live('change', function () {
            emiCalculator.rateOfInterest = $('#interestRate').val().trim();
            if (emiCalculator.validateInputs())
                emiCalculator.hideErrorMessage($('#interestRate'));
        });
    },

    validateInputs: function () {
        var re = /^[0-9]*.?[0-9]*$/
        var reRoi = /^\d{1,2}(\.\d{1,2})?$/;

        var isValid = true;

        if (emiCalculator.loanAmount == "" || emiCalculator.loanAmount == undefined) {
            emiCalculator.showCustomeErrorMessage($('#loanAmount'), "Please enter loan amount.")
            isValid = false;
        }
        else if (!re.test(emiCalculator.loanAmount)) {
            emiCalculator.showCustomeErrorMessage($('#loanAmount'), "Please enter valid Loan Amount.");
            isValid = false;
        }
        else if (emiCalculator.loanAmount < 1 || emiCalculator.loanAmount > 100000000) {
            emiCalculator.showCustomeErrorMessage($('#loanAmount'), "loan Amount should be lesser than ₹ 10,00,00,000");
            isValid = false;
        }
        else
            emiCalculator.hideErrorMessage($('#loanAmount'));

        if (emiCalculator.rateOfInterest == "" || emiCalculator.rateOfInterest == undefined) {
            emiCalculator.showCustomeErrorMessage($('#interestRate'), "Please enter Rate of Interest.");
            isValid = false;
        }
        else if (!reRoi.test(emiCalculator.rateOfInterest)) {
            emiCalculator.showCustomeErrorMessage($('#interestRate'), "Please enter valid Interest Rate");
            isValid = false;
        }
        else if (emiCalculator.rateOfInterest < .0001 || emiCalculator.rateOfInterest > 99) {
            emiCalculator.showCustomeErrorMessage($('#interestRate'), "Please enter Interest Rate between 1 to 99.");
            isValid = false;
        }
        else
            emiCalculator.hideErrorMessage($('#interestRate'));
        return isValid;
    },

    showCustomeErrorMessage: function (field, errMsg) {
        field.addClass('border-red').siblings('.error-icon,.cw-blackbg-tooltip').removeClass('hide');
        field.siblings('.cw-blackbg-tooltip').text(errMsg);
    },

    hideErrorMessage: function (field) {
        field.removeClass('border-red').siblings('.error-icon,.cw-blackbg-tooltip').addClass('hide');
    },

    calculateAllEmi: function () {
        emiCalculator.loanAmount = $('#loanAmount').val().trim();
        emiCalculator.rateOfInterest = $('#interestRate').val().trim();

        if (emiCalculator.validateInputs()) {

            emiCalculator.emiList = [];
            emiCalculator.emiType = $('#emi1').is(':checked') ? 0 : 1;

            emiCalculator.koEmiModel.roi(emiCalculator.rateOfInterest);
            emiCalculator.koEmiModel.loanAmt(emiCalculator.loanAmount);
            emiCalculator.koEmiModel.emiType(emiCalculator.emiType);  //0 for EMI in Advance : 1 for EMI in Arrears

            for (var noMonths = 12; noMonths <= 84; noMonths += 12) {
                emiCalculator.computeEmi(noMonths);
            }
            emiCalculator.koEmiModel.emiList(emiCalculator.emiList);
        }
    },

    computeEmi: function (months) {
        var interest = emiCalculator.rateOfInterest / (1200);

        var finalEmi = 0;

        finalEmi = emiCalculator.emiType == 0 ? (emiCalculator.loanAmount * interest * Math.pow(1 + interest, months - 1)) / (Math.pow(1 + interest, months) - 1) : (emiCalculator.loanAmount * interest * Math.pow(1 + interest, months)) / (Math.pow(1 + interest, months) - 1);

        var emiObj = new Object();
        emiObj.month = months;
        emiObj.emi = " " + Common.utils.formatNumeric(Math.round(finalEmi));
        emiCalculator.emiList.push(emiObj);

        return finalEmi;
    },

    koEmiModel: {
        emiType: ko.observable(),
        roi: ko.observable(),
        loanAmt: ko.observable(),
        emiList: ko.observableArray()
    }
}

$(document).ready(function () {
    emiCalculator.pageLoad();
});
