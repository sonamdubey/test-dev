// JavaScript Document

var firstname = $("#getFirstName");
var lastname = $("#getLastName");
var emailid = $("#getEmailID");
var mobile = $("#getMobile");
var otpContainer = $(".mobile-verification-container");

var detailsSubmitBtn = $("#user-details-submit-btn");
var otpText = $("#getOTP");
var otpBtn = $("#otp-submit-btn");

var prevEmail = "";
var prevMobile = "";

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
            hideError(mobile);
            otpText.val('').removeClass("border-red");
            otpText.siblings("span, div").css("display", "none");
        }
        setPQUserCookie();
        var getCityArea = GetGlobalCityArea();
        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Booking_Page', 'act': 'Step_1_Successful_Submit', 'lab': getCityArea });
    }
});

var ValidateUserDetail = function () {
    
    var isValid = true;
    var getCityArea = GetGlobalCityArea();
    
    isValid = validateEmail(getCityArea);
    isValid &= validateMobile(getCityArea);
    isValid &= validateName(getCityArea);
    isValid &= validateLastName(getCityArea);
    if (!isValid) {
        $('#customize-tab').addClass('disabled-tab').removeClass('active-tab  text-bold');
        $('#confirmation-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
    }
    return isValid;
};

var validateName = function (cityArea) {
    var isValid = true;
    var a = firstname.val().length;
    if (firstname.val().indexOf('&') != -1) {
        isValid = false;
        setError(firstname,'Invalid name');
    }
    else if (a == 0) {
        isValid = false;
        setError(firstname,'Please enter your first name');
    }
    else if (a >= 1) {
        isValid = true;
        nameValTrue()
    }
    if (!isValid) { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Booking Page', 'act': 'Step_1_Submit_Error_Name', 'lab': cityArea }); }
    return isValid;
}

var lastnameValTrue = function () {
    hideError(lastname)
    lastname.siblings("div").text('');
};
var nameValTrue = function () {
    hideError(firstname)
    firstname.siblings("div").text('');
};

firstname.on("focus", function () {
    hideError(firstname);
});

emailid.on("focus", function () {
    hideError(emailid);
    prevEmail = emailid.val().trim();
});

mobile.on("focus", function () {
    hideError(mobile)
    prevMobile = mobile.val().trim();
   
});

emailid.on("blur", function () {
    if (prevEmail != emailid.val().trim())
    {
        var getCityArea = GetGlobalCityArea(); 
        if (validateEmail(getCityArea))
        {
            viewModel.CustomerVM().IsVerified(false);
            detailsSubmitBtn.show();
            otpText.val('');
            otpContainer.removeClass("show").addClass("hide");
            hideError(emailid);
        }       
    }
    else
        viewModel.CustomerVM().IsVerified(true);
});

mobile.on("blur", function () {
    if(prevMobile != mobile.val().trim())
    {
        var getCityArea = GetGlobalCityArea();
        if (validateMobile(getCityArea))
        {
            viewModel.CustomerVM().IsVerified(false);
            detailsSubmitBtn.show();
            otpText.val('');
            otpContainer.removeClass("show").addClass("hide");
            hideError(mobile);
        }
        $('#confirmation-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
        $('#customize-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
    }
    else
        viewModel.CustomerVM().IsVerified(true);

});

var mobileValTrue = function () {
    mobile.removeClass("border-red");
    mobile.siblings("span, div").hide();
};

//emailid.change(function () {
//    viewModel.CustomerVM().IsVerified(false);
//});

//mobile.change(function () {
//    viewModel.CustomerVM().IsVerified(false);
//});

otpText.on("focus", function () {
    otpText.val('');
    otpText.siblings("span, div").css("display", "none");
}); 

var setError = function (ele, msg) {
    ele.addClass("border-red");
    ele.siblings("span, div").show();
    ele.siblings("div").text(msg);
}

function hideError(ele){
    ele.removeClass("border-red");
    ele.siblings("span, div").hide();
}
/* Email validation */
function validateEmail(cityArea) {
    var isValid = true;
    var emailID = emailid.val();
    var reEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;

    if (emailID == "") {
        setError(emailid, 'Please enter email address');
        isValid = false;
    }
    else if (!reEmail.test(emailID)) {
        setError(emailid, 'Invalid Email');
        isValid = false;
    }
    if (!isValid) { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Booking Page', 'act': 'Step_1_Submit_Error_Email', 'lab': cityArea }); }
    return isValid;
}

function validateMobile(cityArea) {
    var isValid = true;
    var reMobile = /^[0-9]{10}$/;
    var mobileNo = mobile.val();
    if (mobileNo == "") {
        isValid = false;
        setError(mobile, "Please enter your Mobile Number");
    }
    else if (!reMobile.test(mobileNo) && isValid) {
        isValid = false;
        setError(mobile,"Mobile Number should be 10 digits");
    }
    else {
        hideError(mobile)
    }
    if (!isValid) { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Booking Page', 'act': 'Step_1_Submit_Error_Mobile', 'lab': cityArea }); }
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
    var getCityArea = GetGlobalCityArea();
    isValid = validateEmail(getCityArea);
    isValid &= validateMobile(getCityArea);
    isValid &= validateName(getCityArea);
    isValid &= validateLastName(getCityArea);
    $('#processing').show();
    if (!validateOTP())
        $('#processing').hide();

    if (validateOTP() && isValid) {
        viewModel.CustomerVM().generateOTP();
        var getCityArea = GetGlobalCityArea();
        
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
            $('#processing').hide();

            detailsSubmitBtn.show();
            otpText.val('');
            otpContainer.removeClass("show").addClass("hide");

            // OTP Success
            dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Booking_Page', 'act': 'Step_1_OTP_Successful_Submit', 'lab': getCityArea });
        }
        else {
            $('#processing').hide();
            otpVal("Please enter a valid OTP.");
            // push OTP invalid
            dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Booking Page', 'act': 'Step_1_OTP_Submit_Error', 'lab': getCityArea });
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

$('#btnMakePayment').on('click', function (e) {
    var cityArea = GetGlobalCityArea();
    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Booking Page', 'act': 'Step 3_Pay_Click', 'lab': cityArea });
});


function setPQUserCookie() {
    var val = firstname.val() + '&' + lastname.val() + '&' + emailid.val() + '&' + mobile.val();
    SetCookie("_PQUser", val);
}
//800x600 
if ($(window).width() < 996 && $(window).width() > 790) {
    $(".bike-booking-title").find("p").removeClass("font16").addClass("font14");
    $(".confirm-otp-text").removeClass("font16").addClass("font14");
    $(".bikeModel-details-table table tr td").first().attr("width", "150");
    $(".bikeModel-balance-text").removeClass("font12").addClass("font11");
    $(".bikeModel-dealerMap-container").css("width", "230px");
    $(".finalBalanceAmount").removeClass("font18").addClass("font14");

}
function setuserDetails() {
    var cookieName = "_PQUser";
    if (isCookieExists(cookieName)) {
        var arr = getCookie(cookieName).split("&");
        return arr;
    }
}

var validateLastName = function (cityArea) {
    var isValid = true;
    if (lastname.val().indexOf('&') != -1) {
        isValid = false;
        setError(lastname, 'Invalid name');
    }
    else {
        isValid = true;
        lastnameValTrue();
    }
    return isValid;
}