var VolvoV90 = {
    pageLoad: function () {
        VolvoV90.pageLoadRegisterEvents();
    },

    pageLoadRegisterEvents: function () {

        $("#downloadDoc").on("click", function () {
            var doc = new jsPDF();
            var receiptContent = $("#receiptContent").html().replace('₹', 'Rs.')
            doc.fromHTML(receiptContent, 15, 15, {
                'width': 170,
                'elementHandlers': VolvoV90.specialElementHandlers
            });
            doc.save('booking-receipt.pdf');
        });

        $("#printDoc").on("click", function (e) {
            VolvoV90.printReceipt('receiptContent');
            e.preventDefault();
        });

        $("#mailDoc").on("click", function () {
            VolvoV90.apiCall.sendMail(modelData);
        });
    },
   
    apiCall: { 
        sendMail: function (data) {
            Common.utils.ajaxCall({
                type: 'POST',
                url: '/es/booking/volvo-v90/sendmail/',
                dataType: 'Json',
                data: data
            });
        }
    },

    printReceipt: function (divID) {
        var mywindow = window.open('', '', '');

        mywindow.document.write('<html><head><title>' + document.title + '</title>');
        mywindow.document.write('</head><body >');
        mywindow.document.write('<h1>' + document.title + '</h1>');
        mywindow.document.write(document.getElementById(divID).innerHTML);
        mywindow.document.write('</body></html>');

        mywindow.document.close(); // necessary for IE >= 10
        mywindow.focus(); // necessary for IE >= 10*/

        mywindow.print();
        mywindow.close();
    }
}

$(document).ready(function () {
    VolvoV90.pageLoad();
});


