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
	
	//interesting fact popup
	interestingFactPopup.registerEvents();

    	//floating navbar
	floatingNav.registerEvents();
});
var floatingNav = (function () {
    var overallTabsContainer, overallContainer;
    function _setSelectores() {
        overallTabsContainer = $('.overall-tabs__content');
        overallContainer = $('#overallContainer');
    }
    function registerEvents() {
        _setSelectores();
        $(window).scroll(function () {
                var windowScrollTop = $(window).scrollTop(),
                    specsTabsOffsetTop = $('.overall-tabs__placeholder').offset().top,
                    overallContainerHeight = overallContainer.outerHeight(),
                    topNavBarHeight = overallTabsContainer.height();

                var currentActiveTab;

                if (windowScrollTop > specsTabsOffsetTop) {
                    overallTabsContainer.addClass('fixed-tab-nav');
                }

                else if (windowScrollTop < specsTabsOffsetTop) {
                    overallTabsContainer.removeClass('fixed-tab-nav');
                }

                if (overallTabsContainer.hasClass('fixed-tab-nav')) {
                    if (windowScrollTop > Math.ceil(overallContainerHeight) - (topNavBarHeight)) {
                        overallTabsContainer.removeClass('fixed-tab-nav');
                    }
                }
		
                $('#overallContainer .overall-tabs-data').each(function () {
                    var top = $(this).offset().top - topNavBarHeight,
                        bottom = top + $(this).outerHeight();
                    if (windowScrollTop >= top && windowScrollTop <= bottom) {
                        overallTabsContainer.find('li').removeClass('tab--active');
                        $('#overallContainer .overall-tabs-data').removeClass('tab--active');

                        $(this).addClass('tab--active');

                        currentActiveTab = overallTabsContainer.find('li[data-tabs="' + $(this).attr('data-id') + '"]');
                        overallTabsContainer.find(currentActiveTab).addClass('tab--active');
                        //centerItVariableWidth(overallTabsContainer.find('li[data-tabs="' + $(this).attr('data-id') + '"]'), '.overall-tabs__content');
                        

                    }
                });

            var tabElementThird = overallContainer.find('.overall-tabs-data:eq(3)'),
                tabElementSixth = overallContainer.find('.overall-tabs-data:eq(6)'),
                tabElementNinth = overallContainer.find('.overall-tabs-data:eq(9)'),
                tabElementTwelve = overallContainer.find('.overall-tabs-data:eq(13)');

                if (tabElementThird.length != 0) {
                    focusFloatingTab(tabElementThird, 300, 0);
                }

                if (tabElementSixth.length != 0) {
                    focusFloatingTab(tabElementSixth, 600, 300);
                }

                if (tabElementNinth.length != 0) {
                    focusFloatingTab(tabElementNinth, 900, 600);
                }

                if (tabElementTwelve.length != 0) {
                    focusFloatingTab(tabElementTwelve, 1200, 900);
                }

        });
        
        
        $('.overall-tabs__list li').on('click', function () {
            var target = $(this).attr('data-tabs'),
                topNavBarHeight = $('.overall-tabs__content').height();
            $('html, body').animate({ scrollTop: Math.ceil($(".overall-tabs-data[data-id=" + target+"]").offset().top) - topNavBarHeight }, 1000);
            centerItVariableWidth($(this), '.overall-tabs__content');
        });

    }
    
    function focusFloatingTab(element, startPosition, endPosition) {
        var windowScrollTop = $(window).scrollTop();
        if (windowScrollTop > element.offset().top - 45) {
            if (!overallTabsContainer.hasClass('scrolled-left-' + startPosition)) {
                overallTabsContainer.addClass('scrolled-left-' + startPosition);
                scrollHorizontal(startPosition);
            }
        }

        else if (windowScrollTop < element.offset().top) {
            if (overallTabsContainer.hasClass('scrolled-left-' + startPosition)) {
                overallTabsContainer.removeClass('scrolled-left-' + startPosition);
                scrollHorizontal(endPosition);
            }
        }
    };
    
    function scrollHorizontal(pos) {
        $('.overall-tabs__content').animate({ scrollLeft: pos - 15 + 'px' }, 500);
    }
    
    return {
        registerEvents: registerEvents
    }
})();

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


var interestingFactPopup = (function () {
    var fixedPoupContainer, container, readMoreBtn, closeBtn;

    function _setSelectores() {
        fixedPoupContainer = $('#interestingFact');
        closeBtn = $('.interesting-fact-popup .close');
        container = $('.interesting-popup--active');
    }

    function registerEvents() {
        _setSelectores();
        $('.interesting-fact__read-more').on('click', function () {
            var interestingFactContainer = $(this).closest('.interesting-fact-section'),
                interestingFactContent = interestingFactContainer.find('.interesting-fact__content').text();
            open(interestingFactContent);
            history.pushState('interestingFactPopup', '', '');
            _setSelectores();
        });

        closeBtn.on('click', function () {
            if (container.is(':visible')) {
                window.history.back();
            }
        });
        $('.interesting-fact__whiteout-window').on('click', function () {
            if (container.is(':visible')) {
                window.history.back();
            }
        });
        $(".interesting-fact-popup .interesting-fact__content").scroll(function () {
            var interestingFactContent = $(this),
                interestingFactContainer = interestingFactContent.closest('.fact-container__block');
            containerPosition = interestingFactContent.scrollTop();
            if (containerPosition <= 0 && containerPosition > interestingFactContent.outerHeight()) {
                interestingFactContainer.attr('data-overlay', 'none');
            }
            else if (containerPosition <= 0) {
                interestingFactContainer.attr('data-overlay', 'bottom');
            }
            else if (containerPosition+40 > interestingFactContent.innerHeight()- 40) {
                interestingFactContainer.attr('data-overlay', 'top');
            }
            else {
                interestingFactContainer.attr('data-overlay', 'both');
            }
        });
    }

    function open(interestingFactContent) {
        bodyBackground.lock();
        fixedPoupContainer.addClass('interesting-popup--active');
        fixedPoupContainer.find('.interesting-fact__content').text(interestingFactContent);
    }

    function close() {
        bodyBackground.unlock();
        fixedPoupContainer.removeClass('interesting-popup--active');
    }

    $(window).on('popstate', function () {
        if (container.is(':visible')) {
            close();
        }
    });

    $(document).keydown(function (e) {
        if (e.keyCode == 27) {
            if (container.is(':visible')) {
                window.history.back();
            }
        }
    });
    return {
        registerEvents: registerEvents,
        open: open
    }
})();

var bodyBackground = {
    lock: function () {
        var htmlElement = $('html'), bodyElement = $('body');
        if ($(document).height() > $(window).height()) {
            var windowScrollTop = htmlElement.scrollTop() ? htmlElement.scrollTop() : bodyElement.scrollTop();
            if (windowScrollTop < 0) {
                windowScrollTop = 0;
            }
            htmlElement.addClass('lock-browser-scroll').css('top', -windowScrollTop);
        }
    },

    unlock: function () {
        var htmlElement = $('html'),
            windowScrollTop = parseInt(htmlElement.css('top'));
        htmlElement.removeClass('lock-browser-scroll');
        $('html, body').scrollTop(-windowScrollTop);
    }
};
