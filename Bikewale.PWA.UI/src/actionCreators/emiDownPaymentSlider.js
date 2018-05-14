import { emiCalculatorAction } from '../actionTypes/emiActionTypes'

const updateDownpaymentSliderValue = function(values, userChange) {
    return function(dispatch) {
       dispatch({
        type: emiCalculatorAction.UPDATE_DOWNPAYMENT_SLIDER_VALUE,
        values,
        userChange
      })
    }
}

const shouldDownpaymentSliderUpdate = (values, slider) => {
	let update = true

	if (slider.values[0] === values[0]) {
		update = false
	}

	return update
}

export const updateDownPaymentSlider = ({values, userChange}) => (dispatch, getState) => {
    let downpaymentSliderMap = getState(['Emi', 'VehicleDownPayment', 'slider']);
	if (shouldDownpaymentSliderUpdate(values, downpaymentSliderMap)) {
		dispatch(updateDownpaymentSliderValue(values, userChange))
	}
}