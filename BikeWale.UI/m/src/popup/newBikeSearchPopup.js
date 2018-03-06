﻿/* Recommended bike popup */

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

        $(document).on('change', '.refine-result__list input', function () {
            var inputType = $(this).attr('type');
            var activeElementList;

            if (inputType === 'checkbox') {
                activeElementList = $(this).closest('.refine-result__list').find('input[type="checkbox"]:checked');
            }
            else if (inputType === 'radio') {
                activeElementList = $(this).closest('.refine-result__list').find('input[type="radio"]:checked');
            }

            if (activeElementList.length) {
                $(this).closest('.refine-result').find('.refine-result__apply').prop('disabled', false);
            }
            else {
                $(this).closest('.refine-result').find('.refine-result__apply').prop('disabled', true);
            }
        });

        closeBtn.on('click', function ()
        {

            vmRecommendedBikes.Filters([]);
            vmRecommendedBikes.FiltersValue([]);
            vmRecommendedBikes.searchFilter = [];
            vmRecommendedBikes.initData();
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

                var activeElementList = filterTypeContainer.find('.refine-result__list input[type="checkbox"]');
                if (!activeElementList.length) {
                    activeElementList = filterTypeContainer.find('.refine-result__list input[type="radio"]');
                }

                $.each(activeElementList, function () {
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
//Knockout Handler to convert a float to a number with particular precision digits.
ko.bindingHandlers.numericText = {
    update: function (element, valueAccessor, allBindingsAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor()),
            precision = ko.utils.unwrapObservable(allBindingsAccessor().precision) || ko.bindingHandlers.numericText.defaultPrecision,
            formattedValue = value.toFixed(precision);

        ko.bindingHandlers.text.update(element, function () { return formattedValue; });
    },
    defaultPrecision: 1
};


var RecommendedBikes = function () {
    var self = this;

    self.bikes = ko.observableArray([]);
    self.noOfBikes = ko.observable();
    self.bikesOtherMakes = ko.observableArray([]);
    self.noOfOtherBikes = ko.observable();

    var pageNo = 0;
    self.isBikesLoading = ko.observable(true);
    self.isOtherBikesLoading = ko.observable(true);

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

    self.searchFilter = { cityId: "", displacement: [], mileage: [], power: [], price: [], bodyStyle: "", makeId: "", abs: "", discBrake: "", drumBrake: "", alloyWheel: "", spokeWheel: "", electric: "", manual: "", excludeMake: "", pageSize: 10, pageNumber: 0 };

    self.budgetSlider.subscribe(function (value) {
        var minBuget = self.budgetSlider()[0];
        var maxBuget = self.budgetSlider()[1];

        var amountPreview = self.getBudgetAmount(self.budgetSlider());
        self.budgetAmountPreview(amountPreview);
        self.Filters()['budget'] = self.FiltersValue()['budget'] = self.budgetStepPoints()[minBuget] + '+' + self.budgetStepPoints()[maxBuget];
        
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
    self.FiltersValue = ko.observable(self.getQueryString());
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
        else if (maxBudget === self.budgetStepPoints().length - 1 || self.budgetStepPoints()[maxBudget] === 0) {
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
        var activeFiltersList = '';
        var selectionPreview = '';

        activeElements.each(function (index) {
            activeElementList += '+' + $(this).attr('data-value');
            activeFiltersList += '+' + $(this).attr('data-valueText');
            if (index) {
                selectionPreview += ', ';
            }
            selectionPreview += $(this).find('.check-box__label').text();
        });

        self.Filters()[filterTypeContainer.attr('data-filter-type')] = activeElementList.substr(1);
        self.FiltersValue()[filterTypeContainer.attr('data-filter-type')] = activeFiltersList.substr(1);
        filterTypeContainer.find('.accordion-head__preview').text(selectionPreview);
    }

    self.SetPageFilters = function () {
        try {

            $('#appliedFilterList').empty();

            for(var key in self.Filters()) {
                switch (key) {
                    case "budget":
                        var arr = self.Filters()[key];

                        self.budgetSlider([
                            $.inArray(parseInt(self.searchFilter.price[0]["min"], 10), self.budgetStepPoints()),
                            $.inArray(parseInt(self.searchFilter.price[0]["max"], 10), self.budgetStepPoints())
                        ]);
                        //if (arr.length > 0) {
                        //    self.budgetSlider([
                        //    $.inArray(parseInt(arr[0], 10), self.budgetStepPoints()), self.budgetStepPoints().length - 1]);
                        //    if (arr.length > 1) self.budgetSlider([$.inArray(parseInt(arr[0], 10), self.budgetStepPoints()), $.inArray(parseInt(arr[1], 10), self.budgetStepPoints())]);
                        //}

                        self.setBudgetSelection();

                        break;

                    default:
                        var filterTypeContainer = $('#filtersPopup li[data-filter-type="' + key + '"]');
                        var arr = self.Filters()[key].split("+");
                        var selectionPreview = '';

                        for(var index in arr) {
                            value = arr[index];
                            if (value !== "" && value) {
                                var element = filterTypeContainer.find('div[data-value="' + value + '"]');
                                element.addClass("check-box--active");
                                self.SetCheckboxSelection(element);

                                
                                selectionPreview += element.find('.check-box__label').text();
                                if (index) {
                                    selectionPreview += ', ';
                                }
                            }
                        }

                        if (selectionPreview.length) {
                            filterTypeContainer.find('.accordion-head__preview').text(selectionPreview);
                        }

                        break;
                }
            }

        } catch (e) {
            console.warn("Unable to set page filters : " + e.message);
        }
    };

    self.UpdateFilters = function (filterTypeContainer) {
        var activeElements = filterTypeContainer.find('input[type="checkbox"]:checked');
        if (!activeElements.length) {
            activeElements = filterTypeContainer.find('input[type="radio"]:checked');
        }

        var activeElementList = '';
        var activeFiltersList = '';

        activeElements.each(function (index) {
            activeElementList += '+' + $(this).val();
            activeFiltersList += '+' + $(this).data("valuetext");

        });

        self.Filters()[filterTypeContainer.attr('data-filter-type')] = activeElementList.substr(1);
        self.FiltersValue()[filterTypeContainer.attr('data-filter-type')] = activeFiltersList.substr(1);
        self.ApplyInPageFilters();
    };

    self.GetFilteredData = function () {
        try {
            if (self.FiltersValue() != null) {
                var displacement = self.FiltersValue().displacement;
                var budget = self.FiltersValue().budget;
                var bodyType = self.FiltersValue().bodyType; // string of all bodytypes
                var mileage = self.FiltersValue().mileage;
                var power = self.FiltersValue().power;
            }


            if (displacement != undefined) {
                self.searchFilter.displacement = (displacement.indexOf('+') > -1) ? new getMinMaxLimitsList(displacement) : new getMinMaxLimits(displacement);
            }

            if (mileage != undefined) {
                self.searchFilter.mileage = (mileage.indexOf('+') > -1) ? new getMinMaxLimitsList(mileage) : new getMinMaxLimits(mileage);
            }
            if (power != undefined) {
                self.searchFilter.power = (power.indexOf('+') > -1) ? new getMinMaxLimitsList(power) : new getMinMaxLimits(power);
            }

            if (budget != undefined) {

                self.searchFilter.price = (budget.indexOf('+') > -1) ? new getMinMaxLimitsList(budget) : new getMinMaxLimits(budget);
            }
            self.searchFilter.bodyStyle = (bodyType != undefined ? bodyType.split('+') : null)

        }
        catch (e) {
            console.warn("GetFilteredData error : " + e.message);
        }

    }

    self.CallAPI = function (searchFilterObj) {
        try {
            return $.ajax({
                type: "POST",
                url: "/api/v2/bikesearch/",
                contentType: "application/json",
                data: ko.toJSON(searchFilterObj),
                success: function (response) {
                    searchFilterObj.excludeMake ? self.isOtherBikesLoading(false) : self.isBikesLoading(false);
                    if (response.length > 0) {
                        response = JSON.parse(response);
                        if (("Bikes" in response) && response.Bikes != null && response.Bikes.length > 0) {
                            var bikeList = response.Bikes;
                            var bikeCount = bikeList.length;
                            if (searchFilterObj.excludeMake) {
                                self.bikesOtherMakes(bikeList);
                                self.noOfOtherBikes(bikeCount);
                            } else {
                                self.bikes(bikeList);
                                self.noOfBikes(bikeCount);
                            }
                        }
                        else {
                            //Bike List is Not Present
                        }
                    }
                    else {
                        //Response is invalid
                    }
                },
                error: function (request, status, error) {
                    searchFilterObj.excludeMake ? self.isOtherBikesLoading(false) : self.isBikesLoading(false);

                },
                complete: function (xhr, ajaxOptions, thrownError) {
                    
                }
            });
        }
        catch (e) {
            console.warn("CallAPI error : " + e.message);
        }
    }

    self.MakeRecommmendations = function () {
        try {
            filterList = jQuery.extend({}, self.searchFilter);
            filterList.excludeMake = false;
            return self.CallAPI(filterList);
        }
        catch (e) {
            console.warn("MakeRecommendations error : " + e.message);
        }
    }

    self.OtherMakeRecommendations = function () {

        try {
            //filterList contains fields such as excludeMake, pageCount, pageSize (since OtherMakeRecommendations uses Paging and stuff)
            filterList = jQuery.extend({}, self.searchFilter);
            filterList.excludeMake = true;
            filterList.pageNumber = pageNo;
            return self.CallAPI(filterList);
        }
        catch (e) {
            console.warn("OtherMakeRecommendations error : " + e.message);
        }
    }

    self.SequenceAPI = function () {
        self.MakeRecommmendations().then(self.OtherMakeRecommendations());
    }


    self.ApplyFilters = function () {
        self.SetPageFilters();
        self.GetFilteredData();
        self.initData();
        self.SequenceAPI();
        BikeFiltersPopup.close();
        window.history.back();
    };

    self.ApplyInPageFilters = function () {
        self.GetFilteredData();
        self.SetPageFilters();
        self.SequenceAPI();
        
    }

    self.setBudgetSelection = function () {
        var amountPreview = self.getBudgetAmount(self.budgetSlider());
        $('#appliedFilterList').append('<li data-id="budget" class="filter-list__item"><span class="filter-item">' + amountPreview + '</span></li>');
    };

    /* Function to Initialize all the data variables */
    self.initData = function () {
        self.isBikesLoading(true);
        self.isOtherBikesLoading(true);
        self.noOfBikes(0);
        self.noOfOtherBikes(0);
        self.bikes([]);
        self.bikesOtherMakes([]);
    };

    self.SetCheckboxSelection = function (targetElement) {
        $('#appliedFilterList').append('<li data-id="' + targetElement.attr("id") + '" class="filter-list__item"><span class="filter-item">' + targetElement.find('.check-box__label').text() + '</span></li>');
    };

    /* priceFormatter */
    self.formatPrice = function (price) {
        var thMatch = /(\d+)(\d{3})$/;
        var thRest = thMatch.exec(price);
        if (!thRest) return price;
        return (thRest[1].replace(/\B(?=(\d{2})+(?!\d))/g, ",") + "," + thRest[2]);
    };

    /* Double to Int */
    self.doubleToInt = function (decimalNo) {
        return parseInt(decimalNo);
    };
    
    /* Next block of other make bikes */
    self.bindNextOtherBikesList = function () {

        filterList = jQuery.extend({}, self.searchFilter);
        filterList.excludeMake = true;
        filterList.pageNumber = ++pageNo;

        $.ajax({
            type: "POST",
            url: "/api/v2/bikesearch/",
            contentType: "application/json",
            data: ko.toJSON(filterList),
            success: function (response) {
                if (response.length > 0) {
                    response = JSON.parse(response);
                    if (("Bikes" in response) && response.Bikes != null && response.Bikes.length > 0) {

                        self.bikesOtherMakes.push(response.Bikes);
                        self.noOfOtherBikes(self.noOfOtherBikes + bikeList.length);
                        
                    }
                    else {
                        //Bike List is Not Present
                    }
                }
                else {
                    //Response is invalid
                }
            },
            error: function (request, status, error) {

            },
            complete: function (xhr, ajaxOptions, thrownError) {

            }
        });
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

function getMinMaxLimitsList(range) {
    var filterArray = [];
    if (range != undefined) {
        var selectedRangeList = range.split('+');
    }

    if (selectedRangeList != null) {
        $.each(selectedRangeList, function (i, val) {
            var filterPair = val.split('-');
            maxMinLimits = {
                "min": filterPair[0],
                "max": filterPair[1]

            }
            filterArray.push(maxMinLimits);
        });

    }

    return filterArray;
}


function getMinMaxLimits(range) {
    var filterArray = [];
    if (range != undefined) {
        var selectedRangeList = range.split('-');
    }
    if (selectedRangeList != null) {
        maxMinLimits = {
            "min": selectedRangeList[0],
            "max": selectedRangeList[1]
        }
        filterArray.push(maxMinLimits);
    }

    return filterArray;
}











