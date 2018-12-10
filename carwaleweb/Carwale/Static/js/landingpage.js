var dealerInquiryDetails = new Object();

$("#btnLpSumbit").click(function () {
    if (isvalid())
    {
        callPostDealerInquiry();
        if($(this).data('versa-tracking') == "True")
        {
            VersaTracking.track();
        }
    }
});

function desktopModelChange() {
    window.location.href = window.location.href.split('&')[0] + '&modelId=' + $("#drpLpModels").val();
}

function callPostDealerInquiry() {
    createDealerObject();
    submitLead(dealerInquiryDetails);
    setCookie();
    trackLpSubmit();
}
var leadDestinations = {
    campaign: 1,
    dealer:2
};

function trackLpSubmit() {
    var label = "";
    if (type == leadDestinations.campaign)
        label = 'campaignId-' + campaignId;
    else if (type == leadDestinations.dealer)
        label = 'dealerId-' + dealerId;
    Common.utils.trackAction('CWNonInteractive', 'LandingPage_' + (PlatformId == 1 ? 'Desktop' : 'Mobile'), ' Lead_Submit', label);
    var url = window.location.href.toLowerCase();
    var clickLabel = "LP_" + (url.indexOf("id") < 0 ? "" : Common.utils.getValueFromQS('id'));
    Common.utils.trackAction('CWInteractive', (PlatformId == 1 ? 'LandingPage' : 'LandingPage_m'), (PlatformId == 1 ? 'Submit_' + clickLabel : 'Submit_' + clickLabel + '_m'), clickLabel);
}

function ModelChange() {
    var modelId = $("#drpLpModels").val();
    var url = ('/webapi/CarModelData/GetCarDetailsByModelId/?modelid=' + modelId)
    $.ajax({
            type: 'GET',
            url: url,
            async: false,
            dataType: 'json',
            success: function (json) {
                    carDetails = json; window.modelId = carDetails.ModelId;
                    $("#divTestDrivePhotos").attr("src", carDetails.HostUrl + '272x153' + carDetails.OriginalImage);
                }
    });
}

function setCookie(custName, custEmail, custMobile) {
    document.cookie = '_CustomerName=' + $('#tdname').val() + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
    document.cookie = '_CustMobile=' + $('#tdmobile').val() + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
    if ($('#tdemail').is(":visible"))
        document.cookie = '_CustEmail=' + $('#tdemail').val() + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
}

function createDealerObject() {
    dealerInquiryDetails.CityId = !document.getElementById("drpLpCities") ? defaultCity : $('#drpLpCities').val();

    if (type == 1)
        dealerInquiryDetails.DealerId = campaignId;
    else if (type == 2)
        dealerInquiryDetails.ActualDealerId = dealerId;

    dealerInquiryDetails.ModelId = !document.getElementById("drpLpModels") ? defaultModel : $("#drpLpModels").val();
    dealerInquiryDetails.Email = $('#tdemail').val();
    dealerInquiryDetails.Name = $('#tdname').val();
    dealerInquiryDetails.Mobile = $('#tdmobile').val();
    dealerInquiryDetails.BuyTimeText = "1 week";
    dealerInquiryDetails.BuyTimeValue = 7;
    dealerInquiryDetails.RequestType = 1;
    dealerInquiryDetails.UtmaCookie = isCookieExists('__utma') ? $.cookie('__utma') : '';
    dealerInquiryDetails.UtmzCookie = isCookieExists('_cwutmz') ? $.cookie('_cwutmz') : '';
    dealerInquiryDetails.InquirySourceId = 97;
    dealerInquiryDetails.LeadClickSource = LeadClickSource;
    dealerInquiryDetails.PlatformSourceId = PlatformId;
    dealerInquiryDetails.PQId = 0;
    dealerInquiryDetails.ModelsHistory = getUserModelHistory();
    dealerInquiryDetails.IsAutoApproved = false;
    dealerInquiryDetails.AssignedDealerId = -1;
    dealerInquiryDetails.SponsoredBannerCookie = isCookieExists('_sb' + dealerInquiryDetails.ModelId) ? $.cookie('_sb' + dealerInquiryDetails.ModelId) : '';
    dealerInquiryDetails.LandingPageId = $('#btnLpSumbit').attr('data-landingPageId');
}

