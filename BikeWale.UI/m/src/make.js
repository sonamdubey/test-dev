﻿var BikeFiltersPopup = (function () {
    var container, backgroundWindow, closeBtn;

    function _setSelectors() {
        container = $('#filtersPopup');
        backgroundWindow = $('#filtersBlackoutWindow');
        closeBtn = $('#filterClose');
    }

    function registerEvents() {
        _setSelectors();
        _setBodyDimension();

        $(backgroundWindow).on('click', function () {
            if (container.hasClass('filters-screen--active')) {
                window.history.back();
            }
        });

        $('#filterClose').on('click', function () {
            backgroundWindow.trigger('click');
        });

        $(window).on('popstate', function () {
            if (container.hasClass('filters-screen--active') && history.state === "recommendedBikePopup") {
                close();
            }
        });
    }

    function _setBodyDimension() {
        var bodyHeight = container.find('.filters__screen').height() - container.find('.filters-screen__head').height();

        container.find('.filters-screen__body').css('height', bodyHeight);
    }

    function open() {
        container.addClass('filters-screen--active');
        history.pushState('filtersPopup', '', '');
    }

    function close() {
        container.removeClass('filters-screen--active');
    }

    return {
        registerEvents: registerEvents,
        open: open,
        close: close
    }



})();

var Accordion = (function () {
    function registerEvents() {
        $('.accordion__list').on('click', '.accordion__head', function () {
            handleClick($(this))
        });
    }

    function handleClick(accordionHead) {
        var accordionList = accordionHead.closest('.accordion__list');

        if (accordionList.attr('data-state') === 'one') {
            var accordionItem = accordionHead.closest('.accordion-list__item');
            var accordionSiblingItems = accordionItem.siblings('.accordion-list__item');

            accordionSiblingItems.find('.accordion-item--active').removeClass('accordion-item--active');
            accordionSiblingItems.find('.accordion__body').css('height', 0);

            var accordionBody = accordionHead.siblings('.accordion__body');
            var accordionContentHeight = accordionBody.find('.accordion-body__content').outerHeight(true);

            if (!accordionHead.hasClass('accordion-item--active')) {
                accordionHead.addClass('accordion-item--active');
                accordionBody.css('height', accordionContentHeight);
            }
            else {
                accordionHead.removeClass('accordion-item--active');
                accordionBody.css('height', 0);
            }
        }

    }

    return {
        registerEvents: registerEvents
    }
})();

//Adjust this value according to the number of upfront discontinued bike models (before `View All` is clicked) to be shown at the bottom.
var discontinuedBikesToShowUpfront = 4;

