var num = 0;
var txtloanamount = 0;
var rateofinterest = 0;
var months = 0;
var nls = "";
var EMItoCalculate;
var emiType;

var ns = "01234567890";
var cr = "";
var str = "";

function validateInputs() {
    str = document.getElementById('txtloanamount').value;

    bl = str.length + 3;
    dp = 2;
    txtloanamount = str;

    str = document.getElementById('interestRate').value;

    dp = 4;
    rateofinterest = str;

    var re = /^[0-9]*.?[0-9]*$/

    if (txtloanamount == "") {
        alert("Enter Loan Amount");
        document.getElementById('txtloanamount').focus();
        return true;
    }
    else {
        if (!re.test(txtloanamount) || txtloanamount < 1 || txtloanamount > 100000000) {
            alert("Invalid Loan Amount");
            document.getElementById('txtloanamount').select();
            document.getElementById('txtloanamount').focus();
            return true;
        }
    }

    if (rateofinterest == "") {
        alert("Enter Rate of Interest");
        document.getElementById('interestRate').focus();
        return true;
    }
    else {
        if (!re.test(rateofinterest) || rateofinterest < .0001 || rateofinterest > 99) {
            alert("Invalid Interest Rate");
            document.getElementById('rateofinterest').select();
            document.getElementById('rateofinterest').focus();
            return true;
        }
    }

    if (document.getElementById('txtEmi')) {
        if (document.getElementById('txtEmi').value == "") {
            alert("Please provide valid EMI to calculate Interest");
            document.getElementById('txtEmi').focus();
            return true;
        }
        else {
            if (!re.test(document.getElementById('txtEmi').value) || document.getElementById('txtEmi').value < 1 || document.getElementById('txtEmi').value > 10000000) {
                alert("Please provide valid EMI to calculate Interest");
                document.getElementById('txtEmi').focus();
                return true;
            }
        }
    }
}

function calculateAllEMI() {
    if (validateInputs()) return;
    for (var noMonths = 12; noMonths <= 84; noMonths += 12) {
        computeEMI(noMonths);
    }
}

function computeEMI(months) {
    var interest = rateofinterest / (12 * 100);

    var finalEmi = 0;

    if (document.getElementById('R1').checked) {
        finalEmi = (txtloanamount * interest * Math.pow(1 + interest, months - 1)) / (Math.pow(1 + interest, months) - 1);
    }
    else {
        finalEmi = (txtloanamount * interest * Math.pow(1 + interest, months)) / (Math.pow(1 + interest, months) - 1);
    }

    document.getElementById(months + 'months').innerHTML = '&#8377; ' + Math.round(finalEmi) + '/-';
    document.getElementById(months + 'print').innerHTML = '<a style="cursor:pointer;" title="Click to view/print EMI Schedule." onClick="printEMISchedule(' + months + ')">View Chart</a>';    
    return finalEmi;

}

function printEMISchedule(months) {
    prtSched(months);
}

