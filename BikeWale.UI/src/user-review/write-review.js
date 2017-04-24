﻿var ratingBox;
var userNameField, userEmailIdField;
var detailedReviewField, reviewTitleField;

var bikeRating = {
    ratingCount: 0,
    overallRating: []
};

var ratingQuestion = [];

var reviewQuestion = [
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
    
    ratingBox = $('#bike-rating-box');

    if ($("#overallratingQuestion") && $("#overallratingQuestion").length)
        bikeRating.overallRating = JSON.parse(Base64.decode($('#overallratingQuestion').val()));

    if ($("#rating-question") && $("#rating-question").length)
        ratingQuestion = JSON.parse(Base64.decode($('#rating-question').val()));

    if ($('#reviewedoverallrating') && $('#reviewedoverallrating').length)
        reviewOverallRatingId = Number($('#reviewedoverallrating').val());

    ko.bindingHandlers.slideIn = {
        update: function (element, valueAccessor) {
            var value = ko.utils.unwrapObservable(valueAccessor());
            value ? $(element).show() : $(element).fadeOut(200);
        }
    };

    ko.bindingHandlers.hoverRating = {
        update: function (element, valueAccessor) {
            var value = ko.utils.unwrapObservable(valueAccessor());

            ko.utils.registerEventHandler(element, "mouseover", function () {
                bikeRatingBox.setFeedback($(this));
            });

            ko.utils.registerEventHandler(element, "mouseout", function () {
                bikeRatingBox.resetFeedback($(this));
            });
        }
    };

    // rate bike
    var rateBike = function () {
        var self = this;

        self.ratingCount = ko.observable(0);
        self.feedbackTitle = ko.observable('Rate your bike');
        self.feedbackSubtitle = ko.observable('');

        self.validateRatingCountFlag = ko.observable(false);
        self.ratingErrorText = ko.observable('');

        self.personalDetails = ko.observable(new personalDetails);

        self.bikeRating = ko.observable(bikeRating);
        self.overallRating = ko.observableArray(self.bikeRating().overallRating);
        self.ratingQuestions = ko.observableArray(ratingQuestion);

        self.clickEventRatingCount = ko.observable(0);

        self.userName = ko.observable($('#txtUserName').val());
        self.emailId = ko.observable($('#txtEmailID').val());


        userNameField = $('#txtUserName');
        userEmailIdField = $('#txtEmailID');

        self.init = function () {
            $('#rate-bike-questions .question-type-text input[type=radio][data-checked="true"]').each(function () {
                $(this).prop("checked", true);
            });
            if (getCookie("_PQUser") != null) {
                var array_cookie = getCookie("_PQUser").split("&");

                if (array_cookie[0] != null && userNameField.val() == "") {
                    userNameField.parent('div').addClass('not-empty');
                    userNameField.val(array_cookie[0]);
                    vmRateBike.userName(array_cookie[0]);

                }
                else {
                    userNameField.val = userNameField.val();
                }
                if (array_cookie[1] != null && userEmailIdField.val() == "") {
                    userEmailIdField.parent('div').addClass("not-empty");
                    userEmailIdField.val(array_cookie[1]);
                    vmRateBike.emailId(array_cookie[1]);
                }
                else {

                    userEmailIdField.val = userEmailIdField.val();
                }
            }
            if (reviewOverallRatingId > 0) {
                var headingText = vmRateBike.overallRating()[reviewOverallRatingId - 1].heading,
                descText = vmRateBike.overallRating()[reviewOverallRatingId - 1].description; // since value starts from 1 and array from 0

                vmRateBike.feedbackTitle(headingText);
                vmRateBike.feedbackSubtitle(descText);

                vmRateBike.ratingCount(reviewOverallRatingId);
                ratingBox.find('.answer-star-list input[type=radio][value="' + reviewOverallRatingId + '"]').trigger("click");
            }



        };

        self.submitRating = function () {
            if (self.validateRateBikeForm()) {
                array_rating = new Array;
                value_overallrating = $("#bike-rating-box input[type='radio']:checked").attr("value");
                $("#rate-bike-questions input[type='radio']:checked").each(function (i) {
                    var r = $(this);
                    var value = r.closest
                    var value = r.closest('.question-field').attr('data-required');
                    if (value) {
                        array_rating[i] = (r.attr("questionId") + ':' + r.attr("value"));
                    }
                    if (r.attr("questionId") == "2" && r.attr("value") == "1") {
                        i++;
                        array_rating[i] = "3:0"
                    }
                });

                $("#finaloverallrating").val(value_overallrating);
                $("#rating-quesition-ans").val(array_rating);

                return true;
            }

            return false;
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
                }
                else {
                    self.validateRatingCountFlag(false);
                }

                return !self.validateRatingCountFlag();
            },

            ratingForm: function () {
                var isValid = false,
                    questionFields = $('#rate-bike-questions .question-field'),
                    questionLength = questionFields.length,
                    errorCount = 0;

                for (var i = 0; i < questionLength; i++) {
                    var item = $(questionFields[i]),
                        itemRequirement = Boolean(item.attr('data-required'));

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

                return isValid;
            }
        };

    };


    var personalDetails = function () {
        var self = this;

       

        self.validateDetails = function () {
            var isValid = true;

            isValid = self.validateUserName();
            isValid &= self.validateEmailId();

            return isValid;
        };

        self.validateUserName = function () {
            var isValid = false;

            if (vmRateBike.userName() != null && vmRateBike.userName().trim() != "") {
                var nameLength = vmRateBike.userName().length;

                if (vmRateBike.userName().indexOf('&') != -1) {
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

            if (vmRateBike.emailId() == "") {
                validate.setError(userEmailIdField, 'Please enter email id');
            }
            else if (!reEmail.test(vmRateBike.emailId())) {
                validate.setError(userEmailIdField, 'Please enter your valid email ID');
            }
            else if (!Boolean($('#isFake').val()))
            {
                validate.setError(userEmailIdField, 'Please enter an authorised email ID to continue.');
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
        vmRateBike.init();
    }
    
    ratingBox.find('.answer-star-list input[type=radio]').change(function () {
        var button = $(this),
            buttonValue = Number(button.val());

        var headingText = vmRateBike.overallRating()[buttonValue - 1].heading,
            descText = vmRateBike.overallRating()[buttonValue - 1].description; // since value starts from 1 and array from 0

        vmRateBike.feedbackTitle(headingText);
        vmRateBike.feedbackSubtitle(descText);
        vmRateBike.ratingCount(buttonValue);
        vmRateBike.clickEventRatingCount(buttonValue);
    });

    $('#rate-bike-questions').find('.question-type-text input[type=radio]').change(function () {
        var questionField = $(this).closest('.question-type-text'),
            subQuestionId = Number(questionField.attr('data-sub-question'));

        questionField.find('.error-text').slideUp();

        if (subQuestionId > 0) {
            var buttonValue = Number($(this).val()),
                subQuestionField = $('#question-' + subQuestionId);

            if (buttonValue == 1) {
                subQuestionField.slideUp();
                subQuestionField.removeAttr('data-required');
            }
            else {
                subQuestionField.find('.error-text').slideUp();
                subQuestionField.slideDown();
                subQuestionField.attr('data-required', true);
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

    var bikeRatingBox = {
        setFeedback: function (element) {
            var count = Number($(element).attr('data-value'));

            vmRateBike.ratingCount(count);
            vmRateBike.feedbackTitle(vmRateBike.overallRating()[count - 1].heading);
            vmRateBike.validateRatingCountFlag(false);

            if (vmRateBike.clickEventRatingCount() > 0) {
                vmRateBike.feedbackSubtitle(vmRateBike.overallRating()[count - 1].description);
            }
        },

        resetFeedback: function () {
            if (!vmRateBike.clickEventRatingCount()) {
                vmRateBike.ratingCount(0);
                vmRateBike.feedbackTitle('Rate your bike');
            }
            else {
                vmRateBike.ratingCount(vmRateBike.clickEventRatingCount());
                vmRateBike.feedbackTitle(vmRateBike.overallRating()[vmRateBike.clickEventRatingCount() - 1].heading);
                vmRateBike.feedbackSubtitle(vmRateBike.overallRating()[vmRateBike.clickEventRatingCount() - 1].description);
            }
        }
    }

    detailedReviewField = $('#getDetailedReview');
    reviewTitleField = $('#getReviewTitle');
        
    // write review
    var writeReview = function () {
        var self = this,
            ratingCount = 5;

        self.reviewCharLimit = ko.observable(300);
        self.reviewTitle = ko.observable('');
        self.detailedReview = ko.observable('');
        self.reviewTips = ko.observable('');

        self.detailedReviewFlag = ko.observable(false);
        self.detailedReviewError = ko.observable('');
        self.focusFormActive = ko.observable(false);

        self.reviewQuestions = ko.observableArray(reviewQuestion);

        //self.hoverFeedbackRating = ko.observable(0);

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

    $('#bike-review-questions').find('.question-type-star input[type=radio]').change(function () {
        var button = $(this),
            questionField = button.closest('.question-type-star');

        var feedbackText = vmWriteReview.reviewQuestions()[questionField.index()].rating[button.val() - 1];
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

    $('#bike-review-questions').find('.question-type-star label').on('mouseover', function () {
        question.setFeedback($(this));
    }).on('mouseout', function () {
        question.resetFeedback($(this));
    });

    var question = {
        setFeedback: function (element) {
            var feedbackElement = $(element).closest('li.question-type-star').find('.feedback-text');

            feedbackElement.text(question.getFeedbackText(element));
        },

        resetFeedback: function (element) {
            var elementParent = $(element).closest('.answer-star-list'),
                checkedButton = elementParent.find('input[type=radio]:checked');

            if (!checkedButton.length) {
                var feedbackElement = $(element).closest('li.question-type-star').find('.feedback-text');

                feedbackElement.text('');
            }
            else {
                question.setFeedback(checkedButton.next('label')); // send checked radio button's next label
            }
        },

        getFeedbackText: function (element) {
            if (typeof element !== "undefined") {
                var elementId = $(element).attr('for'),
                    elementIdArray = elementId.split('-'), // review-0-0 i.e. review-questionIndex-ratingIndex
                    feedbackText;

                feedbackText = vmWriteReview.reviewQuestions()[elementIdArray[1]].rating[elementIdArray[2] - 1];

                return feedbackText;
            }
        }
    }

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
        var elementLength = element.val.length,
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
        if (element.val.length > 0) {
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