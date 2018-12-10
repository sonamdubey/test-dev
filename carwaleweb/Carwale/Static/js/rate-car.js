var ratingData = {
    userDetails: {
        name: $.cookie("_CustomerName") || "",
        email: $.cookie("_CustEmail") || ""
    },
    carDetails: {
        versionId: versionId || 0
    },
    rating: {
        userRating: 0,
        ratingQuestions: [{
            questionId: 1,
            answerId: 0
        },
		{
		    questionId: 2,
		    answerId: 0
		}]
    },
    reviewId: 0
};
var ratingSubmitted;
var feedbackTitle = "Rate your car";
var feedbackSubtitle = '';
var versionName = '';
var currentLogo = 'feedback-0';
var setCurrentLogo = function (id) {
    currentLogo = "feedback-" + id;
}
var responseMeassage;
var submitRating;
var emailError;
var nameError;
var feedbackTitleSelector;
var feedbackSubtitleSelector;
var carRatingBoxError;
var verisonSelectionError;
var txtUserName;
var ratingLogo;
var selectedVersion;
var reviewSection;
var desktopVesrionDropdown;
var platformId = isMobileDevice ? 43 : 1;

var cacheSelectors = function () {
    responseMeassage = $("#responsemessage");
    submitRating = $("#submitrating");
    emailError = $("#emailError");
    nameError = $("#nameError");
    feedbackTitleSelector = $("#feedbackTitle");
    feedbackSubtitleSelector = $("#feedbackSubtitle");
    carRatingBoxError = $('#carRatingBox .error-text');
    verisonSelectionError = $('#verisonSelectionError');
    txtUserName = $("#txtUserName");
    txtEmailID = $("#txtEmailID");
    ratingLogo = $(".rating-logo");
    selectedVersion = $('.js-selected-version__name');
    reviewSection = $("#js-review-section");
    desktopVesrionDropdown = $("#rate-car-form .js-selectcustom-input-box-holder");
}

