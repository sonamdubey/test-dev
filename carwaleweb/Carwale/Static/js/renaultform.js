function getDealerObject() {
    var data = new Object();
    data.MakeId = CarDetails.carMakeId;
    data.MakeName = CarDetails.carMakeName;
    data.ModelId = CarDetails.carModelId;
    data.ModelName = CarDetails.carModelName
    data.VersionId = 0;
    data.VersionName = "";
    data.CityId = window.CityId ? CityId : window.userCityId ? userCityId : 0;
    data.ZoneId = ((typeof (isVersionPage) != "undefined" && isVersionPage == "true") || isModelCityPage.toString().toLowerCase() != "true") ? (Number($.cookie("_CustZoneIdMaster")) > 0 ? $.cookie("_CustZoneIdMaster") : "") : "";
    data.AreaId = ((typeof (isVersionPage) != "undefined" && isVersionPage == "true") || isModelCityPage.toString().toLowerCase() != "true") ? (Number($.cookie("_CustAreaId")) > 0 ? $.cookie("_CustAreaId") : "") : "";
    data.DealerLeadBusinessType = sponsorDlrBusType;
    data.DealerName = sponsorDlrName;
    data.DealerID = campaignId;
    data.DealerAutoAssignPanel = sponsorDlrLeadPanel;
    data.ShowEmail = showEmail;
    data.AdType = (typeof (isVersionPage) != "undefined" && isVersionPage == "true") ? "emi-version-page" : (isModelCityPage.toString().toLowerCase() == "true" ? "emi-modelCity-page" : "emi-model-page");
    data.PQId = "0";
    data.GACat = (typeof (isVersionPage) != "undefined" && isVersionPage == "true") ? "EmiAssistVersionPage" : (isModelCityPage.toString().toLowerCase() == "true" ? "EmiAssistModelCityPage" : "EmiAssistModelPage");
    data.GAActionDifferential = "";
    data.Caption = "Get Accurate EMI Quote";
    return data;
};

// for desktop
function getModelDealerObject() {
    var data = new Object();
    data.MakeId = CarDetails.carMakeId;
    data.MakeName = CarDetails.carMakeName;
    data.ModelId = CarDetails.carModelId;
    data.ModelName = CarDetails.carModelName;
    data.ShowEmail = showEmail.toString() == "true" ? true : false;
    data.VersionId = 0;
    data.VersionName = "";
    data.CityId = window.CityId ? CityId : window.userCityId ? userCityId : 0;
    data.ZoneId = Number($.cookie("_CustZoneIdMaster")) > 0 ? $.cookie("_CustZoneIdMaster") : "";
    data.AreaId = Number($.cookie("_CustAreaId")) > 0 ? $.cookie("_CustAreaId") : "";
    data.DealerLeadBusinessType = sponsorDlrBusType;
    data.DealerName = sponsorDlrName;
    data.DealerID = campaignId;
    data.DealerAutoAssignPanel = sponsorDlrLeadPanel;
    data.LeadClickSource = "";
    data.AdType = "";
    data.PQId = "0";
    data.GACat = "";
    data.GAActionDifferential = "";
    data.Caption = "";
    data.PopupID = "";
    return data;
}


var RenaultCampaignData = {};
var popupData = new Object();
popupData.PopupID = "";
var renaultDesktopForm = false;
var dealerInquiryDetails = new Object();

