function Validate() {
    var el = $("<section></section>");
    var htmlMobile = el.html($('#LeadHtmlMobile').val());
    $(htmlMobile).find('span[name="mfg_maskingNumber"]').text('@Model.MaskingNumber ');
    $(htmlMobile).find('span[name="mfg_makename"]').text('@Model.MakeName');
    return true;
}
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
    $('#emiproperties input[type="checkbox"], #emiproperties input[type=text]').each(function () {
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
    $('#leadproperties input[type="checkbox"], #leadproperties input[type=text]').each(function () {
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
    $('form input[type="text"]').each(function () {
        if ($(this).val().length > 0) {
            $(this).prop("disabled", true);
            $(this).parent().nextAll().has(":checkbox").first().find(":checkbox").prop("checked", false);
        }
    });

    if ($("#HasEmiProperties").prop('checked')) {
        HasEMISelected();
    }
    else {
        HasEMINotSelected();
    }
    if ($("#HasLeadProperties").prop('checked')) {
        HasLeadSelected();
    }
    else {
        HasLeadNotSelected();
    }
});

$('#btnConfigureCampaign').click(function () {
    var isValid = true;
   
    $('form input[type="text"]').each(function () {

        var currentEle = $(this);

        if (!(currentEle.parent().nextAll().has(":checkbox").first().find(":checkbox").prop("checked")))
        {
            if (currentEle.val().trim() == '')
            {
                currentEle.parent().first().find("label").attr("data-error", "Enter " + currentEle.parent().first().find("label").text());
                currentEle.addClass("Invalid");
                isValid = false;
            }
            else {
                currentEle.removeClass("Invalid");
            }
        }
        else
        {
            currentEle.removeClass("Invalid");
        }
    });

    return isValid;
});