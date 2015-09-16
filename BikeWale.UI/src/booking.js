// JavaScript Document

var firstname = $("#getFirstName");
var lastname = $("#getLastName");
var emailid = $("#getEmailID");
var mobile = $("#getMobile");
var otpContainer = $(".mobile-verification-container");

var detailsSubmitBtn = $("#user-details-submit-btn");
var otpText = $("#getOTP");
var otpBtn = $("#otp-submit-btn");


detailsSubmitBtn.click(function () {
    if (ValidateUserDetail()) {
        viewModel.CustomerVM().verifyCustomer();
        if (viewModel.CustomerVM().IsValid()) {
            $.customizeState();
            $("#personalInfo").hide();
            $("#personal-info-tab").removeClass('text-bold');
            $("#customize").show();
            $('#customize-tab').addClass('text-bold');
            $('#customize-tab').addClass('active-tab').removeClass('disabled-tab');
            $('#confirmation-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
            $(".booking-dealer-details").removeClass("hide").addClass("show");
            $(".call-for-queries").hide();
        }
        else {
            otpContainer.removeClass("hide").addClass("show");
            $(this).hide();
            nameValTrue();
            mobileValTrue();
        }
    }
});

var ValidateUserDetail = function () {
    var isValid = true;
    isValid = validateEmail();
    isValid &= validateMobile();
    isValid &= validateName();
    if (!isValid) {
        $('#customize-tab').addClass('disabled-tab').removeClass('active-tab  text-bold');
        $('#confirmation-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
    }
    return isValid;
};

var validateName = function () {
    var isValid = true;
    var a = firstname.val().length;
    if (a == 0) {
        isValid = false;
        fnameVal();
    }
    else if (a >= 1) {
        isValid = true;
        nameValTrue()
    }
    return isValid;
}

var nameValTrue = function () {
    firstname.removeClass("border-red");
    firstname.siblings("span, div").hide();
};

firstname.on("focus", function () {
    emailid.removeClass("border-red");
    emailid.siblings("span, div").hide();
});

emailid.on("focus keyup", function () {
    emailid.removeClass("border-red");
    emailid.siblings("span, div").hide();
    detailsSubmitBtn.show();
    otpText.val('');
    otpContainer.removeClass("show").addClass("hide");
});

var mobileValTrue = function () {
    mobile.removeClass("border-red");
    mobile.siblings("span, div").hide();
};

mobile.change(function () {
    viewModel.CustomerVM().IsVerified(false);
});

otpText.on("focus", function () {
    otpText.val('');
    otpText.siblings("span, div").css("display", "none");
});

mobile.on("keyup focus", function () {
    detailsSubmitBtn.show();
    otpText.val('');
    otpContainer.removeClass("show").addClass("hide");
})

emailid.change(function () {
    viewModel.CustomerVM().IsVerified(false);
});

var fnameVal = function () {
    firstname.addClass("border-red");
    firstname.siblings("span, div").css("display", "block");
};

var emailVal = function (msg) {
    emailid.addClass("border-red");
    emailid.siblings("span, div").css("display", "block")
    emailid.siblings("div").text(msg);
};

var mobileVal = function (msg) {
    mobile.addClass("border-red");
    mobile.siblings("span, div").css("display", "block");
    mobile.siblings("div").text(msg);
};

/* Email validation */
function validateEmail() {
    var isValid = true;
    var emailID = emailid.val();
    var reEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;

    if (emailID == "") {
        emailVal('Please enter email address');
        isValid = false;
    }
    else if (!reEmail.test(emailID)) {
        emailVal('Invalid Email');
        isValid = false;
    }
    return isValid;
}

function validateMobile() {
    var isValid = true;
    var reMobile = /^[0-9]{10}$/;
    var mobileNo = mobile.val();
    if (mobileNo == "") {
        isValid = false;
        mobileVal("Please enter your Mobile Number");
    }
    if (!reMobile.test(mobileNo) && isValid) {
        isValid = false;
        mobileVal("Mobile Number should be 10 digits");
    }
    return isValid;
}

var otpVal = function (msg) {
    otpText.addClass("border-red");
    otpText.siblings("span, div").css("display", "block");
    otpText.siblings("div").text(msg);
};

function validateOTP() {
    var retVal = true;
    var isNumber = /^[0-9]{5}$/;
    var cwiCode = otpText.val();
    viewModel.CustomerVM().IsVerified(false);
    if (cwiCode == "") {
        retVal = false;
        otpVal("Please enter your Verification Code");
    }
    else {
        if (isNaN(cwiCode)) {
            retVal = false;
            otpVal("Verification Code should be numeric");
        }
        else if (cwiCode.length != 5) {
            retVal = false;
            otpVal("Verification Code should be of 5 digits");
        }
    }
    return retVal;

}

otpBtn.click(function () {
    var isValid = true;
    isValid = validateEmail();
    isValid &= validateMobile();
    isValid &= validateName();
    if (validateOTP() && isValid) {
        otpBtn.text("Processing...");
        viewModel.CustomerVM().generateOTP();
        if (viewModel.CustomerVM().IsVerified()) {
            $.customizeState();
            $("#personalInfo").hide();
            $("#personal-info-tab").removeClass('text-bold');
            $("#customize").show();
            $('#customize-tab').addClass('text-bold');
            $('#customize-tab').addClass('active-tab').removeClass('disabled-tab');
            $('#confirmation-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
            $(".booking-dealer-details").removeClass("hide").addClass("show");
            $(".call-for-queries").hide();
        }
        else {
            otpBtn.text("Confirm OTP");
            otpVal("Please enter a valid OTP.");
        }
    }
});

$(".customize-submit-btn").click(function (e) {
    var a = varientSelection();
    if (a == true) {
        $.confirmationState();
        $("#customize").hide();
        $("#customize-tab").removeClass('text-bold');
        $("#confirmation").show();
        $('#confirmation-tab').addClass('active-tab text-bold').removeClass('disabled-tab');
    }
    else {
        $(".varient-heading-text").addClass("text-orange");
    }
});

$("#personal-info-tab").click(function () {
    if (!$(this).hasClass('disabled-tab')) {
        $.personalInfoState();
        $.showCurrentTab('personalInfo');
        $('#personal-info-tab').addClass('active-tab text-bold');
        $('#confirmation-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
        $('#customize-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
    }
});

$(".customizeBackBtn").click(function () {
        $.personalInfoState();
        $.showCurrentTab('personalInfo');
        $('#personal-info-tab').addClass('active-tab text-bold');
        $('#confirmation-tab').addClass('active-tab').removeClass('disabled-tab text-bold');
        $('#customize-tab').addClass('active-tab').removeClass('text-bold');
});

$('#customize-tab, .confirmationBackBtn').click(function () {    
    if (!$(this).hasClass('disabled-tab')) {
        $.customizeState();
        $.showCurrentTab('customize');
        $('#customize-tab').addClass('active-tab text-bold');
        $('#confirmation-tab').addClass('active-tab').removeClass('disabled-tab text-bold');
        $('#personal-info-tab').addClass('active-tab').removeClass('disabled-tab text-bold');
    }
});

$("#confirmation-tab").click(function () {
    if (!$(this).hasClass('disabled-tab')) {
        $.confirmationState();
        $.showCurrentTab('confirmation');
        $('#confirmation-tab').addClass('active-tab text-bold');
        $('#customize-tab').addClass('active-tab').removeClass('disabled-tab text-bold');
        $('#personal-info-tab').addClass('active-tab').removeClass('disabled-tab text-bold');
    }
});

$.showCurrentTab = function (tabType) {
    $('#personalInfo,#customize,#confirmation').hide();
    $('#' + tabType).show();
};

$.personalInfoState = function () {
    var container = $('.bike-to-buy-tabs ul');
    container.find('li:eq(0)').find('span.buy-icon').attr('class', '').attr('class', 'booking-sprite buy-icon personalInfo-icon-selected');
    container.find('li:eq(1)').find('span.buy-icon').attr('class', '').attr('class', 'booking-sprite buy-icon customize-icon-grey');
    container.find('li:eq(2)').find('span.buy-icon').attr('class', '').attr('class', 'booking-sprite buy-icon confirmation-icon-grey');
};

$.customizeState = function () {
    var container = $('.bike-to-buy-tabs ul');
    container.find('li:eq(0)').find('span.buy-icon').attr('class', '').attr('class', 'booking-sprite buy-icon booking-tick-blue');
    container.find('li:eq(1)').find('span.buy-icon').attr('class', '').attr('class', 'booking-sprite buy-icon customize-icon-selected');
    container.find('li:eq(2)').find('span.buy-icon').attr('class', '').attr('class', 'booking-sprite buy-icon confirmation-icon-grey');
    container.find('li').each(function () {
        if ($(this).find('div.bike-buy-part').attr('data-type-tab') == 'preference')
            $(this).find('div.bike-buy-part').removeClass('active-tab').addClass('disabled-tab');
        else
            $(this).find('div.car-buy-part').addClass('active-tab').removeClass('disabled-tab');
    });
};

$.confirmationState = function () {
    var container = $('.bike-to-buy-tabs ul');
    container.find('li:eq(0)').find('span.buy-icon').attr('class', '').attr('class', 'booking-sprite buy-icon booking-tick-blue');
    container.find('li:eq(1)').find('span.buy-icon').attr('class', '').attr('class', 'booking-sprite buy-icon booking-tick-blue');
    container.find('li:eq(2)').find('span.buy-icon').attr('class', '').attr('class', 'booking-sprite buy-icon confirmation-icon-selected');
    container.find('li').each(function () {
        $(this).find('div.bike-buy-part').addClass('active-tab').removeClass('disabled-tab');
    });
};

$(".varient-item").click(function () {
    $(".varient-item").removeClass("border-dark selected");
    $(this).addClass("border-dark selected");
    $(".varient-heading-text").removeClass("text-orange");
});


$('booking-available-colors .booking-color-box').click(function (e) {
    if (!$(this).find('span.ticked').hasClass("selected")) {
        $('.booking-color-box').find('span.ticked').hide();
        $('.booking-color-box').find('span.ticked').removeClass("selected");
        $(this).find('span.ticked').show();
        $(this).find('span.ticked').addClass("selected");
    }
    else {
        $(this).find('span.ticked').hide();
        $(this).find('span.ticked').removeClass("selected");
    }
});


var varientSelection = function () {
    var total = viewModel.SelectedVarient();
    if (total) {
        return true;
    }
    else {
        return false;
    }
}

function _selectVarient() {
    $(".varient-item").removeClass("border-dark selected");
    $(this).addClass("border-dark selected");
    $(".varient-heading-text").removeClass("text-orange");
}




