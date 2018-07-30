var reEmail = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
var reName = /^[a-zA-Z'\- ]+$/;
var re = /^[0-9]*$/
var regPass = /^[a-zA-Z]+$/;
var timer;

$(document).ready(function () {
    if ($(window).innerWidth() > 768 && $(window).innerHeight() < 550) { // for devices with height around 540px
        $('#loginSignupWrapper').css({ 'height': '420px', 'padding': '20px 70px', 'top': '12%' });
    }
    setBackgroundImage();
        
    $("#btnForgetPass").on('click',function () {
        sendPwd();
    });

    $('.login-box-signup-target').on('click', function () {
        clearTimeout(timer);
        showSignUpWindow();
        timer = setTimeout(function () {
            $('.login-box-footer').hide();
        }, 1000);
    });    

    $('.signup-box-back-btn').on('click', function () {
        clearTimeout(timer);
        $('.login-signup-wrapper').removeClass('activate-signup');
        $('.login-box-footer').show();
        timer = setTimeout(function () {
            $('.user-signup-box form').hide();
        }, 1000);
    });

    $('.forgot-password-target').on('click', function () {
        $('.user-login-box, .user-signup-box').hide();
        $('.forgot-password-content').show();
    });

    $('.forgot-password-back-btn').on('click', function () {
        $('.forgot-password-content').hide();
        $('.user-login-box, .user-signup-box').show();
    });


    $("#btnLogin").on('click', function (e) {
        var isSuccess = false;      
        if (ValidateLoginDetails(e)) {
            var source = {
                'Email': email.val().trim(),
                'Password': pass.val().trim(),
                'CreateAuthTicket': true
            };
            
            $.ajax({
                type: "POST",
                url: "/api/customer/authenticate/",
                data: JSON.stringify(source),
                contentType: 'application/json',
                dataType: 'json',
                async: false,
                success: function (response) {                    
                    if (response != null && response.isAuthorized == true) {
                        $("#" + ctrlHdnAuthDataId).val(JSON.stringify(response));
                        isSuccess = true;
                    }
                    else {
                        setError(pass, "Invalid Email or Password");
                    }
                }
            });
        }

        return isSuccess;
    });

    email.on('focus', function () {

        hideError($(this));

    });

    pass.on('focus', function () {

        hideError($(this));

    });


    emailVal.on('focus', function () {

        hideError($(this));

    });

    passVal.on('focus', function () {

        hideError($(this));

    });

    nameVal.on('focus', function () {

        hideError($(this));

    });

    mobileVal.on('focus', function () {
        hideError($(this));
    });

    forgotPass.on('focus', function () {
        hideError($(this));
    });

});

var setBackgroundImage = function () {
    if ($(window).innerWidth() > 768)
        $('body').css({ 'background': '#6e6d71 url(https://imgd.aeplcdn.com/0x0/bw/static/landing-banners/d/login-background-banner.jpg) no-repeat center center', 'background-size' : 'cover' });
        
    else
        $('body').css({ 'background': 'none' });
}

function showSignUpWindow() {
    $('.login-signup-wrapper').addClass('activate-signup');
    $('.user-signup-box form').show();
}

function ValidateLoginDetails(e) {
    var isValid = true;
    emailVal = email.val().trim();
    passVal = pass.val().trim();
    if (!(emailVal.length > 0 && validateEmail(emailVal))) {
        setError(email, 'Enter valid email id');
        isValid = false;
    }
    else if (emailVal.length == 0) {
        setError(email, 'Email should not be empty');
        isValid = false;
    }

    if (!(passVal.length > 0)) {
        setError(pass, 'Password should not be empty');
        isValid = false;
    }

    return isValid;
}

function validateEmail(emailVal) {
    return reEmail.test(emailVal);
}

function setError(ele, msg) {
    ele.addClass("border-red");
    ele.siblings("span, div").show();
    ele.siblings("div").text(msg);
}

function hideError(ele) {
    if (ele != null) {
        ele.removeClass("border-red");
        ele.siblings("span, div").hide();
    }
}
//validation ends here

function pressSignupbutton(e) { 
    if (validateControl()) {
        return false;
    }
    else {
        return true;
    }
};



function validateControl() {

    var nameSignup = nameVal.val();
    var emailSignup = emailVal.val();
    var mobileSignup = mobileVal.val();
    var passSignup = passVal.val();
    var emailValid = false, nameValid = false, mobileValid = false, pwdValid = false;

    var isValid = false;

    if ($.trim(nameSignup) == "") {
        setError(nameVal, 'Required');
        nameValid = false;
    } else if (!reName.test($.trim(nameSignup))) {
        setError(nameVal, 'Name should be Alphabetic.');
        nameValid = false;
    }
    else {
        nameValid = true;
    }



    if (emailSignup.length > 0 && validateEmail(emailSignup)) {
        emailValid = true;
    }
    else {
        setError(emailVal, 'Enter valid email id');
        emailValid = false;
    }


    if ($.trim(mobileSignup) != "") {
        if (!re.test($.trim(mobileSignup).toLowerCase())) {
            setError(mobileVal, 'Mobile No. should be numeric only');
            mobileValid = false;
        } else if (mobileSignup.length < 10) {
            setError(mobileVal, 'Mobile no should be greater than 10 digits');
            mobileValid = false;
        }

        else {
            mobileValid = true;
        }
    }
    else {
        setError(mobileVal, 'Please enter a mobile no.');
        mobileValid = false;
    }

    if (passSignup.length > 5) {
        if (passSignup.length == $.trim(passSignup).replace(" ","").length)
            pwdValid = true;
        else
        {
            setError(passVal, 'Password should not contain blank spaces');
            pwdValid = false;
        }
    }
    else {
        setError(passVal, 'Password should contain atleast 6 characters');
        pwdValid = false;
    }

    isValid = nameValid & emailValid & mobileValid & pwdValid;
    return !isValid;
}

function checkStatus(chk) {
    $('#btnSignup').attr("disabled", chk.checked ? false : true);
}


function getCtrlId(controlId) {
    return document.getElementById('<%=this.ID%>_' + controlId);
}

function sendPwd() {
    var email = forgotPass.val();

    if (reEmail.test(email)) {
        $("#processing_pwd_fp").removeClass("hide");
        setTimeout('requestForgotPwd()', 1000); // prepare loading	
    } else {
        setError(forgotPass, 'Invalid email');
        return false;
    }
}

function requestForgotPwd() {
    var response = "";
    $.ajax({
        type: "POST",
        url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
        data: '{"email":"' + forgotPass.val() + '"}',
        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "SendCustomerPwd"); },
        success: function (response) {
            var responseJSON = eval('(' + response + ')');
            if (responseJSON.value == true) {
                $("#processing_pwd_fp").html("The link to reset your password has been sent on your email id.");
            }
            else if (responseJSON.value == false) {
                $("#processing_pwd_fp").html("<span class='readmore'>This email id is not registered with us.");
            }
        }
    });
}

