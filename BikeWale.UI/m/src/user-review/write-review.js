var userNameField,
    userEmailIdField;

docReady(function () {

    // slideIn animation
    ko.bindingHandlers.slideIn = {
        init: function (element, valueAccessor) {
            var value = ko.utils.unwrapObservable(valueAccessor());
            $(element).toggle(value);
        },
        update: function (element, valueAccessor) {
            var value = ko.utils.unwrapObservable(valueAccessor());
            value ? $(element).slideDown(200, 'linear') : $(element).slideUp(200, 'linear');
        }
    };

    var rateBike = function () {
        var self = this;

        self.bikeUseType = ko.observable(0);
        self.bikeTimeSpan = ko.observable(0);
        self.bikeDistance = ko.observable(0);

        self.personalDetails = ko.observable(new personalDetails);

        self.validationStatus = ko.observable(false);
        self.validateDistance = ko.observable(false);

        self.setBikeUseType = function (data, event) {
            var element = $(event.target),
                elementValue = Number(element.attr('data-value'));

            answer.selection(element);
            self.bikeUseType(elementValue);
        };

        self.setBikeTimeSpan = function () {
            var element = $(event.target),
                elementValue = Number(element.attr('data-value'));

            answer.selection(element);
            self.bikeTimeSpan(elementValue);
            self.bikeDistance(0);
            $('#question-bike-distance').find('li.active').removeClass('active');
            self.validateDistance(false);
        };

        self.setBikeDistance = function () {
            var element = $(event.target),
                elementValue = Number(element.attr('data-value'));

            answer.selection(element);
            self.validateDistance(false);
            self.bikeDistance(elementValue);
        };

        self.submitRating = function () {
            self.validationStatus(true);
            self.validateRating();
        };

        self.validateRating = function () {
            var isValid = false;

            if (self.bikeUseType() > 0 && self.bikeTimeSpan() > 0) {
                if (!self.bikeDistance()) {
                    self.validateDistance(true);
                    isValid = true;
                }
            }

            if (self.personalDetails().validateDetails()) {
                console.log('go to step 2');
            }
            else {
                console.log('error!');
            }
        };

    };

    userNameField = $('#getUserName');
    userEmailIdField = $('#getEmailID');

    var personalDetails = function () {
        var self = this;

        self.userName = ko.observable('');
        self.emailId = ko.observable('');

        self.validateDetails = function () {
            var isValid = true;

            isValid = self.validateUserName();
            isValid &= self.validateEmailId();

            return isValid;
        };

        self.validateUserName = function () {
            var isValid = false;

            if (self.userName() != null && self.userName().trim() != "") {
                var nameLength = self.userName().length;

                if (self.userName().indexOf('&') != -1) {
                    validate.setError(userNameField, 'Invalid name');
                    isValid = false;
                }
                else if (nameLength == 0) {
                    validate.setError(userNameField, 'Please enter your name');
                    isValid = false;
                }
                else if (nameLength >= 1) {
                    validate.hideError(userNameField);
                    isValid = true;
                }
            }
            else {
                validate.setError(userNameField, 'Please enter your name');
                isValid = false;
            }

            return isValid;
        };

        self.validateEmailId = function () {
            var isValid = true,
                emailVal = userEmailIdField.val(),
                reEmail = /^[A-z0-9._+-]+@@[A-z0-9.-]+\.[A-z]{2,6}$/;

            if (emailVal == "") {
                validate.setError(userEmailIdField, 'Please enter email id');
                isValid = false;
            }
            else if (!reEmail.test(emailVal)) {
                validate.setError(userEmailIdField, 'Please enter your valid email ID');
                isValid = false;
            }

            return isValid;
        };
    };

    var vmRateBike = new rateBike();
    ko.applyBindings(vmRateBike, $('#rate-bike-form')[0]);

    userNameField.on("focus", function () {
        validate.onFocus($(this));
    });

    userEmailIdField.on("focus", function () {
        validate.onFocus($(this));
    });

    userNameField.on("blur", function () {
        validate.onBlur($(this));
    });

    userEmailIdField.on("blur", function () {
        validate.onBlur($(this));
    });

});

var answer = {
    selection: function (element) {
        $(element).siblings().removeClass('active');
        $(element).addClass('active');
    }
};

/* form validation */
var validate = {
    setError: function (element, message) {
        var elementLength = element.val().length,
            errorTag = element.siblings('span.error-text');

        errorTag.show().text(message);
        if (!elementLength) {
            element.closest('.input-box').removeClass('not-empty').addClass('invalid');
        }
        else {
            element.closest('.input-box').addClass('not-empty invalid');
        }
    },

    hideError: function (element) {
        element.closest('.input-box').removeClass('invalid').addClass('not-empty');
        element.siblings('span.error-text').text('').hide();
    },

    onFocus: function (inputField) {
        var inputBox = inputField.closest('.input-box');

        if (inputBox.hasClass('invalid')) {
            validate.hideError(inputField);
        }
    },

    onBlur: function (inputField) {
        var inputLength = inputField.val().length;
        if (!inputLength) {
            inputField.closest('.input-box').removeClass('not-empty');
        }
        else {
            inputField.closest('.input-box').addClass('not-empty');
        }
    }
};