docReady(function () {

    $('.overall-tabs__list .overall-tabs-list__item').first().addClass('tab--active');

    $('.model-card__pros-cons').on('click', '.pros-cons__more-btn', function (event) {
        $(this).hide();
        $(this).closest('.pros-cons__content').find('li').show();
    });
    $("#notifySubmitBtn").on("click", function () {
      
        var flag = validateForm.emailField($("#notifyEmailField"));
        if (flag) {
            executeNotification($(this));
        }
    });

    $('#allIndiaUrl').on('click',function()
    {
       $.removeCookie('location', { path: '/' });
        window.location = $(this).attr('data-url');
    });

    // popular bikes carousel
    if (navigator.userAgent.match(/Firefox/gi) || navigator.userAgent.match(/UCBrowser/gi)) {
        $('.carousel__popular-bikes').addClass('popular-bikes--fallback');
    }

    $('.carousel__popular-bikes').on('click', '.view-pros-cons__target', function () {
        var modelCard = $(this).closest('.model__card');

        $(this).hide();

        if (!modelCard.hasClass('card--active')) {
            modelCard.addClass('card--active');
        }
        else {
            modelCard.removeClass('card--active');
        }
    });

    $('.carousel__popular-bikes').on('webkitTransitionEnd transitionend', '.model-card__detail', function () {
        var modelCard = $(this).closest('.model__card');
        var collpaseTargetElement = modelCard.find('.view-pros-cons__target');

        var collapseCurrentText = collpaseTargetElement.html(),
            collapseNextText = collpaseTargetElement.attr('data-text');

        if (!collpaseTargetElement.is(':visible')) {
            if (!modelCard.hasClass('card--active')) {
                modelCard.removeClass('collapse-btn--active');
            }
            else {
                modelCard.addClass('collapse-btn--active');
            }

            collpaseTargetElement.attr('data-text', collapseCurrentText);
            collpaseTargetElement.html(collapseNextText).fadeIn();
        }
    });

    // upcoming card: notify
    notifyPopup.registerEvents();
    

    $('.upcoming-card__notify-btn').on('click', function () {
        $('.notify-details__model').text($(this).attr('data-bikeName'));
        $('.form-field__submit-btn').attr('data-bikename', $(this).attr('data-bikename'));
        $('.form-field__submit-btn').attr('data-makeid', $(this).attr('data-makeid'));
        $('.form-field__submit-btn').attr('data-modelid', $(this).attr('data-modelid'));
        notifyPopup.open();
    });


    $('.emicalculator_link_event').on('click', function () {
        var data = $(this);

        var emiDetails = JSON.parse(atob(data.data('emidetails')));
        var bikePrice = data.data('bikeprice');

        EMIviewModel.processingFees(emiDetails.processingFee);
        EMIviewModel.exshowroomprice(bikePrice);
        EMIviewModel.bikePrice(bikePrice);
        EMIviewModel.minTenure(emiDetails.minTenure);
        EMIviewModel.maxTenure(emiDetails.maxTenure);
        EMIviewModel.minDnPay(emiDetails.minDownPayment);
        EMIviewModel.maxDnPay(emiDetails.maxDownPayment);
        EMIviewModel.minROI(emiDetails.minRateOfInterest);
        EMIviewModel.maxROI(emiDetails.maxRateOfInterest);
        EMIviewModel.loan(emiDetails.minLoanToValue);

        var emiPopup = $('#emiPopup');
        emiCalculator.open(emiPopup);

    });

    //For Discontinued models at the bottom of the make page (taken from `bwm-brand.js`)

    var noOfBikes = $("#discontinuedMore a").length;
    if (noOfBikes > discontinuedBikesToShowUpfront) {
        $('#discontinuedMore').hide();
    }
    else {
        $('#discontinuedLess').hide();
    }

    for (var i = 0; i < discontinuedBikesToShowUpfront; i++) {
        $("#spnContent").append($("#discontinuedMore a:eq(" + i + ")").clone());
        if (i == noOfBikes-1) {
            break;
        }
        if (i < discontinuedBikesToShowUpfront-1) {
            $("#spnContent").append(", ");
        }
    }
    if (discontinuedBikesToShowUpfront < noOfBikes) {
        $("#spnContent").append("... <a class='f-small' id='viewall' >View All</a>");
    }

    $("#viewall").click(function () {
        $("#discontinuedLess").hide();
        $("#discontinuedMore").show();
        var xContents = $('#discontinuedMore').contents();
        xContents[xContents.length - 1].nodeValue = "";
    });

    formField.registerEvents();

    //interesting fact popup
    interestingFactPopup.registerEvents();

    //floating navbar
    floatingNav.registerEvents();

    //recommended bike popup
    recommendedBikePopup.registerEvents();

    // filters popup
    BikeFiltersPopup.registerEvents();
    if (vmRecommendedBikes != null && vmRecommendedBikes.searchFilter!=null)
    {
        vmRecommendedBikes.searchFilter.makeId = $('#hdnMakeId').val();
        vmRecommendedBikes.searchFilter.cityId = $('#hdnCityId').val();
        vmRecommendedBikes.searchFilter.makeName = $('#makeName').val();
    }

    Accordion.registerEvents();
});

