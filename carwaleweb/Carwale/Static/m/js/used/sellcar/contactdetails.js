//dependency:-
// 1. static/m/js/used/sellcar/common.js
// 2. easy-autocomplete js
var contactForm = (function () {
    var container, getName, getMobile, getEmail, shareToCarTradeCheckBox;

    var contactData = {};
    var contactDetailsHistory = [];
    var isModalPopuShown = false;
    var isHistoryReset = false;
    if (typeof events !== 'undefined') {
        events.subscribe("citySubmit", setSelectors);
        events.subscribe("citySubmit", registerDomEvents);
        events.subscribe("setContactScreen", setContactScreen);
        events.subscribe("citySubmit", contactFormLoadHandler);
        events.subscribe("historyIndexChanged", pushHistoryIndex);
        events.subscribe("contactDetailsTabClick", contactDetailsTabClickHandler);
        events.subscribe("carDetailsTabClick", carDetailsTabClickHandler);
        events.subscribe("historyIndexPoped", removeFromHistoryIndex);
        events.subscribe("historynull", resetHistory);
    }

    function resetHistory() {
        contactDetailsHistory = [1]; //city selection screen in visible
        isHistoryReset = true;
    }

    function getContactHistoryLength() {
        return contactDetailsHistory.length;
    }

    function setSelectors() {
        container = $('#formContact');
        getName = $('#getName');
        getMobile = $('#getMobile');
        getEmail = $('#getEmail');
        shareToCarTradeCheckBox = $('#ctAdChecbox');
        $(document).on("change", "#getName,#getMobile,#getEmail,#ctAdChecbox", disableNavigationTab);
    }

    function disableNavigationTab() {
        events.publish("updateNavigationTabClick", { tabNameArray: [parentContainer.navigationTab.carDetails], value: false });
    }

    function registerDomEvents() {
        documentReadyFormReset();
        window.addEventListener("beforeunload", parentContainer.onPageUnload);
    }

    function pushHistoryIndex(historyData) {
        if (parentContainer.isTabActive(parentContainer.navigationTab.contact) && historyData && historyData.index && !parentContainer.isPresentInArray(historyData.index, contactDetailsHistory)) {
            contactDetailsHistory.push(historyData.index);
        }
    }

    function removeFromHistoryIndex(historyData) {
        var maxContactHistroyIndex = 2;
        if (parentContainer.isTabActive(parentContainer.navigationTab.contact) && historyData && historyData.index > maxContactHistroyIndex && parentContainer.isPresentInArray(historyData.index, contactDetailsHistory)) {
            contactDetailsHistory.pop();
        }
    }

    function contactDetailsTabClickHandler() {
        if (!parentContainer.isPresentInArray(historyHelper.getHistoryIndex(), contactDetailsHistory)) {
            var index = contactDetailsHistory[contactDetailsHistory.length - 1] - historyHelper.getHistoryIndex();
            historyObj.goToIndex(index);
            events.publish("updateHistoryIndex", { index: index + 1 }); // need to +1 because goToIndex reduces history index by 1 because of popstate
        }
    }

    function carDetailsTabClickHandler() {
        container.hide();
    }

    function documentReadyFormReset() {
        if (getName.val().length > 0) {
            getName.text('');
        }

        if (getMobile.val().length > 0) {
            getMobile.text('');
        }

        if (getEmail.val().length > 0) {
            getEmail.text('');
        }
    }

    function setContactScreen() {
        if (container && !modalPopup.closeActiveModalPopup()) {
            parentContainer.setButtonText('', 'next');
            summary.hideToolbar();
            container.show();
            parentContainer.setNavigationTab('formContact');
            parentContainer.setButtonTarget('history.back()', 'contactForm.submitForm()');
        }
    };

    function contactFormLoadHandler(eventObj) {
        if (eventObj && eventObj.data) {
            contactData = appState.setSelectedData(contactData, eventObj.data);
        }
    };

    function submitForm() {
        if (validateContactForm()) {
            parentContainer.setLoadingScreen();
            contactData = appState.setSelectedData(contactData, { name: $.trim(getName.val()), mobile: $.trim(getMobile.val()), email: $.trim(getEmail.val().toLowerCase()), shareToCT: shareToCarTradeCheckBox.is(':checked') });
            var partialCardetails = $('.partialCarDetails');
            var url = "/api/used/sell/contactdetails/" + ($.cookie("TempInquiry") ? ("?tempid=" + encodeURIComponent($.cookie("TempInquiry"))) : "");
            var settings = {
                url: url,
                type: "POST",
                data: contactData
            }
            $.ajax(settings).done(function (response) {
                if (response && response.tempInquiryId) {
                    if (!isHistoryReset && !isModalPopuShown && contactForm.getContactHistoryLength() > 1 && $("#formCarDetail.form-screen-group").length) // cardetails screen is already loaded
                    {
                        events.publish("contactDetailsChanged", { data: contactData });
                        parentContainer.setNavigationTab(parentContainer.navigationTab.carDetails);
                        var goIndex = 1;
                        historyObj.goToIndex(goIndex);
                        events.publish("updateHistoryIndex", { index: goIndex + 1 }); // need to add 1 because goToIndex reduces index by 1 due to popstate
                        parentContainer.removeLoadingScreen();
                        return;
                    }
                    sellCarCookie.deleteSellInquiryCookie();
                    sellCarCookie.setTempInquiryCookie(response.tempInquiryId);
                    var settings = {
                        url: 'cardetails/',
                        data: { newCustomer: response.newCustomer }
                    };
                    $.ajax(settings).done(function (response) {
                        container.hide();
                        parentContainer.setNavigationTab(parentContainer.navigationTab.carDetails);
                        historyObj.addToHistory('selectMakeYear');
                        sellCarTracking.forMobile("contact", (shareToCarTradeCheckBox.is(':checked')).toString());
                        partialCardetails.html(response);
                        if (typeof events !== 'undefined') {
                            var eventObj = {
                                data: contactData
                            };
                            events.publish("contactSubmit", eventObj);
                            events.publish("cardetailsViewLoaded");
                        }
                        isModalPopuShown = false;
                        isHistoryReset = false;
                    }).fail(function (xhr) {
                        parentContainer.removeLoadingScreen();
                        modalPopup.showModal(xhr.responseText);
                        isModalPopuShown = true;
                    });
                }
            }).fail(function (xhr) {
                modalPopup.showModalJson(xhr.responseText);
                historyObj.addToHistory('showModal');
                parentContainer.removeLoadingScreen();
                isModalPopuShown = true;
            });
            summary.setSummary(container);
        }
    };

    function validateContactForm() {
        var isValid = false;
        isValid = validateForm.userName(getName);
        isValid &= validateForm.userMobile(getMobile);
        isValid &= validateForm.userEmail(getEmail);

        return isValid;
    };

    var validateForm = {
        userName: function (nameField) {
            var isValid = false,
				nameValue = nameField.val(),
				reName = /^([-a-zA-Z ']*)$/;

            if (nameValue == "") {
                validate.field.setError(nameField, 'Please provide your name');
            } else if (!reName.test(nameValue)) {
                validate.field.setError(nameField, 'Please provide only alphabets');
            } else if (nameValue.length == 1) {
                validate.field.setError(nameField, 'Please provide your complete name');
            }
            else {
                validate.field.hideError(nameField);
                isValid = true;
            }

            return isValid;
        },

        userMobile: function (mobileField) {
            var isValid = false,
				mobileValue = mobileField.val(),
				reMobile = /^[6789]\d{9}$/;

            if (mobileValue == "") {
                validate.field.setError(mobileField, 'Please provide your mobile number');
            } else if (mobileValue.length != 10) {
                validate.field.setError(mobileField, 'Enter your 10 digit mobile number');
            } else if (!reMobile.test(mobileValue)) {
                validate.field.setError(mobileField, 'Please provide a valid 10 digit Mobile number');
            }
            else {
                validate.field.hideError(mobileField);
                isValid = true;
            }

            return isValid;
        },

        userEmail: function (emailField) {
            var isValid = false,
				emailValue = emailField.val(),
				reEmail = /^[a-z0-9._-]+@([a-z0-9-]+\.)+[a-z]{2,6}$/;

            if (emailValue == "") {
                validate.field.setError(emailField, 'Please provide your Email Id');
            } else if (!reEmail.test(emailValue.toLowerCase())) {
                validate.field.setError(emailField, 'Invalid Email Id');
            }
            else {
                validate.field.hideError(emailField);
                isValid = true;
            }

            return isValid;
        }
    };

    return { submitForm: submitForm,getContactHistoryLength: getContactHistoryLength };

})();

var cityForm = (function () {
    var container, cityField, popularCityList, cityTagList, pincodeInputBox, pincodeField, sellCarTabs;

    var cityData = {};

    if (typeof events !== 'undefined') {
        events.subscribe("cityDetailsDocReady", setSelectors);
        events.subscribe("cityDetailsDocReady", registerDomEvents);
        events.subscribe("cityDetailsDocReady", detectCity);
        events.subscribe("historynull", pillCancelClickHandler);
        events.subscribe("setPincodeScreen", setPincodeScreen);
    }
    function detectCity()
    {
        var isSelected = false;
        var cityId = '';
        if (typeof getQueryStringParam != 'undefined'&& (cityId = getQueryStringParam('city')) != '') {
            var settings = {
                url: '/webapi/GeoCity/GetCityNameById/?cityid=' + cityId,
                type: "GET"
            }
            $.ajax(settings).done(function (cityName) {
                if (cityName) {
                    processCitySelection(cityId, cityName.replace(/^"(.*)"$/, '$1'));
                }
            });
        } else if (!setCityFromGlobal()) {
            setCityFromGeoLocation();
        }
    }
    function setCityFromGlobal() {
        var cityId = $.cookie("_CustCityIdMaster");
        var cityName = $.cookie("_CustCityMaster");
        var isCityDetected = false;

        if (cityId && cityId !== "-1" && cityName) {
            processCitySelection(cityId, cityName);
            isCityDetected = true;
        }
        return isCityDetected;
    }

    function isProcessCitySelection(city) {
        return (city && city.id && city.name && !cityField.val());
    }

    function setCityFromGeoLocation() {
        if (typeof geoLocation != 'undefined') {
            geoLocation.getCurrentCity().then(function (city) {
                setTimeout(function () { // to avoid race condition between user city selection and automatic city selection
                    if (isProcessCitySelection(city))
                        processCitySelection(city.id, city.name);
                }, 100);
            }).catch(function (error) {
                console.log(error);
            });
        }
    }

    function setSelectors() {
        container = $('#formCity');
        cityField = $('#bodyCitySelect #getCity');
        popularCityList = $('#popularCityList');
        cityTagList = $('#cityTagList');
        pincodeInputBox = $('#pincodeInputBox');
        pincodeField = $('#getPincode');
        sellCarTabs = $('#sellCarTabs');
    }

    function registerDomEvents() {
        if (cityField.val().length > 0) {
            cityField.text('');
        }

        popularCityList.find('input[name="citySelect"]:checked').prop('checked', false);

        $(cityField).cw_easyAutocomplete({
            inputField: $(cityField),
            resultCount: 5,
            source: ac_Source.globalCityLocation,

            click: function (event) {
                var cityPayload = $(cityField).getSelectedItemData().payload;
                var selectionLabel = $.trim(cityPayload.cityName);
                var selectionId = $.trim(cityPayload.cityId);
                processCitySelection(selectionId, selectionLabel);
            },

            keyup: function () {
                if ($(cityField).val().length === 0) {
                    popularCityList.show();
                }
            },

            afterFetch: function (result, searchText) {
                if (result.length <= 0) {
                    validate.field.setError(cityField, 'Sorry! No matching results found. Try again.');
                }
                else {
                    validate.field.hideError(cityField);
                }
                popularCityList.hide();
            },

            focusout: function () {
                if (!container.hasClass('city-select-done')) {
                    popularCityList.show();
                }
            }
        });
        $(cityField).on('focus', function () {
            $('html, body').animate({
                scrollTop: container.offset().top
            });
        });

        $(cityTagList).on('click', '.pill--active', function () {
            history.back();
        });

        $(popularCityList).on('change', 'input[name=citySelect]', function () {
            if ($(this).val().length != 0) {
                var selectionLabel = $.trim($(this).next('label').find('.list-item__label').text());
                var selectionVal = $.trim($(this).val());

                processCitySelection(selectionVal, selectionLabel);
                $('html, body').animate({
                    scrollTop: container.offset().top
                });
                disableNavigationTab();
            }
        });

        $(pincodeField).on('change', function () {
            var areaId = $.trim($(this).val());
            var selectedText = $(this).find('option:selected').text();
            var pincode = $.trim(selectedText.split(',')[0]);
            pincodeInputBox.attr('data-pincode', pincode);
            pincodeInputBox.attr('data-areaid', areaId);
            disableNavigationTab();
        });

        $(pincodeInputBox).on('keydown', '.chosen-container input', function (e) {
            var eventKeyCode = e.keyCode || e.which;

            if (eventKeyCode == 9 || eventKeyCode == 13) {
                setPincodeUserValue($(this));
            }
        });

        $(pincodeInputBox).on('keyup', '.chosen-container input', function (e) {
            var inputValue = $(this).val();
            var eventKeyCode = e.keyCode || e.which;
            if (eventKeyCode != 13 && eventKeyCode != 9 && inputValue.length < 7) {
                $(this).val('');
                $(this).val(inputValue);
                pincodeInputBox.attr('data-pincode', inputValue);
                pincodeInputBox.attr('data-areaid', -1);
            }
            else {
                var truncateValue = inputValue.substr(0, 6);
                $(this).val(truncateValue);
                $(this).closest('.chosen-container').find('.no-results span').text(truncateValue);
            }
        });

        $(pincodeInputBox).on('blur', '.chosen-container input', function (e) {
            setPincodeUserValue($(this));
        });

        $(pincodeInputBox).on('click', '.no-results', function () {
            var inputField = $(this).closest('.chosen-container').find('input');
            chosenSelect.noResultSelection(pincodeField, inputField);
        });

        $('#submitCityBtn').on('click', function () {
            if (validatePinCode()) {
                cityData = setPincodeData(pincodeInputBox.attr('data-pincode'), pincodeInputBox.attr('data-areaid'));
                var settings = {
                    url: "/api/used/sell/c2bcity/?cityId=" + cityForm.getCityData().cityId,
                    type: "GET",
                }
                parentContainer.setLoadingScreen();
                $.ajax(settings).done(function (response) {
                    parentContainer.removeIntroScreen(response);
                }).fail(function (xhr) {
                    modalPopup.showModalJson(xhr.responseText);
                });
                parentContainer.removeLoadingScreen();
                parentContainer.setButtonTarget('history.back()', 'contactForm.submitForm()');
                if (contactForm.getContactHistoryLength() > 1) // contact screen is visited earlier
                {
                    var goIndex = 1;
                    historyObj.goToIndex(goIndex);
                    events.publish("updateHistoryIndex", { index: goIndex + 1 }); // need to add 1 because gotoindex reduces index by 1 due to popstate
                }
                else {
                    historyObj.addToHistory('contactDetails');
                    if (typeof events !== 'undefined') {
                        var eventObj = {
                            data: cityData
                        };
                        events.publish('citySubmit', eventObj);
                    }
                }
                sellCarTracking.forMobile("pin");
            }
        });

        bindChoosenToPincode();

        $(sellCarTabs).on('click', '.cw-tabs .cw-tabs__item', function () {
            var scrollPosition = sellCarTabs.offset().top;

            $('html, body').animate({
                scrollTop: scrollPosition
            });
        });
    };

    function disableNavigationTab() {
        events.publish("updateNavigationTabClick", { tabNameArray: [parentContainer.navigationTab.carDetails], value: false });
    }

    function processCitySelection(cityId, cityName) {
        fetchPincode(cityId).done(function (resp) { bindPincodeDropDown(pincodeField, resp) });
        cityData = appState.setSelectedData(cityData, { cityId: cityId, cityName: cityName });
        if (typeof events != 'undefined') {
            events.publish("cityChanged", { cityId: cityData.cityId });
        }
        citySelectionDone();
        selectionTag.attach(cityTagList, cityName);
    }

    function validatePinCode() {
        return validateForm.pincode(pincodeInputBox.find('#getPincode'));
    };

    function setPincodeData(pincode, areaId) {
        if (pincode) {
            cityData = appState.setSelectedData(cityData, { pincode: pincode });
        }
        else {
            cityData = appState.deleteObjectProperties(cityData, ["pincode"])
        }
        if (areaId !== "-1") {
            cityData = appState.setSelectedData(cityData, { areaId: areaId });
        }
        else {
            cityData = appState.deleteObjectProperties(cityData, ["areaId"])
        }
        return cityData;
    };

    function clearAllProperties(obj) {
        obj = {};
        return obj;
    }

    function fetchPincode(cityId) {
        var url = "/api/locations/areacode/?cityId=" + cityId;
        return ajaxRequest.getJsonPromise(url);
    };

    function bindPincodeDropDown(pincodeField, resp) {
        if (resp) {
            var bindingObj = resp.map(function (obj) {
                return { val: obj.AreaId, text: obj.Pincode + ', ' + obj.AreaName };
            });
            pincodeField.append(templates.fillDropDownTemplate(bindingObj).join(''));
            pincodeField.trigger('chosen:updated');
        }
    };

    function setPincodeUserValue(inputField) {
        var inputValue = inputField.val();

        if (inputValue && inputValue.length == 6) {
            inputField.val(inputValue);
            chosenSelect.noResultSelection(pincodeField, inputField);
            sellCarTracking.forMobile("pin");
        }
    }

    function citySelectionDone() {
        container.addClass('city-select-done');
        historyObj.addToHistory("selectPincode");
        popularCityList.show();
    };

    function resetForm() {
        container.removeClass('city-select-done');
        formField.resetInput(cityField);
        popularCityList.find('input[type=radio]:checked').attr('checked', false);
        formField.resetSelect(pincodeField);
    };

    function bindChoosenToPincode() {
        $(pincodeField).chosen({
            width: '100%',
            no_results_text: 'Select:'
        })
        $(pincodeField).closest('.select-box').find('.chosen-search input').prop('type', 'number');
    };

    function setPincodeScreen() {
        parentContainer.setIntroScreen();
    };

    function pillCancelClickHandler() {
        selectionTag.detach($(cityTagList).find('.pill--active'));
        cityData = clearAllProperties(cityData);
        resetForm();
    };

    function getCityData() {
        return cityData;
    }

    var validateForm = {
        pincode: function (pincodefield) {
            var fieldData = pincodeInputBox.attr('data-pincode');
            var isValid = false;
            if (!userPincode.validate(fieldData)) {
                validate.field.setError(pincodefield, fieldData && fieldData.trim() ? 'Select correct Pincode' : 'Select pincode');
                pincodefield.closest('.field-box').addClass('done');
            }
            else {
                isValid = true;
            }
            return isValid;
        }
    }

    return { getCityData: getCityData };
})();


$(document).ready(function () {
    if (typeof events !== 'undefined') {
        events.publish("cityDetailsDocReady");
    }
    sellCarCookie.deleteSellInquiryCookie();
    sellCarCookie.deleteTempInquiryCookie();
    sellCarTracking.forMobile("pageLoad");

});