// nav bar code starts here
$(".navbarBtn").on('click', function () {
    var navBtn = $(this);

    if (!navBtn.hasClass('bwm-navbarBtn')) {
        navbarShow();
    }
    else {
        navDrawer.open(); // mobile nav
        appendState('filter');
    }
});

$(".blackOut-window").mouseup(function (event) {
    var blackOutWindow = $(this);

    if (!blackOutWindow.hasClass('bwm-blackOut-window')) {
        navbarHide();
    }
    else {
        if (event.target.id !== navContainer.attr('id') && !navContainer.has(event.target).length) {
            history.back(); // mobile nav
            navDrawer.close();
        }
    }
});

$(".navUL > li > a").on('click', function () {
    if (!$(this).hasClass("open")) {
        var a = $(".navUL li a");
        a.removeClass("open").next("ul").slideUp(350);
        $(this).addClass("open").next("ul").slideDown(350);

        if ($(this).siblings().size() === 0) {
            navbarHide();
        }

        $(".nestedUL > li > a").click(function () {
            $(".nestedUL li a").removeClass("open");
            $(this).addClass("open");
            navbarHide();
        });

    }
    else if ($(this).hasClass("open")) {
        $(this).removeClass("open").next("ul").slideUp(350);
    }
});

$(document).keydown(function (e) {
    if (e.keyCode == 27) {
        navbarHide();
    }
});

function navbarShow() {
    $("#nav").addClass('open').animate({ 'left': '0px' });
    $(".blackOut-window").show();
}

function navbarHide() {
    $("#nav").removeClass('open').animate({ 'left': '-350px' });
    $(".blackOut-window").hide();
}
// nav bar code ends here

// mobile nav bar
var navContainer = $("#nav"),
    effect = 'slide',
    directionLeft = { direction: 'left' },
    duration = 500;

var navDrawer = {
    open: function () {
        navContainer.show(effect, directionLeft, duration, function () {
            navContainer.addClass('drawer-active');
        });
        lockPopup();
    },

    close: function () {
        navContainer.removeClass('drawer-active');
        navContainer.hide(effect, directionLeft, duration, function () { });
        unlockPopup();
    }
};

var appendState = function (state) {
    window.history.pushState(state, '', '');
};

$(window).on('popstate', function (event) {
    if ($('#nav').is(':visible')) {
        navDrawer.close();
    }
});

function lockPopup() {
    $('body').addClass('lock-browser-scroll');
    $(".blackOut-window").show();
}
function unlockPopup() {
    $('body').removeClass('lock-browser-scroll');
    $(".blackOut-window").hide();
}