import {
	UPDATE_BUDGET_INPUT_VALUE,
	UPDATE_BUDGET_SLIDER_VALUE
} from '../actionTypes'

import {
	fireInteractiveTracking
} from '../../utils/Analytics'
const updateSliderValue = (values, min, max, userChange) => {
	return {
		type: UPDATE_BUDGET_SLIDER_VALUE,
		values,
		min,
		max,
		userChange
	}
}

const updateBudgetValue = (value) => {
	return {
		type: UPDATE_BUDGET_INPUT_VALUE,
		value
	}
}

const shouldSliderUpdate = (values, slider) => {
	let update = true

	if (slider.values[0] === values[0]) {
		update = false
	}

	return update
}

export const updateBudgetSlider = ({values, min, max, userChange}) => (dispatch, getState) => {
	if (shouldSliderUpdate(values, getState().newCarFinder.budget.slider)) {
		dispatch(updateSliderValue(values, min, max, userChange))
		dispatch(updateBudgetValue(values[0]))
	}
}
