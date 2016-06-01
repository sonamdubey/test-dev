<%@ Control Language="C#" AutoEventWireup="false"  Inherits="Bikewale.controls.LeadCaptureControl" %>

<style>
    #leadCapturePopup{display:none;width:450px;min-height:470px;background:#fff;margin:0 auto;position:fixed;top:10%;right:5%;left:5%;z-index:10;padding:30px 40px}
.personal-info-form-container{margin:10px auto;width:300px;min-height:100px}
.personal-info-form-container .personal-info-list{margin:0 auto;width:280px;float:left;margin-bottom:20px;border-radius:0}
.personal-info-list .errorIcon,.personal-info-list .errorText{display:none}
#otpPopup{display:none}
#otpPopup .otp-box,#otpPopup .update-mobile-box{width:280px;margin:15px auto 0}
#otpPopup .update-mobile-box .form-control-box{margin-top:25px;margin-bottom:50px}
#otpPopup .otp-box p.resend-otp-btn{color:#0288d1;cursor:pointer;font-size:14px}
#otpPopup .update-mobile-box{display:none}
#otpPopup .edit-mobile-btn,.resend-otp-btn{cursor:pointer}
.icon-outer-container{width:102px;height:102px;margin:0 auto;background:#fff;border:1px solid #ccc}
.icon-inner-container{width:92px;height:92px;margin:4px auto;background:#fff;border:1px solid #666}
.user-contact-details-icon{width:36px;height:44px;background-position:0 -391px}
.otp-icon{width:30px;height:40px;background-position:-46px -391px}
.mobile-prefix{position:absolute;padding:10px 13px 13px;color:#999}
#otpPopup .errorIcon,#otpPopup .errorText{display:none}
.input-border-bottom{border-bottom:1px solid #ccc}
.assistance-response-close.cross-lg-lgt-grey:hover { background-position: -36px -252px; }
</style>
<!-- lead capture popup start-->
<div id="leadCapturePopup" class="text-center rounded-corner2">
    <div class="leadCapture-close-btn position-abt pos-top10 pos-right10 bwsprite cross-lg-lgt-grey cur-pointer"></div>
    <!-- contact details starts here -->
    <div id="contactDetailsPopup">
        <div class="icon-outer-container rounded-corner50">
            <div class="icon-inner-container rounded-corner50">
                <span class="bwsprite user-contact-details-icon margin-top25"></span>
            </div>
        </div>
        <p class="font20 margin-top25 margin-bottom10">Provide contact details</p>
        <p class="text-light-grey margin-bottom20">Dealership will get back to you with offers, EMI quotes, exchange benefits and much more!</p>
        <div class="personal-info-form-container">
            <div class="form-control-box personal-info-list">
                <input type="text" class="form-control get-first-name" placeholder="Full name (mandatory)"
                    id="getFullName" data-bind="textInput: fullName">
                <span class="bwsprite error-icon errorIcon"></span>
                <div class="bw-blackbg-tooltip errorText">Please enter your first name</div>
            </div>
            <div class="form-control-box personal-info-list">
                <input type="text" class="form-control get-email-id" placeholder="Email address (mandatory)"
                    id="getEmailID" data-bind="textInput: emailId">
                <span class="bwsprite error-icon errorIcon"></span>
                <div class="bw-blackbg-tooltip errorText">Please enter email address</div>
            </div>
            <div class="form-control-box personal-info-list">
                <p class="mobile-prefix">+91</p>
                <input type="text" class="form-control padding-left40 get-mobile-no" placeholder="Mobile no. (mandatory)"
                    id="getMobile" maxlength="10" data-bind="textInput: mobileNo">
                <span class="bwsprite error-icon errorIcon"></span>
                <div class="bw-blackbg-tooltip errorText">Please enter mobile number</div>
            </div>
            <div class="clear"></div>
            <a class="btn btn-orange margin-top10" id="user-details-submit-btn" data-bind="event: { click: submitLead }">Submit</a>
        </div>
    </div>
    <!-- contact details ends here -->
    <!-- otp starts here -->
    <div id="otpPopup">
        <div class="icon-outer-container rounded-corner50">
            <div class="icon-inner-container rounded-corner50">
                <span class="bwsprite otp-icon margin-top25"></span>
            </div>
        </div>
        <p class="font18 margin-top25 margin-bottom20">Verify your mobile number</p>
        <p class="font14 text-light-grey margin-bottom20">We have sent an OTP on the following mobile number. Please enter that OTP in the box provided below:</p>
        <div>
            <div class="lead-mobile-box lead-otp-box-container font22">
                <span class="fa fa-phone"></span>
                <span class="text-light-grey font24">+91</span>
                <span class="lead-mobile font24"></span>
                <span class="bwsprite edit-blue-icon edit-mobile-btn"></span>
            </div>
            <div class="otp-box lead-otp-box-container">
                <div class="form-control-box margin-bottom10">
                    <input type="text" class="form-control" maxlength="5" placeholder="Enter your OTP" id="getOTP" data-bind="value: otpCode">
                    <span class="bwsprite error-icon errorIcon"></span>
                    <div class="bw-blackbg-tooltip errorText"></div>
                </div>
                <a class="resend-otp-btn margin-left10 blue rightfloat resend-otp-btn" id="resendCwiCode" data-bind="visible: (NoOfAttempts() < 2), click: function () { regenerateOTP() }">Resend OTP
                </a>
                <p class="otp-alert-text margin-left10 otp-notify-text text-light-grey font12 margin-top10" data-bind="visible: (NoOfAttempts() >= 2)">
                    OTP has been already sent to your mobile
                </p>
                <div class="clear"></div>
                <input type="button" class="btn btn-orange margin-top20" value="Confirm OTP" id="otp-submit-btn">
            </div>
            <div class="update-mobile-box">
                <div class="form-control-box text-left">
                    <p class="mobile-prefix">+91</p>
                    <input type="text" class="form-control padding-left40" placeholder="Mobile no." maxlength="10" id="getUpdatedMobile" data-bind="value: mobileNo" />
                    <span class="bwsprite error-icon errorIcon"></span>
                    <div class="bw-blackbg-tooltip errorText"></div>
                </div>
                <input type="button" class="btn btn-orange" value="Send OTP" id="generateNewOTP" data-bind="event: { click: submitLead }" />
            </div>
        </div>
    </div>
    <!-- otp ends here -->
    <div id="dealer-lead-msg" class="hide">
        <div class="icon-outer-container rounded-corner50">
            <div class="icon-inner-container rounded-corner50">
                <span class="bwsprite otp-icon margin-top25"></span>
            </div>
        </div>
        <p class="font18 margin-top25 margin-bottom20">Thank you for providing your details. <%--<%= dealerName %>, <%= dealerArea %>--%> will get in touch with you soon.</p>

        <a href="javascript:void(0)" class="btn btn-orange okay-thanks-msg">Okay</a>
    </div>
</div>
<!-- lead capture popup End-->

<!-- scripts goes here -->
<script type="text/javascript">

    var leadBtnBookNow = $(".leadcapturebtn"), leadCapturePopup = $("#leadCapturePopup");
    var fullName = $("#getFullName");
    var emailid = $("#getEmailID");
    var mobile = $("#getMobile");
    var otpContainer = $(".mobile-verification-container");
    var detailsSubmitBtn = $("#user-details-submit-btn, #buyingAssistBtn");
    var otpText = $("#getOTP");
    var otpBtn = $("#otp-submit-btn");
    var prevEmail = "";
    var prevMobile = "";
    //var getCityArea = GetGlobalCityArea();
    var customerViewModel = new CustomerModel();



    $(function () {
        leadBtnBookNow.on('click', function () {
            leadSourceId = $(this).attr('leadSourceId');
            leadCapturePopup.show();
            $("#dealer-lead-msg").hide();
            $("div#contactDetailsPopup").show();
            $("#otpPopup").hide();
            $('body').addClass('lock-browser-scroll');
            $(".blackOut-window").show();

        });
        $(".leadCapture-close-btn, .blackOut-window").on("click mouseup", function () {
            leadCapturePopup.hide();
            $('body').removeClass('lock-browser-scroll');
            $(".blackOut-window").hide();
        });

        $(document).on('keydown', function (e) {
            if (e.keyCode === 27) {
                $("#leadCapturePopup .leadCapture-close-btn").click();
                $("div.termsPopUpCloseBtn").click();
            }
        });
        //$("#leadBtn").on('click', function () {
        //    dataLayer.push({ "event": "Bikewale_all", "cat": "Dealer_PQ", "act": "Get_Offers_Clicked", "lab": bikeName + "_" + versionName + "_" + getCityArea });
        //    getOfferClick = true;
        //    getEMIClick = false;
        //});

        //$("#btnEmiQuote").on('click', function () {
        //    dataLayer.push({ "event": "Bikewale_all", "cat": "Dealer_PQ", "act": "Get_EMI_Quote_Clicked", "lab": bikeName + "_" + versionName + "_" + getCityArea });
        //    getEMIClick = true;
        //    getOfferClick = false;
        //});

        //$("#ulVersions li input").on('click', function () {
        //    versionName = $(this).attr("value");
        //    dataLayer.push({ "event": "Bikewale_all", "cat": "Dealer_PQ", "act": "Version_Changed", "lab": bikeName + "_" + versionName + "_" + getCityArea });
        //});
    });

    //if ($('#dealerAssistance').length > 0)
        ko.applyBindings(customerViewModel, $('#dealerAssistance')[0]);

    function CustomerModel() {
        var arr = setuserDetails();
        var self = this;
        if (arr != null && arr.length > 0) {
            self.fullName = ko.observable(arr[0]);
            self.emailId = ko.observable(arr[1]);
            self.mobileNo = ko.observable(arr[2]);
        }
        else {
            self.fullName = ko.observable();
            self.emailId = ko.observable();
            self.mobileNo = ko.observable();
        }
        self.IsVerified = ko.observable(false);
        self.NoOfAttempts = ko.observable(0);
        self.IsValid = ko.computed(function () { return self.IsVerified(); }, this);
        self.otpCode = ko.observable();
        self.isAssist = ko.observable(false);
        self.pqId = ko.observable(0);
        self.modelId = ko.observable(0);
        self.dealerId = ko.observable(0);
        self.versionId = ko.observable(0);

        self.generatePQ = function (data, event) {
            self.IsVerified(false);
            isSuccess = false;
            isValidDetails = false;

            isValidDetails = ValidateUserDetail(fullName, emailid, mobile);

            getBikeData(self);


            if (isValidDetails && self.modelId() && self.versionId()) {
                var url = '/api/RegisterPQ/';
                var objData = {
                    "dealerId": self.dealerId(),
                    "modelId": self.modelId(),
                    "clientIP": clientIP,
                    "pageUrl": pageUrl,
                    "versionId": self.versionId(),
                    "cityId": bikeCityId,
                    "areaId": 0,
                    "sourceType": pageSrcId,
                    "pQLeadId": pqSourceId,
                    "deviceId": getCookie('BWC')
                }
                $.ajax({
                    type: "POST",
                    url: url,
                    async: false,
                    data: ko.toJSON(objData),
                    contentType: "application/json",
                    dataType: 'json',
                    success: function (response) {
                        self.pqId(response.quoteId);
                        isSuccess = true;
                    },
                    complete: function (xhr) {
                        if (xhr.status == 404 || xhr.status == 204) {
                            self.IsVerified(false);
                            isSuccess = false;
                            if (self.isAssist()) {
                                stopLoading($("#buyingAssistanceForm"));
                            }
                            else {
                                stopLoading($("#user-details-submit-btn").parent());
                            }
                        }

                    }
                });
            }

            return isSuccess;

        }

        self.verifyCustomer = function () {

            if (!self.IsVerified()) {
                var objCust = {
                    "dealerId": dealerId,
                    "pqId": pqId,
                    "customerName": self.fullName(),
                    "customerMobile": self.mobileNo(),
                    "customerEmail": self.emailId(),
                    "clientIP": clientIP,
                    "pageUrl": pageUrl,
                    "versionId": versionId,
                    "cityId": cityId,
                    "leadSourceId": leadSourceId,
                    "deviceId": getCookie('BWC')
                }
                $.ajax({
                    type: "POST",
                    url: "/api/PQCustomerDetail/",
                    data: ko.toJSON(objCust),
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader('utma', getCookie('__utma'));
                        xhr.setRequestHeader('utmz', getCookie('__utmz'));
                    },
                    async: false,
                    contentType: "application/json",
                    dataType: 'json',
                    success: function (response) {
                        var obj = ko.toJS(response);
                        self.IsVerified(obj.isSuccess);
                        if (!self.IsVerified()) {
                            self.NoOfAttempts(obj.noOfAttempts);
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        self.IsVerified(false);
                    }
                });
            }
        };


        self.generateOTP = function () {
            if (!self.IsVerified()) {
                var objCust = {
                    "pqId": pqId,
                    "customerMobile": self.mobileNo(),
                    "customerEmail": self.emailId(),
                    "cwiCode": self.otpCode(),
                    "branchId": dealerId,
                    "customerName": self.fullName(),
                    "versionId": versionId,
                    "cityId": cityId
                }
                $.ajax({
                    type: "POST",
                    url: "/api/PQMobileVerification/",
                    data: ko.toJSON(objCust),
                    async: false,
                    contentType: "application/json",
                    dataType: 'json',
                    success: function (response) {
                        var obj = ko.toJS(response);
                        self.IsVerified(obj.isSuccess);
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        self.IsVerified(false);
                    }
                });
            }
        };

        self.regenerateOTP = function () {
            if (self.NoOfAttempts() <= 2 && !self.IsVerified()) {
                var url = '/api/ResendVerificationCode/';
                var objCustomer = {
                    "customerName": self.fullName(),
                    "customerMobile": self.mobileNo(),
                    "customerEmail": self.emailId(),
                    "source": 1
                }
                $.ajax({
                    type: "POST",
                    url: url,
                    async: false,
                    data: ko.toJSON(objCustomer),
                    contentType: "application/json",
                    dataType: 'json',
                    success: function (response) {
                        self.IsVerified(false);
                        self.NoOfAttempts(response.noOfAttempts);
                        alert("You will receive the new OTP via SMS shortly.");
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        self.IsVerified(false);
                    }
                });
            }
        };

        self.submitLead = function (data, event) {
            $("#dealer-lead-msg").hide();
            self.IsVerified(false);
            isValidDetails = false;
            var btnId = event.target.id;
            if (btnId == 'buyingAssistBtn') {
                leadSourceId = $("#" + event.target.id).attr("leadSourceId");
                self.isAssist(true);
                isValidDetails = validateUserInfo(assistanceGetName, assistanceGetEmail, assistanceGetMobile);
            }
            else {
                isValidDetails = ValidateUserDetail(fullName, emailid, mobile);
                self.isAssist(false);
            }
            if (isValidDetails) {
                self.verifyCustomer();
                if (self.IsVerified()) {
                    if (self.isAssist()) {
                        $("#buying-assistance-form").hide();
                        $("#dealer-assist-msg").fadeIn();

                    } else {
                        $("#contactDetailsPopup").hide();
                        $("#personalInfo").hide()
                        $("#otpPopup").hide();
                        $("#dealer-lead-msg").fadeIn();
                    }

                    //if (getOfferClick) {
                    //    dataLayer.push({ "event": "Bikewale_all", "cat": "Dealer_PQ", "act": "Lead_Submitted", "lab": "Main_Form_" + bikeName + "_" + versionName + "_" + getCityArea });
                    //    getOfferClick = false;
                    //}
                    //else if (btnId == 'buyingAssistBtn') {
                    //    dataLayer.push({ "event": "Bikewale_all", "cat": "Dealer_PQ", "act": "Lead_Submitted", "lab": "Open_Form_" + bikeName + "_" + versionName + "_" + getCityArea });
                    //}
                    //else if (getEMIClick) {
                    //    dataLayer.push({ "event": "Bikewale_all", "cat": "Dealer_PQ", "act": "Lead_Submitted", "lab": "Get_EMI_" + bikeName + "_" + versionName + "_" + getCityArea });
                    //    getEMIClick = false;
                    //}
                    //else if (getMoreDetailsClick) {
                    //    dataLayer.push({ "event": "Bikewale_all", "cat": "Dealer_PQ", "act": "Lead_Submitted", "lab": "Get_more_details_" + GetBikeVerLoc() });
                    //    getMoreDetailsClick = false;
                    //}
                }
                else {
                    $("#leadCapturePopup").show();
                    $('body').addClass('lock-browser-scroll');
                    $(".blackOut-window").show();
                    $("#contactDetailsPopup").hide();
                    $("#otpPopup").show();
                    var leadMobileVal = mobile.val();
                    $("#otpPopup .lead-mobile-box").find("span.lead-mobile").text(leadMobileVal);
                    otpContainer.removeClass("hide").addClass("show");
                    //nameValTrue();
                    hideError(mobile);
                    otpText.val('').removeClass("border-red").siblings("span, div").hide();
                }
                setPQUserCookie();
            }
        };

        otpBtn.on("click", function (event) {
            $('#processing').show();
            isValidDetails = false;
            if (!validateOTP())
                $('#processing').hide();

            if (self.isAssist() == true) {
                isValidDetails = validateUserInfo(assistanceGetName, assistanceGetEmail, assistanceGetMobile);
            }
            else {
                isValidDetails = ValidateUserDetail(fullName, emailid, mobile);
            }
            if (validateOTP() && isValidDetails) {
                customerViewModel.generateOTP();
                if (customerViewModel.IsVerified()) {
                    $("#personalInfo").hide();
                    otpText.val('');
                    otpContainer.removeClass("show").addClass("hide");
                    $("#personalInfo").hide()
                    $("#otpPopup").hide();

                    if (self.isAssist()) {
                        $("#leadCapturePopup .leadCapture-close-btn").click();
                        $("#buying-assistance-form").hide();
                        $("#dealer-assist-msg").fadeIn();
                    }
                    else {
                        $("#dealer-lead-msg").fadeIn();
                    }

                    // OTP Success
                    if (getMoreDetailsClick) {
                        dataLayer.push({ "event": "Bikewale_all", "cat": "Dealer_PQ", "act": "Lead_Submitted", "lab": "Get_more_details_" + GetBikeVerLoc() });
                        getMoreDetailsClick = false;
                    }
                }
                else {
                    $('#processing').hide();
                    otpVal("Please enter a valid OTP.");
                }
            }
        });
    }

    function ValidateUserDetail(fullName, emailid, mobile) {
        return validateUserInfo(fullName, emailid, mobile);
    };


    var prevEmail = "",
        prevMobile = "";

    $("#assistanceGetName,#getFullName").on("focus", function () {
        hideError($(this));
    });

    $("#assistanceGetEmail,#getEmailID").on("focus", function () {
        hideError($(this));
        prevEmail = $(this).val().trim();
    });

    $("#assistanceGetMobile,#getMobile,#getUpdatedMobile").on("focus", function () {
        hideError($(this));
        prevMobile = $(this).val().trim();
    });



    $("#assistanceGetEmail,#getEmailID").on("blur", function () {
        if (prevEmail != $(this).val().trim()) {
            if (validateEmailId($(this))) {
                customerViewModel.IsVerified(false);
                otpText.val('');
                otpContainer.removeClass("show").addClass("hide");
                hideError($(this));
            }
        }
    });

    $("#assistanceGetMobile,#getMobile,#getUpdatedMobile").on("blur", function () {
        if (prevMobile != $(this).val().trim()) {
            if (validateMobileNo($(this))) {
                customerViewModel.IsVerified(false);
                otpText.val('');
                otpContainer.removeClass("show").addClass("hide");
                hideError($(this));
            }
        }
    });

    function mobileValTrue() {
        mobile.removeClass("border-red");
        mobile.siblings("span, div").hide();
    };


    otpText.on("focus", function () {
        otpText.val('');
        otpText.siblings("span, div").hide();
    });

    var setError = function (element, msg) {
        element.addClass("border-red").siblings("span.errorIcon, div.errorText").show();
        element.siblings("div.errorText").text(msg);
    };

    var hideError = function (element) {
        element.removeClass("border-red").siblings("span.errorIcon, div.errorText").hide();
    };
    var otpVal = function (msg) {
        otpText.addClass("border-red");
        otpText.siblings("span, div").show();
        otpText.siblings("div").text(msg);
    };

    function validateOTP() {
        var retVal = true;
        var isNumber = /^[0-9]{5}$/;
        var cwiCode = otpText.val();
        customerViewModel.IsVerified(false);
        if (cwiCode == "") {
            retVal = false;
            otpVal("Please enter your Verification Code");
            bindInsuranceText();
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

    function setuserDetails() {
        var cookieName = "_PQUser";
        if (cookieName != undefined && cookieName != null && cookieName != "" && cookieName != "-1") {
            var keyValue = document.cookie.match('(^|;) ?' + cookieName + '=([^;]*)(;|$)');
            var arr = keyValue ? keyValue[2].split("&") : null;
            return arr;
        }
    }

    function setPQUserCookie() {
        var val = customerViewModel.fullName() + '&' + customerViewModel.emailId() + '&' + customerViewModel.mobileNo();
        SetCookie("_PQUser", val);
    }

    $(".edit-mobile-btn").on("click", function () {
        var prevMobile = $(this).prev("span.lead-mobile").text();
        $(".lead-otp-box-container").hide();
        $(".update-mobile-box").show();
        $("#getUpdatedMobile").val(prevMobile).focus();
    });

    $("#generateNewOTP").on("click", function () {
        if (validateMobileNo($("#getUpdatedMobile"))) {
            var updatedNumber = $(".update-mobile-box").find("#getUpdatedMobile").val();
            $(".update-mobile-box").hide();
            $(".lead-otp-box-container").show();
            $(".lead-mobile-box").find(".lead-mobile").text(updatedNumber);
        }
    });


    var variantsDropdown = $(".variants-dropdown"),
    variantSelectionTab = $(".variant-selection-tab"),
    variantUL = $(".variants-dropdown-list"),
    variantListLI = $(".variants-dropdown-list li");

    variantsDropdown.click(function (e) {
        if (!variantsDropdown.hasClass("open"))
            $.variantChangeDown(variantsDropdown);
        else
            $.variantChangeUp(variantsDropdown);
    });

    $.variantChangeDown = function (variantsDropdown) {
        variantsDropdown.addClass("open");
        variantUL.show();
    };

    $.variantChangeUp = function (variantsDropdown) {
        variantsDropdown.removeClass("open");
        variantUL.slideUp();
    };


    $(document).mouseup(function (e) {
        if (!$(".variants-dropdown, .variant-selection-tab, .variant-selection-tab #upDownArrow").is(e.target)) {
            $.variantChangeUp($(".variants-dropdown"));
        }
    });


    var validateUserInfo = function (leadUsername, leadEmailId, leadMobileNo) {
        var isValid = true;
        isValid = validateUserName(leadUsername);
        isValid &= validateEmailId(leadEmailId);
        isValid &= validateMobileNo(leadMobileNo);
        return isValid;
    };

    var validateUserName = function (leadUsername) {
        var isValid = true,
            nameLength = leadUsername.val().length;
        if (leadUsername.val().indexOf('&') != -1) {
            setError(leadUsername, 'Invalid name');
            isValid = false;
        }
        else if (nameLength == 0) {
            setError(leadUsername, 'Please enter your name');
            isValid = false;
        }
        else if (nameLength >= 1) {
            hideError(leadUsername);
            isValid = true;
        }
        return isValid;
    };

    var validateEmailId = function (leadEmailId) {
        var isValid = true,
            emailVal = leadEmailId.val(),
            reEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;
        if (emailVal == "") {
            setError(leadEmailId, 'Please enter email id');
            isValid = false;
        }
        else if (!reEmail.test(emailVal)) {
            setError(leadEmailId, 'Invalid Email');
            isValid = false;
        }
        return isValid;
    };

    var validateMobileNo = function (leadMobileNo) {
        var isValid = true,
            mobileVal = leadMobileNo.val(),
            reMobile = /^[1-9][0-9]{9}$/;
        if (mobileVal == "") {
            setError(leadMobileNo, "Please enter your mobile no.");
            isValid = false;
        }
        else if (mobileVal[0] == "0") {
            setError(leadMobileNo, "Mobile no. should not start with zero");
            isValid = false;
        }
        else if (!reMobile.test(mobileVal) && isValid) {
            setError(leadMobileNo, "Mobile no. should be 10 digits only");
            isValid = false;
        }
        else
            hideError(leadMobileNo)
        return isValid;
    };

</script>

