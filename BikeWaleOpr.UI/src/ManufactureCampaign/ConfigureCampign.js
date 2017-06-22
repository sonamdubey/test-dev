
var $dateInput = $('.datepicker').pickadate({
    selectMonths: true, // Creates a dropdown to control month   
    closeOnSelect: true,
    onClose: function () { if (dateValue != $("#reviewDateEle").val()) $("#reviewDate").val($("#reviewDateEle").val()); },
    onOpen: function () { dateValue = $("#reviewDateEle").val() },
    onSet: function (ele) { if (ele.select) { this.close(); } }
});

$("#chkConfigureEmi").change(function () {
    if ($("#chkConfigureEmi").prop('checked')) {
        $('#chkEmiButtonMobile').removeAttr("disabled");
        $('#chkEmiPropertyMobile').removeAttr("disabled");
        $('#chkEmiButtonDesktop').removeAttr("disabled");
        $('#chkEmiPropertyDesktop').removeAttr("disabled");
        $('#selPropertyPriority').prop('disabled', false);
        $('#selPropertyPriority').material_select();
    }
    else {
        $('#chkEmiButtonMobile').attr("disabled", true);
        $('#chkEmiPropertyMobile').attr("disabled", true);
        $('#chkEmiButtonDesktop').attr("disabled", true);
        $('#chkEmiPropertyDesktop').attr("disabled", true);
        $('#selPropertyPriority').prop('disabled', true);
        $('#selPropertyPriority').material_select();
    }
});

$("#chkConfigureLeads").change(function () {
    if ($("#chkConfigureLeads").prop('checked')) {
        $('#chkLeadsButtonMobile').removeAttr("disabled");
        $('#chkLeadsPropertyMobile').removeAttr("disabled");
        $('#chkLeadsButtonDesktop').removeAttr("disabled");
        $('#chkLeadsPropertyDesktop').removeAttr("disabled");
        $('#chkLeadsMobileTemplate').removeAttr("disabled");
        $('#chkLeadsDesktopTemplate').removeAttr("disabled");
        $('#selLeadPropertyPriority').prop('disabled', false);
        $('#selLeadPropertyPriority').material_select();
    }
    else {
        $('#chkLeadsButtonMobile').attr("disabled", true);
        $('#chkLeadsPropertyMobile').attr("disabled", true);
        $('#chkLeadsButtonDesktop').attr("disabled", true);
        $('#chkLeadsPropertyDesktop').attr("disabled", true);
        $('#chkLeadsMobileTemplate').attr("disabled", true);
        $('#chkLeadsDesktopTemplate').attr("disabled", true);
        $('#selLeadPropertyPriority').attr("disabled", true);
        $('#selLeadPropertyPriority').prop('disabled', true);
        $('#selLeadPropertyPriority').material_select();
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