$(window).load(function () {
    var oemLeadPanelId = 3;
    var desktopPageIds = [31, 55, 38];
    var submitId = $('#renaultBtnSubmit');
    if (PageId != undefined && desktopPageIds.indexOf(parseInt(PageId)) >= 0) {
        renaultDesktopForm = true;
        RenaultCampaignData = getModelDealerObject();
        RenaultCampaignData.PopupID = "#leadform";
        //RenaultCampaignData.DealerAutoAssignPanel == oemLeadPanelId ? bindDealersRenault(RenaultCampaignData) : hideDealers();
    }
    else {
        RenaultCampaignData = getDealerObject();
        RenaultCampaignData.PopupID = "#leadform";
        //sponsorDlrLeadPanel == oemLeadPanelId ? bindDealersRenault(RenaultCampaignData) : hideDealers();
    }

    if (RenaultCampaignData.CityId <= 0)
    {
        //showing city
        $("#personCity").parent().removeClass('hide');
        // binding city autocomplete
        bindCityAutocomplete(renaultDesktopForm);
    }

    $(popupData.PopupID + ' #custName').val($.cookie('_CustomerName'));
    $(popupData.PopupID + ' #custMobile').val($.cookie('_CustMobile'));

    submitId.click(function () {
        popupData = RenaultCampaignData;
        submitId.val("Processing....");
        if (renaultDesktopForm) {
            popupData.LeadClickSource = submitId.attr("leadclicksource");
            if (validateRenaultForm()) {
                setCustomerCookiesRenault();
                callPostDealerInquiryRenault(true);
                hideForm();
            }
        }
        else {
            popupData.LeadClickSource = submitId.attr("sourceclick");
            if (processPQLeadRenault()) {
                hideForm();
            }
        }
        submitId.val("Submit");
    });

    $("#leadform #custName").on('change', function () {
        validateNameRenault($.trim($("#leadform #custName").val()));
    });

    $("#leadform #custMobile").on('change', function () {
        validateMobileRenault($.trim($("#leadform #custMobile").val()));
    });
});

function dealerChange()
{
    popupData = RenaultCampaignData;
    drpDealerChangeRenault();
    validateDealerRenault($("#leadform #hdnDealerId").val());
}

function drpDealerChangeRenault() {
    $("#leadform #hdnDealerId").val($(popupData.PopupID + ' #dealerList option:selected').val());
    $("#leadform #hdnDealerName").val($(popupData.PopupID + ' #dealerList option:selected').text().split(' - ')[1]);
    dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: popupData.GACat, act: 'dealerDrpFocus' });
}

function validateRenaultForm() {
    //var assignedDealerId = $("#leadform #hdnDealerId").val();
    var renaultCustName = $.trim($('#leadform #custName').val());
    var renaultCustMobile = $.trim($(' #leadform #custMobile').val());
    var isValid = true;
    
    isValid = validateNameRenault(renaultCustName);
    isValid = validateMobileRenault(renaultCustMobile) && isValid;
    isValid = validateCity() && isValid;
    //isValid = validateDealerRenault(assignedDealerId) && isValid;
    
    return isValid;
}

function validateMobileRenault(renaultCustMobile) {
    var reMobile = /^[6789]\d{9}$/;
    var id = '#leadform .custMobile';

    if (renaultCustMobile == "") {
        showError(id, 'Please enter your mobile number');
        return false;
    }
    else if (renaultCustMobile.length != 10) {
        showError(id, 'Mobile number should be of 10 digits');
        return false;
    }
    else if (!reMobile.test(renaultCustMobile)) {
        showError(id, 'Please provide a valid 10 digit Mobile number');
        return false;
    }
    hideError(id);
    return true;
}

