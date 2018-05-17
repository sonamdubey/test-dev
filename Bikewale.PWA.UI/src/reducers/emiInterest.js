import { combineReducers } from 'redux-immutable'
import { fromJS, toJS } from 'immutable'

import { emiCalculatorAction } from '../actionTypes/emiActionTypes'

const initialSliderState = fromJS({
	min: 10,
	max: 15,
	values: [12.5],
	userChange: false
})

export const slider = (state = initialSliderState, action) => {
	if (!state)
		return initialSliderState;

	switch (action.type) {
		case emiCalculatorAction.UPDATE_INTEREST_SLIDER_VALUE:
			return fromJS({
				...state.toJS(),
				values: action.values,
				userChange: action.userChange,
			})

		default:
			return state
	}
}

export const VehicleInterest = combineReducers({
	slider
})
