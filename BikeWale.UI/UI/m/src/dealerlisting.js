var makeCityViewModel = {
    makeName : ko.observable(makeName != "" ? makeName : 'Select a bike'),
    selectedMakeId: ko.observable(makeId),
    selectedMakeMaskingName: ko.observable(makeMaskingName),
    cityName : ko.observable(cityName != "" ? cityName : 'Select City'),
    selectedCityId: ko.observable(cityId),
    selectedCityMaskingName: ko.observable(cityMaskingName)
};

ko.applyBindings(makeCityViewModel, $('#divMakeCity')[0]);

var key = "dealerCities_";
lscache.setBucket('DLPage');

$('.listing-filter-btn').on('click', function () {
    $('#dealersFilterWrapper').animate({ 'left': '0' }, 500);
});
$('.filterBackArrow').on('click', function () {
    $('#dealersFilterWrapper').animate({ 'left': '100%' }, 500);
});

$(".maskingNumber").on("click", function () {
    triggerGA("Dealer_Locator", "Dealer_Number_Clicked", makeCityViewModel.makeName() + "_" + makeCityViewModel.cityName());
});

var selectBrand = $('#selectBrand'),
    selectCity = $('#selectCity'),
    dealerFilterContent = $('#dealerFilterContent');

selectBrand.on("click", function () {
    $("#dealerFilterContent .dealers-brand-popup-box").show().siblings("div.dealers-city-popup-box").hide();
    animateFilterList();
});

selectCity.on("click", function () {
    $("#dealerFilterContent .dealers-brand-popup-box").hide().siblings("div.dealers-city-popup-box").show();
    animateFilterList();
});

var animateFilterList = function () {
    dealerFilterContent.addClass("open").stop().animate({ 'left': '0' }, 500);
    $(".user-input-box").stop().animate({ 'left': '0' }, 500);
}

$(".dealers-brand-city-wrapper .dealers-back-arrow-box").on("click", function () {
    dealerFilterContent.removeClass("open").stop().animate({ 'left': '100%' }, 500);
    $(".user-input-box").stop().animate({ 'left': '100%' }, 500);
});

$("#dealersBrandInput, #dealersCityInput, #assistanceBrandInput").on("keyup", function () {
    locationFilter($(this));
});

$(".filter-brand-city-ul").on("click", "li", function () {

    $ulCities = $("#filterCityList");
    $ullMakes = $("#filterBrandList");

    var selectedElement = $(this),
        selectedElementValue = selectedElement.text(),
        selectedElementParent = selectedElement.parent(),
        selectedElementInputField = selectedElementParent.siblings("div.user-input-box"),
        selectedElementParentAttr = selectedElementParent.attr("data-filter-type"),
        selectedElementId = selectedElement.attr("value"),
        selectElementMaskingName = selectedElement.attr("maskingName");

    selectedElementInputField.find("input").val(selectedElementValue);

    if (selectedElementParentAttr == "brand-filter") {       
        makeCityViewModel.makeName(selectedElementValue);
        makeCityViewModel.selectedMakeId(selectedElementId);
        makeCityViewModel.selectedMakeMaskingName(selectElementMaskingName);
        toggleErrorMsg($("#selectBrand"), false);
        makeCityViewModel.selectedCityId(0);
        $ulCities.empty();
        var selMakeId = makeCityViewModel.selectedMakeId();
        startLoading($("#selectCity"));
        makeCityViewModel.cityName("Loading cities")
        if (!isNaN(selMakeId) && selMakeId != "0") {
            if (!checkCacheCityAreas(selMakeId)) {
                $.ajax({
                    type: "GET",
                    url: "/api/v2/DealerCity/?makeId=" + selMakeId,
                    contentType: "application/json",
                    dataType: 'json',
                    success: function (data) {
                        lscache.set(key + selMakeId, data.City, 30);
                        setOptions(data.City);
                    },
                    complete: function (xhr) {
                        if (xhr.status == 404 || xhr.status == 204) {
                            lscache.set(key + selMakeId, null, 30);
                            setOptions(null);
                        }                       
                    }
                });
            }
            else {
                data = lscache.get(key + selMakeId.toString());
                setOptions(data);
            }
        }
        else {
            setOptions(null);
        }

    }
    else {
        makeCityViewModel.cityName(selectedElementValue);
        makeCityViewModel.selectedCityId(selectedElementId);
        makeCityViewModel.selectedCityMaskingName(selectElementMaskingName);
        toggleErrorMsg($("#selectCity"), false);
    }

    setUserSelection();
});