var floatingNav = (function () {
    var overallTabsContainer, overallContainer;

    function _setSelectores() {
        overallTabsContainer = $('.overall-tabs__content');
        overallContainer = $('#overallContainer');
        topNavBarHeight = overallTabsContainer.height();
    }

    function _setFallback() {
        if (navigator.userAgent.match(/OPR/gi)) {
            $('.overall-tabs__content').addClass('overall-tabs--fallback');
        }
    }

    function registerEvents() {
        _setSelectores();
        _setFallback();

        $(window).scroll(function () {
            var windowScrollTop = $(window).scrollTop(),
                specsTabsOffsetTop = $('.overall-tabs__placeholder').offset().top,
                overallContainerHeight = overallContainer.outerHeight();

            if (windowScrollTop > specsTabsOffsetTop) {
                overallTabsContainer.addClass('fixed-tab-nav');
            }

            else if (windowScrollTop < specsTabsOffsetTop) {
                overallTabsContainer.removeClass('fixed-tab-nav');
            }

            if (overallTabsContainer.hasClass('fixed-tab-nav')) {
                if (windowScrollTop > Math.ceil(overallContainerHeight) - (topNavBarHeight)) {
                    overallTabsContainer.removeClass('fixed-tab-nav');
                }
            }

            $('#overallContainer .overall-tabs-data').each(function () {
                var top = $(this).offset().top - topNavBarHeight,
                    bottom = top + $(this).outerHeight();
                if (windowScrollTop >= top && windowScrollTop <= bottom) {

                    $(this).addClass('tab--active');
                    var currentActiveTab = overallTabsContainer.find('li[data-tabs="' + $(this).attr('data-id') + '"]');
                    if (overallTabsContainer.attr('data-clicked') != '1' && !currentActiveTab.hasClass('tab--active')) {
                        centerNavBar($('li[data-tabs="' + $(this).attr('data-id') + '"]'), overallTabsContainer);
                        overallTabsContainer.find('li').removeClass('tab--active');
                        setTimeout(function () {
                            overallTabsContainer.find('li').removeClass('tab--active');
                            $('#overallContainer .overall-tabs-data').removeClass('tab--active');

                            overallTabsContainer.find(currentActiveTab).addClass('tab--active');
                        }, 10);

                    }
                    else {
                        overallTabsContainer.find('li').removeClass('tab--active');
                        $('#overallContainer .overall-tabs-data').removeClass('tab--active');
                        overallTabsContainer.find(currentActiveTab).addClass('tab--active');
                    }


                }

            });

        });
        $('.overall-tabs__list li').on('click', function () {
            var target = $(this).attr('data-tabs'),
                topNavBarHeight = overallTabsContainer.height();
            overallTabsContainer.attr('data-clicked', '1');
            centerItVariableWidth($(this), overallTabsContainer)
            $('html, body').animate({ scrollTop: Math.ceil($(".overall-tabs-data[data-id=" + target + "]").offset().top) - topNavBarHeight }, 1000, function () {
                overallTabsContainer.attr('data-clicked', '0');
            });

        });
        function centerNavBar(target, outer) {
            var out = $(outer);
            var tar = target;
            var x = out.width();
            var y = tar.outerWidth(true);
            var z = tar.index();
            var q = 0;
            var m = out.find('li');
            //Just need to add up the width of all the elements before our target.
            for (var i = 0; i < z; i++) {
                q += $(m[i]).outerWidth(true);
            }
            out.animate({ scrollLeft: Math.max(0, q - (x - y) / 2) }, 10, 'swing');
        }
    }

    return {
        registerEvents: registerEvents
    }
})();

