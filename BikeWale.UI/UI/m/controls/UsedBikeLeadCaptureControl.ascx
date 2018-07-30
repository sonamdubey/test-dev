<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.UsedBikeLeadCaptureControl" %>
<!-- get seller details pop up start  -->
<div id="get-seller-details-popup" class="bw-popup bwm-fullscreen-popup size-small">
    <div class="popup-inner-container text-center">
        <div class="bwmsprite close-btn seller-details-close position-abt pos-top20 pos-right20" data-bind="click: function (data, event) { closePopup(); }"></div>
        <div id="user-details-section">
            <div class="icon-outer-container rounded-corner50percent margin-bottom10">
                <div class="icon-inner-container rounded-corner50percent">
                    <span class="bwmsprite user-contact-details-icon margin-top15"></span>
                </div>
            </div>
            <p class="font18 text-bold margin-bottom10">Get seller details</p>
            <p class="font14 text-light-grey margin-bottom25">For privacy concerns, we hide owner details. Please fill this form to get owner's details.</p>

            <div class="input-box form-control-box margin-bottom10">
                <input type="text" id="getUserName" data-bind="textInput: buyer().userName, event: { focus: function (d, e) { userNameOnFocus($(e.target)); }, blur: function (d, e) { userNameOnBlur($(e.target)); } }" />
                <label for="getUserName">Name<sup>*</sup></label>
                <span class="boundary"></span>
                <span class="error-text"></span>
            </div>
            <div class="input-box form-control-box margin-bottom10">
                <input type="email" id="getUserEmailID" data-bind="textInput: buyer().emailId, event: { focus: function (d, e) { userEmailOnFocus($(e.target)); }, blur: function (d, e) { userEmailOnBlur($(e.target)); } }" />
                <label for="getUserEmailID">Email<sup>*</sup></label>
                <span class="boundary"></span>
                <span class="error-text"></span>
            </div>
            <div class="input-box input-number-box form-control-box margin-bottom15">
                <input type="tel" id="getUserMobile" maxlength="10" data-bind="textInput: buyer().mobileNo, event: { focus: function (d, e) { userMobileOnFocus($(e.target)); }, blur: function (d, e) { userMobileOnBlur($(e.target)); } }" />
                <label for="getUserMobile">Mobile number<sup>*</sup></label>
                <span class="input-number-prefix">+91</span>
                <span class="boundary"></span>
                <span class="error-text"></span>
            </div>
            <p class="font14 red-text margin-bottom10"  data-bind="visible: message() && message().length > 0, text: message()"  style="display:none"></p>
            <a class="btn btn-orange btn-fixed-width" id="submit-user-details-btn" rel="nofollow" data-bind="click: function (data, event) { submitUserDetails(); }">Get seller details</a>
            <p class="margin-top20 margin-bottom10 text-left">By proceeding ahead, you agree to BikeWale <a title="Visitor agreement" href="/visitoragreement.aspx" target="_blank" rel="noopener">visitor agreement</a> and <a title="Privacy policy" href="/privacypolicy.aspx" target="_blank" rel="noopener">privacy policy</a>.</p>
        </div>

        <div id="mobile-verification-section">
            <div class="icon-outer-container rounded-corner50percent margin-bottom10">
                <div class="icon-inner-container rounded-corner50percent">
                    <span class="bwmsprite otp-icon margin-top15"></span>
                </div>
            </div>
            <p class="font18 text-bold margin-bottom10">Mobile verification</p>
            <p class="font14 text-light-grey margin-bottom25">We have just sent a 5-digit verification code on your mobile number.</p>


            <div id="verify-otp-content">
                <div class="margin-bottom35">
                    <div class="leftfloat text-left">
                        <p class="font12 text-light-grey">Mobile number</p>
                        <div class="font16 text-bold">
                            <span class="text-light-grey">+91</span>
                            <span class="user-submitted-mobile" data-bind="text: buyer().mobileNo"></span>
                        </div>
                    </div>
                    <div class="rightfloat bwmsprite edit-blue-icon" id="edit-mobile-btn" data-bind="click: function (data, event) { editMobile(); }"></div>
                    <div class="clear"></div>
                </div>

                <div class="input-box form-control-box margin-bottom15">
                    <input type="tel" id="getUserOTP" maxlength="5" data-bind="textInput: otp, event: { focus: function (d, e) { userOtpOnFocus($(e.target)); }, blur: function (d, e) { userOtpOnBlur($(e.target)); } }" />
                    <label for="getUserOTP">One-time password</label>
                    <span class="boundary"></span>
                    <span class="error-text"></span>
                </div>
                <!-- ko ifnot : isVerified -->
                <p class="font14 red-text margin-bottom10" data-bind="text: message, style: { display: message ? 'block' : 'none' }"></p>
                <!-- /ko -->
                <a class="btn btn-orange btn-fixed-width" id="submit-user-otp-btn" rel="nofollow" data-bind="click: function (d, e) { submitOtp(); }">Verify</a>
            </div>

            <div id="update-mobile-content">
                <div class="input-box input-number-box form-control-box margin-bottom15">
                    <input type="tel" id="getUpdatedMobile" maxlength="10" data-bind="textInput: buyer().mobileNo, event: { focus: function (d, e) { updatedUserMobileOnFocus($(e.target)); }, blur: function (d, e) { updatedUserMobileOnBlur($(e.target)); } }" />
                    <label for="getUpdatedMobile">Mobile number<sup>*</sup></label>
                    <span class="input-number-prefix">+91</span>
                    <span class="boundary"></span>
                    <span class="error-text"></span>
                </div>
                <a class="btn btn-orange btn-fixed-width" id="submit-updated-mobile-btn" rel="nofollow" data-bind="click: function (data, event) { submitUpdateMobile(); }">Done</a>
            </div>
        </div>

        <div id="seller-details-section">
            <div class="icon-outer-container rounded-corner50percent margin-bottom10">
                <div class="icon-inner-container rounded-corner50percent">
                    <span class="bwmsprite user-contact-details-icon margin-top15"></span>
                </div>
            </div>
            <p class="font18 text-bold margin-bottom10">Seller details</p>
            <p class="font14 text-light-grey margin-bottom20">We have also sent you these details through SMS and e-mail.</p>

            <ul class="dealer-details-list text-left">
                <li>
                    <p class="data-key">Name</p>
                    <p class="data-value" data-bind="text: seller().userName"></p>
                </li>
                <li>
                    <p class="data-key">Email</p>
                    <p class="data-value" data-bind="text: seller().emailId"></p>
                </li>
                <li>
                    <p class="data-key">Mobile number</p>
                    <p class="data-value" data-bind="text: seller().mobileNo"></p>
                </li>
                <li>
                    <p class="data-key">City</p>
                    <p class="data-value" data-bind="text: seller().location"></p>
                </li>
            </ul>

        </div>
        <!-- OTP Popup ends here -->
    </div>