function checkCacheCityAreas(cityId) {
    bKey = key + cityId;
    if (lscache.get(bKey)) return true;
    else return false;
}

function setOptions(optList) {  
    if (optList != null) {
        
        makeCityViewModel.cityName("Select city");

        $.each(optList, function (i, value) {
            $ulCities.append($('<li>').text(value.cityName).attr({ 'value': value.cityId, 'maskingName': value.cityMaskingName }));
        });
    }

    stopLoading($("#selectCity"));
}

$("#applyDealerFilter").on("click", function () {

    var ulmakeMasking = makeCityViewModel.selectedMakeMaskingName();
    var ulMakeId = makeCityViewModel.selectedMakeId();
    var ulcityId = makeCityViewModel.selectedCityId();      

    if (ulMakeId == "0") {
        toggleErrorMsg($("#selectBrand"), true, "Choose a brand");
    }

    else {
        toggleErrorMsg($("#selectBrand"), false);
    }

    if (ulcityId != "0" && ulMakeId != "0") {
        var ulcityMasking = makeCityViewModel.selectedCityMaskingName();
        toggleErrorMsg($("#selectCity"), false);
        $(".filterBackArrow").trigger("click");        
        window.location.href = "/m/dealer-showrooms/" + ulmakeMasking + "/"+ ulcityMasking + "/";
    }
    else {
        toggleErrorMsg($("#selectCity"), true, "Choose a city");
    }
});



var setUserSelection = function () {
    $(".dealers-brand-city-wrapper .dealers-back-arrow-box").trigger("click");
};

$("#dealerFilterReset").on("click", function () {
    makeCityViewModel.makeName("Select a bike");
    makeCityViewModel.cityName("Select city");
    makeCityViewModel.selectedMakeId(0);
    makeCityViewModel.selectedCityId(0);
    makeCityViewModel.selectedMakeMaskingName("");
    makeCityViewModel.selectedCityMaskingName("");
    $("#dealerFilterContent").find("input").val("");
});

//$("#applyDealerFilter").on("click", function () {
//    $(".filterBackArrow").trigger("click");
//});

//assistance form
$(".get-assistance-btn").on('click', function () {
    leadSourceId = $(this).attr("leadSourceId");
    $("#leadCapturePopup").show();
    appendHash("assistancePopup");
    $("div#contactDetailsPopup").show();
    $("#otpPopup").hide();

    getDealerBikes($(this).attr("data-item-id"), $(this).attr("campId"));
    triggerGA("Dealer_Locator", "Get_Offers_Clicked", makeCityViewModel.makeName() + "_" + makeCityViewModel.cityName());

});


function getDealerBikes(id, campId) {
    var obj = new Object();    
    if (!isNaN(id) && id != "0" && campId != "0") {
        var dealerKey = "dealerDetails_" + id + "_camp_" + campId;
        var dealerInfo = lscache.get(dealerKey);
        if (!dealerInfo) {
            $.ajax({
                type: "GET",
                url: "/api/DealerBikes/?dealerId="+ id + "&campaignId=" + campId,
                contentType: "application/json",
                dataType: 'json',
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('utma', getCookie('__utma'));
                    xhr.setRequestHeader('utmz', getCookie('_bwutmz'));
                },
                success: function (response) {
                    lscache.set(dealerKey, response, 30);
                    bindDealerDetails(response);                    
                },
                complete: function (xhr) {
                    if (xhr.status == 204 || xhr.status == 404) {
                        lscache.set(dealerKey, null, 30);
                    }
                }
            });
        }
        else {
            bindDealerDetails(dealerInfo);            
        }
    }
}

