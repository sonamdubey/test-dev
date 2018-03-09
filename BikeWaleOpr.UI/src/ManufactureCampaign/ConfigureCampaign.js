
var $dateInput = $('.datepicker').pickadate({
    selectMonths: true, // Creates a dropdown to control month   
    closeOnSelect: true,
    onClose: function () { if (dateValue != $("#reviewDateEle").val()) $("#reviewDate").val($("#reviewDateEle").val()); },
    onOpen: function () { dateValue = $("#reviewDateEle").val() },
    onSet: function (ele) { if (ele.select) { this.close(); } }
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

        var dailyStartTime = $("#dailyStartTimeEle").val();
        var dailyEndTime = $("#dailyEndTimeEle").val();
        if (!self.description() || self.description() == "")
        {
        	Materialize.toast("Campaign Description is Mandatory. Please fill it.", 5000);
            return false;
        }
        
        if (($('#txtDailyLeadLimit').val() && ($('#txtDailyLeadLimit').val() < 0)) || ($('#txtTotalLeadLimit').val() && ($('#txtTotalLeadLimit').val() < 0)))
        {
            Materialize.toast("Lead Limits should be positive", 5000);
            return false;
        }       

        var selectedPages = '';
        
        var selectedOptions = $('#select-pages option:selected');
       
        if (selectedOptions.length == 1)
        {
            Materialize.toast("Please select atleast one page.", 5000);
            return false;
        }

        for(var i = 1; i <selectedOptions.length; i++)
        {
            selectedPages = selectedPages + ',' + selectedOptions[i].value;
        }

        if (!self.startDate() || self.startDate() == "") {
        	Materialize.toast("Campaign Start Date is Mandatory. Please fill it.", 5000);
        	return false;
        }

        if ((dailyStartTime != "" && dailyEndTime == "") || (dailyStartTime == "" && dailyEndTime != "")) {
        	Materialize.toast("Either set both start and end time OR none.", 5000);
        	return false;
        }
        if (dailyStartTime != "" && dailyEndTime != "" && dailyStartTime.localeCompare(dailyEndTime) != -1) {
        	Materialize.toast("Daily Campaign Start time must be less than Daily Campaign End time.", 5000);
        	return false;
        }

        $('#CampaignPages').val(selectedPages);
		
        var campaignDaysValue = 0;
        var campaignDays = $('#select-days option:selected');
        if (campaignDays.length == 1) {
        	Materialize.toast("Please select atleast one campaign day.", 5000);
        	return false;
        }

        for (var i = 1; i < campaignDays.length; i++) {
        	campaignDaysValue += parseInt(campaignDays[i].value);
        }

        $('#campaignDays').val(campaignDaysValue);

        $('#StartDate').val($('#startDateEle').val() + ' ' + $('#startTimeEle').val());

        if ($('#endDateEle').val() != "")
            $('#EndDate').val($('#endDateEle').val() + ' ' + $('#endTimeEle').val());     

        $("#DailyStartTime").val(dailyStartTime);
        $("#DailyEndTime").val(dailyEndTime);

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

var vmConfigureCampaign = new ConfigureCampaign;
if (configureCampaignPage) {
    ko.applyBindings(vmConfigureCampaign, configureCampaignPage[0]);
}
