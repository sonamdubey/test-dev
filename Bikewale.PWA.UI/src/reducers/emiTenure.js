import { combineReducers } from 'redux-immutable'
import { fromJS } from 'immutable'

import { emiCalculatorAction } from '../actionTypes/actionTypes'

const initialSliderState = {
	min: 6,
	max: 72,
	values: [8],
	userChange:false
}

export const slider = (state = initialSliderState, action) => {
	if (!state)
	return initialState; 

	switch (action.type) {
		case emiCalculatorAction.UPDATE_TENURE_SLIDER_VALUE:
			return {
				...state,
				values: action.values,
				min: action.min || state.min,
				max: action.max || state.max,
				userChange: action.userChange,
			}

		default:
			return state
	}
}

export const VehicleTenure = combineReducers({
	slider
})