var customerViewModel;
var leadBtnBookNow = $("a.get-assistance-btn"), leadCapturePopup = $("#leadCapturePopup"), fullName = $("#getFullName"), emailid = $("#getEmailID"), mobile = $("#getMobile"), otpContainer = $(".mobile-verification-container"), getModelName = $("#getModelName");
var getCityArea = GetGlobalCityArea();
var binded = false;
function bindDealerDetails(response) {
    obj = ko.toJS(response);
    if (!binded) {
        customerViewModel = new CustomerModel(obj);
        ko.applyBindings(customerViewModel, $('#leadCapturePopup')[0]);
        binded = true;
    }
    else {
        customerViewModel.dealerId(obj.dealerDetails.id);
        customerViewModel.dealerName(obj.dealerDetails.name);
        if (obj.dealerBikes && obj.dealerBikes.length > 0) {             
            customerViewModel.bikes(obj.dealerBikes);
        }
    }
       
}

function setuserDetails() {
    var cookieName = "_PQUser";
    if (isCookieExists(cookieName)) {
        var arr = getCookie(cookieName).split("&");
        return arr;
    }
}

function CustomerModel(obj) {   
    data = obj.dealerBikes;
    var arr = setuserDetails();
    var self = this;
    if (arr != null && arr.length > 0) {
        self.fullName = ko.observable(arr[0]);
        if (arr[1]!="undefined") {
            self.emailId = ko.observable(arr[1]);
        } else {
            self.emailId = ko.observable();
        }
        self.mobileNo = ko.observable(arr[2]);
    }
    else {
        self.fullName = ko.observable();
        self.emailId = ko.observable();
        self.mobileNo = ko.observable();
    }
   
    self.dealerId = ko.observable(obj.dealerDetails.id);
    self.versionId = ko.observable(0);   
    self.IsVerified = ko.observable(false);
    self.NoOfAttempts = ko.observable(0);
    self.IsValid = ko.computed(function () { return self.IsVerified(); }, this);
    self.otpCode = ko.observable();   
    self.pqId = ko.observable();
    self.modelId = ko.observable(0);
    self.bikes = ko.observableArray([]);
    self.dealerName = ko.observable(obj.dealerDetails.name);
    self.selectedBikeName = ko.observable();

    if (obj.dealerBikes && obj.dealerBikes.length > 0) {             
        self.bikes = ko.observableArray(obj.dealerBikes);
    }


    self.verifyCustomer = function () {     
        if (!self.IsVerified()) {
            var objCust = {
                "dealerId": self.dealerId(),
                "pqId": self.pqId(),
                "customerName": self.fullName(),
                "customerMobile": self.mobileNo(),
                "customerEmail": self.emailId(),
                "clientIP": clientIP,
                "pageUrl": pageUrl,
                "versionId": self.versionId(),
                "cityId": makeCityViewModel.selectedCityId(),
                "leadSourceId": leadSourceId,
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
                "pqId": self.pqId(),
                "customerMobile": self.mobileNo(),
                "customerEmail": self.emailId(),
                "cwiCode": self.otpCode(),
                "branchId": self.dealerId(),
                "customerName": self.fullName(),
                "versionId": self.versionId(),
                "cityId": makeCityViewModel.selectedCityId()
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
                "source": pageSrcId
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

    self.generatePQ = function (data, event) {
        self.IsVerified(false);
        isSuccess = false;
        isValidDetails = false;       
        isValidDetails = validateUserDetail();                

        if (isValidDetails && self.modelId() && self.versionId()) {
            var url = '/api/RegisterPQ/';
            var objData = {
                "dealerId": self.dealerId(),
                "modelId": self.modelId(),
                "clientIP": clientIP,
                "pageUrl": pageUrl,
                "versionId": self.versionId(),
                "cityId": makeCityViewModel.selectedCityId(),
                "areaId": 0,
                "sourceType": 2,
                "pQLeadId": pqSrcId,
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
                    }

                }
            });
        }

        return isSuccess;

    }

    self.submitLead = function (data, event) {
        var isValidDetails = self.generatePQ(data, event);
        var btnId = event.target.id;
        $("#dealer-lead-msg").hide();
        if (isValidDetails) {
            self.verifyCustomer();
            if (self.IsValid()) {
                $("#contactDetailsPopup").hide();
                $("#personalInfo").hide();
                $("#otpPopup").hide();
                $("#dealer-lead-msg").fadeIn();

                $(".lead-mobile").text(self.mobileNo());
                $(".notify-dealerName").text(self.dealerName());
                $("#notify-response").show();

                if (btnId == "user-details-submit-btn") {
                    triggerGA("Dealer_Locator", "Lead_Submitted", "Main_Form_" + customerViewModel.selectedBikeName() + "_" + makeCityViewModel.cityName());
                }
            }
            else {
                $("#leadCapturePopup").show();
                $('body').addClass('lock-browser-scroll');
                $(".blackOut-window").show();
                $("#contactDetailsPopup").hide();
                $("#otpPopup").show();
                var leadMobileVal = mobile.val();
                $("#otpPopup .lead-mobile-box").find("span.lead-mobile").text(leadMobileVal);
              
                hideError(mobile);
                otpText.val('').removeClass("border-red").siblings("span, div").hide();
            }
            setPQUserCookie();            
        }
    };

    function setPQUserCookie() {
        var val = customerViewModel.fullName() + '&' + customerViewModel.emailId() + '&' + customerViewModel.mobileNo();
        SetCookie("_PQUser", val);
    }

    $("body").on('click', '#otp-submit-btn', function () {
        $('#processing').show();
        isValidDetails = false;
        if (!validateOTP())
            $('#processing').hide();            
            isValidDetails = validateUserDetail();
        

        if (validateOTP() && isValidDetails) {
            customerViewModel.generateOTP();
            if (customerViewModel.IsVerified()) {
                $("#personalInfo").hide();
                $(".booking-dealer-details").removeClass("hide").addClass("show");
                otpText.val('');
                otpContainer.removeClass("show").addClass("hide");
                $("#personalInfo").hide();
                $("#otpPopup").hide();
            
                $("#dealer-lead-msg").fadeIn();
                
                // OTP Success

                $(".notify-dealerName").text(self.dealerName());
                $("#notify-response").show();              
            }
            else {
                $('#processing').hide();
                otpVal("Please enter a valid OTP.");              
            }
        }
    });
}

