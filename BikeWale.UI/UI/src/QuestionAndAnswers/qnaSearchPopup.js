var qnaViewModel = null;
var qaSearchPopup = function () {

    var _popupHead, _inputElement, _clearElement, _askElement, _cityId, _modelId, _versionId, _tagsCSV, _platformId, _sourceId, _askPopupButton, _nameInputField, _askQuestionSuggestions, _askQuestionInput, _pageName, _questionAnsPopup;
    var timeout = null;
    var sources = {
        QuestionDedicatedPage: 10,
        ModelPage_Question_Search_Slug: 11,
        ModelPage_Questions_Answer_Section_Search: 12,
        QuestionListing_Search: 13
    };

    var _options = {
        baseApiUrl: "/api/qna/search/",
        sizeOfRecords: 5,
        requestDelay: 250
    };

    function init() {
        _setSelectors();

        _cityId = _popupHead.getAttribute("data-cityid");
        _modelId = _popupHead.getAttribute("data-modelid");
        _versionId = _popupHead.getAttribute("data-versionid");
        _tagsCSV = _popupHead.getAttribute("data-tagscsv");
        _platformId = _popupHead.getAttribute("data-platformid");
        _pageName = _popupHead.getAttribute("data-pagename");
        qnaViewModel = new vmQnaSearch();
        ko.applyBindings(qnaViewModel, _popupHead);
    };

    function _setSelectors() {
        _popupHead = document.getElementById("qnaSearchPopup");
        _inputElement = document.getElementById("qnaSearchInput");
        _clearElement = document.querySelector(".popup-input-clear");
        _askElement = document.getElementById("qnasearch-askbutton");
        _nameInputField = document.getElementById("askquestion-namefield");
        _askQuestionSuggestions = document.getElementById("askquestionssuggestionbase");
        _askQuestionInput = document.getElementById("askQuestionInput");
        _questionAnsPopup = document.getElementById("questionAnswerPopup");
    }


    function update(sourceId) {
        _sourceId = sourceId;
    };

    function registerListeners() {
        _inputElement.addEventListener("keyup", _handleKeyup);
        _clearElement.addEventListener("click", _resetPopupData);
        _askElement.addEventListener("click", _openAskQuestionPopup);
        _askQuestionInput.addEventListener("keyup", _handleAskKeyup);
        window.addEventListener("popstate", _onPopupState);
    };

    function _createUrl(inp_element) {
        return (_options.baseApiUrl + "?cityId=" + _cityId + "&modelId=" + _modelId + "&versionId=" + _versionId + "&searchText=" + inp_element.value);
    }

    function _handleKeyup(event) {
        var self = this;
        self.value.length == 0 ? _resetInputElements(self) : qnaViewModel.showClear(true);

        clearTimeout(timeout);
        timeout = setTimeout(function () {
            if (self.value.trim() != ""){
                qnaViewModel.getQuestions(_createUrl(self));
            }
        }, _options.requestDelay);
    };

    function _handleAskKeyup(event) {
        var self = this;
        if (self.value.length == 0)
            _resetInputElements(self);

        clearTimeout(timeout);
        timeout = setTimeout(function () {
            if (self.value.trim() != "") {
                koVMQnA.searchSuggestions().getQuestions(_createUrl(self));
            }
        }, _options.requestDelay);
    };

    function _resetPopupData() {
        _inputElement.value = "";
        qnaViewModel.resetKOData();

        if (navigator.userAgent.match(/iPhone|iPad|iPod/i)) {
            _inputElement.focus();
        }
        else {
            setTimeout(function () {
                _inputElement.focus();
            }, 500)
        }
    };

    function _resetInputElements(inp_element) {
        inp_element.value = "";
        var selectedKo;

        if (inp_element === _inputElement) {
            selectedKo = qnaViewModel;
        }
        else if (inp_element === _askQuestionInput) {
            selectedKo = koVMQnA.searchSuggestions();
        }

        selectedKo.questions([]);
        selectedKo.bikewaleInfo(null);
        selectedKo.showClear(false);
        selectedKo.showContent(false);
        selectedKo.showBikeInfo(false);
        selectedKo.showPlaceholder(true);


        if (navigator.userAgent.match(/iPhone|iPad|iPod/i)) {
            inp_element.focus();
        }
        else {
            setTimeout(function () {
                inp_element.focus();
            }, 500)
        }
    };

    function _openAskQuestionPopup() {
        koVMQnA.text(_inputElement.value);
        qnaViewModel.showContent(false);
        _askQuestionInput.focus();
        var vmParams = {
            "modelId": _modelId,
            "tagsCSV": _tagsCSV,
            "platformId": _platformId,
            "sourceId": _sourceId
        };
        QuestionAndAns.openPopup(vmParams);
        askButtonGATrigger(false);
    };

    function _onPopupState() {
        if (_popupHead.classList.contains('popup-active') && (history.state !== "Q&ASearch-popup")) {
            _popupHead.classList.remove("popup-active");
        }
    };

    function _askPopupButtonPush() {
        _nameInputField.focus();
        koVMQnA.searchSuggestions().showContent(false);
        askButtonGATrigger(true);
    };

    function registerAskQuestionButton() {
        _askPopupButton = document.getElementById("askquestion-askbutton");
        _askPopupButton.addEventListener("click", _askPopupButtonPush);
    };

    function searchResultsBasedGA(questionsAvailable, questionId, isAskQuestionPopup) {
        var actionGA, labelGA;
        if (!isAskQuestionPopup)
        {
            if (questionsAvailable)
            {
                actionGA = (_sourceId == sources.ModelPage_Question_Search_Slug) ? "Search_Bar_Results_Displayed_Slug" : "Search_Bar_Results_Displayed";
                labelGA = _inputElement.value + "_" + questionId;
            }
            else
            {
                actionGA = (_sourceId == sources.ModelPage_Question_Search_Slug) ? "Search_Bar_No_Results_Displayed_Slug" : "Search_Bar_No_Results_Displayed";
                labelGA = _inputElement.value;
            }
            triggerGA(_pageName, actionGA, labelGA);
        }
    };
    function searchResultInteractionGA(values, isAskQuestionPopup) {
        var input_ele = isAskQuestionPopup ? _askQuestionInput : _inputElement;
        var query = input_ele.value;

        var ga_label = query + "_" + values.clickedElement.getAttribute("data-qid");

        var ga_act = isAskQuestionPopup ? "Popup_Search_Results_Question_Clicked" : "Search_Results_Question_Clicked";

        if (_sourceId == sources.ModelPage_Question_Search_Slug) {
            ga_act = isAskQuestionPopup ? "Popup_Search_Results_Question_Clicked_Slug" : "Search_Results_Question_Clicked_Slug";
        }

        triggerGA(_pageName, ga_act, ga_label);
    };

    function askButtonGATrigger(isAskQuestionPopup) {
        var ga_act, input_ele, questionsShown, bwInfoCardShown, questionid;

        if (isAskQuestionPopup) {
            input_ele = _askQuestionInput;
            questionsShown = koVMQnA.searchSuggestions().questions().length > 0;
            bwInfoCardShown = koVMQnA.searchSuggestions().showBikeInfo();
            questionid = questionsShown ? koVMQnA.searchSuggestions().questions()[0].question.guid : "";
        }

        else {
            input_ele = _inputElement;
            questionsShown = qnaViewModel.questions().length > 0;
            bwInfoCardShown = qnaViewModel.showBikeInfo();
            questionid = questionsShown ? qnaViewModel.questions()[0].question.guid : "";
        }

        var query = input_ele.value;

        var ga_label = questionsShown ? query + "_" + questionid : query;

        if (questionsShown || bwInfoCardShown) {
            ga_act = isAskQuestionPopup ? "Popup_Search_Results_Ask_Clicked" : "Search_Results_Non-zero_Ask_Clicked";
        }
        else {
            ga_act = "Search_Results_Zero_Ask_Clicked";
        }

        ga_act = _sourceId == sources.ModelPage_Question_Search_Slug ? ga_act + "_Slug" : ga_act;

        triggerGA(_pageName, ga_act, ga_label);
    };

    function registerBWInfoCardGA(isAskQuestionPopup, questionListShown, bwInfoShown, questionType) {

        $('.callout-card-action.info-card-bw').on('click', function () {
            var query = isAskQuestionPopup ? _askQuestionInput.value : _inputElement.value;
            var act = $(this).data('action');
            act += (_sourceId == sources.ModelPage_Question_Search_Slug) ? '_Slug' : '';
            triggerGA(_pageName, act, query);
        });
        var questionTypeText = "";
        switch (questionType) {
            case 1:
                questionTypeText = "User";
                break;
            case 2:
                questionTypeText = "Price";
                break;
            case 3:
                questionTypeText = "Mileage";
                break;
            case 4:
                questionTypeText = "EMI";
                break;
        }
        var query = isAskQuestionPopup ? _askQuestionInput.value : _inputElement.value;
        var act = "Search_Bar_";
        if (questionListShown && bwInfoShown) {
            act += questionTypeText + "_Card_And_Suggestions_Displayed";
            act += (_sourceId == sources.ModelPage_Question_Search_Slug) ? '_Slug' : '';
            triggerGA(_pageName, act, query);
        }
        else if (bwInfoShown) {
            act += "Only_" + questionTypeText + "_Displayed";
            act += (_sourceId == sources.ModelPage_Question_Search_Slug) ? '_Slug' : '';
            triggerGA(_pageName, act, query);
        }

    }


    return {
        init: init,
        registerListeners: registerListeners,
        update: update,
        resetPopupData: _resetPopupData,
        registerAskQuestionButton: registerAskQuestionButton,
        searchResultInteractionGA: searchResultInteractionGA,
        searchResultsBasedGA: searchResultsBasedGA,
        registerBWInfoCardGA: registerBWInfoCardGA
    }
}();

