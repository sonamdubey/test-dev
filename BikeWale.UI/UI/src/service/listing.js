var readMoreTarget, validate, captureLeadMobile;
// send service center details
var attemptCount = 1,
    successMessage = 'Service Center details successfully<br />sent on your phone.<br />Not Received? <span class="service-center-resend-btn">Resend</span>',
    threeAttemptsMessage = 'Sorry! You have reached the limit of sending details of this service center. Look for a different service center.',
    failureMessage = "Sorry!, Something went wrong. Please try again.",
    tenAttemptsMessage = 'Sorry! You have reached the daily limit of sending details.<br />Please try again after a day.';

docReady(function () {
    captureLeadMobile = {
        inputBox: {
            set: function (listElement) {
                var inputContainer = listElement.find('.lead-mobile-content');

                listElement.removeClass('response-active').addClass('input-active');
                inputContainer.find('input').focus();
            }
        },

        responseBox: {
            set: function (listElement, stateClass, message) {
                var responseContainer = listElement.find('.response-text');

                listElement.removeClass('input-active').addClass(stateClass);
                responseContainer.html(message).show();
            }
        },

        checkAttempts: function (count, listItem) {
            if (count < 4) {
                captureLeadMobile.responseBox.set(listItem, 'response-active', successMessage);
            }

            if (count == 4) {
                captureLeadMobile.responseBox.set(listItem, 'response-active limit-reached', threeAttemptsMessage);
            }

            if (count == 10) {
                captureLeadMobile.responseBox.set(listItem, 'response-active limit-reached', tenAttemptsMessage);
            }
        }
    };

    // read more-collapse
    readMoreTarget = $('#read-more-target'),
    serviceMoreContent = $('#service-more-content');

    function validatePhone(inputElement) {
    	var leadMobileNo = inputElement.val(),
			isValid = true,
			reMobile = /[7-9][0-9]{9}$/;

    	if (leadMobileNo == "") {
    		validateForm.setError(inputElement, "Enter your mobile number.");
    		isValid = false;
    	}
    	else if (leadMobileNo[0] < 7) {
    		validateForm.setError(inputElement, "Enter a valid mobile number.");
    		isValid = false;
    	}
    	else if (!reMobile.test(leadMobileNo) && isValid) {
    		validateForm.setError(inputElement, "10 digit mobile number only.");
    		isValid = false;
    	}
    	else
    		validateForm.hideError(inputElement)
    	return isValid;
    }

    /* form validation */
    var validateForm = {
    	setError: function (element, message) {
    		var elementLength = element.val().length;
    		errorTag = element.siblings('.field__error-message');

    		errorTag.show().text(message);
    		element.closest('.form-field__content').addClass('field--invalid');
    	},

    	hideError: function (element) {
    		element.closest('.form-field__content').removeClass('field--invalid');
    		element.siblings('.field__error-message').text('');
    	},

    	onFocus: function (inputField) {
    		if (inputField.closest('.form-field__content').hasClass('field--invalid')) {
    			validateForm.hideError(inputField);
    		}
    	}
    }

    readMoreTarget.on('click', function () {
        if (!serviceMoreContent.hasClass('active')) {
            serviceMoreContent.addClass('active');
            readMoreTarget.text('Collapse');
        }
        else {
            serviceMoreContent.removeClass('active');
            readMoreTarget.text('Read more');
        }
    });

	// service center carousel
    $('.carousel-type-city').jcarousel();

    $('.carousel-type-city .jcarousel-control-prev')
		.on('jcarouselcontrol:active', function () {
			$(this).removeClass('inactive');
		})
		.on('jcarouselcontrol:inactive', function () {
			$(this).addClass('inactive');
		})
		.jcarouselControl({
			target: '-=2'
		});

    $('.carousel-type-city .jcarousel-control-next')
		.on('jcarouselcontrol:active', function () {
			$(this).removeClass('inactive');
		})
		.on('jcarouselcontrol:inactive', function () {
			$(this).addClass('inactive');
		})
		.jcarouselControl({
			target: '+=2'
		});

    $('.brand-type-carousel').jcarousel();

    $('.brand-type-carousel .jcarousel-control-prev')
		.on('jcarouselcontrol:active', function () {
			$(this).removeClass('inactive');
		})
		.on('jcarouselcontrol:inactive', function () {
			$(this).addClass('inactive');
		})
		.jcarouselControl({
			target: '-=2'
		});

    $('.brand-type-carousel .jcarousel-control-next')
		.on('jcarouselcontrol:active', function () {
			$(this).removeClass('inactive');
		})
		.on('jcarouselcontrol:inactive', function () {
			$(this).addClass('inactive');
		})
		.jcarouselControl({
			target: '+=2'
		});


    $('#center-list').on('click', '.get-details-btn', function () {
        var getDetailsBtn = $(this),
            listItem = getDetailsBtn.closest('li'),
			activeListItem = $('#center-list').find('.input-active');

        var inputbox = $(this).closest('.dealer-details__form-content').find('.form-field__input');
        //inputbox.attr('placeholder', 'Enter your mobile number');

        if (attemptCount != 10) {
        	if (activeListItem.length) {
        		activeListItem.removeClass('input-active');
        		activeListItem.find('.form-field__content.btn--active').removeClass('btn--active');
        		activeListItem.find('.form-field__input').attr('placeholder', 'Enter your mobile number');
        	}

        	if (inputbox.val().length) {
        		captureLeadMobile.inputBox.set(listItem);
        	}
			else{
        		listItem.removeClass('response-active').addClass('input-active');
			}
		}
        else {
            captureLeadMobile.responseBox.set(listItem, 'response-active limit-reached', tenAttemptsMessage);
        }
        attemptCount = 1;
    });

    $('.service-center-lead-mobile').on('focus', function () {
    	var fieldContainer = $(this).closest('.form-field__content');

    	if (!fieldContainer.hasClass('btn--active')) {
    		fieldContainer.addClass('btn--active');
    		$(this).attr('placeholder', 'Your mobile number');
    	}

    	validateForm.onFocus($(this));
    });

    $('.submit-service-center-lead-btn').on('click', function () {
        var sendBtn = $(this),
            listItem = sendBtn.closest('li'),
            inputbox = listItem.find('input'),
            valid = validatePhone(inputbox);

        $('.lead-mobile-content input').val(inputbox.val());

        var serviceCenterId = $(this).attr("data-id");

        if (!valid) {
            // invalid
        }
        else {
            if (attemptCount < 4) {
                var obj = {
                    "mobilenumber":inputbox.val(),
                    "pageurl":window.location.href.replace('&', '%26'),
                    "id": serviceCenterId
                }
                $.ajax({
                    type: "POST",
                    url: "/api/servicecenter/",
                    contentType: "application/json",
                    data: ko.toJSON(obj),
                    success: function (response) {
                        if (response == 2) {
                            captureLeadMobile.checkAttempts(10, listItem);
                        }
                        else if (response == 1) {
                            captureLeadMobile.checkAttempts(attemptCount, listItem);
                        }
                        else {
                            captureLeadMobile.responseBox.set(listItem, 'response-active limit-reached', failureMessage);
                        }
                    },
                    failure: function (xhr, ajaxOptions, thrownError) {
                        captureLeadMobile.responseBox.set(listItem, 'response-active limit-reached', failureMessage);
                    },
                    complete: function (xhr, ajaxOptions, thrownError) {
                        if (xhr.status != 200)
                            captureLeadMobile.responseBox.set(listItem, 'response-active limit-reached', failureMessage);
                    }
                });
            }
            else {
                captureLeadMobile.checkAttempts(attemptCount, listItem);
            }
        }
    });

    // resend details
    $('#center-list').on('click', '.service-center-resend-btn', function () {
        var resendBtn = $(this),
            listItem = resendBtn.closest('li');
        if (attemptCount != 4) {
            captureLeadMobile.inputBox.set(listItem);
        }
        else {
            captureLeadMobile.responseBox.set(listItem, 'response-active limit-reached', threeAttemptsMessage);
        }

        attemptCount++;
    });
});


function getLocationUrl(lat, long) {

    var url = "https://maps.google.com/?saddr=&daddr=" + lat + "," + long;

    window.open(url);
};
