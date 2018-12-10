var parentContainer = (function () {
    var eventName = {
        documentReady: "commonDocReady",
    }
    var carData = {};
    var prevBtn, nextBtn, loadingScreen, formNavigation, formClose, planScreen, floatButton;

    var navigationTab = { contact: "formContact", carDetails: "formCarDetail", carImages: "carimages", carCondition: "formCarCondition" };
    var stateEventDict = {
        selectPincode: "setPincodeScreen",
        contactDetails: "setContactScreen",
        selectMakeYear: "setYearScreen",
        selectCar: "setCarSelectionScreen",
        selectColor: "setColorScreen",
        selectOwners: "setOwnerScreen",
        enterPrice: "setExpectedPriceScreen",
        enterKms: "setKmsScreen",
        selectInsurance: "setInsuranceScreen",
        uploadImage: "setImageScreen",
        accidentalQuestion: "carConditionHistoryBack",
        partsReplacedQuestion: "carConditionHistoryBack",
        insuranceClaimedQuestion: "carConditionHistoryBack",
        serviceQuestion: "carConditionHistoryBack",
        loanQuestion: "carConditionHistoryBack",
        tyreConditionQuestion: "carConditionHistoryBack",
        wearTearQuestion: "carConditionHistoryBack",
        mechanicalIssueQuestion: "carConditionHistoryBack",
        preventBack: "preventBack"
    };

    if (typeof events !== 'undefined') {
        events.subscribe(eventName.documentReady, setSelectors);
        events.subscribe(eventName.documentReady, registerDomEvents);
        events.subscribe("updateNavigationTabClick", updateNavigationTabClicks);
        events.subscribe("navigateAway", goToListingsPage);
        events.subscribe("takenLive", storeCarData);
    }

    // store car data when listing went live
    function storeCarData(eventObj)
    {
        if (eventObj && eventObj.data) {
            carData = eventObj.data;
        }
    }

    function goToListingsPage() {
        // take user to listings page directly
        window.removeEventListener("beforeunload", parentContainer.onPageUnload);
        parentContainer.setLoadingScreen();
        window.location = "/used/mylistings/?isredirect=true&payst=n&type=1&value=" + carData.sellCarCustomer.mobile + "&authtoken=" + encodeURIComponent($.cookie("encryptedAuthToken"));
    }

    function setSelectors() {
        prevBtn = $('#prevBtn');
        nextBtn = $('#nextBtn');
        loadingScreen = $('#loadingScreen');
        formNavigation = $('#formNavigation');
        formClose = $('#formClose');
        planScreen = $('#planScreen');
        floatButton = $('#floatButton');
    }

    function registerDomEvents() {
        $(document).on('change', '.select-box select', function () {
            if ($(this).val() !== "0") {
                validate.field.hideError($(this));
            }
        });

        /* input field */
        $(document).on('focus', '.input-box input', function () {

            formField.scrollToTop($(this));
        });

        $(document).on('blur', '.input-box input', function () {
            if ($(this).val().length === 0) {
                $(this).closest('.input-box').removeClass('done');
            }
            else {
                validate.field.hideError($(this));
            }
        });

        /* input field autocomplete */
        $(document).on('focus', '.input-box .easy-autocomplete input', function () {
            var autocompleteWrapper = $(this).closest('.easy-autocomplete');

            if (!autocompleteWrapper.hasClass('move-siblings')) {
                var inputSiblings = $(this).closest('.easy-autocomplete').siblings();

                autocompleteWrapper.addClass('move-siblings').append(inputSiblings);
            }
        });

        // form close event
        $(document).on('click', '#formClose', function () {
            var currentForm;
            if (history.state.currentState == 'selectInsurance')
                currentForm = 'formCarInsurance'
            parentContainer.setNavigateAwayModal(currentForm);
        });
        // navigate from page tracking
        $(document).on('click', '.modal_navigate_away', function () {
            sellCarTracking.forMobile("close", (formNavigation.find('.active').attr('data-tab')).toString());
        });

        $(document).on('click', '#formNavigation li', function () {
            sellCarTracking.forMobile("topNavigationClicked", $(this).attr('data-tab').toString());
        });

        $('.collapsible-content').on('click', '.collapsible-target', function () {
            var targetElement = $(this),
				collapsibleContent = targetElement.closest('.collapsible-content');

            if (!collapsibleContent.hasClass('more-active')) {
                collapsibleContent.addClass('more-active');
                targetElement.text('Read less');
            }
            else {
                collapsibleContent.removeClass('more-active');
                targetElement.text('Read more');
            }
        });

        $(window).on('popstate', function () {
            if (history.state === null) {
                events.publish('historynull');
            }
            else if (history.state && typeof events !== 'undefined') {
                events.publish(stateEventDict[history.state.currentState], history.state.currentState);
            }
            events.publish("popHistory");
        });

        $(document).on('click', '.cw-tooltip-target', function () {
            $(this).find('.cw-tooltip').show();
            $('body').addClass('cw-tooltip-active');
        });

        $(document).on('click', '.cw-tooltip-active', function (event) {
            if (!$(event.target).hasClass('cw-tooltip-text')) {
                $('.cw-tooltip').hide();
                $('body').removeClass('cw-tooltip-active');
            }
        });

        var windowDimension = $(window).width() + $(window).height();
        $(window).resize(function () {
            if ($(window).width() + $(window).height() < windowDimension) {
                if ($('input').is(':focus')) {
                    floatButton.hide();
                }
                else {
                    floatButton.show();
                }
            }
            else {
                floatButton.show();
            }
        });

        $(document).on('keyup', '.input-box input', function (event) {
            var key = event.which || event.keyCode;

            if (key == 13) {
                var nextInputField = $(this).closest('.input-box').next('.input-box');

                if (nextInputField.length) {
                    nextInputField.find('input').focus();
                }
                else {
                    $(this).blur();
                }
            }
        });
        $(document).on('click', '#modalBg, .close-icon', function () {
            history.back();
        });
        $(document).on('click', '#formNavigation li', function () {
            if (!summary.closeActiveSummary())
            {
                var tabClicked = $(this).attr('data-tab');
                if (isNavigationTabEnabled(tabClicked)) {
                    switch (tabClicked) {
                        case navigationTab.contact:
                            if (!isTabActive(tabClicked)) {
                                validateScreen.hide();
                                setNavigationTab(navigationTab.contact);
                                events.publish("contactDetailsTabClick");
                            }
                            break;
                        case navigationTab.carDetails:
                            if (!isTabActive(tabClicked)) {
                                setNavigationTab(navigationTab.carDetails);
                                validateScreen.show();
                                events.publish("carDetailsTabClick");
                            }
                            break;
                    }
                }
            }
        });
    };
    
    function isTabActive(tabName)
    {
        return formNavigation.find("li[data-tab='" + tabName + "']").hasClass('active');
    }

    function isNavigationTabEnabled(tabName){
        return formNavigation.find("li[data-tab='" + tabName + "']").attr("data-isEnabled") == 'true';
    }

    function updateNavigationTabClicks(updateData) {
        if (typeof updateData == 'object' && $.isArray(updateData.tabNameArray)) {
            while (updateData.tabNameArray.length) {
                formNavigation.find("li[data-tab='" + updateData.tabNameArray.pop() + "']").attr("data-isenabled", updateData.value);
            }
        }
    }

    function isPresentInArray(element, array) {
        return $.inArray(element, array) != -1;
    }

    function setNavigationTab(tabElement) {
        var tabs = formNavigation.find('.breadcrumb-list__item'),
			tabElement = formNavigation.find('.breadcrumb-list__item[data-tab="' + tabElement + '"]');

        tabs.removeClass('active done');
        tabElement.prevAll().addClass('done');
        tabElement.addClass('active');
    };

    function setNavigateAwayModal(activeElement) {
        var currentActiveTab = activeElement || formNavigation.find('.active').attr('data-tab');
        var obj;
        switch (currentActiveTab) {
            case "formContact":
            case "formCarDetail":
                obj = {
                    heading: "You will lose the filled information, are you sure you want to navigate away?",
                    description: "",
                    isYesButtonActive: true,
                    yesButtonText: "Continue",
                    yesButtonLink: "javascript:void(0)",
                    isNoButtonActive: true,
                    noButtonText: "Leave",
                    noButtonLink: "javascript: events.publish('navigateAwayCarDetails')"
                };
                break;
            case 'formCarInsurance':
                obj = {
                    heading: "Listings with all details get better response. Are you sure you want to leave car listing process?",
                    description: "",
                    isYesButtonActive: true,
                    yesButtonText: "Continue",
                    yesButtonLink: "javascript:void(0)",
                    isNoButtonActive: true,
                    noButtonText: "Leave",
                    noButtonLink: "javascript:events.publish('navigateAwayInsurance')"
                };
                break;
            case "formCarImage":
                obj = {
                    heading: "Listing with photos sell 4x faster. Do you want to add photos?",
                    description: "",
                    isYesButtonActive: true,
                    yesButtonText: "Yes, Add photos",
                    yesButtonLink: "javascript:void(0)",
                    isNoButtonActive: true,
                    noButtonText: "No, Leave",
                    noButtonLink: "javascript:events.publish('navigateAwayImages')"
                };
                break;

            case "formCarCondition":
                obj = {
                    heading: "You are almost on final step. Are you sure you want to leave car listing process?",
                    description: "",
                    isYesButtonActive: true,
                    yesButtonText: "Continue",
                    yesButtonLink: "javascript:void(0)",
                    isNoButtonActive: true,
                    noButtonText: "Leave",
                    noButtonLink: "javascript:events.publish('navigateAwayCondition')"
                };
                break;
        }
        if (obj)
            modalPopup.showModal(templates.modalPopupTemplate(obj));
    };

    function setIntroScreen() {
        $('body').removeClass('form-active header-footer-inactive');
        parentContainer.setNavigationTab('formContact');
    };

    function removeIntroScreen(isc2b) {
        $("ul#formNavigation li:last").show();
        if (!isc2b) {
            $("ul#formNavigation li:last").hide();
        }
        $('body').addClass('form-active header-footer-inactive');
        parentContainer.setNavigationTab('formContact');
    };

    function setButtonTarget(prevFunction, nextFunction) {
        if (prevFunction && prevFunction.length) {
            prevBtn.attr('onClick', prevFunction);
            floatButton.removeClass('prev-btn-inactive');
        }
        else {
            floatButton.addClass('prev-btn-inactive');
        }

        if (nextFunction && nextFunction.length) {
            nextBtn.attr('onClick', nextFunction);
            floatButton.removeClass('next-btn-inactive');
        }
        else {
            floatButton.addClass('next-btn-inactive');
        }
    };

    function setButtonText(prevText, nextText) {
        if (prevText && prevText.length) {
            prevBtn.val(prevText);
        }

        if (nextText && nextText.length) {
            nextBtn.val(nextText);
        }
    };

    function slideTopMostScreen(screenGroup) {
        formClose.removeClass('active active--transition');

        var lastActiveScreen = screenGroup.find('.screen-group--item.active').last();

        toggleBtnClick(prevBtn);
        lastActiveScreen.removeClass('active');
        validateScreen.hideError();

        setTimeout(function () {
            formClose.addClass('active');
        }, 500);
    };

    function slideNextScreen(screenGroup) {
        formClose.removeClass('active active--transition');
        var lastActiveScreen = screenGroup.find('.screen-group--item.active').last();

        toggleBtnClick(nextBtn);
        lastActiveScreen.next('.screen-group--item').addClass('active');

        setTimeout(function () {
            formClose.addClass('active active--transition');
        }, 350);

        if ($('#summaryDetailed').find('ul').length) {
            $('#summaryToolbar').show();
            $('#formContainer').addClass('toolbar-active');
        }
    };

    function toggleBtnClick(btn) {
        btn.attr('disabled', true);
        setTimeout(function () {
            btn.attr('disabled', false);
        }, 700);
    };

    function setLoadingScreen() {
        loadingScreen.show();
    };

    function removeLoadingScreen() {
        loadingScreen.hide();
    };

    function getSourceId() {
        var match = RegExp('[?&]src=([^&]*)').exec(window.location.search);
        if (match) {
            var source = match[1];
            if (source.toLowerCase() === 'android') {
                return 74;
            }
            else if (source.toLowerCase() === 'ios') {
                return 83;
            }
        }
        return parseInt($('#introContainer').data('sourceid'));
    };

    function onPageUnload(e) {
        e.returnValue = 'Are You sure!!! Changes will not be saved';
    }
    return {
        setNavigationTab: setNavigationTab,
        setNavigateAwayModal: setNavigateAwayModal,
        setIntroScreen: setIntroScreen,
        removeIntroScreen: removeIntroScreen,
        setButtonTarget: setButtonTarget,
        setButtonText: setButtonText,
        slideTopMostScreen: slideTopMostScreen,
        slideNextScreen: slideNextScreen,
        setLoadingScreen: setLoadingScreen,
        removeLoadingScreen: removeLoadingScreen,
        getSourceId: getSourceId,
        onPageUnload: onPageUnload,
        isPresentInArray: isPresentInArray,
        navigationTab: navigationTab,
        isTabActive: isTabActive
    };

})();

