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

var congratsScreenDoneFunction = function () {
    window.location = "/mybikewale/";
};

var editMyAd = function () {
    vmSellBike.formStep(1);
};

var sellBike = function () {
    var self = this;
    self.inquiryId = ko.observable(0);
    self.customerId = ko.observable();
    self.profileId = ko.pureComputed(function getProfileId() {
       return ((self.personalDetails().sellerTypeVal() == 1 ? "D" : "S") + self.inquiryId());
    }, this);
    if (isAuthorized == "False") {
        self.isFakeCustomer = ko.observable(true);
    }
    else
    {
        self.isFakeCustomer = ko.observable(false);
    }

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

    self.photoUploadUrl = ko.pureComputed(function () {
        if (self.inquiryId() > 0 && self.personalDetails() && self.personalDetails().sellerTypeVal()) {
            var photoUrl = self.profileId() + "/image/upload/?isMain=false";
        }
        return photoUrl;
    }, this)

    self.serverImg = ko.observableArray([]);    
    self.initPhotoUpload = function () {
        $('#add-photos-dropzone').dropzone({
            maxFilesize: 4,
            maxFiles: 10,
            addRemoveLinks: true,
            acceptedFiles: ".png, .jpg",
            url: "/api/used/" + self.photoUploadUrl(),
            headers: { "customerId": self.customerId() },
            init: function () {
                var myDropzone = this;                                
                $(self.serverImg()).each(function (i) {                    
                    var uF = { name: this.id, size: 12345 };
                    myDropzone.files.push(uF)
                    myDropzone.emit("addedfile", uF);
                    myDropzone.emit("thumbnail", uF, this.imageUrl);
                    myDropzone.createThumbnailFromUrl(uF, this.imageUrl);
                    myDropzone.emit("complete", uF);
                    setProfilePhoto();                    
                    $(myDropzone.files[i].previewElement).addClass("dz-success").find("#spinner-content").hide();
                    $(myDropzone.files[i].previewElement).addClass("dz-success").find(".dz-success-mark").hide();
                    $(myDropzone.files[i].previewElement).find(".dz-remove").attr("photoid", this.id);
                });
                myDropzone.options.maxFiles -= self.serverImg().length;                

                this.on("sending", function (file) {
                    $(file.previewElement).find('#spinner-content').hide();
                });

                this.on("removedfile", function (file) {
                    self.removePhoto($(file._removeLink).attr("photoid"));
                    setProfilePhoto();
                });

                this.on("success", function (file,response) {
                    var resp = JSON.parse(response);
                    setProfilePhoto();
                    if(resp && resp.imageResult && resp.imageResult.length > 0 && resp.status == 1)
                    {                        
                        setRemoveLinkUrl(file, resp.imageResult);
                    }
                });

                this.on("error", function (file, response) {                    
                    $(file.previewElement).find('#spinner-content').hide();
                    if (file.xhr && file.xhr.status == 0)
                        $(file.previewElement).find('.dz-error-message').text("You're offline.");
                    else
                        $(file.previewElement).find('.dz-error-message').text(response);
                    $(file.previewElement).find('.dz-error-mark').on('click', function () {
                        myDropzone.removeFile(file);
                        myDropzone.addFile(file);
                    });
                });

                this.on("maxfilesexceeded", function (file) {                                        
                    $(file.previewElement).find('.dz-error-message').text("File upload limit reached");
                });

                this.on("addedfiles", function (file) {                    
                    if (file.length > myDropzone.options.maxFiles) {
                        $(file).each(function (i) {                            
                            if (i >= self.serverImg().length) {
                                myDropzone.cancelUpload(this);
                                myDropzone.removeFile(this);
                            }
                        });
                    }
                });
            }
        });
    }
    self.removePhoto = function removeUploadedPhoto(photoId) {
        var isSuccess = false;
        if (photoId) {
            try {
                $.ajax({
                    type: "POST",
                    async: false,
                    url: "/api/used/" + self.profileId() + "/image/" + photoId + "/delete/",
                    contentType: "application/json",
                    dataType: 'json',
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader('platformid', 1);
                        xhr.setRequestHeader('customerId', self.customerId());
                    },
                    success: function (response) {
                        
                    },
                    complete: function (xhr, ajaxOptions, thrownError) {
                        if (xhr && xhr.status == 4) {
                            isSuccess = true;
                        }
                        else {
                            isSuccess = false;
                        }
                    }
                });

            } catch (e) {
                isSuccess = false;
            }
        }
        return isSuccess;
    }
    self.markMainImage = function (photoId) {
        if (photoId && self.inquiryId()) {
            try {
                $.ajax({
                    type: "POST",
                    async: false,
                    url: "/api/used/" + self.profileId() + "/image/" + photoId + "/markmainimage/",
                    contentType: "application/json",
                    dataType: 'json',
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader('platformid', 1);
                        xhr.setRequestHeader('customerId', self.customerId());
                    },
                    success: function (response) {

                    },
                    complete: function (xhr, ajaxOptions, thrownError) {

                    }
                });
            } catch (e) {

            }
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
        
        if (self.make() != null) {
            $.ajax({
                type: "Get",
                async: false,
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
                    $('#model-select-element select').prop('disabled', false).trigger("chosen:updated");

                    if (isEdit == "True") {
                        self.model(inquiryDetails.model.modelId);
                        $("#model-select-element select").trigger("change").trigger("chosen:updated");
                    }
                }
            });
        }

    };

    self.modelChanged = function (data, event) {
        self.versionArray([]);        

        self.modelId = $(event.target).val() ? $(event.target).val() : self.model();
        self.modelName = $(event.target).find(':selected').text();

       

        var blankEntry = { "versionId": -1, "versionName": "" };

        if (self.model() != null && self.model() != -1) {
            $.ajax({
                type: "Get",
                url: "/api/versionList/?requestType=3&modelId=" + self.model(),
                contentType: "application/json",
                dataType: 'json',
                success: function (response) {
                    if(response.Version.length > 1){
                    if (response) {
                        var tempArr = [];
                        tempArr.push(blankEntry);
                        tempArr = tempArr.concat(response.Version);
                        self.versionArray(tempArr);
                        $('#version-select-element.select-box').removeClass('done');
                        $('#version-select-element select').prop('disabled', false).trigger("chosen:updated");
                    }
                    } else
                    {
                        self.versionArray(response.Version);                        
                        $('#version-select-element.select-box').addClass('done');
                        $('#version-select-element select').prop('disabled', true).trigger("chosen:updated");
                    }
                },
                complete: function (xhr, ajaxOptions, thrownError) {
                    if (isEdit == "True") {
                        self.version(inquiryDetails.version.versionId);
                        $("#version-select-element select").trigger("change").trigger("chosen:updated");

                    }
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

                    if (isEdit == "True") {
                        self.color(inquiryDetails.color);
                        $("#select-color-box select").trigger("change").trigger("chosen:updated");
                        $('#select-color-box').addClass('selection-done');
                    }
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

    self.manufacturingTime = ko.computed(function () {
        return self.manufactureYear() + '-' + self.manufactureMonth() + '-01';
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
        if(event != null) {
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

    self.sellerTypeVal = ko.observable(0);

    self.listYourBike = function () {        
        self.validate(true);

        if (!("colorId" in window))
            colorId = 0;

        var sellerType = $('#seller-type-list .checked').attr("value");        
        self.sellerTypeVal(sellerType);
        var km = vmSellBike.bikeDetails().kmsRidden();

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
                "manufacturingYear": bdetails.manufacturingTime(),
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
            "registrationNo": vmSellBike.moreDetails().registrationNumber(),
            "insuranceType" :  vmSellBike.moreDetails().insuranceType(),
            "adDescription" : vmSellBike.moreDetails().adDescription().replace(/\s/g,' ')
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
    //set year
    calender.year.set(1980);
    calender.month.set();
});

function setPhotoId() {
    
}

var setProfilePhoto = function () {
    var container = $('#add-photos-dropzone .dz-preview.dz-success').first();    
    if (!container.hasClass('dz-profile-photo')) {
        container.addClass('dz-profile-photo');
        container.append('<div id="profile-photo-content"><span class="sell-bike-sprite ribbon-icon"></span><span class="ribbon-label">Profile photo</span></div>')        
        vmSellBike.markMainImage($(container).find(".dz-remove").attr("photoid"));
    }
};

function setRemoveLinkUrl(file,imageResult) {
    var container = $('#add-photos-dropzone .dz-preview.dz-success');
    var existingPhotoCount = $('#add-photos-dropzone .dz-preview.dz-success[photoid]').length;    
    $(file._removeLink).attr("photoid", imageResult[0].photoId);
}

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



$(function () {
    if (isEdit == "True") {
        if (isAuthorized == "False") {            
            vmSellBike.isFakeCustomer(true);
        }        
        inquiryDetails = JSON.parse(inquiryDetails);
        var bdetails = vmSellBike.bikeDetails();
        var pdetails = vmSellBike.personalDetails();
        var mdetails = vmSellBike.moreDetails();
        bdetails.make(inquiryDetails.make.makeId);
        bdetails.version(inquiryDetails.version.versionId);
        bdetails.kmsRidden(inquiryDetails.kiloMeters);
        bdetails.city(inquiryDetails.cityId);
        bdetails.owner(inquiryDetails.owner);
        bdetails.registeredCity(inquiryDetails.registrationPlace);
        bdetails.expectedPrice(inquiryDetails.expectedprice);
        $("#div-kmsRidden").addClass('not-empty');
        $("#div-expectedPrice").addClass('not-empty');
        bdetails.colorId(inquiryDetails.colorId);        
        bdetails.manufactureYear((new Date(inquiryDetails.manufacturingYear)).getFullYear());
        bdetails.manufactureMonth((new Date(inquiryDetails.manufacturingYear)).getMonth() + 1);       
        $("#select-registeredCity").trigger("change").trigger("chosen:updated");
        var monthArr = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
        var month = (new Date(inquiryDetails.manufacturingYear)).getMonth();
        bdetails.manufactureMonthName(monthArr[month]);
        $("#select-calendar-box").addClass('selection-done');
        pdetails.sellerType(inquiryDetails.seller.sellerType);
        pdetails.sellerName(inquiryDetails.seller.customerName);
        pdetails.sellerEmail(inquiryDetails.seller.customerEmail);
        pdetails.sellerMobile(inquiryDetails.seller.customerMobile);
        pdetails.sellerTypeVal(inquiryDetails.seller.sellerType);
        vmSellBike.inquiryId(inquiryDetails.InquiryId);
        vmSellBike.customerId(inquiryDetails.seller.customerId);
        mdetails.adDescription(inquiryDetails.otherInfo.adDescription);
        mdetails.registrationNumber(inquiryDetails.otherInfo.registrationNo);
        mdetails.insuranceType(inquiryDetails.otherInfo.insuranceType);        
        $("#select-insuranceType").trigger("change").trigger("chosen:updated");

        $('#model-select-element select').prop('disabled', true).trigger("chosen:updated");
        $('#make-select-element select').prop('disabled', true).trigger("chosen:updated");
        $('#city-select-element select').prop('disabled', true).trigger("chosen:updated");

        vmSellBike.serverImg(inquiryDetails.photos);
        if (window.location.hash == "#uploadphoto")
            vmSellBike.formStep(3);
    }    
    if(userId != null)
    {
        var pdetails = vmSellBike.personalDetails();
        pdetails.sellerName(userName);
        pdetails.sellerEmail(userEmail);
    }
});