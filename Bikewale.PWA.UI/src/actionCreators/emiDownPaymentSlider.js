import { emiCalculatorAction } from '../actionTypes/emiActionTypes'

const updateDownpaymentSliderValue = function(values, userChange) {
    values = values.map(x=>parseInt(x))
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

export const openEmiCalculator = (onRoadPrice) => (dispatch) =>{
    let minDnPay = .1 * onRoadPrice;
    let maxDnPay = .4 * onRoadPrice;
    let defaultDnPay = .3 * onRoadPrice;
    let payload = {
        min: parseInt(minDnPay),
        max : parseInt(maxDnPay),
        values : [parseInt(defaultDnPay)],
        onRoadPrice: onRoadPrice
    }
    dispatch({
        type: emiCalculatorAction.OPEN_EMICALCULATOR,
        payload
    });
}