var vmQnaSearch = function () {
    var self = this;
    self.isQnaViewModelInitialized = ko.observable(false);
    self.questions = ko.observableArray([]);
    self.showClear = ko.observable(false);
    self.showPlaceholder = ko.observable(true);
    self.showContent = ko.observable(false);
    self.footerText = ko.observable("Didn't find the questions relevant?");
    self.queryString = ko.observable("");
    self.isAskQuestion = ko.observable(false);
    self.bikewaleInfo = ko.observable(null);
    self.showBikeInfo = ko.observable(false);

    if (window.location.search != undefined && window.location.search != "" && window.location.search.substring(0, 3) === "?q=") {
        self.queryString(decodeURI(window.location.search.substring(3)));
        $('#qnaSearchInput').val(self.queryString());
        $('.qna-search__input').text(self.queryString());
    }
    self.getQuestions = function (requestUrl) {
        try {
            var isQuestionListDisplayed = false, firstQuestionId = "";
            $.get(requestUrl, function (data, status) {
                if (status == "success") {
                    data = JSON.parse(data);
                    if (data.bikeinfo)
                    {
                        self.bikewaleInfo(data.bikeinfo);
                        self.showBikeInfo(true);
                    }
                    else
                    {
                        self.bikewaleInfo(null);
                        self.showBikeInfo(false);
                    }
                    data = data.questions;
                    self.questions(data);
                    if (data == null || data.length == 0)
                    {
                        self.footerText("Oops! No relevant questions found");
                    }
                    else
                    {
                        self.footerText("Didn't find the questions relevant?");
                        isQuestionListDisplayed = true;
                        firstQuestionId = data[0].question.guid;
                    }

                    self.showContent(true);
                    self.showPlaceholder(false);

                    if ((data == null || data.length == 0) && (!self.showBikeInfo()) && self.isAskQuestion()) {
                        self.showContent(false);
                    }
                }
                else {
                    self.questions([]);
                }
                // GA
                qaSearchPopup.searchResultsBasedGA(isQuestionListDisplayed, firstQuestionId, self.isAskQuestion());
                qaSearchPopup.registerBWInfoCardGA(self.isAskQuestion(), isQuestionListDisplayed, self.bikewaleInfo() != null, self.bikewaleInfo() != null ? self.bikewaleInfo().type: null);
            });
        }
        catch (err) {
            self.questions([]);
        }
    };

    self.resetKOData = function () {
        self.questions([]);
        self.bikewaleInfo(null);
        self.showClear(false);
        self.showBikeInfo(false);
        self.showContent(false);
        self.showPlaceholder(true);
    };
};

