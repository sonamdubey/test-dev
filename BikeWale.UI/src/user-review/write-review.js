var ratingBox, page, userNameField, userEmailIdField, vmWriteReview, vmRateBike;
var detailedReviewField, reviewTitleField, reviewQuestion, ratingOverAll, pageSourceID, contentSourceId;
var isSubmit = false, reviewOverallRatingId, bikeRatingBox;
var makeModelName, ratingErrorFields = "", reviewErrorFields = "", writeReviewForm, descReviewField;
var bikeRating = {
    ratingCount: 0,
    overallRating: []
};

var ratingQuestion = [];
var ratingError = false, questionError = false, userNameError = false, emailError = false;

function removeMaliciousCode(text) {
    if (!text)
        return text;
    var regex = /<script[^>]*>[\s\S]*?<\/script\s*>/gi;
    while (regex.test(text)) {
        text = text.replace(regex, "");
    }
    return text;
}

function initKoSlider() {
    ko.bindingHandlers.KOSlider = {
        init: function (element, valueAccessor, allBindingsAccessor) {
            var options = allBindingsAccessor().sliderOptions || {};
            var observable = valueAccessor();

            options.slide = function (e, ui) {
                observable(ui.value);
                $('.carousel-type-questions .jcarousel-control-next').addClass("inactive").attr("data-mileagechanged", 1);
            };

            ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                $(element).slider("destroy");
            });

            $(element).slider(options);
        },
        update: function (element, valueAccessor) {
            var value = ko.utils.unwrapObservable(valueAccessor());
            if (value) {
                $(element).slider(value);
                $(element).change();
            }

        }
    };

    ko.bindingHandlers.slideIn = {
        update: function (element, valueAccessor) {
            var value = ko.utils.unwrapObservable(valueAccessor());
            value ? $(element).show() : $(element).fadeOut(200);
        }
    };

    ko.bindingHandlers.hoverRating = {
        update: function (element, valueAccessor) {
            ko.utils.registerEventHandler(element, "mouseover", function () {
                bikeRatingBox.setFeedback($(this));
            });
            ko.utils.registerEventHandler(element, "mouseout", function () {
                bikeRatingBox.resetFeedback($(this));
            });
        }
    };
};

