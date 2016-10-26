var selectColorBox = $('#select-color-box'),
    selectCalendarBox = $('#select-calendar-box'),
    calendarErrorBox = $('#calendar-error');

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
        var regexMobile = /^[1-9][0-9]{9}$/;
        
        if (val[0] == "0" || !regexMobile.test(val)) {
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

    self.formStep = ko.observable(1);

    self.bikeDetails = ko.observable(new bikeDetails);

    self.verificationDetails = ko.observable(new verificationDetails);

    self.personalDetails = ko.observable(new personalDetails);

    self.moreDetails = ko.observable(new moreDetails);

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
        },
        {
            validator: validation.kmsMaxValue,
            message: 'Please enter kms value less than 10,00,000',
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
            message: 'Please enter expected price less than 60,00,000',
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
        self.validate(true);

        if (self.errors().length === 0) {
            vmSellBike.formStep(2);
        }
        else {
            self.errors.showAllMessages();
        }

        scrollToForm.activate();
    };

    self.errors = ko.validation.group(self);
    self.colorError = ko.validation.group(self.otherColor);
    
    self.manufactureYear = ko.observable('');
    self.manufactureMonth = ko.observable('');
    self.manufactureMonthName = ko.observable('');

    self.manufacturingDate = ko.computed(function () {
        return self.manufactureMonthName() + ' ' + self.manufactureYear();
    }).extend({
        required: {
            params: true,
            message: 'Please select year of manufacturing',
            onlyIf: function () {
                return self.validate();
            }
        }
    });

    self.submitManufacturingDate = function (data, event) {
        if (self.manufactureYear() != '') {
            if (self.manufactureMonth() != '') {
                selectCalendarBox.addClass('selection-done');
                calender.close();
                calendarErrorBox.text('');
            }
            else {
                calendarErrorBox.text('Please select month');
            }
        }
        else {
            calendarErrorBox.text('Please select year');
        }
    };
};

