//*****************************************************************************************************

/* budget slider */
var budgetValue = [0, 10000, 20000, 35000, 50000, 80000, 125000, 200000],
    budgetKey = [0, 1, 2, 3, 4, 5, 6, 7];

//parse query string
var getQueryString = function () {
    var qsColl = new Object();
    var requestUrl = window.location.hash.substr(1); 
    if (requestUrl && requestUrl != '') {
        var kvPairs = requestUrl.split('&');
        $.each(kvPairs, function (i, val) {
            var kvPair = val.split('=');
            qsColl[kvPair[0]] = kvPair[1];
        });
    }
    return qsColl;
}


ko.bindingHandlers.CurrencyText = {
    update: function (element, valueAccessor) {
        var amount = valueAccessor();
        var formattedAmount = ko.unwrap(amount) !== null ? formatPrice(amount) : 0;
        $(element).text(formattedAmount);
    }
};

ko.bindingHandlers.NumberOrdinal = {
    update: function (element, valueAccessor) {
        var num = valueAccessor();
        var num = ko.unwrap(num) != null ? num : "";
        num = parseInt(num, 10);
        switch (num % 100) {
            case 11:
            case 12:
            case 13:
                suf = "th"; break;
        }

        switch (num % 10) {
            case 1:
                suf = "st"; break;
            case 2:
                suf = "nd"; break;
            case 3:
                suf = "rd"; break;
            default:
                suf = "th"; break;
        }

        $(element).text(num+suf);
    }
};

ko.bindingHandlers.KOSlider = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var options = allBindingsAccessor().sliderOptions || {};
        var observable = valueAccessor();

        options.slide = function (e, ui) {
            observable(ui.values ? ui.values : ui.value);
        };

        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $(element).slider("destroy");
        });

        $(element).slider(options);
    },
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        if (value)
        {
            $(element).slider(value.length ? "values" : "value", value);
            $(element).change();
        }          

    }
};

function formatPrice(price) {
    price = price.toString();
    var lastThree = price.substring(price.length - 3);
    var otherNumbers = price.substring(0, price.length - 3);
    if (otherNumbers != '')
        lastThree = ',' + lastThree;
    var price = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
    return price;
}