var rateCarEventCategory = "RateYourCar";
var rateCarEventLabel = 'modelId=' + modelId + '|source=' + platformId;
var rateCar = function () {
    _rateCar = {};
    _rateCar.submitRating = function () {
        Loader.showFullPageLoader();
        submitRating.attr("disabled", true);
        $.ajax({
            url: ("/api/userreviews/rating/"),
            type: 'post',
            contentType: "application/json",
            data: JSON.stringify(ratingData),
            headers: {
                CWK: 'KYpLANI09l53DuSN7UVQ304Xnks=',
                sourceId: platformId,
                platformId: platformId
            },
            success: function (response) {
                ratingSubmitted = true;
                submitRating.removeAttr("disabled");
                if (response.isDuplicate) {
                    responseMeassage.text(response.message);
                    cwTracking.trackCustomData(rateCarEventCategory, 'RatingSubmitDuplicated', rateCarEventLabel + '|reviewId=' + response.rewiewId, false);
                    setTimeout(Loader.hideFullPageLoader, 500);
                }
                else {
                    cwTracking.trackCustomData(rateCarEventCategory, 'RatingSubmitSuccess', rateCarEventLabel + '|reviewId=' + response.rewiewId, false);
                    Loader.hideFullPageLoader();
                    location.href = "/userreviews/write-review/?reviewId=" + response.hash;
                }
            },
            error: function () {
                setTimeout(Loader.hideFullPageLoader, 500);
                submitRating.removeAttr("disabled");
                responseMeassage.text("Error occurred. Please try again later.");
                cwTracking.trackCustomData(rateCarEventCategory, 'RatingSubmitError', rateCarEventLabel + '|errorlabel=erroroccurred', false);
            }
        });
    };

    function ratingValidations() {
        var _ratingValidations = {};

        var validateUserEmail = function (email, errorLabel) {
            var errorMsg = form.validation.checkEmail(email);
            if (errorMsg !== "") {
                if (errorLabel) {
                    errorLabel.label = errorLabel.label + ((errorLabel.label) ? ',' : '') + errorMsg.toLowerCase();
                }
                emailError.text(errorMsg + ".");
                return false;
            }
            else {
                emailError.text('');
                return true;
            }
        }

        var validateUserName = function (name, errorLabel) {
            name = $.trim(name);
            var errorMsg = form.validation.checkName(name);
            if (errorMsg !== "") {
                if (errorLabel) {
                    errorLabel.label = errorLabel.label + ((errorLabel.label) ? ',' : '') + errorMsg.toLowerCase();
                }
                nameError.text(errorMsg + ".");
                return false;
            }
            else {
                nameError.text('');
                return true;
            }
        }

        var isUserLoggedIn = function () {
            if (currentUserEmail != "" && currentUserName != "") {
                ratingData.userDetails.email = currentUserEmail;
                ratingData.userDetails.name = currentUserName;
            }
        }

        _ratingValidations.validateRatingObject = function (errorLabel) {
            var isValid = true;
            var erroneousElement;
            if (!ratingData.carDetails.versionId || ratingData.carDetails.versionId === 0) {
                isvalid = false;
                verisonSelectionError.removeClass('hide');
                errorLabel.label = errorLabel.label + ((errorLabel.label) ? ',version' : 'version');
                erroneousElement = $(".js-scroll-version-error");
            }
            if (ratingData.rating.userRating == 0) {
                carRatingBoxError.removeClass('hide');
                $('#carRatingBox .answer-star-list').data('errorMsgShow', 'show');
                errorLabel.label = errorLabel.label + ((errorLabel.label) ? ',starrating' : 'starrating');
                isValid = false;
                erroneousElement = erroneousElement || $("#carRatingBox label");
            }
            if (ratingData.rating.ratingQuestions[0].answerId == 0) {
                $('#error-question-1').removeClass('hide');
                errorLabel.label = errorLabel.label + ((errorLabel.label) ? ',ques1' : 'ques1');
                isValid = false;
                erroneousElement = erroneousElement || $("#error-question-1");
            }

            if (ratingData.rating.ratingQuestions[1].answerId == 0) {
                $('#error-question-2').removeClass('hide');
                errorLabel.label = errorLabel.label + ((errorLabel.label) ? ',ques2' : 'ques2');
                isValid = false;
                erroneousElement = erroneousElement || $("#error-question-2");
            }
            if (!validateUserName(ratingData.userDetails.name, errorLabel)) {
                txtUserName.data('errorMsgShow', 'show');
                isValid = false;
                erroneousElement = erroneousElement || txtUserName;
            }
            if (!validateUserEmail(ratingData.userDetails.email, errorLabel)) {
                txtEmailID.data('errorMsgShow', 'show');
                isValid = false;
                erroneousElement = erroneousElement || txtEmailID;
            }
            if (erroneousElement) {
                $('html, body').animate({ scrollTop: $(erroneousElement).offset().top - 100 }, 500);
            }
            return isValid;
        }
        _ratingValidations.validateUserEmail = validateUserEmail;
        _ratingValidations.validateUserName = validateUserName;
        _ratingValidations.isUserLoggedIn = isUserLoggedIn;
        return _ratingValidations;
    }

    _rateCar.onRatingAnswerClick = function (event) {
        var questionId = $(event.target).attr('data-questionId');
        var answerId = $(event.target).attr('data-answerId');
        ratingData.rating.ratingQuestions[questionId - 1].answerId = answerId;

        $('#error-question-' + questionId).addClass('hide');
    }

    _rateCar.onRatingMouseEnter = function (event) {
        feedbackTitleSelector.text($(event.target).attr("data-feedbackTitle"));
        feedbackSubtitleSelector.text($(event.target).attr("data-feedbackSubtitle"));
        ratingLogo.removeClass(currentLogo);
        setCurrentLogo($(event.target).attr("data-value"));

        ratingLogo.addClass(currentLogo);
        carRatingBoxError.addClass('hide');
    }
    _rateCar.onRatingMouseLeave = function (event) {
        feedbackTitleSelector.text(feedbackTitle);
        feedbackSubtitleSelector.text(feedbackSubtitle);
        ratingLogo.removeClass(currentLogo);
        setCurrentLogo(ratingData.rating.userRating);
        ratingLogo.addClass(currentLogo);
        if (ratingData.rating.userRating == 0 && $("#carRatingBox .answer-star-list").data('errorMsgShow')) {
            carRatingBoxError.removeClass('hide');
        }
    }
    _rateCar.onRatingClick = function (event) {

        ratingData.rating.userRating = $(event.target).attr("data-value");
        feedbackTitle = $(event.target).attr("data-feedbackTitle");
        feedbackSubtitle = $(event.target).attr("data-feedbackSubtitle");
        feedbackTitleSelector.text(feedbackTitle);
        feedbackSubtitleSelector.text(feedbackSubtitle);
        ratingLogo.removeClass(currentLogo);
        setCurrentLogo(ratingData.rating.userRating);
        ratingLogo.addClass(currentLogo);
        carRatingBoxError.addClass('hide');
    }

    _rateCar.openModelVersionPopup = function () {
        $(".versionDiv").removeClass("hide");
        $('body').addClass('lock-browser-scroll');
    };
    _rateCar.closeModelVersionPopup = function () {
        $('.versionDiv').addClass('hide');
        $('body').removeClass('lock-browser-scroll');
    }

    //find versionId index in array of rating details object 
    _rateCar.findVersionId = function (versionArray) {
        var versionLen = versionArray ? versionArray.length : 0;
        for (var index = 0; index < versionLen; index++) {
            if (versionArray[index].versionId == ratingData.carDetails.versionId)
                return index;
        }
        return -1;
    }

    //find modelId index in Model based rating details array of object 
    _rateCar.findModelRatingHistory = function (modelArray) {
        var versionLen = modelArray ? modelArray.length : 0;
        for (var index = 0; index < versionLen; index++) {
            if (modelArray[index].modelId == modelId)
                return index;
        }
        return -1;
    }
    //creating version rating detail object for storing in local storage
    _rateCar.setRatingHistoryObject = function () {
        var versionRatingHistoryObject = {};
        if (ratingData.carDetails.versionId) {
            versionRatingHistoryObject.versionId = ratingData.carDetails.versionId;
            if (versionName) {
                versionRatingHistoryObject.versionName = versionName;
            }
        }
        if (ratingData.rating.userRating) {
            versionRatingHistoryObject.userRating = ratingData.rating.userRating;
        }
        if (ratingData.rating.ratingQuestions[0].answerId) {
            versionRatingHistoryObject.answerId_1 = ratingData.rating.ratingQuestions[0].answerId;
        }
        if (ratingData.rating.ratingQuestions[1].answerId) {
            versionRatingHistoryObject.answerId_2 = ratingData.rating.ratingQuestions[1].answerId;
        }
        return versionRatingHistoryObject;
    }
    //creating model based rating detail object for storing in local storage
    _rateCar.setModelHistoryRatingObject = function (data) {
        var versionRatingHistoryObject = _rateCar.setRatingHistoryObject();
        if (!($.isEmptyObject(versionRatingHistoryObject))) // remove this check
        {
            if (data) {
                var versionIndex = _rateCar.findVersionId(data["versionsRatingData"]);
                if (versionIndex < 0) {
                    data["versionsRatingData"].push(versionRatingHistoryObject);
                }
                else {
                    data["versionsRatingData"][versionIndex] = versionRatingHistoryObject;
                }
                return data;
            }
            else {
                var modelRatingHistoryObject = {};
                modelRatingHistoryObject.modelId = modelId;
                modelRatingHistoryObject.versionsRatingData = [versionRatingHistoryObject];
                return modelRatingHistoryObject;
            }
        }
    }


    _rateCar.ratingValidations = ratingValidations();
    return _rateCar;
}

