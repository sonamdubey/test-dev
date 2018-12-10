var emiViewChart = {
    loanAmt: 0,
    months: 0,
    interest: 0,
    emiType: 0,
    roi: 0,
    loanParamsList: [],
    emiList: [],
    pageLoad: function () {
        emiViewChart.registerEvents();
    },

    registerEvents: function () {
        ko.applyBindings(emiViewChart.koViewModel, $('#dataBind')[0]);
        
        var url = window.location.href.toLowerCase();

        emiViewChart.loanAmt = url.indexOf("loanamt") < 0 ? -1 : (Common.utils.getValueFromQS('loanamt')).slice(0, 9);
        emiViewChart.roi = url.indexOf("roi") < 0 ? -1 : (Common.utils.getValueFromQS('roi')).slice(0, 7);
        emiViewChart.emiType = url.indexOf("emitype") < 0 ? -1 : (Common.utils.getValueFromQS('emitype')).slice(0, 1);
        emiViewChart.months = url.indexOf("months") < 0 ? -1 : (Common.utils.getValueFromQS('months')).slice(0, 2);
        
        if (emiViewChart.loanAmt != -1 && emiViewChart.roi != -1 && emiViewChart.emiType != -1 && emiViewChart.months != -1 && emiViewChart.checkLoanAmt(emiViewChart.loanAmt) && emiViewChart.checkRoi(emiViewChart.roi) && emiViewChart.checkMonths(emiViewChart.months) && emiViewChart.checkEmiType(emiViewChart.emiType)) {
            emiViewChart.calculateAllEmi();
            emiViewChart.calculateLoanParams(emiViewChart.months);
        }
        else
            window.location.href = "/emicalculator/";
    },

    checkLoanAmt: function(loanAmt){
        var re = /^[0-9]*.?[0-9]*$/;
        return (!(!re.test(loanAmt) || Number(loanAmt) < 1 || Number(loanAmt) > 100000000));
    },

    checkRoi: function (roi) {
        var re = /^\d{1,2}(\.\d{1,2})?$/;

        return (re.test(roi));
    },

    checkEmiType: function(emiType){
        return /^['0','1']$/.test(emiType);
    },

    checkMonths: function (months) {
        return (['12', '24', '36', '48', '60', '72', '84'].indexOf(months) != -1);
    },

    calculateAllEmi: function () {

        for (var noMonths = 12; noMonths <= 84; noMonths += 12) {
            emiViewChart.computeEmi(noMonths);
        }
        emiViewChart.koViewModel.emiList(emiViewChart.emiList);
    },

    computeEmi: function (months) {
        var interest = emiViewChart.roi / (1200);
        var finalEmi = 0;

        finalEmi = emiViewChart.emiType == 0 ? (emiViewChart.loanAmt * interest * Math.pow(1 + interest, months - 1)) / (Math.pow(1 + interest, months) - 1) : (emiViewChart.loanAmt * interest * Math.pow(1 + interest, months)) / (Math.pow(1 + interest, months) - 1);
        
        var emiObj = new Object();
        emiObj.month = months;
        emiObj.emi = " " + Common.utils.formatNumeric(Math.round(finalEmi));
        emiViewChart.emiList.push(emiObj);
        
        return finalEmi;
    },

    calculateLoanParams: function (months) {        
        var dueLoan = emiViewChart.loanAmt;
        var emi = emiViewChart.computeEmi(months);
        var roi = emiViewChart.roi / (1200) ;
        var interestAmt = 0;
        var totalInterest = 0;
        var principalReduction = 0;

        if(emiViewChart.emiType == 0){
            $('#emiType').text("Advance.");
            emiViewChart.months -= 1;
            dueLoan -= emi;
        }
        else
            $('#emiType').text("Arrears (i.e. Rear Ended EMI's).");
        
        $('#loanAmt').text(" " + Common.utils.formatNumeric(emiViewChart.loanAmt));
        $('#months').text(emiViewChart.months);

        for (var j = 0; j < emiViewChart.months; j++) {
            interestAmt = (dueLoan * roi);
            totalInterest = totalInterest + interestAmt;

            principalReduction = emi - interestAmt;

            if (principalReduction > dueLoan) {
                principalReduction = dueLoan;
            }
            dueLoan -= principalReduction;
            if (emi > (interestAmt + principalReduction)) {
                emi = interestAmt + principalReduction;
            }

            var loanObj = new Object();
            loanObj.emi = " " + Common.utils.formatNumeric(Math.round(emi));
            loanObj.interestAmt = " " + Common.utils.formatNumeric(Math.round(interestAmt));
            loanObj.principalReduction = " " + Common.utils.formatNumeric(Math.round(principalReduction));
            loanObj.dueLoanAmt = " " + Common.utils.formatNumeric(Math.round(dueLoan));

            emiViewChart.loanParamsList.push(loanObj);
            
        }
        emiViewChart.koViewModel.loanParams(emiViewChart.loanParamsList);
        emiViewChart.koViewModel.interest(" " + Common.utils.formatNumeric(Math.round(totalInterest)));
    },

    koViewModel: {
        interest: ko.observable(),
        loanParams: ko.observableArray(),
        emiList: ko.observableArray()
    }
}

$(document).ready(function () {
    emiViewChart.pageLoad();
});
