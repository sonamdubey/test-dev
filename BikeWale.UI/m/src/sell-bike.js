var citiesList = $("#filter-city-list li");

var selectColorBox = $('#select-color-box'),
    effect = 'slide',
    options = { direction: 'right' },
    duration = 500;

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
    if (userId != null) {
        var pdetails = vmSellBike.personalDetails();
        pdetails.sellerName(userName);
        pdetails.sellerEmail(userEmail);
    }
    var selectDropdownBox = $('.select-box-no-input');

    selectDropdownBox.each(function () {
        var text = $(this).find('.chosen-select').attr('data-title'),
            searchBox = $(this).find('.chosen-search')

        searchBox.empty().append('<p class="no-input-label">' + text + '</p>');
    });

    var monthList = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
        currentDate = new Date(),
        currentMonth = currentDate.getMonth(),
        currentYear = currentDate.getFullYear();

    $('#manufacturingDate').Zebra_DatePicker({
        format: 'M Y',
        direction: ['Jan 1980', monthList[currentMonth] + ' ' + currentYear],
        start_date: monthList[currentMonth] + ' ' + currentYear,
        onSelect: function () {
            vmSellBike.bikeDetails().manufacturingDate($(this).val());
        }
    });

    Dropzone.autoDiscover = false;

    $('#add-photos-dropzone').dropzone({
        url: "/file/post"
    });
    
});

var vmCities = function () {
    var self = this;
    self.SelectedCity = ko.observable();

    self.cityFilter = ko.observable();

    self.visibleCities = ko.computed(function () {
        filter = self.cityFilter();
        filterObj = citiesList;
        if (filter && filter.length > 0) {
            var pat = new RegExp(filter, "i");
            citiesList.filter(function (place) {
                if (pat.test($(this).text())) $(this).show(); else $(this).hide();
            });

        }
        citiesList.first().show();
    });
}

// custom validation function
var validation = {
    greaterThanOne: function (val) {
        val = val.replace(/,/g, "");
        if (val < 1) {
            return false;
        }
        return true;
    },

    kmsMaxValue: function (val) {
        val = val.replace(/,/g, "");
        if (val > 999999) {
            return false;
        }
        return true;
    },

    priceMaxValue: function (val) {
        val = val.replace(/,/g, "");
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
    },

    isNumber: function (val) {
        val = val.replace(/,/g, "");
        if (isNaN(val)) {
            return false;
        }
        return true;
    }

}

var sellBike = function () {
    var self = this;

    self.formStep = ko.observable(1);

    self.bikeDetails = ko.observable(new bikeDetails);

    self.personalDetails = ko.observable(new personalDetails);

    self.verificationDetails = ko.observable(new verificationDetails);

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
        };
    };

};

