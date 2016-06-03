<%@ Control Language="C#" AutoEventWireup="true" Inherits="Bikewale.Mobile.Controls.LeadCaptureControl" %>

<style>
    #leadCapturePopup .leadCapture-close-btn {
        z-index: 2;
    }

    #leadCapturePopup .error-icon, #leadCapturePopup .bw-blackbg-tooltip {
        display: none;
    }

    .btn-grey {
        background: #fff;
        color: #82888b;
        border: 1px solid #82888b;
    }

        .btn-grey:hover {
            background: #82888b;
            color: #fff;
            text-decoration: none;
            border: 1px solid #82888b;
        }

    #notifyAvailabilityContainer {
        min-height: 320px;
        background: #fff;
        margin: 0 auto;
        padding: 10px;
        position: fixed;
        top: 10%;
        right: 5%;
        left: 5%;
        z-index: 10;
    }

    #notify-form .grid-12 {
        padding: 10px 20px;
    }

    .personal-info-notify-container input {
        margin: 0 auto;
    }

    .notify-offers-list {
        list-style: disc;
        margin-left: 10px;
    }

    #notifyAvailabilityContainer .notify-close-btn {
        z-index: 2;
    }

    #leadCapturePopup .error-icon, #leadCapturePopup .bw-blackbg-tooltip {
        display: none;
    }

    .float-button {
        background-color: #f5f5f5;
        padding: 10px;
    }

        .float-button.float-fixed {
            position: fixed;
            bottom: 0;
            z-index: 8;
            left: 0;
            right: 0;
        }

    #getMobile {
        padding: 9px 40px;
    }

    .mobile-prefix {
        position: absolute;
        padding: 10px 13px 13px;
        color: #999;
        z-index: 2;
    }

    .thankyou-icon {
        width: 48px;
        height: 58px;
        background-position: -137px -404px;
    }

    /*#otpPopup{display:none}
.icon-outer-container{width:102px;height:102px;margin:0 auto;background:#fff;border:1px solid #ccc}
.icon-inner-container{width:92px;height:92px;margin:4px auto;background:#fff;border:1px solid #666}
.user-contact-details-icon{width:36px;height:44px;background-position:-107px -227px}
.otp-icon{width:30px;height:40px;background-position:-107px -177px}
.edit-blue-icon{width:16px;height:16px;background-position:-114px -123px}
#otpPopup .errorIcon,#otpPopup .errorText{display:none}
#otpPopup .otp-box p.resend-otp-btn{color:#0288d1;cursor:pointer;font-size:14px}
#otpPopup .update-mobile-box{display:none}
#otpPopup .edit-mobile-btn{cursor:pointer}*/
</style>
<!-- Lead Capture pop up start  -->
<div id="leadCapturePopup" class="bw-popup bwm-fullscreen-popup contact-details hide">
    <div class="popup-inner-container text-center">
        <div class="bwmsprite close-btn leadCapture-close-btn rightfloat"></div>
        <div id="contactDetailsPopup">
            <h2 class="margin-top10 margin-bottom10">Provide contact details</h2>
            <p class="text-light-grey margin-bottom10">Dealership will get back to you with offers</p>

            <div class="personal-info-form-container">
                <div class="form-control-box margin-top20">
                    <input type="text" class="form-control get-first-name" placeholder="Your name" id="getFullName" data-bind="textInput: fullName">
                    <span class="bwmsprite error-icon errorIcon"></span>
                    <div class="bw-blackbg-tooltip errorText"></div>
                </div>
                <div class="form-control-box margin-top20">
                    <input type="text" class="form-control get-email-id" placeholder="Email address" id="getEmailID" data-bind="textInput: emailId">
                    <span class="bwmsprite error-icon errorIcon"></span>
                    <div class="bw-blackbg-tooltip errorText"></div>
                </div>
                <div class="form-control-box margin-top20">
                    <p class="mobile-prefix">+91</p>
                    <input type="text" class="form-control get-mobile-no" maxlength="10" placeholder="Mobile no." id="getMobile" data-bind="textInput: mobileNo">
                    <span class="bwmsprite error-icon errorIcon"></span>
                    <div class="bw-blackbg-tooltip errorText"></div>
                </div>
                <div class="clear"></div>
                <a class="btn btn-full-width btn-orange margin-top20" id="user-details-submit-btn" data-bind="event: { click: submitLead }">Submit</a>
            </div>
        </div>

        <!-- thank you message starts here -->
        <div id="notify-response" class="hide margin-top10 content-inner-block-20 text-center">
            <div class="icon-outer-container rounded-corner50percent">
                <div class="icon-inner-container rounded-corner50percent">
                    <span class="bwmsprite thankyou-icon margin-top25"></span>
                </div>
            </div>
            <p class="font18 text-bold margin-top20 margin-bottom20">Thank you <span class="notify-leadUser"></span></p>
            <p class="font16 margin-bottom40">Dealer would get back to you shortly with additional information.</p>
            <input type="button" id="notifyOkayBtn" class="btn btn-orange" value="Okay" />
        </div>
        <!-- thank you message ends here -->
        <%--<div id="otpPopup">
                    <p class="font18 margin-bottom5">Verify your mobile number</p>
                    <p class="text-light-grey margin-bottom5">We have sent OTP on your mobile. Please enter that OTP in the box provided below:</p>
                    <div>
                        <div class="lead-mobile-box lead-otp-box-container margin-bottom10 font22">
                            <span class="bwmsprite tel-grey-icon"></span>
                            <span class="text-light-grey font24">+91</span>
                            <span class="lead-mobile font24"></span>
                            <span class="bwmsprite edit-blue-icon edit-mobile-btn"></span>
                        </div>
                        <div class="otp-box lead-otp-box-container">
                            <div class="form-control-box margin-bottom10">
                                <input type="text" class="form-control" placeholder="Enter your OTP" id="getOTP" maxlength="5" data-bind="value: otpCode"/>
                                <span class="bwmsprite error-icon errorIcon"></span>
                                <div class="bw-blackbg-tooltip errorText"></div>
                            </div>
                            <a class="margin-left10 blue resend-otp-btn margin-top10" id="resendCwiCode" data-bind="visible: (NoOfAttempts() < 2), click: function () { regenerateOTP() }">Resend OTP</a>
                            <p class="margin-left10 margin-top10 otp-notify-text text-light-grey font12" data-bind="visible: (NoOfAttempts() >= 2)">
                                OTP has been already sent to your mobile
                            </p>
                            <a class="btn btn-full-width btn-orange margin-top20" id="otp-submit-btn">Confirm</a>
                        </div>
                        <div class="update-mobile-box">
                            <div class="form-control-box text-left">
                                <p class="mobile-prefix">+91</p>
                                <input type="text" class="form-control padding-left40" placeholder="Mobile no." maxlength="10" id="getUpdatedMobile" data-bind="value: mobileNo"  />
                                <span class="bwmsprite error-icon errorIcon"></span>
                                <div class="bw-blackbg-tooltip errorText"></div>
                            </div>
                            <input type="button" class="btn btn-orange margin-top20" value="Send OTP" id="generateNewOTP" data-bind="event: { click: submitLead }" />
                        </div>
                    </div>
                </div>--%>
    </div>
