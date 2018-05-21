import { combineReducers } from 'redux-immutable'
import { fromJS, toJS } from 'immutable'

import { emiCalculatorAction } from '../actionTypes/emiActionTypes'

const initialSliderState = fromJS({
	min: 0,
	max: Number.MAX_SAFE_INTEGER,
	values: [0],
	onRoadPrice: 0,
	userChange: false,
	sliderTitleRight: 'On-Road Price'
})

const slider = (state = initialSliderState, action) => {
	if (!state)
		return initialSliderState;

	switch (action.type) {
		case emiCalculatorAction.UPDATE_DOWNPAYMENT_SLIDER_VALUE:
			return fromJS({
				...state.toJS(),
				values: action.values,
				userChange: action.userChange,
			});
		case emiCalculatorAction.OPEN_EMICALCULATOR:
			return fromJS({
				...state.toJS(),
				values: action.values,
				min: action.min,
				max: action.max,
				onRoadPrice: action.onRoadPrice,
				userChange: false
			});
		default:
			return state;
	}
}

export const VehicleDownPayment = combineReducers({
	slider
})
