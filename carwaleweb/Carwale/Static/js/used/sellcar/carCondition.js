var carCondition = (function () {
    var formContainer, container, carConditionCarousel, bodyItemGroup;

    var carConditionData = {};

    if (typeof events !== 'undefined') {
        // this form will load async, hence no document ready function
        //The caller needs to publish 'carConditionLoaded' event to show the form
        events.subscribe("carConditionLoaded", carConditionFormLoadHandler);
        events.subscribe("navigateAwayFromCarCondition", onNavigateAwayFromCarCondition);
    }

    function onNavigateAwayFromCarCondition() {
        window.removeEventListener("beforeunload", parentContainer.onPageUnload);
        location.href ='/';
    }

    function carConditionFormLoadHandler(eventObj) {
        if (eventObj && eventObj.isFuelEconomy) {
            $('.body__car-condition').addClass('nonc2b');
        }
        setSelectors();
        registerDomEvents();
        setCarConditionCarousel();

        if (eventObj && eventObj.data) {
            carConditionData = appState.setSelectedData(carConditionData, eventObj.data);
        }
        
    }

    function setSelectors() {
        formContainer = $('#formContainer');
        container = $('#formCarCondition');
        carConditionCarousel = $('#carConditionCarousel');
        bodyItemGroup = container.find('.body-group');
    };

    function registerDomEvents() {
        
        carConditionCarousel.on('click', '.btn--next', function () {
            submitCarCondition();
        });

        $('.nonc2b .btn--next').on('click', function () {
            setVoucherScreen();
        });

        bodyItemGroup.on('change', '.item__radio-group input[type=radio]', function () {           
            var bodyItem = carConditionCarousel.jcarousel('target'),
				conditionalBody = bodyItem.find('.item__condition-body');
            if ($(this).closest('.item__condition-body').length) {
                return false;
            }
            else {
                if ($(this).attr('name') == "tyreCondition") {
                    if ($(this).val() == "false") {
                        showConditionalBody(conditionalBody, bodyItem);
                    }
                    else {
                        hideConditionalBody(conditionalBody, bodyItem);
                    }
                }
                else {
                    if ($(this).val() == "true") {
                        showConditionalBody(conditionalBody, bodyItem);
                    }
                    else {
                        hideConditionalBody(conditionalBody, bodyItem);
                    }
                }
            }
        });

        carConditionCarousel.on('change', '.btn-group--checkbox input[type="checkbox"]', function (event) {
            var checkboxGroup = $(this).closest('.btn-group--checkbox');

            checkOtherSelection(checkboxGroup);
        });

		container.on('input propertychange', '.input--format-value', formatValue.withComma);
    };

    function setCarConditionCarousel() {
        parentContainer.removeLoadingScreen();

        $('.partialCarCondition').find('.accordion__head').attr('data-access', 1).trigger('click');
        $('.partialCarImages').find('.accordion__head').attr('data-access', 1);

        $('#carConditionCarousel').jcarousel({
            vertical: false
        });

        $('.jcarousel--car-condition').find('.jcarousel-control-prev').on('jcarouselcontrol:active', function () {
            $(this).removeClass('inactive');
        }).on('jcarouselcontrol:inactive', function () {
            $(this).addClass('inactive');
        }).jcarouselControl({
            target: '-=1'
        });

        $('.jcarousel--car-condition').find('.jcarousel-control-next').on('jcarouselcontrol:active', function () {           
            $(this).removeClass('inactive');
        }).on('jcarouselcontrol:inactive', function () {
            $(this).addClass('inactive');
        }).jcarouselControl({
            target: '+=1'
        });
        $('.jcarousel--car-condition').on('click', '.jcarousel-control-next', function () {
            questionnaireTrack(carConditionCarousel.jcarousel('target').prev('li.body__item'));
        });
        carConditionCarousel.on('jcarousel:animate', function (event, carousel) {           
            resizeCarousel();
        });
    };

    function questionnaireTrack(item) {
        sellCarTracking.forDesktop(item.attr('data-questiontype'),
                   item.find('input[type=radio]:checked').length > 0 ? item.find('input:radio').prop('checked').toString() : "skipped");
    }
    function showConditionalBody(conditionalBody, bodyItem) {
        if (conditionalBody.length) {
            conditionalBody.show();
        }
        else {
            nextGroupItem(bodyItem);
        }

        resizeCarousel();
    }

    function hideConditionalBody(conditionalBody, bodyItem) {
        conditionalBody.hide();
        resetConditionBody(conditionalBody);
        nextGroupItem(bodyItem);

        resizeCarousel();
    }

    function resetConditionBody(conditionalBody) {
        conditionalBody.find('.input-box').removeClass('invalid done');
        conditionalBody.find('input[type="text"], input[type="tel"], input[type="number"]').val('');
        conditionalBody.find('input[type="checkbox"], input[type="radio"]').attr('checked', false);
        conditionalBody.find('.item__subcondition-body').hide();
    }

    function checkOtherSelection(checkboxGroup) {
        var isChecked = false;

        checkboxGroup.find('input[type="checkbox"]:checked').each(function () {
            if ($(this).val() == "0") {
                isChecked = true;
            }
        });

        var nestedBody = checkboxGroup.siblings('.item__subcondition-body');

        if (isChecked && nestedBody.length) {
            nestedBody.show();
        }
        else {
            nestedBody.hide();
        }

        resizeCarousel();
    }

    function setVoucherScreen() {
        parentContainer.setLoadingScreen();
        var carOtherDetails = getOtherDetails();
        var encryptedId = encodeURIComponent($.cookie("SellInquiry"));


        var settings = {
            url: '/api/used/sell/otherdetails/?inquiryId=' + encryptedId ,
            type: 'POST',
            data: carOtherDetails
        }

        $.ajax(settings).done(function (response, msg, xhr) {
            var carConditionJson = getCarConditionJson();
            var settings = {
                url: '/api/used/sell/carcondition/?inquiryId=' + encryptedId + "&cityId=" + cityForm.getCityData().cityId,
                type: 'POST',
                data: carConditionJson
            }
            $.ajax(settings).done(function () {
                getVoucherHtml();
            }).fail(function (xhr) {
                if (xhr.status === 404) {
                    getVoucherHtml();
                }
                else {
                    parentContainer.removeLoadingScreen();
                    modalPopup.showModalJson(xhr.responseText);
                }
            });
            
        }).fail(function (xhr) {
            parentContainer.removeLoadingScreen();
            modalPopup.showModalJson(xhr.responseText);
        });
    }

    function nextGroupItem(bodyItem) {
        questionnaireTrack(bodyItem);
        var nextBody = bodyItem.next('.body__item');
        if (nextBody.length) {
            carConditionCarousel.jcarousel('scroll', '+=1');           
			if (!nextBody.next('.body__item').length) {
				nextBody.find('.btn--next').val('Submit');
			}
        }
        else if(validateFuelEconomy()){
            setVoucherScreen();
        }
    };

    function getVoucherHtml() {
        var partialVoucher = $('#voucherScreen');
        var settings = {
            url: 'voucher/?' + 'cityId=' + cityForm.getCityData().cityId
        }
        $.ajax(settings).done(function (response) {
            partialVoucher.html(response).show();
            $("#congratulations").text($("#congratulations").text() + " " + $("#getName").val());
            formContainer.find('.accordion-wrapper').hide();
            parentContainer.removeLoadingScreen();
            parentContainer.focusDocument(formContainer);
            sellCarTracking.forDesktop("congrats");
        }).fail(function (xhr) {
            parentContainer.removeLoadingScreen();
            modalPopup.showModal(xhr.responseText);
        });
    }

    function resizeCarousel() {
        var newHeight = carConditionCarousel.jcarousel('target').outerHeight(true);

        carConditionCarousel.animate({
            height: newHeight
        });
    };

    function submitCarCondition() {
        var activeBodyItem = carConditionCarousel.jcarousel('target'),
			conditionBody = activeBodyItem.find('.item__condition-body');

        if (conditionBody.hasClass('validate-body')) {
            if (validateConditionBody(conditionBody)) {
                nextGroupItem(activeBodyItem);
            }
        }
        else if (conditionBody.hasClass('validate-nested-body')) {
            if (validateNestedConditionBody(conditionBody)) {
                nextGroupItem(activeBodyItem);
            }
        }
        else if (activeBodyItem.data('questiontype') === 'partsReplacedQuestion') {
            if (validatePartsReplaced(activeBodyItem)) {
                nextGroupItem(activeBodyItem);
            }
        }
        else {
            nextGroupItem(activeBodyItem);
        }
    };

    function validateFuelEconomy() {

        var mileageFiled = $("#getFuelEconomy");
        var mileage = mileageFiled.val();

        if (mileage && !validate.valiadateDecimal(mileage)) {
            validate.field.setError(mileageFiled, 'Value should be numeric.');
            return false;
        }
        else if (parseInt(mileage) > 30 || parseInt(mileage) <= 0) {
            validate.field.setError(mileageFiled, 'Value should be between 1 and 30.');
            return false;
        }
        return true;
    }
    function validatePartsReplaced(body) {
        var conditionBody = body.find('.item__condition-body')
        var inputField = conditionBody.find('#getReplacedParts');

        if (conditionBody.is(':visible')) {
            var isPartsEntered = inputField.val().length;
            var isPartsChecked = conditionBody.find('#replacedParts input[name="replacedPart"]:checked').length;

            if (!isPartsEntered && !isPartsChecked) {
                validate.field.setError(inputField, 'Field required!');
                return false;
            }
        }
        validate.field.hideError(inputField);
        return true;
    }

    function validateConditionBody(conditionBody) {
        var isValid = false,
			fields = conditionBody.find('.validate-type-input');

        fields.each(function () {
            if (!$(this).find('input').val().length) {
                isValid = false;
                validate.field.setError($(this).find('input'), 'Field required!');
            }
            else {
                isValid = true;
            }
        });

        return isValid;
    };

    function validateNestedConditionBody(conditionBody) {
        var nestedBody = conditionBody.find('.validate-body:visible');

        if (!nestedBody.length) {
            return true;
        }
        else {
            var isValid = false;

            nestedBody.each(function () {
                var fields = $(this).find('.validate-type-input');

                fields.each(function () {
                    if (!$(this).find('input').val().length) {
                        isValid = false;
                        validate.field.setError($(this).find('input'), 'Field required!');
                    }
                    else {
                        isValid = true;
                    }
                });
            });

            return isValid;
        }
    };

    function getCarConditionJson() {
        var json = {};

        json["isAccidental"] = $('input[name=accidentCondition]:checked', '#formCarCondition').val();
        json["accidentDetails"] = $("#getAccidentDetails").val();
        json["isPartReplaced"] = $('input[name=partsAssembly]:checked', '#formCarCondition').val();

        var listPartReplaced = [];
        $('#replacedParts').find("input:checkbox:checked").each(function () {
            listPartReplaced.push($(this).val());
        });
        json["partsReplaced"] = listPartReplaced;
        json["partReplacedText"] = $("#getReplacedParts").val();

        json["isInsuranceClaimed"] = $('input[name=insuranceClaim]:checked', '#formCarCondition').val();
        json["totalInsuranceClaimed"] = $("#getTotalClaims").attr("data-value");

        json["isRegularlyServiced"] = $('input[name=serviceCenter]:checked', '#formCarCondition').val();

        json["isLoanPending"] = $('input[name=hypothecate]:checked', '#formCarCondition').val();
        json["outstandingLoanAmt"] = $("#getLoanAmount").attr("data-value");
        json["isTyreOriginal"] = $('input[name=tyreCondition]:checked', '#formCarCondition').val();
        json["tyresChangedAtKm"] = $("#getChangedTyresKms").attr("data-value");

        json["isWearTear"] = $('input[name=wearAndTear]:checked', '#formCarCondition').val();
        var listMinorScratches = [];
        $('#getScratches').find("input:checkbox:checked").each(function () {
            listMinorScratches.push($(this).val());
        });
        json["minorScratchedParts"] = listMinorScratches;
        json["minorScratchText"] = $("#getOtherScratch").val();

        var listDents = [];
        $('#getDents').find("input:checkbox:checked").each(function () {
            listDents.push($(this).val());
        });
        json["dentedParts"] = listDents;
        json["dentedPartText"] = $("#getOtherDent").val();

        json["isCarRepainted"] = $('input[name=carRepaint]:checked', '#formCarCondition').val();

        json["isMechanicalIssue"] = $('input[name=mechIssue]:checked', '#formCarCondition').val();
        var listEngineIssues = [];
        $('#getEngineIssue').find("input:checkbox:checked").each(function () {
            listEngineIssues.push($(this).val());
        });
        json["engineIssues"] = listEngineIssues;
        json["engineIssueText"] = $("#getOtherEngineIssue").val();

        var listElectricalIssues = [];
        $('#getElectricIssue').find("input:checkbox:checked").each(function () {
            listElectricalIssues.push($(this).val());
        });
        json["electricIssues"] = listElectricalIssues;
        json["electricIssueText"] = $("#getOtherElectricIssue").val();

        json["isSuspensionIssue"] = $('input[name=suspensionReplace]:checked', '#formCarCondition').val();
        json["isAcWorking"] = $('input[name=acWorking]:checked', '#formCarCondition').val();

        return json;
    };
    function getOtherDetails() {
        return appState.setSelectedData({}, { mileage: $("#getFuelEconomy").val(), warranties: $("#getAvailableWarranty").val(), comments: $("#getFuelComment").val() });
    }
})();