var selectionTag = (function () {
    var crossIcon = '<span class="pill__cross"></span>';

    function attach(container, label) {
        var template = getTemplate(label);
        container.append(template);
    };

    function detach(element) {
        element.remove();
    };

    function getTemplate(label) {
        var template = '<span class="btn-pill pill--auto-12 pill--active">' + label + crossIcon + '</span>';
        return template;
    };

    return { attach: attach, detach: detach };
})();

var chosenSelect = (function () {
    function noResultSelection(selectField, inputField) {
        addCustomOption(selectField, inputField);
    };

    function addCustomOption(selectField, inputField) {
        var inputValue = inputField.val(),
            template = '<option value="-1">' + inputValue + '</option>';

        selectField.append(template);
        selectField.val("-1").change();
        inputField.closest('.chosen-container').removeClass('chosen-with-drop');
        selectField.trigger('chosen:updated');
    };

    function removeInputField(selectBox) {
        var placeholderText = selectBox.find('.chosen-select').attr('data-title'),
            searchBox = selectBox.find('.chosen-search');

        searchBox.empty().append('<p class="no-input-label">' + placeholderText + '</p>');
    };

    return { noResultSelection: noResultSelection, removeInputField: removeInputField };
})();

var formField = (function () {
    function scrollToTop(field) {
        var noScrollInput = $(field).hasClass('noScrollToTop');

        if (!noScrollInput) {
            var fieldPosition = field.offset().top - 50;

            $('html, body').animate({
                scrollTop: fieldPosition
            }, 200);
        }
    };

    function resetInput(inputField) {
        inputField.closest('.input-box').removeClass('done');
        inputField.val('');
    };

    function resetSelect(selectField) {
        selectField.closest('.select-box').removeClass('done');
        selectField.html('<option value="0"></option>').val("0").change();
        selectField.trigger('chosen:updated');
    };

    function emptySelect(selectField) {
        selectField.closest('.select-box').removeClass('done');
        selectField.val("0").change().trigger('chosen:updated');
    };

    return { scrollToTop: scrollToTop, resetInput: resetInput, resetSelect: resetSelect, emptySelect: emptySelect };
})();

