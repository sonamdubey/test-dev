var readMoreTarget, dealerMoreContent;
var clientip;

var attemptCount = 1,
    successMessage = 'Service Center details successfully<br />sent on your phone.<br />Not Received? <span class="service-center-resend-btn">Resend</span>',
    threeAttemptsMessage = 'Sorry! You have reached the limit of sending details of this service center. Look for a different service center.',
    failureMessage = "Sorry!, Something went wrong. Please try again.",
    tenAttemptsMessage = 'Sorry! You have reached the daily limit of sending details.<br />Please try again after a day.';


docReady(function () {
    clientip = $("#dealerLead").attr("data-clientip");

    // read more-collapse
    readMoreTarget = $('#read-more-target'), dealerMoreContent = $('#dealer-more-content');

    readMoreTarget.on('click', function () {
        if (!dealerMoreContent.hasClass('active')) {
            dealerMoreContent.addClass('active');
            readMoreTarget.text('Collapse');
        }
        else {
            dealerMoreContent.removeClass('active');
            readMoreTarget.text('Read more');
        }
    });
    $(".leadcapturebtn").click(function (e) {
        ele = $(this);
        var leadOptions = {
            "dealerid": ele.attr('data-item-id'),
            "dealername": ele.attr('data-item-name'),
            "dealerarea": ele.attr('data-item-area'),
            "campid": ele.attr('data-campid'),
            "leadsourceid": ele.attr('data-leadsourceid'),
            "pqsourceid": ele.attr('data-pqsourceid'),
            "isdealerbikes": true,
            "pageurl": window.location.href,
            "isregisterpq": true,
            "clientip": clientip,
            "dealercityname": ele.attr('data-cityname'),
            "dealerareaname": ele.attr('data-areaname'),
            "eventcategory": ele.attr('data-eventcategory')
        };
        dleadvm.setOptions(leadOptions);
    });

	// showrooms in popular cities carousel
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

	$('.dealer-details-form__target').on('click', function () {
		var listItem = $(this).closest('.dealer-card-target');
		var activeListItem = $('#dealersList').find('.input-active');

		if (activeListItem.length) {
			activeListItem.removeClass('input-active');
			activeListItem.find('.form-field__content.btn--active').removeClass('btn--active');
			activeListItem.find('.form-field__input').attr('placeholder', 'Enter your mobile number');
		}

		listItem.removeClass('response-active').addClass('input-active');

		var inputbox = listItem.find('.form-field__input');

		if (inputbox.val().length) {
			inputbox.focus();
		}
    	
    });

    $('.dealer-form__submit-btn').on('click', function () {
    	var inputContainer = $(this).closest('.dealer-details__form');
    	var inputbox = inputContainer.find('.form-field__input');
    	var listItem = $(this).closest('li');

    	var isValid = validatePhone(inputbox);

    	if (isValid) {
    	    listItem.removeClass('input-active').addClass('response-active');
    	    if (attemptCount < 4)
    	    {
    	        var dealerId = $(this).attr("data-id");
    	        var obj = {
    	            "mobilenumber": inputbox.val(),
    	            "pageurl": window.location.href.replace('&', '%26'),
    	            "id": dealerId
    	        }
    	        $.ajax({
    	            type: "POST",
    	            url: "/api/dealer/",
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

    $('.form-field__input').on('focus', function () {
    	var fieldContainer = $(this).closest('.form-field__content');

    	if (!fieldContainer.hasClass('btn--active')) {
    		fieldContainer.addClass('btn--active');
    		$(this).attr('placeholder', 'Your mobile number');
    	}

    	validateForm.onFocus($(this));
    });

    var captureLeadMobile = {
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

	// resend details
    $('#dealersList').on('click', '.service-center-resend-btn', function () {
    	var resendBtn = $(this),
            listItem = resendBtn.closest('li');

    	captureLeadMobile.inputBox.set(listItem);
    });

});