function validateNameRenault(renaultCustName)
{
    var reName = /^([-a-zA-Z ']*)$/;
    var id = '#leadform .custName';

    if (renaultCustName == "") {
        showError(id, 'Please enter your name');
        return false;
    } 
    else if (renaultCustName.length == 1) {
        showError(id, 'Please enter your complete name');
        return false;
    }
    else if (!reName.test(renaultCustName)) {
        showError(id, 'Please enter only alphabets');
        return false;
    }
    hideError(id);
    return true;
}

function validateDealerRenault(assignedDealerId)
{
    var id = '#leadform .dealerDropdownRenault';
    if ((typeof assignedDealerId == "undefined" || Number(assignedDealerId) <= 0) && $('#leadform #dealerList').is(":visible"))
    {
        showError(id, 'Please select your nearest dealership');
        return false;
    }
    hideError(id);
    return true;
}

function validateCity() {
    if ($('#personCity').is(":visible") && ($.cookie('_CustCityIdMaster') == null || $.cookie('_CustCityIdMaster') <= 0)) {
        if (renaultDesktopForm) {
            //desktop
            showHideMatchError(true, $('#personCity'), "Please select a city");
        } else {
            // mobile
            showHideMatchError(true, $('#personCity').closest('.easy-autocomplete'), "Please select a city");
        }
        return false;
    }
    return true;
}

function hideDealers()
{
    drpDealerCount = 0;
    $(popupData.PopupID + " #hdnDealerId").val("-1");
    $(popupData.PopupID + " #hdnDealerName").val("us");
    $(popupData.PopupID + ' .dealerDropdownRenault').hide();
}

function hideForm()
{
    $('#renaultCustInfo').hide();
    $('.renault-lead-msg').show();
}

function hideError(id)
{
    $(id + ' .error-icon').hide();
    $(id + ' .cw-blackbg-tooltip').html('').hide();
}

function showError(id, message)
{
    $(id + ' .error-icon').show();
    $(id + ' .cw-blackbg-tooltip').html(message).show();
}

function bindDealersRenault(campaignData) {
    $("#btnSubmit").val("Processing...").prop("disabled", true);
    $('#dealerList').empty();

    drpDealerCount = 0;
    if (data.CityId && data.CityId > 0 && (campaignData.DealerAutoAssignPanel == "3" || campaignData.DealerAutoAssignPanel == "-1")) {
        $.ajax({
            type: 'GET',
            url: '/api/dealers/ncs/?modelid=' + campaignData.ModelId + '&cityid=' + campaignData.CityId + '&campaignid=' + campaignData.DealerID,
            dataType: 'Json',
            success: function (json) {

                var viewModel = {
                    dealer: ko.observableArray(json)
                };

                ko.cleanNode($('#dealerList')[0]);
                ko.applyBindings(viewModel, $('#dealerList')[0]);
                $("#btnSubmit").val("Submit").prop("disabled", false);
                saveDealerNameRenault(json);
                
            },
            error: function (err) {
                $("#btnSubmit").val("Submit").prop("disabled", false);
                saveDealerNameRenault([]);
            }
        });
    }
    else {
        $("#btnSubmit").val("Submit").prop("disabled", false);
    }
}

function callPostDealerInquiryRenault(isNewLead) {
    $('#btnSubmit').val("Processing....").prop("disabled", true);
    var data = popupData;

    if (renaultDesktopForm) {
        createDealerObjectRenaultDesktop(data);
    } else {
        createDealerObjectRenault(data);
    }

    submitSellerLeadRenault(dealerInquiryDetails);
}

function submitSellerLeadRenault(dealerInquiryDetails) {
    $.ajax({
        type: 'POST',
        url: '/webapi/DealerSponsoredAd/PostDealerInquiry/',
        data: dealerInquiryDetails,
        success: function (encryptId) {
            try {

                popupData.EncryptedPQDealerAdLeadId = encryptId;
                
                leadConversionTracking.track(dealerInquiryDetails.LeadClickSource, dealerInquiryDetails.DealerId);

            } catch (err) {
                console.log(err);
            }
        },
        error: function (err) {
            console.log(err);
        }
    });
    popupData.isLeadSubmitted = true;
}

// for mobile
function createDealerObjectRenault(data) {
    dealerInquiryDetails.CityId = data.CityId > 0 ? data.CityId : leadCityId;
    dealerInquiryDetails.ZoneId = data.ZoneId;
    dealerInquiryDetails.AreaId = data.AreaId;
    dealerInquiryDetails.VersionId = data.VersionId;
    dealerInquiryDetails.DealerId = data.DealerID;
    dealerInquiryDetails.ModelId = data.ModelId;
    dealerInquiryDetails.ModelName = data.ModelName;
    dealerInquiryDetails.Email = $.cookie('_CustEmail');
    dealerInquiryDetails.Name = $.cookie('_CustomerName');
    dealerInquiryDetails.Mobile = $.cookie('_CustMobile');
    dealerInquiryDetails.BuyTimeText = "1 week";
    dealerInquiryDetails.BuyTimeValue = 7;
    dealerInquiryDetails.RequestType = 1;
    dealerInquiryDetails.UtmaCookie = isCookieExists('__utma') ? $.cookie('__utma') : '';
    dealerInquiryDetails.UtmzCookie = isCookieExists('_cwutmz') ? $.cookie('_cwutmz') : '';
    dealerInquiryDetails.InquirySourceId = getInquirySourceMobile();
    dealerInquiryDetails.LeadClickSource = data.LeadClickSource;
    dealerInquiryDetails.PlatformSourceId = renaultDesktopForm ? "1" : "43";
    dealerInquiryDetails.EncryptedPQDealerAdLeadId = (typeof popupData.EncryptedPQDealerAdLeadId === 'undefined' || popupData.EncryptedPQDealerAdLeadId == "") ? "" : popupData.EncryptedPQDealerAdLeadId;
    dealerInquiryDetails.PQId = data.PQId;
    dealerInquiryDetails.ModelsHistory = getUserModelHistory();
    dealerInquiryDetails.LeadBussinessType = data.DealerLeadBusinessType;

    var AssignedDealerId = $('#dealerList').is(':visible') ? $('#dealerList option:selected').val() : -1;

    if ((data.DealerAutoAssignPanel == "3" && AssignedDealerId != "-1") || (data.DealerAutoAssignPanel == -1 && AssignedDealerId != "" && AssignedDealerId != "-1")) {
        dealerInquiryDetails.IsAutoApproved = true;
        dealerInquiryDetails.AssignedDealerId = AssignedDealerId;
    } else {
        dealerInquiryDetails.IsAutoApproved = false;
        dealerInquiryDetails.AssignedDealerId = -1;
    }
    dealerInquiryDetails.SponsoredBannerCookie = isCookieExists('_sb' + dealerInquiryDetails.ModelId) ? $.cookie('_sb' + dealerInquiryDetails.ModelId) : '';
}

// for desktop
function createDealerObjectRenaultDesktop(data) {
    $(popupData.PopupID + " .btnSubmit").val("Processing...").prop("disabled", true);

    var AssignedDealerId = $(popupData.PopupID + " #hdnDealerId").val();

    dealerInquiryDetails.CityId = data.CityId > 0 ? data.CityId : leadCityId;
    dealerInquiryDetails.ZoneId = data.ZoneId;
    dealerInquiryDetails.AreaId = data.AreaId;
    dealerInquiryDetails.VersionId = data.VersionId;
    dealerInquiryDetails.DealerId = data.DealerID;
    dealerInquiryDetails.ModelName = data.ModelName;
    dealerInquiryDetails.ModelId = data.ModelId;
    dealerInquiryDetails.Email = $.cookie('_CustEmail');
    dealerInquiryDetails.Name = $.cookie('_CustomerName');
    dealerInquiryDetails.UtmaCookie = isCookieExists('__utma') ? $.cookie('__utma') : '';
    dealerInquiryDetails.UtmzCookie = isCookieExists('_cwutmz') ? $.cookie('_cwutmz') : '';
    dealerInquiryDetails.Mobile = $.cookie('_CustMobile');
    dealerInquiryDetails.BuyTimeText = "1 week";
    dealerInquiryDetails.BuyTimeValue = 7;
    dealerInquiryDetails.RequestType = 1;
    dealerInquiryDetails.InquirySourceId = 99; // check from where it is set
    dealerInquiryDetails.LeadClickSource = data.LeadClickSource;
    dealerInquiryDetails.PlatformSourceId = "1";
    dealerInquiryDetails.PQId = data.PQId;
    dealerInquiryDetails.ModelsHistory = userHistory.getUserModelHistory();
    dealerInquiryDetails.LeadBussinessType = data.DealerLeadBusinessType;

    var AssignedDealerId = $('#dealerList').is(':visible') ? $('#dealerList option:selected').val() : -1;

    if ((data.DealerAutoAssignPanel == "3" && AssignedDealerId != "-1") || (data.DealerAutoAssignPanel == -1 && AssignedDealerId != "" && AssignedDealerId != "-1")) {
        dealerInquiryDetails.IsAutoApproved = true;
        dealerInquiryDetails.AssignedDealerId = AssignedDealerId;
    } else {
        dealerInquiryDetails.IsAutoApproved = false;
        dealerInquiryDetails.AssignedDealerId = -1;
    }
    dealerInquiryDetails.SponsoredBannerCookie = isCookieExists('_sb' + dealerInquiryDetails.ModelId) ? $.cookie('_sb' + dealerInquiryDetails.ModelId) : '';
}

function processPQLeadRenault(isNewLead) {
    var data = popupData;

    var valid = $(popupData.PopupID + ' .custEmailOptional').is(":visible") ? validateOptionalEmailRenault() : isvalidRenault();
    if (valid) {
        setCustomerCookiesRenault();
        if (typeof popupData.CityName != 'undefined' && $.cookie('_CustCityIdMaster') < 1) {
            globalLocation.setLocationCookies(popupData.CityId, popupData.CityName, popupData.AreaId, typeof popupData.AreaName != 'undefined' ? popupData.AreaName : "Select Area", popupData.ZoneId, typeof popupData.ZoneName != 'undefined' ? popupData.ZoneName : "Select Zone");
            $("#pq-popup-close, #btnDone").attr("IsRefresh", true);
        }
        callPostDealerInquiryRenault(isNewLead);
        return true;
    } else {
        dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: popupData.GACat, act: 'Unsuccessful-Submit' });
        return false;
    }
}