var validate = (function () {
    var field = {
        setError: function (element, message) {
            var fieldBox = element.closest('.field-box');
            fieldBox.addClass('invalid');
            if (fieldBox.hasClass('input-box')) {
                if (element.val().length > 0) {
                    fieldBox.addClass('done');
                }
                else {
                    fieldBox.removeClass('done');
                }
            }
            else if (fieldBox.hasClass('select-box')) {
                if (parseInt(element.val()) > 0) {
                    fieldBox.addClass('done');
                }
                else {
                    fieldBox.removeClass('done');
                }
            }

            fieldBox.find('.error-text').text(message);
        },

        hideError: function (element) {
            var fieldBox = element.closest('.field-box');
            fieldBox.removeClass('invalid').addClass('done');
            fieldBox.find('.error-text').text('');
        }
    };

    return { field: field };
})();

var validateScreen = (function () {
    var errorField;
    var eventName = {
        documentReady: "commonDocReady",
    }
    if (typeof events !== 'undefined') {
        events.subscribe(eventName.documentReady, setSelectors);
    }

    function setSelectors() {
        errorField = $('#screenError');
    };

    function setError(message) {
        errorField.show().find('.error-text').text(message);
    };

    function hide()
    {
        errorField.hide();
    }

    function show()
    {
        if (errorField.find('.error-text').text())
        {
            errorField.show();
        }
    }

    function hideError() {
        errorField.hide().find('.error-text').text('');
    };

    return { setError: setError, hideError: hideError, hide: hide, show:show };
})();

