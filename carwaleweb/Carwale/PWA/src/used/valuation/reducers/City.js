import setCity from '../../../actionCreators/CityAutocomplete';
import { TOGGLE_CITY_POPUP,
	SELECT_VALUATION_CITY,
	CLEAR_VALUATION_CITY,
	VALIDATE_FORM,
	FETCH_CITY_MASKING_NAME } from '../actionTypes';
import { CitySelectionPopup } from '../containers/CitySelectionPopup';
import {validateCity} from '../utils/validations'

let initialSelectedState = {
	cityId: -1,
	cityName: 'Select City',
	isConfirmBtnClicked: false,
	cityMaskingName: ''
}

let initialCityState = {
	selected: initialSelectedState,
	isActive:false,
	isValid:true
}

export const city = (state = initialCityState, action) => {
	switch (action.type) {
		case TOGGLE_CITY_POPUP:
			return {
				...state,
				isActive:!action.isActive
			}
		case SELECT_VALUATION_CITY:
			return {
				...state,
				selected: action.selected,
				isValid: true
			}
		case CLEAR_VALUATION_CITY:
		return {
			...state,
			selected: initialSelectedState
			}
		case VALIDATE_FORM:
		return {
			...state,
			isValid: validateCity(state)
		}
		case FETCH_CITY_MASKING_NAME:
		return {
			...state,
			cityMaskingName: action.cityName
		}
		default:
			return state
	}
}

