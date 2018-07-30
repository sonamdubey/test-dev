<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.LeadCaptureControl" %>

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
        <p class="font20 margin-top10 margin-bottom10" data-bind="text: (dealerHeading() != null && dealerHeading() != '') ? dealerHeading() : 'Provide contact details'"></p>
        <p class="text-light-grey margin-bottom20" data-bind="text: (dealerDescription() != null && dealerDescription() != '') ? dealerDescription() : 'Dealership will get back to you with offers, EMI quotes, exchange benefits and much more!'"></p>
        <div class="personal-info-form-container">
            <!-- ko if : isDealerBikes() -->
            <div data-bind="visible: isDealerBikes()" class="form-control-box personal-info-list position-rel">
                <div class="placeholder-loading-text position-abt form-control border-solid" style="display: none; height: 40px; border: 1px solid #e2e2e2;">Loading dealer bikes..<span class="fa fa-spinner fa-spin position-abt text-black btnSpinner"></span></div>
                <select id="getLeadBike" data-placeholder="Choose a bike model" data-bind=" value: selectedBike, options: dealerBikes, optionValue: function (i) { return i.model.modelId; }, optionsText: 'bike', optionsCaption: 'Select a bike'" class="form-control chosen-select margin-bottom10"></select>
                <span class="bwsprite error-icon errorIcon"></span>
                <div class="bw-blackbg-tooltip errorText"></div>
                <span class="position-abt progress-bar" style="width: 100%; overflow: hidden; display: none;"></span>
            </div>
            <!-- /ko -->
            <div class="input-box form-control-box personal-info-list">
                <input type="text" class="get-first-name" id="getFullName" data-bind="textInput: fullName" tabindex="1">
                <label for="getFullName">Name<sup>*</sup></label>
                <span class="boundary"></span>
                <span class="error-text"></span>
            </div>
            <div class="input-box form-control-box personal-info-list">
                <input type="text" class="get-email-id" id="getEmailID" data-bind="textInput: emailId" tabindex="2">
                <label for="getEmailID">Email<sup>*</sup></label>
                <span class="boundary"></span>
                <span class="error-text"></span>
            </div>
            <div class="input-box input-number-box form-control-box personal-info-list">
                <input type="text" class="get-mobile-no" id="getMobile" maxlength="10" data-bind="textInput: mobileNo" tabindex="3">
                <label for="getMobile">Mobile number<sup>*</sup></label>
                <span class="input-number-prefix">+91</span>
                <span class="boundary"></span>
                <span class="error-text"></span>
            </div>

            <div id="getPincode-input-box" class="input-box form-control-box personal-info-list" data-bind="visible : pinCodeRequired()">
                <input type="text" id="getPinCode" autocomplete="off" tabindex="4">
                <label for="getPinCode">Pincode<sup>*</sup></label>
                <span class="boundary"></span>
                <span class="error-text"></span>

                <ul id="errPinCodeSearch" class="ui-autocomplete ui-front ui-menu ui-widget hide">
                    <li class="ui-menu-item" tabindex="-1">
                        <span class="text-bold">Oops! No suggestions found</span><br>
                        <span class="text-light-grey font12">Search by pincode or area e.g: 400708 or airoli</span>
                    </li>
                </ul>
            </div>

            <div class="clear"></div>
            <a class="btn btn-orange" id="user-details-submit-btn" data-bind="event: { click: submitLead }" tabindex="5">Submit</a>
            <p class="margin-top15 text-left">By proceeding ahead, you agree to BikeWale <a title="Visitor agreement" href="/visitoragreement.aspx" target="_blank" rel="noopener">visitor agreement</a> and <a title="Privacy policy" href="/privacypolicy.aspx" target="_blank" rel="noopener">privacy policy</a>.</p>
        </div>
    </div>
    <div id="dealer-lead-msg" class="hide">
        <div class="icon-outer-container rounded-corner50">
            <div class="icon-inner-container rounded-corner50">
                <span class="bwsprite otp-icon margin-top25"></span>
            </div>
        </div>
        <!-- ko if : !dealerMessage() -->
        <p class="font18 margin-top25 margin-bottom20">Thank you for providing your details. <span data-bind="text: dealerName()"></span><span data-bind="    visible: dealerArea() && dealerArea().length > 0, text: ', ' + dealerArea()"></span>&nbsp; will get in touch with you soon.</p>
        <!-- /ko -->
        <!-- ko ifnot : -->
        <p class="font18 margin-top25 margin-bottom20" data-bind="text: dealerMessage()"></p>
        <!-- /ko -->
        <a href="javascript:void(0)" class="btn btn-orange okay-thanks-msg">Okay</a>
    </div>
