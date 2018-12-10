// dependency
// 1. static/js/used/sellcar/common.js
// 2. staic/js/used/sellcar/utilities.js

var carDetailsForm = (function () {
	var container, yearField, manufactureYearForm, carSelectForm, carMakeField, carModelField, carVersionField, colorSelectionForm, ownerSelectionForm, kmsField, expectedPriceForm, getExpectedPrice, insuranceForm, insuranceMonthField, insuranceYearField, registrationForm;
	var isCarImagesViewLoaded = false;
	var carDetailsData = {};
	var registrationTypeSelectionForm;
	var isCarPresentInQS, modelId, versionId;

	if (typeof events !== 'undefined') {
		// this form will load async, hence no document ready function
		// The caller needs to publish 'contactSubmit' event to show the form
	    events.subscribe("contactSubmit", carDetailsFormLoadHandler);
	    events.subscribe("updateCarDetails", updateCarDetails);
	    events.subscribe("carImageLoaded", setCarImageViewParam);
	    events.subscribe("contactDetailsChanged", reInit);
	    events.subscribe("buyingIndexSuccess", buyingIndexSuccess);
	    events.subscribe("buyingIndexFailed", buyingIndexFailed);
	    events.subscribe("navigateAwayFromCardetails", onNavigateAwayFromCardetails);
	};

	function onNavigateAwayFromCardetails() {
	    window.removeEventListener("beforeunload", parentContainer.onPageUnload);
	    history.back();
	}

	function getSelectedData(obj, prop) {
		if (obj.hasOwnProperty(prop)) {
			return obj[prop];
		}
		return null;
	};

	function setCarImageViewParam() {
	    isCarImagesViewLoaded = true;
	}

	function carDetailsFormLoadHandler(eventObj) {
	    setSelectors();
		registerDomEvents();
		init();
		horizontalForm.setInitialSteps(container.find('.horizontal-form'));

		$('.partialCarDetails').find('.accordion__head').attr('data-access', 1).trigger('click');
		$('.partialContactDetails').find('.accordion__head').attr('data-access', 1);
		
		summary.resetToolbar(container.closest('.accordion__item'));

		updateContactData(eventObj);
		prefillData();
		window.addEventListener("beforeunload", parentContainer.onPageUnload);
	};

	function updateContactData(eventObj) {
	    if (eventObj && eventObj.data) {
	        carDetailsData = appState.setSelectedData(carDetailsData, {
	            sellCarCustomer: eventObj.data,
	            shareToCT: eventObj.data.shareToCT,
	            areaId: eventObj.data.areaId,
	            pincode: eventObj.data.pincode,
	            referrer: document.referrer,
	            sourceId: parentContainer.getSourceId()
	        });
	    }
	}

	function prefillData() {
	    //set selectors in case its available in query string
	    var yearId, month, car;
	    if (yearId = getQueryStringParam('year')) {
	        $("#getYear").find("input#year" + yearId).prop('checked', true).change();
	    }
	    if (month = getQueryStringParam('month')) {
	        $("#getMonth").find("input#month" + month).prop('checked', true).change();
	    }
	    if (car = getQueryStringParam('car')) {
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
	                $("#getCarMake").find("input#carMake" + resp.MakeId).prop('checked', true).change();
	            }
	        });
	    }
	}

	function updateCarDetails(eventObj) {
	    if (eventObj) {
	        carDetailsData = appState.setSelectedData(eventObj, true);
	    }
	}

	function setSelectors() {
		container = $('#formCarDetail');
		monthField = $('#getMonth');
		yearField = $('#getYear');
		manufactureYearForm: $('#bodyYearform');
		carSelectForm = $('#bodyCarForm');
		carMakeField = $('#getCarMake');
		carModelField = $('#getCarModel');
		carVersionField = $('#getCarVersion');
		alternateFuelBody = $('#alternateFuelForm');
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
		registrationTypeSelectionForm = $('#bodyRegistrationType');
	};

	function init() {
		parentContainer.removeLoadingScreen();

		// set standard colours
		var colorSelectionUl = colorSelectionForm.find(".color-option-list");
		colorSelectionUl.html('');
		bindColor({}, colorSelectionUl);
	};

	function reInit(eventObj) {
	    parentContainer.removeLoadingScreen();
	    updateContactData(eventObj);
	}

	function registerDomEvents() {
		// yearForm
		$('#bodyYearform').on('change', '.radio-box input[type="radio"]', function () {
			if (!carSelectForm.find('input[name=carMake]:checked').length) {
				resetExpectedPriceForm();
			}
			
			var formStep = $(this).closest('.form__step');

			horizontalForm.setStepSelection(formStep);

			carDetailsData = appState.setSelectedData(carDetailsData, {
				manufactureMonth: $.trim(monthField.find('input[name=makeMonth]:checked').val()),
				manufactureYear: $.trim(yearField.find('input[name=makeYear]:checked').val())
			});
			if (typeof events != 'undefined') {
			    events.publish('yearChanged', { year: carDetailsData.manufactureYear });
			}
			setEditToolTip();
		});

		$('#bodyYearform').on('click', '.step__selection', function () {
			var formStep = $(this).closest('.form__step');

			horizontalForm.resetFormStep(formStep);
		});

		// carForm
		// make
		carSelectForm.on('change', '.radio-box input[type="radio"]', function () {
			var formStep = $(this).closest('.form__step');

			horizontalForm.setStepSelection(formStep);
		});

		carSelectForm.on('click', '.step__selection', function () {
			var formStep = $(this).closest('.form__step');

			horizontalForm.resetFormStep(formStep);
		});

		carSelectForm.on('change', '.radio-box input[type=radio]', function () {
			var selectedItem = $(this);

			if (selectedItem.val() !== "0") {
				var selectionText = $(this).next('label').text(),
					selectionId = $.trim($(this).val()),
					stepBody = $(this).closest('.form-step__body');

				switch (stepBody.attr('id')) {
					case 'getCarMake':
						carDetailsData = appState.setSelectedData(carDetailsData, {
							makeId: selectionId
						});
						parentContainer.setLoadingScreen();
					    fetchModel(selectionId, yearField.find('input[name=makeYear]:checked').val()).done(function (resp) {
					        bindModelRadioList(resp, carModelField);
					        if (isCarPresentInQS) {
					            $("#getCarModel").find("input#carModel" + modelId).prop('checked', true).change();
					        }
							parentContainer.removeLoadingScreen();
					       
						}).fail(function () {
							parentContainer.removeLoadingScreen();
							horizontalForm.setError($('#carModelForm'), 'No model found for this selection');
						});

						formField.emptyRadioList(carModelField.find('input[name=carModel]').closest('.radio-box'));
						formField.emptyRadioList(carVersionField.find('input[name=carVersion]').closest('.radio-box'));

						break;

					case 'getCarModel':
						carDetailsData = appState.setSelectedData(carDetailsData, {
							modelId: selectionId
						});
						parentContainer.setLoadingScreen();
						fetchVersion(selectionId, yearField.find('input[name=makeYear]:checked').val()).done(function (resp) {
						    bindVersionRadioList(resp, carVersionField);
						    if (isCarPresentInQS) {
						        $("#getCarVersion").find("input#carVersion" + versionId).prop('checked', true).change();
						    }
							parentContainer.removeLoadingScreen();
						}).fail(function () {
							parentContainer.removeLoadingScreen();
							horizontalForm.setError($('#carVersionForm'), 'No version found for this selection');
						});

						formField.emptyRadioList(carVersionField.find('input[name=carVersion]').closest('.radio-box'));
						break;

					case 'getCarVersion':
						carDetailsData = appState.setSelectedData(carDetailsData, {
							versionId: selectionId
						});
						if (typeof events != 'undefined') {
						    events.publish('versionChanged', { makeId:carDetailsData.makeId,modelId:carDetailsData.modelId,versionId: carDetailsData.versionId, });
						}
						parentContainer.setLoadingScreen();
						var colorSelectionUl = colorSelectionForm.find(".color-option-list");
						colorSelectionUl.html('');
						fetchColor(selectionId).done(function (resp) {
							bindColor(resp, colorSelectionUl);
							parentContainer.removeLoadingScreen();
						}).fail(function() {
							parentContainer.removeLoadingScreen();
							//validateScreen.setError('No colour found');
						});
						var settings = {
						    url: '/api/used/sell/cardetails/?tempid=' + encodeURIComponent($.cookie("TempInquiry")),
						    type: "POST",
						    data: carDetailsData
						}
						$.ajax(settings)
						break;

					default:
						break;
				}

				//resetColorForm();
				//resetOwnerForm();
				resetExpectedPriceForm();
			}

			//validateScreen.hideError();
		});

		// colour
		colorSelectionForm.on('change', 'input[name=carColour]', function () {
			if ($(this).val() === "0") {
				$('#otherColorField').show();
				var listContainer = $(this).closest('.step--scrollbar');
				listContainer.animate({
					scrollTop: listContainer.height()
				});
			}
			else {
				var formStep = $(this).closest('.form__step');
				$('#otherColorField').hide().val('');
				submitColour();
				horizontalForm.setStepSelection(formStep);
			}
		});

		colorSelectionForm.on('click', '.step__selection', function () {
			var formStep = $(this).closest('.form__step');

			horizontalForm.resetFormStep(formStep);
		});

		$('#submitOtherColour').on('click', function () {
			submitColour();
		});

		// alternateFuelForm
		alternateFuelBody.on('change', 'input[name=alternateFuel]', function () {
			var formStep = $(this).closest('.form__step');
			var owner, kms;
			submitCarDetails();
			horizontalForm.setStepSelection(formStep);

		    //check if owner present
			if (owner = getQueryStringParam('owner')) {
			    $("#bodyOwnerForm").find("input#carOwner" + owner).prop('checked', true).change();
			}
			if (kms = getQueryStringParam('kms')) {
			    kmsField.val(kms);
			    kmsField.trigger('propertychange');
			}
		});

		alternateFuelBody.on('click', '.step__selection', function () {
			var formStep = $(this).closest('.form__step');

			horizontalForm.resetFormStep(formStep);
		});

		// bodyOwnerForm
		ownerSelectionForm.on('change', 'input[name=carOwner]', function () {
			submitOwner();
		});

		ownerSelectionForm.on('click', '.step__selection', function () {
			var formStep = $(this).closest('.form__step');

			horizontalForm.resetFormStep(formStep);
		});

		registrationTypeSelectionForm.on('change', 'input[name=carRegistrationType]', function () {
		    submitRegistrationType();
		});

		registrationTypeSelectionForm.on('click', '.step__selection', function () {
		    var formStep = $(this).closest('.form__step');
		    horizontalForm.resetFormStep(formStep);
		});
		// kilometers driven
		formatValue.formatValueOnInput(kmsField);
		kmsField.on("focusout", function () {
		    if (typeof events != 'undefined') {
		        events.publish("kmsChanged", { kms_driven: $.trim(kmsField.attr('data-value')) });
		    }
		});

		// recommended price
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

			disableInsuranceStep();
		});

		expectedPriceForm.on('keyup', '#getExpectedPrice', function () {
			var recommendedCheckbox = expectedPriceForm.find('#recommendedCheckbox');

			if (recommendedCheckbox.is(':checked')) {
				recommendedCheckbox.prop('checked', false);
			}
		});

		formatValue.formatValueOnInput(getExpectedPrice);

		$('#submitKilometerExpectedPrice').on('click', function() {
			submitKmsExpectedPriceForm();
		});

		$('#bodyKilometer, #expectedPriceInputBox').on('input propertychange', function () {
			disableInsuranceStep();
		});

		// insurance
		insuranceForm.on('change', 'input[name=carInsurance]', function () {
			var formStep = $(this).closest('.form__step');

			horizontalForm.setStepSelection(formStep);

			if ($(this).val() != 3) {
				$('#insuranceValidity').show();
				insuranceForm.addClass('validity-active');
			}
			else {
				resetInsuranceForm();
			}

			$('#submitInsurance').show();
		});

		insuranceForm.on('click', '.step__selection', function () {
			var formStep = $(this).closest('.form__step');

			resetInsuranceForm();
			horizontalForm.resetFormStep(formStep);
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

		$('#submitInsurance').on('click', function() {
		    submitInsurance();
		    $('#modalBg').attr("data-current", "otp-popup-container");
		});
	};

	function fetchModel(makeId, year) {
		var modelType = "used";
		var url = "/webapi/carmodeldata/GetCarModelsByType/?type=" + modelType + "&makeId=" + makeId + "&year=" + year;
		return ajaxRequest.getJsonPromise(url);
	};

	function bindModelRadioList(resp, modelField) {
	    var bindingObj = resp.map(function (obj) {
			return { val: obj.ModelId, text: obj.ModelName };
	    });
	    bindingObj = cardetailsUtil.removeModelYear(bindingObj);
		modelField.find('.step__option-list').html(templates.fillRadioListTemplate(modelField, bindingObj).join(''));
	};

	function fetchVersion(modelId, year) {
		var versionType = "used";
		var url = "/webapi/carversionsdata/GetCarVersions/?type=" + versionType + "&modelId=" + modelId + "&year=" + year;
		return ajaxRequest.getJsonPromise(url);
	};

	function bindVersionRadioList(resp, versionField) {
		var bindingObj = resp.map(function (obj) {
			return { val: obj.ID, text: obj.Name };
		});
		versionField.find('.step__option-list').html(templates.fillRadioListTemplate(versionField, bindingObj).join(''));
	};

	function fetchColor(versionId) {
		var url = "/api/versions/colors/?vids=" + versionId;
		return ajaxRequest.getJsonPromise(url);
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
		colorField.html(colorLi.join(''));
	};

	function submitCarDetails(){
		var selectedOption = alternateFuelBody.find('input[name=alternateFuel]:checked'),
			selectedOptionLabel = $.trim($('label[for=' + selectedOption.attr('id') + ']').text());

		if (selectedOption.val() !== "0") {
			carDetailsData = appState.setSelectedData(carDetailsData, {
				alternateFuel: selectedOptionLabel
			});
		}
		else {
			carDetailsData = appState.deleteObjectProperties(carDetailsData, ["alternateFuel"]);
		}

		carDetailsData = appState.setSelectedData(carDetailsData, {
			referrer: document.referrer,
			sourceId: parentContainer.getSourceId()
		});
		
		//trackSellCar.track('selectColor');
	};

	function submitColour() {
		if (validateColorForm()) {
			var selectedOption = colorSelectionForm.find('input[name=carColour]:checked');
			var selectedOptionLabel = $.trim($('label[for=' + selectedOption.attr('id') + ']').text());

			if (selectedOption.val() !== "0") {
				carDetailsData = appState.setSelectedData(carDetailsData, {
					color: selectedOptionLabel
				});
			}
			else {
				carDetailsData = appState.setSelectedData(carDetailsData, {
					color: $.trim($('#getOtherColour').val())
				});
			}
			//trackSellCar.track('selectOwners');

			colorSelectionForm.addClass('step--done').find('.selection__label').text($('#getOtherColour').val());
			horizontalForm.setNextStep(colorSelectionForm);
		}
	};

	function validateColorForm() {
	    var isValid = false;
	    var otherColorField = $('#getOtherColour');
		var checkedOption = colorSelectionForm.find('input[name=carColour]:checked');

		if (checkedOption.val() == 0) {
		   if (otherColorField.val().length === 0) {
		        validate.field.setError(otherColorField, 'Please enter a colour');
		    } else if (!validate.validateTextOnly(otherColorField.val())) {
			    validate.field.setError(otherColorField,'Please enter valid color');
			}
			else {
			    isValid = true;
			}
		} else {
		    isValid = true;
		}

		return isValid;
	};

	function submitOwner() {
		if (validateOwner()) {
			var owners = $.trim(ownerSelectionForm.find('input[name=carOwner]:checked').val());
			carDetailsData = appState.setSelectedData(carDetailsData, {
				owners: owners
			});
			//trackSellCar.track('enterKms');
			resetBuyingIndexPrice(expectedPriceForm);
			if (typeof events != 'undefined') {
			    events.publish("ownersChanged", { owners: owners });
			}
			resetExpectedPriceForm();
			resetInsuranceForm();
			horizontalForm.setStepSelection(ownerSelectionForm);
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

	function submitRegistrationType() {
	    if (validateRegistrationType()) {
	        var regType = $.trim(registrationTypeSelectionForm.find('input[name=carRegistrationType]:checked').val());
	        carDetailsData = appState.setSelectedData(carDetailsData, {
	            regtype: regType
	        });
	        resetInsuranceForm();
	        horizontalForm.setStepSelection(registrationTypeSelectionForm);
	    }
	}

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

	function buyingIndexSuccess(response) {
	    if (response && response.right_price) {
	        bindBuyingIndexPrice(response, expectedPriceForm);
	        carDetailsData = appState.setSelectedData(carDetailsData, {
	            RecommendedPrice: $.trim(response.right_price)
	        });
	        sellCarTracking.forDesktop("recomPriceShown");
	    }
	    else {
	        resetBuyingIndexPrice(expectedPriceForm);
	    }
	}

	function buyingIndexFailed(response) {
	    resetBuyingIndexPrice(expectedPriceForm);
	}

	function submitKmsExpectedPriceForm() {
		if(validateKmExpectedPriceForm()) {
			submitKmsDriven();
			submitExpectedPrice();

			horizontalForm.setNextStepGroups($('#bodyExpectedPrice'));
			setInsuranceDropdown();
		}
	};

	function validateKmExpectedPriceForm() {
		var isValid = false;

		isValid = validateKms();
		isValid &= validateExpectedPrice();

		return isValid;
	};

	function submitKmsDriven() {
		var kms = $.trim(kmsField.attr('data-value'));
		carDetailsData = appState.setSelectedData(carDetailsData, {
			kmsDriven: kms
		});

		$('#bodyKilometer').find('.selection__label').text(kmsField.val());
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
		var price = $.trim(getExpectedPrice.attr('data-value'));
		carDetailsData = appState.setSelectedData(carDetailsData, {
			expectedPrice: price
		});

		$('#bodyExpectedPrice').find('.selection__label').text(getExpectedPrice.val());
		if ($(".recommend-price-box").is(":visible")) {
		    if ($("#recommendedCheckbox").is(":checked")) {
		        sellCarTracking.forDesktop("recomPriceSelect");
		    }
		    else {
		        sellCarTracking.forDesktop("expectPrice", ($("#recommendedCheckbox").val() - $("#getExpectedPrice").attr("data-value")).toString());
		    }
		}
		else {
		    sellCarTracking.forDesktop("expectPrice", "ExpectPriceFilled");
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
			validate.field.setError(getExpectedPrice, 'Expected Price should be below or equal to 10 Crore');
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
		getExpectedPrice.siblings("div .getNumbersInWord").text("");
	};

	function setInsuranceDropdown() {
		var selectBox = $('#insuranceValidity').find('.select-box');

		selectBox.each(function () {
			var element = $(this);

			element.find('.chosen-select').chosen({
				width: '100%'
			});

			if (element.hasClass('select-box-no-input')) {
				chosenSelect.removeInputField(element);
			}
		});
	};

	function submitInsurance() {
	    parentContainer.setLoadingScreen();
	    if (!(validateInsurance() && validateRegistrationNo() && validateRegistrationType())) {
	        parentContainer.removeLoadingScreen();
			return false;
		}
		var insuranceValue = insuranceForm.find('input[name=carInsurance]:checked').val();
		carDetailsData = appState.setSelectedData(carDetailsData, {
			insurance: insuranceValue
		});

		if (insuranceValue && insuranceValue !== '3') {
			var insuranceExpiryMonth = $.trim(insuranceMonthField.find('option:selected').val());
			var insuranceExpiryYear = $.trim(insuranceYearField.find('option:selected').val());

			carDetailsData = appState.setSelectedData(carDetailsData, {
				insuranceExpiryYear: insuranceExpiryYear,
				insuranceExpiryMonth: insuranceExpiryMonth
			});
		}
		else {
			carDetailsData = appState.deleteObjectProperties(carDetailsData, ['insuranceExpiryYear', 'insuranceExpiryMonth']);
		}

		var regState = registrationForm.find('.register--input-1').val();
		var regNum = registrationForm.find('.register--input-2').val();
		var registrationNo = $.trim(regState) + $.trim(regNum);

		carDetailsData = appState.setSelectedData(carDetailsData, {
			registrationNumber: registrationNo,
			takeLive: true,
        });

		$('#registrationNumber').find('.selection__label').text(registrationNo.toUpperCase());
		
	    // for cardetails
		var settings = {
		    url: "/api/used/sell/cardetails/?tempid=" + encodeURIComponent($.cookie("TempInquiry")),
		    type: "POST",
		    data: carDetailsData
		}
		var inquiryCookie = sellCarCookie.getSellInquiryCookie();
		if (inquiryCookie) {
		    settings.url = "/api/v1/used/sell/cardetails/?encryptedId=" + encodeURIComponent(inquiryCookie);
		    settings.type = "PUT";
		    $.ajax(settings).done(function (response) {
		        parentContainer.removeLoadingScreen();
		        var eventObj = {
		            data: carDetailsData
		        };
		        if (typeof events != 'undefined') {
		            if (isCarImagesViewLoaded) {
		                events.publish('cardetailsUpdated', eventObj);
		            } else {
		                events.publish('takenLive', eventObj);
		            }
		        }
		    }).fail(function (xhr) {
		        parentContainer.removeLoadingScreen();
		        modalPopup.showModalJson(xhr.responseText);
		    });
		} else {
		    $.ajax(settings).done(function (response, msg, xhr) {
		        if (typeof events !== 'undefined') {
		            var eventObj = {
		                data: carDetailsData,
		                callback: onMobileverified
		            };
		            events.publish("carDetailsPosted", eventObj);
		        }
		    }).fail(function (xhr) {
		        parentContainer.removeLoadingScreen();
		        modalPopup.showModalJson(xhr.responseText);
		    });
		}
		setFormSummary();
	};

	function onMobileverified(response,data) {
	    if (response.isMobileVerified) {
	        if (typeof events !== 'undefined') {
	            var eventObj = {
	                data: data,
	            };
	            events.publish("mobileVerified", eventObj);
	        }
	    }
	    else {
	        //show OTP screen
	        if (typeof events !== 'undefined') {
	            var eventObj = {
	                data: data,
	            };
	            events.publish("mobileUnverified", eventObj);
	        }

	    }
	}

	function resetInsuranceForm() {
		$('#insuranceValidity').hide();
		insuranceForm.removeClass('validity-active');
		formField.emptySelect(insuranceMonthField);
		formField.emptySelect(insuranceYearField);
		$('#submitInsurance').hide();
	};

	function disableInsuranceStep() {
		if (insuranceForm.hasClass('step-group--active')) {
			horizontalForm.resetFormStep($('#bodyExpectedPrice'));
			resetInsuranceForm();
		}
	}

	function setFormSummary() {
		var formStepGroup = $('#formCarDetail').find('.form__step-group'),
			formSummary = [];

		formStepGroup.each(function() {
			var formStep = $(this).find('.form__step');

			formStep.each(function() {
				if ($(this).closest('.group-box').length) {
					var groupBox = $(this).closest('.group-box'),
						combinedFieldText = '';

					if(!groupBox.hasClass('step--accessed')) {
						groupBox.find('.form__step').each(function() {
							var formSelection = $(this).find('.selection__label');

							if (formSelection.length) {
								combinedFieldText += formSelection.text() + ' ';
							}
						});

						groupBox.addClass('step--accessed');
						formSummary.push(combinedFieldText);
					}
				}
				else {
					var fieldSelection = $(this).find('.selection__label');

					if (fieldSelection.length) {
						var fieldText = summary.getSelectionText(fieldSelection);

						if (fieldText.length) {
							formSummary.push(fieldText);
						}
					}
				}
			})
		});

		var summaryText = formSummary.join(' | '),
			accordionHead = $('.body__car-details').closest('.accordion__item').find('.accordion__head');
		
		summary.setToolbar(summaryText, accordionHead);
		summary.resetGroupAccessFlag($('#formCarDetail'));
	};

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

	function validateRegistrationType() {
	    var isValid = false;

	    if (registrationTypeSelectionForm.find('input[name=carRegistrationType]:checked').length === 0) {
	        validateScreen.setError('Please select registration type');
	    }
	    else {
	        isValid = true;
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

	function setEditToolTip() {
		if (!yearField.hasClass('tooltip-set') && carSelectForm.hasClass('step-group--active')) {
			yearField.addClass('tooltip-set');

			var tooltip = modalTooltip.getTemplate('Click again to change', 'edit-tooltip');

			modalTooltip.setTooltip($('#editTooltipContent'), tooltip);
			
			$('.edit-tooltip').fadeIn();
			var tooltipTimeOut = setTimeout(function () {
				$('.edit-tooltip').fadeOut();
				clearTimeout(tooltipTimeOut);
			}, 3000);
		}
	}

})();

var horizontalForm = (function () {
    function setStepSelection(formStep) {       
        var optionList = formStep.find('.step__option-list');
        var currentStep = formStep.attr('id');
        if (currentStep) {
            sellCarTracking.forDesktop(currentStep);
        }
		if (optionList.hasClass('radio-box')) {
			var selectionText = optionList.find('input[type="radio"]:checked').next('label').text();
			formStep.find('.selection__label').text(selectionText);
		}
		formStep.addClass('step--done');
		setNextStep(formStep);
	}

	function setNextStep(formStep) {
		var nextStep = formStep.next('.form__step');

		if (nextStep.length) {
			nextStep.addClass('step--active');
			if (nextStep.find('.step__error').length) {
				nextStep.find('.step__error').text('');
			}
			setScrollbar(nextStep);
		}
		else {
			setNextStepGroups(formStep);
		}
	}

	function setNextStepGroups(formStep) {
		var nextStepGroup = formStep.closest('.form__step-group').next('.form__step-group'),
			previewStepGroup = nextStepGroup.next('.form__step-group');

		nextStepGroup.removeClass('step-group--preview').addClass('step-group--active');
		previewStepGroup.addClass('step-group--preview');
		previewStepGroup.find('.form__step').first().addClass('step--active');

		setScrollbar(nextStepGroup.find('.form-step__body').first());
	}

	// reset
	function resetFormStep(formStep) {
		resetStep(formStep);
		resetNextSteps(formStep);
		resetOtherColor();
	}

	function resetStep(formStep) {
		var optionList = formStep.find('.step__option-list');

		formStep.removeClass('step--done').find('.selection__label').text('');

		if (optionList.hasClass('radio-box')) {
			optionList.find('input[type="radio"]:checked').prop('checked', false);
		}
	}

	function resetNextSteps(formStep) {
		var nextAllSteps = formStep.nextAll('.form__step.step--active');
			
		nextAllSteps.each(function () {
			$(this).removeClass('step--active');
			resetStep($(this));
		});

		resetNextStepGroups(formStep);
	}

	function resetOtherColor() {
	    var otherColorBox = $("#otherColorField");
	    var otherColorField = $("#getOtherColour");
	    otherColorField.val('');
	    validate.field.hideError(otherColorField);
	    otherColorBox.hide();
	}
	function resetNextStepGroups(formStep) {
		var nextStepGroup = formStep.closest('.form__step-group').next('.form__step-group');
		nextStepGroup.removeClass('step-group--active').addClass('step-group--preview');

		var nextActiveSteps = nextStepGroup.find('.form__step');
		nextActiveSteps.each(function () {
			if (!$(this).index()) {
				$(this).addClass('step--active');
			}
			else {
				$(this).removeClass('step--active');
			}
			resetStep($(this));
		});

		// next2NextStepGroup
		var next2NextStepGroup = nextStepGroup.nextAll('.form__step-group');
		next2NextStepGroup.removeClass('step-group--active step-group--preview');

		var next2NextActiveSteps = next2NextStepGroup.find('.form__step.step--active');
		next2NextActiveSteps.each(function () {
			$(this).removeClass('step--active');
			resetStep($(this));
		});
	}

	function setInitialSteps(formContainer) {
		var firstFormGroup = formContainer.find('.form__step-group').first(),
			secondFormGroup = firstFormGroup.next('.form__step-group');

		firstFormGroup.addClass('step-group--active');
		secondFormGroup.addClass('step-group--preview');
		$(firstFormGroup).find('.form__step').first().addClass('step--active');
		$(secondFormGroup).find('.form__step').first().addClass('step--active');

		setScrollbar(firstFormGroup.find('.form__step').first().find('.form-step__body'));
	}

	function setScrollbar(formStep) {
		var formStepBody = formStep.find('.form-step__body').length ? formStep.find('.form-step__body') : formStep;

		if (formStepBody.hasClass('step--scrollbar')) {
			var formContainer = formStep.closest('.horizontal-form'),
				listHeight = ($(formContainer).height() - 25) - (formStep.offset().top - $(formContainer).offset().top);

			formStepBody.css({
				'height': listHeight + 'px',
				'overflow-y': 'scroll'
			});
		}
		
	}

	function setError(formStep, message) {
		formStep.find('.step__error').text(message);
	}

	function hideError(formStep) {
		formStep.find('.step__error').text('');
	}

	return {
		setInitialSteps: setInitialSteps,
		setStepSelection: setStepSelection,
		setNextStep: setNextStep,
		setNextStepGroups: setNextStepGroups,
		resetFormStep: resetFormStep,
		setError: setError,
		hideError: hideError
	}

})();

