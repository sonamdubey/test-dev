import {
	UPDATE_TENURE_SLIDER_VALUE,
	UPDATE_TENURE_INPUT_VALUE
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

const updateTenureSliderValue = (values, min, max, userChange) => {
	return {
		type: UPDATE_TENURE_SLIDER_VALUE,
		values,
		min,
		max,
		userChange
	}
}

const updateTenureValue = (value) => {
	return {
		type: UPDATE_TENURE_INPUT_VALUE,
		value
	}
}

const shouldTenureSliderUpdate = (values, slider) => {
	let update = true

	if (slider.values[0] === values[0]) {
		update = false
	}

	return update
}

export const updateTenureSlider = ({ values, min, max, userChange }) => (dispatch, getState) => {
	let model = getModelData(getState().newEmiPrices)
	values[0] = Number(values[0])
	if (shouldTenureSliderUpdate(values, model.data.vehicleTenure.slider)) {
		dispatch(updateTenureSliderValue(values, min, max, userChange))
		dispatch(updateTenureValue(values[0]))
	}
}

export const updateTenureInput = (inputValue, userChange) => (dispatch, getState) => {
	let model = getModelData(getState().newEmiPrices)
	const { slider } = model.data.vehicleTenure

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
	dispatch(updateTenureValue(inputValue))
	dispatch(updateTenureSliderValue([sliderValue], null, null, userChange))
	if (isValid) {
		dispatch(fireTracking({ gaTrackingAction: "Tenure_TextBox_Clicked", brighuTrackingComponent: emiComponents.TenureSlider }));
	}
}
