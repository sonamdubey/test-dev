import { emiCalculatorAction } from '../actionTypes/emiActionTypes'
import { fromJS } from 'immutable'

const updateTenureSliderValue = function(values, min, max, userChange) {
    return function(dispatch) {
       dispatch({
        type: emiCalculatorAction.UPDATE_TENURE_SLIDER_VALUE,
        values,
        min,
        max,
        userChange
      })
    }
}

const shouldTenureSliderUpdate = (values, slider) => {
	let update = true

	if (slider.values[0] === values[0]) {
		update = false
	}

	return update
}

export const updateTenureSlider = ({values, min, max, userChange}) => (dispatch, getState) => {
    let tenureSliderMap = getState(['Emi', 'VehicleTenure', 'slider']);
	if (shouldTenureSliderUpdate(values, tenureSliderMap)) {
		dispatch(updateTenureSliderValue(values, min, max, userChange))
	}
}


module.exports = {
    updateTenureSlider : updateTenureSlider
};
