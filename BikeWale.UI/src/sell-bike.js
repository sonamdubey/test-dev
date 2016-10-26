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
    self.inquiryId = ko.observable();
    self.customerId = ko.observable();
    self.isFakeCustomer = ko.observable(false);
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

    self.modelArray = ko.observableArray();
    self.versionArray = ko.observableArray();
    self.colorArray = ko.observableArray();
    self.makeId = ko.observable();
    self.makeName = ko.observable();
    self.makeMaskingName = ko.observable();
    self.modelId = ko.observable();
    self.modelName = ko.observable();
    self.modelMaskingName = ko.observable();
    self.versionId = ko.observable();
    self.versionName = ko.observable();
    self.versionMaskingName = ko.observable();
    self.colorId = ko.observable();

    self.validate = ko.observable(false);
    self.validateOtherColor = ko.observable(false);

    self.makeChanged = function (data, event) {
        self.modelArray([]);
        self.versionArray([]);
       
        self.makeId = $(event.target).val();
        self.makeName = $(event.target).find(':selected').text();
        self.makeMaskingName = $(event.target).find(':selected').attr("data-masking");
        var blankEntry = { "modelId": -1, "modelName": "", "maskingName": "" };
        
        if (self.make()!=null) {
            $.ajax({
                type: "Get",
                url: "/api/modellist/?requestType=3&makeId=" + self.make(),
                contentType: "application/json",
                dataType: 'json',
                success: function (response) {
                    if (response) {
                        var tempArr = [];
                        tempArr.push(blankEntry);
                        tempArr = tempArr.concat(response.modelList);
                        self.modelArray(tempArr);
                    }
                },
                complete: function (xhr, ajaxOptions, thrownError) {
                    $('#model-select-element.select-box').removeClass('done');
                    $('#version-select-element.select-box').removeClass('done');
                }
            });
        }

    };

    self.modelChanged = function (data, event) {

        self.versionArray([]);

        self.modelId = $(event.target).val();
        self.modelName = $(event.target).find(':selected').text();

        var blankEntry = { "versionId": -1, "versionName": "" };

        if (self.model() != null && self.model() != -1) {
            $.ajax({
                type: "Get",
                url: "/api/versionList/?requestType=3&modelId=" + self.model(),
                contentType: "application/json",
                dataType: 'json',
                success: function (response) {
                    if (response) {
                        var tempArr = [];
                        tempArr.push(blankEntry);
                        tempArr = tempArr.concat(response.Version);
                        self.versionArray(tempArr);
                    }
                },
                complete: function (xhr, ajaxOptions, thrownError) {
                    $('#version-select-element.select-box').removeClass('done');
                }
            });
        }
       
        
        
    };

    self.versionChanged = function (data, event) {

        self.colorArray([]);

        self.versionId = $(event.target).val();
        self.versionName = $(event.target).find(':selected').text();

        if (self.version() != null && self.version() != -1) {
            $.ajax({
                type: "Get",
                url: "/api/version/" + self.version() + "/color/",
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
        colorId = data.colorId;
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

        if (!("colorId" in window))
            colorId = 0;

        var sellerType = $('#seller-type-list .checked').attr("value");

        var km = vmSellBike.bikeDetails().kmsRidden();

        if (self.errors().length === 0) {

            var bdetails = vmSellBike.bikeDetails();
            var pdetails = vmSellBike.personalDetails();

          var inquiryData = {
               "InquiryId": 0,
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
                "manufacturingYear": "2015-01-11T00:00:00",
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
                success: function (response) {
                    var res = JSON.parse(response);                    
                    if (res != null && res.Status != null && res.Status.Code == 4) {      // if user is not verified
                        vmSellBike.verificationDetails().status(true);                     
                        vmSellBike.inquiryId(res.InquiryId);
                        vmSellBike.customerId(res.CustomerId);
                        }
                    else if (res != null && res.Status != null && res.Status.Code == 5) {
                        vmSellBike.formStep(3);
                    }
                    else 
                    {
                        vmSellBike.isFakeCustomer(true);
                    }
                },
                complete: function (xhr, ajaxOptions, thrownError) {

                }
            });
            
            //scrollToForm.activate();
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
            
            scrollToForm.activate();
            
            var otp = vmSellBike.verificationDetails().otpCode();
            var mobile = vmSellBike.personalDetails().sellerMobile();

            $.ajax({
                type: "Post",
                url: "/api/mobileverification/validateotp/?mobile=" + mobile + "&otp=" + otp ,
                contentType: "application/json",
                dataType: 'json',
                success: function (response) {                    
                    if (!response) {
                        $("#otpErrorText").show().text("Please enter correct otp");
                        $('#otpCode').focus();
                    }
                    else {
                        $("#otpErrorText").text("");
                        vmSellBike.formStep(3);
                    }
                },
                complete: function (xhr, ajaxOptions, thrownError) {

                }
            });


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

        var moreDetailsData = {
            "registrationNo" : vmSellBike.moreDetails().registrationNumber(),
            "insuranceType" :  vmSellBike.moreDetails().insuranceType(),
            "adDescription" : vmSellBike.moreDetails().adDescription()
        }

        $.ajax({
            type: "Post",
            url: "/api/used/sell/listing/otherinfo/?inquiryId=" + vmSellBike.inquiryId() + "&customerId=" + vmSellBike.customerId(),
            contentType: "application/json",
            dataType: 'json',
            data: ko.toJSON(moreDetailsData),
            success: function (response) {
                
               
            }
           ,
            complete: function (xhr, ajaxOptions, thrownError) {

            }
        });

        vmSellBike.formStep(4);
        scrollToForm.activate();
    };
    
    self.noThanks = function () {
        vmSellBike.formStep(4);
        scrollToForm.activate();
    };
};

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

    Dropzone.autoDiscover = false;
    var myDropzone = new Dropzone("#add-photos-dropzone", { url: "/file/post" });

    //set year
    //calender.year.set(1980);

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

// year
var calender = {

    year: {
        container: $('#year-list'),

        set: function (startYear) {
            var endYear = new Date().getFullYear(),
                totalWidth = 0,
                count = 0,
                i = startYear,
                totalYear = endYear - startYear + 1,
                item = '';

            for (i; i <= endYear; i++) {
                item += '<span>' + i + '</span>';
                count += 1;
                
                if (count == 5) {
                    calender.year.container.append('<li>'+ item +'</li>');
                    item = '';
                    count = 0;
                }
            }

            console.log(totalYear);

            // arr slice
            //totalWidth = count * 72;
            //calender.year.scrollPosition(totalWidth);
        },

        scrollPosition: function (position) {
            calender.year.container.animate({
                scrollLeft: position20
            });
        }
    }
};

$('.year-prev').on('click', function () {
    var currentPosition = calender.year.container.scrollLeft();
    calender.year.scrollPosition(currentPosition - 298);
});

$('.year-next').on('click', function () {
    var currentPosition = calender.year.container.scrollLeft();
    calender.year.scrollPosition(currentPosition + 298);
});