function setCustomerCookiesRenault() {
    var email = getEmailValueRenault();
    if (email != undefined)
        document.cookie = '_CustEmail=' + email + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';

    document.cookie = '_CustomerName=' + $.trim($(popupData.PopupID + ' #custName').val()) + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
    document.cookie = '_CustMobile=' + $.trim($(popupData.PopupID + ' #custMobile').val()) + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
}

function validateOptionalEmailRenault() {
    var custEmail = $.trim($(popupData.PopupID + ' #custEmailOptional').val());
    var reEmail = /^[a-z0-9._%+-]+@[a-z-]{2,}\.[a-z]{2,}(\.[a-z]{1,}|$)$/;  // carwale mobile
    var retVal = true;
    var errorMsg = "";
    var _email = custEmail.toString().toLowerCase();
    if (!reEmail.test(_email)) {
        retVal = false;
        $(popupData.PopupID + ' .custEmailOptional .error-icon').show();
        $(popupData.PopupID + ' .custEmailOptional .cw-blackbg-tooltip').html('Please enter a valid email').show();
        dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: popupData.GACat, act: 'errEmailNotValid' });
    }
    else {
        hideCustomerFormErrorsRenault();
    }
    return retVal;
}

function hideCustomerFormErrorsRenault() {
    $(popupData.PopupID + ' .error-icon').html('').hide();
    $(popupData.PopupID + ' .cw-blackbg-tooltip ').html('').hide();
}