var modalPopup = (function (modalBox) {
    var eventName = {
        documentReady: "commonDocReady",
    }

    if (typeof events !== 'undefined') {
        events.subscribe(eventName.documentReady, registerDomEvents);
    }

    function registerDomEvents() {
        $(document).on('click', '.modal-box .modal__close', function () {
            history.back();
        });
    };

    function showModal(htmlString, modalBox) {
        modalBox = modalBox || $('#modalPopUp');
        $('body').addClass('lock-browser-scroll').find('#modalBg').show();
        modalBox.html(htmlString).show();
        historyObj.addToHistory('showModal');
    };

    function hideModal(modalBox) {
        modalBox = modalBox || $('#modalPopUp');
        $('body').removeClass('lock-browser-scroll').find('#modalBg').hide();
        modalBox.html('').hide();
    };

    function isVisible(modalBox) {
        modalBox = modalBox || $('#modalPopUp');
        return modalBox.is(':visible');
    }

    function closeActiveModalPopup() {
        if (isVisible()) {
            hideModal();
            return true;
        }
        return false;
    };

    function isOtpModalPopupVisible() {
        if ($("#modalBg").is(":visible")) {
            popUp.hidePopUp();
            return true;
        }
        return false;
    }
    function lockScroll() {
        var html_el = $('html'),
			body_el = $('body');

        if (Common.doc.height() > $(window).height()) {
            var scrollTop = (html_el.scrollTop()) ? html_el.scrollTop() : body_el.scrollTop(); // Works for Chrome, Firefox, IE...

            if (scrollTop < 0) {
                scrollTop = 0;
            }
            html_el.addClass('lock-browser-scroll').css('top', -scrollTop);
            if (scrollTop > 40) {
                setTimeout(function () {
                    $('#header').addClass('header-fixed-with-bg');
                }, 10);
            }
        }
        Common.isScrollLocked = true;
    }

    function unlockScroll() {
        var scrollTop = parseInt($('html').css('top'));

        $('html').removeClass('lock-browser-scroll');
        $('html, body').scrollTop(-scrollTop);
        Common.isScrollLocked = false;
    }

    function showModalJson(json, modalBox) {
        modalBox = modalBox || $('#modalPopUp');
        var parsedJson = JSON.parse(json);
        $('#modalBg').show();
        modalBox.html(templates.modalPopupTemplate(parsedJson)).show();
        lockScroll();
    }
    return {
        showModal: showModal,
        hideModal: hideModal,
        closeActiveModalPopup: closeActiveModalPopup,
        lockScroll: lockScroll,
        unlockScroll: unlockScroll,
        isOtpModalPopupVisible: isOtpModalPopupVisible,
        showModalJson: showModalJson
    }
})();

