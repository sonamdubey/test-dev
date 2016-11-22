var selectColorBox = $('#select-color-box');

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

$(document).ready(function () {
    var selectDropdownBox = $('.select-box-no-input');

    selectDropdownBox.each(function () {
        var text = $(this).find('.chosen-select').attr('data-title'),
            searchBox = $(this).find('.chosen-search')

        searchBox.empty().append('<p class="no-input-label">' + text + '</p>');
    });
    
});

// custom validation function
var validation = {
    greaterThanOne: function (val) {
        if (val < 1) {
            return false;
        }
        return true;
    },

    kmsMaxValue: function (val) {
        if (val > 999999) {
            return false;
        }
        return true;
    },

    priceMaxValue: function (val) {
        if (val > 6000000) {
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
    },

    userMobile: function (val) {
        var regexMobile = /^[7-9][0-9]{9}$/;

        if (val[0] < 7 || !regexMobile.test(val)) {
            vmSellBike.personalDetails().mobileLabel(false);
            return false;
        }
        return true;
    },

    userOTP: function (val) {
        if (isNaN(val)) {
            return false;
        }
        return true;
    },

    otpLength: function (val) {
        if (val.length != 5) {
            return false;
        }
        return true;
    }

}

var sellBike = function () {
    var self = this;

    self.formStep = ko.observable(3);

    self.bikeDetails = ko.observable(new bikeDetails);

    self.personalDetails = ko.observable(new personalDetails);

    self.verificationDetails = ko.observable(new verificationDetails);

    self.gotoStep1 = function () {
        if (self.formStep() > 1) {
            self.formStep(1);
            self.verificationDetails().status(false);
        };
    };

    self.gotoStep2 = function () {
        if (self.formStep() > 2) {
            self.formStep(2);
            self.verificationDetails().status(false);
        };
    };

};

var bikeDetails = function () {
    var self = this;

    self.validate = ko.observable(false);
    self.validateOtherColor = ko.observable(false);

    self.citySelectionStatus = ko.observable(''); // bike city or registered city

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
        },
        {
            validator: validation.kmsMaxValue,
            message: 'Please enter valid kms',
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
        },
        {
            validator: validation.priceMaxValue,
            message: 'Please enter valid price',
            onlyIf: function () {
                return self.validate();
            }
        }]
    });

    self.city = ko.observable('').extend({
        required: {
            params: true,
            message: 'Please select city',
            onlyIf: function () {
                return self.validate();
            }
        }
    });

    self.registeredCity = ko.observable('').extend({
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
        if (event != null) {
            var element = $(event.currentTarget);
        }
        if (data != null) {
            colorId = data.colorId;
        }
        if (!element.hasClass('active')) {
            var selection = element.find('.color-box-label').text();
            self.color(selection);
            self.otherColor('');
            colorBox.active(element);
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

    self.submitColor = function (data, event) {
        modalPopup.close('#color-popup');
    };

    self.submitOtherColor = function (data, event) {
        self.validateOtherColor(true);

        if (self.colorError().length === 0) {
            colorBox.userInput();
            self.color(self.otherColor());
            modalPopup.close('#color-popup');
        } else {
            self.colorError.showAllMessages();
        }
    };

    self.saveBikeDetails = function (data, event) {
        self.validate(true);

        if (self.errors().length === 0) {
            vmSellBike.formStep(2);
        }
        else {
            self.errors.showAllMessages();
        }

        scrollToForm.activate();
        vmSellBike.verificationDetails().status(false);
    };

    self.errors = ko.validation.group(self);
    self.colorError = ko.validation.group(self.otherColor);
};

var personalDetails = function () {
    var self = this;

    self.validate = ko.observable(false);
    self.sellerTypeVal = ko.observable(2);
    self.mobileLabel = ko.observable(true);

    self.sellerType = function (data, event) {
        if (event != null) {
            var element = $(event.currentTarget);

            if (!element.hasClass('checked')) {
                sellerType.check(element);
            }
            self.sellerTypeVal(element.attr("value"));
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

    self.sellerMobile = ko.observable('').extend({
        validation: [{
            validator: function (val) {
                return !ko.validation.utils.isEmptyVal(val);
            },
            message: 'Please enter mobile number',
            onlyIf: function () {
                return self.validate();
            }
        },
        {
            validator: validation.userMobile,
            message: 'Please enter valid mobile number',
            onlyIf: function () {
                return self.validate();
            }
        }]
    });

    self.termsCheckbox = ko.observable(true).extend({
        validation: [{
            validator: function (val) {
                return val;
            },
            message: 'Required!',
            onlyIf: function () {
                return self.validate();
            }
        }]
    });

    self.terms = function (data, event) {
        var element = $(event.currentTarget);

        if (!element.hasClass('active')) {
            element.addClass('active');
            self.termsCheckbox(true);
        }
        else {
            element.removeClass('active');
            self.termsCheckbox(false);
        }
    };

    self.listYourBike = function () {
        self.validate(true);

        if (self.errors().length === 0) {
            // user not verified
            vmSellBike.verificationDetails().status(true);
        }
        else {
            self.errors.showAllMessages();
        }

        if (self.mobileError().length != 0) {
            self.mobileLabel(false);
        }

        scrollToForm.activate();
    };

    self.backToBikeDetails = function () {
        vmSellBike.formStep(1);
        scrollToForm.activate();
        vmSellBike.verificationDetails().status(false);
    };

    self.errors = ko.validation.group(self);
    self.mobileError = ko.validation.group(self.sellerMobile);

};

var verificationDetails = function () {
    var self = this;

    self.status = ko.observable(false);
    self.updateMobileStatus = ko.observable(false);

    self.validateOTP = ko.observable(false);
    self.validateMobile = ko.observable(false);

    self.otpCode = ko.observable('').extend({
        validation: [{
            validator: function (val) {
                return !ko.validation.utils.isEmptyVal(val);
            },
            message: 'Please enter your verification code',
            onlyIf: function () {
                return self.validateOTP();
            }
        },
        {
            validator: validation.userOTP,
            message: 'Verification code should be numeric',
            onlyIf: function () {
                return self.validateOTP();
            }
        },
        {
            validator: validation.otpLength,
            message: 'Verification code should be of 5 digits',
            onlyIf: function () {
                return self.validateOTP();
            }
        }]
    });

    self.updateSellerMobile = function () {
        self.updateMobileStatus(true);
        self.updatedMobile(vmSellBike && vmSellBike.personalDetails() ? vmSellBike.personalDetails().sellerMobile() : '');
        $('#updatedMobile').focus();
    };

    self.updatedMobile = ko.observable('').extend({
        validation: [{
            validator: function (val) {
                return !ko.validation.utils.isEmptyVal(val);
            },
            message: 'Please enter mobile number',
            onlyIf: function () {
                return self.validateMobile();
            }
        },
        {
            validator: validation.userMobile,
            message: 'Please enter valid mobile number',
            onlyIf: function () {
                return self.validateMobile();
            }
        }]
    });

    self.submitUpdatedMobile = function () {
        self.validateMobile(true);
        $('#otpCode').val("");
        if (self.errorMobile().length === 0) {
            self.updateMobileStatus(false);
            vmSellBike.personalDetails().sellerMobile(self.updatedMobile());
            self.otpCode('');
            $('#otpCode').focus();
            self.validateOTP(false);
            //vmSellBike.personalDetails().listYourBike();
        }
        else {
            self.errorMobile.showAllMessages();
        }

        scrollToForm.activate();
    };

    self.verifySeller = function () {
        self.validateOTP(true);

        if (self.errorOTP().length === 0) {
            vmSellBike.formStep(3);
            scrollToForm.activate();
        }
        else {
            self.errorOTP.showAllMessages();
        }
    };

    self.errorOTP = ko.validation.group(self.otpCode);
    self.errorMobile = ko.validation.group(self.updatedMobile);

};

var vmSellBike = new sellBike();
ko.applyBindings(vmSellBike, document.getElementById('sell-bike-content'));

// city
var cityListContainer = $('#city-slideIn-drawer'),
    effect = 'slide',
    options = { direction: 'right' },
    duration = 500;

$('#city-select-element').on('click', '.city-box-default', function () {
    cityListSelection.open();
    vmSellBike.bikeDetails().citySelectionStatus('bike-city');
    appendState('bikeCity');
});

$('#registration-select-element').on('click', '.city-box-default', function () {
    cityListSelection.open();
    vmSellBike.bikeDetails().citySelectionStatus('registered-city');
    appendState('registrationCity');
});

$('#close-city-filter').on('click', function () {
    cityListSelection.close();
});

$('#city-slideIn-drawer').on('click', '.filter-list li', function () {
    var element = $(this).text();

    if (vmSellBike.bikeDetails().citySelectionStatus() == 'bike-city') {
        vmSellBike.bikeDetails().city(element);
    }
    else if (vmSellBike.bikeDetails().citySelectionStatus() == 'registered-city') {
        vmSellBike.bikeDetails().registeredCity(element);
    }

    cityListSelection.close();

});

var cityListSelection = {
    open: function () {
        cityListContainer.show(effect, options, duration, function () {
            cityListContainer.addClass('fix-header-input');
        });
    },

    close: function () {
        cityListContainer.hide(effect, options, duration, function () { });
        cityListContainer.removeClass('fix-header-input');
    }
};

// seller type
var sellerType = {
    check: function (element) {
        element.siblings('.checked').removeClass('checked');
        element.addClass('checked');
    }
}

// color
selectColorBox.on('click', '.color-box-default', function () {
    modalPopup.open('#color-popup');
    appendState('colorPopup');
});

var colorBox = {
    popup: $('#color-popup'),

    active: function (element) {
        colorBox.popup.find('li.active').removeClass('active');
        element.addClass('active');
        selectColorBox.addClass('selection-done');
    },

    userInput: function () {
        colorBox.popup.find('li.active').removeClass('active');
        selectColorBox.addClass('selection-done');
    }
};

$('.cancel-popup-btn').on('click', function () {
    var container = $(this).closest('.modal-popup-container');
    history.back();
    modalPopup.close(container);
});

// modal popup
var modalPopup = {
    open: function (container) {
        $(container).show();
        $('html, body').addClass('lock-browser-scroll');
        $('.modal-background').show();
    },

    close: function (container) {
        $(container).hide();
        $('html, body').removeClass('lock-browser-scroll');
        $('.modal-background').hide();
    }
};

/* popup state */
var appendState = function (state) {
    window.history.pushState(state, '', '');
};

$(window).on('popstate', function (event) {
    if ($('#city-slideIn-drawer').is(':visible')) {
        cityListSelection.close();
    }
    if ($('#color-popup').is(':visible')) {
        modalPopup.close('#color-popup');
    }
});

$('.chosen-container').on('touchstart', function (event) {
    event.preventDefault();
    $(this).trigger('mousedown');
});

$('.select-box select').on('change', function () {
    $(this).closest('.select-box').addClass('done');
    $('body').trigger('click'); // prevent chosen select from triggering background click events
});

var scrollToForm = {
    container: $('#sell-bike-content'),

    activate: function () {
        var position = scrollToForm.container.offset();

        $('html, body').animate({
            scrollTop: position.top
        }, 200);
    }
};