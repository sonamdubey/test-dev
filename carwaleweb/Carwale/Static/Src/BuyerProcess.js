
//(function ($) {
var reEmail = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$/;
var re = /^[0-9]*$/;
var hashParams = "", selectedModels = "";
var multiple_select = false;
var box_obj, boxObjCaptcha;
var profileId_g = "", inquiryId_g = "", isDealer_g = "", rank_g = "", rank_abs = "", isPremium_g = "", kmNumeric_g = "", priceNumeric_g = "", bodyTypeId_g = "", versionSubSegment_g = "", stockRecommendationsUrl_g = "";
var buyersName = "", buyersEmail = "", buyersMobile = "", isDetailsViewed = "0", carModel_g = "", makeYear_g = "", makeName_g = "", price_g = "", cityName_g = "", individual_g = "", dealer_g = "", deliveryCity;
var transToken = "";
var clientId = "";
var time = null;
var requestCount = 1;
var status = 0;
var ltsrc = "";
var cwc = "";
var contactSeller = "";
$.isDisabled = false;
$.isEnabled;
var cityId = '';
var cityName = '';
var UTMA = '';
var originId = 0;
var UTMZ = '';
var RECOMMENDLISTING = ko.observableArray([]);
var isFromCaptcha = "0", isGSDClick = "0";
var ISRECOMMENDED = false, isLimitExceeded_g = false, isForbidden_g = false;
var ACCESSFORBNIDENMSG = 'Access Forbidden.';
var LIMITEXCEEDMSG = 'Oops! You have reached the maximum limit for viewing inquiry details. Please try after a few days.';
var TRYLATERMSG = 'Some problem in retrieving the data. Please try later.';
var boxObj = null;
var ISSELLERDETAILVIEWED = false;

var buyerProcessOriginId = {
    RightPrice: 37,
    RightPriceRecommendations: 39
}

var searchPageVariables = (function () {
    var getChatVerifiedText = function () {
        return "Chat_Verified_Click";
    }
    var getChatUnVerifiedText = function () {
        return "Chat_Unverified_Click";
    }
    var getBhriguCategory = function () {
        return "UsedSearchPage";
    }
    return {
        getChatVerifiedText: getChatVerifiedText,
        getChatUnVerifiedText: getChatUnVerifiedText,
        getBhriguCategory: getBhriguCategory
    }
})();

$(document).ready(function () {
    D_buyerProcess.pageLoad.sellerDetailsLoad();
    D_buyerProcess.closeVerificationPopup.registerEvents();
    D_buyerProcess.pageLoad.sellerDetailsPopupLoad();
    D_buyerProcess.pageLoad.cityWarningLoad();

    chatProcess.getChatHtml(function (isMyChatsVisible, chatHtml) {
        $("#chatPopup").html(chatHtml);
        if (isMyChatsVisible) {
            $('.global-chat-icon').show();
        }
    }, chatUIProcess.setChatIconVisibilty, chatProcess.source.desktopBrowser, chatUIProcess.loader.hide);

    $(document.body).on('click', "#pg_emailTick", function (e) {
        if ($("#pg_emailTick").is(':checked')) {
            $("#pg_email_field").show();
            $("#pg_txtEmail").focus();
        }
        else {
            $("#pg_email_field").hide();
            $("#pg_txtName").focus();
        }
    });
    ko.applyBindings(RECOMMENDLISTING, document.getElementById("recommendations"));
    ko.applyBindings(RECOMMENDLISTING, document.getElementById("similarCarsList"));
    D_buyerProcess.pageLoad.recommendedCarsLoad();

    $(document.body).on('click', ".btnVerifyCaptcha, .pg-btnVerifyCaptcha", function (e) {
        var captchaCode, thisElement = $(this), thisId = thisElement.attr('id');
        if (thisId == "btnVerifyCaptcha")
            captchaCode = thisElement.closest('#captcha').find('#txtCaptchaCode').val();
        else
            captchaCode = thisElement.closest('#pg-captcha').find('#pg-txtCaptchaCode').val();

        if (captchaCode.length == 0) {
            if (thisId == "btnVerifyCaptcha") {
                ShakeFormView($(".captcha-inputbox"));
                thisElement.closest('#captcha').find('#lblCaptcha').html("Code Required").show();
                thisElement.closest('#captcha').find('#lblCaptcha').next().show();
            }
            else {
                ShakeFormView($(".captcha-inputbox"));
                thisElement.closest('#pg-captcha').find('#pg-lblCaptcha').html("Code Required").show();
                thisElement.closest('#pg-captcha').find('#pg-lblCaptcha').next().show();
            }
            return false;
        }
        else {
            VerifyCaptcha(captchaCode, thisElement);
        }
    });
});

function closeRecommendedPopup() {
    $('#recommendCars').hide();
    $('.recommedCars-left').css({ 'float': 'none', 'left': '324px' })
    $('div.suggestCarsTxt').hide();
    $('#blackOut-recommendation').hide();
    if (ISSELLERDETAILVIEWED || $.cookie('TempCurrentUser'))
        D_buyerProcess.utils.changeBtnTextToViewSeller();
    else
        D_buyerProcess.utils.changeBtnTextToGetSeller(contactSeller);
}
function closeSimilarCarPopup() {
    $('#similarCarPopup').hide();
    $('div.blackOut-window-bt').hide();
}

function setBuyersInfoFromCookie() {
    if (!buyersName || !buyersMobile || !buyersEmail) {
        var tempCurrUser = $.cookie('TempCurrentUser');
        var buyerInfo;
        if (tempCurrUser) {
            buyerInfo = tempCurrUser.split(':');
            buyersName = buyerInfo[0];
            buyersMobile = buyerInfo[1];
            buyersEmail = buyerInfo[2];
        }
    }
}

$(document).on('click', '.popup-close, #popup-close-icon', function () {
    $("#txtCwiCode").val("");
    closeRecommendedPopup();
    setBuyersInfoFromCookie();
    if (isForbidden_g || isLimitExceeded_g) {
        $('#getName').val(buyersName);
        $('#txtMobile').val(buyersMobile);
        D_buyerProcess.sellerDetails.openPopup();
    }
});

$(document).on('click', '.similarcar-close', function () {
    closeSimilarCarPopup();
});

function VerifyCaptcha(captchaCode, thisElement) {
    $.ajax({
        type: "POST",
        url: "/ajaxpro/CarwaleAjax.AjaxClassifiedBuyer,Carwale.ashx",
        data: '{"captchaInput":"' + captchaCode + '", "captchaCookie":"' + $.getCookie("CaptchaImageText") + '"}',
        dataType: 'json',
        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "VerifyCaptcha") },
        success: function (response) {
            thisId = thisElement.attr('id');
            var ds = eval('(' + response.value + ')');
            if (ds.captchaStatus == "1") {
                $('.captcha').hide();
                isFromCaptcha = "1";
                D_buyerProcess.sellerDetails.processPurchaseInquiries(boxObjCaptcha);
            }
            else {
                if (thisId == "btnVerifyCaptcha") {
                    ShakeFormView($(".captcha-inputbox"));
                    thisElement.closest('#captcha').find('#lblCaptcha').html("Invalid Code").show();
                    thisElement.closest('#captcha').find('#lblCaptcha').next().show();
                }
                else {
                    ShakeFormView($(".captcha-inputbox"));
                    thisElement.closest('#pg-captcha').find('#pg-lblCaptcha').html("Invalid Code").show();
                    thisElement.closest('#pg-captcha').find('#pg-lblCaptcha').next().show();
                }
                return false;
            }
        }
    });
}

function showSellerDetailsForRecommendations(boxObj, seller) {
    if (seller.mobile != undefined) {
        var similarCarId = D_buyerProcess.recommendedCars.getRankFromRecommendationsId(boxObj.attr('id'));
        $('#loadingIconRecommendations-' + similarCarId).fadeOut('slow', function () {
            $('#sellerdetailsData-' + similarCarId).show();
        });
        $("#seller-Name" + similarCarId).text(seller.name);
        if (isDealer_g == "1")
            $("#seller-Person" + similarCarId).text(seller.contactPerson ? seller.contactPerson : "");
        $("#seller-Email" + similarCarId).text(seller.email);
        $("#seller-Contact" + similarCarId).text(seller.mobile);
        $("#seller-Address" + similarCarId).text(seller.address);
    }
    else {
        showErrorsForRecommendations(boxObj, TRYLATERMSG);
    }
}

function triggerRemarketingCode() {
    //google double click
    var dealerText = remarketingVariables.sellerType == "Dealer" ? "Dealer" : "";
    var individualText = remarketingVariables.sellerType == "Individual" ? "Individual" : "";
    var axel = Math.random() + "";
    var a = axel * 10000000000000000;
    var spotpix = new Image();
    var idx = remarketingVariables.carModel.indexOf('[');
    var model = idx == -1 ? remarketingVariables.carModel : remarketingVariables.carModel.substring(0, idx - 1);
    if (remarketingVariables.sellerDetailsClick == "1")
        spotpix.src = "https://ad.doubleclick.net/ddm/activity/src=4948701;type=usedc0;cat=deskt00;u1=" + remarketingVariables.carMake.replace(/ /g, '') + ";u2=" + model.replace(/ /g, '') + ";u3=" + remarketingVariables.price.replace(/ /g, '') + ";u4=" + remarketingVariables.cityName.replace(/ /g, '') + ";u5=" + dealerText + ";u6=" + individualText + ";ord=" + a + "?";
    else if (remarketingVariables.sellerDetailsClick == "2")
        spotpix.src = "https://ad.doubleclick.net/ddm/activity/src=4948701;type=usedc0;cat=photo0;u1=" + remarketingVariables.carMake.replace(/ /g, '') + ";u2=" + model.replace(/ /g, '') + ";u3=" + remarketingVariables.price.replace(/ /g, '') + ";u4=" + remarketingVariables.cityName.replace(/ /g, '') + ";u5=" + dealerText + ";u6=" + individualText + ";ord=" + a + "?";
}

function initGetSellerDetails() {
    $(".btooltip").bt({
        contentSelector: "$(this).next().html()",
        fill: '#FFFFFF',
        strokeWidth: 1,
        strokeStyle: '#cacaca',
        trigger: ['hover', 'none'],
        width: 'auto',
        spikeLength: 5,
        shadow: true,
        cornerRadius: 0,
        positions: ['top'],
        padding: 5,
        cssStyles: { fontSize: '12px' },
        preShow: function (box) {
            $("div.bt-wrapper").hide();
        }
    });


}

if (typeof String.prototype.trim !== 'function') {
    String.prototype.trim = function () {
        return this.replace(/^\s+|\s+$/g, '');
    }
}