var ajaxRequest = (function () {
    var setting;

    if (typeof events !== 'undefined') {
        events.subscribe("carDetailsPosted", verifyMobile);
        events.subscribe("mobileVerified", takeLive);
        events.subscribe("insuranceSubmitted", getCarImagesHtml);
        events.subscribe("updateContactDetails", verifyMobile);
    };

    function getJsonPromise(url) {
        settings = {
            "async": true,
            "url": url,
            "method": "GET",
            "dataType": "json",
        }
        return $.ajax(settings);
    };

    function verifyMobile(eventObj) {
        var data = {};
        if (eventObj && eventObj.data)
            data = appState.setSelectedData(data, eventObj.data);
        var settings = {
            url: "/api/v1/used/sell/verify/",
            type: "POST",
            data: data.sellCarCustomer.mobile,
            contentType: 'application/json',
            dataType: 'json',
            headers: {
                sourceid: parentContainer.getSourceId(),
            }
        };
        $.ajax(settings).done(function (response) { eventObj.callback(response, data); }).fail(function (xhr) {
            parentContainer.removeLoadingScreen();
            modalPopup.showModalJson(xhr.responseText);
        });
    }

    function takeLive(eventObj) {
        var data = {};
        if (eventObj && eventObj.data)
            data = appState.setSelectedData(data, eventObj.data);
        parentContainer.setLoadingScreen();
        var settings = {
            url: "/api/v2/used/sell/live/",
            type: "POST",
            data: JSON.stringify($.cookie("TempInquiry")),
            contentType: 'application/json',
            dataType: 'json',
            headers: {
                sourceid: parentContainer.getSourceId(),
            }
        };
        $.ajax(settings).done(function (response) {
            parentContainer.removeLoadingScreen();
            sellCarCookie.deleteTempInquiryCookie();
            sellCarCookie.setSellInquiryCookie(response.inquiryId);
            if (typeof events !== 'undefined') {
                var eventObj = {
                    data: data,
                };
                events.publish("takenLive", eventObj);
                events.publish("updateNavigationTabClick", { tabNameArray: [parentContainer.navigationTab.contact,parentContainer.navigationTab.carDetails], value: false });
            }
        }).fail(function (xhr) {
            parentContainer.removeLoadingScreen();
            modalPopup.showModalJson(xhr.responseText);
        });
    }

    function getCarImagesHtml(eventObj) {
        if (eventObj && eventObj.data && !eventObj.isCarImagesViewLoaded) {
            var data = {};
            data = appState.setSelectedData(data, eventObj.data);
            var settings = {
                url: "carimages/",
                type: "GET"
            };
            $.ajax(settings).done(function (response) {
                if (typeof events !== 'undefined') {
                    var eventObj = {
                        data: data,
                        response: response
                    };
                    events.publish("carImageHtmlSuccess", eventObj);
                }
            }).fail(function (xhr) {
                parentContainer.removeLoadingScreen();
                modalPopup.showModal(xhr.responseText);
            });
        } else if (typeof events !== 'undefined') {
            events.publish('carImageHtmlSuccess', eventObj);
        }
    }

    return { getJsonPromise: getJsonPromise };
})();

