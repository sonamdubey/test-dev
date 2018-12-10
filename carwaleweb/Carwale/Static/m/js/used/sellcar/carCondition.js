// dependency
// 1. static/m/js/used/sellcar/common.js

var carCondition = (function() {
	var container,
		bodyItemGroup;

	var carConditionData = {};

	if (typeof events !== 'undefined') {
        // this form will load async, hence no document ready function
        //The caller needs to publish 'carConditionLoaded' event to show the form
	    events.subscribe("carConditionLoaded", carConditionFormLoadHandler);
	    events.subscribe("carConditionHistoryBack", showScreenFromHistory);
	    events.subscribe("navigateAwayCondition", navigateAwayCondition);
	}
	
	function navigateAwayCondition() {
	    history.back();
	    parentContainer.setLoadingScreen();
	    events.publish('navigateAway', { container: container });
	}

	function carConditionFormLoadHandler(eventObj) {
		setSelectors();
		registerDomEvents();
		setInitialQuestion();
		if (eventObj && eventObj.data)
		    carConditionData = appState.setSelectedData(carConditionData, eventObj.data);
	}

	function setSelectors() {
		container = $('#formCarCondition');
		bodyItemGroup = container.find('.body-group');
	};

	function setInitialQuestion() {
		summary.hideToolbar();
		$('body').removeClass('image-form-active');

		parentContainer.setNavigationTab('formCarCondition');
		parentContainer.removeLoadingScreen();

		container.show();
		bodyItemGroup.find('.body__item').first().addClass('active');

		parentContainer.setButtonTarget('history.back()', 'carCondition.submitCarCondition()');
	};

	function registerDomEvents() {
		bodyItemGroup.on('change', '.item__radio-group input[type=radio]', function() {
			var bodyItem = $(this).closest('.body__item.active'),
				conditionalBody = bodyItem.find('.item__condition-body');
            
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
		});

		container.on('change', '.pill-group--checkbox input[type="checkbox"]', function(event) {
			var checkboxGroup = $(this).closest('.pill-group--checkbox');
			checkOtherSelection(checkboxGroup);
		});

		container.on('input propertychange', '.input--format-value', formatValue.withComma);
	};

	function showConditionalBody(conditionalBody, bodyItem)
	{
	    if (conditionalBody.length) {
	        conditionalBody.show();
	    }
	    else {
	        nextGroupItem(bodyItem);
	    }
	}

	function hideConditionalBody(conditionalBody, bodyItem) {
	    conditionalBody.hide();
	    resetConditionBody(conditionalBody);
	    nextGroupItem(bodyItem);
	}

	function resetConditionBody(conditionalBody) {
		conditionalBody.find('.input-box').removeClass('invalid done');
		conditionalBody.find('input[type="text"], input[type="tel"], input[type="number"]').val('');
		conditionalBody.find('input[type="checkbox"], input[type="radio"]').attr('checked', false);
		conditionalBody.find('.item__subcondition-body').hide();
	}

	function checkOtherSelection(checkboxGroup) {
		var isChecked = false;

		checkboxGroup.find('input[type="checkbox"]:checked').each(function() {
			if($(this).val() == "0") {
				isChecked = true;
			}
		});

		var nestedBody = checkboxGroup.siblings('.item__subcondition-body');

		if(isChecked && nestedBody.length) {
			nestedBody.show();
		}
		else {
			nestedBody.hide();
		}
	}

	function prevGroupItem(bodyItem) {
		var prevBody = bodyItem.prev('.body__item');

		if(prevBody.length) {
			bodyItem.removeClass('active');
			prevBody.addClass('active');
		}		
	};

	function nextGroupItem(bodyItem) {
		var nextBody = bodyItem.next('.body__item');

		if (nextBody.length) {
			setTimeout(function() {
				saveHistory(nextBody.data('questiontype'));
				bodyItem.removeClass('active');
				nextBody.addClass('active');
				sellCarTracking.forMobile(bodyItem.data("questiontype"),
                    bodyItem.find('input[type=radio]:checked').length > 0 ? (bodyItem.find('input:radio').prop('checked')).toString() : "skipped");
			}, 200);
		}
		else {
		    sellCarTracking.forMobile("mechanicalIssueQuestion",
                bodyItem.find('input[type=radio]:checked').length > 0 ? (bodyItem.find('input:radio').prop('checked')).toString() : "skipped");
		    parentContainer.setLoadingScreen();
		    var carConditionJson = getCarConditionJson();
		    var partialPlanScreen = $('#planScreen');
		    var encryptedId = encodeURIComponent($.cookie("SellInquiry"));
		    var settings = {
		        url: '/api/used/sell/carcondition/?inquiryId=' + encryptedId+"&cityId="+cityForm.getCityData().cityId,
		        type: 'POST',
		        data: carConditionJson
		    }
		    $.ajax(settings).done(function (response, msg, xhr) {
		        events.publish("navigateAway", { container: container });
		    }).fail(function (xhr) {
		        parentContainer.removeLoadingScreen();
		        modalPopup.showModalJson(xhr.responseText);
		    });
		}	
	};

	function showScreenFromHistory(eventObj) {
	    if (eventObj && !modalPopup.closeActiveModalPopup()) {
	        var questionType = eventObj;
	        $('#planScreen').hide();
	        $('#formCarCondition').show();
	        $('#formContainer').show();
            $('#formCarCondition').find('.body__item.active').removeClass('active');
	        $('#formCarCondition').find('.body__item[data-questionType="' + questionType + '"]').addClass('active');
	    }
	}

	function saveHistory(id){
	    historyObj.addToHistory(id);
	}

	function checkConditionGroup() {
		var prevFormScreen = false;
			activeBodyItem = bodyItemGroup.find('.body__item.active');

		if(activeBodyItem.index()) {
			prevGroupItem(activeBodyItem);
		}
		else {
			prevFormScreen = true;
		}

		return prevFormScreen;
	};

	function submitCarCondition() {
		var activeBodyItem = bodyItemGroup.find('.body__item.active'),
			conditionBody = activeBodyItem.find('.item__condition-body');

		if(conditionBody.hasClass('validate-body')) {
			if(validateConditionBody(conditionBody)) {
				nextGroupItem(activeBodyItem);
			}
		}
		else if(conditionBody.hasClass('validate-nested-body')) {
			if(validateNestedConditionBody(conditionBody)) {
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

		fields.each(function() {
			if(!$(this).find('input').val().length) {
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

		if(!nestedBody.length) {
			return true;
		}
		else {
			var isValid = false;

			nestedBody.each(function() {
				var fields = $(this).find('.validate-type-input');

				fields.each(function() {
					if(!$(this).find('input').val().length) {
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

	return {
		submitCarCondition: submitCarCondition
	}

})();
