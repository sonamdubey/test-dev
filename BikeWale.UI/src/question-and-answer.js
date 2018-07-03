var customer = function () {
    var self = this;
    self.name = ko.observable();
    self.email = ko.observable();
    self.mobile = ko.observable();

    self.init = function (params) {
        if (params) {
            if (params.name) {
                self.name(params.name);
            }
            if (params.email) {
                self.email(params.email);
            }
            if (params.mobile) {
                self.mobile(params.mobile);
            }
        }
    };

    self.readUserCookie = function _readUserCookie() {
        var cookieName = "_PQUser";
        var keyValue = document.cookie.match('(^|;) ?' + cookieName + '=([^;]*)(;|$)');
        if (keyValue) {
            var arr = keyValue ? keyValue[2].split("&") : null;
            if (arr != null && arr.length > 0) {
                var objCustomer = {
                    "name": (arr[0] || ''),
                    "email": ((arr[1] && arr[1] != 'undefined') ? arr[1] : ''),
                    "mobile": (arr[2] || '')
                }
                self.init(objCustomer);
            }
        }
        return self;
    };

    self.setCustomerCookie = function setPQUserCookie() {
        
        var val = self.name() + '&' + self.email();
        if (self.mobile() != null) {
            val = val + '&' + self.mobile();
        }
        SetCookie("_PQUser", val);
    };
};

