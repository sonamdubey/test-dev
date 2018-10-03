var vmUnansweredQuestions = null;
var platformId = 2;
var returnUrl = "/mybikewale";
var makeName = "";
var modelName = "";
var pageSrcId = 9;

var sellLoader = {
    open: function () {
        $('html, body').addClass('lock-browser-scroll loader-active');
    },

    close: function () {
        $('html, body').removeClass('lock-browser-scroll loader-active');
    }
}

var citiesList = $("#filter-city-list li");
$("section").show();

$('.chosen-select').chosen();

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
};

var monthList = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

$(document).ready(function () {

    try {

        vmSellBike = new sellBike();
        vmUnansweredQuestions = new unansweredQuestions();

        if (isEdit != "True" && userId != null) {
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

        var currentDate = new Date(),
            currentMonth = currentDate.getMonth(),
            currentYear = currentDate.getFullYear(),
            manufacturingDateInput = $('#manufacturingDate');

        manufacturingDateInput.Zebra_DatePicker({
            format: 'M Y',
            direction: ['Jan 1980', monthList[currentMonth] + ' ' + currentYear],
            start_date: monthList[currentMonth] + ' ' + currentYear,
            onSelect: function () {
                vmSellBike.bikeDetails().manufacturingDate($(this).val());
            }
        });

        // set custom heading for date picker
        var manufacturingDatePicker = manufacturingDateInput.data('Zebra_DatePicker');
        manufacturingDatePicker.datepicker.find('.dp_heading').text('Year of manufacturing');

        Dropzone.autoDiscover = false;

        var obj = GetGlobalLocationObject();
        if (obj != null) {
            vmSellBike.bikeDetails().cityId(obj.CityId);
            vmSellBike.bikeDetails().city(obj.CityName);
        }

        var inquiryDetails = JSON.parse(inquiryDetailsJSON);

        if (isEdit == "True") {

            var bdetails = vmSellBike.bikeDetails();
            var pdetails = vmSellBike.personalDetails();
            var mdetails = vmSellBike.moreDetails();

            pdetails.sellerName(inquiryDetails.seller.customerName);
            pdetails.sellerEmail(inquiryDetails.seller.customerEmail);

            bdetails.makeName(inquiryDetails.make.makeName);
            bdetails.makeId(inquiryDetails.make.makeId);

            bdetails.modelName(inquiryDetails.model.modelName);
            bdetails.modelId(inquiryDetails.model.modelId);

            bdetails.versionName(inquiryDetails.version.versionName);
            bdetails.versionId(inquiryDetails.version.versionId);

            bdetails.color(inquiryDetails.color);
            bdetails.colorId(inquiryDetails.colorId);

            bdetails.prevColor(bdetails.color());
            bdetails.prevColorId(bdetails.colorId());

            bdetails.cityId(inquiryDetails.cityId);
            bdetails.city(findCityName(bdetails.cityId()));
            bdetails.registeredCity(inquiryDetails.registrationPlace);

            bdetails.kmsRidden(inquiryDetails.kiloMeters);
            $('#kmsRidden').attr('data-value', inquiryDetails.kiloMeters);
            bdetails.kmsRidden(formatNumber(inquiryDetails.kiloMeters));

            bdetails.owner(inquiryDetails.owner);
            bdetails.expectedPrice(inquiryDetails.expectedprice);
            $('#expectedPrice').attr('data-value', inquiryDetails.expectedprice);
            bdetails.expectedPrice(formatNumber(inquiryDetails.expectedprice));

            $('#div-kmsRidden').addClass('not-empty');
            $('#div-expectedPrice').addClass('not-empty');
            $("#div-owner").addClass('done');
            selectColorBox.addClass('selection-done');
            $("#city-select-p").css('color', 'grey');
            $("#bike-select-p").css('color', 'grey');
            bdetails.versionChanged();

            // set manufacture date
            var monthArr = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
                bikeManufactureDate = new Date(inquiryDetails.manufacturingYear),
                manufactureYear = bikeManufactureDate.getFullYear(),
                manufactureMonth = bikeManufactureDate.getMonth(),
                manufactureMonthName = monthArr[manufactureMonth];

            bdetails.manufacturingDate(manufactureMonthName + ' ' + manufactureYear);
            $("#manufacturingDate").val(manufactureMonthName + ' ' + manufactureYear).data('Zebra_DatePicker');

            pdetails.sellerTypeVal(inquiryDetails.seller.sellerType);
            pdetails.sellerName(inquiryDetails.seller.customerName);
            pdetails.sellerEmail(inquiryDetails.seller.customerEmail);
            pdetails.sellerMobile(inquiryDetails.seller.customerMobile);
            pdetails.sellerType(inquiryDetails.seller.sellerType);
            vmSellBike.isEdit(true);

            vmSellBike.inquiryId(inquiryDetails.InquiryId);
            vmSellBike.customerId(inquiryDetails.seller.customerId);

            mdetails.adDescription(inquiryDetails.otherInfo.adDescription);
            mdetails.registrationNumber(inquiryDetails.otherInfo.registrationNo);
            mdetails.insuranceType(inquiryDetails.otherInfo.insuranceType);
            $("#select-insuranceType").val(inquiryDetails.otherInfo.insuranceType);
            $("#select-insuranceType").trigger("chosen:updated");
            $("#div-insuranceType").addClass("done");

            $('#city-select-element').prop('disabled', true);

            vmSellBike.serverImg(inquiryDetails.photos);
            if (window.location.hash == "#uploadphoto" || window.location.search.indexOf("hash") > -1) {
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
            $("#manufacturingDate").val('');
            $("#div-owner").removeClass('done');
            if (cookieCityId && cookieCityName) {
                vmSellBike.bikeDetails().cityId(cookieCityId);
                vmSellBike.bikeDetails().city(cookieCityName);
            }
        }
        ko.applyBindings(vmSellBike, document.getElementById('sell-bike-content'));
        ko.applyBindings(vmUnansweredQuestions, document.getElementById("answer-question-wrapper"));
        sellLoader.close();
        $("section").show();
    } catch (e) {
        console.warn(e);
    }
    sellLoader.close();
});


var findCityName = function (cityId) {
    try {
        for (i = 0; i < 3000; i++) {
            if ($(citiesList[i]).attr('data-cityid') == cityId)
                return ($(citiesList[i]).attr('data-cityname'));
        }
    } catch (e) {
        console.warn(e);
    }
}

var vmCities = function () {
    try {
        var self = this;
        self.cityFilter = ko.observable('');
        self.FilteredCity = ko.observableArray([]);
        self.visibleCities = ko.computed(function () {
            self.FilteredCity([]);
            var filter = self.cityFilter();
            if (filter && filter.length > 0) {
                var pat = new RegExp(filter, "i");
                ko.utils.arrayFilter(citiesList, function (city) {
                    if (pat.test($(city).text())) {
                        self.FilteredCity().push({ city: $(city).text(), id: $(city).attr("data-cityId") });
                    };
                });
            }
            return self.FilteredCity();
        });
    } catch (e) {
        console.warn(e);
    }
}




// custom validation function
var validation = {
    greaterThanOne: function (val) {
        val = val.toString();
        val = val.replace(/,/g, "");
        if (val < 1) {
            return false;
        }
        return true;
    },

    kmsMaxValue: function (val) {
        val = val.toString();
        val = val.replace(/,/g, "");
        if (val > 999999) {
            return false;
        }
        return true;
    },

    priceMaxValue: function (val) {
        val = val.toString();
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
        var regexMobile = /^[6-9][0-9]{9}$/;

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
        val = val.toString();
        val = val.replace(/,/g, "");
        if (isNaN(val)) {
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

var setProfilePhoto = function () {
    try {
        var container = $('#add-photos-dropzone .dz-preview.dz-success').first();
        if (!container.hasClass('dz-profile-photo')) {
            container.addClass('dz-profile-photo');
            container.append('<div id="profile-photo-content"><span class="sell-bike-sprite ribbon-icon"></span><span class="ribbon-label">Profile photo</span></div>')
            vmSellBike.markMainImage($(container).find(".dz-remove").attr("photoid"));
        }
    } catch (e) {
        console.warn(e);
    }
};

function setRemoveLinkUrl(file, imageResult) {
    try {
        var container = $('#add-photos-dropzone .dz-preview.dz-success');
        var existingPhotoCount = $('#add-photos-dropzone .dz-preview.dz-success[photoid]').length;
        $(file._removeLink).attr("photoid", imageResult[0].photoId);
    } catch (e) {
        console.warn(e);
    }
}

$('#add-photos-dropzone').on('click', '#add-more-photos', function (event) {
    $('#add-photos-dropzone').trigger('click');
});

var sellBike = function () {
    var self = this;

    self.isPhotoQueued = ko.observable(false);
    self.isDropzoneInitiated = ko.observable(false);
    self.isEdit = ko.observable(false);
    self.isFakeCustomer = ko.observable(false);
    self.inquiryId = ko.observable();
    self.customerId = ko.observable();
    self.profileId = ko.pureComputed(function getProfileId() {
        return ((self.personalDetails().sellerTypeVal() == 1 ? "D" : "S") + self.inquiryId());
    }, this);

    self.formStep = ko.observable(1); // TODO: Change the value from 4 to 1

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
        try {
            if (!self.isDropzoneInitiated()) {
                self.isDropzoneInitiated(true);
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
                            self.isPhotoQueued(true);
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
                                $(file.previewElement).find('.dz-error-message').text("Upload limit reached");
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
                            $(file.previewElement).find('.dz-error-message').text("Upload limit reached");
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

                        this.on("complete", function (file) {
                            self.isPhotoQueued(false);
                        });
                    }
                });
            }
        } catch (e) {
            console.warn(e);
        }
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
    self.cityId = ko.observable('');
    self.regCityId = ko.observable('');

    self.prevColor = ko.observable();
    self.prevColorId = ko.observable();

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

        //bikePopup.showLoader()
        var element = $(event.currentTarget);

        self.modelName('');
        self.versionName('');


        self.makeName(element.text());
        self.makeId(element.attr("data-id"));
        self.makeMaskingName(element.attr("data-makeMasking"));

        bikePopup.stageModel();
        bikePopup.scrollToHead();

        try {
            if (self.makeId()) {
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
                        //bikePopup.hideLoader()
                    }
                });
            }
        } catch (e) {
            console.warn(e);
        }

        self.bikeStatus(false);

    };

    self.modelChanged = function (data, event) {

        if (isEdit != "True") {
            self.modelName(data.modelName);
            modelName = data.modelName;
            self.modelId(data.modelId);
            self.modelMaskingName(data.maskingName);
        }

        bikePopup.stageVersion();
        bikePopup.scrollToHead();

        try {
            if (self.modelId()) {
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
        } catch (e) {
            console.warn(e);
        }

    };

    self.versionChanged = function (data, event) {

        if (data) {
            self.versionName(data.versionName);
            self.versionId(data.versionId);
        }

        self.bikeStatus(true);

        bikePopup.close();

        try {
            if (self.versionId()) {
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
        } catch (e) {
            console.warn(e);
        }
    };

    self.bike = ko.pureComputed(
    function () {
        if (self.bikeStatus()) {
            return self.makeName() + ' ' + self.modelName() + ' ' + self.versionName();
        }
        return "";
    }, this).extend({
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

    self.cancelColorPopup = function (data, event) {
        self.color(self.prevColor());
        self.colorId(self.prevColorId());
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

    self.manufacturingTime = ko.computed(function () {
        var bikeDate = self.manufacturingDate(), // Jan 1980
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

    self.saveBikeDetails = function (data, event) {
        self.validate(true);

        if (self.errors().length === 0) {
            vmSellBike.formStep(2);
            scrollToForm.activate();
            triggerGA('Sell_Page', 'Step1_Save_and_Continue_Clicked', vmSellBike.bikeDetails().makeName() + '_' + vmSellBike.bikeDetails().modelName() + '_' + self.registeredCity());
        }
        else {
            self.errors.showAllMessages();
            $('#sell-bike-content input.invalid , #sell-bike-content p.invalid').first().focus();
        }

        vmSellBike.verificationDetails().status(false);
    };

    self.closeBikePopup = function (data, event) {
        if (vmSellBike.isEdit()) {
            self.bikeStatus(true);
        }
        bikePopup.close();
        history.back();
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
            message: 'You must accept the Terms & Conditions to list your bike on BikeWale.',
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
        $('#otpCode').val("");

        self.validate(true);

        try {
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
                    "manufacturingYear": bdetails.manufacturingTime(),
                    "kiloMeters": bdetails.kmsRidden().toString().replace(/,/g, ""),
                    "cityId": bdetails.cityId(),
                    "expectedprice": bdetails.expectedPrice().toString().replace(/,/g, ""),
                    "owner": bdetails.owner(),
                    "registrationPlace": bdetails.registeredCity(),
                    "color": bdetails.color(),
                    "colorId": colorId,
                    "sourceId": 2,
                    "status": vmSellBike.inquiryId() > 0 ? 1 : 4,
                    "pageUrl": "used/sell",
                    "seller": {
                        "sellerType": pdetails.sellerTypeVal(),
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
                        scrollToForm.activate();
                        triggerGA('Sell_Page', 'Step2_List_Your_Bike_Clicked', bdetails.makeName() + '_' + bdetails.modelName() + '_' + bdetails.registeredCity() + '_' + res.InquiryId);
                    },
                    complete: function (xhr, ajaxOptions, thrownError) {
                        if (xhr.status != 200)
                            alert("Something went wrong!! Please try again.");
                        vmUnansweredQuestions.makeName(bdetails.makeName());
                        vmUnansweredQuestions.modelName(bdetails.modelName());
                        vmUnansweredQuestions.bindQuestions(bdetails.modelId(), inquiryData.seller.customerName, inquiryData.seller.customerEmail, returnUrl, platformId, pageSrcId);
                    }
                });
            }
            else {
                self.errors.showAllMessages();
            }

            if (self.mobileError().length != 0) {
                self.mobileLabel(false);
            }
        } catch (e) {
            console.warn(e);
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
            vmSellBike.personalDetails().listYourBike();
        }
        else {
            self.errorMobile.showAllMessages();
        }

        scrollToForm.activate();
    };

    self.verifySeller = function () {
        self.validateOTP(true);

        try {
            if (self.errorOTP().length === 0) {
                scrollToForm.activate();

                var otp = vmSellBike.verificationDetails().otpCode();
                var mobile = vmSellBike.personalDetails().sellerMobile();

                var mobileVerificationData = {
                    "sellerType": vmSellBike.personalDetails().sellerTypeVal(),
                    "otp": otp,
                    "customerMobile": mobile,
                    "customerId": vmSellBike.customerId(),
                    "inquiryId": vmSellBike.inquiryId(),
                    "isEdit": isEdit
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
                            triggerGA('Sell_Page', 'Step2_Verification_Successful', vmSellBike.bikeDetails().makeName() + '_' + vmSellBike.bikeDetails().modelName() + '_' + vmSellBike.bikeDetails().registeredCity() + '_' + vmSellBike.inquiryId());
                        }
                    },
                    complete: function (xhr, ajaxOptions, thrownError) {

                    }
                });
            }
            else {
                self.errorOTP.showAllMessages();
            }
        } catch (e) {
            console.warn(e);
        }
    };

    self.errorOTP = ko.validation.group(self.otpCode);
    self.errorMobile = ko.validation.group(self.updatedMobile);

};