function submitLead(dealerInquiryDetails) {
    $.ajax({
        type: 'POST',
        url: '/webapi/DealerSponsoredAd/PostDealerInquiry/',
        data: dealerInquiryDetails,
        success: function (encryptId) {
            try {
                $('#tdDetail').hide();
                $('#primaryHead').hide();
                $('#thankYou').show();

                if (typeof dealerInquiryDetails.EncryptedPQDealerAdLeadId == "undefined" || dealerInquiryDetails.EncryptedPQDealerAdLeadId == "") {
                    leadConversionTracking.track(dealerInquiryDetails.LeadClickSource, dealerInquiryDetails.DealerId);
                }

                trackLeadForFord();

            } catch (err) {
                console.log("call to PostDealerInquiry function");
            }
        }
    });
}

function getUserModelHistory() {
    if (isCookieExists('_userModelHistory')) {
        var userHistoryString = $.cookie('_userModelHistory');
        var userHistory = userHistoryString.split('~').join(',');
        return userHistory;
    } else {
        return "";
    }
}

function trackLeadForFord() {
    if (Common.utils.getValueFromQS('id') == 'MTMy') {
        leadConversionTracking.floodLightTrackingFord();
    }
}

function isvalid() {
    var custName = $.trim($('#tdname').val());
    var custMobile = $.trim($('#tdmobile').val());
    var custEmail = $.trim($('#tdemail').val());
    var custCity = $.trim($('#drpLpCities').val());
    var custModel = $.trim($("#drpLpModels").val());
    isValidCustdetails = false;
    var retVal = true;
    var errorMsg = "";
    var reName = /^([-a-zA-Z ']*)$/;
    var reEmail = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$/;
    var reMobile = /^[6789]\d{9}$/;
    var _custMobile = custMobile;

    hideCustomerFormErrors();
    if (custName == "") {
        retVal = false;
        $(' .custName .error-icon').show();
        $(' .custName .cw-blackbg-tooltip').html('Please enter your name').show();
    } else if (reName.test(custName) == false) {
        retVal = false;
        $(' .custName .error-icon').show();
        $(' .custName .cw-blackbg-tooltip').html('Please enter only alphabets').show();
    }
    else if (custName.length == 1) {
        retVal = false;
        $(' .custName .error-icon').show();
        $(' .custName .cw-blackbg-tooltip').html('Please enter your complete name').show();
    }
    if ($('#tdemail').is(":visible")) {
        if (custEmail.toString().toLowerCase() == "") {
            retVal = false;
            $(' .custEmail .error-icon').show();
            $(' .custEmail .cw-blackbg-tooltip').html('Please enter your email').show();
        }
        else if (!reEmail.test(custEmail.toString().toLowerCase())) {
            retVal = false;
            $(' .custEmail .error-icon').show();
            $(' .custEmail .cw-blackbg-tooltip').html('Invalid email').show();
        }
    }
    if (_custMobile == "") {
        retVal = false;
        $(' .custMobile .error-icon').show();
        $(' .custMobile .cw-blackbg-tooltip').html('Please enter your mobile number').show();
    }
    else if (_custMobile.length != 10) {
        retVal = false;
        $(' .custMobile .error-icon').show();
        $(' .custMobile .cw-blackbg-tooltip').html('Mobile number should be of 10 digits').show();
    }
    else if (!reMobile.test(_custMobile)) {
        retVal = false;
        $(' .custMobile .error-icon').show();
        $(' .custMobile .cw-blackbg-tooltip').html('Please provide a valid 10 digit Mobile number').show();
    }
    if (Number(custCity) <= 0 && $('#drpLpCities').is(":visible")) {
        retVal = false;
        $(' .custCity .error-icon').show();
        $(' .custCity .cw-blackbg-tooltip').html('Please select your city').show();
    }
    if (Number(custModel) <= 0 && $('#drpLpModels').is(":visible")) {
        retVal = false;
        $(' .custModel .error-icon').show();
        $(' .custModel .cw-blackbg-tooltip').html('Please select a model').show();
    }

    return retVal;
}

function hideCustomerFormErrors() {
    $('.error-icon').html('').hide();
    $('.cw-blackbg-tooltip ').html('').hide();
}

$(document).ready(function () {
    $('#tdname').val($.cookie('_CustomerName'));
    if ($('#tdemail').is(":visible"))
        $('#tdemail').val($.cookie('_CustEmail'));
    $('#tdmobile').val($.cookie('_CustMobile'));
    if ($('#drpLpModels').is(":visible")) {
        $('#drpLpModels' + ' option[value=""]').prop('disabled', true);
        $('#drpLpModels' + ' option[value=' + defaultModelId + ']').prop('selected', true);
    }
});
