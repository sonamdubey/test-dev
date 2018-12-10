// dependency
// 1. static/m/js/used/sellcar/common.js
// 2. staic/js/used/sellcar/utilities.js

var carDetailsForm = (function () {
    var container, screenGroup, monthField, yearField, manufactureYearForm, carSelectForm, popularMakeList, carMakeField, carModelField,carVersionField,carSelectionTagList, alternateFuelBody, colorSelectionForm, ownerSelectionForm, kmsField, expectedPriceForm, insuranceForm, getExpectedPrice, registrationForm, insuranceMonthField, insuranceYearField;
    var isCarImagesViewLoaded = false;
    var carDetailsData = {};
    var registrationType;
    var carDetailsHistoryActive = [];
    var modelId, versionId; // used to show prefilled value
    if (typeof events !== 'undefined') {
        // this form will load async, hence no document ready function
        //The caller needs to publish 'contactSubmit' event to show the form
        events.subscribe("contactSubmit", carDetailsFormLoadHandler);
        events.subscribe("setContactScreen", setContactScreen);
        events.subscribe("setInsuranceScreen", setInsuranceScreen);
        events.subscribe("setYearScreen", setYearScreen);
        events.subscribe("setCarSelectionScreen", setCarSelectionScreen);
        events.subscribe("setColorScreen", setColorScreen);
        events.subscribe("setOwnerScreen", setOwnerScreen);
        events.subscribe("setKmsScreen", setKmsScreen);
        events.subscribe("setExpectedPriceScreen", setExpectedPriceScreen);
        events.subscribe("carImageHtmlSuccess", onCarImageHtmlSucces);
        events.subscribe("updateCarDetails", updateCarDetails);
        events.subscribe("buyingIndexSuccess", buyingIndexSuccess);
        events.subscribe("buyingIndexFailed", buyingIndexFailed);
        events.subscribe("takenLive", setInsuranceScreen);
        events.subscribe("preventBack", preventBack);
        events.subscribe("navigateAwayInsurance", navigateAwayInsurance);
        events.subscribe('navigateAwayCarDetails', navigateAwayCarDetails);
        events.subscribe("historyIndexChanged", pushHistoryIndex);
        events.subscribe("carDetailsTabClick", carDetailsTabClickHandler);
        events.subscribe("contactDetailsTabClick", contactDetailsTabClickHandler);
        events.subscribe("historyIndexPoped", removeFromHistoryIndex);
        events.subscribe("historynull", clearHistory);
        events.subscribe("contactDetailsChanged", updateCarDetailsFromContact);
    };

    function navigateAwayCarDetails() {
        window.removeEventListener("beforeunload", parentContainer.onPageUnload);
        location.href = '/used/sell/';
    }

    function navigateAwayInsurance() {
        history.back();
        parentContainer.setLoadingScreen();
        events.publish('navigateAway', { container: container });
    }
    function getSelectedData(obj, prop) {
        if (obj.hasOwnProperty(prop)) {
            return obj[prop];
        }
        return null;
    };

    function carDetailsFormLoadHandler(eventObj) {
        setSelectors();
        registerDomEvents();
        init();
        setYearScreen();
        if (eventObj && eventObj.data) {
            carDetailsData = appState.setSelectedData(carDetailsData, { sellCarCustomer: eventObj.data, shareToCT: eventObj.data.shareToCT, areaId: eventObj.data.areaId, pincode: eventObj.data.pincode });
        }
    };
    function updateCarDetailsFromContact(eventObj) {
        if (eventObj && eventObj.data) {
            carDetailsData = appState.setSelectedData(carDetailsData, { sellCarCustomer: eventObj.data, shareToCT: eventObj.data.shareToCT, areaId: eventObj.data.areaId, pincode: eventObj.data.pincode });
        }
    }
    function updateCarDetails(eventObj) {
        if (eventObj) {
            carDetailsData = appState.setSelectedData(eventObj, true);
            $("#summaryToolbar").find("li[data-field-id='getMobile'] .item-value__data").text(eventObj.sellCarCustomer.mobile);
        }
    }

    function setSelectors() {
        container = $('#formCarDetail');
        screenGroup = $('#formCarDetail.form-screen-group');
        monthField = $('#getMonth');
        yearField = $('#getYear');
        manufactureYearForm = $('#bodyYearform');
        carSelectForm = $('#bodyCarForm');
        popularMakeList = $('#popularMakeList');
        carMakeField = $('#getCarMake');
        carModelField = $('#getCarModel');
        carVersionField = $('#getCarVersion');
        carSelectionTagList = $('#carSelectionTagList');
        alternateFuelBody = $('#alternateFuelBody');
        colorSelectionForm = $('#bodyColorForm');
        otherColorField = $('#otherColorField');
        ownerSelectionForm = $('#bodyOwnerForm');
        kmsField = $('#getKms');
        expectedPriceForm = $('#bodyExpectedPrice');
        getExpectedPrice = $('#getExpectedPrice');
        insuranceForm = $('#bodyInsurance');
        insuranceMonthField = $('#getInsuranceMonth');
        insuranceYearField = $('#getInsuranceYear');
        registrationForm = $('#getRegistration');
        registrationType = $('#getRegistraionType');
    };

    function init() {
        var selectBox = container.find('.select-box');

        selectBox.each(function () {
            var element = $(this);

            element.find('.chosen-select').chosen({
                width: '100%'
            });

            if (element.hasClass('select-box-no-input')) {
                chosenSelect.removeInputField(element);
            }
        });

        parentContainer.removeLoadingScreen();
    };

    function registerDomEvents() {
        popularMakeList.on('change', 'input[type=radio]', function () {
            if ($(this).val() !== "0") {
                var selectionId = $(this).val();

                carSelectForm.find('#getCarMake').val(selectionId).change();
            }
        });

        manufactureYearForm.on('change', '.select-box select', function () {
            if (carSelectForm.find('#getCarMake').val() !== "0") {
                resetCarSelectionForm();
                resetCarSelectionScreen();
                resetColorForm();
                resetOwnerForm();
                resetExpectedPriceForm();
            }

            if ($(this).attr('id') == 'getMonth') {
                yearField.trigger('chosen:open');
            }
        });

        carSelectForm.on('focus', '.chosen-container input', function () {
            var scrollPosition = $('#bodyCarForm .screen__head').outerHeight(true) + $('#carSelectionTagList').height();

            $(carSelectForm).animate({
                scrollTop: scrollPosition
            });
        });

        carSelectForm.on('change', '.step-group select', function () {
            var selectedItem = $(this).find('option:selected');

            if (selectedItem.val() !== "0") {
                var selectionText = $(this).find('option:selected').text(),
                    selectionId = $.trim($(this).find('option:selected').val()),
                    groupItem = $(this).closest('.step-group__item');

                var tagListContainer = $(this).closest('.step-group').find('.step-group__selected'),
                    tagItem = '<li class="btn-pill pill--auto-12 pill--active" data-id="' + $(this).attr('id') + '">' + selectionText + '<span class="pill__cross"></span></li>';

                tagListContainer.show().find('ul').append(tagItem);

                groupItem.hide().next('.step-group__item').show();

                switch ($(this).attr('id')) {
                    case 'getCarMake':
                        carDetailsData = appState.setSelectedData(carDetailsData, { makeId: selectionId });
                        parentContainer.setLoadingScreen();
                        fetchModel(selectionId, yearField.val()).done(function (resp) {
                            bindModelDropDown(resp, carModelField);
                            parentContainer.removeLoadingScreen();
                            carModelField.trigger('chosen:results_without_focus');
                            prefillModel();
                            // scroll
                            carSelectForm.animate({
                                scrollTop: carSelectForm.height()
                            });
                        }).fail(function () {
                            parentContainer.removeLoadingScreen();
                            validateScreen.setError('No model found for this selection');
                        });
                        alternateFuelBody.hide();
                        break;

                    case 'getCarModel':
                        carDetailsData = appState.setSelectedData(carDetailsData, { modelId: selectionId });
                        parentContainer.setLoadingScreen();
                        fetchVersion(selectionId, yearField.val()).done(function (resp) {
                            bindVersionDropDown(resp, carVersionField);
                            parentContainer.removeLoadingScreen();
                            carVersionField.trigger('chosen:results_without_focus');
                            prefillVersion();
                            // scroll
                            carSelectForm.animate({
                                scrollTop: carSelectForm.height()
                            });
                        }).fail(function () {
                            parentContainer.removeLoadingScreen();
                            validateScreen.setError('No version found for this selection');
                        });
                        alternateFuelBody.hide();
                        break;

                    case 'getCarVersion':
                        carDetailsData = appState.setSelectedData(carDetailsData, { versionId: selectionId });
                        if (typeof events != 'undefined') {
                            events.publish('versionChanged', { makeId: carDetailsData.makeId, modelId: carDetailsData.modelId, versionId: carDetailsData.versionId, });
                        }
                        var colorSelectionUl = colorSelectionForm.find(".option-list");
                        colorSelectionUl.html('');
                        fetchColor(selectionId).done(function (resp) {
                            bindColor(resp, colorSelectionUl)
                        });
                        alternateFuelBody.show();
                        break;

                    default:
                        break;
                }

                resetColorForm();
                resetOwnerForm();
                resetExpectedPriceForm();
            }

            validateScreen.hideError();
        });

        carSelectionTagList.on('click', '.pill--active', function () {
            var stepGroup = $(this).closest('.step-group'),
                fieldId = $(this).attr('data-id');

            $(this).nextAll().remove();
            if (!$(this).siblings().length) {
                carSelectionTagList.hide();
            }
            $(this).remove();

            switch (fieldId) {
                case 'getCarMake':
                    carDetailsData = appState.deleteObjectProperties(carDetailsData, ["makeId"]);
                    resetCarSelectionForm();
                    break;

                case 'getCarModel':
                    carDetailsData = appState.deleteObjectProperties(carDetailsData, ["modelId"]);
                    formField.resetSelect(carVersionField);
                    break;

                case 'getCarVersion':
                    carDetailsData = appState.deleteObjectProperties(carDetailsData, ["versionId"]);
                    break;

                default:
            }

            stepGroup.find('.step-group__item').hide();

            var selectField = stepGroup.find('#' + fieldId);
            selectField.closest('.step-group__item').show();
            selectField.closest('.field-box').removeClass('done').find('.chosen-container').removeClass('chosen-container-active');
            selectField.val("0").change();
            selectField.trigger("chosen:updated");
            if (fieldId !== "getCarMake") {
                selectField.trigger('chosen:results_without_focus');
            }

            alternateFuelBody.hide();

            resetColorForm();
            resetOwnerForm();
            resetExpectedPriceForm();
        });

        otherColorField.on('focus', 'input', function () {
            var scrollPosition = colorSelectionForm.find('.screen__head').outerHeight(true) + colorSelectionForm.find('#getColor').height();

            $(colorSelectionForm).animate({
                scrollTop: scrollPosition
            });
        });

        colorSelectionForm.on('change', 'input[name=carColour]', function () {
            if ($(this).val() == 0) {
                otherColorField.show();
                colorSelectionForm.animate({
                    scrollTop: colorSelectionForm.height()
                });
            }
            else {
                otherColorField.hide();
                submitColour();
            }

            validateScreen.hideError();
        });

        ownerSelectionForm.on('change', 'input[name=carOwner]', function () {
            validateScreen.hideError();
            submitOwner();
            resetExpectedPriceForm();
        });

        formatValue.formatValueOnInput(kmsField);

        $('#kmsInputBox').on('focus', 'input', function () {
            $('#floatButton').hide();
        });

        $('#kmsInputBox').on('blur', 'input', function () {
            $('#floatButton').delay(200).fadeIn();
        });

        // recommended price
        expectedPriceForm.on('click', '#recommendedCheckboxLabel', function () {
            expectedPriceForm.find('#recommendedCheckbox').trigger('click');
        });

        expectedPriceForm.on('change', '#recommendedCheckbox', function () {
            if ($(this).is(':checked')) {
                getExpectedPrice.val(Common.utils.formatNumeric($(this).val()));
                getExpectedPrice.attr('data-value', $(this).val());
                validate.field.hideError(getExpectedPrice);
                formatValue.readableTextFromNumber(getExpectedPrice);
            }
            else {
                getExpectedPrice.attr('data-value', '');
                getExpectedPrice.val('').closest('.field-box').removeClass('done');
                getExpectedPrice.siblings("div .getNumbersInWord").text("");
            }
        });

        expectedPriceForm.on('keyup', '#getExpectedPrice', function () {
            var recommendedCheckbox = expectedPriceForm.find('#recommendedCheckbox');
            if (recommendedCheckbox.is(':checked')) {
                recommendedCheckbox.prop('checked', false);
            }
        });

        formatValue.formatValueOnInput(getExpectedPrice);

        $('#expectedPriceInputBox').on('focus', 'input', function () {
            var scrollPosition = expectedPriceForm.find('.screen__head').outerHeight(true) + expectedPriceForm.find('.recommend-price-box').outerHeight(true) - 40;

            $('#bodyExpectedPrice').animate({
                scrollTop: scrollPosition
            });
        });

        // insurance
        insuranceForm.on('change', 'input[name=carInsurance]', function () {
            if ($(this).val() != 3) {
                $('#insuranceValidity').show();
                insuranceForm.addClass('validity-active');

                var scrollPosition = insuranceForm.find('.screen__head').outerHeight(true) + insuranceForm.find('#getInsurance').height() - 20;

                insuranceForm.animate({
                    scrollTop: scrollPosition
                });
            }
            else {
                $('#insuranceValidity').hide();
                insuranceForm.removeClass('validity-active');
                formField.emptySelect(insuranceMonthField);
                formField.emptySelect(insuranceYearField);
            }
        });

        insuranceForm.on('change', '#getInsuranceMonth', function () {
            if (parseInt($(this).val()) > 0) {
                insuranceForm.find('.register--input-1').blur();
                insuranceYearField.trigger('chosen:open');
            }
        });

        insuranceForm.on('change', '#getInsuranceYear', function () {
            if (parseInt($(this).val()) > 0) {
                insuranceForm.find('.register--input-1').focus();
            }
        });

        insuranceForm.on('keyup', '.register--input-1', function () {
            if ($(this).val().length == 2) {
                $(this).next('.register--input-2').focus();
            }
        });

        insuranceForm.on('mouseup', '.chosen-container', function () {
            $('#registrationNumber').find('input').blur();
        });

        $('#registrationNumber').on('focus', 'input', function () {
            var scrollPosition = insuranceForm.find('.screen__head').outerHeight(true) + insuranceForm.find('#getInsurance').height() + insuranceForm.find('#insuranceValidity').height() + 80;

            $(insuranceForm).animate({
                scrollTop: scrollPosition
            });
        });

        $('#registrationType').on('change', '#getRegistraionType', function () {
            var registrationType = $('#registrationType');
            if ($(this).val() != 0 && registrationType.hasClass('invalid')) {
                registrationType.removeClass('invalid').find('.error-text').text('');
            }
            if ($(this).val() != 0) {
                registrationType.find('.select-label').hide();
            }
        });
    };

    function resetBuyingIndexPrice(expectedPriceField) {
        expectedPriceField.find('#recommendedCheckbox').val('');
        expectedPriceField.find('#recommendedPriceText').text('');
        expectedPriceField.find('.recommend-price-box').hide();
    }

    function bindBuyingIndexPrice(resp, expectedPriceField) {
        expectedPriceField.find('#recommendedCheckbox').val(resp.right_price);
        expectedPriceField.find('#recommendedPriceText').text(Common.utils.formatNumeric(resp.right_price));
        expectedPriceField.find('.recommend-price-box').show();
    };

    function buyingIndexSuccess(resp) {
        if (resp && resp.right_price) {
            bindBuyingIndexPrice(resp, expectedPriceForm);
            carDetailsData = appState.setSelectedData(carDetailsData, { RecommendedPrice: $.trim(resp.right_price) });
            sellCarTracking.forMobile("recomPriceShown");
        }
        else {
            resetBuyingIndexPrice(expectedPriceForm);
        }
    }

    function buyingIndexFailed(response) {
        resetBuyingIndexPrice(expectedPriceForm);
    }

    function preventBack() {
        historyObj.addToHistory('selectInsurance');
        parentContainer.setNavigateAwayModal('formCarInsurance');
    };

    function bindColor(resp, colorField) {
        var colorLi = [];
        var otherColorObj = { colorId: "color0", colorVal: 0, colorHash: "fff", colorName: "Other Colour" };
        if (resp && resp.carColors && resp.carColors[0].length > 0) {
            var uniqueColorList = resp.carColors[0]
                .map(function (item) {
                    return item.name.split('/')[0] + "_" + item.value.split(',')[0];
                })// give an array ["name_value","name_value"]
                .filter(function (item, index, self) {
                    return self.indexOf(item) === index;
                })// filter duplicate "name_value" string
                .map(function (item, index) {
                    var colorVal = index + 1;
                    var colorId = 'color' + colorVal;
                    var data = item.split('_');
                    return {
                        colorId: colorId, colorVal: colorVal, colorHash: data[1], colorName: data[0]
                    };
                });// give an array for bindig with template
            uniqueColorList.push(otherColorObj);// Push other color data
            colorLi = templates.fillColorTemplate(uniqueColorList)

        }
        else {
            //standard template
            var stdcolors = [
                { colorId: "color1", colorVal: 1, colorHash: "f7f7f7", colorName: "White" },
                { colorId: "color2", colorVal: 2, colorHash: "dbdbdb", colorName: "Silver" },
                { colorId: "color3", colorVal: 3, colorHash: "696a6d", colorName: "Gray" },
                { colorId: "color4", colorVal: 4, colorHash: "171717", colorName: "Black" },
                { colorId: "color5", colorVal: 5, colorHash: "ef3f30", colorName: "Red" },
                { colorId: "color6", colorVal: 6, colorHash: "0288d1", colorName: "Blue" },
                { colorId: "color7", colorVal: 7, colorHash: "ff9400", colorName: "Gold" },
                { colorId: "color8", colorVal: 8, colorHash: "800000", colorName: "Maroon" },
                { colorId: "color9", colorVal: 9, colorHash: "a52a2a", colorName: "Brown" },
                otherColorObj
            ];
            colorLi = templates.fillColorTemplate(stdcolors)
        }
        colorField.append(colorLi.join(''));
    };

    function fetchColor(versionId) {
        var url = "/api/versions/colors/?vids=" + versionId;
        return ajaxRequest.getJsonPromise(url);
    };

    function bindVersionDropDown(resp, versionField) {
        var bindingObj = resp.map(function (obj) {
            return { val: obj.ID, text: obj.Name };
        });
        versionField.append(templates.fillDropDownTemplate(bindingObj).join(''));
        versionField.trigger('chosen:updated');
    };

    function fetchVersion(modelId, year) {
        var versionType = "used";
        var url = "/webapi/carversionsdata/GetCarVersions/?type=" + versionType + "&modelId=" + modelId + "&year=" + year;
        return ajaxRequest.getJsonPromise(url);
    };

    function bindModelDropDown(resp, modelField) {
        var bindingObj = resp.map(function (obj) {
            return { val: obj.ModelId, text: obj.ModelName };
        });
        bindingObj = cardetailsUtil.removeModelYear(bindingObj);
        modelField.append(templates.fillDropDownTemplate(bindingObj).join(''));
        modelField.trigger('chosen:updated');
    };

    function fetchModel(makeId, year) {
        var modelType = "used";
        var url = "/webapi/carmodeldata/GetCarModelsByType/?type=" + modelType + "&makeId=" + makeId + "&year=" + year;
        return ajaxRequest.getJsonPromise(url);
    };

    function setContactScreen() {
        if (container && !modalPopup.closeActiveModalPopup()) {
            container.hide();
        }
    };

    function closeActiveSummary() {
        if (summary.isSummaryActive()) {
            summary.closeSummary();
            return true;
        }
        return false;
    }

    function setYearScreen() {
        if (!closeActiveSummary() && !modalPopup.closeActiveModalPopup()) {
            if ($('#bodyYearform').hasClass('active')) {
                $("#formCarDetail.form-screen-group").children('div').removeClass('active');
            }
            while(carDetailsHistoryActive.length > 1)
            {
                carDetailsHistoryActive.pop();
            }
            summary.showToolbar();
            container.show();
            validateScreen.show();
            parentContainer.setNavigationTab('formCarDetail');
            $('#bodyYearform').addClass('active');
            prefillYearAndMonth();
            parentContainer.setButtonTarget('history.back()', 'carDetailsForm.submitYear()');
            events.publish("updateNavigationTabClick", { tabNameArray: [parentContainer.navigationTab.contact, parentContainer.navigationTab.carDetails], value: true });
        }
    };

    function setCarSelectionScreen() {
        if (!closeActiveSummary() && !modalPopup.closeActiveModalPopup()) {
            slideToTopIfNoTabclicked();
            parentContainer.setButtonTarget('history.back()', 'carDetailsForm.submitCarDetails()');
        }
    };

    function setColorScreen() {
        if (!closeActiveSummary() && !modalPopup.closeActiveModalPopup()) {
            slideToTopIfNoTabclicked();
            parentContainer.setButtonTarget('history.back()', 'carDetailsForm.submitColour()');
        }
    };

    function setOwnerScreen() {
        if (!closeActiveSummary() && !modalPopup.closeActiveModalPopup()) {
            slideToTopIfNoTabclicked();
            parentContainer.setButtonTarget('history.back()', 'carDetailsForm.submitOwner()');
        }
    };

    function setKmsScreen() {
        if (!closeActiveSummary() && !modalPopup.closeActiveModalPopup()) {
            slideToTopIfNoTabclicked();
            parentContainer.setButtonText('', 'Next');
            parentContainer.setButtonTarget('history.back()', 'carDetailsForm.submitKmsDriven()');
        }
    };

    function setExpectedPriceScreen() {
        if (!closeActiveSummary() && !modalPopup.isOtpModalPopupVisible() && !modalPopup.closeActiveModalPopup()) {
            slideToTopIfNoTabclicked();
            parentContainer.setButtonTarget('history.back()', 'carDetailsForm.submitExpectedPrice()');
        }
    };

    function setInsuranceScreen() {
        if (!closeActiveSummary() && !modalPopup.closeActiveModalPopup()) {
            historyObj.replaceHistory('preventBack');
            historyObj.addToHistory('selectInsurance');
            parentContainer.slideNextScreen(screenGroup);
            parentContainer.setButtonText('', 'Next');
            summary.hideToolbar();
            $('#planScreen').hide();
            $("#formContainer").show();
            container.show();
            $('body').addClass('insurance-form-active');
            $('#formCarImage').hide();
            parentContainer.setNavigationTab('formCarDetail');
            parentContainer.setButtonTarget(null, 'carDetailsForm.submitInsurance()');

        }
    };
    function removeQuotes(value) {
        return value.replace(/^"(.*)"$/, '$1')
    }
    function prefillYearAndMonth(){
        var month, year;
        month = getQueryStringParam('month');
        year = getQueryStringParam('year');
        if (month && !monthField.val()) {
            monthField.val(removeQuotes(month)).change().trigger('chosen:updated');
        }
        if (year && !yearField.val()) {
            yearField.val(removeQuotes(year)).change().trigger('chosen:updated').trigger('chosen:close');
        }
    }

    function prefillMake() {
        var car;
        if (carMakeField.val() == 0 && (car = getQueryStringParam('car')) != '') { // prefil only if no make is not selected
            isCarPresentInQS = true;
            var settings = {
                type: 'GET',
                url: '/webapi/CarVersionsData/GetCarDetailsByVersionId/?versionid=' + car,
                dataType: 'Json',
            };
            $.ajax(settings).done(function (resp) {
                if (resp) {
                    modelId = resp.ModelId;
                    versionId = resp.VersionId;
                    carMakeField.val(resp.MakeId).change().trigger('chosen:update');
                }
            });
        }
    }
    function prefillModel() {
        if (carModelField.val() == 0) { // prefill only if no model is not selected
            carModelField.val(modelId);
            if (carModelField.val(modelId)) {// trigger change only if value is selected
                carModelField.change().trigger('chosen:updated');
            }
        }
    }

    function prefillVersion() {
        if (carVersionField.val() == 0) { // prefil only if no version is selected
            carVersionField.val(versionId);
            if (carVersionField.val()) { // trigger change only if value is selected
                carVersionField.change().trigger('chosen:updated').trigger('chosen:close');
            }
        }
    }

    function prefillOwners() {
        var owner = '';
        if (!ownerSelectionForm.find('input[name=carOwner]').is(':checked') && (owner = getQueryStringParam('owner')) != '') { // prefil only if owners is not selected
            owner = owner > 4 ? 4 : owner;
            ownerSelectionForm.find('input[name=carOwner]#owner' + owner).prop('checked',true);
        }
    }

    function prefillKms() {
        var kms = '';
        if (!kmsField.val() && (kms = getQueryStringParam('kms')) != '') { // prefill only if kilometers are not selected
            kmsField.val(kms);
            kmsField.trigger('propertychange');
            kmsField.closest('.field-box').addClass('done');
        }
    }
    // submit buttons
    function submitYear() {
        if (validateYearForm()) {
            carDetailsData = appState.setSelectedData(carDetailsData, { manufactureMonth: $.trim(monthField.find('option:selected').val()) });
            carDetailsData = appState.setSelectedData(carDetailsData, { manufactureYear: $.trim(yearField.find('option:selected').val()) });
            if (typeof events != 'undefined') {
                events.publish('yearChanged', { year: carDetailsData.manufactureYear });
            }
            historyObj.addToHistory('selectCar');
            parentContainer.slideNextScreen(screenGroup);
            prefillMake();
            sellCarTracking.forMobile("mfgYear");
            parentContainer.setButtonTarget('history.back()', 'carDetailsForm.submitCarDetails()');
            summary.setSummary(manufactureYearForm);
        }
    };

    function setTabClickAttribute(value) {
        container.attr('tabclick', value);
    }

    function isTabClicked() {
        return container && container.attr('tabclick') === 'true';
    }

    function validateYearForm() {
        var isValid;
        isValid = validateManufacture.monthField();
        isValid &= validateManufacture.yearField();

        return isValid;
    };

    var validateManufacture = {
        monthField: function () {
            var isValid = false;

            if (!monthField.val()) {
                validate.field.setError(monthField, 'Select month');
            }
            else {
                isValid = true;
            }

            return isValid;
        },

        yearField: function () {
            var isValid = false;

            if (!yearField.val()) {
                validate.field.setError(yearField, 'Select year');
            }
            else {
                isValid = true;
            }

            return isValid;
        }
    };



    function submitCarDetails() {
        if (validateCarSelection()) {
            var selectedOption = alternateFuelBody.find('input[name=alternateFuel]:checked');
            var selectedOptionLabel = $.trim($('label[for=' + selectedOption.attr('id') + ']').text());
            if (selectedOption.val() !== "0")
                carDetailsData = appState.setSelectedData(carDetailsData, { alternateFuel: selectedOptionLabel });
            else
                carDetailsData = appState.deleteObjectProperties(carDetailsData, ["alternateFuel"]);
            carDetailsData = appState.setSelectedData(carDetailsData, { referrer: document.referrer, sourceId: parentContainer.getSourceId() });
            var settings = {
                url: '/api/used/sell/cardetails/?tempid=' + encodeURIComponent($.cookie("TempInquiry")),
                type: "POST",
                data: carDetailsData
            }
            $.ajax(settings);
            historyObj.addToHistory('selectColor');
            sellCarTracking.forMobile("mmv");
            parentContainer.slideNextScreen(screenGroup);
            parentContainer.setButtonTarget('history.back()', 'carDetailsForm.submitColour()');
            summary.setSummary(carSelectForm);
        }
    };

    function validateCarSelection() {
        var isValid = false;

        var makeValue = $('#getCarMake').val(),
			modelValue = $('#getCarModel').val(),
			versionValue = $('#getCarVersion').val();

        if (makeValue !== "0" && modelValue !== "0" && versionValue !== "0") {
            isValid = true;
        }
        else {
            validateScreen.setError('Please select car');
        }

        return isValid;
    };

    function resetCarSelectionForm() {
        var makeField = carSelectForm.find('#getCarMake');

        makeField.val("0").change().trigger("chosen:updated");
        makeField.closest('.field-box').removeClass('done');

        formField.resetSelect(carModelField);
        formField.resetSelect(carVersionField);

        popularMakeList.find('input[name=popularMake]:checked').attr('checked', false);
        alternateFuelBody.hide().find('input[name=alternateFuel]').val('0').attr('checked', true);
    };

    function resetCarSelectionScreen() {
        carSelectionTagList.hide().find('ul').html('');
        carSelectForm.find('.step-group__item').hide();
        carSelectForm.find('#carMakeBody').show();
    };

    function submitColour() {
        if (validateColorForm()) {
            var selectedOption = colorSelectionForm.find('input[name=carColour]:checked');
            var selectedOptionLabel = $.trim($('label[for=' + selectedOption.attr('id') + ']').text());
            if (selectedOption.val() !== "0")
                carDetailsData = appState.setSelectedData(carDetailsData, { color: selectedOptionLabel });
            else
                carDetailsData = appState.setSelectedData(carDetailsData, { color: $.trim($('#getOtherColour').val()) });
            historyObj.addToHistory('selectOwners');
            sellCarTracking.forMobile("color");
            parentContainer.slideNextScreen(screenGroup);
            prefillOwners();
            parentContainer.setButtonTarget('history.back()', 'carDetailsForm.submitOwner()');
            setColorSummary();
        }
    };

    function validateColorForm() {
        var isValid = false,
            checkedOption = colorSelectionForm.find('input[name=carColour]:checked');

        if (checkedOption.length === 0) {
            validateScreen.setError('Please select colour');
        }
        else if (checkedOption.val() == 0) {
            if ($('#getOtherColour').val().length === 0) {
                validate.field.setError($('#getOtherColour'), 'Please enter a colour');
            }
            else {
                isValid = true;
            }
        }
        else {
            isValid = true;
        }

        return isValid;
    };

    function resetColorForm() {
        colorSelectionForm.find('input[name=carColour]:checked').attr('checked', false);
        colorSelectionForm.find('#otherColorField').hide();
    };

    function setColorSummary() {
        var selectedColor = colorSelectionForm.find('input[name=carColour]:checked');

        var summaryList = summary.setList(colorSelectionForm);

        if (selectedColor.val() == 0) {
            var fieldObj = summary.getFieldDetails($('#otherColorField'));
            $('#summaryDetailed').find('li[data-field-id="getColor"]').remove();
        }
        else {
            var fieldObj = summary.getFieldDetails($('#getColor'));
            $('#summaryDetailed').find('li[data-field-id="otherColorField"]').remove();
        }

        summary.setListItem(summaryList, fieldObj);
    };

    function submitOwner() {
        if (validateOwner()) {
            var owners = $.trim(ownerSelectionForm.find('input[name=carOwner]:checked').val());
            carDetailsData = appState.setSelectedData(carDetailsData, { owners: owners });
            historyObj.addToHistory('enterKms');
            sellCarTracking.forMobile("owner");
            resetBuyingIndexPrice(expectedPriceForm);
            if (typeof events != 'undefined') {
                events.publish("ownersChanged", { owners: owners });
            }

            parentContainer.slideNextScreen(screenGroup);
            prefillKms();
            parentContainer.setButtonTarget('history.back()', 'carDetailsForm.submitKmsDriven()');

            summary.setSummary(ownerSelectionForm);
        }
    };

    function validateOwner() {
        var isValid = false;

        if (ownerSelectionForm.find('input[name=carOwner]:checked').length === 0) {
            validateScreen.setError('Please select Number of owners');
        }
        else {
            isValid = true;
        }

        return isValid;
    };

    function resetOwnerForm() {
        ownerSelectionForm.find('input[name=carOwner]:checked').attr('checked', false);
    };

    function submitKmsDriven() {
        if (validateKms()) {
            var kms = $.trim(kmsField.attr('data-value'));
            carDetailsData = appState.setSelectedData(carDetailsData, { kmsDriven: kms });
            historyObj.addToHistory('enterPrice');
            sellCarTracking.forMobile("kms");
            parentContainer.slideNextScreen(screenGroup);
            parentContainer.setButtonTarget('history.back()', 'carDetailsForm.submitExpectedPrice()');
            summary.setSummary($('#bodyKilometer'));
            parentContainer.setButtonText('', 'Post your ad');
            if (typeof events != 'undefined') {
                events.publish("kmsChanged", { kms_driven: kms });
            }
        }
    };

    function validateKms() {
        var isValid = false;
        var kms = parseInt(kmsField.attr('data-value'));
        if (isNaN(kms)) {
            validate.field.setError(kmsField, 'Enter kilometers driven');
        }
        else if (kms < 100) {
            validate.field.setError(kmsField, 'KMs driven should be more than 100');
        }
        else if (kms > 900000) {
            validate.field.setError(kmsField, 'KMs driven should be below 9 Lakh kms ');
        } else {
            isValid = true;
        }

        return isValid;
    };

    function submitExpectedPrice() {
        parentContainer.setLoadingScreen();
        if (validateExpectedPrice()) {
            var price = $.trim(getExpectedPrice.attr('data-value'));
            carDetailsData = appState.setSelectedData(carDetailsData, { expectedPrice: price });
            if ($(".recommend-price-box").is(":visible")) {
                if ($("#recommendedCheckbox").is(":checked")) {
                    sellCarTracking.forMobile("recomPriceSelect");
                }
                else {
                    sellCarTracking.forMobile("expectPrice", ($("#recommendedCheckbox").val() - $("#getExpectedPrice").attr("data-value")).toString());
                }
            }
            else {
                sellCarTracking.forMobile("expectPrice", "ExpectPriceFilled");
            }
            parentContainer.setButtonTarget('history.back()', 'carDetailsForm.submitExpectedPrice()');
            summary.setSummary(expectedPriceForm);
            var settings = {
                url: "/api/used/sell/cardetails/?tempid=" + encodeURIComponent($.cookie("TempInquiry")),
                type: "POST",
                data: carDetailsData
            }
            $.ajax(settings).done(function (response, msg, xhr) {
                if (typeof events !== 'undefined') {
                    var eventObj = {
                        data: carDetailsData,
                        callback: onMobileVerified
                    };
                    events.publish("carDetailsPosted", eventObj);
                }
            }).fail(function (xhr) {
                parentContainer.removeLoadingScreen();
                modalPopup.showModalJson(xhr.responseText);
            });
            $('#modalPopUp').attr("data-current", "otp-popup-container");
        }
        else {
            parentContainer.removeLoadingScreen();
        }
    };

    function validateExpectedPrice() {
        var isValid = false;
        var expectedPrice = parseInt(getExpectedPrice.attr('data-value'));
        if (isNaN(expectedPrice)) {
            validate.field.setError(getExpectedPrice, 'Enter expected price');
        }
        else if (expectedPrice < 20000) {
            validate.field.setError(getExpectedPrice, 'Expected Price should be more than 20,000');
        }
        else if (expectedPrice > 100000000) {
            validate.field.setError(getExpectedPrice, 'Expected Price should be below 10 Crore');
        }
        else {
            isValid = true;
        }
        return isValid;
    };

    function resetExpectedPriceForm() {
        expectedPriceForm.find('#recommendedCheckbox').attr('checked', false);
        getExpectedPrice.val('').closest('.field-box').removeClass('done');
        getExpectedPrice.attr('data-value', '');
    };

    function submitInsurance() {
        if (!(validateInsurance() && validateRegistrationNo() && validateRegistrationType())) {
            return false;
        }
        var insuranceValue = insuranceForm.find('input[name=carInsurance]:checked').val();
        carDetailsData = appState.setSelectedData(carDetailsData, { insurance: insuranceValue });
        if (insuranceValue && insuranceValue !== '3') {
            var insuranceExpiryMonth = $.trim(insuranceMonthField.find('option:selected').val());
            var insuranceExpiryYear = $.trim(insuranceYearField.find('option:selected').val());
            carDetailsData = appState.setSelectedData(carDetailsData, { insuranceExpiryYear: insuranceExpiryYear, insuranceExpiryMonth: insuranceExpiryMonth });
        }
        else {
            carDetailsData = appState.deleteObjectProperties(carDetailsData, ['insuranceExpiryYear', 'insuranceExpiryMonth']);
        }

        var regState = registrationForm.find('.register--input-1').val();
        var regNum = registrationForm.find('.register--input-2').val();
        carDetailsData = appState.setSelectedData(carDetailsData, { registrationNumber: $.trim(regState) + $.trim(regNum), regType: registrationType.val(), takeLive: true });
        parentContainer.setLoadingScreen();
        // use PUT API here
        var inquiryCookie = sellCarCookie.getSellInquiryCookie();
        if (inquiryCookie) {
            var settings = {
                url: "/api/v1/used/sell/cardetails/?encryptedId=" + encodeURIComponent(inquiryCookie),
                type: "PUT",
                data: carDetailsData
            }
            $.ajax(settings).done(function (response) {
                parentContainer.removeLoadingScreen();
                var eventObj = {
                    data: carDetailsData,
                    isCarImagesViewLoaded: isCarImagesViewLoaded
                };
                if (typeof events != 'undefined') {
                    events.publish("insuranceSubmitted", eventObj);
                }
            }).fail(function (xhr) {
                parentContainer.removeLoadingScreen();
                modalPopup.showModalJson(xhr.responseText);
            });
        }
    };

    function onMobileVerified(response, data) {
        if (response.isMobileVerified) {
            if (typeof events !== 'undefined') {
                var eventObj = {
                    data: data,
                };
                events.publish("mobileVerified", eventObj);
            }
        }
        else {
            if (typeof events !== 'undefined') {
                var eventObj = {
                    data: data,
                };
                historyObj.addToHistory('hideOtp');
                events.publish("mobileUnverified", eventObj);
            }
        }
    }

    function onCarImageHtmlSucces(eventObj) {
        container.hide();
        if (eventObj.response) {
            $('.partialCarImages').html(eventObj.response);
            if (typeof events !== 'undefined') {
                var eObj = {
                    data: eventObj.data
                };
                events.publish("carImageLoaded", eObj);
            }
        }
        else if (typeof events != 'undefined') {
            events.publish('setImageScreen');
        }
        historyObj.addToHistory('uploadImage');
        sellCarTracking.forMobile("live");
        isCarImagesViewLoaded = true;
        parentContainer.setButtonText('', 'Next');
    }

    function validateInsurance() {
        var isValid = true;
        var insuranceValue = insuranceForm.find('input[name=carInsurance]:checked').val();
        if (insuranceValue && insuranceValue != 3) {
            var insuranceExpiryMonth = insuranceForm.find('#getInsuranceMonth option:selected').val();
            var insuranceExpiryYear = insuranceForm.find('#getInsuranceYear option:selected').val();
            if (parseInt(insuranceExpiryMonth) <= 0) {
                isValid = false;
                validate.field.setError(insuranceMonthField, 'Select month');
            }
            if (parseInt(insuranceExpiryYear) <= 0) {
                isValid = false;
                validate.field.setError(insuranceYearField, 'Select year');
            }
        }
        return isValid;
    }

    function validateRegistrationNo() {
        var isValid = true;
        var regState = registrationForm.find('.register--input-1').val();
        var regNum = registrationForm.find('.register--input-2').val();
        var regStateRegex = /([a-zA-Z]+){2}/g;
        if ((regState && !regStateRegex.test(regState)) || (!regState && regNum)) {
            registrationForm.addClass('invalid').find('.error-text').text('First two letters should be alphabets');
            isValid = false;
        }
        else {
            registrationForm.removeClass('invalid').find('.error-text').text('');
        }
        return isValid;
    }

    function validateRegistrationType() {
        var regType = registrationType.val();
        var regTypeContainer = $('#registrationType');
        if (regType == 0) {
            regTypeContainer.addClass('invalid').find('.error-text').text('Select Registration type');
            return false;
        }
        regTypeContainer.removeClass('invalid').find('.error-text').text('');
        return true;
    }
    function pushHistoryIndex(historyData) {
        if (parentContainer.isTabActive(parentContainer.navigationTab.carDetails) && historyData && historyData.index && !parentContainer.isPresentInArray(historyData.index, carDetailsHistoryActive)) {
            carDetailsHistoryActive.push(historyData.index);
        }
    }

    function removeFromHistoryIndex(historyData) {
        if (parentContainer.isTabActive(parentContainer.navigationTab.carDetails) && historyData && historyData.index && parentContainer.isPresentInArray(historyData.index, carDetailsHistoryActive)) {
            carDetailsHistoryActive.pop();
        }
    }

    function clearHistory() {
        carDetailsHistoryActive = [];
    }

    function slideToTopIfNoTabclicked() {
        if (!isTabClicked()) {
            parentContainer.slideTopMostScreen(screenGroup);
        }
        setTabClickAttribute(false);
    }

    function carDetailsTabClickHandler() {
        var index = 0;
        if (carDetailsHistoryActive.length && !parentContainer.isPresentInArray(historyHelper.getHistoryIndex(), carDetailsHistoryActive)) {
            if (carDetailsHistoryActive.length)
            {
                index = carDetailsHistoryActive[carDetailsHistoryActive.length - 1] - historyHelper.getHistoryIndex();
            }
            index = (index <= 0) ? 1 : index; // if no active histroy, then take user to first page
        }
        else
        {
           index = 1;
        }
        container.show();
        summary.showToolbar();
        parentContainer.setNavigationTab(parentContainer.navigationTab.carDetails);
        setTabClickAttribute(true);
        historyObj.goToIndex(index);
        events.publish("updateHistoryIndex", { index: index + 1 }); // need to +1 because goToIndex reduces history index by 1 because of popstate

    }

    function contactDetailsTabClickHandler() {
       setTabClickAttribute(false);
    }

    return {
        submitYear: submitYear,
        submitCarDetails: submitCarDetails,
        submitColour: submitColour,
        submitOwner: submitOwner,
        submitKmsDriven: submitKmsDriven,
        submitExpectedPrice: submitExpectedPrice,
        submitInsurance: submitInsurance,
    }
})();