var bikeDetails = function () {
    var self = this;

    self.Cities = ko.observable(new vmCities());

    self.modelArray = ko.observableArray();
    self.versionArray = ko.observableArray();
    self.colorArray = ko.observableArray();

    self.validate = ko.observable(false);
    self.validateOtherColor = ko.observable(false);

    self.citySelectionStatus = ko.observable(''); // bike city or registered city

    self.bikeStatus = ko.observable(false);

    self.color = ko.observable();
    self.colorId = ko.observable();

    self.makeName = ko.observable('');
    self.modelName = ko.observable('');
    self.versionName = ko.observable('');

    self.makeId = ko.observable('');
    self.makeMaskingName = ko.observable('');

    self.modelId = ko.observable('');
    self.modelMaskingName = ko.observable('');

    self.versionId = ko.observable('');
    self.versionMaskingName = ko.observable('');

    self.makeChanged = function (data, event) {
        var element = $(event.currentTarget);

        self.modelName('');
        self.versionName('');
        

        self.makeName(element.text());
        self.makeId(element.attr("data-id"));
        self.makeMaskingName(element.attr("data-makeMasking"));

        bikePopup.stageModel();
        bikePopup.scrollToHead();

        if (self.makeName() != null) {
            $.ajax({
                type: "Get",
                async: false,
                url: "/api/modellist/?requestType=3&makeId=" + self.makeId(),
                contentType: "application/json",
                dataType: 'json',
                success: function (response) {
                    if (response) {                        
                        self.modelArray(response.modelList);
                    }
                },
                complete: function (xhr, ajaxOptions, thrownError) {
                    
                }
            });
        }

        self.bikeStatus(false);

        // beforesend - bikePopup.showLoader()
        // complete - bikePopup.hideLoader()
        
    };

    self.modelChanged = function (data, event) {
        var element = $(event.currentTarget);

        self.versionName('');
        self.bikeStatus(false);

        self.modelName(data.modelName);
        self.modelId(data.modelId);
        self.modelMaskingName(data.maskingName);

        bikePopup.stageVersion();
        bikePopup.scrollToHead();

        if (self.modelId() != null && self.modelId() != -1) {
            $.ajax({
                type: "Get",
                url: "/api/versionList/?requestType=3&modelId=" + self.modelId(),
                contentType: "application/json",
                dataType: 'json',
                success: function (response) {
                    self.versionArray(response.Version);
                },
                complete: function (xhr, ajaxOptions, thrownError) {
                }
            });
        }

       
    };

    self.versionChanged = function (data, event) {
        var element = $(event.currentTarget);

        self.versionName(data.versionName);
        self.versionId(data.versionId);

        self.versionName(element.text());
        self.bikeStatus(true);

        bikePopup.close();

        if (self.versionId() != null && self.versionId() != -1) {
            $.ajax({
                type: "Get",
                url: "/api/version/" + self.versionId() + "/color/",
                contentType: "application/json",
                dataType: 'json',
                success: function (response) {
                    if (response) {
                        self.colorArray(response.colors);
                    }
                },
                complete: function (xhr, ajaxOptions, thrownError) {                    
                }
            });
        }
    };   

    self.bike = ko.computed(function () {
        if (self.bikeStatus()) {
            return self.makeName() + ' ' + self.modelName() + ' ' + self.versionName();
        }
    }).extend({
        required: {
            params: true,
            message: 'Please select bike',
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
            validator: validation.isNumber,
            message: 'Please enter valid kms',
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
            validator: validation.isNumber,
            message: 'Please enter valid price',
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
        self.color(data.colorName);
        self.colorId(data.colorId);
        if (event != null) {
            var element = $(event.currentTarget);
        }
        if (data != null) {
            colorId = data.colorId;
        }
        if (!element.hasClass('active')) {
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
        var selectedColor = $('#color-popup li.active .color-box-label').text();
        self.color(selectedColor);
        modalPopup.close('#color-popup');
        history.back();
    };

    self.submitOtherColor = function (data, event) {
        self.validateOtherColor(true);

        if (self.colorError().length === 0) {
            colorBox.userInput();
            self.color(self.otherColor());
            modalPopup.close('#color-popup');
            history.back();
        } else {
            self.colorError.showAllMessages();
        }
    };

    self.manufacturingDate = ko.observable('').extend({
        required: {
            params: true,
            message: 'Please select year of manufacturing',
            onlyIf: function () {
                return self.validate();
            }
        }
    });

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

        if (!("colorId" in window))
            colorId = 0;

        if (self.errors().length === 0) {
            
            var bdetails = vmSellBike.bikeDetails();
            var pdetails = vmSellBike.personalDetails();
            var inquiryData = {
                "InquiryId": vmSellBike.inquiryId() > 0 ? vmSellBike.inquiryId() : 0,
                "make": {
                    "makeId": bdetails.makeId,
                    "makeName": bdetails.makeName,
                    "maskingName": bdetails.makeMaskingName
                },
                "model": {
                    "modelId": bdetails.modelId,
                    "modelName": bdetails.modelName,
                    "maskingName": null
                },
                "version": {
                    "versionId": bdetails.versionId,
                    "versionName": bdetails.versionName,
                    "modelName": bdetails.modelName,
                    "price": 0,
                    "maskingName": null
                },
                "manufacturingYear": bdetails.manufacturingDate(),
                "kiloMeters": bdetails.kmsRidden(),
                "cityId": bdetails.city(),
                "expectedprice": bdetails.expectedPrice(),
                "owner": bdetails.owner(),
                "registrationPlace": bdetails.registeredCity(),
                "color": bdetails.color(),
                "colorId": colorId,
                "sourceId": 1,
                "status": 1,
                "pageUrl": "used/sell",
                "seller": {
                    "sellerType": sellerType,
                    "customerId": userId > 0 ? userId : 0,
                    "customerName": pdetails.sellerName(),
                    "customerEmail": pdetails.sellerEmail(),
                    "customerMobile": pdetails.sellerMobile()
                },
                "otherInfo": {
                    "registrationNo": "",
                    "insuranceType": "",
                    "adDescription": ""
                }
            }

            $.ajax({
                type: "POST",
                url: "/api/used/sell/listing/",
                contentType: "application/json",
                data: ko.toJSON(inquiryData),
                async: false,
                success: function (response) {
                    var res = JSON.parse(response);
                    if (res != null && res.Status != null && res.Status.Code == 4) {      // if user is not verified
                        vmSellBike.verificationDetails().status(true);
                        vmSellBike.inquiryId(res.InquiryId);
                        vmSellBike.customerId(res.CustomerId);
                    }
                    else if (res != null && res.Status != null && res.Status.Code == 5) {
                        vmSellBike.inquiryId(res.InquiryId);
                        vmSellBike.customerId(res.CustomerId);
                        vmSellBike.formStep(3);
                        vmSellBike.initPhotoUpload();
                    }
                    else {
                        vmSellBike.isFakeCustomer(true);
                    }
                },
                complete: function (xhr, ajaxOptions, thrownError) {

                }
            });

            //scrollToForm.activate();


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

var moreDetails = function () {
    var self = this;

    self.insuranceType = ko.observable();
    self.adDescription = ko.observable();

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

var vmSellBike = new sellBike();
ko.applyBindings(vmSellBike, document.getElementById('sell-bike-content'));

// bike popup
var selectBikeMake = $('#select-make-wrapper'),
    selectBikeModel = $('#select-model-wrapper'),
    selectBikeVersion = $('#select-version-wrapper');

$('#bike-select-element').on('click', '.bike-box-default', function () {
    bikePopup.open();
    appendState('selectBike');
});

$('#close-bike-popup').on('click', function () {
    bikePopup.close();
    history.back();
});

$('#select-model-back-btn').on('click', function () {
    bikePopup.stageMake();
});

$('#select-version-back-btn').on('click', function () {
    bikePopup.stageModel();
});

var bikePopup = {
    container: $('#select-bike-cover-popup'),

    loader: $('.cover-popup-loader-body'),

    makeBody: $('#select-make-wrapper'),

    modelBody: $('#select-model-wrapper'),

    versionBody: $('#select-version-wrapper'),

    open: function () {
        bikePopup.container.show(effect, options, duration, function () {
            bikePopup.container.addClass('extra-padding');
        });
        windowScreen.lock();
    },

    close: function () {
        bikePopup.container.hide(effect, options, duration, function () {
            bikePopup.stageMake();
        });
        bikePopup.container.removeClass('extra-padding');
        windowScreen.unlock();
    },

    stageMake: function () {
        bikePopup.modelBody.hide();
        bikePopup.versionBody.hide();
        bikePopup.makeBody.show();
    },

    stageModel: function () {
        bikePopup.makeBody.hide();
        bikePopup.versionBody.hide();
        bikePopup.modelBody.show();
    },

    stageVersion: function () {
        bikePopup.makeBody.hide();
        bikePopup.modelBody.hide();
        bikePopup.versionBody.show();
    },

    showLoader: function () {
        bikePopup.container.find(bikePopup.loader).show();
    },

    hideLoader: function () {
        bikePopup.container.find(bikePopup.loader).hide();
    },

    scrollToHead: function () {
        $('.cover-window-popup').animate({ scrollTop: 0 });
    }
};

// city
var cityListContainer = $('#city-slideIn-drawer');

$('#city-select-element').on('click', '.city-box-default', function () {
    $('#city-search-box').val("");
    $(citiesList).show();
    cityListSelection.open();
    vmSellBike.bikeDetails().citySelectionStatus('bike-city');
    appendState('bikeCity');
});

$('#registration-select-element').on('click', '.city-box-default', function () {
    $('#city-search-box').val("");
    $(citiesList).show();
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
        windowScreen.lock();
    },

    close: function () {
        cityListContainer.hide(effect, options, duration, function () { });
        cityListContainer.removeClass('fix-header-input');
        windowScreen.unlock();
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
    if (vmSellBike.bikeDetails().versionName() != "") {
        modalPopup.open('#color-popup');
        appendState('colorPopup');
    }
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
        $('.modal-background').show();
        windowScreen.lock();
    },

    close: function (container) {
        $(container).hide();
        $(".modal-background").hide();
        windowScreen.unlock();
    }
};

var windowScreen = {
    htmlElement: $('html'),

    bodyElement: $('body'),

    lock: function () {
        if ($(document).height() > $(window).height()) {
            var windowScrollTop = windowScreen.htmlElement.scrollTop() ? windowScreen.htmlElement.scrollTop() : windowScreen.bodyElement.scrollTop();
            if (windowScrollTop < 0) {
                windowScrollTop = 0;
            }
            windowScreen.htmlElement.addClass('lock-browser-scroll').css('top', -windowScrollTop);
        }
    },

    unlock: function () {
        var windowScrollTop = parseInt(windowScreen.htmlElement.css('top'));
        
        windowScreen.htmlElement.removeClass('lock-browser-scroll');
        $('html, body').scrollTop(-windowScrollTop);
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
    if ($('#select-bike-cover-popup').is(':visible')) {
        bikePopup.close();
    }
});

$('#add-photos-dropzone').on('click', '#add-more-photos', function (event) {
    $('#add-photos-dropzone').trigger('click');
});

var morePhotos = {
    dropzoneDiv: $('#add-photos-dropzone'),

    attach: function () {
        var addPhotosDiv;

        if (!morePhotos.dropzoneDiv.hasClass('dz-under-limit')) {
            addPhotosDiv = '<div id="add-more-photos"><div class="more-photos-content"><span class="sell-bike-sprite plus-icon"></span><br /><span class="font12 text-light-grey">Add photos</span></div></div>';

            morePhotos.dropzoneDiv.addClass('dz-under-limit').append(addPhotosDiv);
        }
    },

    detach: function () {
        if (morePhotos.dropzoneDiv.hasClass('dz-under-limit')) {
            morePhotos.dropzoneDiv.removeClass('dz-under-limit');
            morePhotos.dropzoneDiv.find('#add-more-photos').remove();
        }
    }
};

$('.chosen-container').on('touchstart', function (event) {
    event.stopPropagation();
    event.preventDefault();
    $(this).trigger('mousedown');
}).on('touchend', function (event) {
    event.preventDefault();
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

// Disable Mouse scrolling
$('input[type=number]').on('mousewheel', function (event) { $(this).blur(); });
// Disable keyboard scrolling
$('input[type=number]').on('keydown', function (event) {
    var key = event.charCode || event.keyCode;
    // Disable Up and Down Arrows on Keyboard
    if (key == 38 || key == 40) {
        event.preventDefault();
    } else {
        return;
    }
});

$('#kmsRidden, #expectedPrice').on('keyup', function () {
    var inputBox = $(this),
        inputValue = inputBox.val(),
        withoutCommaValue = inputValue.replace(/,/g, "");

    inputBox.attr('data-value', withoutCommaValue);
    inputBox.val(formatNumber(withoutCommaValue));
});

// input value formatter
var formatNumber = function (num) {
    var thMatch = /(\d+)(\d{3})$/,
        thRest = thMatch.exec(num);

    if (!thRest) return num;
    return (thRest[1].replace(/\B(?=(\d{2})+(?!\d))/g, ",") + "," + thRest[2]);
}