$("#submitrating").click(function () {
    var errorLabel = { label: '' };
    if (rateYourCar.ratingValidations.validateRatingObject(errorLabel)) {
        rateYourCar.submitRating();
        SetCookieInDays('_CustomerName', ratingData.userDetails.name, 365);
        SetCookieInDays('_CustEmail', ratingData.userDetails.email, 365);
    }
    else {
        errorLabel.label = (errorLabel.label) ? '|errorlabel=' + errorLabel.label : '';
        cwTracking.trackCustomData(rateCarEventCategory, "RatingSubmitInvalid", rateCarEventLabel + errorLabel.label, false);
    }
});

/**************************
 * TODO: Need to handle this with updated dropdown plugin and assign a callback function for click events
**************************/
$(".js-select-version").click(function () {
    ratingData.carDetails.versionId = $(this).attr('data-versionid');
    versionName = $(this).attr('data-versionName');
    reviewSection.removeClass("rate-car-container--disabled");
    setRatingCookie(modelId, ratingData.carDetails.versionId);

    //if rating data available for respective version in local strorage prefill it.
    prefillRatingData();

    selectedVersion.text(versionName);
    verisonSelectionError.addClass('hide');
    selectedVersion.removeClass('text-center');
    if (isMobileDevice) {
        rateYourCar.closeModelVersionPopup();
        if (window.location.hash == "#version") {
            window.history.back();
        }
    }
    cwTracking.trackCustomData(rateCarEventCategory, "Versionclick", rateCarEventLabel + "|versionId=" + ratingData.carDetails.versionId, false);
});

