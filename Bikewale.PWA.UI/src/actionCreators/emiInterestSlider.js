import { emiCalculatorAction } from '../actionTypes/actionTypes'

const updateInterestSliderValue = function(values, min, max, userChange) {
    return function(dispatch) {
       dispatch({
        type: emiCalculatorAction.UPDATE_INTEREST_SLIDER_VALUE,
        values,
        min,
        max,
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

export const updateInterestSlider = ({values, min, max, userChange}) => (dispatch, getState) => {
    let interestSliderMap = getState(['Finance', 'VehicleInterest', 'slider']);
	if (shouldInterestSliderUpdate(values, interestSliderMap)) {
		dispatch(updateInterestSliderValue(values, min, max, userChange))
	}
}


module.exports = {
    updateInterestSlider : updateInterestSlider
};
