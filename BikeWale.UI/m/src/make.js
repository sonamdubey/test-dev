docReady(function () {
	$('.model-card__pros-cons').on('click', '.pros-cons__more-btn', function (event) {
		$(this).hide();
		$(this).closest('.pros-cons__content').find('li').show();
	});

	// popular bikes carousel
	if (navigator.userAgent.match(/Firefox/gi)) {
		$('.carousel__popular-bikes').addClass('popular-bikes--fallback');
	}

	$('.carousel__popular-bikes').on('click', '.view-pros-cons__target', function () {
		var modelCard = $(this).closest('.model__card');

		$(this).hide();

		if (!modelCard.hasClass('card--active')) {
			modelCard.addClass('card--active');
		}
		else {
			modelCard.removeClass('card--active');
		}
	});

	$('.carousel__popular-bikes').on('webkitTransitionEnd transitionend', '.model-card__detail', function () {
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

	$('.upcoming-card__notify-btn').on('click', function () {
		notifyPopup.open();
	});

	formField.registerEvents();

	//interesting fact popup
	interestingFactPopup.registerEvents();

	//floating navbar
	floatingNav.registerEvents();

	//recommended bike popup
	recommendedBikePopup.registerEvents();

	// filters popup
	BikeFiltersPopup.registerEvents();

	Accordion.registerEvents();
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
						}, 10);

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
var notifyPopup = (function () {
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

		formSubmitBtn.on('click', function () {
			var isValid = validateForm.emailField(emailField);

			if (isValid) {
				formField.setSuccessState($(this), 'Thank You!');
				setTimeout(function () {
					$('#notifyCloseBtn').trigger('click');
				}, 1000);
			}
		});

		emailField.on('focus', function () {
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
var formField = (function () {
	function registerEvents() {
		$(document).on('focus', '.form-field__input', function () {
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
var validateForm = (function () {

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

/* Recommended bike popup */
var recommendedBikePopup = (function () {
	var applyBtn, popup, appliedFilterList, container, closeBtn;

	function _setSelectores() {
		applyBtn = $('#refineResultApply');
		popup = $('#recommendedBikePopup');
		closeBtn = $('#recommendedBikeCloseBtn');
		container = $('.recommended-bike-container');
		appliedFilterList = $('#appliedFilterList');
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

			// for other recommended bike overlay
			recommendedBikeOverlay();

			// get selected filters from inpage filter and update filters object
			var filterTypeContainer = $(this).closest('.filter-type__item');
			vmRecommendedBikes.UpdateFilters(filterTypeContainer);

			//set filter list
			//vmRecommendedBikes.SetDefaultPageFilters();
			vmRecommendedBikes.SetPageFilters();
		});

		$(document).on('change', 'input[name="inpageMileageFilter"]', function () {
			var checkedBoxList = $('input[name="inpageMileageFilter"]:checked');

			if (checkedBoxList.length) {
				applyBtn.prop('disabled', false);
			}
			else {
				applyBtn.prop('disabled', true);
			}
		});

		closeBtn.on('click', function () {
			close();
			updateInpageFilters();
			window.history.back();
		});

		container.on('scroll', function () {
			/* other recommended overlay */
			recommendedBikeOverlay();

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
			if (appliedFilterList.find('.filter-list__item').length === 1) { //if last item is clicked
				close();
			}
			$('#' + targetElement.attr('data-id')).trigger('click');
			targetElement.remove();
		});

		$('.filter__edit').on('click', function () {
			BikeFiltersPopup.open();
		});

		$('.other-recommended-bike').addClass('overlay--inactive');
	}

	function recommendedBikeOverlay() {
		var containerHeading = ($('.recommended-bike__found-result').outerHeight() + $('.recommended-bike__filter-section').outerHeight()),
			otherBikeContainer = $('.other-recommended-bike'),
			recommendedBikeElement = otherBikeContainer.find('.recommended-bike__list-card:first-child');
		if (!popup.find('.not-found-container').length) {
			if (recommendedBikeElement.length && (recommendedBikeElement.offset().top + (recommendedBikeElement.outerHeight() + containerHeading)) < container.scrollTop() + $(window).height() - containerHeading) {
				otherBikeContainer.removeClass('overlay--inactive');
			}
			else if (recommendedBikeElement.length) {
				otherBikeContainer.addClass('overlay--inactive');
			}
		}
	}

	function open() {
		popup.addClass('recommended-bike-popup--active');
		container.scrollTop(0);
		documentBody.lock();
	}

	function close() {
		popup.removeClass('recommended-bike-popup--active');
		documentBody.unlock();
	}

	function updateInpageFilters() {
		$.each(vmRecommendedBikes.Filters(), function (key, value) {
			var filterTypeContainer = $('.all-model__list li[data-filter-type="' + key + '"]');
			if (filterTypeContainer.length) {
				var arr = vmRecommendedBikes.Filters()[key].split("+");

				var checkboxList = filterTypeContainer.find('.refine-result__list input[type="checkbox"]')

				$.each(checkboxList, function () {
					if ($.inArray($(this).val(), arr) < 0) {
						if ($(this).is(':checked')) {
							$(this).trigger('click');
						}
					}
				});
			}
		});
	}

	$(window).on('popstate', function () {
		if (popup.hasClass('recommended-bike-popup--active') && history.state !== "recommendedBikePopup") {
			close();
		}
	});

	return {
		registerEvents: registerEvents,
		open: open,
		close: close
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
				$('html, body').animate({ scrollTop: (windowScrollTop - (bodyShowableArea - shownArea)) }, 100);
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

var documentBody = (function () {
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

var BikeFiltersPopup = (function () {
	var container, backgroundWindow, closeBtn;

	function _setSelectors() {
		container = $('#filtersPopup');
		backgroundWindow = $('#filtersBlackoutWindow');
		closeBtn = $('#filterClose');
	}

	function registerEvents() {
		_setSelectors();
		_setBodyDimension();

		$(backgroundWindow).on('click', function () {
			if (container.hasClass('filters-screen--active')) {
				window.history.back();
			}
		});

		$('#filterClose').on('click', function () {
			backgroundWindow.trigger('click');
		});

		$(window).on('popstate', function () {
			if (container.hasClass('filters-screen--active') && history.state === "recommendedBikePopup") {
				close();
			}
		});
	}

	function _setBodyDimension() {
		var bodyHeight = container.find('.filters__screen').height() - container.find('.filters-screen__head').height();

		container.find('.filters-screen__body').css('height', bodyHeight);
	}

	function open() {
		container.addClass('filters-screen--active');
		history.pushState('filtersPopup', '', '');
	}

	function close() {
		container.removeClass('filters-screen--active');
	}

	return {
		registerEvents: registerEvents,
		open: open,
		close: close
	}

})();

var Accordion = (function () {
	function registerEvents() {
		$('.accordion__list').on('click', '.accordion__head', function () {
			handleClick($(this))
		});
	}

	function handleClick(accordionHead) {
		var accordionList = accordionHead.closest('.accordion__list');

		if (accordionList.attr('data-state') === 'one') {
			var accordionItem = accordionHead.closest('.accordion-list__item');
			var accordionSiblingItems = accordionItem.siblings('.accordion-list__item');

			accordionSiblingItems.find('.accordion-item--active').removeClass('accordion-item--active');
			accordionSiblingItems.find('.accordion__body').css('height', 0);

			var accordionBody = accordionHead.siblings('.accordion__body');
			var accordionContentHeight = accordionBody.find('.accordion-body__content').outerHeight(true);

			if (!accordionHead.hasClass('accordion-item--active')) {
				accordionHead.addClass('accordion-item--active');
				accordionBody.css('height', accordionContentHeight);
			}
			else {
				accordionHead.removeClass('accordion-item--active');
				accordionBody.css('height', 0);
			}
		}

	}

	return {
		registerEvents: registerEvents
	}
})();

ko.bindingHandlers.KOSlider = {
	init: function (element, valueAccessor, allBindingsAccessor) {
		var options = allBindingsAccessor().sliderOptions || {};
		var observable = valueAccessor();

		options.slide = function (e, ui) {
			if (ui.values && ui.values.length > 0) {
				if (ui.values[0] != ui.values[1])
					observable(ui.values);
			}
			else observable(ui.value);
		};

		ko.utils.registerEventHandler(element, "slide", function (event, ui) {
			if (ui.values && ui.values.length > 0 && ui.values[0] == ui.values[1]) {
				return false;
			}
		});

		ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
			$(element).slider("destroy");
		});

		ko.utils.registerEventHandler(element, "slidestop", function (event, ui) {
		});

		$(element).slider(options);
	},
	update: function (element, valueAccessor) {
		var value = ko.utils.unwrapObservable(valueAccessor());
		if (value) {
			$(element).slider(value.length ? "values" : "value", value);
			$(element).change();

		}

	}
};

var RecommendedBikes = function () {
	var self = this;

	var budgetArray = [
		{
			step: 0.3,
			start: 0,
			end: 30000
		},
		{
			step: 0.1,
			start: 30000,
			end: 100000,
		},
		{
			step: 0.5,
			start: 100000,
			end: 350000,
		},
		{
			step: 1.5,
			start: 350000,
			end: 500000,
		},
		{
			step: 2.5,
			start: 500000,
			end: 1500000,
		},
		{
			step: 15,
			start: 1500000,
			end: 3000000,
		},
		{
			step: 30,
			start: 3000000,
			end: 6000000,
		}
	];

	self.budgetAmountPreview = ko.observable('');
	self.budgetSlider = ko.observable();
	self.budgetStepPoints = ko.observable();

	self.budgetSlider.subscribe(function (value) {
		var minBuget = self.budgetSlider()[0];
		var maxBuget = self.budgetSlider()[1];

		var amountPreview = self.getBudgetAmount(self.budgetSlider());
		self.budgetAmountPreview(amountPreview);
		self.Filters()['budget'] = self.budgetStepPoints()[minBuget] + '+' + self.budgetStepPoints()[maxBuget];
	});

	self.getQueryString = function () {
		var query = {};
		try {
			//var requestUrl = window.location.hash.substr(1);
			//var requestUrl = 'budget=30000+100000&bodyType=1+2&mileage=2&suspension=1+2&power=1';
			var requestUrl = '';
			if (requestUrl && requestUrl != '') {
				var kvPairs = requestUrl.split('&');
				$.each(kvPairs, function (i, val) {
					var kvPair = val.split('=');
					query[kvPair[0]] = kvPair[1];
				});
			}
		} catch (e) {
			console.warn("Unable to get query string : " + e.message);
		}
		return query;
	};

	self.Filters = ko.observable(self.getQueryString());

	// generate budget step points
	self.getBudgetStepPoints = function () {
		var stepPoints = [];

		for (var i = 0; i < budgetArray.length; i++) {
			var budget = budgetArray[i];

			for (var j = budget.start; j <= budget.end; j = j + budget.step * 100000) {
				if (!(stepPoints.indexOf(j) > -1)) {
					stepPoints.push(j);
				}
			}
		}

		self.budgetStepPoints(stepPoints);
	}

	self.getBudgetStepPoints();

	// set budget amount
	self.getBudgetAmount = function (values) {
		var amount = '';
		var minBudget = values[0];
		var maxBudget = values[1];

		if (minBudget == 0 && maxBudget == self.budgetStepPoints().length - 1) {
			amount = 'All Range'
		}
		else if (maxBudget == self.budgetStepPoints().length - 1) {
			amount = 'Above ' + convertAmount(self.budgetStepPoints()[minBudget], true);
		}
		else if (minBudget == 0) {
			amount = 'Below ' + convertAmount(self.budgetStepPoints()[maxBudget], true);
		}
		else {
			amount = convertAmount(self.budgetStepPoints()[minBudget], true);
			amount += ' - ';
			amount += convertAmount(self.budgetStepPoints()[maxBudget], true);
		}

		return amount;
	}

	/* 
	 * filter: checkbox selection
	 * bodytype, mileage, suspension, power
	 */
	self.checkboxSelection = function (data, event) {
		var targetElement = $(event.currentTarget);
		var filterTypeContainer = $(targetElement).closest('.filter-type__item');

		if (!targetElement.hasClass('check-box--active')) {
			targetElement.addClass('check-box--active');
		}
		else {
			targetElement.removeClass('check-box--active');
		}

		var activeElements = filterTypeContainer.find('.check-box--active');
		var activeElementlist = '';
		var selectionPreview = '';

		activeElements.each(function (index) {
			activeElementlist += '+' + $(this).attr('data-value');
			if (index) {
				selectionPreview += ', ';
			}
			selectionPreview += $(this).find('.check-box__label').text();
		});

		self.Filters()[filterTypeContainer.attr('data-filter-type')] = activeElementlist.substr(1);
		filterTypeContainer.find('.accordion-head__preview').text(selectionPreview);
	}

	self.SetPageFilters = function () {
		try {

			$('#appliedFilterList').empty();

			$.each(self.Filters(), function (key, value) {
				switch (key) {
					case "budget":
						var arr = self.Filters()[key].split("+");

						if (arr.length > 0) {
							self.budgetSlider([$.inArray(parseInt(arr[0], 10), self.budgetStepPoints()), self.budgetStepPoints().length - 1]);
							if (arr.length > 1) self.budgetSlider([$.inArray(parseInt(arr[0], 10), self.budgetStepPoints()), $.inArray(parseInt(arr[1], 10), self.budgetStepPoints())]);
						}

						self.setBudgetSelection();

						break;

					default:
						var filterTypeContainer = $('#filtersPopup li[data-filter-type="' + key + '"]');
						var arr = self.Filters()[key].split("+");
						var selectionPreview = '';

						$.each(arr, function (index, value) {
							if (value !== "" && value) {
								var element = filterTypeContainer.find('div[data-value="' + value + '"]');
								element.addClass("check-box--active");
								self.SetCheckboxSelection(element);

								if (index) {
									selectionPreview += ', ';
								}
								selectionPreview += element.find('.check-box__label').text();
							}
						});

						if (selectionPreview.length) {
							filterTypeContainer.find('.accordion-head__preview').text(selectionPreview);
						}

						break;
				}
			});

		} catch (e) {
			console.warn("Unable to set page filters : " + e.message);
		}
	};

	self.UpdateFilters = function (filterTypeContainer) {
		var activeElements = filterTypeContainer.find('input[type="checkbox"]:checked');
		var activeElementlist = '';

		activeElements.each(function (index) {
			activeElementlist += '+' + $(this).val();
		});

		self.Filters()[filterTypeContainer.attr('data-filter-type')] = activeElementlist.substr(1);
	};

	self.ApplyFilters = function () {
		self.SetPageFilters();
		BikeFiltersPopup.close();
		window.history.back();
	};

	self.setBudgetSelection = function () {
		var amountPreview = self.getBudgetAmount(self.budgetSlider());
		$('#appliedFilterList').append('<li data-id="budget" class="filter-list__item"><span class="filter-item">' + amountPreview + '</span></li>');
	};

	self.SetCheckboxSelection = function (targetElement) {
		$('#appliedFilterList').append('<li data-id="' + targetElement.attr("id") + '" class="filter-list__item"><span class="filter-item">' + targetElement.find('.check-box__label').text() + '</span></li>');
	};
};

var vmRecommendedBikes = new RecommendedBikes();
ko.applyBindings(vmRecommendedBikes, document.getElementById('bikeMakeContainer'));

function convertAmount(amount, rupeeIcon) {
	if (amount > 99999) {
		amount = parseFloat((amount / 100000).toFixed(2)) + ' L'
	}
	else {
		amount = parseFloat((amount / 1000).toFixed(2)) + ' K'
	}

	if (rupeeIcon) {
		return '\u20b9 ' + amount;
	}

	return amount;
}