var templates = (function () {
    function fillColorTemplate(obj) {
        return obj.map(function (curr) {
            return '<li class="option-list__item"> ' +
                    '<input type="radio" id="' + curr.colorId + '" name="carColour" value="' + curr.colorVal + '"> ' +
                    '<label for="' + curr.colorId + '"> ' +
                        '<span class="list-item__icon" style="background-color: #' + curr.colorHash + '"></span> ' +
                        '<span class="list-item__label">' + curr.colorName + '</span> ' +
                    '</label> ' +
                '</li>'
        });
    };

    function fillDropDownTemplate(obj) {
        return obj.map(function (curr) {
            return '<option value="' + curr.val + '">' + curr.text + '</option>';
        });
    };

    function modalPopupTemplate(obj) {
        var template = '';
        template += '<div class="error-popup-template">';
        template += '<span class="modal__close cross-default-15x16"></span>';
        template += '<p class="modal__header">' + obj.heading + '</p>';
        if (obj.description.length > 0) {
            template += '<p class="modal__description">' + obj.description + '</p>';
        }
        if (obj.isNoButtonActive) {
            template += '<a href="' + obj.noButtonLink + '" class="btn btn-white btn-124-36 modal_navigate_away">' + obj.noButtonText + '</a>';
        }
        if (obj.isYesButtonActive) {
            template += '<a href="' + obj.yesButtonLink + '" class="btn btn-orange btn-124-36 modal__close">' + obj.yesButtonText + '</a>';
        }
        template += '</div>';
        return template;
    };

    return {
        fillColorTemplate: fillColorTemplate,
        modalPopupTemplate: modalPopupTemplate,
        fillDropDownTemplate: fillDropDownTemplate
    };
})();

var summary = (function () {
    var summaryDetail, summaryToolbar, detailSummaryTarget, summaryModalBg;

    var eventName = {
        documentReady: "commonDocReady",
    }

    if (typeof events !== 'undefined') {
        events.subscribe(eventName.documentReady, setSelectors);
        events.subscribe(eventName.documentReady, registerDomEvents);
    }

    function setSelectors() {
        summaryToolbar = $('#summaryToolbar');
        summaryDetail = $('#summaryDetailed');
        detailSummaryTarget = $('#detailedSummaryTarget');
        summaryModalBg = $('#summaryModalBg');
    };

    function registerDomEvents() {
        $(detailSummaryTarget).on('click', function () {
            if (!summaryToolbar.hasClass('detail-active')) {
                summaryToolbar.addClass('detail-active');
                historyObj.addToHistory('summaryOpen');
            }
            else {
                history.back();
            }
        });

        $(summaryModalBg).on('click', function () {
            $(detailSummaryTarget).trigger('click');
        });
    };

    function closeSummary() {
        if (summaryToolbar.hasClass('detail-active')) {
            summaryToolbar.removeClass('detail-active');
        }
    };

    function closeActiveSummary()
    {
        var isSummaryActive = false;
        if (summary.isSummaryActive()) {
            $(detailSummaryTarget).trigger('click');
            isSummaryActive = true;
        }
        return isSummaryActive;
    }

    function isSummaryactive() {
        return summaryToolbar.hasClass('detail-active');
    }

    function setList(formContainer) {
        var formScreen = formContainer.closest('.form-screen').length ? formContainer.closest('.form-screen') : formContainer;

        var listId = formScreen.attr('id') + '__list';

        if (!summaryDetail.find('#' + listId).length) {
            summaryDetail.append('<p class="summary-form__head">' + formScreen.attr('data-form-title') + '</p>');
            summaryDetail.append('<ul id="' + listId + '" class="summary-form__list"></ul>');
        }

        return $('#' + listId);
    };

    function setSummary(formScreenItem) {
        var summaryList = summary.setList(formScreenItem);

        var groupBox = formScreenItem.find('.group-box');

        if (groupBox.length > 0) {
            groupBox.each(function () {
                var combinedFieldText = '',
					fieldBox = $(this).find('.field-box');

                fieldBox.each(function () {
                    if ($(this).hasClass('no-auto-summary')) {
                        return;
                    }

                    var fieldObj = summary.getFieldDetails($(this));

                    combinedFieldText += fieldObj.fieldValue + ' ';
                });

                var combinedFieldObj = {
                    field: $(this),
                    fieldValue: combinedFieldText
                }

                summary.setListItem(summaryList, combinedFieldObj);
            });
        }

        var fieldBox = formScreenItem.find('.field-box');

        fieldBox.each(function () {
            if ($(this).hasClass('no-auto-summary') || $(this).closest('.group-box').length) {
                return;
            }
            var fieldObj = summary.getFieldDetails($(this));

            summary.setListItem(summaryList, fieldObj);
        });
    };

    function setManualGroupSummary(formScreenItem) {
        var summaryList = summary.setList(formScreenItem);

        var groupBox = formScreenItem.find('.group-box');

        if (groupBox.length > 0) {
            groupBox.each(function () {
                var combinedFieldText = '',
					fieldBox = $(this).find('.field-box');

                fieldBox.each(function () {
                    var fieldObj = summary.getFieldDetails($(this));

                    if (fieldObj.fieldValue.length) {
                        combinedFieldText += fieldObj.fieldValue + ' ';
                    }
                });

                var combinedFieldObj = {
                    field: $(this),
                    fieldValue: combinedFieldText
                }

                summary.setListItem(summaryList, combinedFieldObj);
            });
        }
    };

    function getFieldDetails(fieldBox) {
        var field,
			fieldValue;

        if (fieldBox.hasClass('input-box')) {
            field = fieldBox.find('input');
            fieldValue = field.val();
        }
        else if (fieldBox.hasClass('select-box')) {
            field = fieldBox.find('select');
            fieldValue = field.find('option:selected').text();
        }
        else if (fieldBox.hasClass('radio-box')) {
            field = fieldBox;

            var radioLabel = field.find('input[type=radio]:checked').siblings('label');

            if (radioLabel.find('.list-item__label').length) {
                fieldValue = radioLabel.find('.list-item__label').text();
            }
            else {
                fieldValue = radioLabel.text();
            }
        }

        return {
            field: field,
            fieldValue: fieldValue
        };
    };

    function setListItem(summaryList, fieldObj) {
        if (fieldObj.fieldValue !== "") {
            var existingListItem = summaryList.find('li[data-field-id="' + fieldObj.field.attr('id') + '"]');

            var fieldBox = fieldObj.field.closest('.field-box').length ? fieldObj.field.closest('.field-box') : fieldObj.field;

            if (!existingListItem.length) {
                var listItem = '';

                listItem += '<li data-field-id="' + fieldObj.field.attr('id') + '" data-title-toolbar="' + fieldBox.attr('data-title-toolbar') + '">';
                listItem += '<p class="summary-item__key">' + fieldBox.attr('data-title') + '</p>';
                listItem += '<p class="summary-item__value">';
                if (typeof fieldBox.attr('data-prefix') !== "undefined") {
                    listItem += fieldBox.attr('data-prefix') + ' ';
                }
                listItem += '<span class="item-value__data">' + fieldObj.fieldValue + '</span>';
                if (typeof fieldBox.attr('data-suffix') !== "undefined") {
                    listItem += ' ' + fieldBox.attr('data-suffix');
                }
                listItem += '</p>';
                listItem += '</li>';

                summaryList.append(listItem);
            }
            else {
                if (existingListItem.find('.item-value__data').text() !== fieldObj.fieldValue) {
                    existingListItem.find('.item-value__data').text(fieldObj.fieldValue);
                    summary.removeListItem(fieldObj.field.attr('id'));
                }
            }

            summary.setToolbar();
        }
    };

    function removeListItem(fieldId) {
        var fieldItem = summaryDetail.find('li[data-field-id=' + fieldId + ']');

        fieldItem.nextAll('li').remove();
        fieldItem.closest('.summary-form__list').nextAll().remove();
    };

    function setToolbar() {
        var listItems = summaryDetail.find('li');

        var itemArray = listItems.map(function () {
            var item = '';

            if ($(this).attr('data-title-toolbar') == "true") {
                item += $(this).find('.summary-item__key').text() + ": ";
            }

            item += $(this).find('.summary-item__value').text();

            return item;
        });

        var toolbarText = itemArray.toArray().reverse().join(' | ');

        summaryToolbar.find('.toolbar__label').text(toolbarText);
    };

    function showToolbar() {
        if (summaryDetail.find('ul').length) {
            summaryToolbar.show();
            $('#formContainer').addClass('toolbar-active');
        }
    };

    function hideToolbar() {
        summaryToolbar.hide();
        $('#formContainer').removeClass('toolbar-active');
    };

    return {
        setList: setList,
        setSummary: setSummary,
        setManualGroupSummary: setManualGroupSummary,
        getFieldDetails: getFieldDetails,
        setListItem: setListItem,
        removeListItem: removeListItem,
        setToolbar: setToolbar,
        showToolbar: showToolbar,
        hideToolbar: hideToolbar,
        closeSummary: closeSummary,
        isSummaryActive: isSummaryactive,
        closeActiveSummary: closeActiveSummary
    };

})();