function hideBuyerForm(boxObj) {
    if (boxObj != undefined && boxObj.attr('id') == 'pg_seller_details') {
        boxObj.find('pg_contactSellerForm').hide();
    }
    else {
        boxObj.find("#buyer_form").hide();
    }
}

function ValidateSellerForm(box_obj, isValuation) {
    if (isValuation) {
        var mobileError = $("#rp_mobileError");
        var emailError = $("#rp_emailError");
        var formMobile = $(".form-field__mobile");
        var formEmail = $(".form-field__email");
        mobileError.hide();
        emailError.hide();

        var isNameValid = validateBuyerName($('#rpName'));
        if (!isNameValid) {
            return isNameValid;
        }

        if (!buyersMobile) {
            ShakeFormView(formMobile);
            mobileError.text("Please enter your mobile number").show();
            mobileError.next().show();
            return false;
        } else if (buyersMobile != "" && re.test(buyersMobile) == false) {
            ShakeFormView(formMobile);
            mobileError.text("Please enter a valid mobile number").show();
            mobileError.next().show();
            return false;
        } else if (buyersMobile != "" && (!re.test(buyersMobile) || buyersMobile.length < 10 || buyersMobile.length > 10)) {
            ShakeFormView(formMobile);
            mobileError.text("Please enter a 10-digit valid mobile number").show();
            mobileError.next().show();
            return false;
        } else {
            mobileError.next().hide();
            mobileError.hide();
        }

        if ($("#emailTick").hasClass('uc-checked')) {
            if (!buyersEmail) {
                ShakeFormView(formEmail);
                emailError.text("Please enter your Email").show();
                emailError.next().show();
                return false;
            } else if (!reEmail.test(buyersEmail.toLowerCase())) {
                ShakeFormView(formEmail);
                emailError.text("Please enter a valid Email").show();
                emailError.next().show();
                return false;
            } else {
                mobileError.next().hide();
                mobileError.hide();
            }
        }
    } else {
        box_obj.find("#pg_seller_details .error").hide();
        box_obj.find("#pg_txtName, #pg_txtMobile , #pg_emailTick, #pg_txtValidMobileError ,#pg_txtEmail,#pg_txtValidEmailError").removeClass("red-border");
        box_obj.find("#pg_txtNameError,#pg_txtMobileError,#pg_txtEmailError,#pg_txtValidEmailError,#pg_txtValidMobileError").hide();

        var isNameValid = validateBuyerName($('#pg_txtName'));
        if (!isNameValid) {
            return isNameValid;
        }

        if (buyersMobile == "") {
            ShakeFormView($(".uc-pg-fields.mobile-ug-field"));
            box_obj.find("#pg_txtMobileError").html("Please enter your mobile number");
            box_obj.find("#pg_txtMobileError").show();
            box_obj.find("#pg_txtMobileError").next().show();
            return false;
        } else if (buyersMobile != "" && re.test(buyersMobile) == false) {
            ShakeFormView($(".uc-pg-fields.mobile-ug-field"));
            box_obj.find("#pg_txtMobileError").html("Please enter a valid mobile number");
            box_obj.find("#pg_txtMobileError").show();
            box_obj.find("#pg_txtMobileError").next().show();
            return false;
        } else if (buyersMobile != "" && (!re.test(buyersMobile) || buyersMobile.length < 10 || buyersMobile.length > 10)) {
            ShakeFormView($(".uc-pg-fields.mobile-ug-field"));
            box_obj.find("#pg_txtMobileError").html("Please enter a 10-digit valid mobile number");
            box_obj.find("#pg_txtMobileError").show();
            box_obj.find("#pg_txtMobileError").next().show();
            return false;
        } else {
            box_obj.find("#pg_txtMobileError").hide();
            box_obj.find("#pg_txtMobileError").next().hide();
        }

        if (buyersEmail == "") {

            if (box_obj.find("#pg_emailTick").is(':checked')) {
                ShakeFormView($("#pg_email_field"));
                box_obj.find("#pg_txtEmailError").html("Please enter your Email");
                box_obj.find("#pg_txtEmailError").show();
                box_obj.find("#pg_txtEmailError").next().show();
                return false;
            }
        } else if (!reEmail.test(buyersEmail.toLowerCase())) {
            ShakeFormView($("#pg_email_field"));
            box_obj.find("#pg_txtEmailError").html("Please enter a valid Email");
            box_obj.find("#pg_txtEmailError").show();
            box_obj.find("#pg_txtEmailError").next().show();
            return false;
        } else {
            box_obj.find("#pg_txtEmailError").hide();
            box_obj.find("#pg_txtEmailError").next().hide();
        }
    }
    return true;
}

