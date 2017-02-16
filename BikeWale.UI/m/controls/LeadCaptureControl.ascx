﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="Bikewale.Mobile.Controls.LeadCaptureControl" %>

<!-- Lead Capture pop up start  -->
<div id="leadCapturePopup" class="bw-popup bwm-fullscreen-popup contact-details hide">
    <div class="popup-inner-container text-center">
        <div class="bwmsprite close-btn leadCapture-close-btn rightfloat"></div>
        <div id="contactDetailsPopup">
            <p class="font18 margin-top10 margin-bottom10">Provide contact details</p>
            <p class="text-light-grey margin-bottom10">Dealership will get back to you with offers</p>


            <div class="personal-info-form-container">
                <!-- ko if : isDealerBikes() -->
                <div id="getLeadBike" class="margin-top10 form-control-box">
                    <div class="dealer-search-brand form-control-box">
                        
                        <div class="dealer-search-brand-form"><span id="selectedbike">Select a bike</span></div>
                        <span class="bwmsprite error-icon errorIcon"></span>
                        <div class="bw-blackbg-tooltip errorText"></div>
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
                <div class="form-control-box margin-top20">
                    <input type="text" class="form-control get-first-name" placeholder="Your name" id="getFullName" data-bind="textInput: fullName">
                    <span class="bwmsprite error-icon errorIcon"></span>
                    <div class="bw-blackbg-tooltip errorText"></div>
                </div>
                <div class="form-control-box margin-top20">
                    <input type="email" class="form-control get-email-id" placeholder="Email address" id="getEmailID" data-bind="textInput: emailId">
                    <span class="bwmsprite error-icon errorIcon"></span>
                    <div class="bw-blackbg-tooltip errorText"></div>
                </div>
                <div class="form-control-box margin-top20">
                    <p class="mobile-prefix">+91</p>
                    <input type="tel" class="form-control get-mobile-no" maxlength="10" placeholder="Mobile no." id="getMobile" data-bind="textInput: mobileNo">
                    <span class="bwmsprite error-icon errorIcon"></span>
                    <div class="bw-blackbg-tooltip errorText"></div>
                </div>
                <div class="clear"></div>
                <a class="btn btn-full-width btn-orange margin-top20" id="user-details-submit-btn" data-bind="event: { click: submitLead }">Submit</a>
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
            <p class="font18 text-bold margin-top20 margin-bottom20">Thank you <span class="notify-leadUser"></span></p>
            
            <p class="font16 margin-bottom40" data-bind="visible : !(campaignId() > 0)">Thank you for providing your details. <span data-bind="text : dealerName()"></span><span data-bind="visible : dealerArea() && dealerArea().length > 0 ,text : ', ' + dealerArea()"></span>&nbsp; will get in touch with you soon.</p>
            <p class="font16 margin-bottom40" data-bind="visible: (campaignId() > 0)"><span data-bind="    text: dealerName()"></span> Company would get back to you shortly with additional information.</p>
            <input type="button" id="notifyOkayBtn" class="btn btn-orange" value="Okay" />
        </div>
        <!-- thank you message ends here -->
    </div>
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
    var leadmodelid = '<%= ModelId %>', leadcityid = '<%= CityId %>', leadareaid = '<%= AreaId %>';
    var CityArea = '<%=cityName%>' + '<%=areaName != "" ? "_" + areaName : "" %>';
    

    $(function () {

        leadBtnBookNow.on('click', function () {
            $('#selectedbike').text('Select a bike');
            dleadvm.selectedBike(null);
            leadCapturePopup.show();
            $("#notify-response").hide();
            $("div#contactDetailsPopup").show();
            $(".blackOut-window").show();
        });

        $(".leadCapture-close-btn, .blackOut-window").on("click mouseup", function () {
            leadCapturePopup.hide();
            $("#notify-response").hide();
            $('body').removeClass('lock-browser-scroll');
            $(".blackOut-window").hide();
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
                if (dleadvm.validateMobileNo($(this))) {
                    dleadvm.IsVerified(false);
                  
                    hideError($(this));
                }
            }
        });

        $("#getEmailID").on("blur", function () {
            if (prevEmail != $(this).val().trim()) {
                if (dleadvm.validateEmailId($(this))) {
                    dleadvm.IsVerified(false);
                  
                    hideError($(this));
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
        }
        else {
            self.fullName = ko.observable();
            self.emailId = ko.observable();
            self.mobileNo = ko.observable();
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

            if(self.isDealerBikes())
            {
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
                var url = '/api/RegisterPQ/';
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
            }
            else
            {
                setError(leadUsername, 'Please enter your name');
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
            var mobileVal = leadMobileNo.val();
            if (!validateMobileNo(mobileVal, self))
            {
                setError(leadMobileNo, self.msg);
                return false;
            }
            else
            {
                hideError(leadMobileNo);
                return true;
            }
        };

        self.validateBike = function () {
            var isValid = true;
            eleBike =  $("#getLeadBike").find(".dealer-search-brand-form");
            if(eleBike!=null && self.selectedBike()!=null)
            {
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
                    "deviceId": getCookie('BWC')
                }
                $.ajax({
                    type: "POST",
                    url: "/api/ManufacturerLead/",
                    data: ko.toJSON(objCust),
                    beforeSend: function (xhr) {
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
                    }
                });

                setPQUserCookie();                
            }
        };
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
        hideError($(".dealer-search-brand-form"));
    });

    function setSelectedElement(_self, selectedElement) {
        _self.parent().prev("input[type='text']").val(selectedElement);
        $("#brandSearchBar").addClass('open').animate({ 'left': '100%' }, 500);
    };

    leadCapturePopup.on("click",".dealer-brand-wrapper .back-arrow-box", function () {
        $("#brandSearchBar").removeClass("open").animate({ 'left': '100%' }, 500);
        $("#brandSearchBar").find(".user-input-box").animate({ 'left': '100%' }, 500);
    });

</script>