var appState = (function () {
    function setSelectedData(target, src, deepCopy) {
        if (deepCopy)
            return $.extend(true, {}, target, src);
        return $.extend({}, target, src);
    };

    function deleteObjectProperties(obj, props) {
        for (var i = 0; i < props.length; i++) {
            if (obj.hasOwnProperty(props[i])) {
                delete obj[props[i]];
            }
        }
        return obj;
    };

    return { setSelectedData: setSelectedData, deleteObjectProperties: deleteObjectProperties };
})();

var formatValue = (function () {
    function withComma() {
        var fieldValue = this.value;

        fieldValue = fieldValue.replace(/[^\d]/g, "").replace(/^0+/, "");
        this.setAttribute('data-value', fieldValue);
        this.value = Common.utils.formatNumeric(fieldValue);
    }

    function formatValueOnInput(inputField) {
        inputField.on('input propertychange', formatValue.withComma);
        inputField.on('input propertychange', function () {
            readableTextFromNumber(inputField);
        });
    }

    function readableTextFromNumber(input) {
        var placeHolder = input.siblings("div .getNumbersInWord");
        placeHolder.text("");
        placeHolder.text(cardetailsUtil.capitalizeFirstLetter(cardetailsUtil.numberToWords((input).attr("data-value"))));
    }
    return {
        withComma: withComma,
        formatValueOnInput: formatValueOnInput,
        readableTextFromNumber: readableTextFromNumber
    }
})();

