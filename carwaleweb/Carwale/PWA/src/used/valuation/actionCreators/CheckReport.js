import {
    VALIDATE_FORM,
    FETCHING_VALUATION,
    FETCHED_VALUATION_SUCCESS,
    SET_REDIRECT_TO_CAMPAIGN,
    CLOSE_VALUATION_REPORT
} from '../actionTypes/index'
import ApiCall from '../apis/ValuationAPIs';
import { VALUATION_HTML_END_POINT, BUY_CAR_ID } from '../constants/index';
import { serialzeObjectToQueryString } from '../../../utils/Common';
import {trackForMobile, trackingActionType} from '../utils/valuationTracking'

const validateReport = () => {
    return {
        type: VALIDATE_FORM
    }
}

const fetchingValuation = (queryString) => {
    return {
        type: FETCHING_VALUATION,
        queryString
    }
}

const fetchedValuationSuccess = (valuationData) => {
    return {
        type: FETCHED_VALUATION_SUCCESS,
        valuationData
    }
}

export const closeValidationReport = () => {
    return {
        type: CLOSE_VALUATION_REPORT
    }
}

export const setRedirectToCampaign = (dataObj, campaignType) => {
    return {
        type: SET_REDIRECT_TO_CAMPAIGN,
        dataObj,
        campaignType
    }
}

export const getValuation = () => (dispatch, getState) => {
    dispatch(validateReport())
    const state = getState().usedCar.valuation
    trackForMobile(trackingActionType.checkValuationButtonClick, getTrackingLabel(state))
    if (isValidForFetchingValuation("isValid", true, state)) {
        const option = {
            year: state.manufacturingDetails.year,
            month: state.manufacturingDetails.month,
            car: state.car.selected.version.id,
            city: state.city.selected.cityId,
            owner: state.owners.data.filter(val => { return val.isSelected })[0].id,
            kms: state.kmsDriven.value,
            isSellingIndex: state.type == 1 ? true : false
        }
        dispatch(fetchingValuation(serialzeObjectToQueryString(option)))
        let qs = '?' + serialzeObjectToQueryString(option);
        ApiCall.get(VALUATION_HTML_END_POINT, option, 'html')
            .then((data) => {
                dispatch(fetchedValuationSuccess(data, qs));
                history.pushState("valuationReportPopup", "", qs);
                let dataObj = {
                    buyCarParam: {
                        makeName: state.car.selected.make.name,
                        rootName: state.car.selected.model.rootName,
                        cityName: state.city.selected.cityName,
                        cityMaskingName: state.city.selected.cityMaskingName
                    },
                    sellCarParam: option
                }
                dispatch(setRedirectToCampaign(dataObj, state.type))
            })
            .catch(error => console.log(error));
    }
}
const isObject = (data) => {
    return toString.call(data) === '[object Object]';
}
const isValidForFetchingValuation = (keyToFind, valueToFind, data) => {
    if (!isObject(data)) {
        return false;
    }
    let queue = [];
    let count = 0;
    let found = true;
    queue.push(data);
    // use BFS to check if all if objects and nested objects contains keyToFind then object[keyToFind]==valueToFind
    while (count < queue.length) {
        let current = queue[count++];
        Object.keys(current).forEach(key => {
            if (key === keyToFind) {
                found = found && current[key] === valueToFind;
                if (!found) {
                    return found;
                }
            }
            if (isObject(current[key])) {
                queue.push(current[key]);
            }
        })
    }
    return found;
}

const getTrackingLabel = (state) => {
    if(isObject(state)){
        let labelArray = []
        labelArray.push(state.type == BUY_CAR_ID ? "Buy Car" : "Sell Car")
        labelArray.push(state.manufacturingDetails.year)
        labelArray.push(state.car.selected.make.name)
        labelArray.push(state.car.selected.model.name)
        labelArray.push(state.car.selected.version.name)
        labelArray.push(state.city.selected.cityName)
        return labelArray.filter(function (item) { if (item) { return true; } }).join('|');
    }
    return ''
}

