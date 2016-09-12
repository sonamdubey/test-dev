$(document).ready(function () {
    //set filters
    filters.set.all();
});

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
    //accordion.resetAll();
});

/* set slider default values */
var filters = {

    budgetAmount: function (units) {
        var budgetminValue = getRealValue(units[0]),
            budgetmaxValue = getRealValue(units[1]);

        if (units[0] == 0 && units[1] == 7) {
            $("#budget-amount").html('<span class="bwsprite inr-sm-dark"></span> 0 - <span class="bwsprite inr-sm-dark"></span> ' + budgetmaxValue);
        }
        else {
            $("#budget-amount").html('<span class="bwsprite inr-sm-dark"></span> ' + budgetminValue + ' - <span class="bwsprite inr-sm-dark"></span> ' + budgetmaxValue);
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

    set: {

        all: function () {
            //filters.set.city();
            //filters.set.bike();
            filters.set.budget();
            filters.set.kilometers();
            filters.set.bikeAge();
            filters.set.previousOwners();
            //filters.set.sellerType();
        },

        city: function () {
            $('#filter-type-city .selected-filters').text('All India');
        },

        bike: function () {
            filterTypeBike.find('.selected-filters').text('All Bikes');
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

        previousOwners: function () {
            $('#previous-owners-list li.active').removeClass('active');
        },

        sellerType: function () {
            $('.filter-type-seller.checked').removeClass('checked');
        }

    },

    reset: {

        all: function () {
            //filters.reset.city();
            //filters.reset.bike();
            filters.reset.budget();
            filters.reset.kilometers();
            filters.reset.bikeAge();
            filters.reset.previousOwners();
            //filters.reset.sellerType();
        },

        city: function () {
            $('#filter-type-city .selected-filters').text('All India');
        },

        bike: function () {
            filterTypeBike.find('.selected-filters').text('All Bikes');
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
        },

        previousOwners: function () {
            $('#previous-owners-list li.active').removeClass('active');
        },

        sellerType: function () {
            $('.filter-type-seller.checked').removeClass('checked');
        }
    }
};