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
    var a = validateEmail();
    var b = validateMobile();
    var c = validateName();
    if (c == false) {
        fnameVal();
    }
    else {
        if (a == false) {
            emailVal();
        }
        else {
            if (b == false) {
                mobileVal();
            }
        }
        if (a == true && b == true && c == true) {            
            if (!viewModel.CustomerVM().IsVerified()) {
                viewModel.CustomerVM().verifyCustomer();
                otpContainer.removeClass("hide").addClass("show");
                if (viewModel.CustomerVM().IsVerified()) {
                    
                    return;
                }
                $(this).hide();
                nameValTrue();
                mobileValTrue();
            }
            else {
                if (viewModel.CustomerVM().IsVerified()) {
                    otpBtn.trigger("click");
                }                
            }
        }
    }
});

var validateName = function () {
    var a = firstname.val().length;
    if (a == 0)
        return false;
    else if (a >= 1)
        return true;
}

var nameValTrue = function () {
    firstname.removeClass("border-red");
    firstname.siblings("span, div").hide();
};

firstname.on("focus", function () {
    firstname.removeClass("border-red");
    firstname.siblings("span, div").hide();
});

emailid.on("focus", function () {
    emailid.removeClass("border-red");
    emailid.siblings("span, div").hide();
});

var mobileValTrue = function () {
    mobile.removeClass("border-red");
    mobile.siblings("span, div").hide();
};

mobile.on("blur", function () {
    var b = validateMobile();
    if (b == false) {
        mobileVal();
        otpContainer.removeClass("show").addClass("hide");
        detailsSubmitBtn.show();        
    }
    else if (b == true) {
        mobileValTrue();
        otpContainer.removeClass("hide").addClass("show");
        detailsSubmitBtn.hide();
        detailsSubmitBtn.trigger("click");
    }
});



var fnameVal = function () {
    firstname.addClass("border-red");
    firstname.siblings("span, div").css("display", "block");
};

var emailVal = function () {
    emailid.addClass("border-red");
    emailid.siblings("span, div").css("display", "block");
};

var mobileVal = function () {
    mobile.addClass("border-red");
    mobile.siblings("span, div").css("display", "block");
};



/* Email validation */

function validateEmail() {
    var emailID = emailid.val();
    atpos = emailID.indexOf("@");
    dotpos = emailID.lastIndexOf(".");

    if (atpos < 1 || (dotpos - atpos < 2)) {
        emailVal();
        return false;
    }
    return true;
}

function validateMobile() {
    var a = mobile.val().length;
    if (a < 10)
        return false;
    else
        return true;
}

var otpVal = function () {
    otpText.addClass("border-red");
    otpText.siblings("span, div").css("display", "block");
};

otpBtn.click(function () {
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
        viewModel.CustomerVM().generateOTP();
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

$("#personal-info-tab, .customizeBackBtn").click(function () {    
    if (!$(this).hasClass('disabled-tab')) {
        $.personalInfoState();
        $.showCurrentTab('personalInfo');
        $('#personal-info-tab').addClass('active-tab text-bold');
        $('#confirmation-tab').addClass('active-tab').removeClass('disabled-tab text-bold');
        $('#customize-tab').addClass('active-tab').removeClass('text-bold');
    }
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
    else{
        return false;
    }
}

function _selectVarient() {
    $(".varient-item").removeClass("border-dark selected");
    $(this).addClass("border-dark selected");
    $(".varient-heading-text").removeClass("text-orange");
}




