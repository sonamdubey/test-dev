var selectedModelId = "";
var selectedModelName = "";
var selectedCityId = "";
var selectedZoneId = "";

var Dealers = {
    utils: {
        showCallDealer: function () {
            $("#contact-fixed").show();
            $(".extraDivHt").show();
        },
        hideCallDealer: function () {
            $("#contact-fixed").hide();
            $(".extraDivHt").hide();
        }
    }
}

function modelChanged(modelLI) {
    selectedModelId = $(modelLI).val();
    selectedModelName = $(modelLI).text();
    $('#tab1').text(selectedModelName);
    $("#cw-m-dlp-accordian li div").removeClass("show-div");
    $(this).removeClass("cw-m-dlp-ac-inactive");
    $("#slide1,#slide2").addClass("hide-div");
    $('#slide2').removeClass('hide-div');
    $('.select-car-icon').addClass('select-car-icon-selected');
    $('#tab1').addClass('blue-text-dark');
    alterBuyingAssitTab();
}

function alterBuyingAssitTab() {
    var buyingAssistTab = $('#divSelectCar');
    buyingAssistTab.find('.filled-tick-blue').show();
    buyingAssistTab.find('.arrow-icon').hide();
    buyingAssistTab.find('.dlp-error-icon').hide();
    buyingAssistTab.find('.cw-m-dlp-ac-info select-car').addClass('hide');
    if (!(buyingAssistTab.find('.cw-m-blackbg-tooltip').hasClass('hide')))
        buyingAssistTab.find('.cw-m-blackbg-tooltip').addClass('hide');
    closeSelectCarSection();
}
function showLoadingForDiv(currentPopup, prevPopup) {
    prevPopup.hide();
    currentPopup.find("div.popup_content").hide();
    currentPopup.find("div.m-loading-popup").show();
    currentPopup.addClass("popup_layer").show().scrollTop(0);
    window.scrollTo(0, 0);
}

function hideLoadingForDiv(currentPopup) {
    currentPopup.find("div.popup_content").show();
    currentPopup.find("div.m-loading-popup").hide();
}

$("#custMobile").on('blur', function () {
    var mobile = $("#custMobile").val();
    checkCustMobile(mobile);
});

$("#custName").on('blur', function () {
    var name = $("#custName").val();
    checkCustName(name);
});

$("#custEmail").on('blur', function () {
    var email = $("#custEmail").val();
    checkCustEmail(email);
});

function openSelectCarSection() {
    $('#liBuyingAssistance').find('.select-car').removeClass('hide');
    closeSelectServicesSection();
}

function closeSelectCarSection() {
    $('#liBuyingAssistance').find('.select-car').addClass('hide');
}

function closeSelectServicesSection() {
    $('#liServiceDetails').find('.info-check-box').hide();
    $('#liServiceDetails').find('.fa-plus').show();
    $('#liServiceDetails').find('.fa-minus').hide();
}

function openSelectServicesSection() {
    $('#liServiceDetails').find('.info-check-box').show();
    $('#liServiceDetails').find('.fa-plus').hide();
    $('#liServiceDetails').find('.fa-minus').show();
    closeSelectCarSection();
}

//To check all the sections after submit
function checkAllSections() {
    var validSection = true;
    var contactDetailError = isCustValidated();
    var validModel = checkModelName();
    if (!validModel)
        validSection = false;
    if (!contactDetailError)
        validSection = false;
    //Open the section which was in open state
    if (!validModel)
        openSelectCarSection();
    return validSection;
}