$("#ratingVersionPopup").click(function () {
    rateYourCar.openModelVersionPopup();
    window.location.hash = "#version";
    cwTracking.trackCustomData(rateCarEventCategory, "Editclick", rateCarEventLabel, false);
});

$(document).on('click', '.rate-car-container--disabled', function () {
    verisonSelectionError.removeClass('hide');
    $('html, body').animate({ scrollTop: $(verisonSelectionError).offset().top - 100 }, 500);
});

var getVersionIndexFromCookie = function (modelVersions, modelId) {
    var modelIndex;
    var length = modelVersions.length;
    for (modelIndex = 0; modelIndex < length; modelIndex++) {
        var pair = modelVersions[modelIndex].split('~');
        if (pair[0] == modelId) {
            return modelIndex;
        }
    }
    return -1;
}


var setRatingCookie = function (modelId, versionId) {
    var currentCookie = $.cookie("reviewedVersions");
    if (currentCookie) {
        var modelVersions = currentCookie.split('&');
        var index = getVersionIndexFromCookie(modelVersions, modelId);
        if (index >= 0) {
            modelVersions[index] = modelId + '~' + versionId;
        }
        else {
            modelVersions.push(modelId + "~" + versionId);
        }
        currentCookie = modelVersions.join('&');
    }
    else {
        currentCookie = modelId + '~' + versionId;
    }
    var expire = new Date();
    expire.setTime(new Date().getTime() + parseInt(expiryTime) * (60000));
    document.cookie = "reviewedVersions=" + currentCookie + ";path=" + location.pathname + ";domain=" + document.domain + ";expires=" + expire.toUTCString();
}


//save modified ratings in local storage for version
var saveUnsubmittedRatingData = function () {
    var ratingValue = clientCache.get("ratingHistoryData");
    var historyRatingData = ((ratingValue) ? ratingValue.val : null);
    if (!historyRatingData) {
        historyRatingData = [
           { modelId: modelId, versionsRatingData: [rateYourCar.setRatingHistoryObject()] }
        ];
    }
    else {
        var index = rateYourCar.findModelRatingHistory(historyRatingData);
        var modelHistoryRating = rateYourCar.setModelHistoryRatingObject(historyRatingData[index]);
        if (index < 0) {
            historyRatingData.push(modelHistoryRating);
        }
        else {
            historyRatingData[index] = modelHistoryRating;
        }
    }
    clientCache.set({ key: "ratingHistoryData", value: historyRatingData, expiryTime: parseInt(expiryTime) }); //set expiryTime 10 days
}

