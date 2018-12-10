import { fireNonInteractiveTracking, fireInteractiveTracking } from '../../../utils/Analytics'


const gaTrackingCategoryMsite = "PriceValuationMsite";
const gaTrackingCategoryDesktop = "PriceValuationDesktop";
const eventType = {
    cwInteractive: "CWInteractive",
    cwNonInteractive: "CWNonInteractive"
}
const trackingActionType = {
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
    searchPageButtonClick: "searchPageButtonClick",
    valuationTypeSelect: "valuationTypeSelect",
}

const trackForDesktop = (action, label) => {
    if (action) {
        track(action, gaTrackingCategoryDesktop, label);
    }
}

const trackForMobile = (action, label) => {
    if (action) {
        track(action, gaTrackingCategoryMsite, label);
    }
}

function track(action, category, label) {
    if (category) {
        switch (action) {
            case trackingActionType.valuationFormLoad:
                fireNonInteractiveTracking(category, trackingActionType.valuationFormLoad, trackingActionType.valuationFormLoad)
                break;
            case trackingActionType.manufacturingYearSelect:
                fireInteractiveTracking(category, trackingActionType.manufacturingYearSelect, trackingActionType.manufacturingYearSelect)
                break;
            case trackingActionType.makeSelect:
                fireInteractiveTracking(category, trackingActionType.makeSelect, trackingActionType.makeSelect);
                break;
            case trackingActionType.modelSelect:
                fireInteractiveTracking(category, trackingActionType.modelSelect, trackingActionType.modelSelect);
                break;
            case trackingActionType.versionSelect:
                fireInteractiveTracking(category, trackingActionType.versionSelect, trackingActionType.versionSelect);
                break;
            case trackingActionType.citySelect:
                fireInteractiveTracking(category, trackingActionType.citySelect, label);
                break;
            case trackingActionType.stateSelect:
                fireInteractiveTracking(category, trackingActionType.stateSelect, trackingActionType.stateSelect);
                break;
            case trackingActionType.otherCitySelect:
                fireInteractiveTracking(category, trackingActionType.otherCitySelect, label);
                break;
            case trackingActionType.ownersSelect:
                fireInteractiveTracking(category, trackingActionType.ownersSelect, trackingActionType.ownersSelect);
                break;
            case trackingActionType.kilometerDrivenFocusOut:
                fireInteractiveTracking(category, trackingActionType.kilometerDrivenFocusOut, trackingActionType.kilometerDrivenFocusOut);
                break;
            case trackingActionType.checkValuationButtonClick:
                fireInteractiveTracking(category, trackingActionType.checkValuationButtonClick, label);
                break;
            case trackingActionType.valuationExactMatchLoad:
                fireNonInteractiveTracking(category, trackingActionType.valuationExactMatchLoad, trackingActionType.valuationExactMatchLoad);
                break;
            case trackingActionType.valuationApproximateMatchLoad:
                fireNonInteractiveTracking(category, trackingActionType.valuationApproximateMatchLoad, trackingActionType.valuationApproximateMatchLoad);
                break;
            case trackingActionType.valuationNotAvailableLoad:
                fireNonInteractiveTracking(category, trackingActionType.valuationNotAvailableLoad, trackingActionType.valuationNotAvailableLoad);
                break;
            case trackingActionType.evaluateAgainButtonClick:
                fireInteractiveTracking(category, trackingActionType.evaluateAgainButtonClick, trackingActionType.evaluateAgainButtonClick);
                break;
            case trackingActionType.sellCarButtonClick:
                fireInteractiveTracking(category, trackingActionType.sellCarButtonClick, trackingActionType.sellCarButtonClick);
                break;
            case trackingActionType.searchPageButtonClick:
                fireInteractiveTracking(category, trackingActionType.searchPageButtonClick, trackingActionType.searchPageButtonClick);
                break;
            case trackingActionType.valuationTypeSelect:
                fireInteractiveTracking(category, trackingActionType.valuationTypeSelect, trackingActionType.valuationTypeSelect);
                break;
            default:
                return false;
        }
        return true;
    }
    return false;
}

export { trackingActionType, trackForDesktop, trackForMobile }