function checkCustName(custName) {
    var re = /^([-a-zA-Z ']*)$/;
    var name = $.trim(custName);
    var custNameDiv = $('#divCustName');
    var nameMsg;

    if (name == "" || name == "Enter your name" || name == "Enter Your Name") {
        nameMsg = "Please enter your name";
        custNameDiv.find('.dlp-error-icon').show();
        custNameDiv.find('.cw-m-blackbg-tooltip').text(nameMsg).removeClass('hide');
        return false;

    } else if (name.length == 1) {
        nameMsg = "Please enter your complete name";
        custNameDiv.find('.dlp-error-icon').show();
        custNameDiv.find('.cw-m-blackbg-tooltip').text(nameMsg).removeClass('hide');
        return false;
    } else if (re.test(name) == false) {
        nameMsg = "Please enter only alphabets";
        custNameDiv.find('.dlp-error-icon').show();
        custNameDiv.find('.cw-m-blackbg-tooltip').text(nameMsg).removeClass('hide');
        return false;
    } else {
        custNameDiv.find('.dlp-error-icon').hide();
        custNameDiv.find('.cw-m-blackbg-tooltip').addClass('hide');
    }
    return true;
}

function checkCustEmail(custEmail) {
    var re = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$/;
    var name = $.trim(custEmail);
    var custEmailDiv = $('#divCustEmail');
    var nameMsg;
    if (!(name == "")) {
        if (!re.test(name.toLowerCase())) {
            nameMsg = "Invalid Email Id";
            custEmailDiv.find('.cw-m-blackbg-tooltip').text(nameMsg).removeClass('hide');
            custEmailDiv.find('.dlp-error-icon').show();
            return false;
        } else {
            nameMsg = "";
            return true;
        } if (name == "" || name == "Enter your e-mail id") {
            nameMsg = "";
            custEmailDiv.find('.cw-m-blackbg-tooltip').text(nameMsg).removeClass('hide');
            custEmailDiv.find('.dlp-error-icon').show();
            return true;
        }
    }
    return true;
}

function checkCustMobile(custMobile) {

    var re = /^[6789]\d{9}$/;
    var name = $.trim(custMobile);
    var custMobileDiv = $('#divCustMob');
    var nameMsg;

    if (name == "" || name == "Enter your mobile number") {
        nameMsg = "Please enter your mobile number";
        custMobileDiv.find('.dlp-error-icon').show();
        custMobileDiv.find('.cw-m-blackbg-tooltip').text(nameMsg).removeClass('hide');
        return false;
    } else if (name.length != 10) {
        nameMsg = "Enter your 10 digit mobile number";
        custMobileDiv.find('.dlp-error-icon').show();
        custMobileDiv.find('.cw-m-blackbg-tooltip').text(nameMsg).removeClass('hide');
        return false;
    } else if (re.test(name) == false) {
        nameMsg = "Please provide a valid 10 digit Mobile number";
        custMobileDiv.find('.dlp-error-icon').show();
        custMobileDiv.find('.cw-m-blackbg-tooltip').text(nameMsg).removeClass('hide');
        return false;
    } else {
        custMobileDiv.find('.dlp-error-icon').hide();
        custMobileDiv.find('.cw-m-blackbg-tooltip').addClass('hide');
    }
    return true;
}

function checkModelName() {
    var div = $('#liBuyingAssistance');
    if (selectedModelName == "") {
        div.find('.cw-m-blackbg-tooltip').removeClass('hide');
        div.find('.dlp-error-icon').show();
        div.find('.cw-m-dlp-ac-info select-car').removeClass('hide');
        return false;
    } else return true;
}

function isCustValidated() {
    var reName = /^([-a-zA-Z ']*)$/;
    var reMobile = /^[6789]\d{9}$/;
    var retVal = true;
    var name = $.trim($("#custName").val());
    var mobile = $.trim($("#custMobile").val());
    var email = $.trim($("#custEmail").val());

    if (!checkCustName(name))
        retVal = false;

    if (!checkCustMobile(mobile))
        retVal = false;

    if ($('#custEmail').is(':visible')) {
        if (!checkCustEmail(email))
            retVal = false;
    }
    return retVal;
}

function bindModels(makeId, dealerId) {
    $.ajax({
        type: 'GET',
        url: '/api/NewCarDealers/GetDealerModels/?&makeId=' + makeId + (dealerId == undefined ? '' : ('&dealerId=' + dealerId)),
        dataType: 'Json',
        success: function (json) {
            json = $.grep(json, function (v) {
                return v.makeId === makeId;
            });

            if (json.length > 0) {
                json = json[0].modelDetails;
            }

            var viewModel = {
                pqCarModels: ko.observableArray(json)
            };
            ko.cleanNode(document.getElementById("model"));
            ko.applyBindings(viewModel, document.getElementById("model"));
        }
    });
}

function ContactFixedScroll() {
    var footerTop, contactFixed = $('#contact-fixed'), adDiv = $(document).find('.ad-div');
    var extraDiv = $(document).find('.extraDivHt'), extraDivHt = contactFixed.outerHeight();
    footerTop = $('footer').offset().top - $(window).height() + 60;
    extraDiv.height(extraDivHt);

    if ($("#buying_assistance").hasClass("nav_tab active")) {
        Dealers.utils.hideCallDealer();
    }
    else {
        if ($(this).scrollTop() > footerTop) {
            contactFixed.removeClass('contact-fixed');
            $('.extraDivHt').hide();
        }
        else {
            contactFixed.addClass('contact-fixed');
            $('.extraDivHt').show();
        }
    }
}

$(window).load(function () {
    ContactFixedScroll();
    $(window).scroll(function () {
        ContactFixedScroll();
    });
});
