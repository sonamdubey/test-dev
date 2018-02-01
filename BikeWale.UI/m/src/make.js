docReady(function() {
	$('.model-card__pros-cons').on('click', '.pros-cons__more-btn', function(event) {
		$(this).hide();
		$(this).closest('.pros-cons__content').find('li').show();
	});

	// popular bikes carousel
	if (navigator.userAgent.match(/Firefox/gi)) {
		$('.carousel__popular-bikes').addClass('popular-bikes--fallback');
	}

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

    //recommended bike popup
	recommendedBike.registerEvents();
});

var floatingNav = (function () {
	var overallTabsContainer, overallContainer;

	function _setSelectores() {
		overallTabsContainer = $('.overall-tabs__content');
		overallContainer = $('#overallContainer');
		topNavBarHeight = overallTabsContainer.height();
	}

	function registerEvents() {
		_setSelectores();
		$(window).scroll(function () {
		    var windowScrollTop = $(window).scrollTop(),
                specsTabsOffsetTop = $('.overall-tabs__placeholder').offset().top,
				overallContainerHeight = overallContainer.outerHeight();

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
				    
					$(this).addClass('tab--active');
					var currentActiveTab = overallTabsContainer.find('li[data-tabs="' + $(this).attr('data-id') + '"]');
					if (overallTabsContainer.attr('data-clicked') != '1' && !currentActiveTab.hasClass('tab--active')) {
					    centerNavBar($('li[data-tabs="' + $(this).attr('data-id') + '"]'), overallTabsContainer);
					    overallTabsContainer.find('li').removeClass('tab--active');
					    setTimeout(function () {
					    overallTabsContainer.find('li').removeClass('tab--active');
					    $('#overallContainer .overall-tabs-data').removeClass('tab--active');					      
					        
					      overallTabsContainer.find(currentActiveTab).addClass('tab--active');
					    },10);
					   
					}
					else {
					    overallTabsContainer.find('li').removeClass('tab--active');
					    $('#overallContainer .overall-tabs-data').removeClass('tab--active');
					    overallTabsContainer.find(currentActiveTab).addClass('tab--active');
					}


				}

			});

		});
		$('.overall-tabs__list li').on('click', function () {
			var target = $(this).attr('data-tabs'),
				topNavBarHeight = overallTabsContainer.height();
			overallTabsContainer.attr('data-clicked', '1');
			centerItVariableWidth($(this), overallTabsContainer)
			$('html, body').animate({ scrollTop: Math.ceil($(".overall-tabs-data[data-id=" + target + "]").offset().top) - topNavBarHeight }, 1000, function () {
			    overallTabsContainer.attr('data-clicked', '0');
			});
			
		});

		function centerNavBar(target, outer) {
			var out = $(outer);
			var tar = target;
			var x = out.width();
			var y = tar.outerWidth(true);
			var z = tar.index();
			var q = 0;
			var m = out.find('li');
			//Just need to add up the width of all the elements before our target. 
			for (var i = 0; i < z; i++) {
				q += $(m[i]).outerWidth(true);
			}
			out.animate({ scrollLeft: Math.max(0, q - (x - y) / 2) }, 10, 'swing');
		}
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
				setTimeout(function() {
					$('#notifyCloseBtn').trigger('click');
				}, 1000);
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
			if (container.hasClass('notify-popup--active')) {
				notifyPopup.close();
			}
		});
	}

	function open() {
		_resetForm();
		container.addClass('notify-popup--active');
		appendState('notifyPopup');
		documentBody.lock();
	}

	function close() {
		container.removeClass('notify-popup--active');
		documentBody.unlock();
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
	var container, readMoreBtn, closeBtn;

	function _setSelectores() {
		container = $('#interestingFact');
		closeBtn = $('#interestingFactCloseBtn');
	}

	function registerEvents() {
		_setSelectores();

		$('.interesting-fact__read-more').on('click', function () {
		    var interestingFactContainer = $(this).closest('.interesting-fact-section'),
		        windowScrollTop = $(window).scrollTop(),
                bodyShowableArea = $(window).height() * .30,
		        shownArea = interestingFactContainer.offset().top - windowScrollTop;

		    if (shownArea < bodyShowableArea) { // to move interesting fact container if it is visible in background after popup open 
		        $('html, body').animate({ scrollTop: (windowScrollTop - (bodyShowableArea-shownArea)) }, 100);
		    }
		    open(interestingFactContainer);
		    history.pushState('interestingFactPopup', '', '');

            /* to check content is scrollable on popup to add bottom overlay */
		    if (isScrollable($('.interesting-fact__content'))) {
		        interestingFactContainer.find('.fact-container__block').attr('data-overlay', 'bottom');
		    }

		    /* this timeout required for if background container scrolltop position changed on popup open */
		    setTimeout(function () {
                documentBody.lock();
		    }, 100);

		});

		closeBtn.on('click', function () {
			close();
			history.back();
		});

		$('.interesting-fact__whiteout-window').on('click', function () {
			if (container.hasClass('interesting-popup--active')) {
				history.back();
			}
		});

		$(".interesting-fact-popup .interesting-fact__content").scroll(function () {
			var interestingFactContent = $(this),
				interestingFactContainer = interestingFactContent.closest('.fact-container__block'),
				contentScrollTop = interestingFactContent.scrollTop();
			if (contentScrollTop <= 0) {
				interestingFactContainer.attr('data-overlay', 'bottom');
			}
			else if (contentScrollTop > interestingFactContent.innerHeight()) {
				interestingFactContainer.attr('data-overlay', 'top');
			}
			else {
				interestingFactContainer.attr('data-overlay', 'both');
			}

		});

		$(window).on('popstate', function () {
			if (container.hasClass('interesting-popup--active')) {
				close();
			}
		});
	}

	function open(interestingFactContainer) {
	    var interestingFactContent = interestingFactContainer.find('.interesting-fact__content').html();
		container.addClass('interesting-popup--active');
		container.find('.interesting-fact__content').html(interestingFactContent);
		
	}

	function close() {
		container.removeClass('interesting-popup--active');
		documentBody.unlock();
	}

	function isScrollable(element) {
	    return element[0].scrollWidth > element[0].clientWidth || element[0].scrollHeight > element[0].clientHeight;
	};

	return {
		registerEvents: registerEvents,
		open: open
	}
})();

