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
    filterContainer.show(effect, options, duration, function () {
        $('html, body').addClass('lock-browser-scroll');
        filterContainer.addClass('fixed');
    });

    appendState('filter');
});

$('#close-filter').on('click', function () {
    history.back();
    filters.close();
});


/* budget slider */
var budgetValue = ['0', '10,000', '20,000', '35,000', '50,000', '80,000', '1,25,000', '2,00,000+'],
    budgetKey = [0, 1, 2, 3, 4, 5, 6, 7];

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

/* kms slider */
$("#kms-range-slider").slider({
    range: 'min',
    value: 80000,
    min: 5000,
    max: 80000,
    step: 5000,
    slide: function (event, ui) {
        filters.kilometerAmount(ui.value);
    }
});

/* bike age slider */
$("#bike-age-slider").slider({
    range: 'min',
    value: 8,
    min: 1,
    max: 8,
    step: 1,
    slide: function (event, ui) {        
        filters.bikeAgeAmount(ui.value);
    }
});

$('#reset-filters').on('click', function () {
    filters.reset.all();
    accordion.resetAll();
});

var filterTypeBike = $('#filter-type-bike');

/* set slider default values */
var filters = {

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

    kilometerAmount: function (unit) {
        var kilometerValue = unit.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");

        if (unit == 80000) {
            $("#kms-amount").html('0 - ' + kilometerValue + '+ kms');
        }
        else {
            $("#kms-amount").html('0 - ' + kilometerValue + ' kms');
        }
    },

    bikeAgeAmount: function (unit) {
        if (unit == 8) {
            $("#bike-age-amount").html('0 - ' + unit + '+ years');
        }
        else {
            $("#bike-age-amount").html('0 - ' + unit + ' years');
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

    set: {

        all: function () {
            filters.set.bike();
            filters.set.budget();
            filters.set.kilometers();
            filters.set.bikeAge();
        },

        bike: function () {
            filterTypeBike.find('.filter-option-key').hide();
            filterTypeBike.find('.selected-filters').text('Bike');
        },

        budget: function () {
            var values = [3, 5];
            $('#budget-range-slider').slider('option', 'values', values);
            
            filters.budgetAmount(values);
        },

        kilometers: function () {
            var kilometerSlider = $('#kms-range-slider'),
                kmSliderValue;

            kilometerSlider.slider('option', 'value', 50000);
            kmSliderValue = kilometerSlider.slider('value');

            filters.kilometerAmount(kmSliderValue);
        },
        
        bikeAge: function () {
            var ageSlider = $('#bike-age-slider'),
                ageSliderValue;

            ageSlider.slider('option', 'value', 5);
            ageSliderValue = ageSlider.slider('value');

            filters.bikeAgeAmount(ageSliderValue);
        },

    },

    reset: {

        all: function () {
            filters.reset.bike();
            filters.reset.budget();
            filters.reset.kilometers();
            filters.reset.bikeAge();
            $('#previous-owners-list li.active').removeClass('active');
            $('.filter-type-seller.checked').removeClass('checked');
        },

        city: $('#filter-type-city .selected-filters').text('All India'),

        bike: function () {
            filterTypeBike.find('.filter-option-key').hide();
            filterTypeBike.find('.selected-filters').text('Bike');
        },

        budget: function () {
            $('#budget-range-slider').slider('option', 'values', [0, 7]);
            $('#budget-amount').html('<span class="bwmsprite inr-xxsm-icon"></span>0 - <span class="bwmsprite inr-xxsm-icon"></span>2,00,000+');
        },

        kilometers: function () {
            $('#kms-range-slider').slider('option', 'value', 80000);
            $("#kms-amount").html('0 - ' + $("#kms-range-slider").slider("value").toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + ' kms');
        },

        bikeAge: function () {
            $('#bike-age-slider').slider('option', 'value', 8);
            $("#bike-age-amount").html('0 - ' + $("#bike-age-slider").slider("value") + '+ years');
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

/* city filter */
var cityFilter = $('#filter-city-container');

$('#filter-type-city').on('click', '.filter-option-value', function () {
    filters.city.open();
    appendState('filterCity');
});

$('#close-city-filter').on('click', function () {
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
});