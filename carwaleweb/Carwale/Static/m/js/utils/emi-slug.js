var EmiCalculator = {
    customizedEmiDataKey: "customizedEmiData",
    defaultTenure: 5,
    defaultInterest: 9.5,
    EmiCalculatorDocReady: function () {
        EmiCalculator.setSelectors();
        EmiCalculator.registerEvents();
    },
    //Variables declared for selectors
    setSelectors: function () {
        EmiCalculator.rootContainer = '#root-container',
        EmiCalculator.customizeEmi = '.customize-emi',
        EmiCalculator.emiClose = '#emi-close',
        EmiCalculator.emiBlackOutClose = '.emi-popup_blackout-window',
        EmiCalculator.body = 'body',
        EmiCalculator.bodylock = 'bodylock',
        EmiCalculator.bodyunlock = 'bodyunlock',
        EmiCalculator.tabData = '.loan-summary-popup .cw-tabs-data'
        EmiCalculator.tabList = '.loan-summary-popup .cw-tabs li'
    },
    //All events for the selectors
    registerEvents: function () {
        if ($('#loanSummary').length) {
            var newpopup = new Popup('.js-emi-summary-link', {
                onCloseClick: function () {
                    if ($('body').hasClass('bodylock')) {
                        $('body').removeClass('bodylock');
                        if (typeof HandelBodyScroll !== 'undefined') {
                            HandelBodyScroll.unlockScroll();
                        }
                    }
                },
                onPopupOpen: function (popupContainer) {
                    if (window.outerWidth < 768) {
                        var popupHeaderHeight = popupContainer.querySelector('.popup-close-unit').offsetHeight;
                        var tabHeaderHeight = popupContainer.querySelector('.cw-tabs').offsetHeight;
                        var windowHeight = window.outerHeight;
                        var tabBody = popupContainer.querySelectorAll('.featured-popup__body');
                        for (var i = 0; i < tabBody.length; i++) {
                            tabBody[i].style.height = windowHeight - tabHeaderHeight - popupHeaderHeight + "px";
                        }
                    }
                }
            });
        }
        $(EmiCalculator.customizeEmi).unbind().on('click', function () {
            var id = $(this).attr('id');
            EmiCalculator.showPopup(id);
            EmiCalculator.focusRemoval();
            if ($('#loanSummary').length) {
                Common.utils.callTracking($('#js-vwfs-calc'), '_shown');
                Common.utils.callTracking($('#js-visit-website-link'), '_shown');
            }
        });

        $('.popup-close-button').on('click', function () {
            EmiCalculator.resetTab();
            EmiCalculator.popupHideFunctionality(false);
        })

        $(EmiCalculator.emiClose).unbind().on('click', function (event, removeState) {
            EmiCalculator.popupHideFunctionality(removeState);
        });
        $(EmiCalculator.emiBlackOutClose).unbind().on('click', function (event, removeState) {
            EmiCalculator.popupHideFunctionality(removeState);
        });
        $(".dealer-cta__btn").unbind().on('click', function () {
            if ($(".emi-pop-div").is(":visible")) {
                EmiCalculator.popupHideFunctionality(false);
                EMI_PRICE_STORE.dispatch(EMI_PRICES_EVENTS.hideEmiPopupState());
                $(EmiCalculator.body).addClass(EmiCalculator.bodyunlock);
                $(EmiCalculator.body).removeClass(EmiCalculator.bodylock);
            }
            if ($('.loan-summary-popup').hasClass('popup-active')) {
                $('.loan-summary-popup .popup-close-button').trigger("click");
            }
        });
    },
    resetTab: function () {
        $(EmiCalculator.tabData).removeAttr('style');
        $(EmiCalculator.tabList).removeClass('active');
        $(EmiCalculator.tabList).first().addClass('active');
    },

    focusRemoval: function () {
        var clickedElement;
        $(document).on('click', function (e) {
            clickedElement = $(e.target);
            clickedElement.focus();
        });
    },
    showPopup: function (id) {
        EmiCalculator.showEmiPopUpFunc(id);
        EmiCalculator.viewportHeightCalc();
        EmiCalculator.appendState("emi-popup");
    },
    popupHideFunctionality: function (removeState) {
        EmiCalculator.hideEmiPopUpFunc();
        if (typeof removeState === 'undefined' || removeState) {
            EmiCalculator.removeState();
        }
        EmiCalculator.setCustomizedEmiToClientCache();
    },
    setCustomizedEmiToClientCache : function() {
        var currentEmiObj = EMI_PRICE_STORE.getState().newEmiPrices.data.find(function (item) {
            return item.id === EMI_PRICE_STORE.getState().newEmiPrices.activeModelId;
        });
        if (currentEmiObj) {
            var currentEmiData = currentEmiObj.data;
            if (currentEmiData && currentEmiData.isEmiCustomized && currentEmiData.isEmiValid) {
                var emiData = { downpayment: parseFloat(currentEmiData.vehicleDownPayment.inputBox.value), interest: parseFloat(currentEmiData.vehicleInterest.inputBox.value), tenure: parseFloat(currentEmiData.vehicleTenure.inputBox.value) }
                clientCache.set(EmiCalculator.customizedEmiDataKey, JSON.stringify(emiData));
            }
        }
    },

    showEmiPopUpFunc: function (id) {
        if (typeof EMI_PRICE_STORE !== 'undefined') {
            EMI_PRICE_STORE.dispatch(EMI_PRICES_EVENTS.setEMIModel(id));
            EMI_PRICE_STORE.dispatch(EMI_PRICES_EVENTS.showEmiPopupState());
            scrollLockFunc.lockScroll();
            $(EmiCalculator.rootContainer).show();
            if (navigator.userAgent.match(/(iPod|iPhone|iPad)/)) {
                $(EmiCalculator.rootContainer).css({ "padding-bottom": "80px" });
            }
            if ($('.pie-collapse .accordion__body').height() !== 0) {
                $(EmiCalculator.body).removeClass(EmiCalculator.bodyunlock);
                $(EmiCalculator.body).addClass(EmiCalculator.bodylock);
            }
            else {
                $(EmiCalculator.body).addClass(EmiCalculator.bodyunlock);
                $(EmiCalculator.body).removeClass(EmiCalculator.bodylock);
            }
        }
        //else {
        //    Loader.showFullPageLoader();
        //    EmiCalculator.fetchEmiPopUp(id);
        //}
    },

    hideEmiPopUpFunc: function (dropdownField) {
        $(EmiCalculator.rootContainer).hide();
        setTimeout(function () {
            scrollLockFunc.unlockScroll();
        })
    },

    viewportHeightCalc: function () {

    },

    createEmiStateObject: function (divEmiCalculatorSlug) {
        var data = [];
        for (var model = 0; model < divEmiCalculatorSlug.length; model++) {
            var currentEmiObj = divEmiCalculatorSlug[model];
            var emiData = EmiCalculatorExtended.getCustomizedEmiData(currentEmiObj);
            var finalDownpayment = emiData.downpaymentValue;
            var finalInterest = emiData.interestValue;
            var finalTenure = emiData.tenureValue;
            var sliderInterval = EmiCalculator.getSliderIntervals(Number(currentEmiObj.getAttribute("data-downpaymentminvalue")), Number(currentEmiObj.getAttribute("data-downpaymentmaxvalue")));
            var modelData = {
                id: currentEmiObj.getAttribute("id"),
                data: {
                    vehicleData: {
                        id: Number(currentEmiObj.getAttribute("data-versionid")),
                        makeName: currentEmiObj.getAttribute("data-makename"),
                        modelName: currentEmiObj.getAttribute("data-modelname"),
                        versionName: currentEmiObj.getAttribute("data-versionname"),
                    },
                    vehicleDownPayment: {
                        inputBox: {
                            value: finalDownpayment
                        },
                        slider: {
                            max: emiData.downpaymentMax,
                            min: emiData.downpaymentMin,
                            sliderTitleRight: "On-road price",
                            snapPoints: [],
                            userChange: false,
                            values: [finalDownpayment]
                        },
                        sliderInterval: sliderInterval
                    },
                    vehicleInterest: {
                        inputBox: {
                            value: finalInterest
                        },
                        slider: {
                            max: 15,
                            min: 1,
                            userChange: false,
                            values: [finalInterest]
                        }
                    },
                    vehicleTenure: {
                        inputBox: {
                            value: finalTenure
                        },
                        slider: {
                            max: 7,
                            min: 1,
                            userChange: false,
                            values: [finalTenure]
                        }
                    },
                    emiLoanAMount: 0,
                    activeModelMin: currentEmiObj.getAttribute("data-downpaymentminvalue"),
                    activeModelMax: currentEmiObj.getAttribute("data-downpaymentmaxvalue"),
                    campaignTemplate: {
                        htmlString: divEmiCalculatorSlug[model].getAttribute("data-campaign-template"),
                        campaignType: divEmiCalculatorSlug[model].getAttribute("campaign-ad-type"),
                        leadClickSource: divEmiCalculatorSlug[model].getAttribute("data-campaign-lead-click-source"),
                        inquirySource: divEmiCalculatorSlug[model].getAttribute("data-campaign-inquiry-source"),
                    },
                    campaignDetails: {
                        modelId: divEmiCalculatorSlug[model].getAttribute("data-model-id"),
                        versionId: divEmiCalculatorSlug[model].getAttribute("data-versionid"),
                        campaignDealerId: divEmiCalculatorSlug[model].getAttribute("data-campaign-dealer-id"),
                        userLocation: divEmiCalculatorSlug[model].getAttribute("data-user-location"),
                        isCampaignAvailable: divEmiCalculatorSlug[model].getAttribute("data-is-campaign-available")
                    },
                    reactCampaignCta: currentEmiObj.getAttribute("data-react-campaign-cta"),
                    isEmiCustomized: false,
                    isEmiValid: true
                }
            }
            data.push(modelData);
        }
        return data;
    },

    setEMIModelData: function () {
        var divEmiCalculatorSlug = $('.customizeEmiContainer__js');
        var data = EmiCalculator.createEmiStateObject(divEmiCalculatorSlug);
        EMI_PRICE_STORE.dispatch(EMI_PRICES_EVENTS.setPricesData(data));
    },

    onlyUnique: function (value, index, self) {
        return self.indexOf(value) === index;
    },

    setEMIModelResult: function () {
        EmiCalculatorExtended.setEMIModelResult();
    },
    appendState: function (state) {
        window.history.pushState(state, '', '');
    },
    removeState: function () {
        window.history.back();
    },
    calculateEmi: function (finalLoanAmout, tenure, interest) {
        var numberOfMonths = tenure * 12;
        var rateOfInterest = interest;
        var monthlyInterestRatio = (rateOfInterest / 100) / 12;
        var top = Math.pow((1 + monthlyInterestRatio), numberOfMonths);
        var bottom = top - 1;
        var sp = top / bottom;
        return parseInt((finalLoanAmout * monthlyInterestRatio) * sp);
    },
    getSliderIntervals: function (min, max) {
        var interval = [];
        var minValue = min;

        if (min < 100000) {
            minValue = Math.floor(minValue / 5000) * 5000;

            interval.push({
                step: 0.05, // base value is 100000
                start: minValue,
                end: 100000
            })

            if (max >= 100000) {
                interval.push({
                    step: 0.1,
                    start: 100000,
                    end: max
                })
            }
        }
        else if (min < 1000000) {
            minValue = Math.floor(minValue / 10000) * 10000;
            interval.push({
                step: 0.1,
                start: minValue,
                end: max
            })
        }
        else {
            minValue = Math.floor(minValue / 100000) * 100000;
            interval.push({
                step: 1,
                start: minValue,
                end: max
            })
        }

        return interval;
    }
}

