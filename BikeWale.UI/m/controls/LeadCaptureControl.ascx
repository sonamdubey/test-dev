﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="Bikewale.Mobile.Controls.LeadCaptureControl" %>

<!-- Lead Capture pop up start  -->
<div id="leadCapturePopup" class="bw-popup bwm-fullscreen-popup contact-details hide">
    <div class="popup-inner-container text-center">
        <div class="bwmsprite close-btn leadCapture-close-btn rightfloat"></div>
        <div id="contactDetailsPopup">
            <p class="font18 margin-top10 margin-bottom10"data-bind="text: (dealerHeading() != null && dealerHeading() != '') ? dealerHeading() : 'Provide contact details'"></p>
            <p class="text-light-grey margin-bottom20" data-bind="text: (dealerDescription() != null && dealerDescription() != '') ? dealerDescription() : 'Dealership will get back to you with offers, EMI quotes, exchange benefits and much more!'"></p>


            <div class="personal-info-form-container">
                <!-- ko if : isDealerBikes() -->
                <div id="getLeadBike" class="form-control-box margin-bottom15">
                    <div class="input-select-box dealer-search-brand form-control-box">                        
                        <div class="dealer-search-brand-form font16 text-light-grey">
                            <span id="selectedbike">Select a bike<sup>*</sup></span>
                        </div>
                        <span class="bwmsprite grey-right-icon"></span>
                        <span class="boundary"></span>
                        <span class="error-text"></span>
                        <span class="position-abt progress-bar"></span>
                    </div>
                </div>

                <div id="brandSearchBar">
                    <div class="dealer-brand-wrapper bwm-dealer-brand-box form-control-box text-left">
                        <div class="user-input-box">
                            <span class="back-arrow-box"><span class="bwmsprite back-long-arrow-left"></span></span>
                            <input class="form-control" type="text" id="assistanceBrandInput" placeholder="Select a bike" />
                        </div>
                        <ul id="sliderBrandList" class="slider-brand-list margin-top40" data-bind="foreach : dealerBikes ">
                            <li data-bind="attr : {modelid : model.modelid , bikeName : bike , versionId : version.versionId} , text : bike, click : function(data,event){ return $root.selectedBike($data);}"></li>
                        </ul>
                    </div>
                </div>
                <!-- /ko -->
                <div class="input-box form-control-box margin-bottom15">
                    <input type="text" id="getFullName" data-bind="textInput: fullName">
                    <label for="getFullName">Name<sup>*</sup></label>
                    <span class="boundary"></span>
                    <span class="error-text"></span>
                </div>
                <div class="input-box form-control-box margin-bottom15">
                    <input type="email" id="getEmailID" data-bind="textInput: emailId">
                    <label for="getEmailID">Email<sup>*</sup></label>
                    <span class="boundary"></span>
                    <span class="error-text"></span>
                </div>
                
                <div class="input-box input-number-box form-control-box margin-bottom15">
                    <input type="tel" maxlength="10" id="getMobile" data-bind="textInput: mobileNo">
                    <label for="getMobile">Mobile number<sup>*</sup></label>
                    <span class="input-number-prefix">+91</span>
                    <span class="boundary"></span>
                    <span class="error-text"></span>
                </div>
                  <!-- ko if : pinCodeRequired() -->
             <div id="getPincode-input-box" class="input-box form-control-box margin-bottom15">
                    <input type="text" maxlength="6" id="getPinCode" data-bind="textInput: pincode">
                    <label for="getPincode">Pincode<sup>*</sup></label>
                    <span class="boundary"></span>
                    <span class="error-text"></span>
                </div>
             <!-- /ko -->
                <div class="clear"></div>
                <a class="btn btn-orange" id="user-details-submit-btn" data-bind="event: { click: submitLead }">Get details</a>
                <p class="margin-top20 margin-bottom10 text-left">By proceeding ahead, you agree to BikeWale <a title="Visitor agreement" href="/visitoragreement.aspx" target="_blank">visitor agreement</a> and <a title="Privacy policy" href="/privacypolicy.aspx" target="_blank">privacy policy</a>.</p>
            </div>
        </div>

        <!-- thank you message starts here -->
        <div id="notify-response" class="hide content-inner-block-20 text-center">
            <div class="icon-outer-container rounded-corner50percent">
                <div class="icon-inner-container rounded-corner50percent">
                    <span class="bwmsprite thankyou-icon margin-top15"></span>
                </div>
            </div>
            <!-- ko if : !dealerMessage() -->
             <p class="font18 text-bold margin-top20 margin-bottom20">Thank you <span class="notify-leadUser"></span></p>
            <p class="font16 margin-bottom40" data-bind="visible : !(campaignId() > 0)">Thank you for providing your details. <span data-bind="text : dealerName()"></span><span data-bind="visible : dealerArea() && dealerArea().length > 0 ,text : ', ' + dealerArea()"></span>&nbsp; will get in touch with you soon.</p>
            <p class="font16 margin-bottom40" data-bind="visible: (campaignId() > 0)"><span data-bind="text: dealerName()"></span> Company would get back to you shortly with additional information.</p>
            <!-- /ko -->
            <!-- ko ifnot : -->
            <p class="font16 margin-bottom40" data-bind="text:dealerMessage()"></p>
            <!-- /ko -->
            <input type="button" id="notifyOkayBtn" class="btn btn-orange" value="Okay" />
        </div>
        <!-- thank you message ends here -->
    </div>
