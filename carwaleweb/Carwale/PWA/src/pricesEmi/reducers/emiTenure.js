import { combineReducers } from 'redux'

import {
	UPDATE_TENURE_SLIDER_VALUE,
	UPDATE_TENURE_INPUT_VALUE
} from '../actionTypes'

import { minDownPayment, maxDownPayment } from '../constants/index'

const initialSliderState = {
	min: 1,
	max: 7,
	values: [1],
	userChange:false
}


export const slider = (state = initialSliderState, action) => {
	switch (action.type) {
		case UPDATE_TENURE_SLIDER_VALUE:
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

let initialInputState = {
	value: 1
}

export const inputBox = (state = initialInputState, action) => {
	switch (action.type) {
		case UPDATE_TENURE_INPUT_VALUE:
			return {
				...state,
				value: action.value
			}

		default:
			return state
	}
}

export const vehicleTenure = combineReducers({
	slider,
	inputBox
})