var documentBody = (function() {
	function lock() {
		var htmlElement = $('html'), bodyElement = $('body');

		if ($(document).height() > $(window).height()) {
			var windowScrollTop = htmlElement.scrollTop() ? htmlElement.scrollTop() : bodyElement.scrollTop();
			if (windowScrollTop < 0) {
				windowScrollTop = 0;
			}
			htmlElement.addClass('lock-browser-scroll').css('top', -windowScrollTop);
		}
	}

	function unlock() {
		var htmlElement = $('html'),
			windowScrollTop = parseInt(htmlElement.css('top'));

		htmlElement.removeClass('lock-browser-scroll');
		$('html, body').scrollTop(-windowScrollTop);
	}

	return {
		lock: lock,
		unlock: unlock
	}

})();

/* Recommended bike popup */
var recommendedBike = (function () {
    var applyBtn, popup, filterList, container, closeBtn;

    function _setSelectores() {
        applyBtn = $('#refineResultApply');
        popup = $('#recommendedBikePopup');
        closeBtn = $('#recommendedBikeCloseBtn');
        container = $('.recommended-bike-container');
        filterList = $('.filter-list');
    }
    function registerEvents() {
        _setSelectores();

        applyBtn.on('click', function (e) {
            open();
            setTimeout(function () {
                $('.model-loader-list__content').hide();
                $('.recommended-bike-list.hide').show();
            }, 3000);
            setTimeout(function () {
                container.find('img.lazy').lazyload();
            }, 200);
            history.pushState('recommendedBikePopup', '', '');

            //add filter item in filter list
            var refineResultContainer = applyBtn.closest('.refine-result').find('.refine-result__list'),
                checkedBox = $('.refine-result__list .refine-result__list-checkbox:checked');
            filterList.empty();
            for (i = 0; i < checkedBox.length; i++) {
                if (filterList.find('.filter-list__item[data-id=' + checkedBox[i].getAttribute('id')+ ']').length === 0)
                filterList.append('<li data-id="' + checkedBox[i].getAttribute('id')+ '"class="filter-list__item"><span class="filter-item">' + checkedBox[i].value + '</span></li>');
            }
        });
        $('.refine-result__list-checkbox').on('click', function () {
            var checkedBox = $('.refine-result__list .refine-result__list-checkbox:checked');
            (checkedBox.length > 0) ? applyBtn.prop('disabled', false) : applyBtn.prop('disabled', true);
        });
        closeBtn.on('click', function () {
            close();
            window.history.back();
        });
        container.on('scroll', function () {
            var otherBikeContainer = $('.other-recommended-bike'),
                recommendedBikeElement = otherBikeContainer.find('.recommended-bike__list-card:first-child'),
                containerHeading = ($('.recommended-bike__found-result').outerHeight() + $('.recommended-bike__filter-section').outerHeight());
            
            /* other recommended overlay */

            if ((recommendedBikeElement.length !== 0) && (recommendedBikeElement.offset().top + (recommendedBikeElement.outerHeight() + containerHeading)) < container.scrollTop() + $(window).height() - containerHeading) {
                otherBikeContainer.removeClass('overlay--inactive');
            }
            else if (recommendedBikeElement.length !== 0) {  // .. 

                otherBikeContainer.addClass('overlay--inactive');
            }

            /* add box shadow to container on top while scroll */
            if (container.scrollTop() !== 0) {
                container.addClass('recommended-bike__shadow-top');
            }
            else {
                container.removeClass('recommended-bike__shadow-top');
            }
           container.find('img.lazy[src=""]').lazyload();
        });

        /* filter element click */
        $('.recommended-bike-popup').on('click', '.filter-item', function () {
            var targetElement = $(this).closest('.filter-list__item');
            if (filterList.find('.filter-list__item').length <= 1) { //if last item is clicked
                close();          
            }
            $('#' + targetElement.attr('data-id')).trigger('click');
            targetElement.remove();
        });
    }

    function open() {
        popup.addClass('recommended-bike-popup--active');
        documentBody.lock();
    }

    function close() {
        popup.removeClass('recommended-bike-popup--active');
        documentBody.unlock();
    }

    $(window).on('popstate', function () {
        if (popup.hasClass('recommended-bike-popup--active')) {
            close();
        }
    });

    return {
        registerEvents: registerEvents,
        open: open,
        close: close
    }

})();