function executeNotification(buttonElement) {
    var userData = {
        "emailId": $("#notifyEmailField").val(),
        "makeId": $('.form-field__submit-btn').attr("data-makeid"),
        "modelId": $('.form-field__submit-btn').attr("data-modelid"),
        "bikeName": $('.form-field__submit-btn').attr("data-bikename"),
    };
    $.ajax({
        type: "POST",
        url: "/api/notifyuser/",
        contentType: "application/json",
        data: ko.toJSON(userData),
        success: function (response) {
            if (response) {
                notifyPopup.setSuccessState(buttonElement);
            }
            else {
                validateForm.setError($('#notifyEmailField'), "An error has occured");
            }
        },
        error: function (response) {
            validateForm.setError($('#notifyEmailField'), "An error has occured");
        }

    });
}
/* upcoming bikes set notification popup */
var notifyPopup = (function () {
    var container, emailField, formSubmitBtn;

    function _setSelectors() {
        container = $('#notifyPopup');
        emailField = $('#notifyEmailField');
        formSubmitBtn = $('#notifySubmitBtn');
    }

    function _resetForm() {
        formField.resetInputField(emailField);
        emailField.val('');
        formSubmitBtn.html('Submit');
    }

    function registerEvents() {
        _setSelectors();

        formSubmitBtn.on('click', function () {
            var isValid = validateForm.emailField(emailField);

            if (isValid) {
                formField.setSuccessState($(this), 'Thank You!');
                setTimeout(function () {
                    $('#notifyCloseBtn').trigger('click');
                }, 1000);
            }
        });

        emailField.on('focus', function () {
            validateForm.onFocus($(this));
        });

        $('#notifyWhiteoutWindow, #notifyCloseBtn').on('click', function () {
            close();
            history.back();
        });

        $(window).on('popstate', function () {
            if (container.hasClass('filter-screen--active')) {
                close();
            }
        });
    }

    function open() {
        _resetForm();
        container.addClass('notify-popup--active');
        emailField.focus()
        appendState('notifyPopup');
        documentBody.lock();
    }

    function close() {
        container.removeClass('notify-popup--active');
        documentBody.unlock();
    }

    function setSuccessState(buttonElement) {
        formField.setSuccessState(buttonElement, 'Thank You!');
        setTimeout(function () {
            $('#notifyCloseBtn').trigger('click');
        }, 1000);
    }

    return {
        registerEvents: registerEvents,
        open: open,
        close: close,
        setSuccessState: setSuccessState
    }
})();

/* Form fields */
var formField = (function () {
    function registerEvents() {
        $(document).on('focus', '.form-field__input', function () {
            var fieldContainer = $(this).closest('.form-field__content');

            if (!fieldContainer.hasClass('btn--active')) {
                fieldContainer.addClass('btn--active');
            }
        });
    }

    function resetInputField(inputField) {
        var fieldContainer = inputField.closest('.form-field__content');

        fieldContainer.removeClass('btn--active field--success field--invalid');
        fieldContainer.find('.field__error-message').html('');
    }

    function setSuccessState(btnElement, btnText) {
        var fieldContainer = btnElement.closest('.form-field__content');

        if (!fieldContainer.hasClass('field--success')) {
            fieldContainer.addClass('field--success');
            btnElement.html(btnText);
        }
    }

    return {
        registerEvents: registerEvents,
        resetInputField: resetInputField,
        setSuccessState: setSuccessState
    }
})();

/* Form field validation */
var validateForm = (function () {

    function emailField(inputField) {
        var isValid = true,
            emailVal = inputField.val(),
            reEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;

        if (!emailVal) {
            setError(inputField, 'Please enter email id');
            isValid = false;
        }
        else if (!reEmail.test(emailVal)) {
            setError(inputField, 'Invalid Email');
            isValid = false;
        }

        return isValid;
    };

    function setError(element, message) {
        var errorTag = element.siblings('.field__error-message');

        errorTag.show().text(message);
        element.closest('.form-field__content').addClass('field--invalid');
    }

    function hideError(element) {
        element.closest('.form-field__content').removeClass('field--invalid');
        element.siblings('.field__error-message').text('');
    }

    function onFocus(inputField) {
        if (inputField.closest('.form-field__content').hasClass('field--invalid')) {
            validateForm.hideError(inputField);
        }
    }

    return {
        emailField: emailField,
        setError: setError,
        hideError: hideError,
        onFocus: onFocus
    }

})();

