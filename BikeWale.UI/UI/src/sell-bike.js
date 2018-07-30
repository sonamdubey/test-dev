var selectColorBox = $('#select-color-box'),
    selectCalendarBox = $('#select-calendar-box'),
    calendarErrorBox = $('#calendar-error');

ko.validation.init({
    errorElementClass: 'invalid',
    insertMessages: false
}, true);



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
        var regexMobile = /^[6-9][0-9]{9}$/;

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
    else {
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
    self.photoUploadValidUrl = ko.pureComputed(function () {
        if (self.inquiryId() > 0 && self.personalDetails() && self.personalDetails().sellerTypeVal()) {
            var photoValidUrl = "/api/used/" + self.profileId() + "/image/validate/?isMain=false&extension=";
        }
        return photoValidUrl;
    });

    self.photoUploadUrl = ko.pureComputed(function () {
        if (self.inquiryId() > 0 && self.personalDetails() && self.personalDetails().sellerTypeVal()) {
            var photoUrl = "/api/used/" + self.profileId() + "/image/upload/?isMain=false&extension=";
        }
        return photoUrl;
    });

    self.serverImg = ko.observableArray([]);
    self.initPhotoUpload = function () {
        $('#add-photos-dropzone').dropzone({
            maxFilesize: 4,
            maxFiles: 10,
            addRemoveLinks: true,
            acceptedFiles: ".png, .jpg, .jpeg",
            url: self.photoUploadValidUrl(),
            headers: { "customerId": self.customerId() },
            init: function () {
                var myDropzone = this;                
                myDropzone.itemId = self.inquiryId();
                myDropzone.photoIdGenerateUrl = self.photoUploadUrl();
                myDropzone.customerId = self.customerId();
                myDropzone.profileId = self.profileId();
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
                if (myDropzone.files.length > 0 && myDropzone.files.length < 10) {
                    morePhotos.attach();
                }
                else {
                    morePhotos.detach();
                }

                this.on("sending", function (file) {
                    $(file.previewElement).find('#spinner-content').hide();
                });

                this.on("removedfile", function (file) {
                    morePhotos.detach();
                    if (myDropzone.options.maxFiles < 10)
                        ++myDropzone.options.maxFiles;
                    self.removePhoto($(file._removeLink).attr("photoid"));
                    setProfilePhoto();
                    if (myDropzone.files.length > 0 && myDropzone.files.length < 10) {
                        morePhotos.attach();
                    }
                    else {
                        morePhotos.detach();
                    }
                });

                this.on("success", function (file, response) {
                    var resp = JSON.parse(response);
                    if (resp && resp.imageResult && resp.imageResult.length > 0 && resp.status == 1) {
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
                        morePhotos.detach();
                        myDropzone.addFile(file);
                        if (myDropzone.files.length > 0 && myDropzone.files.length < 10) {
                            morePhotos.attach();
                        }
                    });
                });

                this.on("maxfilesexceeded", function (file) {
                    $(file.previewElement).find('.dz-error-message').text("File upload limit reached");
                });                

                this.on("addedfiles", function (file) {                    
                    morePhotos.detach();
                    if (myDropzone.files.length > 10) {
                        $(file).each(function (i) {                                    
                            if (i >= 10 - self.serverImg().length) {
                                myDropzone.cancelUpload(this);
                                myDropzone.removeFile(this);
                            }
                        });
                    }
                    if (myDropzone.options.maxFiles > 0)
                        myDropzone.options.maxFiles -= file.length;
                    if (myDropzone.files.length > 0 && myDropzone.files.length < 10) {
                        morePhotos.attach();
                    }
                    else {
                        morePhotos.detach();
                    }
                    $.each(myDropzone.getQueuedFiles(), function (index) {
                        triggerGA('Sell_Page', 'Photo_Upload_Initiated', vmSellBike.inquiryId() + '_' + myDropzone.getQueuedFiles()[index].size);
                    })
                });

                this.on("drop", function (file) {                    
                    morePhotos.detach();
                    if (myDropzone.files.length > myDropzone.options.maxFiles) {
                        $(file).each(function (i) {
                            if (10 > i >= self.serverImg().length) {
                                myDropzone.cancelUpload(this);
                                myDropzone.removeFile(this);
                            }
                        });
                    }
                    if (myDropzone.options.maxFiles > 0)
                        myDropzone.options.maxFiles -= file.length;
                    if (myDropzone.files.length > 0 && myDropzone.files.length < 10) {
                        morePhotos.attach();
                    }
                    else {
                        morePhotos.detach();
                    }
                });

                this.on("queuecomplete", function (file) {
                    setProfilePhoto();
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
                    if (response && response.Version.length > 1) {
                        var tempArr = [];
                        tempArr.push(blankEntry);
                        tempArr = tempArr.concat(response.Version);
                        self.versionArray(tempArr);
                        $('#version-select-element.select-box').removeClass('done');
                        $('#version-select-element select').prop('disabled', false).trigger("chosen:updated");
                        $('#version-select-element .chosen-disabled').removeClass('single-version');
                    }
                    else {
                        self.versionArray(response.Version);
                        $('#version-select-element.select-box').addClass('done');
                        $('#version-select-element select').prop('disabled', true).trigger("chosen:updated");
                        $('#version-select-element .chosen-disabled').addClass('single-version');
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

        self.color('');
        $('#select-color-box').removeClass('selection-done');

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
            triggerGA('Sell_Page', 'Step1_Save_and_Continue_Clicked', vmSellBike.bikeDetails().makeName +'_'+ vmSellBike.bikeDetails().modelName + '_' + self.registeredCity());
        }
        else {
            self.errors.showAllMessages();
        }

        scrollToForm.activate();
        vmSellBike.verificationDetails().status(false);
    };

    self.errors = ko.validation.group(self);
    self.colorError = ko.validation.group(self.otherColor);

    self.manufacturingDate = ko.observable('').extend({
        required: {
            params: true,
            message: 'Please select year of manufacturing',
            onlyIf: function () {
                return self.validate();
            }
        }
    });

    self.manufacturingTime = ko.computed(function () {
        var bikeDate = self.manufacturingDate(), // Jan 1980
            monthList = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
            monthName = bikeDate.substr(0, 3),
            yearNumber = bikeDate.substr(4, bikeDate.length - 1),
            monthNumber,
            i;

        for (i = 0; i < 12; i++) {
            if (monthList[i] == monthName) {
                monthNumber = i + 1;
                break;
            }
        }

        return yearNumber + '-' + monthNumber + '-01'; //yyyy-mm-dd
    });
};

var personalDetails = function () {
    var self = this;

    self.validate = ko.observable(false);
    self.mobileLabel = ko.observable(true);
    self.isEdit = ko.observable(false);
    self.termsCheckbox = ko.observable(true).extend({
        validation: [{
            validator: function (val) {
                return val;
            },
            message: 'You must accept the Terms & Conditions to list your bike on BikeWale.',
            onlyIf: function () {
                return self.validate();
            }
        }]
    });

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

    self.sellerTypeVal = ko.observable(2);

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
                "status": vmSellBike.inquiryId() > 0 ? 1 : 4,
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
                        vmSellBike.inquiryId(res.InquiryId);
                        vmSellBike.customerId(res.CustomerId);
                        vmSellBike.verificationDetails().status(true);
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
                    triggerGA('Sell_Page', 'Step2_List_Your_Bike_Clicked', bdetails.makeName + '_' + bdetails.modelName + '_' + bdetails.registeredCity() + '_' + res.InquiryId);
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
        $('#otpCode').val("");
        if (self.errorMobile().length === 0) {
            self.updateMobileStatus(false);
            vmSellBike.personalDetails().sellerMobile(self.updatedMobile());
            self.otpCode('');
            $('#otpCode').focus();
            self.validateOTP(false);
            scrollToForm.activate();
            vmSellBike.personalDetails().listYourBike();
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
            var mobileVerificationData = {
                "sellerType": vmSellBike.personalDetails().sellerTypeVal() ,
                "otp": otp,
                "customerMobile": mobile,
                "customerId": vmSellBike.customerId(),
                "inquiryId": vmSellBike.inquiryId(),
                "isEdit"   : isEdit
            }

            $.ajax({
                type: "Post",
                url: "/api/used/sell/listing/verifymobile/",
                contentType: "application/json",
                data: ko.toJSON(mobileVerificationData),
                dataType: 'json',
                success: function (response) {
                    if (!response) {
                        $("#otpErrorText").show().text("Please enter correct otp");
                        $('#otpCode').focus();
                    }
                    else {
                        $("#otpErrorText").text("");
                        vmSellBike.formStep(3);                        
                        vmSellBike.initPhotoUpload();
                        triggerGA('Sell_Page', 'Step2_Verification_Successful', vmSellBike.bikeDetails().makeName + '_' + vmSellBike.bikeDetails().modelName + '_' + vmSellBike.bikeDetails().registeredCity() + '_' + vmSellBike.inquiryId());
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
        var pdetails = vmSellBike.personalDetails();
        var moreDetailsData = {
            "registrationNo": vmSellBike.moreDetails().registrationNumber(),
            "insuranceType": vmSellBike.moreDetails().insuranceType(),
            "adDescription": vmSellBike.moreDetails().adDescription() ? vmSellBike.moreDetails().adDescription().replace(/\s/g, ' ') : '',
            "seller": {
                "sellerType": vmSellBike.personalDetails().sellerTypeVal(),
                "customerId": userId > 0 ? userId : 0,
                "customerName": pdetails.sellerName(),
                "customerEmail": pdetails.sellerEmail(),
                "customerMobile": pdetails.sellerMobile()
            }
        };
        $.ajax({
            type: "Post",
            url: "/api/used/sell/listing/otherinfo/?inquiryId=" + vmSellBike.inquiryId() + "&customerId=" + vmSellBike.customerId(),
            contentType: "application/json",
            dataType: 'json',
            data: ko.toJSON(moreDetailsData),
            success: function (response) {
                if (response && response.Status && response.Status.Code) {
                    switch (response.Status.Code) {
                        case 1:
                            vmSellBike.formStep(4);
                            scrollToForm.activate();
                            break;
                        default:
                            vmSellBike.formStep(2);
                            scrollToForm.activate();
                    }
                }
                else {
                    vmSellBike.formStep(2);
                    scrollToForm.activate();
                }
            }
           ,
            complete: function (xhr, ajaxOptions, thrownError) {

            }
        });
        triggerGA('Sell_Page', 'Step3_Update_My_Ad_Clicked', vmSellBike.bikeDetails().makeName + '_' + vmSellBike.bikeDetails().modelName + '_' + vmSellBike.bikeDetails().registeredCity() + '_' + vmSellBike.inquiryId());
    };

    self.noThanks = function () {
        vmSellBike.formStep(4);
        scrollToForm.activate();
        triggerGA('Sell_Page', 'Step3_No_Thanks_Clicked', vmSellBike.bikeDetails().makeName + '_' + vmSellBike.bikeDetails().modelName + '_' + vmSellBike.bikeDetails().registeredCity() + '_' + vmSellBike.inquiryId());
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

    var monthList = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
        currentDate = new Date(),
        currentMonth = currentDate.getMonth(),
        currentYear = currentDate.getFullYear(),
        manufacturingDateInput = $('#manufacturingDate');

    manufacturingDateInput.Zebra_DatePicker({
        format: 'M Y',
        direction: ['Jan 1980', monthList[currentMonth] + ' ' + currentYear],
        start_date: monthList[currentMonth] + ' ' + currentYear,
        onOpen: function () {
            var positionLeft = manufacturingDateInput.offset().left,
                manufacturingDatePicker = manufacturingDateInput.data('Zebra_DatePicker');

            manufacturingDatePicker.datepicker.css({
                'top': manufacturingDatePicker.datepicker.offset().top - 4,
                'left': positionLeft
            });
        },
        onSelect: function () {
            vmSellBike.bikeDetails().manufacturingDate($(this).val());
        }
    });

    // set custom heading for date picker
    var manufacturingDatePicker = manufacturingDateInput.data('Zebra_DatePicker');
    manufacturingDatePicker.datepicker.find('.dp_heading').text('Year of manufacturing');
    var obj = GetGlobalLocationObject();
    if (obj != null) {
        $('#city-select-element select').val(obj.CityId).trigger("chosen:updated");
        $('#city-select-element').addClass('done');
        vmSellBike.bikeDetails().city(obj.CityId);
    }
});

$('#add-photos-dropzone').on('click', '#add-more-photos', function (event) {
    $('#add-photos-dropzone').trigger('click');
});

var setProfilePhoto = function () {
    var container = $('#add-photos-dropzone .dz-preview.dz-success').first();
    if (!container.hasClass('dz-profile-photo')) {
        container.addClass('dz-profile-photo');
        container.append('<div id="profile-photo-content"><span class="sell-bike-sprite ribbon-icon"></span><span class="ribbon-label">Profile photo</span></div>')
        vmSellBike.markMainImage($(container).find(".dz-remove").attr("photoid"));
    }
};

function setRemoveLinkUrl(file, imageResult) {
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


$(function () {
    if (isEdit == "True") {
        if (isAuthorized == "False") {
            vmSellBike.isFakeCustomer(true);
        }
        inquiryDetails = JSON.parse(inquiryDetailsJSON);
        var bdetails = vmSellBike.bikeDetails();
        var pdetails = vmSellBike.personalDetails();
        var mdetails = vmSellBike.moreDetails();
       
        pdetails.sellerName(inquiryDetails.seller.customerName);
        pdetails.sellerEmail(inquiryDetails.seller.customerEmail);

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
        
        // set manufacture date
        var monthArr = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
            bikeManufactureDate = new Date(inquiryDetails.manufacturingYear),
            manufactureYear = bikeManufactureDate.getFullYear(),
            manufactureMonth = bikeManufactureDate.getMonth(),
            manufactureMonthName = monthArr[manufactureMonth];

        bdetails.manufacturingDate(manufactureMonthName + ' ' + manufactureYear);
        $("#manufacturingDate").val(manufactureMonthName + ' ' + manufactureYear).data('Zebra_DatePicker');

        $("#select-registeredCity").trigger("change").trigger("chosen:updated");
        pdetails.sellerTypeVal(inquiryDetails.seller.sellerType);
        pdetails.sellerName(inquiryDetails.seller.customerName);
        pdetails.sellerEmail(inquiryDetails.seller.customerEmail);
        pdetails.sellerMobile(inquiryDetails.seller.customerMobile);
        pdetails.sellerType(inquiryDetails.seller.sellerType);
        pdetails.isEdit(true);
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
        if (window.location.hash == "#uploadphoto" || window.location.search.indexOf("hash") > -1)
        {
            vmSellBike.formStep(3);
            vmSellBike.initPhotoUpload();
        }

        $('#btnSaveBikeDetails').val("Update and Continue");
        $('#btnListBike').val("Update and Continue");
        $('#btnUpdateAd').val("Update my listing");
        $('#btnEditAd').hide();
    }
    else {
        $("#kmsRidden").val('');
        $("#expectedPrice").val('');
        if ($('#city-select-element select').val() == '') {
            $('#city-select-element').removeClass('done');
        }
    }
    if (isEdit != "True" &&  userId != null) {
        var pdetails = vmSellBike.personalDetails();
        pdetails.sellerName(userName);
        pdetails.sellerEmail(userEmail);
    }  
    
});

$('.accordion-list').on('click', '.accordion-head', function () {
    var element = $(this);

    if (!element.hasClass('active')) {
        accordion.open(element);
    }
    else {
        accordion.close(element);
    }
});

var accordion = {
    open: function (element) {
        var elementSiblings = element.closest('.accordion-list').find('.accordion-head.active');
        elementSiblings.removeClass('active').next('.accordion-body').slideUp();

        element.addClass('active').next('.accordion-body').slideDown();
    },

    close: function (element) {
        element.removeClass('active').next('.accordion-body').slideUp();
    }
};