function isvalidRenault() {
    var assignedDealerId = $(popupData.PopupID + " #hdnDealerId").val();
    var custName = $.trim($(popupData.PopupID + ' #custName').val());
    var custMobile = $.trim($(popupData.PopupID + ' #custMobile').val());
    isValidCustdetails = false;
    var retVal = true;
    var errorMsg = "";
    var reName = /^([-a-zA-Z ']*)$/;
    var reEmail = /^[a-z0-9._%+-]+@[a-z-]{2,}\.[a-z]{2,}(\.[a-z]{1,}|$)$/;  // carwale mobile
    var reMobile = /^[6789]\d{9}$/;
    var _custMobile = custMobile;

    hideCustomerFormErrorsRenault();
    if (custName == "") {
        retVal = false;
        $(popupData.PopupID + ' .custName .error-icon').show();
        $(popupData.PopupID + ' .custName .cw-blackbg-tooltip').html('Please enter your name').show();
        dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: popupData.GACat, act: 'errNameNotFilled' });
    } else if (reName.test(custName) == false) {
        retVal = false;
        $(popupData.PopupID + ' .custName .error-icon').show();
        $(popupData.PopupID + ' .custName .cw-blackbg-tooltip').html('Please enter only alphabets').show();
        dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: popupData.GACat, act: 'errNonAlphabetName' });//chnage action acorrding to error
    }
    else if (custName.length == 1) {
        retVal = false;
        $(popupData.PopupID + ' .custName .error-icon').show();
        $(popupData.PopupID + ' .custName .cw-blackbg-tooltip').html('Please enter your complete name').show();
        dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: popupData.GACat, act: 'errOneCharName' });
    }
    if (popupData.ShowEmail == true && $.trim($(popupData.PopupID + ' #custEmail').val()).toString().toLowerCase() == "") {
        retVal = false;
        $(popupData.PopupID + ' .custEmail .error-icon').show();
        $(popupData.PopupID + ' .custEmail .cw-blackbg-tooltip').html('Please enter your email').show();
        dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: popupData.GACat, act: 'errEmailNotFilled' });
    }
    else if (popupData.ShowEmail == true && !reEmail.test($.trim($(popupData.PopupID + ' #custEmail').val()).toString().toLowerCase())) {
        retVal = false;
        $(popupData.PopupID + ' .custEmail .error-icon').show();
        $(popupData.PopupID + ' .custEmail .cw-blackbg-tooltip').html('Invalid Email').show();
        dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: popupData.GACat, act: 'errEmailNotValid' });
    }
    if (_custMobile == "") {
        retVal = false;
        $(popupData.PopupID + ' .custMobile .error-icon').show();
        $(popupData.PopupID + ' .custMobile .cw-blackbg-tooltip').html('Please enter your mobile number').show();
        dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: popupData.GACat, act: 'errMobileNotFilled' });
    }
    else if (_custMobile.length != 10) {
        retVal = false;
        $(popupData.PopupID + ' .custMobile .error-icon').show();
        $(popupData.PopupID + ' .custMobile .cw-blackbg-tooltip').html('Mobile number should be of 10 digits').show();
        dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: popupData.GACat, act: 'errMobileNot10Digits' });
    }
    else if (!reMobile.test(_custMobile)) {
        retVal = false;
        $(popupData.PopupID + ' .custMobile .error-icon').show();
        $(popupData.PopupID + ' .custMobile .cw-blackbg-tooltip').html('Please provide a valid 10 digit Mobile number').show();
        dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: popupData.GACat, act: 'errMobileNotValid' });
    }

    //if ((typeof assignedDealerId === "undefined" || Number(assignedDealerId) <= 0) && drpDealerCount > 1 && $(popupData.PopupID + ' #dealerList').is(":visible") && Number($(popupData.PopupID + ' #dealerList').val()) <= 0) {
    //    retVal = false;
    //    $(popupData.PopupID + ' div.dealerDropdownRenault  .error-icon').show();
    //    $(popupData.PopupID + ' div.dealerDropdownRenault  .cw-blackbg-tooltip').html('Please select your nearest dealership').show();
    //    dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: popupData.GACat, act: 'errDealerNotSelected' });
    //}

    if (Number($(popupData.PopupID + ' #drpCityDealerPopup').val()) <= 0 && $(popupData.PopupID + ' #drpCityDealerPopup').is(":visible")) {
        retVal = false;
        $(popupData.PopupID + ' div.cityDiv .error-icon').show();
        $(popupData.PopupID + ' div.cityDiv .cw-blackbg-tooltip').html('Please select your city').show();
        dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: popupData.GACat, act: 'errCityNotSelected' });
    }

    return retVal;
}