</div>
<!-- lead capture popup End-->
<div id="ub-ajax-loader">
    <div id="spinner-content">
        <svg class="bw-spinner" width="50px" height="50px" viewBox="0 0 50 50">
            <circle class="circle-path" fill="none" stroke-width="4" stroke-linecap="round" cx="25" cy="25" r="22"></circle>
        </svg>
    </div>
</div>
<!-- scripts goes here -->
<script type="text/javascript">

    var leadBtnBookNow , leadCapturePopup , leadBike ;
    var fullName,emailid , mobile ;
    var assistanceGetName , assistanceGetEmail, assistanceGetMobile,detailsSubmitBtn ;
    var prevEmail = "",prevMobile = "",prevPinCode = "";
    var leadmodelid = '<%= ModelId %>', leadcityid = '<%= CityId %>', leadareaid = '<%= AreaId %>';
    var objPinCodes = new Object(),dleadvm,validate;

    function leadModel() {
        var arr = setuserDetails();
        var self = this;
        if (arr != null && arr.length > 0) {
            self.fullName = ko.observable(arr[0]);
            if (arr[1]!="undefined") {
                self.emailId = ko.observable(arr[1]);
            }
            else {
                self.emailId = ko.observable();
            }

            self.mobileNo = ko.observable(arr[2]);
        }
        else {
            self.fullName = ko.observable();
            self.emailId = ko.observable();
            self.mobileNo = ko.observable();
        }
        self.msg = "";
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
        self.GAObject = ko.observable();
        self.mfgCampaignId = ko.observable();
        self.IsLeadPopup = ko.observable(true);
        self.dealerHeading = ko.observable();
        self.dealerMessage = ko.observable();
        self.dealerDescription = ko.observable();
        self.pinCodeRequired = ko.observable();
        self.pincode = ko.observable();
        self.setOptions = function (options) {
            if (options != null) {
                if (options.dealerid != null)
                    self.dealerId(options.dealerid);

                if (options.dealername != null)
                    self.dealerName(options.dealername);

                if (options.dealerarea != null)
                    self.dealerArea(options.dealerarea);

                if (options.versionid != null)
                    self.versionId(options.versionid);

                if (options.leadsourceid != null)
                    self.leadSourceId(options.leadsourceid);

                if (options.pqsourceid != null)
                    self.pqSourceId(options.pqsourceid);

                if (options.isregisterpq != null)
                    self.isRegisterPQ(options.isregisterpq);

                if (options.campid != null)
                    self.campaignId(options.campid);

                if (options.mfgCampid != null) {
                    self.mfgCampaignId(options.mfgCampid);
                }
                if (options.dealerHeading != null && options.dealerHeading != "")
                    self.dealerHeading(options.dealerHeading);

                if (options.dealerMessage != null && options.dealerMessage != "")
                    self.dealerMessage(options.dealerMessage);

                if (options.dealerDescription != null && options.dealerDescription != "")
                    self.dealerDescription(options.dealerDescription);

                if (options.pinCodeRequired != null)
                    self.pinCodeRequired(options.pinCodeRequired != 'true' ? false : true);

                if (options.isdealerbikes != null) {
                    self.isDealerBikes(options.isdealerbikes);
                    self.getDealerBikes();
                }

                if (options.isleadpopup != null)
                    self.IsLeadPopup(options.isleadpopup != "false" ? true : false);
                else self.IsLeadPopup(true);

                if (options.pqid != null)
                    self.pqId(options.pqid);

                if (options.gaobject != null) {
                    self.GAObject(options.gaobject);
                }
                if (options.pageurl != null)
                    self.pageUrl = options.pageurl;

                if (options.clientip != null)
                    self.clientIP = options.clientip;
            }
        };

        self.getDealerBikes = function (data, event) {
            if (!isNaN(self.dealerId()) && self.dealerId() > 0 && self.campaignId() > 0) {
                var dealerKey = "dealerDetails_" + self.dealerId() + "_camp_" + self.campaignId();
                var dealerInfo = lscache.get(dealerKey);

                if (!dealerInfo) {

                    startLoading(leadBike.parent());
                    leadBike.prev().show();

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

                            stopLoading(leadBike.parent());
                            leadBike.prev().hide();
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
                    "sourceType": 1,
                    "pQLeadId": self.pqSourceId(),
                    "deviceId": getCookie('BWC')
                };

                return self.registerPQ(objData);
            }

            return isSuccess;

        };

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
        };

        self.pushToGA = function (data, event) {
            if (data != null && data.act != null) {
                triggerGA(data.cat, data.act, data.lab)
            }
        };

        self.verifyCustomer = function (data, event) {

            if (self.isRegisterPQ())
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
                    "clientIP": self.clientIP,
                    "PageUrl": self.pageUrl,
                    "leadSourceId": self.leadSourceId(),
                    "deviceId": getCookie('BWC')
                };
                $.ajax({
                    type: "POST",
                    url: "/api/PQCustomerDetail/",
                    data: ko.toJSON(objCust),
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader('utma', getCookie('__utma'));
                        xhr.setRequestHeader('utmz', getCookie('_bwutmz'));
                        self.showLoader();
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

        self.submitCampaignLead = function (data, event) {

            self.pincode($("#getPinCode").val().trim());

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
                };
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
                        $("#contactDetailsPopup").hide();
                        $("#personalInfo").hide();
                        $("#dealer-lead-msg").fadeIn();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        $("#leadCapturePopup").show();
                        $('body').addClass('lock-browser-scroll');
                        $(".blackOut-window").show();
                    },
                    complete: function (xhr, ajaxOptions, thrownError) {
                        self.hideLoader();
                    }
                });
                setPQUserCookie();
            }
        };

        self.submitLead = function (data, event) {

            if (self.mfgCampaignId() > 0) {
                self.submitCampaignLead(data, event);
            }
            else {
                self.IsVerified(false);
                isValidDetails = self.validateUserInfo(fullName, emailid, mobile);
                var eventButtonId = $(event.currentTarget).attr('id');
                switch (eventButtonId) {
                    case 'user-details-submit-btn': // model 'get offers from dealers', dealer locator listing 'submit'
                        isValidDetails = self.validateUserInfo('#getFullName', '#getEmailID', '#getMobile');
                        break;

                    case 'assistFormSubmit': // model 'get dealer assistance'
                        isValidDetails = self.validateUserInfo('#assistGetName', '#assistGetEmail', '#assistGetMobile');
                        break;

                    case 'dealer-assist-btn': // dealer details 'get assistance'
                        isValidDetails = self.validateUserInfo('#assistGetName', '#assistGetEmail', '#assistGetMobile');
                        break;

                    default:
                        break;
                }

                if (self.dealerId() && isValidDetails) {
                    self.verifyCustomer();
                    if (self.IsVerified()) {

                        if (self.IsLeadPopup()) {
                            $("#contactDetailsPopup").hide();
                            $("#personalInfo").hide();
                            $("#dealer-lead-msg").fadeIn();
                        }
                        else {
                            $("#buyingAssistance").hide();
                            $("#dealer-assist-msg").fadeIn();
                        }
                    }
                    else {
                        if (self.IsLeadPopup()) {
                            $("#leadCapturePopup").show();
                            $('body').addClass('lock-browser-scroll');
                            $(".blackOut-window").show();
                        }
                    }
                    setPQUserCookie();
                }
            }
        };

        self.HiddenSubmitLead = function (d, e) {
            ele = $(e.target);
            var leadOptions = {
                "dealerid": ele.attr('data-item-id'),
                "dealername": ele.attr('data-item-name'),
                "dealerarea": ele.attr('data-item-area'),
                "versionid": versionId,
                "leadsourceid": ele.attr('data-leadsourceid'),
                "pqsourceid": ele.attr('data-pqsourceid'),
                "isleadpopup": ele.attr('data-isleadpopup'),
                "mfgCampid": ele.attr('data-mfgcampid'),
                "pqid": pqId,
                "isregisterpq": ele.attr('data-isregisterpq'),
                "pageurl": pageUrl,
                "clientip": clientIP
            };

            self.setOptions(leadOptions);

            self.submitLead(d, e);

        };

        self.setPinCodeSuggestion = function () {

            $.fn.hint = bwHint;
            $.fn.bw_autocomplete = bwAutoComplete;

            $("#getPinCode").bw_autocomplete({
                source: 4,
                recordCount: 3,
                minLength: 2,
                onClear: function () {
                    objPinCodes = new Object();
                },
                click: function (event, ui, orgTxt) {
                    if (self.selectedBike() && self.selectedBike().make && self.selectedBike().model) {
                        var keywrd = self.selectedBike().make.makeName + '_' + self.selectedBike().model.modelName + '_pinCode_' + $('#getPinCode').val();
                        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'LeadCapture_Popup', 'act': 'PinCode_Selected', 'lab': keywrd });
                    }
                    if (ui && ui.item) {
                        self.pincode(ui.item.payload.pinCode);
                    }
                    else {
                        self.pincode(0);
                    }

                },
                open: function (result) {
                    objPinCodes.result = result;
                },
                focusout: function () {
                    if ($('#getPinCode').find('li.ui-state-focus a:visible').text() != "") {
                        $('#errPinCodeSearch').hide();
                        focusedMakeModel = new Object();
                        focusedMakeModel = objPinCodes.result ? objPinCodes.result[$('li.ui-state-focus').index()] : null;
                    }
                    else {
                        $('#errPinCodeSearch').hide();
                    }
                },
                afterfetch: function (result, searchtext) {
                    if (result != undefined && result.length > 0 && searchtext.trim() != "") {
                        $('#errPinCodeSearch').hide();
                    }
                    else {
                        focusedMakeModel = null;
                        if (searchtext.trim() != "") {
                            $('#errPinCodeSearch').show();
                            self.pincode(0);
                        }
                    }
                },
                keyup: function () {
                    if ($('#getPinCode').val().trim() != '' && $('li.ui-state-focus a:visible').text() != "") {
                        focusedMakeModel = new Object();
                        focusedMakeModel = objPinCodes.result ? objPinCodes.result[$('li.ui-state-focus').index()] : null;
                        $('#errPinCodeSearch').hide();
                    } else {
                        if ($('#getPinCode').val().trim() == '') {
                            $('#errPinCodeSearch').hide();
                        }
                    }

                    if ($('#getPinCode').val().trim() == '' || e.keyCode == 27 || e.keyCode == 13) {
                        if (focusedMakeModel == null || focusedMakeModel == undefined) {
                            if ($('#getPinCode').val().trim() != '') {
                                $('#errPinCodeSearch').show();
                                self.pincode(0);
                            }
                        }
                        else {
                            $('#errPinCodeSearch').hide();
                        }

                    }
                }
            }).autocomplete({ appendTo: $("#getPinCode").closest(".input-box") }).autocomplete("widget").addClass("pincode-autocomplete");
        };

        self.checkPinCode = function () {
            isValid = false;
            $.ajax({
                async: false,
                type: "GET",
                url: "/api/autosuggest/?source=4&inputText=" + self.pincode() + "&noofrecords=5",
                contentType: "application/json",
                dataType: "json",
                success: function (data) {
                    if (data && data.suggestionList.length > 0) {
                        $('#getPinCode').val(data.suggestionList[0].text);
                        self.pincode(data.suggestionList[0].payload.pinCodeId);
                        isValid = true;
                    }
                    else {
                        validate.setError($('#getPinCode'), 'Invalid pincode');
                        self.pincode(0);
                        isValid = false;
                    }
                }
            });

            return isValid;
        };

        self.validateUserInfo = function (inputName, inputEmail, inputMobile) {
            var isValid = true;
            isValid = self.validateUserName(inputName);
            isValid &= self.validateEmailId(inputEmail);
            isValid &= self.validateMobileNo(inputMobile);
            if (self.pinCodeRequired()) {
                isValid &= self.validatePinCode();
                if (isValid)
                    isValid &= self.checkPinCode(self.pincode());
            }
            if (self.isDealerBikes())
                isValid &= self.validateBike();
            return isValid;
        };

        self.validateUserName = function (inputName) {
            var isValid = false;
            if (self.fullName() != null && self.fullName().trim() != "") {
                var nameLength = self.fullName().length;

                if (self.fullName().indexOf('&') != -1) {
                    validate.setError($(inputName), 'Invalid name');
                    isValid = false;
                }
                else if (nameLength >= 1) {
                    validate.hideError($(inputName));
                    isValid = true;
                }
            }
            else {
                validate.setError($(inputName), 'Please enter your name');
                isValid = false;
            }
            return isValid;
        };

        self.validateEmailId = function (inputEmail) {
            var isValid = true,
                emailVal = $(inputEmail).val(),
                reEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;

            if (emailVal == "") {
                validate.setError($(inputEmail), 'Please enter email id');
                isValid = false;
            }
            else if (!reEmail.test(emailVal)) {
                validate.setError($(inputEmail), 'Invalid Email');
                isValid = false;
            }
            return isValid;
        };
        self.validatePinCode = function () {
            leadPinCode = $('#getPinCode'); var isValid = true, pc = leadPinCode.val().trim();
            if (pc.indexOf(',') > 0)
                pc = pc.substring(0, 6);
            if (!(/^[1-9][0-9]{5}$/.test(pc))) {
                validate.setError(leadPinCode, 'Invalid pincode');
                isValid = false;
            }
            return isValid;
        };
        self.validateMobileNo = function (inputMobile) {

            mobileVal = $(inputMobile).val();
            if (!validateMobileNo(mobileVal, self)) {
                validate.setError($(inputMobile), self.msg);
                return false;
            }
            else {
                validate.hideError($(inputMobile));
                return true;
            }
        };

        self.validateBike = function () {
            var isValid = true;
            eleBike = $("#getLeadBike");
            if (eleBike != null && self.selectedBike() != null) {
                if (self.selectedBike().model && self.selectedBike().model.modelId > 0) {
                    hideError(eleBike);
                    isValid = true;
                }
                else {
                    setError(eleBike, 'Select a bike');
                    isValid = false;
                }
            }
            else {
                setError(eleBike, 'Select a bike');
                isValid = false;
            }

            return isValid;
        };

        self.setInputValues = function () {
            var inputBoxes = $('.personal-info-form-container .input-box, #buyingAssistance .input-box');
            for (var i = 0; i < inputBoxes.length; i++) {
                var item = $(inputBoxes[i]),
                    inputValue = item.find('input').val();

                if (inputValue.length > 0) {
                    item.addClass('not-empty');
                }

            }
        };

        self.showLoader = function () {
            $('#ub-ajax-loader').show();
        };

        self.hideLoader = function () {
            $('#ub-ajax-loader').hide();
        };

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
        var val = dleadvm.fullName() + '&' + dleadvm.emailId() + '&' + dleadvm.mobileNo();
        SetCookie("_PQUser", val);
    }

    var setError = function (element, msg) {
        element.addClass("border-red").siblings("span.errorIcon, div.errorText").show();
        element.siblings("div.errorText").show().text(msg);
    };

    var hideError = function (element) {
        element.removeClass("border-red").siblings("span.errorIcon, div.errorText").hide();
    };

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

    docReady(function () {

        leadBtnBookNow = $(".leadcapturebtn"), leadCapturePopup = $("#leadCapturePopup"), leadBike = $("#getLeadBike");
        fullName = $("#getFullName"),emailid = $("#getEmailID"), mobile = $("#getMobile");
        assistanceGetName = $('#assistanceGetName'), assistanceGetEmail = $('#assistanceGetEmail'), assistanceGetMobile = $('#assistanceGetMobile');
        detailsSubmitBtn = $("#user-details-submit-btn");

        /* form validation */
        validate = {
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
        };

        $(document).on('click', ".leadcapturebtn", function () {
            leadCapturePopup.show();
            $("#dealer-lead-msg").hide();
            $("div#contactDetailsPopup").show();
            popup.lock();
        });

        $(".leadCapture-close-btn, .blackOut-window").on("click mouseup", function () {
            leadCapturePopup.hide();
            $("#dealer-lead-msg").hide();
            popup.unlock();
        });

        $(document).on('click', '#dealer-lead-msg .okay-thanks-msg', function () {
            $(".leadCapture-close-btn").click();
        });

        $(document).on('keydown', function (e) {
            if (e.keyCode === 27) {
                $("#leadCapturePopup .leadCapture-close-btn").click();
            }
        });

        $("#dealer-assist-msg .assistance-response-close").on("click", function () {
            $("#dealer-assist-msg").parent().slideUp();
        });

        $("#getFullName, #assistGetName").on("focus", function () {
            validate.onFocus($(this));
        });

        $("#getEmailID, #assistGetEmail").on("focus", function () {
            validate.onFocus($(this));
            prevEmail = $(this).val().trim();
        });

        $("#getMobile, #assistGetMobile").on("focus", function () {
            validate.onFocus($(this));
            prevMobile = $(this).val().trim();
        });

        $(document).on("focus", "#getPinCode", function () {
            validate.onFocus($(this));
            prevPinCode = $(this).val().trim().substring(0,6);
        });

        $("#getFullName, #assistGetName").on("blur", function () {
            validate.onBlur($(this));
        });

        $("#getMobile, #assistGetMobile").on("blur", function () {
            validate.onBlur($(this));
            if (prevMobile != $(this).val().trim()) {
                if (dleadvm.validateMobileNo($(this))) {
                    dleadvm.IsVerified(false);
                }
            }
        });
        $(document).on("blur", "#getPinCode", function () {
            validate.onBlur($(this));
            var pc = $(this).val().trim();
            if (pc.indexOf(',') > 0)
                pc = pc.substring(0, 6);
            if (pc.length > 0 && !(/^[1-9][0-9]{5}$/.test(pc))) {
                validate.setError($("#getPinCode"), 'Invalid pincode');
            }
        });
        $(document).on("change", "#getLeadBike", function () {
            if ($(this).val() != null && $(this).val() != "0")
                hideError($(this));
            else setError($(this));
        });

        $("#getEmailID, #assistGetEmail").on("blur", function () {
            validate.onBlur($(this));
            if (prevEmail != $(this).val().trim()) {
                if (dleadvm.validateEmailId($(this))) {
                    dleadvm.IsVerified(false);
                }
            }
        });

        dleadvm = new leadModel();
        ko.applyBindings(dleadvm, document.getElementById("leadCapturePopup"));

        if ($("#dealerAssistance") && $("#dealerAssistance").length > 0)
        {
            ko.applyBindings(dleadvm, document.getElementById("dealerAssistance"));
        }
           
        if ($("#getPinCode") && $("#getPinCode").length > 0)
        {
            dleadvm.setPinCodeSuggestion();
        }

        dleadvm.setInputValues();

    });
</script>

