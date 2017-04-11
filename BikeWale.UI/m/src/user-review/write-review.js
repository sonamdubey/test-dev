var userNameField, userEmailIdField;
var detailedReviewField, reviewTitleField;

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

    // rate bike
    var rateBike = function () {
        var self = this;

        self.ratingCount = ko.observable(0);
        self.feedbackTitle = ko.observable('Rate your bike');
        self.feedbackSubtitle = ko.observable('');
        self.validateRatingCountFlag = ko.observable(false);

        self.bikeUseType = ko.observable(0);
        self.bikeTimeSpan = ko.observable(0);
        self.bikeDistance = ko.observable(0);

        self.personalDetails = ko.observable(new personalDetails);

        self.validationFlag = ko.observable(false);
        self.validateDistanceFlag = ko.observable(false);
        self.focusFormActive = ko.observable(false);

        self.setRating = function (data, event) {
            var count = Number($(event.target).attr('data-count'));

            self.ratingCount(count);

            switch (count) {
                case 1:
                    self.feedbackTitle("Terrible!");
                    self.feedbackSubtitle("I regret riding this bike.");
                    break;
                case 2:
                    self.feedbackTitle("It's bad!");
                    self.feedbackSubtitle("I know better bikes in the same price range.");
                    break;
                case 3:
                    self.feedbackTitle("Ummm..!");
                    self.feedbackSubtitle("It's okay. Could have been better.");
                    break;
                case 4:
                    self.feedbackTitle("Superb!");
                    self.feedbackSubtitle("It's good to ride, I love it.");
                    break;
                case 5:
                    self.feedbackTitle("Amazing!");
                    self.feedbackSubtitle("I love everything about the bike.");
                    break;
                default:
                    self.feedbackTitle("Rate your bike");
                    self.feedbackSubtitle("");
                    break;
            }
        };

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
            // reset bike distance answer
            self.bikeDistance(0);
            $('#question-bike-distance').find('li.active').removeClass('active');
            self.validateDistanceFlag(false);
        };

        self.setBikeDistance = function () {
            var element = $(event.target),
                elementValue = Number(element.attr('data-value'));

            answer.selection(element);
            self.validateDistanceFlag(false);
            self.bikeDistance(elementValue);
        };

        self.submitRating = function () {
            self.validationFlag(true);

            if (self.validateRateBikeForm()) {
                // go to step 2
            }
        };

        self.validateRateBikeForm = function () {
            var isValid = false;

            isValid = self.validate.ratingCount();
            isValid &= self.validate.ratingForm();
            isValid &= self.personalDetails().validateDetails();

            return isValid;
        };

        self.validate = {
            ratingCount: function () {
                if (self.ratingCount() == 0) {
                    self.validateRatingCountFlag(true);
                    self.focusFormActive(true);
                    answer.focusForm($('#bike-rating-box'));
                }

                return self.validateRatingCountFlag();
            },

            ratingForm: function () {
                var isValid = false;

                if (self.bikeUseType() > 0 && self.bikeTimeSpan() > 0) {
                    if (!self.bikeDistance()) {
                        self.validateDistanceFlag(true);
                        if (!self.focusFormActive()) {
                            answer.focusForm($('#question-bike-distance'));
                        }
                    }
                    else {
                        isValid = true;
                    }
                }
                else {
                    if (!self.focusFormActive()) {
                        answer.focusForm($('#rate-bike-questions'));
                    }
                }

                self.focusFormActive(false);

                return isValid;
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
                }
                else if (nameLength == 0) {
                    validate.setError(userNameField, 'Please enter your name');
                }
                else if (nameLength >= 1) {
                    validate.hideError(userNameField);
                    isValid = true;
                }
            }
            else {
                validate.setError(userNameField, 'Please enter your name');
            }

            return isValid;
        };

        self.validateEmailId = function () {
            var isValid = false,
                reEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;

            if (self.emailId() == "") {
                validate.setError(userEmailIdField, 'Please enter email id');
            }
            else if (!reEmail.test(self.emailId())) {
                validate.setError(userEmailIdField, 'Please enter your valid email ID');
            }

            else {
                validate.hideError(userEmailIdField);
                isValid = true;
            }

            return isValid;
        };
    };

    var vmRateBike = new rateBike(),
        rateBikeForm = document.getElementById('rate-bike-form');

    if (rateBikeForm) {
        ko.applyBindings(vmRateBike, rateBikeForm);
    }

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

    detailedReviewField = $('#getDetailedReview');
    reviewTitleField = $('#getReviewTitle');

    // write review
    var writeReview = function () {
        var self = this,
            ratingCount = 5;

        self.headingText = ko.observable('hello');
        self.reviewCharLimit = ko.observable(300);
        self.reviewTitle = ko.observable('');
        self.detailedReview = ko.observable('');

        self.detailedReviewFlag = ko.observable(false);
        self.detailedReviewError = ko.observable('');
        self.focusFormActive = ko.observable(false);

        // optional rating section
        self.visualAppealCount = ko.observable(0);
        self.visualAppealFeedback = ko.observable('');

        self.reliabilityCount = ko.observable(0);
        self.reliabilityFeedback = ko.observable('');

        self.performanceCount = ko.observable(0);
        self.performanceFeedback = ko.observable('');

        self.serviceExperienceCount = ko.observable(0);
        self.serviceExperienceFeedback = ko.observable('');

        self.comfortCount = ko.observable(0);
        self.comfortFeedback = ko.observable('');

        self.maintenanceCount = ko.observable(0);
        self.valueForMoneyCount = ko.observable(0);

        self.extraFeaturesCount = ko.observable(0);
        self.extraFeaturesFeedback = ko.observable('');
        
        self.setHeading = function () {
            switch (ratingCount) {
                case 1:
                case 2:
                    self.headingText('Had a poor experience?');
                    break;
                case 3:
                    self.headingText('Had a reasonable experience?');
                    break;
                case 4:
                case 5:
                    self.headingText('Had an amazing experience?');
                    break;
                default:
                    self.headingText('Had a reasonable experience?');
                    break;
            }
        };

        self.setHeading();

        self.setRating = function (data, event) {
            var element = $(event.target),
                elementType = element.closest('.item-rating-list').attr('data-list-type');

            switch (elementType) {
                case "visualAppeal":
                    self.visualAppealCount(Number(element.attr('data-count')));
                    self.setFeedback.visualAppeal();
                    break;
                case "reliability":
                    self.reliabilityCount(Number(element.attr('data-count')));
                    self.setFeedback.reliability();
                    break;
                case "performance":
                    self.performanceCount(Number(element.attr('data-count')));
                    self.setFeedback.performance();
                    break;
                case "serviceExperience":
                    self.serviceExperienceCount(Number(element.attr('data-count')));
                    self.setFeedback.serviceExperience();
                    break;
                case "comfort":
                    self.comfortCount(Number(element.attr('data-count')));
                    self.setFeedback.comfort();
                    break;
                case "extraFeatures":
                    self.extraFeaturesCount(Number(element.attr('data-count')));
                    self.setFeedback.extraFeatures();
                    break;
                default:
                    break;
            }
        };

        self.setMaintenanceCost = function (data, event) {
            var element = $(event.target),
                elementValue = Number(element.attr('data-value'));

            answer.selection(element);
            self.maintenanceCount(elementValue);
        };

        self.setValueForMoney = function (data, event) {
            var element = $(event.target),
                elementValue = Number(element.attr('data-value'));

            answer.selection(element);
            self.valueForMoneyCount(elementValue);
        };

        self.setFeedback = {
            visualAppeal: function () {
                switch (self.visualAppealCount()) {
                    case 1:
                        self.visualAppealFeedback("Terrible!");
                        break;
                    case 2:
                        self.visualAppealFeedback("It's bad");
                        break;
                    case 3:
                        self.visualAppealFeedback("Okay");
                        break;
                    case 4:
                        self.visualAppealFeedback("Excellent");
                        break;
                    case 5:
                        self.visualAppealFeedback("Gorgeous");
                        break;
                    default:
                        break;
                }
            },

            reliability: function () {
                switch (self.reliabilityCount()) {
                    case 1:
                        self.reliabilityFeedback("Unpredictable");
                        break;
                    case 2:
                        self.reliabilityFeedback("Unreliable");
                        break;
                    case 3:
                        self.reliabilityFeedback("You can rely!");
                        break;
                    case 4:
                        self.reliabilityFeedback("Dependable");
                        break;
                    case 5:
                        self.reliabilityFeedback("Trustworthy");
                        break;
                    default:
                        break;
                }
            },

            performance: function () {
                switch (self.performanceCount()) {
                    case 1:
                        self.performanceFeedback("Very poor");
                        break;
                    case 2:
                        self.performanceFeedback("Poor");
                        break;
                    case 3:
                        self.performanceFeedback("Not bad");
                        break;
                    case 4:
                        self.performanceFeedback("Good");
                        break;
                    case 5:
                        self.performanceFeedback("Excellent");
                        break;
                    default:
                        break;
                }
            },

            serviceExperience: function () {
                switch (self.serviceExperienceCount()) {
                    case 1:
                        self.serviceExperienceFeedback("Very poor");
                        break;
                    case 2:
                        self.serviceExperienceFeedback("Poor");
                        break;
                    case 3:
                        self.serviceExperienceFeedback("It was okay!");
                        break;
                    case 4:
                        self.serviceExperienceFeedback("It was good!");
                        break;
                    case 5:
                        self.serviceExperienceFeedback("Awesome!");
                        break;
                    default:
                        break;
                }
            },

            comfort: function () {
                switch (self.comfortCount()) {
                    case 1:
                        self.comfortFeedback("It's tough");
                        break;
                    case 2:
                        self.comfortFeedback("Not comfortable");
                        break;
                    case 3:
                        self.comfortFeedback("Can live with it!");
                        break;
                    case 4:
                        self.comfortFeedback("Comfortable");
                        break;
                    case 5:
                        self.comfortFeedback("Very comfortable");
                        break;
                    default:
                        break;
                }
            },

            extraFeatures: function () {
                switch (self.extraFeaturesCount()) {
                    case 1:
                        self.extraFeaturesFeedback("Useless");
                        break;
                    case 2:
                        self.extraFeaturesFeedback("Not useful");
                        break;
                    case 3:
                        self.extraFeaturesFeedback("Rarely useful");
                        break;
                    case 4:
                        self.extraFeaturesFeedback("Fairly useful");
                        break;
                    case 5:
                        self.extraFeaturesFeedback("Very useful");
                        break;
                    default:
                        break;
                }
            }
        };

        self.submitReview = function () {
            if (self.detailedReview().length > 0 || self.reviewTitle().length > 0) {
                self.validateReviewForm();
            }
            else {
                self.detailedReviewFlag(false);
                validate.hideError(reviewTitleField);
                // go to step 3
            }
        };

        self.validateReviewForm = function () {
            var isValid = false;

            isValid = self.validate.detailedReview();
            isValid &= self.validate.reviewTitle();

            self.focusFormActive(false);
            return isValid;
        };

        self.validate = {
            detailedReview: function () {
                if (self.detailedReview().length < 300) {
                    self.detailedReviewFlag(true);
                    self.detailedReviewError('Your review should contain at least 300 characters.');
                    self.focusFormActive(true);
                    answer.focusForm(detailedReviewField);
                }
                else {
                    self.detailedReviewFlag(false);
                }

                return self.detailedReviewFlag();
            },

            reviewTitle: function () {
                var isValid = false;

                if (self.reviewTitle().length == 0) {
                    validate.setError(reviewTitleField, 'Please provide a title for your review!');
                    if (!self.focusFormActive()) {
                        answer.focusForm(detailedReviewField);
                    }
                }
                else {
                    validate.hideError(reviewTitleField);
                    isValid = true;
                }

                return isValid;
            }
        };

    };

    var vmWriteReview = new writeReview(),
        writeReviewForm = document.getElementById('write-review-form');

    if (writeReviewForm) {
        ko.applyBindings(vmWriteReview, writeReviewForm);
    }

    detailedReviewField.on('focus', function () {
        vmWriteReview.detailedReviewFlag(false);
    });

    reviewTitleField.on("focus", function () {
        validate.onFocus($(this));
    });

    reviewTitleField.on("blur", function () {
        validate.onBlur($(this));
    });

});

var answer = {
    selection: function (element) {
        $(element).siblings().removeClass('active');
        $(element).addClass('active');
    },

    focusForm: function (element) {
        $('html, body').animate({ scrollTop: $(element).offset().top }, 500);
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
        var inputBox = element.closest('.input-box');

        inputBox.removeClass('invalid');
        if (element.val().length > 0) {
            inputBox.addClass('not-empty');
        }
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