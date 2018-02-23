/* Recommended bike popup */

var vmRecommendedBikes;
var recommendedBikePopup = (function () {
	var applyBtn, popup, appliedFilterList, container, closeBtn;

	function _setSelectores() {
	    applyBtn = $('#refineResultApply');
	    popup = $('#recommendedBikePopup');
	    closeBtn = $('#recommendedBikeCloseBtn');
	    container = $('.recommended-bike-container');
	    appliedFilterList = $('#appliedFilterList');
	};

	function registerEvents() {
	    _setSelectores();
	    initViewModel();
	    $(document).on('click', '.refine-result__apply', function (e) {
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

	    $(document).on('change', '.refine-result__list input[type="checkbox"]', function () {
	        var checkedBoxList = $(this).closest('.refine-result__list').find('input[type="checkbox"]:checked');

	        if (checkedBoxList.length) {
	            $(this).closest('.refine-result').find('.refine-result__apply').prop('disabled', false);
	        }
	        else {
	            $(this).closest('.refine-result').find('.refine-result__apply').prop('disabled', true);
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

	        $('#' + targetElement.attr('data-id')).trigger('click');
	        if (appliedFilterList.find('.filter-list__item').length === 1) { //if last item is clicked
	            closeBtn.trigger('click');
	        }
	        targetElement.remove();
	    });

	    $('.filter__edit').on('click', function () {
	        BikeFiltersPopup.open();
	    });

	    $('.other-recommended-bike').addClass('overlay--inactive');
	};

	function recommendedBikeOverlay() {
	    var otherBikeContainer = $('.other-recommended-bike'),
			recommendedBikeElement = otherBikeContainer.find('.recommended-bike__list-card:first-child');

	    if (recommendedBikeElement.length) {
	        if ((recommendedBikeElement.offset().top + recommendedBikeElement.outerHeight() + container.scrollTop()) < container.scrollTop() + $(window).height()) {
	            otherBikeContainer.removeClass('overlay--inactive');
	        }
	        else {
	            otherBikeContainer.addClass('overlay--inactive');
	        }
	    }
	};

	function open() {
	    popup.addClass('recommended-bike-popup--active');
	    container.scrollTop(0);
	    documentBody.lock();
	};

	function close() {
	    popup.removeClass('recommended-bike-popup--active');
	    documentBody.unlock();
	};

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
	};

	function initViewModel() {
	    vmRecommendedBikes = new RecommendedBikes()
	    ko.applyBindings(vmRecommendedBikes, document.getElementById('dvNewBikeSearchPopup'));
	};

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

        if (minBudget === 0 && maxBudget === self.budgetStepPoints().length - 1) {
            amount = 'All Range'
        }
        else if (maxBudget === self.budgetStepPoints().length - 1) {
            amount = 'Above ' + convertAmount(self.budgetStepPoints()[minBudget], true);
        }
        else if (minBudget === 0) {
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
        var activeElementList = '';
        var selectionPreview = '';

        activeElements.each(function (index) {
            activeElementList += '+' + $(this).attr('data-value');
            if (index) {
                selectionPreview += ', ';
            }
            selectionPreview += $(this).find('.check-box__label').text();
        });

        self.Filters()[filterTypeContainer.attr('data-filter-type')] = activeElementList.substr(1);
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
        var activeElementList = '';

        activeElements.each(function (index) {
            activeElementList += '+' + $(this).val();
        });

        self.Filters()[filterTypeContainer.attr('data-filter-type')] = activeElementList.substr(1);
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