var moreDetails = function () {
    var self = this;

    self.insuranceType = ko.observable();
    self.adDescription = ko.observable('');
    self.registrationNumber = ko.observable('');

    try {
        self.updateAd = function () {
            var pdetails = vmSellBike.personalDetails();
            if ((!vmSellBike.isPhotoQueued()) || confirm("Photos are still being uploaded. Are you sure you want to abort photo upload?")) {

                var moreDetailsData = {
                    "registrationNo": vmSellBike.moreDetails().registrationNumber().trim() ? vmSellBike.moreDetails().registrationNumber() : '',
                    "insuranceType": vmSellBike.moreDetails().insuranceType(),
                    "adDescription": vmSellBike.moreDetails().adDescription().trim() ? vmSellBike.moreDetails().adDescription().replace(/\s/g, ' ') : '',
                    "seller": {
                        "sellerType": vmSellBike.personalDetails().sellerTypeVal(),
                        "customerId": userId > 0 ? userId : 0,
                        "customerName": pdetails.sellerName(),
                        "customerEmail": pdetails.sellerEmail(),
                        "customerMobile": pdetails.sellerMobile()
                    }
                }
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
                                    vmUnansweredQuestions.formStep(4);
                                    vmUnansweredQuestions.gaHandler();
                                    scrollToForm.activate();
                                    break;
                                default:
                                    vmSellBike.formStep(2);
                                    vmUnansweredQuestions.formStep(2);
                                    scrollToForm.activate();
                            }
                        }
                        else {
                            vmSellBike.formStep(2);
                            vmUnansweredQuestions.formStep(2);
                            scrollToForm.activate();
                        }
                    }
                   ,
                    complete: function (xhr, ajaxOptions, thrownError) {

                    }
                });
                triggerGA('Sell_Page', 'Step3_Update_My_Ad_Clicked', vmSellBike.bikeDetails().makeName() + '_' + vmSellBike.bikeDetails().modelName() + '_' + vmSellBike.bikeDetails().registeredCity() + '_' + vmSellBike.inquiryId());
            }
        };
    } catch (e) {
        console.warn(e);
    }

    self.noThanks = function () {
        vmSellBike.formStep(4);
        vmUnansweredQuestions.formStep(4);
        vmUnansweredQuestions.gaHandler();
        scrollToForm.activate();
        triggerGA('Sell_Page', 'Step3_No_Thanks_Clicked', vmSellBike.bikeDetails().makeName() + '_' + vmSellBike.bikeDetails().modelName() + '_' + vmSellBike.bikeDetails().registeredCity() + '_' + vmSellBike.inquiryId());
    };
};



