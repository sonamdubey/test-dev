var userNameField, userEmailIdField;
var detailedReviewField, reviewTitleField;

var review = [
    {
        type: 'star',
        heading: "Visual Appeal/Looks",
        description: "Remember, what others thought when they first saw your bike!",
        rating: ["Terrible!", "It's bad", "Okay", "Excellent", "Gorgeous"],
        currentlySelected: 0
    },
    {
        type: 'star',
        heading: "Reliability",
        description: "Remember, what others thought when they first saw this bike!",
        rating: ["Unpredictable", "Unreliable", "You can rely!", "Dependable", "Trustworthy"],
        currentlySelected: 0
    },
    {
        type: 'star',
        heading: "Performance",
        description: "Remember, what others thought when they first saw this bike!",
        rating: ["Very poor", "Poor", "Not bad", "Good", "Excellent"],
        currentlySelected: 0
    },
    {
        type: 'text',
        heading: "Maintenance cost",
        description: "Are regular replacement parts reasonably priced? How much does it cost to fix a broken part?",
        rating: ["Unreasonably High", "Very High", "High", "Reasonable", "Well priced"],
        currentlySelected: 0
    }
];

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
        
        self.feedbackQuestions = ko.observableArray(review);
        
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

        self.setRating = function (parentNodeElement, data, event) {
            var element = $(event.currentTarget),
                elementCount = Number(element.attr('data-count')),
                elementParent = element.closest('ul');

            element.closest('.list-item').find('.feedback-text').text(data);
            parentNodeElement.currentlySelected = elementCount;

            elementParent.find('li').removeClass('star-one-active');
            element.addClass('star-one-active');
        };

        self.setButtonSelection = function (parentElement, data, event) {
            var element = $(event.currentTarget),
                elementCount = Number(element.attr('data-count')),
                elementParent = element.closest('ul');

            parentElement.currentlySelected = elementCount;

            element.siblings().removeClass('active');
            element.addClass('active');
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