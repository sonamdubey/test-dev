var Policyboss = {
    insNew: false,
    doc: $(document),
    nameField: $('#txtName'),
    emailField: $('#txtEmail'),
    mobileField: $('#txtMobile'),
    carData: {},
    prefilledModel: false,
    insuranceCity: {},
    registerEvents: function () {
        $('#drpCity').cw_autocomplete({
            resultCount: 8,
            source: ac_Source.allCarCities,
            onClear: function () {

            },
            click: function (event, ui, orgTxt) {
                Policyboss.insuranceCity.Name = Common.utils.getSplitCityName(ui.item.label);
                Policyboss.insuranceCity.Id = ui.item.id;
                $("#hdn_drpCityId").val(Policyboss.insuranceCity.Id);
                ui.item.value = Policyboss.insuranceCity.Name;
                $("#drpcity").val(Policyboss.insuranceCity.Name);
            },
            open: function (result) {
                Policyboss.insuranceCity.result = result;
            },
            afterfetch: function (result, searchtext) {
                if (result != undefined && result.length > 0) {
                    $("#hdn_drpCityId").val(Policyboss.insuranceCity.Id);
                    $("#drpcity").val(Policyboss.insuranceCity.Name);
                    Policyboss.utils.hideErrorMessage($("#drpCity"));
                }
                else {
                    $("#hdn_drpCityId").val(-1);
                    Policyboss.utils.showCustomeErrorMessage($("#drpCity"), "No matching results found.");
                }
            },
            focusout: function () {
                if ($('li.ui-state-focus a:visible').text() != "") {
                    if (Policyboss.insuranceCity == undefined) Policyboss.insuranceCity = new Object();
                    var focused = Policyboss.insuranceCity.result[$('li.ui-state-focus').index()];
                    if (focused != undefined && focused.label == $('#drpCity').val()) {
                        Policyboss.insuranceCity.Name = Common.utils.getSplitCityName(focused.label);
                        Policyboss.insuranceCity.Id = focused.id;
                        $("#hdn_drpCityId").val(Policyboss.insuranceCity.Id);
                    }
                    else {
                        $("#hdn_drpCityId").val(-1);
                        Policyboss.insuranceCity = {};
                    }
                }
            },
        });        

        $('#btnVerify').on('click', function () {
            Policyboss.otpVerification.verifyOtp();
        });

        $('#otp-close').on('click', function () {
            $('#btnSubmit').val('Request a Call');
            Policyboss.otpVerification.hideOtpPopup();
        });


        window.onhashchange = function (e) {
            if (e.oldURL.indexOf('#thankyou') > 0 && ((e.newURL.indexOf('') == 0 || e.newURL.indexOf('#') > 0) && $('#thankYou').is(":visible")))
                Policyboss.onBrowserBack();
        }

        Policyboss.utils.prefillCity();
        Policyboss.utils.prefillRegdate();
    },

    onBrowserBack: function () {
        $('#enquiry-form').removeClass('hide');
        $('#thankYou').addClass('hide');
        window.location.reload();
    },

    utils: {
        checkRegisteration: function (id) {
            var thisText = $(id).text();
            thisText == "Not registered yet" ? $(id).siblings('#txtsDate').attr('disabled', true) : $(id).siblings('#txtsDate').attr('disabled', false);
            $(id).text(thisText == "Not registered yet" ? "Enter registration date" : "Not registered yet");
            return ($(id).text() == "Not registered yet");
        },

        validateInputField: function (field, regex) {
            try {
                if (!regex.test(field.val().toLowerCase())) {
                    return false;
                }

                Policyboss.utils.hideErrorMessage(field);
                return true;
            }
            catch (e) { console.log(e) }
            return false;
        },

        showCustomeErrorMessage: function (field, errMsg) {
            try {
                field.addClass('border-red').siblings('.error-icon,.cw-blackbg-tooltip').removeClass('hide');
                field.siblings('.cw-blackbg-tooltip').text(errMsg);
            }
            catch (e) { console.log(e) }
        },

        hideErrorMessage: function (field) {
            try {
                field.removeClass('border-red').siblings('.error-icon,.cw-blackbg-tooltip').addClass('hide');
            }
            catch (e) { console.log(e) }
        },
        prefillCustomerDetails: function () {
            if ($.cookie('_CustomerName') != null)
                Policyboss.nameField.val($.cookie('_CustomerName'));
            if ($.cookie('_CustEmail') != null && $.cookie('_CustEmail').length > 0)
                Policyboss.emailField.val($.cookie('_CustEmail'));
            if ($.cookie('_CustMobile') != null)
                Policyboss.mobileField.val($.cookie('_CustMobile'));
        },
        setCustomerCookies: function (custName, custEmail, custMobile) {
            document.cookie = '_CustomerName=' + custName + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
            document.cookie = '_CustEmail=' + custEmail + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
            document.cookie = '_CustMobile=' + custMobile + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
        },
        prefillCity: function () {
            var url = window.location.href.toLowerCase();
            var cityId = url.indexOf("cityid") < 0 ? "" : Common.utils.getValueFromQS('cityid');
            if (!(url.indexOf("cityid") < 0) && cityId != "" && cityId > 0) {
                cityId = cityId.indexOf("#") < 0 ? cityId : Common.utils.getValueFromQS('cityid').split("#")[0];
                $.when(Common.utils.ajaxCall({
                    type: 'GET',
                    url: '/webapi/GeoCity/GetCityNameById/?cityid=' + cityId,
                    dataType: 'Json'
                })).done(function (cityName) {
                    $('#drpCity').val(cityName);
                    $('#hdn_drpCityId').val(cityId);                    
                });
            }
            else if (Number(masterCityIdCookie) > 0 && $.trim(masterCityNameCookie) != null && $.trim(masterCityNameCookie) != "" && $.trim(masterCityNameCookie) != "Select City") {
                $('#drpCity').val(masterCityNameCookie);
                $('#hdn_drpCityId').val(masterCityIdCookie);
            }
        },
        prefillRegdate: function () {
            var url = window.location.href.toLowerCase();
            var regdate = url.indexOf("reg") < 0 ? "" : Common.utils.getValueFromQS('reg');
            if (Date.parse(regdate) > 0 && regdate.indexOf("-") > 0)
                $('#txtsDate').val(regdate);
        }
    },

    apiCalls: {
        getMakes: function () {
            return Common.utils.ajaxCall({
                url: '/api/insurance/makes/',
                headers: { "clientid": clientId }
            });
        },

        getModels: function (makeId) {
            return Common.utils.ajaxCall({
                url: '/api/insurance/models/' + makeId + '/',
                headers: { "clientid": clientId }
            });
        },

        getVersions: function (modelId) {
            return Common.utils.ajaxCall({
                url: '/api/insurance/versions/' + modelId + '/',
                headers: { "clientid": clientId }
            });
        },

        getQuote: function (apiInput) {
            return Common.utils.ajaxCall({
                type: 'POST',
                url: '/api/v2/insurance/quote/',
                data: apiInput,
                contentType: "application/x-www-form-urlencoded",
                dataType: 'Json',
                headers: { "clientid": clientId, "sourceID": "1" }
            });
        },

        getCarModelData: function (modelId) {
            return Common.utils.ajaxCall({
                type: 'GET',
                url: '/webapi/CarModelData/GetCarDetailsByModelId/?modelid=' + modelId,
                dataType: 'Json'
            });
        },

        getCarVersionData: function (versionId) {
            return Common.utils.ajaxCall({
                type: 'GET',
                url: '/webapi/CarVersionsData/GetCarDetailsByVersionId/?versionid=' + versionId,
                dataType: 'Json',
            });
        },
    },

    bindView: {
        registerEvents: function () {
            Policyboss.utils.prefillCustomerDetails();

            Policyboss.doc.on('change', '#drpMake', function () {
                if ($(this).val() > 0) {
                    $('#drpModel').empty();
                    $('#drpVersion').empty();
                    $("#drpModel").append($("<option></option>").val(-1).html("Loading..."));
                    $("#drpVersion").append($("<option></option>").val(-1).html("Select Version"));
                    $("#drpVersion, #drpModel").attr('disabled', 'disabled');
                    Policyboss.bindView.bindModels($(this).val());
                }
                else {
                    $("#drpModel,#drpVersion").val(-1).attr('disabled', 'disabled');
                }
            });

            Policyboss.doc.on('change', '#drpModel', function () {
                if ($(this).val() > 0) {
                    $('#drpVersion').empty();
                    $("#drpVersion").append($("<option></option>").val(-1).html("Loading..."));
                    $("#drpVersion").val(-1).attr('disabled', 'disabled');
                    Policyboss.bindView.bindVersions($(this).val());
                }
                else {
                    $("#drpVersion").val(-1).attr('disabled', 'disabled');
                }
            });

            Policyboss.doc.on('click', '#checkRegistered', function () {
                if (!Policyboss.utils.checkRegisteration('#checkRegistered')) {
                    $("#txtsDate").val("");
                    Policyboss.insNew = true;
                }
                else
                    Policyboss.insNew = false;
            });

            Policyboss.doc.on('click', '#btnSubmit', function () {         //call policyboss qoute api
                var isSuccess = Policyboss.bindView.validateInputDetails();
                if (isSuccess) {
                    $('#btnSubmit').val("Processing...");
                    Policyboss.otpVerification.checkMobileVerification();
                    Policyboss.utils.setCustomerCookies(Policyboss.nameField.val(), Policyboss.emailField.val(), Policyboss.mobileField.val());;
                }
                return isSuccess;
            });

            $("#txtsDate").Zebra_DatePicker({
                default_position: "below",
                view: 'years',
                show_icon: false,
                direction: false
            });
            $("input[type='text']").blur(function () {
                if ($.trim($(this).val()) != "")
                    $(this).removeClass("border-red");
                Policyboss.utils.hideErrorMessage($(this));
            });

            $("select").change(function () {
                if ($(this).val() > 0)
                    $(this).removeClass("border-red");
                Policyboss.utils.hideErrorMessage($(this));
            });
        },
        prefill: function(response)
        {
            if(response)
            {
                Policyboss.bindView.prefillCarData();
            }
        },
        prefillModel: function(response)
        {
            if(response)
            {
                $("#drpModel option").each(function () {
                    if ($(this).text() == Policyboss.carData.ModelName) {
                        $(this).attr('selected', 'selected');
                    }
                });
                if (!Policyboss.prefilledModel)
                    Policyboss.bindView.bindVersions(Policyboss.carData.ModelId, Policyboss.bindView.prefillVersion);
            }
        },
        prefillVersion: function (response) {
            if (response) {
                $("#drpVersion option").each(function () {
                    if ($(this).text() == Policyboss.carData.VersionName) {
                        $(this).attr('selected', 'selected');
                    }
                });
                Policyboss.prefilledModel = false;
            }
        },
        prefillCarData: function () {
            var url = window.location.href.toLowerCase();
            var versionId = url.indexOf("versionid") < 0 ? "" : Common.utils.getValueFromQS('versionid');
            var modelId = url.indexOf("modelid") < 0 ? "" : Common.utils.getValueFromQS('modelid');
            if (url.indexOf("modelid") != -1 && modelId != "") {
                modelId = modelId.indexOf("#") < 0 ? modelId : modelId.split("#")[0];
                if (modelId > 0) {
                    $.when(Policyboss.apiCalls.getCarModelData(modelId)).done(function (data) {
                        Policyboss.carData = data;
                        $("#drpMake option").each(function () {
                            if ($(this).text() == data.MakeName) {
                                $(this).attr('selected', 'selected');
                            }
                        });
                        Policyboss.bindView.bindModels(data.MakeId, Policyboss.bindView.prefillModel);
                    });
                }
                Policyboss.prefilledModel = true;
            }
            else if (url.indexOf("versionid") != -1 && versionId != "") {
                versionId = versionId.indexOf("#") < 0 ? versionId : versionId.split("#")[0];
                if (versionId > 0) {
                    $.when(Policyboss.apiCalls.getCarVersionData(versionId)).done(function (data) {
                        Policyboss.carData = data;
                        $("#drpMake option").each(function () {
                            if ($(this).text() == data.MakeName) {
                                $(this).attr('selected', 'selected');
                            }
                        });
                        Policyboss.bindView.bindModels(data.MakeId,Policyboss.bindView.prefillModel);
                    });
                }
            }

        },

        bindMakes: function (callback) {
            try {
                $('#drpMake option:first').text('Loading...');
                $.when(Policyboss.apiCalls.getMakes()).done(function (data) {
                    var obj = {};
                    obj = typeof (data) == "object" ? data : obj;
                    $('#drpMake').find('option:gt(0)').remove();
                    $.each(obj, function (i, opt) { $("#drpMake").append($("<option></option>").val(opt.makeId).html(opt.makeName)); });
                    $('#drpMake option:first').text('Select Make');
                    if (callback)
                        callback(true);
                });
            } catch (e) { console.log(e) };
        },

        bindModels: function (makeId, callback) {
            try {
                $.when(Policyboss.apiCalls.getModels(makeId)).done(function (data) {
                    $("#drpModel").removeAttr('disabled');
                    $('#drpModel option:first').val(-1).text('Select Model');
                    var obj = {};
                    obj = typeof (data) == "object" ? data : obj;
                    $.each(obj, function (i, opt) { $("#drpModel").append($("<option></option>").val(opt.modelId).html(opt.modelName)); });
                    if (callback)
                        callback(true);
                });
            } catch (e) { console.log(e) };
        },

        bindVersions: function (modelId, callback) {
            try {
                $.when(Policyboss.apiCalls.getVersions(modelId)).done(function (data) {
                    $("#drpVersion").removeAttr('disabled');
                    $('#drpVersion option:first').val(-1).text('Select Version');
                    var obj = {};
                    obj = typeof (data) == "object" ? data : obj;
                    $.each(obj, function (i, opt) { $("#drpVersion").append($("<option></option>").val(opt.versionId).html(opt.versionName)); });
                    if (callback)
                        callback(true);
                });
            } catch (e) { console.log(e) };
        },

        submitLead: function (apiInput) {
            try {
                $.when(Policyboss.apiCalls.getQuote(apiInput)).done(function (data) {
                    Policyboss.bindView.processPostSubmit(data);                
                });
            } catch (e) { console.log(e) };
        },

        processPostSubmit: function (data) {
            Policyboss.bindView.showThankYouMsgWithQuote(data);
            $('#btnSubmit').addClass('hide');
        },

        showThankYouMsgWithQuote: function (data) {
            $('#enquiry-form').addClass('hide');
            $('#thankYou').removeClass('hide');
            if (data != null && data.total != null && data.total > 0) {
                var quoteValue = Common.utils.formatNumeric(parseInt(data.total));
                $('#showQuote').removeClass('hide');
                $('#showQuote').find('strong').text(quoteValue);
                $('#clientMsg').text("");
            }
            else if (!data.success)
                $('#clientMsg').text("Sorry we couldn't fetch a quote basis your requirements.");

            Policyboss.bindView.pushState(null, "", "thankyou");
        },

        pushState: function (obj, title, hashStr) {
            window.location.hash = hashStr;
        },

        getApiInput: function () {
            var apiInput = new Object();
            apiInput.Name = Policyboss.nameField.val(),
            apiInput.Email = Policyboss.emailField.val(),
            apiInput.Mobile = Policyboss.mobileField.val(),
            apiInput.VersionId = $('#drpVersion').val(),
            apiInput.CityId = $('#hdn_drpCityId').val(),
            apiInput.CityName = $('#drpCity').val(),
            apiInput.CarPurchaseDate = $('#txtsDate').val(),
            apiInput.InsuranceNew = Policyboss.insNew;
            if (!apiInput.InsuranceNew) {
                var arraytemp = apiInput.CarPurchaseDate.split('-');
                apiInput.CarManufactureYear = $.trim(arraytemp[0]);
            }
            var url = window.location.href.toLowerCase();
            apiInput.UtmCode = url.indexOf("utm") < 0 || Common.utils.getValueFromQS('utm') == "" ? "othersdesktop" : Common.utils.getValueFromQS('utm');
            return apiInput;
        },
        validateInputDetails: function () {
            try {
                isValid = true;
                var name = $.trim(Policyboss.nameField.val());                

                if (name == "") {
                    Policyboss.utils.showCustomeErrorMessage(Policyboss.nameField, "Please enter name.")
                    isValid = false;
                }
                else if (!Policyboss.utils.validateInputField(Policyboss.nameField, /^([-a-zA-Z ']*)$/)) {
                    Policyboss.utils.showCustomeErrorMessage(Policyboss.nameField, "Invalid name.")
                    isValid = false;
                }

                var mobileNo = $.trim(Policyboss.mobileField.val());
                if (mobileNo == "") {
                    Policyboss.utils.showCustomeErrorMessage(Policyboss.mobileField, "Please enter mobile number.");
                    isValid = false;
                }
                else if (!Policyboss.utils.validateInputField(Policyboss.mobileField, /^[6789]\d{9}$/)) {
                    Policyboss.utils.showCustomeErrorMessage(Policyboss.mobileField, "Invalid mobile number.");
                    isValid = false;
                }

                var emailId = $.trim(Policyboss.emailField.val());
                if (emailId == "") {
                    Policyboss.utils.showCustomeErrorMessage(Policyboss.emailField, "Please enter email.")
                    isValid = false;
                }
                else if (!Policyboss.utils.validateInputField(Policyboss.emailField, /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$/)) {
                    Policyboss.utils.showCustomeErrorMessage(Policyboss.emailField, "Invalid email.")
                    isValid = false;
                }
                if (!Policyboss.insNew && $.trim($("#txtsDate").val()) == "") {
                    Policyboss.utils.showCustomeErrorMessage($("#txtsDate"), "Please enter date.");
                    isValid = false;
                }
                else {
                    Policyboss.utils.hideErrorMessage($("#txtsDate"));
                }

                if ($("#drpMake").val() <= 0) {
                    Policyboss.utils.showCustomeErrorMessage($("#drpMake"), "Please select make.");
                    isValid = false;
                }
                else {
                    Policyboss.utils.hideErrorMessage($("#drpMake"));
                }

                if ($("#drpModel").val() <= 0) {
                    Policyboss.utils.showCustomeErrorMessage($("#drpModel"), "Please select model.");
                    isValid = false;
                }
                else {
                    Policyboss.utils.hideErrorMessage($("#drpModel"));
                }

                if ($("#drpVersion").val() <= 0) {
                    Policyboss.utils.showCustomeErrorMessage($("#drpVersion"), "Please select version.");
                    isValid = false;
                }
                else {
                    Policyboss.utils.hideErrorMessage($("#drpVersion"));
                }

                if ($("#drpCity").val() <= 0) {
                    Policyboss.utils.showCustomeErrorMessage($("#drpCity"), "Please enter city.");
                    isValid = false;
                }
                else {
                    Policyboss.utils.hideErrorMessage($("#drpCity"));
                }
                return isValid;
            }
            catch (e) { console.log(e) }
        },

        pageload: function () {
            Policyboss.bindView.bindMakes(Policyboss.bindView.prefill);
            Policyboss.bindView.registerEvents();
            Policyboss.registerEvents();
        },
    },

    otpVerification: {
        checkMobileVerification: function () {
            var mobileNumber = $.trim($("#txtMobile").val());
            var tollFreeNumber;
            $.ajax({
                type: 'GET',
                headers: { "sourceid": "43" },
                url: '/api/mobile/verify/' + mobileNumber + '/',
                dataType: 'Json',
                success: function (json) {
                    if (json != null) {
                        $('#btnMissCallAnchorTag').text(json.tollFreeNumber);
                        if (!json.isMobileVerified) {
                            Policyboss.otpVerification.showOtpPopup();
                        }
                        else {
                            Policyboss.bindView.submitLead(Policyboss.bindView.getApiInput());
                        }
                    }                   
                }
            });
        },
        verifyOtp: function () {
            var enteredOTP = $.trim($("#txtOTP").val());
            var mobileNumber = $.trim($("#txtMobile").val());
            if (enteredOTP !== '') {
                $('#imgLoadingBtnVerify').removeClass('hide');

                $.ajax({
                    type: 'GET',
                    url: '/api/verifymobile/?mobileNo=' + mobileNumber + "&cwiCode=" + enteredOTP,
                    headers: { 'CWK': 'KYpLANI09l53DuSN7UVQ304Xnks=', 'SourceId': '1' },
                    dataType: 'Json',
                    success: function (json) {
                        $('#imgLoadingBtnVerify').addClass('hide');
                        if (json.responseCode == 1) {
                            $('#otpError').addClass('hide');
                            Policyboss.otpVerification.hideOtpPopup();
                            Policyboss.bindView.submitLead(Policyboss.bindView.getApiInput());
                        }
                        else {
                            $('#otpError').removeClass('hide').text('Please enter valid OTP');
                            console.log("verification error: " + json.responseMessage);
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        console.log("error: " + errorThrown);
                    }
                });
            }
            else $('#otpError').removeClass('hide').text('Please enter 5 digit OTP');
        },

        showOtpPopup: function () {
            $('#buyerForm,#m-blackOut-window,#buyerForm .screen2').show();
            Common.utils.lockPopup()
        },

        hideOtpPopup: function () {
            $('#buyerForm,#buyerForm .screen2,#m-blackOut-window').hide();
            Common.utils.unlockPopup()
        }
    },
}
$(document).ready(function () {
    Policyboss.bindView.pageload();
});