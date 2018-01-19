docReady(function() {
	$('.model-card__pros-cons').on('click', '.pros-cons__more-btn', function(event) {
		$(this).hide();
		$(this).closest('.pros-cons__content').find('li').show();
	});

	// popular bikes carousel
	$('.carousel__popular-bikes').on('click', '.view-pros-cons__target', function() {
		var modelCard = $(this).closest('.model__card');

		$(this).hide();

		if (!modelCard.hasClass('card--active')) {
			modelCard.addClass('card--active');
		}
		else {
			modelCard.removeClass('card--active');
		}
	});

	$('.carousel__popular-bikes').on('webkitTransitionEnd transitionend', '.model-card__detail', function() {
		var modelCard = $(this).closest('.model__card');		
		var collpaseTargetElement = modelCard.find('.view-pros-cons__target');

		var collapseCurrentText = collpaseTargetElement.html(),
			collapseNextText = collpaseTargetElement.attr('data-text');

		if (!collpaseTargetElement.is(':visible')) {
			if (!modelCard.hasClass('card--active')) {
				modelCard.removeClass('collapse-btn--active');
			}
			else {
				modelCard.addClass('collapse-btn--active');
			}
			
			collpaseTargetElement.attr('data-text', collapseCurrentText);
			collpaseTargetElement.html(collapseNextText).fadeIn();
		}
	});

	// upcoming card: notify
	notifyPopup.registerEvents();

	$('.upcoming-card__notify-btn').on('click', function() {
		notifyPopup.open();
	});

	formField.registerEvents();

});

/* upcoming bikes set notification popup */
var notifyPopup = (function() {
	var container, emailField, formSubmitBtn;

	function _setSelectors() {
		container = $('#notifyPopup');
		emailField = $('#notifyEmailField');
		formSubmitBtn = $('#notifySubmitBtn');
	}

	function _resetForm() {
		formField.resetInputField(emailField);
		emailField.val('');
		formSubmitBtn.html('Submit');
	}

	function registerEvents() {
		_setSelectors();

		formSubmitBtn.on('click', function() {
			var isValid = validateForm.emailField(emailField);

			if(isValid) {
				formField.setSuccessState($(this), 'Thank You!');
			}
		});

		emailField.on('focus', function() {
			validateForm.onFocus($(this));
		});

		$('#notifyWhiteoutWindow, #notifyCloseBtn').on('click', function () {
			close();
			history.back();
		});

		$(window).on('popstate', function () {
			if (container.is(':visible')) {
				notifyPopup.close();
			}
		});
	}

	function open() {
		_resetForm();
		container.addClass('notify-popup--active');
		appendState('notifyPopup');
	}

	function close() {
		container.removeClass('notify-popup--active');
	}

	return {
		registerEvents: registerEvents,
		open: open,
		close: close
	}
})();

/* Form fields */
var formField = (function() {
	function registerEvents() {
		$(document).on('focus', '.form-field__input', function() {
			var fieldContainer = $(this).closest('.form-field__content');

			if (!fieldContainer.hasClass('btn--active')) {
				fieldContainer.addClass('btn--active');
			}
		});
	}

	function resetInputField(inputField) {
		var fieldContainer = inputField.closest('.form-field__content');

		fieldContainer.removeClass('btn--active field--success field--invalid');
		fieldContainer.find('.field__error-message').html('');
	}

	function setSuccessState(btnElement, btnText) {
		var fieldContainer = btnElement.closest('.form-field__content');

		if (!fieldContainer.hasClass('field--success')) {
			fieldContainer.addClass('field--success');
			btnElement.html(btnText);
		}
	}

	return {
		registerEvents: registerEvents,
		resetInputField: resetInputField,
		setSuccessState: setSuccessState
	}
})();

/* Form field validation */
var validateForm = (function() {

	function emailField(inputField) {
		var isValid = true,
			emailVal = inputField.val(),
			reEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;

		if (emailVal == "") {
			setError(inputField, 'Please enter email id');
			isValid = false;
		}
		else if (!reEmail.test(emailVal)) {
			setError(inputField, 'Invalid Email');
			isValid = false;
		}

		return isValid;
	};

	function setError(element, message) {
		var elementLength = element.val().length;
		var errorTag = element.siblings('.field__error-message');

		errorTag.show().text(message);
		element.closest('.form-field__content').addClass('field--invalid');
	}

	function hideError(element) {
		element.closest('.form-field__content').removeClass('field--invalid');
		element.siblings('.field__error-message').text('');
	}

	function onFocus(inputField) {
		if (inputField.closest('.form-field__content').hasClass('field--invalid')) {
			validateForm.hideError(inputField);
		}
	}

	return {
		emailField: emailField,
		setError: setError,
		hideError: hideError,
		onFocus: onFocus
	}

})();
