import { combineReducers } from 'redux-immutable'
import { fromJS, toJS } from 'immutable'

import { emiCalculatorAction } from '../actionTypes/emiActionTypes'
import { createNewSnapPoints } from '../utils/rheostat/function/DiffSnapPoints'

const minInterest = 10, maxInterest = 15;
const snapPoints = createNewSnapPoints({
	startPoint: minInterest,
	endPoint: maxInterest,
	difference: .1
})

const initialSliderState = fromJS({
	min: minInterest,
	max: maxInterest,
	values: [12.5],
	userChange: false,
	snapPoints: snapPoints
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