docReady(function () {

    // QnA search popup
    var qnaSearchPopup = new Popup('.js-qna-popup-link', {
        closeButtonClass: 'js-qna-search-back-btn',
        onPopupOpen: function () {

            if (navigator.userAgent.match(/iPhone|iPad|iPod/i)) {
                $("#qnaSearchInput").focus();
            }
            else {
                setTimeout(function () {
                    $("#qnaSearchInput").focus();
                }, 500)
            }
        },
        onCloseClick: function () {
            if (typeof HandelBodyScroll !== 'undefined') {
                HandelBodyScroll.unlockScroll();
            }
            if (history.state === 'Q&ASearch-popup') {
                window.history.back();
            }
        },

    });

    // QnA search popup accordion
    var qnaAccordion = new Accordion(".js-qna-accordion", {
        multipleOpen: false,
        onExpandEvent: function (values) {
            qaSearchPopup.searchResultInteractionGA(values, false);
        }
    });

    qaSearchPopup.init();
    qaSearchPopup.registerListeners();

    var searchElements = document.querySelectorAll(".qna-search__popup");
    var len = searchElements.length;
    for (var i = 0 ; i < len ; i++) {
        searchElements[i].addEventListener("click", handleSearchElementsClick);
    }

    //document.querySelector(".js-qna-search-back-btn").addEventListener("click", handlePopupCloseEvent);
    document.querySelector(".question-answer__close-icon").addEventListener("click", handlePopupCloseEvent);

    function handlePopupCloseEvent() {
        history.back();
    }

    function handleSearchElementsClick(e) {
        var targetElem = e.currentTarget;
        history.pushState('Q&ASearch-popup', '', '');
        qaSearchPopup.update(targetElem.getAttribute("data-sourceid"));
    };

    $(document).on("click", ".qna-accordion-read-more", function (event) {
        $(this).toggleClass("hide");
        $(this).siblings(".stripped-answer").toggleClass("hide");
        $(this).siblings(".full-answer").toggleClass("hide");
        qnaAccordion.resizeAccordionItemHeight(event);
    });
});