function getEmailValueRenault() {
    if ($(popupData.PopupID + ' .custEmail').is(":visible"))
        return $.trim($(popupData.PopupID + ' #custEmail').val());
    else if ($(popupData.PopupID + ' .custEmailOptional').is(":visible"))
        return $.trim($(popupData.PopupID + ' #custEmailOptional').val());
}

function saveDealerNameRenault(json) {
    drpDealerCount = json.length;

    if (drpDealerCount > 1) {
        $(popupData.PopupID + ' .dealerDropdownRenault').show();
    } else if (drpDealerCount == 1) {
        $(popupData.PopupID + " #hdnDealerId").val(json[0].id);
        $(popupData.PopupID + " #hdnDealerName").val(json[0].name.split(" - ")[1]);
        $(popupData.PopupID + ' .dealerDropdownRenault').hide();
    } else {
        $(popupData.PopupID + " #hdnDealerId").val("-1");
        $(popupData.PopupID + " #hdnDealerName").val("us");
        $(popupData.PopupID + ' .dealerDropdownRenault').hide();
    }
}

function getInquirySourceMobile() {
    if (window.isVersionPage && window.isVersionPage.toString().toLowerCase() == "true") {
        return 142;
    } else if (window.isModelCityPage && window.isModelCityPage.toString().toLowerCase() == "true") {
        return 385;
    } else {
        return 112;
    }
}

