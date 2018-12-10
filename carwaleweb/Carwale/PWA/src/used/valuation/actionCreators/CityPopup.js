import { TOGGLE_CITY_POPUP,
    SELECT_VALUATION_CITY,
    CLEAR_VALUATION_CITY,
    FETCH_CITY_MASKING_NAME } from '../actionTypes'

import {CITY_MASKING_NAME_END_POINT} from '../constants/index'
import ApiCall from '../apis/ValuationAPIs';

const toggle = (isActive) => {
    return {
        type: TOGGLE_CITY_POPUP,
        isActive
    }
}

const select = (selectedCity) => {
    return {
        type: SELECT_VALUATION_CITY,
        selected: selectedCity
    }
}

const clear = () => {
    return {
        type: CLEAR_VALUATION_CITY
    }
}

const getCityMaskingName = (cityName) => {
    return {
        type: FETCH_CITY_MASKING_NAME,
        cityName
    }
}

export const toggleCityPopup = (isActive) => dispatch => {
    dispatch(toggle(isActive));
}

export const selectCity = (selectedCity) => dispatch => {
    dispatch(select(selectedCity));
}

export const clearCity = () => dispatch => {
    dispatch(clear());
}

export const fetchCityMaskingName = (cityId) => dispatch => {
    ApiCall.get(CITY_MASKING_NAME_END_POINT + cityId + "/")
        .then((data) => {
            dispatch(getCityMaskingName(data.cityMaskingName));
        })
        .catch(error => console.log(error));
}