var usedBikes = function()
{
    var self = this;
    self.Filters = ko.observable(getQueryString());
    self.QueryString = ko.computed(function () {
        var qs = "";
        $.each(self.Filters(), function (i, val) {
            if (val != null && val != "")
                qs += "&" + i + "=" + val;
        });
        qs = qs.substr(1);
        window.location.hash = qs;
        return qs;
    });     
    self.OnInit = ko.observable(true);
    self.PageHeading = ko.observable();
    self.TotalBikes = ko.observable();
    self.BikeDetails = ko.observableArray();
    self.PageUrl = ko.observable();
    self.CurPageNo = ko.observable();
    self.BikePhotos = function () {
        var self = this;
        self.hostUrl = ko.observable();
        self.OriginalImgPath = ko.observable();
        self.imgPath = ko.observable();
    };
    self.PrevPageUrl = ko.observable();
    self.NextPageUrl = ko.observable();
    self.Pagination = function () {

    };
    self.SelectedCity = ko.observable({ "id": 0, "name": "All India" });
    self.BudgetValues = ko.observable([0, 7]);
    self.ShowBudgetRange = ko.computed(function (d, e) {

        if(self.BudgetValues())
        {
            var minBuget = self.BudgetValues()[0] ,maxBuget =self.BudgetValues()[1]; 
            if (minBuget == 0 && maxBuget == 7) {
                $("#budget-amount").html('<span class="bwmsprite inr-xxsm-icon"></span>0 - <span class="bwmsprite inr-xxsm-icon"></span>' + formatPrice(budgetValue[maxBuget]));
            }
            else {
                $("#budget-amount").html('<span class="bwmsprite inr-xxsm-icon"></span>' + formatPrice(budgetValue[minBuget]) + ' - <span class="bwmsprite inr-xxsm-icon"></span>' + formatPrice(budgetValue[maxBuget]) + ((maxBuget == 7)?'+':''));
            }
        }
    });
    self.KmsDriven = ko.observable(10000);
    self.BikeAge = ko.observable(2);

    self.ApplyFilters = function () {
        self.ResetFilters();
        if (self.SelectedCity() && self.SelectedCity().id > 0) self.Filters()["cityid"] = self.SelectedCity().id;
        if (self.KmsDriven() > 10000) self.Filters()["kms"] = self.KmsDriven();
        if (self.BikeAge() > 0) self.Filters()["age"] = self.BikeAge();
        if (self.BudgetValues())        {
            var minBuget = self.BudgetValues()[0], maxBuget = self.BudgetValues()[1];
            self.Filters()["budget"] = budgetValue[minBuget];
            if (maxBuget != 7) self.Filters()["budget"] += "+" + budgetValue[maxBuget];
        }

        self.GetUsedBikes();

    };

    self.ResetFilters = function () {
        self.Filters()["cityid"] = "";
        self.Filters()["kms"] = "";
        self.Filters()["age"] = "";
        self.Filters()["budget"] = "";
    };

    self.objSorts = ko.observableArray([{ id: 1, text: "Most recent" }, { id: 2, text: "Price - Low to High" }, { id: 3, text: "Price - High to Low" }, { id: 4, text: "Kms - Low to High" }, { id: 5, text: "Kms - High to Low" }]);
    
    self.applySort = function (d, e) {
        var so = $("#sort-by-list li.active").attr("data-sortorder");
        self.Filters()["so"] = so;
        self.GetUsedBikes();
    };

    self.FilterCity = function (d, e) {
        var ele = $(e.target);
        if (!ele.hasClass("active")) {
            ele.addClass("active").siblings().removeClass("active");
            self.SelectedCity({ "id": ele.attr("data-cityid"), "name": ele.text() });
        };
    };

    self.ManageFilters = ko.computed(function () {

    });
    

    self.setFilters = function () {

    };

    self.SelectSeller = function () {

    };

    self.GetUsedBikes = function () {
        self.Filters.notifySubscribers();
        var qs = self.QueryString();
        $.ajax({
            type: 'GET',
            url: '/api/used/search/?' + qs,
            dataType: 'json',
            success: function (response) {
                window.location.hash = qs;
                self.OnInit(false);
                self.TotalBikes(response.totalCount);
                self.CurPageNo(response.currentPageNo);
                self.PageUrl(response.pageUrl);
                self.BikeDetails(ko.toJS(response.result));
            },
            complete: function (xhr) {
                if (xhr != 200) {

                }
            }
        });
    };
}



var vwUsedBikes = new usedBikes();
ko.applyBindings(vwUsedBikes, document.getElementById("usedBikesSection"));

var objFilters = vwUsedBikes.Filters();

$(function () {
    
    setSortFilter();
    //GetUsedBikes();
});

function setSortFilter()
{
    if(objFilters && objFilters["so"])
    {
        $("#sort-by-list li[data-sortorder=" + objFilters["so"] + "]").addClass("active").siblings().removeClass("active");
    }

}




//*****************************************************************************************************

$(document).ready(function () {

    var sortFilter = $('#sort-filter-wrapper'),
        bodyHeight = $('body').height(),
        footerHeight = $('footer').height();

    $(window).scroll(function () {
        var scrollPosition = $(window).scrollTop();

        if (scrollPosition + $(window).height() > (bodyHeight - footerHeight)) {
            sortFilter.hide();
        }
        else {
            sortFilter.show();
        }
    });

    //set filters
    filters.set.all();
});

var filterContainer = $("#filter-container"),
    effect = 'slide',
    options = { direction: 'right' },
    duration = 500;

$('#filter-floating-btn').on('click', function () {
    filters.open();
    appendState('filter');
});

$('#close-filter').on('click', function () {
    history.back();
    filters.close();
});