//prefill data if rating details available in local storage
var prefillRatingData = function () {
    var ratingValue = clientCache.get("ratingHistoryData");
    var historyRatingData = ((ratingValue) ? ratingValue.val : null);
    if (historyRatingData) {
        var modelIndex = rateYourCar.findModelRatingHistory(historyRatingData);
        if (modelIndex >= 0) {
            var versionsRatingDataArray = historyRatingData[modelIndex]["versionsRatingData"];
            if (versionsRatingDataArray) {
                var versionIndex = _rateCar.findVersionId(versionsRatingDataArray);
                var savedRatingData = (versionIndex >= 0 ? versionsRatingDataArray[versionIndex] : null);
                if (savedRatingData && savedRatingData["versionId"]) {
                    ratingData.carDetails.versionId = savedRatingData["versionId"];
                    reviewSection.removeClass("rate-car-container--disabled");
                    verisonSelectionError.addClass('hide');
                    if (!savedRatingData["versionName"]) {
                        var selectedElement = $("#rate-car-form .js-select-version[data-versionId=" + ratingData.carDetails.versionId + "]");
                        if (selectedElement && selectedElement.length > 0) {
                            versionName = selectedElement.attr("data-versionname");
                            savedRatingData["versionName"] = versionName;
                        }
                    }
                    selectedVersion.text(savedRatingData["versionName"]);
                    selectedVersion.removeClass('text-center');
                    if (isMobileDevice) {
                        rateYourCar.closeModelVersionPopup();
                    }
                }

                if (savedRatingData && savedRatingData["userRating"]) {
                    ratingData.rating.userRating = savedRatingData["userRating"];
                    $("#rate-" + ratingData.rating.userRating).prop("checked", true);
                    ratingLogo.removeClass(currentLogo);
                    setCurrentLogo(ratingData.rating.userRating);
                    ratingLogo.addClass(currentLogo);
                    feedbackTitle = $("#carRatingBox label[data-value=" + ratingData.rating.userRating + "]").attr("data-feedbackTitle");
                    feedbackTitleSelector.text(feedbackTitle);
                    feedbackSubtitle = $("#carRatingBox label[data-value=" + ratingData.rating.userRating + "]").attr("data-feedbackSubtitle");
                    feedbackSubtitleSelector.text(feedbackSubtitle);
                }
                else {
                    $("#rate-" + ratingData.rating.userRating).prop("checked", false);
                    ratingLogo.removeClass(currentLogo);
                    ratingData.rating.userRating = 0;
                    setCurrentLogo(ratingData.rating.userRating);
                    ratingLogo.addClass(currentLogo);
                    feedbackTitleSelector.text("Rate Your Car");
                    feedbackSubtitleSelector.text("");
                }

                var rateCarQuestions = $("#rateCarQuestion .answer-radio-list input[type='radio']");
                if (savedRatingData && savedRatingData["answerId_1"]) {
                    ratingData.rating.ratingQuestions[0].answerId = savedRatingData["answerId_1"];
                    $("#q-1-" + ratingData.rating.ratingQuestions[0].answerId).prop("checked", true);

                }
                else {
                    $("#q-1-" + ratingData.rating.ratingQuestions[0].answerId).prop("checked", false);
                    ratingData.rating.ratingQuestions[0].answerId = 0;
                }

                if (savedRatingData && savedRatingData["answerId_2"]) {
                    ratingData.rating.ratingQuestions[1].answerId = savedRatingData["answerId_2"];
                    $("#q-2-" + ratingData.rating.ratingQuestions[1].answerId).prop("checked", true);
                }
                else {
                    $("#q-2-" + ratingData.rating.ratingQuestions[1].answerId).prop("checked", false);
                    ratingData.rating.ratingQuestions[1].answerId = 0;
                }
            }

        }
    }
    responseMeassage.text("");
    carRatingBoxError.addClass('hide');
    $('#error-question-1').addClass('hide');
    $('#error-question-2').addClass('hide');
}

