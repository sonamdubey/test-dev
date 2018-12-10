import { combineReducers } from 'redux'

import {
	UPDATE_DOWNPAYMENT_SLIDER_VALUE,
	UPDATE_DOWNPAYMENT_INPUT_VALUE,
	SET_DOWNPAYMENT_SNAPPOINT,
} from '../actionTypes'

import { minDownPayment, maxDownPayment } from '../constants/index'

const initialSliderState = {
	min: 80000,
	max: 1000000,
	values: [150000],
	userChange:false,
	sliderTitleRight: 'On-Road Price',
	snapPoints: [],
}

export const slider = (state = initialSliderState, action) => {
	switch (action.type) {
		case UPDATE_DOWNPAYMENT_SLIDER_VALUE:
		return {
				...state,
				values: action.values,
				min: action.min || state.min,
				max: action.max || state.max,
				userChange: action.userChange
			}
		case SET_DOWNPAYMENT_SNAPPOINT:
			return {
				...state,
				snapPoints: action.snapPoints
			}

		default:
			return state
	}
}


let initialInputState = {
	value: 150000
}

export const inputBox = (state = initialInputState, action) => {
	switch (action.type) {
		case UPDATE_DOWNPAYMENT_INPUT_VALUE:
			return {
				...state,
				value: action.value
			}

		default:
			return state
	}
}

export const vehicleDownPayment = combineReducers({
	slider,
	inputBox
})
