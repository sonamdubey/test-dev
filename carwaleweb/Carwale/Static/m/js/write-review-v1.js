var userNameField, userEmailIdField;
var descReviewField, reviewTitleField;
var totalLimitSpan;
var totalCharsTillNowSpan;
var charCountDiv;
var submitReviewButton;
var reviewQuestionList;
var offsetScroll = 20;
var headerHeight = 0;
var submitAPI = '/api/v1/userreviews/';
var reviewForm;
var reviewResponse;
var reviewId;
var responseMessage;
var IsReviewsubmitted;
var minLength = {
    title: 1,
    description: 300
}
var errorMessages = {
    title: 'Please provide a title for your review.',
    description: 'Your review should contain at least 300 characters.'
}

var answer = {
    selection: function (element) {
        $(element).siblings().removeClass('active');
        $(element).addClass('active');
    },
    focusForm: function (element, offsetTop) {
        try {
            offsetTop = (typeof offsetTop) == "undefined" ? 0 : offsetTop;
            $('html, body').animate({ scrollTop: $(element).offset().top - offsetTop }, 500);
        }
        catch (e) {
            console.warn(e.message);
        }
    }
};

var showFeedbackText = function (event) {
    var parent = $(this).closest(".answer-star-list");
    var checkedState = parent.find("input[type=radio]:checked").length,
		button = $(this).prev("input[type='radio']"),
		questionField = button.closest('.question-type-star'),
		feedbackText = "";
    if (event.type === "click" || event.type === "mouseenter") {
        feedbackText = $(this).attr('data-text');
        parent.parent().find('.error-text').hide();
    }
    if (checkedState && event.type === "mouseleave") {
        feedbackText = parent.find('input[type=radio]:checked').attr('data-text');
        parent.parent().find('.error-text').hide();
    }
    if (!checkedState && event.type === "mouseleave") {
        if (parent.data('errorMsgShow')) {
            parent.parent().find('.error-text').show();
        }
    }
    questionField.find('.feedback-text').text(feedbackText);
}
var cacheSelectors = function () {
    // cache jquery elements to variables
    totalLimitSpan = $("#totalCharsLimit");
    totalCharsTillNowSpan = $("#totalCharsTillNow");
    charCountDiv = $("#charCountDiv");
    descReviewField = $('#reviewDesc');
    reviewTitleField = $('#getReviewTitle');
    submitReviewButton = $('#submitReviewBtn');
    reviewQuestionList = $('#car-review-questions');
    reviewForm = $('#write-review-form');
    reviewResponse = $('#write-review-response');
    responseMessage = $('#responsemessage');
}

