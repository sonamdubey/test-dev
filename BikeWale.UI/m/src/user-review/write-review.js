var ratingBox, selectedAnswer;

var userNameField, userEmailIdField;
var detailedReviewField, reviewTitleField;

var bikeRating = {
    ratingCount: 0,
    overallRating: [
        {
            heading: "Terrible!",
            description: "I regret riding this bike."
        },
        {
            heading: "It's bad!",
            description: "I know better bikes in the same price range."
        },
        {
            heading: "Ummm..!",
            description: "It's okay. Could have been better."
        },
        {
            heading: "Superb!",
            description: "It's good to ride, I love it."
        },
        {
            heading: "Amazing!",
            description: "I love everything about the bike."
        }
    ]
};

var ratingQuestion = [
    {
        qid: 101,
        type: 'text',
        heading: "What do you use this bike for?",
        rating: ["Daily Commute", "Occasional Commute", "Leisure Rides", "Tours", "All"],
        isRequired: true,
        visibility: true,
        subQuestionId: 0
    },
    {
        qid: 102,
        type: 'text',
        heading: "How long have you owned this bike for?",
        rating: ["Don't own", "< 3 months", "3-6 months", "6 months-1 yr", "> 1 yr"],
        isRequired: true,
        visibility: true,
        subQuestionId: 103
    },
    {
        qid: 103,
        type: 'text',
        heading: "How much have you ridden this bike?",
        rating: ["< 5000 kms", "5000-10000 kms", "10000-15000 kms", "> 15000 kms"],
        isRequired: true,
        visibility: false,
        subQuestionId: 0
    }
];

var reviewQuestion = JSON.parse($("#review-question-list").text())


//var reviewQuestion = [
//    {
//        type: 'star',
//        heading: "Visual Appeal/Looks",
//        description: "Remember, what others thought when they first saw your bike!",
//        rating: ["Terrible!", "It's bad", "Okay", "Excellent", "Gorgeous"],
//        currentlySelected: 0
//    },
//    {
//        type: 'star',
//        heading: "Reliability",
//        description: "Remember, what others thought when they first saw this bike!",
//        rating: ["Unpredictable", "Unreliable", "You can rely!", "Dependable", "Trustworthy"],
//        currentlySelected: 0
//    },
//    {
//        type: 'star',
//        heading: "Performance",
//        description: "Remember, what others thought when they first saw this bike!",
//        rating: ["Very poor", "Poor", "Not bad", "Good", "Excellent"],
//        currentlySelected: 0
//    },
//    {
//        type: 'text',
//        heading: "Maintenance cost",
//        description: "Are regular replacement parts reasonably priced? How much does it cost to fix a broken part?",
//        rating: ["Unreasonably High", "Very High", "High", "Reasonable", "Well priced"],
//        currentlySelected: 0
//    }
//];

docReady(function () {

    ratingBox = $('#bike-rating-box');

    // rate bike
    var rateBike = function () {
        var self = this;

        self.ratingCount = ko.observable(0);
        self.feedbackTitle = ko.observable('Rate your bike');
        self.feedbackSubtitle = ko.observable('');
        self.validateRatingCountFlag = ko.observable(false);
        self.ratingErrorText = ko.observable('');

        self.personalDetails = ko.observable(new personalDetails);

        self.focusFormActive = ko.observable(false);

        self.bikeRating = ko.observable(bikeRating);
        self.overallRating = ko.observableArray(self.bikeRating().overallRating);
        self.ratingQuestions = ko.observableArray(ratingQuestion);        

        self.submitRating = function () {
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
                    self.ratingErrorText("Please rate the bike before submitting!");

                    self.focusFormActive(true);
                    answer.focusForm(ratingBox);
                }

                return self.validateRatingCountFlag();
            },

            ratingForm: function () {
                var isValid = false,
                    questionFields = $('#rate-bike-questions .question-field'),
                    questionLength = questionFields.length,
                    errorCount = 0;

                for (var i = 0; i < questionLength; i++) {
                    var item = $(questionFields[i]),
                        itemRequirement = item.attr('data-required');

                    if (itemRequirement) {
                        var checkedRadioButton = item.find('.answer-radio-list input[type=radio]:checked');

                        if (!checkedRadioButton.length) {
                            item.find('.error-text').show();
                            errorCount++;
                        }
                        else {
                            item.find('.error-text').hide();
                        }
                    }
                }

                if (!errorCount) {
                    isValid = true;
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
    
    ratingBox.find('.answer-star-list input[type=radio]').change(function () {
        var button = $(this),
            buttonValue = Number(button.val());
            
        var headingText = vmRateBike.overallRating()[buttonValue - 1].heading,
            descText = vmRateBike.overallRating()[buttonValue - 1].description; // since value starts from 1 and array from 0

        vmRateBike.feedbackTitle(headingText);
        vmRateBike.feedbackSubtitle(descText);
        vmRateBike.ratingCount(buttonValue);
    });

    $('#rate-bike-questions').find('.question-type-text input[type=radio]').change(function () {
        var questionField = $(this).closest('.question-type-text'),
            subQuestionId = Number(questionField.attr('data-sub-question'));

        questionField.find('.error-text').hide();

        if (subQuestionId > 0) {
            var buttonValue = Number($(this).val()),
                subQuestionField = $('#question-' + subQuestionId);

            if (buttonValue == 1) {
                subQuestionField.hide();
            }
            else {
                subQuestionField.find('.error-text').hide();
                subQuestionField.show();
            }
        }
    });

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
        var self = this;

        self.reviewCharLimit = ko.observable(300);
        self.reviewTitle = ko.observable('');
        self.detailedReview = ko.observable('');

        self.detailedReviewFlag = ko.observable(false);
        self.detailedReviewError = ko.observable('');
        self.focusFormActive = ko.observable(false);
        
        self.reviewQuestions = ko.observableArray(reviewQuestion);

        self.submitReview = function () {

            var array = new Array;

            $(".list-item input[type='radio']:checked").each(function (i) {
                var r = $(this);
                array[i] = (r.attr("questiontId") + ':' + r.val());
            });

            $('#reviewQuestion').val(array.join(","));

            if (self.detailedReview().length > 0 || self.reviewTitle().length > 0) {
                if (self.validateReviewForm()) {
                    return true;
                }               
            }
            else {
                self.detailedReviewFlag(false);
                validate.hideError(reviewTitleField);
                return true;
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

                return !self.detailedReviewFlag();
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
    
    $('#bike-review-questions').find('.question-type-star input[type=radio]').change(function () {
        var button = $(this),
            questionField = button.closest('.question-type-star');
        
        var feedbackText = vmWriteReview.reviewQuestions()[questionField.index()].rating[button.val() - 1].ratingText;
        questionField.find('.feedback-text').text(feedbackText);
    });

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