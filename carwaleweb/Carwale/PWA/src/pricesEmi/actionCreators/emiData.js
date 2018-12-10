import {
    PQ_EMI_DATA,
    EMI_PQ_DATA,
    SET_EMI_MODEL,
    UPDATE_EMI_MODEL,
    SET_DOWNPAYMENT_SNAPPOINT,
    SET_IS_EMI_VALID
} from '../actionTypes'
import { fireNonInteractiveTracking } from '../../utils/Analytics';

import {
    getModelData
} from '../utils/Prices';

import {
    trackingCategory
} from '../constants';

import {
    emiComponents
} from '../enum/emiComponents';

import {
    isDesktop
} from '../../utils/Common';

import {
    trackCustomData
} from '../../utils/cwTrackingPwa';

import {
    fireInteractiveTracking
} from '../../utils/Analytics';

const setData = (data) => {
    return {
        type: 'SET_EMI_DATA',
        data
    }
}

export const setPricesData = (data) => (dispatch, state) => {
    dispatch(setData(data));
}

const setModel = (id) => {
    return {
        type: SET_EMI_MODEL,
        id
    }
}

export const setEMIModel = (id) => (dispatch, getState) => {
    const emiState = getState().newEmiPrices;

    if (emiState.activeModelId != id) {
        dispatch(setModel(id));
        for (let i = 0; i < emiState.data.length; i++) {
            let model = emiState.data[i];

            if (model.id == id) {
                if (!model.data.vehicleDownPayment.slider.snapPoints.length) {
                    dispatch(setDownPaymentSnapPoint(id));
                    break;
                }
            }
        }
    }
    fireNonInteractiveTracking(trackingCategory, "EMICalculatorPage_Shown", "");
}

const setDownPaymentSnapPoint = (id) => {
    return {
        type: SET_DOWNPAYMENT_SNAPPOINT,
        id
    }
}

const update = (data) => {
    return {
        type: UPDATE_EMI_MODEL,
        data
    }
}

export const updateEmiModel = (data) => (dispatch) => {
    dispatch(update(data));
}

export const setIsEmiValid = (isValid) => {
    return {
        type : SET_IS_EMI_VALID,
        value: isValid
    }
}

export const fireTracking = (tracking) => (dispatch, getState) => {
    if (typeof tracking === "undefined") {
        return;
    }

    let gaTrackingAction = tracking.gaTrackingAction;
    if (typeof gaTrackingAction !== "undefined") {
        fireInteractiveTracking(trackingCategory, gaTrackingAction);
    }

    let brighuTrackingComponent = tracking.brighuTrackingComponent;
    if (typeof brighuTrackingComponent === "undefined") {
        return;
    }

    let model = getModelData(getState().newEmiPrices);
    let platform = isDesktop ? "1" : "43";
    let versionId = model.data.vehicleData.id;
    let label = "SourceId=" + platform + "|VersionId=" + versionId;
    let labelTypeValue;
    let labelType;
    let brighuCat;
    switch (brighuTrackingComponent) {
        case emiComponents.DownpaymentSlider:
            labelType = "|DownPayment=";
            labelTypeValue = model.data.vehicleDownPayment.inputBox.value;
            brighuCat = "DownPayment_Changed";
            break;
        case emiComponents.TenureSlider:
            labelType = "|Tenure=";
            labelTypeValue = model.data.vehicleTenure.inputBox.value;
            brighuCat = "Tenure_Changed"
            break;
        case emiComponents.InterestSlider:
            labelType = "|Interest=";
            labelTypeValue = model.data.vehicleInterest.inputBox.value;
            brighuCat = "Interest_Changed";
            break;
        default:
            return;
    }

    label += (labelType + labelTypeValue);
    trackCustomData("EmiCalculator", brighuCat, label, false);
}
