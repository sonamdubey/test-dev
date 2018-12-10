import {
	UPDATE_INTEREST_SLIDER_VALUE,
	UPDATE_INTEREST_INPUT_VALUE
} from '../actionTypes'

import {
	getModelData
} from '../utils/Prices'

import {
	fireTracking
} from '../actionCreators/emiData';

import {
	emiComponents
} from '../enum/emiComponents';

const updateInterestSliderValue = (values, min, max, userChange) => {
	return {
		type: UPDATE_INTEREST_SLIDER_VALUE,
		values,
		min,
		max,
		userChange
	}
}

const updateInterestValue = (value) => {
	return {
		type: UPDATE_INTEREST_INPUT_VALUE,
		value
	}
}

const shouldInterestSliderUpdate = (values, slider) => {
	let update = true

	if (slider.values[0] === values[0]) {
		update = false
	}

	return update
}

export const updateInterestSlider = ({ values, min, max, userChange }) => (dispatch, getState) => {
	let model = getModelData(getState().newEmiPrices)
	values[0] = Number(values[0])
	if (shouldInterestSliderUpdate(values, model.data.vehicleInterest.slider)) {
		dispatch(updateInterestSliderValue(values, min, max, userChange))
		dispatch(updateInterestValue(values[0]))
	}
}

export const updateInterestInput = (inputValue, userChange) => (dispatch, getState) => {
	let model = getModelData(getState().newEmiPrices)
	const { slider } = model.data.vehicleInterest

	let sliderValue = (inputValue * 100) / 100
	let isValid = isNaN(inputValue.slice(-1)) ? false : true;
	if (inputValue < slider.min) {
		sliderValue = slider.min
		isValid = false;
	}
	else if (inputValue > slider.max) {
		sliderValue = slider.max
		isValid = false;
	}
	dispatch(updateInterestValue(inputValue))
	dispatch(updateInterestSliderValue([sliderValue], null, null, userChange))
	if (isValid) {
		dispatch(fireTracking({ gaTrackingAction: "Interest_TextBox_Clicked", brighuTrackingComponent: emiComponents.InterestSlider }));
	}
}