$('#budget-range-slider').slider({
    orientation: 'horizontal',
    range: true,
    min: 0,
    max: 7,
    step: 1,
    values: [0, 7],
    slide: function (event, ui) {
        var left = event.keyCode != $.ui.keyCode.RIGHT,
            right = event.keyCode != $.ui.keyCode.LEFT,
            value = findNearest(left, right, ui.value);

        if (ui.values[0] == ui.values[1]) {
            return false;
        }

        filters.budgetAmount(ui.values);        
    }
});

function findNearest(left, right, value) {
    var nearest = null;
    var diff = null;
    for (var i = 0; i < budgetKey.length; i++) {
        if ((left && budgetKey[i] <= value) || (right && budgetKey[i] >= value)) {
            var newDiff = Math.abs(value - budgetKey[i]);
            if (diff == null || newDiff < diff) {
                nearest = budgetKey[i];
                diff = newDiff;
            }
        }
    }
    return nearest;
}

function getRealValue(sliderValue) {
    for (var i = 0; i < budgetKey.length; i++) {
        if (budgetKey[i] >= sliderValue) {
            return budgetValue[i];
        }
    }
    return 0;
}



$('#reset-filters').on('click', function () {
    filters.reset.all();
    accordion.resetAll();
});

$('#apply-filters').on('click', function () {
    filters.close();
});


var filterTypeBike = $('#filter-type-bike');
/* city filter */
var cityFilter = $('#filter-city-container');

/* set slider default values */
var filters = {

    open: function () {
        filterContainer.show(effect, options, duration, function () {
            $('html, body').addClass('lock-browser-scroll');
            filterContainer.addClass('fixed');
        });
    },

    close: function () {
        filterContainer.removeClass('fixed');
        filterContainer.hide(effect, options, duration, function () { });
        $('html, body').removeClass('lock-browser-scroll');
    },

    budgetAmount: function (units) {
        var budgetminValue = getRealValue(units[0]),
            budgetmaxValue = getRealValue(units[1]);

        if (units[0] == 0 && units[1] == 7) {
            $("#budget-amount").html('<span class="bwmsprite inr-xxsm-icon"></span>0 - <span class="bwmsprite inr-xxsm-icon"></span>' + budgetmaxValue);
        }
        else {
            $("#budget-amount").html('<span class="bwmsprite inr-xxsm-icon"></span>' + budgetminValue + ' - <span class="bwmsprite inr-xxsm-icon"></span>' + budgetmaxValue);
        }
    },

    bike: {

        open: function () {
            bikeFilter.show(effect, options, duration, function () {
                filterContainer.addClass('bikes-footer');
            });
        },

        close: function () {
            bikeFilter.hide(effect, options, duration, function () { });
            filterContainer.removeClass('bikes-footer');
        },

        setSelection: function () {
            var selection = accordion.selectedItems();
            if (!selection.length == 0) {
                filterTypeBike.find('.filter-option-key').show();
                filterTypeBike.find('.selected-filters').text(selection);
            }
            else {
                filters.reset.bike();
            }
        }
    },

    city: {

        open: function () {
            cityFilter.show(effect, options, duration, function () {
                cityFilter.addClass('city-header');
            });
        },

        close: function () {
            cityFilter.hide(effect, options, duration, function () { });
            cityFilter.removeClass('city-header');
        }

    },

    set: {

        all: function () {
            filters.set.bike();
            filters.set.previousOwners();
            filters.set.sellerType();
        },

        bike: function () {
            filterTypeBike.find('.selected-filters').text('All Bikes');
        },

        previousOwners: function () {
            $('#previous-owners-list li.active').removeClass('active');
        },

        sellerType: function () {
            $('.filter-type-seller.checked').removeClass('checked');
        }

    },

    reset: {

        all: function () {
            filters.reset.bike();
            filters.reset.previousOwners();
            filters.reset.sellerType();
        },

        bike: function () {
            filterTypeBike.find('.selected-filters').text('All Bikes');
        },

        previousOwners: function () {
            $('#previous-owners-list li.active').removeClass('active');
        },

        sellerType: function () {
            $('.filter-type-seller.checked').removeClass('checked');
        }
    }
};

