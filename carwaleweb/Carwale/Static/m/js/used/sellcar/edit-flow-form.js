var EditCarFlowForm = (function(){
    var reasonsInputBox,editVersionInputBox,editColorInputBox,editOwnerInputBox,editInsuranceYearInputBox, editInsuranceMonthInputBox, ownerSelectField,
    colorSelectField,otherColorField, expPriceField, kmDrivenField, registrationNoField, formControlBox, selectBox, editInsurance, otherColorUnit, insuranceYearUnit, insuranceMonthUnit,
    registrationTypeField;
    var oldCarData;
    function editSellcarDocReady() {
        setSelectors();
        registerEvents();
        oldCarData = getCarData();
        editCarTracking.trackForMobile(editCarTracking.actionType.editPageLoad, 's'+$(editForm).attr('data-profileid'));
    };
    //Variables declared for selectors
    function setSelectors() {
        editForm = '.edit-flow-form';
        reasonsInputBox = '.select-box';
        editVersionInputBox = $('#editVersion');
        editColorInputBox = $('#editColor');
        editOwnerInputBox = $('#editOwner');
        editInsuranceYearInputBox = $('#editInsuranceYear');
        editInsuranceMonthInputBox = $('#editInsuranceMonth');
        ownerSelectField = $('#ownerSelect');
        colorSelectField = $('#colorSelect');
        otherColorField = $('#getOtherColor');
        expPriceField = $('#getExpectedPrice');
        kmDrivenField = $('#getkmDriven');
        registrationTypeField = $('#editRegistrationType');
        registrationNoField = $('#registration-no');
        formControlBox = $('.form-control-box');
        selectBox = '.select-box';
        editInsurance = '#editInsurance';
        otherColorUnit = $('.other-color-unit');
        insuranceYearUnit = $('.insurance-year-unit');
        insuranceMonthUnit = $('.insurance-month-unit');
    };
    //All events for the selectors
    function registerEvents() {
        formControlBox.children(selectBox).on('change', 'select', function () {
            if ($(this).val() !== "0") {
                field.hideError($(this));
                $(this).parent().addClass('done');
            }
        });

        $('.input-box input').blur(function () {
            tmpval = $(this).val();
            if (tmpval.length > 0) {
                $(this).parent().addClass('done');
            }
            else {
                $(this).parent().removeClass('done');
            }
        });

        $(document).on('click', '.modal-box .modal__close', function () {
            history.back();
        });
        $(document).on('click', '#modalBg, .close-icon', function () {
            popUp.hidePopUp();
        });

        $(window).scroll(function () {
            fixButton();
        });      

        $('#prevBtn').on('click', function () {
            editCarTracking.trackForMobile(editCarTracking.actionType.editBack, editCarTracking.actionType.editBack);
            window.history.back();
        });
        $('#nextBtn').on('click', function () {
            if (validateAllFields()) {
                var sellCarInfo = getCarData();
                trackChanges(sellCarInfo);
                $.ajax({
                    url: '/v1/used/mylistings/' + $(editForm).attr('data-profileid') + '/?authToken=' + $.cookie("encryptedAuthToken"),
                    data: sellCarInfo,
                    type: 'PUT',
                    success: function (resp) {
                        if (resp && resp.status) {
                            window.location.assign("/used/mylistings/" + $(editForm).attr('data-profileid') + "/images/?authToken=" + $.cookie("encryptedAuthToken"));
                        }
                    },
                    error: function (xhr) {
                        editCarCommon.showModal(xhr.responseText);
                    }
                });
           };
        });
        
        $(document).on('click', function () {
            if (!expPriceField.val() == 0) {
                validateExpPrice();
            }
            if (!kmDrivenField.val() == 0) {
                validateKmDriven();
            }
            if (!registrationNoField.val() == 0) {
                validateRegisterNo();
            }       
        });

        $("#versionSelect").on('change', function () {
            $.ajax({
                url: '/api/versions/colors/?vids='+ $("#editVersion option:selected").val(),
                type: 'GET',
                success: function (resp) {
                    $(colorSelectField).empty();
                    if (resp && resp.carColors && resp.carColors[0].length > 0) {
                        
                        $(colorSelectField).append('<option value=0></option>');
                        for (var i = 0; i < resp.carColors[0].length; i++) {
                            $(colorSelectField).append('<option value=' + resp.carColors[0][i].value + '>' + resp.carColors[0][i].name + '</option>');
                        }
                        $(colorSelectField).append('<option value="0">Other</option>');
                        
                    }
                    else {
                        $(colorSelectField).append('<option value=0></option>');
                        var stdcolors = [{ value: "f7f7f7", name: "White" }, { value: "dbdbdb", name: "Silver" }, { value: "696a6d", name: "Gray" },
                                         { value: "171717", name: "Black" }, { value: "ef3f30", name: "Red" }, { value: "0288d1", name: "Blue" },
                                         { value: "ff9400", name: "Gold" }, { value: "800000", name: "Maroon" }, { value: "a52a2a", name: "Brown" },
                                         { value: "0", name: "Other" }
                        ];
                       
                        for (var i = 0; i < stdcolors.length; i++) {
                            $(colorSelectField).append('<option value=' + stdcolors[i].value + '>' + stdcolors[i].name + '</option>');
                        }
                    }
                    $(colorSelectField).closest('.select-box').removeClass('done');
                    $(colorSelectField).trigger("chosen:updated");
                    $(colorSelectField).trigger("change");
                    $(otherColorField).val("");
                }
            });

        });

        formControlBox.children(colorSelectField).on('change', 'select', function () {
            if (($("#colorSelect option:selected").text()) == "Other") {
                otherColorUnit.show();
            }
            else {
                otherColorUnit.hide();
            }
        });

        formControlBox.children(editInsurance).on('change', 'select', function () {
            if (($("#insuranceSelect option:selected").text()) == "Expired") {
                insuranceYearUnit.hide();
                insuranceMonthUnit.hide();
            }
            else {
                insuranceYearUnit.show();
                insuranceMonthUnit.show();
            }
        });
        kmDrivenField.on("keypress", function (e) {
            var charCode = (e.which) ? e.which : e.keyCode;
            return (charCode > 31 && (charCode < 48 || charCode > 57)) ? false : true;
        });
        kmDrivenField.on('input propertychange', withComma);
        kmDrivenField.on('input propertychange', handleCommaDelete);

        expPriceField.on("keypress", function (e) {
            var charCode = (e.which) ? e.which : e.keyCode;
            return (charCode > 31 && (charCode < 48 || charCode > 57)) ? false : true;
        });
        expPriceField.on('input propertychange', withComma);
        expPriceField.on('input propertychange', handleCommaDelete);
        $('#colorSelect').val($('#editColor option:first-child').val()).trigger('chosen:updated');

    }
    //Function for fixing buttons
    function fixButton() {
        $(document).find('.extraDivHt').height($('.floating-container').outerHeight());
        setButtonsScroll();
    };
    //Function for scrolling buttons
    function setButtonsScroll() {
        var scrollPosition = (window.pageYOffset !== undefined) ? window.pageYOffset : (document.documentElement || document.body.parentNode || document.body).scrollTop;
        if (scrollPosition + $(window).height() > ($('body').height() - $('footer').height())) {
            $('.extraDivHt').hide();
            $('.floating-container').removeClass('float-fixed').addClass('float');
        }
        else {
            $('.extraDivHt').show();
            $('.floating-container').removeClass('float').addClass('float-fixed');
        }
    };
    //Function for validating Owner
    function validateOwner() {
        if (editOwnerInputBox.find('#ownerSelect').val() == "0") {
            field.setError(editOwnerInputBox, 'Please select owner type');
            return false;
        }
        return true;
    }
    //Function for validating registration type
    function validateRegistrationType() {
        if (registrationTypeField.find('#registrationTypeSelect').val() == "0") {
            field.setError(registrationTypeField, 'Please select registration type');
            return false;
        }
        return true;
    }
    //Function for validating Version
    function validateVersion() {
        if (editVersionInputBox.find('#versionSelect').val() == "0") {
            field.setError(editVersionInputBox, 'Please select version type');
            return false;
        }
        return true;
    };
    //Function for validating Color
    function validateColor() {        
        if ((editColorInputBox.find('#colorSelect option:selected').text() == "Other" && otherColorField.val() == "")) {
            field.setError(otherColorField, 'Please enter color');
            return false;
        }
        else if (editColorInputBox.find('#colorSelect').val() == "0" && editColorInputBox.find('#colorSelect option:selected').text() != "Other") {
            field.setError(editColorInputBox, 'Please select color');
            return false;
        }
        return true;
    };
    //Function for validating Insurance Type
    function validateInsurance() {
        if ($(editInsurance).find('#insuranceSelect').val() == "0") {
            field.setError($(editInsurance), 'Please select insurance type');
            return false;
        }
        return true;
    };
    //Function for validating Insurance Year
    function validateYear() {
        if (editInsuranceYearInputBox.find('#insuranceYearSelect').val() == "0" && $('#insuranceSelect option:selected').text()!='Expired') {
            field.setError(editInsuranceYearInputBox, 'Please select insurance year');
            return false;
        }
        return true;
    };
    //Function for validating Insurance Month
    function validateMonth() {
        if (editInsuranceMonthInputBox.find('#insuranceMonthSelect').val() == "0" && $('#insuranceSelect option:selected').text()!='Expired') {
            field.setError(editInsuranceMonthInputBox, 'Please select insurance month');
            return false;
        }
        return true;
    };
    //Function for validating Expected Price
    function validateExpPrice() {
        var isValid = false;
        var expectedPrice = parseInt(expPriceField.attr("data-value"));
        if (isNaN(expectedPrice)) {
            field.setError(expPriceField, 'Enter expected price');
        }
        else if (expectedPrice < 20000) {
            field.setError(expPriceField, 'Expected Price should be more than 20,000');
        }
        else if (expectedPrice > 100000000) {
            field.setError(expPriceField, 'Expected Price should be below 10 Crore');
        }
        else {
            isValid = true;
            field.hideError(expPriceField);
        }
        return isValid;

    };
    //Function for validating Kilometer Driven
    function validateKmDriven() {
        var isValid = false;
        var kms = parseInt(kmDrivenField.attr("data-value"));
        if (isNaN(kms)) {
            field.setError(kmDrivenField, 'Enter kilometers driven');
        }
        else if (kms < 100) {
            field.setError(kmDrivenField, 'KMs driven should be more than 100');
        }
        else if (kms > 900000) {
            field.setError(kmDrivenField, 'KMs driven should be below 9 Lakh kms ');
        } else {
            isValid = true;
            field.hideError(kmDrivenField);
        }

        return isValid;
    };

    function getCarData()
    {
        var carData = {
            ManufactureYear: $('.month-in-number').find('span:nth-of-type(1)').attr("data-value-year"),
            ManufactureMonth: $('.month-in-number').find('span:nth-of-type(1)').attr("data-value-month"),
            VersionId: editVersionInputBox.find('#versionSelect').val(),
            Owners: ownerSelectField.val(),
            KmsDriven: kmDrivenField.attr("data-value"),
            ExpectedPrice: expPriceField.attr("data-value"),
            Insurance: $(editInsurance).find('#insuranceSelect').val(),
            RegistrationNumber: registrationNoField.val().trim(),
            Color: $("#colorSelect option:selected").text(),
            RegType: registrationTypeField.find("#registrationTypeSelect option:selected").text()
        }
        if (carData.Insurance && carData.Insurance != 3) {
            carData.InsuranceExpiryYear = editInsuranceYearInputBox.find('#insuranceYearSelect').val();
            carData.InsuranceExpiryMonth = editInsuranceMonthInputBox.find('#insuranceMonthSelect').val();
        }
        if (($("#colorSelect option:selected").text()) == "Other") {
            carData.Color = otherColorField.val();
        }
        return carData;
    }

    function trackChanges(currentData)
    {
        if(oldCarData && currentData)
        {
            var label = '';
            for(var key in currentData)
            {
                if(oldCarData[key] != currentData[key])
                {
                    if(label)
                    {
                        label = label + '|' + key + 'change';
                    }
                    else
                    {
                        label = key + 'change';
                    }
                }
            }
            if(label)
            {
                editCarTracking.trackForMobile(editCarTracking.actionType.editContinue, label);
            }
        }
    }
    //Function for validating Registration Number
    function validateRegisterNo() {
        var regStateRegex = /([a-zA-Z]+){2}/g;
        var isValid = false;
        var registraionValue = registrationNoField.val() ? registrationNoField.val().trim() : registrationNoField.val();
        if (registraionValue && !regStateRegex.test(registraionValue)){
            field.setError(registrationNoField, 'Invalid registration no');
        }
        else {
            field.hideError(registrationNoField);
            isValid = true;
        }
        return isValid;
    };
    
    function validateAllFields() {
        return (
                validateOwner()
             && validateVersion()
             && validateColor()
             && validateInsurance()
             && validateYear()
             && validateMonth()
             && validateExpPrice()
             && validateKmDriven()
             && validateRegistrationType()
             && validateRegisterNo()
            );
    }
    function withComma() {
        var fieldValue = this.value,
            caretPos = this.selectionStart,
            lenBefore = fieldValue.length;
        fieldValue = fieldValue.replace(/[^\d]/g, "").replace(/^0+/, "");
        this.setAttribute('data-value', fieldValue);
        this.value = Common.utils.formatNumeric(fieldValue);
        var selEnd = caretPos + this.value.length - lenBefore;
        if (this.value[selEnd - 1] == ',') {
            selEnd--;
        }
        this.selectionEnd = selEnd > 0 ? selEnd : 0;
    }
    function handleCommaDelete(event) {
        var fieldValue = this.value;
        if (event.keyCode == 8) {             //backspace
            if (fieldValue[this.selectionEnd - 1] == ',') {
                this.selectionEnd--;
            }
        }
        else if (event.keyCode == 46) {       //delete
            if (fieldValue[this.selectionEnd] == ',') {
                this.selectionStart++;
            }
        }
    }

    return{
        editSellcarDocReady: editSellcarDocReady,
        withComma: withComma,
        handleCommaDelete: handleCommaDelete

    };
})();

$(document).ready(function () {
    EditCarFlowForm.editSellcarDocReady();
    ChosenInit($(editForm));
});

$(window).on('popstate', function () {
    if (editCarCommon.isVisible()) {
        editCarCommon.hideModal();
    }
});