// rate bike
var rateBike = function () {

    var self = this;

    self.ratingCount = ko.observable(0);
    self.feedbackTitle = ko.observable('');
    self.feedbackSubtitle = ko.observable('');

    self.validateRatingCountFlag = ko.observable(false);
    self.ratingErrorText = ko.observable('');
    self.focusFormActive = ko.observable(false);

    self.personalDetails = ko.observable(new personalDetails);

    self.bikeRating = ko.observable(bikeRating);
    self.overallRating = ko.observableArray(self.bikeRating().overallRating);
    self.ratingQuestions = ko.observableArray(ratingQuestion);

    self.clickEventRatingCount = ko.observable(0);
    self.userName = ko.observable(userNameField.val());
    self.emailId = ko.observable(userEmailIdField.val());

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
            if (array_cookie[1] != null && array_cookie[1] != "undefined" && userEmailIdField.val() == "") {
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
            vmRateBike.clickEventRatingCount(reviewOverallRatingId);
            ratingBox.find('.answer-star-list input[type=radio][value="' + reviewOverallRatingId + '"]').trigger("click");
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

    };

    self.submitRating = function () {
        if (self.validateRateBikeForm()) {
            var array_rating = new Array(),
            value_overallrating = $("#bike-rating-box input[type='radio']:checked").attr("value");
            $("#rate-bike-questions input[type='radio']:checked").each(function (i) {
                var r = $(this);
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

        self.userName($('#txtUserName').val());
        self.emailId($('#txtEmailID').val());

        isValid = self.validate.ratingCount();
        isValid &= self.validate.ratingForm();
        isValid &= self.personalDetails().validateDetails();

        if (isValid) {
            triggerGA('Rate_Bike', 'Rating_Submit_Success', makeModelName + pageSourceID);
        }
        else {
            ratingErrorFields = "";
            if (ratingError)
                ratingErrorFields = ratingErrorFields + '_' + 'Rating_Field';
            if (questionError)
                ratingErrorFields = ratingErrorFields + '_' + 'Questions_Field';
            if (userNameError)
                ratingErrorFields = ratingErrorFields + '_' + 'Name_Field';
            if (emailError)
                ratingErrorFields = ratingErrorFields + '_' + 'Email_Field';
            triggerGA('Rate_Bike', 'Rating_Submit_Error', makeModelName + pageSourceID + ratingErrorFields);
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

var writeReview = function () {
    var self = this;
    self.reviewCharLimit = ko.observable(300);
    self.reviewTitle = ko.observable('');
    self.detailedReview = ko.observable('');
    self.reviewTips = ko.observable('');
    self.detailedReviewFlag = ko.observable(false);
    self.detailedReviewError = ko.observable('');
    self.focusFormActive = ko.observable(false);
    self.bikeMileage = ko.observable();
    self.reviewQuestions = ko.observableArray(reviewQuestion);
    self.reviewCheckbox = ko.observable(true);
    self.descLength = ko.computed(function () {
        return self.detailedReview().replace(/\n|\r/g, "").length ;
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

        for (var i = 0; i < descArray.length; i++) {
            if (descArray[i].trim() != "") {
                // sentence case expression
                var rg = /(^\w{1}|\.\s*\w{1})/gi;
                descArray[i] = descArray[i].toLowerCase().replace(rg, function (toReplace) {
                    return toReplace.toUpperCase();
                });

                formattedDescArray += "<p>" + descArray[i] + "</p>";
            }
        }

        // sentence case expression title and review
        var rg = /(^\w{1}|\.\s*\w{1})/gi;
        if ($("#getReviewTitle").length > 0) {

            $("#getReviewTitle").val($("#getReviewTitle").val().toLowerCase().replace(/[\/\\#,_@^+()$~%'":*?<>{}]/g, '').replace(rg, function (toReplace) {
                return toReplace.toUpperCase();
            }));
        }
        if ($("#getReviewTip").length > 0) {
            $("#getReviewTip").val($("#getReviewTip").val().toLowerCase().replace(/[\/\\#,_@^+()$~%'":*?<>{}]/g, '').replace(rg, function (toReplace) {
                return toReplace.toUpperCase();
            }));
        }

        if ($('#formattedDescripton'))
            $('#formattedDescripton').val(formattedDescArray);

        var isValidMileage = true;
        if (self.bikeMileage() && self.bikeMileage().length > 0)
            isValidMileage = $.isNumeric(self.bikeMileage()) && Number(self.bikeMileage()) <= 150 && Number(self.bikeMileage()) >= 0;

        if (isValidMileage) {

            if ($('#reviewDesc').length > 0) {
                if (self.validateReviewForm()) {
                    return true;
                }
            }
            else {
                self.detailedReviewFlag(false);
                validate.hideError(reviewTitleField);
                triggerGA('Write_Review', 'Review_Submit_Success', makeModelName + pageSourceID + '_' + (self.detailedReview().trim().length > 0) + '_' + self.detailedReview().trim().length);
                return true;
            }
        }
        else {

            validate.setError($('#getMileage'), 'Please enter a valid mileage in kmpl.');
            answer.focusForm($('#getMileage'));
        }

    };

    self.validateReviewForm = function () {
        var isValidDesc = self.validate.detailedReview();
        var isValidTitle = self.validate.reviewTitle();


        reviewErrorFields = "";
        if (isValidTitle && !isValidDesc)
            reviewErrorFields = reviewErrorFields + '_' + 'Review_Description';
        else if (!isValidTitle && isValidDesc)
            reviewErrorFields = reviewErrorFields + '_' + 'Review_Title';
        else if (!isValidDesc && !isValidTitle)
            reviewErrorFields = reviewErrorFields + '_' + 'Review_Title' + '_' + 'Review_Description';

        var isValid = isValidDesc && isValidTitle && self.reviewCheckbox();
        if (isValid) {
            triggerGA('Write_Review', 'Review_Submit_Success', makeModelName + pageSourceID + '_' + (self.detailedReview().trim().length > 0) + '_' + self.detailedReview().trim().length);
        }
        else {
            triggerGA('Write_Review', 'Review_Submit_Error', makeModelName + pageSourceID + '_' + (self.detailedReview().trim().length > 0) + '_' + self.detailedReview().trim().length + reviewErrorFields);
        }

        self.focusFormActive(false);
        return isValid;
    };

    self.validate = {
        detailedReview: function () {
            self.detailedReview(removeMaliciousCode(self.detailedReview()).trim());
            
            if (self.descLength() < 300) {
                self.detailedReviewFlag(true);

                if (self.descLength() == 0)
                    self.detailedReviewError('This is a compulsory field!');
                else
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

                if (self.descLength() == 0)
                    validate.setError(reviewTitleField, 'This is a compulsory field!');
                else
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
            mileage: self.bikeMileage(),
            ratingArray: savearray
        };
        bwcache.set(window.location.search.substring(3, window.location.search.length), pageObj, 10);
    };

    self.GetFromBwCache = function () {
        var obj = bwcache.get(window.location.search.substring(3, window.location.search.length));
        if (obj != null) {
            self.detailedReview(obj.detailedReview);
            reviewTitleField.val(obj.reviewTitle);
            self.bikeMileage(obj.mileage);
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

    self.updateOptionRatings = function (d, e) {
        var selBox = $(e.target), qId = $("#" + selBox.attr("for")).attr("questiontId"), aId = selBox.attr("data-value");
        var objData = {
            reviewId: $("#reviewId").val(),
            encodedId: $("#encodedId").val(),
            qamapping: qId + ":" + aId
        };

        $.ajax({
            type: 'POST',
            url: '/api/user-reviews/parameter-ratings/save/',
            data: JSON.stringify(objData),
            contentType: 'application/json',
            dataType: 'json'
        });
        return true;
    };

    self.updateMileage = function(d,e)
    {
        var objData = {
            reviewId: $("#reviewId").val(),
            encodedId: $("#encodedId").val(),
            mileage: self.bikeMileage()
        };

        $.ajax({
            type: 'POST',
            url: '/api/user-reviews/parameter-ratings/save/',
            data: JSON.stringify(objData),
            contentType: 'application/json',
            dataType: 'json'
        });
        return true;
    }

};

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

docReady(function () {

    bwcache.setScope('ReviewPage');
    window.history.pushState('ReviewPage', '', '');
    ratingBox = $('#bike-rating-box');
    descReviewField = $('#reviewDesc');
    reviewTitleField = $('#getReviewTitle');
    userNameField = $('#txtUserName');
    userEmailIdField = $('#txtEmailID');

    initKoSlider();


    if ($('#review-question-list') && $('#review-question-list').text())
        reviewQuestion = JSON.parse($('#review-question-list').text());


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

    if ($('#contestSrc') && $('#contestSrc').length)
        contentSourceId = Number($('#contestSrc').val());


    if (ratingBox != null && ratingBox.attr("data-make-model")) {
        makeModelName = ratingBox.attr("data-make-model");
    }

    if (document.getElementById("bike-review-questions") != null && document.getElementById("bike-review-questions").getAttribute("data-make-model")) {
        makeModelName = document.getElementById("bike-review-questions").getAttribute("data-make-model");
    }

    if ($("#reviewDesc") && $("#reviewDesc").data("validate") && $("#reviewDesc").data("validate").length)
        vmWriteReview.validate.detailedReview();

    if ($("#getReviewTitle") && $("#getReviewTitle").data("validate") && $("#getReviewTitle").data("validate").length) {

        vmWriteReview.validate.reviewTitle();
    }

   

    rateBikeForm = document.getElementById('rate-bike-form');

    if (rateBikeForm) {
        vmRateBike = new rateBike();
        ko.applyBindings(vmRateBike, rateBikeForm);
        vmRateBike.init();
    }

    ratingBox.find('.answer-star-list input[type=radio]').change(function () {
        if (vmRateBike)
        {
            var button = $(this),
           buttonValue = Number(button.val());

            var headingText = vmRateBike.overallRating()[buttonValue - 1].heading,
                descText = vmRateBike.overallRating()[buttonValue - 1].description; // since value starts from 1 and array from 0

            vmRateBike.feedbackTitle(headingText);
            vmRateBike.feedbackSubtitle(descText);
            vmRateBike.ratingCount(buttonValue);
            vmRateBike.clickEventRatingCount(buttonValue);
            triggerGA('Rate_Bike', 'Stars_Rating_Clicked', makeModelName + buttonValue + '_' + contentSourceId);
        }       

    });
    var selRating = ratingBox.attr("data-selectedrating");
    if (selRating > 0) {
        ratingBox.find('.answer-star-list input[type=radio][value="' + selRating + '"]').trigger("click");
    }

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

    bikeRatingBox = {
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
                vmRateBike.feedbackTitle('');
            }
            else {
                vmRateBike.ratingCount(vmRateBike.clickEventRatingCount());
                vmRateBike.feedbackTitle(vmRateBike.overallRating()[vmRateBike.clickEventRatingCount() - 1].heading);
                vmRateBike.feedbackSubtitle(vmRateBike.overallRating()[vmRateBike.clickEventRatingCount() - 1].description);
            }
        }
    }
    // write review
     writeReviewForm = document.getElementById('write-review-form');
     if (writeReviewForm) {
         vmWriteReview = new writeReview();
        ko.applyBindings(vmWriteReview, writeReviewForm);
        descReviewField.focus();
        vmWriteReview.GetFromBwCache();
        vmWriteReview.FillReviewData();
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


    //rating-face
    $('.other-review__button-block').on('click', function () {
        var next = $('.carousel-type-questions .jcarousel-control-next');
        if (next.attr("data-mileagechanged") > 0) {
            next.removeClass("inactive");
            next.attr("data-mileagechanged", 0);
        }
        next.trigger('click');
    });

    $('.carousel-type-questions').on('change', '.rating-face-list input[type="radio"]', function () {
        $('.carousel-type-questions .jcarousel-control-next').trigger('click');
    })

    $('.carousel-type-questions .jcarousel').on('jcarousel:scroll', function (e) {
        var next = $('.carousel-type-questions .jcarousel-control-next');
        return (!next.attr("data-mileagechanged") || next.attr("data-mileagechanged") <= 0);
    });


    $('.edit-link').on('click', function () {
        if (page == "writeReview" && $("#previousPageUrl") && $("#previousPageUrl").length) {
            window.location.href = $('#previousPageUrl').text();
        }
        else if (page == "otherDetails" && $("#returnUrl") && $("#returnUrl").length) {
            window.location.href = $('#returnUrl').text();
        }
        else if (page == "reviewSummary" && $("#pageSource") && $("#pageSource").length) {
            window.location.href = $("#pageSource").val();
        }
    });

    $(window).on('popstate', function (event) {

        if (page == "writeReview" && $("#previousPageUrl") && $("#previousPageUrl").length) {
            window.location.href = $('#previousPageUrl').text();
        }
        else if (page == "otherDetails" && $("#returnUrl") && $("#returnUrl").length) {
            window.location.href = $('#returnUrl').text();
        }
        else if (page == "reviewSummary" && $("#pageSource") && $("#pageSource").length) {
            window.location.href = $("#pageSource").val();
        }

    });

    window.onbeforeunload = function () {
        if ((!isSubmit && $("#bike-rating-box input[type='radio']:checked").attr("value") != null))
            return false;
    }
});