var interestingFactPopup = (function () {
    var container, closeBtn;

    function _setSelectores() {
        container = $('#interestingFact');
        closeBtn = $('#interestingFactCloseBtn');
    }

    function registerEvents() {
        _setSelectores();

        $('.interesting-fact__read-more').on('click', function () {
            var interestingFactContainer = $(this).closest('.interesting-fact-section'),
                windowScrollTop = $(window).scrollTop(),
                bodyShowableArea = $(window).height() * .30,
                shownArea = interestingFactContainer.offset().top - windowScrollTop;

            if (shownArea < bodyShowableArea) { // to move interesting fact container if it is visible in background after popup open 
                $('html, body').animate({ scrollTop: (windowScrollTop - (bodyShowableArea - shownArea)) }, 100);
            }
            open(interestingFactContainer);
            history.pushState('interestingFactPopup', '', '');

            /* to check content is scrollable on popup to add bottom overlay */
            if (isScrollable($('.interesting-fact__content'))) {
                interestingFactContainer.find('.fact-container__block').attr('data-overlay', 'bottom');
            }

            /* this timeout required for if background container scrolltop position changed on popup open */
            setTimeout(function () {
                documentBody.lock();
            }, 100);

        });

        closeBtn.on('click', function () {
            close();
            history.back();
        });

        $('.interesting-fact__whiteout-window').on('click', function () {
            if (container.hasClass('interesting-popup--active')) {
                history.back();
            }
        });

        $(".interesting-fact-popup .interesting-fact__content").scroll(function () {
            var interestingFactContent = $(this),
                interestingFactContainer = interestingFactContent.closest('.fact-container__block'),
                contentScrollTop = interestingFactContent.scrollTop();
            if (contentScrollTop <= 0) {
                interestingFactContainer.attr('data-overlay', 'bottom');
            }
            else if (contentScrollTop > interestingFactContent.innerHeight()) {
                interestingFactContainer.attr('data-overlay', 'top');
            }
            else {
                interestingFactContainer.attr('data-overlay', 'both');
            }

        });

        $(window).on('popstate', function () {
            if (container.hasClass('interesting-popup--active')) {
                close();
            }
        });
    }

    function open(interestingFactContainer) {
        var interestingFactContent = interestingFactContainer.find('.interesting-fact__content').html();
        container.addClass('interesting-popup--active');
        container.find('.interesting-fact__content').html(interestingFactContent);

    }

    function close() {
        container.removeClass('interesting-popup--active');
        documentBody.unlock();
    }

    function isScrollable(element) {
        return element[0].scrollWidth > element[0].clientWidth || element[0].scrollHeight > element[0].clientHeight;
    };

    return {
        registerEvents: registerEvents,
        open: open
    }
})();

var documentBody = (function () {
    function lock() {
        var htmlElement = $('html'), bodyElement = $('body');

        if ($(document).height() > $(window).height()) {
            var windowScrollTop = htmlElement.scrollTop() ? htmlElement.scrollTop() : bodyElement.scrollTop();
            if (windowScrollTop < 0) {
                windowScrollTop = 0;
            }
            htmlElement.addClass('lock-browser-scroll').css('top', -windowScrollTop);
        }
    }

    function unlock() {
        var htmlElement = $('html'),
            windowScrollTop = parseInt(htmlElement.css('top'));

        htmlElement.removeClass('lock-browser-scroll');
        $('html, body').scrollTop(-windowScrollTop);
    }

    return {
        lock: lock,
        unlock: unlock
    }

})();
