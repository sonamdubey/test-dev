<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.LeadCaptureControl" %>

<style>
    #otpPopup,#otpPopup .errorIcon,#otpPopup .errorText,#otpPopup .update-mobile-box,.personal-info-list .errorIcon,.personal-info-list .errorText{display:none}#leadCapturePopup{display:none;width:450px;min-height:470px;background:#fff;margin:0 auto;position:fixed;top:10%;right:5%;left:5%;z-index:10;padding:30px 40px}.personal-info-form-container{margin:10px auto;width:300px;min-height:100px}.personal-info-form-container .personal-info-list{margin:0 auto 20px;width:280px;float:left;border-radius:0}#otpPopup .otp-box,#otpPopup .update-mobile-box{width:280px;margin:15px auto 0}#otpPopup .update-mobile-box .form-control-box{margin-top:25px;margin-bottom:50px}#otpPopup .otp-box p.resend-otp-btn{color:#0288d1;cursor:pointer;font-size:14px}#otpPopup .edit-mobile-btn,.resend-otp-btn{cursor:pointer}.icon-outer-container{width:102px;height:102px;margin:0 auto;background:#fff;border:1px solid #ccc}.icon-inner-container{width:92px;height:92px;margin:4px auto;background:#fff;border:1px solid #666}.user-contact-details-icon{width:36px;height:44px;background-position:0 -391px}.otp-icon{width:30px;height:40px;background-position:-46px -391px}.mobile-prefix{position:absolute;padding:10px 13px 13px;color:#999}.input-border-bottom{border-bottom:1px solid #ccc}.assistance-response-close.cross-lg-lgt-grey:hover{background-position:-36px -252px}
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
            <!-- ko if : isDealerBikes() -->  
            <div data-bind="visible : isDealerBikes()" class="form-control-box personal-info-list position-rel">
                <div class="placeholder-loading-text position-abt form-control border-solid" style="display: none; height: 40px; border: 1px solid #e2e2e2;">Loading dealer bikes..<span class="fa fa-spinner fa-spin position-abt text-black btnSpinner"></span></div>
                <select id="getLeadBike" data-placeholder="Choose a bike model" data-bind=" value: selectedBike, options: dealerBikes, optionValue : function(i){ return i.model.modelId ;}, optionsText: 'bike',optionsCaption: 'Select a bike'" class="form-control chosen-select"></select>
                <span class="bwsprite error-icon errorIcon"></span>
                <div class="bw-blackbg-tooltip errorText"></div>
                <span class="position-abt progress-bar" style="width: 100%; overflow: hidden; display: none;"></span>
            </div> 
            <!-- /ko --> 
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
    <div id="dealer-lead-msg" class="hide">
        <div class="icon-outer-container rounded-corner50">
            <div class="icon-inner-container rounded-corner50">
                <span class="bwsprite otp-icon margin-top25"></span>
            </div>
        </div>
        <p class="font18 margin-top25 margin-bottom20">Thank you for providing your details. <span data-bind="text : dealerName()"></span><span data-bind="    visible : dealerArea() && dealerArea().length > 0 ,text : ', ' + dealerArea()"></span>&nbsp; will get in touch with you soon.</p>

        <a href="javascript:void(0)" class="btn btn-orange okay-thanks-msg">Okay</a>
    </div>
</div>
<!-- lead capture popup End-->

<!-- scripts goes here -->
<script type="text/javascript">

    var leadBtnBookNow = $(".leadcapturebtn"), leadCapturePopup = $("#leadCapturePopup"), leadBike = $("#getLeadBike");
    var fullName = $("#getFullName");
    var emailid = $("#getEmailID");
    var mobile = $("#getMobile");
    var detailsSubmitBtn = $("#user-details-submit-btn");
    var prevEmail = "";
    var prevMobile = "";
    var leadmodelid =  '<%= ModelId %>', leadcityid = '<%= CityId %>', leadareaid =  '<%= AreaId %>';
    //var getCityArea = GetGlobalCityArea();


    $(function () {
        
        leadBtnBookNow.on('click', function () {
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
                if (self.validateMobileNo($(this))) {
                    dleadvm.IsVerified(false);
                    otpText.val('');
                    otpContainer.removeClass("show").addClass("hide");
                    hideError($(this));
                }
            }
        });

        $(document).on("change","#getLeadBike",function(){
            if($(this).val()!=null && $(this).val()!="0")
                hideError($(this));
            else setError($(this)) ;
        });

        $("#getEmailID").on("blur", function () {
            if (prevEmail != $(this).val().trim()) {
                if (self.validateEmailId($(this))) {
                    dleadvm.IsVerified(false);
                    otpText.val('');
                    otpContainer.removeClass("show").addClass("hide");
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

                if(options.isdealerbikes!=null && options.isdealerbikes)
                {
                    self.isDealerBikes(options.isdealerbikes);
                    self.getDealerBikes();
                }                    

                if(options.pageurl!=null)
                    self.pageUrl = options.pageurl;

                if(options.clientip!=null)
                    self.clientIP = options.clientip;
            }
        };

        self.getDealerBikes = function (data,event) {

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

        };

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
                        if(xhr.status!=200)
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
                    $("#dealer-lead-msg").fadeIn();

                }
                else {
                    $("#leadCapturePopup").show();
                    $('body').addClass('lock-browser-scroll');
                    $(".blackOut-window").show();
                }
                setPQUserCookie();
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

        self.validateBike = function () {
            var isValid = true;
            eleBike =  $("#getLeadBike");
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

</script>

