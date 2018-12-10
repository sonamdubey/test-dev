import {
	UPDATE_DOWNPAYMENT_SLIDER_VALUE,
	UPDATE_DOWNPAYMENT_INPUT_VALUE
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

const updateDownpaymentSliderValue = (values, min, max, userChange) => {
	return {
		type: UPDATE_DOWNPAYMENT_SLIDER_VALUE,
		values,
		min,
		max,
		userChange
	}
}

const updateDownpaymentValue = (value) => {
	return {
		type: UPDATE_DOWNPAYMENT_INPUT_VALUE,
		value
	}
}

const shouldDownpaymentSliderUpdate = (values, slider) => {
	let update = true

	if (slider.values[0] === values[0]) {
		update = false
	}

	return update
}

export const updateDownPaymentSlider = ({ values, min, max, userChange }) => (dispatch, getState) => {
	let model = getModelData(getState().newEmiPrices)
	if (shouldDownpaymentSliderUpdate(values, model.data.vehicleDownPayment.slider)) {
		dispatch(updateDownpaymentSliderValue(values, min, max, userChange))
		dispatch(updateDownpaymentValue(values[0]))
	}
}

export const updateDownPaymentInput = (inputValue, userChange) => (dispatch, getState) => {
	let model = getModelData(getState().newEmiPrices)
	const { slider } = model.data.vehicleDownPayment
	let sliderValue = inputValue

	let isValid = true
	if (inputValue < slider.min) {
		sliderValue = slider.min
		isValid = false;
	}
	else if (inputValue > slider.max) {
		sliderValue = slider.max
		isValid = false;
	}
	dispatch(updateDownpaymentValue(inputValue))
	dispatch(updateDownpaymentSliderValue([sliderValue], null, null, userChange))
	if (isValid) {
		dispatch(fireTracking({ gaTrackingAction: "DownPayment_TextBox_Clicked", brighuTrackingComponent: emiComponents.DownpaymentSlider }));
	}
}

export const updateVehicleLoanText = ({ textValue, userChange }) => (dispatch, getState) => {
	dispatch(updateDownpaymentSliderValue(values, min, max, userChange))
	dispatch(updateDownpaymentValue(values[0]))
}