</div>
<div id="ub-ajax-loader">
    <div id="popup-loader"></div>
</div>
<!-- Lead Capture pop up end  -->

<script type="text/javascript">

    var leadBtnBookNow = $(".leadcapturebtn"), leadCapturePopup = $("#leadCapturePopup"), leadBike = $("#getLeadBike");
    var fullName = $("#getFullName");
    var emailid = $("#getEmailID");
    var mobile = $("#getMobile");
    var detailsSubmitBtn = $("#user-details-submit-btn");
    var prevEmail = "";
    var prevMobile = "";
    var prevPinCode = "";
    var leadmodelid = '<%= ModelId %>', leadcityid = '<%= CityId %>', leadareaid = '<%= AreaId %>';
    var CityArea = '<%=cityName%>' + '<%=areaName != "" ? "_" + areaName : "" %>';
    

    $(function () {

        leadBtnBookNow.on('click', function () {
            $('#selectedbike').html('Select a bike<sup>*</sup>');
            dleadvm.selectedBike(null);
            leadCapturePopup.show();
            $("#notify-response").hide();
            $("div#contactDetailsPopup").show();
            appendState('leadCapture');
            lockPopup();
        });

        $(".leadCapture-close-btn, .blackOut-window").on("click mouseup", function () {
            leadCapturePopup.hide();
            $("#notify-response").hide();
            $('body').removeClass('lock-browser-scroll');
            unlockPopup();
        });

        $(document).on('click', '#notifyOkayBtn', function () {
            $(".leadCapture-close-btn").trigger("click");
        });

        $(document).on('keydown', function (e) {
            if (e.keyCode === 27) {
                $("#leadCapturePopup .leadCapture-close-btn").trigger("click");
            }
        });
        
        $("#getFullName").on("focus", function () {
            validate.onFocus($(this));
        });
        $(document).on("focus", "#getPinCode", function () {
            var pincode = $("#getPinCode");
            validate.hideError(pincode);
            prevPinCode = pincode.val().trim();
        });
        $("#getEmailID").on("focus", function () {
            validate.onFocus($(this));
            prevEmail = $(this).val().trim();
        });

        $("#getMobile").on("focus", function () {
            validate.onFocus($(this));
            prevMobile = $(this).val().trim();
        });

        $("#getFullName").on("blur", function () {
            validate.onBlur($(this));
        });
        $(document).on("blur", "#getPinCode", function () {
            var pincode = $("#getPinCode");
            if (prevPinCode != $(this).val().trim()) {
                if (dleadvm.validatePinCode(pincode)) {
                    dleadvm.IsVerified(false);
                    validate.hideError(pincode);
                }
            }
        });
        $("#getEmailID").on("blur", function () {
            validate.onBlur($(this));
            if (prevEmail != $(this).val().trim()) {
                if (dleadvm.validateEmailId($(this))) {
                    dleadvm.IsVerified(false);
                }
            }
        });

        $("#getMobile").on("blur", function () {
            validate.onBlur($(this));
            if (prevMobile != $(this).val().trim()) {
                if (dleadvm.validateMobileNo($(this))) {
                    dleadvm.IsVerified(false);
                }
            }
        });

    });


    function leadModel() {
        var arr = setuserDetails();
        var self = this;
        if (arr != null && arr.length > 0) {
            self.fullName = ko.observable(arr[0]);
            self.emailId = ko.observable(arr[1]);
            self.mobileNo = ko.observable(arr[2]);
            self.pincode = ko.observable(arr[3]);
            $('.personal-info-form-container .input-box').addClass('not-empty');
        }
        else {
            self.fullName = ko.observable();
            self.emailId = ko.observable();
            self.mobileNo = ko.observable();
            self.pincode = ko.observable();
        }
        self.msg ="";
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
        self.isDealerBikes = ko.observable(false);
        self.dealerBikes = ko.observableArray([]);
        self.selectedBike = ko.observable();
        self.campaignId = ko.observable();
        self.mfgCampaignId = ko.observable();
        self.GAObject = ko.observable();
        self.dealerHeading = ko.observable();
        self.dealerMessage = ko.observable();
        self.dealerDescription = ko.observable();
        self.pinCodeRequired = ko.observable();
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

                if(options.campid!=null)
                    self.campaignId(options.campid);

                if (options.mfgCampid != null)
                {
                    self.mfgCampaignId(options.mfgCampid);
                }

                if(options.isdealerbikes!=null && options.isdealerbikes)
                {
                    self.isDealerBikes(options.isdealerbikes);
                    self.getDealerBikes();
                }
                if (options.dealerHeading != null && options.dealerHeading != "")
                    self.dealerHeading(options.dealerHeading);

                if (options.dealerMessage != null && options.dealerMessage != "")
                    self.dealerMessage(options.dealerMessage);

                if (options.dealerDescription != null && options.dealerDescription != "")
                    self.dealerDescription(options.dealerDescription);

                if (options.pinCodeRequired != null)
                    self.pinCodeRequired(options.pinCodeRequired);

                if(options.pageurl!=null)
                    self.pageUrl = options.pageurl;

                if(options.clientip!=null)
                    self.clientIP = options.clientip;

                if (options.pqid != null)
                    self.pqId(options.pqid);

                if (options.gaobject != null)
                    self.GAObject(options.gaobject);
            }
        };

        self.getDealerBikes = function (data,event) {

            if (!isNaN(self.dealerId()) && self.dealerId() > 0 && self.campaignId() > 0) {
                var dealerKey = "dealerDetails_" + self.dealerId() + "_camp_" + self.campaignId();
                var dealerInfo = lscache.get(dealerKey);

                if (!dealerInfo) {

                    startLoading(leadBike);
                    leadBike.find(".btnSpinner").show(); 

                    $.ajax({
                        type: "GET",
                        url: "/api/DealerBikes/?dealerId=" + self.dealerId() + "&campaignId=" + self.campaignId(),
                        contentType: "application/json",
                        dataType: 'json',
                        beforeSend: function (xhr) {
                            self.showLoader();
                            xhr.setRequestHeader('utma', getCookie('__utma'));
                            xhr.setRequestHeader('utmz', getCookie('_bwutmz'));
                        },
                        success: function (response) {
                            lscache.set(dealerKey, response, 30);
                            obj = ko.toJS(response);
                            self.dealerBikes([]);
                            self.dealerBikes(obj.dealerBikes);
                        },
                        complete: function (xhr) {
                            self.hideLoader();
                            if (xhr.status == 204 || xhr.status == 404) {
                                lscache.set(dealerKey, null, 30);
                            }
                            
                            stopLoading(leadBike);
                            leadBike.find(".btnSpinner").hide(); 
                        }
                    });
                }
                else {
                    obj = ko.toJS(dealerInfo);
                    self.dealerBikes([]);
                    self.dealerBikes(obj.dealerBikes);
                }
            }
        };

        self.generatePQ = function (data, event) {

            isSuccess = false;
            isValidDetails = false;

            isValidDetails = self.validateUserInfo(fullName, emailid, mobile);

            if (self.isDealerBikes()) {
                var bike = self.selectedBike();
                if (bike && bike.version && bike.model) {
                    self.versionId(bike.version.versionId);
                    self.modelId(bike.model.modelId);
                }
                else {
                    self.versionId(0);
                    self.modelId(0);
                }
            }

            if (isValidDetails && self.modelId() && self.versionId()) {

                var objData = {
                    "dealerId": self.dealerId(),
                    "modelId": self.modelId(),
                    "versionId": self.versionId(),
                    "cityId": self.cityId(),
                    "areaId": self.areaId(),
                    "clientIP": self.clientIP,
                    "pageUrl": self.pageUrl,
                    "sourceType": 2,
                    "pQLeadId": self.pqSourceId(),
                    "deviceId": getCookie('BWC')
                }
                return self.registerPQ(objData);
               
            }
            return isSuccess;

        }

        self.registerPQ = function (objPQData) {
            var isSuccess = false;
            if (objPQData) {
                var url = '/api/RegisterPQ/';
                $.ajax({
                    type: "POST",
                    url: url,
                    async: false,
                    data: ko.toJSON(objPQData),
                    contentType: "application/json",
                    dataType: 'json',
                    beforeSend: function (xhr) {
                        self.showLoader();
                    },
                    success: function (response) {
                        self.pqId(response.quoteId);
                        isSuccess = true;
                    },
                    complete: function (xhr) {
                        self.hideLoader();
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

        self.pushToGA = function (data, event) {
         
            if (data != null && data.act != null) {
                if (data.lab == "lead_label") {
                    data.lab = self.selectedBike().make.makeName + '_' + self.selectedBike().model.modelName + '_' + CityArea;
                }
                triggerGA(data.cat, data.act, data.lab)
            }
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
                        self.showLoader();
                        xhr.setRequestHeader('utma', getCookie('__utma'));
                        xhr.setRequestHeader('utmz', getCookie('_bwutmz'));
                    },
                    async: false,
                    contentType: "application/json",
                    dataType: 'json',
                    success: function (response) {
                        var obj = ko.toJS(response);
                        self.IsVerified(obj.isSuccess);
                    },
                    complete: function (xhr, ajaxOptions, thrownError) {
                        self.hideLoader();
                        if (xhr.status != 200)
                            self.IsVerified(false);

                       self.pushToGA(self.GAObject());
                    }
                });
            }
        };

        self.submitLead = function (data, event) {
           
            if (self.mfgCampaignId() > 0) {
                self.submitCampaignLead(data, event);
            }
            else {

                self.IsVerified(false);
                
                isValidDetails = self.validateUserInfo(fullName, emailid, mobile);
                if (self.dealerId() && isValidDetails) {
                    self.verifyCustomer();
                    if (self.IsVerified()) {

                        $("#contactDetailsPopup").hide();
                        $("#personalInfo").hide()
                        $("#notify-response").fadeIn();
                    }
                    else {
                        $("#leadCapturePopup").show();
                        $('body').addClass('lock-browser-scroll');
                        $(".blackOut-window").show();
                    }
                    setPQUserCookie();
                }
            }
        };

        self.validateUserInfo = function () {
            var isValid = true;
            
            isValid =  self.validateUserName();
            isValid &= self.validateEmailId();
            isValid &= self.validateMobileNo();
            if (self.pinCodeRequired())
                isValid &= self.validatePinCode();
            if(self.isDealerBikes())
                isValid &= self.validateBike(); 
            return isValid;
        };

        self.validateUserName = function () {
            leadUsername = fullName;
            var isValid = false;              
            if (self.fullName()!=null && self.fullName().trim() != "") {
                nameLength = self.fullName().length;

                if (self.fullName().indexOf('&') != -1) {
                    validate.setError(leadUsername, 'Invalid name');
                    isValid = false;
                }
                else if (nameLength == 0) {
                    validate.setError(leadUsername, 'Please enter your name');
                    isValid = false;
                }
                else if (nameLength >= 1) {
                    validate.hideError(leadUsername);
                    isValid = true;
                }
            }
            else
            {
                validate.setError(leadUsername, 'Please enter your name');
                isValid = false;
            }
            return isValid;
        };

        self.validateEmailId = function () {
            leadEmailId = emailid;

            var isValid = true,
                emailVal = leadEmailId.val(),
                reEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;
            if (emailVal == "") {
                validate.setError(leadEmailId, 'Please enter email id');
                isValid = false;
            }
            else if (!reEmail.test(emailVal)) {
                validate.setError(leadEmailId, 'Invalid Email');
                isValid = false;
            }
            return isValid;
        };
        self.validatePinCode = function () {
            leadPinCode = $('#getPinCode');
            var isValid = true,
                pinCodeValue = self.pincode(),
                rePinCode =/^[1-9][0-9]{5}$/;
            if (pinCodeValue == "") {
                validate.setError(leadPinCode, 'Please enter pincode');
                isValid = false;
            }
            else if (!rePinCode.test(pinCodeValue)) {
                validate.setError(leadPinCode, 'Invalid pincode');
                isValid = false;
            }
            return isValid;
        };
        self.validateMobileNo = function () {
            leadMobileNo = mobile;
            var mobileVal = leadMobileNo.val();
            if (!validateMobileNo(mobileVal, self))
            {
                validate.setError(leadMobileNo, self.msg);
                return false;
            }
            else
            {
                validate.hideError(leadMobileNo);
                return true;
            }
        };

        self.validateBike = function () {
            var isValid = true;
            eleBike =  $("#getLeadBike").find(".dealer-search-brand-form");
            if(eleBike!=null && self.selectedBike()!=null)
            {
                if (self.selectedBike().model && self.selectedBike().model.modelId > 0) {
                    validateInputSelection.hideError(eleBike);
                    isValid = true;
                }
                else {
                    validateInputSelection.setError(eleBike, 'Select a bike');
                    isValid = false;
                }
            }
            else {
                validateInputSelection.setError(eleBike, 'Select a bike');
                isValid = false;
            }

            return isValid;
        };

        self.submitCampaignLead = function (data, event) {            
            var isValidCustomer = self.validateUserInfo(fullName, emailid, mobile);

            if (isValidCustomer && self.mfgCampaignId() > 0) {

                if (self.isRegisterPQ())
                    self.generatePQ(data, event);

                $('#processing').show();
                var objCust = {
                    "dealerId": self.dealerId(),
                    "pqId": self.pqId(),
                    "name": self.fullName(),
                    "mobile": self.mobileNo(),
                    "email": self.emailId(),
                    "versionId": self.versionId(),
                    "cityId": self.cityId(),
                    "leadSourceId": self.leadSourceId(),
                    "PinCode": self.pincode(),
                    "deviceId": getCookie('BWC')
                }
                $.ajax({
                    type: "POST",
                    url: "/api/ManufacturerLead/",
                    data: ko.toJSON(objCust),
                    beforeSend: function (xhr) {
                        self.showLoader();
                        xhr.setRequestHeader('utma', getCookie('__utma'));
                        xhr.setRequestHeader('utmz', getCookie('_bwutmz'));
                    },
                    async: false,
                    contentType: "application/json",
                    dataType: 'json',
                    success: function (response) {                        
                        $("#personalInfo,#otpPopup").hide();
                        $('#processing').hide();

                        $("#contactDetailsPopup").hide();
                        $('#notify-response .notify-leadUser').text(self.fullName());
                        $('#notify-response').show();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        $('#processing').hide();
                        $("#contactDetailsPopup, #otpPopup").hide();
                    },
                    complete: function (xhr, ajaxOptions, thrownError) {
                        self.hideLoader();
                    }
                });

                setPQUserCookie();                
            }
        }

        self.showLoader = function () {
            $('#ub-ajax-loader').show();
        }

        self.hideLoader = function () {
            $('#ub-ajax-loader').hide();
        }
    }

    var dleadvm = new leadModel();
    ko.applyBindings(dleadvm, document.getElementById("leadCapturePopup"));

    function setuserDetails() {
        var cookieName = "_PQUser";
        if (cookieName != undefined && cookieName != null && cookieName != "" && cookieName != "-1") {
            var keyValue = document.cookie.match('(^|;) ?' + cookieName + '=([^;]*)(;|$)');
            var arr = keyValue ? keyValue[2].split("&") : null;
            return arr;
        }
    }

    function setPQUserCookie() {
        var val = dleadvm.fullName() + '&' + dleadvm.emailId() + '&' + dleadvm.mobileNo();
        SetCookie("_PQUser", val);
    }

    var setError = function (element, msg) {
        element.addClass("border-red").siblings("span.errorIcon, div.errorText").show();
        element.siblings("div.errorText").text(msg);
    };

    var hideError = function (element) {
        element.removeClass("border-red").siblings("span.errorIcon, div.errorText").hide();
    };

    /* form validation */
    var validate = {
        setError: function (element, message) {
            var elementLength = element.val().length,
                errorTag = element.siblings('span.error-text');

            errorTag.text(message);
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
    }

    var validateInputSelection = {
        setError: function (element, message) {
            var errorTag = element.siblings('span.error-text');

            element.closest('.input-select-box').addClass('invalid');
            errorTag.text(message);
        },

        hideError: function (element) {
            element.closest('.input-select-box').removeClass('invalid');
            element.siblings('span.error-text').text('');
        }
    }

    function startLoading(ele) {
        try {
            var _self = $(ele).find(".progress-bar").css({ 'width': '0' }).show();
            _self.animate({ width: '100%' }, 500);
        }
        catch (e) { return };
    }

    function stopLoading(ele) {
        try {
            var _self = $(ele).find(".progress-bar");
            _self.stop(true, true).css({ 'width': '100%' }).fadeOut(1000);
        }
        catch (e) { return };
    }

    var brandSearchBar = $("#brandSearchBar"), dealerSearchBrand = $(".dealer-search-brand"), dealerSearchBrandForm = $(".dealer-search-brand-form");

    
    leadCapturePopup.on('click',".dealer-search-brand", function () {
        $('.dealer-brand-wrapper').show();
        $("#brandSearchBar").addClass('open').animate({ 'left': '0px' }, 500);
        $("#brandSearchBar").find(".user-input-box").animate({ 'left': '0px' }, 500);
        $("#assistanceBrandInput").focus();
    });

    leadCapturePopup.on("click", "#sliderBrandList li", function () {
        var _self = $(this),
            selectedElement = _self.text();
        setSelectedElement(_self, selectedElement);
        _self.addClass('activeBrand').siblings().removeClass('activeBrand');
        $(".dealer-search-brand-form").addClass('selection-done').find("span").text(selectedElement);
        $("#brandSearchBar").find(".user-input-box").animate({ 'left': '100%' }, 500);
        validateInputSelection.hideError($(".dealer-search-brand-form"));
    });

    function setSelectedElement(_self, selectedElement) {
        _self.parent().prev("input[type='text']").val(selectedElement);
        $("#brandSearchBar").addClass('open').animate({ 'left': '100%' }, 500);
    };

    leadCapturePopup.on("click",".dealer-brand-wrapper .back-arrow-box", function () {
        $("#brandSearchBar").removeClass("open").animate({ 'left': '100%' }, 500);
        $("#brandSearchBar").find(".user-input-box").animate({ 'left': '100%' }, 500);
    });

    $(function () {
        var availablePincodes = [
          "401101, Bhayander West - Thane",
          "401102, Umbarpada - Thane",
          "401103, Vangaon - Thane",
          "401104, Bhayander East - Thane",
          "401105, Uttan -Thane"
        ];
        $("#getPincode").autocomplete({
            source: availablePincodes,
            appendTo: "#getPincode-input-box"
        }).autocomplete("widget").addClass("pincode-autocomplete");
    });

</script>