$(".leadCapture-close-btn, #notifyOkayBtn").on("click", function () {
    assistancePopupClose();
    window.history.back();
});


var assistancePopupClose = function () {
    $("#leadCapturePopup").hide();
    $("#notify-response").hide();
};

function validateUserDetail() {
    var isValid = true;
    isValid = validateName();
    isValid &= validateEmail();
    isValid &= validateMobile();
    isValid &= validateModel();
    return isValid;
}

var validateName = function () {
    var isValid = true,
        name = $("#getFullName"),
        nameLength = name.val().length;
    if (name.val().indexOf('&') != -1) {
        setError(name, 'Invalid name');
        isValid = false;
    }
    else if (nameLength == 0) {
        setError(name, 'Please enter your name');
        isValid = false;
    }
    else if (nameLength >= 1) {
        hideError(name);
        isValid = true;
    }
    return isValid;
};

var validateEmail = function () {
    var isValid = true,
        emailId = $("#getEmailID"),
        emailVal = emailId.val(),
        reEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;
    if (emailVal == "") {
       
        isValid = true;
    }
    else if (!reEmail.test(emailVal)) {
        setError(emailId, 'Invalid Email');
        isValid = false;
    }
    return isValid;
};

var validateMobile = function () {
    var isValid = true,
        mobileNo = $("#getMobile"),
        mobileVal = mobileNo.val(),
        reMobile = /^[0-9]{10}$/;
    if (mobileVal == "") {
        setError(mobileNo, "Please enter your Mobile Number");
        isValid = false;
    }
    else if (!reMobile.test(mobileVal) && isValid) {
        setError(mobileNo, "Mobile number should be 10 digits");
        isValid = false;
    }
    else
        hideError(mobileNo)
    return isValid;
};

var validateModel = function () {
    var isValid = true,
        model = $('.dealer-search-brand-form');

    if (!model.hasClass('selection-done')) {
        setError(model, 'Please select a bike');
        isValid = false;
    }
    else if (model.hasClass('selection-done')) {
        hideError(model);
        isValid = true;
    }
    return isValid;
};

var setError = function (element, msg) {
    element.addClass("border-red").siblings("span.errorIcon, div.errorText").show();
    element.siblings("div.errorText").text(msg);
};

var hideError = function (element) {
    element.removeClass("border-red").siblings("span.errorIcon, div.errorText").hide();
};

$("#getMobile, #getFullName, #getEmailID, #getModelName, #getUpdatedMobile, #getOTP").on("focus", function () {
    hideError($(this));
});

