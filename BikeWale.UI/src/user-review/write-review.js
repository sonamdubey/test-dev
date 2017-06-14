﻿var ratingBox, page;
var userNameField, userEmailIdField, vmWriteReview;
var detailedReviewField, reviewTitleField, reviewQuestion, ratingOverAll, pageSourceID;
var writeReview, isSubmit = false;
var makeModelName;
var bikeRating = {
    ratingCount: 0,
    overallRating: []
};

var ratingQuestion = [];
var ratingError = false, questionError = false, userNameError = false, emailError = false;

docReady(function () {
    bwcache.setScope('ReviewPage');
    if (page == "writeReview") {
        setTimeout(function () { window.location.hash = "writeReview"; }, 1000);
        $(window).on('hashchange', function (e) {
            oldUrl = e.originalEvent.oldURL;
            if (oldUrl && (oldUrl.indexOf('#') > 0)) {
                if ($("#previousPageUrl") && $("#previousPageUrl").length)
                    window.location.href = $('#previousPageUrl').text();
            }
        });
    }

    if ($('#review-question-list') && $('#review-question-list').text())
        reviewQuestion = JSON.parse($('#review-question-list').text());

    ratingBox = $('#bike-rating-box');

    if ($("#overallratingQuestion") && $("#overallratingQuestion").length)
        bikeRating.overallRating = JSON.parse(Base64.decode($('#overallratingQuestion').val()));

    if ($("#rating-question") && $("#rating-question").length)
        ratingQuestion = JSON.parse(Base64.decode($('#rating-question').val()));

    if ($('#reviewedoverallrating') && $('#reviewedoverallrating').length)
        reviewOverallRatingId = Number($('#reviewedoverallrating').val());

    if ($('#overAllRatings') && $('#overAllRatings').length)
        ratingOverAll = Number($('#overAllRatings').val());

    if ($('#pageSourceId') && $('#pageSourceId').length)
        pageSourceID = Number($('#pageSourceId').val());

    if (document.getElementById("bike-rating-box") != null && document.getElementById("bike-rating-box").getAttribute("data-make-model")) {
        makeModelName = document.getElementById("bike-rating-box").getAttribute("data-make-model");
    }
    if (document.getElementById("bike-review-questions") != null && document.getElementById("bike-review-questions").getAttribute("data-make-model")) {
        makeModelName = document.getElementById("bike-review-questions").getAttribute("data-make-model");
    }
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
        self.focusFormActive = ko.observable(false);

        self.personalDetails = ko.observable(new personalDetails);

        self.bikeRating = ko.observable(bikeRating);
        self.overallRating = ko.observableArray(self.bikeRating().overallRating);
        self.ratingQuestions = ko.observableArray(ratingQuestion);

        self.clickEventRatingCount = ko.observable(0);

        self.userName = ko.observable($('#txtUserName').val());
        self.emailId = ko.observable($('#txtEmailID').val());


        userNameField = $('#txtUserName');
        userEmailIdField = $('#txtEmailID');
        if (document.getElementById("rate-bike-form") != null && document.getElementById("rate-bike-form").getAttribute("data-value")) {
            validate.setError(userEmailIdField, 'Please enter an authorised email ID to continue.');
        }

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
                if (array_cookie[1] != null && array_cookie[1]!="undefined" && userEmailIdField.val() == "") {
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
                isSubmit = true;
                return true;
            }

            return false;
        };

        self.validateRateBikeForm = function () {
            var isValid = false;

            isValid = self.validate.ratingCount();
            isValid &= self.validate.ratingForm();
            isValid &= self.personalDetails().validateDetails();

            if (isValid) {
                triggerGA('Rate_Bike', 'Rating_Submit_Success', makeModelName + pageSourceID);
            }
            else
            {
                triggerGA('Rate_Bike', 'Rating_Submit_Error', makeModelName+ratingError+'_'+questionError+'_'+userNameError+'_'+emailError);
            }
            self.focusFormActive(false);
            return isValid;
        };

        self.validate = {
            ratingCount: function () {
                if (self.ratingCount() == 0) {
                    self.validateRatingCountFlag(true);
                    self.ratingErrorText("Please rate the bike before submitting!");
                    self.focusFormActive(true);
                    answer.focusForm($('#rate-bike-form'));
                    ratingError = true;
                }
                else {
                    self.validateRatingCountFlag(false);
                    ratingError = false;
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
                            questionError = true;
                            if (!self.focusFormActive()) {
                                answer.focusForm($('#rate-bike-questions'));
                            }
                        }
                        else {
                            item.find('.error-text').hide();
                        }
                    }
                }

                if (!errorCount) {
                    isValid = true;
                    questionError = false;
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
                    userNameError = true;
                }
                else if (nameLength == 0) {
                    validate.setError(userNameField, 'Please enter your name');
                    userNameError = true;
                }
                else if (nameLength >= 1) {
                    validate.hideError(userNameField);
                    isValid = true;
                    userNameError = false;
                }
            }
            else {
                validate.setError(userNameField, 'Please enter your name');
                userNameError = true;
            }

            return isValid;
        };

        self.validateEmailId = function () {
            var isValid = false,
                reEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;

            if (vmRateBike.emailId() == "") {
                validate.setError(userEmailIdField, 'Please enter email id');
                emailError = true;
            }
            else if (!reEmail.test(vmRateBike.emailId())) {
                emailError = true;
                validate.setError(userEmailIdField, 'Please enter your valid email ID');
            }

            else {
                validate.hideError(userEmailIdField);
                isValid = true;
                emailError = false;
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

        triggerGA('Rate_Bike', 'Stars_Rating_Clicked', makeModelName + buttonValue);

    });
    var selRating = $("#bike-rating-box").attr("data-selectedrating");
    if (selRating > 0) {
        ratingBox.find('.answer-star-list input[type=radio][value="' + selRating + '"]').trigger("click");
    }

    $('#rate-bike-questions').find('.question-type-text input[type=radio]').change(function () {
        var questionField = $(this).closest('.question-type-text'),
            subQuestionId = Number(questionField.attr('data-sub-question'));

        questionField.find('.error-text').slideUp();
        triggerGA('Rate_Bike', 'Rate_' + questionField.attr('id') + '_' + makeModelName + Number($(this).val()));
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

    descReviewField = $('#reviewDesc');
    reviewTitleField = $('#getReviewTitle');

    // write review
    writeReview = function () {
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

        self.descLength = ko.computed(function () {

            return self.detailedReview().replace(/\n|\r/g, "").length;
        });

        self.submitReview = function () {
            var array = new Array;

            $(".list-item input[type='radio']:checked").each(function (i) {
                var r = $(this);
                array[i] = (r.attr("questiontId") + ':' + r.val());
            });

            $('#reviewQuestion').val(array.join(","));

            var descArray = vmWriteReview.detailedReview().split('\n');
            var formattedDescArray = "";

            for (i = 0; i < descArray.length; i++) {
                if (descArray[i].trim() != "") {
                    formattedDescArray += "<p>" + descArray[i] + "</p>";
                }
            }

            if ($('#formattedDescripton'))
                $('#formattedDescripton').val(formattedDescArray);

            if (self.detailedReview().length > 0 || self.reviewTitle().length > 0) {
                if (self.validateReviewForm()) {
                    return true;
                }
            }
            else {
                self.detailedReviewFlag(false);
                validate.hideError(reviewTitleField);
                triggerGA('Write_Review', 'Review_Submit_Success', makeModelName + self.detailedReviewFlag() + '_' + pageSourceID + '_' + self.detailedReview().trim().length);
                return true;
            }

        };

        self.validateReviewForm = function () {
            var isValid = false;

            isValid = self.validate.detailedReview();
            isValid &= self.validate.reviewTitle();
            if (isValid) {
                triggerGA('Write_Review', 'Review_Submit_Success', makeModelName + !vmWriteReview.detailedReviewFlag() + '_' + pageSourceID + '_' + self.detailedReview().trim().length);
            }
            else {
                triggerGA('Write_Review', 'Review_Submit_Error', makeModelName + self.detailedReview().trim().length);
            }


            self.focusFormActive(false);
            return isValid;
        };

        self.validate = {
            detailedReview: function () {
                if (self.descLength() < 300) {
                    self.detailedReview(self.detailedReview().trim());
                    self.detailedReviewFlag(true);
                    self.detailedReviewError('Your review should contain at least 300 characters.');
                    self.focusFormActive(true);
                    answer.focusForm(descReviewField);
                }
                else {
                    self.detailedReviewFlag(false);
                }

                return !self.detailedReviewFlag();
            },

            reviewTitle: function () {
                var isValid = false;

                if (self.reviewTitle().trim().length == 0) {
                    validate.setError(reviewTitleField, 'Please provide a title for your review!');
                    if (!self.focusFormActive()) {
                        answer.focusForm(descReviewField);
                    }
                }
                else {
                    validate.hideError(reviewTitleField);
                    isValid = true;
                }

                return isValid;
            }
        };

        self.SaveToBwCache = function () {
            var savearray = new Array;
            $(".list-item input[type='radio']:checked").each(function (i) {
                var r = $(this);
                savearray[i] = (r.attr("questiontId") + ':' + r.val());
            });
            var pageObj = {
                reviewTitle: reviewTitleField.val(),
                detailedReview: self.detailedReview(),
                reviewTips: self.reviewTips(),
                ratingArray: savearray
            };
            bwcache.set(window.location.search.substring(3, window.location.search.length), pageObj, 10);
        };

        self.GetFromBwCache = function () {
            var obj = bwcache.get(window.location.search.substring(3, window.location.search.length));
            if (obj != null) {
                self.detailedReview(obj.detailedReview);
                reviewTitleField.val(obj.reviewTitle);
                reviewTitleField.parent('div').addClass('not-empty');
                $('#getReviewTip').val(obj.reviewTips);

                if (obj.reviewTips && obj.reviewTips != "")
                    $('#getReviewTip').parent('div').addClass('not-empty');

                var i;
                for (i = 0; i < obj.ratingArray.length; ++i) {
                    var quest = obj.ratingArray[i].split(':')[0];
                    var ans = obj.ratingArray[i].split(':')[1];
                    var starbtn = $('#bike-review-questions').find(" input[ratingId=review-" + quest + "-" + ans + "]");
                    if (starbtn != null) {
                        starbtn.trigger("click");
                    }
                }
            }
        };

        self.FillReviewData = function () {
            if ($('#review-page-data').text() != null && $('#review-page-data').text() != "") {
                var obj = JSON.parse($('#review-page-data').text());
                if (obj != null) {
                    self.reviewTips(obj.Tips);
                    var i;

                    for (i = 0; i < obj.Questions.length; ++i) {

                        var quest = obj.Questions[i].qId;
                        var ans = obj.Questions[i].selectedRatingId;
                        var starbtn = $('#bike-review-questions').find("input[ratingId=review-" + quest + "-" + ans + "]");
                        if (starbtn.length != 0) {
                            starbtn.trigger("click");
                        }
                    }
                }
            }
        };

    };

    vmWriteReview = new writeReview(),
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

    descReviewField.on('focus', function () {
        vmWriteReview.detailedReviewFlag(false);
        triggerGA('Write_Review', 'Write_a_Review', makeModelName + ratingOverAll + '_' + pageSourceID);

    });

    reviewTitleField.on("focus", function () {
        validate.onFocus($(this));
        triggerGA('Write_Review', 'Review_Title', makeModelName + ratingOverAll + '_' + pageSourceID);

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

                feedbackText = vmWriteReview.reviewQuestions()[elementIdArray[1]].rating[elementIdArray[2] - 1].ratingText;

                return feedbackText;
            }
        }
    }

    if ($("#reviewDesc") && $("#reviewDesc").data("validate") && $("#reviewDesc").data("validate").length)
        vmWriteReview.validate.detailedReview();

    if ($("#getReviewTitle") && $("#getReviewTitle").data("validate") && $("#getReviewTitle").data("validate").length)
        vmWriteReview.validate.reviewTitle();

    vmWriteReview.GetFromBwCache();
    vmWriteReview.FillReviewData();
    window.onbeforeunload = function () {
        if((!isSubmit && $("#bike-rating-box input[type='radio']:checked").attr("value") != null))
            return false;
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