var historyHelper = (function () {
    var historyIndex = 0;

    if (typeof events !== 'undefined') {
        events.subscribe("updateHistoryIndex", updateHistoryIndex);
        events.subscribe("popHistory", popHistoryIndex);
        events.subscribe("pushHistory", pushHistoryIndex);
        events.subscribe("historynull", clearHistory);
    }

    function clearHistory() {
        historyIndex = 0;
    }

    function updateHistoryIndex(data) {
        if (data && data.index) {
            historyIndex += data.index;
        }
    }
    function popHistoryIndex() {
        if (historyIndex)
        {
            historyIndex--;
            events.publish("historyIndexPoped", { index: historyIndex + 1 });
        }
    }
    function pushHistoryIndex() {
        historyIndex++;
        events.publish("historyIndexChanged", { index: historyIndex });
    }
    function getHistoryIndex() {
        return historyIndex;
    }
    return { getHistoryIndex: getHistoryIndex }
})();

var historyObj = (function () {
    function addToHistory(currentState, title, url) {
        history.pushState({ currentState: currentState }, title, url);
        events.publish("pushHistory");
    }

    function replaceHistory(currentState, title, url) {
        history.replaceState({ currentState: currentState }, title, url)
    }

    function goToIndex(index) {
        if (index != 0) {
            history.go(index);
        }
    }
    return { addToHistory: addToHistory, replaceHistory: replaceHistory, goToIndex: goToIndex }
})();

var sellCarCookie = (function () {
    var cookieName = { sellInquiry: 'SellInquiry', tempInquiry: 'TempInquiry' };


    function setSellInquiryCookie(cookieValue) {
        setCookie(cookieName.sellInquiry, cookieValue, 90);
    }

    function deleteSellInquiryCookie() {
        setCookie(cookieName.sellInquiry, null, -1);
    }

    function setTempInquiryCookie(cookieValue) {
        setCookie(cookieName.tempInquiry, cookieValue, 90);
    }

    function deleteTempInquiryCookie() {
        setCookie(cookieName.tempInquiry, null, -1);
    }

    function getSellInquiryCookie() {
        return $.cookie(cookieName.sellInquiry);
    }
    function setCookie(name, val, expireInDays) {
        var today = new Date();
        var expire = new Date();
        expire.setTime(today.getTime() + 3600000 * 24 * expireInDays);
        document.cookie = name + "=" + val + ";expires=" + expire.toGMTString() + "; domain=" + defaultCookieDomain + '; path =/';
    }

    return {
        setSellInquiryCookie: setSellInquiryCookie,
        deleteSellInquiryCookie: deleteSellInquiryCookie,
        setTempInquiryCookie: setTempInquiryCookie,
        deleteTempInquiryCookie: deleteTempInquiryCookie,
        getSellInquiryCookie: getSellInquiryCookie
    }

})();

$(document).ready(function () {
    if (typeof events !== 'undefined') {
        events.publish("commonDocReady");
    }
});

var popUp = (function () {
    function showPopUp() {
        element = $("." + $('#modalPopUp').attr("data-current"));
        $('#modalBg').show();
        $('.modal-box').show();
        (element).appendTo($(".modal-box")).show();
        modalPopup.lockScroll();
    }
    function hidePopUp() {
        element = $("." + $('#modalPopUp').attr("data-current"));
        $('#modalBg').hide();
        $('.modal-box').hide().empty();
        (element).appendTo(".popup-box-container");
        modalPopup.unlockScroll();
    }
    return {
        showPopUp: showPopUp,
        hidePopUp: hidePopUp
    }

})();

var iphoneInputFocus = {
    OnFocus: function () {
        if (navigator.userAgent.match(/(iPod|iPhone|iPad)/)) {
            $("body").css({ "position": "fixed", "width": "100%" });
        }
    },
    OutFocus: function () {
        if (navigator.userAgent.match(/(iPod|iPhone|iPad)/)) {
            $("body").css({ "position": "", "width": "" });
        }
    }
};

//Function to initialize chosen
function ChosenInit(containerName) {
    var selectBox = containerName.find('.select-box');
    selectBox.each(function () {
        var element = $(this);
        element.find('.chosen-select').chosen({
            width: '100%'
        });
        if (element.hasClass('select-box-no-input')) {
            chosenSelect.removeInputField(element);
        }
    });

};