var handleRatingPagechange = function () {
    ratingSubmitted = false;
    var promptMessage = "Leave Page ?\nChanges that you made may not be saved.";
    var promptOnLeave = function (event) {
        if (modelId > 0 && ratingData.carDetails.versionId > 0) {
            saveUnsubmittedRatingData();
        }
        if (!ratingSubmitted) {
            if (event) {
                event.returnValue = promptMessage;
            }
            return promptMessage;
        }
    }
    window.onbeforeunload = promptOnLeave
}


var validateVersionName = function (versionName) {
    var isValidVersion = false;
    $('.js-version-name').map(function () {
        if ($(this).text() === versionName) {
            isValidVersion = true;
        }
    });
    return isValidVersion;
}

$(document).ready(function () {
    history.scrollRestoration = 'manual';// written to avoid page flicker
    rateYourCar = new rateCar();
    cacheSelectors();
    var url = window.location.href.split('&');
    window.history.replaceState('', document.title, url[0]); // to remove version

    if (url.length > 1) {
        setRatingCookie(modelId, versionId);
    }
    //set versionId from cookie
    var currentCookie = $.cookie("reviewedVersions");
    if (versionId <= 0 && currentCookie) {
        var modelVersions = currentCookie.split('&');
        var index = getVersionIndexFromCookie(modelVersions, modelId);
        if (index >= 0) {
            ratingData.carDetails.versionId = modelVersions[index].split('~')[1];
        }
    }
    //prefill data if rating details available in local storage for respective model
    prefillRatingData();

    ratingSubmitted = false;
    rateYourCar.ratingValidations.isUserLoggedIn();
    handleRatingPagechange();



    if (isMobileDevice) {
        window.onpopstate = function () {
            var hashValue = location.hash;
            if (hashValue) {
                rateYourCar.openModelVersionPopup();
            }
            else {
                rateYourCar.closeModelVersionPopup();
            }
        };
    }
    $("#carRatingBox label").on({
        click: _rateCar.onRatingClick
    });
    if (!isMobileDevice) {
        $("#carRatingBox label").on({
            mouseenter: rateYourCar.onRatingMouseEnter,
            mouseleave: rateYourCar.onRatingMouseLeave
        })
    }
    $("#rateCarQuestion label").on({
        click: _rateCar.onRatingAnswerClick
    });
    txtEmailID.on({
        change: function (event) {
            ratingData.userDetails.email = event.target.value;
            if ($(this).data('errorMsgShow')) {
                rateYourCar.ratingValidations.validateUserEmail(ratingData.userDetails.email);
            }
        },
        focus: function () {
            cwTracking.trackCustomData(rateCarEventCategory, "EmailIdClicked", rateCarEventLabel, false);
        }
    });
    txtUserName.on({
        change: function (event) {
            ratingData.userDetails.name = event.target.value;
            if ($(this).data('errorMsgShow')) {
                rateYourCar.ratingValidations.validateUserName(ratingData.userDetails.name);
            }
        },
        focus: function () {
            cwTracking.trackCustomData(rateCarEventCategory, "NameClicked", rateCarEventLabel, false);
        }
    });
    if (desktopVesrionDropdown) {
        desktopVesrionDropdown.on({
            click: function () {
                if ($(this).next(CustomDropDown.customContent).is(':visible')) {
                    cwTracking.trackCustomData(rateCarEventCategory, "VersionDropdownClick", rateCarEventLabel, false);
                }
            }
        });
    }
});