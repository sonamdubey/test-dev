import { emiCalculatorAction } from '../actionTypes/emiActionTypes'

const updateInterestSliderValue = function(values, userChange) {
    return function(dispatch) {
       dispatch({
        type: emiCalculatorAction.UPDATE_INTEREST_SLIDER_VALUE,
        values,
        userChange
      })
    }
}

const shouldInterestSliderUpdate = (values, slider) => {
	let update = true

	if (slider.values[0] === values[0]) {
		update = false
	}

	return update
}

export const updateInterestSlider = ({values,userChange}) => (dispatch, getState) => {
    let interestSliderMap = getState(['Emi', 'VehicleInterest', 'slider']);
	if (shouldInterestSliderUpdate(values, interestSliderMap)) {
		dispatch(updateInterestSliderValue(values, userChange))
	}
}


module.exports = {
    updateInterestSlider : updateInterestSlider
};