function validateBuyerName(field) {
    var isValid = false,
		message = '';

    var fieldVal = field.val(),
		reTest = /^[a-zA-Z_'. ]{3,50}$/,
		fieldLength = fieldVal.length,
		fieldContainer = field.closest(".form-control-box");

    if (!fieldLength) {
        message = "Please enter your name."
    }
    else if (fieldLength < 3) {
        message = "Name should be atleast 3 characters."
    }
    else if (!reTest.test(fieldVal)) {
        message = "Invalid name."
    }
    else {
        isValid = true;
    }

    if (!isValid) {
        ShakeFormView(fieldContainer);
        field.siblings(".cw-blackbg-tooltip").text(message).show();
        field.siblings(".error-icon").show();
    }

    return isValid;
}

function setBuyerAlerts(objSetAlert) {
    var imgAjaxProcess = $(objSetAlert).next();
    imgAjaxProcess.removeClass("hide").show();
    var alertEmail = $('#alertEmail').val().toLowerCase();
    var city = "";
    var budget = "0-";
    var carAge = "0-";
    var kms = "0-";
    var bodyStyle = "";
    var filterBy = "";
    var car = "";
    var fuel = "";
    var transmission = "";
    var seller = "";
    var owners = "";
    var selectType = $("#selAlertFrq").val();
    var urlCriteria = "";

    var params = D_usedSearch.utils.getAllParamsWithValuesFromQS();

    city = params["city"] == undefined ? "" : params["city"];
    urlCriteria = window.location.hash.replace('#', '');
    filterBy = params["filterbyadditional"] == undefined ? "" : params["filterbyadditional"];
    owners = params["owners"] == undefined ? "" : params["owners"];
    carAge = params["year"] == undefined ? "-" : params["year"];
    kms = params["kms"] == undefined ? "-" : params["kms"];
    bodyStyle = params["bodytype"] == undefined ? "" : params["bodytype"];
    budget = params["budget"] == undefined ? "-" : params["budget"];
    fuel = params["fuel"] == undefined ? "" : params["fuel"];;
    transmission = params["trans"] == undefined ? "" : params["trans"];
    seller = params["seller"] == undefined ? "" : params["seller"];
    car = params["car"] == undefined ? "" : params["car"];



    if (validEmail(alertEmail)) {
        $.ajax({
            type: "POST",
            url: "/ajaxpro/CarwaleAjax.AjaxClassifiedBuyer,Carwale.ashx",
            data: '{"email":"' + alertEmail + '","alertFrq":"' + selectType + '","url":"' + urlCriteria + '","city":"' + city + '","budget":"' + budget +
                    '","year":"' + carAge + '","kms":"' + kms + '","car":"' + car + '","bodyStyle":"' + bodyStyle +
                    '","fuel":"' + fuel + '","transmission":"' + transmission + '","seller":"' + seller +
                    '","filterBy":"' + filterBy + '","owner":"' + owners + '"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "SetBuyerAlerts"); },
            success: function (response) {

                var responseObj = eval('(' + response + ')');

                if (responseObj.value == true) {
                    setTimeout(function () { alertResponse(imgAjaxProcess); }, 100);
                    $("#alert_content").hide();
                    $("#alert_status").html("<b>Alert has been successfully set.</b>");
                    $("#alert_status").show();
                } else {
                    imgAjaxProcess.hide();
                    $("#alert_content").hide();
                    $("#alert_status").html("<b>Error occured while setting alert.</b>");
                    $("#alert_status").show();
                }
            }
        });
    } else {
        imgAjaxProcess.hide();
        alert("Please enter valid email to set alert");
    }
}

function alertResponse(imgAjaxProcess) {
    imgAjaxProcess.hide();
}

function validEmail(email) {
    if (reEmail.test(email)) {
        return true;
    } else {
        return false;
    }
}

function formViewSimilarCar(boxObj) {
    var cityId, makeId, rootId;
    if (boxObj.attr('id') == 'pg_seller_details') {
        var profileId = $("#pg_seller_details").attr('profileId');
        var list = $('.contact-seller[profileid="' + profileId + '"]');
        cityId = list.attr('cityid') == 1 ? 3000 : list.attr('cityid');
        makeId = list.attr('makeid');
        rootId = list.attr('rootid');
    } else if (boxObj.attr('id') == "rpgetsellerDetails") {
        cityId = boxObj.attr('cityid');
        makeId = boxObj.attr('makeid');
        rootId = boxObj.attr('rootid');
    }
    else {
        cityId = remarketingVariables.cityId == 1 ? 3000 : remarketingVariables.cityId;
        makeId = remarketingVariables.makeId;
        rootId = remarketingVariables.rootId;
    }
    var URL = "/used/cars-for-sale/?city=" + cityId + "&car=" + makeId + "." + rootId;
    $('.bp-SimilarCars').attr('href', URL);
    $('.pg-similarCars').attr('href', URL);
}



function toggleViewDetailsBtn(boxObj) {
    boxObj.closest('li').find('.othersellerDetails').slideToggle(500);
    changeGSDBtnText(boxObj);
    boxObj.toggleClass('expand add-grey').find('span.fa-angle-down').toggleClass('transform');
}

function changeGSDBtnText(recommendedGsdBtn) {
    if (recommendedGsdBtn.find(".oneClickDetails").hasClass("hideImportant")) {
        recommendedGsdBtn.find(".oneClickDetails").removeClass("hideImportant");
        recommendedGsdBtn.find(".hideSellerDetails").addClass("hideImportant");
    }
    else {
        recommendedGsdBtn.find(".oneClickDetails").addClass("hideImportant");
        recommendedGsdBtn.find(".hideSellerDetails").removeClass("hideImportant");
    }
};

function showErrorsForRecommendations(boxObj, message) {
    $("#suggestDetailsData-" + D_buyerProcess.recommendedCars.getRankFromRecommendationsId(boxObj.attr('id')))
        .html('<li>' + message +'</li>');
}

var D_buyerProcess = {
    doc: $(document),
    buyerProcessApis: {
        evaluateResponse: function (response) {
            return eval('(' + response.value + ')');
        },
        processUsedCarPurchaseInquiries: function (boxObj, leadData) {
            return $.ajax({
                type: "POST",
                url: "/api/v1/stockleads/",
                headers: { 'sourceid': 1 },
                data: JSON.stringify(leadData),
                contentType: 'application/json',
                dataType: 'json'
            });
        }
    },
    leadType: {
        getLeadType: function() {
            return chatProcess.isChatLead ? 1 : 0;
        }
    },
    sellerDetails: {
        boxObj: $('#recommendCars'),
        popupContainer: $('#sellerDetailsPopup'),
        form: $('#sellerDetailsForm'),
        otpForm: $('#otpForm'),
        clearTimerTimeout: '',
        gsdButton: '',
        chatBtn: '',
        recommendationBlackOut: $('#blackOut-recommendation'),
        registerEvents: function () {
            D_buyerProcess.doc.on('click', '.contact-seller', function () {
                chatProcess.isChatLead = false;
                leadData.setCurrGsdButton(this);
                D_buyerProcess.sellerDetails.gsdButton = $(this);
                D_buyerProcess.sellerDetails.openGetSellerForm();
                tracker.trackGsdClick(originId);
            });
            D_buyerProcess.doc.on('click', '.contact-seller-chat', function () {
                chatProcess.isChatLead = true;
                D_buyerProcess.sellerDetails.chatBtn = $(this);
                D_buyerProcess.sellerDetails.gsdButton = $(this).parent().parent().find('.contact-seller');
                D_buyerProcess.sellerDetails.openChat();
            });
            D_buyerProcess.sellerDetails.boxObj.on('click', '.emailTick', function () {
                D_buyerProcess.sellerDetails.checkShareEmail();
            });
            D_buyerProcess.sellerDetails.boxObj.on('click', '#closeBox', function () {
                D_buyerProcess.sellerDetails.closeSellerForm();//verify html
            });
            D_buyerProcess.doc.on('click', '#pg_get_details', function () {
                chatProcess.isChatLead = false;
                leadData.setCurrGsdButton(this);
                D_buyerProcess.sellerDetails.getPgSellerDetails();
                tracker.trackGsdClick(originId);
            });
            D_buyerProcess.doc.on('click', '#pg_chat_btn_container', function () {
                chatProcess.isChatLead = true;
                chatUIProcess.loader.show($(this).find('.chat-btn'));
                D_buyerProcess.sellerDetails.getPgSellerDetails();
            });
            $(".back-to-gsd-form").on("click", function () {
                var parent = $(this).parent();
                if (parent.attr("id") == "pg-not_auth") {
                    $("#pg_seller_details").show();
                    $("#pg_contactSellerForm").show();

                }
                parent.hide();
            });
            // start of Verification click event
            D_buyerProcess.doc.on('click', ".missed-call__verify-link", function (e) {
                $("#missed-call__loading").show();
                D_buyerProcess.sellerDetails.hitIsMobileVerifiedApi(buyersMobile, D_buyerProcess.sellerDetails.missedCallClickVeriHandler);
            });
            // Ends
            D_buyerProcess.doc.on('keyup', '#getOTP', function () {
                var otp = this.value;
                $(this).val(otp.replace(/[^\d]/, ''));
                if (otp.length == 5) {
                    D_buyerProcess.sellerDetails.hitVerifyOtpApi(otp);
                }
            });
            D_buyerProcess.doc.on('click', '#submitSellerDetailsForm', function () {
                D_buyerProcess.sellerDetails.processLead();
            });
            D_buyerProcess.doc.on('click', '#sellerDetailsClose', function () {
                D_buyerProcess.sellerDetails.closePopup();
                D_buyerProcess.utils.changeBtnTextToGetSeller(contactSeller);
                D_buyerProcess.sellerDetails.form.find('.cw-blackbg-tooltip, .error-icon').hide();
            });
            D_buyerProcess.doc.on('click', '#otpClose', function () {
                D_buyerProcess.sellerDetails.otpForm.hide();
                $('.otp-modal-bg').hide();
                $('#blackOut-recommendation').hide();
                D_buyerProcess.utils.changeBtnTextToGetSeller(contactSeller);
                setBuyersInfoFromCookie();
                isLimitExceeded_g = false;
                isForbidden_g = false;
                if ($('.valuation-popup').is(':visible')) {
                    D_usedSearch.Valuation.sellerDetails.gettingDetailsBtn.addClass('hide');
                    D_usedSearch.Valuation.sellerDetails.sellerDetailsBtn.show();
                }
            });
            D_buyerProcess.doc.on('click', '#verifyOTP', function () {
                var enteredOTP = $("#getOTP").val();
                if (enteredOTP.length == 0) {
                    D_buyerProcess.sellerDetails.otpForm.find('.otp__error').text('Please enter OTP');
                }
                else if (enteredOTP.length < 5) {
                    D_buyerProcess.sellerDetails.otpForm.find('.otp__error').text('Invalid OTP!');
                }
                else {
                    D_buyerProcess.sellerDetails.hitVerifyOtpApi(enteredOTP);
                }
            });
            D_buyerProcess.doc.on('click', '#otpTimer', function () {
                $.ajax({
                    type: "POST",
                    url: "/api/v1/resendotp/",
                    headers: { 'sourceid': 1 },
                    data: JSON.stringify(D_buyerProcess.sellerDetails.getMobileVerificationApiData(buyersMobile)),
                    contentType: 'application/json',
                    dataType: 'json',
                    success: function (response) {
                        $('#otpTimer').addClass('otp-status--done').html('OTP sent to Mobile');
                    },
                    error: function (xhr) {
                        D_buyerProcess.sellerDetails.otpForm.find('.otp__error').text('Something went wrong!');
                    }
                });
            });
        },

        hitVerifyOtpApi: function (otp) {
            $.ajax({
                type: 'GET',
                url: '/api/v1/mobile/' + buyersMobile + '/verification/verifyotp/?otpCode=' + otp + '&sourceModule=' + D_buyerProcess.utils.otpVariables.sourceModule,
                headers: { 'CWK': 'KYpLANI09l53DuSN7UVQ304Xnks=', 'SourceId': '1' },
                dataType: 'Json',
                success: function (json) {
                    if (json.responseCode == 1) {
                        D_buyerProcess.sellerDetails.handleVerificationSuccessResponse();
                    }
                    else {
                        D_buyerProcess.sellerDetails.otpForm.find('.otp__error').text('Invalid OTP!');
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    D_buyerProcess.sellerDetails.otpForm.find('.otp__error').text('something went wrong, please try again later!');
                }
            });
        },

        resetTimer: function (container, timeoutElement) {
            clearTimeout(timeoutElement);
            container.removeClass('counter--active counter-done otp-status--done');
            container.html('Resend OTP in <span class="time-counter">30</span>s');
        },

        setTimer: function (container, successMessage, count) {
            container.addClass('counter--active');
            count -= 1;

            var clearCounter = setInterval(function () {
                if (!count) {
                    container.removeClass('counter--active').text(successMessage);
                    clearTimeout(clearCounter);
                }
                else {
                    container.find('.time-counter').text(count);
                    count -= 1;
                }
            }, 1000);

            return clearCounter;
        },

        getPgSellerDetails: function () {
            var gallerySellerDetails = $('#pg_seller_details');
            $('#pg-txtCwiCode').val('');
            originId = 2;
            isGSDClick = "1";
            ISRECOMMENDED = false;
            D_buyerProcess.sellerDetails.readBuyersData(gallerySellerDetails);
        },

        closeSellerForm: function () {
            var boxObj = D_buyerProcess.sellerDetails.boxObj;
            $('.quick-call-box a').removeClass('uc-pg-btn-orange-bt-open');
            $('.blackOut-window-bt').hide();
            boxObj.hide();
            mainElement.find('.quick-call-box').removeClass('visible');
            $.isDisabled = false;
            clearTimeout($.isEnabled);
            if (ISSELLERDETAILVIEWED || $.cookie('TempCurrentUser'))
                D_buyerProcess.utils.changeBtnTextToViewSeller();
            else
                D_buyerProcess.utils.changeBtnTextToGetSeller(contactSeller);
        },
        checkShareEmail: function () {
            var boxObj = D_buyerProcess.sellerDetails.boxObj;
            var emailBox = boxObj.find(".emailBox");
            if (boxObj.find(".emailTick").is(':checked')) {
                emailBox.show();  // checked
                emailBox.find("input[name*='txtEmail']").show();
                boxObj.find("#txtEmail").focus();
            }
            else {
                emailBox.hide();  // Unchecked
                emailBox.find("input[name*='txtEmail']").hide();
                boxObj.find("#txtMobile").focus();
            }
        },
        setVariables: function (mainElement) {
            contactSeller = contactSeller ? contactSeller : mainElement;
            profileId = contactSeller.attr("profileid");
            cityId = contactSeller.attr("cityId");
            cityName = contactSeller.attr("cityName");
            deliveryCity = contactSeller.attr("dc");
            var modelName = "", makeName = "", price = "", cityName = "", Seller = "";
            modelName = contactSeller.attr("modelName");
            makeName = contactSeller.attr("makeName");
            price = contactSeller.attr("price");
            cityName = contactSeller.attr("cityName");
            Seller = contactSeller.attr("Seller");
            makeId = contactSeller.attr("makeId");
            rootId = contactSeller.attr("rootId");

            //declaration of remarketingVariables is in classified_search.js
            remarketingVariables.carMake = makeName;
            remarketingVariables.carModel = modelName;
            remarketingVariables.price = price;
            remarketingVariables.cityName = cityName;
            remarketingVariables.sellerType = Seller;
            remarketingVariables.sellerDetailsClick = "1"
            remarketingVariables.cityId = cityId;
            remarketingVariables.makeId = makeId;
            remarketingVariables.rootId = rootId;

            mainElement.find('.quick-call-box').addClass('visible');


            profileId_g = profileId;
            rank_g = mainElement.parents("li").attr("rank");
            rank_abs = mainElement.parents("li").attr("rankAbs");
            isPremium_g = mainElement.parents("li").attr("isPremium");
            inquiryId_g = profileId.substring(1, profileId.length);
            isDealer_g = Seller == "Dealer" ? "1" : "0";
            carModel_g = modelName;
            makeYear_g = '';
            makeName_g = makeName;
            price_g = price;
            cityName_g = cityName;
            dealer_g = Seller == "Dealer" ? "Dealer" : "";
            individual_g = Seller == "Individual" ? "Individual" : ""
            kmNumeric_g = contactSeller.attr("kmNumeric");
            priceNumeric_g = contactSeller.attr("priceNumeric");
            bodyTypeId_g = contactSeller.attr("bodyStyleId");
            versionSubSegment_g = contactSeller.attr("versionSubsegmentId");
            stockRecommendationsUrl_g = contactSeller.attr('stockRecommendationsUrl');

        },
        openChat: function () {
            var mainElement = D_buyerProcess.sellerDetails.gsdButton.parent().parent().parent().parent();
            boxObj = D_buyerProcess.sellerDetails.boxObj;
            chatUIProcess.loader.show(D_buyerProcess.sellerDetails.chatBtn.find('.chat-btn'));
            D_buyerProcess.sellerDetails.processCommonGSDClick(mainElement, boxObj);
        },
        openGetSellerForm: function () {
            boxObj = D_buyerProcess.sellerDetails.boxObj;
            var mainElement = D_buyerProcess.sellerDetails.gsdButton.parent().parent().parent().parent();
            D_buyerProcess.sellerDetails.showGSDBlackout();
            D_buyerProcess.deliveryText.displayDeliveryText(mainElement);
            boxObj.find("#initWait").css({ "display": "inline-block" });
            D_buyerProcess.utils.changeBtnTextToLoading(mainElement);
            D_buyerProcess.sellerDetails.processCommonGSDClick(mainElement, boxObj);
        },
        processCommonGSDClick: function (mainElement, boxObj) {
            boxObj = D_buyerProcess.sellerDetails.boxObj;
            ISRECOMMENDED = false;
            setFocusOnTxt();
            contactSeller = mainElement.find('.contact-seller');
            D_buyerProcess.sellerDetails.setVariables(mainElement);
            if ($('#similarCarPopup').is(':visible'))
                originId = 20;
            else
                originId = 1;
            D_buyerProcess.sellerDetails.hasShownInterest(boxObj);
        },
        showGSDBlackout: function () {
            D_buyerProcess.sellerDetails.recommendationBlackOut.show();
        },
        hideGSDBlackout: function () {
            D_buyerProcess.sellerDetails.recommendationBlackOut.hide();
        },
        hasShownInterest: function (boxObj) {
            if (boxObj != undefined && (boxObj.attr('id') == 'pg_seller_details' || (boxObj.attr('id') != undefined && boxObj.attr('id').indexOf('viewDetailsBtn-') === 0))) {
                var profileId = boxObj.attr('ProfileId');
                profileId_g = profileId;
                deliveryCity = boxObj.attr("dc");
                inquiryId_g = profileId.substring(1, profileId.length);
                isDealer_g = (profileId.charAt(0) == 'D' || profileId.charAt(0) == 'd') ? '1' : '0';
                rank_abs = boxObj.closest('li').attr('rankabs');
            }
            transToken = null;
            if ((ISSELLERDETAILVIEWED || $.cookie('TempCurrentUser'))) {
                D_buyerProcess.sellerDetails.processHasShownInterestResponse(contactSeller, boxObj);
            }
            else {
                if (chatProcess.isChatLead) {
                    chatUIProcess.loader.hide();
                    cwTracking.trackCustomDataWithQs(searchPageVariables.getBhriguCategory(), searchPageVariables.getChatUnVerifiedText(), 'profileId=' + profileId_g
                        , D_usedSearch.utils.getQS());
                }
                D_buyerProcess.utils.changeBtnTextToGetSeller(contactSeller);
                D_buyerProcess.sellerDetails.hideGSDBlackout();
                $('#newLoading').hide();
                $.isDisabled = false;
                clearTimeout($.isEnabled);
                D_buyerProcess.sellerDetails.openPopup();
            }
        },
        processHasShownInterestResponse: function (contactSeller, boxObj) {
            $('#newLoading').hide();
            isGSDClick = "1";
            var tempUser = $.cookie("TempCurrentUser");
            if (tempUser && !isForbidden_g && !isLimitExceeded_g) {
                hideBuyerForm(boxObj);
                var buyer = tempUser.split(':');
                buyersName = buyer[0];
                buyersMobile = buyer[1];
                buyersEmail = buyer[2];
                if (!chatProcess.isChatLead) {
                    D_buyerProcess.utils.changeBtnTextToGettingDetails(contactSeller);
                }
                D_buyerProcess.sellerDetails.processPurchaseInquiries(boxObj);
                boxObj.find("#loadDetails").css({ "display": "inline-block", "margin-top": "120px", "margin-left": "180px" });
            }
            else {
                D_buyerProcess.sellerDetails.readBuyersData(contactSeller);
                Common.utils.trackAction('Verification_Stage2', 'UsedCarSearch', 'Verification_Stage2');
            }
            boxObj.find("#initWait").hide();
        },
        prepareSellInfo: function (boxObj, seller) {
            if (boxObj != undefined && boxObj.attr('id') == 'pg_seller_details') {
                D_buyerProcess.photoGallery.showPgSellerDetails(boxObj, seller);
            }
            if (!ISRECOMMENDED) {
                D_buyerProcess.recommendedCars.hitRecommendationsApi(boxObj, seller);
            }
            else {
                showSellerDetailsForRecommendations(boxObj, seller);
                var recommendedCarRank = 'r' + D_buyerProcess.recommendedCars.getRankFromRecommendationsId(boxObj.attr('id')) + 1;
            }
            ISSELLERDETAILVIEWED = true;
            D_buyerProcess.utils.changeBtnTextToViewSeller();

            if (ISSELLERDETAILVIEWED || $.cookie('TempCurrentUser') != null) {
                $("#pg_get_details").attr('value', 'VIEW SELLER DETAILS');
            }
            triggerRemarketingCode();
        },
        readBuyersData: function (box_obj, isValuation) {
            var emailExist;
            if (box_obj && isValuation) {
                buyersName = $("#rpName").val();
                buyersMobile = $("#rpMobile").val();
                profileId_g = box_obj.attr('profileid');
                rank_g = box_obj.attr('rank');
                rank_abs = box_obj.attr('rankAbs');
                isPremium_g = box_obj.attr('isPremium');
                emailExist = $('#emailTick').hasClass('uc-checked');
                deliveryCity = box_obj.attr("dc");
                if (emailExist) {
                    buyersEmail = $("#txtEmail").val();
                }
                else {
                    buyersEmail = "";
                }
                originId = buyerProcessOriginId.RightPrice;
            }
            else if (box_obj != undefined && box_obj.attr('id') == 'pg_seller_details') {
                buyersName = box_obj.find('#pg_txtName').val();
                buyersMobile = box_obj.find('#pg_txtMobile').val();
                profileId_g = box_obj.attr('ProfileId');
                rank_g = box_obj.attr('rank');
                rank_abs = box_obj.attr('rankAbs');
                isPremium_g = box_obj.attr('isPremium');
                deliveryCity = box_obj.find('#pg_get_details').attr("dc");

                emailExist = box_obj.find('#pg_emailTick').is(':checked');
                if (emailExist) {
                    buyersEmail = box_obj.find("#pg_txtEmail").val();
                }
                else {
                    buyersEmail = "";
                }
            }
            else {
                buyersName = $("#getName").val();
                buyersMobile = $('#txtMobile').val();
            }
            ltsrc = $.getCookie("CWLTS").split(':')[0];
            cwc = $.getCookie("CWC");
            if (box_obj != undefined && box_obj.attr('id') == 'pg_seller_details') {
                if (ValidateSellerForm(box_obj, isValuation)) {
                    D_buyerProcess.sellerDetails.processPurchaseInquiries(box_obj);
                }
                else {
                    if (chatProcess.isChatLead) {
                        chatUIProcess.loader.hide();
                        cwTracking.trackCustomDataWithQs(searchPageVariables.getBhriguCategory(), searchPageVariables.getChatUnVerifiedText(), 'profileId=' + profileId_g, D_usedSearch.utils.getQS());
                    }
                }
            } else if (isValuation) {
                if (ValidateSellerForm(box_obj, isValuation)) {
                    D_buyerProcess.sellerDetails.processPurchaseInquiries(box_obj);
                }
                else {
                    if (chatProcess.isChatLead) {
                        chatUIProcess.loader.hide();
                        cwTracking.trackCustomDataWithQs(searchPageVariables.getBhriguCategory(), searchPageVariables.getChatUnVerifiedText(), 'profileId=' + profileId_g, D_usedSearch.utils.getQS());
                    }
                    else {
                        D_usedSearch.Valuation.sellerDetails.sellerDetailsBtn.show();
                        D_usedSearch.Valuation.sellerDetails.gettingDetailsBtn.addClass('hide');
                    }
                }
            }
            else {

                D_buyerProcess.sellerDetails.processPurchaseInquiries(boxObj);
                dataLayer.push({ event: 'GetSellerDetailsNow_AllSendDetailsClick', cat: 'UsedCarSearch', act: 'GetSellerDetailsNow_AllSendDetailsClicked' });
            }
        },
        originIds: {
            search: 1,
            search_pg: 2,
            search_recommendations: 15,
            search_pg_recommendations: 18,
            search_similarcars: 20,
            search_nearbycity: 22,
            search_rp: 37,
            search_rp_recommendations: 39
        },
        processPurchaseInquiries: function (boxObj) {
            var leadApiData = {
                "profileId": profileId_g,
                "buyer": {
                    "name": buyersName,
                    "mobile": buyersMobile,
                    "email": buyersEmail
                },
                "leadTrackingParams": {
                    "originId": originId,
                    "rank": rank_abs,
                    "deliveryCity": deliveryCity ? deliveryCity : undefined,
                    "leadType": D_buyerProcess.leadType.getLeadType()
                }
            };
            if (
                leadApiData.leadTrackingParams.originId === D_buyerProcess.sellerDetails.originIds.search ||
                leadApiData.leadTrackingParams.originId === D_buyerProcess.sellerDetails.originIds.search_pg ||
                leadApiData.leadTrackingParams.originId === D_buyerProcess.sellerDetails.originIds.search_nearbycity ||
                leadApiData.leadTrackingParams.originId === D_buyerProcess.sellerDetails.originIds.search_rp
            ) {
                leadApiData.leadTrackingParams["queryString"] = window.location.hash.substr(1);
            }
            var gsdBtn = leadData.getCurrGsdButton();
            if (gsdBtn && gsdBtn.dataset) {
                leadApiData.leadTrackingParams["slotId"] = gsdBtn.dataset.slotId || 0;
            }
            var deferStockLeads = $.Deferred();
            var deferRecommendations = $.Deferred();
            var hitRecoApi = $.Deferred();
            var deferChat = $.Deferred();

            var usedCarPurchaseInqPromise = D_buyerProcess.buyerProcessApis.processUsedCarPurchaseInquiries(boxObj, leadApiData).complete(deferStockLeads.resolve);
            usedCarPurchaseInqPromise.always(function () {
                if (isFromCaptcha == "1")
                    isFromCaptcha = "0";
                if (isGSDClick == "1")
                    isGSDClick = "0";
                hideBuyerForm(boxObj);
                $('#newLoading').hide();
                boxObj.find('#pg-process_img,#pg-loadingImg').hide();
                boxObj.find("#loadDetails").hide();
            }).fail(function (jqXHR) {
                D_buyerProcess.sellerDetails.processLeadFailureResponse(boxObj, jqXHR);
            });

            if (!ISRECOMMENDED && !$('#similarCarPopup').is(':visible')) {
                if (typeof boxObj !== 'undefined' && boxObj.attr('id') == 'rpgetsellerDetails') {
                    stockRecommendationsUrl_g = boxObj.attr('stockRecommendationsUrl');
                }
                else if (typeof boxObj !== 'undefined' && boxObj.attr('id') == 'pg_seller_details') {
                    stockRecommendationsUrl_g = boxObj.find('#pg_get_details').attr('stockRecommendationsUrl');
                }

                if (!chatProcess.isChatLead) {
                    hitRecoApi = $.ajax({
                        type: 'GET',
                        url: stockRecommendationsUrl_g,
                        headers: { "sourceid": "1" },
                        dataType: 'json'
                    })
                    .complete(deferRecommendations.resolve);
                }
                else {
                    deferChat.resolve();// in case of chat lead except on recommendations and similar's car chat lead.
                }
            }
            else {
                // in case of recommendation and similar car's chat lead, resolve deferChat else deferRecommendations is resolved.
                chatProcess.isChatLead ? deferChat.resolve() : deferRecommendations.resolve(); 
            }

            $.when(usedCarPurchaseInqPromise, hitRecoApi).done(function (response, jsonData) {
                D_buyerProcess.sellerDetails.processLeadSuccessResponse(boxObj, response[0], jsonData);
            });

            // if deferChat is not used, in gsd lead, this will also be excecuted. 
            $.when(usedCarPurchaseInqPromise, deferChat).done(function (response) { // executed when chat lead is given.
                D_buyerProcess.sellerDetails.processLeadSuccessResponse(boxObj, response[0]);
            });

            $.when(deferStockLeads, deferRecommendations).done(function () {
                if (usedCarPurchaseInqPromise.isResolved() && !hitRecoApi.isResolved()) {
                    D_buyerProcess.sellerDetails.processLeadSuccessResponse(boxObj, JSON.parse(usedCarPurchaseInqPromise.responseText));
                }
            });
        },
        processLeadSuccessResponse: function (boxObj, response, jsonData) {
            Common.utils.lockPopup();
            D_buyerProcess.sellerDetails.closePopup();
            isForbidden_g = false;
            isLimitExceeded_g = false;
            var date = new Date();
            date.setTime(date.getTime() + (90 * 24 * 60 * 60 * 1000)); //90 days
            var cookieExpiry = date.toGMTString();
            document.cookie = "TempCurrentUser=" + buyersName + ":" + buyersMobile + ":" + buyersEmail + ":0; expires=" + cookieExpiry + "; path=/";
            chatProcess.processChatRegistration(response.appId, response.buyer, chatUIProcess.setChatIconVisibilty, chatProcess.source.desktopBrowser, chatUIProcess.loader.hide);
            if (chatProcess.isChatLead) {
                chatProcess.startChat(response.appId, $('#chat-btn-search'), response.seller, response.buyer, response.stock, chatUIProcess.setChatIconVisibilty, chatUIProcess.loader.hide);
                D_buyerProcess.utils.changeBtnTextToViewSeller();
                cwTracking.trackCustomDataWithQs(searchPageVariables.getBhriguCategory(), searchPageVariables.getChatVerifiedText(), 'profileId=' + profileId_g, D_usedSearch.utils.getQS());
            }
            else {
                if (!$('#similarCarPopup').is(':visible')) {
                    D_buyerProcess.utils.changeBtnTextToViewSeller();
                    if (!ISRECOMMENDED) {
                        if (jsonData && jsonData[0].length > 0) {
                            if (boxObj.attr('id') != 'rpgetsellerDetails')
                                D_buyerProcess.recommendedCars.bindRecommendations(jsonData[0], response.seller);
                            else
                                D_usedSearch.Valuation.recommendations.bindRecommendations(jsonData[0]);
                        }
                        else {
                            if (boxObj.attr('id') != 'pg_seller_details' && boxObj.attr('id') != 'rpgetsellerDetails') {
                                D_buyerProcess.sellerDetails.openNoRecommendationSellerDetails(response.seller);
                            }
                        }
                    }
                }
                else {
                    D_buyerProcess.sellerDetails.openNoRecommendationSellerDetails(response.seller);
                }
                if (boxObj && boxObj.attr('id') == "rpgetsellerDetails") {
                    D_usedSearch.Valuation.sellerDetails.bindSellerDetails(response.seller);
                    D_buyerProcess.sellerDetails.processRatingText(response.seller.ratingText, $(".list-item__details-col .top-rated-seller-tag"), $("#sellerName"));
                    formViewSimilarCar(boxObj);
                }
                else {
                    if (boxObj != undefined && boxObj.attr('id') == 'pg_seller_details') {
                        D_buyerProcess.photoGallery.showPgSellerDetails(boxObj, response.seller);
                    }
                    if (ISRECOMMENDED) {
                        showSellerDetailsForRecommendations(boxObj, response.seller);
                        var recommendedCarRank = 'r' + D_buyerProcess.recommendedCars.getRankFromRecommendationsId(boxObj.attr('id')) + 1;
                    }
                    ISSELLERDETAILVIEWED = true;
                    D_buyerProcess.utils.changeBtnTextToViewSeller();

                    if (ISSELLERDETAILVIEWED || $.cookie('TempCurrentUser') != null) {
                        $("#pg_get_details").attr('value', 'VIEW SELLER DETAILS');
                    }
                    triggerRemarketingCode();
                }
            }
            if (boxObj != undefined && boxObj.attr('id') == 'pg_seller_details') {
                fbq('track', 'Lead', { content_name: 'Desktop Search PhotoGallery', content_category: 2 });
            }
            else if (boxObj != undefined && boxObj.hasClass('view-details')) {
                fbq('track', 'Lead', { content_name: 'Desktop Search Recommendation', content_category: 5 });
            }
            else {
                fbq('track', 'Lead', { content_name: 'Desktop Search Page', content_category: 1 });
            }
            D_buyerProcess.tracking.outbrainTracking();
            D_buyerProcess.tracking.trovitTracking();
            D_buyerProcess.tracking.conversionTracking();
            D_buyerProcess.tracking.adWordTracking();
        },
        processLeadFailureResponse: function (boxObj, jqXHR) {
            var response = JSON.parse(jqXHR.responseText);
            if (chatProcess.isChatLead) {
                chatUIProcess.loader.hide();
                cwTracking.trackCustomDataWithQs(searchPageVariables.getBhriguCategory(), searchPageVariables.getChatUnVerifiedText(), 'profileId=' + profileId_g, D_usedSearch.utils.getQS());
            }
            switch (parseInt(jqXHR.status)) {
                case 403:
                    isForbidden_g = true;
                    isLimitExceeded_g = true;
                    if (boxObj != undefined && boxObj.attr('id') == 'pg_seller_details') {
                        if (D_buyerProcess.sellerDetails.isMobileUnverified(response)) {
                            D_buyerProcess.sellerDetails.hitMobileVerificationApi(buyersMobile, D_buyerProcess.sellerDetails.sendOtpApiHandler);
                        }
                        else {
                            $('#pg_contactSellerForm').hide();
                            boxObj.show();
                            boxObj.find("#pg-not_auth").show().find(".back-to-gsd-form > span").text(response.Message);
                            $('span.verification-close-btn').show();
                        }
                    }
                    else if (boxObj && boxObj.attr('id') == "rpgetsellerDetails") {
                        if (D_buyerProcess.sellerDetails.isMobileUnverified(response)) {
                            D_buyerProcess.sellerDetails.hitMobileVerificationApi(buyersMobile, D_buyerProcess.sellerDetails.sendOtpApiHandler);
                        }
                        else {
                            D_usedSearch.Valuation.sellerDetails.showServerErrorMessage(response.Message);
                        }
                    }
                    else {
                        if (ISRECOMMENDED) {
                            showErrorsForRecommendations(boxObj, response.Message);
                        }
                        else if ($('.view-details').is(':visible')) {
                            var  similarCarId = D_buyerProcess.recommendedCars.getRankFromRecommendationsId(boxObj.attr('id'));
                            D_buyerProcess.recommendedCars.toggleOrShowError(boxObj, similarCarId);
                        }
                        else {
                            if (D_buyerProcess.sellerDetails.isMobileUnverified(response)) {
                                D_buyerProcess.sellerDetails.hitMobileVerificationApi(buyersMobile, D_buyerProcess.sellerDetails.sendOtpApiHandler);
                            }
                            else {
                                D_buyerProcess.sellerDetails.showGSDBlackout();
                                boxObj.find('#sellerDetailsScreen').hide();
                                boxObj.show().addClass('withoutRecommendation');
                                boxObj.find("#not_auth").show().html(response.Message);
                                $('span.verification-close-btn').show();
                                D_buyerProcess.utils.changeBtnTextToViewSeller();
                                $('div.noRecommendation').css({ 'float': 'left', 'left': '0' }).removeClass('grid-4').addClass('grid-12');
                            }
                        }
                    }
                    break;
                default:
                    if (boxObj != undefined && boxObj.attr('id') == 'pg_seller_details') {
                        $('#pg_contactSellerForm').hide();
                        boxObj.show();
                        boxObj.find("#pg-not_auth").show().find(".back-to-gsd-form > span").text(response.Message);
                        $('span.verification-close-btn').show();
                    } else if (boxObj && boxObj.attr('id') == "rpgetsellerDetails") {
                        D_usedSearch.Valuation.sellerDetails.showServerErrorMessage(response.Message);
                    }
                    else {
                        if (!ISRECOMMENDED) {
                            boxObj.find('#sellerDetailsScreen').hide();
                            boxObj.show().addClass('withoutRecommendation');
                            boxObj.find("#not_auth").show().html(response.Message);
                            D_buyerProcess.utils.changeBtnTextToViewSeller();
                            $('div.noRecommendation').css({ 'float': 'left', 'left': '0' }).removeClass('grid-4').addClass('grid-12');
                            $('span.verification-close-btn').show();
                        }
                    }
                    break;
            }
        },
        missedCallPollingVeriHandler: function (json) {
            if (json && json.isMobileVerified) {
                D_buyerProcess.sellerDetails.otpForm.find('.otp__error').text('');
                D_buyerProcess.sellerDetails.otpForm.hide();
                D_buyerProcess.sellerDetails.handleVerificationSuccessResponse();
            }
        },
        handleVerificationSuccessResponse: function () {
            ISSELLERDETAILVIEWED = true;
            $('#otpError').addClass('hide');
            D_buyerProcess.sellerDetails.otpForm.find('.otp__error').text('');
            D_buyerProcess.sellerDetails.closePopup();
            D_buyerProcess.sellerDetails.otpForm.hide();
            $('.otp-modal-bg').hide();
            if ($('#photoGallery').is(':visible')) {
                if (chatProcess.isChatLead) {
                    $('#pg_chat_btn_container').trigger('click');
                }
                else {
                    D_buyerProcess.sellerDetails.getPgSellerDetails();
                }
            }
            else if ($('.valuation-popup').is(':visible')) {
                if (chatProcess.isChatLead) {
                    $("#rp_chat_btn_container").trigger('click');
                }
                else {
                    $('#rpgetsellerDetails').trigger('click');
                }
            }
            else if (chatProcess.isChatLead) {
                D_buyerProcess.sellerDetails.openChat();
            }
            else {
                D_buyerProcess.sellerDetails.openGetSellerForm();
            }
        },
        missedCallClickVeriHandler: function (veriResponse) {
            $("#missed-call__loading").hide();
            if (veriResponse && veriResponse.isMobileVerified) {
                D_buyerProcess.sellerDetails.otpForm.find('.otp__error').text('');
                D_buyerProcess.sellerDetails.otpForm.hide();
                D_buyerProcess.sellerDetails.handleVerificationSuccessResponse();
            }
            else {
                $(".missed-call__info-text").hide();
                $(".missed-call__error-msg").show();
            }
        },
        sendOtpApiHandler: function (json) {
            if (json && json.isOtpGenerated) {
                $('#getOTP').val("");
                D_buyerProcess.sellerDetails.otpForm.find('.otp__error').text('');
                D_buyerProcess.sellerDetails.resetTimer($('#otpTimer'), D_buyerProcess.sellerDetails.clearTimerTimeout);
                D_buyerProcess.sellerDetails.otpForm.show();
                $('.otp-modal-bg').show();
                D_buyerProcess.sellerDetails.clearTimerTimeout = D_buyerProcess.sellerDetails.setTimer($('#otpTimer'), 'Resend OTP', 30);
                $("#missed-call-number").html('<span class="tel-icon"></span>' + json.tollFreeNumber);
                $(".missed-call__info-text").show();
                $(".missed-call__error-msg").hide();
                commonUtilities.executeTimely(function () {
                    if ($("#missed-call-number").is(":visible")) {
                        D_buyerProcess.sellerDetails.hitIsMobileVerifiedApi(buyersMobile, D_buyerProcess.sellerDetails.missedCallPollingVeriHandler);
                    }
                    else {
                        return true;
                    }
                }, 5000, 15000, 10);
            }
        },
        getMobileVerificationApiData: function (mobile) {
            var mobileVerificationApiData = {
                "sourceModule": D_buyerProcess.utils.otpVariables.sourceModule, //For UsedcarLead module
                "mobileVerificationByType": D_buyerProcess.utils.otpVariables.mobileVerificationByType.otpAndMissedCall,
                "validityInMins": D_buyerProcess.utils.otpVariables.defaultValidityInMins,
                "otpLength": D_buyerProcess.utils.otpVariables.defaultOtpLength
            };
            if (mobile) {
                mobileVerificationApiData["mobile"] = mobile;
            }
            return mobileVerificationApiData;
        },
        isMobileUnverified: function (response) {
            return (response.ModelState && response.ModelState.hasOwnProperty("MobileUnverified"));
        },
        hitMobileVerificationApi: function (mobileNumber, responseHandler) {//checks if mobile is verified and sends otp if not.
            $.ajax({
                type: 'POST',
                headers: { "sourceid": "1" },
                url: '/api/v1/mobile/' + mobileNumber + '/verification/start/',
                data: JSON.stringify(D_buyerProcess.sellerDetails.getMobileVerificationApiData()),
                contentType: 'application/json',
                dataType: 'json',
                success: function (json) {
                    if (responseHandler)
                        responseHandler(json);
                }
            });
        },
        hitIsMobileVerifiedApi: function (mobileNumber, responseHandler) {//checks if mobile is verified.
            $.ajax({
                type: 'GET',
                headers: { "sourceid": "1" },
                url: '/api/v1/mobile/' + mobileNumber + '/verification/status/',
                dataType: 'Json',
                success: function (json) {
                    if (responseHandler)
                        responseHandler(json);
                }
            });
        },
        bindSellerDetails: function (seller) {
            $("#blackOut-recommendation").show();
            $("#sellerDetailsScreen").show();
            $("#sellerNameId").show();
            if (seller.dealerShowroomPage) {
                $('.seller-virtual-link').attr('href', seller.dealerShowroomPage).show();
            }
            else {
                $(".seller-virtual-link").hide();
            }
            D_buyerProcess.sellerDetails.processRatingText(seller.ratingText, $("#topRatedSellerTag"), $("#contactPersonId"));
            if (isDealer_g == "1") {
                $("#sellerNameId").text(seller.name);
                $("#contactPersonId").text(seller.contactPerson ? seller.contactPerson : "");
            }
            else {
                $("#sellerNameId").hide();
                $("#contactPersonId").text(seller.name);
            }
            $("#sellerMobileId").text(seller.mobile);
            $("#sellerEmailId").text(seller.email);
            $("#sellerAddressId").text(seller.address);
            $('#contactSeller').show();
            $('#not_auth').hide();
        },
        processRatingText: function (ratingText, $topSellerNode, $sellerNameNode) {         //seller rating text show/hide logic,  
            //this function will hide or show top seller tag based on rating text available or not and adjust width of seller name div based on rating text
            if (ratingText) {
                $topSellerNode.html(ratingText).show();
                $sellerNameNode.removeClass("full-width");
            }
            else {
                $topSellerNode.hide();
                $sellerNameNode.addClass("full-width");
            }
        },
        openNoRecommendationSellerDetails: function (seller) {
            D_buyerProcess.sellerDetails.bindSellerDetails(seller);
            $("#recommendCars").show().addClass('withoutRecommendation');
            $(".suggestCarsTxt").hide();
            $("span.verification-close-btn").show();
            $('.bp-SimilarCars').show();
            $('div.noRecommendation').css({ 'float': 'left', 'left': '0' }).removeClass('grid-4').addClass('grid-12');
            formViewSimilarCar(boxObj);
            D_buyerProcess.sellerDetails.formSimilarCarLink(boxObj);
            if (D_buyerProcess.openVerificationPopup.isPopupEligible())
                D_buyerProcess.openVerificationPopup.slidePopup();
        },
        formSimilarCarLink: function (boxObj) {
            var cityId, makeId, rootId;
            if (boxObj.attr('id') == 'pg_seller_details') {
                var profileId = $("#pg_seller_details").attr('profileId');
                var list = $('.contact-seller[profileid="' + profileId + '"]');
                cityId = list.attr('cityid') == 1 ? 3000 : list.attr('cityid');
                makeId = list.attr('makeid');
                rootId = list.attr('rootid');
            }
            else {
                cityId = remarketingVariables.cityId == 1 ? 3000 : remarketingVariables.cityId;
                makeId = remarketingVariables.makeId;
                rootId = remarketingVariables.rootId;
            }
            var URL = "/used/cars-for-sale/?city=" + cityId + "&car=" + makeId + "." + rootId;
            $('.similarCars').attr('href', URL);
            $('.pg-similarCars').attr('href', URL);
        },
        openPopup: function () {
            D_buyerProcess.sellerDetails.popupContainer.show();
            $('.seller-details-modal-bg').show();
            D_buyerProcess.sellerDetails.hideGSDBlackout();
            Common.utils.lockPopup();
        },
        closePopup: function () {
            D_buyerProcess.sellerDetails.popupContainer.hide();
            $('.seller-details-modal-bg').hide();
            Common.utils.unlockPopup();
        },
        processLead: function () {
            var isValid = false;

            isValid = validateBuyerName($('#getName'));
            isValid &= D_buyerProcess.sellerDetails.validateMobile(contactSeller);

            if ((ISSELLERDETAILVIEWED || $.cookie('TempCurrentUser')) || isValid) {
                D_buyerProcess.sellerDetails.processHasShownInterestResponse(contactSeller, boxObj);
            }
        },
        validateMobile: function (contactSellerButtonElement) {
            if (contactSellerButtonElement) {
                var buyersMobileElement = $('#txtMobile');
                var buyersMobile = buyersMobileElement.val();
                var mobileErrorElement = D_buyerProcess.sellerDetails.form.find("#txtMobileError");
                var fieldContainer = buyersMobileElement.closest('.form-control-box');

                if (buyersMobile == "") {
                    ShakeFormView(fieldContainer);
                    mobileErrorElement.html("Please enter your mobile number");
                    mobileErrorElement.show();
                    mobileErrorElement.next().show();
                    return false;
                } else if (buyersMobile != "" && re.test(buyersMobile) == false) {
                    ShakeFormView(fieldContainer);
                    mobileErrorElement.html("Invalid mobile number");
                    mobileErrorElement.show();
                    mobileErrorElement.next().show();
                    return false;
                } else if (buyersMobile != "" && (!re.test(buyersMobile) || buyersMobile.length < 10 || buyersMobile.length > 10)) {
                    ShakeFormView(fieldContainer);
                    mobileErrorElement.html("Your mobile number should be of 10 digits only");
                    mobileErrorElement.show();
                    mobileErrorElement.next().show();
                    return false;
                } else {
                    mobileErrorElement.hide();
                    mobileErrorElement.next().hide();
                    buyersMobileElement.removeClass("red-border");
                }
            }
            return true;
        }
    },
    openVerificationPopup: {
        isPopupEligible: function () {
            if ($.cookie('TempCurrentUser') && $.cookie('ShowVerificationPopup') != "0")
                return true;
            else return false;
        },
        slidePopup: function () {
            if ($.cookie('TempCurrentUser'))
                var verifiedNumber = $.cookie('TempCurrentUser').split(':')[1];
            $('div.verificationDetails span.verifiedNumber').text(verifiedNumber);
            $("div.verificationDetails").delay('2000').animate({ right: '0' }, 2000);
            if (!$.cookie('ShowVerificationPopup'))
                SetCookieInDays('ShowVerificationPopup', 1, 30);
        }
    },
    closeVerificationPopup: {
        registerEvents: function () {
            D_buyerProcess.doc.on('click', 'span.closeVerificationBtn', function () {
                D_buyerProcess.closeVerificationPopup.hidePopup();
            });
        },
        hidePopup: function () {
            SetCookieInDays('ShowVerificationPopup', 0);
            $("div.verificationDetails").animate({ right: '-185px' }, 1000);
        }
    },
    recommendedCars: {
        viewSellerBtnVisible: ko.observable(true),
        registerEvents: function () {
            D_buyerProcess.doc.on('click', 'a.view-details', function () {
                ISRECOMMENDED = true;
                chatProcess.isChatLead = false;
                leadData.setCurrGsdButton(this);
                var recommendedGsdBtn = $(this);
                recommendedGsdBtn.prop('disabled', true);
                var similarCarId = D_buyerProcess.recommendedCars.getRankFromRecommendationsId(recommendedGsdBtn.attr('id'));
                D_buyerProcess.recommendedCars.hideRecommendation(similarCarId);
                D_buyerProcess.utils.changeBtnTextToLoading(contactSeller);
                if (isForbidden_g || isLimitExceeded_g) {
                    D_buyerProcess.recommendedCars.toggleOrShowError(recommendedGsdBtn, similarCarId);
                    setTimeout(function () { recommendedGsdBtn.prop('disabled', false); }, 500);
                    return;
                }
                D_buyerProcess.recommendedCars.getSellerDetailsOrToggle(recommendedGsdBtn, similarCarId);
                D_buyerProcess.recommendedCars.trackCodeForRecommendations(recommendedGsdBtn);
                tracker.trackGsdClick(originId);
                setTimeout(function () { recommendedGsdBtn.prop('disabled', false); }, 500);
            });
            D_buyerProcess.doc.on('click', '.view-details-chat', function () {
                ISRECOMMENDED = false;
                chatProcess.isChatLead = true;
                var $chatBtn = $(this);
                var recommendedGsdBtn = $chatBtn.parent().parent().find('.view-details');
                if (isForbidden_g || isLimitExceeded_g) {
                    var similarCarId = D_buyerProcess.recommendedCars.getRankFromRecommendationsId(recommendedGsdBtn.attr('id'));
                    D_buyerProcess.recommendedCars.toggleOrShowError(recommendedGsdBtn, similarCarId);
                    return;
                }
                chatUIProcess.loader.show($chatBtn.find('.chat-btn'));
                D_buyerProcess.sellerDetails.hasShownInterest(recommendedGsdBtn);
                D_buyerProcess.recommendedCars.trackCodeForRecommendations(recommendedGsdBtn);
            });

        },
        hideRecommendation: function (recommendedCarRank) {
            $('#suggestDetailsData-' + recommendedCarRank).hide();
            $('#loadingIconRecommendations-' + recommendedCarRank).hide();
            $('#sellerdetailsData-' + recommendedCarRank).hide();
        },
        trackCodeForRecommendations: function (recommendedGsdBtn) {
            if ($('#photoGallery').is(':visible'))
                Common.utils.trackAction('UsedRecommendations', 'Desk_UsedRecommendations', 'Desk_UsedRecoResponses', 'SearchPhoto-FormFill-' + recommendedGsdBtn.parents('li:first').attr('id'));
            else
                Common.utils.trackAction('UsedRecommendations', 'Desk_UsedRecommendations', 'Desk_UsedRecoResponses', 'SearchPage-FormFill-' + recommendedGsdBtn.parents('li:first').attr('id'));
        },
        showErrors: function (recommendedGsdBtn) {
            if (isForbidden_g || isLimitExceeded_g) {
                showErrorsForRecommendations(recommendedGsdBtn, LIMITEXCEEDMSG);
            }
        },
        toggleOrShowError: function (recommendedGsdBtn, similarCarId) {
            if (!D_buyerProcess.recommendedCars.IsRecommended1ClickBtnView(recommendedGsdBtn)) {//if HIDE SELLER DETAILS is in view
                toggleViewDetailsBtn(recommendedGsdBtn);
                D_buyerProcess.recommendedCars.hideRecommendation(similarCarId);
            }
            else {
                recommendedGsdBtn.closest('li').find('.othersellerDetails').slideToggle(500);
                D_buyerProcess.recommendedCars.showErrors(recommendedGsdBtn);
            }
        },
        getSellerDetailsOrToggle: function (recommendedGsdBtn, similarCarId) {
            if (D_buyerProcess.recommendedCars.IsRecommended1ClickBtnView(recommendedGsdBtn)) {
                if ($('#photoGallery').is(':visible'))
                    originId = 18;
                else if (D_usedSearch.Valuation.valuationPopupObj.is(':visible')) {
                    originId = buyerProcessOriginId.RightPriceRecommendations;
                }
                else
                    originId = 15;

                $('#loadingIconRecommendations-' + similarCarId).show();
                toggleViewDetailsBtn(recommendedGsdBtn);
                D_buyerProcess.sellerDetails.hasShownInterest(recommendedGsdBtn);
            }
            else {
                toggleViewDetailsBtn(recommendedGsdBtn);
                D_buyerProcess.recommendedCars.hideRecommendation(similarCarId);
            }
        },
        IsRecommended1ClickBtnView: function (recommendedGsdBtn) {
            return !recommendedGsdBtn.find(".oneClickDetails").hasClass("hideImportant");
        },
        hitRecommendationsApi: function (boxObj, seller) {
            if (boxObj != undefined && boxObj.attr('id') == 'pg_seller_details') {
                var pgGetDetailsBtn = boxObj.find('#pg_get_details');
                cityId = pgGetDetailsBtn.attr('cityid');
                rootId = pgGetDetailsBtn.attr('rootid');
                bodyTypeId_g = pgGetDetailsBtn.attr('bodystyleid');
                priceNumeric_g = pgGetDetailsBtn.attr('pricenumeric');
                kmNumeric_g = pgGetDetailsBtn.attr('kmnumeric');
                makeId = pgGetDetailsBtn.attr('makeid');
                versionSubSegment_g = pgGetDetailsBtn.attr('versionsubsegmentid');
                profileId_g = pgGetDetailsBtn.attr('profileid');
                stockRecommendationsUrl_g = pgGetDetailsBtn.attr('stockRecommendationsUrl');
            } else if (boxObj.attr('id') == 'rpgetsellerDetails') {
                cityId = boxObj.attr('cityid');
                rootId = boxObj.attr('rootid');
                bodyTypeId_g = boxObj.attr('bodystyleid');
                priceNumeric_g = boxObj.attr('pricenumeric');
                kmNumeric_g = boxObj.attr('kmnumeric');
                makeId = boxObj.attr('makeid');
                versionSubSegment_g = boxObj.attr('versionsubsegmentid');
                profileId_g = boxObj.attr('profileid');
                stockRecommendationsUrl_g = boxObj.attr('stockRecommendationsUrl');
            }

            if (!$('#similarCarPopup').is(':visible')) {
                $.ajax({
                    type: 'GET',
                    url: stockRecommendationsUrl_g,
                    headers: { "sourceid": "1" },
                    dataType: 'json',
                    success: function (jsonData) {
                        D_buyerProcess.utils.changeBtnTextToViewSeller();
                        if (jsonData.length > 0) {
                            if (boxObj.attr('id') != 'rpgetsellerDetails')
                                D_buyerProcess.recommendedCars.bindRecommendations(jsonData, seller);
                            else
                                D_usedSearch.Valuation.recommendations.bindRecommendations(jsonData);
                        }
                        else {
                            if (boxObj.attr('id') != 'pg_seller_details' && boxObj.attr('id') != 'rpgetsellerDetails') {
                                D_buyerProcess.sellerDetails.openNoRecommendationSellerDetails(seller);
                            }
                        }
                    },
                    error: function (xhr) {
                        if (boxObj.attr('id') != 'pg_seller_details' && boxObj.attr('id') != 'rpgetsellerDetails') {
                            D_buyerProcess.sellerDetails.openNoRecommendationSellerDetails(seller);
                        }
                    }
                });
            }
            else {
                if (boxObj.attr('id') != 'pg_seller_details') {
                    D_buyerProcess.sellerDetails.openNoRecommendationSellerDetails(seller);
                }
            }
        },
        bindRecommendations: function (jsonData, seller) {
            D_buyerProcess.recommendedCars.viewSellerBtnVisible(true);
            RECOMMENDLISTING(jsonData);
            $('.bp-SimilarCars').hide();
            D_buyerProcess.sellerDetails.bindSellerDetails(seller);
            $("#recommendCars").removeClass('withoutRecommendation').show("scale", { percent: 100 }, 400);
            if (D_buyerProcess.openVerificationPopup.isPopupEligible())
                D_buyerProcess.openVerificationPopup.slidePopup();
            setTimeout(function () { $('div.suggestCarsTxt').fadeIn(); }, 4000);
            $('.noRecommendation').animate({ left: '0' }, 1000).css('float', 'left').addClass('grid-4').removeClass('grid-12');
            $(".animateRecommendation").delay('500').show('slide', { direction: 'left' }, 1500);
            $('span.verification-close-btn').hide();
            if ($('#photoGallery').is(':visible'))
                Common.utils.trackAction('UsedRecommendationsImpressions', 'Desk_UsedRecommendations', 'Desk_UsedRecommendations', 'SearchPhoto-' + $('.recommendedList').length);
            else
                Common.utils.trackAction('UsedRecommendationsImpressions', 'Desk_UsedRecommendations', 'Desk_UsedRecommendations', 'SearchPage-' + $('.recommendedList').length);
        },
        getRankFromRecommendationsId: function (nodeId) {
            //return numeric part in Id present after "-"
            return nodeId.substring(nodeId.lastIndexOf("-") + 1);
        }
    },
    utils: {
        changeBtnTextToViewSeller: function () {
            if (ISSELLERDETAILVIEWED || $.cookie('TempCurrentUser') != null) {
                $("span.gsdTxt").hide();
                $("span.oneClickDetails").show();
                $('p.get-seller-tel').addClass('hide');
                $('span.preVerification').removeClass('preVerification');
                $('span.or-line').removeClass('hide');
                $('span.view-seller-tel').removeClass('hide');
                $('p.seller-note').addClass('hide');
            }
        },
        changeBtnTextToGetSeller: function (contactSeller) {
            if (contactSeller != undefined && $.cookie('TempCurrentUser') == null) {
                contactSeller.find('span.gsdTxt').text('Get Seller Details');
            }
        },
        changeBtnTextToGettingDetails: function (contactSeller) {
            if (contactSeller && (ISSELLERDETAILVIEWED || $.cookie('TempCurrentUser') != null)) {
                contactSeller.find('span.gsdTxt').text('Getting Details...');
            }
        },
        changeBtnTextToLoading: function (contactSeller) {
            if (contactSeller) {
                contactSeller.find('span.gsdTxt').text('Loading...');
            }
        },
        registerEvents: function () {
            D_buyerProcess.doc.on('click', '.blackOut-window-bt', function () {
                D_buyerProcess.utils.closeSellerDetailsPopup();
            });

            D_buyerProcess.doc.on('click', '#blackOut-recommendation', function () {
                D_buyerProcess.utils.closeSellerDetailsPopup();
            });
        },
        closeSellerDetailsPopup: function () {

            if ($('#not_auth').is(':visible'))
                $('#popup-close-icon').trigger('click');
            if ($('#recommendCars').is(':visible'))
                $('.popup-close').trigger('click');
            if ($('.bt-content').is(':visible'))
                $('#closeBox').trigger('click');
            if ($('#similarCarPopup').is(':visible'))
                closeSimilarCarPopup();
            if ($('.valuation-popup').is(':visible'))
                $('#valuation-popup-close').trigger('click');
        },
        closeRecommendations: function () {
            if ($('#recommendCars').is(':visible'))
                $('.popup-close').trigger('click');
        },
        getpriceInLakhs: function (numericPrice) {
            var priceInLakhs = parseInt(numericPrice) / 100000;
            return priceInLakhs;
        },
        getkmInThousandsRounded: function (kmValue) {
            var kmInThousands = Math.round(parseInt(kmValue) / 1000);
            return kmInThousands;
        },
        isNumberChanged: function (newMobileNumber) {
            if ($.cookie('TempCurrentUser')) {
                var currentMobileNumber = $.cookie('TempCurrentUser').split(':')[1];
                return currentMobileNumber != newMobileNumber;
            }
            return false;
        },
        otpVariables: {
            sourceModule: 1,                        //For UsedcarLead module
            mobileVerificationByType: {
                otp: 1,
                missedCall: 2,
                otpAndMissedCall: 3
            },
            defaultValidityInMins: 30,
            defaultOtpLength:5
        }
    },
    pageLoad: {
        recommendedCarsLoad: function () {
            D_buyerProcess.recommendedCars.registerEvents();
        },
        sellerDetailsPopupLoad: function () {
            D_buyerProcess.utils.registerEvents();
        },
        cityWarningLoad: function () {
            D_buyerProcess.cityWarning.registerEvents();
        },
        sellerDetailsLoad: function () {
            D_buyerProcess.sellerDetails.registerEvents();
        }
    },
    deliveryText: {
        displayDeliveryText: function (mainElement) {
            var gsdDeliveryTextElement = $('div.gsd-deliveryText');
            var disclaimerTextEle = gsdDeliveryTextElement.find('p.gsd-disclaimertext');
            var listingsDeliveryElement = mainElement.find('.delivery-text');
            var stockCity = mainElement.find('span.cityName').text();
            var gsdCarLocationIcon = gsdDeliveryTextElement.find('div.gsd-car-location-ic');
            gsdDeliveryTextElement.find('span.gsd-carcityname').text(stockCity);
            if (listingsDeliveryElement.length > 0 && listingsDeliveryElement.text().trim()) {
                mainElement.find('div.gsd-deliveryText').show();
                if (disclaimerTextEle.hasClass('hide')) {
                    disclaimerTextEle.removeClass('hide');
                }
                gsdCarLocationIcon.addClass('hide');
                disclaimerTextEle.text(listingsDeliveryElement.text());
            }
            else {
                if (D_buyerProcess.cityWarning.checkGsdCityWarning(mainElement)) {
                    disclaimerTextEle.addClass('hide')
                    if (gsdCarLocationIcon.hasClass('hide')) {
                        gsdCarLocationIcon.removeClass('hide');
                    }
                }
            }
        }
    },
    cityWarning: {
        registerEvents: function () {
            D_buyerProcess.doc.on('click', '.gsd_changeCityLink', function () {
                D_buyerProcess.cityWarning.changeGlobalCity();
            });
        },
        changeGlobalCity: function () {
            D_usedSearch.globalCityArea.changeGlobalAreaFromNoCar();
            D_buyerProcess.utils.closeSellerDetailsPopup();
            $('.blackOut-window-bt').show();
        },
        checkGsdCityWarning: function (element) {
            if ($.showCityWarning || D_buyerProcess.cityWarning.isNearByCity(element) || D_usedSearch.cityWarning.showCityWarningOnSoldOut) {
                element.find('div.gsd-deliveryText').show();
                return true;
            }
            else {
                element.find('div.gsd-deliveryText').hide();
                return false;
            }
        },
        isNearByCity: function (element) {
            if (element.parents('.stock-list').find('#nbCitiesTitle').length > 0) {
                return true;
            }
            return false;
        }
    },
    photoGallery: {
        showPgSellerDetails: function (boxObj, seller) {
            boxObj.find('#pg_contactSellerForm').hide();
            kmNumeric_g = boxObj.attr("kmNumeric");
            priceNumeric_g = boxObj.attr("priceNumeric");
            bodyTypeId_g = boxObj.attr("bodyStyleId");
            versionSubSegment_g = boxObj.attr("versionSubsegmentId");
            makeId = boxObj.attr("makeId");
            rootId = boxObj.attr("rootId");
            profileId_g = boxObj.attr("profileid");
            isDealer_g = (profileId_g.charAt(0) == 'D' || profileId_g.charAt(0) == 'd') ? '1' : '0';
            boxObj.find('#pg_contactSellerForm, #pg-mobile-verification').hide();
            boxObj.find("#pg-seller_name").text(seller.name);

            boxObj.find("#pg-seller_email").text(seller.email).attr('href', 'mailto:' + seller.email);
            boxObj.find("#pg-seller_mobile").text(seller.mobile);
            boxObj.find("#pg-seller_address").text(seller.address);
            D_buyerProcess.sellerDetails.processRatingText(seller.ratingText, boxObj.find("#pg-seller-info .top-rated-seller-tag"), $("#pg-contact_person"));
            if (seller.dealerShowroomPage) {
                $('.seller-virtual-link').attr('href', seller.dealerShowroomPage).show();
            }
            else {
                $(".seller-virtual-link").hide();
            }
            if (isDealer_g == "1") {
                boxObj.find("#pg-contact_person").show().text(seller.contactPerson ? seller.contactPerson : "");
            }
            else {
                boxObj.find("#pg-contact_person").hide();
            }

            boxObj.find("#pg-seller-info").show();
            if (D_buyerProcess.openVerificationPopup.isPopupEligible())
                D_buyerProcess.openVerificationPopup.slidePopup();

            formViewSimilarCar(boxObj);
            Common.utils.trackAction('PhotoGallery_SellerDetailsStage3', 'UsedCarSearch', 'PhotoGallery_SellerDetailsStage3');
        }
    },
    tracking: {
        trovitTracking: function () {
            if (typeof ta !== 'undefined') {
                ta('send', 'lead');
            }
        },
        conversionTracking: function () {
            var label = 'profileId=' + profileId_g + '|buyerMobile=' + buyersMobile + '|ipDetectedCity=' + $.appliedIpDetectedCityId;
            cwTracking.trackCustomDataWithQs(searchPageVariables.getBhriguCategory(), 'Lead', label, D_usedSearch.utils.getQS());
        },
        outbrainTracking: function () {
            obApi('track', 'Used Car Leads');
        },
        adWordTracking: function () {
            if (typeof dataLayer !== 'undefined') {
                dataLayer.push({ event: 'Desktop_UsedCarLeads' });
            }
        }
    }
};

var chatUIProcess = function () {
    var setChatIconVisibilty = function (responseMsg, count) {
        if (responseMsg === chatProcess.chatRegistrationResponse.Success) {
            $('.global-chat-icon').show();
            if (count > 0) {
                $('.global-chat-btn .chat-icon').addClass('global-chat');
                var countText = count > 99 ? '99+' : count;
                $('#chat-count').text(countText);
            }
            else if (count === 0) {
                $('#chat-count').empty();
                $('.global-chat-btn .chat-icon').removeClass('global-chat');
            }
        }
        else {
            $('.global-chat-icon').hide();
        }
    };

    var loader = (function () {
        var _container;
        var show = function ($container) {
            _container = $container;
            $container.find('.js-threedot-loader').hide();
            $container.find('.oxygenLoaderContainer__js').show();
        }
        var hide = function () {
            if (_container) {
                _container.find('.js-threedot-loader').show();
                _container.find('.oxygenLoaderContainer__js').hide();
                _container = undefined;
            }
        }
        return { show: show, hide: hide };
    })();

    return { setChatIconVisibilty: setChatIconVisibilty, loader: loader };
}();

var tracker = (function () {
    var _getInputParamsForTracking = function (originId) {
        var obj = {
            originId: originId,
            qs: D_usedSearch.utils.getQS(),
        };
        var gsdBtn = leadData.getCurrGsdButton();
        if (gsdBtn && gsdBtn.dataset) {
            obj.slotId = gsdBtn.dataset.slotId || 0;
            obj.ctePackageId = gsdBtn.dataset.ctePackageId;
        }
        return obj;
    };
    var trackGsdClick = function (originId) {
        var inputParams = _getInputParamsForTracking(originId);
        if (typeof Common !== 'undefined' && typeof cwUsedTracking !== 'undefined') {
            var eventAction = Common.utils.getCookie("TempCurrentUser")
                ? cwUsedTracking.eventActions.gsdVerifiedText
                : cwUsedTracking.eventActions.gsdUnverifiedText;

            var eventLabel = "originId=" + inputParams.originId + "|slotId=" + inputParams.slotId + "|ctePackageId=" + inputParams.ctePackageId;

            var trackingParams = {
                action: eventAction,
                label: eventLabel,
                qs: inputParams.qs,
            };
            cwUsedTracking.track(trackingParams);
        }
    };
    return {
        trackGsdClick: trackGsdClick,
    };
})();

var leadData = (function () {
    var _currGsdButton;                             //html element
    var setCurrGsdButton = function (element) {
        _currGsdButton = element;
    };
    var getCurrGsdButton = function () {
        return _currGsdButton;
    };
    return {
        setCurrGsdButton: setCurrGsdButton,
        getCurrGsdButton: getCurrGsdButton,
    };
})();