function prtSched(months) {
    if (validateInputs()) return;
    var totalInterest = 0;
    fpv = computeEMI(months);

    var str = "";
    str = fpv;
    str = Math.round(str);

    pct = rateofinterest / 12 / 100;

    if (document.getElementById('R2').checked) {
        emiType = "Arrears(i.e. Rear Ended EMI's).";
    }
    else {
        emiType = "Advance."
    }

    if (document.getElementById('R1').checked) {
        months -= 1;
        txtloanamount -= fpv;
    }

    ls = "<h3 style='border-bottom:2px solid #555555;font-family:arial, verdana;'>BikeWale EMI Schedule</h3>";
    ls += "<div align='right' class='doNotPrint' style='padding:5px;'><a href='javascript:print()' title='Print the EMI schedule'><img border='0' src='https://www.carwale.com/images/icons/print.gif' />&nbsp;Print</a></div>";
    ls += "<p align='justify'>Following  Schedule Is For : " + document.getElementById('txtloanamount').value
	  + " to repay in " + months
	  + " months.<br>"
	  + "All calculations are based on EMI in " + emiType + "</p>"
	  + "<table border='1' cellpadding='0'><tr>"
	  + "<th>EMI Number</th>"
	  + "<th>EMI Amount</th>"
	  + "<th>Interest Amount</th>"
	  + " <th>Principal Reduction</th>"
	  + " <th>Balance Due</th></tr>";

    for (var j = 0; j < months; j++) {
        ntr = (txtloanamount * pct);
        // Next Line is added by Banwari to calculate Total Interest Amount.
        totalInterest = Number(totalInterest) + Number(ntr);

        prp = fpv - ntr;

        if (prp > txtloanamount) {
            prp = txtloanamount;
        }

        txtloanamount -= prp;

        if (fpv > (ntr + prp)) {
            fpv = ntr + prp;
        }

        str = (j + 1) + ".";

        ls += '<tr>'
			+ '<td>' + str + '</td>'
			+ '<td>' + '&#8377;' + Math.round(fpv) + '</td>'
			+ '<td>' + '&#8377;' + Math.round(ntr) + '</td>'
			+ '<td>' + '&#8377;' + Math.round(prp) + '</td>'
			+ '<td>' + '&#8377;' + Math.round(txtloanamount) + '</td>'
			+ '</tr>';
    }

    ls += '</table>';

    var emiChart = document.getElementById('emiChart').innerHTML;

    document.getElementById('divChartVal').innerHTML = ls
		+ '<p>Total Interest Amount : &#8377; ' + Math.round(totalInterest) + '</p>'
		+ '<p>Complete EMI List</p>'
	 	+ emiChart
		+ '<p>Note: Interest calculated at 1/12th of annual interest rate on'
		+ "    the remaining principal amount. (Rounding errors "
		+ "possible)</p>";

    printSchedule();

}

function advanceEMI(calculatedEMI, monthsEMI) {
    EMItoCalculate = calculatedEMI
    document.getElementById('txtloanamount').value = document.getElementById('txtloanamount').value - calculatedEMI;
    document.getElementById('txtloanamount').value = parseInt(document.getElementById('txtloanamount').value) + parseInt(EMItoCalculate);
}

function printSchedule() {
    var myWindow = window.open('', 'myWindow', 'toolbars=no,width=650,height=500,scrollbars=yes');
    myWindow.document.write('<html><head><title>BikeWale : EMI Schedule</title>');
    myWindow.document.write('<style>body, table, p { font-family:arial, verdana;font-size:11px; }');
    myWindow.document.write('p { align:justify;padding:10px 0 10px 0; } table { border-collapse:collapse; } tr { border:1px solid #000; }');
    myWindow.document.write('td, th { padding:2px; text-align:center;border-right:1px solid #000; } .doNotShow { display:none; } th { background-color:#f3f3f3; } @media print { .doNotShow { display:none; } .doNotPrint { display:none;} }</style>');
    myWindow.document.write('</head><body>');
    myWindow.document.write('<pre>' + document.getElementById('divChartVal').innerHTML + '</pre>');
    myWindow.document.write('</body></html>')
    myWindow.document.close();
}

// Compute Loan Interest.
function getLoanInterest(bikePrice, months, emi, rate) {
    // First verify that the inputs are correct!
    if (validateInputs())
        return "";

    var totalInterest = 0;

    var loanAmount = bikePrice;

    var interest = rate / (12 * 100);
    var prp = 0, fpv = emi, ntr = 0;

    for (var i = 0; i < months; i++) {
        ntr = loanAmount * interest;

        totalInterest += ntr;

        prp = fpv - ntr;

        if (prp > loanAmount) {
            prp = loanAmount;
        }

        loanAmount -= prp;

        if (fpv > (ntr + prp)) {
            fpv = ntr + prp;
        }
        totalInterest = Math.round(totalInterest, 2);
    }
    return totalInterest;
}