import { emiCalculatorAction } from '../actionTypes/emiActionTypes'

const updatePieValue = function(pieInterestPayable, pieTotalPrincipalAmount, pieloanAmount) {
    return function(dispatch) {
       dispatch({
        type: emiCalculatorAction.UPDATE_PIE_VALUE,
        pieInterestPayable,
		pieTotalPrincipalAmount,
		pieloanAmount
      })
    }
}


export const updatePieValueData = ({pieInterestPayable, pieTotalPrincipalAmount, pieloanAmount}) => (dispatch, getState) => {
		dispatch(updatePieValue(pieInterestPayable, pieTotalPrincipalAmount, pieloanAmount))
		getState(['Emi', 'VehiclePieData'])
}

module.exports = {
    updatePieValueData : updatePieValueData
};