</div>
<!-- Lead Capture pop up end  -->

<script type="text/javascript">

    var leadBtnBookNow = $(".leadcapturebtn"), leadCapturePopup = $("#leadCapturePopup");
    var fullName = $("#getFullName");
    var emailid = $("#getEmailID");
    var mobile = $("#getMobile");
    var detailsSubmitBtn = $("#user-details-submit-btn");
    var prevEmail = "";
    var prevMobile = "";
    var leadmodelid =  <%= ModelId %>, leadcityid = <%= CityId %>, leadareaid =  <%= AreaId %>;
    //var getCityArea = GetGlobalCityArea();
    

   <%-- var otpContainer = $(".mobile-verification-container"), otpText = $("#getOTP"), otpBtn = $("#otp-submit-btn");  --%>


    $(function () {

        leadBtnBookNow.on('click', function () {
            leadCapturePopup.show();
            $("#notify-response").hide();
            $("div#contactDetailsPopup").show();
            $("#otpPopup").hide();
            $('body').addClass('lock-browser-scroll');
            $(".blackOut-window").show();

        });

        $(".leadCapture-close-btn, .blackOut-window").on("click mouseup", function () {
            leadCapturePopup.hide();
            $("#notify-response").hide();
            $('body').removeClass('lock-browser-scroll');
            $(".blackOut-window").hide();
        });

        $(document).on('click', '#notifyOkayBtn', function () {
            $(".leadCapture-close-btn").click();
        });

        $(document).on('keydown', function (e) {
            if (e.keyCode === 27) {
                $("#leadCapturePopup .leadCapture-close-btn").click();
            }
        });

        $("#getFullName").on("focus", function () {
            hideError($(this));
        });

        $("#getEmailID").on("focus", function () {
            hideError($(this));
            prevEmail = $(this).val().trim();
        });

        $("#getMobile").on("focus", function () {
            hideError($(this));
            prevMobile = $(this).val().trim();
        }); 

        $("#getMobile").on("blur", function () {
            if (prevMobile != $(this).val().trim()) {
                if (validateMobileNo($(this))) {
                    customerViewModel.IsVerified(false);
                    otpText.val('');
                    otpContainer.removeClass("show").addClass("hide");
                    hideError($(this));
                }
            }
        });

        $("#getEmailID").on("blur", function () {
            if (prevEmail != $(this).val().trim()) {
                if (validateEmailId($(this))) {
                    customerViewModel.IsVerified(false);
                    otpText.val('');
                    otpContainer.removeClass("show").addClass("hide");
                    hideError($(this));
                }
            }
        });

        <%-- 
        //$(".edit-mobile-btn").on("click", function () {
        //    var prevMobile = $(this).prev("span.lead-mobile").text();
        //    $(".lead-otp-box-container").hide();
        //    $(".update-mobile-box").show();
        //    $("#getUpdatedMobile").val(prevMobile).focus();
        //});

        //$("#generateNewOTP").on("click", function () {
        //    if (validateMobileNo($("#getUpdatedMobile"))) {
        //        var updatedNumber = $(".update-mobile-box").find("#getUpdatedMobile").val();
        //        $(".update-mobile-box").hide();
        //        $(".lead-otp-box-container").show();
        //        $(".lead-mobile-box").find(".lead-mobile").text(updatedNumber);
        //    }
        //}); 
        

        //$("#getMobile,#getUpdatedMobile").on("focus", function () {
        //    hideError($(this));
        //    prevMobile = $(this).val().trim();
        //}); 

        //$("#getMobile,#getUpdatedMobile").on("blur", function () {
        //    if (prevMobile != $(this).val().trim()) {
        //        if (validateMobileNo($(this))) {
        //            customerViewModel.IsVerified(false);
        //            otpText.val('');
        //            otpContainer.removeClass("show").addClass("hide");
        //            hideError($(this));
        //        }
        //    }
        //});
    --%>

    });


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
        self.pqId = ko.observable(0);
        self.dealerId = ko.observable();
        self.modelId = ko.observable(leadmodelid);
        self.versionId = ko.observable();
        self.cityId = ko.observable(leadcityid);
        self.areaId = ko.observable(leadareaid);
        self.dealerName = ko.observable();
        self.leadSourceId = ko.observable();
        self.dealerArea = ko.observable();
        self.pqSourceId = ko.observable();
        self.pageUrl = window.location.href;
        self.clientIP = "";
        self.isRegisterPQ = ko.observable(false);

       <%-- //self.NoOfAttempts = ko.observable(0); //self.otpCode = ko.observable(); //self.isAssist = ko.observable(false); --%>
        
        self.setOptions = function(options)
        {
            if(options!=null)
            {
                if(options.dealerid!=null)
                    self.dealerId(options.dealerid);

                if(options.dealername!=null)
                    self.dealerName(options.dealername);

                if(options.dealerarea!=null)
                    self.dealerArea(options.dealerarea);

                if(options.versionid!=null)
                    self.versionId(options.versionid);

                if(options.leadsourceid!=null)
                    self.leadSourceId(options.leadsourceid);

                if(options.pqsourceid!=null)
                    self.pqSourceId(options.pqsourceid);

                if(options.isregisterpq!=null)
                    self.isRegisterPQ(options.isregisterpq);

                if(options.pageurl!=null)
                    self.pageUrl = options.pageurl;

                if(options.clientip!=null)
                    self.clientIP = options.clientip;
            }
        }

        self.generatePQ = function (data, event) {

            isSuccess = false;
            isValidDetails = false;

            isValidDetails = self.validateUserInfo(fullName, emailid, mobile);

            if (isValidDetails && self.modelId() && self.versionId()) {
                var url = '/api/RegisterPQ/';
                var objData = {
                    "dealerId": self.dealerId(),
                    "modelId": self.modelId(),
                    "versionId": self.versionId(),
                    "cityId": self.cityId(),
                    "areaId": self.areaId(),
                    "clientIP": self.clientIP,
                    "pageUrl": self.pageUrl,
                    "sourceType": 1,
                    "pQLeadId": self.pqSourceId(),
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
                        if (xhr.status != 200) {
                            self.IsVerified(false);
                            isSuccess = false;
                            stopLoading($("#user-details-submit-btn").parent());
                        }

                    }
                });
            }

            return isSuccess;

        }

        self.verifyCustomer = function (data, event) {
            
            if(self.isRegisterPQ())
                self.generatePQ(data, event);

            if (self.pqId() && self.dealerId()) {
                var objCust = {
                    "dealerId": self.dealerId(),
                    "pqId": self.pqId(),
                    "versionId": self.versionId(),
                    "cityId": self.cityId(),
                    "customerName": self.fullName(),
                    "customerMobile": self.mobileNo(),
                    "customerEmail": self.emailId(),
                    "clientIP": clientIP,
                    "PageUrl": pageUrl,
                    "leadSourceId": self.leadSourceId(),
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
                        <%--//self.IsVerified(obj.isSuccess); //if (!self.IsVerified()) { //    self.NoOfAttempts(obj.noOfAttempts); //} --%>
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        self.IsVerified(false);
                    }
                });
            }
        };

        self.submitLead = function (data, event) {
            self.IsVerified(false);
            isValidDetails = self.validateUserInfo(fullName, emailid, mobile);

            if (self.dealerId() && isValidDetails) {
                self.verifyCustomer();
                if (self.IsVerified()) {

                    $("#contactDetailsPopup").hide();
                    $("#personalInfo").hide()
                    //$("#otpPopup").hide();
                    $("#notify-response").fadeIn();

                }
                else {
                    $("#leadCapturePopup").show();
                    $('body').addClass('lock-browser-scroll');
                    $(".blackOut-window").show();
                    <%-- 
                   // $("#contactDetailsPopup").hide();
                   // $("#otpPopup").show();
                   // var leadMobileVal = mobile.val();
                   // $("#otpPopup .lead-mobile-box").find("span.lead-mobile").text(leadMobileVal);
                    //otpContainer.removeClass("hide").addClass("show");
                   // hideError(mobile);
                   // otpText.val('').removeClass("border-red").siblings("span, div").hide();
                    --%>
                }
                setPQUserCookie();
            }
        };

        self.validateUserInfo = function () {
            var isValid = true;
            isValid =  self.validateUserName();
            isValid &= self.validateEmailId();
            isValid &= self.validateMobileNo();
            return isValid;
        };

        self.validateUserName = function () {
            leadUsername = fullName;
            var isValid = true,
                nameLength = self.fullName().length;
            if (self.fullName().indexOf('&') != -1) {
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

        self.validateEmailId = function () {
            leadEmailId = emailid;
            //var emailid = $("#getEmailID");

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

        self.validateMobileNo = function () {
            leadMobileNo = mobile;
            //var mobile = $("#getMobile");
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


       <%-- //self.generateOTP = function () {
        //    if (!self.IsVerified()) {
        //        var objCust = {
        //            "pqId": self.pqId(),
        //            "customerMobile": self.mobileNo(),
        //            "customerEmail": self.emailId(),
        //            "cwiCode": self.otpCode(),
        //            "branchId": self.dealerId(),
        //            "customerName": self.fullName(),
        //            "versionId": self.versionId(),
        //            "cityId": self.cityId(),
        //        }
        //        $.ajax({
        //            type: "POST",
        //            url: "/api/PQMobileVerification/",
        //            data: ko.toJSON(objCust),
        //            async: false,
        //            contentType: "application/json",
        //            dataType: 'json',
        //            success: function (response) {
        //                var obj = ko.toJS(response);
        //                self.IsVerified(obj.isSuccess);
        //            },
        //            error: function (xhr, ajaxOptions, thrownError) {
        //                self.IsVerified(false);
        //            }
        //        });
        //    }
        //};

        //self.regenerateOTP = function () {
        //    if (self.NoOfAttempts() <= 2 && !self.IsVerified()) {
        //        var url = '/api/ResendVerificationCode/';
        //        var objCustomer = {
        //            "customerName": self.fullName(),
        //            "customerMobile": self.mobileNo(),
        //            "customerEmail": self.emailId(),
        //            "source": 1
        //        }
        //        $.ajax({
        //            type: "POST",
        //            url: url,
        //            async: false,
        //            data: ko.toJSON(objCustomer),
        //            contentType: "application/json",
        //            dataType: 'json',
        //            success: function (response) {
        //                self.IsVerified(false);
        //                self.NoOfAttempts(response.noOfAttempts);
        //                alert("You will receive the new OTP via SMS shortly.");
        //            },
        //            error: function (xhr, ajaxOptions, thrownError) {
        //                self.IsVerified(false);
        //            }
        //        });
        //    }
        //};

        //otpBtn.on("click", function (event) {
        //    $('#processing').show();
        //    isValidDetails = false;
        //    if (!validateOTP())
        //        $('#processing').hide();

        //    isValidDetails = self.validateUserInfo(fullName, emailid, mobile);

        //    if (validateOTP() && isValidDetails) {
        //        customerViewModel.generateOTP();
        //        if (customerViewModel.IsVerified()) {
        //            $("#personalInfo").hide();
        //            otpText.val('');
        //            otpContainer.removeClass("show").addClass("hide");
        //            $("#personalInfo").hide()
        //            $("#otpPopup").hide();

        //            $("#dealer-lead-msg").fadeIn();


        //            // OTP Success
        //            if (getMoreDetailsClick) {
        //                dataLayer.push({ "event": "Bikewale_all", "cat": "Dealer_PQ", "act": "Lead_Submitted", "lab": "Get_more_details_" + GetBikeVerLoc() });
        //                getMoreDetailsClick = false;
        //            }
        //        }
        //        else {
        //            $('#processing').hide();
        //            otpVal("Please enter a valid OTP.");
        //        }
        //    }
        //});   --%>
    }

    var customerViewModel = new CustomerModel();
    ko.applyBindings(customerViewModel, $('#leadCapturePopup')[0]);

    function ValidateUserDetail(fullName, emailid, mobile) {
        return customerviewmodel.validateUserInfo(fullName, emailid, mobile);
    };


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

    var setError = function (element, msg) {
        element.addClass("border-red").siblings("span.errorIcon, div.errorText").show();
        element.siblings("div.errorText").text(msg);
    };

    var hideError = function (element) {
        element.removeClass("border-red").siblings("span.errorIcon, div.errorText").hide();
    };

    <%--
    //otpText.on("focus", function () {
    //    otpText.val('');
    //    otpText.siblings("span, div").hide();
    //}); 

    //var otpVal = function (msg) {
    //    otpText.addClass("border-red");
    //    otpText.siblings("span, div").show();
    //    otpText.siblings("div").text(msg);
    //};

    //function validateOTP() {
    //    var retVal = true;
    //    var isNumber = /^[0-9]{5}$/;
    //    var cwiCode = otpText.val();
    //    customerViewModel.IsVerified(false);
    //    if (cwiCode == "") {
    //        retVal = false;
    //        otpVal("Please enter your Verification Code");
    //        bindInsuranceText();
    //    }
    //    else {
    //        if (isNaN(cwiCode)) {
    //            retVal = false;
    //            otpVal("Verification Code should be numeric");
    //        }
    //        else if (cwiCode.length != 5) {
    //            retVal = false;
    //            otpVal("Verification Code should be of 5 digits");
    //        }
    //    }
    //    return retVal;
    //}   --%>

</script>
