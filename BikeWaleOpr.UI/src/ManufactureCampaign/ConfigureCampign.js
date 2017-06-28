
var $dateInput = $('.datepicker').pickadate({
    selectMonths: true, // Creates a dropdown to control month   
    closeOnSelect: true,
    onClose: function () { if (dateValue != $("#reviewDateEle").val()) $("#reviewDate").val($("#reviewDateEle").val()); },
    onOpen: function () { dateValue = $("#reviewDateEle").val() },
    onSet: function (ele) { if (ele.select) { this.close(); } }
});

//function Validate() {
//    console.log('yeh');
//    return true;
//}
$("#HasEmiProperties").change(function () {
    if ($("#HasEmiProperties").prop('checked')) {
        HasEMISelected();
    }
    else {
        HasEMINotSelected();
    }
});

$("#HasLeadProperties").change(function () {
    if ($("#HasLeadProperties").prop('checked')) {
        HasLeadSelected();
    }
    else {
        HasLeadNotSelected();
    }
});


function HasEMISelected() {
    $('#emiproperties input[type="checkbox"]').each(function () {
            $(this).prop("disabled", false);
    });
    $('#emiproperties input[type="text"]').each(function () {
        if ($(this).val().length > 0) {
            $(this).prop("disabled", false);
        }
    });
    $('#EmiPriority').prop('disabled', false);
    $('#EmiPriority').material_select();
}
function HasEMINotSelected() {
    $('#emiproperties input[type="checkbox"],input[type=text]').each(function () {
        $(this).prop("disabled", true);
    });
    $('#EmiPriority').prop('disabled', true);
    $('#EmiPriority').material_select();
}

function HasLeadSelected() {
    $('#leadproperties input[type="checkbox"]').each(function () {
        $(this).prop("disabled", false);
    });
    $('#leadproperties input[type="text"]').each(function () {
        if ($(this).val().length > 0) {
            $(this).prop("disabled", false);
        }
    });
    $('#LeadPriority').prop('disabled', false);
    $('#LeadPriority').material_select();
}

function HasLeadNotSelected() {
    $('#leadproperties input[type="checkbox"],input[type=text]').each(function () {
        $(this).prop("disabled", true);
    });
    $('#LeadPriority').attr("disabled", true);
    $('#LeadPriority').prop('disabled', true);
    $('#LeadPriority').material_select();
}
$(".default-chk").change(function () {
    if ($(this).prop('checked')) {
        $(this).parent().prev().find("input[type='text']").prop('disabled', true);
    }
    else {
        $(this).parent().prev().find("input[type='text']").prop('disabled', false);
    }
});

$(document).ready(function () {
    debugger;
    // All checkboxes are selected by default
    $('form input[type="text"]').each(function () {
        if ($(this).val().length > 0) {
            $(this).prop("disabled", true);
            $(this).parent().nextAll().has(":checkbox").first().find(":checkbox").prop("checked", false);
        }
    });

    if ($("#HasEmiProperties").prop('checked')) {
        HasEMISelected();
        //$('#emiproperties input[type="checkbox"]').each(function () {
        //    $(this).prop("disabled", false);
        //});
        //$('#emiproperties input[type="text"]').each(function () {
        //    if ($(this).val().length > 0) {
        //        $(this).prop("disabled", false);
        //    }
        //});
    }
    else {
        HasEMINotSelected();
    }
    if ($("#HasLeadProperties").prop('checked')) {
        HasLeadSelected();
        //$('#leadproperties input[type="checkbox"]').each(function () {
        //    $(this).prop("disabled", false);
        //});
        //$('#leadproperties input[type="text"]').each(function () {
        //    if ($(this).val().length > 0) {
        //        $(this).prop("disabled", false);
        //    }
        //});
    }
    else {
        HasLeadNotSelected();
        //$('#leadproperties input').each(function () {
        //    $(this).prop("disabled", true);
        //});
    }
});