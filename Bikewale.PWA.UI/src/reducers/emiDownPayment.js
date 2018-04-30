import { combineReducers } from 'redux-immutable'
import { fromJS } from 'immutable'

import { emiCalculatorAction } from '../actionTypes/actionTypes'

const initialSliderState = {
	min: 15400,
	max: 1200000,
	values: [15400],
	userChange:false,
	sliderTitleRight:'On-Road Price'
}

export const slider = (state = initialSliderState, action) => {
	if (!state)
	return initialState; 

	switch (action.type) {
		case emiCalculatorAction.UPDATE_DOWNPAYMENT_SLIDER_VALUE:
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

export const VehicleDownPayment = combineReducers({
	slider
})
