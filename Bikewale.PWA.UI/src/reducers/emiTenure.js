import { combineReducers } from 'redux-immutable'
import { fromJS } from 'immutable'

import { emiCalculatorAction } from '../actionTypes/emiActionTypes'

const minTenure = 12, maxTenure = 48;
const initialSliderState = {
	min: minTenure,
	max: maxTenure,
	values: [(maxTenure-minTenure)/2+minTenure],
	userChange: false
}

export const slider = (state = initialSliderState, action) => {
	if (!state)
		return state; 

	switch (action.type) {
		case emiCalculatorAction.UPDATE_TENURE_SLIDER_VALUE:
			return {
				...state,
				values: action.values,
				userChange: action.userChange,
			}

		default:
			return state
	}
}

export const VehicleTenure = combineReducers({
	slider
})