var triggerCountValidation = function () {
    var currentLength = this.value.trim().length;
    totalLimitSpan.text(minLength.description);
    if (currentLength < minLength.description) {
        charCountDiv.show();
        totalCharsTillNowSpan.text(currentLength);
    }
    else {
        charCountDiv.hide();
    }
}
var showDescriptionError = function (element, message) {
    var errorTag = element.parent().find('span.error-text');
    errorTag.show().text(message);
    charCountDiv.hide();
}
var hideDescriptionError = function (element) {
    var errorTag = $(element).parent().find('span.error-text');
    errorTag.hide();
    $(element).trigger("input");
}
var reviewValidations = {
    description: function (errorLabel) {
        if (descReviewField.val().trim().length < minLength.description) {
            showDescriptionError(descReviewField, errorMessages.description);
            errorLabel.label = errorLabel.label + ((errorLabel.label) ? ',description' : 'description');
            descReviewField.data('errorMsgShow', 'show');
            return false;
        }
        return true;
    },
    title: function (errorLabel) {
        if (reviewTitleField.val().trim().length < minLength.title) {
            validate.setError(reviewTitleField, errorMessages.title);
            errorLabel.label = errorLabel.label + ((errorLabel.label) ? ',title' : 'title');
            reviewTitleField.data('errorMsgShow', 'show');
            return false;
        }
        return true;
    },
    moreRatings: function (errorLabel) {
        var questionFields = reviewQuestionList.find('.question-field'),
			questionLength = questionFields.length,
			errorCount = 0;

        for (var i = 0; i < questionLength; i++) {
            var item = $(questionFields[i]);
            var checkedRadioButton = item.find('.answer-star-list input[type=radio]:checked');

            if (!checkedRadioButton.length) {
                item.find('.answer-star-list').data('errorMsgShow', 'show');
                item.find('.error-text').show();
                errorLabel.label = errorLabel.label + ((errorLabel.label) ? ',ques' : 'ques') + (i + 1);
                errorCount++;
            }
            else {
                item.find('.error-text').hide();
            }
        }

        if (errorCount == 0) {
            return true;
        }
        return false;
    }
}
var getQuestions = function () {
    var questionsArray = []
    reviewQuestionList.find('input[type=radio]:checked').each(function () {
        questionsArray.push({
            'questionId': $(this).attr('questiontId'),
            'answerId': $(this).attr('value')
        });
    });
    return questionsArray;
}
var submitReview = function () {
    Loader.showFullPageLoader();
    var questions = getQuestions();
    var requestBody = {
        "reviewId": reviewId,
        "description": descReviewField.val().trim(),
        "title": reviewTitleField.val().trim(),
        "questions": questions
    };
    IsReviewsubmitted = true;
    $.ajax({
        url: submitAPI,
        headers: { 'CWK': 'KYpLANI09l53DuSN7UVQ304Xnks=', 'SourceId': '1' },
        type: 'PUT',
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(requestBody),
        success: function (data) {
            cwTracking.trackCustomData(eventCategory, "ReviewSubmitSuccess", eventLabel + '|reviewId=' + decryptedReviewId, false);
            reviewForm.hide();
            var responseObj = jQuery.parseJSON(JSON.stringify(data));
            reviewResponse.find('.thankyou__title').html(responseObj.heading);
            reviewResponse.find('.thankyou__subtitle').html(responseObj.message);
            reviewResponse.show();
            answer.focusForm("body");
            setTimeout(Loader.hideFullPageLoader, 500);
        },
        error: function () {
            cwTracking.trackCustomData(eventCategory, "ReviewSubmitError", eventLabel + '|errorlabel=erroroccured', false);
            responseMessage.text("Error Occurred. Please try again later.");
            setTimeout(Loader.hideFullPageLoader, 500);
        }
    });
};

//find index of reviewId element
var findReviewId = function (reviewArray) {
    var reviewLen = reviewArray ? reviewArray.length : 0;
    for (var index = 0; index < reviewLen; index++) {
        if (reviewArray[index].reviewId == reviewId)
            return index;
    }
    return -1;
};

//prefill data if available at local storage
var preFillReview = function () {
    var reviewValue = clientCache.get("reviewHistoryData");
    var historyReviewData = ((reviewValue) ? reviewValue.val : null);
    if (historyReviewData) {
        var index = findReviewId(historyReviewData);
        if (index >= 0) {
            var versionReviewDetails = historyReviewData[index];
            reviewTitleField.val(versionReviewDetails["title"]);
            descReviewField.val(versionReviewDetails["description"]);
            versionReviewDetails["questions"].forEach(function (element) {
                if (element["answerId"]) {
                    $("#review-" + element["questionId"] + "-" + element["answerId"]).prop("checked", true);
                    var feedbackText = $("#review-" + element["questionId"] + "-" + element["answerId"]).attr("data-text");
                    $("#js-feedback-text-quest-" + element["questionId"]).text(feedbackText);
                }
            });
        }
    }
};

