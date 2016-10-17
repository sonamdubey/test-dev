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

// custom validation function
var validation = {
    greaterThanOne: function (val) {
        if (val < 1) {
            return false;
        }
        return true;
    },

    userName: function (val) {
        var regexName = /([A-Za-z])\w+/;

        if (!regexName.test(val)) {
            return false;
        }
        return true;
    },

    userEmail: function (val) {
        var regexEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;

        if (!regexEmail.test(val)) {
            return false;
        }
        return true;
    }

}

var sellBike = function () {
    var self = this;

    self.formStep = ko.observable(2);

    self.bikeDetails = ko.observable(new bikeDetails);

    self.personalDetails = ko.observable(new personalDetails);

    self.gotoStep1 = function () {
        if (self.formStep() > 1) {
            self.formStep(1);
        };
    };

    self.gotoStep2 = function () {
        if (self.formStep() > 1) {
            self.formStep(2);
        }
    }

};

var bikeDetails = function () {
    var self = this;

    self.validate = ko.observable(false);
    self.validateOtherColor = ko.observable(false);

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
        validation: [{
            validator: function (val) {
                return !ko.validation.utils.isEmptyVal(val);
            },
            message: 'Please enter kms ridden',
            onlyIf: function () {
                return self.validate();
            }
        },
        {
            validator: validation.greaterThanOne,
            message: 'Please enter kms value greater than 1',
            onlyIf: function () {
                return self.validate();
            }
        }]
    });

    self.expectedPrice = ko.observable('').extend({
        validation: [{
            validator: function (val) {
                return !ko.validation.utils.isEmptyVal(val);
            },
            message: 'Please enter expected price',
            onlyIf: function () {
                return self.validate();
            }
        },
        {
            validator: validation.greaterThanOne,
            message: 'Please enter expected price greater than 1',
            onlyIf: function () {
                return self.validate();
            }
        }]
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

    self.registeredCity = ko.observable().extend({
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
            params: true,
            message: 'Please select a colour',
            onlyIf: function () {
                return self.validate();
            }
        }
    });

    self.colorSelection = function (data, event) {
        var element = $(event.currentTarget);

        if (!element.hasClass('active')) {
            var selection = element.find('.color-box-label').text();
            self.color(selection);
            self.otherColor('');
            colorBox.active(element);
        }
        else {
            colorBox.inactive(element);
        }
    };

    self.otherColor = ko.observable('').extend({
        pattern: {
            params: '[A-Za-z\s]',
            message: 'Please enter valid color',
            onlyIf: function () {
                return self.validateOtherColor();
            }
        }
    });

    self.submitOtherColor = function (data, event) {
        self.validateOtherColor(true);

        if (self.colorError().length === 0) {
            colorBox.userInput();
            self.color(self.otherColor());
        } else {
            self.colorError.showAllMessages();
        }
    };

    self.saveBikeDetails = function (data, event) {
        /*
        self.validate(true);

        if (self.errors().length === 0) {
            vmSellBike.formStep(2);
            scrollToForm.activate();
        } else {
            self.errors.showAllMessages();
        }
        */
        vmSellBike.formStep(2);
        scrollToForm.activate();
    };

    self.errors = ko.validation.group(self);
    self.colorError = ko.validation.group(self.otherColor);

};

var personalDetails = function () {
    var self = this;

    self.validate = ko.observable(false);

    self.sellerType = function (data, event) {
        var element = $(event.currentTarget);

        if (!element.hasClass('checked')) {
            sellerType.check(element);
        }
    };

    self.sellerName = ko.observable('').extend({
        validation: [{
            validator: function (val) {
                return !ko.validation.utils.isEmptyVal(val);
            },
            message: 'Please enter name',
            onlyIf: function () {
                return self.validate();
            }
        },
        {
            validator: validation.userName,
            message: 'Please enter valid name',
            onlyIf: function () {
                return self.validate();
            }
        }]
    });

    self.sellerEmail = ko.observable('').extend({
        validation: [{
            validator: function (val) {
                return !ko.validation.utils.isEmptyVal(val);
            },
            message: 'Please enter email',
            onlyIf: function () {
                return self.validate();
            }
        },
        {
            validator: validation.userEmail,
            message: 'Please enter valid email',
            onlyIf: function () {
                return self.validate();
            }
        }]
    });

    self.sellerMobile = ko.observable('');

    self.savePersonalDetails = function () {
        self.validate(true);

        if (self.errors().length === 0) {
            vmSellBike.formStep(3);
            scrollToForm.activate();
        } else {
            self.errors.showAllMessages();
        }
    };

    self.backToBikeDetails = function () {
        vmSellBike.formStep(1);
        scrollToForm.activate();
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

var vmSellBike = new sellBike();

ko.applyBindings(vmSellBike, document.getElementById('sell-bike-content'));

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

// close color dropdown
$(document).mouseup(function (event) {
    event.stopPropagation();

    if (selectColorBox.hasClass('open') && $('.color-dropdown').is(':visible')) {
        if (!selectColorBox.is(event.target) && selectColorBox.has(event.target).length === 0) {
            colorBox.close();
        }
    }

});

// seller type
var sellerType = {

    check: function (element) {
        element.siblings('.checked').removeClass('checked');
        element.addClass('checked');
    }
}

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

var scrollToForm = {
    container: $('#sell-bike-content'),

    activate: function () {
        var position = scrollToForm.container.offset();

        $('html, body').animate({
            scrollTop: position.top - 51
        }, 200);
        // 51: navbar height
    }
};