//otp form
$("#otpPopup .edit-mobile-btn").on("click", function () {
    var prevMobile = $(this).prev("span.lead-mobile").text();
    $(".lead-otp-box-container").hide();
    $(".update-mobile-box").show();
    $("#getUpdatedMobile").val(prevMobile).focus();
});

$("#generateNewOTP").on("click", function () {
    if (validateUpdatedMobile()) {
        var updatedNumber = $(".update-mobile-box").find("#getUpdatedMobile").val();
        $(".update-mobile-box").hide();
        $(".lead-otp-box-container").show();
        $(".lead-mobile-box").find(".lead-mobile").text(updatedNumber);
    }
});

var validateUpdatedMobile = function () {
    var isValid = true,
        mobileNo = $("#getUpdatedMobile"),
        mobileVal = mobileNo.val(),
        reMobile = /^[0-9]{10}$/;
    if (mobileVal == "") {
        setError(mobileNo, "Please enter your Mobile number");
        isValid = false;
    }
    else if (!reMobile.test(mobileVal) && isValid) {
        setError(mobileNo, "Mobile number should be 10 digits");
        isValid = false;
    }
    else
        hideError(mobileNo)
    return isValid;
};

var otpText = $("#getOTP"),
    otpBtn = $("#otp-submit-btn");

otpBtn.on("click", function () {
    if (validateOTP()) {

    }
});

var otpVal = function (msg) {
    otpText.addClass("border-red");
    otpText.siblings("span, div").show();
    otpText.siblings("div").text(msg);
};

function validateOTP() {
    var retVal = true;
    var isNumber = /^[0-9]{5}$/;
    var cwiCode = otpText.val();
    if (cwiCode == "") {
        retVal = false;
        otpVal("Please enter your Verification Code");
    }
    else {
        if (isNaN(cwiCode)) {
            retVal = false;
            otpVal("Verification code should be numeric");
        }
        else if (cwiCode.length != 5) {
            retVal = false;
            otpVal("Verification code should be of 5 digits");
        }
    }
    return retVal;
}

function startLoading(ele) {
    try {
        $("#btnSpinner").removeClass("hide");
        var _self = $(ele).find(".progress-bar").css({ 'width': '0' }).show();
        _self.animate({ width: '100%' }, 7000);
    }
    catch (e) { return };
}

function stopLoading(ele) {
    try {
        $("#btnSpinner").addClass("hide");
        var _self = $(ele).find(".progress-bar");
        _self.stop(true, true).css({ 'width': '100%' }).fadeOut(1000);
    }
    catch (e) { return };
}

function toggleErrorMsg(element, error, msg) {
    if (error) {
        element.find('.error-icon').removeClass('hide');
        element.find('.bw-blackbg-tooltip').text(msg).removeClass('hide');
        element.addClass('border-red')
    }
    else {
        element.find('.error-icon').addClass('hide');
        element.find('.bw-blackbg-tooltip').text("").addClass('hide');
        element.removeClass('border-red');
    }
}

var brandSearchBar = $("#brandSearchBar"),
                dealerSearchBrand = $(".dealer-search-brand"),
                dealerSearchBrandForm = $(".dealer-search-brand-form");
dealerSearchBrand.on('click', function () {
    $('.dealer-brand-wrapper').show();
    brandSearchBar.addClass('open').animate({ 'left': '0px' }, 500);
    brandSearchBar.find(".user-input-box").animate({ 'left': '0px' }, 500);
    $("#assistanceBrandInput").focus();
});
$("#sliderBrandList").on("click", "li", function () {
    var _self = $(this),
        selectedElement = _self.text();
    setSelectedElement(_self, selectedElement);
    _self.addClass('activeBrand').siblings().removeClass('activeBrand');
    dealerSearchBrandForm.addClass('selection-done').find("span").text(selectedElement);
    brandSearchBar.find(".user-input-box").animate({ 'left': '100%' }, 500);
    hideError(dealerSearchBrandForm);
});
function setSelectedElement(_self, selectedElement) {
    _self.parent().prev("input[type='text']").val(selectedElement);
    brandSearchBar.addClass('open').animate({ 'left': '100%' }, 500);
};
$(".dealer-brand-wrapper .back-arrow-box").on("click", function () {
    brandSearchBar.removeClass("open").animate({ 'left': '100%' }, 500);
    brandSearchBar.find(".user-input-box").animate({ 'left': '100%' }, 500);
});