$('#previous-owners-list').on('click', 'li', function () {
    var item = $(this);

    if (!item.hasClass('active')) {
        $(this).addClass('active');
    }
    else {
        $(this).removeClass('active');
    }
});

$('.filter-type-seller').on('click', function () {
    var item = $(this);

    if (!item.hasClass('checked')) {
        $(this).addClass('checked');
    }
    else {
        $(this).removeClass('checked');
    }
});



$('#filter-type-city').on('click', '.filter-option-value', function () {
    filters.city.open();
    appendState('filterCity');
});

$('#close-city-filter').on('click', function () {
    filters.city.close();
});

$('#filter-city-list').on('click', 'li', function () {    
    filters.city.close();

});


/* bikes filter */
var bikeFilter = $('#filter-bike-container');

$('#filter-type-bike').on('click', '.filter-option-value', function () {
    filters.bike.open();
    appendState('filterBikes');
});

$('#close-bike-filter, #set-bikes-filter').on('click', function () {
    filters.bike.setSelection();
    filters.bike.close();
});

$('#reset-bikes-filter').on('click', function () {
    accordion.resetAll();
});

var bikeFilterList =  $('#filter-bike-list');

bikeFilterList.on('click', '.accordion-label-tab', function () {
    var tab = $(this).closest('.accordion-tab');
    if (!tab.hasClass('active')) {
        accordion.open(tab);
    }
    else {
        accordion.close(tab);
    }
});

bikeFilterList.on('click', '.accordion-tab .accordion-checkbox', function () {
    var tab = $(this).closest('.accordion-tab');

    if (!tab.hasClass('tab-checked')) {
        tab.addClass('tab-checked');
        accordion.setTab(tab);
    }
    else {
        tab.removeClass('tab-checked');
        accordion.resetTab(tab);
    }
});

bikeFilterList.on('click', '.bike-model-list li', function () {
    var item = $(this);

    if (!item.hasClass('active')) {
        item.addClass('active');
        accordion.setCount(item);
    }
    else {
        item.removeClass('active');
        accordion.setCount(item);
    }
});

/* accordion */
var accordion = {

    tabs: $('#filter-bike-list .accordion-tab'),

    open: function (item) {
        accordion.tabs.removeClass('active');
        accordion.tabs.siblings('ul').slideUp();
        item.addClass('active');
        item.siblings('ul').slideDown();
    },

    close: function (item) {
        item.removeClass('active');
        item.siblings('ul').slideUp();
    },

    setCount: function (item) {
        var modelList = item.closest('.bike-model-list'),
            modelsCount = modelList.find('li.active').length,
            tab = modelList.siblings('.accordion-tab'),
            tabCountLabel = tab.find('.accordion-count');

        if (tab.hasClass('tab-checked')) {
            tab.removeClass('tab-checked');
        }

        if (!modelsCount == 0) {
            if (modelsCount == 1) {
                tabCountLabel.html('(' + modelsCount + ' Model)');
            }
            else {
                tabCountLabel.html('(' + modelsCount + ' Models)');
            }
        }
        else {
            tabCountLabel.empty();
        }

    },

    setTab: function (tab) {
        var modelList = tab.siblings('.bike-model-list');

        modelList.find('li').addClass('active');
        tab.find('.accordion-count').html('(All models)');
    },

    resetTab: function (tab) {
        var modelList = tab.siblings('.bike-model-list');

        modelList.find('li').removeClass('active');
        tab.find('.accordion-count').empty();
    },

    selectedItems: function () {
        var list = $('#filter-bike-list .bike-model-list li.active'),
            listLength = list.length,
            selection = '';

        list.each(function (index) {
            if (index == 7) {
                return false;
            }
            else {
                if (!index == 0) {
                    selection += ', ' + $(this).find('.bike-model-label').text();
                }
                else {
                    selection = $(this).find('.bike-model-label').text();
                }
            }
        })

        return selection;
    },

    resetAll: function () {
        var bikeList = $('#filter-bike-list');

        bikeList.find('.accordion-tab.active').siblings('.bike-model-list').hide();
        bikeList.find('.accordion-tab.active').removeClass('active');
        bikeList.find('.accordion-tab.tab-checked').removeClass('tab-checked');
        bikeList.find('.bike-model-list li.active').removeClass('active');
        accordion.tabs.find('.accordion-count').text('');
    }
};

