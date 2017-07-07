﻿function Validate() {
    var mobileHtml = FormatHtml($('#LeadHtmlMobile').val());
    var desktopHtml = FormatHtml($('#LeadHtmlDesktop').val());
    $('#FormattedHtmlDesktop').val(mobileHtml);
    $('#FormattedHtmlMobile').val(desktopHtml);
    return true;
}

function FormatHtml(strHtml) {
    try {
        var el = $("<section></section>");
        el.html(strHtml.replace('@media', '@@media'));
        el.find('span[name="mfg_maskingNumber"]').text('@Model.MaskingNumber');
        el.find('span[name="mfg_organization"]').text('@Model.Organization');

        var maskingNumber = el.find('a#btnMaskingNumber');
        maskingNumber.before('@if(!String.IsNullOrEmpty(Model.MaskingNumber)){')
        maskingNumber.after('}')
        maskingNumber.attr("href", "tel:+91@Model.MaskingNumber")

        var leadCaptureBtn = el.find('a#btnManufacturer');
        leadCaptureBtn.attr('data-mfgcampid', '@Model.CampaignId').addClass('bw-ga').
            attr('data-item-id', '@Model.DealerId').
            attr('data-item-area', '@Model.Area').
            attr('data-leadsourceid', '@Model.LeadSourceId').
            attr('data-pqsourceid', '@Model.PqSourceId').
            attr('a', '@Model.GAAction').
            attr('c', '@Model.GACategory').
            attr('l', '@Model.GALabel').
            attr('data-item-message', '@Model.PopupSuccessMessage').
            attr('data-item-heading', '@Model.PopupHeading').
            attr('data-item-description', '@Model.PopupDescription').
            attr('data-ispincodrequired', '@Model.PincodeRequired').
            attr('data-dealersrequired', '@Model.DealerRequired').
            attr('data-item-name', '@Model.MakeName').
            attr('data-isemailrequired', '@Model.EmailRequired').
            attr('data-Organization', '@Model.Organization');
    }
    catch(e){}
    return el.html();
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

    if ($(".stepper"))
    {
        $('.stepper').activateStepper();
    }

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