var ThirdPartyEmiCalculator = {
    trackVWFS: function () {
        var pageName = "";
        if (typeof PageInfo !== "undefined" && PageInfo !== null) {
            pageName = PageInfo.isModelPage ? "Model" : (PageInfo.isModelCity ? "Pic" : "");
        }
        
        var platformName = window.platform === 1 ? "Desktop" : (window.platform === 43 ? "Msite" : "");
        var eventAction = pageName + platformName + "_VWFSSlug_Shown";
        var eventLabel = CarDetails.carMakeName + "_" + CarDetails.carModelName;
        var eventCategory = platformName + "_VWFS";
        Common.utils.trackAction("CWNonInteractive", eventCategory, eventAction, eventLabel);
    }
};

function formatNumeric(inputPrice) {
    var inputPrice = inputPrice.toString();
    var formattedPrice = "";
    var breakPoint = 3;
    for (var i = inputPrice.length - 1; i >= 0; i--) {

        formattedPrice = inputPrice.charAt(i) + formattedPrice;
        if ((inputPrice.length - i) == breakPoint && inputPrice.length > breakPoint) {
            formattedPrice = "," + formattedPrice;
            breakPoint = breakPoint + 2;
        }
    }
    return formattedPrice;
}
var EmiCalculatorExtended = {
    setInitialEMIModelResult: function () {
        var divEmiCalculatorSlug = $('.customizeEmiContainer__js');
        for (var i = 0; i < divEmiCalculatorSlug.length; i++) {
            var currentEmiObj = divEmiCalculatorSlug[i];
            var id = currentEmiObj.getAttribute("id");
            var emiData = EmiCalculatorExtended.getCustomizedEmiData(currentEmiObj);
            var finalLoanAmout = emiData.finalLoanAmout
            var tenureValue = emiData.tenureValue;
            var formattedtenure = EmiCalculatorExtended.getFormattedTenure(tenureValue);
            var interestValue = emiData.interestValue;
            var emi = EmiCalculator.calculateEmi(finalLoanAmout, tenureValue, interestValue);
            var formattedEMI = formatNumeric(emi);
            if (emi > 0) {
                EmiCalculatorExtended.showEmiLink();
                EmiCalculatorExtended.showCustomizedEmiTextOnDiv($('[data-divtxtid=' + id + ']'), formattedEMI, formattedtenure);
                EmiCalculatorExtended.showCustomizedEmiTextOnTooltip($('[data-tooltiptextid=' + id + ']'), interestValue, formattedtenure, false);
            }
            else {
                EmiCalculatorExtended.hideEmiCalcSlug();
            }
        }
    },
    setEMIModelResult: function () {
        try {
            var emiStore = EMI_PRICE_STORE.getState().newEmiPrices.data;

            for (var i = 0; i < emiStore.length; i++) {

                var currentEmiObj = emiStore[i];
                var loanAmount = currentEmiObj.data.vehicleDownPayment.slider;
                var tenure = currentEmiObj.data.vehicleTenure.slider;
                var interest = currentEmiObj.data.vehicleInterest.slider;
                var isEmiCustomized = currentEmiObj.data.isEmiCustomized;

                var emi = EMI_PRICES_EVENTS.EmiCalculation(loanAmount, tenure, interest);
                if (emi > 0) {
                    var formattedEMI = Common.utils.formatNumeric(emi);
                    var tenureValue = EmiCalculatorExtended.getFormattedTenure(tenure.values[0]);

                    EmiCalculatorExtended.showCustomizedEmiTextOnDiv($('[data-divtxtid=' + currentEmiObj.id + ']'), formattedEMI, tenureValue);
                    EmiCalculatorExtended.showCustomizedEmiTextOnTooltip($('[data-tooltiptextid=' + currentEmiObj.id + ']'), interest.values[0], tenureValue, isEmiCustomized);
                }
                else {
                    EmiCalculatorExtended.hideCustomizedEmiText($('[data-divtxtid=' + currentEmiObj.id + ']'), $('[data-tooltiptextid=' + currentEmiObj.id + ']'), isEmiCustomized);
                }
            }
        }
        catch (err) {
            console.warn(err);
        }
    },
    hideCustomizedEmiText: function (divContainer, toolTipContainer, isEmiCustomized) {
        $(toolTipContainer).hide();
        $(divContainer).find('.emi-absent-text').show();
        $(divContainer).find('.emi-present-text').hide();
        $(divContainer).find('.change-absent-text').hide();

        EmiCalculatorExtended.showHideCustomizedText(toolTipContainer, isEmiCustomized);
    },
    showHideCustomizedText: function (container, isEmiCustomized) {
        if (isEmiCustomized) {
            EmiCalculatorExtended.showCustomizedText(container);
        }
    },
    showCustomizedText: function (container) {
        $(container).find('.emi-cust-final-text').removeClass("hide");
        $(container).find('.emi-cust-tenure-end-text').hide();
    },
    hideCustomizedText: function (container) {
        $('.emi-cust-final-text').addClass("hide");
        $('.emi-cust-tenure-end-text').show();
    },
    showCustomizedEmiTextOnTooltip: function (container, interest, tenure, isEmiCustomized) {
        $(container).show();
        $(container).find('.emi-tenure-text').text(tenure);
        $(container).find('.emi-interest-text').text(interest);
        EmiCalculatorExtended.showHideCustomizedText(container, isEmiCustomized);
    },
    showCustomizedEmiTextOnDiv: function (container, emi, tenure) {
        $(container).removeClass('hide');
        $(container).find('.emi-absent-text').hide();
        $(container).find('.change-absent-text').show();
        $(container).find('.emi-present-text').show();
        $(container).find('.emi-unit-text').hide();
        $('.emi-link-wrapper').removeClass('hide');
        $(container).find('.emi-cacl-result').removeClass('hide').find('.emi-amount-text').html('&#x20b9; ' + emi);
        $(container).find('.emi-tenure-text').text(tenure);
    },
    getFormattedTenure: function (tenureValue) {
        var formattedTenureValue = 0;
        if (tenureValue % 1 == 0) {
            formattedTenureValue = Math.round(tenureValue);
            if (formattedTenureValue > 1) {
                formattedTenureValue += ' Years';
            }
            else {
                formattedTenureValue += ' Year';
            }
        }
        else {
            formattedTenureValue = tenureValue + ' Years';
        }

        return formattedTenureValue;
    },
    showEmiCalcSlug: function (versionDetails, vid, versionName) {
        $('.emi-cacl-result').removeClass('hide');
        $('.customize-emi').removeClass('hide');
        $('.finacial-services-slug').removeClass('hide');
        EmiCalculatorExtended.setVersionEmiCalculator(versionDetails, vid, versionName);
        EmiCalculatorExtended.setInitialEMIModelResult();
        EmiCalculatorExtended.hideCustomizedText();
    },
    setVersionEmiCalculator: function (versionDetails, versionId, versionName) {
        EmiCalculatorExtended.setRequiredEmiAttributes(versionDetails, versionId, versionName);
        if (typeof EMI_PRICE_STORE !== 'undefined') {
            var data = EmiCalculator.createEmiStateObject($('.customizeEmiContainer__js'));
            EMI_PRICE_STORE.dispatch(EMI_PRICES_EVENTS.updateEmiModel(data));
        }
    },

    getThirdPartyEmiDetails: function (versionId, isMetallic, orp, oldVersionId) {
        var platformId = window.platform ? window.platform : globalPlatformObject.id;
        $.ajax({

            url: "/ThirdPartyEmiSummary/?inputVersionId=" + versionId + "&isMetallic=" + isMetallic + "&orp=" + orp + "&platform=" + platformId
                    + "&pageId=" + PageId,
            type: 'GET',
            complete: function (response) {
                if (response != null && response.responseText != null && response.responseText != "") {
                    $('#brand-emi').html(response.responseText);
                    $('.emi-cacl-result').removeClass('hide');
                    $('.finacial-services-slug').removeClass('hide');
                    $('.emi-value__js').html(thirdPartyEmi);
                    $('.tenure-value__js').html(thirdPartyTenure);
                    $('.interest-value__js').html(thirdPartyInterestRate);
                    $('.thirdPartyEmiType__js').html(thirdPartyEmiType);
                    $('.thirdPartyLogoImg__js').attr("src", thirdPartyImageUrl);
                    if (oldVersionId != versionId) {
                        ThirdPartyEmiCalculator.trackVWFS(oldVersionId, versionId);
                    }
                } else {
                    EmiCalculatorExtended.hideEmiCalcSlug();
                }
            },
            error: function (ex) {
                console.log("Third party EMI Error");
            }
        });
    },

    setRequiredEmiAttributes: function (versionDetails, versionId, versionName) {
        var customizeEmiContainer = $('.customizeEmiContainer__js');
        customizeEmiContainer.attr('data-versionname', versionName);
        customizeEmiContainer.attr('id', versionId); // Unique EmiCalc ID attribute
        customizeEmiContainer.attr('data-versionid', versionId);
        customizeEmiContainer.attr('data-downpaymentdefaultvalue', versionDetails.attr('data-downpaymentdefaultvalue'));
        customizeEmiContainer.attr('data-downpaymentminvalue', versionDetails.attr('data-downpaymentminvalue'));
        customizeEmiContainer.attr('data-downpaymentmaxvalue', versionDetails.attr('data-downpaymentmaxvalue'));
        $('.customize-emi').attr('id', versionId);
        $('.customize-emi__text').attr('data-divtxtid', versionId);
        $('.emi-tooltip').attr('data-tooltiptextid', versionId);
    },
    hideEmiCalcSlug: function () {
        $('.emi-link-wrapper').addClass('hide');
        $('.emi-cacl-result').addClass('hide');
        $('.customize-emi').addClass('hide');
        $('.finacial-services-slug').addClass('hide');
    },
    showEmiLink: function () {
        $('.customizeEmiContainer__js').removeClass('hide');
    },
    getCustomizedEmiData: function (currentEmiObj) {
        var emiData = {};
        emiData.customizedEmiData = JSON.parse(clientCache.get(EmiCalculator.customizedEmiDataKey));
        emiData.downpaymentMax = Number(currentEmiObj.dataset["downpaymentmaxvalue"]);
        emiData.downpaymentMin = Number(currentEmiObj.dataset["downpaymentminvalue"]);
        emiData.downpaymentdefaultvalue = Number(currentEmiObj.dataset["downpaymentdefaultvalue"]);
        emiData.iscustomizedEmiData = (emiData.customizedEmiData && (emiData.customizedEmiData.downpayment >= emiData.downpaymentMin && emiData.customizedEmiData.downpayment <= emiData.downpaymentMax));
        emiData.finalLoanAmout = parseFloat(emiData.downpaymentMax - (emiData.iscustomizedEmiData ? emiData.customizedEmiData.downpayment : emiData.downpaymentdefaultvalue));

        if (emiData.iscustomizedEmiData) {
            emiData.tenureValue = emiData.customizedEmiData.tenure;
            emiData.interestValue = emiData.customizedEmiData.interest;
            emiData.downpaymentValue = emiData.customizedEmiData.downpayment;
        }
        else {
            emiData.tenureValue = EmiCalculator.defaultTenure;
            emiData.interestValue = EmiCalculator.defaultInterest;
            emiData.downpaymentValue = emiData.downpaymentdefaultvalue;
        }
        return emiData;
    },
}

$(window).on('popstate', function (event) {
    if ($(".emi-pop-div").is(":visible")) {
        $(EmiCalculator.emiClose).trigger("click", [false]);
    }
    if ($('.loan-summary-popup').hasClass('popup-active')) {
        $('.loan-summary-popup .popup-close-button').trigger("click");
    }
});