var personalDetails = function () {
    var self = this;

    self.validate = ko.observable(false);
    self.mobileLabel = ko.observable(true);
    self.termsCheckbox = ko.observable(true);

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

    self.terms = function (data, event) {
        var element = $(event.currentTarget);

        if (!element.hasClass('checked')) {
            element.addClass('checked');
            self.termsCheckbox(true);
        }
        else {
            element.removeClass('checked');
            self.termsCheckbox(false);
        }
    };

    self.listYourBike = function () {
        self.validate(true);

        if (self.errors().length === 0) {
            if (true) { // if user is not verified
                vmSellBike.verificationDetails().status(true);
                $('#otpCode').focus();
            }
            else {
                vmSellBike.formStep(3);
            }

            scrollToForm.activate();
        }
        else {
            self.errors.showAllMessages();
        }

        if (self.mobileError().length != 0) {
            self.mobileLabel(false);
        }
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

        if (self.errorMobile().length === 0) {
            self.updateMobileStatus(false);
            vmSellBike.personalDetails().sellerMobile(self.updatedMobile());
            self.otpCode('');
            $('#otpCode').focus();
            self.validateOTP(false);
            scrollToForm.activate();
        }
        else {
            self.errorMobile.showAllMessages();
        }
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

var moreDetails = function () {
    var self = this;

    self.registrationNumber = ko.observable('');

    self.updateAd = function () {
        vmSellBike.formStep(4);
        scrollToForm.activate();
    };
    
    self.noThanks = function () {
        vmSellBike.formStep(4);
        scrollToForm.activate();
    };
};

Dropzone.autoDiscover = false;

$(document).ready(function () {
    var chosenSelectBox = $('.chosen-select');

    chosenSelectBox.each(function () {
        var text = $(this).attr('data-placeholder');

        $(this).siblings('.chosen-container').find('input[type=text]').attr('placeholder', text);
    });

    var selectDropdownBox = $('.select-box-no-input');

    selectDropdownBox.each(function () {
        var text = $(this).find('.chosen-select').attr('data-title'),
            searchBox = $(this).find('.chosen-search')

        searchBox.empty().append('<p class="no-input-label">' + text + '</p>');
    });
    
    $('#add-photos-dropzone').dropzone({
        maxFilesize: 4,
        maxFiles: 10,
        addRemoveLinks: true,
        acceptedFiles: ".png, .jpg",
        url: "/target",
        init: function () {
            var myDropzone = this;
            
            this.on("sending", function (file) {
                $(file.previewElement).find('#spinner-content').hide();
            });

            this.on("removedfile", function (file) {
                setProfilePhoto();
            });

            this.on("success", function (file) {
                setProfilePhoto();
            });

            this.on("error", function (file, response) {
                $(file.previewElement).find('#spinner-content').hide();
                $(file.previewElement).find('.dz-error-message').text('Failed to upload');
                $(file.previewElement).find('.dz-error-mark').on('click', function () {
                    myDropzone.removeFile(file);
                    myDropzone.addFile(file);
                });
            });

            this.on("maxfilesexceeded", function (file) {
                alert("You can upload maximum 10 photos!");
                myDropzone.removeFile(file);
            });
        }
    });

    //set year
    calender.year.set(1980);
    calender.month.set();
});

var setProfilePhoto = function () {
    var container = $('#add-photos-dropzone .dz-preview.dz-success').first();
    if (!container.hasClass('dz-profile-photo')) {
        container.addClass('dz-profile-photo');
        container.append('<div id="profile-photo-content"><span class="sell-bike-sprite ribbon-icon"></span><span class="ribbon-label">Profile photo</span></div>')
    }
};

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

    if (selectCalendarBox.hasClass('open') && $('#calendar-content').is(':visible')) {
        if (!selectCalendarBox.is(event.target) && selectCalendarBox.has(event.target).length === 0) {
            if (vmSellBike.bikeDetails().manufactureYear() == '' && vmSellBike.bikeDetails().manufactureMonth() != '') {
                calendarErrorBox.text('Please select year');
            }
            if (vmSellBike.bikeDetails().manufactureYear() != '' && vmSellBike.bikeDetails().manufactureMonth() == '') {
                calendarErrorBox.text('Please select month');
            }
            if (vmSellBike.bikeDetails().manufactureYear() == '' && vmSellBike.bikeDetails().manufactureMonth() == '') {
                calender.close();
            }
            else {
                $('#submit-calendar-btn').trigger('click');
            }
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
$('input[type=number]').on('mousewheel', function (e) { $(this).blur();});
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

selectCalendarBox.on('click', '.calendar-box-default', function () {
    if (!selectCalendarBox.hasClass('open')) {
        calender.open();
    }
    else {
        calender.close();
    }
});

$('.year-prev').on('click', function () {
    var activeElement = calender.year.list.find('.active'),
        prevElement = activeElement.prev();

    if (prevElement.length !== 0) {
        calender.year.scrollPosition(prevElement);
    }
});

$('.year-next').on('click', function () {
    var activeElement = calender.year.list.find('.active'),
        nextElement = activeElement.next();

    if (nextElement.length !== 0) {
        calender.year.scrollPosition(nextElement);
    }
});

$('#year-list').on('click', 'span', function () {
    var element = $(this);
    calender.year.selection(element);
});

$('#month-list').on('click', 'li', function () {
    var element = $(this);
    calender.month.selection(element);
});

// calender
var calender = {

    width: 360,

    year: {
        list: $('#year-list'),

        set: function (startYear) {
            var endYear = new Date().getFullYear(),
                yearCount = endYear - startYear,
                years = [],
                limit = 5;
            
            for (var i = endYear; i >= startYear; i--) {
                years.push(i);
            }

            for (var i = 0; i < yearCount; i += 5) {
                if (i != 0) {
                    limit = i + 5;
                }

                var bundle = [];
                for (var j = i; j < limit; j++) {
                    if (years[j] !== undefined) {
                        bundle.push(years[j]);
                        bundle.sort();
                    }
                }

                var item = '';
                for(var x = 0; x < bundle.length; x++) {
                    item += '<span data-value="' + bundle[x] + '">' + bundle[x] + '</span>';
                };

                var listItems = calender.year.list.find('li');
                if (listItems.length == 0) {
                    calender.year.list.append('<li>' + item + '</li>');
                }
                else {
                    $('<li>' + item + '</li>').insertBefore(listItems.first());
                }
            }
                        
        },

        selection: function (element) {
            if (!element.hasClass('selected')) {
                calender.year.list.find('.selected').removeClass('selected');
                element.addClass('selected')
                elementValue = element.attr('data-value');
                vmSellBike.bikeDetails().manufactureYear(elementValue);
                calendarErrorBox.text('');

                var currentYear = new Date().getFullYear();
                if (elementValue == currentYear) {
                    var currentMonth = new Date().getMonth() + 1,
                        monthList = calender.month.list.find('li');

                    for (var i = 0; i < 12; i++) {
                        var item = monthList[i];
                        if ($(item).attr('data-value') > currentMonth) {
                            $(item).removeClass('selected').addClass('not-allowed');
                        }
                    }
                    if (vmSellBike.bikeDetails().manufactureMonth() > currentMonth) {
                        vmSellBike.bikeDetails().manufactureMonth('');
                        vmSellBike.bikeDetails().manufactureMonthName('');
                    }
                }
                else {
                    calender.month.list.find('.not-allowed').removeClass('not-allowed');
                }
            }
        },

        scrollPosition: function (element) {
            var containerOffset = calender.year.list.offset().left - 70;

            calender.year.list.find('.active').removeClass('active');
            element.addClass('active');
            calender.year.list.animate({
                scrollLeft: element.index() * element.width() - containerOffset
            });
        },

        initScroll: function (element) {
            var containerOffset = calender.year.list.offset().left - 100;

            element.addClass('active');
            calender.year.list.scrollLeft(element.index() * element.width() - containerOffset);
        }
    },

    month: {
        list: $('#month-list'),

        set: function () {
            var monthArr = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

            for (var i = 0; i < 12; i++) {
                calender.month.list.append('<li data-value="' + (i + 1) + '">' + monthArr[i] + '</li>');
            }

            var currentMonth = new Date().getMonth() + 1;
            
        },

        selection: function (element) {
            if (!element.hasClass('selected')) {
                var elementValue = element.attr('data-value'),
                    elementText = element.text();

                calender.month.list.find('.selected').removeClass('selected');
                element.addClass('selected')
                calendarErrorBox.text('');

                vmSellBike.bikeDetails().manufactureMonth(elementValue);
                vmSellBike.bikeDetails().manufactureMonthName(elementText);
            }
        },
    },

    open: function () {
        var lastElement = calender.year.list.find('li').last();

        selectCalendarBox.addClass('open');
        calender.year.initScroll(lastElement);
    },

    close: function () {
        selectCalendarBox.removeClass('open');
    }
};