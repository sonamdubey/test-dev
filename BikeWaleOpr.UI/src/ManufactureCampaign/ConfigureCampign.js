
var $dateInput = $('.datepicker').pickadate({
    selectMonths: true, // Creates a dropdown to control month   
    closeOnSelect: true,
    onClose: function () { if (dateValue != $("#reviewDateEle").val()) $("#reviewDate").val($("#reviewDateEle").val()); },
    onOpen: function () { dateValue = $("#reviewDateEle").val() },
    onSet: function (ele) { if (ele.select) { this.close(); } }
});

function Validate() {
    console.log('yeh');
    return true;
}
$("#HasEmiProperties").change(function () {
    if ($("#HasEmiProperties").prop('checked')) {
        $('#chkEmiButtonMobile').removeAttr("disabled");
        $('#chkEmiPropertyMobile').removeAttr("disabled");
        $('#chkEmiButtonDesktop').removeAttr("disabled");
        $('#chkEmiPropertyDesktop').removeAttr("disabled");
        $('#EmiPriority').prop('disabled', false);
        $('#EmiPriority').material_select();
    }
    else {
        $('#chkEmiButtonMobile').attr("disabled", true);
        $('#chkEmiPropertyMobile').attr("disabled", true);
        $('#chkEmiButtonDesktop').attr("disabled", true);
        $('#chkEmiPropertyDesktop').attr("disabled", true);
        $('#EmiPriority').prop('disabled', true);
        $('#EmiPriority').material_select();
    }
});

$("#HasLeadProperties").change(function () {
    if ($("#HasLeadProperties").prop('checked')) {
        $('#chkLeadsButtonMobile').removeAttr("disabled");
        $('#chkLeadsPropertyMobile').removeAttr("disabled");
        $('#chkLeadsButtonDesktop').removeAttr("disabled");
        $('#chkLeadsPropertyDesktop').removeAttr("disabled");
        $('#chkLeadsMobileTemplate').removeAttr("disabled");
        $('#chkLeadsDesktopTemplate').removeAttr("disabled");
        $('#LeadPriority').prop('disabled', false);
        $('#LeadPriority').material_select();
    }
    else {
        $('#chkLeadsButtonMobile').attr("disabled", true);
        $('#chkLeadsPropertyMobile').attr("disabled", true);
        $('#chkLeadsButtonDesktop').attr("disabled", true);
        $('#chkLeadsPropertyDesktop').attr("disabled", true);
        $('#chkLeadsMobileTemplate').attr("disabled", true);
        $('#chkLeadsDesktopTemplate').attr("disabled", true);
        $('#LeadPriority').attr("disabled", true);
        $('#LeadPriority').prop('disabled', true);
        $('#LeadPriority').material_select();
    }
});

$(".default-chk").change(function () {
    if ($(this).prop('checked')) {
        $(this).parent().prev().find("input[type='text']").prop('disabled', true);
    }
    else {
        $(this).parent().prev().find("input[type='text']").prop('disabled', false);
    }
});

$(document).ready(function () {
   
});