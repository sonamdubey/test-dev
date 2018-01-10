var readMoreTarget, dealerMoreContent;
var clientip;



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

		activeListItem.removeClass('input-active');
		activeListItem.find('.form-field__content.btn--active').removeClass('btn--active');
		listItem.removeClass('response-active').addClass('input-active');

		var inputbox = listItem.find('.form-field__input');

		if (inputbox.val().length) {
			inputbox.focus();
		}
    	
    });

    $('.dealer-form__submit-btn').on('click', function () {
    	var inputContainer = $(this).closest('.dealer-details__form');
    	var inputbox = inputContainer.find('.form-field__input');
    	var listElement = $(this).closest('li');

    	var isValid = validatePhone(inputbox);

    	if (isValid) {
    		listElement.removeClass('input-active').addClass('response-active');
    		$(this).closest('.dealer-details__form-content').append('<p class="dealer-form__response">Details has been sent</p>');
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

});