// bike popup
var selectBikeMake = $('#select-make-wrapper'),
    selectBikeModel = $('#select-model-wrapper'),
    selectBikeVersion = $('#select-version-wrapper');

$('#bike-select-element').on('click', '.bike-box-default', function () {
    bikePopup.open();
    appendState('selectBike');
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

        if (vmSellBike.isEdit()) {
            vmSellBike.bikeDetails().modelChanged();
        }

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
    try {
        if (isEdit != "True") {
            $('#city-search-box').val("");
            $(citiesList).show();
            cityListSelection.open();
            vmSellBike.bikeDetails().citySelectionStatus('bike-city');
            appendState('bikeCity');
        }
    } catch (e) {
        console.warn(e);
    }
});

$('#registration-select-element').on('click', '.city-box-default', function () {
    try {
        $('#city-search-box').val("");
        $(citiesList).show();
        cityListSelection.open();
        vmSellBike.bikeDetails().citySelectionStatus('registered-city');
        appendState('registrationCity');
    } catch (e) {
        console.warn(e);
    }
});

$('.city-box-default').on('click', function () {
    vmSellBike.bikeDetails().Cities().cityFilter('');
});

$('#close-city-filter').on('click', function () {
    cityListSelection.close();
});

$('#city-slideIn-drawer').on('click', '.filter-list li', function () {
    try {
        var element = $(this);
        if (!((vmSellBike.bikeDetails().Cities().visibleCities().length == 0) && (vmSellBike.bikeDetails().Cities().cityFilter().length > 0))) {
            if (vmSellBike.bikeDetails().citySelectionStatus() == 'bike-city') {
                vmSellBike.bikeDetails().city(element.text());
                vmSellBike.bikeDetails().cityId(element.attr("data-cityId"));
            }
            else if (vmSellBike.bikeDetails().citySelectionStatus() == 'registered-city') {
                vmSellBike.bikeDetails().registeredCity(element.text());
                vmSellBike.bikeDetails().regCityId(element.attr("data-cityId"));
            }
            cityListSelection.close();
        }
    } catch (e) {
        console.warn(e);
    }

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
        vmSellBike.bikeDetails().prevColor(vmSellBike.bikeDetails().color());
        vmSellBike.bikeDetails().prevColorId(vmSellBike.bikeDetails().colorId());
        colorBox.popup.find('li.active').removeClass('active');

        colorBox.popup.find('li').each(function () {
            var current = $(this);
            if (current.text().trim() == vmSellBike.bikeDetails().color())
                current.addClass("active");
        })

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

$('.chosen-container').on('mousedown', function (event) {
    $('input').blur();
});


$('.chosen-container').on('touchstart', function (event) {
    event.stopPropagation();
    event.preventDefault();
    $(this).trigger('mousedown');
}).on('touchend', function (event) {
    event.preventDefault();
});

$('.select-box select').on('change', function () {
    if ($(this).val().length > 0) {
        $(this).closest('.select-box').addClass('done');
        $('body').trigger('click'); // prevent chosen select from triggering background click events
    }
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

$('#kmsRidden, #expectedPrice').on('focus', function () {
    var inputBox = $(this),
        withoutCommaValue = inputBox.attr('data-value');

    inputBox.val(withoutCommaValue);
});

$('#kmsRidden, #expectedPrice').on('blur', function () {
    var inputBox = $(this),
        inputValue = inputBox.val();

    inputBox.attr('data-value', inputValue);
    inputBox.val(formatNumber(inputValue));
});

// input value formatter
var formatNumber = function (num) {
    var thMatch = /(\d+)(\d{3})$/,
        thRest = thMatch.exec(num);

    if (!thRest) return num;
    return (thRest[1].replace(/\B(?=(\d{2})+(?!\d))/g, ",") + "," + thRest[2]);
}
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

$(document).on("click", ".answer-question__item a", function () {
    triggerGA("List_Used_Bike", "Answer_This_Question_Link_Clicked", vmSellBike.bikeDetails().makeName() + "_" + vmSellBike.bikeDetails().modelName());
});


var unansweredQuestions = function()
{
    var self = this;
    self.questions = ko.observableArray([]);
    self.isUnansweredQuestionsKOInitialized = ko.observable(false);
    self.formStep = ko.observable(2);
    self.makeName = ko.observable("");
    self.modelName = ko.observable("");

    self.bindQuestions = function (modelId, userName, userEmail, returnUrl, platformId, sourceId)
    {

        var requestUrl = "/api/models/" + modelId + "/unanswered-questions/";
        var requestPayload = "?userName=" + userName + "&userEmail=" + userEmail + "&returnURL=" + returnUrl + "&platformId=" + platformId + "&sourceId=" + sourceId;

        $.get(requestUrl + requestPayload, function (responseData) {
            responseData = JSON.parse(responseData);
            if (responseData.length > 0)
            {
                self.questions(responseData);
                self.isUnansweredQuestionsKOInitialized(true);           
            }
            
        });
    }

    self.gaHandler = function()
    {
        if(self.questions().length > 0)
        {
            triggerNonInteractiveGA("List_Used_Bike", "Answer_This_Question_Link_Displayed", self.makeName() + "_" + self.modelName());
        }
    }

}
