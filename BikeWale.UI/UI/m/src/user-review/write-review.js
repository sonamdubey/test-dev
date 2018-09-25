var ratingBox, selectedAnswer, page;

var userNameField, userEmailIdField;
var descReviewField, reviewTitleField;
var value_overallrating, reviewQuestion, reviewOverallRatingId, ratingOverAll, pageSourceID, pageSrc, contestSrc;
var vmWriteReview, isSubmit = false;
var makeModelName, ratingErrorFields = "", reviewErrorFields = "";
var array_rating;
var bikeRating = {
    ratingCount: 0,
    overallRating: []
};

var ratingQuestion = [];
var ratingError = false, questionError = false, userNameError = false, emailError = false;
var reviewFlag = false;

function removeMaliciousCode(text) {
    if (!text)
        return text;
    var regex = /<script[^>]*>[\s\S]*?<\/script\s*>/gi;
    while(regex.test(text)) {
        text = text.replace(regex, "");
        }
    return text;
        }

docReady(function () {

    bwcache.setScope('ReviewPage');
    window.history.pushState('ReviewPage', '', '');

    $(window).on('popstate', function (event) {

        if (page == "writeReview" && $("#previousPageUrl") && $("#previousPageUrl").length) {
            window.location.href = $('#previousPageUrl').text();
        }
        else if (page == "reviewSummary" && $("#pageSource") && $("#pageSource").length) {
            window.location.href = $("#pageSource").val();
        }

    });

    ratingBox = $('#bike-rating-box');

    if ($("#overallratingQuestion") && $("#overallratingQuestion").length)
        bikeRating.overallRating = JSON.parse(Base64.decode($('#overallratingQuestion').val()));
    if ($("#rating-question") && $("#rating-question").length)
        ratingQuestion = JSON.parse(Base64.decode($('#rating-question').val()));

    if ($('#review-question-list').text())
        reviewQuestion = JSON.parse($('#review-question-list').text());

    if ($('#reviewedoverallrating') && $('#reviewedoverallrating').length)
        reviewOverallRatingId = Number($('#reviewedoverallrating').val());
    if ($('#overAllRatings') && $('#overAllRatings').length)
        ratingOverAll = Number($('#overAllRatings').val());

    if ($('#pageSourceId') && $('#pageSourceId').length)
        pageSourceID = Number($('#pageSourceId').val());

    if ($('#contestSrc') && $('#contestSrc').length)
        contestSrc = Number($('#contestSrc').val());

    if (contestSrc > 0)
        pageSrc = 'reviewcontest_' + contestSrc;
    else
        pageSrc = pageSourceID;

    if (document.getElementById("bike-rating-box") != null && document.getElementById("bike-rating-box").getAttribute("data-make-model")) {
        makeModelName = document.getElementById("bike-rating-box").getAttribute("data-make-model");
    }
    if (document.getElementById("bike-review-questions") != null && document.getElementById("bike-review-questions").getAttribute("data-make-model")) {
        makeModelName = document.getElementById("bike-review-questions").getAttribute("data-make-model");
    }

    // rate bike
    var rateBike = function () {
        var self = this;

        self.ratingCount = ko.observable(0);
        self.feedbackTitle = ko.observable('Rate this bike');
        self.feedbackSubtitle = ko.observable('');
        self.validateRatingCountFlag = ko.observable(false);
        self.ratingErrorText = ko.observable('');

        self.personalDetails = ko.observable(new personalDetails);

        self.focusFormActive = ko.observable(false);

        self.bikeRating = ko.observable(bikeRating);
        self.overallRating = ko.observableArray(self.bikeRating().overallRating);
        self.ratingQuestions = ko.observableArray(ratingQuestion);
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
                triggerGA('Rate_Bike', 'Rating_Submit_Success', makeModelName + pageSrc);
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
                triggerGA('Rate_Bike', 'Rating_Submit_Error', makeModelName + pageSrc + ratingErrorFields);
               }

            return isValid;
        };

        self.validate = {
            ratingCount: function () {
                if (self.ratingCount() == 0) {
                    self.validateRatingCountFlag(true);
                    self.ratingErrorText("Please rate the bike before submitting!");
                    self.focusFormActive(true);
                    answer.focusForm(ratingBox);
                    ratingError = true;
                    return false;
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
                    userNameError = true;
                    validate.setError(userNameField, 'Please enter your name');
                }
                else if (!(/^[A-Za-z'-\s]+$/.test(vmRateBike.userName()))) {
                    userNameError = true;
                    validate.setError(userNameField, 'Invalid name');
                }
                else if (nameLength >= 1) {
                    validate.hideError(userNameField);
                    isValid = true;
                    userNameError = false;
                }
            }
            else {
                userNameError = true;
                validate.setError(userNameField, 'Please enter your name');
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
        var buttonValue = Number($(this).val());

        var headingText = vmRateBike.overallRating()[buttonValue - 1].heading,
            descText = vmRateBike.overallRating()[buttonValue - 1].description; // since value starts from 1 and array from 0

        vmRateBike.feedbackTitle(headingText);
        vmRateBike.feedbackSubtitle(descText);
        vmRateBike.ratingCount(buttonValue);
        triggerGA('Rate_Bike', 'Stars_Rating_Clicked', makeModelName + buttonValue + '_' + contestSrc);

		updateStarZeroIcon($(this));
    });

	function updateStarZeroIcon(button){
		var list = $(button).closest('.answer-star-list');

		if($(button).val() > 0 && !list.hasClass('rating-start')) {
			list.addClass('rating-start');
		}
	}

    $('#rate-bike-questions').find('.question-type-text input[type=radio]').change(function () {
        var questionField = $(this).closest('.question-type-text'),
            subQuestionId = Number(questionField.attr('data-sub-question'));

       questionField.find('.error-text').hide();

        if (subQuestionId > 0) {
            var buttonValue = Number($(this).val()),
                subQuestionField = $('#question-' + subQuestionId);

            if (buttonValue == 1) {
                subQuestionField.hide();
                subQuestionField.removeAttr('data-required');
            }
            else {
                subQuestionField.find('.error-text').hide();
                subQuestionField.attr('data-required', true);
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

    descReviewField = $('#reviewDesc');
    reviewTitleField = $('#getReviewTitle');

    // write review
    var writeReview = function () {
        var self = this;

        self.reviewCharLimit = ko.observable(300);
        self.reviewTitle = ko.observable('');
        self.detailedReview = ko.observable('');
        self.reviewTips = ko.observable('');

		self.bikeMileage = ko.observable('');

        self.detailedReviewFlag = ko.observable(false);
        self.detailedReviewError = ko.observable('');
        self.focusFormActive = ko.observable(false);
        self.reviewQuestions = ko.observableArray(reviewQuestion);

        self.descLength = ko.computed(function () {
            return self.detailedReview().replace(/\n|\r/g, "").length;
        });
        self.reviewCheckbox = ko.observable(true);
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
                    // sentence case expression
                    var rg = /(^\w{1}|\.\s*\w{1})/gi;
                    descArray[i] = descArray[i].toLowerCase().replace(rg, function (toReplace) {
                        return toReplace.toUpperCase();
                    });

                    formattedDescArray += "<p>" + descArray[i] + "</p>";
                }
            }



            // sentence case expression title and review
            // sentence case expression title and review
            if ($("#getReviewTitle").length > 0) {
                var rg = /(^\w{1}|\.\s*\w{1})/gi;

                $("#getReviewTitle").val($("#getReviewTitle").val().toLowerCase().replace(/[\/\\#,_@^+()$~%'":*?<>{}]/g, '').replace(rg, function (toReplace) {
                    return toReplace.toUpperCase();
                }));
            }
            if ($("#getReviewTip").length > 0) {
                var rg = /(^\w{1}|\.\s*\w{1})/gi;
                $("#getReviewTip").val($("#getReviewTip").val().toLowerCase().replace(/[\/\\#,_@^+()$~%'":*?<>{}]/g, '').replace(rg, function (toReplace) {
                    return toReplace.toUpperCase();
                }));
            }

            if ($('#formattedDescripton'))
                $('#formattedDescripton').val(formattedDescArray);

            var isValidMileage = true;
            if (self.bikeMileage().length > 0)
                isValidMileage = $.isNumeric(self.bikeMileage()) && Number(self.bikeMileage()) <= 150 && Number(self.bikeMileage()) >= 0;

             if (isValidMileage) {
                 if (self.detailedReview().length > 0 || self.reviewTitle().length > 0) {
                     if (self.validateReviewForm()) {
                         return true;
                     }
                 }
                 else {
                     self.detailedReviewFlag(false);
                     
                     validate.hideError(reviewTitleField);
                     triggerGA('Write_Review', 'Review_Submit_Success', makeModelName + pageSrc + '_' + (self.detailedReview().trim().length > 0) + '_' + self.detailedReview().trim().length);
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
                triggerGA('Write_Review', 'Review_Submit_Success', makeModelName + pageSrc + '_' + (self.detailedReview().trim().length > 0) + '_' + self.detailedReview().trim().length);
                var questionResponse = $('#AnswerText').val();
                if (typeof questionResponse !== "undefined" && questionResponse.length > 0)
                {
                    triggerGA('Write_Review', 'Review_Submit_Success_Answer', makeModelName);
                }
            }
            else {

                triggerGA('Write_Review', 'Review_Submit_Error', makeModelName + pageSrc + '_' + (self.detailedReview().trim().length > 0) + '_' + self.detailedReview().trim().length + reviewErrorFields);
            }

            self.focusFormActive(false);
            return isValid;
        };

        self.validate = {
            detailedReview: function () {
                self.detailedReview(removeMaliciousCode(self.detailedReview().trim()));
                if (self.descLength() < 300) {
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
                reviewTips: $('#getMileage').val(),
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
                reviewTitleField.parent('div').addClass('not-empty');
                self.reviewTips(obj.reviewTips);
                self.bikeMileage(obj.mileage);
                var i;
                for (i = 0; i < obj.ratingArray.length; ++i) {
                    var quest = obj.ratingArray[i].split(':')[0];
                    var ans = obj.ratingArray[i].split(':')[1];
                    var starbtn = $('#bike-review-questions').find(" input[questiontid=" + quest + "][id=review-" + quest + "-" + ans + "]");
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
                    self.bikeMileage(obj.Mileage);
                    self.reviewTips(obj.Tips);
                    var i;

                    for (i = 0; i < obj.Questions.length; ++i) {

                        var quest = obj.Questions[i].qId;
                        var ans = obj.Questions[i].selectedRatingId;
                        var starbtn = $('#bike-review-questions').find("input[id=review-" + quest + "-" + ans + "]");
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

		updateStarZeroIcon(button);
    });

    descReviewField.on('focus', function () {
        vmWriteReview.detailedReviewFlag(false);
        triggerGA('Write_Review', 'Write_a_Review', makeModelName + ratingOverAll + '_' + pageSrc);
        answer.focusForm($('#guidelineList'));
    });

    reviewTitleField.on("focus", function () {
        validate.onFocus($(this));
        triggerGA('Write_Review', 'Review_Title', makeModelName + ratingOverAll + '_' + pageSrc);
    });

    reviewTitleField.on("blur", function () {
        validate.onBlur($(this));
    });

    if (descReviewField && descReviewField.data("validate") && descReviewField.data("validate").length)
        vmWriteReview.validate.detailedReview();

    if (reviewTitleField && reviewTitleField.data("validate") && reviewTitleField.data("validate").length)
        vmWriteReview.validate.reviewTitle();

    vmWriteReview.GetFromBwCache();

    vmWriteReview.FillReviewData();

    var selRating = $("#bike-rating-box").attr("data-selectedRating");
    if (selRating > 0) {
        ratingBox.find('.answer-star-list input[type=radio][value="' + selRating + '"]').trigger("click");
    }

    window.onbeforeunload = function () {
        if (!isSubmit && $("#bike-rating-box input[type='radio']:checked").attr("value") != null)
            return false;
    }

    // Review Summary
    $(".review-summary__link").on('click', function () {
        var self, parentElement, parentElementPosition;
        self = $(this);
        parentElement = self.closest('.review-summary__wrapper');
        parentElementPosition = parentElement.offset().top;
        parentElement.addClass('review-summary--active');
        $('html, body').animate({
            scrollTop: parentElementPosition
        });
    });

    $(".answer-question__item a").on('click', function () {
        var makeModel = $('#makeName').val() + "_" + $('#modelName').val();
        if ($('#reviewTitle').val().length > 0)
        {
            triggerGA("Write_Review", "Answer_This_Question_Link_Clicked", makeModel);
        }
        else
        {
            triggerGA("Rate_Bike", "Answer_This_Question_Link_Clicked", makeModel);
        }
        
    });

    // Text Area
    if ($('#answerQuestionBox').length) {
        TextArea.registerEvents();
    }
});

var answer = {
    selection: function (element) {
        $(element).siblings().removeClass('active');
        $(element).addClass('active');
    },

    focusForm: function (element) {
        try {
            $('html, body').animate({ scrollTop: $(element).offset().top }, 500);
        }
        catch (e) {
            console.warn(e.message);
        }
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