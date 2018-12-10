var valuationTracking = (function () {
    var gaTrackingCategoryMsite = "PriceValuationMsite";
    var gaTrackingCategoryDesktop = "PriceValuationDesktop";
    var eventType = {
        cwInteractive: "CWInteractive",
        cwNonInteractive: "CWNonInteractive"
    }
    var actionType = {
        valuationFormLoad: "ValuationFormLoad",
        manufacturingYearSelect: "ManufacturingYearSelect",
        makeSelect: "MakeSelect",
        modelSelect: "ModelSelect",
        versionSelect: "VersionSelect",
        citySelect: "CitySelect",
        stateSelect: "StateSelect",
        otherCitySelect: "OtherCitySelect",
        ownersSelect: "OwnersSelect",
        kilometerDrivenFocusOut: "KilometerDrivenFocusOut",
        checkValuationButtonClick: "CheckValuationButtonClick",
        valuationExactMatchLoad: "ValuationExactMatchLoad",
        valuationApproximateMatchLoad: "ValuationApproximateMatchLoad",
        valuationNotAvailableLoad: "ValuationNotAvailableLoad",
        evaluateAgainButtonClick: "EvaluateAgainButtonClick",
        sellCarButtonClick: "SellCarButtonClick",
    }

    function forDesktop(action, label) {
        if (action) {
            track(action, gaTrackingCategoryDesktop, label);
        }
    }

    function forMobile(action, label) {
        if (action) {
            track(action, gaTrackingCategoryMsite, label);
        }
    }

    function track(action, category, label) {
        if (category) {
            switch (action) {
                case actionType.valuationFormLoad:
                    Common.utils.trackAction(eventType.cwNonInteractive, category, actionType.valuationFormLoad, actionType.valuationFormLoad);
                    break;
                case actionType.manufacturingYearSelect:
                    Common.utils.trackAction(eventType.cwInteractive, category, actionType.manufacturingYearSelect, actionType.manufacturingYearSelect);
                    break;
                case actionType.makeSelect:
                    Common.utils.trackAction(eventType.cwInteractive, category, actionType.makeSelect, actionType.makeSelect);
                    break;
                case actionType.modelSelect:
                    Common.utils.trackAction(eventType.cwInteractive, category, actionType.modelSelect, actionType.modelSelect);
                    break;
                case actionType.versionSelect:
                    Common.utils.trackAction(eventType.cwInteractive, category, actionType.versionSelect, actionType.versionSelect);
                    break;
                case actionType.citySelect:
                    Common.utils.trackAction(eventType.cwInteractive, category, actionType.citySelect, label);
                    break;
                case actionType.stateSelect:
                    Common.utils.trackAction(eventType.cwInteractive, category, actionType.stateSelect, actionType.stateSelect);
                    break;
                case actionType.otherCitySelect:
                    Common.utils.trackAction(eventType.cwInteractive, category, actionType.otherCitySelect, label);
                    break;
                case actionType.ownersSelect:
                    Common.utils.trackAction(eventType.cwInteractive, category, actionType.ownersSelect, actionType.ownersSelect);
                    break;
                case actionType.kilometerDrivenFocusOut:
                    Common.utils.trackAction(eventType.cwInteractive, category, actionType.kilometerDrivenFocusOut, actionType.kilometerDrivenFocusOut);
                    break;
                case actionType.checkValuationButtonClick:
                    Common.utils.trackAction(eventType.cwInteractive, category, actionType.checkValuationButtonClick, label);
                    break;
                case actionType.valuationExactMatchLoad:
                    Common.utils.trackAction(eventType.cwNonInteractive, category, actionType.valuationExactMatchLoad, actionType.valuationExactMatchLoad);
                    break;
                case actionType.valuationApproximateMatchLoad:
                    Common.utils.trackAction(eventType.cwNonInteractive, category, actionType.valuationApproximateMatchLoad, actionType.valuationApproximateMatchLoad);
                    break;
                case actionType.valuationNotAvailableLoad:
                    Common.utils.trackAction(eventType.cwNonInteractive, category, actionType.valuationNotAvailableLoad, actionType.valuationNotAvailableLoad);
                    break;
                case actionType.evaluateAgainButtonClick:
                    Common.utils.trackAction(eventType.cwInteractive, category, actionType.evaluateAgainButtonClick, actionType.evaluateAgainButtonClick);
                    break;
                case actionType.sellCarButtonClick:
                    Common.utils.trackAction(eventType.cwInteractive, category, actionType.sellCarButtonClick, actionType.sellCarButtonClick);
                    break;
                default:
                    return false;
            }
            return true;
        }
        return false;
    }
    return {
        forDesktop: forDesktop,
        forMobile: forMobile,
        actionType: actionType
    }
})();