var vmQuestionAndAns = function () {
    var self = this;
    self.isKOInitialized = ko.observable(false);
    self.questionId = ko.observable();
    self.text = ko.observable();
    self.askedBy = ko.observable();
    self.tags = ko.observableArray();
    self.modelId = ko.observable();
    self.platformId = ko.observable();
    self.sourceId = ko.observable();
    self.isSubmittedSuccessfully = ko.observable(false);    
    self.initCustomer = function () {
        self.askedBy(new customer().readUserCookie());
    };

    self.init = function (options) {
        self.initCustomer();
        if (options) {
            if (options.modelId) {
                self.modelId(options.modelId);
            }
            if (options.tagsCSV) {
                var objTags = options.tagsCSV.split(",");
                var lenTag = objTags.length;
                var arrTag = [];
                for (var i = 0; i < lenTag; i++) {
                    arrTag.push({ "name": objTags[i] });
                }
                self.tags(arrTag);
            }
            if (options.platformId) {
                self.platformId(options.platformId);
            }
            if (options.sourceId) {
                self.sourceId(options.sourceId);
            }
        }
    };
};
var koVMQnA;
var QuestionAndAns = function () {
    var questionAnsPopup, closeIcon, postQuestion, questionAnsContainer, link;
    var bikeName, askedBy, makeName, modelName, userName, userEmail, ques, cat, act, lab, prefilledName, prefilledEmail;

    function _setSelector() {
        link = $('.ask-question__link');
        questionAnsPopup = $('#questionAnswerPopup');
        closeIcon = $('.question-answer__close-icon');
        postQuestion = $('#postQuestion');
        questionAnsContainer = $('.question-answer-container');
    };

    function _initGAdata() {
        askedBy = koVMQnA.askedBy();
        makeName = questionAnsPopup.data("makename");
        modelName = questionAnsPopup.data("modelname");
        userName = askedBy.name();
        userEmail = askedBy.email();
        prefilledName = userName;
        prefilledEmail = userEmail;
        cat = questionAnsPopup.data('cat');
        lab = makeName + '_' + modelName;
    }
    
    function _nonInteractionGA() {
        if ($('#qnaReviewListItem').length > 0 && $('#questionAnswerWrapper').length > 0 && $('#viewAllQnASlug').length > 0) {
            triggerNonInteractiveGA(cat, "Q&A_Displayed", lab);
        }
        if ($('#askQuestionReviewListItem').length > 0) {
            triggerNonInteractiveGA(cat, "Ask_Question_Link_Loaded", lab);
        }

        if ($('#askQuestionQnASlug').length > 0) {
            triggerNonInteractiveGA(cat, "Ask_Question_Loaded", lab + "_Slug");
        }
    }

    function _handleLinkClick() {
        link.on('click', function () {
            var ele = $(this);
            questionAnsPopup.addClass('popup--active')
            popup.lock();
            $('.blackOut-window').hide();
            history.pushState('Q&A-popup', '', '');
            if (koVMQnA) {
                var vmParams = {
                    "modelId": ele.data("modelid"),
                    "tagsCSV": ele.data("tags"),
                    "platformId": ele.data("platform"),
                    "sourceId": ele.data("source")
                };
                koVMQnA.init(vmParams);
            }

            triggerAskQuestionGA("Ask_Question_Button_Clicked", ele);
            triggerPreFilledGA("Ask_Question_Link_Clicked");

        });
    };

    function _handleCloseIconClick() {
        closeIcon.on('click', function () {
            if (postQuestion.text() === "Done") {
                triggerGA(cat, "Ask_Question_Form_Thanks_Closed", makeName + '_' + modelName);
                koVMQnA.text('');
                resetAskQuestionPopup();
            }
            else {
                triggerFormCloseGA('Ask_Question_Form_Closed');
                resetAskQuestionPopupExit();
            }
            
            _closePopup();
        });

    };

    function _closePopup() {
        questionAnsPopup.removeClass('popup--active')
        popup.unlock();

    };

    function resetAskQuestionPopup() {
        koVMQnA.isSubmittedSuccessfully(false);
        koVMQnA.questionId(null);
        postQuestion.text('Post Question');
        $('.text-area__character-count').text('Max 300 Characters');
        $('.form-control-box').removeClass('invalid');
        $('.form-control__text-area').height("");
        $('.question-answer__thank-you').hide();
        $('.question-answer__content').show();
    }

    function resetAskQuestionPopupExit() {
        koVMQnA.isSubmittedSuccessfully(false);
        koVMQnA.questionId(null);
        postQuestion.text('Post Question');
        $('.question-answer__email').removeClass('invalid');
        $('.question-answer__thank-you').hide();
        $('.question-answer__content').show();
    }

    function resetGAdata(action) {
        act = action;
        lab = makeName + '_' + modelName;
        prefilledName = userName;
        prefilledEmail = userEmail;
        askedBy = koVMQnA.askedBy();
        userName = askedBy.name();
        userEmail = askedBy.email();
        quesText = koVMQnA.text();
    };

    function triggerAskQuestionGA(action, ele) {
        resetGAdata(action);
        var labSuffix = ele.data('lab-suffix');
        labSuffix = labSuffix != undefined ? labSuffix : "";
        triggerGA(cat, act, lab + labSuffix);
    }

    function triggerPreFilledGA(action) {
        resetGAdata(action);
        if (userName != undefined && userName != '') {
            lab += "_Name"
        }
        if (userEmail != undefined && userEmail != '') {
            lab += "_Email"
        }
        if (lab != makeName + '_' + modelName) {
            lab += "_Prefilled"
        }        
        triggerGA(cat, act, lab);
    };

    function triggerFormCloseGA(action) {
        resetGAdata(action);
        var emailPrefilled = false, namePrefilled = false;
        if (prefilledName != undefined && prefilledName != '' && prefilledName === userName) {
            lab += "_Name";
            namePrefilled = true;
        }
        if (prefilledEmail != undefined && prefilledEmail != '' && prefilledEmail === userEmail) {
            lab += "_Email";
            emailPrefilled = true;
        }
        if (emailPrefilled || namePrefilled) {
            triggerGA(cat, act, lab + "_Prefilled");
        }
        lab = makeName + '_' + modelName;
        if (quesText != undefined) {
            lab += "_Question";
        }
        if (!namePrefilled && userName != undefined && userName != '') {
            lab += "_Name";
        }
        if (!emailPrefilled && userEmail != undefined && userEmail != '') {
            lab += "_Email";
        }       
        triggerGA(cat, act, lab);
    };    

    function removeMaliciousCode(text) {
        if (!text)
            return text;
        var regex = /<script[^>]*>[\s\S]*?<\/script\s*>/gi;
        while (regex.test(text)) {
            text = text.replace(regex, "");
        }
        return text;
    };

    function showLoader() {
        $('#ub-ajax-loader').show();
    };

    function hideLoader() {
        $('#ub-ajax-loader').hide();
    };

    function saveQuestion() {
        var isSuccess = false;
        try {
            var objQuestion = {
                "questionId": koVMQnA.questionId() ? koVMQnA.questionId() : null,
                "text": koVMQnA.text(),
                "askedBy": {
                    "customerName": koVMQnA.askedBy().name(),
                    "customerEmail": koVMQnA.askedBy().email(),
                    "customerMobile": koVMQnA.askedBy().mobile()
                },
                "tags": koVMQnA.tags(),
                "modelId": koVMQnA.modelId(),
                "platformId": koVMQnA.platformId(),
                "sourceId": koVMQnA.sourceId()
            };
            $.ajax({
                type: "POST",
                url: "/api/questions/",
                data: ko.toJSON(objQuestion),
                beforeSend: function (xhr) {
                    showLoader();
                },
                async: false,
                contentType: "application/json",
                dataType: 'json',
                success: function (response) {
                    isSuccess = response != null || response != undefined;
                    if (isSuccess) {
                        koVMQnA.isSubmittedSuccessfully(isSuccess);
                        koVMQnA.questionId(response);
                        koVMQnA.askedBy().setCustomerCookie();                        
                        triggerGA(cat, "Ask_Question_Form_Submit_Success", makeName + '_' + modelName);
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    koVMQnA.isSubmittedSuccessfully(false);
                },
                complete: function (xhr, ajaxOptions, thrownError) {
                    hideLoader();
                }
            });
        } catch (e) {
            console.log("Error occured while saving question.", e.message);
        }
        return isSuccess;
    };

    function _handlePostQuestionClick() {
        postQuestion.on('click', function () {
            var container = $('.question-answer__content');
            var thankYouContainer = $('.question-answer__thank-you');
            if (container.is(':visible') && _isValidInput(container.find('.form-control__input'))) {
                container.hide();
                if (koVMQnA.isSubmittedSuccessfully() || saveQuestion()) {
                    thankYouContainer.show();
                    $(this).text('Done');
                }
            }
            else if (thankYouContainer.is(':visible')) {
                triggerGA(cat, "Ask_Question_Form_Thanks_Done_Clicked", makeName + '_' + modelName);
                resetAskQuestionPopup();
                koVMQnA.text('');
                _closePopup();
            }
        });
    };

    function _onInputFocused() {
        $('.form-control__input').on('focusin', function () {
            var parent = $(this).closest('.form-control-box');
            if (parent.hasClass('invalid')) {
                parent.removeClass('invalid');
            }
        });
    };

    function _isValidInput(element) {
        var validation = true;
        resetGAdata('Ask_Question_Form_Submit_Failure');
        element.each(function () {
            var input = $(this);
            var validationType = input.data("validationtype");
            var parent = input.closest('.form-control-box');
            var isValid = input.is("textarea") ? input.closest('.form-control-box').hasClass('invalid') : false;
            var currentVal = input.val();
            switch (validationType) {
                case "name":
                    var objValid = validateUserName(currentVal);
                    isValid = objValid.isValid;
                    if (!objValid.isValid) {
                        parent.find("span.form-control__error").text(objValid.message);
                        act += '_Name';
                    }
                    break;
                case "email":
                    var objValid = validateEmail(currentVal);
                    isValid = objValid.isValid;
                    if (!objValid.isValid) {
                        parent.find("span.form-control__error").text(objValid.message);
                        act += '_Email'
                    }
                    break;
                case "nonempty":                    
                    isValid = !(currentVal.length == 0 || currentVal.length > 300);
                    if (!isValid) {
                        act += '_Question'
                    }
                    break;
                default:
                    break;
            }            
            if (!isValid) {
                parent.addClass('invalid');
                validation = false;
            }
            else {
                parent.removeClass('invalid');
            }
        });
        if (act != 'Ask_Question_Form_Submit_Failure') {
            triggerGA(cat, act, lab);
        }
        return validation;
    };

    function validateEmail(leadEmailId) {
        var regMaxLen = /^[A-z0-9._+-@]{1,50}$/;
        var errorMessage = '';
        var isValid = true,            
            reEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;
        if (leadEmailId == "") {
            errorMessage = "Please enter Email ID";
            isValid = false;
        }
        else if (!reEmail.test(leadEmailId)) {
            errorMessage = "Please enter valid Email ID";
            isValid = false;
        }
        else if (!regMaxLen.test(leadEmailId)) {
            errorMessage ="Email cannot be more than 50 characters";
            isValid = false;
        }
        return { "isValid" : isValid , "message" : errorMessage };
    };

    function validateUserName(leadUsername) {        
        var isValid = false;
        var errorMessage = '';
        var regMaxLen = /^[a-zA-Z ]{1,50}$/;
        var regValidName = /^[a-zA-Z ]{1,}$/;
        if (leadUsername != null && leadUsername.trim() != "") {
            nameLength = leadUsername.length;
            if (nameLength == 0) {
                errorMessage = "Please enter Name";
                isValid = false;
            }
            else if (!regValidName.test(leadUsername)) {
                errorMessage = "Please enter valid Name";
                isValid = false;
            }
            else if (!regMaxLen.test(leadUsername)) {
                errorMessage ='Name cannot be more than 50 characters';
                isValid = false;
            }
            else if (nameLength >= 1) {                
                isValid = true;
            }
        }
        else {
            errorMessage ="Please enter Name";
            isValid = false;
        }
        return { "isValid": isValid, "message": errorMessage };
    };

    function _onPopupState() {
        window.onpopstate = function () {
            if (questionAnsPopup.hasClass('popup--active')) {
                setTimeout(function () {
                    _closePopup();
                })
            }
        }
    };

    function registerEvents() {
        _setSelector();
        _handleLinkClick();
        _handleCloseIconClick();
        _handlePostQuestionClick();
        _onPopupState();
        _onInputFocused();
        _initializeKO();
        _initGAdata();
        _nonInteractionGA();        
    }

    return {
        registerEvents: registerEvents
    }

    function _initializeKO() {        
        koVMQnA = new vmQuestionAndAns();
        koVMLoadMoreQuestions = new vmQuestionAnswerLoadMore();
        if (koVMQnA && !koVMQnA.isKOInitialized()) {
            try {
                koVMQnA.initCustomer();
                ko.applyBindings(koVMQnA, document.getElementById("questionAnswerPopup"));
                koVMQnA.isKOInitialized(true);
            } catch (e) {
                console.log("Failed to initialize the KO");
            }
        }
        if(document.getElementById("questionAnswerList") !=null && koVMLoadMoreQuestions && !koVMLoadMoreQuestions.isKOInitialized())
        {
            try
            {
                ko.applyBindings(koVMLoadMoreQuestions, document.getElementById("questionAnswerList"));
                koVMLoadMoreQuestions.isKOInitialized(true);
            }
            catch (e) {
                console.log("Failed to initialize the koVMLoadMoreQuestions");
            }


        }
    }
}();

var TextArea = function () {
    var input
    function _setSelector() {
        input = $('.form-control__text-area')
    }
    function _changeHeight(element) {
        var elementHeight = element[0].scrollHeight;
        var elementMinHeight = element.css('min-height');
        if (!element.val()) {
            element.css({
                'height': elementMinHeight
            })
        }
        else {
            element.css({
                'height': elementHeight + 'px'
            });
        }

        element.scrollTop(elementHeight)
    }

    function _handleCharCount(element, event) {
        var parent = element.closest('.area-control-box');
        var characterCount = parent.find('.text-area__character-count');
        var maxCharCount = parseInt(element.attr('data-maxlength'));
        var currentCharCount = element.val().length
        if (!currentCharCount) {
            characterCount.text('Max ' + maxCharCount + ' Characters')
        }
        else {
            characterCount.text(maxCharCount - currentCharCount + ' Characters left');
            if (currentCharCount >= maxCharCount) {
                characterCount.text('Max character limit reached')
                parent.removeClass('invalid')
            }
            if (currentCharCount >= maxCharCount + 1) {
                characterCount.text('Max character limit exceeded')
                parent.addClass('invalid')
            }
            else if (currentCharCount < maxCharCount && parent.hasClass('invalid')) {
                parent.removeClass('invalid')
                characterCount.text('Max ' + maxCharCount + ' Characters')
            }
        }
    }

    function _handleInputClick() {
        input.on('keyup keydown keypress', function (event) {
            var element = $(this);
            _handleCharCount(element, event);
            _changeHeight(element);
        })
        input.on('paste', function (event) {
            var element = $(this);
            setTimeout(function () {
                _handleCharCount(element, event);
                _changeHeight(element);
            })

        })
    }
    function registerEvents() {
        _setSelector();
        _handleInputClick();
    }

    return {
        registerEvents: registerEvents
    }
}();


var vmQuestionAnswerLoadMore = function () {
    var self = this;
    var pageNo = 1;
    var pageSize = 10;
    self.isKOInitialized = ko.observable(false);
    self.modelId = ($('#askQuestionButton').length > 0 ? $('#askQuestionButton').data('modelid') : 0);
    self.otherQuestions = ko.observableArray([]);
    self.remainingQuestions = ko.observable($('#totalAnsweredQuestions').val() - pageSize);

    self.readMoreText = function (d, e) {
        $(e.target).parent('.question-answer-wrapper__answer-box').addClass("question-answer-wrapper--active");
    };
    self.bindNextQuestions = function () {
        try
        {
            pageNo = pageNo + 1;
            if (koVMLoadMoreQuestions.modelId > 0 && (pageNo > 0) && (pageSize > 0)) {
                $.ajax({
                    type: "GET",
                    url: "/api/models/" + koVMLoadMoreQuestions.modelId + "/questions/?pageNo=" + pageNo + "&pageSize=" + pageSize,
                    contentType: "application/json",
                    success: function (response) {
                        if (response && response.length > 0) {

                            response = JSON.parse(response);
                            if (("questions" in response) && response.questions != null && response.questions.length > 0) {

                                var questionList = response.questions;
                                self.remainingQuestions(self.remainingQuestions() - questionList.length);
                                self.otherQuestions.push.apply(self.otherQuestions, questionList);
                            }
                            else {
                                console.log("Error: Question list in Api Response is null")
                            }
                        }
                        else {
                            console.log("Error: Api Response is invalid")
                        }
                    },
                    error: function()
                    {
                        console.log("Error: Api Response is null")
                    }
                });
            }
        }
        catch (e) {
            console.warn(e);
        }
     

    };


}

$(document).ready(function () {
    TextArea.registerEvents();
    QuestionAndAns.registerEvents();
    $('.qna-redirection-link').on('click', function () {
        var link = $(this);
        triggerGA(link.data('cat'), link.data('act'), link.data('lab'));
    });
})