</div>
<!-- get seller details pop up end  -->
<div id="ub-ajax-loader">
    <div class="cover-popup-loader"></div>
</div>
<script type="text/javascript">
    var getUserName = $('#getUserName'),
    getUserEmailID = $('#getUserEmailID'),
    getUserMobile = $('#getUserMobile'),
    getUpdatedUserMobile = $('#getUpdatedMobile'),
    getUserOTP = $('#getUserOTP');
    var msg = "";
    $(document).on('click', '.used-bike-lead', function () {
        ele = $(this);
        ubLeadVM.init();
        ubLeadVM.leadInitGAObject({
            cat: ele.attr('data-ga-cat'),
            act: ele.attr('data-ga-act'),
            lab: ele.attr('data-ga-lab')
        });
        ubLeadVM.widgetName(ele.attr('data-ga-widgetname'));
        ubLeadVM.profileId(ele.attr('data-profile-id'));
        ubLeadVM.pushInitGAObject();
        ubLeadVM.shownInterest();
        getSellerDetailsPopup.open();
        appendHash("sellerDealers");
    });

    var getSellerDetailsPopup = {

        popup: $('#get-seller-details-popup'),

        userDetails: $('#user-details-section'),

        mobileVerification: $('#mobile-verification-section'),

        verifyOTP: $('#verify-otp-content'),

        updateMobile: $('#update-mobile-content'),

        seller: $('#seller-details-section'),

        open: function () {
            getSellerDetailsPopup.userDetails.hide();
            getSellerDetailsPopup.popup.show();
        },

        close: function () {
            ubLeadVM.reset();
            getSellerDetailsPopup.popup.hide();
            getSellerDetailsPopup.userDetailsSection();
        },

        userDetailsSection: function () {
            getSellerDetailsPopup.mobileVerification.hide();
            getSellerDetailsPopup.seller.hide();
            getSellerDetailsPopup.userDetails.show();
            getSellerDetailsPopup.generateNewOTP();
        },

        verifySection: function () {
            getSellerDetailsPopup.userDetails.hide();
            getSellerDetailsPopup.mobileVerification.show();
        },

        updateMobileSection: function () {
            getSellerDetailsPopup.verifyOTP.hide();
            getSellerDetailsPopup.updateMobile.show();
        },

        generateNewOTP: function () {
            getSellerDetailsPopup.updateMobile.hide();
            getSellerDetailsPopup.verifyOTP.show();
        },

        sellerDetails: function () {
            getSellerDetailsPopup.mobileVerification.hide();
            getSellerDetailsPopup.seller.show();
        },
        loader: {
            open: function () {
                $('html, body').addClass('lock-browser-scroll');
                $('#ub-ajax-loader').show();
            },

            close: function () {
                $('html, body').removeClass('lock-browser-scroll');
                $('#ub-ajax-loader').hide();
            }
        }
    };

    function ValidateUserDetail(name, email, mobile) {
        var isValid = true;
        isValid = validateEmail(email);
        isValid &= validateMobile(mobile);
        isValid &= validateName(name);
        return isValid;
    };

    function validateName(name) {
        var isValid = true;
        var a = name.val().trim().length;
        if (a == 0) {
            isValid = false;
            validate.setError(name, 'Please enter your name');
        }
        else if (!(/^[a-z ,.'-]+$/i).test(name.val())) {
            isValid = false;
            validate.setError(name, 'Invalid name');
        }
        else if (a >= 1) {
            validate.hideError(name);
        }
        return isValid;
    }

    function validateEmail(email) {
        var isValid = true;
        var emailID = email.val().trim();
        var reEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;

        if (emailID == "") {
            validate.setError(email, 'Please enter email address');
            isValid = false;
        }
        else if (!reEmail.test(emailID)) {
            validate.setError(email, 'Invalid Email');
            isValid = false;
        }

        return isValid;
    }

    function validateMobile(mobile) {
        var mobileNo = mobile.val().trim();
        if (!validateMobileNo(mobileNo, this)) {
            validate.setError(mobile, this.msg);
            return false;
        }
        else {
            validate.hideError(mobile);
            return true;
        }
    }

    function validateOTP() {
        var retVal = true;
        var isNumber = /^[0-9]{5}$/;
        var cwiCode = getUserOTP.val().trim();
        if (cwiCode == "") {
            retVal = false;
            validate.setError(getUserOTP, "Please enter your Verification Code");
        }
        else {
            if (isNaN(cwiCode)) {
                retVal = false;
                validate.setError(getUserOTP, "Verification Code should be numeric");
            }
            else if (cwiCode.length != 5) {
                retVal = false;
                validate.setError(getUserOTP, "Verification Code should be of 5 digits");
            }
        }
        if (retVal) {
            ubLeadVM.validateOTP();
        }
        return retVal;
    }

    var validate = {
        setError: function (element, message) {
            var elementLength = element.val().length;
            errorTag = element.siblings('span.error-text');

            errorTag.show().text(message);
            if (!elementLength) {
                element.closest('.input-box').removeClass('not-empty').addClass('invalid');
            }
            else {
                element.closest('.input-box').addClass('not-empty invalid');
            }
        },

        hideError: function (element) {
            element.closest('.input-box').removeClass('invalid').addClass('not-empty');
            element.siblings('span.error-text').text('');
        },

        onFocus: function (inputField) {
            if (inputField.closest('.input-box').hasClass('invalid')) {
                validate.hideError(inputField);
            }
        },

        onBlur: function (inputField) {
            var inputLength = inputField.val().length;
            if (!inputLength) {
                inputField.closest('.input-box').removeClass('not-empty');
            }
            else {
                inputField.closest('.input-box').addClass('not-empty');
            }
        }
    };

    function usedBikeLead() {
        var self = this;
        self.targetBindItem = "get-seller-details-popup";
        self.buyer = ko.observable();
        self.seller = ko.observable();
        self.isVerified = ko.observable(true);
        self.profileId = ko.observable();
        self.status = ko.observable();
        self.message = ko.observable();
        self.otp = ko.observable();
        self.pageUrl = ko.observable(location.href);
        self.leadInitGAObject = ko.observable();
        self.platformId = ko.observable(2);
        self.userCookieName = ko.observable("_PQUser");
        self.widgetName = ko.observable('');
        self.pushInitGAObject = function () {
            if (self.leadInitGAObject())
                triggerGA(self.leadInitGAObject().cat, (self.widgetName() ? self.widgetName() : '') + self.leadInitGAObject().act, self.leadInitGAObject().lab);
        };
        self.pushLeadSubmit = function () {
            if (self.profileId()) {
                triggerGA(self.leadInitGAObject().cat, (self.widgetName() ? self.widgetName() : '') + 'Lead_Submit_Used_Bike', self.profileId());
            }
        };
        self.submitPurchaseRequest = function () {
            if (self.profileId()) {
                var objBuyer = {
                    "customerName": self.buyer().userName(),
                    "customerEmail": self.buyer().emailId(),
                    "customerMobile": self.buyer().mobileNo()
                };

                $.ajax({
                    type: "POST",
                    url: "/api/usedbike/purchaseinquiry/?profileId=" + self.profileId() + "&pageurl=" + self.pageUrl(),
                    data: ko.toJSON(objBuyer),
                    contentType: "application/json",
                    dataType: 'json',
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader('platformid', self.platformId());
                        self.setSeller();
                        getSellerDetailsPopup.loader.open();
                    },
                    success: function (resp) {

                    },
                    complete: function (xhr, ajaxOptions, thrownError) {
                        getSellerDetailsPopup.loader.close();
                        if (xhr.status == 200) {
                            var resp = xhr.responseJSON;
                            if (resp) {
                                switch (resp.inquiryStatus.status) {
                                    case 1:
                                    case 4:
                                        self.pushLeadSubmit();
                                        self.setSeller(resp.seller, resp.sellerAddress);
                                        getSellerDetailsPopup.sellerDetails();
                                        getSellerDetailsPopup.userDetails.hide();
                                        getSellerDetailsPopup.seller.show();
                                        break;
                                    case 0:
                                        self.showInvalidRequest(resp.inquiryStatus.message);
                                        break;
                                    case 2:
                                        self.showInvalidInfoError(resp.inquiryStatus.message);
                                        break;
                                    case 3:
                                        self.isVerified(false);
                                        self.otp('');
                                        getSellerDetailsPopup.verifySection();
                                        break;
                                    case 5:
                                        self.showMaxLimitReached(resp.inquiryStatus.message, resp.inquiryStatus.status);
                                        break;
                                }
                            }
                        }
                        if (xhr.status != 200)
                            alert('error');
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(thrownError);
                    }
                });
            }
        };
        self.shownInterest = function () {
            self.message('');
            if (self.profileId()) {
                var objBuyer = {
                    "customerName": self.buyer().userName(),
                    "customerEmail": self.buyer().emailId(),
                    "customerMobile": self.buyer().mobileNo()
                };

                $.ajax({
                    type: "POST",
                    url: "/api/bikebuyer/showninterest/?profileId=" + self.profileId() + "&isDealer=" + false,
                    data: ko.toJSON(objBuyer),
                    contentType: "application/json",
                    dataType: 'json',
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader('platformid', self.platformId());
                        self.setSeller();
                        getSellerDetailsPopup.loader.open();
                    },
                    success: function (resp) {

                    },
                    complete: function (xhr, ajaxOptions, thrownError) {
                        getSellerDetailsPopup.loader.close();
                        if (xhr.status == 200) {
                            if (xhr.responseJSON) {
                                if (xhr.responseJSON.shownInterest) {
                                    self.setSeller(xhr.responseJSON.seller.details, xhr.responseJSON.seller.address);
                                    getSellerDetailsPopup.sellerDetails();
                                    getSellerDetailsPopup.userDetails.hide();
                                    getSellerDetailsPopup.seller.show();
                                }
                                else {
                                    getSellerDetailsPopup.userDetails.show();
                                }
                            }
                            else {
                                getSellerDetailsPopup.userDetails.show();
                            }
                        }
                        else {
                            getSellerDetailsPopup.userDetails.show();
                        }
                        if (xhr.status != 200)
                            self.message("Some error occured");
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        self.message("Some error occured");
                    }
                });
            }
        };
        self.validateOTP = function () {
            if (self.profileId()) {
                $.ajax({
                    type: "POST",
                    url: "/api/mobileverification/validateotp/?mobile=" + self.buyer().mobileNo() + "&otp=" + self.otp(),
                    contentType: "application/json",
                    dataType: 'json',
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader('platformid', self.platformId());
                        getSellerDetailsPopup.loader.open();
                    },
                    success: function (resp) {

                    },
                    complete: function (xhr, ajaxOptions, thrownError) {
                        getSellerDetailsPopup.loader.close();
                        if (xhr.status == 200) {
                            if (xhr.responseJSON) {
                                self.isVerified(true);
                                self.submitPurchaseRequest();
                            }
                            else {
                                self.message("Invalid OTP.");
                                self.isVerified(false);
                            }
                        }

                        if (xhr.status != 200)
                            alert('error');
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(thrownError);
                    }
                });
            }
        };
        self.setBuyer = function (n, e, m) {
            var buyer = new customerVM();
            buyer.userName(n);
            if (e != "undefined") {
                buyer.emailId(e);
            }
            else {
                buyer.emailId();
            }
            
            buyer.mobileNo(m);
            self.buyer(buyer);
        };
        self.readTempUserCookie = function () {
            var cookieName = self.userCookieName();
            if (cookieName != undefined && cookieName != null && cookieName != "" && cookieName != "-1") {
                var keyValue = document.cookie.match('(^|;) ?' + cookieName + '=([^;]*)(;|$)');
                var arr = keyValue ? keyValue[2].split("&") : null;
                return arr;
            }
        };
        self.removeUserCookie = function () {
            var cookieName = self.userCookieName();
            $.cookie(cookieName, null, { path: '/' });
        };
        self.setSeller = function (s, sa) {
            var slr = new customerVM();
            if (s && sa) {
                slr.userName(s.customerName);
                slr.emailId(s.customerEmail);
                slr.mobileNo(s.customerMobile);
                slr.location(sa);
            }
            self.seller(slr);
        };
        self.showInvalidInfoError = function (msg) {
            self.message(msg);
            self.status(code);
        };
        self.showInvalidRequest = function (msg) {

        };
        self.showMaxLimitReached = function (msg, code) {
            self.message(msg);
            self.status(code);
        };
        self.reset = function () {
            self.setSeller();
            self.isVerified(true);
            self.message('');
            self.status('');
        };
        self.init = function () {
            var arr = self.readTempUserCookie();
            if (arr && arr.length > 1) {
                self.setBuyer(arr[0], arr[1], arr[2]);
                
                var obj = getSellerDetailsPopup.userDetails.find(".input-box");
                if (arr[0] != "undefined")
                    $(obj[0]).addClass("not-empty");
                if (arr[1] != "undefined")
                    $(obj[1]).addClass("not-empty");
                if (arr[2] != "undefined")
                    $(obj[2]).addClass("not-empty");
            }
            else
                self.setBuyer();
            self.setSeller();
            ko.cleanNode(document.getElementById(self.targetBindItem));
            ko.applyBindings(self, document.getElementById(self.targetBindItem));
        };
        self.closePopup = function () {
            getSellerDetailsPopup.close();
            window.history.back();
        };
        self.submitUserDetails = function () {
            if (ValidateUserDetail(getUserName, getUserEmailID, getUserMobile)) {
                self.removeUserCookie();
                self.submitPurchaseRequest();
            }
        };
        self.editMobile = function () {
            var prevMobile = getUserMobile.val();
            getSellerDetailsPopup.updateMobileSection();
            getUserOTP.val('');
            getUpdatedUserMobile.focus().val(prevMobile);
        };
        self.submitUpdateMobile = function () {
            if (validateMobile(getUpdatedUserMobile)) {
                var newMobile = getUpdatedUserMobile.val();
                $('#verify-otp-content').find('.user-submitted-mobile').text(newMobile);

                var inputBox = getUserOTP.closest('.input-box');
                if (inputBox.hasClass('invalid')) {
                    inputBox.removeClass('invalid').find('.error-text').text('');
                }

                getSellerDetailsPopup.generateNewOTP();
                self.removeUserCookie();
                self.submitPurchaseRequest();
            }
        };
        self.submitOtp = function () {
            if (validateOTP()) {
                self.submitPurchaseRequest();
            }
        };
        self.userNameOnFocus = function (getUserName) {
            validate.onFocus(getUserName);
        };
        self.userEmailOnFocus = function (getUserEmailID) {
            validate.onFocus(getUserEmailID);
        };
        self.userMobileOnFocus = function (getUserMobile) {
            validate.onFocus(getUserMobile);
        };
        self.updatedUserMobileOnFocus = function (getUpdatedUserMobile) {
            validate.onFocus(getUpdatedUserMobile);
        };
        self.userOtpOnFocus = function (getUserOTP) {
            validate.onFocus(getUserOTP);
        };
        self.userNameOnBlur = function (getUserName) {
            validate.onBlur(getUserName);
        };
        self.userEmailOnBlur = function (getUserEmailID) {
            validate.onBlur(getUserEmailID);
        };
        self.userMobileOnBlur = function (getUserMobile) {
            validate.onBlur(getUserMobile);
        };
        self.updatedUserMobileOnBlur = function (getUpdatedUserMobile) {
            validate.onBlur(getUpdatedUserMobile);
        };
        self.userOtpOnBlur = function (getUserOTP) {
            validate.onBlur(getUserOTP);
        };
    }

    function customerVM() {
        var self = this;
        self.userName = ko.observable();
        self.emailId = ko.observable();
        self.mobileNo = ko.observable();
        self.location = ko.observable();
    }

    var ubLeadVM = new usedBikeLead();    
</script>
