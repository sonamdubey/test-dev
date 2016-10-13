var selectColorBox = $('#select-color-box');

$('.chosen-select').chosen();

ko.validation.init({
    errorElementClass: 'invalid',
    insertMessages: false
}, true);

//for jquery chosen
ko.bindingHandlers.chosen = {
    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        var $element = $(element);
        var options = ko.unwrap(valueAccessor());
        if (typeof options === 'object')
            $element.chosen(options);

        ['options', 'selectedOptions', 'value'].forEach(function (propName) {
            if (allBindings.has(propName)) {
                var prop = allBindings.get(propName);
                if (ko.isObservable(prop)) {
                    prop.subscribe(function () {
                        $element.trigger('chosen:updated');
                    });
                }
            }
        });
    }
}

var sellBike = function () {
    var self = this;

    self.bikeDetails = ko.observable(new bikeDetails);

};

var bikeDetails = function () {
    var self = this;

    self.validate = ko.observable(false);

    self.make = ko.observable().extend({
        required: {
            params: true,
            message: 'Please select brand',
            onlyIf: function () {
                return self.validate();
            }
        }
    });

    self.model = ko.observable().extend({
        required: {
            params: true,
            message: 'Please select model',
            onlyIf: function () {
                return self.validate();
            }
        }
    });

    self.version = ko.observable().extend({
        required: {
            params: true,
            message: 'Please select version',
            minLength: 1,
            onlyIf: function () {
                return self.validate();
            }
        }
    });

    self.kmsRidden = ko.observable('').extend({
        required: {
            params: true,
            message: 'Please enter kms ridden',
            onlyIf: function () {
                return self.validate();
            }
        }
    });

    self.expectedPrice = ko.observable('').extend({
        required: {
            params: true,
            message: 'Please enter amount',
            onlyIf: function () {
                return self.validate();
            }
        }
    });

    self.city = ko.observable().extend({
        required: {
            params: true,
            message: 'Please select city',
            onlyIf: function () {
                return self.validate();
            }
        }
    });

    self.owner = ko.observable().extend({
        required: {
            params: true,
            message: 'Please select owner type',
            onlyIf: function () {
                return self.validate();
            }
        }
    });

    self.color = ko.observable().extend({
        required: {
            onlyIf: function () {
                return self.validate();
            }
        },
        pattern: {
            params: '[A-Za-z\s]',
            message: 'Please select a colour',
        }
    });

    self.colorSelection = function (data, event) {
        var element = $(event.currentTarget);

        if (!element.hasClass('active')) {
            var selection = element.find('.color-box-label').text();
            self.color(selection);
            colorBox.active(element);
        }
        else {
            colorBox.inactive(element);
        }
    };

    self.otherColor = ko.observable('');

    self.submitOtherColor = function (data, event) {
        self.color(self.otherColor());
        colorBox.userInput();
    };


    self.saveBikeDetails = function (data, event) {
        self.validate(true);

        if (self.errors().length === 0) {
            alert('Thank you.');
        } else {
            self.errors.showAllMessages();
        }
    };

    self.errors = ko.validation.group(self);

};

$(document).ready(function () {
    var chosenSelectBox = $('.chosen-select');

    chosenSelectBox.each(function () {
        var text = $(this).attr('data-placeholder');

        $(this).siblings('.chosen-container').find('input[type=text]').attr('placeholder', text);
    });

    var ownerSearchBox = $('.select-box-no-input').find('.chosen-search');
    ownerSearchBox.empty().append('<p class="no-input-label">Owner</p>');

});

// color box
selectColorBox.on('click', '.color-box-default', function () {
    if (!selectColorBox.hasClass('open')) {
        colorBox.open();
    }
    else {
        colorBox.close();
    }
});

var colorBox = {
    dropdown: $('.color-dropdown'),

    open: function () {
        selectColorBox.addClass('open');
    },

    close: function () {
        selectColorBox.removeClass('open');
    },

    active: function (element) {
        colorBox.dropdown.find('li.active').removeClass('active');
        element.addClass('active');
        selectColorBox.addClass('selection-done');
        colorBox.close();
    },

    inactive: function (element) {
        element.removeClass('active');
    },

    userInput: function () {
        colorBox.dropdown.find('li.active').removeClass('active');
        selectColorBox.addClass('selection-done');
        colorBox.close();
    }
};

var vmSellBike = new sellBike();

ko.applyBindings(vmSellBike, document.getElementById('sell-bike-content'));

// close color dropdown
$(document).mouseup(function (event) {
    event.stopPropagation();

    if (selectColorBox.hasClass('open') && $('.color-dropdown').is(':visible')) {
        if (!selectColorBox.is(event.target) && selectColorBox.has(event.target).length === 0) {
            colorBox.close();
        }
    }

});

$('.select-box select').on('change', function () {
    $(this).closest('.select-box').addClass('done');
});

// Disable Mouse scrolling
$('input[type=number]').on('mousewheel', function (e) { $(this).blur(); });
// Disable keyboard scrolling
$('input[type=number]').on('keydown', function (e) {
    var key = e.charCode || e.keyCode;
    // Disable Up and Down Arrows on Keyboard
    if (key == 38 || key == 40) {
        e.preventDefault();
    } else {
        return;
    }
});
