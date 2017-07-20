var $dateInput = $('.datepicker').pickadate({
    selectMonths: true, // Creates a dropdown to control month   
    closeOnSelect: true,
    onClose: function () { if (dateValue != $("#reviewDateEle").val()) $("#reviewDate").val($("#reviewDateEle").val()); },
    onOpen: function () { dateValue = $("#reviewDateEle").val() },
    onSet: function (ele) { if (ele.select) { this.close(); } }
});

$(document).ready(function () {    

    $('#startTimeEleDesktop').val("00:00:00");
    $('#endTimeEleDesktop').val("00:00:00");
    $('#startTimeEleMobile').val("00:00:00");
    $('#endTimeEleMobile').val("00:00:00");

    if ($(".stepper"))
    {
        $('.stepper').activateStepper();
    }

});