/* sort by */
var sortByList = $('#sort-by-list');

sortByList.on('click', 'li', function () {
    var item = $(this);

    if (!item.hasClass('active')) {
        sortByList.find('li.active').removeClass('active');
        item.addClass('active');
    }
});

$('#sort-floating-btn').on('click', function () {
    sortBy.open();
    appendState('sortBy');
});

$('#cancel-sort-by').on('click', function () {
    history.back();
    sortBy.close();
});

$('#apply-sort-by').on('click', function () {
    history.back();
    sortBy.close();
});

var sortBy = {
    popup: $('#sort-by-container'),

    open: function () {
        sortBy.popup.show();
        $('html, body').addClass('lock-browser-scroll');
        $('.modal-background').show();
    },

    close: function () {
        sortBy.popup.hide();
        $('html, body').removeClass('lock-browser-scroll');
        $('.modal-background').hide();
    }    
};

/* popup state */
var appendState = function (state) {
    window.history.pushState(state, '', '');   
};

$(window).on('popstate', function (event) {
    if ($('#filter-container').is(':visible')) {
        if ($('#filter-city-container').is(':visible')) {
            filters.city.close();
        }
        else if ($('#filter-bike-container').is(':visible')) {
            filters.bike.close();
        }

        else {
            filters.close();
        }
    }
    if ($('#sort-by-container').is(':visible')) {
        sortBy.close();
    }
});

(function ($, ko) {
    'use strict';
    // TODO: Hook into image load event before loading others...
    function KoLazyLoad() {
        var self = this;

        var updatebit = ko.observable(true).extend({ throttle: 50 });

        var handlers = {
            img: updateImage
        };

        function flagForLoadCheck() {
            updatebit(!updatebit());
        }

        $(window).on('scroll', flagForLoadCheck);
        $(window).on('resize', flagForLoadCheck);
        $(window).on('load', flagForLoadCheck);

        function isInViewport(element) {
            var rect = element.getBoundingClientRect();
            return rect.bottom > 0 && rect.right > 0 &&
              rect.top < (window.innerHeight || document.documentElement.clientHeight) &&
              rect.left < (window.innerWidth || document.documentElement.clientWidth);
        }

        function updateImage(element, valueAccessor, allBindings, viewModel, bindingContext) {
            var value = ko.unwrap(valueAccessor());
            if (isInViewport(element)) {
                element.src = value;
                $(element).data('kolazy', true);
            }
        }

        function init(element, valueAccessor, allBindings, viewModel, bindingContext) {
            var initArgs = arguments;
            updatebit.subscribe(function () {
                update.apply(self, initArgs);
            });
        }

        function update(element, valueAccessor, allBindings, viewModel, bindingContext) {
            var $element = $(element);

            if ($element.is(':hidden') || $element.css('visibility') == 'hidden' || $element.data('kolazy')) {
                return;
            }

            var handlerName = element.tagName.toLowerCase();
            if (handlers.hasOwnProperty(handlerName)) {
                return handlers[handlerName].apply(this, arguments);
            } else {
                throw new Error('No lazy handler defined for "' + handlerName + '"');
            }
        }

        return {
            handlers: handlers,
            init: init,
            update: update
        }
    }

    ko.bindingHandlers.lazyload = new KoLazyLoad();

})(jQuery, ko);