var leadCityId, leadCityName;

function bindCityAutocomplete(isDesktopDevice) {

    if (isDesktopDevice) {
        $("#personCity").cw_autocomplete({
            resultCount: 5,
            source: ac_Source.allCarCities,
            click: function (event, ui, orgTxt) {
                leadCityName = Common.utils.getSplitCityName(ui.item.label);
                leadCityId = ui.item.id;
                ui.item.value = leadCityName;
                setCookie(leadCityName, leadCityId);
            },
            afterfetch: function (result, searchtext) {
                this.result = result;
                if (typeof result == "undefined" || result.length <= 0)
                    showHideMatchError(true, $('#personCity'), "No city Match");
                else
                    showHideMatchError(false, $('#personCity'));
            }
        });
    }
    else {
        var personCityInputField = $("#personCity");

        $(personCityInputField).cw_easyAutocomplete({
            inputField: personCityInputField,
            resultCount: 5,
            source: ac_Source.allCarCities,
            click: function (event) {
                var selectionValue = personCityInputField.getSelectedItemData().value,
                selectionLabel = personCityInputField.getSelectedItemData().label;

                leadCityName = Common.utils.getSplitCityName(selectionLabel);
                leadCityId = selectionValue;
                $(personCityInputField).val(leadCityName);
                setCookie(leadCityName, leadCityId);
            },

            afterFetch: function (result, searchText) {
                if (typeof result == "undefined" || result.length <= 0) {
                    showHideMatchError(true, $(personCityInputField).closest('.easy-autocomplete'), "No city Match");
                }
                else {
                    showHideMatchError(false, $(personCityInputField).closest('.easy-autocomplete'));
                }
            }
        });
    }
}

function showHideMatchError(error, TargetId, errText) {
    if (error) {
        TargetId.siblings('.error-icon').removeClass('hide');
        TargetId.siblings('.cw-blackbg-tooltip').removeClass('hide').text(errText);
        TargetId.addClass('border-red');
    }
    else {
        TargetId.siblings().addClass('hide');
        TargetId.removeClass('border-red');
    }
}