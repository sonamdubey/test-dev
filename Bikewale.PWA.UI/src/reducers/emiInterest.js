import { combineReducers } from 'redux-immutable'
import { fromJS } from 'immutable'

import { emiCalculatorAction } from '../actionTypes/emiActionTypes'

const initialSliderState = {
	min: 10,
	max: 15,
	values: [12],
	userChange: false
}

export const slider = (state = initialSliderState, action) => {
	if (!state)
	return initialState; 

	switch (action.type) {
		case emiCalculatorAction.UPDATE_INTEREST_SLIDER_VALUE:
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

export const VehicleInterest = combineReducers({
	slider
})