// to create reviewId object 
var setReviewHistoryObject = function () {
    var versionReviewData = {};
    versionReviewData.reviewId = reviewId;
    versionReviewData.title = reviewTitleField.val()
    versionReviewData.description = descReviewField.val();
    versionReviewData.questions = getQuestions();
    return versionReviewData;
};

//save unmodified review data
var saveUnsubmittedReviewData = function () {
    var reviewValue = clientCache.get("reviewHistoryData");
    var historyReviewData = ((reviewValue) ? reviewValue.val : null);
    if (!historyReviewData) {
        historyReviewData = [
           setReviewHistoryObject()
        ];
    }
    else {
        var index = findReviewId(historyReviewData);
        if (index < 0) {
            historyReviewData.push(setReviewHistoryObject());
        }
        else {
            historyReviewData[index] = setReviewHistoryObject();
        }
    }
    clientCache.set({ key: "reviewHistoryData", value: historyReviewData, expiryTime: parseInt(expiryTime) }); //set expiryTime 10 days
};

var handleReviewPagechange = function () {
    IsReviewsubmitted = false;
    var promptMessage = "Leave Page ?\nChanges that you made may not be saved.";
    var promptOnLeave = function () {
        saveUnsubmittedReviewData();
        if (!IsReviewsubmitted) {
            if (event) {
                event.returnValue = promptMessage;
            }
            return promptMessage;
        }
    }
    window.onbeforeunload = promptOnLeave
};


var submitButtonClicked = function (event) {
    var descriptionValid = true,
		titleValid = true,
		ratingValid = true;
    var errorLabel = { label: '' };
    descriptionValid = reviewValidations.description(errorLabel);
    titleValid = reviewValidations.title(errorLabel);
    ratingValid = reviewValidations.moreRatings(errorLabel);
    if (descriptionValid && titleValid && ratingValid) {
        submitReview();
    }
    else {
        errorLabel.label = ((errorLabel.label) ? '|errorlabel=' + errorLabel.label : '');
        cwTracking.trackCustomData(eventCategory, "ReviewSubmitInvalid", eventLabel + errorLabel.label, false);
        if (!descriptionValid) {
            answer.focusForm(descReviewField, headerHeight + offsetScroll);
        }
        else if (!titleValid) {
            answer.focusForm(reviewTitleField, headerHeight + offsetScroll);
        }
        else {
            answer.focusForm(reviewQuestionList, headerHeight);
        }
    }
};


var triggerMoreRatingChange = function () {
    var button = $(this),
		questionField = button.closest('.question-type-star');

    var feedbackText = $(this).attr('data-text');
    questionField.find('.feedback-text').text(feedbackText);
    button.parent().parent().find('.error-text').hide();
};

$(document).ready(function () {
    history.scrollRestoration = 'manual';// written to avoid page flicker
    cacheSelectors();
    descReviewField.on("input", triggerCountValidation);

    reviewId = $('#id').val();

    var answerStarList = reviewQuestionList.find('.answer-star-list');
    answerStarList.find('input[type=radio]').on('change', triggerMoreRatingChange);
    answerStarList.find('label').on('click', showFeedbackText);

    reviewTitleField.on("focus", function () {
        validate.onFocus($(this));
    });

    descReviewField.on("focus", function () {
        hideDescriptionError(this);
    });

    reviewTitleField.on("change", function () {
        if (reviewTitleField.val().trim().length < minLength.title && reviewTitleField.data('errorMsgShow')) {
            validate.setError(reviewTitleField, errorMessages.title);
        }
    });
    descReviewField.on("blur", function () {
        if (descReviewField.val().trim().length < minLength.description && descReviewField.data('errorMsgShow')) {
            showDescriptionError(descReviewField, errorMessages.description);
        }
    });

    submitReviewButton.on('click', submitButtonClicked);
    if (!isMobileDevice) {
        $(document).on("hover", ".answer-star-list label", showFeedbackText);
        headerHeight += 50;
    }
    handleReviewPagechange();
    preFillReview();
});

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
