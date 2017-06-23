﻿
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

$("#selMaskingNumber").change(function () {
    vmConfigureCampaign.maskingNumber($('#selMaskingNumber').find("option:selected").val());
    $('#labelMaskingNumber').addClass('active');    
});

var configureCampaignPage = $('#ConfigureCampaign');

var ConfigureCampaign = function () {
    var self = this;
    self.description = ko.observable($('#txtCampaignDescription').attr('data-value'));
    self.startDate = ko.observable($('#startDateEle').attr('data-value'));
    self.maskingNumber = ko.observable($('#txtMaskingNumber').attr('data-value'));

    if ($('#inputDealerId') && $('#inputDealerId').val())
        var dealerId = $('#inputDealerId').val().trim();

    if ($('#inputCampaignId') && $('#inputCampaignId').val())
        var campaignId = $('#inputCampaignId').val().trim();

    self.configureCampaign = function () {        
        if (!self.description() || self.description() == "" || !self.startDate() || self.startDate() == "")
        {
            $("#alertModal").modal('open');
            return false;
        }
        
        if (($('#txtDailyLeadLimit').val() && ($('#txtDailyLeadLimit').val() < 0)) || ($('#txtTotalLeadLimit').val() && ($('#txtTotalLeadLimit').val() < 0)))
        {
            $('#modal-p').text("Lead Limits should be positive");
            $("#alertModal").modal('open');
            return false;
        }

        var selectedPages = '';
        
        var selectedOptions = $('#select-pages').find("option:selected");

        for(var i = 1; i <selectedOptions.length; i++)
        {
            selectedPages = selectedPages + ',' + selectedOptions[i].value;
        }

        $('#CampaignPages').val(selectedPages);

        $('#StartDate').val($('#startDateEle').val() + ' ' + $('#startTimeEle').val());

        if ($('#endDateEle').val() != "")
            $('#EndDate').val($('#endDateEle').val() + ' ' + $('#endTimeEle').val());              

        var oldMaskingNumber = $("#olMaskingNumber").val().trim();        
        if (self.maskingNumber() != "" && oldMaskingNumber != "" && oldMaskingNumber != self.maskingNumber()) {
            var r = confirm("You are mapping " + nos + " dealer numbers to 1 masking number. Are you sure you want to continue?");
            if (!r) {
                isValid = false;
                alert("Please ensure that there is only one number for this dealer in DCRM. Campaign has not been saved.");
            }

        }

        return true;
    };

    self.releaseMaskingNumber = function () {
        try
        {                    
            var maskingNumber = $("#txtMaskingNumber").val().trim();
            $('#labelMaskingNumber').removeClass('active');
            if (self.maskingNumber().length > 0) {
                if (confirm("Do you want to release the number?")) {
                    $.ajax({
                        type: "POST",
                        url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                        data: '{"dealerId":"' + dealerId + '","campaignId":"' + campaignId + '", "maskingNumber":"' + self.maskingNumber() + '"}',
                        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "ReleaseNumber"); },
                        success: function (response) {
                            if (JSON.parse(response).value) {
                                $("#txtMaskingNumber").val('');
                                alert("Masking Number is released successful.");                               
                            }
                            else {
                                alert("There was error while releasing masking number. Please contact System Administrator for more details.");
                            }
                        }

                    });
                }
            }
            self.maskingNumber("");
            return false;
        } catch (e) {
            alert("An error occured. Please contact System Administrator for more details.");
        }
    }

    self.bindMaskingNumber = function () {
        try {

            $.ajax({
                type: "POST",
                url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                data: '{"dealerId":"' + dealerId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetDealerMaskingNumbers"); },
                success: function (response) {
                    var res = JSON.parse(response);
                    if (res) {
                        $('#ddlMaskingNumber').empty();
                        $.each(res.value, function (index, value) {
                            ('#ddlMaskingNumber').append($('<option>').text(value.Number).attr('value', value.IsAssigned));
                        });
                    }
                }

            });
        } catch (e) {
            alert("An error occured. Please contact System Administrator for more details.");
        }
    }

}

$(document).ready(function () {
    
});

var vmConfigureCampaign = new ConfigureCampaign;
if (configureCampaignPage) {
    ko.applyBindings(vmConfigureCampaign, configureCampaignPage[0]);
}
